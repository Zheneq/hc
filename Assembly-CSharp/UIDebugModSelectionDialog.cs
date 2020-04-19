using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDebugModSelectionDialog : UIDialogBox
{
	private UICharacterAbilitiesPanel m_abilitiesPanel;

	public Text m_Title;

	public Text m_modPointsDisplay;

	public Text m_currentSelectionDetails;

	public Text m_currentSelectionTitle;

	public Button m_firstButton;

	public Text m_firstButtonLabel;

	public GridLayoutGroup m_gridLayout;

	public Button[] columnHeaderButtons;

	private List<bool> reverseSort;

	public Scrollbar m_scrollBarRef;

	public Image m_selectedModIcon;

	public UIModSelectionGridEntry m_modEntryPrefab;

	[HideInInspector]
	public AbilityData.AbilityEntry m_selectedAbility;

	[HideInInspector]
	public int m_selectedAbilityIndex;

	private UIDialogBox.DialogButtonCallback onAcceptButton;

	public void Update()
	{
		if (this.m_gridLayout != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDebugModSelectionDialog.Update()).MethodHandle;
			}
			RectTransform component = this.m_gridLayout.GetComponent<RectTransform>();
			if (component != null)
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
				this.m_gridLayout.cellSize = new Vector2(component.rect.width, this.m_gridLayout.cellSize.y);
			}
		}
	}

	protected override void CloseCallback()
	{
	}

	public override void ClearCallback()
	{
		this.onAcceptButton = null;
	}

	public void OnAcceptClicked(BaseEventData data)
	{
		if (this.onAcceptButton != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDebugModSelectionDialog.OnAcceptClicked(BaseEventData)).MethodHandle;
			}
			this.onAcceptButton(this);
		}
		this.m_abilitiesPanel = null;
		UIDialogPopupManager.Get().CloseDialog(this);
	}

	public void Start()
	{
		if (this.m_firstButton != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDebugModSelectionDialog.Start()).MethodHandle;
			}
			UIEventTriggerUtils.AddListener(this.m_firstButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnAcceptClicked));
		}
	}

	public void Setup(AbilityData.AbilityEntry inAbility, int inAbilityIndex, UICharacterAbilitiesPanel abilitiesPanel, UIDialogBox.DialogButtonCallback onAccept = null)
	{
		PersistedCharacterData characterData = ClientGameManager.Get().GetPlayerCharacterData(abilitiesPanel.m_selectedCharacter);
		this.onAcceptButton = onAccept;
		this.m_selectedAbilityIndex = inAbilityIndex;
		this.m_abilitiesPanel = abilitiesPanel;
		this.m_Title.text = inAbility.ability.m_abilityName;
		this.m_abilitiesPanel.UpdateModEquipPointsLeft(this.m_modPointsDisplay);
		this.reverseSort = new List<bool>();
		for (int i = 0; i < this.columnHeaderButtons.Length; i++)
		{
			Button button = this.columnHeaderButtons[i];
			if (button != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIDebugModSelectionDialog.Setup(AbilityData.AbilityEntry, int, UICharacterAbilitiesPanel, UIDialogBox.DialogButtonCallback)).MethodHandle;
				}
				int closureCopy = i;
				UIEventTriggerUtils.AddListener(button.gameObject, EventTriggerType.PointerClick, delegate(BaseEventData x)
				{
					this.SortByColumn(x, closureCopy);
				});
				this.reverseSort.Insert(i, false);
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		List<UIModSelectionGridEntry> list = new List<UIModSelectionGridEntry>(this.m_gridLayout.GetComponentsInChildren<UIModSelectionGridEntry>(true));
		List<AbilityMod> availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(inAbility.ability);
		int selectedAbilitiesModIndex = this.m_abilitiesPanel.m_modInfo.GetModForAbility(inAbilityIndex);
		float selectedY = 0f;
		int counter = 0;
		List<UIModSelectionGridEntry> list2 = list;
		if (UIDebugModSelectionDialog.<>f__am$cache0 == null)
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
			UIDebugModSelectionDialog.<>f__am$cache0 = delegate(UIModSelectionGridEntry x)
			{
				UnityEngine.Object.Destroy(x);
			};
		}
		list2.ForEach(UIDebugModSelectionDialog.<>f__am$cache0);
		availableModsForAbility.ForEach(delegate(AbilityMod x)
		{
			AbilityMod modForAbility = AbilityModHelper.GetModForAbility(inAbility.ability, selectedAbilitiesModIndex);
			UIModSelectionGridEntry uimodSelectionGridEntry = UnityEngine.Object.Instantiate<UIModSelectionGridEntry>(this.m_modEntryPrefab);
			uimodSelectionGridEntry.transform.SetParent(this.m_gridLayout.transform);
			uimodSelectionGridEntry.transform.localPosition = new Vector3(uimodSelectionGridEntry.transform.localPosition.x, uimodSelectionGridEntry.transform.localPosition.y, 0f);
			uimodSelectionGridEntry.transform.localScale = Vector3.one;
			if (characterData == null)
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
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIDebugModSelectionDialog.<Setup>c__AnonStorey1.<>m__0(AbilityMod)).MethodHandle;
				}
			}
			else
			{
				bool flag = characterData.CharacterComponent.IsModUnlocked(inAbilityIndex, x.m_abilityScopeId) || GameManager.Get().GameplayOverrides.EnableAllMods;
			}
			bool flag2 = selectedAbilitiesModIndex == x.m_abilityScopeId && selectedAbilitiesModIndex != -1;
			bool isUnlocked = GameManager.Get().GameplayOverrides.IsAbilityModAllowed(characterData.CharacterType, inAbilityIndex, x.m_abilityScopeId);
			if (flag2)
			{
				this.m_currentSelectionTitle.text = x.m_name;
				this.m_currentSelectionDetails.text = x.GetUnlocalizedFullTooltip(inAbility.ability);
				this.m_selectedModIcon.sprite = ((!modForAbility) ? null : modForAbility.m_iconSprite);
				selectedY = (float)counter * this.m_gridLayout.cellSize.y;
			}
			counter++;
			uimodSelectionGridEntry.Setup(x, flag2, isUnlocked, new UnityAction<bool>(this.OnSelected));
		});
		RectTransform component = this.m_gridLayout.GetComponent<RectTransform>();
		Vector2 sizeDelta = new Vector2(component.sizeDelta.x, (float)availableModsForAbility.Count * this.m_gridLayout.cellSize.y);
		if (sizeDelta.y > component.sizeDelta.y)
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
			component.sizeDelta = sizeDelta;
			this.m_scrollBarRef.value = Mathf.Abs(selectedY / sizeDelta.y - 1f);
		}
	}

	private void SortByColumn(BaseEventData baseEvent, int columnIndex)
	{
		List<UIModSelectionGridEntry> list = new List<UIModSelectionGridEntry>();
		list.AddRange(this.m_gridLayout.GetComponentsInChildren<UIModSelectionGridEntry>());
		List<UIModSelectionGridEntry> list2;
		if (columnIndex == 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDebugModSelectionDialog.SortByColumn(BaseEventData, int)).MethodHandle;
			}
			IEnumerable<UIModSelectionGridEntry> source = list;
			if (UIDebugModSelectionDialog.<>f__am$cache1 == null)
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
				UIDebugModSelectionDialog.<>f__am$cache1 = ((UIModSelectionGridEntry element) => element.m_ModName.text);
			}
			IOrderedEnumerable<UIModSelectionGridEntry> source2 = source.OrderBy(UIDebugModSelectionDialog.<>f__am$cache1);
			list2 = source2.ToList<UIModSelectionGridEntry>();
		}
		else
		{
			IOrderedEnumerable<UIModSelectionGridEntry> source3 = from element in list
			orderby element.m_ModPoints.text
			select element;
			list2 = source3.ToList<UIModSelectionGridEntry>();
		}
		bool flag = this.reverseSort[columnIndex];
		if (this.reverseSort[columnIndex])
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
			list2.Reverse();
		}
		IEnumerable<bool> source4 = this.reverseSort;
		if (UIDebugModSelectionDialog.<>f__am$cache3 == null)
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
			UIDebugModSelectionDialog.<>f__am$cache3 = ((bool x) => false);
		}
		source4.All(UIDebugModSelectionDialog.<>f__am$cache3);
		this.reverseSort[columnIndex] = !flag;
		foreach (UIModSelectionGridEntry uimodSelectionGridEntry in list)
		{
			int siblingIndex = list2.IndexOf(uimodSelectionGridEntry);
			uimodSelectionGridEntry.transform.SetSiblingIndex(siblingIndex);
			uimodSelectionGridEntry.transform.SetParent(this.m_gridLayout.transform);
		}
		this.m_gridLayout.gameObject.SetActive(false);
		this.m_gridLayout.gameObject.SetActive(true);
	}

	public void OnSelected(bool enabledSomething)
	{
		List<UIModSelectionGridEntry> list = new List<UIModSelectionGridEntry>(this.m_gridLayout.GetComponentsInChildren<UIModSelectionGridEntry>(true));
		if (enabledSomething)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIDebugModSelectionDialog.OnSelected(bool)).MethodHandle;
			}
			using (List<UIModSelectionGridEntry>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIModSelectionGridEntry uimodSelectionGridEntry = enumerator.Current;
					if (uimodSelectionGridEntry.justSet && !this.m_abilitiesPanel.UnderTotalModEquipCost(uimodSelectionGridEntry.associatedAbilityMod))
					{
						uimodSelectionGridEntry.justSet = false;
						uimodSelectionGridEntry.m_ModEnabled.isOn = false;
						this.m_abilitiesPanel.ShowOutOfModEquipPointsDialog();
						return;
					}
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			using (List<UIModSelectionGridEntry>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					UIModSelectionGridEntry uimodSelectionGridEntry2 = enumerator2.Current;
					uimodSelectionGridEntry2.m_ModEnabled.isOn = uimodSelectionGridEntry2.justSet;
					if (uimodSelectionGridEntry2.m_ModEnabled.isOn)
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
						if (!uimodSelectionGridEntry2.justSet)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							Graphic modPoints = uimodSelectionGridEntry2.m_ModPoints;
							Color color = new Color(uimodSelectionGridEntry2.m_NotSelectedColor.r, uimodSelectionGridEntry2.m_NotSelectedColor.g, uimodSelectionGridEntry2.m_NotSelectedColor.b, uimodSelectionGridEntry2.m_ModPoints.color.a);
							uimodSelectionGridEntry2.m_ModName.color = color;
							modPoints.color = color;
						}
					}
					if (uimodSelectionGridEntry2.justSet)
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
						this.m_currentSelectionDetails.text = uimodSelectionGridEntry2.associatedAbilityMod.GetUnlocalizedFullTooltip(null);
						this.m_currentSelectionTitle.text = uimodSelectionGridEntry2.associatedAbilityMod.m_name;
						this.m_abilitiesPanel.m_selectedAbilityButton.SetSelectedMod(uimodSelectionGridEntry2.associatedAbilityMod);
						this.m_abilitiesPanel.m_selectedAbilityButton.SetSelectedModIndex(uimodSelectionGridEntry2.associatedAbilityMod.m_abilityScopeId);
						this.m_abilitiesPanel.m_modInfo.SetModForAbility(this.m_selectedAbilityIndex, uimodSelectionGridEntry2.associatedAbilityMod.m_abilityScopeId);
						this.m_selectedModIcon.sprite = uimodSelectionGridEntry2.associatedAbilityMod.m_iconSprite;
						AppState_CharacterSelect.Get().UpdateSelectedMods(this.m_abilitiesPanel.m_modInfo);
						UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
					}
					uimodSelectionGridEntry2.justSet = false;
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
			}
		}
		else
		{
			foreach (UIModSelectionGridEntry uimodSelectionGridEntry3 in list)
			{
				if (uimodSelectionGridEntry3.m_ModEnabled.isOn)
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
					return;
				}
			}
			this.m_currentSelectionDetails.text = "No mod selected";
			this.m_currentSelectionTitle.text = string.Empty;
			this.m_abilitiesPanel.m_selectedAbilityButton.SetSelectedMod(null);
			this.m_abilitiesPanel.m_selectedAbilityButton.SetSelectedModIndex(-1);
			this.m_abilitiesPanel.m_modInfo.SetModForAbility(this.m_selectedAbilityIndex, -1);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModClear);
			AppState_CharacterSelect.Get().UpdateSelectedMods(this.m_abilitiesPanel.m_modInfo);
		}
		this.m_abilitiesPanel.UpdateModCounter();
		this.m_abilitiesPanel.UpdateModEquipPointsLeft(this.m_modPointsDisplay);
	}
}
