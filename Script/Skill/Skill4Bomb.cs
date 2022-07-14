using System.Collections;
using UnityEngine;

public class Skill4Bomb : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
        yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 30, ElementalTypeEnum.None));
    }
}
