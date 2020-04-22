using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStarsPanel : MonoBehaviour
{
	public enum StarsPanelType
	{
		IndividualBot,
		EnemyBots,
		TeamBots
	}

	public Button[] m_activeButtons;

	public Button[] m_inactiveButtons;

	private int m_currentValue;

	public StarsPanelType m_panelType;

	private bool m_clickable;

	public bool Clickable => m_clickable;

	public bool IsCurrentValueValid
	{
		get
		{
			int result;
			if (m_currentValue >= 1)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				result = ((m_currentValue <= 5) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}
	}

	public BotDifficulty CurrentValueAsBotDifficulty
	{
		get
		{
			if (m_currentValue < 1)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return BotDifficulty.Stupid;
					}
				}
			}
			if (m_currentValue > 5)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return BotDifficulty.Expert;
					}
				}
			}
			return (BotDifficulty)(m_currentValue - 1);
		}
	}

	private void Start()
	{
		for (int i = 0; i < 5; i++)
		{
			UIEventTriggerUtils.AddListener(m_activeButtons[i].gameObject, EventTriggerType.PointerClick, OnButtonClicked);
			UIEventTriggerUtils.AddListener(m_inactiveButtons[i].gameObject, EventTriggerType.PointerClick, OnButtonClicked);
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_clickable = true;
			return;
		}
	}

	private void Update()
	{
		int num;
		if (ClientGameManager.Get().GroupInfo != null)
		{
			while (true)
			{
				switch (5)
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
			if (ClientGameManager.Get().GroupInfo.InAGroup)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				num = ((!ClientGameManager.Get().GroupInfo.IsLeader) ? 1 : 0);
				goto IL_0059;
			}
		}
		num = 0;
		goto IL_0059;
		IL_0059:
		if (num != 0)
		{
			GetComponent<CanvasGroup>().alpha = 0.4f;
		}
		else
		{
			GetComponent<CanvasGroup>().alpha = 1f;
		}
	}

	public void SetClickable(bool clickable)
	{
		if (m_clickable != clickable)
		{
			m_clickable = clickable;
			for (int i = 0; i < 5; i++)
			{
				m_activeButtons[i].interactable = clickable;
				m_inactiveButtons[i].interactable = clickable;
			}
		}
	}

	private void OnButtonClicked(BaseEventData data)
	{
		if (!m_clickable)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		bool flag = true;
		bool flag2 = GameManager.Get().QueueInfo != null && GameManager.Get().QueueInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial);
		int num;
		if (ClientGameManager.Get().GroupInfo != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (ClientGameManager.Get().GroupInfo.InAGroup)
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
				num = ((!ClientGameManager.Get().GroupInfo.IsLeader) ? 1 : 0);
				goto IL_00a0;
			}
		}
		num = 0;
		goto IL_00a0;
		IL_00a0:
		if (num != 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			flag = false;
		}
		else if (flag2)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				flag = !AppState_GroupCharacterSelect.Get().IsReady();
			}
			else if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				flag = !AppState_CharacterSelect.IsReady();
			}
		}
		if (!flag)
		{
			return;
		}
		for (int i = 0; i < 5; i++)
		{
			if (m_activeButtons[i].gameObject == data.selectedObject)
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
				SetCurrentValue(i + 1);
			}
			if (m_inactiveButtons[i].gameObject == data.selectedObject)
			{
				SetCurrentValue(i + 1);
			}
		}
		if (m_panelType == StarsPanelType.IndividualBot)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					UICharacterScreen.Get().m_partyListPanel.SelectBotDifficulty(CurrentValueAsBotDifficulty);
					return;
				}
			}
		}
		if (m_panelType == StarsPanelType.EnemyBots)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
					{
						ClientRequestedEnemyBotDifficulty = m_currentValue - 1
					});
					return;
				}
			}
		}
		if (m_panelType != StarsPanelType.TeamBots)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				ClientRequestedAllyBotDifficulty = m_currentValue - 1
			});
			return;
		}
	}

	public void SetCurrentValue(int newValue)
	{
		m_currentValue = newValue;
		for (int i = 0; i < 5; i++)
		{
			if (i < newValue)
			{
				while (true)
				{
					switch (7)
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
				UIManager.SetGameObjectActive(m_activeButtons[i], true);
				UIManager.SetGameObjectActive(m_inactiveButtons[i], false);
			}
			else
			{
				UIManager.SetGameObjectActive(m_activeButtons[i], false);
				UIManager.SetGameObjectActive(m_inactiveButtons[i], true);
			}
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public int GetCurrentValue()
	{
		return m_currentValue;
	}
}
