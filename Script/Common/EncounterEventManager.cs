using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using static SingletonLoader;

public partial class EncounterEventManager : Singleton<EncounterEventManager>, ISerialize
{
	public PlayableDirector BattlePlayableDirector;
	public SignalReceiver TimelineSignalReceiver;

	[ReadOnly] public Monster CurrentBattleEnemy;

	void Start()
	{
		GetComponent<Canvas>().worldCamera = Player.Instance.MainCamera;
		BlackOverlay.SetActive(false);
		BattleBeginAlert.SetActive(false);
		BattlePanel.SetActive(false);
		EnemyDamageIndicator.SetActive(false);
		PlayerDamageIndicator.SetActive(false);
		EnemyHealIndicator.SetActive(false);
		PlayerHealIndicator.SetActive(false);
		EnemyArmorIndicator.SetActive(false);
		PlayerArmorIndicator.SetActive(false);
		SkillAlert.gameObject.SetActive(false);
		BattleButtonPanel.SetActive(false);
		BattleResultWinPanel.gameObject.SetActive(false);
		BattleResultLosePanel.gameObject.SetActive(false);
		PlayerStatIndicator.SetActive(false);
		EnemyStatIndicator.SetActive(false);
		BattlePlayableDirector.stopped += (dir) => { TimelineCompleteTrigger = true; };
		SetBattleSpeed(1);
	}

	public GameObject BlackOverlay;
	public IEnumerator SetBlackOverlayState(bool On)
	{
		if (On)
		{
			BlackOverlay.SetActive(true);
			yield return BlackOverlay.GetComponent<Image>().DOFade(dc.CurrentDungeonData.BattleOverlayAlpha, AnimationTime).WaitForCompletion();
		}
		else
		{
			yield return BlackOverlay.GetComponent<Image>().DOFade(0f, AnimationTime).WaitForCompletion();
			BlackOverlay.SetActive(false);
		}
	}

	public struct SaveData
	{
		public int BattleSpeedSaved;
	}
	public object SerializeThisObject()
	{
		return new SaveData()
		{
			BattleSpeedSaved = BattleSpeed
		};
	}

	public void DeserializeThisObject(object SavedData)
	{
		SaveData LoadedData = (SaveData)SavedData;
		BattleSpeed = LoadedData.BattleSpeedSaved;
	}
}
