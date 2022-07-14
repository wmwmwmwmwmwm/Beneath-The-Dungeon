using System.Collections;
using UnityEngine;

public class Skill141ShadowStep : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = Mathf.Max(0, (sd.UserBattleStatus.DEX - sd.TargetBattleStatus.DEX) * 3);
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None));
	}
}
