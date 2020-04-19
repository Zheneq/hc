using System;
using UnityEngine;

public class EffectSource
{
	private string m_name;

	private GameObject m_sequencePrefab;

	public EffectSource(Ability source)
	{
		this.Ability = source;
		this.m_name = source.m_abilityName;
		this.m_sequencePrefab = source.m_sequencePrefab;
	}

	public EffectSource(Passive source)
	{
		this.Passive = source;
		this.m_name = source.m_passiveName;
		this.m_sequencePrefab = source.m_sequencePrefab;
	}

	public EffectSource(string name, GameObject sequencePrefab, GameObject secondarySequencePrefab)
	{
		this.m_name = name;
		this.m_sequencePrefab = sequencePrefab;
	}

	private EffectSource()
	{
	}

	public Ability Ability { get; private set; }

	public Passive Passive { get; private set; }

	public bool IsAbility()
	{
		return this.Ability != null;
	}

	public bool IsPassive()
	{
		return this.Passive != null;
	}

	public string GetName()
	{
		return this.m_name;
	}

	public GameObject GetSequencePrefab()
	{
		return this.m_sequencePrefab;
	}

	public bool IsCharacterSpecificAbility(ActorData caster)
	{
		if (this.Ability == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(EffectSource.IsCharacterSpecificAbility(ActorData)).MethodHandle;
			}
			return false;
		}
		if (!(caster == null))
		{
			if (!(caster.\u000E() == null))
			{
				AbilityData.ActionType actionTypeOfAbility = caster.\u000E().GetActionTypeOfAbility(this.Ability);
				return AbilityData.IsCharacterSpecificAbility(actionTypeOfAbility);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}
}
