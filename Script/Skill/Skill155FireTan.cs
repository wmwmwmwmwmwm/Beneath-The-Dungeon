using System.Collections;
using UnityEngine;

public class Skill155FireTan : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.CON, sd.UserType, -5, WaitPosition: 0.3f));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.Armor, sd.UserType, -3, WaitPosition: 0.3f));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.STR, sd.UserType, 10, WaitPosition: 0.3f));
		yield return StartCoroutine(EncounterEventManager.Instance.AddSpecialStatus(sd.UserType, "fire_tan", -1));
	}
}
