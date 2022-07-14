using System.Collections;
using UnityEngine;

public class Skill129Paint : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.Armor, sd.TargetType, -3));
    }
}
