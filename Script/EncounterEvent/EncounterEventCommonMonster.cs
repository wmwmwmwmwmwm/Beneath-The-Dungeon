using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncounterEventCommonMonster : MonoBehaviour, IEncounterEvent
{
	public IEnumerator OnPlayerEncounter()
	{
		Monster MonsterComponent = GetComponent<Monster>();
		yield return StartCoroutine(EncounterEventManager.Instance.StartBattle(MonsterComponent));
	}
}
