using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SingletonLoader;

public partial class Player : Singleton<Player>, ISerialize
{
	public Camera MainCamera, EffectCamera;
	public Transform EquipmentParent;
	public Transform SpecialStatusParent;
	public GameObject LevelUpTrailEffect;
	public GameObject BattleEnemyPuppet;
	public Transform BattleEnemyParent;
	public Animator BattleEnemyPuppetAnimator;
	[ReadOnly] public string Name;
	[ReadOnly] public CommonStatus Status;
	[ReadOnly] public CommonStatus BattleStatus;
	[ReadOnly] public RaceData ThisRaceData;
	[ReadOnly] public int Exp;
	[ReadOnly] public int LevelUpSTR, LevelUpDEX, LevelUpINT, LevelUpCON;
	[ReadOnly] public Dictionary<EquipmentSlotTypeEnum, GameObject> Equipments;
	[ReadOnly] public Vector2Int GridPosition;
	[ReadOnly] public Vector2Int LookAt;
	[ReadOnly] public bool TempleRuneObtained, TunnelRuneObtained;
	[ReadOnly] public int HuntMonsterCount;
	bool IsMoving;

	void Start()
	{
		BattleEnemyPuppet.SetActive(false);
		Equipments = new Dictionary<EquipmentSlotTypeEnum, GameObject>();
		Array AllSlotTypes = Enum.GetValues(typeof(EquipmentSlotTypeEnum));
		foreach (EquipmentSlotTypeEnum OneSlotType in AllSlotTypes)
		{
			Equipments.Add(OneSlotType, null);
		}
	}

	public void TeleportPosition(RoomInformation PlaceRoom)
    {
		GridPosition = PlaceRoom.WorldGridPoint;
		DungeonController.Instance.RevealTile(GridPosition);
		Vector3 WorldPosition = PlaceRoom.transform.position;
		WorldPosition.y += DungeonController.Instance.CurrentDungeonData.PlayerHeight;
		transform.position = WorldPosition;
		UICanvas.Instance.UpdateMinimapPosition();
	}

	public void SetRotation(Vector2Int NewLookAt, bool SetTransformRotation)
    {
		if (SetTransformRotation) transform.LookAt(transform.position + GridHelper.GridToWorldPoint(NewLookAt));
		LookAt = NewLookAt;
		UICanvas.Instance.UpdateMinimapArrowRotation();
    }

	public void Rest()
	{
		Status.CurrentHP += Mathf.CeilToInt(Status.MaxHP * 0.1f);
		Status.CurrentHP = Mathf.Min(Status.CurrentHP, Status.MaxHP);
		Status.CurrentMP += Mathf.CeilToInt(Status.MaxMP * 0.1f);
		Status.CurrentMP = Mathf.Min(Status.CurrentMP, Status.MaxMP);
		Status.CurrentSP += Mathf.CeilToInt(Status.MaxSP * 0.2f);
		Status.CurrentSP = Mathf.Min(Status.CurrentSP, Status.MaxSP);
	}

	public void SetEquipment(EquipmentSlotTypeEnum SlotType, GameObject EquipmentObject)
	{
		EquipmentData Equipment = EquipmentObject?.GetComponent<EquipmentData>();
		if (Equipments[SlotType])
		{
			foreach (EquipmentElement OneElement in Equipments[SlotType].GetComponent<EquipmentData>().ElementComponents)
			{
				if (OneElement.SpecialEffectObject)
				{
					DungeonController.Instance.RemoveSpecialStatus(Status, OneElement.SpecialEffectObject);
				}
			}
			Equipments[SlotType].transform.SetParent(null);
		}
		Equipments[SlotType] = Equipment?.gameObject;
		if (Equipment)
		{
			foreach (EquipmentElement OneElement in Equipment.ElementComponents)
			{
				if (OneElement.SpecialEffectPrefab)
				{
					OneElement.SpecialEffectObject = DungeonController.Instance.AddSpecialStatus(UserTypeEnum.Player, Status, UICanvas.Instance.GaugeGroup, OneElement.SpecialEffectPrefab.ID, -1, true);
				}
			}
			Equipment.transform.SetParent(EquipmentParent);
		}
		RecalculatePlayerStatus();
	}

