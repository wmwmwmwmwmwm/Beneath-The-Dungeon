using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableUI : SerializedMonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public Action<GameObject, PointerEventData> ClickCallback;
	public Action<GameObject, PointerEventData> BeginDragCallback;
	public Action<GameObject, PointerEventData> DragCallback;
	public Action<GameObject, GameObject, PointerEventData> EndDragCallback;

	float DragStartTime;
	GameObject DraggingObject;

	public void OnPointerClick(PointerEventData eventData)
	{
		ClickCallback?.Invoke(eventData.pointerCurrentRaycast.gameObject, eventData);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		DragStartTime = Time.time;
		DraggingObject = eventData.pointerCurrentRaycast.gameObject;
		DraggingObject.GetComponent<Graphic>().raycastTarget = false;
		BeginDragCallback?.Invoke(DraggingObject, eventData);
	}

	public void OnDrag(PointerEventData eventData)
	{
		DragCallback?.Invoke(DraggingObject, eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		DraggingObject.GetComponent<Graphic>().raycastTarget = true;
		DraggingObject = null;
		GameObject DropPlaceObject = eventData.pointerEnter;
		EndDragCallback?.Invoke(DraggingObject, DropPlaceObject, eventData);
	}


	void Update()
	{
		if (DraggingObject && Time.time - DragStartTime > 1f && Input.touchCount == 0)
		{
			OnEndDrag(new PointerEventData(null));
		}
	}
}
