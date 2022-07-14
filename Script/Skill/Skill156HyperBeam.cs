using System.Collections;
using UnityEngine;

public class Skill156HyperBeam : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 150, ElementalTypeEnum.Light));
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "immovable", 1));
	}
}
