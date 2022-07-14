using System.Collections;
using UnityEngine;

public class Special22CurseRite : MonoBehaviour, ISpecialStatusEventEveryTurn
{
	public IEnumerator EveryTurnEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		int Damage = (int)(DataComponent.UserBattleStatus.CurrentHP * 0.05f);
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(DataComponent.UserType, Damage, ElementalTypeEnum.Poison, IgnoreArmor: true));
	}
}
