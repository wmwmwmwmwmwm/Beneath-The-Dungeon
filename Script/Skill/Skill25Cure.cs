using System.Collections;
using UnityEngine;

public class Skill25Cure : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		SpecialStatusData OneDebuff = sd.UserBattleStatus.SpecialStatuses.Find((x) => x.GetComponent<SpecialStatusData>().BuffType == SpecialStatusData.BuffTypeEnum.Debuff).GetComponent<SpecialStatusData>();
		if (OneDebuff) yield return StartCoroutine(EncounterEventManager.Instance.RemoveSpecialStatus(sd.UserType, OneDebuff));
		else yield return StartCoroutine(EncounterEventManager.Instance.GiveHP(sd.UserType, 70));
	}
}
