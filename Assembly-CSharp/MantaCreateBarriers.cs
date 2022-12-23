using System.Collections.Generic;
using UnityEngine;

public class MantaCreateBarriers : Ability
{
	[Header("-- Whether require Manta to be inside target area --")]
	public bool m_requireCasterInShape = true;
	public AbilityAreaShape m_targetAreaShape = AbilityAreaShape.Five_x_Five;
	[Header("-- Barriers")]
	[Separator("NOTE: you can also use MantaCreateBarriersChainFinal for damage stuff!")]
	public bool m_delayBarriersUntilStartOfNextTurn;
	public int m_prisonSides = 8;
	public float m_prisonRadius = 3.5f;
	public StandardBarrierData m_prisonBarrierData;
	public AbilityAreaShape m_shapeForTargeter = AbilityAreaShape.Seven_x_Seven;
	[Tooltip("WARNING: don't do this if it's a Blast phase ability unless the walls don't block abilities")]
	public bool m_createBarriersImmediately;
	[Header("-- Ground effect")]
	public StandardGroundEffectInfo m_groundEffectInfo;
	public int m_damageOnCast = 30;
	[Header("-- On Cast Ally Hit (applies to caster as well)")]
	public int m_allyHealOnCast;
	public StandardEffectInfo m_effectOnAlliesOnCast;
	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private Manta_SyncComponent m_syncComp;
	private AbilityMod_MantaCreateBarriers m_abilityMod;
	private MantaCreateBarriersChainFinal m_finalDamageChain;
	private StandardBarrierData m_cachedPrisonBarrierData;
	private StandardEffectInfo m_cachedEffectOnAlliesOnCast;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Lair";
		}
		if (m_prisonSides < 3)
		{
			m_prisonSides = 4;
		}
		foreach (Ability ability in GetChainAbilities())
		{
			if (ability != null && ability is MantaCreateBarriersChainFinal final)
			{
				m_finalDamageChain = final;
				break;
			}
		}
		m_syncComp = GetComponent<Manta_SyncComponent>();
		Setup();
		ResetTooltipAndTargetingNumbers();
	}

	private void Setup()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_TeslaPrison(
			this,
			TrackerTeslaPrison.PrisonWallSegmentType.RegularPolygon,
			0,
			0,
			GetPrisonSides(),
			GetPrisonRadius(),
			GetShapeForTargeter(),
			true);
		Targeter.SetAffectedGroups(true, IncludeAllies(), false);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetPrisonRadius();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		m_prisonBarrierData.AddTooltipTokens(tokens, "PrisonBarrierData");
		AddTokenInt(tokens, "DamageOnCast", string.Empty, m_damageOnCast);
		AddTokenInt(tokens, "AllyHealOnCast", string.Empty, m_allyHealOnCast);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnAlliesOnCast, "EffectOnAlliesOnCast", m_effectOnAlliesOnCast);
	}

	private void SetCachedFields()
	{
		m_cachedPrisonBarrierData = m_abilityMod != null
			? m_abilityMod.m_prisonBarrierDataMod.GetModifiedValue(m_prisonBarrierData)
			: m_prisonBarrierData;
		m_cachedEffectOnAlliesOnCast = m_abilityMod != null
			? m_abilityMod.m_effectOnAlliesOnCastMod.GetModifiedValue(m_effectOnAlliesOnCast)
			: m_effectOnAlliesOnCast;
	}

	public bool RequireCasterInShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_requireCasterInShapeMod.GetModifiedValue(m_requireCasterInShape)
			: m_requireCasterInShape;
	}

	public AbilityAreaShape GetTargetAreaShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targetAreaShapeMod.GetModifiedValue(m_targetAreaShape)
			: m_targetAreaShape;
	}

	public bool DelayBarriersUntilStartOfNextTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_delayBarriersUntilStartOfNextTurnMod.GetModifiedValue(m_delayBarriersUntilStartOfNextTurn)
			: m_delayBarriersUntilStartOfNextTurn;
	}

	public int GetPrisonSides()
	{
		return m_abilityMod != null
			? m_abilityMod.m_prisonSidesMod.GetModifiedValue(m_prisonSides)
			: m_prisonSides;
	}

	public float GetPrisonRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_prisonRadiusMod.GetModifiedValue(m_prisonRadius)
			: m_prisonRadius;
	}

	public AbilityAreaShape GetShapeForTargeter()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shapeForTargeterMod.GetModifiedValue(m_shapeForTargeter)
			: m_shapeForTargeter;
	}

	public bool CreateBarriersImmediately()
	{
		return m_abilityMod != null
			? m_abilityMod.m_createBarriersImmediatelyMod.GetModifiedValue(m_createBarriersImmediately)
			: m_createBarriersImmediately;
	}

	public StandardGroundEffectInfo GetGroundEffectInfo()
	{
		return m_abilityMod != null && m_abilityMod.m_groundEffectInfoMod.m_applyGroundEffect
			? m_abilityMod.m_groundEffectInfoMod
			: m_groundEffectInfo;
	}

	public int GetDamageOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageOnCastMod.GetModifiedValue(m_damageOnCast)
			: m_damageOnCast;
	}

	public int GetAllyHealOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHealOnCastMod.GetModifiedValue(m_allyHealOnCast)
			: m_allyHealOnCast;
	}

	public StandardEffectInfo GetEffectOnAlliesOnCast()
	{
		return m_cachedEffectOnAlliesOnCast ?? m_effectOnAlliesOnCast;
	}

	private StandardBarrierData GetPrisonBarrierData()
	{
		return m_cachedPrisonBarrierData ?? m_prisonBarrierData;
	}

	private bool ShouldAddVisionProvider()
	{
		return m_abilityMod != null 
		       && m_abilityMod.m_addVisionProviderInsideBarriers.GetModifiedValue(false);
	}

	public bool IncludeAllies()
	{
		return GetAllyHealOnCast() > 0 || GetEffectOnAlliesOnCast().m_applyEffect;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MantaCreateBarriers))
		{
			m_abilityMod = abilityMod as AbilityMod_MantaCreateBarriers;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_finalDamageChain != null)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_finalDamageChain.GetDamageOnCast()));
		}
		else
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, GetDamageOnCast()));
			m_groundEffectInfo.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
			GetEffectOnAlliesOnCast().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
			GetEffectOnAlliesOnCast().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
			AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetAllyHealOnCast());
			AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetAllyHealOnCast());
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int damage = m_finalDamageChain != null
			? m_finalDamageChain.GetDamageOnCast()
			: GetDamageOnCast();
		if (GetGroundEffectInfo().m_applyGroundEffect && GetGroundEffectInfo().m_groundEffectData.damageAmount > 0)
		{
			damage += GetGroundEffectInfo().m_groundEffectData.damageAmount;
		}
		dictionary[AbilityTooltipSymbol.Damage] = damage;
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (m_syncComp == null)
		{
			return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
		}
		
		int energy = 0;
		foreach (AbilityUtil_Targeter.ActorTarget actorTarget in Targeters[currentTargeterIndex].GetActorsInRange())
		{
			energy += m_syncComp.GetDirtyFightingExtraTP(actorTarget.m_actor);
		}
		return energy;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return symbolType == AbilityTooltipSymbol.Damage && m_syncComp != null
			? m_syncComp.GetAccessoryStringForDamage(targetActor, ActorData, this)
			: null;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (!m_requireCasterInShape || caster.GetCurrentBoardSquare() == null)
		{
			return true;
		}
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare != null)
		{
			return AreaEffectUtils.IsSquareInShape(
				caster.GetCurrentBoardSquare(),
				GetTargetAreaShape(),
				target.FreePos,
				targetSquare,
				true,
				caster);
		}
		return false;
	}

	public override bool AllowInvalidSquareForSquareBasedTarget()
	{
		return true;
	}
}
