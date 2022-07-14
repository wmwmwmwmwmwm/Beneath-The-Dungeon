using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class SpecialStatusData : SerializedMonoBehaviour
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
	public enum BuffTypeEnum { Skill, Buff, Debuff };
	[ReadOnly] public BuffTypeEnum BuffType;
	[ReadOnly] public bool IsPermanent;
	[ReadOnly] public int Duration;
	[ReadOnly] public SpecialStatusIcon IconObject;
	public string ID;
	public string KoreanName;
	[TextArea] public string KoreanDescription;
	public Sprite IconSprite;
	public TimelineAsset Effect;
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

	[ReadOnly] public ISpecialStatusEventStart StartEventReceiver;
	[ReadOnly] public ISpecialStatusEventEveryTurn EveryTurnEventReceiver;
	[ReadOnly] public ISpecialStatusEventEnd EndEventReceiver;
	[ReadOnly] public ISpecialStatusEventAttackDamage AttackDamageEventReceiver;

	void Start()
	{
		StartEventReceiver = GetComponent<ISpecialStatusEventStart>();
		EveryTurnEventReceiver = GetComponent<ISpecialStatusEventEveryTurn>();
		EndEventReceiver = GetComponent<ISpecialStatusEventEnd>();
		AttackDamageEventReceiver = GetComponent<ISpecialStatusEventAttackDamage>();
	}

	public string GetDescription()
	{
		ISpecialStatusFormatDescription FormatDescription = GetComponent<ISpecialStatusFormatDescription>();
		if (FormatDescription != null) return FormatDescription.GetDescription();
		else return KoreanDescription;
	}
}
