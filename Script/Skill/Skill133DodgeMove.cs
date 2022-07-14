using System.Collections;
using UnityEngine;

public class Skill133DodgeMove : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "dodge_move", 3));
	}
}
