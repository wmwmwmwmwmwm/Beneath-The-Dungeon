using System.Collections;
using UnityEngine;

public class Skill7DoubleStab : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = (int)(sd.UserBattleStatus.DEX * 0.7f) + 5;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None, AnimationTimeMultiplier: 2f));
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None, AnimationTimeMultiplier: 2f));
	}
}
