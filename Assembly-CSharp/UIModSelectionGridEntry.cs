using System;
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
		this.associatedAbilityMod = inAbility;
		bool flag = true;
		this.m_ModEnabled.enabled = flag;
		flag = flag;
		this.m_ModName.enabled = flag;
		flag = flag;
		this.m_ModPoints.enabled = flag;
		base.enabled = flag;
		this.m_ModName.text = this.associatedAbilityMod.GetName();
		this.m_ModPoints.text = this.associatedAbilityMod.m_equipCost.ToString();
		this.m_isUnLocked = isUnlocked;
		this.m_ModEnabled.isOn = isSelected;
		UIManager.SetGameObjectActive(this.m_ModName, true, null);
		bool isSelected2 = isSelected;
		bool isHighlighted = false;
		Text modName = this.m_ModName;
		Color color;
		if (!this.m_isUnLocked)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIModSelectionGridEntry.Setup(AbilityMod, bool, bool, UnityAction<bool>)).MethodHandle;
			}
			color = this.m_LockedColor;
		}
		else if (isSelected)
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
			color = this.m_SelectedColor;
		}
		else
		{
			color = this.m_NotSelectedColor;
		}
		this.setColor(isSelected2, isHighlighted, modName, color);
		bool isSelected3 = isSelected;
		bool isHighlighted2 = false;
		Text modPoints = this.m_ModPoints;
		Color color2;
		if (!this.m_isUnLocked)
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
			color2 = this.m_LockedColor;
		}
		else if (isSelected)
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
			color2 = this.m_SelectedColor;
		}
		else
		{
			color2 = this.m_NotSelectedColor;
		}
		this.setColor(isSelected3, isHighlighted2, modPoints, color2);
		if (this.m_lockIcon != null)
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
			this.m_lockIcon.enabled = !isUnlocked;
		}
		this.m_ModPoints.text = this.associatedAbilityMod.m_equipCost.ToString();
		this.thisDelegate = callDelegate;
		if (!isUnlocked)
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
			this.m_ModEnabled.enabled = false;
			if (this.m_lockBackground != null)
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
				this.m_lockBackground.color = Color.gray;
			}
		}
		else
		{
			this.m_ModEnabled.onValueChanged.AddListener(delegate(bool x)
			{
				if (isUnlocked)
				{
					if (this.m_lockBackground != null)
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
							RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIModSelectionGridEntry.<Setup>c__AnonStorey0.<>m__0(bool)).MethodHandle;
						}
						this.m_lockBackground.color = Color.white;
					}
					isSelected = x;
					UIModSelectionGridEntry $this = this;
					bool isSelected4 = isSelected;
					bool isHighlighted3 = false;
					Text modName2 = this.m_ModName;
					Color color3;
					if (isSelected)
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
						color3 = this.m_SelectedColor;
					}
					else
					{
						color3 = this.m_NotSelectedColor;
					}
					$this.setColor(isSelected4, isHighlighted3, modName2, color3);
					UIModSelectionGridEntry $this2 = this;
					bool isSelected5 = isSelected;
					bool isHighlighted4 = false;
					Text modPoints2 = this.m_ModPoints;
					Color color4;
					if (isSelected)
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
						color4 = this.m_SelectedColor;
					}
					else
					{
						color4 = this.m_NotSelectedColor;
					}
					$this2.setColor(isSelected5, isHighlighted4, modPoints2, color4);
					this.justSet = x;
					if (this.thisDelegate != null)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						this.thisDelegate(x);
					}
				}
			});
		}
	}

	private void setColor(bool selected, bool isHighlighted, Text text, Color color)
	{
		text.color = new Color(color.r, color.g, color.b, text.color.a);
		text.fontSize = ((!selected) ? this.notSelectedSize : this.selectedSize);
		Outline component = this.m_ModPoints.GetComponent<Outline>();
		if (component != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIModSelectionGridEntry.setColor(bool, bool, Text, Color)).MethodHandle;
			}
			component.effectColor = Color.black;
			Shadow shadow = component;
			Vector2 effectDistance;
			if (selected)
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
				effectDistance = new Vector2(1f, -1f);
			}
			else
			{
				effectDistance = new Vector2(2f, -2f);
			}
			shadow.effectDistance = effectDistance;
		}
		Shadow component2 = this.m_ModPoints.GetComponent<Shadow>();
		if (component2 != null)
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
			component2.effectColor = Color.black;
		}
	}

	public void Disable()
	{
		this.associatedAbilityMod = null;
		base.enabled = false;
		Text modPoints = this.m_ModPoints;
		string empty = string.Empty;
		this.m_ModName.text = empty;
		modPoints.text = empty;
		Graphic modPoints2 = this.m_ModPoints;
		Color notSelectedColor = this.m_NotSelectedColor;
		this.m_ModName.color = notSelectedColor;
		modPoints2.color = notSelectedColor;
		Behaviour modPoints3 = this.m_ModPoints;
		bool flag = false;
		this.m_ModEnabled.enabled = flag;
		flag = flag;
		this.m_ModName.enabled = flag;
		modPoints3.enabled = flag;
		this.m_ModEnabled.onValueChanged.RemoveAllListeners();
		this.thisDelegate = null;
		this.justSet = false;
	}
}
