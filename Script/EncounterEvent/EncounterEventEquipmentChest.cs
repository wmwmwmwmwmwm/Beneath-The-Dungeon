using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SingletonLoader;

public class EncounterEventEquipmentChest : MonoBehaviour, IEncounterEvent, ISerialize
{
	[ReadOnly] public GameObject ContainEquipment;

	public IEnumerator OnPlayerEncounter()
	{
		StartCoroutine(EncounterEventManager.Instance.SetBlackOverlayState(true));
		EventInformation EventInfoComponent = GetComponent<EventInformation>();
		yield return StartCoroutine(CommonUI.Instance.EquipmentObtainProcess(ContainEquipment));
		yield return new WaitUntil(() => !CommonUI.Instance.EquipmentPanel.activeSelf);
		StartCoroutine(EncounterEventManager.Instance.SetBlackOverlayState(false));
		Destroy(gameObject);
	}

	public object SerializeThisObject()
	{
		return sv.SerializeGeneralGameObject(ContainEquipment);
	}

	public void DeserializeThisObject(object SavedData)
	{
		ContainEquipment = sv.DeserializeGeneralGameObject((byte[])SavedData);
	}
}
