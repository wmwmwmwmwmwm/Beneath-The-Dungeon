using System.Collections;
using UnityEngine;

public class Special8AimedShoot : MonoBehaviour, ISpecialStatusEventEnd
{
	public IEnumerator EndEffect()
	{
		SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
		int Damage = DataComponent.UserBattleStatus.DEX * 2 + 50;
		yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(DataComponent.TargetType, Damage, ElementalTypeEnum.None));
	}
}
