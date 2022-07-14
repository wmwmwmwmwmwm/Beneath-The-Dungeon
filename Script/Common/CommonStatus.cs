using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using static SingletonLoader;

[Serializable]
public class CommonStatus : ISerialize
{
	public int CurrentHP, CurrentMP, CurrentSP;
	public int MaxHP, MaxMP, MaxSP;
	public int STR, DEX, INT, CON;
	public int Armor;
	public int Level;
	public ElementalTypeEnum ElementalType;
	public List<GameObject> Skills;
	[ReadOnly] public List<GameObject> SpecialStatuses;

	// 전투 중 변화값들
	[ReadOnly] public int AdditionalHP, AdditionalMP, AdditionalSP;

	public void SetValues(CommonStatus status)
	{
		CurrentHP = status.CurrentHP;
		CurrentMP = status.CurrentMP;
		CurrentSP = status.CurrentSP;
		MaxHP = status.MaxHP;
		MaxMP = status.MaxMP;
		MaxSP = status.MaxSP;
		STR = status.STR;
		DEX = status.DEX;
		INT = status.INT;
		CON = status.CON;
		Armor = status.Armor;
		Level = status.Level;
		ElementalType = status.ElementalType;
		Skills = new List<GameObject>(status.Skills);
		SpecialStatuses = new List<GameObject>(status.SpecialStatuses);
		AdditionalHP = status.AdditionalHP;
		AdditionalMP = status.AdditionalMP;
		AdditionalSP = status.AdditionalSP;
	}

	public void SetStartPlayerStatus(RaceData _Race)
	{
		Level = 1;
		Skills = new List<GameObject>(_Race.StartSkills);
		SpecialStatuses = new List<GameObject>();
		Player.Instance.RecalculatePlayerStatus();
		HealAll();
	}

	public void HealAll()
	{
		CurrentHP = MaxHP;
		CurrentMP = MaxMP;
		CurrentSP = MaxSP;
	}

	public void UpdateMaxHP(CommonStatus BaseStatus)
	{
		SetMaxPoint(ref CurrentHP, ref MaxHP, BaseStatus.MaxHP + AdditionalHP + (STR - BaseStatus.STR) * 5 + (CON - BaseStatus.CON) * 10, 1);
	}

	public void UpdateMaxMP(CommonStatus BaseStatus)
	{
		SetMaxPoint(ref CurrentMP, ref MaxMP, BaseStatus.MaxMP + AdditionalMP + (INT - BaseStatus.INT) * 5, 0);
	}

	public void UpdateMaxSP(CommonStatus BaseStatus)
	{
		SetMaxPoint(ref CurrentSP, ref MaxSP, BaseStatus.MaxSP + AdditionalSP + (DEX - BaseStatus.DEX) * 10, 0);
	}

	void SetMaxPoint(ref int CurrentPoint, ref int MaxPoint, int NewMaxPoint, int Limit)
	{
		float Percent = (float)CurrentPoint / MaxPoint;
		MaxPoint = NewMaxPoint;
		MaxPoint = Mathf.Max(Limit, MaxPoint);
		CurrentPoint = (int)(MaxPoint * Percent);
		CurrentPoint = Mathf.Max(Limit, CurrentPoint);
	}

	public struct SaveData
	{
		public int CurrentHPSaved, CurrentMPSaved, CurrentSPSaved;
		public int MaxHPSaved, MaxMPSaved, MaxSPSaved;
		public int STRSaved, DEXSaved, INTSaved, CONSaved;
		public int ArmorSaved;
		public int LevelSaved;
		public ElementalTypeEnum ElementalTypeSaved;
		public List<string> SkillsSaved;
		public List<string> SpecialStatusesSaved;
	}
	public object SerializeThisObject()
	{
		SaveData NewSaveData = new SaveData()
		{
			CurrentHPSaved = CurrentHP,
			CurrentMPSaved = CurrentMP,
			CurrentSPSaved = CurrentSP,
			MaxHPSaved = MaxHP,
			MaxMPSaved = MaxMP,
			MaxSPSaved = MaxSP,
			STRSaved = STR,
			DEXSaved = DEX,
			INTSaved = INT,
			CONSaved = CON,
			ArmorSaved = Armor,
			LevelSaved = Level,
			ElementalTypeSaved = ElementalType,
			SkillsSaved = new List<string>(),
			SpecialStatusesSaved = new List<string>()
		};
		foreach (GameObject OneSkill in Skills)
		{
			NewSaveData.SkillsSaved.Add(OneSkill.GetComponent<SkillData>().SkillID);
		}
		foreach (GameObject OneSpecialStatus in SpecialStatuses)
		{
			NewSaveData.SpecialStatusesSaved.Add(OneSpecialStatus.GetComponent<SpecialStatusData>().ID);
		}
		return NewSaveData;
	}

	public void DeserializeThisObject(object SavedData)
	{
		SaveData LoadedData = (SaveData)SavedData;
		CurrentHP = LoadedData.CurrentHPSaved;
		CurrentMP = LoadedData.CurrentMPSaved;
		CurrentSP = LoadedData.CurrentSPSaved;
		MaxHP = LoadedData.MaxHPSaved;
		MaxMP = LoadedData.MaxMPSaved;
		MaxSP = LoadedData.MaxSPSaved;
		STR = LoadedData.STRSaved;
		DEX = LoadedData.DEXSaved;
		INT = LoadedData.INTSaved;
		CON = LoadedData.CONSaved;
		Armor = LoadedData.ArmorSaved;
		Level = LoadedData.LevelSaved;
		ElementalType = LoadedData.ElementalTypeSaved;
		foreach (string OneSkillID in LoadedData.SkillsSaved)
		{
			Skills.Add(db.SkillDictionary[OneSkillID].gameObject);
		}
		foreach (string OneSpecialStatusID in LoadedData.SpecialStatusesSaved)
		{
			SpecialStatuses.Add(db.SpecialStatusDictionary[OneSpecialStatusID].gameObject);
		}
	}
}