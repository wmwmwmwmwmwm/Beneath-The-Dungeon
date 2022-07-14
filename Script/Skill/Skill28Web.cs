using System.Collections;
using UnityEngine;

public class Skill28Web : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = Mathf.Min(30, sd.TargetBattleStatus.CurrentSP);
		sd.TargetBattleStatus.CurrentSP = Mathf.Max(0, sd.TargetBattleStatus.CurrentSP - 30);
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None));
	}
}
