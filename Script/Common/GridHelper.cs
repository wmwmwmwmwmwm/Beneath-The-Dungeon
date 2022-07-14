﻿using UnityEngine;

public static class GridHelper {

	public static Vector2Int[] AllDirections = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

	public static Vector3 GridToWorldPoint(Vector2Int Grid)
	{
		return new Vector3(Grid.x, 0f, Grid.y);
	}

	public static Vector2Int WorldToGridPoint(Vector3 WorldCoordinate)
	{
		return new Vector2Int(Mathf.RoundToInt(WorldCoordinate.x), Mathf.RoundToInt(WorldCoordinate.z));
	}

	public static Vector2Int WorldToGridDirection(Vector3 EulerAngles)
	{
		float RotationY = Util.Mod(EulerAngles.y, 360f);
		if (RotationY > 45f && RotationY <= 135f) return Vector2Int.right;
		else if (RotationY > 135f && RotationY <= 225f) return Vector2Int.down;
		else if (RotationY > 225f && RotationY <= 315f) return Vector2Int.left;
		else return Vector2Int.up;
	}

	public static Vector2Int GetRandomDirection()
	{
		float random = Random.value;
		if (random < 0.25f)
			return Vector2Int.up;
		else if (random < 0.5f)
			return Vector2Int.down;
		else if (random < 0.75f)
			return Vector2Int.left;
		else
			return Vector2Int.right;
	}

	public static Vector2Int[] GetLocalForwardLeftRight(Vector2Int ForwardDirection)
	{
		return new Vector2Int[] { ForwardDirection, TurnLeft(ForwardDirection), TurnRight(ForwardDirection) };
	}

	public static Vector2Int TurnRight(Vector2Int Direction)
	{
		if (Direction == Vector2Int.up)
			return Vector2Int.right;
		else if(Direction == Vector2Int.right)
			return Vector2Int.down;
		else if (Direction == Vector2Int.down)
			return Vector2Int.left;
		else 
			return Vector2Int.up;
	}

	public static Vector2Int TurnLeft(Vector2Int Direction)
	{
		if (Direction == Vector2Int.up)
			return Vector2Int.left;
		else if (Direction == Vector2Int.left)
			return Vector2Int.down;
		else if (Direction == Vector2Int.down)
			return Vector2Int.right;
		else
			return Vector2Int.up;
	}

	public static Vector2Int TurnBackward(Vector2Int Direction)
	{
		if (Direction == Vector2Int.up)
			return Vector2Int.down;
		else if (Direction == Vector2Int.down)
			return Vector2Int.up;
		else if (Direction == Vector2Int.left)
			return Vector2Int.right;
		else
			return Vector2Int.left;
	}

	public static int DirectionToIndex(Vector2Int Direction)
	{
		if (Direction == Vector2Int.up)
			return 0;
		else if (Direction == Vector2Int.right)
			return 1;
		else if (Direction == Vector2Int.down)
			return 2;
		else
			return 3;
	}

	public static Vector2Int LocalToWorldDirection(Vector2Int v, Transform target)
	{
		Vector2Int TargetForward = WorldToGridPoint(target.forward);
		if (TargetForward == Vector2Int.up)
			return v;
		else if (TargetForward == Vector2Int.right)
			return TurnRight(v);
		else if (TargetForward == Vector2Int.down)
			return TurnBackward(v);
		else
			return TurnLeft(v);
	}

	public static Vector2Int WorldToLocalDirection(Vector2Int v, Transform target)
	{
		Vector2Int TargetForward = WorldToGridPoint(target.forward);
		if (TargetForward == Vector2Int.up)
			return v;
		else if (TargetForward == Vector2Int.right)
			return TurnLeft(v);
		else if (TargetForward == Vector2Int.down)
			return TurnBackward(v);
		else
			return TurnRight(v);
	}
}
