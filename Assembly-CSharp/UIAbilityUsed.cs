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
		UIAbilityTooltip uIAbilityTooltip = (UIAbilityTooltip)tooltip;
		uIAbilityTooltip.Setup(m_abilityRef);
		return true;
	}

	private void Awake()
	{
		m_tooltipHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, ShowTooltip);
	}

	public Ability GetAbilityRef()
	{
		return m_abilityRef;
	}

	public void Setup(Ability newAbility, ActorData theOwner)
	{
		m_abilityIcon.sprite = newAbility.sprite;
		m_playerIcon.sprite = theOwner.GetAliveHUDIcon();
		if (!(GameFlowData.Get().activeOwnedActorData == null))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(GameFlowData.Get().activeOwnedActorData == theOwner))
			{
				if (GameFlowData.Get().activeOwnedActorData.GetTeam() == theOwner.GetTeam())
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
					m_teamColorIndicator.sprite = HUD_UIResources.Get().m_teammateBorder;
				}
				else
				{
					m_teamColorIndicator.sprite = HUD_UIResources.Get().m_enemyBorder;
				}
				goto IL_00df;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_teamColorIndicator.sprite = HUD_UIResources.Get().m_selfBorder;
		goto IL_00df;
		IL_00df:
		m_abilityRef = newAbility;
	}
}
