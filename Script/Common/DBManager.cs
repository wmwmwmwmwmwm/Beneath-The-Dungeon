using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using static DBManager.EquipmentNameInfo;
using static EquipmentElement;
using Object = UnityEngine.Object;

public partial class DBManager : Singleton<DBManager>
{
	public Dictionary<string, AssetReference> GlobalAssetReferences;
	public Dictionary<string, ReferenceID> SceneObjectReferences;
	public GameObject StairPrefab, EquipmentChestSteelPrefab, SkillChestSteelPrefab, DungeonExitPrefab;
	public Dictionary<DungeonAreaEnum, GameObject> DungeonEntranceDictionary;
	public Dictionary<RaceTypeEnum, RaceData> RaceDictionary;
	public Dictionary<DungeonAreaEnum, DungeonData> DungeonDataDictionary;
	public Dictionary<string, SpecialStatusData> SpecialStatusDictionary;
	public Dictionary<ElementalTypeEnum, Sprite> ElementalTypeIconDictionary;
	public Dictionary<string, SkillData> SkillDictionary;
	public Dictionary<int, LevelData> LevelDataDictionary;
	public List<EquipmentIcon> AllEquipmentIcons;
	Dictionary<EquipmentDropType3.TypeEnum, List<EquipmentIcon>> EquipmentIconByType3;
	public List<EquipmentDropType1> AllEquipmentDropType1;
	public List<EquipmentDropType2> AllEquipmentDropType2;
	public List<EquipmentDropType3> AllEquipmentDropType3;
	public List<EquipmentNameInfo> AllEquipmentNameInfo;
	Dictionary<NameKind1Enum, List<EquipmentNameInfo>> EquipmentNameInfoByKind1;
	Dictionary<NameKind2Enum, List<EquipmentNameInfo>> EquipmentNameInfoByKind2;
	public List<EquipmentElement> AllEquipmentElement;
	Dictionary<int, List<EquipmentElement>> EquipmentElementByTier;
	Dictionary<EquipmentElementTypeEnum, List<EquipmentElement>> EquipmentElementByType;
#if UNITY_EDITOR
	public Dictionary<string, Monster> MonsterDictionary;
#endif

	void Start()
	{
		EquipmentNameInfoByKind1 = new Dictionary<NameKind1Enum, List<EquipmentNameInfo>>();
		EquipmentNameInfoByKind2 = new Dictionary<NameKind2Enum, List<EquipmentNameInfo>>();
		foreach (EquipmentNameInfo OneNameInfo in AllEquipmentNameInfo)
		{
			EquipmentNameInfoByKind1.TryAndAddDictionaryList(OneNameInfo.NameKind1, OneNameInfo);
			EquipmentNameInfoByKind2.TryAndAddDictionaryList(OneNameInfo.NameKind2, OneNameInfo);
		}
		EquipmentElementByTier = new Dictionary<int, List<EquipmentElement>>();
		foreach (EquipmentElement OneElement in AllEquipmentElement)
		{
			EquipmentElementByTier.TryAndAddDictionaryList(OneElement.Tier, OneElement);
		}
		EquipmentElementByType = new Dictionary<EquipmentElementTypeEnum, List<EquipmentElement>>();
		foreach (EquipmentElement OneElement in AllEquipmentElement)
		{
			EquipmentElementByType.TryAndAddDictionaryList(OneElement.EquipmentElementType, OneElement);
		}
		EquipmentIconByType3 = new Dictionary<EquipmentDropType3.TypeEnum, List<EquipmentIcon>>();
		foreach (EquipmentIcon OneIcon in AllEquipmentIcons)
		{
			EquipmentIconByType3.TryAndAddDictionaryList(OneIcon.Type, OneIcon);
		}
	}

	public RaceData GetRaceByName(string RaceName)
	{
		return RaceDictionary.Where((x) => x.Key.ToString() == RaceName).First().Value;
	}

	public T LoadAssetReference<T>(AssetReference LoadingReference) where T : Object
	{
		if (!LoadingReference.Asset)
		{
			LoadingReference.LoadAssetAsync<T>().WaitForCompletion();
		}
		return (T)LoadingReference.Asset;
	}

	public void UnloadAssetReference(AssetReference LoadedReference)
	{
		if (LoadedReference.Asset != null) LoadedReference.ReleaseAsset();
	}
}
