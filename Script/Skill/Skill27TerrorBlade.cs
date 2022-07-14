using System.Collections;
using UnityEngine;

public class Skill27TerrorBlade : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = sd.UserBattleStatus.DEX + 10;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None));
		if (sd.TargetBattleStatus.STR <= sd.TargetBattleStatus.DEX) 
			yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.STR, sd.TargetType, -10));
		else
			yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.DEX, sd.TargetType, -10));
	}
}
