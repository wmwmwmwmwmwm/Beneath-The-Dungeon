using System.Collections;
using UnityEngine;

public class Skill41SavageBlow : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = (int)(sd.UserBattleStatus.DEX * 0.7f);
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None, AnimationTimeMultiplier: 0.3f));
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None, AnimationTimeMultiplier: 0.3f));
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None, AnimationTimeMultiplier: 0.3f));
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None, AnimationTimeMultiplier: 0.3f));
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None, AnimationTimeMultiplier: 0.3f));
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None, AnimationTimeMultiplier: 0.3f));
	}
}
