using System.Collections;
using UnityEngine;

public class Skill21WolfTransformation : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.STR, sd.UserType, 5));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.DEX, sd.UserType, 5));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.INT, sd.UserType, (int)(sd.UserBattleStatus.INT * -0.5f)));
	}
}
