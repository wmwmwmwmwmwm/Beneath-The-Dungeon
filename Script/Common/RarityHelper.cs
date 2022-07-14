using UnityEngine;

public static class RarityHelper
{
	public static RarityTypeEnum StringToRarity(string str)
	{
		switch (str)
		{
			case "커먼": return RarityTypeEnum.Common;
			case "언커먼": return RarityTypeEnum.Uncommon;
			case "레어": return RarityTypeEnum.Rare;
			case "유니크": return RarityTypeEnum.Unique;
			case "에픽": return RarityTypeEnum.Epic;
		}
		Debug.LogWarning($"{str} 오타");
		return RarityTypeEnum.Common;
	}

	public static bool IsLower(RarityTypeEnum a, RarityTypeEnum b, bool IncludeEqual)
	{
		if (IncludeEqual) return (int)a<= (int)b;
		else return (int)a < (int)b;
	}

	public static bool IsHigher(RarityTypeEnum a, RarityTypeEnum b, bool IncludeEqual)
	{
		if (IncludeEqual) return (int)a >= (int)b;
		else return (int)a > (int)b;
	}
}
