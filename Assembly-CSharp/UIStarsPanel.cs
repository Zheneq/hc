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
		this.m_clickable = true;
	}

	private void Update()
	{
		bool flag;
		if (ClientGameManager.Get().GroupInfo != null)
		{
			if (ClientGameManager.Get().GroupInfo.InAGroup)
			{
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
			return;
		}
		bool flag = true;
		bool flag2 = GameManager.Get().QueueInfo != null && GameManager.Get().QueueInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.AntiSocial);
		bool flag3;
		if (ClientGameManager.Get().GroupInfo != null)
		{
			if (ClientGameManager.Get().GroupInfo.InAGroup)
			{
				flag3 = !ClientGameManager.Get().GroupInfo.IsLeader;
				goto IL_A0;
			}
		}
		flag3 = false;
		IL_A0:
		bool flag4 = flag3;
		if (flag4)
		{
			flag = false;
		}
		else if (flag2)
		{
			if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
			{
				flag = !AppState_GroupCharacterSelect.Get().IsReady();
			}
			else if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
			{
				flag = !AppState_CharacterSelect.IsReady();
			}
		}
		if (flag)
		{
			for (int i = 0; i < 5; i++)
			{
				if (this.m_activeButtons[i].gameObject == data.selectedObject)
				{
					this.SetCurrentValue(i + 1);
				}
				if (this.m_inactiveButtons[i].gameObject == data.selectedObject)
				{
					this.SetCurrentValue(i + 1);
				}
			}
			if (this.m_panelType == UIStarsPanel.StarsPanelType.IndividualBot)
			{
				UICharacterScreen.Get().m_partyListPanel.SelectBotDifficulty(this.CurrentValueAsBotDifficulty);
			}
			else if (this.m_panelType == UIStarsPanel.StarsPanelType.EnemyBots)
			{
				UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					ClientRequestedEnemyBotDifficulty = new int?(this.m_currentValue - 1)
				});
			}
			else if (this.m_panelType == UIStarsPanel.StarsPanelType.TeamBots)
			{
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
				UIManager.SetGameObjectActive(this.m_activeButtons[i], true, null);
				UIManager.SetGameObjectActive(this.m_inactiveButtons[i], false, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_activeButtons[i], false, null);
				UIManager.SetGameObjectActive(this.m_inactiveButtons[i], true, null);
			}
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
				return BotDifficulty.Stupid;
			}
			if (this.m_currentValue > 5)
			{
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
