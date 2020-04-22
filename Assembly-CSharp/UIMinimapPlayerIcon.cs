using UnityEngine;
using UnityEngine.UI;

public class UIMinimapPlayerIcon : MonoBehaviour
{
	public Image m_playerIcon;

	public Image m_teamColorImage;

	public void Setup(ActorData playerData)
	{
		if (playerData == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		m_playerIcon.sprite = playerData.GetAliveHUDIcon();
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (!(activeOwnedActorData != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (playerData == activeOwnedActorData)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_teamColorImage.color = HUD_UIResources.Get().m_selfColorHighlight;
						return;
					}
				}
			}
			if (playerData.GetTeam() == activeOwnedActorData.GetTeam())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						m_teamColorImage.color = HUD_UIResources.Get().m_allyColorHighlight;
						return;
					}
				}
			}
			m_teamColorImage.color = HUD_UIResources.Get().m_enemyColorHighlight;
			return;
		}
	}
}
