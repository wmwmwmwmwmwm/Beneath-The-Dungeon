using System.Collections;
using UnityEngine;

public class Skill159Venom : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.TargetType, "venom", 2));
	}
}
