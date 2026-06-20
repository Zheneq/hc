// ROGUES
// SERVER
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class SenseiYingYangDash : Ability
{
	[Separator("Custom colors for Ability Bar Icon")]
	public Color m_colorForAllyDash = new Color(0f, 1f, 1f);
	public Color m_colorForEnemyDash = new Color(1f, 0f, 1f);
	[Separator("Targeting Info", "cyan")]
	public bool m_chooseDestination;
	public AbilityAreaShape m_chooseDestShape = AbilityAreaShape.Three_x_Three;
	public bool m_useActorAtSquareBeforeEvadeIfMiss = true;
	[Separator("For Second Dash", "cyan")]
	public int m_secondCastTurns = 1;
	public bool m_secondDashAllowBothTeams;
	
	// TODO SENSEI
	// rogues
	// [Separator("On Hit Authored Data")]
	// public OnHitAuthoredData OnHitData;
	
	// reactor
	[Separator("On Enemy Hit")]
	// rogues
	// [Separator("On Enemy Hit (Legacy)")]
	public int m_damage = 10;
	
	public StandardEffectInfo m_enemyHitEffect;
	public int m_extraDamageForDiffTeamSecondDash;
	public int m_extraDamageForLowHealth;
	public float m_enemyLowHealthThresh;
	public bool m_reverseHealthThreshForEnemy;
	
	// TODO SENSEI
	// reactor
	[Separator("On Ally Hit")]
	// rogues
	// [Separator("On Ally Hit (Legacy)")]
	public int m_healOnAlly = 10;
	
	public StandardEffectInfo m_allyHitEffect;
	public int m_extraHealOnAllyForDiffTeamSecondDash;
	public int m_extraHealOnAllyForLowHealth;
	public float m_allyLowHealthThresh;
	public bool m_reverseHealthThreshForAlly;
	[Header("-- whether to heal allies who dashed away")]
	public bool m_healAllyWhoDashedAway;
	[Header("-- Cooldown reduction")]
	public int m_cdrIfNoSecondDash;
	[Header("-- Sequences --")]
	public GameObject m_castOnAllySequencePrefab;
	public GameObject m_castOnEnemySequencePrefab;

	private AbilityMod_SenseiYingYangDash m_abilityMod;
	private Sensei_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedEnemyHitEffect;
	private StandardEffectInfo m_cachedAllyHitEffect;

#if SERVER
	// added in rogues
	private AbilityData.ActionType m_myActionType;

	private Passive_Sensei m_passive;

	// rogues
	// private OnHitAuthoredData m_cachedOnHitData =>
	// 	m_abilityMod != null
	// 		? m_abilityMod.m_onHitDataMod.GetModdedOnHitData(OnHitData)
	// 		: OnHitData;
#endif
	
	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "SenseiYingYangDash";
		}
		Setup();
	}

	private void Setup()
	{
#if SERVER
		// added in rogues
		AbilityData abilityData = GetComponent<AbilityData>();
		if (abilityData != null)
		{
			m_myActionType = abilityData.GetActionTypeOfAbility(this);
		}
		m_passive = GetPassiveOfType(typeof(Passive_Sensei)) as Passive_Sensei;
		// custom
		ClearTargeters();
#endif
		
		SetCachedFields();
		m_syncComp = GetComponent<Sensei_SyncComponent>();
		AbilityUtil_Targeter_Charge targeter = new AbilityUtil_Targeter_Charge(
			this,
			AbilityAreaShape.SingleSquare,
			false,
			AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos,
			true,
			true)
			{
				// TODO SENSEI
				m_affectCasterDelegate = IncludeCasterInTargeter  // removed in rogues
			};
		if (ChooseDestinaton())
		{
			Targeters.Add(targeter);
			AbilityUtil_Targeter_Charge targeter2 = new AbilityUtil_Targeter_Charge(
				this,
				AbilityAreaShape.SingleSquare,
				true,
				AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos,
				false);
			targeter2.SetUseMultiTargetUpdate(true);
			Targeters.Add(targeter2);
		}
		else
		{
			Targeter = targeter;
		}
		
		// TODO SENSEI
		// rogues
		// m_abilityMod = TalentManager.Get().GetAbilityMod(CharacterType.Sensei, CachedActionType) as AbilityMod_SenseiYingYangDash;
	}
	
	// removed in rogues
	private bool IncludeCasterInTargeter(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
	{
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		return moddedEffectForSelf != null && moddedEffectForSelf.m_applyEffect;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public bool ChooseDestinaton()
	{
		return m_chooseDestination && m_targetData.Length > 1;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return ChooseDestinaton() ? 2 : 1;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	// rogues
	// public override void GetAbilityStatusData(out Dictionary<string, string> statusData, bool includeNames = false)
	// {
	// 	base.GetAbilityStatusData(out statusData, includeNames);
	// 	base.GatherVisibleStatusTooltips(ref statusData, m_cachedOnHitData.m_effectTemplateFields, includeNames);
	// 	base.GatherVisibleStatusTooltips(ref statusData, m_cachedOnHitData.m_allyHitEffectTemplateFields, includeNames);
	// 	base.GatherVisibleStatusTooltips(ref statusData, m_cachedOnHitData.m_enemyHitEffectTemplateFields, includeNames);
	// }

	private void SetCachedFields()
	{
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
		m_cachedAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
	}

	public AbilityAreaShape GetChooseDestShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chooseDestShapeMod.GetModifiedValue(m_chooseDestShape)
			: m_chooseDestShape;
	}

	public int GetSecondCastTurns()
	{
		return m_abilityMod != null
			? m_abilityMod.m_secondCastTurnsMod.GetModifiedValue(m_secondCastTurns)
			: m_secondCastTurns;
	}

	public bool SecondDashAllowBothTeams()
	{
		return m_abilityMod != null
			? m_abilityMod.m_secondDashAllowBothTeamsMod.GetModifiedValue(m_secondDashAllowBothTeams)
			: m_secondDashAllowBothTeams;
	}

	public int GetDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public int GetExtraDamageForDiffTeamSecondDash()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForDiffTeamSecondDashMod.GetModifiedValue(m_extraDamageForDiffTeamSecondDash)
			: m_extraDamageForDiffTeamSecondDash;
	}

	public int GetExtraDamageForLowHealth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForLowHealthMod.GetModifiedValue(m_extraDamageForLowHealth)
			: m_extraDamageForLowHealth;
	}

	public float GetEnemyLowHealthThresh()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyLowHealthThreshMod.GetModifiedValue(m_enemyLowHealthThresh)
			: m_enemyLowHealthThresh;
	}

	public bool ReverseHealthThreshForEnemy()
	{
		return m_abilityMod != null
			? m_abilityMod.m_reverseHealthThreshForEnemyMod.GetModifiedValue(m_reverseHealthThreshForEnemy)
			: m_reverseHealthThreshForEnemy;
	}

	public int GetHealOnAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healOnAllyMod.GetModifiedValue(m_healOnAlly)
			: m_healOnAlly;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_cachedAllyHitEffect ?? m_allyHitEffect;
	}

	public int GetExtraHealOnAllyForDiffTeamSecondDash()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealOnAllyForDiffTeamSecondDashMod.GetModifiedValue(m_extraHealOnAllyForDiffTeamSecondDash)
			: m_extraHealOnAllyForDiffTeamSecondDash;
	}

	public int GetExtraHealOnAllyForLowHealth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealOnAllyForLowHealthMod.GetModifiedValue(m_extraHealOnAllyForLowHealth)
			: m_extraHealOnAllyForLowHealth;
	}

	public float GetAllyLowHealthThresh()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyLowHealthThreshMod.GetModifiedValue(m_allyLowHealthThresh)
			: m_allyLowHealthThresh;
	}

	public bool ReverseHealthThreshForAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_reverseHealthThreshForAllyMod.GetModifiedValue(m_reverseHealthThreshForAlly)
			: m_reverseHealthThreshForAlly;
	}

	public int GetCdrIfNoSecondDash()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfNoSecondDashMod.GetModifiedValue(m_cdrIfNoSecondDash)
			: m_cdrIfNoSecondDash;
	}

	public int GetCurrentAllyHeal(ActorData allyActor, ActorData caster)
	{
		int num = GetHealOnAlly();
		if (allyActor == null)
		{
			return num;
		}
		bool isAllyLowHealth = allyActor.GetHitPointPercent() < GetAllyLowHealthThresh();
		if (ReverseHealthThreshForAlly())
		{
			isAllyLowHealth = allyActor.GetHitPointPercent() > GetAllyLowHealthThresh();
		}
		if (GetExtraHealOnAllyForLowHealth() > 0 && GetAllyLowHealthThresh() > 0f && isAllyLowHealth)
		{
			num += GetExtraHealOnAllyForLowHealth();
		}
		if (ShouldApplyBonusForDiffTeam(allyActor, caster) && GetExtraHealOnAllyForDiffTeamSecondDash() > 0)
		{
			num += GetExtraHealOnAllyForDiffTeamSecondDash();
		}
		return num;
	}

	public int GetCurrentDamage(ActorData enemyActor, ActorData caster)
	{
		int num = GetDamage();
		if (enemyActor == null)
		{
			return num;
		}
		bool isEnemyLowHealth = enemyActor.GetHitPointPercent() < GetEnemyLowHealthThresh();
		if (ReverseHealthThreshForEnemy())
		{
			isEnemyLowHealth = enemyActor.GetHitPointPercent() > GetEnemyLowHealthThresh();
		}
		if (GetExtraDamageForLowHealth() > 0 && GetEnemyLowHealthThresh() > 0f && isEnemyLowHealth)
		{
			num += GetExtraDamageForLowHealth();
		}
		if (ShouldApplyBonusForDiffTeam(enemyActor, caster) && GetExtraDamageForDiffTeamSecondDash() > 0)
		{
			num += GetExtraDamageForDiffTeamSecondDash();
		}
		return num;
	}

	public bool CanTargetEnemy()
	{
		return m_syncComp == null
		       || !IsForSecondDash()
		       || SecondDashAllowBothTeams()
		       || m_syncComp.m_syncLastYingYangDashedToAlly;
	}

	public bool CanTargetAlly()
	{
		return m_syncComp == null
		       || !IsForSecondDash()
		       || SecondDashAllowBothTeams()
		       || !m_syncComp.m_syncLastYingYangDashedToAlly;
	}

	private bool IsForSecondDash()
	{
		return m_syncComp != null && m_syncComp.m_syncTurnsForSecondYingYangDash > 0;
	}

	private bool ShouldApplyBonusForDiffTeam(ActorData targetActor, ActorData caster)
	{
		if (IsForSecondDash())
		{
			bool isAlly = targetActor.GetTeam() == caster.GetTeam();
			return m_syncComp.m_syncLastYingYangDashedToAlly != isAlly;
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damage);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, m_healOnAlly);
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		
		// TODO SENSEI
		// removed in rogues
		AppendTooltipNumbersFromBaseModEffects(ref numbers);
		
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Ally) > 0)
		{
			results.m_healing = GetCurrentAllyHeal(targetActor, ActorData);
		}
		else if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			results.m_damage = GetCurrentDamage(targetActor, ActorData);
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		// rogues
		// OnHitData.AddTooltipTokens(tokens);
		
		AddTokenInt(tokens, "SecondCastTurns", string.Empty, m_secondCastTurns);
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "ExtraDamageForDiffTeamSecondDash", string.Empty, m_extraDamageForDiffTeamSecondDash);
		AddTokenInt(tokens, "ExtraDamageForLowHealth", string.Empty, m_extraDamageForLowHealth);
		AddTokenInt(tokens, "HealOnAlly", string.Empty, m_healOnAlly);
		AbilityMod.AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AddTokenInt(tokens, "ExtraHealOnAllyForDiffTeamSecondDash", string.Empty, m_extraHealOnAllyForDiffTeamSecondDash);
		AddTokenInt(tokens, "ExtraHealOnAllyForLowHealth", string.Empty, m_extraHealOnAllyForLowHealth);
		AddTokenInt(tokens, "CdrIfNoSecondDash", string.Empty, m_cdrIfNoSecondDash);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SenseiYingYangDash))
		{
			m_abilityMod = abilityMod as AbilityMod_SenseiYingYangDash;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(
			caster,
			CanTargetEnemy(),
			CanTargetAlly(),
			false,
			ValidateCheckPath.CanBuildPath,
			true,
			false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool isTargetValid = false;
		bool isDashValid = false;
		if (targetIndex == 0)
		{
			BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
			if (targetSquare != null && targetSquare.OccupantActor != null)
			{
				bool canTargetActorInDecision = CanTargetActorInDecision(
					caster,
					targetSquare.OccupantActor,
					CanTargetEnemy(),
					CanTargetAlly(),
					false,
					ValidateCheckPath.CanBuildPath,
					true,
					false);
				if (canTargetActorInDecision)
				{
					isTargetValid = true;
					isDashValid = true;
				}
			}
		}
		else
		{
			isTargetValid = true;
			BoardSquare prevTargetSquare = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
			BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
			if (targetSquare != null
			    && targetSquare.IsValidForGameplay()
			    && targetSquare != prevTargetSquare
			    && targetSquare != caster.GetCurrentBoardSquare())
			{
				bool isSquareInShape = AreaEffectUtils.IsSquareInShape(
					targetSquare, GetChooseDestShape(), target.FreePos, prevTargetSquare, false, caster);
				if (isSquareInShape)
				{
					isDashValid = KnockbackUtils.CanBuildStraightLineChargePath(caster, targetSquare, prevTargetSquare, false, out _);
				}
			}
		}

		return isDashValid && isTargetValid;
	}

	public override bool UseCustomAbilityIconColor()
	{
		return m_syncComp != null && m_syncComp.m_syncTurnsForSecondYingYangDash > 0;
	}

	public override Color GetCustomAbilityIconColor(ActorData actor)
	{
		if (m_syncComp != null && m_syncComp.m_syncTurnsForSecondYingYangDash > 0)
		{
			if (CanTargetAlly())
			{
				return m_colorForAllyDash;
			}
			if (CanTargetEnemy())
			{
				return m_colorForEnemyDash;
			}
		}
		return Color.white;
	}
	
#if SERVER
	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] GetChargePath(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (!ChooseDestinaton())
		{
			return base.GetChargePath(targets, caster, additionalData);
		}
		
		ServerEvadeUtils.ChargeSegment[] array = new ServerEvadeUtils.ChargeSegment[3];
		array[0] = new ServerEvadeUtils.ChargeSegment
		{
			m_pos = caster.GetCurrentBoardSquare(),
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
			m_end = BoardSquarePathInfo.ChargeEndType.None
		};
		array[1] = new ServerEvadeUtils.ChargeSegment
		{
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
			m_pos = Board.Get().GetSquare(targets[0].GridPos),
			m_end = BoardSquarePathInfo.ChargeEndType.Pivot
		};
		array[2] = new ServerEvadeUtils.ChargeSegment
		{
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
			m_pos = Board.Get().GetSquare(targets[1].GridPos),
			m_end = BoardSquarePathInfo.ChargeEndType.Impact
		};
		float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(array));
		array[0].m_segmentMovementSpeed = segmentMovementSpeed;
		array[1].m_segmentMovementSpeed = segmentMovementSpeed;
		array[2].m_segmentMovementSpeed = segmentMovementSpeed;
		return array;
	}

	// added in rogues
	public override bool GetChargeThroughInvalidSquares()
	{
		return true;
	}

	// added in rogues
	public override BoardSquare GetValidChargeTestSourceSquare(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		return chargeSegments[chargeSegments.Length - 1].m_pos;
	}

	// added in rogues
	public override Vector3 GetChargeBestSquareTestVector(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		return ServerEvadeUtils.GetChargeBestSquareTestDirection(chargeSegments);
	}

	// added in rogues
	public override BoardSquare GetIdealDestination(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return targets.Count > 1
			? Board.Get().GetSquare(targets[1].GridPos)
			: base.GetIdealDestination(targets, caster, additionalData);
	}

	// added in rogues
	public override bool ShouldTriggerCooldownOnCast(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return m_syncComp == null || m_syncComp.m_syncTurnsForSecondYingYangDash > 0;
	}

	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_syncComp == null)
		{
			return;
		}
		ActorData actorData = null;
		ActorData actorOnTargetSquareBeforeEvades = GetActorOnTargetSquareBeforeEvades(targets);
		List<ActorData> list = additionalData.m_abilityResults.HitActorList();
		list.Remove(caster);
		if (list.Count > 0 || actorOnTargetSquareBeforeEvades != null)
		{
			actorData = list.Count > 0 ? list[0] : actorOnTargetSquareBeforeEvades;
			m_syncComp.Networkm_syncLastYingYangDashedToAlly = actorData.GetTeam() == caster.GetTeam();
		}
		if (m_syncComp.m_syncTurnsForSecondYingYangDash <= 0 && actorData != null)
		{
			m_syncComp.Networkm_syncTurnsForSecondYingYangDash = (sbyte)Mathf.Clamp(GetSecondCastTurns() + 1, 0, 127);
			return;
		}
		m_syncComp.Networkm_syncTurnsForSecondYingYangDash = 0;
		if (m_passive != null)
		{
			m_passive.DashCooldownAdjust = 0;
		}
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		ActorData chargeHitActor = GetChargeHitActor(targets, caster);
		ActorData actorOnTargetSquareBeforeEvades = GetActorOnTargetSquareBeforeEvades(targets);
		GameObject prefab = chargeHitActor != null && chargeHitActor.GetTeam() != caster.GetTeam()
		                    || actorOnTargetSquareBeforeEvades != null && actorOnTargetSquareBeforeEvades.GetTeam() != caster.GetTeam()
			? m_castOnEnemySequencePrefab
			: m_castOnAllySequencePrefab;

		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				prefab,
				targets[0].FreePos,
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<ActorData> hitActors = base.GetHitActors(targets, caster, nonActorTargetInfo);
		ActorData chargeHitActor = GetChargeHitActor(targets, caster);
		if (chargeHitActor != null)
		{
			hitActors.Add(chargeHitActor);
		}
		return hitActors;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorData chargeHitActor = GetChargeHitActor(targets, caster);
		ActorData actorOnTargetSquareBeforeEvades = GetActorOnTargetSquareBeforeEvades(targets);
		ActorHitContext actorContext = new ActorHitContext();
		ContextVars abilityContext = new ContextVars();
		NumericHitResultScratch numericHitResultScratch = new NumericHitResultScratch();
		if (!(chargeHitActor != null) && !(actorOnTargetSquareBeforeEvades != null))
		{
			if (m_syncComp.m_syncTurnsForSecondYingYangDash <= 0)
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
				int moddedCooldown = GetModdedCooldown();
				MiscHitEventData_OverrideCooldown hitEvent = new MiscHitEventData_OverrideCooldown(m_myActionType, moddedCooldown);
				actorHitResults.AddMiscHitEvent(hitEvent);
				actorHitResults.SetIgnoreTechpointInteractionForHit(true);
				abilityResults.StoreActorHit(actorHitResults);
			}
			return;
		}
		int overrideValue = -1;
		int syncTurnsForSecondYingYangDash = m_syncComp.m_syncTurnsForSecondYingYangDash;
		bool flag = syncTurnsForSecondYingYangDash > 0;
		if (flag)
		{
			overrideValue = Mathf.Max(0, GetModdedCooldown() - (GetSecondCastTurns() - syncTurnsForSecondYingYangDash + 1) + (m_passive ? m_passive.DashCooldownAdjust : 0));
		}
		MiscHitEventData_OverrideCooldown hitEvent2 = new MiscHitEventData_OverrideCooldown(m_myActionType, overrideValue);
		if (chargeHitActor != null)
		{
			ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(chargeHitActor, caster.GetFreePos()));
			if (chargeHitActor.GetTeam() == caster.GetTeam())
			{
				int currentAllyHeal = GetCurrentAllyHeal(chargeHitActor, caster);
				actorHitResults2.SetBaseHealing(currentAllyHeal);
				actorHitResults2.AddStandardEffectInfo(GetAllyHitEffect());
				
				// rogues - this ability does not use generic ability system in reactor
				// GenericAbility_Container.CalcIntFieldValues(chargeHitActor, caster, actorContext, abilityContext, m_cachedOnHitData.m_allyHitIntFields, numericHitResultScratch);
				// GenericAbility_Container.SetNumericFieldsOnHitResults(actorHitResults2, numericHitResultScratch);
				// GenericAbility_Container.SetKnockbackFieldsOnHitResults(chargeHitActor, caster, actorContext, abilityContext, actorHitResults2, m_cachedOnHitData.m_allyHitKnockbackFields);
				// GenericAbility_Container.SetCooldownReductionFieldsOnHitResults(chargeHitActor, caster, actorContext, abilityContext, actorHitResults2, m_cachedOnHitData.m_allyHitCooldownReductionFields, 1);
				// GenericAbility_Container.SetEffectFieldsOnHitResults(chargeHitActor, caster, actorContext, abilityContext, actorHitResults2, m_cachedOnHitData.m_allyHitEffectFields);
				// GenericAbility_Container.SetEffectTemplateFieldsOnHitResults(chargeHitActor, caster, actorContext, abilityContext, actorHitResults2, m_cachedOnHitData.m_allyHitEffectTemplateFields);
			}
			else
			{
				int currentDamage = GetCurrentDamage(chargeHitActor, caster);
				actorHitResults2.SetBaseDamage(currentDamage);
				actorHitResults2.AddStandardEffectInfo(GetEnemyHitEffect());
				
				// rogues - this ability does not use generic ability system in reactor
				// GenericAbility_Container.CalcIntFieldValues(chargeHitActor, caster, actorContext, abilityContext, m_cachedOnHitData.m_enemyHitIntFields, numericHitResultScratch);
				// GenericAbility_Container.SetNumericFieldsOnHitResults(actorHitResults2, numericHitResultScratch);
				// GenericAbility_Container.SetKnockbackFieldsOnHitResults(chargeHitActor, caster, actorContext, abilityContext, actorHitResults2, m_cachedOnHitData.m_enemyHitKnockbackFields);
				// GenericAbility_Container.SetCooldownReductionFieldsOnHitResults(chargeHitActor, caster, actorContext, abilityContext, actorHitResults2, m_cachedOnHitData.m_enemyHitCooldownReductionFields, 1);
				// GenericAbility_Container.SetEffectFieldsOnHitResults(chargeHitActor, caster, actorContext, abilityContext, actorHitResults2, m_cachedOnHitData.m_enemyHitEffectFields);
				// GenericAbility_Container.SetEffectTemplateFieldsOnHitResults(chargeHitActor, caster, actorContext, abilityContext, actorHitResults2, m_cachedOnHitData.m_enemyHitEffectTemplateFields);
			}
			if (flag)
			{
				actorHitResults2.AddMiscHitEvent(hitEvent2);
			}
			abilityResults.StoreActorHit(actorHitResults2);
			return;
		}
		ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		if (flag)
		{
			casterHitResults.AddMiscHitEvent(hitEvent2);
		}
		casterHitResults.SetIgnoreTechpointInteractionForHit(true);
		abilityResults.StoreActorHit(casterHitResults);
	}

	// added in rogues
	private ActorData GetChargeHitActor(List<AbilityTarget> targets, ActorData caster)
	{
		ActorData actorData = null;
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			AbilityAreaShape.SingleSquare, targets[0], true, caster, null, null);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInShape);
		if (actorsInShape.Count > 0)
		{
			actorData = actorsInShape[0];
		}
		if (actorData == null && m_healAllyWhoDashedAway)
		{
			ActorData actorOnTargetSquareBeforeEvades = GetActorOnTargetSquareBeforeEvades(targets);
			if (actorOnTargetSquareBeforeEvades != null && actorOnTargetSquareBeforeEvades.GetTeam() == caster.GetTeam())
			{
				actorData = actorOnTargetSquareBeforeEvades;
			}
		}
		return actorData;
	}

	// added in rogues
	private ActorData GetActorOnTargetSquareBeforeEvades(List<AbilityTarget> targets)
	{
		if (m_useActorAtSquareBeforeEvadeIfMiss)
		{
			BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
			if (square != null)
			{
				return AreaEffectUtils.GetActorOnSquareOnPhaseStart(square);
			}
		}
		return null;
	}

	// added in rogues
	public override void OnDodgedDamage(ActorData caster, int damageDodged)
	{
		caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SenseiStats.DamageDodgedPlusDamageDealtPlusHealingDealtByDash, damageDodged);
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SenseiStats.DamageDodgedPlusDamageDealtPlusHealingDealtByDash, results.FinalDamage);
		}
		if (results.FinalHealing > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SenseiStats.DamageDodgedPlusDamageDealtPlusHealingDealtByDash, results.FinalHealing);
		}
	}
#endif
}
