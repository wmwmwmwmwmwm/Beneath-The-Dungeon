using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static SingletonLoader;

public partial class UICanvas : Singleton<UICanvas>
{
	public Text AreaIndicatorText;
	public StatGaugeGroup GaugeGroup;
	public Button SkillPanelButton, EquipmentPanelButton;
	public Text TurnText;

	Vector2 DragStartPosition;

	void Start()
	{
		MinimapPopup.SetActive(false);
		SkillPanelButton.onClick.AddListener(() => CommonUI.Instance.OnSkillButton());
		EquipmentPanelButton.onClick.AddListener(() => CommonUI.Instance.OnEquipmentButton());
	}

	public void OnStraightButtonPressed()
	{
		CommonUI.Instance.CloseAllAlert();
		Player.Instance.Move(Vector2Int.up);
	}

	public void OnTurnLeftPressed()
	{
		Player.Instance.Move(Vector2Int.left);
	}

	public void OnTurnRightPressed()
	{
		Player.Instance.Move(Vector2Int.right);
	}

	public void OnDragButtonClick(GameObject DragObject, PointerEventData eventData)
	{
		CommonUI.Instance.CloseAllAlert();
		Player.Instance.Move(Vector2Int.up);
	}

	public void OnDragButtonBeginDrag(GameObject DragObject, PointerEventData eventData)
	{
		DragStartPosition = eventData.position;
	}

	public void OnDragButtonEndDrag(GameObject DragObject, GameObject DropPlace, PointerEventData eventData)
	{
		Vector2 DragVector = eventData.position - DragStartPosition;
		Vector2 DragDirection = DragVector.normalized;
		float DragDistance = DragVector.magnitude;
		if(DragDistance > 30f && Mathf.Abs(DragDirection.x) > Mathf.Abs(DragDirection.y))
		{
			if (DragDirection.x > 0f) Player.Instance.Move(Vector2Int.right);
			else Player.Instance.Move(Vector2Int.left);
		}
	}

	public void OnWaitButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		CommonStatus Status = Player.Instance.Status;
		bool HPNotMax = Status.CurrentHP < Status.MaxHP;
		bool MPNotMax = Status.CurrentMP < Status.MaxMP;
		bool SPNotMax = Status.CurrentSP < Status.MaxSP;
		do
		{
			DungeonController.Instance.OnPlayerTurnComplete(false);
			bool HPMaxNow = Status.CurrentHP >= Status.MaxHP;
			bool MPMaxNow = Status.CurrentMP >= Status.MaxMP;
			bool SPMaxNow = Status.CurrentSP >= Status.MaxSP;
			if (HPNotMax && HPMaxNow)
			{
				StartCoroutine(CommonUI.Instance.ShowMessageAlertText("체력이 모두 회복되었습니다", Color.clear));
				break;
			}
			else if (MPNotMax && MPMaxNow)
			{
				StartCoroutine(CommonUI.Instance.ShowMessageAlertText("마력이 모두 회복되었습니다", Color.clear));
				break;
			}
			else if (SPNotMax && SPMaxNow)
			{
				StartCoroutine(CommonUI.Instance.ShowMessageAlertText("기력이 모두 회복되었습니다", Color.clear));
				break;
			}
		} while (HPNotMax || MPNotMax || SPNotMax);
	}

	public GameObject TempleRuneIcon, TunnelRuneIcon;
	public void UpdateRuneProgress()
	{
		TempleRuneIcon.GetComponent<Image>().color = p.TempleRuneObtained ? Color.white : Color.black;
		TunnelRuneIcon.GetComponent<Image>().color = p.TunnelRuneObtained ? Color.white : Color.black;
	}
}
