using System.Collections;
using UnityEngine;

public class Skill145Carapace : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.Armor, sd.UserType, 5));
    }
}
