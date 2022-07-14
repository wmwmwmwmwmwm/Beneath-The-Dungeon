using System.Collections;
using UnityEngine;

public class Special21Venom : MonoBehaviour, ISpecialStatusFormatDescription, ISpecialStatusEventStart, ISpecialStatusEventEveryTurn
{
	int EveryTurnDamage;

	public IEnumerator StartEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		EveryTurnDamage = (int)(DataComponent.UserBattleStatus.MaxHP * 0.15f);
		yield break;
	}

	public IEnumerator EveryTurnEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(DataComponent.UserType, EveryTurnDamage, ElementalTypeEnum.Poison, IgnoreArmor: true));
	}

	public string GetDescription()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		return string.Format(DataComponent.KoreanDescription, EveryTurnDamage);
	}
}
