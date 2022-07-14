using System.Collections;
using UnityEngine;

public class Skill99HyperRegeneration : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "hyper_regeneration", 3));
	}
}
