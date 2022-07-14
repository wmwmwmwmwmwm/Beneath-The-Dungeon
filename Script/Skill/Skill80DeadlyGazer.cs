using System.Collections;
using UnityEngine;

public class Skill80DeadlyGazer : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 150, ElementalTypeEnum.None, IgnoreArmor: true));
	}
}
