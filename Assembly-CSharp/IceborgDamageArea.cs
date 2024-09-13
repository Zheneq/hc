using System.Collections.Generic;
using System.Linq;
using AbilityContextNamespace;
using UnityEngine;

public class IceborgDamageArea : GenericAbility_Container
{
	[Separator("Targeting, Max Ranges")]
	public float m_initialCastMaxRange = 5.5f;
	public float m_moveAreaCastMaxRange = 1.45f;
	public bool m_targetingAreaCheckLos = true;
	public bool m_moveAreaTargetingCheckLos;
	[Separator("Movement Adjust Type for Moving Field")]
	public MovementAdjustment m_moveAreaMovementAdjustType;
	[Separator("Whether to add damage field")]
	public bool m_addGroundField;
	public bool m_stopMoversWithSlowStatus; // TODO ICEBORG unused, always false
	public bool m_stopMoverIfHitPreviousTurn; // TODO ICEBORG unused, always false
	public GroundEffectField m_groundFieldData;
	[Separator("Extra Damage on Initial Cast")]
	public int m_extraDamageOnInitialCast;
	[Separator("Damage change on ground field per turn")]
	public int m_groundFieldDamageChangePerTurn;
	[Separator("Min Damage")]
	public int m_minDamage;
	[Separator("Shielding per enemy hit on cast")]
	public int m_shieldPerEnemyHit; // TODO ICEBORG unused, always 0
	public int m_shieldDuration = 1; // TODO ICEBORG unused, always 0
	[Separator("Effect to apply if target has been hit by this ability on previous turn")]
	public StandardEffectInfo m_effectOnEnemyIfHitPreviousTurn;
	[Separator("Apply Nova effect?")]
	public bool m_applyDelayedAoeEffect; // TODO ICEBORG unused, always false
	public bool m_applyNovaCoreIfHitPreviousTurn;
	[Separator("Animation index for moving field")]
	public int m_animationIndexForMoveArea;
	[Separator("Sequence for moving field")]
	public GameObject m_moveFieldSeqPrefab;
	[Header("-- For timing of removing existing field")]
	public GameObject m_fieldRemoveOnMoveSeqPrefab;

	private AbilityMod_IceborgDamageArea m_abilityMod;
	private Iceborg_SyncComponent m_syncComp;

	public static ContextNameKeyPair s_cvarTurnsSinceInitialCast = new ContextNameKeyPair("TurnsSinceInitialCast");

	private GroundEffectField m_cachedGroundFieldData;
	private StandardEffectInfo m_cachedEffectOnEnemyIfHitPreviousTurn;

	public override string GetUsageForEditor()
	{
		return base.GetUsageForEditor()
		       + ContextVars.GetContextUsageStr(
			       IceborgConeOrLaser.s_cvarHasSlow.GetName(),
			       "Set on enemies hit, 1 if has Slow, 0 otherwise")
		       + ContextVars.GetContextUsageStr(
			       s_cvarTurnsSinceInitialCast.GetName(),
			       "turns since initial cast, 0 on first turn", false);
	}

	public override List<string> GetContextNamesForEditor()
	{
		List<string> contextNamesForEditor = base.GetContextNamesForEditor();
		contextNamesForEditor.Add(IceborgConeOrLaser.s_cvarHasSlow.GetName());
		contextNamesForEditor.Add(s_cvarTurnsSinceInitialCast.GetName());
		return contextNamesForEditor;
	}

