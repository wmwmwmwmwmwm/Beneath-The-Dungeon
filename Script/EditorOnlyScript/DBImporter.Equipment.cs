#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using static DBManager;
using static DBManager.EquipmentNameInfo;
using static EquipmentElement;

public partial class DBImporter
{
	[Button("장비정보 csv 임포트", ButtonHeight = 80)]
	void EquipmentImport()
	{
		// EquipmentElementDatabase 임포트
		TextAsset EquipmentElementCSVFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/0Game/CSVFiles/EquipmentElementDatabase.csv");
		List<Dictionary<string, object>> EquipmentElementDatas = CSVReader.Read(EquipmentElementCSVFile);
		GameObject BasePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/0Game/Prefab/Base/EquipmentElementBase.prefab");
		DBManagerPrefab.AllEquipmentElement = new List<EquipmentElement>();
		foreach (Dictionary<string, object> csvData in EquipmentElementDatas)
		{
			string PrefabPath = string.Format("Assets/0Game/Prefab/EquipmentElement/{0}_{1}.prefab", csvData["티어"].ToString(), csvData["ID"].ToString());
			GameObject Prefab = GetOrCreatePrefab(BasePrefab, PrefabPath);
			EquipmentElement DataComponent = Prefab.GetComponent<EquipmentElement>();
			DataComponent.ID = (int)csvData["ID"];
			string RarityString = csvData["희귀도"].ToString();
			if (!string.IsNullOrWhiteSpace(RarityString)) DataComponent.Rarity = RarityHelper.StringToRarity(RarityString);
			else DataComponent.Rarity = RarityTypeEnum.Common;
			DataComponent.Tier = (int)csvData["티어"];
			DataComponent.Point = (int)csvData["포인트"];
			DataComponent.HP = (int)csvData["체력"];
			DataComponent.MP = (int)csvData["마력"];
			DataComponent.SP = (int)csvData["기력"];
			DataComponent.Armor = (int)csvData["방어력"];
			DataComponent.STR = (int)csvData["힘"];
			DataComponent.DEX = (int)csvData["민첩"];
			DataComponent.INT = (int)csvData["지능"];
			DataComponent.CON = (int)csvData["인내"];
			DataComponent.Damage = (int)csvData["데미지"];
			DataComponent.SpecialEffectDescription = csvData["특수효과설명"].ToString();
			string ElementTypeString = csvData["속성"].ToString();
			if (!string.IsNullOrEmpty(ElementTypeString)) DataComponent.ElementalType = ElementalHelper.StringToEnum(ElementTypeString);
			DataComponent.CanDuplicated = csvData["중복가능여부"].ToString() == "O";
			string TypeString = csvData["종류"].ToString();
			DataComponent.EquipmentElementType = StringToEquipmentElementTypeEnum(TypeString);
			DBManagerPrefab.AllEquipmentElement.Add(DataComponent);
			EditorUtility.SetDirty(Prefab);
		}

		// EquipmentDropPossibility 임포트
		TextAsset DropPossibilityCSVFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/0Game/CSVFiles/EquipmentDropPossibility.csv");
		List<Dictionary<string, object>> DropPossibilityDatas = CSVReader.Read(DropPossibilityCSVFile);
		DBManagerPrefab.AllEquipmentDropType1 = new List<EquipmentDropType1>();
		List<Dictionary<string, object>> DropType1CSVDatas = DropPossibilityDatas.FindAll((x) => (int)x["차수"] == 1);
		foreach (Dictionary<string, object> DropType1Record in DropType1CSVDatas)
		{
			DBManagerPrefab.AllEquipmentDropType1.Add(new EquipmentDropType1()
			{
				Type = StringToType1(DropType1Record["종류"].ToString()),
				PossibilityInt = (int)DropType1Record["드랍율"],
			});
		}
		EquipmentDropType1.TypeEnum StringToType1(string str)
		{
			return str switch
			{
				"머리" => EquipmentDropType1.TypeEnum.Head,
				"목" => EquipmentDropType1.TypeEnum.Neck,
				"몸" => EquipmentDropType1.TypeEnum.Body,
				"신발" => EquipmentDropType1.TypeEnum.Boots,
				"무기" => EquipmentDropType1.TypeEnum.Weapon,
				"장갑" => EquipmentDropType1.TypeEnum.Gloves,
				"반지" => EquipmentDropType1.TypeEnum.Ring,
				_ => throw new System.Exception($"{str} 오타"),
			};
		}
		Util.CalculatePossibilities(DBManagerPrefab.AllEquipmentDropType1, (x) => x.PossibilityInt, (x, Min) => x.PossibilityMin = Min, (x, Max) => x.PossibilityMax = Max);

		DBManagerPrefab.AllEquipmentDropType2 = new List<EquipmentDropType2>();
		List<Dictionary<string, object>> DropType2CSVDatas = DropPossibilityDatas.FindAll((x) => (int)x["차수"] == 2);
		foreach (Dictionary<string, object> DropType2Record in DropType2CSVDatas)
		{
			DBManagerPrefab.AllEquipmentDropType2.Add(new EquipmentDropType2()
			{
				Type = StringToType2(DropType2Record["종류"].ToString()),
				PossibilityInt = (int)DropType2Record["드랍율"],
				ParentType = StringToType1(DropType2Record["대분류"].ToString()),
			});
		}
		EquipmentDropType2.TypeEnum StringToType2(string str)
		{
			return str switch
			{
				"공통 갑옷" => EquipmentDropType2.TypeEnum.CommonArmor,
				"힘 갑옷" => EquipmentDropType2.TypeEnum.StrArmor,
				"민첩 갑옷" => EquipmentDropType2.TypeEnum.DexArmor,
				"지능 갑옷" => EquipmentDropType2.TypeEnum.IntArmor,
				"모자" => EquipmentDropType2.TypeEnum.Hat,
				"투구" => EquipmentDropType2.TypeEnum.Helmet,
				"장갑" => EquipmentDropType2.TypeEnum.Gloves,
				"건틀렛" => EquipmentDropType2.TypeEnum.Gauntlets,
				"신발" => EquipmentDropType2.TypeEnum.Boots,
				"목걸이" => EquipmentDropType2.TypeEnum.Necklace,
				"반지" => EquipmentDropType2.TypeEnum.Ring,
				"무기" => EquipmentDropType2.TypeEnum.Weapon,
				_ => throw new System.Exception($"{str} 오타"),
			};
		}

		DBManagerPrefab.AllEquipmentDropType3 = new List<EquipmentDropType3>();
		List<Dictionary<string, object>> DropType3CSVDatas = DropPossibilityDatas.FindAll((x) => (int)x["차수"] == 3);
		foreach (Dictionary<string, object> DropType3Record in DropType3CSVDatas)
		{
			EquipmentDropType3 NewDropType3 = new EquipmentDropType3()
			{
				Type = StringToType3(DropType3Record["종류"].ToString()),
				KoreanName = DropType3Record["종류"].ToString(),
				PossibilityInt = (int)DropType3Record["드랍율"],
				ParentType = StringToType2(DropType3Record["대분류"].ToString()),
				RarityMin = RarityHelper.StringToRarity(DropType3Record["희귀도 최소"].ToString()),
				RarityMax = RarityHelper.StringToRarity(DropType3Record["희귀도 최대"].ToString()),
				PointPriority = DropType3Record["우선도"].ToString().ForceParseToFloat(),
				WeaponDamageMultiplier = DropType3Record["무기 계수"].ToString().ForceParseToFloat(),
			};
			NewDropType3.FixedEquipmentElementTypes = new List<EquipmentElementTypeEnum>();
			List<string> FixedElementStrings = DropType3Record["필수 포함"].ToString().Split(',').ToList();
			foreach (string FixedElementString in FixedElementStrings)
			{
				if (string.IsNullOrWhiteSpace(FixedElementString)) continue;
				NewDropType3.FixedEquipmentElementTypes.Add(StringToEquipmentElementTypeEnum(FixedElementString));
			}
			DBManagerPrefab.AllEquipmentDropType3.Add(NewDropType3);
		}
		EquipmentDropType3.TypeEnum StringToType3(string str)
		{
			return str switch
			{
				"면 갑옷" => EquipmentDropType3.TypeEnum.CottonCloth,
				"도복" => EquipmentDropType3.TypeEnum.Uniform,
				"로브" => EquipmentDropType3.TypeEnum.Robe,
				"철 갑옷" => EquipmentDropType3.TypeEnum.IronArmor,
				"강철 갑옷" => EquipmentDropType3.TypeEnum.SteelArmor,
				"가죽 갑옷" => EquipmentDropType3.TypeEnum.LeatherArmor,
				"판금 갑옷" => EquipmentDropType3.TypeEnum.PlateArmor,
				"도둑의 갑옷" => EquipmentDropType3.TypeEnum.ThiefArmor,
				"사냥꾼의 갑옷" => EquipmentDropType3.TypeEnum.HunterArmor,
				"무사의 갑옷" => EquipmentDropType3.TypeEnum.WarriorArmor,
				"상인의 옷" => EquipmentDropType3.TypeEnum.MerchantArmor,
				"마법사의 로브" => EquipmentDropType3.TypeEnum.WizardArmor,
				"모자" => EquipmentDropType3.TypeEnum.Hat,
				"후드" => EquipmentDropType3.TypeEnum.Hood,
				"투구" => EquipmentDropType3.TypeEnum.Helmet,
				"장갑" => EquipmentDropType3.TypeEnum.Gloves,
				"건틀렛" => EquipmentDropType3.TypeEnum.Gauntlets,
				"목걸이" => EquipmentDropType3.TypeEnum.Necklace,
				"반지" => EquipmentDropType3.TypeEnum.Ring,
				"신발" => EquipmentDropType3.TypeEnum.Boots,
				"검" => EquipmentDropType3.TypeEnum.Sword,
				"대검" => EquipmentDropType3.TypeEnum.GiantSword,
				"도끼" => EquipmentDropType3.TypeEnum.Axe,
				"양날 도끼" => EquipmentDropType3.TypeEnum.DoubleAxe,
				"단검" => EquipmentDropType3.TypeEnum.Dagger,
				"활" => EquipmentDropType3.TypeEnum.Bow,
				"장궁" => EquipmentDropType3.TypeEnum.LongBow,
				"석궁" => EquipmentDropType3.TypeEnum.Crossbow,
				"이중 석궁" => EquipmentDropType3.TypeEnum.DoubleCrossbow,
				"스태프" => EquipmentDropType3.TypeEnum.Staff,
				"창" => EquipmentDropType3.TypeEnum.Spear,
				"낫" => EquipmentDropType3.TypeEnum.Scythe,
				"마법검" => EquipmentDropType3.TypeEnum.MagicSword,
				_ => throw new System.Exception($"{str} 오타"),
			};
		}

		// EquipmentNameDatabase 임포트
		TextAsset EquipmentNameCSVFile =  AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/0Game/CSVFiles/EquipmentNameDatabase.csv");
		List<Dictionary<string, object>> EquipmentNameDatas = CSVReader.Read(EquipmentNameCSVFile);
		DBManagerPrefab.AllEquipmentNameInfo = new List<EquipmentNameInfo>();
		foreach (Dictionary<string, object> NameData in EquipmentNameDatas)
		{
			EquipmentNameInfo NewNameInfo = new EquipmentNameInfo()
			{
				KoreanName = NameData["이름"].ToString(),
				NameKind1 = StringToKind1(NameData["종류"].ToString()),
				NameKind2 = StringToKind2(NameData["분류"].ToString()),
			};
			string ElementalTypeString = NameData["속성"].ToString();
			if (!string.IsNullOrEmpty(ElementalTypeString)) NewNameInfo.ElementalType = ElementalHelper.StringToEnum(ElementalTypeString);
			NameKind1Enum StringToKind1(string s)
			{
				return s switch
				{
					"단어" => NameKind1Enum.Word,
					"보석" => NameKind1Enum.Jewel,
					"속성" => NameKind1Enum.Elemental,
					_ => default,
				};
			}
			NameKind2Enum StringToKind2(string s)
			{
				return s switch
				{
					"형용사" => NameKind2Enum.Adjective,
					"명사" => NameKind2Enum.Noun,
					"보석" => NameKind2Enum.Jewel,
					"속성" => NameKind2Enum.Elemental,
					_ => default,
				};
			}
			DBManagerPrefab.AllEquipmentNameInfo.Add(NewNameInfo);
		}

		// EquipmentIconDatabase 임포트
		TextAsset EquipmentIconCSVFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/0Game/CSVFiles/EquipmentIconDatabase.csv");
		List<Dictionary<string, object>> EquipmentIconDatas = CSVReader.Read(EquipmentIconCSVFile);
		DBManagerPrefab.AllEquipmentIcons = new List<EquipmentIcon>();
		Dictionary<string, Sprite> AllEquipmentIconSprites = ReadAllFiles<Sprite>("png", "/2000_Icons/Equipment");
		foreach (Dictionary<string, object> IconData in EquipmentIconDatas)
		{
			EquipmentIcon NewIconInfo = new EquipmentIcon();
			try
			{
				NewIconInfo.IconSprite = AllEquipmentIconSprites[IconData["파일이름"].ToString()];
			}
			catch
			{
				Debug.LogError($"{IconData["파일이름"]} {IconData["이름"]} {IconData["희귀도"]} 파일이름 오타");
			}
			NewIconInfo.Type = StringToType3(IconData["이름"].ToString());
			NewIconInfo.Name = IconData["이름"].ToString();
			NewIconInfo.Rarity = RarityHelper.StringToRarity(IconData["희귀도"].ToString());
			DBManagerPrefab.AllEquipmentIcons.Add(NewIconInfo);
		}

		EditorUtility.SetDirty(DBManagerPrefab);
		AssetDatabase.SaveAssets();
		Debug.Log("장비 정보 임포트 완료");
	}

	EquipmentElementTypeEnum StringToEquipmentElementTypeEnum(string TypeString)
	{
		return TypeString switch
		{
			"체력" => EquipmentElementTypeEnum.HP,
			"마력" => EquipmentElementTypeEnum.MP,
			"기력" => EquipmentElementTypeEnum.SP,
			"방어력" => EquipmentElementTypeEnum.Armor,
			"힘" => EquipmentElementTypeEnum.STR,
			"민첩" => EquipmentElementTypeEnum.DEX,
			"지능" => EquipmentElementTypeEnum.INT,
			"인내" => EquipmentElementTypeEnum.CON,
			"속성" => EquipmentElementTypeEnum.Elemental,
			"특수" => EquipmentElementTypeEnum.Special,
			"무기" => EquipmentElementTypeEnum.Weapon,
			_ => throw new System.Exception($"{TypeString} Element Type String 오류"),
		};
	}
}
#endif