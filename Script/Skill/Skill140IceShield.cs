using System.Collections;
using UnityEngine;

public class Skill140IceShield : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		 
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "ice_shield", -1));
	}
}
