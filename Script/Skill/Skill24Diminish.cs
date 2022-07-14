using System.Collections;
using UnityEngine;

public class Skill24Diminish : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.HP, sd.TargetType, (int)(sd.TargetBattleStatus.MaxHP * -0.1f)));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.Armor, sd.TargetType, -5));
	}
}
