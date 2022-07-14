using UnityEngine;
using Sirenix.OdinInspector;

public abstract class Singleton<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
{
	public static T Instance;
	public static void CreateInstance(GameObject SingletonPrefab)
	{
		if (Instance == null)
		{
			GameObject NewGameObject = Instantiate(SingletonPrefab);
			Instance = NewGameObject.GetComponent<T>();
			DontDestroyOnLoad(NewGameObject);
		}
	}
}
