using UnityEngine;

namespace I2.Loc
{
	public class CallbackNotification : MonoBehaviour
	{
		public void OnModifyLocalization()
		{
			if (!string.IsNullOrEmpty(Localize.MainTranslation))
			{
				string termTranslation = LocalizationManager.GetTermTranslation("Color/Red");
				Localize.MainTranslation = Localize.MainTranslation.Replace("{PLAYER_COLOR}", termTranslation);
			}
		}
	}
}
