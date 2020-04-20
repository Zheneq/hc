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
		UINewUserFlowManager.m_showPlayHighlight = shouldShow;
	}

	public static void MarkSeasonsNew(bool areSeasonsNew)
	{
		UINewUserFlowManager.m_areSeasonsNew = areSeasonsNew;
	}

	public static void OnNavBarDisplayed()
	{
		UINewUserFlowManager.Highlight(UINewUserHighlightsController.DisplayState.PlayButton, UINewUserFlowManager.m_showPlayHighlight);
	}

	public static void OnGameModeButtonDisplayed()
	{
		UINewUserFlowManager.Unhighlight(UINewUserFlowManager.m_showPlayHighlight);
		UINewUserFlowManager.m_showPlayHighlight = false;
	}

	public static void OnCharacterSelectDisplayed()
	{
		UINewUserFlowManager.Unhighlight(UINewUserFlowManager.m_showPlayHighlight);
	}

	public static void OnDoneWithReadyButton()
	{
	}

	public static void OnHasLootMatrix()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumLootMatrixesOpened;
		int uistate = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
		UINewUserFlowManager.Highlight(UINewUserHighlightsController.DisplayState.LootMatrixNavButton, uistate == 0);
	}

	public static void OnLootMatrixScreenVisible()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumLootMatrixesOpened;
		int uistate = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
		UINewUserFlowManager.Unhighlight(uistate == 0);
	}

	public static void OnMainLootMatrixDisplayed()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumLootMatrixesOpened;
		int uistate = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
		UINewUserFlowManager.Highlight(UINewUserHighlightsController.DisplayState.MainLootMatrixOpenButton, uistate == 0);
	}

	public static void OnSpecialLootMatrixDisplayed()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumLootMatrixesOpened;
		int uistate = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
		UINewUserFlowManager.Highlight(UINewUserHighlightsController.DisplayState.SpecialLootMatrixOpenButton, uistate == 0);
	}

	public static void OnMainLootMatrixOpenClicked()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumLootMatrixesOpened;
		int uistate = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
		UINewUserFlowManager.Unhighlight(uistate == 0);
	}

	public static void OnFreelancerCurrencyOwned()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.HasViewedFluxHighlight;
		bool flag = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState) > 0;
		UINewUserFlowManager.Highlight(UINewUserHighlightsController.DisplayState.FluxEarned, !flag);
	}

	public static void OnFreelancerTokenOwned()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.HasViewedFreelancerTokenHighlight;
		bool flag = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState) > 0;
		UINewUserFlowManager.Highlight(UINewUserHighlightsController.DisplayState.FreelancerToken1, !flag);
	}

	public static void OnGGBoostOwned()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.HasViewedGGHighlight;
		bool flag = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState) > 0;
		UINewUserFlowManager.Highlight(UINewUserHighlightsController.DisplayState.GGEarned, !flag);
	}

	public static void OnDailyMissionsViewed()
	{
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.NumDailiesChosen;
		UINewUserFlowManager.m_areQuestsNew = (ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState) == 0);
		UINewUserFlowManager.Highlight(UINewUserHighlightsController.DisplayState.DailyContracts, UINewUserFlowManager.m_areQuestsNew);
	}

	public static void OnDailyMissionsSelected()
	{
		UINewUserFlowManager.Highlight(UINewUserHighlightsController.DisplayState.DailyContractsPowerUp, UINewUserFlowManager.m_areQuestsNew);
	}

	public static void OnDailyMissionsClosed()
	{
		UINewUserFlowManager.m_areQuestsNew = false;
	}

	public static void OnTutorialSeasonEnded()
	{
		UINewUserFlowManager.Highlight(UINewUserHighlightsController.DisplayState.ChapterInfoButton, UINewUserFlowManager.m_areSeasonsNew);
	}

	public static void OnChapterMoreInfoClicked()
	{
		UINewUserFlowManager.Unhighlight(UINewUserFlowManager.m_areSeasonsNew);
	}

	public static void OnChapterMoreInfoClosed()
	{
		UINewUserFlowManager.Highlight(UINewUserHighlightsController.DisplayState.SeasonNavButton, UINewUserFlowManager.m_areSeasonsNew);
	}

	public static void OnSeasonsTabClicked()
	{
		UINewUserFlowManager.Highlight(UINewUserHighlightsController.DisplayState.SeasonsChapters, UINewUserFlowManager.m_areSeasonsNew);
	}

	public static UINewUserHighlightsController GetController()
	{
		return UINewUserHighlightsController.Get();
	}

	public static void HighlightQueued()
	{
		if (UINewUserFlowManager.m_queuedState != UINewUserHighlightsController.DisplayState.None)
		{
			UINewUserFlowManager.Highlight(UINewUserFlowManager.m_queuedState, true);
		}
	}

	public static void HideDisplay()
	{
		UINewUserHighlightsController controller = UINewUserFlowManager.GetController();
		if (controller)
		{
			controller.HideDisplay();
		}
	}

	private static bool IsDebugMode()
	{
		UINewUserHighlightsController controller = UINewUserFlowManager.GetController();
		if (controller)
		{
			return controller.m_debugMode;
		}
		return false;
	}

	private static void Highlight(UINewUserHighlightsController.DisplayState state, bool shouldShow)
	{
		if ((shouldShow && !UINewUserFlowManager.m_shown[(int)state]) || UINewUserFlowManager.IsDebugMode())
		{
			if (!(UINewUserFlowManager.GetController() == null))
			{
				if (UITutorialSeasonInterstitial.Get() != null)
				{
					if (UITutorialSeasonInterstitial.Get().IsVisible())
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							goto IL_71;
						}
					}
				}
				UINewUserFlowManager.GetController().SetDesiredDisplay(state);
				UINewUserFlowManager.m_queuedState = UINewUserHighlightsController.DisplayState.None;
				UINewUserFlowManager.m_shown[(int)state] = true;
				return;
			}
			IL_71:
			UINewUserFlowManager.m_queuedState = state;
		}
	}

	private static void Unhighlight(bool shouldShow)
	{
		UINewUserHighlightsController controller = UINewUserFlowManager.GetController();
		if (!shouldShow)
		{
			if (!UINewUserFlowManager.IsDebugMode())
			{
				return;
			}
		}
		if (controller)
		{
			controller.SetDesiredDisplay(UINewUserHighlightsController.DisplayState.None);
		}
	}
}
