using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class EncounterEventSkillChest : MonoBehaviour, IEncounterEvent, ISerialize
{
	[ReadOnly] public string SkillID;

	public IEnumerator OnPlayerEncounter()
	{
		StartCoroutine(EncounterEventManager.Instance.SetBlackOverlayState(true));
		EventInformation EventInfoComponent = GetComponent<EventInformation>();
		yield return StartCoroutine(CommonUI.Instance.SkillObtainProcess(DBManager.Instance.SkillDictionary[SkillID]));
		yield return new WaitUntil(() => !CommonUI.Instance.SkillPanel.activeSelf);
		StartCoroutine(EncounterEventManager.Instance.SetBlackOverlayState(false));
		Destroy(gameObject);
	}

	public object SerializeThisObject() => SkillID;

	public void DeserializeThisObject(object SavedData)
	{
		SkillID = (string)SavedData;
	}
}
