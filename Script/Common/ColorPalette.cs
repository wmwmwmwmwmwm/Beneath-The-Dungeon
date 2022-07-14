using UnityEngine;

public class ColorPalette : Singleton<ColorPalette>
{
	public Color CommonIconBoundary, UncommonIconBoundary, RareIconBoundary, UniqueIconBoundary, EpicIconBoundary,
		CommonIconForeground, UncommonIconForeground, RareIconForeground, UniqueIconForeground, EpicIconForeground;
	public Color MonsterOutlineRed, MonsterOutlineYellow, MonsterOutlineGreen;
	public Color CommonGreenText, CommonGrayText, CommonRedText, CommonGreenImage, CommonGrayImage, CommonRedImage;
	public Color CommonText, UncommonText, RareText, UniqueText, EpicText;
	public Color ElementalFire, ElementalWater, ElementalLight, ElementalDark, ElementalHellfire, ElementalPoison, ElementalPlasma, ElementalAbyssal, ElementalChaos, ElementalLaw;
	public Color HPText, MPText, SPText;

	public Color GetCommonImageColor(int delta)
	{
		if (delta > 0) return CommonGreenImage;
		else if (delta < 0) return CommonRedImage;
		else return CommonGrayImage;
	}

	public Color GetCommonTextColor(int delta)
	{
		if (delta > 0) return CommonGreenText;
		else if (delta < 0) return CommonRedText;
		else return CommonGrayText;
	}

	public Color GetRarityTextColor(RarityTypeEnum Rarity)
	{
		return Rarity switch
		{
			RarityTypeEnum.Common => CommonText,
			RarityTypeEnum.Uncommon => UncommonText,
			RarityTypeEnum.Rare => RareText,
			RarityTypeEnum.Unique => UniqueText,
			RarityTypeEnum.Epic => EpicText,
			_ => throw new System.Exception($"{Rarity} rarity error")
		};
	}
}
