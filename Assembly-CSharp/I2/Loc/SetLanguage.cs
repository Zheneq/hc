using System;
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
			this.ApplyLanguage();
		}

		public void ApplyLanguage()
		{
			if (LocalizationManager.HasLanguage(this._Language, true, true))
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SetLanguage.ApplyLanguage()).MethodHandle;
				}
				LocalizationManager.CurrentLanguage = this._Language;
			}
		}
	}
}
