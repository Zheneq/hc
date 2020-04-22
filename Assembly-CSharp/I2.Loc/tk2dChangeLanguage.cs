using UnityEngine;

namespace I2.Loc
{
	public class tk2dChangeLanguage : MonoBehaviour
	{
		public void SetLanguage_English()
		{
			SetLanguage("English");
		}

		public void SetLanguage_French()
		{
			SetLanguage("French");
		}

		public void SetLanguage_Spanish()
		{
			SetLanguage("Spanish");
		}

		public void SetLanguage(string LangName)
		{
			if (!LocalizationManager.HasLanguage(LangName))
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				LocalizationManager.CurrentLanguage = LangName;
				return;
			}
		}
	}
}
