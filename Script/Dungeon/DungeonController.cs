using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DunGen;
using DuloGames.UI;
using System;
using Random = UnityEngine.Random;
using System.Linq;
using static SingletonLoader;
using DunGen.Graph;
using UnityEngine.AddressableAssets;

public partial class DungeonController : Singleton<DungeonController>, ISerialize
{
	public RuntimeDungeon DungenComponent;

	[ReadOnly] public DungeonData CurrentDungeonData;
	[ReadOnly] public int CurrentFloor;
	[ReadOnly] public PortalTypeEnum LastUsedPortalType;
	[ReadOnly] public int LastUsedPortalIndex;
	[ReadOnly] public Dictionary<DungeonAreaEnum, int> DungeonEntranceFloor;
	[ReadOnly] public Dictionary<(DungeonAreaEnum, int), (int SkillChestCount, int EquipmentChestCount)> ChestCountDictionary;
	[ReadOnly] public Dictionary<(DungeonAreaEnum, int), List<GameObject>> DungeonNamedMonsters;
	[ReadOnly] public int Turn;

	public void NewGameSetup()
	{
		FloorSaveDatas = new Dictionary<string, FloorData>();
		LastUsedPortalType = PortalTypeEnum.Start;
		DungeonEntranceFloor = new Dictionary<DungeonAreaEnum, int>
		{
			[DungeonAreaEnum.LostTemple] = Random.Range(4, 8),
			[DungeonAreaEnum.ShadowTunnel] = Random.Range(4, 8),
		};
#if BTD_TEST
		Debug.Log($"LostTemple Floor : {DungeonEntranceFloor[DungeonAreaEnum.LostTemple]}" +
			$"ShadowTunnel Floor : {DungeonEntranceFloor[DungeonAreaEnum.ShadowTunnel]}");
#endif
		DungeonNamedMonsters = new Dictionary<(DungeonAreaEnum, int), List<GameObject>>();
		foreach (DungeonAreaEnum DungeonArea in Enum.GetValues(typeof(DungeonAreaEnum)))
		{
			DungeonData ThisDungeonData = db.DungeonDataDictionary[DungeonArea];
			for (int i = ThisDungeonData.StartFloorNumber; i <= ThisDungeonData.EndFloorNumber; i++)
			{
				DungeonNamedMonsters[(DungeonArea, i)] = new List<GameObject>();
			}
			List<Monster> NamedMonsterList = ThisDungeonData.NamedMonsterPrefabs.SampleList(Random.Range(ThisDungeonData.MinNamedSpawn, ThisDungeonData.MaxNamedSpawn + 1));
			foreach (Monster NamedMonster in NamedMonsterList)
			{
				int NamedAppearingFloor = Random.Range(ThisDungeonData.StartFloorNumber, ThisDungeonData.EndFloorNumber + 1);
				DungeonNamedMonsters[(DungeonArea, NamedAppearingFloor)].Add(NamedMonster.gameObject);
			}
		}
		SetTurn(1);

		ChestCountDictionary = new Dictionary<(DungeonAreaEnum, int), (int SkillChestCount, int EquipmentChestCount)>();
		Array AllDungeonAreas = Enum.GetValues(typeof(DungeonAreaEnum));
		foreach (DungeonAreaEnum DungeonArea in AllDungeonAreas)
		{
			DungeonData OneDungeonData = db.DungeonDataDictionary[DungeonArea];
			for (int i = OneDungeonData.StartFloorNumber; i <= OneDungeonData.EndFloorNumber; i++)
			{
				ChestCountDictionary.Add((DungeonArea, i), (0, 0));
			}
			int SkillChestCount = Random.Range(OneDungeonData.MinSkillChestSpawn, OneDungeonData.MaxSkillChestSpawn + 1);
			for (int i = 0; i < SkillChestCount; i++)
			{
				int Floor = Random.Range(OneDungeonData.StartFloorNumber, OneDungeonData.EndFloorNumber + 1);
				(int SkillChestCount, int EquipmentChestCount) ChestCountPair = ChestCountDictionary[(DungeonArea, Floor)];
				ChestCountPair.SkillChestCount++;
				ChestCountDictionary[(DungeonArea, Floor)] = ChestCountPair;
			}
			int EquipmentChestCount = Random.Range(OneDungeonData.MinEquipmentChestSpawn, OneDungeonData.MaxEquipmentChestSpawn + 1);
			for (int i = 0; i < EquipmentChestCount; i++)
			{
				int Floor = Random.Range(OneDungeonData.StartFloorNumber, OneDungeonData.EndFloorNumber + 1);
				(int SkillChestCount, int EquipmentChestCount) ChestCountPair = ChestCountDictionary[(DungeonArea, Floor)];
				ChestCountPair.EquipmentChestCount++;
				ChestCountDictionary[(DungeonArea, Floor)] = ChestCountPair;
			}
		}
	}

