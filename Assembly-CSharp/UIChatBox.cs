using UnityEngine;

public class UIChatBox : UIScene
{
	public UITextConsole m_chatBox;

	public EmoticonPanel m_emoticonPanel;

	public OverconsPanel m_overconsPanel;

	public RectTransform m_InGameChatboxContainer;

	public RectTransform m_FrontEndChatboxContainer;

	public Vector2 m_inGamePosition;

	public Vector2 m_frontEndPosition;

	public Vector2 m_inGameSize;

	public Vector2 m_frontEndSize;

	private static UIChatBox s_instance;

	public static UIChatBox Get()
	{
		return s_instance;
	}

	public static UITextConsole GetChatBox(UIManager.ClientState state)
	{
		if (Get() != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return Get().m_chatBox;
				}
			}
		}
		return null;
	}

	public EmoticonPanel GetCurrentActiveEmoticonPanel()
	{
		return m_emoticonPanel;
	}

	public override void Awake()
	{
		s_instance = this;
		base.Awake();
		m_chatBox.AddHandleMessage();
	}

	public override SceneType GetSceneType()
	{
		return SceneType.ChatBox;
	}

	public override void NotifyGameStateChange(SceneStateParameters newState)
	{
		UIManager.ClientState? newClientGameState = newState.NewClientGameState;
		if (!newClientGameState.HasValue)
		{
			return;
		}
		UIManager.ClientState? newClientGameState2 = newState.NewClientGameState;
		if (newClientGameState2.HasValue)
		{
			UIManager.ClientState? newClientGameState3 = newState.NewClientGameState;
			if (newClientGameState3.Value == UIManager.ClientState.InFrontEnd)
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
				m_chatBox.transform.SetParent(m_FrontEndChatboxContainer.transform);
				(m_chatBox.transform as RectTransform).anchoredPosition = m_frontEndPosition;
				(m_chatBox.transform as RectTransform).sizeDelta = m_frontEndSize;
			}
			else
			{
				UIManager.ClientState? newClientGameState4 = newState.NewClientGameState;
				if (newClientGameState4.Value == UIManager.ClientState.InGame)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					m_chatBox.transform.SetParent(m_InGameChatboxContainer.transform);
					(m_chatBox.transform as RectTransform).anchoredPosition = m_inGamePosition;
					(m_chatBox.transform as RectTransform).sizeDelta = m_inGameSize;
				}
			}
			m_chatBox.UpdateGameState();
			m_overconsPanel.UpdateGameState();
		}
		m_emoticonPanel.SetPanelOpen(false);
		m_overconsPanel.SetPanelOpen(false);
	}
}
