using System.Collections;
using UnityEngine;

public class Special3Dull : MonoBehaviour, ISpecialStatusEventStart, ISpecialStatusEventEnd, ISpecialStatusEventEveryTurn
{
	int ReducedArmor;

	public IEnumerator StartEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		ReducedArmor = DataComponent.UserBattleStatus.Armor;
		DataComponent.UserBattleStatus.Armor -= ReducedArmor;
		yield break;
	}

	public IEnumerator EndEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		DataComponent.UserBattleStatus.Armor += ReducedArmor;
		yield break;
	}

	public IEnumerator EveryTurnEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		if (DataComponent.UserBattleStatus.DEX > 0)
			DungeonController.Instance.RemoveSpecialStatus(DataComponent.UserBattleStatus, DataComponent);
		yield break;
	}
}
