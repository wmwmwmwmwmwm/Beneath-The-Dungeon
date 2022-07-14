using System.Collections;
using UnityEngine;

public class Skill135Rage : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.STR, sd.UserType, (int)(sd.UserBattleStatus.STR * 0.2f), WaitPosition: 0.5f));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.Armor, sd.UserType, -3));
    }
}
