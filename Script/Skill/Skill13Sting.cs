using System.Collections;
using UnityEngine;

public class Skill13Sting : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 10, ElementalTypeEnum.Poison));
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "sting", 2));
	}
}
