using System.Collections;
using UnityEngine;

public class Skill153LaserBeam : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, 100, ElementalTypeEnum.Light));
	}
}
