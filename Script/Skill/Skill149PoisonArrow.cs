using System.Collections;
using UnityEngine;

public class Skill149PoisonArrow : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 30, ElementalTypeEnum.Poison));
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.TargetType, "poison_arrow", 1));
	}
}
