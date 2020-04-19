using System;
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
		return UIChatBox.s_instance;
	}

	public static UITextConsole GetChatBox(UIManager.ClientState state)
	{
		if (UIChatBox.Get() != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIChatBox.GetChatBox(UIManager.ClientState)).MethodHandle;
			}
			return UIChatBox.Get().m_chatBox;
		}
		return null;
	}

	public EmoticonPanel GetCurrentActiveEmoticonPanel()
	{
		return this.m_emoticonPanel;
	}

	public override void Awake()
	{
		UIChatBox.s_instance = this;
		base.Awake();
		this.m_chatBox.AddHandleMessage();
	}

	public override SceneType GetSceneType()
	{
		return SceneType.ChatBox;
	}

	public override void NotifyGameStateChange(SceneStateParameters newState)
	{
		UIManager.ClientState? newClientGameState = newState.NewClientGameState;
		if (newClientGameState != null)
		{
			UIManager.ClientState? newClientGameState2 = newState.NewClientGameState;
			if (newClientGameState2 != null)
			{
				UIManager.ClientState? newClientGameState3 = newState.NewClientGameState;
				if (newClientGameState3.Value == UIManager.ClientState.InFrontEnd)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIChatBox.NotifyGameStateChange(SceneStateParameters)).MethodHandle;
					}
					this.m_chatBox.transform.SetParent(this.m_FrontEndChatboxContainer.transform);
					(this.m_chatBox.transform as RectTransform).anchoredPosition = this.m_frontEndPosition;
					(this.m_chatBox.transform as RectTransform).sizeDelta = this.m_frontEndSize;
				}
				else
				{
					UIManager.ClientState? newClientGameState4 = newState.NewClientGameState;
					if (newClientGameState4.Value == UIManager.ClientState.InGame)
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
						this.m_chatBox.transform.SetParent(this.m_InGameChatboxContainer.transform);
						(this.m_chatBox.transform as RectTransform).anchoredPosition = this.m_inGamePosition;
						(this.m_chatBox.transform as RectTransform).sizeDelta = this.m_inGameSize;
					}
				}
				this.m_chatBox.UpdateGameState();
				this.m_overconsPanel.UpdateGameState();
			}
			this.m_emoticonPanel.SetPanelOpen(false);
			this.m_overconsPanel.SetPanelOpen(false);
		}
	}
}
