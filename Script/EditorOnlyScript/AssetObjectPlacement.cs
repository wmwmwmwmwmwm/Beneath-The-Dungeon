using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetObjectPlacement : MonoBehaviour
{
	public float MaxX;
	public Transform ObjectsParent;

	[Button("오브젝트들 보기좋게 배열하기", ButtonHeight = 100)]
	void Place()
	{
		Vector3 NextPlacePoint = Vector3.zero;
		float NextLineZ = 0f;
		foreach (Transform OneAsset in ObjectsParent)
		{
			OneAsset.transform.localPosition = NextPlacePoint;
			Vector3 Size = Vector3.one * 10f;
			Collider ColliderComponent = OneAsset.GetComponent<Collider>();
			if (ColliderComponent) Size = ColliderComponent.bounds.extents * 2f;
			NextLineZ = Mathf.Max(NextLineZ, Size.y);
			NextPlacePoint.x += Size.x;
			if (NextPlacePoint.x > MaxX)
			{
				NextPlacePoint.x = 0f;
				NextPlacePoint.z += NextLineZ;
				NextLineZ = 0f;
			}
		}
	}
}
