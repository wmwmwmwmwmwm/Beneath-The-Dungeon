using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static SingletonLoader;

public partial class UICanvas
{
	public GameObject MinimapPopup;
	public void OnMinimapPopupOpenButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		MinimapPopup.SetActive(true);
		UpdateMinimapPosition();
		UpdateMinimapArrowRotation();
	}

	public void OnMinimapPopupCloseButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		MinimapPopup.SetActive(false);
	}

	public GameObject MinimapTilePrefab;
	public Sprite OneWayMinimapSprite, TwoWayCornerMinimapSprite, TwoWayStraightMinimapSprite, ThreeWayMinimapSprite, FourWayMinimapSprite;
	public Transform MinimapSmallAreaParent, MinimapPopupParent;
	public void CreateMinimapData()
	{
		MinimapSmallAreaParent.DestroyChildren();
		foreach (Transform Child in MinimapPopupParent)
		{
			if (Child.GetSiblingIndex() != MinimapPopupParent.childCount - 1) Destroy(Child.gameObject);
		}

		// 미니맵 데이터로 미니맵 오브젝트들 생성
		foreach (KeyValuePair<Vector2Int, RoomInformation> RoomInformationPair in DungeonController.Instance.Map)
		{
			Sprite MinimapSprite = null;
			RoomInformation Room = RoomInformationPair.Value;
			int WallCount = RoomInformationPair.Value.GetWallCount();
			// ************
			// 벽이 하나인 프리팹 : 벽이 오른쪽에 있다
			// 벽이 두개이고 직진인 프리팹 : 벽이 앞과 뒤에 있다
			// 벽이 두개이고 코너인 프리팹 : 벽이 앞과 오른쪽에 있다
			// 벽이 세개인 프리팹 : 벽이 앞과 뒤와 왼쪽에 있다
			// ************
			switch (WallCount)
			{
				case 0: MinimapSprite = FourWayMinimapSprite; break;
				case 1: MinimapSprite = ThreeWayMinimapSprite; break;
				case 2:
					if ((Room.WallUp && Room.WallDown) || (Room.WallLeft && Room.WallRight))
						MinimapSprite = TwoWayStraightMinimapSprite;
					else
						MinimapSprite = TwoWayCornerMinimapSprite;
					break;
				case 3: MinimapSprite = OneWayMinimapSprite; break;
			}
			Vector3 NewMinimapBlockPosition = new Vector3(Room.WorldGridPoint.x * 50f, Room.WorldGridPoint.y * 50f, 0f);
			Vector3 NewMinimapBlockRotation = Vector3.zero;
			switch (WallCount)
			{
				case 0:
					NewMinimapBlockRotation = new Vector3(0f, 0f, 90f) * Random.Range(0, 4);
					break;
				case 1:
					if (Room.WallRight) NewMinimapBlockRotation = Vector3.zero;
					else if (Room.WallDown) NewMinimapBlockRotation = new Vector3(0f, 0f, -90f);
					else if (Room.WallLeft) NewMinimapBlockRotation = new Vector3(0f, 0f, -180f);
					else NewMinimapBlockRotation = new Vector3(0f, 0f, -270f);
					break;
				case 2:
					if ((Room.WallUp && Room.WallDown) || (Room.WallLeft && Room.WallRight))
					{
						if (Room.WallUp) NewMinimapBlockRotation = Vector3.zero;
						else NewMinimapBlockRotation = new Vector3(0f, 0f, 90f);
					}
					else
					{
						if (Room.WallUp && Room.WallRight) NewMinimapBlockRotation = Vector3.zero;
						else if (Room.WallRight && Room.WallDown) NewMinimapBlockRotation = new Vector3(0f, 0f, -90f);
						else if (Room.WallDown && Room.WallLeft) NewMinimapBlockRotation = new Vector3(0f, 0f, -180f);
						else NewMinimapBlockRotation = new Vector3(0f, 0f, -270f);
					}
					break;
				case 3:
					if (Room.WallDown && Room.WallLeft && Room.WallUp) NewMinimapBlockRotation = Vector3.zero;
					else if (Room.WallLeft && Room.WallUp && Room.WallRight) NewMinimapBlockRotation = new Vector3(0f, 0f, -90f);
					else if (Room.WallUp && Room.WallRight && Room.WallDown) NewMinimapBlockRotation = new Vector3(0f, 0f, -180f);
					else NewMinimapBlockRotation = new Vector3(0f, 0f, -270f);
					break;
			}
			GameObject NewMinimapTilePopupObject = Instantiate(MinimapTilePrefab, MinimapPopupParent);
			NewMinimapTilePopupObject.transform.localPosition = NewMinimapBlockPosition;
			NewMinimapTilePopupObject.transform.eulerAngles = NewMinimapBlockRotation;
			NewMinimapTilePopupObject.GetComponent<Image>().sprite = MinimapSprite;
			NewMinimapTilePopupObject.SetActive(false);
			Room.MinimapTilePopup = NewMinimapTilePopupObject;
			GameObject NewMinimapTileSmallAreaObject = Instantiate(MinimapTilePrefab, MinimapSmallAreaParent);
			NewMinimapTileSmallAreaObject.transform.localPosition = NewMinimapBlockPosition;
			NewMinimapTileSmallAreaObject.transform.eulerAngles = NewMinimapBlockRotation;
			NewMinimapTileSmallAreaObject.GetComponent<Image>().sprite = MinimapSprite;
			NewMinimapTileSmallAreaObject.SetActive(false);
			Room.MinimapTileSmall = NewMinimapTileSmallAreaObject;
		}
		MinimapPopupArrow.transform.SetAsLastSibling();

		List<Vector2Int> AllTileGridPoints = DungeonController.Instance.Map.Keys.ToList();
		int MapMaxX = AllTileGridPoints.Max((grid) => grid.x);
		int MapMinX = AllTileGridPoints.Min((grid) => grid.x);
		int MapMaxY = AllTileGridPoints.Max((grid) => grid.y);
		int MapMinY = AllTileGridPoints.Min((grid) => grid.y);
		Vector2Int MapSize = new Vector2Int(MapMaxX - MapMinX, MapMaxY - MapMinY);
		MinimapPopupParent.transform.localScale = Vector3.one;
		if (MapSize.x > 26 || MapSize.y > 14)
		{
			float HorizontalExceedAmount = MapSize.x / 26f;
			float VertialExceedAmount = MapSize.y / 14f;
			if (HorizontalExceedAmount > VertialExceedAmount) MinimapPopupParent.transform.localScale = new Vector3(1f / HorizontalExceedAmount, 1f / HorizontalExceedAmount, 1f);
			else MinimapPopupParent.transform.localScale = new Vector3(1f / VertialExceedAmount, 1f / VertialExceedAmount, 1f);
		}
		float MapHorizontalCenter = (MapMinX + MapMaxX) * 0.5f;
		float MapVerticalCenter = (MapMinY + MapMaxY) * 0.5f;
		MinimapPopupParent.transform.localPosition = new Vector3(MapHorizontalCenter * -50f * MinimapPopupParent.localScale.x, MapVerticalCenter * -50f * MinimapPopupParent.localScale.y, 0f);
	}

	public void UpdateMinimapPosition()
	{
		Vector2Int PlayerPosition = Player.Instance.GridPosition;
		MinimapSmallAreaParent.localPosition = new Vector3(PlayerPosition.x * -50f, PlayerPosition.y * -50f, 0f);
		MinimapPopupArrow.transform.localPosition = new Vector3(PlayerPosition.x * 50f, PlayerPosition.y * 50f, 0f);
	}

	public GameObject MinimapSmallAreaArrow, MinimapPopupArrow;
	public void UpdateMinimapArrowRotation()
	{
		Vector2Int PlayerLookAt = Player.Instance.LookAt;
		Vector3 ArrowLookAt = new Vector3(0f, 0f, -(Mathf.Atan2(PlayerLookAt.x, PlayerLookAt.y) * Mathf.Rad2Deg) + 90f);
		MinimapSmallAreaArrow.transform.eulerAngles = ArrowLookAt;
		MinimapPopupArrow.transform.eulerAngles = ArrowLookAt;
	}
}
