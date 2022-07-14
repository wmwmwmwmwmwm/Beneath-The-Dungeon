using System.Collections;
using UnityEngine;

public class Skill15HealingPotion : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.GiveHP(sd.UserType, 50));
	}
}