	protected override void SetupTargetersAndCachedVars()
	{
		m_syncComp = GetComponent<Iceborg_SyncComponent>();
		base.SetupTargetersAndCachedVars();

#if SERVER // TODO HACK
		m_groundFieldData.effectOnEnemies.m_applyEffect = true;
#endif
		
		SetCachedFields();
		if (GetTargetSelectComp() is TargetSelect_Shape)
		{
			TargetSelect_Shape targetSelect_Shape = GetTargetSelectComp() as TargetSelect_Shape;
			targetSelect_Shape.m_isMovingShapeDelegate = IsMovingShape;
			targetSelect_Shape.m_moveStartSquareDelegate = GetMoveStartSquare;
			targetSelect_Shape.m_moveStartFreePosDelegate = GetMoveStartFreePos;
		}
		if (Targeter is AbilityUtil_Targeter_MovingShape)
		{
			AbilityUtil_Targeter_MovingShape abilityUtil_Targeter_MovingShape = Targeter as AbilityUtil_Targeter_MovingShape;
			abilityUtil_Targeter_MovingShape.m_delegateIsMovingShape = IsMovingShape;
			abilityUtil_Targeter_MovingShape.m_delegateMoveStartSquare = GetMoveStartSquare;
			abilityUtil_Targeter_MovingShape.m_delegateMoveStartFreePos = GetMoveStartFreePos;
		}

		if (m_animationIndexForMoveArea < 0)
		{
			m_animationIndexForMoveArea = 0;
		}
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AddTokenInt(tokens, "ExtraDamageOnInitialCast", string.Empty, m_extraDamageOnInitialCast);
		AddTokenInt(tokens, "GroundFieldDamageChangePerTurn", string.Empty, m_groundFieldDamageChangePerTurn);
		AddTokenInt(tokens, "MinDamage", string.Empty, m_minDamage);
		AddTokenInt(tokens, "ShieldPerEnemyHit", string.Empty, m_shieldPerEnemyHit);
		AddTokenInt(tokens, "ShieldDuration", string.Empty, m_shieldDuration);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnEnemyIfHitPreviousTurn, "EffectOnEnemyIfHitPreviousTurn", m_effectOnEnemyIfHitPreviousTurn);
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Iceborg_SyncComponent>();
		}
		if (m_syncComp != null)
		{
			m_syncComp.AddTooltipTokens(tokens);
		}
	}

	private void SetCachedFields()
	{
		m_cachedGroundFieldData = m_abilityMod != null
			? m_abilityMod.m_groundFieldDataMod.GetModifiedValue(m_groundFieldData)
			: m_groundFieldData;
		m_cachedEffectOnEnemyIfHitPreviousTurn = m_abilityMod != null
			? m_abilityMod.m_effectOnEnemyIfHitPreviousTurnMod.GetModifiedValue(m_effectOnEnemyIfHitPreviousTurn)
			: m_effectOnEnemyIfHitPreviousTurn;
	}

	public float GetInitialCastMaxRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_initialCastMaxRangeMod.GetModifiedValue(m_initialCastMaxRange)
			: m_initialCastMaxRange;
	}

	public float GetMoveAreaCastMaxRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_moveAreaCastMaxRangeMod.GetModifiedValue(m_moveAreaCastMaxRange)
			: m_moveAreaCastMaxRange;
	}

	public bool TargetingAreaCheckLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targetingAreaCheckLosMod.GetModifiedValue(m_targetingAreaCheckLos)
			: m_targetingAreaCheckLos;
	}

	public bool AddGroundField()
	{
		return m_abilityMod != null
			? m_abilityMod.m_addGroundFieldMod.GetModifiedValue(m_addGroundField)
			: m_addGroundField;
	}

	// TODO ICEBORG unused, always false
	public bool StopMoversWithSlowStatus()
	{
		return m_abilityMod != null
			? m_abilityMod.m_stopMoversWithSlowStatusMod.GetModifiedValue(m_stopMoversWithSlowStatus)
			: m_stopMoversWithSlowStatus;
	}

	// TODO ICEBORG unused, always false
	public bool StopMoverIfHitPreviousTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_stopMoverIfHitPreviousTurnMod.GetModifiedValue(m_stopMoverIfHitPreviousTurn)
			: m_stopMoverIfHitPreviousTurn;
	}

	public GroundEffectField GetGroundFieldData()
	{
		return m_cachedGroundFieldData ?? m_groundFieldData;
	}

	public int GetExtraDamageOnInitialCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageOnInitialCastMod.GetModifiedValue(m_extraDamageOnInitialCast)
			: m_extraDamageOnInitialCast;
	}

	public int GetGroundFieldDamageChangePerTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_groundFieldDamageChangePerTurnMod.GetModifiedValue(m_groundFieldDamageChangePerTurn)
			: m_groundFieldDamageChangePerTurn;
	}

	public int GetMinDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minDamageMod.GetModifiedValue(m_minDamage)
			: m_minDamage;
	}

	// TODO ICEBORG unused, always 0
	public int GetShieldPerEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldPerEnemyHitMod.GetModifiedValue(m_shieldPerEnemyHit)
			: m_shieldPerEnemyHit;
	}

	// TODO ICEBORG unused, always 0
	public int GetShieldDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldDurationMod.GetModifiedValue(m_shieldDuration)
			: m_shieldDuration;
	}

	public StandardEffectInfo GetEffectOnEnemyIfHitPreviousTurn()
	{
		return m_cachedEffectOnEnemyIfHitPreviousTurn ?? m_effectOnEnemyIfHitPreviousTurn;
	}

	// TODO ICEBORG unused, always false
	public bool ApplyDelayedAoeEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_applyDelayedAoeEffectMod.GetModifiedValue(m_applyDelayedAoeEffect)
			: m_applyDelayedAoeEffect;
	}

	public bool ApplyNovaCoreIfHitPreviousTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_applyNovaCoreIfHitPreviousTurnMod.GetModifiedValue(m_applyNovaCoreIfHitPreviousTurn)
			: m_applyNovaCoreIfHitPreviousTurn;
	}

	public override void PostProcessTargetingNumbers(
		ActorData targetActor,
		int currentTargeterIndex,
		Dictionary<ActorData, ActorHitContext> actorHitContext,
		ContextVars abilityContext,
		ActorData caster,
		TargetingNumberUpdateScratch results)
	{
		IceborgConeOrLaser.SetShieldPerEnemyHitTargetingNumbers(targetActor, caster, GetShieldPerEnemyHit(), actorHitContext, results);
		if (targetActor.GetTeam() == caster.GetTeam())
		{
			return;
		}
		if (!CanCastToMoveArea() && GetExtraDamageOnInitialCast() > 0)
		{
			results.m_damage += GetExtraDamageOnInitialCast();
		}
		if (CanCastToMoveArea() && GetGroundFieldDamageChangePerTurn() != 0)
		{
			results.m_damage += m_syncComp.GetTurnsSinceInitialCast() * GetGroundFieldDamageChangePerTurn();
		}
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		return m_syncComp != null
			? m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, ActorData)
			: null;
	}

	public bool IsMovingShape(ActorData caster)
	{
		return CanCastToMoveArea();
	}

	public BoardSquare GetMoveStartSquare(AbilityTarget target, ActorData caster)
	{
		BoardSquare result = caster.GetCurrentBoardSquare();
		if (IsMovingShape(caster))
		{
			BoardSquare boardSquare = Board.Get().GetSquareFromIndex(m_syncComp.m_damageAreaCenterX, m_syncComp.m_damageAreaCenterY);
			if (boardSquare != null)
			{
				result = boardSquare;
			}
		}
		return result;
	}

	public Vector3 GetMoveStartFreePos(AbilityTarget target, ActorData caster)
	{
		return IsMovingShape(caster)
			? m_syncComp.m_damageAreaFreePos
			: caster.GetFreePos();
	}

	public bool CanCastToMoveArea()
	{
		return m_syncComp != null
		       && m_syncComp.m_damageAreaCanMoveThisTurn
		       && m_syncComp.m_damageAreaCenterX >= 0
		       && m_syncComp.m_damageAreaCenterY >= 0;
	}

	public override bool IsFreeAction()
	{
		return CanCastToMoveArea();
	}

	public override int GetModdedCost()
	{
		return CanCastToMoveArea()
			? 0
			: base.GetModdedCost();
	}

	public override MovementAdjustment GetMovementAdjustment()
	{
		return CanCastToMoveArea()
			? m_moveAreaMovementAdjustType
			: base.GetMovementAdjustment();
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null || !targetSquare.IsValidForGameplay())
		{
			return false;
		}
		BoardSquare startSquare = caster.GetCurrentBoardSquare();
		Vector3 startPos = startSquare.ToVector3();
		float moveRange = GetInitialCastMaxRange();
		bool checkLos = TargetingAreaCheckLos();
		if (CanCastToMoveArea())
		{
			BoardSquare prevSquare = Board.Get().GetSquareFromIndex(m_syncComp.m_damageAreaCenterX, m_syncComp.m_damageAreaCenterY);
			if (prevSquare != null)
			{
				if (targetSquare == prevSquare)
				{
					return false;
				}
				startSquare = prevSquare;
				moveRange = GetMoveAreaCastMaxRange();
				startPos = AreaEffectUtils.GetCenterOfShape(GetGroundFieldData().shape, m_syncComp.m_damageAreaFreePos, prevSquare);
			}
			checkLos = m_moveAreaTargetingCheckLos;
		}
		float dist = VectorUtils.HorizontalPlaneDistInSquares(targetSquare.ToVector3(), startPos);
		bool isInRange = dist <= moveRange;
		bool passedLosCheck = !isInRange || !checkLos || startSquare.GetLOS(targetSquare.x, targetSquare.y);
		return isInRange && passedLosCheck;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		return CanCastToMoveArea()
			? (ActorModelData.ActionAnimationType)m_animationIndexForMoveArea
			: base.GetActionAnimType();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return !CanCastToMoveArea();
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return CanCastToMoveArea()
			? 0f
			: GetInitialCastMaxRange();
	}

	protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
	{
		m_abilityMod = abilityMod as AbilityMod_IceborgDamageArea;
	}

	protected override void GenModImpl_ClearModRef()
	{
		m_abilityMod = null;
	}

