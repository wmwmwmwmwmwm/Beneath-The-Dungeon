using DunGen;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SingletonLoader;

public partial class DungeonController
{
	public struct FloorData
	{
		public int Seed;
		public Dictionary<Vector2Int, bool> RevealDictionary;
		public List<byte[]> EventObjectDatas;
	}
	[ReadOnly] public Dictionary<string, FloorData> FloorSaveDatas;
	void SaveMap()
	{
		FloorData SaveDataThisFloor = new FloorData();
		SaveDataThisFloor.Seed = DungenComponent.Generator.ChosenSeed;
		SaveDataThisFloor.RevealDictionary = new Dictionary<Vector2Int, bool>();
		foreach (KeyValuePair<Vector2Int, RoomInformation> OneRoom in Map)
		{
			SaveDataThisFloor.RevealDictionary[OneRoom.Key] = OneRoom.Value.IsRevealed;
		}
		SaveDataThisFloor.EventObjectDatas = AllEventObjects.Select(x => sv.SerializeGeneralGameObject(x)).ToList();
		FloorSaveDatas[GetDungeonKey()] = SaveDataThisFloor;
	}

	void LoadMap()
	{
		sv.StartLoad();
		Dictionary<Vector2Int, bool> RevealDictionary = FloorSaveDatas[GetDungeonKey()].RevealDictionary;
		foreach (Tile OneTile in DungenComponent.Generator.CurrentDungeon.AllTiles)
		{
			foreach (RoomInformation OneRoom in OneTile.GetComponent<TileData>().RoomDatas)
			{
				Map[OneRoom.WorldGridPoint] = OneRoom;
				OneRoom.IsRevealed = RevealDictionary[OneRoom.WorldGridPoint];
				OneRoom.MinimapTilePopup.SetActive(OneRoom.IsRevealed);
				OneRoom.MinimapTileSmall.SetActive(OneRoom.IsRevealed);
			}
		}
		foreach (byte[] LoadedData in FloorSaveDatas[GetDungeonKey()].EventObjectDatas)
		{
			GameObject LoadedEvent = sv.DeserializeGeneralGameObject(LoadedData);
			RoomInformation OneRoom = Map[LoadedEvent.GetComponent<EventInformation>().GridPoint];
			OneRoom.EventObject = LoadedEvent;
			LoadedEvent.transform.position = OneRoom.transform.position;
		}
		sv.EndLoad();
	}

	string GetDungeonKey() => CurrentDungeonData.DungeonArea.ToString() + CurrentFloor.ToString();

	public struct SaveData
	{
		public DungeonAreaEnum CurrentDungeonDataSaved;
		public int CurrentFloorSaved;
		public PortalTypeEnum LastUsedPortalTypeSaved;
		public int LastUsedPortalIndexSaved;
		public Dictionary<DungeonAreaEnum, int> DungeonEntranceFloorSaved;
		public Dictionary<(DungeonAreaEnum, int), (int SkillChestCount, int EquipmentChestCount)> ChestCountDictionarySaved;
		public Dictionary<(DungeonAreaEnum, int), List<string>> DungeonNamedMonstersSaved;
		public int TurnSaved;
		public Dictionary<string, FloorData> FloorSaveDatasSaved;
	}
	public object SerializeThisObject()
	{
		SaveMap();
		SaveData NewSaveData = new SaveData()
		{
			CurrentDungeonDataSaved = CurrentDungeonData.DungeonArea,
			CurrentFloorSaved = CurrentFloor,
			LastUsedPortalTypeSaved = LastUsedPortalType,
			LastUsedPortalIndexSaved = LastUsedPortalIndex,
			DungeonEntranceFloorSaved = DungeonEntranceFloor,
			ChestCountDictionarySaved = ChestCountDictionary,
			DungeonNamedMonstersSaved = new Dictionary<(DungeonAreaEnum, int), List<string>>(),
			TurnSaved = Turn,
			FloorSaveDatasSaved = FloorSaveDatas
		};
		foreach (KeyValuePair<(DungeonAreaEnum, int), List<GameObject>> NamedMonsterListPair in DungeonNamedMonsters)
		{
			List<string> PrefabIDList = new List<string>();
			foreach (GameObject OneMonster in NamedMonsterListPair.Value)
			{
				PrefabIDList.Add(OneMonster.GetComponent<ReferenceID>().PrefabID);
			}
			NewSaveData.DungeonNamedMonstersSaved[NamedMonsterListPair.Key] = PrefabIDList;
		}
		return NewSaveData;
	}

	public void DeserializeThisObject(object SavedData)
	{
		SaveData LoadedData = (SaveData)SavedData;
		CurrentDungeonData = db.DungeonDataDictionary[LoadedData.CurrentDungeonDataSaved];
		CurrentFloor = LoadedData.CurrentFloorSaved;
		LastUsedPortalType = LoadedData.LastUsedPortalTypeSaved;
		LastUsedPortalIndex = LoadedData.LastUsedPortalIndexSaved;
		DungeonEntranceFloor = LoadedData.DungeonEntranceFloorSaved;
		ChestCountDictionary = LoadedData.ChestCountDictionarySaved;
		DungeonNamedMonsters = new Dictionary<(DungeonAreaEnum, int), List<GameObject>>();
		foreach (KeyValuePair<(DungeonAreaEnum, int), List<string>> LoadedNamedPair in LoadedData.DungeonNamedMonstersSaved)
		{
			List<GameObject> NamedPrefabList = new List<GameObject>();
			foreach (string NamedID in LoadedNamedPair.Value)
			{
				NamedPrefabList.Add(db.LoadAssetReference<GameObject>(db.GlobalAssetReferences[NamedID]));
			}
			DungeonNamedMonsters[LoadedNamedPair.Key] = NamedPrefabList;
		}
		Turn = LoadedData.TurnSaved;
		FloorSaveDatas = LoadedData.FloorSaveDatasSaved;
	}
}
