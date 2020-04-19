using System;
using System.Collections.Generic;
using I2.Loc;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestOfferPanel : UIScene
{
	private QuestOfferNotification m_quests;

	public Animator m_animator;

	public TextMeshProUGUI m_resetString;

	public _SelectableBtn m_questListButton;

	public GridLayoutGroup m_questOffersGroup;

	public TextMeshProUGUI m_currentContractCount;

	private static QuestOfferPanel s_instance;

	private bool m_initialized;

	private QuestOfferPanel.DisplayStates m_displayState;

	public static QuestOfferPanel Get()
	{
		return QuestOfferPanel.s_instance;
	}

	public QuestOfferPanel.DisplayStates DisplayState
	{
		get
		{
			return this.m_displayState;
		}
	}

	public override SceneType GetSceneType()
	{
		return SceneType.QuestOffer;
	}

	public override void Awake()
	{
		QuestOfferPanel.s_instance = this;
		this.m_displayState = QuestOfferPanel.DisplayStates.None;
		this.m_questListButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.QuestListButtonClicked);
		this.m_initialized = false;
		UIManager.SetGameObjectActive(this.m_animator, false, null);
		ClientGameManager.Get().OnAccountDataUpdated += this.HandleAccountDataUpdated;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			this.HandleAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
		}
		base.Awake();
	}

	private void Setup()
	{
		this.m_initialized = true;
		int num = 0;
		if (ClientGameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOfferPanel.Setup()).MethodHandle;
			}
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
				PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
				if (playerAccountData != null)
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
					using (Dictionary<int, QuestProgress>.Enumerator enumerator = playerAccountData.QuestComponent.Progress.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<int, QuestProgress> keyValuePair = enumerator.Current;
							int key = keyValuePair.Key;
							if (QuestWideData.Get().IsDailyQuest(key))
							{
								num++;
							}
						}
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
			}
		}
		this.m_currentContractCount.text = Convert.ToString(num);
	}

	private void OnDestroy()
	{
		if (this.m_initialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOfferPanel.OnDestroy()).MethodHandle;
			}
			if (ClientGameManager.Get() != null)
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
				ClientGameManager.Get().OnAccountDataUpdated -= this.HandleAccountDataUpdated;
			}
		}
	}

	private void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		int num = 0;
		using (Dictionary<int, QuestProgress>.Enumerator enumerator = accountData.QuestComponent.Progress.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, QuestProgress> keyValuePair = enumerator.Current;
				int key = keyValuePair.Key;
				if (QuestWideData.Get().IsDailyQuest(key))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOfferPanel.HandleAccountDataUpdated(PersistedAccountData)).MethodHandle;
					}
					num++;
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_currentContractCount.text = Convert.ToString(num);
	}

	public void NotifyOfferClicked(QuestOffer offerClicked)
	{
		QuestOffer[] componentsInChildren = this.m_questOffersGroup.GetComponentsInChildren<QuestOffer>(true);
		bool flag = false;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Clickable = false;
			if (componentsInChildren[i] != offerClicked)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOfferPanel.NotifyOfferClicked(QuestOffer)).MethodHandle;
				}
				bool flag2;
				if (!componentsInChildren[i].NotifyRejectedQuest())
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
					flag2 = flag;
				}
				else
				{
					flag2 = true;
				}
				flag = flag2;
			}
			else
			{
				UINewUserHighlightsController.Get().SetDailyMissionSelected(i);
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumDailiesChosen;
		int uistate = clientGameManager.GetPlayerAccountData().AccountComponent.GetUIState(uiState);
		clientGameManager.RequestUpdateUIState(uiState, uistate + 1, null);
		if (uistate == 0)
		{
			UINewUserFlowManager.OnDailyMissionsSelected();
		}
		if (!flag)
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
			this.SetVisible(false);
		}
	}

	public void QuestListButtonClicked(BaseEventData data)
	{
		if (QuestListPanel.Get().DisplayState != QuestListPanel.DisplayStates.FadeIn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOfferPanel.QuestListButtonClicked(BaseEventData)).MethodHandle;
			}
			if (QuestListPanel.Get().DisplayState != QuestListPanel.DisplayStates.Idle)
			{
				this.m_questListButton.SetSelected(true, false, string.Empty, string.Empty);
				QuestListPanel.Get().SetVisible(true, false, false);
				return;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_questListButton.SetSelected(false, false, string.Empty, string.Empty);
		QuestListPanel.Get().SetVisible(false, false, false);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
	}

	public void SetVisible(bool visible)
	{
		if (visible)
		{
			this.ShowDailyQuests(this.m_quests);
		}
		else if (UINewUserHighlightsController.Get().GetDisplayState() != UINewUserHighlightsController.DisplayState.DailyContractsPowerUp)
		{
			this.StartFadeOut();
			UINewUserFlowManager.OnDailyMissionsClosed();
		}
	}

	public void ShowDailyQuests(QuestOfferNotification quests)
	{
		if (!this.m_initialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOfferPanel.ShowDailyQuests(QuestOfferNotification)).MethodHandle;
			}
			this.Setup();
		}
		this.m_quests = quests;
		this.StartFadeIn();
		UINewUserFlowManager.OnDailyMissionsViewed();
		string text = string.Empty;
		string text2 = StringUtil.TR("Pacific", "Global");
		HydrogenConfig hydrogenConfig = HydrogenConfig.Get();
		string accountCurrency = hydrogenConfig.Ticket.AccountCurrency;
		if (!(accountCurrency == "GBP"))
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
			if (!(accountCurrency == "EUR"))
			{
				goto IL_99;
			}
		}
		text2 = StringUtil.TR("GMT", "Global");
		IL_99:
		if (LocalizationManager.CurrentLanguageCode == "en")
		{
			string arg = (QuestWideData.Get().m_questResetHour % 0xC).ToString();
			string text3;
			if (QuestWideData.Get().m_questResetHour < 0xC)
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
				text3 = StringUtil.TR("AM", "Global");
			}
			else
			{
				text3 = StringUtil.TR("PM", "Global");
			}
			string arg2 = text3;
			text = string.Format(StringUtil.TR("DailyContractReset", "Quests"), arg, arg2, text2);
		}
		else
		{
			int questResetHour = QuestWideData.Get().m_questResetHour;
			string arg3 = questResetHour.ToString() + ":00";
			text = string.Format(StringUtil.TR("DailyContractReset24Hour", "Quests"), arg3, text2);
		}
		this.m_resetString.text = text;
		QuestOffer[] componentsInChildren = this.m_questOffersGroup.GetComponentsInChildren<QuestOffer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].Clickable = true;
			if (i < quests.DailyQuestIds.Count)
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
				UIManager.SetGameObjectActive(componentsInChildren[i], true, null);
				int rejectedCount = 0;
				if (quests.RejectedQuestCount.ContainsKey(quests.DailyQuestIds[i]))
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
					rejectedCount = quests.RejectedQuestCount[quests.DailyQuestIds[i]];
				}
				componentsInChildren[i].SetupDailyQuest(quests.DailyQuestIds[i], rejectedCount);
			}
			else
			{
				UIManager.SetGameObjectActive(componentsInChildren[i], false, null);
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void StartFadeIn()
	{
		QuestListPanel.Get().SetVisible(false, false, false);
		UIManager.SetGameObjectActive(this.m_animator, true, null);
		this.m_displayState = QuestOfferPanel.DisplayStates.FadeIn;
		this.m_animator.Play("PickContractDefaultIN");
	}

	private void StartIdle()
	{
		this.m_displayState = QuestOfferPanel.DisplayStates.Idle;
		this.m_animator.Play("PickContractDefaultIDLE");
	}

	private void StartFadeOut()
	{
		if (!UIFrontEnd.Get().m_frontEndNavPanel.m_notificationsBtn.IsSelected())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOfferPanel.StartFadeOut()).MethodHandle;
			}
			UIFrontEnd.Get().m_frontEndNavPanel.m_notificationsBtn.SetSelected(true, false, string.Empty, string.Empty);
		}
		QuestListPanel.Get().SetVisible(true, false, false);
		this.m_displayState = QuestOfferPanel.DisplayStates.FadeOut;
		this.m_animator.Play("PickContractDefaultOUT");
	}

	public void PickDailyQuest(int questId)
	{
		if (this.m_quests.DailyQuestIds.Contains(questId))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOfferPanel.PickDailyQuest(int)).MethodHandle;
			}
			this.StartFadeOut();
		}
	}

	private void Update()
	{
		if (this.m_displayState == QuestOfferPanel.DisplayStates.FadeIn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOfferPanel.Update()).MethodHandle;
			}
			if (this.IsAnimationDone(this.m_animator, "PickContractDefaultIN"))
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
				this.StartIdle();
			}
		}
		if (this.m_displayState == QuestOfferPanel.DisplayStates.FadeOut && this.IsAnimationDone(this.m_animator, "PickContractDefaultOUT"))
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
			UIManager.SetGameObjectActive(this.m_animator, false, null);
			this.m_displayState = QuestOfferPanel.DisplayStates.None;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestOfferPanel.IsAnimationDone(Animator, string)).MethodHandle;
			}
			return false;
		}
		AnimatorClipInfo animatorClipInfo = currentAnimatorClipInfo[0];
		AnimationClip clip = animatorClipInfo.clip;
		if (clip == null)
		{
			return false;
		}
		if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
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
			return false;
		}
		return !(clip.name != animName);
	}

	public bool IsActive()
	{
		return this.m_animator.isActiveAndEnabled;
	}

	public enum DisplayStates
	{
		None,
		FadeIn,
		Idle,
		FadeOut
	}
}
