using System;
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
		bool result;
		if (ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason == this.AppliedSeason)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGenericSeasonChapterIntroduction.AreConditionsMetToAutoDisplay()).MethodHandle;
			}
			result = (ClientGameManager.Get().GetHighestOpenSeasonChapterIndexForActiveSeason() == this.AppliedChapter);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override void DisplayIntroduction(int pageNum = 0)
	{
		base.DisplayIntroduction(pageNum);
		GameSubTypeData.GameSubTypeInstructions instructionSet = GameSubTypeData.Get().GetInstructionSet(this.m_instructionType);
		if (instructionSet != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGenericSeasonChapterIntroduction.DisplayIntroduction(int)).MethodHandle;
			}
			for (int i = 0; i < this.m_tooltips.Length; i++)
			{
				if (i < instructionSet.DisplayInfos.Length)
				{
					UIManager.SetGameObjectActive(this.m_tooltips[i], true, null);
					this.m_tooltips[i].Setup(instructionSet.DisplayInfos[i]);
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_tooltips[i], false, null);
				}
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
		if (this.m_thermoStat != null)
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
			InventoryItem itemByTemplateId = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.GetItemByTemplateId(this.m_LootMatrixThermostatTemplateID);
			List<InventoryItem> items = ClientGameManager.Get().GetPlayerAccountData().InventoryComponent.Items;
			List<int> list = new List<int>();
			for (int j = 0; j < items.Count; j++)
			{
				if (items[j].TemplateId == this.m_LootMatrixThermostatTemplateID)
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
					list.Add(items[j].Id);
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
			this.m_thermoStat.UpdateThermostat(ClientGameManager.Get().GetPlayerAccountData().InventoryComponent, itemByTemplateId, InventoryWideData.Get().GetItemTemplate(this.m_LootMatrixThermostatTemplateID), list, false);
		}
	}
}
