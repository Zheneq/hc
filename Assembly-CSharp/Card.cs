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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return StringUtil.TR_CardDisplayName(m_cardType);
				}
			}
		}
		if (m_useAbility != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_useAbility.GetNameString();
				}
			}
		}
		return "UNKNOWN";
	}

	public string GetUnlocalizedDisplayName()
	{
		if (DisplayName.Length > 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return DisplayName;
				}
			}
		}
		if (m_useAbility != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_useAbility.m_abilityName;
				}
			}
		}
		return "UNKNOWN";
	}

	private void Awake()
	{
		if (m_useAbility != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_useAbility.sprite = GetIconSprite();
					return;
				}
			}
		}
		Log.Error("Card prefab " + DisplayName + " has no Use Ability. Card prefabs are currently required to have Use Abilities.");
	}

	public Sprite GetIconSprite()
	{
		if (!string.IsNullOrEmpty(m_spriteIconPath))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return (Sprite)Resources.Load(m_spriteIconPath, typeof(Sprite));
				}
			}
		}
		return null;
	}

	internal bool IsFreeAction()
	{
		if (m_useAbility != null)
		{
			if (m_useAbility.IsFreeAction())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		return false;
	}

	public AbilityRunPhase GetAbilityRunPhase()
	{
		if (m_useAbility != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return AbilityPriorityToRunPhase(m_useAbility.RunPriority);
				}
			}
		}
		return AbilityRunPhase.Unknown;
	}

	internal static AbilityRunPhase AbilityPriorityToRunPhase(AbilityPriority priority)
	{
		if (priority >= AbilityPriority.Prep_Defense)
		{
			if (priority <= AbilityPriority.Prep_Offense)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return AbilityRunPhase.Prep;
					}
				}
			}
		}
		switch (priority)
		{
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
