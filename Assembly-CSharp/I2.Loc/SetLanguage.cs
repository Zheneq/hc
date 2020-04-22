using UnityEngine;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/SetLanguage")]
	public class SetLanguage : MonoBehaviour
	{
		public string _Language;

		public LanguageSource mSource;

		private void OnClick()
		{
			ApplyLanguage();
		}

		public void ApplyLanguage()
		{
			if (!LocalizationManager.HasLanguage(_Language))
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				LocalizationManager.CurrentLanguage = _Language;
				return;
			}
		}
	}
}
