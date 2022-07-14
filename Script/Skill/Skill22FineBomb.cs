using System.Collections;
using UnityEngine;

public class Skill22FineBomb : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 70, ElementalTypeEnum.None));
	}
}
