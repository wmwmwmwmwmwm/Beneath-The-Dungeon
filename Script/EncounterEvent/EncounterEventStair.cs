using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SingletonLoader;

public class EncounterEventStair : MonoBehaviour, IEncounterEvent, ISerialize
{
	public List<GameObject> UpStairArrows, DownStairArrows;
	[ReadOnly] public PortalTypeEnum PortalType;
	[ReadOnly] public int StairIndex;
	[ReadOnly] public bool IsUpstair;

	public IEnumerator OnPlayerEncounter()
	{
		DungeonData Destination = dc.CurrentDungeonData;
		int DestinationFloor = 0;
		switch (PortalType)
		{
			case PortalTypeEnum.Upstair: DestinationFloor = dc.CurrentFloor - 1; break;
			case PortalTypeEnum.Downstair: DestinationFloor = dc.CurrentFloor + 1; break;
		}
		if (dc.IsMainDungeon(Destination.DungeonArea))
		{
			if (DestinationFloor <= 3) Destination = db.DungeonDataDictionary[DungeonAreaEnum.MainDungeon13];
			else if (DestinationFloor <= 7) Destination = db.DungeonDataDictionary[DungeonAreaEnum.MainDungeon47];
			else if (DestinationFloor <= 12) Destination = db.DungeonDataDictionary[DungeonAreaEnum.MainDungeon812];
			else Destination = db.DungeonDataDictionary[DungeonAreaEnum.MainDungeon1315];
		}
		string StairText = "";
		switch (PortalType)
		{
			case PortalTypeEnum.Upstair:
				StairText = string.Format("{0} {1}층으로 올라가는 계단이 있습니다. 올라갑니까?", Destination.DungeonKoreanName, DestinationFloor);
				break;
			case PortalTypeEnum.Downstair:
				StairText = string.Format("{0} {1}층으로 내려가는 계단이 있습니다. 내려갑니까?", Destination.DungeonKoreanName, DestinationFloor);
				break;
		}
		yield return CommonUI.Instance.ShowAlertDialog(StairText, true);
		if (CommonUI.Instance.AlertDialogResult)
		{
			am.PlaySfx(AudioManager.SfxTypeEnum.Stair);
			dc.LastUsedPortalType = PortalType;
			dc.LastUsedPortalIndex = StairIndex;
			dc.ChangeDungeonScene(Destination, DestinationFloor);
		}
	}

	public void StairSetting(bool _IsUpstair)
	{
		IsUpstair = _IsUpstair;
		EncounterEventStair StairComponent = GetComponent<EncounterEventStair>();
		if (IsUpstair)
		{
			StairComponent.UpStairArrows.ForEach((Arrow) => Arrow.SetActive(true));
			GetComponent<Animator>().Play("UpStairAnimation");
		}
		else
		{
			StairComponent.DownStairArrows.ForEach((Arrow) => Arrow.SetActive(true));
			GetComponent<Animator>().Play("DownStairAnimation");
		}
	}

	public object SerializeThisObject() => (PortalType, StairIndex, IsUpstair);

	public void DeserializeThisObject(object SavedData)
	{
		(PortalTypeEnum _PortalType, int _StairIndex, bool _IsUpstair) = ((PortalTypeEnum, int, bool))SavedData;
		PortalType = _PortalType;
		StairIndex = _StairIndex;
		IsUpstair = _IsUpstair;
		StairSetting(IsUpstair);
	}
}
