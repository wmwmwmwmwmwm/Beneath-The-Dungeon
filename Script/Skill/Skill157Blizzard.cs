using System.Collections;
using UnityEngine;

public class Skill157Blizzard : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 80, ElementalTypeEnum.Water));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.DEX, sd.TargetType, -5));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.CON, sd.TargetType, -5));
	}
}
