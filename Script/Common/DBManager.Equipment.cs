using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static DungeonData;
using static DBManager.EquipmentNameInfo;
using static EquipmentElement;

public partial class DBManager
{
	[System.Serializable]
	public class EquipmentDropType1
	{
		public enum TypeEnum { None, Head, Neck, Body, Boots, Weapon, Gloves, Ring }
		public TypeEnum Type;
		public int PossibilityInt;
		public float PossibilityMin, PossibilityMax;
	}
	[System.Serializable]
	public class EquipmentDropType2
	{
		public EquipmentDropType1.TypeEnum ParentType;
		public enum TypeEnum { None, CommonArmor, StrArmor, DexArmor, IntArmor, Hat, Helmet, Gloves, Gauntlets, Boots, Necklace, Ring, Weapon }
		public TypeEnum Type;
		public int PossibilityInt;
		public float PossibilityMin, PossibilityMax;
	}
	[System.Serializable]
	public class EquipmentDropType3
	{
		public EquipmentDropType2.TypeEnum ParentType;
		public enum TypeEnum
		{
			CottonCloth, Uniform, Robe, IronArmor, SteelArmor, LeatherArmor, PlateArmor, ThiefArmor, HunterArmor, WarriorArmor, MerchantArmor, WizardArmor,
			Hat, Hood, Helmet, Gloves, Gauntlets, Necklace, Ring, Boots, Sword, GiantSword, Axe, DoubleAxe, Dagger, Bow, LongBow, Crossbow, DoubleCrossbow, Staff, Spear, Scythe, MagicSword
		}
		public TypeEnum Type;
		public string KoreanName;
		public int PossibilityInt;
		public float PossibilityMin, PossibilityMax;
		public RarityTypeEnum RarityMin, RarityMax;
		public List<EquipmentElementTypeEnum> FixedEquipmentElementTypes;
		public float PointPriority;
		public float WeaponDamageMultiplier;
	}

	[System.Serializable]
	public class EquipmentNameInfo
	{
		public string KoreanName;
		public enum NameKind1Enum { Word, Jewel, Elemental }
		public NameKind1Enum NameKind1;
		public enum NameKind2Enum { Adjective, Noun, Jewel, Elemental }
		public NameKind2Enum NameKind2;
		public ElementalTypeEnum ElementalType;
	}

	[System.Serializable]
	public class EquipmentIcon
	{
		public Sprite IconSprite;
		public EquipmentDropType3.TypeEnum Type;
		public string Name;
		public RarityTypeEnum Rarity;
	}

