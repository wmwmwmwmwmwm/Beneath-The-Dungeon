using System.Collections;
using UnityEngine;
using static SingletonLoader;

public class EncounterEventDungeonExit : MonoBehaviour, IEncounterEvent
{
	public IEnumerator OnPlayerEncounter()
	{
		yield return CommonUI.Instance.ShowAlertDialog("던전에서 빠져나가는 출구가 있습니다. 탈출할까요?", true);
		if (CommonUI.Instance.AlertDialogResult)
		{
			yield return CommonUI.Instance.ShowAlertDialog("던전을 탈출했습니다.", false);
			StartCoroutine(cu.GameEndProcess());
		}
	}
}