	public void ChangeDungeonScene(DungeonData DungeonDataToMove, int Floor)
	{
		StartCoroutine(ChangeDungeonCoroutine());
		IEnumerator ChangeDungeonCoroutine()
		{
			yield return StartCoroutine(cu.FadeOut(0.8f));
			if (gm.StartGameType == GameManager.StartGameTypeEnum.Playing) SaveMap();
			foreach (KeyValuePair<string, AssetReference> ReferencePair in db.GlobalAssetReferences)
			{
				db.UnloadAssetReference(ReferencePair.Value);
			} 
			if (CurrentDungeonData && DungeonDataToMove.DungenFlow != CurrentDungeonData.DungenFlow) 
			{
				db.UnloadAssetReference(CurrentDungeonData.DungenFlow);
			}
			CurrentDungeonData = DungeonDataToMove;
			DungenComponent.Generator.DungeonFlow = db.LoadAssetReference<DungeonFlow>(CurrentDungeonData.DungenFlow);
			CurrentFloor = Floor;
			UILoadingOverlayManager.Instance.Create().LoadScene(CurrentDungeonData.SceneName);
		}
	}

	public IEnumerator LoadFloor()
	{
		if (gm.StartGameType == GameManager.StartGameTypeEnum.NewGame) 
			NewGameSetup();
		if (gm.StartGameType != GameManager.StartGameTypeEnum.Playing)
		{
			p.RecalculatePlayerStatus();
			dc.SetTurn(dc.Turn);
			ui.UpdateRuneProgress();
			em.SetBattleSpeed(em.BattleSpeed);
		}

		UICanvas.Instance.AreaIndicatorText.text = string.Format("{0} {1}층", CurrentDungeonData.DungeonKoreanName, CurrentFloor);

		Map = new Dictionary<Vector2Int, RoomInformation>(new Vector2IntComparer());
		float FadeInTime;
		bool FirstVisit = !FloorSaveDatas.ContainsKey(GetDungeonKey());
		if (FirstVisit)
		{
			DungenComponent.Generator.ShouldRandomizeSeed = true;
			FadeInTime = 1.2f;
		}
		else
		{
			DungenComponent.Generator.ShouldRandomizeSeed = false;
			DungenComponent.Generator.Seed = FloorSaveDatas[GetDungeonKey()].Seed;
			FadeInTime = 0.5f;
		}
		yield return StartCoroutine(GenerateMap());
		UICanvas.Instance.CreateMinimapData();
		if (FirstVisit)
		{
			GenerateEventObjects();
		}
		else
		{
			LoadMap();
		}

		if (gm.StartGameType != GameManager.StartGameTypeEnum.LoadGame)
		{
			RoomInformation PlayerPlaceRoom = null;
			switch (LastUsedPortalType)
			{
				case PortalTypeEnum.Start:
					PlayerPlaceRoom = Map[AllEventObjects.FirstOrDefault((x) => x.GetComponent<EncounterEventDungeonExit>()).GetComponent<EventInformation>().GridPoint];
					break;
				case PortalTypeEnum.Upstair:
					PlayerPlaceRoom = GetStairRoom(PortalTypeEnum.Downstair);
					break;
				case PortalTypeEnum.Downstair:
					PlayerPlaceRoom = GetStairRoom(PortalTypeEnum.Upstair);
					break;
				case PortalTypeEnum.Entrance:
					PlayerPlaceRoom = Map[AllEventObjects.FirstOrDefault((x) => x.GetComponent<EncounterEventEntrance>()).GetComponent<EventInformation>().GridPoint];
					break;
			}
			RoomInformation GetStairRoom(PortalTypeEnum StairType)
			{
				List<EncounterEventStair> AllStairs = AllEventObjects.Where((x) => x.GetComponent<EncounterEventStair>()).Select((x) => x.GetComponent<EncounterEventStair>()).ToList();
				EncounterEventStair FoundStair = AllStairs.Find((x) => x.PortalType == StairType && x.StairIndex == LastUsedPortalIndex);
				return Map[FoundStair.GetComponent<EventInformation>().GridPoint];
			}
			p.TeleportPosition(PlayerPlaceRoom);
			p.LookAt = Vector2Int.up;
			while (!p.CanMoveForward())
			{
				p.LookAt = GridHelper.TurnRight(p.LookAt);
			}
			p.SetRotation(p.LookAt, true);
		}
		else
		{
			p.TeleportPosition(dc.Map[p.GridPosition]);
			p.SetRotation(p.LookAt, true);
		}
		UICanvas.Instance.GaugeGroup.UpdateAllStatusImmediate(p.Status, true);
		StartCoroutine(CommonUI.Instance.FadeIn(FadeInTime));

		gm.StartGameType = GameManager.StartGameTypeEnum.Playing;
	}

