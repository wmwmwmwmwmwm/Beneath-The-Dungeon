#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using static DungeonData;
using System;

public partial class DBImporter
{
	[Button("던전정보 csv 임포트", ButtonHeight = 80)]
	void DungeonImport()
	{
		TextAsset csvFile =  AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/0Game/CSVFiles/EncounterDatabase.csv");
		List<Dictionary<string, object>> AllEncounterDatas = CSVReader.Read(csvFile);
		TextAsset csvFile2 = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/0Game/CSVFiles/DungeonDatabase.csv");
		List<Dictionary<string, object>> DungeonDatas = CSVReader.Read(csvFile2);
		List<(DungeonAreaEnum DungeonArea, string KeyString)> DungeonAreaKeyList = new List<(DungeonAreaEnum DungeonArea, string KeyString)>();
		foreach (DungeonAreaEnum OneArea in Enum.GetValues(typeof(DungeonAreaEnum))) 
		{
			DungeonAreaKeyList.Add((OneArea, DungeonAreaToString(OneArea)));
		}
		foreach ((DungeonAreaEnum DungeonArea, string KeyString) OneDungeonAreaKey in DungeonAreaKeyList)
		{
			DungeonData OneDungeonData = DBManagerPrefab.DungeonDataDictionary[OneDungeonAreaKey.DungeonArea];
			OneDungeonData.MonsterSpawnInfoList = new List<MonsterSpawnInfo>();
			OneDungeonData.NamedMonsterPrefabs = new List<Monster>();
			List<Dictionary<string, object>> ThisEncounterDatas = AllEncounterDatas.FindAll((record) => record["장소"].ToString() == OneDungeonAreaKey.KeyString);
			foreach (Dictionary<string, object> EncounterData in ThisEncounterDatas)
			{
				Monster SpawnMonsterPrefab = DBManagerPrefab.MonsterDictionary.Where((monster) => monster.Value.MonsterKoreanName == EncounterData["출현대상"].ToString()).First().Value;
				switch (SpawnMonsterPrefab.MonsterType)
				{
					case Monster.MonsterTypeEnum.Named:
						OneDungeonData.NamedMonsterPrefabs.Add(SpawnMonsterPrefab);
						break;
					default:
						OneDungeonData.MonsterSpawnInfoList.Add(new MonsterSpawnInfo()
						{
							MonsterPrefab = SpawnMonsterPrefab,
							SpawnPossibilityInt = (int)EncounterData["확률"],
						});
						break;
				}
			}
			Util.CalculatePossibilities(OneDungeonData.MonsterSpawnInfoList, (x) => x.SpawnPossibilityInt, (x, MinValue) => x.MinSpawnPossibility = MinValue, (x, MaxValue) => x.MaxSpawnPossibility = MaxValue);
			Dictionary<string, object> DungeonCSVData = DungeonDatas.Where((record) => record["ID"].ToString() == OneDungeonAreaKey.KeyString).First();
			OneDungeonData.DungeonKoreanName = DungeonCSVData["이름"].ToString();
			OneDungeonData.MinMonsterSpawn = (int)DungeonCSVData["일반 몬스터 출현 최소"];
			OneDungeonData.MaxMonsterSpawn = (int)DungeonCSVData["일반 몬스터 출현 최대"];
			OneDungeonData.MinNamedSpawn = (int)DungeonCSVData["네임드 출현 최소"];
			OneDungeonData.MaxNamedSpawn = (int)DungeonCSVData["네임드 출현 최대"];
			OneDungeonData.MinSkillChestSpawn = (int)DungeonCSVData["기술 출현 최소"];
			OneDungeonData.MaxSkillChestSpawn = (int)DungeonCSVData["기술 출현 최대"];
			OneDungeonData.MinEquipmentChestSpawn = (int)DungeonCSVData["아이템 출현 최소"];
			OneDungeonData.MaxEquipmentChestSpawn = (int)DungeonCSVData["아이템 출현 최대"];
			OneDungeonData.RecommendLevel = (int)DungeonCSVData["권장 레벨"];
			OneDungeonData.StartFloorNumber = (int)DungeonCSVData["시작 층수"];
			OneDungeonData.EndFloorNumber = (int)DungeonCSVData["마지막 층수"];
			OneDungeonData.EquipmentPointTier1 = (int)DungeonCSVData["장비 포인트 티어1"];
			OneDungeonData.EquipmentPointTier2 = (int)DungeonCSVData["장비 포인트 티어2"];

			// 권장 레벨로 등장할 수 있는 아이템 등급 설정
			OneDungeonData.EquipmentPossiblities = new List<EquipmentPossibility>();
			switch (OneDungeonData.RecommendLevel)
			{
				case 1:
					OneDungeonData.EquipmentPossiblities = new List<EquipmentPossibility>()
					{
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Common },
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Uncommon },
					};
					break;
				case 11:
					OneDungeonData.EquipmentPossiblities = new List<EquipmentPossibility>()
					{
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Common },
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Uncommon },
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Rare },
					};
					break;
				case 21:
					OneDungeonData.EquipmentPossiblities = new List<EquipmentPossibility>()
					{
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Common },
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Uncommon },
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Rare },
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Unique },
					};
					break;
				case 31:
					OneDungeonData.EquipmentPossiblities = new List<EquipmentPossibility>()
					{
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Common },
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Uncommon },
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Rare },
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Unique },
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Epic },
					};
					break;
				case 41:
					OneDungeonData.EquipmentPossiblities = new List<EquipmentPossibility>()
					{
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Uncommon },
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Rare },
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Unique },
						new EquipmentPossibility() { Rarity = RarityTypeEnum.Epic },
					};
					break;
			}

			// 등장 확률은 등급 하나 상승시 마다 1/3배가 됨
			int EquipmentRarityCount = OneDungeonData.EquipmentPossiblities.Count;
			for (int i = 0; i < EquipmentRarityCount; i++)
			{
				OneDungeonData.EquipmentPossiblities[i].PossibilityInt = (int)Mathf.Pow(3, EquipmentRarityCount - i);
			}
			Util.CalculatePossibilities(OneDungeonData.EquipmentPossiblities, (x) => x.PossibilityInt, (x, PossibilityMin) => x.PossibilityMin = PossibilityMin, (x, PossiblityMax) => x.PossibilityMax = PossiblityMax);

			EditorUtility.SetDirty(OneDungeonData);
		}
		EditorUtility.SetDirty(DBManagerPrefab);
		AssetDatabase.SaveAssets();
		Debug.Log("던전 정보 임포트 완료");
	}

	string DungeonAreaToString(DungeonAreaEnum DungeonArea)
	{
		return DungeonArea switch
		{
			DungeonAreaEnum.MainDungeon13 => "main1",
			DungeonAreaEnum.MainDungeon47 => "main2",
			DungeonAreaEnum.MainDungeon812 => "main3",
			DungeonAreaEnum.MainDungeon1315 => "main4",
			DungeonAreaEnum.LostTemple => "temple",
			DungeonAreaEnum.ShadowTunnel => "tunnel",
			DungeonAreaEnum.OceanGarden => "garden",
			DungeonAreaEnum.Pandemonium => "pandemonium",
			DungeonAreaEnum.IronCanyon => "canyon",
			DungeonAreaEnum.AncientForest => "forest",
			DungeonAreaEnum.Necropolis => "necropolis",
			DungeonAreaEnum.SpiritGrave => "grave",
			DungeonAreaEnum.Inferno => "inferno",
			DungeonAreaEnum.Monolith => "monolith",
			_ => throw new System.Exception($"{DungeonArea} 오류"),
		};
	}
}
#endif