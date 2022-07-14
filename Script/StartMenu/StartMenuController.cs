using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DuloGames.UI;
using static SingletonLoader;

public class StartMenuController : MonoBehaviour 
{
	public GameObject TitleCanvas, MakeNewCharacterCanvas;
	public DungeonData StartDungeonTheme;

	void Start()
	{
		TitleCanvas.SetActive(true);
		MakeNewCharacterCanvas.SetActive(false);
		StartCoroutine(StartSceneAnimationCoroutine());
	}

	public List<GameObject> TitleTexts;
	public CanvasGroup StartButton, ScoreButton;
	IEnumerator StartSceneAnimationCoroutine()
	{
		Vector2 EffectDistance = TitleTexts[0].GetComponent<Shadow>().effectDistance;
		foreach (GameObject text in TitleTexts)
		{
			text.GetComponent<Shadow>().effectDistance = Vector2.zero;
			text.transform.position = text.transform.position + Vector3.up * 150f;
			text.GetComponent<Text>().color = text.GetComponent<Text>().color.WithAlpha(0f);
		}
		StartButton.alpha = 0f;
		ScoreButton.alpha = 0f;
		yield return StartCoroutine(CommonUI.Instance.FadeIn(2f));
		Tweener ShadowTweener = null;
		foreach (GameObject text in TitleTexts)
		{
			ShadowTweener = DOTween.To(() => text.GetComponent<Shadow>().effectDistance, (x) => text.GetComponent<Shadow>().effectDistance = x, EffectDistance, 2.5f);
			text.transform.DOBlendableMoveBy(Vector3.down * 150f, 1.5f);
			text.GetComponent<Text>().DOFade(1f, 1.5f);
		}
		ButtonAnimation(StartButton);
		ButtonAnimation(ScoreButton);
		void ButtonAnimation(CanvasGroup Button)
		{
			Button.DOFade(1f, 0.4f);
			Button.transform.localPosition = Button.transform.localPosition.WithX(Button.transform.localPosition.x + 50f);
			Button.transform.DOBlendableLocalMoveBy(Vector3.left * 50f, 0.4f);
		}
		yield return ShadowTweener.WaitForCompletion();
	}

	public GameObject MakeNewCharacterWindow;
	public void OnStartButton()
	{
		bool CharacterExist = sv.FileExists();
		if (!CharacterExist)
		{
			MakeNewCharacterCanvas.SetActive(true);
		}
		else
		{
			sv.LoadGame();
			gm.StartGameType = GameManager.StartGameTypeEnum.LoadGame;
			dc.ChangeDungeonScene(dc.CurrentDungeonData, dc.CurrentFloor);
		}
	}

	public void OnScoreButton()
	{
		StartCoroutine(cu.ShowScorePanel());
	}

	public InputField NewCharacterNameInputField;
	public ToggleGroup NewCharacterRaceToggleGroup;
	public void MakeNewCharacterOK()
	{
		if (string.IsNullOrWhiteSpace(NewCharacterNameInputField.text))
		{
			UIModalBox InvalidNameAlert = UIModalBoxManager.Instance.Create(MakeNewCharacterWindow.gameObject);
			InvalidNameAlert.SetText1("이름을 정해주세요");
			InvalidNameAlert.SetText2("");
			InvalidNameAlert.SetConfirmButtonText("확인");
			InvalidNameAlert.Show();
			return;
		}
		Toggle RaceToggle = NewCharacterRaceToggleGroup.ActiveToggles().FirstOrDefault();
		if (!RaceToggle)
		{
			UIModalBox SelectRaceAlert = UIModalBoxManager.Instance.Create(MakeNewCharacterWindow.gameObject);
			SelectRaceAlert.SetText1("종족이 선택되지 않았습니다");
			SelectRaceAlert.SetText2("");
			SelectRaceAlert.SetConfirmButtonText("확인");
			SelectRaceAlert.Show();
			return;
		}
		p.Name = NewCharacterNameInputField.text;
		p.ThisRaceData = DBManager.Instance.GetRaceByName(RaceToggle.name);
		p.Status.SetStartPlayerStatus(p.ThisRaceData);
		gm.StartGameType = GameManager.StartGameTypeEnum.NewGame;
		dc.ChangeDungeonScene(StartDungeonTheme, 1);
	}

	public void MakeNewCharacterCancel()
	{
		MakeNewCharacterCanvas.SetActive(false);
	}
}
