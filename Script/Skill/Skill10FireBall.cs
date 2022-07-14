using System.Collections;
using UnityEngine;

public class Skill10FireBall : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = sd.UserBattleStatus.INT + 30;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.Fire));
    }
}
