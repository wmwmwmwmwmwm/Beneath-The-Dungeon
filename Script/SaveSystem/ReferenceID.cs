using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using static SingletonLoader;

[DisallowMultipleComponent]
public class ReferenceID : MonoBehaviour, ISerialize
{
	public Object PrefabAsset;
	public string PrefabID;
	[ReadOnly] public string InstanceID;

	void Start()
	{
		if (string.IsNullOrEmpty(InstanceID))
		{
			InstanceID = System.Guid.NewGuid().ToString();
			db.SceneObjectReferences[InstanceID] = this;
		}
	}

	public object SerializeThisObject()
	{
		return (PrefabID, InstanceID);
	}

	public void DeserializeThisObject(object SavedData)
	{
		(string PrefabID, string InstanceID) data = ((string, string))SavedData;
		PrefabID = data.PrefabID;
		InstanceID = data.InstanceID;
		db.SceneObjectReferences[InstanceID] = this;
	}
}
