using System.Collections;
using UnityEngine;

public class Special4Schizophrenia : MonoBehaviour, ISpecialStatusEventAfterGiveDamage, ISpecialStatusEventEveryTurn
{
	public IEnumerator AfterGiveDamageEffect(bool IsDamageGiver, IntRef Damage)
	{
		if (IsDamageGiver)
		{
			SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
			yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(DataComponent.UserType, Damage.Value, ElementalTypeEnum.None));
		}
	}

	public IEnumerator EveryTurnEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		if (DataComponent.UserBattleStatus.INT > 0)
			DungeonController.Instance.RemoveSpecialStatus(DataComponent.UserBattleStatus, DataComponent);
		yield break;
	}
}
