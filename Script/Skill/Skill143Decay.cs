using System.Collections;
using UnityEngine;

public class Skill143Decay : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		if(sd.TargetBattleStatus.ElementalType != ElementalTypeEnum.Dark && sd.TargetBattleStatus.ElementalType != ElementalTypeEnum.Chaos && sd.TargetBattleStatus.ElementalType != ElementalTypeEnum.Abyssal)
			yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 60, ElementalTypeEnum.Dark));
		if (sd.UserBattleStatus.ElementalType != ElementalTypeEnum.Dark && sd.UserBattleStatus.ElementalType != ElementalTypeEnum.Chaos && sd.UserBattleStatus.ElementalType != ElementalTypeEnum.Abyssal)
			yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.UserType, 60, ElementalTypeEnum.Dark));
	}
}