	public void RecalculatePlayerStatus()
	{
		float HPPercent = Status.MaxHP > 0 ? (float)Status.CurrentHP / Status.MaxHP : 0f;
		float MPPercent = Status.MaxMP > 0 ? (float)Status.CurrentMP / Status.MaxMP : 0f;
		float SPPercent = Status.MaxSP > 0 ? (float)Status.CurrentSP / Status.MaxSP : 0f;
		Status.MaxHP = ThisRaceData.StartHP;
		Status.MaxMP = ThisRaceData.StartMP;
		Status.MaxSP = ThisRaceData.StartSP;
		Status.Armor = ThisRaceData.StartArmor;
		Status.STR = ThisRaceData.StartSTR + LevelUpSTR;
		Status.DEX = ThisRaceData.StartDEX + LevelUpDEX;
		Status.INT = ThisRaceData.StartINT + LevelUpINT;
		Status.CON = ThisRaceData.StartCON + LevelUpCON;
		IEnumerable<EquipmentData> ExistingEquipments = Equipments.Values.Where(x => x).Select(x => x.GetComponent<EquipmentData>());
		foreach (EquipmentData OneEquipment in ExistingEquipments)
		{
			Status.MaxHP += OneEquipment.HP;
			Status.MaxMP += OneEquipment.MP;
			Status.MaxSP += OneEquipment.SP;
			Status.Armor += OneEquipment.Armor;
			Status.STR += OneEquipment.STR;
			Status.DEX += OneEquipment.DEX;
			Status.INT += OneEquipment.INT;
			Status.CON += OneEquipment.CON;
		}
		Status.MaxHP += Status.STR * 5 + Status.CON * 10;
		Status.MaxMP += Status.INT * 5;
		Status.MaxSP += Status.DEX * 10;
		Status.Armor += Status.DEX;
		Status.MaxHP = Mathf.Max(1, Status.MaxHP);
		Status.MaxMP = Mathf.Max(0, Status.MaxMP);
		Status.MaxSP = Mathf.Max(0, Status.MaxSP);
		Status.CurrentHP = (int)(Status.MaxHP * HPPercent);
		Status.CurrentMP = (int)(Status.MaxMP * MPPercent);
		Status.CurrentSP = (int)(Status.MaxSP * SPPercent);
		List<ElementalTypeEnum> ElementalTypeCandidates = ExistingEquipments.Select(x => x.ElementalType).ToList();
		ElementalTypeCandidates.Add(ThisRaceData.StartElementalType);
		Status.ElementalType = ElementalTypeCandidates.OrderBy(x => ElementalHelper.ElementalTypeEnumToInt(x)).First();
		UICanvas.Instance.GaugeGroup.UpdateAllStatusImmediate(Status, true);
	}

	public struct SaveData
	{
		public string NameSaved;
		public CommonStatus.SaveData StatusSaved;
		public RaceTypeEnum ThisRaceDataSaved;
		public int ExpSaved;
		public int LevelUpSTRSaved, LevelUpDEXSaved, LevelUpINTSaved, LevelUpCONSaved;
		public Dictionary<EquipmentSlotTypeEnum, byte[]> EquipmentsSaved;
		public Vector2Int GridPositionSaved;
		public Vector2Int LookAtSaved;
		public bool TempleRuneObtainedSaved, TunnelRuneObtainedSaved;
		public int HuntMonsterCountSaved;
	}
	public object SerializeThisObject()
	{
		SaveData NewSaveData = new SaveData()
		{
			NameSaved = Name,
			StatusSaved = (CommonStatus.SaveData)Status.SerializeThisObject(),
			ThisRaceDataSaved = ThisRaceData.RaceType,
			ExpSaved = Exp,
			LevelUpSTRSaved = LevelUpSTR,
			LevelUpDEXSaved = LevelUpDEX,
			LevelUpINTSaved = LevelUpINT,
			LevelUpCONSaved = LevelUpCON,
			EquipmentsSaved = new Dictionary<EquipmentSlotTypeEnum, byte[]>(),
			GridPositionSaved = GridPosition,
			LookAtSaved = LookAt,
			TempleRuneObtainedSaved = TempleRuneObtained,
			TunnelRuneObtainedSaved = TunnelRuneObtained,
			HuntMonsterCountSaved = HuntMonsterCount,
		};
		foreach (KeyValuePair<EquipmentSlotTypeEnum, GameObject> OneEquipmentPair in Equipments)
		{
			byte[] OneEquipmentData = sv.SerializeGeneralGameObject(OneEquipmentPair.Value);
			NewSaveData.EquipmentsSaved[OneEquipmentPair.Key] = OneEquipmentData;
		}
		return NewSaveData;
	}

	public void DeserializeThisObject(object SavedData)
	{
		SaveData LoadedData = (SaveData)SavedData;
		Name = LoadedData.NameSaved;
		Status.DeserializeThisObject(LoadedData.StatusSaved);
		ThisRaceData = db.RaceDictionary[LoadedData.ThisRaceDataSaved];
		Exp = LoadedData.ExpSaved;
		LevelUpSTR = LoadedData.LevelUpSTRSaved;
		LevelUpDEX = LoadedData.LevelUpDEXSaved;
		LevelUpINT = LoadedData.LevelUpINTSaved;
		LevelUpCON = LoadedData.LevelUpCONSaved;
		foreach (KeyValuePair<EquipmentSlotTypeEnum, byte[]> EquipmentSaved in LoadedData.EquipmentsSaved)
		{
			GameObject LoadedEquipment = sv.DeserializeGeneralGameObject(EquipmentSaved.Value);
			if (LoadedEquipment != null)
			{
				LoadedEquipment.transform.parent = EquipmentParent;
				Equipments[EquipmentSaved.Key] = LoadedEquipment;
			}
		}
		GridPosition = LoadedData.GridPositionSaved;
		LookAt = LoadedData.LookAtSaved;
		TempleRuneObtained = LoadedData.TempleRuneObtainedSaved;
		TunnelRuneObtained = LoadedData.TunnelRuneObtainedSaved;
		HuntMonsterCount = LoadedData.HuntMonsterCountSaved;
	}
}
