using System;
using System.Collections.Generic;
using System.Text;
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
			int num2 = 0;
			while (true)
			{
				if (num2 >= Transforms.Count)
				{
					break;
				}
				if (Transforms[num2].GetTransform() == first.GetTransform())
				{
					num = num2;
					break;
				}
				num2++;
			}
			int num3 = 0;
			int num4 = 0;
			while (true)
			{
				if (num4 >= Transforms.Count)
				{
					break;
				}
				if (Transforms[num4].GetTransform() == second.GetTransform())
				{
					num3 = num4;
					break;
				}
				num4++;
			}
			int num5 = first.GetTransformPriority() * Transforms.Count + num;
			int num6 = second.GetTransformPriority() * Transforms.Count + num3;
			return num5 - num6;
		});
		for (int i = 0; i < Transforms.Count; i++)
		{
			Transforms[i].GetTransform().SetAsLastSibling();
		}
	}

	public int GetTransformPriority()
	{
		return m_colorData.m_sortOrder;
	}

	public Transform GetTransform()
	{
		return base.gameObject.transform;
	}

	protected override void Start()
	{
		base.Start();
		if (!(m_hitbox != null))
		{
			return;
		}
		while (true)
		{
			m_hitbox.callback = OnColorClicked;
			return;
		}
	}

	public void OnColorClicked(BaseEventData data)
	{
		m_uiSkinBrowserPanel.ColorClicked(this, m_skinIndex);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectOptionsChoice);
	}

	public void Setup(UIColorData colorData, int skinIndex, int patternIndex, int tintIndex, UISkinBrowserPanel parent)
	{
		m_colorData = colorData;
		m_colorIcon.color = colorData.m_buttonColor;
		UIManager.SetGameObjectActive(m_lockedIcon, !colorData.m_isAvailable);
		m_patternIndex = patternIndex;
		m_skinIndex = skinIndex;
		m_tintIndex = tintIndex;
		m_uiSkinBrowserPanel = parent;
		m_transformPriority = tintIndex;
		Color color;
		if (ColorUtility.TryParseHtmlString(colorData.m_rarity.GetColorHexString(), out color))
		{
			m_defaultBorder.color = color;
		}
		UIManager.SetGameObjectActive(m_skinLevelIcon, colorData.m_styleLevelType != StyleLevelType.None);
		m_skinLevelIcon.sprite = (Resources.Load(CharacterColor.GetIconResourceStringForStyleLevelType(colorData.m_styleLevelType), typeof(Sprite)) as Sprite);
		SetSelected(false);
		m_unlockTooltipTitle = string.Format(StringUtil.TR("SkinName", "Global"), colorData.m_name);
		m_unlockTooltipText = colorData.m_description;
		if (m_unlockTooltipText.IsNullOrEmpty())
		{
			m_unlockTooltipText = string.Empty;
			int num = 0;
			if (colorData.m_unlockCharacterLevel > 0)
			{
				m_unlockTooltipText = new StringBuilder().Append(m_unlockTooltipText).AppendLine(string.Format(StringUtil.TR("UnlockedAtCharacterLevel", "Global"), colorData.m_unlockCharacterLevel)).ToString();
				num++;
			}
			if (colorData.m_isoCurrencyCost > 0)
			{
				m_unlockTooltipText = new StringBuilder().Append(m_unlockTooltipText).AppendLine(string.Format(StringUtil.TR("BuyForISO", "Global"), colorData.m_isoCurrencyCost)).ToString();
				num++;
			}
			if (colorData.m_freelancerCurrencyCost > 0)
			{
				m_unlockTooltipText = new StringBuilder().Append(m_unlockTooltipText).AppendLine(string.Format(StringUtil.TR("BuyForFreelancerCurrency", "Global"), colorData.m_freelancerCurrencyCost)).ToString();
				num++;
			}
			if (colorData.m_realCurrencyCost > 0f)
			{
				m_unlockTooltipText = new StringBuilder().Append(m_unlockTooltipText).AppendLine(string.Format(StringUtil.TR("BuyFor", "Global"), UIStorePanel.GetLocalizedPriceString(colorData.m_realCurrencyCost, HydrogenConfig.Get().Ticket.AccountCurrency))).ToString();
				num++;
			}
			if (colorData.m_requiredLevelForEquip > 1)
			{
				m_unlockTooltipText = new StringBuilder().Append(m_unlockTooltipText).AppendLine(string.Format(StringUtil.TR("CanEquipAtCharacterLevel", "Global"), colorData.m_requiredLevelForEquip)).ToString();
				num++;
			}
			if (num > 1)
			{
				m_unlockTooltipText = new StringBuilder().AppendLine(StringUtil.TR("ObtainableViaMethods", "Global")).AppendLine(m_unlockTooltipText).ToString();
			}
		}
		else
		{
			m_unlockTooltipText += Environment.NewLine;
		}
		if (colorData.m_isAvailable)
		{
			if (!colorData.m_isSkinAvailable)
			{
				m_unlockTooltipText = new StringBuilder().Append(m_unlockTooltipText).AppendLine(StringUtil.TR("MustUnlockSkin", "Global")).ToString();
			}
		}
		if (!colorData.m_flavorText.IsNullOrEmpty())
		{
			string unlockTooltipText = m_unlockTooltipText;
			m_unlockTooltipText = new StringBuilder().Append(unlockTooltipText).Append("<i>").Append(colorData.m_flavorText).AppendLine("</i>").ToString();
		}
		string obtainedDescription = GameWideData.Get().GetCharacterResourceLink(m_colorData.m_characterType).m_skins[m_colorData.m_skinIndex].m_patterns[m_colorData.m_patternIndex].m_colors[m_colorData.m_colorIndex].m_colorUnlockData.GetObtainedDescription();
		if (obtainedDescription.IsNullOrEmpty())
		{
			return;
		}
		while (true)
		{
			m_unlockTooltipText += obtainedDescription;
			return;
		}
	}
}
