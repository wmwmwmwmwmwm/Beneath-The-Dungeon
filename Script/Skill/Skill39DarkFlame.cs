using System.Collections;
using UnityEngine;

public class Skill39DarkFlame : SkillEffect
{
	int Counter = 0;
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = sd.UserBattleStatus.INT + 20;
		if(Counter == 0)
		{
			yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.Fire));
		}
		else if(Counter == 1)
		{
			yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.Dark));
		}
		Counter++;
    }
}
