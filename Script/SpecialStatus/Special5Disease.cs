using System.Collections;
using UnityEngine;

public class Special5Disease : MonoBehaviour, ISpecialStatusEventStart, ISpecialStatusEventEnd, ISpecialStatusEventEveryTurn
{
	int ReducedMaxHP;

	public IEnumerator StartEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		ReducedMaxHP = DataComponent.UserBattleStatus.MaxHP / 2;
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.HP, DataComponent.UserType, -ReducedMaxHP));
	}

	public IEnumerator EndEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		yield return StartCoroutine(EncounterEventManager.Instance.ChangeStat(StatTypeEnum.HP, DataComponent.UserType, ReducedMaxHP));
	}

	public IEnumerator EveryTurnEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		if (DataComponent.UserBattleStatus.CON > 0)
			DungeonController.Instance.RemoveSpecialStatus(DataComponent.UserBattleStatus, DataComponent);
		yield break;
	}
}