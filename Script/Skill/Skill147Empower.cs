using System.Collections;
using UnityEngine;

public class Skill147Empower : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "empower", 1));
	}
}
