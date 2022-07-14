using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using DunGen.Graph;
using UnityEngine.AddressableAssets;

public class DungeonData : SerializedScriptableObject
{
	public DungeonAreaEnum DungeonArea;
	[ValueDropdown("GetAllSceneName")]
	public string SceneName;
	List<string> GetAllSceneName()
	{
		return SRScenes.All.Select((scene)=>scene.name).ToList();
	}
	public string DungeonKoreanName;
	public float PlayerHeight = 0.3f;
	public float BattleOverlayAlpha = 0.6f;
	public AssetReferenceT<DungeonFlow> DungenFlow;
	[System.Serializable]
	public class MonsterSpawnInfo
	{
		public Monster MonsterPrefab;
		public int SpawnPossibilityInt;
		public float MinSpawnPossibility, MaxSpawnPossibility;
	}
	public List<MonsterSpawnInfo> MonsterSpawnInfoList;
	public int MinMonsterSpawn, MaxMonsterSpawn;
	public List<Monster> NamedMonsterPrefabs;
	public Monster BossMonster;
	public int MinNamedSpawn, MaxNamedSpawn;
	public int MinSkillChestSpawn, MaxSkillChestSpawn;
	public int MinEquipmentChestSpawn, MaxEquipmentChestSpawn;
	public int RecommendLevel;
	public int StartFloorNumber, EndFloorNumber;
	public int EquipmentPointTier1, EquipmentPointTier2;

	[System.Serializable]
	public class EquipmentPossibility
	{
		public RarityTypeEnum Rarity;
		public int PossibilityInt;
		public float PossibilityMin, PossibilityMax;
	}
	public List<EquipmentPossibility> EquipmentPossiblities;
}
