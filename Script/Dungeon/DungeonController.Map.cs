using DunGen;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SingletonLoader;
using static RoomInformation;

public class Vector2IntComparer : IEqualityComparer<Vector2Int>
{
	bool IEqualityComparer<Vector2Int>.Equals(Vector2Int a, Vector2Int b)
	{
		return (a.x == b.x && a.y == b.y);
	}

	int IEqualityComparer<Vector2Int>.GetHashCode(Vector2Int obj)
	{
		return obj.GetHashCode();
	}
}

public partial class DungeonController
{
	[ReadOnly] public Dictionary<Vector2Int, RoomInformation> Map;
	public List<GameObject> AllEventObjects => Map.Values.Where((x) => x.EventObject).Select((x) => x.EventObject).ToList();

	public IEnumerator GenerateMap()
	{
		DungenComponent.Generate();
		yield return new WaitUntil(() => DungenComponent.Generator.Status == GenerationStatus.Complete);
		Dungeon CurrentDungeon = DungenComponent.Generator.CurrentDungeon;
		Map = new Dictionary<Vector2Int, RoomInformation>();
		foreach (Tile OneTile in CurrentDungeon.AllTiles)
		{
			TileData TileDataComponent = OneTile.GetComponent<TileData>();
			foreach (RoomInformation OneRoomData in TileDataComponent.RoomDatas)
			{
				// 모든 Doorway가 Block되어있다면 Room을 Map에 등록하지 않음
				if (OneRoomData.AdjacentDoorways.All((doorway) => doorway && doorway.BlockerSceneObjects.Find((x) => x))) continue;
				OneRoomData.WorldGridPoint = GridHelper.WorldToGridPoint(OneRoomData.transform.position);
				foreach (Vector2Int Direction in GridHelper.AllDirections)
				{
					OneRoomData.WorldWalls[GridHelper.DirectionToIndex(Direction)] = IsThereWall(OneTile, OneRoomData, Direction);
				}
				Map.Add(OneRoomData.WorldGridPoint, OneRoomData);
				bool IsThereWall(Tile tile, RoomInformation RoomData, Vector2Int WorldDirection)
				{
					Vector2Int LocalDirection = GridHelper.WorldToLocalDirection(WorldDirection, RoomData.transform);
					return RoomData.RoomDirectionTypes[GridHelper.DirectionToIndex(LocalDirection)] switch
					{
						RoomDirectionTypeEnum.AlwaysBlocked => true,
						RoomDirectionTypeEnum.AlwaysOpen => false,
						RoomDirectionTypeEnum.OpenWhenConnected => !tile.UsedDoorways.Contains(RoomData.GetDungenDoor(LocalDirection)),
						_ => true,
					};
				}
			}
		}
	}

