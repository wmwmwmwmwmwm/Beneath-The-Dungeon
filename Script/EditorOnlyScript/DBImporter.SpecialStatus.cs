#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using static SpecialStatusData;

public partial class DBImporter
{
	[Button("상태이상 csv 임포트", ButtonHeight = 80)]
	void SpecialStatusImport()
	{
		GameObject BasePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/0Game/Prefab/Base/SpecialStatusBase.prefab");
		Dictionary<string, Sprite> AllSkillIconSprites = ReadAllFiles<Sprite>("png", "/SpellBookMegaPack", "/2000_Icons/500_skillicons");
		TextAsset csvFile =  AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/0Game/CSVFiles/SpecialStatusDatabase.csv");
		List<Dictionary<string, object>> SpecialStatusDatas = CSVReader.Read(csvFile);
		DBManagerPrefab.SpecialStatusDictionary = new Dictionary<string, SpecialStatusData>();
		foreach (Dictionary<string, object> csvData in SpecialStatusDatas)
		{
			string PrefabPath = string.Format("Assets/0Game/Prefab/Special Status/{0}_{1}.prefab", csvData["번호"].ToString(), csvData["ID"].ToString());
			GameObject Prefab = GetOrCreatePrefab(BasePrefab, PrefabPath);
			SpecialStatusData DataComponent = Prefab.GetComponent<SpecialStatusData>();
			DataComponent.ID = (string)csvData["ID"];
			DataComponent.KoreanName = (string)csvData["한글이름"];
			try
			{
				DataComponent.IconSprite = AllSkillIconSprites[(string)csvData["아이콘 파일경로"]];
			}
			catch
			{
				Debug.LogError($"{csvData["한글이름"]}의 아이콘 파일이름 에러!!");
			}
			DataComponent.BuffType = csvData["버프디버프"].ToString() switch
			{
				"버프" => BuffTypeEnum.Buff,
				"디버프" => BuffTypeEnum.Debuff,
				"스킬" => BuffTypeEnum.Skill,
				_ => throw new System.Exception($"{csvData["버프디버프"]} 오타")
			};
			DataComponent.KoreanDescription = (string)csvData["설명"];
			DBManagerPrefab.SpecialStatusDictionary.Add(DataComponent.ID, DataComponent);
			EditorUtility.SetDirty(Prefab);
		}
		EditorUtility.SetDirty(DBManagerPrefab);
		AssetDatabase.SaveAssets();
		Debug.Log("상태이상 정보 임포트 완료");
	}
}
#endif
