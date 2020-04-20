﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OverconsPanel : MonoBehaviour
{
	public _SelectableBtn m_panelOpenBtn;

	public RectTransform m_panelContainer;

	public CanvasGroup m_panelCanvasGroup;

	public Animator m_panelAnimController;

	public RectTransform[] m_overconLabels;

	public GridLayoutGroup m_overconsGridlayout;

	public OverconSelectBtn m_overconPrefab;

	[HideInInspector]
	public List<OverconSelectBtn> overconButtons;

	private bool m_panelOpen;

	private bool m_initialized;

	private ScrollRect m_scrollRect;

	private void Awake()
	{
		this.overconButtons = new List<OverconSelectBtn>();
		this.m_panelOpenBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.IconBtnClicked);
		this.Init();
		this.DoPanelOpen(false);
		ClientGameManager.Get().OnAccountDataUpdated += this.HandleAccountDataUpdated;
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= this.HandleAccountDataUpdated;
		}
	}

	private void Init()
	{
		if (this.m_initialized)
		{
			return;
		}
		this.m_initialized = true;
		this.m_scrollRect = base.GetComponentInChildren<ScrollRect>();
		bool flag;
		if (GameManager.Get() != null)
		{
			flag = GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		foreach (UIOverconData.NameToOverconEntry nameToOverconEntry in UIOverconData.Get().m_nameToOverconEntry)
		{
			bool flag3 = nameToOverconEntry.m_isHidden || !GameBalanceVarsExtensions.MeetsVisibilityConditions(nameToOverconEntry.CreateUnlockDataEntry());
			bool flag4 = ClientGameManager.Get().IsOverconUnlocked(nameToOverconEntry.m_overconId);
			if (!flag2)
			{
				if (flag3)
				{
					if (!flag4)
					{
						continue;
					}
				}
			}
			this.overconButtons.Add(this.CreateNewOverconBtn(nameToOverconEntry, flag4));
		}
		this.SortDisplayList();
	}

	public void UpdateGameState()
	{
		bool doActive = UIManager.Get().CurrentState == UIManager.ClientState.InGame;
		UIManager.SetGameObjectActive(base.gameObject, doActive, null);
		this.DoPanelOpen(false);
	}

	private OverconSelectBtn CreateNewOverconBtn(UIOverconData.NameToOverconEntry overcon, bool unlocked)
	{
		OverconSelectBtn overconSelectBtn = UnityEngine.Object.Instantiate<OverconSelectBtn>(this.m_overconPrefab);
		overconSelectBtn.transform.SetParent(this.m_overconsGridlayout.transform);
		overconSelectBtn.transform.localPosition = Vector3.zero;
		overconSelectBtn.transform.localEulerAngles = Vector3.zero;
		overconSelectBtn.transform.localScale = Vector3.one;
		overconSelectBtn.Setup(overcon, unlocked);
		if (this.m_scrollRect != null)
		{
			overconSelectBtn.m_selectableBtn.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		return overconSelectBtn;
	}

	private void OnScroll(BaseEventData data)
	{
		this.m_scrollRect.OnScroll((PointerEventData)data);
	}

	private void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		this.Init();
		bool flag;
		if (GameManager.Get() != null)
		{
			flag = GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		using (List<UIOverconData.NameToOverconEntry>.Enumerator enumerator = UIOverconData.Get().m_nameToOverconEntry.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIOverconData.NameToOverconEntry nameToOverconEntry = enumerator.Current;
				bool flag3;
				if (!nameToOverconEntry.m_isHidden)
				{
					flag3 = !GameBalanceVarsExtensions.MeetsVisibilityConditions(nameToOverconEntry.CreateUnlockDataEntry());
				}
				else
				{
					flag3 = true;
				}
				bool flag4 = flag3;
				bool flag5 = ClientGameManager.Get().IsOverconUnlocked(nameToOverconEntry.m_overconId);
				if (!flag2)
				{
					if (flag4)
					{
						if (!flag5)
						{
							continue;
						}
					}
				}
				bool flag6 = false;
				for (int i = 0; i < this.overconButtons.Count; i++)
				{
					if (this.overconButtons[i].GetOvercon() == nameToOverconEntry)
					{
						flag6 = true;
						this.overconButtons[i].Setup(nameToOverconEntry, flag5);
					}
				}
				if (!flag6)
				{
					this.overconButtons.Add(this.CreateNewOverconBtn(nameToOverconEntry, flag5));
				}
			}
		}
		this.SortDisplayList();
	}

	private void SortDisplayList()
	{
		this.overconButtons.Sort(new Comparison<OverconSelectBtn>(this.CompareOverconButton));
		for (int i = 0; i < this.overconButtons.Count; i++)
		{
			OverconSelectBtn overconSelectBtn = this.overconButtons[i];
			if (overconSelectBtn.transform.GetSiblingIndex() != i)
			{
				overconSelectBtn.transform.SetSiblingIndex(i);
			}
		}
	}

	private int CompareOverconButton(OverconSelectBtn first, OverconSelectBtn second)
	{
		if (first == null)
		{
			return -1;
		}
		if (second == null)
		{
			return 1;
		}
		if (first == null && second == null)
		{
			return 0;
		}
		if (first.IsUnlocked && !second.IsUnlocked)
		{
			return -1;
		}
		if (!first.IsUnlocked)
		{
			if (second.IsUnlocked)
			{
				return 1;
			}
		}
		UIOverconData.NameToOverconEntry overcon = first.GetOvercon();
		UIOverconData.NameToOverconEntry overcon2 = second.GetOvercon();
		if (overcon.m_sortOrder == -1)
		{
			return -1;
		}
		if (overcon2.m_sortOrder == -1)
		{
			return 1;
		}
		if (overcon.m_sortOrder < overcon2.m_sortOrder)
		{
			return -1;
		}
		if (overcon2.m_sortOrder < overcon.m_sortOrder)
		{
			return 1;
		}
		return 0;
	}

	public void IconBtnClicked(BaseEventData data)
	{
		this.SetPanelOpen(!this.m_panelOpen);
	}

	public void SetPanelOpen(bool open)
	{
		if (this.m_panelOpen != open)
		{
			this.DoPanelOpen(open);
		}
	}

	private void DoPanelOpen(bool open)
	{
		this.m_panelOpen = open;
		this.m_panelOpenBtn.SetSelected(this.m_panelOpen, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(this.m_panelContainer, true, null);
		this.m_panelCanvasGroup.interactable = this.m_panelOpen;
		this.m_panelCanvasGroup.blocksRaycasts = this.m_panelOpen;
		if (!this.m_panelOpen)
		{
			this.m_panelAnimController.Play("EmoticonPanelDefaultOUT");
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			bool flag = true;
			if (EventSystem.current != null)
			{
				if (EventSystem.current.IsPointerOverGameObject(-1))
				{
					StandaloneInputModuleWithEventDataAccess component = EventSystem.current.gameObject.GetComponent<StandaloneInputModuleWithEventDataAccess>();
					if (component != null)
					{
						if (component.GetLastPointerEventDataPublic(-1).pointerEnter != null)
						{
							OverconsPanel componentInParent = component.GetLastPointerEventDataPublic(-1).pointerEnter.GetComponentInParent<OverconsPanel>();
							if (componentInParent != null)
							{
								flag = false;
							}
						}
					}
				}
			}
			if (flag)
			{
				this.SetPanelOpen(false);
			}
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		GameManager gameManager = GameManager.Get();
		bool flag2;
		if (clientGameManager != null && clientGameManager.PlayerInfo != null)
		{
			if (clientGameManager.PlayerInfo.TeamId == Team.Spectator)
			{
				flag2 = true;
				goto IL_15E;
			}
		}
		if (gameManager != null)
		{
			if (gameManager.PlayerInfo != null)
			{
				flag2 = (gameManager.PlayerInfo.TeamId == Team.Spectator);
				goto IL_15B;
			}
		}
		flag2 = false;
		IL_15B:
		IL_15E:
		bool flag3 = flag2;
		bool flag4;
		if (gameManager != null)
		{
			if (gameManager.GameConfig != null)
			{
				flag4 = (gameManager.GameConfig.GameType == GameType.Tutorial);
				goto IL_19B;
			}
		}
		flag4 = false;
		IL_19B:
		bool flag5 = flag4;
		if (!flag3)
		{
			if (!flag5)
			{
				return;
			}
		}
		UIManager.SetGameObjectActive(base.gameObject, false, null);
	}
}
