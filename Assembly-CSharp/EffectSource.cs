// ROGUES
// SERVER
using UnityEngine;

public class EffectSource
{
	private string m_name;
	private GameObject m_sequencePrefab;

	public Ability Ability { get; private set; }

	// rogues
	//public GearStatData GearStatData { get; private set; }

	// added in rogues
	//public AbilityResults AbilityResults { get; private set; }

	public Passive Passive { get; private set; }

	// rogues
	//public EffectSource(Ability source, GearStatData gearStatData = null)
	//{
	//	Ability = source;
	//	GearStatData = gearStatData;
	//	m_name = source.m_abilityName;
	//	m_sequencePrefab = source.m_sequencePrefab;
	//}

	public EffectSource(Ability source) // , AbilityResults abilityResults in rogues
	{
		Ability = source;
		//AbilityResults = abilityResults;
		m_name = source.m_abilityName;
		m_sequencePrefab = source.m_sequencePrefab;
	}

	public EffectSource(Passive source)
	{
		Passive = source;
		m_name = source.m_passiveName;
		m_sequencePrefab = source.m_sequencePrefab;
	}

	// rogues
	//public EffectSource(Passive source, GearStatData gearStatData = null)
	//{
	//	Passive = source;
	//	m_name = source.m_passiveName;
	//	m_sequencePrefab = source.m_sequencePrefab;
	//	GearStatData = gearStatData;
	//}

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
		return Ability != null
			&& caster != null
			&& caster.GetAbilityData() != null
			&& AbilityData.IsCharacterSpecificAbility(caster.GetAbilityData().GetActionTypeOfAbility(Ability));
	}
}
