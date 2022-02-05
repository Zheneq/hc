// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only
#if SERVER
public class AbilityResults_Powerup
{
	private List<GameObject> m_sequencePrefabs;

	private BoardSquare m_targetSquare;

	private ActorData m_caster;

	private SequenceSource m_sequenceSource;

	private Sequence.IExtraSequenceParams[] m_extraParams;

	private PowerUp m_powerup;

	private AbilityResults m_results;

	public AbilityResults_Powerup(
        PowerUp powerup,
        ActorHitResults powerupHitResults,
        GameObject sequencePrefab,
        BoardSquare sequenceTargetSquare,
        SequenceSource parentSequenceSource,
        Sequence.IExtraSequenceParams[] extraParams = null)
	{
		SetupSequenceData(sequencePrefab, sequenceTargetSquare, parentSequenceSource, extraParams);
		SetupGameplayData(powerup, powerupHitResults);
	}

	public AbilityResults_Powerup()
	{
	}

	public void SetupSequenceData(GameObject sequencePrefab, BoardSquare sequenceTargetSquare, SequenceSource parentSequenceSource, Sequence.IExtraSequenceParams[] extraParams = null)
	{
		m_sequencePrefabs = new List<GameObject>();
		m_sequencePrefabs.Add(sequencePrefab);
		m_targetSquare = sequenceTargetSquare;
		m_sequenceSource = new SequenceSource(new SequenceSource.ActorDelegate(OnPowerupHitActor), new SequenceSource.Vector3Delegate(OnPowerupHitPosition), true, parentSequenceSource, null);
		m_extraParams = extraParams;
	}

	public void SetupGameplayData(PowerUp powerup, ActorHitResults powerupHitResults)
	{
		ActorData target = powerupHitResults.m_hitParameters.Target;
		m_results = new AbilityResults(target, powerup.m_ability, powerup.SequenceSource, true, false);
		m_results.StoreActorHit(powerupHitResults);
		m_results.GatheredResults = true;
		m_powerup = powerup;
		m_caster = target;
	}

	public Dictionary<ActorData, int> GetPowerupDamageResults()
	{
		return m_results.DamageResults;
	}

	public void ReactToStealingHitExecution()
	{
		m_powerup.OnStealingHit(m_caster);
		m_results.ExecuteUnexecutedAbilityHits(false);
	}

	public void OnPowerupHitActor(ActorData target)
	{
		m_results.ExecuteForActor(target);
	}

	public void OnPowerupHitPosition(Vector3 pos)
	{
		m_results.ExecuteForPosition(pos);
	}

	public ActorData Caster
	{
		get
		{
			return m_caster;
		}
		private set
		{
		}
	}

	public AbilityResults AbilityResults
	{
		get
		{
			return m_results;
		}
		private set
		{
		}
	}

	public List<ServerClientUtils.SequenceStartData> BuildAbilityRunSequenceDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ActorData[] targetActorArray = m_results.HitActorsArray();
		foreach (GameObject prefab in m_sequencePrefabs)
		{
			ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(prefab, m_targetSquare, targetActorArray, m_caster, m_sequenceSource, m_extraParams);
			list.Add(item);
		}
		return list;
	}
}
#endif
