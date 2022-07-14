using System.Collections;
using UnityEngine;

public class Skill44PierceKiss : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = sd.UserBattleStatus.DEX + 25;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None));
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "bleeding", 3));
	}
}
