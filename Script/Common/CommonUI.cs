using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using static EquipmentElement;
using static SingletonLoader;
using DuloGames.UI;

public class CommonUI : Singleton<CommonUI>
{
	public Image BlackImage;
	public Image WhiteForeground;

	void Start()
	{
#if BTD_TEST
		CheatPanel.SetActive(true);
#else
		Destroy(CheatPanel);
#endif
		BlackImage.gameObject.SetActive(false);
		AlertDialog.SetActive(false);
		AlertDialogSelectBox.SetActive(false);
		AlertDialogInitialPosition = AlertDialog.transform.localPosition;
		CenterAlertDialog.SetActive(false);
		TextBoxObject.SetActive(false);
		SkillInfoPanel.SetActive(false);
		SkillPanel.SetActive(false);
		MessageAlert.gameObject.SetActive(false);
		LevelUpPanel.gameObject.SetActive(false);
		WhiteForeground.gameObject.SetActive(false);
		ObtainItemAlertPanel.SetActive(false);
		EquipmentPanel.SetActive(false);
		EquipmentInfoPanel.SetActive(false);
		SpecialStatusInfoPanel.SetActive(false);
		GameResultPanel.SetActive(false);
		ScorePanel.SetActive(false);
		for (int i = 0; i < 10; i++)
		{
			SkillIcon OneSkillIcon = SkillPanelIcons[i];
			OneSkillIcon.PlayerSkillGroup = Player.Instance.Status.Skills;
			OneSkillIcon.PlayerSkillIndex = i;
		}
		ObtainedSkillIcon.PlayerSkillIndex = -1;
	}

	public IEnumerator FadeIn(float Duration)
	{
		BlackImage.gameObject.SetActive(true);
		BlackImage.color = BlackImage.color.WithAlpha(1f);
		yield return BlackImage.DOFade(0f, Duration).WaitForCompletion();
		BlackImage.gameObject.SetActive(false);
	}

	public IEnumerator FadeOut(float Duration)
	{
		BlackImage.gameObject.SetActive(true);
		BlackImage.color = BlackImage.color.WithAlpha(0f);
		yield return BlackImage.DOFade(1f, Duration).WaitForCompletion();
	}

	public GameObject AlertDialog, AlertDialogSelectBox;
	public Text AlertDialogText;
	public GameObject AlertDialogProceedButton;
	[ReadOnly] public bool AlertDialogTrigger;
	[ReadOnly] public bool AlertDialogResult;
	Vector3 AlertDialogInitialPosition;
	IEnumerator AlertDialogCoroutineInstance;
	public IEnumerator ShowAlertDialog(string Message, bool ShowSelectBox)
	{
		if (AlertDialogCoroutineInstance != null)
			StopCoroutine(AlertDialogCoroutineInstance);
		AlertDialogCoroutineInstance = ShowAlertDialogInternal(Message, ShowSelectBox);
		yield return StartCoroutine(AlertDialogCoroutineInstance);

		IEnumerator ShowAlertDialogInternal(string Message, bool ShowSelectBox)
		{
			AlertDialog.SetActive(true);
			AlertDialogText.text = Message;
			CanvasGroup CanvasGroupComponent = AlertDialog.GetComponent<CanvasGroup>();
			CanvasGroupComponent.alpha = 0f;
			CanvasGroupComponent.DOFade(1f, 0.7f);
			Vector3 AlertDialogDownPosition = AlertDialog.transform.localPosition.WithY(AlertDialog.transform.localPosition.y - 100f);
			AlertDialog.transform.localPosition = AlertDialogDownPosition;
			AlertDialog.transform.DOLocalMoveY(AlertDialogInitialPosition.y, 0.7f);
			AlertDialogSelectBox.SetActive(ShowSelectBox);
			AlertDialogProceedButton.SetActive(!ShowSelectBox);
			AlertDialogTrigger = false;
			yield return new WaitUntil(() => AlertDialogTrigger);
			AlertDialogTrigger = false;
			AlertDialogSelectBox.SetActive(false);
			AlertDialog.SetActive(false);
			AlertDialogCoroutineInstance = null;
		}
	}

	public void OnAlertDialogButtonClick(bool Yes)
	{
		am.PlaySfx(Yes ? AudioManager.SfxTypeEnum.ClickIn : AudioManager.SfxTypeEnum.ClickOut);
		AlertDialogTrigger = true;
		AlertDialogResult = Yes;
	}

