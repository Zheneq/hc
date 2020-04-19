using System;
using UnityEngine;

namespace I2.Loc
{
	public class CallbackNotification : MonoBehaviour
	{
		public void OnModifyLocalization()
		{
			if (string.IsNullOrEmpty(Localize.MainTranslation))
			{
				return;
			}
			string termTranslation = LocalizationManager.GetTermTranslation("Color/Red");
			Localize.MainTranslation = Localize.MainTranslation.Replace("{PLAYER_COLOR}", termTranslation);
		}
	}
}
