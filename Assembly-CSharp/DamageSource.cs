using System;
using UnityEngine;

public class DamageSource
{
	private Passive m_passive;

	public DamageSource(Ability source, Vector3 damageLocation)
	{
		this.Ability = source;
		this.Name = this.Ability.m_abilityName;
		this.IgnoresCover = AbilityUtils.AbilityIgnoreCover(this.Ability, null);
		this.DamageSourceLocation = damageLocation;
	}

	public DamageSource(Passive source, Vector3 damageLocation)
	{
		this.m_passive = source;
		this.Name = this.m_passive.m_passiveName;
		this.IgnoresCover = false;
		this.DamageSourceLocation = damageLocation;
	}

	public DamageSource(string name, bool ignoresCover, Vector3 damageLocation)
	{
		this.Name = name;
		this.IgnoresCover = ignoresCover;
		this.DamageSourceLocation = damageLocation;
	}

	private DamageSource()
	{
	}

	public Ability Ability { get; private set; }

	public string Name { get; private set; }

	public bool IgnoresCover { get; private set; }

	public Vector3 DamageSourceLocation { get; private set; }

	public bool IsAbility()
	{
		return this.Ability != null;
	}

	public bool IsPassive()
	{
		return this.m_passive != null;
	}

	public bool IgnoreDamageBuffsAndDebuffs()
	{
		if (this.IsAbility())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DamageSource.IgnoreDamageBuffsAndDebuffs()).MethodHandle;
			}
			return this.Ability.m_tags.Contains(AbilityTags.IgnoreOutgoingDamageHealAbsorbBuffsAndDebuffs);
		}
		return false;
	}

	public bool IsCharacterSpecificAbility(ActorData caster)
	{
		if (this.Ability == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DamageSource.IsCharacterSpecificAbility(ActorData)).MethodHandle;
			}
			return false;
		}
		if (!(caster == null))
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
			if (!(caster.GetAbilityData() == null))
			{
				AbilityData.ActionType actionTypeOfAbility = caster.GetAbilityData().GetActionTypeOfAbility(this.Ability);
				return AbilityData.IsCharacterSpecificAbility(actionTypeOfAbility);
			}
		}
		return false;
	}
}
