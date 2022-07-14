using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Timeline;

public class SkillData : SerializedMonoBehaviour
{
	[ReadOnly] public UserTypeEnum UserType;
	public UserTypeEnum TargetType
	{
		get
		{
			switch (UserType)
			{
				case UserTypeEnum.Player:
					return UserTypeEnum.Enemy;
				case UserTypeEnum.Enemy:
					return UserTypeEnum.Player;
			}
			return UserTypeEnum.Player;
		}
	}
	public string SkillID;
	public string KoreanName;
	[TextArea] public string KoreanDescription;
	public RarityTypeEnum Rarity;
	public ElementalTypeEnum ElementalType;
	public int HPCost, MPCost, SPCost;
	public float HPCostPercent, MPCostPercent, SPCostPercent;
	public int RecommendLevel;
	public int SpawnPossibilityInt;
	[ReadOnly] public float MinSpawnPossibility, MaxSpawnPossibility;
	public Sprite IconSprite;
	public TimelineAsset SkillEffect;
	public CommonStatus UserStatus
	{
		get
		{
			return UserType switch
			{
				UserTypeEnum.Player => Player.Instance.Status,
				UserTypeEnum.Enemy => EncounterEventManager.Instance.CurrentBattleEnemy.Status,
				_ => null,
			};
		}
	}
	public CommonStatus TargetStatus
	{
		get
		{
			return UserType switch
			{
				UserTypeEnum.Player => EncounterEventManager.Instance.CurrentBattleEnemy.Status,
				UserTypeEnum.Enemy => Player.Instance.Status,
				_ => null,
			};
		}
	}
	public CommonStatus UserBattleStatus
	{
		get
		{
			return UserType switch
			{
				UserTypeEnum.Player => Player.Instance.BattleStatus,
				UserTypeEnum.Enemy => EncounterEventManager.Instance.CurrentBattleEnemy.BattleStatus,
				_ => null,
			};
		}
	}
	public CommonStatus TargetBattleStatus
	{
		get
		{
			return UserType switch
			{
				UserTypeEnum.Player => EncounterEventManager.Instance.CurrentBattleEnemy.BattleStatus,
				UserTypeEnum.Enemy => Player.Instance.BattleStatus,
				_ => null,
			};
		}
	}

	[ReadOnly] public SkillEffect ActivateSkillEvent;
	void Start()
	{
		ActivateSkillEvent = GetComponent<SkillEffect>();
	}
}
