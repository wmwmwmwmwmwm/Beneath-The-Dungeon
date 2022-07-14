using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ElementalHelper
{
	public static ElementalTypeEnum StringToEnum(string KoreanString)
	{
		switch(KoreanString)
		{
			case "무": return ElementalTypeEnum.None;
			case "화": return ElementalTypeEnum.Fire;
			case "수": return ElementalTypeEnum.Water;
			case "명": return ElementalTypeEnum.Light;
			case "암": return ElementalTypeEnum.Dark;
			case "지옥불": return ElementalTypeEnum.Hellfire;
			case "독": return ElementalTypeEnum.Poison;
			case "플라즈마": return ElementalTypeEnum.Plasma;
			case "심연": return ElementalTypeEnum.Abyssal;
			case "질서": return ElementalTypeEnum.Law;
			case "혼돈": return ElementalTypeEnum.Chaos;
		}
		Debug.LogWarning($"{KoreanString} 속성 오류");
		return ElementalTypeEnum.None;
	}

	public static float GetDamageMultiplier(ElementalTypeEnum AttackType, ElementalTypeEnum TargetType)
	{
		switch (AttackType)
		{
			case ElementalTypeEnum.None:
				switch(TargetType)
				{
					case ElementalTypeEnum.Plasma:
					case ElementalTypeEnum.Abyssal:
						return 0.5f;
				}
				break;
			case ElementalTypeEnum.Fire:
				switch(TargetType)
				{
					case ElementalTypeEnum.Water:
						return 2f;
					case ElementalTypeEnum.Poison:
						return 1.5f;
					case ElementalTypeEnum.Fire:
					case ElementalTypeEnum.Plasma:
					case ElementalTypeEnum.Abyssal:
					case ElementalTypeEnum.Law:
						return 0.5f;
				}
				break;
			case ElementalTypeEnum.Water:
				switch(TargetType)
				{
					case ElementalTypeEnum.Fire:
						return 2f;
					case ElementalTypeEnum.Poison:
						return 1.5f;
					case ElementalTypeEnum.Water:
					case ElementalTypeEnum.Plasma:
					case ElementalTypeEnum.Abyssal:
					case ElementalTypeEnum.Law:
						return 0.5f;
				}
				break;
			case ElementalTypeEnum.Light:
				switch (TargetType)
				{
					case ElementalTypeEnum.Dark:
						return 2f;
					case ElementalTypeEnum.Poison:
						return 1.5f;
					case ElementalTypeEnum.Light:
					case ElementalTypeEnum.Plasma:
					case ElementalTypeEnum.Abyssal:
					case ElementalTypeEnum.Law:
						return 0.5f;
				}
				break;
			case ElementalTypeEnum.Dark:
				switch (TargetType)
				{
					case ElementalTypeEnum.Light:
						return 2f;
					case ElementalTypeEnum.Poison:
						return 1.5f;
					case ElementalTypeEnum.Dark:
					case ElementalTypeEnum.Plasma:
					case ElementalTypeEnum.Abyssal:
					case ElementalTypeEnum.Law:
						return 0.5f;
				}
				break;
			case ElementalTypeEnum.Hellfire:
				switch(TargetType)
				{
					case ElementalTypeEnum.None:
						return 3f;
					case ElementalTypeEnum.Fire:
					case ElementalTypeEnum.Water:
					case ElementalTypeEnum.Light:
					case ElementalTypeEnum.Dark:
						return 2f;
				}
				break;
			case ElementalTypeEnum.Poison:
				switch (TargetType)
				{
					case ElementalTypeEnum.None:
					case ElementalTypeEnum.Fire:
					case ElementalTypeEnum.Water:
					case ElementalTypeEnum.Light:
					case ElementalTypeEnum.Dark:
						return 2f;
				}
				break;
			case ElementalTypeEnum.Plasma:
				switch(TargetType)
				{
					case ElementalTypeEnum.Dark:
					case ElementalTypeEnum.Chaos:
						return 2f;
				}
				break;
			case ElementalTypeEnum.Abyssal:
				switch (TargetType)
				{
					case ElementalTypeEnum.Light:
					case ElementalTypeEnum.Law:
						return 2f;
				}
				break;
			case ElementalTypeEnum.Chaos:
				switch (TargetType)
				{
					case ElementalTypeEnum.None:
						return 2f;
					case ElementalTypeEnum.Fire:
					case ElementalTypeEnum.Water:
					case ElementalTypeEnum.Light:
					case ElementalTypeEnum.Dark:
					case ElementalTypeEnum.Chaos:
						return 1.5f;
				}
				break;
			case ElementalTypeEnum.Law:
				switch (TargetType)
				{
					case ElementalTypeEnum.Chaos:
						return 2f;
				}
				break;
		}
		return 1f;
	}

	public static void SetElementalIconImage(Image ImageComponent, ElementalTypeEnum ElementalType)
	{
		ImageComponent.color = ElementalType switch
		{
			ElementalTypeEnum.None => Color.clear,
			ElementalTypeEnum.Fire => ColorPalette.Instance.ElementalFire,
			ElementalTypeEnum.Water => ColorPalette.Instance.ElementalWater,
			ElementalTypeEnum.Light => ColorPalette.Instance.ElementalLight,
			ElementalTypeEnum.Dark => ColorPalette.Instance.ElementalDark,
			ElementalTypeEnum.Hellfire => ColorPalette.Instance.ElementalHellfire,
			ElementalTypeEnum.Poison => ColorPalette.Instance.ElementalPoison,
			ElementalTypeEnum.Plasma => ColorPalette.Instance.ElementalPlasma,
			ElementalTypeEnum.Abyssal => ColorPalette.Instance.ElementalAbyssal,
			ElementalTypeEnum.Chaos => ColorPalette.Instance.ElementalChaos,
			ElementalTypeEnum.Law => ColorPalette.Instance.ElementalLaw,
			_ => throw new System.Exception($"{ElementalType} elemental error")
		};
		ImageComponent.sprite = DBManager.Instance.ElementalTypeIconDictionary[ElementalType];
	}

	public static int ElementalTypeEnumToInt(ElementalTypeEnum x)
	{
		return x switch
		{
			ElementalTypeEnum.None => 11,
			ElementalTypeEnum.Fire => 7,
			ElementalTypeEnum.Water => 8,
			ElementalTypeEnum.Light => 9,
			ElementalTypeEnum.Dark => 10,
			ElementalTypeEnum.Hellfire => 3,
			ElementalTypeEnum.Poison => 6,
			ElementalTypeEnum.Plasma => 4,
			ElementalTypeEnum.Abyssal => 5,
			ElementalTypeEnum.Chaos => 2,
			ElementalTypeEnum.Law => 1,
			_ => 0,
		};
	}
}
