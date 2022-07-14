using System.Collections;
using UnityEngine;

public class Special2Inertie : MonoBehaviour, ISpecialStatusEventEveryTurn, ISpecialStatusEventAttackDamage
{
	public void AttackDamageEffect(IntRef Damage, ElementalTypeEnum ElementalType)
	{
		Damage.Value /= 2;
	}

	public IEnumerator EveryTurnEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		if (DataComponent.UserBattleStatus.STR > 0) 
			DungeonController.Instance.RemoveSpecialStatus(DataComponent.UserBattleStatus, DataComponent);
		yield break;
	}
}
