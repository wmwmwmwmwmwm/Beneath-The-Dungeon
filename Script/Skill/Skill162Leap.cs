using System.Collections;
using UnityEngine;

public class Skill162Leap : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.DEX, sd.UserType, 10));
		sd.UserBattleStatus.CurrentSP = Mathf.Min(sd.UserBattleStatus.MaxSP, sd.UserBattleStatus.CurrentSP + (int)(sd.UserBattleStatus.MaxSP * 0.5f));
    }
}
