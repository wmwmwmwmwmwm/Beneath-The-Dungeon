using System.Collections;
using UnityEngine;

public class Skill76SunriseBlast : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 100, ElementalTypeEnum.Fire));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.Armor, sd.TargetType, -5));
	}
}