	void GenerateEventObjects()
	{
		List<RoomInformation> AllRooms = new List<RoomInformation>(Map.Values.ToList());
		AllRooms.ShuffleList();
		List<RoomInformation>.Enumerator AllRoomEnumerator = AllRooms.GetEnumerator();
		RoomInformation GetNextRandomRoom()
		{
			AllRoomEnumerator.MoveNext();
			return AllRoomEnumerator.Current;
		}

		// 몬스터 생성
		int GeneralMonsterSpawnNumber = Random.Range(CurrentDungeonData.MinMonsterSpawn, CurrentDungeonData.MaxMonsterSpawn + 1);
		for (int i = 0; i < GeneralMonsterSpawnNumber; i++)
		{
			RoomInformation RandomRoom = GetNextRandomRoom();
			float RandomValue = Random.value;
			Monster MonsterToSpawn = Util.GetWeightedItem(CurrentDungeonData.MonsterSpawnInfoList, (x) => x.MinSpawnPossibility, (x) => x.MaxSpawnPossibility).MonsterPrefab;
			CreateEventObject(RandomRoom, MonsterToSpawn.gameObject);
		}
		foreach (GameObject OneNamedPrefab in DungeonNamedMonsters[(CurrentDungeonData.DungeonArea, CurrentFloor)])
		{
			RoomInformation RandomRoom = GetNextRandomRoom();
			CreateEventObject(RandomRoom, OneNamedPrefab);
		}
		// 특수 던전 마지막 층은 보스가 등장
		if (CurrentFloor == CurrentDungeonData.EndFloorNumber && CurrentDungeonData.BossMonster)
		{
			RoomInformation RandomRoom = GetNextRandomRoom();
			CreateEventObject(RandomRoom, CurrentDungeonData.BossMonster.gameObject);
		}
		// 상자 생성
		int SkillChestSpawnCount = ChestCountDictionary[(CurrentDungeonData.DungeonArea, CurrentFloor)].SkillChestCount;
		for (int i = 0; i < SkillChestSpawnCount; i++)
		{
			RoomInformation RandomRoom = GetNextRandomRoom();
			GameObject NewSkillChest = CreateEventObject(RandomRoom, DBManager.Instance.SkillChestSteelPrefab);
			NewSkillChest.GetComponent<EncounterEventSkillChest>().SkillID = GetSpawnSkillID();
		}
		int EquipmentChestSpawnCount = ChestCountDictionary[(CurrentDungeonData.DungeonArea, CurrentFloor)].EquipmentChestCount;
		for (int i = 0; i < EquipmentChestSpawnCount; i++)
		{
			RoomInformation RandomRoom = GetNextRandomRoom();
			GameObject NewEquipmentChest = CreateEventObject(RandomRoom, DBManager.Instance.EquipmentChestSteelPrefab);
			NewEquipmentChest.GetComponent<EncounterEventEquipmentChest>().ContainEquipment = DBManager.Instance.GetRandomEquipment();
		}
		if (CurrentFloor > 1)
		{
			// 올라가는 계단 생성
			// 2층 이상일때 올라가는 계단들은 위층 계단들과 매칭되어야 한다
			for (int i = 0; i < 3; i++)
			{
				RoomInformation RandomRoom = GetNextRandomRoom();
				EncounterEventStair NewUpstair = CreateEventObject(RandomRoom, DBManager.Instance.StairPrefab).GetComponent<EncounterEventStair>();
				NewUpstair.PortalType = PortalTypeEnum.Upstair;
				NewUpstair.StairIndex = i + 1;
				NewUpstair.StairSetting(true);
			}
		}
		// 1층이라면 올라가는 계단들은 게임 종료 / 이전 던전의 포탈이어야 한다, 메인 던전으로 돌아가는 출구는 메인 던전의 입구와 매칭되어야 한다
		else
		{
			RoomInformation RandomRoom = GetNextRandomRoom();
			switch (CurrentDungeonData.DungeonArea)
			{
				case DungeonAreaEnum.MainDungeon13:
					GameObject DungeonExit = CreateEventObject(RandomRoom, db.DungeonExitPrefab);
					break;
				case DungeonAreaEnum.LostTemple:
				case DungeonAreaEnum.ShadowTunnel:
					GameObject NewMainDungeonEntrance = CreateEventObject(RandomRoom, db.DungeonEntranceDictionary[DungeonAreaEnum.MainDungeon13]);
					break;
			}
		}
		// 내려가는 계단 생성
		if (CurrentFloor != CurrentDungeonData.EndFloorNumber
			|| CurrentDungeonData.DungeonArea == DungeonAreaEnum.MainDungeon13 
			)//|| CurrentDungeonData.DungeonArea == DungeonAreaEnum.MainDungeon47 
			//|| CurrentDungeonData.DungeonArea == DungeonAreaEnum.MainDungeon812)
		{
			for (int i = 0; i < 3; i++)
			{
				RoomInformation RandomRoom = GetNextRandomRoom();
				EncounterEventStair NewDownstair = CreateEventObject(RandomRoom, DBManager.Instance.StairPrefab).GetComponent<EncounterEventStair>();
				NewDownstair.PortalType = PortalTypeEnum.Downstair;
				NewDownstair.StairIndex = i + 1;
				NewDownstair.StairSetting(false);
			}
		}

		switch (CurrentDungeonData.DungeonArea)
		{
			case DungeonAreaEnum.MainDungeon47:
				if (CurrentFloor == DungeonEntranceFloor[DungeonAreaEnum.LostTemple])
				{
					// 잊혀진 신전으로 가는 입구 생성
					RoomInformation RandomRoom = GetNextRandomRoom();
					CreateEventObject(RandomRoom, db.DungeonEntranceDictionary[DungeonAreaEnum.LostTemple]);
				}
				if (CurrentFloor == DungeonEntranceFloor[DungeonAreaEnum.ShadowTunnel])
				{
					// 그림자 터널로 가는 입구 생성
					RoomInformation RandomRoom = GetNextRandomRoom();
					CreateEventObject(RandomRoom, db.DungeonEntranceDictionary[DungeonAreaEnum.ShadowTunnel]);
				}
				break;
		}
	}

	public string GetSpawnSkillID()
	{
		List<SkillData> SkillList = DBManager.Instance.SkillDictionary.Where((x) => CurrentDungeonData.RecommendLevel <= x.Value.RecommendLevel && CurrentDungeonData.RecommendLevel >= x.Value.RecommendLevel - 10).Select((x) => x.Value).ToList();
		Util.CalculatePossibilities(SkillList, (x) => x.SpawnPossibilityInt, (x, MinValue) => x.MinSpawnPossibility = MinValue, (x, MaxValue) => x.MaxSpawnPossibility = MaxValue);
		float RandomValue = Random.value;
		string SpawnSkillID = Util.GetWeightedItem(SkillList, (x) => x.MinSpawnPossibility, (x) => x.MaxSpawnPossibility).SkillID;
		return SpawnSkillID;
	}

	public GameObject CreateEventObject(RoomInformation Room, GameObject Prefab)
	{
		GameObject NewObject = Instantiate(Prefab);
		NewObject.transform.position = Room.transform.position;
		NewObject.GetComponent<EventInformation>().GridPoint = Room.WorldGridPoint;
		Room.EventObject = NewObject.gameObject;
		return NewObject;
	}

	public void RevealTile(Vector2Int GridPoint)
	{
		Map[GridPoint].IsRevealed = true;
		Map[GridPoint].MinimapTilePopup.SetActive(true);
		Map[GridPoint].MinimapTileSmall.SetActive(true);
	}

	public void OpenDoor(Vector2Int GridPoint, Vector2Int Direction)
	{
		RoomInformation CurrentRoom = Map[GridPoint];
		Vector2Int LocalDirection = GridHelper.WorldToLocalDirection(Direction, CurrentRoom.transform);
		GameObject DoorInstance = CurrentRoom.GetDungenDoor(LocalDirection)?.UsedDoorPrefabInstance;
		if (DoorInstance)
		{
			am.PlaySfx(AudioManager.SfxTypeEnum.Door);
			Animator AnimatorComponent = DoorInstance.GetComponent<Animator>();
			Vector2Int DoorDirection = GridHelper.WorldToGridDirection(DoorInstance.transform.eulerAngles);
			Vector2Int PlayerDirection = GridHelper.WorldToGridDirection(Player.Instance.transform.eulerAngles);
			if (DoorDirection == PlayerDirection)
			{
				AnimatorComponent.speed = 1f;
				AnimatorComponent.Play("DoorPush", 0, 0f);
			}
			else
			{
				AnimatorComponent.speed = 1f;
				AnimatorComponent.Play("DoorPull", 0, 0f);
			}
		}
	}
}
