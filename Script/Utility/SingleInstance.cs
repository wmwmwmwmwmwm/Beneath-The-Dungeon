using UnityEngine;
using Sirenix.OdinInspector;

public abstract class SingleInstance<T> : SerializedMonoBehaviour where T : SerializedMonoBehaviour
{
	public static T Instance;

	void Awake()
	{
		Instance = this as T;
	}
}
