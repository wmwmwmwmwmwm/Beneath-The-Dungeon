using System.Collections;
using UnityEngine;

public class Skill5Heal : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
        yield return StartCoroutine(EncounterEventManager.Instance.GiveHP(sd.UserType, 30 + sd.UserBattleStatus.INT));
    }
}
