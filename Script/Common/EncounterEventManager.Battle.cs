using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using static SingletonLoader;

public partial class EncounterEventManager
{
	float AnimationTime;

	public GameObject BattlePanel;
	public List<SkillIcon> PlayerSkillIcons, EnemySkillIcons;
	public GameObject BattleBeginAlert;
	public Graphic BattleBeginBackground, BattleBeginText;
	public Text BattleTurnText;
	public GameObject BattleButtonPanel;
	public Text BattleNextButtonText;
	[ReadOnly] public int BattleTurn;
	bool BattleEnded, PlayerVictory;
	public IEnumerator StartBattle(Monster EnemyMonster)
	{
		ui.gameObject.SetActive(false);
		StartCoroutine(SetBlackOverlayState(true));
		BlackOverlay.GetComponent<Image>().color = Color.black.WithAlpha(dc.CurrentDungeonData.BattleOverlayAlpha);
		RenderSettings.fog = false;
		BattleEnded = false; PlayerVictory = false; BattleDodgeButtonClicked = false; BattleDodgeElapsedTurn = 0;
		AutoProceedToggle.isOn = false;
		CurrentBattleEnemy = EnemyMonster;
		p.BattleStatus.SetValues(p.Status);
		CurrentBattleEnemy.BattleStatus.SetValues(CurrentBattleEnemy.Status);
		PlayerGaugeGroup.UpdateAllStatusImmediate(p.BattleStatus, true);
		EnemyGaugeGroup.UpdateAllStatusImmediate(CurrentBattleEnemy.BattleStatus, true);
		yield return StartCoroutine(SetMonsterSprite(CurrentBattleEnemy));
		List<GameObject> PlayerSkillPrefabs = p.Status.Skills;
		List<GameObject> PlayerSkills = p.BattleStatus.Skills = new List<GameObject>();
		List<SkillIcon>.Enumerator PlayerIconsEnumerator = PlayerSkillIcons.GetEnumerator();
		foreach (GameObject OneSkillObject in PlayerSkillPrefabs)
		{
			SkillData OneSkill = OneSkillObject.GetComponent<SkillData>();
			OneSkill.UserType = UserTypeEnum.Player;
			PlayerSkills.Add(Instantiate(OneSkill.gameObject));
			PlayerIconsEnumerator.MoveNext();
			PlayerIconsEnumerator.Current.SetSkillIconData(OneSkill);
		}
		List<GameObject> EnemySkillPrefabs = CurrentBattleEnemy.Status.Skills;
		List<GameObject> EnemySkills = CurrentBattleEnemy.BattleStatus.Skills = new List<GameObject>();
		List<SkillIcon>.Enumerator EnemyIconsEnumerator = EnemySkillIcons.GetEnumerator();
		foreach (GameObject OneSkillObject in EnemySkillPrefabs)
		{
			SkillData OneSkill = OneSkillObject.GetComponent<SkillData>();
			OneSkill.UserType = UserTypeEnum.Enemy;
			EnemySkills.Add(Instantiate(OneSkill.gameObject));
			EnemyIconsEnumerator.MoveNext();
			EnemyIconsEnumerator.Current.SetSkillIconData(OneSkill);
		}

		BattlePanel.SetActive(true);
		BattleTurnText.text = string.Format("Turn 1");
		BattleBeginAlert.SetActive(true);
		BattleBeginBackground.color = new Color(1f, 1f, 1f, 0f);
		BattleBeginBackground.DOFade(1f, AnimationTime);
		BattleBeginText.color = new Color(1f, 1f, 1f, 0f);
		yield return BattleBeginText.DOFade(1f, AnimationTime).WaitForCompletion();
		yield return new WaitForSeconds(AnimationTime * 3f);
		Vector3 TextInitialPosition = BattleBeginText.transform.localPosition;
		BattleBeginBackground.DOFade(0f, AnimationTime * 2f);
		BattleBeginText.DOFade(0f, AnimationTime * 2f);
		yield return BattleBeginText.transform.DOLocalMoveY(TextInitialPosition.y + 30f, AnimationTime * 2f).WaitForCompletion();
		BattleBeginText.transform.localPosition = TextInitialPosition;
		BattleBeginAlert.SetActive(false);
		p.BattleEnemyPuppetAnimator.Play("BattleEnemyNormal");

		for (BattleTurn = 1; BattleTurn <= 10; BattleTurn++)
		{
			BattleTurnText.text = string.Format("Turn {0}", BattleTurn);
			if (!AutoProceed && !BattleDodgeButtonClicked)
			{
				BattleButtonPanel.SetActive(true);
				BattleNextButtonText.text = BattleTurn == 1 ? "시작" : "다음";
				BattleNextTrigger = false;
				yield return new WaitUntil(() => BattleNextTrigger);
				BattleNextTrigger = false;
				BattleButtonPanel.SetActive(false);
			}

			if (!BattleDodgeButtonClicked)
			{
				if (p.Status.DEX >= CurrentBattleEnemy.Status.DEX)
				{
					yield return StartCoroutine(SpecialStatusProcess(UserTypeEnum.Player));
					if (BattleEnded) break;
					yield return StartCoroutine(SpecialStatusProcess(UserTypeEnum.Enemy));
					if (BattleEnded) break;
					yield return StartCoroutine(SkillProcess(PlayerSkills[BattleTurn - 1]));
					if (BattleEnded) break;
					yield return StartCoroutine(SkillProcess(EnemySkills[BattleTurn - 1]));
					if (BattleEnded) break;
				}
				else
				{
					yield return StartCoroutine(SpecialStatusProcess(UserTypeEnum.Enemy));
					if (BattleEnded) break;
					yield return StartCoroutine(SpecialStatusProcess(UserTypeEnum.Player));
					if (BattleEnded) break;
					yield return StartCoroutine(SkillProcess(EnemySkills[BattleTurn - 1]));
					if (BattleEnded) break;
					yield return StartCoroutine(SkillProcess(PlayerSkills[BattleTurn - 1]));
					if (BattleEnded) break;
				}
			}
			else if (BattleDodgeElapsedTurn == 3)
			{
				break;
			}
			else
			{
				yield return StartCoroutine(SpecialStatusProcess(UserTypeEnum.Enemy));
				if (BattleEnded) break;
				yield return StartCoroutine(SpecialStatusProcess(UserTypeEnum.Player));
				if (BattleEnded) break;
				yield return StartCoroutine(SkillProcess(EnemySkills[BattleTurn - 1]));
				if (BattleEnded) break;
				BattleDodgeElapsedTurn++;
			}

			p.BattleStatus.CurrentSP += (int)(p.BattleStatus.MaxSP * 0.1f);
			p.BattleStatus.CurrentSP = Mathf.Min(p.BattleStatus.CurrentSP, p.BattleStatus.MaxSP);
			PlayerGaugeGroup.UpdateAllStatusImmediate(p.BattleStatus, false);
			CurrentBattleEnemy.BattleStatus.CurrentSP += (int)(CurrentBattleEnemy.BattleStatus.MaxSP * 0.1f);
			CurrentBattleEnemy.BattleStatus.CurrentSP = Mathf.Min(CurrentBattleEnemy.BattleStatus.CurrentSP, CurrentBattleEnemy.BattleStatus.MaxSP);
			EnemyGaugeGroup.UpdateAllStatusImmediate(CurrentBattleEnemy.BattleStatus, false);
		}
		if (BattleEnded)
		{
			if (PlayerVictory)
			{
				p.BattleEnemyPuppetAnimator.Play("None");
				yield return BattleEnemySpriteMaterial.DOColor(Color.white, AnimationTime * 0.3f).WaitForCompletion();
				yield return BattleEnemySpriteMaterial.DOFloat(0f, "_Alpha", AnimationTime).WaitForCompletion();
				yield return new WaitForSeconds(AnimationTime * 0.5f);
				p.BattleEnemyPuppet.SetActive(false);
				yield return StartCoroutine(BattleWinProcess());
			}
			else
			{
				yield return StartCoroutine(BattleLoseProcess());
			}
		}
		else
		{
			if (BattleDodgeButtonClicked)
			{
				yield return StartCoroutine(CommonUI.Instance.ShowAlertDialog("전투를 회피했습니다", false));
			}
			else
			{
				yield return StartCoroutine(BattleDrawProcess());
			}
		}
		PlayerSkills.ForEach((OneSkill) => Destroy(OneSkill.gameObject));
		EnemySkills.ForEach((OneSkill) => Destroy(OneSkill.gameObject));
		List<GameObject> PlayerSpecialStatusCopy = new List<GameObject>(p.BattleStatus.SpecialStatuses);
		foreach (GameObject SpecialStatusObject in PlayerSpecialStatusCopy)
		{
			SpecialStatusData SpecialStatus = SpecialStatusObject.GetComponent<SpecialStatusData>();
			StartCoroutine(RemoveSpecialStatus(UserTypeEnum.Player, SpecialStatus, false));
		}
		List<GameObject> EnemySpecialStatusCopy = new List<GameObject>(CurrentBattleEnemy.BattleStatus.SpecialStatuses);
		foreach (GameObject SpecialStatusObject in EnemySpecialStatusCopy)
		{
			SpecialStatusData SpecialStatus = SpecialStatusObject.GetComponent<SpecialStatusData>();
			StartCoroutine(RemoveSpecialStatus(UserTypeEnum.Enemy, SpecialStatus, false));
		}
		p.Status.CurrentHP = Mathf.Min(p.BattleStatus.CurrentHP, p.Status.MaxHP);
		p.Status.CurrentMP = Mathf.Min(p.BattleStatus.CurrentMP, p.Status.MaxMP);
		p.Status.CurrentSP = Mathf.Min(p.BattleStatus.CurrentSP, p.Status.MaxSP);
		CurrentBattleEnemy.Status.CurrentHP = Mathf.Min(CurrentBattleEnemy.BattleStatus.CurrentHP, CurrentBattleEnemy.Status.MaxHP);
		CurrentBattleEnemy.Status.CurrentMP = Mathf.Min(CurrentBattleEnemy.BattleStatus.CurrentMP, CurrentBattleEnemy.Status.MaxMP);
		CurrentBattleEnemy.Status.CurrentSP = Mathf.Min(CurrentBattleEnemy.BattleStatus.CurrentSP, CurrentBattleEnemy.Status.MaxSP);
		ui.GaugeGroup.UpdateAllStatusImmediate(p.Status, true);
		BattlePanel.SetActive(false);
		p.BattleEnemyPuppet.SetActive(false);
		RenderSettings.fog = true;
		dc.OnBattleComplete(BattleTurn);
		StartCoroutine(SetBlackOverlayState(false));
		ui.gameObject.SetActive(true);
	}

