﻿using System.Collections;
using UnityEngine;

public class Skill127Bite : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		int Damage = (int)(sd.UserBattleStatus.STR * 0.8f);
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(sd.TargetType, Damage, ElementalTypeEnum.None));
	}
}
