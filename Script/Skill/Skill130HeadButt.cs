using System.Collections;
using UnityEngine;

public class Skill130HeadButt : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = (int)(sd.UserBattleStatus.STR * 1.5f);
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None));
    }
}
