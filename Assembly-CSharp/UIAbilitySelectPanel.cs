using TMPro;
using UnityEngine;

public class UIAbilitySelectPanel : MonoBehaviour
{
	public _SelectableBtn m_ability1Btn;

	public _SelectableBtn m_ability2Btn;

	public _SelectableBtn m_ability3Btn;

	public _SelectableBtn m_ability4Btn;

	public _SelectableBtn m_ability5Btn;

	public _SelectableBtn m_catalyst1Btn;

	public _SelectableBtn m_catalyst2Btn;

	public _SelectableBtn m_catalyst3Btn;

	public CanvasGroup m_closeButtonHover;

	public GameObject m_closeButton;

	public GameObject m_line;

	private KeyPreference m_hoverAbility;

	private void Awake()
	{
	}

	public void Init(AbilityData abilityData)
	{
		AbilityData.AbilityEntry[] abilityEntries = abilityData.abilityEntries;
		TextMeshProUGUI[] componentsInChildren = m_ability1Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
		TextMeshProUGUI[] array = componentsInChildren;
		foreach (TextMeshProUGUI textMeshProUGUI in array)
		{
			textMeshProUGUI.text = abilityEntries[0].ability.GetNameString();
		}
		TextMeshProUGUI[] componentsInChildren2 = m_ability2Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
		TextMeshProUGUI[] array2 = componentsInChildren2;
		foreach (TextMeshProUGUI textMeshProUGUI2 in array2)
		{
			textMeshProUGUI2.text = abilityEntries[1].ability.GetNameString();
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			TextMeshProUGUI[] componentsInChildren3 = m_ability3Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
			TextMeshProUGUI[] array3 = componentsInChildren3;
			foreach (TextMeshProUGUI textMeshProUGUI3 in array3)
			{
				textMeshProUGUI3.text = abilityEntries[2].ability.GetNameString();
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				TextMeshProUGUI[] componentsInChildren4 = m_ability4Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
				TextMeshProUGUI[] array4 = componentsInChildren4;
				foreach (TextMeshProUGUI textMeshProUGUI4 in array4)
				{
					textMeshProUGUI4.text = abilityEntries[3].ability.GetNameString();
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					TextMeshProUGUI[] componentsInChildren5 = m_ability5Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
					TextMeshProUGUI[] array5 = componentsInChildren5;
					foreach (TextMeshProUGUI textMeshProUGUI5 in array5)
					{
						textMeshProUGUI5.text = abilityEntries[4].ability.GetNameString();
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						if (abilityEntries[7].ability == null)
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
							UIManager.SetGameObjectActive(m_catalyst1Btn, false);
						}
						else
						{
							UIManager.SetGameObjectActive(m_catalyst1Btn, true);
							TextMeshProUGUI[] componentsInChildren6 = m_catalyst1Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
							TextMeshProUGUI[] array6 = componentsInChildren6;
							foreach (TextMeshProUGUI textMeshProUGUI6 in array6)
							{
								textMeshProUGUI6.text = abilityEntries[7].ability.GetNameString();
							}
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						if (abilityEntries[8].ability == null)
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
							UIManager.SetGameObjectActive(m_catalyst2Btn, false);
						}
						else
						{
							UIManager.SetGameObjectActive(m_catalyst2Btn, true);
							TextMeshProUGUI[] componentsInChildren7 = m_catalyst2Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
							TextMeshProUGUI[] array7 = componentsInChildren7;
							foreach (TextMeshProUGUI textMeshProUGUI7 in array7)
							{
								textMeshProUGUI7.text = abilityEntries[8].ability.GetNameString();
							}
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						if (abilityEntries[9].ability == null)
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
							UIManager.SetGameObjectActive(m_catalyst3Btn, false);
						}
						else
						{
							UIManager.SetGameObjectActive(m_catalyst3Btn, true);
							TextMeshProUGUI[] componentsInChildren8 = m_catalyst3Btn.gameObject.GetComponentsInChildren<TextMeshProUGUI>();
							TextMeshProUGUI[] array8 = componentsInChildren8;
							foreach (TextMeshProUGUI textMeshProUGUI8 in array8)
							{
								textMeshProUGUI8.text = abilityEntries[9].ability.GetNameString();
							}
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						m_ability1Btn.SetSelected(false, false, string.Empty, string.Empty);
						m_ability2Btn.SetSelected(false, false, string.Empty, string.Empty);
						m_ability3Btn.SetSelected(false, false, string.Empty, string.Empty);
						m_ability4Btn.SetSelected(false, false, string.Empty, string.Empty);
						m_ability5Btn.SetSelected(false, false, string.Empty, string.Empty);
						m_catalyst1Btn.SetSelected(false, false, string.Empty, string.Empty);
						m_catalyst2Btn.SetSelected(false, false, string.Empty, string.Empty);
						m_catalyst3Btn.SetSelected(false, false, string.Empty, string.Empty);
						m_ability1Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[0].m_disabled.gameObject.activeSelf);
						m_ability2Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[1].m_disabled.gameObject.activeSelf);
						m_ability3Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[2].m_disabled.gameObject.activeSelf);
						m_ability4Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[3].m_disabled.gameObject.activeSelf);
						m_ability5Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[4].m_disabled.gameObject.activeSelf);
						m_catalyst1Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[0].m_disabled.gameObject.activeSelf);
						m_catalyst2Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[1].m_disabled.gameObject.activeSelf);
						m_catalyst3Btn.SetDisabled(HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[2].m_disabled.gameObject.activeSelf);
						m_hoverAbility = KeyPreference.NullPreference;
						return;
					}
				}
			}
		}
	}

	private void SetSelectedButton(KeyPreference hoverAbility, bool clear = false)
	{
		_SelectableBtn selectableBtn = null;
		switch (hoverAbility)
		{
		case KeyPreference.Ability1:
			selectableBtn = m_ability1Btn;
			break;
		case KeyPreference.Ability2:
			selectableBtn = m_ability2Btn;
			break;
		case KeyPreference.Ability3:
			selectableBtn = m_ability3Btn;
			break;
		case KeyPreference.Ability4:
			selectableBtn = m_ability4Btn;
			break;
		case KeyPreference.Ability5:
			selectableBtn = m_ability5Btn;
			break;
		case KeyPreference.Card1:
			selectableBtn = m_catalyst1Btn;
			break;
		case KeyPreference.Card2:
			selectableBtn = m_catalyst2Btn;
			break;
		case KeyPreference.Card3:
			selectableBtn = m_catalyst3Btn;
			break;
		}
		if (!(selectableBtn != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!selectableBtn.IsDisabled)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					selectableBtn.SetSelected(!clear, false, string.Empty, string.Empty);
					return;
				}
			}
			return;
		}
	}

	public void SelectAbilityButtonFromAngle(float angle, float lineSize)
	{
		if (lineSize == 0f && m_hoverAbility != 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					SetSelectedButton(m_hoverAbility, true);
					m_hoverAbility = KeyPreference.NullPreference;
					return;
				}
			}
		}
		if (!(lineSize > 0f))
		{
			return;
		}
		KeyPreference keyPreference = KeyPreference.NullPreference;
		angle += 10f;
		if (angle < 0f)
		{
			angle += 360f;
		}
		switch ((int)(angle / 360f * 8f))
		{
		case 0:
			keyPreference = KeyPreference.Ability3;
			break;
		case 1:
			keyPreference = KeyPreference.Ability2;
			break;
		case 2:
			keyPreference = KeyPreference.Ability1;
			break;
		case 3:
			keyPreference = KeyPreference.Card3;
			break;
		case 4:
			keyPreference = KeyPreference.Card2;
			break;
		case 5:
			keyPreference = KeyPreference.Card1;
			break;
		case 6:
			keyPreference = KeyPreference.Ability5;
			break;
		case 7:
			keyPreference = KeyPreference.Ability4;
			break;
		}
		if (keyPreference == m_hoverAbility)
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
			if (m_hoverAbility != 0)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				SetSelectedButton(m_hoverAbility, true);
			}
			m_hoverAbility = keyPreference;
			SetSelectedButton(m_hoverAbility);
			return;
		}
	}

	public KeyPreference GetAbilityHover()
	{
		return m_hoverAbility;
	}
}
