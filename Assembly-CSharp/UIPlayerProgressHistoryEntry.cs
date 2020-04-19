using System;
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

	public string GameServerProcessCode { get; private set; }

	private void Awake()
	{
		this.m_watchReplay.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.WatchReplayClick);
	}

	public void Setup(PersistedCharacterMatchData entry, UIPlayerProgressHistory parent)
	{
		UIManager.SetGameObjectActive(this.m_winLabel, entry.MatchComponent.Result == PlayerGameResult.Win, null);
		UIManager.SetGameObjectActive(this.m_lostLabel, entry.MatchComponent.Result == PlayerGameResult.Lose, null);
		UIManager.SetGameObjectActive(this.m_drawLabel, entry.MatchComponent.Result == PlayerGameResult.Tie, null);
		if (entry.MatchComponent.GetFirstPlayerCharacter() != CharacterType.None)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressHistoryEntry.Setup(PersistedCharacterMatchData, UIPlayerProgressHistory)).MethodHandle;
			}
			CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(entry.MatchComponent.GetFirstPlayerCharacter());
			if (characterResourceLink != null)
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
				this.m_heroIcon.sprite = characterResourceLink.GetCharacterSelectIcon();
				this.m_heroIcon.enabled = true;
				this.SetAllLabels(this.m_heroLabels, characterResourceLink.GetDisplayName());
			}
		}
		else
		{
			this.m_heroIcon.enabled = false;
			this.SetAllLabels(this.m_heroLabels, string.Empty);
		}
		this.SetAllLabels(this.m_modeLabels, entry.MatchComponent.GameType.GetDisplayName());
		this.SetAllLabels(this.m_mapLabels, GameWideData.Get().GetMapDisplayName(entry.MatchComponent.MapName));
		this.SetAllLabels(this.m_timeLabels, string.Format(StringUtil.TR("MatchTimeDifference", "Global"), entry.MatchComponent.GetTimeDifferenceText()));
		this.SetAllLabels(this.m_turnLabels, string.Format("{0:n0}", entry.MatchComponent.NumOfTurns));
		this.SetSelected(false);
		this.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnClick);
		this.GameServerProcessCode = entry.GameServerProcessCode;
		UIManager.SetGameObjectActive(this.m_watchReplay, false, null);
		this.m_replayPath = parent.GetReplayFilename(this.GameServerProcessCode);
		if (!this.m_replayPath.IsNullOrEmpty())
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
			string json = File.ReadAllText(this.m_replayPath);
			Replay replay = JsonUtility.FromJson<Replay>(json);
			LobbyGameInfo lobbyGameInfo = JsonUtility.FromJson<LobbyGameInfo>(replay.m_gameInfo_Serialized);
			UIManager.SetGameObjectActive(this.m_watchReplay, lobbyGameInfo.GameConfig.GameType != GameType.Tutorial, null);
		}
		this.m_matchData = entry;
	}

	private void SetAllLabels(TextMeshProUGUI[] labels, string text)
	{
		for (int i = 0; i < labels.Length; i++)
		{
			labels[i].text = text;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressHistoryEntry.SetAllLabels(TextMeshProUGUI[], string)).MethodHandle;
		}
	}

	private void OnClick(BaseEventData eventData)
	{
		UIPlayerProgressPanel.Get().m_historyPanel.MatchClicked(this);
		this.SetSelected(!this.isSelected);
		if (this.m_matchData != null)
		{
			UIGameOverPlayerEntry.PreSetupInitialization();
			UIGameStatsWindow.Get().SetupTeamMemberList(this.m_matchData);
			UIGameStatsWindow.Get().ToggleStatsWindow();
			UIGameStatsWindow.Get().SetStatePage(UIGameStatsWindow.StatsPage.Numbers);
		}
		else
		{
			Debug.LogFormat("No details available for selected match", new object[0]);
		}
	}

	public void SetSelected(bool isSelected)
	{
		this.m_hitbox.selectableButton.SetSelected(isSelected, false, string.Empty, string.Empty);
		this.isSelected = isSelected;
	}

	private void WatchReplayClick(BaseEventData eventData)
	{
		ReplayPlayManager.Get().StartReplay(this.m_replayPath);
	}
}
