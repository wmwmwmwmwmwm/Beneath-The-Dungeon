using UnityEngine;
using UnityEngine.UI;

public class ItemIcon : MonoBehaviour
{
	public Image Icon, Background, Foreground, Boundary, SelectionEffect;

	public void UpdateRarityColor(RarityTypeEnum Rarity)
	{
		Color ForegroundColor = Color.clear, BoundaryColor = Color.clear;
		switch (Rarity)
		{
			case RarityTypeEnum.Common: 
				ForegroundColor = ColorPalette.Instance.CommonIconForeground;
				BoundaryColor = ColorPalette.Instance.CommonIconBoundary;
				break;
			case RarityTypeEnum.Uncommon:
				ForegroundColor = ColorPalette.Instance.UncommonIconForeground;
				BoundaryColor = ColorPalette.Instance.UncommonIconBoundary;
				break;
			case RarityTypeEnum.Rare:
				ForegroundColor = ColorPalette.Instance.RareIconForeground;
				BoundaryColor = ColorPalette.Instance.RareIconBoundary;
				break;
			case RarityTypeEnum.Unique:
				ForegroundColor = ColorPalette.Instance.UniqueIconForeground;
				BoundaryColor = ColorPalette.Instance.UniqueIconBoundary;
				break;
			case RarityTypeEnum.Epic:
				ForegroundColor = ColorPalette.Instance.EpicIconForeground;
				BoundaryColor = ColorPalette.Instance.EpicIconBoundary;
				break;
		}
		Foreground.color = ForegroundColor;
		Boundary.color = BoundaryColor;
	}
}
