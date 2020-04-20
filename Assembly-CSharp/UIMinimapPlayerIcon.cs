using System;
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
			return;
		}
		this.m_playerIcon.sprite = playerData.GetAliveHUDIcon();
		if (GameFlowData.Get() != null)
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
			{
				if (playerData == activeOwnedActorData)
				{
					this.m_teamColorImage.color = HUD_UIResources.Get().m_selfColorHighlight;
				}
				else if (playerData.GetTeam() == activeOwnedActorData.GetTeam())
				{
					this.m_teamColorImage.color = HUD_UIResources.Get().m_allyColorHighlight;
				}
				else
				{
					this.m_teamColorImage.color = HUD_UIResources.Get().m_enemyColorHighlight;
				}
			}
		}
	}
}
