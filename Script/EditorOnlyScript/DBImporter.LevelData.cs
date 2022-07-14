#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sirenix.OdinInspector;

public partial class DBImporter
{
	[Button("레벨데이터 csv 임포트", ButtonHeight = 80)]
	void LevelDataImport()
	{
		TextAsset csvFile =  AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/0Game/CSVFiles/LevelDatabase.csv");
		List<Dictionary<string, object>> LevelDataRecords = CSVReader.Read(csvFile);
		DBManagerPrefab.LevelDataDictionary = new Dictionary<int, LevelData>();
		foreach (Dictionary<string, object> csvData in LevelDataRecords)
		{
			LevelData NewLevelData = new LevelData();
			NewLevelData.Level = (int)csvData["레벨"];
			NewLevelData.ExpToLevelUp = (int)csvData["필요 경험치"];
			NewLevelData.MonsterExp = (int)csvData["몬스터 경험치"];
			DBManagerPrefab.LevelDataDictionary.Add(NewLevelData.Level, NewLevelData);
		}
		EditorUtility.SetDirty(DBManagerPrefab);
		AssetDatabase.SaveAssets();
		Debug.Log("레벨데이터 임포트 완료");
	}
}
#endif