	public GameObject CenterAlertDialog;
	public Text CenterAlertDialogText;
	public Button CenterAlertDialogYesButton, CenterAlertDialogNoButton;
	[ReadOnly] public bool CenterAlertDialogTrigger;
	[ReadOnly] public bool CenterAlertDialogResult;
	IEnumerator CenterAlertDialogCoroutineInstance;
	public IEnumerator ShowCenterAlertDialog(string Message)
	{
		if (CenterAlertDialogCoroutineInstance != null)
			StopCoroutine(CenterAlertDialogCoroutineInstance);
		CenterAlertDialogCoroutineInstance = ShowCenterAlertDialogInternal(Message);
		yield return StartCoroutine(CenterAlertDialogCoroutineInstance);

		IEnumerator ShowCenterAlertDialogInternal(string Message)
		{
			CenterAlertDialog.SetActive(true);
			CenterAlertDialogText.text = Message;
			CenterAlertDialogTrigger = false;
			yield return new WaitUntil(() => CenterAlertDialogTrigger);
			CenterAlertDialogTrigger = false;
			CenterAlertDialog.SetActive(false);
			CenterAlertDialogCoroutineInstance = null;
		}
	}

	public void OnCenterAlertDialogButtonClick(bool Yes)
	{
		CenterAlertDialogTrigger = true;
		CenterAlertDialogResult = Yes;
	}

	public void CloseAllAlert()
	{
		AlertDialogTrigger = true;
		AlertDialogResult = false;
		AlertDialog.SetActive(false);
		CenterAlertDialogTrigger = true;
		CenterAlertDialogResult = false;
		CenterAlertDialog.SetActive(false);
	}

	public GameObject TextBoxObject;
	public Text TextBoxTellerNameText;
	public Text TextBoxMessageText;
	public IEnumerator ShowDialogTexts(string TellerName, List<string> DialogTexts)
	{
		TextBoxTellerNameText.text = TellerName;
		TextBoxObject.SetActive(true);
		foreach (string dialogData in DialogTexts)
		{
			yield return StartCoroutine(TextAnimation(dialogData));
			yield return new WaitUntil(() => NextDialogDemand);
		}
		TextBoxObject.SetActive(false);
	}

	[ReadOnly] public bool NextDialogDemand;
	public void NextDialog()
	{
		NextDialogDemand = true;
	}

	public float TextAnimationInterval;
	IEnumerator TextAnimation(string MessageText)
	{
		for (int i = 0; i < MessageText.Length; i++)
		{
			TextBoxMessageText.text = MessageText.Substring(0, i);
			yield return new WaitForSeconds(TextAnimationInterval);
			if (NextDialogDemand)
			{
				NextDialogDemand = false;
				TextBoxMessageText.text = MessageText;
				break;
			}
		}
	}

	public GameObject SkillInfoPanel;
	public Text SkillInfoName, SkillInfoDescription;
	public SkillIcon SkillInfoIcon;
	public Image SkillInfoElementTypeImage;
	public Text SkillInfoHPCostText, SkillInfoMPCostText, SkillInfoSPCostText;
	public void ShowSkillInfo(SkillData Skill)
	{
		SkillInfoPanel.SetActive(true);
		SkillInfoName.text = Skill.KoreanName;
		SkillInfoName.color = ColorPalette.Instance.GetRarityTextColor(Skill.Rarity);
		SkillInfoDescription.text = Skill.KoreanDescription;
		SkillInfoHPCostText.gameObject.SetActive(Skill.HPCost > 0 || Skill.HPCostPercent > 0f);
		SkillInfoMPCostText.gameObject.SetActive(Skill.MPCost > 0 || Skill.MPCostPercent > 0f);
		SkillInfoSPCostText.gameObject.SetActive(Skill.SPCost > 0 || Skill.SPCostPercent > 0f);
		if (Skill.HPCost > 0) SkillInfoHPCostText.text = string.Format("HP     {0}", Skill.HPCost);
		if (Skill.MPCost > 0) SkillInfoMPCostText.text = string.Format("MP     {0}", Skill.MPCost);
		if (Skill.SPCost > 0) SkillInfoSPCostText.text = string.Format("SP     {0}", Skill.SPCost);
		if (Skill.HPCostPercent > 0f) SkillInfoHPCostText.text = string.Format("HP     {0}", Skill.HPCostPercent.ToPercentString());
		if (Skill.SPCostPercent > 0f) SkillInfoMPCostText.text = string.Format("MP     {0}", Skill.MPCostPercent.ToPercentString());
		if (Skill.SPCostPercent > 0f) SkillInfoSPCostText.text = string.Format("SP     {0}", Skill.SPCostPercent.ToPercentString());
		SkillInfoIcon.SetSkillIconData(Skill);
		ElementalHelper.SetElementalIconImage(SkillInfoElementTypeImage, Skill.ElementalType);
	}

	public void HideSkillInfo()
	{
		SkillInfoPanel.SetActive(false);
	}

