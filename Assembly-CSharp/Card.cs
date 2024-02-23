using System.Text;
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
		if (DisplayName.Length > 0)
		{
			return StringUtil.TR_CardDisplayName(m_cardType);
		}
		if (m_useAbility != null)
		{
			return m_useAbility.GetNameString();
		}
		return "UNKNOWN";
	}

	public string GetUnlocalizedDisplayName()
	{
		if (DisplayName.Length > 0)
		{
			return DisplayName;
		}
		if (m_useAbility != null)
		{
			return m_useAbility.m_abilityName;
		}
		return "UNKNOWN";
	}

	private void Awake()
	{
		if (m_useAbility == null)
		{
			Log.Error(new StringBuilder().Append("Card prefab ").Append(DisplayName).Append(" has no Use Ability. Card prefabs are currently required to have Use Abilities.").ToString());
			return;
		}
		m_useAbility.sprite = GetIconSprite();
	}

	public Sprite GetIconSprite()
	{
		if (string.IsNullOrEmpty(m_spriteIconPath))
		{
			return null;
		}
		return (Sprite)Resources.Load(m_spriteIconPath, typeof(Sprite));
	}

	internal bool IsFreeAction()
	{
		return m_useAbility != null && m_useAbility.IsFreeAction();
	}

	public AbilityRunPhase GetAbilityRunPhase()
	{
		return m_useAbility != null
			? AbilityPriorityToRunPhase(m_useAbility.RunPriority)
			: AbilityRunPhase.Unknown;
	}

	internal static AbilityRunPhase AbilityPriorityToRunPhase(AbilityPriority priority)
	{
		switch (priority)
		{
			case AbilityPriority.Prep_Defense:
			case AbilityPriority.Prep_Offense:
				return AbilityRunPhase.Prep;
			case AbilityPriority.Evasion:
				return AbilityRunPhase.Dash;
			case AbilityPriority.Combat_Damage:
			case AbilityPriority.DEPRICATED_Combat_Charge:
			case AbilityPriority.Combat_Knockback:
			case AbilityPriority.Combat_Final:
				return AbilityRunPhase.Combat;
			default:
				return AbilityRunPhase.Unknown;
		}
	}
}
