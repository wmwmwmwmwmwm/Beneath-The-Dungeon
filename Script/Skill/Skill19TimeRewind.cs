using System.Collections;
using UnityEngine;

public class Skill19TimeRewind : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		EncounterEventManager.Instance.BattleTurn = Mathf.Max(1, EncounterEventManager.Instance.BattleTurn - 2);
		EncounterEventManager.Instance.ReplaceSkill(sd.UserType, sd, EncounterEventManager.Instance.GetDefaultSkill(sd.UserType));
		yield break;
	}
}
