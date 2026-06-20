using UnityEngine;
using UnityEngine.UI;

public class UIPlayerReadyIcon : MonoBehaviour
{
	public Image m_readyGlow;

	public Image m_readyDefault;

	public void SetReady(bool isReady)
	{
		m_readyGlow.enabled = isReady;
		m_readyDefault.enabled = !isReady;
	}
}
