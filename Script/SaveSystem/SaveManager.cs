using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static SingletonLoader;

public class SaveManager : Singleton<SaveManager>
{
	public DataFormat SaveDataFormat;
	Dictionary<string, byte[]> SaveDatas;
	string SavePath, ScoreSavePath;

	void Start()
	{
		SaveDatas = new Dictionary<string, byte[]>();
		HighScores = new List<ScoreData>();
		SavePath = Application.persistentDataPath + "/save.sav";
		ScoreSavePath = Application.persistentDataPath + "/score.sav";
	}

	public void SaveGame()
	{
		Save("DungeonController", dc);
		Save("EncounterManager", em);
		Save("Player", p);
		Store();
	}

	List<Action> DelayedLoadingList;
	public void LoadGame()
	{
		Cache();
		StartLoad();
		Load<DungeonController.SaveData>("DungeonController", dc);
		Load<EncounterEventManager.SaveData>("EncounterManager", em);
		Load<Player.SaveData>("Player", p);
		EndLoad();
	}

	public void StartLoad()
	{
		DelayedLoadingList = new List<Action>();
	}

	public void EndLoad()
	{
		foreach (Action DelayedLoading in DelayedLoadingList)
		{
			DelayedLoading();
		}
	}

	public bool FileExists() => File.Exists(SavePath);
	public void DeleteFile() =>	File.Delete(SavePath);

	void Save(string Key, ISerialize SavingObject)
	{
		byte[] Bytes = SerializationUtility.SerializeValue(SavingObject.SerializeThisObject(), SaveDataFormat);
		SaveDatas[Key] = Bytes;
	}

	void Load<T>(string Key, ISerialize LoadingObject)
	{
		LoadingObject.DeserializeThisObject(SerializationUtility.DeserializeValue<T>(SaveDatas[Key], SaveDataFormat));
	}

	void Store() => File.WriteAllBytes(SavePath, SerializationUtility.SerializeValue(SaveDatas, SaveDataFormat));

	void Cache() => SaveDatas = SerializationUtility.DeserializeValue<Dictionary<string, byte[]>>(File.ReadAllBytes(SavePath), SaveDataFormat);

	public struct GameObjectData
	{
		public Vector3 Position, Rotation, Scale;
		public string ParentID;
		public List<(string, object)> ComponentData;
	}

	public byte[] SerializeGeneralGameObject(GameObject GameObjectToSave)
	{
		if (!GameObjectToSave) return null;
		if (!GameObjectToSave.GetComponent<ReferenceID>())
			throw new Exception($"{GameObjectToSave} : ReferenceID 없음");
		GameObjectData Data = new GameObjectData()
		{
			Position = GameObjectToSave.transform.localPosition,
			Rotation = GameObjectToSave.transform.localEulerAngles,
			Scale = GameObjectToSave.transform.localScale,
			ParentID = GameObjectToSave.transform.parent?.GetComponent<ReferenceID>()?.InstanceID,
			ComponentData = new List<(string, object)>()
		};
		ISerialize[] Components = GameObjectToSave.GetComponents<ISerialize>();
		foreach (ISerialize OneComponent in Components)
		{
			Data.ComponentData.Add((OneComponent.GetType().Name, OneComponent.SerializeThisObject()));
		}
		return SerializationUtility.SerializeValue(Data, SaveDataFormat);
	}

	public GameObject DeserializeGeneralGameObject(byte[] SerializedData)
	{
		if (SerializedData == null) return null;
		GameObjectData Data = SerializationUtility.DeserializeValue<GameObjectData>(SerializedData, SaveDataFormat);
		(string PrefabID, string InstanceID) = ((string, string))Data.ComponentData.Find(x => x.Item1 == "ReferenceID").Item2;
		GameObject NewGameObject = Instantiate(db.LoadAssetReference<GameObject>(db.GlobalAssetReferences[PrefabID]));
		if (!string.IsNullOrEmpty(Data.ParentID))
		{
			DelayedLoadingList.Add(() =>
			{
				NewGameObject.transform.SetParent(db.SceneObjectReferences[Data.ParentID].transform);
			});
		}
		NewGameObject.transform.localPosition = Data.Position;
		NewGameObject.transform.localEulerAngles = Data.Rotation;
		NewGameObject.transform.localScale = Data.Scale;
		foreach ((string ComponentName, object ComponentData) in Data.ComponentData)
		{
			ISerialize SerializerInterface = (ISerialize)NewGameObject.GetComponent(ComponentName);
			SerializerInterface.DeserializeThisObject(ComponentData);
		}
		return NewGameObject;
	}

	public struct ScoreData
	{
		public string Name;
		public int Score;
	}
	[ReadOnly] public List<ScoreData> HighScores;
	public void SaveHighScore(ScoreData NewScoreData)
	{
		HighScores.Add(NewScoreData);
		HighScores.Sort((a, b) => a.Score - b.Score);
		if (HighScores.Count > 10) 
		{
			HighScores.RemoveRange(10, HighScores.Count - 10);
		}
		File.WriteAllBytes(ScoreSavePath, SerializationUtility.SerializeValue(HighScores, SaveDataFormat));
	}

	public void LoadHighScores()
	{
		HighScores = SerializationUtility.DeserializeValue<List<ScoreData>>(File.ReadAllBytes(ScoreSavePath), SaveDataFormat);
	}


#if UNITY_EDITOR
	[Button("저장파일 폴더 열기", ButtonHeight = 100)]
	public void OpenPersistentDataPath()
	{
		UnityEditor.EditorUtility.RevealInFinder(Application.persistentDataPath);
	}
#endif
}