	public void OnPlayerTurnComplete(bool SelfEncounter)
	{
		if (SelfEncounter && Map.TryGetValue(p.GridPosition, out RoomInformation EncounterRoom) && EncounterRoom.EventObject)
		{
			StartCoroutine(EncounterRoom.EventObject.GetComponent<IEncounterEvent>().OnPlayerEncounter());
		}
		p.Rest();
		UICanvas.Instance.GaugeGroup.UpdateAllStatusImmediate(p.Status, false);
		SetTurn(Turn + 1);
		ProcessMonstersTurn();
	}

	public void OnBattleComplete(int ElapsedTurn)
	{
		SetTurn(Turn + ElapsedTurn);
	}

	public void OnLevelUp()
	{
		foreach (RoomInformation OneRoom in Map.Values)
		{
			if (!OneRoom.EventObject) continue;
			if (OneRoom.EventObject.TryGetComponent(out Monster MonsterObject))
			{
				MonsterObject.UpdateWorldSpriteOutlineColor();
			}
		}
	}

	public void SetTurn(int _Turn)
	{
		Turn = _Turn;
		UICanvas.Instance.TurnText.text = string.Format("T {0}", Turn);
	}

	void ProcessMonstersTurn()
	{
#warning monster move when proceed turn (need to check overlap event objects)
	}

	public bool IsMainDungeon(DungeonAreaEnum DungeonArea)
	{
		return DungeonArea == DungeonAreaEnum.MainDungeon13
			|| DungeonArea == DungeonAreaEnum.MainDungeon47
			|| DungeonArea == DungeonAreaEnum.MainDungeon812
			|| DungeonArea == DungeonAreaEnum.MainDungeon1315;
	}

	public DungeonData GetMainDungeonData(int Floor)
	{
		if (Floor < 4) return db.DungeonDataDictionary[DungeonAreaEnum.MainDungeon13];
		else if (Floor < 8) return db.DungeonDataDictionary[DungeonAreaEnum.MainDungeon47];
		else if (Floor < 13) return db.DungeonDataDictionary[DungeonAreaEnum.MainDungeon812];
		else return db.DungeonDataDictionary[DungeonAreaEnum.MainDungeon1315];
	}
}
