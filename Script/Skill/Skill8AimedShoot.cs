using System.Collections;
using UnityEngine;

public class Skill8AimedShoot : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "aimed_shoot", 0));
	}
}