	public Material BattleEnemySpriteMaterial;
	public IEnumerator SetMonsterSprite(Monster TargetMonster)
	{
		p.BattleEnemyPuppet.SetActive(true);
		Texture2D MonsterTexture = TargetMonster.MonsterSprite.texture;
		BattleEnemySpriteMaterial.mainTexture = MonsterTexture;
		Vector2 MonsterSpriteScale = new Vector2(1000f / MonsterTexture.width, 1000f / MonsterTexture.height);
		BattleEnemySpriteMaterial.mainTextureScale = MonsterSpriteScale;
		BattleEnemySpriteMaterial.mainTextureOffset = new Vector2(-((MonsterSpriteScale.x - 1f) / 2f), 0.5f - MonsterSpriteScale.y);
		p.BattleEnemyParent.transform.localPosition = TargetMonster.PositionOffset;
		p.BattleEnemyParent.transform.localScale = Vector3.one * TargetMonster.SpriteScale;
		BattleEnemySpriteMaterial.color = Color.black;
		yield return new WaitForSeconds(AnimationTime * 0.5f);
		yield return BattleEnemySpriteMaterial.DOFloat(1f, "_Alpha", AnimationTime * 0.3f).WaitForCompletion();
		yield return BattleEnemySpriteMaterial.DOColor(Color.clear, AnimationTime).WaitForCompletion();
	}

	public CanvasGroup BattleResultWinPanel;
	public Transform WinPanelWinText, WinPanelDrawText;
	public Image WinPanelDecorationLeft, WinPanelDecorationRight;
	public Text WinPanelLevelText, WinPanelExpCurrentText, WinPanelExpMaxText, WinPanelExpAcquiredText;
	public Image WinPanelExpGauge;
	void BattleWinPanelInitialize()
	{
		WinPanelLevelText.text = p.Status.Level.ToString();
		int CurrentExp = p.Exp;
		int ExpToLevelUp = DBManager.Instance.LevelDataDictionary[p.Status.Level].ExpToLevelUp;
		WinPanelExpCurrentText.text = CurrentExp.ToString();
		WinPanelExpAcquiredText.color = ColorPalette.Instance.CommonGrayText;
		WinPanelExpAcquiredText.text = "0";
		WinPanelExpMaxText.text = ExpToLevelUp.ToString();
		WinPanelExpGauge.fillAmount = (float)CurrentExp / ExpToLevelUp;
	}

