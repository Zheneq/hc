using System;
using UnityEngine;

public class Card : MonoBehaviour
{
	internal const int c_invalidID = -1;

	public CardType m_cardType;

	public string DisplayName;

	public bool m_isHidden;

	[AssetFileSelector("Assets/UI/Textures/Resources/AbilityIcons/Catalyst/", "AbilityIcons/Catalyst/", ".psd")]
	public string m_spriteIconPath = string.Empty;

	public Ability m_useAbility;

	public string GetDisplayName()
	{
		if (this.DisplayName.Length > 0)
		{
			return StringUtil.TR_CardDisplayName(this.m_cardType);
		}
		if (this.m_useAbility != null)
		{
			return this.m_useAbility.GetNameString();
		}
		return "UNKNOWN";
	}

	public string GetUnlocalizedDisplayName()
	{
		if (this.DisplayName.Length > 0)
		{
			return this.DisplayName;
		}
		if (this.m_useAbility != null)
		{
			return this.m_useAbility.m_abilityName;
		}
		return "UNKNOWN";
	}

	private void Awake()
	{
		if (this.m_useAbility != null)
		{
			this.m_useAbility.sprite = this.GetIconSprite();
		}
		else
		{
			Log.Error("Card prefab " + this.DisplayName + " has no Use Ability. Card prefabs are currently required to have Use Abilities.", new object[0]);
		}
	}

	public Sprite GetIconSprite()
	{
		if (!string.IsNullOrEmpty(this.m_spriteIconPath))
		{
			return (Sprite)Resources.Load(this.m_spriteIconPath, typeof(Sprite));
		}
		return null;
	}

	internal bool IsFreeAction()
	{
		if (this.m_useAbility != null)
		{
			if (this.m_useAbility.IsFreeAction())
			{
				return true;
			}
		}
		return false;
	}

	public AbilityRunPhase GetAbilityRunPhase()
	{
		if (this.m_useAbility != null)
		{
			return Card.AbilityPriorityToRunPhase(this.m_useAbility.RunPriority);
		}
		return AbilityRunPhase.Unknown;
	}

	internal static AbilityRunPhase AbilityPriorityToRunPhase(AbilityPriority priority)
	{
		if (priority >= AbilityPriority.Prep_Defense)
		{
			if (priority <= AbilityPriority.Prep_Offense)
			{
				return AbilityRunPhase.Prep;
			}
		}
		if (priority == AbilityPriority.Evasion)
		{
			return AbilityRunPhase.Dash;
		}
		if (priority >= AbilityPriority.Combat_Damage && priority <= AbilityPriority.Combat_Final)
		{
			return AbilityRunPhase.Combat;
		}
		return AbilityRunPhase.Unknown;
	}
}
