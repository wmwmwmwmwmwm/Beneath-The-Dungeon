#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System.Linq;
using UnityEngine.Timeline;

public partial class DBImporter
{
	[Button("스킬정보 csv 임포트", ButtonHeight = 80)]
	void SkillImport()
	{
		GameObject SkillBasePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/0Game/Prefab/Base/SkillBase.prefab");
		Dictionary<string, Sprite> AllSkillIconSprites = ReadAllFiles<Sprite>("png", "/SpellBookMegaPack", "/2000_Icons/500_skillicons");
		Dictionary<string, TimelineAsset> AllSkillEffects = ReadAllFiles<TimelineAsset>("playable", "/0Game/Effect/SkillTimeline");
		TextAsset csvFile =  AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/0Game/CSVFiles/SkillDatabase.csv");
		List<Dictionary<string, object>> SkillDatas = CSVReader.Read(csvFile);
		DBManagerPrefab.SkillDictionary = new Dictionary<string, SkillData>();
		foreach(Dictionary<string, object> skillData in SkillDatas)
		{
			string SkillPrefabPath = string.Format("Assets/0Game/Prefab/Skill/{0}_{1}.prefab", skillData["번호"].ToString(), skillData["ID"].ToString());
			GameObject SkillPrefab = GetOrCreatePrefab(SkillBasePrefab, SkillPrefabPath);
			SkillData SkillComponent = SkillPrefab.GetComponent<SkillData>();
			SkillComponent.SkillID = skillData["ID"].ToString();
			SkillComponent.KoreanName = skillData["한글이름"].ToString();
			SkillComponent.RecommendLevel = (int)skillData["권장 레벨"];
			SkillComponent.Rarity = RarityHelper.StringToRarity(skillData["등급"].ToString());
			try
			{
				SkillComponent.IconSprite = AllSkillIconSprites[skillData["아이콘 파일경로"].ToString()];
			}
			catch
			{
				Debug.LogError($"{skillData["한글이름"]}의 아이콘 파일이름 에러!!");
			}
			SkillComponent.KoreanDescription = skillData["효과 설명"].ToString();
			SkillComponent.ElementalType = ElementalHelper.StringToEnum(skillData["속성"].ToString());
			SkillComponent.HPCost = 0; SkillComponent.MPCost = 0; SkillComponent.SPCost = 0;
			SkillComponent.HPCostPercent = 0f; SkillComponent.MPCostPercent = 0f; SkillComponent.SPCostPercent = 0f;
			List<string> CostNames = skillData["소모 스탯"].ToString().Split(' ').ToList();
			List<string> Costs = skillData["소모량"].ToString().Split(' ').ToList();
			List<string>.Enumerator CostEnumerator = Costs.GetEnumerator();
			foreach (string CostString in CostNames)
			{
				CostEnumerator.MoveNext();
				switch(CostString)
				{
					case "체력":
						SkillComponent.HPCost = int.Parse(CostEnumerator.Current);
						break;
					case "마력":
						SkillComponent.MPCost = int.Parse(CostEnumerator.Current);
						break;
					case "기력":
						SkillComponent.SPCost = int.Parse(CostEnumerator.Current);
						break;
					case "체력퍼센트":
						SkillComponent.HPCostPercent = float.Parse(CostEnumerator.Current);
						break;
					case "마력퍼센트":
						SkillComponent.MPCostPercent = float.Parse(CostEnumerator.Current);
						break;
					case "기력퍼센트":
						SkillComponent.SPCostPercent = float.Parse(CostEnumerator.Current);
						break;
				}
			}
			SkillComponent.SpawnPossibilityInt = (int)skillData["상자 등장 확률"];
			SkillComponent.RecommendLevel = (int)skillData["권장 레벨"];
			string SkillEffectName = skillData["스킬이펙트"].ToString();
			if (AllSkillEffects.TryGetValue(SkillEffectName, out TimelineAsset SkillEffectAsset)) SkillComponent.SkillEffect = SkillEffectAsset;
			else Debug.Log($"{SkillEffectName} 이름의 스킬이펙트 애셋 없음");
			DBManagerPrefab.SkillDictionary[SkillComponent.SkillID] = SkillComponent;
			EditorUtility.SetDirty(SkillPrefab);
		}
		EditorUtility.SetDirty(DBManagerPrefab);
		AssetDatabase.SaveAssets();
		Debug.Log("스킬 정보 임포트 완료");
	}
}
#endif
