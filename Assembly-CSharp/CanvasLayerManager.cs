using System;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLayerManager
{
	private List<_CanvasLayerSorter> m_canvases;

	private List<CanvasLayerName> m_canvasSortOrder;

	private int nameplateItemBeginIndex;

	private int nameplateItemEndIndex;

	private const int c_nameplatePadding = 0x1F4;

	private bool m_addedNewNameplate;

	private int m_refreshCounter;

	private static CanvasLayerManager s_instance;

	private CanvasLayerManager()
	{
		this.m_canvases = new List<_CanvasLayerSorter>();
		this.CreateSortOrder();
	}

	public static CanvasLayerManager Get()
	{
		if (CanvasLayerManager.s_instance == null)
		{
			CanvasLayerManager.s_instance = new CanvasLayerManager();
		}
		return CanvasLayerManager.s_instance;
	}

	~CanvasLayerManager()
	{
		CanvasLayerManager.s_instance = null;
	}

	public void NotifyAddedNewNameplate()
	{
		this.m_addedNewNameplate = true;
		this.m_refreshCounter = 0;
	}

	public void Update()
	{
		if (this.m_addedNewNameplate)
		{
			foreach (_CanvasLayerSorter canvasLayerSorter in this.m_canvases)
			{
				canvasLayerSorter.DoCanvasRefresh();
			}
			this.m_refreshCounter++;
			if (this.m_refreshCounter > 1)
			{
				this.m_addedNewNameplate = false;
			}
		}
	}

	private void CreateSortOrder()
	{
		this.m_canvasSortOrder = new List<CanvasLayerName>();
		this.m_canvasSortOrder.Add(CanvasLayerName.None);
		this.m_canvasSortOrder.Add(CanvasLayerName.InputBlock);
		this.m_canvasSortOrder.Add(CanvasLayerName.PersistantWorldCanvas);
		this.m_canvasSortOrder.Add(CanvasLayerName.TutorialPanel);
		this.m_canvasSortOrder.Add(CanvasLayerName.OffscreenIndicatorPanel);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplatePanel);
		this.nameplateItemBeginIndex = this.m_canvasSortOrder.Count;
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateItem);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateTargetGlow);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateBG);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateArrowBG);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateTeamColor);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateResolutionBar);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateDamageEaseBar);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateHPGainBar);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateShieldMask);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateCurrentHP);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateTechPointBarEase);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateTechPointBar);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateTickContainer);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateOnTopOfBars);
		this.m_canvasSortOrder.Add(CanvasLayerName.NameplateHealthBarMouseOver);
		this.nameplateItemEndIndex = this.m_canvasSortOrder.Count - 1;
		this.m_canvasSortOrder.Add(CanvasLayerName.MainHUDScreen);
		this.m_canvasSortOrder.Add(CanvasLayerName.AbilityBarBG);
		this.m_canvasSortOrder.Add(CanvasLayerName.InfoScreen);
		this.m_canvasSortOrder.Add(CanvasLayerName.TimeContainer);
		this.m_canvasSortOrder.Add(CanvasLayerName.AbilityButtons);
		this.m_canvasSortOrder.Add(CanvasLayerName.CardBar);
		this.m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectGameMode);
		this.m_canvasSortOrder.Add(CanvasLayerName.GameOverStatsWindow);
		this.m_canvasSortOrder.Add(CanvasLayerName.WaitingInQueueCanvas);
		this.m_canvasSortOrder.Add(CanvasLayerName.PersistantScreenOverlayCanvasBelow);
		this.m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectLeft);
		this.m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectRight);
		this.m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectSpells);
		this.m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectSkins);
		this.m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectAbilities);
		this.m_canvasSortOrder.Add(CanvasLayerName.CharacterSelectBottomMenu);
		this.m_canvasSortOrder.Add(CanvasLayerName.FrontEndTopCanvas);
		this.m_canvasSortOrder.Add(CanvasLayerName.TextScrollView);
		this.m_canvasSortOrder.Add(CanvasLayerName.TopDisplayPanel);
		this.m_canvasSortOrder.Add(CanvasLayerName.GameSpecificDisplay);
		this.m_canvasSortOrder.Add(CanvasLayerName.PersistantScreenOverlayCanvas);
		this.m_canvasSortOrder.Add(CanvasLayerName.PersistantScreenOverlayCanvasTop);
		this.m_canvasSortOrder.Add(CanvasLayerName.NewReward);
		this.m_canvasSortOrder.Add(CanvasLayerName.CharacterProfileBackground);
		this.m_canvasSortOrder.Add(CanvasLayerName.CharacterShield);
		this.m_canvasSortOrder.Add(CanvasLayerName.CharacterHealth);
		this.m_canvasSortOrder.Add(CanvasLayerName.CharacterStatusLabels);
		this.m_canvasSortOrder.Add(CanvasLayerName.BuffTooltip);
		this.m_canvasSortOrder.Add(CanvasLayerName.TooltipContainer);
		this.m_canvasSortOrder.Add(CanvasLayerName.TutorialEnergyArrows);
		this.m_canvasSortOrder.Add(CanvasLayerName.TutorialBackground);
		this.m_canvasSortOrder.Add(CanvasLayerName.SystemMenu);
		this.m_canvasSortOrder.Add(CanvasLayerName.SystemMenuPanel);
		this.m_canvasSortOrder.Add(CanvasLayerName.OptionsMenu);
		this.m_canvasSortOrder.Add(CanvasLayerName.DefeatVictoryBackground);
		this.m_canvasSortOrder.Add(CanvasLayerName.FrontEndLoadingScreen);
	}

	private bool IsLayerNameplate(CanvasLayerName layerName)
	{
		bool result;
		if (this.nameplateItemBeginIndex <= this.GetIndex(layerName))
		{
			result = (this.GetIndex(layerName) <= this.nameplateItemEndIndex);
		}
		else
		{
			result = false;
		}
		return result;
	}

	private bool IsLayerNameplate(int layer)
	{
		bool result;
		if (this.nameplateItemBeginIndex <= layer)
		{
			result = (layer <= this.nameplateItemEndIndex);
		}
		else
		{
			result = false;
		}
		return result;
	}

	private int GetIndex(CanvasLayerName layerName)
	{
		int result = -1;
		int num = 0;
		using (List<CanvasLayerName>.Enumerator enumerator = this.m_canvasSortOrder.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				CanvasLayerName canvasLayerName = enumerator.Current;
				if (canvasLayerName != layerName)
				{
					num++;
				}
				else
				{
					result = num;
				}
			}
		}
		return result;
	}

	public void UpdateNameplateOrder()
	{
		this.m_canvases.RemoveAll((_CanvasLayerSorter item) => item == null);
		using (List<_CanvasLayerSorter>.Enumerator enumerator = this.m_canvases.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				_CanvasLayerSorter canvasLayerSorter = enumerator.Current;
				Canvas canvas = canvasLayerSorter.GetCanvas();
				int index = this.GetIndex(canvasLayerSorter.m_layerName);
				if (this.IsLayerNameplate(index))
				{
					UINameplateItem componentInParent = canvasLayerSorter.GetComponentInParent<UINameplateItem>();
					if (componentInParent != null)
					{
						int num = UIManager.Get().GetNameplateCanvasLayer() + componentInParent.GetSortOrder();
						if (canvas.sortingOrder != index + num)
						{
							canvas.sortingOrder = index + num;
						}
					}
				}
			}
		}
	}

	public void AddCanvas(_CanvasLayerSorter newCanvas)
	{
		List<_CanvasLayerSorter> canvases = this.m_canvases;
		
		canvases.RemoveAll(((_CanvasLayerSorter item) => item == null));
		if (!this.m_canvases.Contains(newCanvas))
		{
			this.m_canvases.Add(newCanvas);
			Canvas canvas = newCanvas.GetCanvas();
			int index = this.GetIndex(newCanvas.m_layerName);
			if (this.IsLayerNameplate(index))
			{
				UINameplateItem componentInParent = newCanvas.GetComponentInParent<UINameplateItem>();
				if (componentInParent != null)
				{
					canvas.sortingOrder = index + (this.nameplateItemEndIndex - this.nameplateItemBeginIndex + 1) * componentInParent.GetSortOrder();
				}
			}
			else if (index > this.nameplateItemEndIndex)
			{
				canvas.sortingOrder = index + 0x1F4;
			}
			else
			{
				canvas.sortingOrder = index;
			}
			newCanvas.GetCanvas().overrideSorting = false;
			newCanvas.GetCanvas().overrideSorting = true;
		}
	}
}
