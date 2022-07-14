using System.Collections;
using UnityEngine;

public class Skill11FierceAttack : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = sd.UserBattleStatus.STR * 2 + 50;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.STR, sd.UserType, -5));
	}
}
