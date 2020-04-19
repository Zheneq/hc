using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIStarsPanel : MonoBehaviour
{
	public Button[] m_activeButtons;

	public Button[] m_inactiveButtons;

	private int m_currentValue;

	public UIStarsPanel.StarsPanelType m_panelType;

	private bool m_clickable;

	public bool Clickable
	{
		get
		{
			return this.m_clickable;
		}
	}

	private void Start()
	{
		for (int i = 0; i < 5; i++)
		{
			UIEventTriggerUtils.AddListener(this.m_activeButtons[i].gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnButtonClicked));
			UIEventTriggerUtils.AddListener(this.m_inactiveButtons[i].gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnButtonClicked));
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIStarsPanel.Start()).MethodHandle;
		}
		this.m_clickable = true;
	}

	private void Update()
	{
		bool flag;
		if (ClientGameManager.Get().GroupInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStarsPanel.Update()).MethodHandle;
			}
			if (ClientGameManager.Get().GroupInfo.InAGroup)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				flag = !ClientGameManager.Get().GroupInfo.IsLeader;
				goto IL_59;
			}
		}
		flag = false;
		IL_59:
		bool flag2 = flag;
		if (flag2)
		{
			base.GetComponent<CanvasGroup>().alpha = 0.4f;
		}
		else
		{
			base.GetComponent<CanvasGroup>().alpha = 1f;
		}
	}

	public void SetClickable(bool clickable)
	{
		if (this.m_clickable == clickable)
		{
			return;
		}
		this.m_clickable = clickable;
		for (int i = 0; i < 5; i++)
		{
			this.m_activeButtons[i].interactable = clickable;
			this.m_inactiveButtons[i].interactable = clickable;
		}
	}

	private void OnButtonClicked(BaseEventData data)
	{
		if (!this.m_clickable)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIStarsPanel.OnButtonClicked(BaseEventData)).MethodHandle;
			}
			return;
		}
		bool flag = true;
		bool flag2 = GameManager.Get().QueueInfo != null && GameManager.Get().QueueInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial);
		bool flag3;
		if (ClientGameManager.Get().GroupInfo != null)
		{
			for (;;)
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				flag3 = !ClientGameManager.Get().GroupInfo.IsLeader;
				goto IL_A0;
			}
		}
		flag3 = false;
		IL_A0:
		bool flag4 = flag3;
		if (flag4)
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
			flag = false;
		}
		else if (flag2)
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
			if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
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
				flag = !AppState_GroupCharacterSelect.Get().IsReady();
			}
			else if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
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
				flag = !AppState_CharacterSelect.IsReady();
			}
		}
		if (flag)
		{
			for (int i = 0; i < 5; i++)
			{
				if (this.m_activeButtons[i].gameObject == data.selectedObject)
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
					this.SetCurrentValue(i + 1);
				}
				if (this.m_inactiveButtons[i].gameObject == data.selectedObject)
				{
					this.SetCurrentValue(i + 1);
				}
			}
			if (this.m_panelType == UIStarsPanel.StarsPanelType.IndividualBot)
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
				UICharacterScreen.Get().m_partyListPanel.SelectBotDifficulty(this.CurrentValueAsBotDifficulty);
			}
			else if (this.m_panelType == UIStarsPanel.StarsPanelType.EnemyBots)
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
				UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					ClientRequestedEnemyBotDifficulty = new int?(this.m_currentValue - 1)
				});
			}
			else if (this.m_panelType == UIStarsPanel.StarsPanelType.TeamBots)
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
				UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					ClientRequestedAllyBotDifficulty = new int?(this.m_currentValue - 1)
				});
			}
		}
	}

	public void SetCurrentValue(int newValue)
	{
		this.m_currentValue = newValue;
		for (int i = 0; i < 5; i++)
		{
			if (i < newValue)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIStarsPanel.SetCurrentValue(int)).MethodHandle;
				}
				UIManager.SetGameObjectActive(this.m_activeButtons[i], true, null);
				UIManager.SetGameObjectActive(this.m_inactiveButtons[i], false, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_activeButtons[i], false, null);
				UIManager.SetGameObjectActive(this.m_inactiveButtons[i], true, null);
			}
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
	}

	public int GetCurrentValue()
	{
		return this.m_currentValue;
	}

	public bool IsCurrentValueValid
	{
		get
		{
			bool result;
			if (this.m_currentValue >= 1)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIStarsPanel.get_IsCurrentValueValid()).MethodHandle;
				}
				result = (this.m_currentValue <= 5);
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	public BotDifficulty CurrentValueAsBotDifficulty
	{
		get
		{
			if (this.m_currentValue < 1)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIStarsPanel.get_CurrentValueAsBotDifficulty()).MethodHandle;
				}
				return BotDifficulty.Stupid;
			}
			if (this.m_currentValue > 5)
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
				return BotDifficulty.Expert;
			}
			return (BotDifficulty)(this.m_currentValue - 1);
		}
	}

	public enum StarsPanelType
	{
		IndividualBot,
		EnemyBots,
		TeamBots
	}
}
