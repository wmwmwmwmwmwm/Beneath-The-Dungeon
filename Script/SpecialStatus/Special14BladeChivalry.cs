using System.Collections;
using UnityEngine;

public class Special14BladeChivalry : MonoBehaviour, ISpecialStatusEventAfterGiveDamage
{
	public IEnumerator AfterGiveDamageEffect(bool IsDamageGiver, IntRef Damage)
	{
		if (!IsDamageGiver) 
		{
			SpecialStatusData DataComponent = GetComponent<SpecialStatusData>();
			yield return StartCoroutine(EncounterEventManager.Instance.GiveDamage(DataComponent.TargetType, 30, ElementalTypeEnum.None));
		}
	}
}
