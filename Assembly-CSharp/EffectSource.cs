using UnityEngine;

public class EffectSource
{
	private string m_name;
	private GameObject m_sequencePrefab;

	public Ability Ability { get; private set; }
	public Passive Passive { get; private set; }

	public EffectSource(Ability source)
	{
		Ability = source;
		m_name = source.m_abilityName;
		m_sequencePrefab = source.m_sequencePrefab;
	}

	public EffectSource(Passive source)
	{
		Passive = source;
		m_name = source.m_passiveName;
		m_sequencePrefab = source.m_sequencePrefab;
	}

	public EffectSource(string name, GameObject sequencePrefab, GameObject secondarySequencePrefab)
	{
		m_name = name;
		m_sequencePrefab = sequencePrefab;
	}

	private EffectSource()
	{
	}

	public bool IsAbility()
	{
		return Ability != null;
	}

	public bool IsPassive()
	{
		return Passive != null;
	}

	public string GetName()
	{
		return m_name;
	}

	public GameObject GetSequencePrefab()
	{
		return m_sequencePrefab;
	}

	public bool IsCharacterSpecificAbility(ActorData caster)
	{
		if (Ability != null && caster != null && caster.GetAbilityData() != null)
		{
			AbilityData.ActionType actionTypeOfAbility = caster.GetAbilityData().GetActionTypeOfAbility(Ability);
			return AbilityData.IsCharacterSpecificAbility(actionTypeOfAbility);
		}
		return false;
	}
}
