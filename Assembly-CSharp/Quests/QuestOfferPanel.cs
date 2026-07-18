using I2.Loc;
using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestOfferPanel : UIScene
{
	public enum DisplayStates
	{
		None,
		FadeIn,
		Idle,
		FadeOut
	}

	private QuestOfferNotification m_quests;

	public Animator m_animator;

	public TextMeshProUGUI m_resetString;

	public _SelectableBtn m_questListButton;

	public GridLayoutGroup m_questOffersGroup;

	public TextMeshProUGUI m_currentContractCount;

	private static QuestOfferPanel s_instance;

	private bool m_initialized;

	private DisplayStates m_displayState;

	public DisplayStates DisplayState => m_displayState;

	public static QuestOfferPanel Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.QuestOffer;
	}

	public override void Awake()
	{
		s_instance = this;
		m_displayState = DisplayStates.None;
		m_questListButton.spriteController.callback = QuestListButtonClicked;
		m_initialized = false;
		UIManager.SetGameObjectActive(m_animator, false);
		ClientGameManager.Get().OnAccountDataUpdated += HandleAccountDataUpdated;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			HandleAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
		}
		base.Awake();
	}

	private void Setup()
	{
		m_initialized = true;
		int num = 0;
		if (ClientGameManager.Get() != null)
		{
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
				if (playerAccountData != null)
				{
					using (Dictionary<int, QuestProgress>.Enumerator enumerator = playerAccountData.QuestComponent.Progress.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							int key = enumerator.Current.Key;
							if (QuestWideData.Get().IsDailyQuest(key))
							{
								num++;
							}
						}
					}
				}
			}
		}
		m_currentContractCount.text = Convert.ToString(num);
	}

	private void OnDestroy()
	{
		if (!m_initialized)
		{
			return;
		}
		while (true)
		{
			if (ClientGameManager.Get() != null)
			{
				while (true)
				{
					ClientGameManager.Get().OnAccountDataUpdated -= HandleAccountDataUpdated;
					return;
				}
			}
			return;
		}
	}

	private void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		int num = 0;
		using (Dictionary<int, QuestProgress>.Enumerator enumerator = accountData.QuestComponent.Progress.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int key = enumerator.Current.Key;
				if (QuestWideData.Get().IsDailyQuest(key))
				{
					num++;
				}
			}
		}
		m_currentContractCount.text = Convert.ToString(num);
	}

	public void NotifyOfferClicked(QuestOffer offerClicked)
	{
		QuestOffer[] componentsInChildren = m_questOffersGroup.GetComponentsInChildren<QuestOffer>(true);
		bool flag = false;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Clickable = false;
			if (componentsInChildren[i] != offerClicked)
			{
				int num;
				if (!componentsInChildren[i].NotifyRejectedQuest())
				{
					num = (flag ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				flag = ((byte)num != 0);
			}
			else
			{
				UINewUserHighlightsController.Get().SetDailyMissionSelected(i);
			}
		}
		while (true)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumDailiesChosen;
			int uIState = clientGameManager.GetPlayerAccountData().AccountComponent.GetUIState(uiState);
			clientGameManager.RequestUpdateUIState(uiState, uIState + 1, null);
			if (uIState == 0)
			{
				UINewUserFlowManager.OnDailyMissionsSelected();
			}
			if (!flag)
			{
				while (true)
				{
					SetVisible(false);
					return;
				}
			}
			return;
		}
	}

	public void QuestListButtonClicked(BaseEventData data)
	{
		if (QuestListPanel.Get().DisplayState != QuestListPanel.DisplayStates.FadeIn)
		{
			if (QuestListPanel.Get().DisplayState != QuestListPanel.DisplayStates.Idle)
			{
				m_questListButton.SetSelected(true, false, string.Empty, string.Empty);
				QuestListPanel.Get().SetVisible(true);
				return;
			}
		}
		m_questListButton.SetSelected(false, false, string.Empty, string.Empty);
		QuestListPanel.Get().SetVisible(false);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
	}

	public void SetVisible(bool visible)
	{
		if (visible)
		{
			ShowDailyQuests(m_quests);
		}
		else if (UINewUserHighlightsController.Get().GetDisplayState() != UINewUserHighlightsController.DisplayState.DailyContractsPowerUp)
		{
			StartFadeOut();
			UINewUserFlowManager.OnDailyMissionsClosed();
		}
	}

	public void ShowDailyQuests(QuestOfferNotification quests)
	{
		if (!m_initialized)
		{
			Setup();
		}
		m_quests = quests;
		StartFadeIn();
		UINewUserFlowManager.OnDailyMissionsViewed();
		string empty = string.Empty;
		string text = StringUtil.TR("Pacific", "Global");
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		string accountCurrency = hydrogenConfig.Ticket.AccountCurrency;
		if (!(accountCurrency == "GBP"))
		{
			if (!(accountCurrency == "EUR"))
			{
				goto IL_0099;
			}
		}
		text = StringUtil.TR("GMT", "Global");
		goto IL_0099;
		IL_0099:
		if (LocalizationManager.CurrentLanguageCode == "en")
		{
			string arg = (QuestWideData.Get().m_questResetHour % 12).ToString();
			string text2;
			if (QuestWideData.Get().m_questResetHour < 12)
			{
				text2 = StringUtil.TR("AM", "Global");
			}
			else
			{
				text2 = StringUtil.TR("PM", "Global");
			}
			string arg2 = text2;
			empty = string.Format(StringUtil.TR("DailyContractReset", "Quests"), arg, arg2, text);
		}
		else
		{
			int questResetHour = QuestWideData.Get().m_questResetHour;
			string arg3 = questResetHour + ":00";
			empty = string.Format(StringUtil.TR("DailyContractReset24Hour", "Quests"), arg3, text);
		}
		m_resetString.text = empty;
		QuestOffer[] componentsInChildren = m_questOffersGroup.GetComponentsInChildren<QuestOffer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Clickable = true;
			if (i < quests.DailyQuestIds.Count)
			{
				UIManager.SetGameObjectActive(componentsInChildren[i], true);
				int rejectedCount = 0;
				if (quests.RejectedQuestCount.ContainsKey(quests.DailyQuestIds[i]))
				{
					rejectedCount = quests.RejectedQuestCount[quests.DailyQuestIds[i]];
				}
				componentsInChildren[i].SetupDailyQuest(quests.DailyQuestIds[i], rejectedCount);
			}
			else
			{
				UIManager.SetGameObjectActive(componentsInChildren[i], false);
			}
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void StartFadeIn()
	{
		QuestListPanel.Get().SetVisible(false);
		UIManager.SetGameObjectActive(m_animator, true);
		m_displayState = DisplayStates.FadeIn;
		m_animator.Play("PickContractDefaultIN");
	}

	private void StartIdle()
	{
		m_displayState = DisplayStates.Idle;
		m_animator.Play("PickContractDefaultIDLE");
	}

	private void StartFadeOut()
	{
		if (!UIFrontEnd.Get().m_frontEndNavPanel.m_notificationsBtn.IsSelected())
		{
			UIFrontEnd.Get().m_frontEndNavPanel.m_notificationsBtn.SetSelected(true, false, string.Empty, string.Empty);
		}
		QuestListPanel.Get().SetVisible(true);
		m_displayState = DisplayStates.FadeOut;
		m_animator.Play("PickContractDefaultOUT");
	}

	public void PickDailyQuest(int questId)
	{
		if (!m_quests.DailyQuestIds.Contains(questId))
		{
			return;
		}
		while (true)
		{
			StartFadeOut();
			return;
		}
	}

	private void Update()
	{
		if (m_displayState == DisplayStates.FadeIn)
		{
			if (IsAnimationDone(m_animator, "PickContractDefaultIN"))
			{
				StartIdle();
			}
		}
		if (m_displayState != DisplayStates.FadeOut || !IsAnimationDone(m_animator, "PickContractDefaultOUT"))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_animator, false);
			m_displayState = DisplayStates.None;
			return;
		}
	}

	private bool IsAnimationDone(Animator animator, string animName)
	{
		if (!animator.isInitialized)
		{
			return false;
		}
		AnimatorClipInfo[] currentAnimatorClipInfo = animator.GetCurrentAnimatorClipInfo(0);
		if (currentAnimatorClipInfo.Length == 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		AnimatorClipInfo animatorClipInfo = currentAnimatorClipInfo[0];
		AnimationClip clip = animatorClipInfo.clip;
		if (clip == null)
		{
			return false;
		}
		if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (clip.name != animName)
		{
			return false;
		}
		return true;
	}

	public bool IsActive()
	{
		return m_animator.isActiveAndEnabled;
	}
}
