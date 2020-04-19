using System;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilityChargeCounter : MonoBehaviour
{
	public Image[] m_ticks;

	public void SetTick(int tickCount)
	{
		int num = Mathf.Clamp(tickCount, 0, this.m_ticks.Length - 1);
		for (int i = 0; i < this.m_ticks.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_ticks[i], i == num, null);
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityChargeCounter.SetTick(int)).MethodHandle;
		}
	}
}
