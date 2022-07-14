using System.Collections;
using UnityEngine;

public class Skill23Curse : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.STR, sd.TargetType, -3));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.DEX, sd.TargetType, -3));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.INT, sd.TargetType, -3));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.CON, sd.TargetType, -3));
	}
}
