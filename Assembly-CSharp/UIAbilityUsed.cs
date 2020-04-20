using System;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilityUsed : MonoBehaviour
{
	public Image m_abilityIcon;

	public Image m_playerIcon;

	public Image m_teamColorIndicator;

	public Button m_tooltipHitBox;

	private Ability m_abilityRef;

	private bool ShowTooltip(UITooltipBase tooltip)
	{
		UIAbilityTooltip uiabilityTooltip = (UIAbilityTooltip)tooltip;
		uiabilityTooltip.Setup(this.m_abilityRef);
		return true;
	}

	private void Awake()
	{
		this.m_tooltipHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, new TooltipPopulateCall(this.ShowTooltip), null);
	}

	public Ability GetAbilityRef()
	{
		return this.m_abilityRef;
	}

	public void Setup(Ability newAbility, ActorData theOwner)
	{
		this.m_abilityIcon.sprite = newAbility.sprite;
		this.m_playerIcon.sprite = theOwner.GetAliveHUDIcon();
		if (!(GameFlowData.Get().activeOwnedActorData == null))
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityUsed.Setup(Ability, ActorData)).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData == theOwner)
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
			}
			else
			{
				if (GameFlowData.Get().activeOwnedActorData.GetTeam() == theOwner.GetTeam())
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
					this.m_teamColorIndicator.sprite = HUD_UIResources.Get().m_teammateBorder;
					goto IL_DF;
				}
				this.m_teamColorIndicator.sprite = HUD_UIResources.Get().m_enemyBorder;
				goto IL_DF;
			}
		}
		this.m_teamColorIndicator.sprite = HUD_UIResources.Get().m_selfBorder;
		IL_DF:
		this.m_abilityRef = newAbility;
	}
}
