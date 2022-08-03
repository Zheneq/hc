// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ScoundrelTrapWire : Ability
{
	public AbilityGridPattern m_pattern = AbilityGridPattern.Plus_Two_x_Two;
	public float m_barrierSizeScale = 1f;
	public StandardBarrierData m_barrierData;

	private AbilityMod_ScoundrelTrapWire m_abilityMod;

#if SERVER
	// added in rogues
	private Passive_Scoundrel m_passive;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Trap Wires";
		}
		if (m_pattern != AbilityGridPattern.NoPattern)
		{
			ModdedBarrierData().SetupForPattern(m_pattern);
		}
#if SERVER
		// added in rogues
		PassiveData component = GetComponent<PassiveData>();
		if (component != null)
		{
			m_passive = (component.GetPassiveOfType(typeof(Passive_Scoundrel)) as Passive_Scoundrel);
		}
#endif
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_pattern == AbilityGridPattern.NoPattern)
		{
			Targeter = new AbilityUtil_Targeter_Barrier(this, ModdedBarrierData().m_width * ModdedBarrierScale());
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_Grid(this, m_pattern, ModdedBarrierScale());
		}
		Targeter.ShowArcToShape = true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		ModdedBarrierData().AddTooltipTokens(tokens, "Wall");
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		ModdedBarrierData().ReportAbilityTooltipNumbers(ref numbers);
		return numbers;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ScoundrelTrapWire))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}

		m_abilityMod = (abilityMod as AbilityMod_ScoundrelTrapWire);
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public StandardBarrierData ModdedBarrierData()
	{
		// reactor
		return m_abilityMod != null
			? m_abilityMod.m_barrierDataMod.GetModifiedValue(m_barrierData)
			: m_barrierData;
		// rogues
		// if (m_abilityMod != null)
		// {
		// 	return m_abilityMod.m_barrierDataMod.GetModifiedValue(m_barrierData);
		// }
		// if (m_barrierData == null)
		// {
		// 	m_barrierData = ScriptableObject.CreateInstance<StandardBarrierData>();
		// }
		// return m_barrierData;
	}

	private float ModdedBarrierScale()
	{
		float scale = m_barrierSizeScale;
		if (m_abilityMod != null)
		{
			return m_abilityMod.m_barrierScaleMod.GetModifiedValue(scale);
		}
		return scale;
	}

	public List<GameObject> ModdedBarrierSequencePrefab()
	{
		if (m_abilityMod != null
			&& m_abilityMod.m_barrierSequence != null
			&& m_abilityMod.m_barrierSequence.Count > 0)
		{
			return m_abilityMod.m_barrierSequence;
		}
		return ModdedBarrierData().m_barrierSequencePrefabs;
	}
	
#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_passive != null)
		{
			m_passive.m_trapwireLastCastTurn = GameFlowData.Get().CurrentTurn;
			m_passive.m_trapwireDidDamage = false;
		}
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 centerOfGridPattern = AreaEffectUtils.GetCenterOfGridPattern(m_pattern, targets[0]);
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			centerOfGridPattern,
			null,
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Vector3 center = AreaEffectUtils.GetCenterOfGridPattern(m_pattern, targets[0]);
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(center));
		StandardBarrierData standardBarrierData = ModdedBarrierData();
		List<GameObject> barrierSequencePrefabs = standardBarrierData.m_barrierSequencePrefabs;
		standardBarrierData.m_width *= ModdedBarrierScale();
		standardBarrierData.m_barrierSequencePrefabs = ModdedBarrierSequencePrefab();
		bool playSequences = false;
		if (m_abilityMod != null)
		{
			playSequences = m_abilityMod.m_barrierPlaySequenceOnAllBarriers;
		}
		Barrier barrier1 = new Barrier(m_abilityName, center, new Vector3(0f, 0f, 1f), caster, standardBarrierData);
		barrier1.SetSourceAbility(this);
		Barrier barrier2 = new Barrier(m_abilityName, center, new Vector3(1f, 0f, 0f), caster, standardBarrierData, playSequences);
		barrier2.SetSourceAbility(this);
		if (m_abilityMod != null)
		{
			if (m_abilityMod.m_useEnemyMovedThroughOverride)
			{
				barrier1.SetEnemyMovedThroughResponse(m_abilityMod.m_enemyMovedThroughOverride);
				barrier2.SetEnemyMovedThroughResponse(m_abilityMod.m_enemyMovedThroughOverride);
			}
			if (m_abilityMod.m_useAllyMovedThroughOverride)
			{
				barrier1.SetAllyMovedThroughResponse(m_abilityMod.m_allyMovedThroughOverride);
				barrier2.SetAllyMovedThroughResponse(m_abilityMod.m_allyMovedThroughOverride);
			}
		}
		standardBarrierData.m_width = standardBarrierData.m_width;
		standardBarrierData.m_barrierSequencePrefabs = barrierSequencePrefabs;
		LinkedBarrierData linkData = new LinkedBarrierData();
		List<Barrier> list = new List<Barrier>
		{
			barrier1,
			barrier2
		};
		positionHitResults.AddBarrier(barrier1);
		positionHitResults.AddBarrier(barrier2);
		BarrierManager.Get().LinkBarriers(list, linkData);
		abilityResults.StorePositionHit(positionHitResults);
	}

	// added in rogues
	public void TrapwireExpiredWithoutHitting()
	{
		if (m_abilityMod != null && m_abilityMod.m_cooldownReductionsWhenNoHits.HasCooldownReduction())
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(ActorData, ActorData.GetFreePos()));
			actorHitResults.SetIgnoreTechpointInteractionForHit(true);
			m_abilityMod.m_cooldownReductionsWhenNoHits.AppendCooldownMiscEvents(actorHitResults, true, 0, 0);
			MovementResults.SetupAndExecuteAbilityResultsOutsideResolution(ActorData, ActorData, actorHitResults, this);
		}
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0 && results.IsFromMovement() && results.ForMovementStage == MovementStage.Evasion)
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.ScoundrelStats.DashersHitByTrapwire);
		}
	}
#endif
}
