using System.Collections;
using UnityEngine;

public class Skill150FlameThrower : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 80, ElementalTypeEnum.Fire));
	}
}
