using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInfoScreenPlayerInfo : MonoBehaviour
{
	public TextMeshProUGUI m_playerName;

	public TextMeshProUGUI m_playerClass;

	public TextMeshProUGUI m_playerKDA;

	public TextMeshProUGUI m_playerContribution;

	public TextMeshProUGUI m_playerGold;

	public TextMeshProUGUI m_playerTime;

	public Image m_playerHighlight;

	public Image m_playerIcon;

	public Image[] m_abilityList;

	public Image[] m_modList;

	private ActorData m_playerData;

	private List<Ability> m_abilityReferences;

	private List<AbilityMod> m_abilityModReferences;

	public void Setup(ActorData data)
	{
		m_playerData = data;
		if (m_playerData == null)
		{
			UIManager.SetGameObjectActive(this, false);
			return;
		}
		m_playerName.text = data.DisplayName;
		CharacterResourceLink characterResourceLink = null;
		if (data.m_characterType != 0)
		{
			characterResourceLink = GameWideData.Get().GetCharacterResourceLink(data.m_characterType);
		}
		if (characterResourceLink != null)
		{
			m_playerClass.text = characterResourceLink.GetDisplayName();
		}
		if (Options_UI.Get().m_secretButtonClicked)
		{
			PlayerDetails playerDetails = GameFlow.Get().playerDetails[data.GetPlayer()];
			if (playerDetails.m_accPrivateElo == playerDetails.m_usedMatchmakingElo)
			{
				m_playerName.text += new StringBuilder().Append(" A: ").AppendFormat("{0:F0}", playerDetails.m_accPrivateElo).ToString();
			}
			else if (playerDetails.m_charPrivateElo == playerDetails.m_usedMatchmakingElo)
			{
				m_playerName.text += new StringBuilder().Append(" C: ").AppendFormat("{0:F0}", playerDetails.m_usedMatchmakingElo).ToString();
			}
			else
			{
				m_playerName.text += new StringBuilder().Append(" M: ").AppendFormat("{0:F0}", playerDetails.m_usedMatchmakingElo).ToString();
			}

			m_playerClass.text += new StringBuilder().Append(" C: ").AppendFormat("{0:F0}", playerDetails.m_charPrivateElo).ToString();
		}
		ActorBehavior actorBehavior = data.GetActorBehavior();
		m_playerContribution.text = actorBehavior.totalPlayerContribution.ToString();
		m_playerGold.text = data.GetItemData().credits.ToString();
		m_playerTime.text = "?";
		string text = new StringBuilder().Append(actorBehavior.totalPlayerKills).Append(" : ").Append(actorBehavior.totalDeaths).Append(" : ").Append(actorBehavior.totalPlayerAssists).ToString();
		m_playerKDA.text = text;
		m_playerIcon.sprite = data.GetAliveHUDIcon();
		if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData == data)
		{
			UIManager.SetGameObjectActive(m_playerHighlight, true);
		}
		else
		{
			UIManager.SetGameObjectActive(m_playerHighlight, false);
		}
		if (m_abilityReferences == null)
		{
			m_abilityReferences = new List<Ability>();
		}
		if (m_abilityModReferences == null)
		{
			m_abilityModReferences = new List<AbilityMod>();
		}
		m_abilityReferences.Clear();
		m_abilityModReferences.Clear();
		if (m_playerData != null)
		{
			if (m_playerData.GetAbilityData() != null)
			{
				for (int i = 0; i < m_abilityList.Length; i++)
				{
					if (i < m_playerData.GetAbilityData().abilityEntries.Length && m_playerData.GetAbilityData().abilityEntries[i].ability != null)
					{
						m_abilityList[i].sprite = m_playerData.GetAbilityData().abilityEntries[i].ability.sprite;
						m_abilityReferences.Add(m_playerData.GetAbilityData().abilityEntries[i].ability);
						if (m_playerData.GetAbilityData().abilityEntries[i].ability.CurrentAbilityMod != null)
						{
							m_modList[i].color = new Color(1f, 1f, 1f, 1f);
							m_modList[i].sprite = m_playerData.GetAbilityData().abilityEntries[i].ability.CurrentAbilityMod.m_iconSprite;
							m_abilityModReferences.Add(m_playerData.GetAbilityData().abilityEntries[i].ability.CurrentAbilityMod);
						}
						else
						{
							m_modList[i].color = new Color(0f, 0f, 0f, 0f);
							m_modList[i].sprite = null;
							m_abilityModReferences.Add(null);
						}
					}
				}
			}
		}
		UIManager.SetGameObjectActive(this, true);
	}

	private void Start()
	{
		int i;
		for (i = 0; i < m_modList.Length; i++)
		{
			m_modList[i].GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, (UITooltipBase tooltip) => ShowModTooltip((UIAbilityTooltip)tooltip, i));
		}
		int j;
		while (true)
		{
			for (j = 0; j < m_abilityList.Length; j++)
			{
				m_modList[j].GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, (UITooltipBase tooltip) => ShowAbilityTooltip((UIAbilityTooltip)tooltip, j));
			}
			while (true)
			{
				m_playerContribution.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Contribution, SetupContibutionTooltip);
				return;
			}
		}
	}

	private bool ShowModTooltip(UIAbilityTooltip tooltip, int index)
	{
		if (index < m_modList.Length && index >= 0)
		{
			if (index < m_abilityReferences.Count)
			{
				tooltip.Setup(m_abilityReferences[index], m_abilityModReferences[index]);
				return true;
			}
		}
		return false;
	}

	private bool ShowAbilityTooltip(UIAbilityTooltip tooltip, int index)
	{
		if (index < m_abilityList.Length && index >= 0)
		{
			if (index < m_abilityReferences.Count)
			{
				tooltip.Setup(m_abilityReferences[index]);
				return true;
			}
		}
		return false;
	}

	private bool SetupContibutionTooltip(UITooltipBase tooltip)
	{
		UIContributionTooltip uIContributionTooltip = tooltip as UIContributionTooltip;
		if (m_playerData != null && uIContributionTooltip != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					ActorBehavior actorBehavior = m_playerData.GetActorBehavior();
					uIContributionTooltip.Setup(StringUtil.TR("Contribution", "GameOver"), actorBehavior.GetContributionBreakdownForUI());
					return true;
				}
				}
			}
		}
		return false;
	}
}
