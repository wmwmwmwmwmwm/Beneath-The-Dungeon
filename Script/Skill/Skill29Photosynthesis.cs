using System.Collections;
using UnityEngine;

public class Skill29Photosynthesis : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "photosynthesis", 3));
	}
}
