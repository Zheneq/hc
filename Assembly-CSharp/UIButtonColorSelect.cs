using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonColorSelect : UICharacterVisualsSelectButton, ITransformSortOrder
{
	public Image m_colorIcon;

	public Image m_skinLevelIcon;

	public Image m_defaultBorder;

	public _ButtonSwapSprite m_hitbox;

	public UIColorData m_colorData;

	[HideInInspector]
	public int m_skinIndex;

	[HideInInspector]
	public int m_patternIndex;

	[HideInInspector]
	public int m_tintIndex;

	[HideInInspector]
	public int m_transformPriority;

	private UISkinBrowserPanel m_uiSkinBrowserPanel;

	public static ITransformSortOrder ButtonToTransformSortOrder(UIButtonColorSelect button)
	{
		return button;
	}

	public static void SortTransform(List<ITransformSortOrder> Transforms)
	{
		Transforms.Sort(delegate(ITransformSortOrder first, ITransformSortOrder second)
		{
			int num = 0;
			int j = 0;
			while (j < Transforms.Count)
			{
				if (Transforms[j].GetTransform() == first.GetTransform())
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIButtonColorSelect.<SortTransform>c__AnonStorey0.<>m__0(ITransformSortOrder, ITransformSortOrder)).MethodHandle;
					}
					num = j;
					IL_5D:
					int num2 = 0;
					for (int k = 0; k < Transforms.Count; k++)
					{
						if (Transforms[k].GetTransform() == second.GetTransform())
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
							num2 = k;
							IL_B1:
							int num3 = first.GetTransformPriority() * Transforms.Count + num;
							int num4 = second.GetTransformPriority() * Transforms.Count + num2;
							return num3 - num4;
						}
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						goto IL_B1;
					}
				}
				else
				{
					j++;
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				goto IL_5D;
			}
		});
		for (int i = 0; i < Transforms.Count; i++)
		{
			Transforms[i].GetTransform().SetAsLastSibling();
		}
	}

	public int GetTransformPriority()
	{
		return this.m_colorData.m_sortOrder;
	}

	public Transform GetTransform()
	{
		return base.gameObject.transform;
	}

	protected override void Start()
	{
		base.Start();
		if (this.m_hitbox != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIButtonColorSelect.Start()).MethodHandle;
			}
			this.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnColorClicked);
		}
	}

	public void OnColorClicked(BaseEventData data)
	{
		this.m_uiSkinBrowserPanel.ColorClicked(this, this.m_skinIndex);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectOptionsChoice);
	}

	public void Setup(UIColorData colorData, int skinIndex, int patternIndex, int tintIndex, UISkinBrowserPanel parent)
	{
		this.m_colorData = colorData;
		this.m_colorIcon.color = colorData.m_buttonColor;
		UIManager.SetGameObjectActive(this.m_lockedIcon, !colorData.m_isAvailable, null);
		this.m_patternIndex = patternIndex;
		this.m_skinIndex = skinIndex;
		this.m_tintIndex = tintIndex;
		this.m_uiSkinBrowserPanel = parent;
		this.m_transformPriority = tintIndex;
		Color color;
		if (ColorUtility.TryParseHtmlString(colorData.m_rarity.GetColorHexString(), out color))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIButtonColorSelect.Setup(UIColorData, int, int, int, UISkinBrowserPanel)).MethodHandle;
			}
			this.m_defaultBorder.color = color;
		}
		UIManager.SetGameObjectActive(this.m_skinLevelIcon, colorData.m_styleLevelType != StyleLevelType.None, null);
		this.m_skinLevelIcon.sprite = (Resources.Load(CharacterColor.GetIconResourceStringForStyleLevelType(colorData.m_styleLevelType), typeof(Sprite)) as Sprite);
		base.SetSelected(false);
		this.m_unlockTooltipTitle = string.Format(StringUtil.TR("SkinName", "Global"), colorData.m_name);
		this.m_unlockTooltipText = colorData.m_description;
		if (this.m_unlockTooltipText.IsNullOrEmpty())
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
			this.m_unlockTooltipText = string.Empty;
			int num = 0;
			if (colorData.m_unlockCharacterLevel > 0)
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
				this.m_unlockTooltipText = this.m_unlockTooltipText + string.Format(StringUtil.TR("UnlockedAtCharacterLevel", "Global"), colorData.m_unlockCharacterLevel) + Environment.NewLine;
				num++;
			}
			if (colorData.m_isoCurrencyCost > 0)
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
				this.m_unlockTooltipText = this.m_unlockTooltipText + string.Format(StringUtil.TR("BuyForISO", "Global"), colorData.m_isoCurrencyCost) + Environment.NewLine;
				num++;
			}
			if (colorData.m_freelancerCurrencyCost > 0)
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
				this.m_unlockTooltipText = this.m_unlockTooltipText + string.Format(StringUtil.TR("BuyForFreelancerCurrency", "Global"), colorData.m_freelancerCurrencyCost) + Environment.NewLine;
				num++;
			}
			if (colorData.m_realCurrencyCost > 0f)
			{
				this.m_unlockTooltipText = this.m_unlockTooltipText + string.Format(StringUtil.TR("BuyFor", "Global"), UIStorePanel.GetLocalizedPriceString(colorData.m_realCurrencyCost, HydrogenConfig.Get().Ticket.AccountCurrency)) + Environment.NewLine;
				num++;
			}
			if (colorData.m_requiredLevelForEquip > 1)
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
				this.m_unlockTooltipText = this.m_unlockTooltipText + string.Format(StringUtil.TR("CanEquipAtCharacterLevel", "Global"), colorData.m_requiredLevelForEquip) + Environment.NewLine;
				num++;
			}
			if (num > 1)
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
				this.m_unlockTooltipText = StringUtil.TR("ObtainableViaMethods", "Global") + Environment.NewLine + this.m_unlockTooltipText + Environment.NewLine;
			}
		}
		else
		{
			this.m_unlockTooltipText += Environment.NewLine;
		}
		if (colorData.m_isAvailable)
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
			if (!colorData.m_isSkinAvailable)
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
				this.m_unlockTooltipText = this.m_unlockTooltipText + StringUtil.TR("MustUnlockSkin", "Global") + Environment.NewLine;
			}
		}
		if (!colorData.m_flavorText.IsNullOrEmpty())
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
			string unlockTooltipText = this.m_unlockTooltipText;
			this.m_unlockTooltipText = string.Concat(new string[]
			{
				unlockTooltipText,
				"<i>",
				colorData.m_flavorText,
				"</i>",
				Environment.NewLine
			});
		}
		string obtainedDescription = GameWideData.Get().GetCharacterResourceLink(this.m_colorData.m_characterType).m_skins[this.m_colorData.m_skinIndex].m_patterns[this.m_colorData.m_patternIndex].m_colors[this.m_colorData.m_colorIndex].m_colorUnlockData.GetObtainedDescription();
		if (!obtainedDescription.IsNullOrEmpty())
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
			this.m_unlockTooltipText += obtainedDescription;
		}
	}
}
