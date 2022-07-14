using System.Collections;
using UnityEngine;

public class Special11PoisonArrow : MonoBehaviour, ISpecialStatusEventEveryTurn
{
	public IEnumerator EveryTurnEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(DataComponent.UserType, 10, ElementalTypeEnum.Poison, IgnoreArmor: true));
	}
}
