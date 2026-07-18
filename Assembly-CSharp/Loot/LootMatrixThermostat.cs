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
		UITooltipHoverObject thermostatTooltipObj = m_thermostatTooltipObj;
		
		thermostatTooltipObj.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
			{
				(tooltip as UISimpleTooltip).Setup(StringUtil.TR("ThermostatTooltip", "LootMatrixScene"));
				return true;
			});
	}

	public void UpdateThermostat(InventoryComponent component, InventoryItem InvItem, InventoryItemTemplate InvItemTemplate, List<int> m_boxIds, bool GotLegendary = false)
	{
		bool flag = false;
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
						int num2 = 0;
						while (true)
						{
							if (num2 < m_boxIds.Count)
							{
								InventoryItem item = component.GetItem(m_boxIds[num2]);
								if (item.TemplateId == InvItemTemplate.Index)
								{
									loot = component.GetLoot(item.Id);
									break;
								}
								num2++;
								continue;
							}
							break;
						}
					}
					if (loot != null)
					{
						using (List<CheckKarma>.Enumerator enumerator = lootTable.CheckKarmas.GetEnumerator())
						{
							while (true)
							{
								if (!enumerator.MoveNext())
								{
									break;
								}
								CheckKarma current = enumerator.Current;
								int karmaQuantity = loot.GetKarmaQuantity(current.KarmaTemplateId);
								if (0 < karmaQuantity)
								{
									num = Mathf.RoundToInt(current.GetChance(karmaQuantity));
									flag = true;
									break;
								}
							}
						}
					}
					else
					{
						for (int i = 0; i < lootTable.CheckKarmas.Count; i++)
						{
							using (Dictionary<int, Karma>.Enumerator enumerator2 = component.Karmas.GetEnumerator())
							{
								while (true)
								{
									if (!enumerator2.MoveNext())
									{
										break;
									}
									KeyValuePair<int, Karma> current2 = enumerator2.Current;
									if (current2.Value.TemplateId == lootTable.CheckKarmas[i].KarmaTemplateId)
									{
										int quantity = current2.Value.Quantity;
										if (0 < quantity)
										{
											while (true)
											{
												switch (1)
												{
												case 0:
													break;
												default:
													num = Mathf.RoundToInt(lootTable.CheckKarmas[i].GetChance(quantity));
													flag = true;
													goto end_IL_014d;
												}
											}
										}
									}
								}
								end_IL_014d:;
							}
						}
					}
				}
			}
		}
		flag = false;
		UIManager.SetGameObjectActive(m_ThermostatGroup, flag);
		if (!flag)
		{
			return;
		}
		m_ThermostateSlider.fillAmount = (float)num / 100f;
		m_ThermostatPercentLabel.text = string.Empty;
		if (GotLegendary)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					UIAnimationEventManager.Get().PlayAnimation(m_ThermostatAnimator, "LootMatrixThermostatGotItem", null, string.Empty, 1);
					return;
				}
			}
		}
		if (num >= 100)
		{
			UIAnimationEventManager.Get().PlayAnimation(m_ThermostatAnimator, "LootMatrixThermostat100Glow", null, string.Empty, 1);
			return;
		}
		if (num >= 75)
		{
			UIAnimationEventManager.Get().PlayAnimation(m_ThermostatAnimator, "LootMatrixThermostat75Glow", null, string.Empty, 1);
			return;
		}
		if (num >= 50)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					UIAnimationEventManager.Get().PlayAnimation(m_ThermostatAnimator, "LootMatrixThermostat50Glow", null, string.Empty, 1);
					return;
				}
			}
		}
		if (num >= 25)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					UIAnimationEventManager.Get().PlayAnimation(m_ThermostatAnimator, "LootMatrixThermostat25Glow", null, string.Empty, 1);
					return;
				}
			}
		}
		UIAnimationEventManager.Get().PlayAnimation(m_ThermostatAnimator, "LootMatrixThermostat0Glow", null, string.Empty, 1);
	}
}
