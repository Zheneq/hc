// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class BattleMonkSelfBuff : Ability
{
	public int m_damagePerHit = 10;
	public StandardActorEffectData m_standardActorEffectData;
	[Header("-- Enemy Hit Effect --")]
	public StandardEffectInfo m_returnEffectOnEnemy;
	
	// removed in rogues, seems redundant
	[Header("-- Whether to ignore LoS when checking for allies, used for mod")]
	public bool m_ignoreLos;
	// end removed in rogues
	
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_reactionProjectilePrefab;

	private AbilityMod_BattleMonkSelfBuff m_abilityMod;
	private StandardEffectInfo m_cachedReturnEffectOnEnemy;
	
#if SERVER
	private Passive_BattleMonk m_passive;
#endif

	private void Start()
	{
		Setup();
#if SERVER
		PassiveData component = GetComponent<PassiveData>();
		if (component != null)
		{
			m_passive = component.GetPassiveOfType(typeof(Passive_BattleMonk)) as Passive_BattleMonk;
		}
#endif
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BattleMonkSelfBuff abilityMod_BattleMonkSelfBuff = modAsBase as AbilityMod_BattleMonkSelfBuff;
		tokens.Add(new TooltipTokenInt("DamageReturn", "damage amount on revenge hit", abilityMod_BattleMonkSelfBuff != null
			? abilityMod_BattleMonkSelfBuff.m_damageReturnMod.GetModifiedValue(m_damagePerHit)
			: m_damagePerHit));
		tokens.Add(new TooltipTokenInt("ShieldAmount", "shield amount", abilityMod_BattleMonkSelfBuff != null
			? abilityMod_BattleMonkSelfBuff.m_absorbMod.GetModifiedValue(m_standardActorEffectData.m_absorbAmount)
			: m_standardActorEffectData.m_absorbAmount));
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			CanTargetNearbyAllies() ? GetAllyTargetShape() : AbilityAreaShape.SingleSquare,
			m_ignoreLos,  // true in rogues
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false,
			CanTargetNearbyAllies(),
			AbilityUtil_Targeter.AffectsActor.Always);
		Targeter.ShowArcToShape = false;
	}

	private void SetCachedFields()
	{
		m_cachedReturnEffectOnEnemy = m_abilityMod != null
			? m_abilityMod.m_returnEffectOnEnemyMod.GetModifiedValue(m_returnEffectOnEnemy)
			: m_returnEffectOnEnemy;
	}

	public int GetDamagePerHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageReturnMod.GetModifiedValue(m_damagePerHit)
			: m_damagePerHit;
	}

	public StandardEffectInfo GetReturnEffectOnEnemy()
	{
		return m_cachedReturnEffectOnEnemy ?? m_returnEffectOnEnemy;
	}

	public int GetAbsorbAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_absorbMod.GetModifiedValue(m_standardActorEffectData.m_absorbAmount)
			: m_standardActorEffectData.m_absorbAmount;
	}

	public bool CanTargetNearbyAllies()
	{
		return m_abilityMod != null && m_abilityMod.m_hitNearbyAlliesMod.GetModifiedValue(false);
	}

	public AbilityAreaShape GetAllyTargetShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyTargetShapeMod.GetModifiedValue(AbilityAreaShape.SingleSquare)
			: AbilityAreaShape.SingleSquare;
	}

	public StandardEffectInfo GetSelfEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_effectOnSelfNextTurn
			: null;
	}

	public int GetDurationOfSelfEffect(int numHits)
	{
		return m_abilityMod != null
			? m_abilityMod.m_selfEffectDurationPerHit.GetModifiedValue(numHits)
			: 0;
	}

	public bool HasEffectForStartOfNextTurn()
	{
		return GetSelfEffect() != null && GetSelfEffect().m_applyEffect;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damagePerHit));
		m_standardActorEffectData.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, GetAbsorbAmount());
		if (CanTargetNearbyAllies() && m_abilityMod.m_effectOnAllyHit.m_applyEffect)
		{
			m_abilityMod.m_effectOnAllyHit.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		return numbers;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BattleMonkSelfBuff))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_BattleMonkSelfBuff;
		Setup();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
	
#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_passive != null)
		{
			m_passive.m_buffLastCastTurn = GameFlowData.Get().CurrentTurn;
		}
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			caster.GetFreePos(),
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		if (m_standardActorEffectData != null)
		{
			ActorHitParameters hitParams = new ActorHitParameters(caster, caster.GetFreePos());
			StandardActorEffectData shallowCopy = m_standardActorEffectData.GetShallowCopy();
			shallowCopy.m_absorbAmount = GetAbsorbAmount();
			BattleMonkThornsEffect effect = new BattleMonkThornsEffect(
				AsEffectSource(),
				caster.GetCurrentBoardSquare(),
				caster,
				caster,
				shallowCopy,
				GetDamagePerHit(),
				GetReturnEffectOnEnemy(),
				m_reactionProjectilePrefab);
			abilityResults.StoreActorHit(new ActorHitResults(effect, hitParams));
		}
		if (CanTargetNearbyAllies())
		{
			List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
				GetAllyTargetShape(),
				targets[0],
				false,
				caster,
				caster.GetTeamAsList(),
				nonActorTargetInfo);
			if (actorsInShape.Contains(caster))
			{
				actorsInShape.Remove(caster);
			}
			if (m_abilityMod != null && m_abilityMod.m_effectOnAllyHit.m_applyEffect)
			{
				foreach (ActorData target in actorsInShape)
				{
					ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, caster.GetFreePos()));
					actorHitResults.AddStandardEffectInfo(m_abilityMod.m_effectOnAllyHit);
					abilityResults.StoreActorHit(actorHitResults);
				}
			}
			abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
	}

	// added in rogues
	public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
	{
		base.OnExecutedActorHit_Effect(caster, target, results);
		if (caster.GetTeam() != target.GetTeam())
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.BattleMonkStats.DamageReturnedByShield, results.FinalDamage);
		}
	}
#endif
}
