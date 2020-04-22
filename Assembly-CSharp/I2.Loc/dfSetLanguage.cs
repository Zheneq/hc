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
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				LocalizationManager.CurrentLanguage = Language;
				return;
			}
		}
	}
}
