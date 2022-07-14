using DuloGames.UI;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class FirstController : MonoBehaviour
{
	public SingletonLoader loader;
	void Start()
	{
		UILoadingOverlayManager.Instance.Create().LoadScene(SRScenes._100StartMenu.name);
		loader.LoadSingletons();
	}
}
