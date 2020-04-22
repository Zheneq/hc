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
		if (_003C_003Ef__am_0024cache0 == null)
		{
			while (true)
			{
				switch (2)
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
			_003C_003Ef__am_0024cache0 = delegate(UITooltipBase tooltip)
			{
				(tooltip as UISimpleTooltip).Setup(StringUtil.TR("ThermostatTooltip", "LootMatrixScene"));
				return true;
			};
		}
		thermostatTooltipObj.Setup(TooltipType.Simple, _003C_003Ef__am_0024cache0);
	}

	public void UpdateThermostat(InventoryComponent component, InventoryItem InvItem, InventoryItemTemplate InvItemTemplate, List<int> m_boxIds, bool GotLegendary = false)
	{
		bool flag = false;
		int num = -1;
		if (InvItem != null && InvItemTemplate != null)
		{
			while (true)
			{
				switch (6)
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
			if (InvItemTemplate.Type == InventoryItemType.Lockbox)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				LootTable lootTable = InventoryWideData.Get().GetLootTable(InvItemTemplate.TypeSpecificData[0]);
				if (lootTable != null)
				{
					Loot loot = component.GetLoot(InvItem.Id);
					if (loot == null)
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
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
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
									while (true)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
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
										while (true)
										{
											switch (7)
											{
											case 0:
												continue;
											}
											break;
										}
										break;
									}
									KeyValuePair<int, Karma> current2 = enumerator2.Current;
									if (current2.Value.TemplateId == lootTable.CheckKarmas[i].KarmaTemplateId)
									{
										while (true)
										{
											switch (1)
											{
											case 0:
												continue;
											}
											break;
										}
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
