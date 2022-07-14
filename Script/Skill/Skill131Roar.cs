using System.Collections;
using UnityEngine;

public class Skill131Roar : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.CON, sd.UserType, 10, WaitPosition: 0.5f));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.STR, sd.UserType, 5));
    }
}
