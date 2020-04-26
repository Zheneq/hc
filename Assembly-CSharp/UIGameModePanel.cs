using System;
using UnityEngine;
using UnityEngine.UI;

public class UIGameModePanel : MonoBehaviour
{
	public Text m_infoLabel;

	protected Func<string> m_getInfoString;

	public void Setup(Func<string> getInfoString)
	{
		m_getInfoString = getInfoString;
		(base.transform as RectTransform).sizeDelta = Vector2.zero;
		(base.transform as RectTransform).anchoredPosition = Vector2.zero;
	}

	private void Update()
	{
		if (m_getInfoString == null)
		{
			return;
		}
		string text = m_getInfoString();
		if (!(text != m_infoLabel.text))
		{
			return;
		}
		while (true)
		{
			m_infoLabel.text = text;
			return;
		}
	}
}
