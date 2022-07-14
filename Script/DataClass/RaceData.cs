using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class RaceData : ScriptableObject
{
	public RaceTypeEnum RaceType;
	public int StartHP, StartMP, StartSP;
	public int StartArmor;
	public int StartSTR, StartDEX, StartINT, StartCON;
	public ElementalTypeEnum StartElementalType;
	public SkillData DefaultSkill;
	public List<GameObject> StartSkills;
	public int LevelUpStatIncrement;
	public int LevelUpSTRPossibilityInt, LevelUpDEXPossibilityInt, LevelUpINTPossibilityInt, LevelUpCONPossibilityInt;
	[ReadOnly] public float LevelUpSTRPossibilityMin, LevelUpSTRPossibilityMax, LevelUpDEXPossibilityMin, LevelUpDEXPossibilityMax, LevelUpINTPossibilityMin, LevelUpINTPossibilityMax, LevelUpCONPossibilityMin, LevelUpCONPossibilityMax;
}
