using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
	public AudioSource Bgm;
	public List<AudioSource> AllSfx;
	public enum BgmTypeEnum { None, CaveEntrance, };
	public BgmTypeEnum PlayingBgmType;
	public enum SfxTypeEnum { ClickIn, ClickOut, Door, LevelUp, Switching, Treasure, Walk, EnterEntrance, Stair };

	public AudioClip CaveEntrance;
	public AudioClip ClickIn, ClickOut, LevelUp, Treasure;
	public AudioClip EnterEntrance, Stair;
	public List<AudioClip> Door, Switching, Walk;

	public void PlayBgm(BgmTypeEnum BgmType)
	{
		//if (PlayingBgmType == BgmType) return;
		//PlayingBgmType = BgmType;
		//Bgm.clip = PlayingBgmType switch
		//{
		//	BgmTypeEnum.CaveEntrance => CaveEntrance,
		//	_ => null,
		//};
		//Bgm.Play();
	}

	public void PlaySfx(SfxTypeEnum SfxType)
	{
		AudioSource SfxSource = null;
		foreach (AudioSource OneSfx in AllSfx)
		{
			if (!OneSfx.isPlaying)
			{
				SfxSource = OneSfx;
				break;
			}
		}
		if (SfxSource == null) return;
		SfxSource.clip = SfxType switch
		{
			SfxTypeEnum.ClickIn => ClickIn,
			SfxTypeEnum.ClickOut => ClickOut,
			SfxTypeEnum.Door => Door.PickOne(),
			SfxTypeEnum.LevelUp => LevelUp,
			SfxTypeEnum.Switching => Switching.PickOne(),
			SfxTypeEnum.Treasure => Treasure,
			SfxTypeEnum.Walk => Walk.PickOne(),
			_ => null,
		};
		SfxSource.Play();
	}
}
