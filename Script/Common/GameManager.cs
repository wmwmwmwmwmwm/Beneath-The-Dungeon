using DuloGames.UI;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static SingletonLoader;

public partial class GameManager : Singleton<GameManager>
{
	public enum PhaseEnum { StartMenu, Dungeon };
	[ReadOnly] public PhaseEnum Phase;
	public enum StartGameTypeEnum { NewGame, LoadGame, Playing }
	[ReadOnly] public StartGameTypeEnum StartGameType;
	public bool EnableSave;

	void Start()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
		EnableSave = true;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == SRScenes._100StartMenu.name) Phase = PhaseEnum.StartMenu;
		else Phase = PhaseEnum.Dungeon;
		switch (Phase)
		{
			case PhaseEnum.StartMenu:
				UICanvas.Instance.gameObject.SetActive(false);
				Player.Instance.gameObject.SetActive(false);
				am.PlayBgm(AudioManager.BgmTypeEnum.CaveEntrance);
				break;
			case PhaseEnum.Dungeon:
				UICanvas.Instance.gameObject.SetActive(true);
				Player.Instance.gameObject.SetActive(true);
				am.PlayBgm(AudioManager.BgmTypeEnum.CaveEntrance);
				StartCoroutine(DungeonController.Instance.LoadFloor());
				break;
		}
	}

	void OnApplicationPause(bool pause)
	{
		if (EnableSave && pause && Phase == PhaseEnum.Dungeon)
		{
			if (pause) sv.SaveGame();
		}
	}

	void OnApplicationQuit()
	{
		if (EnableSave && Phase == PhaseEnum.Dungeon)
		{
			sv.SaveGame();
		}
	}
}
