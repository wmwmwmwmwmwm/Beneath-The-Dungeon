using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class SkillIcon : MonoBehaviour
{
	public ItemIcon IconComponent;
	public SkillData ThisSkillData { get; private set; }
	[ReadOnly] public List<GameObject> PlayerSkillGroup;
	[ReadOnly] public int PlayerSkillIndex;

	public void SetSkillIconData(SkillData SkillDataToSet)
	{
		IconComponent.UpdateRarityColor(SkillDataToSet.Rarity);
		IconComponent.Icon.sprite = SkillDataToSet.IconSprite;
		ThisSkillData = SkillDataToSet;
	}

	public void SetSpecialStatusData(SpecialStatusData SpecialStatus)
	{
		IconComponent.Icon.sprite = SpecialStatus.IconSprite;
	}
}
