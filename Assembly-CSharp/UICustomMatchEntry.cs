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
		bool doActive = game.ActiveSpectators < game.GameConfig.Spectators;
		RectTransform[] componentsInChildren = m_joinAsSpectatorButton.transform.parent.GetComponentsInChildren<RectTransform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i] != m_joinAsSpectatorButton.transform.parent as RectTransform)
			{
				UIManager.SetGameObjectActive(componentsInChildren[i], doActive);
			}
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			bool doActive2 = game.ActivePlayers < game.GameConfig.TotalPlayers;
			componentsInChildren = m_joinButton.transform.parent.GetComponentsInChildren<RectTransform>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (componentsInChildren[j] != m_joinButton.transform.parent as RectTransform)
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
					UIManager.SetGameObjectActive(componentsInChildren[j], doActive2);
				}
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}
}
