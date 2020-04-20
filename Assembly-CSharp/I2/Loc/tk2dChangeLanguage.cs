using System;
using UnityEngine;

namespace I2.Loc
{
	public class tk2dChangeLanguage : MonoBehaviour
	{
		public void SetLanguage_English()
		{
			this.SetLanguage("English");
		}

		public void SetLanguage_French()
		{
			this.SetLanguage("French");
		}

		public void SetLanguage_Spanish()
		{
			this.SetLanguage("Spanish");
		}

		public void SetLanguage(string LangName)
		{
			if (LocalizationManager.HasLanguage(LangName, true, true))
			{
				LocalizationManager.CurrentLanguage = LangName;
			}
		}
	}
}