#if SERVER

	public override void Run(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);
		
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		m_syncComp.Networkm_damageAreaCenterX = (short)targetSquare.x;
		m_syncComp.Networkm_damageAreaCenterY = (short)targetSquare.y;
		m_syncComp.Networkm_damageAreaFreePos = AreaEffectUtils.GetCenterOfShape(GetGroundFieldData().shape, targets[0].FreePos, targetSquare);
	}

	// custom
	protected override void PreProcessForCalcAbilityHits(
		List<AbilityTarget> targets,
		ActorData caster,
		Dictionary<ActorData, ActorHitContext> actorHitContextMap,
		ContextVars abilityContext)
	{
		//BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		base.PreProcessForCalcAbilityHits(targets, caster, actorHitContextMap, abilityContext);
		// When casting the ability, we set the context vars and syncvars of the turn it was casted
		if (!CanCastToMoveArea())
		{
			m_syncComp.Networkm_damageFieldLastCastTurn = (short)GameFlowData.Get().CurrentTurn;
		}
		
		abilityContext.SetValue(s_cvarTurnsSinceInitialCast.GetKey(), m_syncComp.GetTurnsSinceInitialCast());
	}

	// custom
	protected override void ProcessGatheredHits(
		List<AbilityTarget> targets,
		ActorData caster,
		AbilityResults abilityResults,
		List<ActorHitResults> actorHitResults,
		List<PositionHitResults> positionHitResults,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		base.ProcessGatheredHits(targets, caster, abilityResults, actorHitResults, positionHitResults, nonActorTargetInfo);

		if (IsMovingShape(caster))
		{
			ProcessGatheredHitsForMovingShape(
				targets,
				caster,
				abilityResults,
				actorHitResults,
				positionHitResults,
				nonActorTargetInfo);
		}
		else
		{
			ProcessGatheredHitsForCast(targets, caster, abilityResults, actorHitResults, positionHitResults, nonActorTargetInfo);
		}

		if (AddGroundField())
		{
			List<ActorData> hitActors = actorHitResults.Select(ahr => ahr.m_hitParameters.Target).ToList();
			AddDamageAreaEffect(targets, caster, positionHitResults, hitActors);
		}
	}

	private void ProcessGatheredHitsForCast(
		List<AbilityTarget> targets,
		ActorData caster,
		AbilityResults abilityResults,
		List<ActorHitResults> actorHitResults,
		List<PositionHitResults> positionHitResults,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		foreach (ActorHitResults hitResults in actorHitResults)
		{
			if (hitResults.m_hitParameters.Target.GetTeam() == caster.GetTeam())
			{
				continue;
			}
	        
			hitResults.AddBaseDamage(GetExtraDamageOnInitialCast());
		}
	}
	
	// custom
	private void AddDamageAreaEffect(
		List<AbilityTarget> targets,
		ActorData caster,
		List<PositionHitResults> positionHitResults,
		List<ActorData> alreadyHitActors)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(GetGroundFieldData().shape, targets[0]);

		// Add Damage Area Effect
		IceborgDamageAreaEffect standardGroundEffect = new IceborgDamageAreaEffect(
			AsEffectSource(),
			targetSquare,
			targets[0].FreePos,
			caster,
			alreadyHitActors,
			GetGroundFieldData(),
			GetGroundFieldDamageChangePerTurn(),
			GetExtraDamageOnInitialCast(),
			GetMinDamage(),
			GetEffectOnEnemyIfHitPreviousTurn(),
			ApplyNovaCoreIfHitPreviousTurn());
		standardGroundEffect.m_time.duration -= m_syncComp.GetTurnsSinceInitialCast();
		PositionHitParameters positionHitParameters = new PositionHitParameters(centerOfShape);
		// positionHitParameters.Caster = caster;
		PositionHitResults positionHit = new PositionHitResults(standardGroundEffect, positionHitParameters);
		positionHitResults.Add(positionHit);
	}

	// custom
	private void ProcessGatheredHitsForMovingShape(
		List<AbilityTarget> targets,
		ActorData caster,
		AbilityResults abilityResults,
		List<ActorHitResults> actorHitResults,
		List<PositionHitResults> positionHitResults,
		List<NonActorTargetInfo> nonActorTargetInfo)
    {
	    IceborgDamageAreaEffect effect = ServerEffectManager.Get()
		    .GetWorldEffectsByCaster(caster, typeof(IceborgDamageAreaEffect))
		    .FirstOrDefault() as IceborgDamageAreaEffect;
	    
        // If we move the area, we have to modify the damage
        foreach (ActorHitResults hitResults in actorHitResults)
        {
	        ActorData hitActor = hitResults.m_hitParameters.Target;
	        if (hitActor.GetTeam() == caster.GetTeam())
	        {
		        continue;
	        }
	        
            hitResults.AddBaseDamage(GetGroundFieldDamageChangePerTurn() * m_syncComp.GetTurnsSinceInitialCast());
            if (hitResults.BaseDamage < GetMinDamage())
            {
                hitResults.SetBaseDamage(GetMinDamage());
            }
            
            if (effect != null && effect.IsActorHitThisTurn(hitActor, true))
            {
	            hitResults.SetBaseDamage(0);
            }

            if (m_syncComp.m_actorsHitByDamageAreaOnPrevTurn.Contains(hitActor))
            {
	            hitResults.AddStandardEffectInfo(GetEffectOnEnemyIfHitPreviousTurn());
	            if (ApplyNovaCoreIfHitPreviousTurn())
	            {
		            hitResults.AddEffect(
			            m_syncComp.CreateNovaCoreEffect(
				            AsEffectSource(),
				            hitActor.GetCurrentBoardSquare(),
				            hitActor,
				            caster));
	            }
            }
        }

        if (effect != null)
        {
	        PositionHitParameters posHitParams = new PositionHitParameters(Vector3.zero);
	        PositionHitResults positionHitResults1 = new PositionHitResults(posHitParams);
	        positionHitResults1.AddEffectForRemoval(effect, ServerEffectManager.Get().GetWorldEffects());
	        // positionHitResults1.AddEffectSequenceToEnd(GetGroundFieldData().persistentSequencePrefab, effect.m_guid);
	        positionHitResults.Add(positionHitResults1);
        }
    }
	
	// custom
    public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
	    List<AbilityTarget> targets,
	    ActorData caster,
	    ServerAbilityUtils.AbilityRunData additionalData)
    {
	    if (!IsMovingShape(caster))
	    {
		    return base.GetAbilityRunSequenceStartDataList(targets, caster, additionalData);
	    }

        BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		Vector3 shapeCenter = AreaEffectUtils.GetCenterOfShape(GetGroundFieldData().shape, targets[0].FreePos, targetSquare);
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_moveFieldSeqPrefab,
				shapeCenter,
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource,
				new Sequence.IExtraSequenceParams[]
				{
					new SplineProjectileSequence.DelayedProjectileExtraParams
					{
						useOverrideStartPos = true,
						overrideStartPos = Board.Get().GetSquareFromIndex(
							m_syncComp.m_damageAreaCenterX,
							m_syncComp.m_damageAreaCenterY)
							.ToVector3()
					}
				}),
			new ServerClientUtils.SequenceStartData(
				m_fieldRemoveOnMoveSeqPrefab,
				Vector3.zero, 
				null,
				caster,
				additionalData.m_sequenceSource)
		};
    }
	
    // custom
    public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
    {
	    if (results.AppliedStatus(StatusType.Rooted) || results.AppliedStatus(StatusType.Snared))
	    {
		    caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.IceborgStats.NumSlowsPlusRootsApplied);
	    }
    }

    // custom
    public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
    {
	    if (results.FinalDamage > 0)
	    {
		    caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.IceborgStats.UltDamage, results.FinalDamage);
	    }
    }
#endif
}
