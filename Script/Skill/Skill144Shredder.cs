using System.Collections;
using UnityEngine;

public class Skill144Shredder : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		 
		int Damage = (int)(sd.UserBattleStatus.STR * 1.5f) + 20;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None));
	}
}
