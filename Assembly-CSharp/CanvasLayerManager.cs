using System.Collections.Generic;
using UnityEngine;

public class CanvasLayerManager
{
	private List<_CanvasLayerSorter> m_canvases;

	private List<CanvasLayerName> m_canvasSortOrder;

	private int nameplateItemBeginIndex;

	private int nameplateItemEndIndex;

	private const int c_nameplatePadding = 500;

	private bool m_addedNewNameplate;

	private int m_refreshCounter;

	private static CanvasLayerManager s_instance;

	private CanvasLayerManager()
	{
		m_canvases = new List<_CanvasLayerSorter>();
		CreateSortOrder();
	}

	public static CanvasLayerManager Get()
	{
		if (s_instance == null)
		{
			s_instance = new CanvasLayerManager();
		}
		return s_instance;
	}

	~CanvasLayerManager()
	{
		s_instance = null;
	}

	public void NotifyAddedNewNameplate()
	{
		m_addedNewNameplate = true;
		m_refreshCounter = 0;
	}

	public void Update()
	{
		if (!m_addedNewNameplate)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			foreach (_CanvasLayerSorter canvase in m_canvases)
			{
				canvase.DoCanvasRefresh();
			}
			m_refreshCounter++;
			if (m_refreshCounter > 1)
			{
				m_addedNewNameplate = false;
			}
			return;
		}
	}

	private void CreateSortOrder()
	{
		m_canvasSortOrder = new List<CanvasLayerName>();
		m_canvasSortOrder.Add(CanvasLayerName.None);
		m_canvasSortOrder.Add(CanvasLayerName.InputBlock);
		m_canvasSortOrder.Add(CanvasLayerName.PersistantWorldCanvas);
		m_canvasSortOrder.Add(CanvasLayerName.TutorialPanel);
		m_canvasSortOrder.Add(CanvasLayerName.OffscreenIndicatorPanel);
		m_canvasSortOrder.Add(CanvasLayerName.NameplatePanel);
		nameplateItemBeginIndex = m_canvasSortOrder.Count;
		m_canvasSortOrder.Add(CanvasLayerName.NameplateItem);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateTargetGlow);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateBG);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateArrowBG);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateTeamColor);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateResolutionBar);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateDamageEaseBar);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateHPGainBar);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateShieldMask);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateCurrentHP);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateTechPointBarEase);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateTechPointBar);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateTickContainer);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateOnTopOfBars);
		m_canvasSortOrder.Add(CanvasLayerName.NameplateHealthBarMouseOver);
		nameplateItemEndIndex = m_canvasSortOrder.Count - 1;
		m_canvasSortOrder.Add(CanvasLayerName.MainHUDScreen);
		m_canvasSortOrder.Add(CanvasLayerName.AbilityBarBG);
		m_canvasSortOrder.Add(CanvasLayerName.InfoScreen);
		m_canvasSortOrder.Add(CanvasLayerName.TimeContainer);
		m_canvasSortOrder.Add(CanvasLayerName.AbilityButtons);
		m_canvasSortOrder.Add(CanvasLayerName.CardBar);
		m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectGameMode);
		m_canvasSortOrder.Add(CanvasLayerName.GameOverStatsWindow);
		m_canvasSortOrder.Add(CanvasLayerName.WaitingInQueueCanvas);
		m_canvasSortOrder.Add(CanvasLayerName.PersistantScreenOverlayCanvasBelow);
		m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectLeft);
		m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectRight);
		m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectSpells);
		m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectSkins);
		m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectAbilities);
		m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectBottomMenu);
		m_canvasSortOrder.Add(CanvasLayerName.FrontEndTopCanvas);
		m_canvasSortOrder.Add(CanvasLayerName.TextScrollView);
		m_canvasSortOrder.Add(CanvasLayerName.TopDisplayPanel);
		m_canvasSortOrder.Add(CanvasLayerName.GameSpecificDisplay);
		m_canvasSortOrder.Add(CanvasLayerName.PersistantScreenOverlayCanvas);
		m_canvasSortOrder.Add(CanvasLayerName.PersistantScreenOverlayCanvasTop);
		m_canvasSortOrder.Add(CanvasLayerName.NewReward);
		m_canvasSortOrder.Add(CanvasLayerName.CharacterProfileBackground);
		m_canvasSortOrder.Add(CanvasLayerName.CharacterShield);
		m_canvasSortOrder.Add(CanvasLayerName.CharacterHealth);
		m_canvasSortOrder.Add(CanvasLayerName.CharacterStatusLabels);
		m_canvasSortOrder.Add(CanvasLayerName.BuffTooltip);
		m_canvasSortOrder.Add(CanvasLayerName.TooltipContainer);
		m_canvasSortOrder.Add(CanvasLayerName.TutorialEnergyArrows);
		m_canvasSortOrder.Add(CanvasLayerName.TutorialBackground);
		m_canvasSortOrder.Add(CanvasLayerName.SystemMenu);
		m_canvasSortOrder.Add(CanvasLayerName.SystemMenuPanel);
		m_canvasSortOrder.Add(CanvasLayerName.OptionsMenu);
		m_canvasSortOrder.Add(CanvasLayerName.DefeatVictoryBackground);
		m_canvasSortOrder.Add(CanvasLayerName.FrontEndLoadingScreen);
	}

	private bool IsLayerNameplate(CanvasLayerName layerName)
	{
		int result;
		if (nameplateItemBeginIndex <= GetIndex(layerName))
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
			result = ((GetIndex(layerName) <= nameplateItemEndIndex) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private bool IsLayerNameplate(int layer)
	{
		int result;
		if (nameplateItemBeginIndex <= layer)
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
			result = ((layer <= nameplateItemEndIndex) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private int GetIndex(CanvasLayerName layerName)
	{
		int result = -1;
		int num = 0;
		using (List<CanvasLayerName>.Enumerator enumerator = m_canvasSortOrder.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CanvasLayerName current = enumerator.Current;
				if (current != layerName)
				{
					while (true)
					{
						switch (5)
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
					num++;
				}
				else
				{
					result = num;
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	public void UpdateNameplateOrder()
	{
		m_canvases.RemoveAll((_CanvasLayerSorter item) => item == null);
		using (List<_CanvasLayerSorter>.Enumerator enumerator = m_canvases.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				_CanvasLayerSorter current = enumerator.Current;
				Canvas canvas = current.GetCanvas();
				int index = GetIndex(current.m_layerName);
				if (IsLayerNameplate(index))
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					UINameplateItem componentInParent = current.GetComponentInParent<UINameplateItem>();
					if (componentInParent != null)
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
						int num = UIManager.Get().GetNameplateCanvasLayer() + componentInParent.GetSortOrder();
						if (canvas.sortingOrder != index + num)
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
							canvas.sortingOrder = index + num;
						}
					}
				}
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void AddCanvas(_CanvasLayerSorter newCanvas)
	{
		List<_CanvasLayerSorter> canvases = m_canvases;
		if (_003C_003Ef__am_0024cache1 == null)
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
			_003C_003Ef__am_0024cache1 = ((_CanvasLayerSorter item) => item == null);
		}
		canvases.RemoveAll(_003C_003Ef__am_0024cache1);
		if (m_canvases.Contains(newCanvas))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			m_canvases.Add(newCanvas);
			Canvas canvas = newCanvas.GetCanvas();
			int index = GetIndex(newCanvas.m_layerName);
			if (IsLayerNameplate(index))
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
				UINameplateItem componentInParent = newCanvas.GetComponentInParent<UINameplateItem>();
				if (componentInParent != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					canvas.sortingOrder = index + (nameplateItemEndIndex - nameplateItemBeginIndex + 1) * componentInParent.GetSortOrder();
				}
			}
			else if (index > nameplateItemEndIndex)
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
				canvas.sortingOrder = index + 500;
			}
			else
			{
				canvas.sortingOrder = index;
			}
			newCanvas.GetCanvas().overrideSorting = false;
			newCanvas.GetCanvas().overrideSorting = true;
			return;
		}
	}
}
