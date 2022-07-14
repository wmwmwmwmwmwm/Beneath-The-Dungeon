using System.Collections;
using UnityEngine;

public class Skill146GhostFire : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = sd.UserBattleStatus.INT * 2;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.Fire));
	}
}
