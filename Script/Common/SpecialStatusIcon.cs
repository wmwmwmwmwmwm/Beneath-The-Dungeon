using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SpecialStatusIcon : MonoBehaviour
{
	[ReadOnly] public SpecialStatusData SpecialStatus;
	public Image IconImage;
	public Image IconTextBackground;
	public Text IconText;

	public void SetDurationText(int Duration)
	{
		IconTextBackground.gameObject.SetActive(Duration >= 0);
		IconText.gameObject.SetActive(Duration >= 0);
		IconText.text = Duration.ToString();
	}
}
