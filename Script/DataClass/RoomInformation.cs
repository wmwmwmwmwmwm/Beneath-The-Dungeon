using DunGen;
using Sirenix.OdinInspector;
using UnityEngine;

public class RoomInformation : MonoBehaviour
{
	[ReadOnly] public Vector2Int WorldGridPoint;
	[ReadOnly] public bool[] WorldWalls;
	[ReadOnly] public GameObject EventObject;
	[ReadOnly] public bool IsRevealed;
	[ReadOnly] public GameObject MinimapTilePopup, MinimapTileSmall;

	[ReadOnly] public Doorway[] AdjacentDoorways;
	public enum RoomDirectionTypeEnum { AlwaysBlocked, AlwaysOpen, OpenWhenConnected }
	[ReadOnly] public RoomDirectionTypeEnum[] RoomDirectionTypes;

	public bool WallUp => WorldWalls[GridHelper.DirectionToIndex(Vector2Int.up)];
	public bool WallDown => WorldWalls[GridHelper.DirectionToIndex(Vector2Int.down)];
	public bool WallLeft => WorldWalls[GridHelper.DirectionToIndex(Vector2Int.left)];
	public bool WallRight => WorldWalls[GridHelper.DirectionToIndex(Vector2Int.right)];

	void Awake()
	{
		WorldWalls = new bool[4];
	}

	public int GetWallCount()
	{
		int WallCount = 0;
		foreach (bool wall in WorldWalls)
		{
			if (wall) WallCount++;
		}
		return WallCount;
	}

	public Doorway GetDungenDoor(Vector2Int LocalDirection)
	{
		return AdjacentDoorways[GridHelper.DirectionToIndex(LocalDirection)];
	}
}
