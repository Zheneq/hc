using System;
using System.Collections.Generic;
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
		this.m_playerData = data;
		if (this.m_playerData == null)
		{
			UIManager.SetGameObjectActive(this, false, null);
			return;
		}
		this.m_playerName.text = data.DisplayName;
		CharacterResourceLink characterResourceLink = null;
		if (data.m_characterType != CharacterType.None)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIInfoScreenPlayerInfo.Setup(ActorData)).MethodHandle;
			}
			characterResourceLink = GameWideData.Get().GetCharacterResourceLink(data.m_characterType);
		}
		if (characterResourceLink != null)
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
			this.m_playerClass.text = characterResourceLink.GetDisplayName();
		}
		if (Options_UI.Get().m_secretButtonClicked)
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
			PlayerDetails playerDetails = GameFlow.Get().playerDetails[data.\u000E()];
			if (playerDetails.m_accPrivateElo == playerDetails.m_usedMatchmakingElo)
			{
				TextMeshProUGUI playerName = this.m_playerName;
				playerName.text += string.Format(" A: {0:F0}", playerDetails.m_accPrivateElo);
			}
			else if (playerDetails.m_charPrivateElo == playerDetails.m_usedMatchmakingElo)
			{
				TextMeshProUGUI playerName2 = this.m_playerName;
				playerName2.text += string.Format(" C: {0:F0}", playerDetails.m_usedMatchmakingElo);
			}
			else
			{
				TextMeshProUGUI playerName3 = this.m_playerName;
				playerName3.text += string.Format(" M: {0:F0}", playerDetails.m_usedMatchmakingElo);
			}
			TextMeshProUGUI playerClass = this.m_playerClass;
			playerClass.text += string.Format(" C: {0:F0}", playerDetails.m_charPrivateElo);
		}
		ActorBehavior actorBehavior = data.\u000E();
		this.m_playerContribution.text = actorBehavior.totalPlayerContribution.ToString();
		this.m_playerGold.text = data.\u000E().credits.ToString();
		this.m_playerTime.text = "?";
		string text = string.Concat(new string[]
		{
			actorBehavior.totalPlayerKills.ToString(),
			" : ",
			actorBehavior.totalDeaths.ToString(),
			" : ",
			actorBehavior.totalPlayerAssists.ToString()
		});
		this.m_playerKDA.text = text;
		this.m_playerIcon.sprite = data.\u000E();
		if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData == data)
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
			UIManager.SetGameObjectActive(this.m_playerHighlight, true, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_playerHighlight, false, null);
		}
		if (this.m_abilityReferences == null)
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
			this.m_abilityReferences = new List<Ability>();
		}
		if (this.m_abilityModReferences == null)
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
			this.m_abilityModReferences = new List<AbilityMod>();
		}
		this.m_abilityReferences.Clear();
		this.m_abilityModReferences.Clear();
		if (this.m_playerData != null)
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
			if (this.m_playerData.\u000E() != null)
			{
				for (int i = 0; i < this.m_abilityList.Length; i++)
				{
					if (i < this.m_playerData.\u000E().abilityEntries.Length && this.m_playerData.\u000E().abilityEntries[i].ability != null)
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
						this.m_abilityList[i].sprite = this.m_playerData.\u000E().abilityEntries[i].ability.sprite;
						this.m_abilityReferences.Add(this.m_playerData.\u000E().abilityEntries[i].ability);
						if (this.m_playerData.\u000E().abilityEntries[i].ability.CurrentAbilityMod != null)
						{
							this.m_modList[i].color = new Color(1f, 1f, 1f, 1f);
							this.m_modList[i].sprite = this.m_playerData.\u000E().abilityEntries[i].ability.CurrentAbilityMod.m_iconSprite;
							this.m_abilityModReferences.Add(this.m_playerData.\u000E().abilityEntries[i].ability.CurrentAbilityMod);
						}
						else
						{
							this.m_modList[i].color = new Color(0f, 0f, 0f, 0f);
							this.m_modList[i].sprite = null;
							this.m_abilityModReferences.Add(null);
						}
					}
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		UIManager.SetGameObjectActive(this, true, null);
	}

	private void Start()
	{
		int j;
		for (j = 0; j < this.m_modList.Length; j++)
		{
			this.m_modList[j].GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, (UITooltipBase tooltip) => this.ShowModTooltip((UIAbilityTooltip)tooltip, j), null);
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIInfoScreenPlayerInfo.Start()).MethodHandle;
		}
		int i;
		for (i = 0; i < this.m_abilityList.Length; i++)
		{
			this.m_modList[i].GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, (UITooltipBase tooltip) => this.ShowAbilityTooltip((UIAbilityTooltip)tooltip, i), null);
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		this.m_playerContribution.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Contribution, new TooltipPopulateCall(this.SetupContibutionTooltip), null);
	}

	private bool ShowModTooltip(UIAbilityTooltip tooltip, int index)
	{
		if (index < this.m_modList.Length && index >= 0)
		{
			if (index < this.m_abilityReferences.Count)
			{
				tooltip.Setup(this.m_abilityReferences[index], this.m_abilityModReferences[index]);
				return true;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIInfoScreenPlayerInfo.ShowModTooltip(UIAbilityTooltip, int)).MethodHandle;
			}
		}
		return false;
	}

	private bool ShowAbilityTooltip(UIAbilityTooltip tooltip, int index)
	{
		if (index < this.m_abilityList.Length && index >= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIInfoScreenPlayerInfo.ShowAbilityTooltip(UIAbilityTooltip, int)).MethodHandle;
			}
			if (index < this.m_abilityReferences.Count)
			{
				tooltip.Setup(this.m_abilityReferences[index]);
				return true;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	private bool SetupContibutionTooltip(UITooltipBase tooltip)
	{
		UIContributionTooltip uicontributionTooltip = tooltip as UIContributionTooltip;
		if (this.m_playerData != null && uicontributionTooltip != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIInfoScreenPlayerInfo.SetupContibutionTooltip(UITooltipBase)).MethodHandle;
			}
			ActorBehavior actorBehavior = this.m_playerData.\u000E();
			uicontributionTooltip.Setup(StringUtil.TR("Contribution", "GameOver"), actorBehavior.GetContributionBreakdownForUI());
			return true;
		}
		return false;
	}
}
