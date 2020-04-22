using UnityEngine;

namespace I2.Loc
{
	public class dfSetLanguage : MonoBehaviour
	{
		public string Language;

		private void OnClick()
		{
			if (!LocalizationManager.HasLanguage(Language))
			{
				return;
			}
			while (true)
			{
				LocalizationManager.CurrentLanguage = Language;
				return;
			}
		}
	}
}
