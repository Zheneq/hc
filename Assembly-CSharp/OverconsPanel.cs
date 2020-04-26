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
		overconButtons = new List<OverconSelectBtn>();
		m_panelOpenBtn.spriteController.callback = IconBtnClicked;
		Init();
		DoPanelOpen(false);
		ClientGameManager.Get().OnAccountDataUpdated += HandleAccountDataUpdated;
	}

	private void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= HandleAccountDataUpdated;
			return;
		}
	}

	private void Init()
	{
		if (m_initialized)
		{
			return;
		}
		m_initialized = true;
		m_scrollRect = GetComponentInChildren<ScrollRect>();
		int num;
		if (GameManager.Get() != null)
		{
			num = (GameManager.Get().GameplayOverrides.EnableHiddenCharacters ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		foreach (UIOverconData.NameToOverconEntry item in UIOverconData.Get().m_nameToOverconEntry)
		{
			bool flag2 = item.m_isHidden || !GameBalanceVarsExtensions.MeetsVisibilityConditions(item.CreateUnlockDataEntry());
			bool flag3 = ClientGameManager.Get().IsOverconUnlocked(item.m_overconId);
			if (!flag)
			{
				if (flag2)
				{
					if (!flag3)
					{
						continue;
					}
				}
			}
			overconButtons.Add(CreateNewOverconBtn(item, flag3));
		}
		SortDisplayList();
	}

	public void UpdateGameState()
	{
		bool doActive = UIManager.Get().CurrentState == UIManager.ClientState.InGame;
		UIManager.SetGameObjectActive(base.gameObject, doActive);
		DoPanelOpen(false);
	}

	private OverconSelectBtn CreateNewOverconBtn(UIOverconData.NameToOverconEntry overcon, bool unlocked)
	{
		OverconSelectBtn overconSelectBtn = Object.Instantiate(m_overconPrefab);
		overconSelectBtn.transform.SetParent(m_overconsGridlayout.transform);
		overconSelectBtn.transform.localPosition = Vector3.zero;
		overconSelectBtn.transform.localEulerAngles = Vector3.zero;
		overconSelectBtn.transform.localScale = Vector3.one;
		overconSelectBtn.Setup(overcon, unlocked);
		if (m_scrollRect != null)
		{
			overconSelectBtn.m_selectableBtn.spriteController.RegisterScrollListener(OnScroll);
		}
		return overconSelectBtn;
	}

	private void OnScroll(BaseEventData data)
	{
		m_scrollRect.OnScroll((PointerEventData)data);
	}

	private void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		Init();
		int num;
		if (GameManager.Get() != null)
		{
			num = (GameManager.Get().GameplayOverrides.EnableHiddenCharacters ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		using (List<UIOverconData.NameToOverconEntry>.Enumerator enumerator = UIOverconData.Get().m_nameToOverconEntry.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIOverconData.NameToOverconEntry current = enumerator.Current;
				int num2;
				if (!current.m_isHidden)
				{
					num2 = ((!GameBalanceVarsExtensions.MeetsVisibilityConditions(current.CreateUnlockDataEntry())) ? 1 : 0);
				}
				else
				{
					num2 = 1;
				}
				bool flag2 = (byte)num2 != 0;
				bool flag3 = ClientGameManager.Get().IsOverconUnlocked(current.m_overconId);
				if (!flag)
				{
					if (flag2)
					{
						if (!flag3)
						{
							continue;
						}
					}
				}
				bool flag4 = false;
				for (int i = 0; i < overconButtons.Count; i++)
				{
					if (overconButtons[i].GetOvercon() == current)
					{
						flag4 = true;
						overconButtons[i].Setup(current, flag3);
					}
				}
				if (!flag4)
				{
					overconButtons.Add(CreateNewOverconBtn(current, flag3));
				}
			}
		}
		SortDisplayList();
	}

	private void SortDisplayList()
	{
		overconButtons.Sort(CompareOverconButton);
		for (int i = 0; i < overconButtons.Count; i++)
		{
			OverconSelectBtn overconSelectBtn = overconButtons[i];
			if (overconSelectBtn.transform.GetSiblingIndex() != i)
			{
				overconSelectBtn.transform.SetSiblingIndex(i);
			}
		}
		while (true)
		{
			return;
		}
	}

	private int CompareOverconButton(OverconSelectBtn first, OverconSelectBtn second)
	{
		if (first == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return -1;
				}
			}
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
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return 1;
					}
				}
			}
		}
		UIOverconData.NameToOverconEntry overcon = first.GetOvercon();
		UIOverconData.NameToOverconEntry overcon2 = second.GetOvercon();
		if (overcon.m_sortOrder == -1)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return -1;
				}
			}
		}
		if (overcon2.m_sortOrder == -1)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return 1;
				}
			}
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
		SetPanelOpen(!m_panelOpen);
	}

	public void SetPanelOpen(bool open)
	{
		if (m_panelOpen == open)
		{
			return;
		}
		while (true)
		{
			DoPanelOpen(open);
			return;
		}
	}

	private void DoPanelOpen(bool open)
	{
		m_panelOpen = open;
		m_panelOpenBtn.SetSelected(m_panelOpen, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(m_panelContainer, true);
		m_panelCanvasGroup.interactable = m_panelOpen;
		m_panelCanvasGroup.blocksRaycasts = m_panelOpen;
		if (!m_panelOpen)
		{
			m_panelAnimController.Play("EmoticonPanelDefaultOUT");
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
				SetPanelOpen(false);
			}
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		GameManager gameManager = GameManager.Get();
		int num;
		if (clientGameManager != null && clientGameManager.PlayerInfo != null)
		{
			if (clientGameManager.PlayerInfo.TeamId == Team.Spectator)
			{
				num = 1;
				goto IL_015e;
			}
		}
		if (gameManager != null)
		{
			if (gameManager.PlayerInfo != null)
			{
				num = ((gameManager.PlayerInfo.TeamId == Team.Spectator) ? 1 : 0);
				goto IL_015e;
			}
		}
		num = 0;
		goto IL_015e;
		IL_015e:
		bool flag2 = (byte)num != 0;
		int num2;
		if (gameManager != null)
		{
			if (gameManager.GameConfig != null)
			{
				num2 = ((gameManager.GameConfig.GameType == GameType.Tutorial) ? 1 : 0);
				goto IL_019b;
			}
		}
		num2 = 0;
		goto IL_019b;
		IL_019b:
		bool flag3 = (byte)num2 != 0;
		if (!flag2)
		{
			if (!flag3)
			{
				return;
			}
		}
		UIManager.SetGameObjectActive(base.gameObject, false);
	}
}
