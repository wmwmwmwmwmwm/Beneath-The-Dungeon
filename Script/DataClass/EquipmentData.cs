using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DBManager;
using static SingletonLoader;

public class EquipmentData : MonoBehaviour, ISerialize
{
	[ReadOnly] public EquipmentTypeEnum EquipmentType;
	[ReadOnly] public string KoreanName;
	[ReadOnly] public int IconIndex;
	[ReadOnly] public RarityTypeEnum Rarity;
	[ReadOnly] public int Tier1Point, Tier2Point;
	[ReadOnly] public bool Tier3;
	[ReadOnly] public List<GameObject> Elements;
	public List<EquipmentElement> ElementComponents => Elements.Select((x) => x.GetComponent<EquipmentElement>()).ToList();
	public EquipmentIcon IconData => db.AllEquipmentIcons[IconIndex];

	public int HP
	{
		get => ElementComponents.Sum((x) => x.HP);
	}
	public int MP
	{
		get => ElementComponents.Sum((x) => x.MP);
	}
	public int SP
	{
		get => ElementComponents.Sum((x) => x.SP);
	}
	public int Armor
	{
		get => ElementComponents.Sum((x) => x.Armor);
	}
	public int STR
	{
		get => ElementComponents.Sum((x) => x.STR);
	}
	public int DEX
	{
		get => ElementComponents.Sum((x) => x.DEX);
	}
	public int INT
	{
		get => ElementComponents.Sum((x) => x.INT);
	}
	public int CON
	{
		get => ElementComponents.Sum((x) => x.CON);
	}
	public int Damage
	{
		get => ElementComponents.Sum((x) => x.Damage);
	}
	public ElementalTypeEnum ElementalType
	{
		get
		{
			EquipmentElement ElementalElement = ElementComponents.Find((x) => x.ElementalType != ElementalTypeEnum.None);
			if (ElementalElement) return ElementalElement.ElementalType;
			else return ElementalTypeEnum.None;
		}
	}

	struct SaveData
	{
		public EquipmentTypeEnum SaveEquipmentType;
		public string SaveKoreanName;
		public int SaveIconIndex;
		public RarityTypeEnum SaveRarity;
		public int SaveTier1Point, SaveTier2Point;
		public bool SaveTier3;
		public List<byte[]> SaveElements;
	}

	public object SerializeThisObject()
	{
		SaveData NewSaveData = new SaveData()
		{
			SaveEquipmentType = EquipmentType,
			SaveKoreanName = KoreanName,
			SaveIconIndex = IconIndex,
			SaveRarity = Rarity,
			SaveTier1Point = Tier1Point,
			SaveTier2Point = Tier2Point,
			SaveTier3 = Tier3,
			SaveElements = new List<byte[]>()
		};
		foreach (GameObject OneElement in Elements)
		{
			NewSaveData.SaveElements.Add(sv.SerializeGeneralGameObject(OneElement));
		}
		return NewSaveData;
	}

	public void DeserializeThisObject(object SavedData)
	{
		SaveData LoadedData = (SaveData)SavedData;
		EquipmentType = LoadedData.SaveEquipmentType;
		KoreanName = LoadedData.SaveKoreanName;
		IconIndex = LoadedData.SaveIconIndex;
		Rarity = LoadedData.SaveRarity;
		Tier1Point = LoadedData.SaveTier1Point;
		Tier2Point = LoadedData.SaveTier2Point;
		Tier3 = LoadedData.SaveTier3;
		foreach (byte[] OneElementData in LoadedData.SaveElements)
		{
			Elements.Add(sv.DeserializeGeneralGameObject(OneElementData));
		}
	}
}
