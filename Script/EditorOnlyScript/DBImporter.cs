#if UNITY_EDITOR
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class DBImporter : SerializedMonoBehaviour
{
	public DBManager DBManagerPrefab;

	GameObject GetOrCreatePrefab(GameObject BasePrefab, string PrefabPath)
	{
		GameObject PrefabTry = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabPath);
		GameObject Prefab;
		if (PrefabTry)
		{
			Prefab = PrefabTry;
		}
		else
		{
			GameObject BaseObject = (GameObject)PrefabUtility.InstantiatePrefab(BasePrefab);
			GameObject NewPrefab = PrefabUtility.SaveAsPrefabAsset(BaseObject, PrefabPath);
			Prefab = NewPrefab;
			GameObject.DestroyImmediate(BaseObject);
		}
		return Prefab;
	}

	static Dictionary<string, T> ReadAllFiles<T>(string Extension, params string[] SubPaths) where T : Object
	{
		Dictionary<string, T> ResultDictionary = new Dictionary<string, T>();
		foreach (string SubPath in SubPaths)
		{
			IEnumerable<string> AllFiles = Directory.EnumerateFiles(Application.dataPath + SubPath, "*." + Extension, SearchOption.AllDirectories);
			foreach (string filename in AllFiles)
			{
				string convertedFilename = "Assets" + filename.Substring(Application.dataPath.Length);
				ResultDictionary.Add(Path.GetFileNameWithoutExtension(filename), AssetDatabase.LoadAssetAtPath<T>(convertedFilename));
			}
		}
		return ResultDictionary;
	}

	[Button("글로벌 애셋 참조 새로고침", ButtonHeight = 80)]
	void GlobalReferenceRefresh()
	{
		DBManagerPrefab.GlobalAssetReferences = new Dictionary<string, AssetReference>();
		Dictionary<string, Object> AllScriptables = ReadAllFiles<Object>("asset", "/0Game/ScriptableObjects");
		AddReference(AllScriptables);
		Dictionary<string, Object> AllDungeonEntrances = ReadAllFiles<Object>("prefab", "/0Game/Prefab/DungeonEntrance");
		AddReference(AllDungeonEntrances);
		Dictionary<string, Object> AllEquipmentElements = ReadAllFiles<Object>("prefab", "/0Game/Prefab/EquipmentElement");
		AllEquipmentElements["EquipmentBase"] = AssetDatabase.LoadAssetAtPath<Object>("Assets/0Game/Prefab/Base/EquipmentBase.prefab");
		AddReference(AllEquipmentElements);
		Dictionary<string, Object> AllMonsters = ReadAllFiles<Object>("prefab", "/0Game/Prefab/Monster");
		AddReference(AllMonsters);
		Dictionary<string, Object> AllProps = ReadAllFiles<Object>("prefab", "/0Game/Prefab/Prop");
		AddReference(AllProps);
		Dictionary<string, Object> AllSkills = ReadAllFiles<Object>("prefab", "/0Game/Prefab/Skill");
		AddReference(AllSkills);
		Dictionary<string, Object> AllStatuses = ReadAllFiles<Object>("prefab", "/0Game/Prefab/Special Status");
		AddReference(AllStatuses);
		void AddReference(Dictionary<string, Object> Objects)
		{
			foreach (KeyValuePair<string, Object> Pair in Objects)
			{
				string ThisGuid;
				if (Pair.Value is GameObject Prefab)
				{
					ReferenceID ReferenceComponent = Prefab.GetComponent<ReferenceID>();
					if (ReferenceComponent)
					{
						ThisGuid = ReferenceComponent.PrefabID;
					}
					else
					{
						ThisGuid = System.Guid.NewGuid().ToString();
						ReferenceID IDComponent = Prefab.AddComponent<ReferenceID>();
						IDComponent.PrefabAsset = Prefab;
						IDComponent.PrefabID = ThisGuid;
					}
					DBManagerPrefab.GlobalAssetReferences.Add(ThisGuid, new AssetReference(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(Pair.Value))));
					EditorUtility.SetDirty(Pair.Value);
				}
				else
				{
					ThisGuid = System.Guid.NewGuid().ToString();
					DBManagerPrefab.GlobalAssetReferences.Add(ThisGuid, new AssetReference(AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(Pair.Value))));
				}
			}
		}
		EditorUtility.SetDirty(DBManagerPrefab);
		AssetDatabase.SaveAssets();
	}
}
#endif
