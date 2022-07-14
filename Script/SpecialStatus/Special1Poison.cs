using System.Collections;
using UnityEngine;

public class Special1Poison : MonoBehaviour, ISpecialStatusEventEveryTurn, ISpecialStatusFormatDescription
{
	int EveryTurnDamage;

	public IEnumerator EveryTurnEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		EveryTurnDamage = (int)(DataComponent.UserBattleStatus.CurrentHP * 0.1f);
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(DataComponent.UserType, EveryTurnDamage, ElementalTypeEnum.Poison, IgnoreArmor: true));
	}

	public string GetDescription()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		return string.Format(DataComponent.KoreanDescription, EveryTurnDamage);
	}
}
