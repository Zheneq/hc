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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIMinimapPlayerIcon.Setup(ActorData)).MethodHandle;
			}
			return;
		}
		this.m_playerIcon.sprite = playerData.GetAliveHUDIcon();
		if (GameFlowData.Get() != null)
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (playerData == activeOwnedActorData)
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
					this.m_teamColorImage.color = HUD_UIResources.Get().m_selfColorHighlight;
				}
				else if (playerData.GetTeam() == activeOwnedActorData.GetTeam())
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
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
