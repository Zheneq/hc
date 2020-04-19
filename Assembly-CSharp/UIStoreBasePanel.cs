using System;
using UnityEngine;

public class UIStoreBasePanel : MonoBehaviour
{
	public Animator m_animatorController;

	public UIStorePanel.StorePanelScreen ScreenType { get; set; }

	public void SetVisible(bool visible)
	{
		if (visible)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStoreBasePanel.SetVisible(bool)).MethodHandle;
			}
			UIManager.SetGameObjectActive(base.gameObject, true, null);
		}
		else if (this.m_animatorController.gameObject.activeInHierarchy)
		{
			this.m_animatorController.Play("StorePanelDefaultOUT");
			this.OnHidden();
		}
	}

	protected virtual void OnHidden()
	{
	}
}
