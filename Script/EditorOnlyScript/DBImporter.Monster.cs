#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;

public partial class DBImporter
{
	[Button("몬스터정보 csv 임포트", ButtonHeight = 80)]
	void MonsterImport()
	{
		DBManagerPrefab.MonsterDictionary = new Dictionary<string, Monster>();
		GameObject MonsterBasePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/0Game/Prefab/MonsterBase.prefab");
		Dictionary<string, Sprite> AllMonsterSprites = ReadAllFiles<Sprite>("png", "/2D Fantasy Art Assets Full Pack");
		TextAsset csvFile =  AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/0Game/CSVFiles/MonsterDatabase.csv");
		List<Dictionary<string, object>> MonsterDatas = CSVReader.Read(csvFile); 

		Dictionary<string, SkillData> AllSkillDictionary = new Dictionary<string, SkillData>();
		string[] AllSkillsPath = Directory.GetFiles(Application.dataPath + "/0Game/Prefab/Skill", "*.prefab");
		foreach (string SkillFullPath in AllSkillsPath)
		{
			string SkillPath = "Assets" + SkillFullPath.Replace(Application.dataPath, "").Replace('\\', '/');
			SkillData OneSkill = AssetDatabase.LoadAssetAtPath<GameObject>(SkillPath).GetComponent<SkillData>();
			AllSkillDictionary.Add(OneSkill.KoreanName, OneSkill);
		}

		foreach(Dictionary<string, object> monsterData in MonsterDatas)
		{
			string MonsterPrefabPath = string.Format("Assets/0Game/Prefab/Monster/{0}/{1}_{2}.prefab", monsterData["최초 출현장소"].ToString(), monsterData["번호"].ToString(), monsterData["ID"].ToString());
			GameObject MonsterPrefab = GetOrCreatePrefab(MonsterBasePrefab, MonsterPrefabPath);
			Monster MonsterComponent = MonsterPrefab.GetComponent<Monster>();
			MonsterComponent.MonsterID = (string)monsterData["ID"];
			MonsterComponent.Status.Level = (int)monsterData["레벨"];
			MonsterComponent.MonsterKoreanName = (string)monsterData["한글이름"];
			string MonsterTypeString = (string)monsterData["몬스터 타입"];
			switch(MonsterTypeString)
			{
				case "일반":
					MonsterComponent.MonsterType = Monster.MonsterTypeEnum.Common;
					break;
				case "네임드":
					MonsterComponent.MonsterType = Monster.MonsterTypeEnum.Named;
					break;
				case "NPC":
					MonsterComponent.MonsterType = Monster.MonsterTypeEnum.NPC;
					break;
				default:
					MonsterComponent.MonsterType = Monster.MonsterTypeEnum.Boss;
					break;
			}
			try
			{
				Sprite MonsterSprite = AllMonsterSprites[(string)monsterData["일러스트 파일이름"]];
				MonsterComponent.MonsterSprite = MonsterSprite;
				MonsterComponent.WorldSprite.sprite = MonsterSprite;
				MonsterComponent.WorldSpriteOutline.sprite = MonsterSprite;
			}
			catch
			{
				Debug.LogError($"{monsterData["한글이름"]}의 일러스트 파일이름 에러!!");
			}
			int.TryParse(monsterData["체력"].ToString(), out int HP);
			MonsterComponent.Status.CurrentHP = MonsterComponent.Status.MaxHP = HP;
			int.TryParse(monsterData["마나"].ToString(), out int MP);
			MonsterComponent.Status.CurrentMP = MonsterComponent.Status.MaxMP = MP;
			int.TryParse(monsterData["스테미나"].ToString(), out int ST);
			MonsterComponent.Status.CurrentSP = MonsterComponent.Status.MaxSP = ST;
			int.TryParse(monsterData["방어력"].ToString(), out int Armor);
			MonsterComponent.Status.Armor = Armor;
			int.TryParse(monsterData["힘"].ToString(), out int STR);
			MonsterComponent.Status.STR = STR;
			int.TryParse(monsterData["민첩"].ToString(), out int DEX);
			MonsterComponent.Status.DEX = DEX;
			int.TryParse(monsterData["지능"].ToString(), out int INT);
			MonsterComponent.Status.INT = INT;
			int.TryParse(monsterData["인내"].ToString(), out int CON);
			MonsterComponent.Status.CON = CON;
			MonsterComponent.Status.ElementalType = ElementalHelper.StringToEnum(monsterData["속성"].ToString());
			MonsterComponent.Status.Skills = new List<GameObject>();
			string SpecialSkillName = monsterData["전용 기술"].ToString();
			for (int i = 0; i < 10; i++)
			{
				string SkillString = string.Format("기술{0}", (i + 1).ToString());
				string MonsterSkillKoreanName = monsterData[SkillString].ToString();
				if (!AllSkillDictionary.TryGetValue(MonsterSkillKoreanName, out SkillData MonsterSkill))
				{
					Debug.LogError($"{monsterData["한글이름"]}의 {MonsterSkillKoreanName} 스킬 이름 에러!!");
				}
				MonsterComponent.Status.Skills.Add(MonsterSkill?.gameObject);
				if (MonsterSkillKoreanName == SpecialSkillName)
				{
					MonsterComponent.SpecialSkill = MonsterSkill;
				}
			}

			DBManagerPrefab.MonsterDictionary[MonsterComponent.MonsterID] = MonsterComponent;
			EditorUtility.SetDirty(MonsterPrefab);
		}
		EditorUtility.SetDirty(DBManagerPrefab);
		AssetDatabase.SaveAssets();
		Debug.Log("몬스터 정보 임포트 완료");
	}

	//[Button("몬스터 폴더옮기기")]
	//public void aaa()
	//{
	//	TextAsset csvFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/0Game/CSVFiles/MonsterDatabase.csv");
	//	List<Dictionary<string, object>> MonsterDatas = CSVReader.Read(csvFile);
	//	foreach (Dictionary<string, object> monsterData in MonsterDatas)
	//	{
	//		string MonsterPrefabName = string.Format("{0}_{1}", monsterData["번호"].ToString(), monsterData["ID"].ToString());
	//		string DungeonName = monsterData["최초 출현장소"].ToString();
	//		string OldPath = string.Format("Assets/0Game/Prefab/Monster/{0}.prefab", MonsterPrefabName);
	//		string NewPath = string.Format("Assets/0Game/Prefab/Monster/{0}/{1}.prefab", DungeonName, MonsterPrefabName);
	//		string Result = AssetDatabase.MoveAsset(OldPath, NewPath);
	//		if(!string.IsNullOrEmpty(Result)) print(Result);
	//	}
	//}
}
#endif
