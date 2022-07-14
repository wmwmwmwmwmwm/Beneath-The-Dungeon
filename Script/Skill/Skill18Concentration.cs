using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Skill18Concentration : SkillEffect
{
	public override IEnumerator ActivateEffect()
	{
		
		List<(StatTypeEnum Stat, int Value)> StatList = new List<(StatTypeEnum Stat, int Value)>()
		{
			(StatTypeEnum.STR, sd.UserBattleStatus.STR),
			(StatTypeEnum.DEX, sd.UserBattleStatus.DEX),
			(StatTypeEnum.INT, sd.UserBattleStatus.INT),
			(StatTypeEnum.CON, sd.UserBattleStatus.CON),
		};
		StatList = StatList.OrderBy((x) => x.Value).ToList();
		(StatTypeEnum Stat, int Value) LowStatFirst = StatList[0];
		(StatTypeEnum Stat, int Value) LowStatSecond = StatList[1];
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(LowStatFirst.Stat, sd.UserType, 10));
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(LowStatSecond.Stat, sd.UserType, 5));
	}
}
