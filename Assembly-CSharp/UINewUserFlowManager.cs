using System;

public class UINewUserFlowManager
{
	private static bool[] m_shown = new bool[Enum.GetValues(typeof(UINewUserHighlightsController.DisplayState)).Length];

	private static bool m_showPlayHighlight = false;

	private static bool m_areQuestsNew = false;

	private static bool m_areSeasonsNew = false;

	private static UINewUserHighlightsController.DisplayState m_queuedState = UINewUserHighlightsController.DisplayState.None;

	public static void MarkShowPlayHighlight(bool shouldShow)
	{
		m_showPlayHighlight = shouldShow;
	}

	public static void MarkSeasonsNew(bool areSeasonsNew)
	{
		m_areSeasonsNew = areSeasonsNew;
	}

	public static void OnNavBarDisplayed()
	{
		Highlight(UINewUserHighlightsController.DisplayState.PlayButton, m_showPlayHighlight);
	}

	public static void OnGameModeButtonDisplayed()
	{
		Unhighlight(m_showPlayHighlight);
		m_showPlayHighlight = false;
	}

	public static void OnCharacterSelectDisplayed()
	{
		Unhighlight(m_showPlayHighlight);
	}

	public static void OnDoneWithReadyButton()
	{
	}

	public static void OnHasLootMatrix()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumLootMatrixesOpened;
		int uIState = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
		Highlight(UINewUserHighlightsController.DisplayState.LootMatrixNavButton, uIState == 0);
	}

	public static void OnLootMatrixScreenVisible()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumLootMatrixesOpened;
		int uIState = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
		Unhighlight(uIState == 0);
	}

	public static void OnMainLootMatrixDisplayed()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumLootMatrixesOpened;
		int uIState = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
		Highlight(UINewUserHighlightsController.DisplayState.MainLootMatrixOpenButton, uIState == 0);
	}

	public static void OnSpecialLootMatrixDisplayed()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumLootMatrixesOpened;
		int uIState = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
		Highlight(UINewUserHighlightsController.DisplayState.SpecialLootMatrixOpenButton, uIState == 0);
	}

	public static void OnMainLootMatrixOpenClicked()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumLootMatrixesOpened;
		int uIState = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
		Unhighlight(uIState == 0);
	}

	public static void OnFreelancerCurrencyOwned()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.HasViewedFluxHighlight;
		bool flag = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState) > 0;
		Highlight(UINewUserHighlightsController.DisplayState.FluxEarned, !flag);
	}

	public static void OnFreelancerTokenOwned()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.HasViewedFreelancerTokenHighlight;
		bool flag = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState) > 0;
		Highlight(UINewUserHighlightsController.DisplayState.FreelancerToken1, !flag);
	}

	public static void OnGGBoostOwned()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.HasViewedGGHighlight;
		bool flag = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState) > 0;
		Highlight(UINewUserHighlightsController.DisplayState.GGEarned, !flag);
	}

	public static void OnDailyMissionsViewed()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumDailiesChosen;
		m_areQuestsNew = (ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState) == 0);
		Highlight(UINewUserHighlightsController.DisplayState.DailyContracts, m_areQuestsNew);
	}

	public static void OnDailyMissionsSelected()
	{
		Highlight(UINewUserHighlightsController.DisplayState.DailyContractsPowerUp, m_areQuestsNew);
	}

	public static void OnDailyMissionsClosed()
	{
		m_areQuestsNew = false;
	}

	public static void OnTutorialSeasonEnded()
	{
		Highlight(UINewUserHighlightsController.DisplayState.ChapterInfoButton, m_areSeasonsNew);
	}

	public static void OnChapterMoreInfoClicked()
	{
		Unhighlight(m_areSeasonsNew);
	}

	public static void OnChapterMoreInfoClosed()
	{
		Highlight(UINewUserHighlightsController.DisplayState.SeasonNavButton, m_areSeasonsNew);
	}

	public static void OnSeasonsTabClicked()
	{
		Highlight(UINewUserHighlightsController.DisplayState.SeasonsChapters, m_areSeasonsNew);
	}

	public static UINewUserHighlightsController GetController()
	{
		return UINewUserHighlightsController.Get();
	}

	public static void HighlightQueued()
	{
		if (m_queuedState != 0)
		{
			Highlight(m_queuedState, true);
		}
	}

	public static void HideDisplay()
	{
		UINewUserHighlightsController controller = GetController();
		if (!controller)
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
			controller.HideDisplay();
			return;
		}
	}

	private static bool IsDebugMode()
	{
		UINewUserHighlightsController controller = GetController();
		if ((bool)controller)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return controller.m_debugMode;
				}
			}
		}
		return false;
	}

	private static void Highlight(UINewUserHighlightsController.DisplayState state, bool shouldShow)
	{
		if ((!shouldShow || m_shown[(int)state]) && !IsDebugMode())
		{
			return;
		}
		if (!(GetController() == null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (UITutorialSeasonInterstitial.Get() != null)
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
				if (UITutorialSeasonInterstitial.Get().IsVisible())
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
					goto IL_0071;
				}
			}
			GetController().SetDesiredDisplay(state);
			m_queuedState = UINewUserHighlightsController.DisplayState.None;
			m_shown[(int)state] = true;
			return;
		}
		goto IL_0071;
		IL_0071:
		m_queuedState = state;
	}

	private static void Unhighlight(bool shouldShow)
	{
		UINewUserHighlightsController controller = GetController();
		if (!shouldShow)
		{
			while (true)
			{
				switch (6)
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
			if (!IsDebugMode())
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (!controller)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			controller.SetDesiredDisplay(UINewUserHighlightsController.DisplayState.None);
			return;
		}
	}
}
