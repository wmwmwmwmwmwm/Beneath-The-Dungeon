using System.Collections;
using UnityEngine;

public class Skill136Regeneration : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "regeneration", 3));
	}
}
