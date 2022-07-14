using System.Collections;
using UnityEngine;

public class Special23HyperRegeneration : MonoBehaviour, ISpecialStatusFormatDescription, ISpecialStatusEventStart, ISpecialStatusEventEveryTurn
{
	int EveryTurnHeal;

	public IEnumerator StartEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		EveryTurnHeal = (int)((DataComponent.UserBattleStatus.MaxHP - DataComponent.UserBattleStatus.CurrentHP) / 3f);
		yield break;
	}

	public IEnumerator EveryTurnEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		yield return StartCoroutine(EncounterEventManager.Instance.GiveHP(DataComponent.UserType, EveryTurnHeal));
	}

	public string GetDescription()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		return string.Format(DataComponent.KoreanDescription, EveryTurnHeal);
	}
}
