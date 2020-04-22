using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIModSelectionGridEntry : MonoBehaviour
{
	private bool m_isUnLocked;

	public Text m_ModName;

	public Text m_ModPoints;

	public Toggle m_ModEnabled;

	public Image m_lockBackground;

	public Image m_lockIcon;

	[HideInInspector]
	public Image m_previousIcon;

	public Color m_SelectedColor;

	public Color m_NotSelectedColor;

	public Color m_LockedColor;

	public int selectedSize;

	public int notSelectedSize;

	public _ButtonSwapSprite m_buttonHitBox;

	[HideInInspector]
	internal AbilityMod associatedAbilityMod;

	[HideInInspector]
	internal bool justSet;

	private UnityAction<bool> thisDelegate;

	public void Setup(AbilityMod inAbility, bool isSelected, bool isUnlocked, UnityAction<bool> callDelegate = null)
	{
		bool isSelected2 = isSelected;
		associatedAbilityMod = inAbility;
		bool flag = true;
		m_ModEnabled.enabled = flag;
		flag = flag;
		m_ModName.enabled = flag;
		flag = flag;
		m_ModPoints.enabled = flag;
		base.enabled = flag;
		m_ModName.text = associatedAbilityMod.GetName();
		m_ModPoints.text = associatedAbilityMod.m_equipCost.ToString();
		m_isUnLocked = isUnlocked;
		m_ModEnabled.isOn = isSelected2;
		UIManager.SetGameObjectActive(m_ModName, true);
		bool selected = isSelected2;
		Text modName = m_ModName;
		Color color;
		if (!m_isUnLocked)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			color = m_LockedColor;
		}
		else if (isSelected2)
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
			color = m_SelectedColor;
		}
		else
		{
			color = m_NotSelectedColor;
		}
		setColor(selected, false, modName, color);
		bool selected2 = isSelected2;
		Text modPoints = m_ModPoints;
		Color color2;
		if (!m_isUnLocked)
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
			color2 = m_LockedColor;
		}
		else if (isSelected2)
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
			color2 = m_SelectedColor;
		}
		else
		{
			color2 = m_NotSelectedColor;
		}
		setColor(selected2, false, modPoints, color2);
		if (m_lockIcon != null)
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
			m_lockIcon.enabled = !isUnlocked;
		}
		m_ModPoints.text = associatedAbilityMod.m_equipCost.ToString();
		thisDelegate = callDelegate;
		if (!isUnlocked)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_ModEnabled.enabled = false;
					if (m_lockBackground != null)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								m_lockBackground.color = Color.gray;
								return;
							}
						}
					}
					return;
				}
			}
		}
		m_ModEnabled.onValueChanged.AddListener(delegate(bool x)
		{
			if (isUnlocked)
			{
				if (m_lockBackground != null)
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
					m_lockBackground.color = Color.white;
				}
				isSelected2 = x;
				UIModSelectionGridEntry uIModSelectionGridEntry = this;
				bool selected3 = isSelected2;
				Text modName2 = m_ModName;
				Color color3;
				if (isSelected2)
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
					color3 = m_SelectedColor;
				}
				else
				{
					color3 = m_NotSelectedColor;
				}
				uIModSelectionGridEntry.setColor(selected3, false, modName2, color3);
				UIModSelectionGridEntry uIModSelectionGridEntry2 = this;
				bool selected4 = isSelected2;
				Text modPoints2 = m_ModPoints;
				Color color4;
				if (isSelected2)
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
					color4 = m_SelectedColor;
				}
				else
				{
					color4 = m_NotSelectedColor;
				}
				uIModSelectionGridEntry2.setColor(selected4, false, modPoints2, color4);
				justSet = x;
				if (thisDelegate != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							thisDelegate(x);
							return;
						}
					}
				}
			}
		});
	}

	private void setColor(bool selected, bool isHighlighted, Text text, Color color)
	{
		float r = color.r;
		float g = color.g;
		float b = color.b;
		Color color2 = text.color;
		text.color = new Color(r, g, b, color2.a);
		text.fontSize = ((!selected) ? notSelectedSize : selectedSize);
		Outline component = m_ModPoints.GetComponent<Outline>();
		if (component != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			component.effectColor = Color.black;
			Vector2 effectDistance;
			if (selected)
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
				effectDistance = new Vector2(1f, -1f);
			}
			else
			{
				effectDistance = new Vector2(2f, -2f);
			}
			component.effectDistance = effectDistance;
		}
		Shadow component2 = m_ModPoints.GetComponent<Shadow>();
		if (!(component2 != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			component2.effectColor = Color.black;
			return;
		}
	}

	public void Disable()
	{
		associatedAbilityMod = null;
		base.enabled = false;
		Text modPoints = m_ModPoints;
		string empty = string.Empty;
		m_ModName.text = empty;
		modPoints.text = empty;
		Text modPoints2 = m_ModPoints;
		Color notSelectedColor = m_NotSelectedColor;
		m_ModName.color = notSelectedColor;
		modPoints2.color = notSelectedColor;
		Text modPoints3 = m_ModPoints;
		bool flag = false;
		m_ModEnabled.enabled = flag;
		flag = flag;
		m_ModName.enabled = flag;
		modPoints3.enabled = flag;
		m_ModEnabled.onValueChanged.RemoveAllListeners();
		thisDelegate = null;
		justSet = false;
	}
}
