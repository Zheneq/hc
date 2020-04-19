using System;
using System.Collections.Generic;
using UnityEngine;

namespace I2.Loc
{
	public class ToggleLanguage : MonoBehaviour
	{
		private void Start()
		{
			base.Invoke("test", 3f);
		}

		private void test()
		{
			List<string> allLanguages = LocalizationManager.GetAllLanguages();
			int num = allLanguages.IndexOf(LocalizationManager.CurrentLanguage);
			if (num >= 0)
			{
				num = (num + 1) % allLanguages.Count;
			}
			base.Invoke("test", 3f);
		}
	}
}
