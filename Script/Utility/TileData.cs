using DunGen;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using static RoomInformation;

public class TileData : MonoBehaviour
{
	public GameObject DoorPrefab;
	public List<RoomInformation> RoomDatas;
	[ReadOnly] public List<int> UsedDoorwayIndexes = new List<int>();

	[Button("Connector, Blocker에 Children 자동 할당, RoomData 계산", ButtonHeight = 100)]
	void Assign()
	{
		foreach (Transform OneDoor in transform)
		{
			OneDoor.TryGetComponent(out Doorway OneDoorway);
			if (!OneDoorway) continue;
			Transform Connector = OneDoor.GetChild(0);
			OneDoorway.ConnectorPrefabWeights.Clear();
			OneDoorway.ConnectorPrefabWeights.Add(new GameObjectWeight(null, 20));
			OneDoorway.ConnectorPrefabWeights.Add(new GameObjectWeight(DoorPrefab, 1));
			OneDoorway.ConnectorSceneObjects.Clear();
			foreach (Transform OneObject in Connector)
			{
				OneDoorway.ConnectorSceneObjects.Add(OneObject.gameObject);
			}
			OneDoorway.BlockerSceneObjects.Clear();
			Transform Blocker = OneDoor.GetChild(1);
			foreach (Transform OneObject in Blocker)
			{
				OneDoorway.BlockerSceneObjects.Add(OneObject.gameObject);
			}
		}

		foreach (RoomInformation RoomData in RoomDatas)
		{
			RoomData.AdjacentDoorways = new Doorway[4];
			RoomData.RoomDirectionTypes = new RoomDirectionTypeEnum[4];
			foreach (Vector2Int Direction in GridHelper.AllDirections)
			{
				Vector3 RaycastPosition = RoomData.transform.position + Vector3.up * 0.3f;
				Vector3 RaycastDirection = GridHelper.GridToWorldPoint(Direction);
				bool RaycastResult = gameObject.scene.GetPhysicsScene().Raycast(RaycastPosition, RaycastDirection, out RaycastHit HitInfo, 1f);
				if (RaycastResult)
				{
					Doorway HitDoorway = HitInfo.collider.GetComponentInParent<Doorway>();
					if (!HitDoorway)
					{
						RoomData.RoomDirectionTypes[GridHelper.DirectionToIndex(Direction)] = RoomDirectionTypeEnum.AlwaysBlocked;
						continue;
					}
					RoomData.AdjacentDoorways[GridHelper.DirectionToIndex(Direction)] = HitDoorway;
					HitDoorway.ConnectorSceneObjects?.ForEach((x) => x.SetActive(true));
					HitDoorway.BlockerSceneObjects?.ForEach((x) => x.SetActive(false));
					bool ConnectorRaycastResult = gameObject.scene.GetPhysicsScene().Raycast(RaycastPosition, RaycastDirection, 1f);
					if (ConnectorRaycastResult)
					{
						RoomData.RoomDirectionTypes[GridHelper.DirectionToIndex(Direction)] = RoomDirectionTypeEnum.AlwaysBlocked;
					}
					else
					{
						RoomData.RoomDirectionTypes[GridHelper.DirectionToIndex(Direction)] = RoomDirectionTypeEnum.OpenWhenConnected;
					}
					HitDoorway.ConnectorSceneObjects?.ForEach((x) => x.SetActive(true));
					HitDoorway.BlockerSceneObjects?.ForEach((x) => x.SetActive(true));
				}
				else
				{
					RoomData.RoomDirectionTypes[GridHelper.DirectionToIndex(Direction)] = RoomDirectionTypeEnum.AlwaysOpen;
				}
			}
		}
	}


	//public GameObject Cave1Prefab, Cave2Prefab;
	//[Button("cave애셋 변경", ButtonHeight = 80)]
	//void sss()
	//{
	//	Transform[] AllChilds = GetComponentsInChildren<Transform>();
	//	foreach (Transform target in AllChilds)
	//	{
	//		if (!target) continue;
	//		if (target.name.StartsWith("Cave_Wall1"))
	//		{
	//			GameObject NewOne = UnityEditor.PrefabUtility.InstantiatePrefab(Cave1Prefab) as GameObject;
	//			NewOne.transform.parent = target.parent;
	//			NewOne.transform.SetPositionAndRotation(target.position, target.rotation);
	//			NewOne.transform.localScale = target.localScale;
	//			DestroyImmediate(target.gameObject);
	//		}
	//		else if (target.name.StartsWith("Cave_Wall2"))
	//		{
	//			GameObject NewOne = UnityEditor.PrefabUtility.InstantiatePrefab(Cave2Prefab) as GameObject;
	//			NewOne.transform.parent = target.parent;
	//			NewOne.transform.SetPositionAndRotation(target.position, target.rotation);
	//			NewOne.transform.localScale = target.localScale;
	//			DestroyImmediate(target.gameObject);
	//		}
	//	}
	//	Assign();
	//}
}
