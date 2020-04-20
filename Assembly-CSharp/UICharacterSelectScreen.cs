using System;
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
		return UICharacterSelectScreen.s_instance;
	}

	public void SelectedGameMode(GameType gameType)
	{
		UICharacterScreen.Get().DoRefreshFunctions(0x120);
	}

	public void Awake()
	{
		UICharacterSelectScreen.s_instance = this;
		if (base.gameObject.transform.parent == null)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	public bool IsTeamBotsTogged()
	{
		return UICharacterScreen.GetCurrentSpecificState().DisplayAllyBotTeammates;
	}

	public void SetGameSettingsButtonVisibility(bool visible)
	{
		if (NavigationBar.Get() != null)
		{
			UIManager.SetGameObjectActive(NavigationBar.Get().m_gameSettingsBtn, visible, null);
		}
	}

	public void ShowGameSettingsPanel(LobbyGameConfig gameConfig, LobbyTeamInfo teamInfo, LobbyPlayerInfo playerInfo)
	{
		UIGameSettingsPanel.Get().SetVisible(true);
		UIGameSettingsPanel.Get().Setup(gameConfig, teamInfo, playerInfo);
		foreach (UICharacterSelectRing uicharacterSelectRing in UICharacterSelectWorldObjects.Get().m_ringAnimations)
		{
			uicharacterSelectRing.SetClickable(false);
		}
		UICharacterSelectScreenController.Get().UpdateReadyCancelButtonStates();
	}

	public void AllowCharacterSwapForConflict()
	{
		UICharacterSelectScreenController.Get().AllowCharacterSwapForConflict();
	}

	public void HideGameSettingsPanel(LobbyGameConfig gameConfig)
	{
		UIGameSettingsPanel.Get().SetVisible(false);
		foreach (UICharacterSelectRing uicharacterSelectRing in UICharacterSelectWorldObjects.Get().m_ringAnimations)
		{
			uicharacterSelectRing.SetClickable(true);
		}
	}

	public void ShowPleaseEquipModsDialog()
	{
		string description = StringUtil.TR("PleaseSelectYourAbilityMods", "Global");
		if (this.m_selectModsDialog == null)
		{
			this.m_selectModsDialog = UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("SelectAbilityMods", "Global"), description, StringUtil.TR("Ok", "Global"), null, -1, false);
		}
	}

	public void SetVisible(bool visible)
	{
		if (this.m_selectModsDialog != null)
		{
			UIDialogPopupManager.Get().CloseDialog(this.m_selectModsDialog);
		}
	}

	public void Setup(LobbyGameConfig gameConfig, LobbyGameInfo gameInfo, CharacterType charType)
	{
		UIGameSettingsPanel.Get().SetVisible(false);
		UIManager.SetGameObjectActive(NavigationBar.Get().m_gameSettingsBtn, gameConfig.GameType == GameType.Custom, null);
	}

	public void OnShowGameSettingsClicked(BaseEventData data)
	{
		AppState_CharacterSelect.Get().OnShowGameSettingsClicked();
	}
}
