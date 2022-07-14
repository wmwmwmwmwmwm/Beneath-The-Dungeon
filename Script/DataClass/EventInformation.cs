using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public class EventInformation : MonoBehaviour, ISerialize
{
	[ReadOnly] public Vector2Int GridPoint;

	public object SerializeThisObject()
	{
		return GridPoint;
	}

	public void DeserializeThisObject(object SavedData)
	{
		GridPoint = (Vector2Int)SavedData;
	}
}
