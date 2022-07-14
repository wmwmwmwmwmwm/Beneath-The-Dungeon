using System.Collections;
using UnityEngine;

public class Skill158Frenzy : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.Armor, sd.UserType, -sd.UserBattleStatus.Armor, WaitPosition: 0.5f));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.HP, sd.UserType, 50, WaitPosition: 0.5f));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.STR, sd.UserType, 10, WaitPosition: 0.5f));
	}
}
