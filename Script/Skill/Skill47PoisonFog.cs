using System.Collections;
using UnityEngine;

public class Skill47PoisonFog : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.TargetType, "poison", 5));
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "poison", 5));
	}
}
