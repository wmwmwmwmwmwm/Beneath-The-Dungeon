using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StatGaugeGroup : MonoBehaviour
{
	public Image HPImage, MPImage, SPImage, ElementalTypeIconImage;
	public Text LevelText, HPText, MPText, SPText, ArmorText;
	public Text STRText, DEXText, INTText, CONText;
	public GameObject SpecialStatusIconPrefab;
	public Transform SpecialStatusIconParent;
	IntRef DisplayedHP, DisplayedMP, DisplayedSP;
	float AnimationTime = 0.3f;

	void Awake()
	{
		DisplayedHP = new IntRef();
		DisplayedMP = new IntRef();
		DisplayedSP = new IntRef();
	}

	public void UpdateAllStatusImmediate(CommonStatus Status, bool ResetStatColor)
	{
		LevelText.text = Status.Level.ToString();
		ElementalHelper.SetElementalIconImage(ElementalTypeIconImage, Status.ElementalType);
		UpdateStatValues(Status, ResetStatColor);
		UpdateTextImmediate(HPImage, HPText, DisplayedHP, Status.CurrentHP, Status.MaxHP);
		UpdateTextImmediate(MPImage, MPText, DisplayedMP, Status.CurrentMP, Status.MaxMP);
		UpdateTextImmediate(SPImage, SPText, DisplayedSP, Status.CurrentSP, Status.MaxSP);
		void UpdateTextImmediate(Image GaugeImage, Text GaugeText, IntRef DisplayedValue, int EndValue, int MaxValue)
		{
			EndValue = Mathf.Max(0, EndValue);
			float FillAmount = MaxValue <= 0 ? 0f : (float)EndValue / MaxValue;
			GaugeImage.fillAmount = FillAmount;
			GaugeText.text = string.Format("{0} / {1}", EndValue, MaxValue);
			DisplayedValue.Value = EndValue;
		}
	}

	/// <summary>
	/// CompareStatus는 4개 능력치 변화할때만 사용
	/// </summary>
	public IEnumerator UpdateAllStatus(CommonStatus Status, CommonStatus CompareStatus) 
	{
		ElementalHelper.SetElementalIconImage(ElementalTypeIconImage, Status.ElementalType);
		UpdateStatValues(Status, CompareStatus, false);
		StartCoroutine(UpdateText(HPImage, HPText, DisplayedHP, Status.CurrentHP, Status.MaxHP));
		StartCoroutine(UpdateText(MPImage, MPText, DisplayedMP, Status.CurrentMP, Status.MaxMP));
		yield return StartCoroutine(UpdateText(SPImage, SPText, DisplayedSP, Status.CurrentSP, Status.MaxSP));
		IEnumerator UpdateText(Image GaugeImage, Text GaugeText, IntRef DisplayedValue, int EndValue, int MaxValue)
		{
			EndValue = Mathf.Max(0, EndValue);
			float FillAmount = MaxValue <= 0 ? 0f : (float)EndValue / MaxValue;
			GaugeImage.DOFillAmount(FillAmount, AnimationTime * 0.5f);
			yield return DOTween.To(() => DisplayedValue.Value, (x) => DisplayedValue.Value = x, EndValue, AnimationTime).OnUpdate(() =>
			{
				GaugeText.text = string.Format("{0} / {1}", DisplayedValue.Value, MaxValue);
			}).WaitForCompletion();
		}
	}

	public Color StatColorGreen, StatColorRed, StatColorNormal;
	public void Update4StatValues(CommonStatus Status, CommonStatus CompareStatus) => UpdateStatValues(Status, CompareStatus, false);
	public void UpdateStatValues(CommonStatus Status, bool ResetStatColor) => UpdateStatValues(Status, null, ResetStatColor);
	void UpdateStatValues(CommonStatus Status, CommonStatus CompareStatus, bool ResetStatColor)
	{
		if (CompareStatus != null) 
		{
			UpdateValue(STRText, Status.STR, CompareStatus.STR);
			UpdateValue(DEXText, Status.DEX, CompareStatus.DEX);
			UpdateValue(INTText, Status.INT, CompareStatus.INT);
			UpdateValue(CONText, Status.CON, CompareStatus.CON);
			UpdateValue(ArmorText, Status.Armor, CompareStatus.Armor);
			void UpdateValue(Text TextComponent, int Value, int CompareValue)
			{
				if (Value > CompareValue)
				{
					TextComponent.color = StatColorGreen;
				}
				else if (Value < CompareValue)
				{
					TextComponent.color = StatColorRed;
				}
				else
				{
					TextComponent.color = StatColorNormal;
				}
				TextComponent.text = Value.ToString();
			}
		}
		else
		{
			UpdateValueImmediate(STRText, Status.STR);
			UpdateValueImmediate(DEXText, Status.DEX);
			UpdateValueImmediate(INTText, Status.INT);
			UpdateValueImmediate(CONText, Status.CON);
			UpdateValueImmediate(ArmorText, Status.Armor);
			void UpdateValueImmediate(Text TextComponent, int Value)
			{
				if (ResetStatColor) TextComponent.color = StatColorNormal;
				TextComponent.text = Value.ToString();
			}
		}
	}

	public void ShowSpecialStatusInfo(SpecialStatusIcon SpecialStatusIcon)
	{
		CommonUI.Instance.ShowSpecialStatusInfoPanel(SpecialStatusIcon.SpecialStatus);
	}
}
