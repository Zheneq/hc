using System;
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
		this.m_gameName.text = game.GameConfig.RoomName;
		this.m_playerCount.text = game.ActivePlayers + "/" + game.GameConfig.TotalPlayers;
		this.m_spectatorCount.text = game.ActiveSpectators + "/" + game.GameConfig.Spectators;
		this.m_mapName.text = GameWideData.Get().GetMapDisplayName(game.GameConfig.Map);
		bool doActive = game.ActiveSpectators < game.GameConfig.Spectators;
		RectTransform[] componentsInChildren = this.m_joinAsSpectatorButton.transform.parent.GetComponentsInChildren<RectTransform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != this.m_joinAsSpectatorButton.transform.parent as RectTransform)
			{
				UIManager.SetGameObjectActive(componentsInChildren[i], doActive, null);
			}
		}
		bool doActive2 = game.ActivePlayers < game.GameConfig.TotalPlayers;
		componentsInChildren = this.m_joinButton.transform.parent.GetComponentsInChildren<RectTransform>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if (componentsInChildren[j] != this.m_joinButton.transform.parent as RectTransform)
			{
				UIManager.SetGameObjectActive(componentsInChildren[j], doActive2, null);
			}
		}
	}
}
