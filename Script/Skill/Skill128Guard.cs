using System.Collections;
using UnityEngine;

public class Skill128Guard : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "guard", 1));
	}
}
