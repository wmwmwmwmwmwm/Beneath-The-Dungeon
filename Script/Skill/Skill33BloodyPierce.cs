using System.Collections;
using UnityEngine;

public class Skill33BloodyPierce : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "bleeding", 5));
	}
}
