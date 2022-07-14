using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Monster : MonoBehaviour
{
	[TabGroup("Monster Datas")] public string MonsterID;
	[TabGroup("Monster Datas")] public enum MonsterTypeEnum { Common, Named, Boss, NPC }
	[TabGroup("Monster Datas")] public MonsterTypeEnum MonsterType;
	[TabGroup("Monster Datas")] public CommonStatus Status;
	[TabGroup("Monster Datas")] [ReadOnly] public CommonStatus BattleStatus;
	[TabGroup("Monster Datas")] public Vector3 PositionOffset;
	[TabGroup("Monster Datas")] public float SpriteScale;
	[TabGroup("Monster Datas")] public string MonsterKoreanName;
	[PreviewField(Height = 200f)]
	[TabGroup("Monster Datas")] public Sprite MonsterSprite;
	[TabGroup("Monster Datas")] public SkillData SpecialSkill;

	[TabGroup("References")] public Transform WorldSpriteParent;
	[TabGroup("References")] public SpriteRenderer WorldSprite, WorldSpriteOutline;
	[TabGroup("References")] public Material SpriteOutlineRedMaterial, SpriteOutlineYellowMaterial, SpriteOutlineGreenMaterial;


	void Start()
	{
		UpdateWorldSpriteOutlineColor();
	}

	public void UpdateWorldSpriteOutlineColor()
	{
		int LevelDifference = Status.Level - Player.Instance.Status.Level;
		if (LevelDifference >= 3)
		{
			//WorldSpriteOutline.color = ColorPalette.Instance.MonsterOutlineRed;
			WorldSpriteOutline.material = SpriteOutlineRedMaterial;
		}
		else if (LevelDifference >= 0)
		{
			//WorldSpriteOutline.color = ColorPalette.Instance.MonsterOutlineYellow;
			WorldSpriteOutline.material = SpriteOutlineYellowMaterial;
		}
		else
		{
			//WorldSpriteOutline.color = ColorPalette.Instance.MonsterOutlineGreen;
			WorldSpriteOutline.material = SpriteOutlineGreenMaterial;
		}
	}
}
