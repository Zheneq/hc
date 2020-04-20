using System;
using TMPro;
using UnityEngine;

public class UINotchedFillBar : MonoBehaviour
{
	public RectTransform[] m_notches;

	public TextMeshProUGUI m_textCount;

	public void Setup(int filled)
	{
		for (int i = 0; i < this.m_notches.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_notches[i], i < filled, null);
		}
		this.m_textCount.text = filled.ToString();
	}
}