	public EquipmentData EquipmentBasePrefab;
	public GameObject GetRandomEquipment()
	{
		EquipmentData NewEquipment = Instantiate(EquipmentBasePrefab);

		IntRef Tier1Point = new IntRef() { Value = (int)(DungeonController.Instance.CurrentDungeonData.EquipmentPointTier1 * Random.Range(0.9f, 1.1f)) };
		IntRef Tier2Point = new IntRef() { Value = (int)(DungeonController.Instance.CurrentDungeonData.EquipmentPointTier2 * Random.Range(0.9f, 1.1f)) };
		bool HasTier3 = false;

		EquipmentPossibility RarityInfo = Util.GetWeightedItem(DungeonController.Instance.CurrentDungeonData.EquipmentPossiblities, (x) => x.PossibilityMin, (x) => x.PossibilityMax);
		NewEquipment.Rarity = RarityInfo.Rarity;
		switch (RarityInfo.Rarity)
		{
			case RarityTypeEnum.Common:
				Tier1Point.Value = (int)(Tier1Point.Value * 0.7f);
				Tier2Point.Value = (int)(Tier2Point.Value * 0.7f);
				break;
			case RarityTypeEnum.Uncommon:
				break;
			case RarityTypeEnum.Rare:
				Tier1Point.Value = (int)(Tier1Point.Value * 1.2f);
				Tier2Point.Value = (int)(Tier2Point.Value * 1.2f);
				break;
			case RarityTypeEnum.Unique:
				if (Random.value < 0.33f)
				{
					HasTier3 = true;
				}
				else
				{
					Tier1Point.Value = (int)(Tier1Point.Value * 1.5f);
					Tier2Point.Value = (int)(Tier2Point.Value * 1.5f);
				}
				break;
			case RarityTypeEnum.Epic:
				HasTier3 = true;
				Tier1Point.Value = (int)(Tier1Point.Value * 1.5f);
				Tier2Point.Value = (int)(Tier2Point.Value * 1.5f);
				break;
		}
		NewEquipment.Tier1Point = Tier1Point.Value;
		NewEquipment.Tier2Point = Tier2Point.Value;
		NewEquipment.Tier3 = HasTier3;

		EquipmentDropType1 Type1Item = Util.GetWeightedItem(AllEquipmentDropType1, (x) => x.PossibilityMin, (x) => x.PossibilityMax);
		List<EquipmentDropType2> Type2List = AllEquipmentDropType2.FindAll((x) => x.ParentType == Type1Item.Type);
		Util.CalculatePossibilities(Type2List, (x) => x.PossibilityInt, (x, Min) => x.PossibilityMin = Min, (x, Max) => x.PossibilityMax = Max);
		EquipmentDropType2 Type2Item = Util.GetWeightedItem(Type2List, (x) => x.PossibilityMin, (x) => x.PossibilityMax);
		List<EquipmentDropType3> Type3List = AllEquipmentDropType3.FindAll((x) => x.ParentType == Type2Item.Type);
		Type3List = Type3List.FindAll((x) => RarityHelper.IsHigher(RarityInfo.Rarity, x.RarityMin, true) && RarityHelper.IsLower(RarityInfo.Rarity, x.RarityMax, true));
		Util.CalculatePossibilities(Type3List, (x) => x.PossibilityInt, (x, Min) => x.PossibilityMin = Min, (x, Max) => x.PossibilityMax = Max);
		EquipmentDropType3 Type3Item = Util.GetWeightedItem(Type3List, (x) => x.PossibilityMin, (x) => x.PossibilityMax);
		EquipmentIcon IconData = EquipmentIconByType3[Type3Item.Type].FindAll((x) => RarityInfo.Rarity == x.Rarity).PickOne();
		NewEquipment.IconIndex = AllEquipmentIcons.IndexOf(IconData);

		// 네이밍 규칙
		// 공통 : 속성은 마지막 단어 바로 앞에
		// 커먼 : (장비 이름)
		// 언커먼 : (장비 이름)
		// 레어 : (장비 이름), (보석, 지팡이)
		// 유니크, 에픽 : 레어 + "(형용사, 명사)"
		int OriginalTier1Point = Tier1Point.Value;
		int OriginalTier2Point = Tier2Point.Value;
		string EquipmentName = "";
		float RandomValue = Random.value;
		switch (RarityInfo.Rarity)
		{
			case RarityTypeEnum.Common:
			case RarityTypeEnum.Uncommon:
				EquipmentName = Type3Item.KoreanName;
				break;
			case RarityTypeEnum.Rare:
			case RarityTypeEnum.Unique:
			case RarityTypeEnum.Epic:
				if (Type3Item.Type == EquipmentDropType3.TypeEnum.Staff)
				{
					EquipmentName = string.Format("{0} {1}", EquipmentNameInfoByKind2[NameKind2Enum.Jewel].PickOne().KoreanName, Type3Item.KoreanName);
				}
				else
				{
					EquipmentName = Type3Item.KoreanName;
				}
				break;
		}
		switch (RarityInfo.Rarity)
		{
			case RarityTypeEnum.Unique:
			case RarityTypeEnum.Epic:
				EquipmentNameInfo NicknameAdjective = EquipmentNameInfoByKind2[NameKind2Enum.Adjective].PickOne();
				EquipmentNameInfo NicknameNoun = EquipmentNameInfoByKind2[NameKind2Enum.Noun].PickOne();
				EquipmentName += string.Format(" <color=#FFF6E1>\"{0} {1}\"</color>", NicknameAdjective.KoreanName, NicknameNoun.KoreanName);
				break;
		}

		NewEquipment.KoreanName = EquipmentName;
		NewEquipment.EquipmentType = Type1Item.Type switch
		{
			EquipmentDropType1.TypeEnum.Body => EquipmentTypeEnum.Body,
			EquipmentDropType1.TypeEnum.Head => EquipmentTypeEnum.Head,
			EquipmentDropType1.TypeEnum.Neck => EquipmentTypeEnum.Neck,
			EquipmentDropType1.TypeEnum.Weapon => EquipmentTypeEnum.Weapon,
			EquipmentDropType1.TypeEnum.Ring => EquipmentTypeEnum.Ring,
			EquipmentDropType1.TypeEnum.Boots => EquipmentTypeEnum.Boots,
			EquipmentDropType1.TypeEnum.Gloves => EquipmentTypeEnum.Glove,
			_ => throw new System.Exception($"{Type1Item.Type} 오류"),
		};

		// 장비 타입에 대한 능력치들을 먼저 뽑는다
		List<EquipmentElement> Type3PossibleElementsTier1 = new List<EquipmentElement>(EquipmentElementByTier[1]);
		FixedElementProcess(Type3PossibleElementsTier1, Tier1Point, OriginalTier1Point);
		List<EquipmentElement> Type3PossibleElementsTier2 = new List<EquipmentElement>(EquipmentElementByTier[2]);
		FixedElementProcess(Type3PossibleElementsTier2, Tier2Point, OriginalTier2Point);
		void FixedElementProcess(List<EquipmentElement> ElementList, IntRef Point, int OriginalPoint)
		{
			ElementList = ElementList.FindAll((x) => Type3Item.FixedEquipmentElementTypes.Contains(x.EquipmentElementType));
			while (Point.Value > (int)(OriginalPoint * Type3Item.PointPriority) && ElementList.Count != 0)
			{
				ElementList.RemoveAll((x) => !IsPossibleElement(x));
				EquipmentElement NewElementPrefab = ElementList.PickOne();
				EquipmentElement NewElement = AddElement(NewElementPrefab);
				DecreasePoint(NewElement);
			}
		}

		// 무기라면 데미지 추가
		if (Type1Item.Type == EquipmentDropType1.TypeEnum.Weapon)
		{
			EquipmentElement NewWeaponElement = AddElement(EquipmentElementByType[EquipmentElementTypeEnum.Weapon].PickOne());
			NewWeaponElement.Damage = (int)((OriginalTier1Point + OriginalTier2Point) * 0.3f * Random.Range(0.8f, 1.2f) * Type3Item.WeaponDamageMultiplier) + Random.Range(1, 3);
		}

		// 남은 포인트로 적당한 능력치들을 부여한다
		bool IsDone = false;
		for (int Tier = 1; Tier <= 3; Tier++)
		{
			List<EquipmentElement> PossibleElements = new List<EquipmentElement>(EquipmentElementByTier[Tier]);
			while (true)
			{
				PossibleElements.RemoveAll((x) => !IsPossibleElement(x));
				if (PossibleElements.Count == 0) break;
				EquipmentElement NewElementPrefab = PossibleElements.PickOne();
				EquipmentElement NewElement = AddElement(NewElementPrefab);
				DecreasePoint(NewElement);
				if (!NewElement.CanDuplicated) PossibleElements.Remove(NewElement);
				if (NewElement.EquipmentElementType == EquipmentElementTypeEnum.Elemental) PossibleElements.RemoveAll((x) => x.EquipmentElementType == EquipmentElementTypeEnum.Elemental);
				IsDone = IsLimitedElementKindCount();
				if (IsDone) break;
			}
			if (IsDone) break;
		}
		bool IsLimitedElementKindCount()
		{
			List<EquipmentElementTypeEnum> ElementTypes = new List<EquipmentElementTypeEnum>();
			int KindLimit = NewEquipment.Rarity switch
			{
				RarityTypeEnum.Common => 2,
				RarityTypeEnum.Uncommon => 3,
				RarityTypeEnum.Rare => 4,
				RarityTypeEnum.Unique => 5,
				RarityTypeEnum.Epic => 6,
				_ => 0,
			};
			foreach (EquipmentElement Element in NewEquipment.ElementComponents)
			{
				if (!ElementTypes.Contains(Element.EquipmentElementType)) ElementTypes.Add(Element.EquipmentElementType);
				if (ElementTypes.Count == KindLimit) return true;
			}
			return false;
		}

		// 속성이 뽑혔다면 이름을 수정한다
		EquipmentElement ElementalEquipmentElement = NewEquipment.ElementComponents.Find((x) => x.EquipmentElementType == EquipmentElementTypeEnum.Elemental);
		if (ElementalEquipmentElement)
		{
			EquipmentNameInfo ElementalNameInfo = AllEquipmentNameInfo.Find((x) => x.ElementalType == ElementalEquipmentElement.ElementalType);
			NewEquipment.KoreanName = string.Format("{0} {1}", ElementalNameInfo.KoreanName, NewEquipment.KoreanName);
		}

		EquipmentElement AddElement(EquipmentElement ElementPrefab)
		{
			EquipmentElement NewElement = Instantiate(ElementPrefab, NewEquipment.transform);
			NewEquipment.Elements.Add(NewElement.gameObject);
			NewElement.GetComponent<IEquipmentCreateEvent>()?.OnCreate();
			return NewElement;
		}
		bool IsPossibleElement(EquipmentElement Element)
		{
			if (RarityHelper.IsHigher(RarityInfo.Rarity, Element.Rarity, true))
			{
				return Element.Tier switch
				{
					1 => Tier1Point.Value >= Element.Point,
					2 => Tier2Point.Value >= Element.Point,
					3 => HasTier3,
					_ => false,
				};
			}
			else return false;
		}
		void DecreasePoint(EquipmentElement Element)
		{
			switch (Element.Tier)
			{
				case 1: Tier1Point.Value -= Element.Point; break;
				case 2: Tier2Point.Value -= Element.Point; break;
				case 3: HasTier3 = false; break;
			}
		}

		return NewEquipment.gameObject;
	}


}
