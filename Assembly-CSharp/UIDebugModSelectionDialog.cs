using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

	private DialogButtonCallback onAcceptButton;

	public void Update()
	{
		if (!(m_gridLayout != null))
		{
			return;
		}
		while (true)
		{
			RectTransform component = m_gridLayout.GetComponent<RectTransform>();
			if (component != null)
			{
				while (true)
				{
					GridLayoutGroup gridLayout = m_gridLayout;
					float width = component.rect.width;
					Vector2 cellSize = m_gridLayout.cellSize;
					gridLayout.cellSize = new Vector2(width, cellSize.y);
					return;
				}
			}
			return;
		}
	}

	protected override void CloseCallback()
	{
	}

	public override void ClearCallback()
	{
		onAcceptButton = null;
	}

	public void OnAcceptClicked(BaseEventData data)
	{
		if (onAcceptButton != null)
		{
			onAcceptButton(this);
		}
		m_abilitiesPanel = null;
		UIDialogPopupManager.Get().CloseDialog(this);
	}

	public void Start()
	{
		if (!(m_firstButton != null))
		{
			return;
		}
		while (true)
		{
			UIEventTriggerUtils.AddListener(m_firstButton.gameObject, EventTriggerType.PointerClick, OnAcceptClicked);
			return;
		}
	}

	public void Setup(AbilityData.AbilityEntry inAbility, int inAbilityIndex, UICharacterAbilitiesPanel abilitiesPanel, DialogButtonCallback onAccept = null)
	{
		PersistedCharacterData characterData = ClientGameManager.Get().GetPlayerCharacterData(abilitiesPanel.m_selectedCharacter);
		onAcceptButton = onAccept;
		m_selectedAbilityIndex = inAbilityIndex;
		m_abilitiesPanel = abilitiesPanel;
		m_Title.text = inAbility.ability.m_abilityName;
		m_abilitiesPanel.UpdateModEquipPointsLeft(m_modPointsDisplay);
		reverseSort = new List<bool>();
		for (int i = 0; i < columnHeaderButtons.Length; i++)
		{
			Button button = columnHeaderButtons[i];
			if (button != null)
			{
				int closureCopy = i;
				UIEventTriggerUtils.AddListener(button.gameObject, EventTriggerType.PointerClick, delegate(BaseEventData x)
				{
					SortByColumn(x, closureCopy);
				});
				reverseSort.Insert(i, false);
			}
		}
		int selectedAbilitiesModIndex;
		float selectedY;
		int counter;
		while (true)
		{
			List<UIModSelectionGridEntry> list = new List<UIModSelectionGridEntry>(m_gridLayout.GetComponentsInChildren<UIModSelectionGridEntry>(true));
			List<AbilityMod> availableModsForAbility = AbilityModHelper.GetAvailableModsForAbility(inAbility.ability);
			selectedAbilitiesModIndex = m_abilitiesPanel.m_modInfo.GetModForAbility(inAbilityIndex);
			selectedY = 0f;
			counter = 0;
			if (_003C_003Ef__am_0024cache0 == null)
			{
				_003C_003Ef__am_0024cache0 = delegate(UIModSelectionGridEntry x)
				{
					Object.Destroy(x);
				};
			}
			list.ForEach(_003C_003Ef__am_0024cache0);
			availableModsForAbility.ForEach(delegate(AbilityMod x)
			{
				AbilityMod modForAbility = AbilityModHelper.GetModForAbility(inAbility.ability, selectedAbilitiesModIndex);
				UIModSelectionGridEntry uIModSelectionGridEntry = Object.Instantiate(m_modEntryPrefab);
				uIModSelectionGridEntry.transform.SetParent(m_gridLayout.transform);
				Transform transform = uIModSelectionGridEntry.transform;
				Vector3 localPosition = uIModSelectionGridEntry.transform.localPosition;
				float x3 = localPosition.x;
				Vector3 localPosition2 = uIModSelectionGridEntry.transform.localPosition;
				transform.localPosition = new Vector3(x3, localPosition2.y, 0f);
				uIModSelectionGridEntry.transform.localScale = Vector3.one;
				int num2;
				if (characterData != null)
				{
					num2 = ((characterData.CharacterComponent.IsModUnlocked(inAbilityIndex, x.m_abilityScopeId) || GameManager.Get().GameplayOverrides.EnableAllMods) ? 1 : 0);
				}
				else
				{
					num2 = 1;
				}
				bool flag = (byte)num2 != 0;
				bool flag2 = selectedAbilitiesModIndex == x.m_abilityScopeId && selectedAbilitiesModIndex != -1;
				flag = GameManager.Get().GameplayOverrides.IsAbilityModAllowed(characterData.CharacterType, inAbilityIndex, x.m_abilityScopeId);
				if (flag2)
				{
					m_currentSelectionTitle.text = x.m_name;
					m_currentSelectionDetails.text = x.GetUnlocalizedFullTooltip(inAbility.ability);
					m_selectedModIcon.sprite = ((!modForAbility) ? null : modForAbility.m_iconSprite);
					float num3 = counter;
					Vector2 cellSize2 = m_gridLayout.cellSize;
					selectedY = num3 * cellSize2.y;
				}
				counter++;
				uIModSelectionGridEntry.Setup(x, flag2, flag, OnSelected);
			});
			RectTransform component = m_gridLayout.GetComponent<RectTransform>();
			Vector2 sizeDelta = component.sizeDelta;
			float x2 = sizeDelta.x;
			float num = availableModsForAbility.Count;
			Vector2 cellSize = m_gridLayout.cellSize;
			Vector2 sizeDelta2 = new Vector2(x2, num * cellSize.y);
			float y = sizeDelta2.y;
			Vector2 sizeDelta3 = component.sizeDelta;
			if (y > sizeDelta3.y)
			{
				while (true)
				{
					component.sizeDelta = sizeDelta2;
					m_scrollBarRef.value = Mathf.Abs(selectedY / sizeDelta2.y - 1f);
					return;
				}
			}
			return;
		}
	}

	private void SortByColumn(BaseEventData baseEvent, int columnIndex)
	{
		List<UIModSelectionGridEntry> list = new List<UIModSelectionGridEntry>();
		list.AddRange(m_gridLayout.GetComponentsInChildren<UIModSelectionGridEntry>());
		List<UIModSelectionGridEntry> list2;
		if (columnIndex == 0)
		{
			if (_003C_003Ef__am_0024cache1 == null)
			{
				_003C_003Ef__am_0024cache1 = ((UIModSelectionGridEntry element) => element.m_ModName.text);
			}
			IOrderedEnumerable<UIModSelectionGridEntry> source = list.OrderBy(_003C_003Ef__am_0024cache1);
			list2 = source.ToList();
		}
		else
		{
			IOrderedEnumerable<UIModSelectionGridEntry> source2 = list.OrderBy((UIModSelectionGridEntry element) => element.m_ModPoints.text);
			list2 = source2.ToList();
		}
		bool flag = reverseSort[columnIndex];
		if (reverseSort[columnIndex])
		{
			list2.Reverse();
		}
		List<bool> source3 = reverseSort;
		if (_003C_003Ef__am_0024cache3 == null)
		{
			_003C_003Ef__am_0024cache3 = ((bool x) => x = false);
		}
		source3.All(_003C_003Ef__am_0024cache3);
		reverseSort[columnIndex] = !flag;
		foreach (UIModSelectionGridEntry item in list)
		{
			int siblingIndex = list2.IndexOf(item);
			item.transform.SetSiblingIndex(siblingIndex);
			item.transform.SetParent(m_gridLayout.transform);
		}
		m_gridLayout.gameObject.SetActive(false);
		m_gridLayout.gameObject.SetActive(true);
	}

	public void OnSelected(bool enabledSomething)
	{
		List<UIModSelectionGridEntry> list = new List<UIModSelectionGridEntry>(m_gridLayout.GetComponentsInChildren<UIModSelectionGridEntry>(true));
		if (enabledSomething)
		{
			using (List<UIModSelectionGridEntry>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIModSelectionGridEntry current = enumerator.Current;
					if (current.justSet && !m_abilitiesPanel.UnderTotalModEquipCost(current.associatedAbilityMod))
					{
						current.justSet = false;
						current.m_ModEnabled.isOn = false;
						m_abilitiesPanel.ShowOutOfModEquipPointsDialog();
						return;
					}
				}
			}
			using (List<UIModSelectionGridEntry>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					UIModSelectionGridEntry current2 = enumerator2.Current;
					current2.m_ModEnabled.isOn = current2.justSet;
					if (current2.m_ModEnabled.isOn)
					{
						if (!current2.justSet)
						{
							Text modPoints = current2.m_ModPoints;
							float r = current2.m_NotSelectedColor.r;
							float g = current2.m_NotSelectedColor.g;
							float b = current2.m_NotSelectedColor.b;
							Color color = current2.m_ModPoints.color;
							Color color2 = new Color(r, g, b, color.a);
							current2.m_ModName.color = color2;
							modPoints.color = color2;
						}
					}
					if (current2.justSet)
					{
						m_currentSelectionDetails.text = current2.associatedAbilityMod.GetUnlocalizedFullTooltip(null);
						m_currentSelectionTitle.text = current2.associatedAbilityMod.m_name;
						m_abilitiesPanel.m_selectedAbilityButton.SetSelectedMod(current2.associatedAbilityMod);
						m_abilitiesPanel.m_selectedAbilityButton.SetSelectedModIndex(current2.associatedAbilityMod.m_abilityScopeId);
						m_abilitiesPanel.m_modInfo.SetModForAbility(m_selectedAbilityIndex, current2.associatedAbilityMod.m_abilityScopeId);
						m_selectedModIcon.sprite = current2.associatedAbilityMod.m_iconSprite;
						AppState_CharacterSelect.Get().UpdateSelectedMods(m_abilitiesPanel.m_modInfo);
						UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
					}
					current2.justSet = false;
				}
			}
		}
		else
		{
			foreach (UIModSelectionGridEntry item in list)
			{
				if (item.m_ModEnabled.isOn)
				{
					while (true)
					{
						switch (1)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
			}
			m_currentSelectionDetails.text = "No mod selected";
			m_currentSelectionTitle.text = string.Empty;
			m_abilitiesPanel.m_selectedAbilityButton.SetSelectedMod(null);
			m_abilitiesPanel.m_selectedAbilityButton.SetSelectedModIndex(-1);
			m_abilitiesPanel.m_modInfo.SetModForAbility(m_selectedAbilityIndex, -1);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModClear);
			AppState_CharacterSelect.Get().UpdateSelectedMods(m_abilitiesPanel.m_modInfo);
		}
		m_abilitiesPanel.UpdateModCounter();
		m_abilitiesPanel.UpdateModEquipPointsLeft(m_modPointsDisplay);
	}
}
