// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ThiefHiddenTrap : Ability
{
	[Header("-- Trap Ground Field")]
	public GroundEffectField m_trapFieldInfo;
	[Header("-- Extra Damage")]
	public int m_extraDamagePerTurn;
	public int m_maxExtraDamage;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ThiefHiddenTrap m_abilityMod;
	private GroundEffectField m_cachedTrapFieldInfo;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Hidden Trap";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		GroundEffectField trapFieldInfo = GetTrapFieldInfo();
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			trapFieldInfo.shape,
			trapFieldInfo.penetrateLos,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			trapFieldInfo.IncludeEnemies(),
			trapFieldInfo.IncludeAllies(),
			trapFieldInfo.IncludeAllies()
				? AbilityUtil_Targeter.AffectsActor.Possible
				: AbilityUtil_Targeter.AffectsActor.Never);
	}

	private void SetCachedFields()
	{
		m_cachedTrapFieldInfo = m_abilityMod != null
			? m_abilityMod.m_trapFieldInfoMod.GetModifiedValue(m_trapFieldInfo)
			: m_trapFieldInfo;
	}

	public GroundEffectField GetTrapFieldInfo()
	{
		// reactor
		return m_cachedTrapFieldInfo ?? m_trapFieldInfo;
		// rogues
		// return m_trapFieldInfo ?? (m_trapFieldInfo = ScriptableObject.CreateInstance<GroundEffectField>());
	}

	public int GetExtraDamagePerTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamagePerTurnMod.GetModifiedValue(m_extraDamagePerTurn)
			: m_extraDamagePerTurn;
	}

	public int GetMaxExtraDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxExtraDamageMod.GetModifiedValue(m_maxExtraDamage)
			: m_maxExtraDamage;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetTrapFieldInfo().damageAmount);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ThiefHiddenTrap abilityMod_ThiefHiddenTrap = modAsBase as AbilityMod_ThiefHiddenTrap;
#if PURE_REACTOR
		m_trapFieldInfo.AddTooltipTokens(tokens, "GroundEffect");
#else
		GetTrapFieldInfo().AddTooltipTokens(tokens, "GroundEffect");
#endif
		AddTokenInt(tokens, "ExtraDamagePerTurn", string.Empty, abilityMod_ThiefHiddenTrap != null
			? abilityMod_ThiefHiddenTrap.m_extraDamagePerTurnMod.GetModifiedValue(m_extraDamagePerTurn)
			: m_extraDamagePerTurn);
		AddTokenInt(tokens, "MaxExtraDamage", string.Empty, abilityMod_ThiefHiddenTrap != null
			? abilityMod_ThiefHiddenTrap.m_maxExtraDamageMod.GetModifiedValue(m_maxExtraDamage)
			: m_maxExtraDamage);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ThiefHiddenTrap))
		{
			m_abilityMod = abilityMod as AbilityMod_ThiefHiddenTrap;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				Board.Get().GetSquare(targets[0].GridPos),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(square.ToVector3()));
		positionHitResults.AddEffect(new ThiefHiddenTrapEffect(
			AsEffectSource(),
			square,
			targets[0].FreePos,
			null,
			caster,
			GetTrapFieldInfo(),
			GetExtraDamagePerTurn(),
			GetMaxExtraDamage()));
		abilityResults.StorePositionHit(positionHitResults);
	}

	// added in rogues
	public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
	{
		caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.ThiefStats.ProximityMineHits);
	}
#endif
}
