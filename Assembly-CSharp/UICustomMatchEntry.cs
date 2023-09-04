using TMPro;
using UnityEngine;

public class UICustomMatchEntry : MonoBehaviour
{
	public TextMeshProUGUI m_gameName;
	public TextMeshProUGUI m_playerCount;
	public TextMeshProUGUI m_spectatorCount;
	public TextMeshProUGUI m_mapName;
	public _ButtonSwapSprite m_joinButton;
	public _ButtonSwapSprite m_joinAsSpectatorButton;

	public void Setup(LobbyGameInfo game)
	{
		m_gameName.text = game.GameConfig.RoomName;
		m_playerCount.text = game.ActivePlayers + "/" + game.GameConfig.TotalPlayers;
		m_spectatorCount.text = game.ActiveSpectators + "/" + game.GameConfig.Spectators;
		m_mapName.text = GameWideData.Get().GetMapDisplayName(game.GameConfig.Map);
		bool canAddSpectators = game.ActiveSpectators < game.GameConfig.Spectators;
		foreach (RectTransform t in m_joinAsSpectatorButton.transform.parent.GetComponentsInChildren<RectTransform>())
		{
			if (t != m_joinAsSpectatorButton.transform.parent as RectTransform)
			{
				UIManager.SetGameObjectActive(t, canAddSpectators);
			}
		}
		bool canAddPlayers = game.ActivePlayers < game.GameConfig.TotalPlayers;
		foreach (RectTransform t in m_joinButton.transform.parent.GetComponentsInChildren<RectTransform>())
		{
			if (t != m_joinButton.transform.parent as RectTransform)
			{
				UIManager.SetGameObjectActive(t, canAddPlayers);
			}
		}
	}
}
