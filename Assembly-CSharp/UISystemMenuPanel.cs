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
		return SceneType.symbol_001D;
	}

	public override void Awake()
	{
		UISystemMenuPanel.s_instance = this;
		if (this.m_debugButton != null)
		{
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
			if (HydrogenConfig.Get().DevMode)
			{
				this.m_debugButton.gameObject.SetActive(true);
			}
			else
			{
				this.m_debugButton.gameObject.SetActive(false);
			}
		}
	}
}
