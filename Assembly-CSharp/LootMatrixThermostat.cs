using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LootMatrixThermostat : MonoBehaviour
{
	public RectTransform m_ThermostatGroup;

	public Animator m_ThermostatAnimator;

	public Image m_ThermostateSlider;

	public TextMeshProUGUI m_ThermostatPercentLabel;

	public UITooltipHoverObject m_thermostatTooltipObj;

	private void Awake()
	{
		UITooltipObject thermostatTooltipObj = this.m_thermostatTooltipObj;
		TooltipType tooltipType = TooltipType.Simple;
		
		thermostatTooltipObj.Setup(tooltipType, delegate(UITooltipBase tooltip)
			{
				(tooltip as UISimpleTooltip).Setup(StringUtil.TR("ThermostatTooltip", "LootMatrixScene"));
				return true;
			}, null);
	}

	public void UpdateThermostat(InventoryComponent component, InventoryItem InvItem, InventoryItemTemplate InvItemTemplate, List<int> m_boxIds, bool GotLegendary = false)
	{
		int num = -1;
		if (InvItem != null && InvItemTemplate != null)
		{
			if (InvItemTemplate.Type == InventoryItemType.Lockbox)
			{
				LootTable lootTable = InventoryWideData.Get().GetLootTable(InvItemTemplate.TypeSpecificData[0]);
				if (lootTable != null)
				{
					Loot loot = component.GetLoot(InvItem.Id);
					if (loot == null)
					{
						for (int i = 0; i < m_boxIds.Count; i++)
						{
							InventoryItem item = component.GetItem(m_boxIds[i]);
							if (item.TemplateId == InvItemTemplate.Index)
							{
								loot = component.GetLoot(item.Id);
								goto IL_C4;
							}
						}
					}
					IL_C4:
					if (loot != null)
					{
						using (List<CheckKarma>.Enumerator enumerator = lootTable.CheckKarmas.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								CheckKarma checkKarma = enumerator.Current;
								int karmaQuantity = loot.GetKarmaQuantity(checkKarma.KarmaTemplateId);
								if (0 < karmaQuantity)
								{
									num = Mathf.RoundToInt(checkKarma.GetChance(karmaQuantity));
									goto IL_12F;
								}
							}
						}
						IL_12F:;
					}
					else
					{
						for (int j = 0; j < lootTable.CheckKarmas.Count; j++)
						{
							using (Dictionary<int, Karma>.Enumerator enumerator2 = component.Karmas.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									KeyValuePair<int, Karma> keyValuePair = enumerator2.Current;
									if (keyValuePair.Value.TemplateId == lootTable.CheckKarmas[j].KarmaTemplateId)
									{
										int quantity = keyValuePair.Value.Quantity;
										if (0 < quantity)
										{
											num = Mathf.RoundToInt(lootTable.CheckKarmas[j].GetChance(quantity));
											goto IL_1F2;
										}
									}
								}
							}
							IL_1F2:;
						}
					}
				}
			}
		}
		bool flag = false;
		UIManager.SetGameObjectActive(this.m_ThermostatGroup, flag, null);
		if (flag)
		{
			this.m_ThermostateSlider.fillAmount = (float)num / 100f;
			this.m_ThermostatPercentLabel.text = string.Empty;
			if (GotLegendary)
			{
				UIAnimationEventManager.Get().PlayAnimation(this.m_ThermostatAnimator, "LootMatrixThermostatGotItem", null, string.Empty, 1, 0f, true, false, null, null);
			}
			else if (num >= 0x64)
			{
				UIAnimationEventManager.Get().PlayAnimation(this.m_ThermostatAnimator, "LootMatrixThermostat100Glow", null, string.Empty, 1, 0f, true, false, null, null);
			}
			else if (num >= 0x4B)
			{
				UIAnimationEventManager.Get().PlayAnimation(this.m_ThermostatAnimator, "LootMatrixThermostat75Glow", null, string.Empty, 1, 0f, true, false, null, null);
			}
			else if (num >= 0x32)
			{
				UIAnimationEventManager.Get().PlayAnimation(this.m_ThermostatAnimator, "LootMatrixThermostat50Glow", null, string.Empty, 1, 0f, true, false, null, null);
			}
			else if (num >= 0x19)
			{
				UIAnimationEventManager.Get().PlayAnimation(this.m_ThermostatAnimator, "LootMatrixThermostat25Glow", null, string.Empty, 1, 0f, true, false, null, null);
			}
			else
			{
				UIAnimationEventManager.Get().PlayAnimation(this.m_ThermostatAnimator, "LootMatrixThermostat0Glow", null, string.Empty, 1, 0f, true, false, null, null);
			}
		}
	}
}
