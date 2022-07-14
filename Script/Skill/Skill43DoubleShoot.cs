using System.Collections;
using UnityEngine;

public class Skill43DoubleShoot : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = sd.UserBattleStatus.DEX * 2;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None));
	}
}
