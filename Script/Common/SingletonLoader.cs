using UnityEngine;

public class SingletonLoader : MonoBehaviour
{
	public CommonUI CommonUIManagerPrefab;
	public GameManager GameManagerPrefab;
	public DBManager DBManagerPrefab;
	public LocalizationManager LocalizationManagerPrefab;
	public Player PlayerPrefab;
	public AdManager AdManagerPrefab;
	public EncounterEventManager EncounterEventManagerPrefab;
	public UICanvas UICanvasPrefab;
	public DungeonController DungeonControllerPrefab;
	public ColorPalette ColorPalettePrefab;
	public AudioManager AudioManagerPrefab;
	public SaveManager SaveManagerPrefab;

	public static CommonUI cu => CommonUI.Instance;
	public static GameManager gm => GameManager.Instance;
	public static DBManager db => DBManager.Instance;
	public static LocalizationManager lm => LocalizationManager.Instance;
	public static Player p => Player.Instance;
	public static AdManager ad => AdManager.Instance;
	public static EncounterEventManager em => EncounterEventManager.Instance;
	public static UICanvas ui => UICanvas.Instance;
	public static DungeonController dc => DungeonController.Instance;
	public static ColorPalette cp => ColorPalette.Instance;
	public static AudioManager am => AudioManager.Instance;
	public static SaveManager sv => SaveManager.Instance;

#if UNITY_EDITOR
	void Start()
	{
		LoadSingletons();
	}
#endif

	public void LoadSingletons()
	{
		CommonUI.CreateInstance(CommonUIManagerPrefab.gameObject);
		GameManager.CreateInstance(GameManagerPrefab.gameObject);
		DBManager.CreateInstance(DBManagerPrefab.gameObject);
		LocalizationManager.CreateInstance(LocalizationManagerPrefab.gameObject);
		Player.CreateInstance(PlayerPrefab.gameObject);
		AdManager.CreateInstance(AdManagerPrefab.gameObject);
		EncounterEventManager.CreateInstance(EncounterEventManagerPrefab.gameObject);
		UICanvas.CreateInstance(UICanvasPrefab.gameObject);
		DungeonController.CreateInstance(DungeonControllerPrefab.gameObject);
		ColorPalette.CreateInstance(ColorPalettePrefab.gameObject);
		AudioManager.CreateInstance(AudioManagerPrefab.gameObject);
		SaveManager.CreateInstance(SaveManagerPrefab.gameObject);
	}
}
