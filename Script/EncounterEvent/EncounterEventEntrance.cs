using System.Collections;
using UnityEngine;
using static SingletonLoader;

public class EncounterEventEntrance : MonoBehaviour, IEncounterEvent
{
	public DungeonData DestinationDungeon;
	
	public IEnumerator OnPlayerEncounter()
	{
		string PortalText = string.Format("{0}(으)로 통하는 포탈이 있습니다. 진입합니까?", DestinationDungeon.DungeonKoreanName);
		yield return cu.ShowAlertDialog(PortalText, true);
		if (cu.AlertDialogResult)
		{
			am.PlaySfx(AudioManager.SfxTypeEnum.EnterEntrance);
			dc.LastUsedPortalType = PortalTypeEnum.Entrance;
			dc.LastUsedPortalIndex = 4;
			int DestinationFloor = 1;
			if(dc.IsMainDungeon(DestinationDungeon.DungeonArea))
			{
				DestinationFloor = dc.DungeonEntranceFloor[dc.CurrentDungeonData.DungeonArea];
				DestinationDungeon = dc.GetMainDungeonData(DestinationFloor);
			}
			dc.ChangeDungeonScene(DestinationDungeon, DestinationFloor);
		}
	}
}