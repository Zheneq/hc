using UnityEngine;

public class DamageSource
{
	private Passive m_passive;

	public Ability Ability
	{
		get;
		private set;
	}

	public string Name
	{
		get;
		private set;
	}

	public bool IgnoresCover
	{
		get;
		private set;
	}

	public Vector3 DamageSourceLocation
	{
		get;
		private set;
	}

	public DamageSource(Ability source, Vector3 damageLocation)
	{
		Ability = source;
		Name = Ability.m_abilityName;
		IgnoresCover = AbilityUtils.AbilityIgnoreCover(Ability, null);
		DamageSourceLocation = damageLocation;
	}

	public DamageSource(Passive source, Vector3 damageLocation)
	{
		m_passive = source;
		Name = m_passive.m_passiveName;
		IgnoresCover = false;
		DamageSourceLocation = damageLocation;
	}

	public DamageSource(string name, bool ignoresCover, Vector3 damageLocation)
	{
		Name = name;
		IgnoresCover = ignoresCover;
		DamageSourceLocation = damageLocation;
	}

	private DamageSource()
	{
	}

	public bool IsAbility()
	{
		return Ability != null;
	}

	public bool IsPassive()
	{
		return m_passive != null;
	}

	public bool IgnoreDamageBuffsAndDebuffs()
	{
		if (IsAbility())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return Ability.m_tags.Contains(AbilityTags.IgnoreOutgoingDamageHealAbsorbBuffsAndDebuffs);
				}
			}
		}
		return false;
	}

	public bool IsCharacterSpecificAbility(ActorData caster)
	{
		if (Ability == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (!(caster == null))
		{
			if (!(caster.GetAbilityData() == null))
			{
				AbilityData.ActionType actionTypeOfAbility = caster.GetAbilityData().GetActionTypeOfAbility(Ability);
				return AbilityData.IsCharacterSpecificAbility(actionTypeOfAbility);
			}
		}
		return false;
	}
}
