using UnityEngine;
using UnityEngine.UI;

public class UIAbilityChargeCounter : MonoBehaviour
{
	public Image[] m_ticks;

	public void SetTick(int tickCount)
	{
		int num = Mathf.Clamp(tickCount, 0, m_ticks.Length - 1);
		for (int i = 0; i < m_ticks.Length; i++)
		{
			UIManager.SetGameObjectActive(m_ticks[i], i == num);
		}
		while (true)
		{
			return;
		}
	}
}