	IEnumerator BattleWinProcess()
	{
		p.HuntMonsterCount++;
		BattleResultWinPanel.gameObject.SetActive(true);
		BattleResultWinPanel.alpha = 0f;
		BattleWinPanelInitialize();
		WinPanelWinText.gameObject.SetActive(true);
		WinPanelDrawText.gameObject.SetActive(false);
		WinPanelDecorationLeft.color = Color.white.WithAlpha(0f);
		WinPanelDecorationRight.color = Color.white.WithAlpha(0f);
		WinPanelDecorationLeft.DOFade(1f, 0.4f);
		WinPanelDecorationRight.DOFade(1f, 0.4f);
		BattleResultWinPanel.DOFade(1f, 0.2f);
		yield return WinPanelWinText.DOPunchScale(Vector3.one * 0.3f, 0.4f, 1, 0f).WaitForCompletion();
		WinPanelLevelText.text = p.Status.Level.ToString();
		int PlayerExpBefore = p.Exp;
		int MonsterExp = DBManager.Instance.LevelDataDictionary[CurrentBattleEnemy.Status.Level].MonsterExp;
		int ExpToLevelUp = DBManager.Instance.LevelDataDictionary[p.Status.Level].ExpToLevelUp;
		WinPanelExpCurrentText.text = PlayerExpBefore.ToString();
		WinPanelExpMaxText.text = ExpToLevelUp.ToString();
		WinPanelExpAcquiredText.color = ColorPalette.Instance.GetCommonTextColor(MonsterExp);
		WinPanelExpAcquiredText.text = string.Format("({0})", MonsterExp.GetSignedText());
		int PlayerExpAfter = p.Exp = PlayerExpBefore + MonsterExp;
		WinPanelExpCurrentText.DOCounter(PlayerExpBefore, PlayerExpAfter, 0.8f);
		float FillAmountAfter = (float)PlayerExpAfter / ExpToLevelUp;
		yield return WinPanelExpGauge.DOFillAmount(FillAmountAfter, 0.8f).WaitForCompletion();
		while (p.Exp >= ExpToLevelUp)
		{
			// 레벨 업
			p.Exp -= ExpToLevelUp;
			yield return StartCoroutine(CommonUI.Instance.LevelUpProcess());
			WinPanelLevelText.text = p.Status.Level.ToString();
			WinPanelExpGauge.fillAmount = 0f;
			WinPanelExpCurrentText.DOCounter(0, p.Exp, 0.8f);
			ExpToLevelUp = DBManager.Instance.LevelDataDictionary[p.Status.Level].ExpToLevelUp;
			WinPanelExpMaxText.text = ExpToLevelUp.ToString();
			float FillAmountAfterLevelUp = (float)p.Exp / ExpToLevelUp;
			yield return WinPanelExpGauge.DOFillAmount(FillAmountAfterLevelUp, 0.8f).WaitForCompletion();
		}
		WinPanelOKButtonTrigger = false;
		yield return new WaitUntil(() => WinPanelOKButtonTrigger);
		WinPanelOKButtonTrigger = false;

		// 스킬 획득
		SkillData ObtainedSkill = null;
		if (CurrentBattleEnemy.SpecialSkill && Random.value < 0.1f) 
		{
			ObtainedSkill = CurrentBattleEnemy.SpecialSkill;
		}
		else if (Random.value < 0.6f)
		{
			if(CurrentBattleEnemy.SpecialSkill) CurrentBattleEnemy.Status.Skills.Remove(CurrentBattleEnemy.SpecialSkill.gameObject);
			List<SkillData> MonsterSkillsDistinct = CurrentBattleEnemy.Status.Skills.Distinct().Select(x => x.GetComponent<SkillData>()).ToList();
			Util.CalculatePossibilities(MonsterSkillsDistinct, (x) => x.SpawnPossibilityInt, (x, Min) => x.MinSpawnPossibility = Min, (x, Max) => x.MaxSpawnPossibility = Max);
			ObtainedSkill = Util.GetWeightedItem(MonsterSkillsDistinct, (x) => x.MinSpawnPossibility, (x) => x.MaxSpawnPossibility);
		}
		if (ObtainedSkill && !p.Status.Skills.Contains(ObtainedSkill.gameObject))
		{
			yield return StartCoroutine(CommonUI.Instance.SkillObtainProcess(ObtainedSkill));
		}

		// 보스라면 룬을 얻는다
		if (CurrentBattleEnemy.MonsterType == Monster.MonsterTypeEnum.Boss)
		{
			switch (dc.CurrentDungeonData.DungeonArea)
			{
				case DungeonAreaEnum.LostTemple:
					p.TempleRuneObtained = true;
					break;
				case DungeonAreaEnum.ShadowTunnel:
					p.TunnelRuneObtained = true;
					break;
			}
			ui.UpdateRuneProgress();
		}
		yield return BattleResultWinPanel.DOFade(0f, 0.2f).WaitForCompletion();
		BattleResultWinPanel.gameObject.SetActive(false);
		Destroy(CurrentBattleEnemy.gameObject);
	}

