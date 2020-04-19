using System;
using UnityEngine;
using UnityEngine.UI;

public class UIGameModePanel : MonoBehaviour
{
	public Text m_infoLabel;

	protected Func<string> m_getInfoString;

	public void Setup(Func<string> getInfoString)
	{
		this.m_getInfoString = getInfoString;
		(base.transform as RectTransform).sizeDelta = Vector2.zero;
		(base.transform as RectTransform).anchoredPosition = Vector2.zero;
	}

	private void Update()
	{
		if (this.m_getInfoString != null)
		{
			string text = this.m_getInfoString();
			if (text != this.m_infoLabel.text)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameModePanel.Update()).MethodHandle;
				}
				this.m_infoLabel.text = text;
			}
		}
	}
}
