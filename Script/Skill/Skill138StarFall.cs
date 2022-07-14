using System.Collections;
using UnityEngine;

public class Skill138StarFall : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = (sd.UserBattleStatus.STR + sd.UserBattleStatus.DEX + sd.UserBattleStatus.INT) / 3 + 30;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.Light));
	}
}
