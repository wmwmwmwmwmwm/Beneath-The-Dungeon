using System.Collections;
using UnityEngine;

public class Skill164PoisonSkull : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 60, ElementalTypeEnum.Poison));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.Armor, sd.TargetType, -3));
    }
}
