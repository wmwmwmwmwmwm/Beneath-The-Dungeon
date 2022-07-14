using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationManager : Singleton<LocalizationManager>
{
	public enum LanguageEnum { Korean, English, Chinese }
	public List<GameObject> PrefabsToChange;
	[System.Serializable]
	public class FontSet
	{
		public LanguageEnum Language;
		public Font LanguageFont;
	}
	public List<FontSet> FontSetList;
	LanguageEnum CurrentLanguage;

	public void ChangeLanguage(LanguageEnum _Language)
	{
		CurrentLanguage = _Language;
		// todo
	}


}
