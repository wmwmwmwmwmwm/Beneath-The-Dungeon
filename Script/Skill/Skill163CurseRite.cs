using System.Collections;
using UnityEngine;

public class Skill163CurseRite : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "curse_rite", -1));
	}
}
