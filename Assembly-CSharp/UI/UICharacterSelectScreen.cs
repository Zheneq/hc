using UnityEngine;
using UnityEngine.EventSystems;

public class UICharacterSelectScreen : MonoBehaviour
{
	public UICharacterSelectSimplePartyList m_simplePartyListPanel;

	public UISpectatorPartyList m_spectatorPartyListPanel;

	private static UICharacterSelectScreen s_instance;

	private UIOneButtonDialog m_selectModsDialog;

	public static UICharacterSelectScreen Get()
	{
		return s_instance;
	}

	public void SelectedGameMode(GameType gameType)
	{
		UICharacterScreen.Get().DoRefreshFunctions(
			(ushort)UICharacterScreen.RefreshFunctionType.RefreshGameSubTypes
			| (ushort)UICharacterScreen.RefreshFunctionType.RefreshSelectedGameType);
	}

	public void Awake()
	{
		s_instance = this;
		if (!(base.gameObject.transform.parent == null))
		{
			return;
		}
		while (true)
		{
			Object.DontDestroyOnLoad(base.gameObject);
			return;
		}
	}

	public bool IsTeamBotsTogged()
	{
		return UICharacterScreen.GetCurrentSpecificState().DisplayAllyBotTeammates;
	}

	public void SetGameSettingsButtonVisibility(bool visible)
	{
		if (!(NavigationBar.Get() != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(NavigationBar.Get().m_gameSettingsBtn, visible);
			return;
		}
	}

	public void ShowGameSettingsPanel(LobbyGameConfig gameConfig, LobbyTeamInfo teamInfo, LobbyPlayerInfo playerInfo)
	{
		UIGameSettingsPanel.Get().SetVisible(true);
		UIGameSettingsPanel.Get().Setup(gameConfig, teamInfo, playerInfo);
		UICharacterSelectRing[] ringAnimations = UICharacterSelectWorldObjects.Get().m_ringAnimations;
		foreach (UICharacterSelectRing uICharacterSelectRing in ringAnimations)
		{
			uICharacterSelectRing.SetClickable(false);
		}
		while (true)
		{
			UICharacterSelectScreenController.Get().UpdateReadyCancelButtonStates();
			return;
		}
	}

	public void AllowCharacterSwapForConflict()
	{
		UICharacterSelectScreenController.Get().AllowCharacterSwapForConflict();
	}

	public void HideGameSettingsPanel(LobbyGameConfig gameConfig)
	{
		UIGameSettingsPanel.Get().SetVisible(false);
		UICharacterSelectRing[] ringAnimations = UICharacterSelectWorldObjects.Get().m_ringAnimations;
		foreach (UICharacterSelectRing uICharacterSelectRing in ringAnimations)
		{
			uICharacterSelectRing.SetClickable(true);
		}
		while (true)
		{
			return;
		}
	}

	public void ShowPleaseEquipModsDialog()
	{
		string description = StringUtil.TR("PleaseSelectYourAbilityMods", "Global");
		if (!(m_selectModsDialog == null))
		{
			return;
		}
		while (true)
		{
			m_selectModsDialog = UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("SelectAbilityMods", "Global"), description, StringUtil.TR("Ok", "Global"));
			return;
		}
	}

	public void SetVisible(bool visible)
	{
		if (!(m_selectModsDialog != null))
		{
			return;
		}
		while (true)
		{
			UIDialogPopupManager.Get().CloseDialog(m_selectModsDialog);
			return;
		}
	}

	public void Setup(LobbyGameConfig gameConfig, LobbyGameInfo gameInfo, CharacterType charType)
	{
		UIGameSettingsPanel.Get().SetVisible(false);
		UIManager.SetGameObjectActive(NavigationBar.Get().m_gameSettingsBtn, gameConfig.GameType == GameType.Custom);
	}

	public void OnShowGameSettingsClicked(BaseEventData data)
	{
		AppState_CharacterSelect.Get().OnShowGameSettingsClicked();
	}
}
