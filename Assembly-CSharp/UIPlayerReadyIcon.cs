using System;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerReadyIcon : MonoBehaviour
{
	public Image m_readyGlow;

	public Image m_readyDefault;

	public void SetReady(bool isReady)
	{
		this.m_readyGlow.enabled = isReady;
		this.m_readyDefault.enabled = !isReady;
	}
}
