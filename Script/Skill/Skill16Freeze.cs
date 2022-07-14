using System.Collections;
using UnityEngine;

public class Skill16Freeze : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = sd.UserBattleStatus.INT * 2 + 10;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.Water));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.DEX, sd.TargetType, -5));
	}
}
