using System.Collections;
using UnityEngine;

public class Skill42BladeChivalry : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "blade_chivalry", -1));
	}
}
