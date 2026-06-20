using System.Collections.Generic;
using UnityEngine;

public class UIGenericSeasonChapterIntroduction : UIIntroductions
{
	[Header("Season Chapter Introduction")]
	public int AppliedSeason = 2;

	public int AppliedChapter = 1;

	public LootMatrixThermostat m_thermoStat;

	public int m_LootMatrixThermostatTemplateID;

	[Header("Introduction Specific")]
	public GameSubType.GameLoadScreenInstructions m_instructionType;

	public LoadingScreenSubtypeTooltip[] m_tooltips;

	public override bool AreConditionsMetToAutoDisplay()
	{
		int result;
		if (ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason == AppliedSeason)
		{
			result = ((ClientGameManager.Get().GetHighestOpenSeasonChapterIndexForActiveSeason() == AppliedChapter) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override void DisplayIntroduction(int pageNum = 0)
	{
		base.DisplayIntroduction(pageNum);
		GameSubTypeData.GameSubTypeInstructions instructionSet = GameSubTypeData.Get().GetInstructionSet(m_instructionType);
		if (instructionSet != null)
		{
			for (int i = 0; i < m_tooltips.Length; i++)
			{
				if (i < instructionSet.DisplayInfos.Length)
				{
					UIManager.SetGameObjectActive(m_tooltips[i], true);
					m_tooltips[i].Setup(instructionSet.DisplayInfos[i]);
				}
				else
				{
					UIManager.SetGameObjectActive(m_tooltips[i], false);
				}
			}
		}
		if (!(m_thermoStat != null))
		{
			return;
		}
		while (true)
		{
			InventoryItem itemByTemplateId = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.GetItemByTemplateId(m_LootMatrixThermostatTemplateID);
			List<InventoryItem> items = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.Items;
			List<int> list = new List<int>();
			for (int j = 0; j < items.Count; j++)
			{
				if (items[j].TemplateId == m_LootMatrixThermostatTemplateID)
				{
					list.Add(items[j].Id);
				}
			}
			while (true)
			{
				m_thermoStat.UpdateThermostat(ClientGameManager.Get().GetPlayerAccountData().InventoryComponent, itemByTemplateId, InventoryWideData.Get().GetItemTemplate(m_LootMatrixThermostatTemplateID), list);
				return;
			}
		}
	}
}
