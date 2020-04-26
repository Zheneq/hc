using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerProgressHistoryEntry : MonoBehaviour
{
	public Image m_heroIcon;

	public _ButtonSwapSprite m_hitbox;

	public TextMeshProUGUI[] m_heroLabels;

	public TextMeshProUGUI[] m_modeLabels;

	public TextMeshProUGUI[] m_mapLabels;

	public TextMeshProUGUI[] m_timeLabels;

	public TextMeshProUGUI[] m_turnLabels;

	public GameObject m_winLabel;

	public GameObject m_lostLabel;

	public GameObject m_drawLabel;

	public _SelectableBtn m_watchReplay;

	private bool isSelected;

	private string m_replayPath;

	private PersistedCharacterMatchData m_matchData;

	public string GameServerProcessCode
	{
		get;
		private set;
	}

	private void Awake()
	{
		m_watchReplay.spriteController.callback = WatchReplayClick;
	}

	public void Setup(PersistedCharacterMatchData entry, UIPlayerProgressHistory parent)
	{
		UIManager.SetGameObjectActive(m_winLabel, entry.MatchComponent.Result == PlayerGameResult.Win);
		UIManager.SetGameObjectActive(m_lostLabel, entry.MatchComponent.Result == PlayerGameResult.Lose);
		UIManager.SetGameObjectActive(m_drawLabel, entry.MatchComponent.Result == PlayerGameResult.Tie);
		if (entry.MatchComponent.GetFirstPlayerCharacter() != 0)
		{
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(entry.MatchComponent.GetFirstPlayerCharacter());
			if (characterResourceLink != null)
			{
				m_heroIcon.sprite = characterResourceLink.GetCharacterSelectIcon();
				m_heroIcon.enabled = true;
				SetAllLabels(m_heroLabels, characterResourceLink.GetDisplayName());
			}
		}
		else
		{
			m_heroIcon.enabled = false;
			SetAllLabels(m_heroLabels, string.Empty);
		}
		SetAllLabels(m_modeLabels, entry.MatchComponent.GameType.GetDisplayName());
		SetAllLabels(m_mapLabels, GameWideData.Get().GetMapDisplayName(entry.MatchComponent.MapName));
		SetAllLabels(m_timeLabels, string.Format(StringUtil.TR("MatchTimeDifference", "Global"), entry.MatchComponent.GetTimeDifferenceText()));
		SetAllLabels(m_turnLabels, $"{entry.MatchComponent.NumOfTurns:n0}");
		SetSelected(false);
		m_hitbox.callback = OnClick;
		GameServerProcessCode = entry.GameServerProcessCode;
		UIManager.SetGameObjectActive(m_watchReplay, false);
		m_replayPath = parent.GetReplayFilename(GameServerProcessCode);
		if (!m_replayPath.IsNullOrEmpty())
		{
			string json = File.ReadAllText(m_replayPath);
			Replay replay = JsonUtility.FromJson<Replay>(json);
			LobbyGameInfo lobbyGameInfo = JsonUtility.FromJson<LobbyGameInfo>(replay.m_gameInfo_Serialized);
			UIManager.SetGameObjectActive(m_watchReplay, lobbyGameInfo.GameConfig.GameType != GameType.Tutorial);
		}
		m_matchData = entry;
	}

	private void SetAllLabels(TextMeshProUGUI[] labels, string text)
	{
		for (int i = 0; i < labels.Length; i++)
		{
			labels[i].text = text;
		}
		while (true)
		{
			return;
		}
	}

	private void OnClick(BaseEventData eventData)
	{
		UIPlayerProgressPanel.Get().m_historyPanel.MatchClicked(this);
		SetSelected(!isSelected);
		if (m_matchData != null)
		{
			UIGameOverPlayerEntry.PreSetupInitialization();
			UIGameStatsWindow.Get().SetupTeamMemberList(m_matchData);
			UIGameStatsWindow.Get().ToggleStatsWindow();
			UIGameStatsWindow.Get().SetStatePage(UIGameStatsWindow.StatsPage.Numbers);
		}
		else
		{
			Debug.LogFormat("No details available for selected match");
		}
	}

	public void SetSelected(bool isSelected)
	{
		m_hitbox.selectableButton.SetSelected(isSelected, false, string.Empty, string.Empty);
		this.isSelected = isSelected;
	}

	private void WatchReplayClick(BaseEventData eventData)
	{
		ReplayPlayManager.Get().StartReplay(m_replayPath);
	}
}
