using System.Collections;
using UnityEngine;

public class Skill36ShiningBlade : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = sd.UserBattleStatus.STR + sd.UserBattleStatus.DEX + sd.UserBattleStatus.INT + sd.UserBattleStatus.CON;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None));
	}
}
