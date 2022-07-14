using System.Collections;
using UnityEngine;

public class Skill119Devour : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = sd.TargetBattleStatus.MaxHP - sd.TargetBattleStatus.CurrentHP;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None));
    }
}
