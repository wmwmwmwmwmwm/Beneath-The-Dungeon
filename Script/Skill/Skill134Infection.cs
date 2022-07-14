using System.Collections;
using UnityEngine;

public class Skill134Infection : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		if (sd.UserBattleStatus.ElementalType == ElementalTypeEnum.Poison) 
			yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 150, ElementalTypeEnum.Poison));
		else
			yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 50, ElementalTypeEnum.Poison));
	}
}
