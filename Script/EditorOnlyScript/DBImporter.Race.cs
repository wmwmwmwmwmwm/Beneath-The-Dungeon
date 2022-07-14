#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;

public partial class DBImporter
{
	[Button("종족정보 csv 임포트", ButtonHeight = 80)]
	void RaceImport()
	{
		DBManagerPrefab.RaceDictionary = new Dictionary<RaceTypeEnum, RaceData>();
		TextAsset csvFile = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/0Game/CSVFiles/RaceDatabase.csv");
		List<Dictionary<string, object>> RaceDatas = CSVReader.Read(csvFile);
		foreach (Dictionary<string, object> raceData in RaceDatas)
		{
			string RaceDataPath = string.Format("Assets/0Game/ScriptableObjects/Race/{0}.asset", (string)raceData["ID"]);
			RaceData RaceDataTry = AssetDatabase.LoadAssetAtPath<RaceData>(RaceDataPath);
			RaceData EditingRaceData;
			if (RaceDataTry)
			{
				EditingRaceData = RaceDataTry;
			}
			else
			{
				EditingRaceData = ScriptableObject.CreateInstance<RaceData>();
				AssetDatabase.CreateAsset(EditingRaceData, RaceDataPath);
			}
			EditingRaceData.RaceType = (RaceTypeEnum)System.Enum.Parse(typeof(RaceTypeEnum), (string)raceData["ID"]);
			EditingRaceData.StartHP = (int)raceData["시작체력"];
			EditingRaceData.StartMP = (int)raceData["시작마력"];
			EditingRaceData.StartSP = (int)raceData["시작기력"];
			EditingRaceData.StartArmor = (int)raceData["시작방어력"];
			EditingRaceData.StartSTR = (int)raceData["힘"];
			EditingRaceData.StartDEX = (int)raceData["민첩"];
			EditingRaceData.StartINT = (int)raceData["지능"];
			EditingRaceData.StartCON = (int)raceData["인내"];
			EditingRaceData.StartElementalType = ElementalHelper.StringToEnum(raceData["속성"].ToString());
			EditingRaceData.LevelUpStatIncrement = (int)raceData["스텟 증가"];
			EditingRaceData.LevelUpSTRPossibilityInt = (int)raceData["힘 증가 계수"];
			EditingRaceData.LevelUpDEXPossibilityInt = (int)raceData["민첩 증가 계수"];
			EditingRaceData.LevelUpINTPossibilityInt = (int)raceData["지능 증가 계수"];
			EditingRaceData.LevelUpCONPossibilityInt = (int)raceData["인내 증가 계수"];
			List<(int IntValue, FloatRef FloatMin, FloatRef FloatMax)> PossibilityList = new List<(int, FloatRef, FloatRef)>()
			{
				(EditingRaceData.LevelUpSTRPossibilityInt, new FloatRef(), new FloatRef()),
				(EditingRaceData.LevelUpDEXPossibilityInt, new FloatRef(), new FloatRef()),
				(EditingRaceData.LevelUpINTPossibilityInt, new FloatRef(), new FloatRef()),
				(EditingRaceData.LevelUpCONPossibilityInt, new FloatRef(), new FloatRef())
			};
			Util.CalculatePossibilities(PossibilityList, (x) => x.IntValue, (x, MinValue) => x.FloatMin.Value = MinValue, (x, MaxValue) => x.FloatMax.Value = MaxValue);
			EditingRaceData.LevelUpSTRPossibilityMin = PossibilityList[0].FloatMin.Value;
			EditingRaceData.LevelUpSTRPossibilityMax = PossibilityList[0].FloatMax.Value;
			EditingRaceData.LevelUpDEXPossibilityMin = PossibilityList[1].FloatMin.Value;
			EditingRaceData.LevelUpDEXPossibilityMax = PossibilityList[1].FloatMax.Value;
			EditingRaceData.LevelUpINTPossibilityMin = PossibilityList[2].FloatMin.Value;
			EditingRaceData.LevelUpINTPossibilityMax = PossibilityList[2].FloatMax.Value;
			EditingRaceData.LevelUpCONPossibilityMin = PossibilityList[3].FloatMin.Value;
			EditingRaceData.LevelUpCONPossibilityMax = PossibilityList[3].FloatMax.Value;
			DBManagerPrefab.RaceDictionary.Add(EditingRaceData.RaceType, EditingRaceData);
			EditorUtility.SetDirty(EditingRaceData);
		}
		EditorUtility.SetDirty(DBManagerPrefab);
		AssetDatabase.SaveAssets();
		Debug.Log("종족 정보 임포트 완료");
	}
}
#endif
