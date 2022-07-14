using System.Collections;
using UnityEngine;

public class Skill14ElectricShock : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 20, ElementalTypeEnum.None));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.INT, sd.TargetType, -5));
	}
}