	public GameObject SkillPanel;
	public List<SkillIcon> SkillPanelIcons;
	public SkillIcon ObtainedSkillIcon;
	public GameObject SkillPanelSeparator;
	public GameObject SkillPanelForgetButton, SkillPanelInfoButton;
	public RectTransform SkillPanelWindow;
	[ReadOnly] public SkillIcon SelectedSkillIcon;
	float SkillPanelHeightSmall = 600f, SkillPanelHeightLarge = 900f;
	public void OnSkillButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickIn);
		SkillPanel.SetActive(true);
		SkillPanelWindow.sizeDelta = SkillPanelWindow.sizeDelta.WithY(SkillPanelHeightSmall);
		ObtainedSkillIcon.gameObject.SetActive(false);
		SkillPanelSeparator.SetActive(false);
		SkillPanelRefresh();
	}

	public IEnumerator SkillObtainProcess(SkillData ObtainedSkill)
	{
		yield return StartCoroutine(ItemAlertCoroutine(ObtainedSkill));
		SkillPanel.SetActive(true);
		SkillPanelWindow.sizeDelta = SkillPanelWindow.sizeDelta.WithY(SkillPanelHeightLarge);
		ObtainedSkillIcon.gameObject.SetActive(true);
		SkillPanelSeparator.SetActive(true);
		ObtainedSkillIcon.SetSkillIconData(ObtainedSkill);
		SkillPanelRefresh();
		ShowSkillInfo(ObtainedSkill);
	}

	public GameObject ObtainItemAlertPanel;
	public Animator ObtainItemAlertAnimator;
	public Text ObtainItemAlertNameText;
	public SkillIcon ObtainItemAlertSkillIcon;
	public EquipmentSlot ObtainItemAlertEquipmentSlot;
	IEnumerator ItemAlertCoroutine(SkillData ObtainedSkill) { yield return StartCoroutine(ItemAlertCoroutineInternal(false, ObtainedSkill, null)); }
	IEnumerator ItemAlertCoroutine(EquipmentData ObtainedEquipment)	{ yield return StartCoroutine(ItemAlertCoroutineInternal(true, null, ObtainedEquipment)); }
	IEnumerator ItemAlertCoroutineInternal(bool IsEquipment, SkillData ObtainedSkill, EquipmentData ObtainedEquipment)
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.Treasure);
		if (IsEquipment)
		{
			ObtainItemAlertSkillIcon.gameObject.SetActive(false);
			ObtainItemAlertEquipmentSlot.gameObject.SetActive(true);
			ObtainItemAlertNameText.text = ObtainedEquipment.KoreanName;
			ObtainItemAlertNameText.color = ColorPalette.Instance.GetRarityTextColor(ObtainedEquipment.Rarity);
			ObtainItemAlertEquipmentSlot.SetEquipmentData(ObtainedEquipment);
		}
		else
		{
			ObtainItemAlertSkillIcon.gameObject.SetActive(true);
			ObtainItemAlertEquipmentSlot.gameObject.SetActive(false);
			ObtainItemAlertNameText.text = ObtainedSkill.KoreanName;
			ObtainItemAlertNameText.color = ColorPalette.Instance.GetRarityTextColor(ObtainedSkill.Rarity);
			ObtainItemAlertSkillIcon.SetSkillIconData(ObtainedSkill);
		}
		ObtainItemAlertPanel.gameObject.SetActive(true);
		ObtainItemAlertAnimator.Play("ItemAlertAnimation");
		yield return new WaitUntil(() => ObtainItemAlertAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
		ObtainItemAlertPanel.gameObject.SetActive(false);
	}

	void SkillPanelRefresh()
	{
		SelectedSkillIcon?.IconComponent.SelectionEffect.gameObject.SetActive(false);
		SelectedSkillIcon = null;
		SkillPanelForgetButton.SetActive(false);
		SkillPanelInfoButton.SetActive(false);
		List<SkillIcon>.Enumerator IconEnumerator = SkillPanelIcons.GetEnumerator();
		foreach (GameObject OneSkillObject in Player.Instance.Status.Skills)
		{
			SkillData OneSkill = OneSkillObject.GetComponent<SkillData>();
			IconEnumerator.MoveNext();
			IconEnumerator.Current.SetSkillIconData(OneSkill);
		}
	}

	public void OnSkillIconSelect(SkillIcon NewSelectedSkillIcon)
	{
		// 선택한 아이콘 없을 때 - 선택
		if (!SelectedSkillIcon)
		{
			SelectedSkillIcon?.IconComponent.SelectionEffect.gameObject.SetActive(false);
			SelectedSkillIcon = NewSelectedSkillIcon;
			SelectedSkillIcon.IconComponent.SelectionEffect.gameObject.SetActive(true);
		}
		// 아이콘 두 번 선택 시 - 선택 해제
		else if (SelectedSkillIcon == NewSelectedSkillIcon)
		{
			SelectedSkillIcon?.IconComponent.SelectionEffect.gameObject.SetActive(false);
			SelectedSkillIcon = null;
		}
		// 아이콘 선택 후 다른 아이콘 선택시 - 스왑
		else
		{
			if (SelectedSkillIcon.PlayerSkillIndex >= 0)
			{
				Player.Instance.Status.Skills[SelectedSkillIcon.PlayerSkillIndex] = NewSelectedSkillIcon.ThisSkillData.gameObject;
			}
			if (NewSelectedSkillIcon.PlayerSkillIndex >= 0)
			{
				Player.Instance.Status.Skills[NewSelectedSkillIcon.PlayerSkillIndex] = SelectedSkillIcon.ThisSkillData.gameObject;
			}
			SkillData temp = SelectedSkillIcon.ThisSkillData;
			SelectedSkillIcon.SetSkillIconData(NewSelectedSkillIcon.ThisSkillData);
			NewSelectedSkillIcon.SetSkillIconData(temp);
			SkillPanelRefresh();
		}
		SkillPanelForgetButton.SetActive(SelectedSkillIcon);
		SkillPanelInfoButton.SetActive(SelectedSkillIcon);
	}

	public void OnSkillPanelForgetButton()
	{
		if (SelectedSkillIcon == ObtainedSkillIcon) return;
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		StartCoroutine(ForgetCoroutine());
		IEnumerator ForgetCoroutine()
		{
			yield return StartCoroutine(ShowCenterAlertDialog(string.Format("\"{0}\"\n다음 스킬을 망각하시겠습니까?", SelectedSkillIcon.ThisSkillData.KoreanName)));
			if (CenterAlertDialogResult)
			{
				Player.Instance.Status.Skills[SelectedSkillIcon.PlayerSkillIndex] = Player.Instance.ThisRaceData.DefaultSkill.gameObject;
			}
			SkillPanelRefresh();
		}
	}

	public void OnSkillPanelInfoButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		ShowSkillInfo(SelectedSkillIcon.ThisSkillData);
		SkillPanelRefresh();
	}

	public void OnSkillPanelCloseButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		SkillPanel.SetActive(false);
	}

	public CanvasGroup MessageAlert;
	public Text MessageAlertText;
	public Color DefaultTextColor;
	public IEnumerator ShowMessageAlertText(string MessageText, Color color)
	{
		if (color == Color.clear)
		{
			color = DefaultTextColor;
		}
		MessageAlertText.color = color;
		MessageAlertText.text = MessageText;
		MessageAlert.gameObject.SetActive(true);
		MessageAlert.alpha = 0f;
		yield return MessageAlert.DOFade(1f, 0.2f).WaitForCompletion();
		yield return new WaitForSeconds(1.1f);
		MessageAlert.DOFade(0f, 0.8f).OnComplete(() => MessageAlert.gameObject.SetActive(false));
	}

	public CanvasGroup LevelUpPanel;
	public Text LevelUpHPBefore, LevelUpHPAfter, LevelUpMPBefore, LevelUpMPAfter, LevelUpSPBefore, LevelUpSPAfter;
	public Text LevelUpSTRBefore, LevelUpSTRAfter, LevelUpDEXBefore, LevelUpDEXAfter, LevelUpINTBefore, LevelUpINTAfter, LevelUpCONBefore, LevelUpCONAfter;
	public Color AfterTextGreenColor, AfterTextNormalColor;
	public IEnumerator LevelUpProcess()
	{
		Player.Instance.Status.Level++;
		DungeonController.Instance.OnLevelUp();
		am.PlaySfx(AudioManager.SfxTypeEnum.LevelUp);
		Player.Instance.LevelUpTrailEffect.SetActive(true);
		WhiteForeground.gameObject.SetActive(true);
		WhiteForeground.color = Color.white.WithAlpha(0f);
		yield return WhiteForeground.DOFade(0.3f, 0.3f).WaitForCompletion();
		yield return WhiteForeground.DOFade(0f, 1.7f).WaitForCompletion();
		LevelUpPanel.gameObject.SetActive(true);
		CommonStatus PlayerStatus = Player.Instance.Status;
		LevelUpHPBefore.text = PlayerStatus.MaxHP.ToString();
		LevelUpMPBefore.text = PlayerStatus.MaxMP.ToString();
		LevelUpSPBefore.text = PlayerStatus.MaxSP.ToString();
		LevelUpSTRBefore.text = PlayerStatus.STR.ToString();
		LevelUpDEXBefore.text = PlayerStatus.DEX.ToString();
		LevelUpINTBefore.text = PlayerStatus.INT.ToString();
		LevelUpCONBefore.text = PlayerStatus.CON.ToString();

		// 스텟 올리기
		bool HPIncreased = false; bool MPIncreased = false; bool SPIncreased = false; bool STRIncreased = false; bool DEXIncreased = false; bool INTIncreased = false; bool CONIncreased = false;
		RaceData PlayerRaceData = Player.Instance.ThisRaceData;
		for (int i = 0; i < PlayerRaceData.LevelUpStatIncrement; i++)
		{
			float RandomValue = Random.value;
			if (RandomValue >= PlayerRaceData.LevelUpSTRPossibilityMin && RandomValue < PlayerRaceData.LevelUpSTRPossibilityMax)
			{
				Player.Instance.LevelUpSTR++;
				HPIncreased = true;
				STRIncreased = true;
			}
			else if (RandomValue >= PlayerRaceData.LevelUpDEXPossibilityMin && RandomValue < PlayerRaceData.LevelUpDEXPossibilityMax)
			{
				Player.Instance.LevelUpDEX++;
				SPIncreased = true;
				DEXIncreased = true;
			}
			else if (RandomValue >= PlayerRaceData.LevelUpINTPossibilityMin && RandomValue < PlayerRaceData.LevelUpINTPossibilityMax)
			{
				Player.Instance.LevelUpINT++;
				MPIncreased = true;
				INTIncreased = true;
			}
			else
			{
				Player.Instance.LevelUpCON++;
				HPIncreased = true;
				CONIncreased = true;
			}
		}
		Player.Instance.RecalculatePlayerStatus();
		PlayerStatus.HealAll();

		UpdateAfterValue(LevelUpHPAfter, PlayerStatus.MaxHP, HPIncreased);
		UpdateAfterValue(LevelUpMPAfter, PlayerStatus.MaxMP, MPIncreased);
		UpdateAfterValue(LevelUpSPAfter, PlayerStatus.MaxSP, SPIncreased);
		UpdateAfterValue(LevelUpSTRAfter, PlayerStatus.STR, STRIncreased);
		UpdateAfterValue(LevelUpDEXAfter, PlayerStatus.DEX, DEXIncreased);
		UpdateAfterValue(LevelUpINTAfter, PlayerStatus.INT, INTIncreased);
		UpdateAfterValue(LevelUpCONAfter, PlayerStatus.CON, CONIncreased);
		void UpdateAfterValue(Text AfterText, int StatValue, bool Increased)
		{
			AfterText.text = StatValue.ToString();
			if (Increased) AfterText.color = AfterTextGreenColor;
			else AfterText.color = AfterTextNormalColor;
		}

		yield return new WaitUntil(() => LevelUpPanelOKButtonTrigger);
		LevelUpPanel.gameObject.SetActive(false);
		LevelUpPanelOKButtonTrigger = false;
	}

	bool LevelUpPanelOKButtonTrigger;
	public void LevelUpPanelOKButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		LevelUpPanelOKButtonTrigger = true;
	}

	public GameObject EquipmentPanel;
	public RectTransform EquipmentPanelWindow;
	public EquipmentSlot ObtainedEquipmentSlot;
	public GameObject EquipmentPanelDeleteButton, EquipmentPanelInfoButton;
	public EquipmentSlot HeadEquipmentSlot, NeckEquipmentSlot, BodyEquipmentSlot, WeaponEquipmentSlot, GloveEquipmentSlot, Ring1EquipmentSlot, Ring2EquipmentSlot, BootsEquipmentSlot;
	public GameObject EquipmentPanelSeparator;
	[ReadOnly] public EquipmentSlot SelectedEquipmentSlot;
	float EquipmentPanelWidthBig = 1242f, EquipmentPanelWidthSmall = 1030f;
	public void OnEquipmentButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickIn);
		EquipmentPanelWindow.sizeDelta = EquipmentPanelWindow.sizeDelta.WithX(EquipmentPanelWidthSmall);
		ObtainedEquipmentSlot.gameObject.SetActive(false);
		EquipmentPanelSeparator.SetActive(false);
		EquipmentPanel.SetActive(true);
		EquipmentPanelRefresh();
	}

	public IEnumerator EquipmentObtainProcess(GameObject ObtainedEquipment)
	{
		EquipmentData EquipmentComponent = ObtainedEquipment.GetComponent<EquipmentData>();
		yield return StartCoroutine(ItemAlertCoroutine(EquipmentComponent));
		EquipmentPanel.SetActive(true);
		EquipmentPanelWindow.sizeDelta = EquipmentPanelWindow.sizeDelta.WithX(EquipmentPanelWidthBig);
		ObtainedEquipmentSlot.gameObject.SetActive(true);
		ObtainedEquipmentSlot.EquipmentSlotType = EquipmentHelper.EquipmentTypeToSlotType(EquipmentComponent.EquipmentType);
		EquipmentPanelSeparator.SetActive(true);
		ObtainedEquipmentSlot.SetEquipmentData(EquipmentComponent);
		EquipmentPanelRefresh();
		ShowEquipmentInfoPanel(EquipmentComponent);
	}

	public void OnEquipmentSelect(EquipmentSlot NewSelectedEquipmentSlot)
	{
		// 선택한 아이콘 없을 때 - 선택
		if (!SelectedEquipmentSlot && NewSelectedEquipmentSlot.ThisEquipmentData)
		{
			SelectedEquipmentSlot?.IconComponent.SelectionEffect.gameObject.SetActive(false);
			SelectedEquipmentSlot = NewSelectedEquipmentSlot;
			SelectedEquipmentSlot.IconComponent.SelectionEffect.gameObject.SetActive(true);
			EquipmentPanelDeleteButton.SetActive(true);
			EquipmentPanelInfoButton.SetActive(true);
			return;
		}
		// 아이콘 두 번 선택 시 - 선택 해제
		if (SelectedEquipmentSlot == NewSelectedEquipmentSlot)
		{
			SelectedEquipmentSlot?.IconComponent.SelectionEffect.gameObject.SetActive(false);
			SelectedEquipmentSlot = null;
			EquipmentPanelDeleteButton.SetActive(false);
			EquipmentPanelInfoButton.SetActive(false);
			return;
		}
		// 아이콘 선택 후 다른 아이콘 선택시 - 스왑
		else if (SelectedEquipmentSlot)
		{
			if (!EquipmentHelper.IsEqualEquipmentType(SelectedEquipmentSlot.EquipmentSlotType, NewSelectedEquipmentSlot.EquipmentSlotType)) return;
			if (SelectedEquipmentSlot != ObtainedEquipmentSlot) Player.Instance.SetEquipment(SelectedEquipmentSlot.EquipmentSlotType, NewSelectedEquipmentSlot.ThisEquipmentData?.gameObject);
			if (NewSelectedEquipmentSlot != ObtainedEquipmentSlot) Player.Instance.SetEquipment(NewSelectedEquipmentSlot.EquipmentSlotType, SelectedEquipmentSlot.ThisEquipmentData?.gameObject);
			EquipmentData temp = SelectedEquipmentSlot.ThisEquipmentData;
			SelectedEquipmentSlot.SetEquipmentData(NewSelectedEquipmentSlot.ThisEquipmentData);
			NewSelectedEquipmentSlot.SetEquipmentData(temp);
			EquipmentPanelRefresh();
		}
	}

	void EquipmentPanelRefresh()
	{
		SelectedEquipmentSlot?.IconComponent.SelectionEffect.gameObject.SetActive(false);
		SelectedEquipmentSlot = null;
		EquipmentPanelDeleteButton.SetActive(false);
		EquipmentPanelInfoButton.SetActive(false);
		HeadEquipmentSlot.SetEquipmentData(Player.Instance.Equipments[EquipmentSlotTypeEnum.Head]?.GetComponent<EquipmentData>());
		NeckEquipmentSlot.SetEquipmentData(Player.Instance.Equipments[EquipmentSlotTypeEnum.Neck]?.GetComponent<EquipmentData>());
		BodyEquipmentSlot.SetEquipmentData(Player.Instance.Equipments[EquipmentSlotTypeEnum.Body]?.GetComponent<EquipmentData>());
		WeaponEquipmentSlot.SetEquipmentData(Player.Instance.Equipments[EquipmentSlotTypeEnum.Weapon]?.GetComponent<EquipmentData>());
		GloveEquipmentSlot.SetEquipmentData(Player.Instance.Equipments[EquipmentSlotTypeEnum.Glove]?.GetComponent<EquipmentData>());
		Ring1EquipmentSlot.SetEquipmentData(Player.Instance.Equipments[EquipmentSlotTypeEnum.Ring1]?.GetComponent<EquipmentData>());
		Ring2EquipmentSlot.SetEquipmentData(Player.Instance.Equipments[EquipmentSlotTypeEnum.Ring2]?.GetComponent<EquipmentData>());
		BootsEquipmentSlot.SetEquipmentData(Player.Instance.Equipments[EquipmentSlotTypeEnum.Boots]?.GetComponent<EquipmentData>());
	}

	public void OnEquipmentPanelDeleteButton()
	{
		if (SelectedEquipmentSlot == ObtainedEquipmentSlot) return;
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		StartCoroutine(DeleteCoroutine());
		IEnumerator DeleteCoroutine()
		{
			yield return StartCoroutine(ShowCenterAlertDialog(string.Format("\"{0}\"\n다음 장비를 버립니까?", SelectedEquipmentSlot.ThisEquipmentData.KoreanName)));
			if (CenterAlertDialogResult)
			{
				Player.Instance.SetEquipment(SelectedEquipmentSlot.EquipmentSlotType, null);
			}
			EquipmentPanelRefresh();
		}
	}

	public void OnEquipmentPanelInfoButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		ShowEquipmentInfoPanel(SelectedEquipmentSlot.ThisEquipmentData);
		EquipmentPanelRefresh();
	}

	public void OnEquipmentPanelCloseButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		EquipmentPanel.SetActive(false);
	}

	public GameObject EquipmentInfoPanel;
	public EquipmentSlot EquipmentInfoPanelSlot;
	public Text EquipmentInfoPanelNameText, EquipmentInfoPanelDescriptionText;
	public void ShowEquipmentInfoPanel(EquipmentData Equipment)
	{
		EquipmentInfoPanel.SetActive(true);
		EquipmentInfoPanelSlot.SetEquipmentData(Equipment);
		EquipmentInfoPanelNameText.text = Equipment.KoreanName;
		float HorizontalPreferredSize = EquipmentInfoPanelNameText.GetComponent<RectTransform>().sizeDelta.x;
		EquipmentInfoPanelNameText.transform.localScale = Vector3.one * Mathf.Min(1f, 500f / HorizontalPreferredSize);
		string DescriptionText = "";
		int HPAmount = Equipment.HP;
		if (HPAmount != 0) DescriptionText += string.Format("HP {0}\n", HPAmount.GetSignedText());
		int MPAmount = Equipment.MP;
		if (MPAmount != 0) DescriptionText += string.Format("MP {0}\n", MPAmount.GetSignedText());
		int SPAmount = Equipment.SP;
		if (SPAmount != 0) DescriptionText += string.Format("SP {0}\n", SPAmount.GetSignedText());
		int ArmorAmount = Equipment.Armor;
		if (ArmorAmount != 0) DescriptionText += string.Format("방어력 {0}\n", ArmorAmount.GetSignedText());
		int STRAmount = Equipment.STR;
		if (STRAmount != 0) DescriptionText += string.Format("STR {0}\n", STRAmount.GetSignedText());
		int DEXAmount = Equipment.DEX;
		if (DEXAmount != 0) DescriptionText += string.Format("DEX {0}\n", DEXAmount.GetSignedText());
		int INTAmount = Equipment.INT;
		if (INTAmount != 0) DescriptionText += string.Format("INT {0}\n", INTAmount.GetSignedText());
		int CONAmount = Equipment.CON;
		if (CONAmount != 0) DescriptionText += string.Format("CON {0}\n", CONAmount.GetSignedText());
		int DamageAmount = Equipment.Damage;
		if (DamageAmount != 0) DescriptionText += string.Format("피해량 {0}\n", DamageAmount.GetSignedText());
		if (Equipment.ElementalType != ElementalTypeEnum.None) 
		{
			string ElementText = "";
			switch (Equipment.ElementalType)
			{
				case ElementalTypeEnum.Fire: ElementText = "화 속성 부여"; break;
				case ElementalTypeEnum.Water: ElementText = "수 속성 부여"; break;
				case ElementalTypeEnum.Light: ElementText = "명 속성 부여"; break;
				case ElementalTypeEnum.Dark: ElementText = "암 속성 부여"; break;
				case ElementalTypeEnum.Hellfire: ElementText = "지옥불 속성 부여"; break;
				case ElementalTypeEnum.Poison: ElementText = "독 속성 부여"; break;
				case ElementalTypeEnum.Plasma: ElementText = "플라즈마 속성 부여"; break;
				case ElementalTypeEnum.Abyssal: ElementText = "심연 속성 부여"; break;
				case ElementalTypeEnum.Chaos: ElementText = "혼돈 속성 부여"; break;
				case ElementalTypeEnum.Law: ElementText = "질서 속성 부여"; break;
			}
			DescriptionText += string.Format("{0}\n", ElementText);
		}
		foreach (EquipmentElement OneElement in Equipment.ElementComponents)
		{
			if (OneElement.EquipmentElementType == EquipmentElementTypeEnum.Special && !string.IsNullOrWhiteSpace(OneElement.SpecialEffectDescription)) 
				DescriptionText += string.Format("{0}\n", OneElement.SpecialEffectDescription);
		}
		EquipmentInfoPanelDescriptionText.text = DescriptionText;
	}

	public void HideEquipmentInfoPanel()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		EquipmentInfoPanel.SetActive(false);
	}

	public GameObject SpecialStatusInfoPanel;
	public SkillIcon SpecialStatusInfoSkillIcon;
	public Text SpecialStatusInfoName, SpecialStatusInfoDescription;
	public void ShowSpecialStatusInfoPanel(SpecialStatusData SpecialStatus)
	{
		SpecialStatusInfoPanel.SetActive(true);
		SpecialStatusInfoSkillIcon.SetSpecialStatusData(SpecialStatus);
		SpecialStatusInfoName.text = SpecialStatus.KoreanName;
		SpecialStatusInfoDescription.text = SpecialStatus.GetDescription();
	}

	public void HideSpecialStatusInfoPanel()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		SpecialStatusInfoPanel.SetActive(false);
	}

	public GameObject GameResultPanel;
	public CanvasGroup GameResultPanelFloorCount, GameResultPanelMonsterCount, GameResultPanelRuneObtained;
	public Image GameResultPanelTempleRune, GameResultPanelTunnelRune;
	public Text GameResultPanelScoreText;
	public IEnumerator GameEndProcess()
	{
		sv.DeleteFile();
		int FloorCount = dc.FloorSaveDatas.Count;
		int MonsterCount = p.HuntMonsterCount;
		GameResultPanelFloorCount.transform.Find("ValueText").GetComponent<Text>().text = FloorCount.ToString();
		GameResultPanelMonsterCount.transform.Find("ValueText").GetComponent<Text>().text = MonsterCount.ToString();
		GameResultPanelRuneObtained.transform.Find("TempleRune").GetComponent<Image>().color = p.TempleRuneObtained ? Color.white : Color.black;
		GameResultPanelRuneObtained.transform.Find("TunnelRune").GetComponent<Image>().color = p.TunnelRuneObtained ? Color.white : Color.black;
		GameResultPanelScoreText.text = "0";
		int GameScore = 0;
		GameScore += p.TempleRuneObtained ? 50000 : 0;
		GameScore += p.TunnelRuneObtained ? 50000 : 0;
		GameScore += MonsterCount * 200;
		GameScore += FloorCount * 500;
		GameScore -= dc.Turn * 20;
		GameScore = Mathf.Max(GameScore, 0);
		sv.SaveHighScore(new SaveManager.ScoreData()
		{
			Name = p.Name,
			Score = GameScore,
		});

		GameResultPanel.SetActive(true);
		GameResultPanelFloorCount.alpha = 0; GameResultPanelMonsterCount.alpha = 0; GameResultPanelRuneObtained.alpha = 0;
		yield return GameResultPanelFloorCount.DOFade(1f, 0.4f).WaitForCompletion();
		yield return new WaitForSeconds(0.5f);
		yield return GameResultPanelMonsterCount.DOFade(1f, 0.4f).WaitForCompletion();
		yield return new WaitForSeconds(0.5f);
		yield return GameResultPanelRuneObtained.DOFade(1f, 0.4f).WaitForCompletion();
		yield return new WaitForSeconds(0.5f);
		yield return GameResultPanelScoreText.DOCounter(0, GameScore, 2.5f).WaitForCompletion();
		yield return new WaitForSeconds(1f);
		GameResultPanelTrigger = false;
		yield return new WaitUntil(() => GameResultPanelTrigger);
		GameResultPanelTrigger = false;
		GameResultPanel.SetActive(false);
		yield return StartCoroutine(ShowScorePanel());
		UILoadingOverlayManager.Instance.Create().LoadScene(SRScenes._100StartMenu.name);
	}

	[ReadOnly] public bool GameResultPanelTrigger;
	public void GameResultPanelOKButton()
	{
		GameResultPanelTrigger = true;
	}

	public GameObject ScorePanel;
	public List<GameObject> ScorePanelItems;
	public IEnumerator ShowScorePanel()
	{
		sv.LoadHighScores();
		ScorePanel.SetActive(true);
		for (int i = 0; i < ScorePanelItems.Count; i++)
		{
			Text SubjectText = ScorePanelItems[i].transform.Find("SubjectText").GetComponent<Text>();
			Text ValueText = ScorePanelItems[i].transform.Find("ValueText").GetComponent<Text>();
			if (sv.HighScores.Count > i)
			{
				SubjectText.text = string.Format("{0}. {1}", i + 1, sv.HighScores[i].Name);
				ValueText.text = sv.HighScores[i].Score.ToString();
			}
			else
			{
				SubjectText.text = "";
				ValueText.text = "";
			}
		}
		ScorePanelTrigger = false;
		yield return new WaitUntil(() => ScorePanelTrigger);
		ScorePanelTrigger = false;
		ScorePanel.SetActive(false);
	}

	[ReadOnly] public bool ScorePanelTrigger;
	public void ScorePanelOKButton()
	{
		ScorePanelTrigger = true;
	}

	public GameObject CheatPanel;
	public void CheatPanelPowerUp()
	{
		p.LevelUpSTR = 50;
		p.LevelUpDEX = 50;
		p.LevelUpINT = 50;
		p.LevelUpCON = 50;
		p.RecalculatePlayerStatus();
		p.Status.HealAll();
	}
}