	bool WinPanelOKButtonTrigger;
	public void WinPanelOKButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		WinPanelOKButtonTrigger = true;
	}

	public CanvasGroup BattleResultLosePanel;
	public CanvasGroup LosePanelLoseButton;
	public Image LosePanelBlackBackground;
	public Text LosePanelLoseText;
	IEnumerator BattleLoseProcess()
	{
		BattleResultLosePanel.gameObject.SetActive(true);
		LosePanelBlackBackground.color = LosePanelBlackBackground.color.WithAlpha(0f);
		LosePanelBlackBackground.DOFade(1f, 4f);
		LosePanelLoseText.color = LosePanelLoseText.color.WithAlpha(0f);
		LosePanelLoseButton.alpha = 0f;
		yield return LosePanelLoseText.DOFade(1f, 2.5f).WaitForCompletion();
		yield return LosePanelLoseButton.DOFade(1f, 1.5f).WaitForCompletion();
		yield return new WaitUntil(() => LosePanelNextButtonTrigger);
		BattleResultLosePanel.gameObject.SetActive(false);
		StartCoroutine(cu.GameEndProcess());
	}

	bool LosePanelNextButtonTrigger;
	public void LosePanelNextButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		LosePanelNextButtonTrigger = true;
	}

	public IEnumerator BattleDrawProcess()
	{
		BattleResultWinPanel.gameObject.SetActive(true);
		BattleResultWinPanel.alpha = 0f;
		BattleWinPanelInitialize();
		WinPanelWinText.gameObject.SetActive(false);
		WinPanelDrawText.gameObject.SetActive(true);
		WinPanelDecorationLeft.color = Color.white.WithAlpha(0f);
		WinPanelDecorationRight.color = Color.white.WithAlpha(0f);
		WinPanelDecorationLeft.DOFade(1f, 0.4f);
		WinPanelDecorationRight.DOFade(1f, 0.4f);
		yield return BattleResultWinPanel.DOFade(1f, 0.2f).WaitForCompletion();
		Text DrawTextComponent = WinPanelDrawText.GetComponent<Text>();
		DrawTextComponent.color = DrawTextComponent.color.WithAlpha(0f);
		yield return DrawTextComponent.DOFade(1f, 1.2f).WaitForCompletion();
		WinPanelLevelText.text = p.Status.Level.ToString();
		int PlayerExpBefore = p.Exp;
		int MonsterExp = 0;
		int ExpToLevelUp = DBManager.Instance.LevelDataDictionary[p.Status.Level].ExpToLevelUp;
		WinPanelExpCurrentText.text = PlayerExpBefore.ToString();
		WinPanelExpMaxText.text = ExpToLevelUp.ToString();
		WinPanelExpAcquiredText.color = ColorPalette.Instance.GetCommonTextColor(MonsterExp);
		WinPanelExpAcquiredText.text = string.Format("({0})", MonsterExp.GetSignedText());
		WinPanelOKButtonTrigger = false;
		yield return new WaitUntil(() => WinPanelOKButtonTrigger);
		WinPanelOKButtonTrigger = false;
		yield return BattleResultWinPanel.DOFade(0f, 0.2f).WaitForCompletion();
		BattleResultWinPanel.gameObject.SetActive(false);
	}

	IEnumerator SpecialStatusProcess(UserTypeEnum UserType)
	{
		GetUserStatus(UserType, out _, out CommonStatus UserStatus, out _, out _);
		List<SpecialStatusData> StatusesToEnd = new List<SpecialStatusData>();
		foreach (GameObject OneStatusObject in UserStatus.SpecialStatuses)
		{
			SpecialStatusData OneStatus = OneStatusObject.GetComponent<SpecialStatusData>();
			if (OneStatus.GetComponent<ISpecialStatusEventEveryTurn>() != null) 
			{
				yield return PlayTimeline(OneStatus.UserType, OneStatus.Effect, OneStatus.GetComponent<ISpecialStatusEventEveryTurn>().EveryTurnEffect);
			}
			if (!OneStatus.IsPermanent)
			{
				OneStatus.Duration--;
				OneStatus.IconObject.IconText.text = Mathf.Max(0, OneStatus.Duration - 1).ToString();
				if (OneStatus.Duration <= 0)
				{
					StatusesToEnd.Add(OneStatus);
				}
			}
		}
		foreach (SpecialStatusData OneStatus in StatusesToEnd)
		{
			yield return StartCoroutine(RemoveSpecialStatus(UserType, OneStatus));
		}
	}

	public CanvasGroup SkillAlert;
	public SkillIcon SkillAlertIcon;
	public Text SkillAlertText;
	public List<Image> SkillAlertBorders;
	public Color SkillAlertBorderPlayerColor, SkillAlertBorderEnemyColor;
	IEnumerator SkillProcess(GameObject SkillObject)
	{
		SkillData Skill = SkillObject.GetComponent<SkillData>();
		if (Skill.UserBattleStatus.SpecialStatuses.Find((x) => x.GetComponent<Special20Immovable>()))
		{
			yield return StartCoroutine(CommonUI.Instance.ShowMessageAlertText("행동불능 상태로 인하여 스킬을 사용할 수 없습니다", ColorPalette.Instance.CommonText));
			yield break;
		}

		SkillAlertIcon.SetSkillIconData(Skill);
		SkillAlertText.text = Skill.KoreanName;
		switch (Skill.UserType)
		{
			case UserTypeEnum.Player:
				SkillAlertBorders.ForEach((x) => x.color = SkillAlertBorderPlayerColor);
				break;
			case UserTypeEnum.Enemy:
				SkillAlertBorders.ForEach((x) => x.color = SkillAlertBorderEnemyColor);
				break;
		}
		SkillAlert.gameObject.SetActive(true);
		SkillAlert.alpha = 0f;
		Vector3 InitialPosition = SkillAlert.transform.localPosition;
		SkillAlert.DOFade(1f, AnimationTime * 0.3f);
		SkillAlert.transform.localPosition = InitialPosition.WithY(InitialPosition.y - 30f);
		yield return SkillAlert.transform.DOLocalMoveY(InitialPosition.y, AnimationTime * 0.3f).WaitForCompletion();
		yield return new WaitForSeconds(AnimationTime);
		SkillAlert.DOFade(0f, AnimationTime * 0.3f);
		yield return SkillAlert.transform.DOLocalMoveY(InitialPosition.y + 30f, AnimationTime * 0.3f).WaitForCompletion();
		SkillAlert.transform.localPosition = InitialPosition;
		SkillAlert.gameObject.SetActive(false);

		GetUserStatus(Skill.UserType, out _, out CommonStatus UserBattleStatus, out _, out StatGaugeGroup UserGaugeGroup);
		Color SkillFailAlertColor = Color.white;
		string SkillFailAlertText = "";
		bool SkillSuccess = true;
		int HPCost = 0; int MPCost = 0; int SPCost = 0;
		CostProcess(ref UserBattleStatus.CurrentHP, ref HPCost, Skill.HPCost, "체력이 부족하여 스킬을 사용할 수 없습니다", ColorPalette.Instance.HPText);
		CostProcess(ref UserBattleStatus.CurrentMP, ref MPCost, Skill.MPCost, "마력이 부족하여 스킬을 사용할 수 없습니다", ColorPalette.Instance.MPText);
		CostProcess(ref UserBattleStatus.CurrentSP, ref SPCost, Skill.SPCost, "기력이 부족하여 스킬을 사용할 수 없습니다", ColorPalette.Instance.SPText);
		void CostProcess(ref int StatPoint, ref int Cost, int SkillCost, string FailAlertText, Color FailAlertColor)
		{
			if (SkillSuccess && SkillCost > 0)
			{
				if (StatPoint > SkillCost)
				{
					Cost += SkillCost;
				}
				else
				{
					SkillSuccess = false;
					SkillFailAlertText = FailAlertText;
					SkillFailAlertColor = FailAlertColor;
				}
			}
		}
		CostProcessPercent(ref UserBattleStatus.CurrentHP, ref HPCost, Skill.HPCostPercent, "체력이 부족하여 스킬을 사용할 수 없습니다", ColorPalette.Instance.HPText);
		CostProcessPercent(ref UserBattleStatus.CurrentMP, ref MPCost, Skill.MPCostPercent, "마력이 부족하여 스킬을 사용할 수 없습니다", ColorPalette.Instance.MPText);
		CostProcessPercent(ref UserBattleStatus.CurrentSP, ref SPCost, Skill.SPCostPercent, "기력이 부족하여 스킬을 사용할 수 없습니다", ColorPalette.Instance.SPText);
		void CostProcessPercent(ref int StatPoint, ref int Cost, float CostPercent, string FailAlertText, Color FailAlertColor)
		{
			int SkillCost = (int)(StatPoint * CostPercent);
			if (SkillSuccess && SkillCost > 0f)
			{
				if (StatPoint > SkillCost)
				{
					Cost += SkillCost;
				}
				else
				{
					SkillSuccess = false;
					SkillFailAlertText = FailAlertText;
					SkillFailAlertColor = FailAlertColor;
				}
			}
		}

		if (SkillSuccess)
		{
			UserBattleStatus.CurrentHP -= HPCost;
			UserBattleStatus.CurrentMP -= MPCost;
			UserBattleStatus.CurrentSP -= SPCost;
			yield return StartCoroutine(UserGaugeGroup.UpdateAllStatus(UserBattleStatus, null));
			switch (Skill.UserType)
			{
				case UserTypeEnum.Enemy:
					yield return BattleEnemySpriteMaterial.DOColor(Color.white.WithAlpha(0.6f), AnimationTime * 0.3f).WaitForCompletion();
					BattleEnemySpriteMaterial.DOColor(Color.white.WithAlpha(0f), AnimationTime * 0.2f).WaitForCompletion();
					break;
			}
			if (Skill.ActivateSkillEvent == null)
			{
				Debug.LogWarning($"{Skill.KoreanName} 미구현");
				yield break;
			}
			yield return StartCoroutine(PlayTimeline(Skill.UserType, Skill.SkillEffect, Skill.ActivateSkillEvent.ActivateEffect));
		}
		else
		{
			yield return StartCoroutine(CommonUI.Instance.ShowMessageAlertText(SkillFailAlertText, SkillFailAlertColor));
		}
	}

	public List<AudioSource> BattleEffectAudioSources;
	[ReadOnly] public bool TimelineCompleteTrigger;
	IEnumerator PlayTimeline(UserTypeEnum UserType, TimelineAsset EffectTimeline, Func<IEnumerator> SignalAction)
	{
		if (!EffectTimeline)
		{
			Debug.LogWarning($"{UserType} {EffectTimeline} {SignalAction.Method.Name} 없음");
			yield return StartCoroutine(SignalAction());
			yield break;
		}
		TimelineSignalReceiver.GetReactionAtIndex(0).RemoveAllListeners();
		TimelineSignalReceiver.GetReactionAtIndex(0).AddListener(() => StartCoroutine(SignalAction()));
		BattlePlayableDirector.playableAsset = EffectTimeline;
		IEnumerable<TrackAsset> Tracks = EffectTimeline.GetOutputTracks();
		foreach (TrackAsset Track in Tracks)
		{
			if (Track is ControlTrack)
			{
				foreach (TimelineClip OneClip in Track.GetClips())
				{
					ControlPlayableAsset ParticleAsset = OneClip.asset as ControlPlayableAsset;
					BattlePlayableDirector.SetReferenceValue(ParticleAsset.sourceGameObject.exposedName, p.EffectCamera.gameObject);
					SkillParticleData ParticleData = ParticleAsset.prefabGameObject.GetComponent<SkillParticleData>();
					if (ParticleData)
					{
						switch (ParticleData.TranslateType)
						{
							case SkillParticleData.TranslateTypeEnum.RotateX:
								switch (UserType)
								{
									case UserTypeEnum.Player:
										ParticleAsset.prefabGameObject.transform.localEulerAngles = Vector3.zero;
										break;
									case UserTypeEnum.Enemy:
										ParticleAsset.prefabGameObject.transform.localEulerAngles = new Vector3(180f, 0f, 0f);
										break;
								}
								break;
							case SkillParticleData.TranslateTypeEnum.MoveZUser:
								switch (UserType)
								{
									case UserTypeEnum.Player:
										ParticleAsset.prefabGameObject.transform.localPosition = new Vector3(0f, -0.75f, 2f);
										break;
									case UserTypeEnum.Enemy:
										ParticleAsset.prefabGameObject.transform.localPosition = new Vector3(0f, -0.5f, 3f);
										break;
								}
								break;
							case SkillParticleData.TranslateTypeEnum.MoveZTarget:
								switch (UserType)
								{
									case UserTypeEnum.Player:
										ParticleAsset.prefabGameObject.transform.localPosition = new Vector3(0f, -0.5f, 3f);
										break;
									case UserTypeEnum.Enemy:
										ParticleAsset.prefabGameObject.transform.localPosition = new Vector3(0f, -0.75f, 2f);
										break;
								}
								break;
						}
					}
				}
			}
		}
		BattlePlayableDirector.RebuildGraph();
		BattlePlayableDirector.playableGraph.GetRootPlayable(0).SetSpeed(BattleSpeed);
		int i = 0;
		foreach (PlayableBinding output in BattlePlayableDirector.playableAsset.outputs)
		{
			if (output.outputTargetType == typeof(AudioSource))
			{
				BattlePlayableDirector.SetGenericBinding(output.sourceObject, BattleEffectAudioSources[i]);
				i++;
			}
		} 
		BattlePlayableDirector.Play();
		TimelineCompleteTrigger = false;
		yield return new WaitUntil(() => TimelineCompleteTrigger);
		TimelineCompleteTrigger = false;
		yield return new WaitForSeconds(AnimationTime * 1.5f);
	}

	(bool End, bool Victory) JudgeEndBattle()
	{
		if (p.BattleStatus.CurrentHP <= 0) return (true, false);
		else if (CurrentBattleEnemy.BattleStatus.CurrentHP <= 0) return (true, true);
		else return (false, false);
	}

	public StatGaugeGroup PlayerGaugeGroup, EnemyGaugeGroup;
	public GameObject EnemyDamageIndicator, PlayerDamageIndicator;
	public AnimationCurve PlayerDamageTextEase;
	public Color EnemyDamageColor;
	public Image PlayerDamageScreenImage;
	public IEnumerator GiveDamage(UserTypeEnum DamagedUserType, int Damage, ElementalTypeEnum ElementalType, bool IgnoreArmor = false, float AnimationTimeMultiplier = 1f)
	{
		GetUserStatus(DamagedUserType, out CommonStatus DamagedUserStatus, out CommonStatus DamagedUserBattleStatus, out CommonStatus AttackerBattleStatus, out StatGaugeGroup UserGaugeGroup);

		IntRef DamageRef = new IntRef() { Value = Damage };
		List<GameObject> AttackerSpecialStatusesCopy = new List<GameObject>(AttackerBattleStatus.SpecialStatuses);
		foreach (GameObject SpecialStatus in AttackerSpecialStatusesCopy)
		{
			SpecialStatus.GetComponent<ISpecialStatusEventAttackDamage>()?.AttackDamageEffect(DamageRef, ElementalType);
		}
		List<GameObject> DamagedUserSpecialStatusesCopy = new List<GameObject>(DamagedUserBattleStatus.SpecialStatuses);
		foreach (GameObject SpecialStatus in DamagedUserSpecialStatusesCopy)
		{
			SpecialStatus.GetComponent<ISpecialStatusEventReceiveDamage>()?.ReceiveDamageEffect(DamageRef);
		}
		Damage = DamageRef.Value;

		if (!IgnoreArmor) Damage -= DamagedUserBattleStatus.Armor;
		if (Damage > 0)
		{
			ElementalType = ElementalType == ElementalTypeEnum.None ? AttackerBattleStatus.ElementalType : ElementalType;
			Damage = (int)(Damage * ElementalHelper.GetDamageMultiplier(ElementalType, DamagedUserBattleStatus.ElementalType));
			DamagedUserBattleStatus.CurrentHP -= Damage;
			StartCoroutine(UserGaugeGroup.UpdateAllStatus(DamagedUserBattleStatus, DamagedUserStatus));

			GameObject DamageIndicator = null;
			Text DamageIndicatorText;
			switch (DamagedUserType)
			{
				case UserTypeEnum.Player:
					DamageIndicator = Instantiate(PlayerDamageIndicator, BattlePanel.transform, false);
					break;
				case UserTypeEnum.Enemy:
					DamageIndicator = Instantiate(EnemyDamageIndicator, BattlePanel.transform, false);
					break;
			}
			DamageIndicator.SetActive(true);
			DamageIndicatorText = DamageIndicator.transform.Find("NumberText").GetComponent<Text>();
			DamageIndicatorText.text = Damage.ToString();
			switch (DamagedUserType)
			{
				case UserTypeEnum.Player:
					DamageIndicatorText.transform.DOBlendableLocalMoveBy(Vector3.up * 400f, AnimationTime * 2f).SetEase(PlayerDamageTextEase);
					DamageIndicatorText.transform.DOBlendableLocalMoveBy(Vector3.right * Random.Range(-1f, 1f) * 600f, AnimationTime * 2f);
					DamageIndicatorText.transform.DOPunchScale(Vector3.one * 1.3f, AnimationTime * 2f, 1, 1);
					PlayerDamageScreenImage.color = PlayerDamageScreenImage.color.WithAlpha(0.25f);
					PlayerDamageScreenImage.DOFade(0f, 0.4f);
					p.MainCamera.transform.DOShakePosition(0.2f, strength: 0.05f, vibrato: 50);
					BattleEnemyShake();
					break;
				case UserTypeEnum.Enemy:
					DamageIndicator.transform.DOLocalMoveY(DamageIndicator.transform.localPosition.y + 130f, AnimationTime * 2f);
					BattleEnemySpriteMaterial.color = EnemyDamageColor;
					BattleEnemySpriteMaterial.DOColor(EnemyDamageColor.WithAlpha(0f), AnimationTime);
					BattleEnemyShake();
					break;
			}
			void BattleEnemyShake() => p.BattleEnemyPuppet.transform.DOShakePosition(0.4f, strength: 0.05f, vibrato: 20);
			DamageIndicator.GetComponent<CanvasGroup>().DOFade(0f, AnimationTime * 0.5f).SetDelay(AnimationTime * 1.5f).OnComplete(() => Destroy(DamageIndicator));
			yield return new WaitForSeconds((AnimationTime + 0.3f) * AnimationTimeMultiplier);
		}

		bool DisplayArmorIndicator = Damage <= 0;
		if (!IgnoreArmor)
		{
			switch (DamagedUserType)
			{
				case UserTypeEnum.Player:
					StartCoroutine(ChangeStat(StatTypeEnum.Armor, DamagedUserType, Mathf.FloorToInt(p.BattleStatus.Armor * -0.1f), DisplayArmorIndicator));
					break;
				case UserTypeEnum.Enemy:
					StartCoroutine(ChangeStat(StatTypeEnum.Armor, DamagedUserType, Mathf.FloorToInt(CurrentBattleEnemy.BattleStatus.Armor * -0.1f), DisplayArmorIndicator));
					break;
			}
		}

		DamageRef.Value = Damage;
		List<GameObject> AttackerSpecialStatusesCopy2 = new List<GameObject>(AttackerBattleStatus.SpecialStatuses);
		foreach (GameObject SpecialStatus in AttackerSpecialStatusesCopy2)
		{
			ISpecialStatusEventAfterGiveDamage AfterGiveDamageEffect = SpecialStatus.GetComponent<ISpecialStatusEventAfterGiveDamage>();
			if (AfterGiveDamageEffect != null) 
				yield return StartCoroutine(AfterGiveDamageEffect.AfterGiveDamageEffect(true, DamageRef));
		}
		List<GameObject> DamagedUserSpecialStatusesCopy2 = new List<GameObject>(AttackerBattleStatus.SpecialStatuses);
		foreach (GameObject SpecialStatus in DamagedUserSpecialStatusesCopy2)
		{
			ISpecialStatusEventAfterGiveDamage AfterGiveDamageEffect = SpecialStatus.GetComponent<ISpecialStatusEventAfterGiveDamage>();
			if (AfterGiveDamageEffect != null)
				yield return StartCoroutine(AfterGiveDamageEffect.AfterGiveDamageEffect(false, DamageRef));
		}
		(BattleEnded, PlayerVictory) = JudgeEndBattle();
	}


	public GameObject EnemyHealIndicator, PlayerHealIndicator;
	public IEnumerator GiveHP(UserTypeEnum SkillUserType, int HealAmount)
	{
		GetUserStatus(SkillUserType, out CommonStatus UserStatus, out CommonStatus UserBattleStatus, out _, out StatGaugeGroup UserGaugeGroup);
		UserBattleStatus.CurrentHP = Mathf.Min(Mathf.Max(UserBattleStatus.CurrentHP, UserBattleStatus.MaxHP), UserBattleStatus.CurrentHP + HealAmount);
		StartCoroutine(UserGaugeGroup.UpdateAllStatus(UserBattleStatus, UserStatus));
		GameObject AmountIndicator = null;
		Text AmountIndicatorText;
		switch (SkillUserType)
		{
			case UserTypeEnum.Player:
				AmountIndicator = Instantiate(EnemyHealIndicator, BattlePanel.transform, false);
				break;
			case UserTypeEnum.Enemy:
				AmountIndicator = Instantiate(PlayerHealIndicator, BattlePanel.transform, false);
				break;
		}
		AmountIndicator.SetActive(true);
		AmountIndicatorText = AmountIndicator.transform.Find("NumberText").GetComponent<Text>();
		AmountIndicatorText.text = HealAmount.ToString();
		AmountIndicator.transform.DOLocalMoveY(AmountIndicator.transform.localPosition.y + 130f, AnimationTime * 2f);
		yield return AmountIndicator.GetComponent<CanvasGroup>().DOFade(0f, AnimationTime * 1f).SetDelay(AnimationTime).WaitForCompletion();
		Destroy(AmountIndicator);
	}

	public GameObject PlayerArmorIndicator, EnemyArmorIndicator;
	public GameObject PlayerStatIndicator, EnemyStatIndicator;
	public IEnumerator ChangeStat(StatTypeEnum StatType, UserTypeEnum TargetUserType, int Amount, bool DisplayIndicator = true, float WaitPosition = 1f)
	{
		if (Amount == 0) yield break;
		GetUserStatus(TargetUserType, out CommonStatus UserStatus, out CommonStatus UserBattleStatus, out _, out StatGaugeGroup UserGaugeGroup);
		switch (StatType)
		{
			case StatTypeEnum.HP: UserBattleStatus.AdditionalHP += Amount; break;
			case StatTypeEnum.MP: UserBattleStatus.AdditionalMP += Amount; break;
			case StatTypeEnum.SP: UserBattleStatus.AdditionalSP += Amount; break;
			case StatTypeEnum.Armor: UserBattleStatus.Armor += Amount; break;
			case StatTypeEnum.STR: UserBattleStatus.STR += Amount; break;
			case StatTypeEnum.DEX: UserBattleStatus.DEX += Amount; break;
			case StatTypeEnum.INT: UserBattleStatus.INT += Amount; break;
			case StatTypeEnum.CON: UserBattleStatus.CON += Amount; break;
		}
		switch (StatType)
		{
			case StatTypeEnum.HP:
			case StatTypeEnum.STR:
			case StatTypeEnum.CON:
				UserBattleStatus.UpdateMaxHP(UserStatus);
				break;
			case StatTypeEnum.MP:
			case StatTypeEnum.INT:
				UserBattleStatus.UpdateMaxMP(UserStatus);
				break;
			case StatTypeEnum.SP:
			case StatTypeEnum.DEX:
				UserBattleStatus.UpdateMaxSP(UserStatus);
				break;
		}
		yield return UserGaugeGroup.UpdateAllStatus(UserBattleStatus, UserStatus);
		if (DisplayIndicator)
		{
			GameObject AmountIndicatorPrefab;
			if (StatType == StatTypeEnum.Armor)
			{
				AmountIndicatorPrefab = TargetUserType switch
				{
					UserTypeEnum.Player => PlayerArmorIndicator,
					UserTypeEnum.Enemy => EnemyArmorIndicator,
					_ => null,
				};
				GameObject AmountIndicator = Instantiate(AmountIndicatorPrefab, BattlePanel.transform, false);
				AmountIndicator.SetActive(true);
				AmountIndicator.transform.Find("IncreaseImage").gameObject.SetActive(Amount >= 0);
				AmountIndicator.transform.Find("IncreaseText").gameObject.SetActive(Amount >= 0);
				AmountIndicator.transform.Find("DecreaseImage").gameObject.SetActive(Amount < 0);
				AmountIndicator.transform.Find("DecreaseText").gameObject.SetActive(Amount < 0);
				Text AmountIndicatorText = Amount >= 0 ? AmountIndicator.transform.Find("IncreaseText").GetComponent<Text>() : AmountIndicator.transform.Find("DecreaseText").GetComponent<Text>();
				AmountIndicatorText.text = Amount.ToString();
				AmountIndicator.transform.DOLocalMoveY(AmountIndicator.transform.localPosition.y + 130f, AnimationTime * 2f);
				yield return AmountIndicator.GetComponent<CanvasGroup>().DOFade(0f, AnimationTime).SetDelay(AnimationTime).WaitForPosition(WaitPosition);
				Destroy(AmountIndicator);
			}
			else
			{
				AmountIndicatorPrefab = TargetUserType switch
				{
					UserTypeEnum.Player => PlayerStatIndicator,
					UserTypeEnum.Enemy => EnemyStatIndicator,
					_ => null,
				};
				GameObject AmountIndicator = Instantiate(AmountIndicatorPrefab, BattlePanel.transform, false);
				AmountIndicator.SetActive(true);
				Color TextColor = Amount > 0 ? ColorPalette.Instance.CommonGreenText : ColorPalette.Instance.CommonRedText;
				Text StatName = AmountIndicator.transform.Find("StatName").GetComponent<Text>();
				static string StatTypeToString(StatTypeEnum type)
				{
					return type switch
					{
						StatTypeEnum.HP => "HP",
						StatTypeEnum.MP => "HP",
						StatTypeEnum.SP => "SP",
						StatTypeEnum.Armor => "ARMOR",
						StatTypeEnum.STR => "STR",
						StatTypeEnum.DEX => "DEX",
						StatTypeEnum.INT => "INT",
						StatTypeEnum.CON => "CON",
						_ => "",
					};
				}
				StatName.text = StatTypeToString(StatType);
				StatName.color = TextColor;
				Text NumberText = AmountIndicator.transform.Find("NumberText").GetComponent<Text>();
				NumberText.text = Amount.GetSignedText();
				NumberText.color = TextColor;
				float MovementY = Amount > 0 ? 130f : -100f;
				AmountIndicator.transform.DOLocalMoveY(AmountIndicator.transform.localPosition.y + MovementY, AnimationTime * 2f);
				yield return AmountIndicator.GetComponent<CanvasGroup>().DOFade(0f, AnimationTime * 1f).SetDelay(AnimationTime).WaitForPosition(WaitPosition);
				Destroy(AmountIndicator);
			}
		}
	}

	public GameObject PlayerSpecialStatusIconPrefab, EnemySpecialStatusIconPrefab;
	public Transform PlayerSpecialStatusGroupParent, EnemySpecialStatusGroupParent;
	public IEnumerator AddSpecialStatus(UserTypeEnum UserType, string SpecialStatusID, int Duration)
	{
		CommonStatus UserStatus = null;
		StatGaugeGroup GaugeGroup = null;
		switch (UserType)
		{
			case UserTypeEnum.Player:
				GaugeGroup = PlayerGaugeGroup;
				UserStatus = p.BattleStatus;
				break;
			case UserTypeEnum.Enemy:
				GaugeGroup = EnemyGaugeGroup;
				UserStatus = CurrentBattleEnemy.BattleStatus;
				break;
		}
		SpecialStatusData NewSpecialStatus = dc.AddSpecialStatus(UserType, UserStatus, GaugeGroup, SpecialStatusID, Duration, false);
		if (NewSpecialStatus.GetComponent<ISpecialStatusEventStart>() != null)
		{
			yield return PlayTimeline(NewSpecialStatus.UserType, NewSpecialStatus.Effect, NewSpecialStatus.GetComponent<ISpecialStatusEventStart>().StartEffect);
		}
	}

	public IEnumerator RemoveSpecialStatus(UserTypeEnum UserType, SpecialStatusData SpecialStatus, bool PlayEffect = true)
	{
		CommonStatus UserStatus = null;
		switch (UserType)
		{
			case UserTypeEnum.Player:
				UserStatus = p.BattleStatus;
				break;
			case UserTypeEnum.Enemy:
				UserStatus = CurrentBattleEnemy.BattleStatus;
				break;
		}
		if (SpecialStatus.GetComponent<ISpecialStatusEventEnd>() != null)
		{
			if (PlayEffect) yield return PlayTimeline(SpecialStatus.UserType, SpecialStatus.Effect, SpecialStatus.GetComponent<ISpecialStatusEventEnd>().EndEffect);
			else yield return StartCoroutine(SpecialStatus.GetComponent<ISpecialStatusEventEnd>().EndEffect());
		}
		dc.RemoveSpecialStatus(UserStatus, SpecialStatus, false);
	}

	void GetUserStatus(UserTypeEnum UserType, out CommonStatus UserStatus, out CommonStatus UserBattleStatus, out CommonStatus TargetBattleStatus, out StatGaugeGroup UserGaugeGroup)
	{
		UserStatus = null;
		UserBattleStatus = null;
		TargetBattleStatus = null;
		UserGaugeGroup = null;
		switch (UserType)
		{
			case UserTypeEnum.Player:
				UserStatus = p.Status;
				UserBattleStatus = p.BattleStatus;
				TargetBattleStatus = CurrentBattleEnemy.BattleStatus;
				UserGaugeGroup = PlayerGaugeGroup;
				break;
			case UserTypeEnum.Enemy:
				UserStatus = CurrentBattleEnemy.Status;
				UserBattleStatus = CurrentBattleEnemy.BattleStatus;
				TargetBattleStatus = p.BattleStatus;
				UserGaugeGroup = EnemyGaugeGroup;
				break;
		}
	}

	bool BattleNextTrigger;
	bool BattleDodgeButtonClicked;
	int BattleDodgeElapsedTurn;
	public void OnBattleNextButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		BattleNextTrigger = true;
		BattleDodgeButtonClicked = false;
	}

	public void OnBattleDodgeButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		StartCoroutine(DodgeCoroutine());
		IEnumerator DodgeCoroutine()
		{
			yield return StartCoroutine(CommonUI.Instance.ShowCenterAlertDialog("전투를 회피하시겠습니까?"));
			if (CommonUI.Instance.CenterAlertDialogResult)
			{
				BattleNextTrigger = true;
				BattleDodgeButtonClicked = true;
			}
		}
	}

	public void ReplaceSkill(UserTypeEnum Target, SkillData SkillOriginal, SkillData SkillToReplacePrefab)
	{
		List<SkillIcon> SkillIcons = Target switch
		{
			UserTypeEnum.Player => PlayerSkillIcons,
			UserTypeEnum.Enemy => EnemySkillIcons,
			_ => null,
		};
		List<GameObject> SkillObjectList = Target switch
		{
			UserTypeEnum.Player => p.BattleStatus.Skills,
			UserTypeEnum.Enemy => CurrentBattleEnemy.BattleStatus.Skills,
			_ => null,
		}; ;
		int SkillIndex = SkillObjectList.IndexOf(SkillOriginal.gameObject);
		Destroy(SkillObjectList[SkillIndex]);
		SkillData NewSkill = Instantiate(SkillToReplacePrefab);
		SkillObjectList[SkillIndex] = NewSkill.gameObject;
		SkillIcons[SkillIndex].SetSkillIconData(NewSkill);
	}

	public SkillData GetDefaultSkill(UserTypeEnum Target)
	{
		CommonStatus TargetStatus = Target switch
		{
			UserTypeEnum.Player => p.BattleStatus,
			UserTypeEnum.Enemy => CurrentBattleEnemy.BattleStatus,
			_ => null,
		};
		int MaxStat = Mathf.Max(TargetStatus.STR, TargetStatus.DEX, TargetStatus.INT);
		if (MaxStat == TargetStatus.STR) return DBManager.Instance.SkillDictionary["slash"];
		else if (MaxStat == TargetStatus.DEX) return DBManager.Instance.SkillDictionary["shoot"];
		else return DBManager.Instance.SkillDictionary["magic_missile"];
	}

	public void OnSkillIconSelected(SkillIcon SelectedSkillIcon)
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		CommonUI.Instance.ShowSkillInfo(SelectedSkillIcon.ThisSkillData);
	}

	public Toggle AutoProceedToggle;
	[ReadOnly] public bool AutoProceed;
	public void OnAutoProceedCheckBox(bool On)
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		AutoProceed = On;
		if (AutoProceed)
		{
			BattleNextTrigger = true;
		}
	}

	[ReadOnly] public int BattleSpeed;
	public void OnChangeSpeedButton()
	{
		am.PlaySfx(AudioManager.SfxTypeEnum.ClickOut);
		BattleSpeed = BattleSpeed % 3 + 1;
		SetBattleSpeed(BattleSpeed);
	}

	public GameObject BattleSpeedNormalImage, BattleSpeedFastImage, BattleSpeedSuperFastImage;
	public void SetBattleSpeed(int NewBattleSpeed)
	{
		BattleSpeed = NewBattleSpeed;
		AnimationTime = 0.4f / BattleSpeed;
		BattleSpeedNormalImage.SetActive(false);
		BattleSpeedFastImage.SetActive(false);
		BattleSpeedSuperFastImage.SetActive(false);
		switch (BattleSpeed)
		{
			case 1: BattleSpeedNormalImage.SetActive(true); break;
			case 2: BattleSpeedFastImage.SetActive(true); break;
			case 3: BattleSpeedSuperFastImage.SetActive(true); break;
		}
	}
}
