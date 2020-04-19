using System;
using UnityEngine;

namespace I2.Loc
{
	public class dfSetLanguage : MonoBehaviour
	{
		public string Language;

		private void OnClick()
		{
			if (LocalizationManager.HasLanguage(this.Language, true, true))
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(dfSetLanguage.OnClick()).MethodHandle;
				}
				LocalizationManager.CurrentLanguage = this.Language;
			}
		}
	}
}
