using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISystemMenuPanel : UIScene
{
	public Button m_debugButton;

	private static UISystemMenuPanel s_instance;

	public static UISystemMenuPanel Get()
	{
		return UISystemMenuPanel.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.\u001D;
	}

	public override void Awake()
	{
		UISystemMenuPanel.s_instance = this;
		if (this.m_debugButton != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISystemMenuPanel.Awake()).MethodHandle;
			}
			UIEventTriggerUtils.AddListener(this.m_debugButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnDebugClick));
		}
		base.Awake();
	}

	private void OnDestroy()
	{
		UISystemMenuPanel.s_instance = null;
	}

	private void OnDebugClick(BaseEventData data)
	{
		if (UIDebugMenu.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISystemMenuPanel.OnDebugClick(BaseEventData)).MethodHandle;
			}
			bool flag = !UIDebugMenu.Get().m_container.gameObject.activeSelf;
			if (flag)
			{
				UIDebugMenu.Get().ResetIfNeeded();
			}
			UIManager.SetGameObjectActive(UIDebugMenu.Get().m_container, flag, null);
		}
	}

	private void Update()
	{
		if (this.m_debugButton != null)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISystemMenuPanel.Update()).MethodHandle;
			}
			if (HydrogenConfig.Get().DevMode)
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
				this.m_debugButton.gameObject.SetActive(true);
			}
			else
			{
				this.m_debugButton.gameObject.SetActive(false);
			}
		}
	}
}
