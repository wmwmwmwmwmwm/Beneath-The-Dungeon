using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FixedMapData : SingleInstance<FixedMapData>
{
	public enum FixedMapTypeEnum { Lobby };
	public FixedMapTypeEnum FixedMapType;
	public List<GameObject> EncounterEvents;
	public Transform StartingPoint;
	//[TableMatrix(DrawElementMethod = "DrawElement", SquareCells = true, ResizableColumns = false)]
//	public FixedMapWallData[,] FixedMapWallDatas;
//	[System.Serializable]
//	public struct FixedMapWallData
//	{
//		public bool WallUp, WallDown, WallLeft, WallRight;
//	}

//#if UNITY_EDITOR
//	static FixedMapWallData DrawElement(Rect rect, FixedMapWallData element)
//	{
//		Rect WallUpRect = new Rect(rect.x + 20f, rect.y, 20f, 20f);
//		element.WallUp = EditorGUI.Toggle(WallUpRect, element.WallUp);
//		Rect WallDownRect = new Rect(rect.x + 20f, rect.y + 40f, 20f, 20f);
//		element.WallDown = EditorGUI.Toggle(WallDownRect, element.WallDown);
//		Rect WallLeftRect = new Rect(rect.x, rect.y + 20f, 20f, 20f);
//		element.WallLeft = EditorGUI.Toggle(WallLeftRect, element.WallLeft);
//		Rect WallRightRect = new Rect(rect.x + 40f, rect.y + 20f, 20f, 20f);
//		element.WallRight = EditorGUI.Toggle(WallRightRect, element.WallRight);
//		return element;
//	}
//#endif
}
