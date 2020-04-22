using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISystemMenuPanel : UIScene
{
	public Button m_debugButton;

	private static UISystemMenuPanel s_instance;

	public static UISystemMenuPanel Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType._001D;
	}

	public override void Awake()
	{
		s_instance = this;
		if (m_debugButton != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIEventTriggerUtils.AddListener(m_debugButton.gameObject, EventTriggerType.PointerClick, OnDebugClick);
		}
		base.Awake();
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void OnDebugClick(BaseEventData data)
	{
		if (!(UIDebugMenu.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			bool flag = !UIDebugMenu.Get().m_container.gameObject.activeSelf;
			if (flag)
			{
				UIDebugMenu.Get().ResetIfNeeded();
			}
			UIManager.SetGameObjectActive(UIDebugMenu.Get().m_container, flag);
			return;
		}
	}

	private void Update()
	{
		if (!(m_debugButton != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (HydrogenConfig.Get().DevMode)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						m_debugButton.gameObject.SetActive(true);
						return;
					}
				}
			}
			m_debugButton.gameObject.SetActive(false);
			return;
		}
	}
}
