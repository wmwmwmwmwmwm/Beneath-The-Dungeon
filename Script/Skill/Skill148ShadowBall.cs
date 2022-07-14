using System.Collections;
using UnityEngine;

public class Skill148ShadowBall : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = (int)(sd.UserBattleStatus.INT * 1.5f) + 5;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.Dark));
    }
}
