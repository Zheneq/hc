﻿// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SamuraiSwordDash : Ability
{
	[Header("-- Targeting")]
	public float m_damageRadius = 2f;
	public float m_damageRadiusAtStart = 2f;
	public float m_damageRadiusAtEnd;
	public bool m_penetrateLineOfSight;
	[Space(5f)]
	public bool m_canMoveAfterEvade;
	[Header("-- MaxTargets -> How many targets can be hit total | MaxDamageTargets-> how many targets can be damaged")]
	public int m_maxTargets;
	public int m_maxDamageTargets = 1;
	[Header("-- Enemy Hits, Dash Phase")]
	public int m_dashDamage = 30;
	public int m_dashLessDamagePerTarget = 5;
	public StandardEffectInfo m_dashEnemyHitEffect;
	[Header("-- Effect on Self")]
	public StandardEffectInfo m_dashSelfHitEffect;
	[Header("-- Mark data")]
	public StandardEffectInfo m_markEffectInfo;
	[Header("-- Energy Refund if target dashed away")]
	public int m_energyRefundIfTargetDashedAway;
	public bool m_energyRefundIgnoreBuff = true;
	[Separator("For Chain Ability (Knockback phase)")]
	public int m_knockbackDamage = 30;
	public int m_knockbackLessDamagePerTarget = 5;
	public float m_knockbackExtraDamageFromDamageTakenMult;
	[Space(10f)]
	public int m_knockbackExtraDamageByDist;
	public int m_knockbackExtraDamageChangePerDist;
	[Header("-- Knockback")]
	public float m_knockbackDist = 2f;
	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;
	[Separator("Sequences - Dash Phase (for knockback sequences, see chain ability)")]
	public GameObject m_castSequencePrefab;
	public GameObject m_afterimageSequencePrefab;

	private SamuraiAfterimageStrike m_chainedStrike;
	private AbilityMod_SamuraiSwordDash m_abilityMod;
	private Samurai_SyncComponent m_syncComponent;
	private StandardEffectInfo m_cachedDashEnemyHitEffect;
	private StandardEffectInfo m_cachedDashSelfHitEffect;
	private StandardEffectInfo m_cachedMarkEffectInfo;
	
#if SERVER
	// added in rogues
	private List<ActorData> m_lastGatheredHitActors = new List<ActorData>();
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Sword Dash";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_chainedStrike == null)
		{
			foreach (Ability ability in m_chainAbilities)
			{
				m_chainedStrike = ability as SamuraiAfterimageStrike;
				if (m_chainedStrike != null)
				{
					break;
				}
			}
		}
		m_syncComponent = ActorData.GetComponent<Samurai_SyncComponent>();
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_SamuraiShowdown(
			this,
			GetDamageRadiusAtStart(),
			GetDamageRadiusAtEnd(),
			GetDamageRadius(),
			GetMaxTargets(),
			false,
			PenetrateLineOfSight(),
			m_knockbackDist,
			m_knockbackType);
	}

	private void SetCachedFields()
	{
		m_cachedDashEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_dashEnemyHitEffectMod.GetModifiedValue(m_dashEnemyHitEffect)
			: m_dashEnemyHitEffect;
		m_cachedDashSelfHitEffect = m_abilityMod != null
			? m_abilityMod.m_dashSelfHitEffectMod.GetModifiedValue(m_dashSelfHitEffect)
			: m_dashSelfHitEffect;
		m_cachedMarkEffectInfo = m_abilityMod != null
			? m_abilityMod.m_markEffectInfoMod.GetModifiedValue(m_markEffectInfo)
			: m_markEffectInfo;
	}

	public float GetDamageRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageRadiusMod.GetModifiedValue(m_damageRadius)
			: m_damageRadius;
	}

	public float GetDamageRadiusAtStart()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageRadiusAtStartMod.GetModifiedValue(m_damageRadiusAtStart)
			: m_damageRadiusAtStart;
	}

	public float GetDamageRadiusAtEnd()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageRadiusAtEndMod.GetModifiedValue(m_damageRadiusAtEnd)
			: m_damageRadiusAtEnd;
	}

	public bool PenetrateLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight)
			: m_penetrateLineOfSight;
	}

	public bool CanMoveAfterEvade()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canMoveAfterEvadeMod.GetModifiedValue(m_canMoveAfterEvade)
			: m_canMoveAfterEvade;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public int GetMaxDamageTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxDamageTargetsMod.GetModifiedValue(m_maxDamageTargets)
			: m_maxDamageTargets;
	}

	public int GetDashDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashDamageMod.GetModifiedValue(m_dashDamage)
			: m_dashDamage;
	}

	public int GetDashLessDamagePerTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashLessDamagePerTargetMod.GetModifiedValue(m_dashLessDamagePerTarget)
			: m_dashLessDamagePerTarget;
	}

	public StandardEffectInfo GetDashEnemyHitEffect()
	{
		return m_cachedDashEnemyHitEffect ?? m_dashEnemyHitEffect;
	}

	public StandardEffectInfo GetDashSelfHitEffect()
	{
		return m_cachedDashSelfHitEffect ?? m_dashSelfHitEffect;
	}

	public StandardEffectInfo GetMarkEffectInfo()
	{
		return m_cachedMarkEffectInfo ?? m_markEffectInfo;
	}

	public int GetEnergyRefundIfTargetDashedAway()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyRefundIfTargetDashedAwayMod.GetModifiedValue(m_energyRefundIfTargetDashedAway)
			: m_energyRefundIfTargetDashedAway;
	}

	public int GetKnockbackDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDamageMod.GetModifiedValue(m_knockbackDamage)
			: m_knockbackDamage;
	}

	public int GetKnockbackLessDamagePerTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackLessDamagePerTargetMod.GetModifiedValue(m_knockbackLessDamagePerTarget)
			: m_knockbackLessDamagePerTarget;
	}

	public float GetKnockbackExtraDamageFromDamageTakenMult()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackExtraDamageFromDamageTakenMultMod.GetModifiedValue(m_knockbackExtraDamageFromDamageTakenMult)
			: m_knockbackExtraDamageFromDamageTakenMult;
	}

	public int GetKnockbackExtraDamageByDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackExtraDamageByDistMod.GetModifiedValue(m_knockbackExtraDamageByDist)
			: m_knockbackExtraDamageByDist;
	}

	public int GetKnockbackExtraDamageChangePerDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackExtraDamageChangePerDistMod.GetModifiedValue(m_knockbackExtraDamageChangePerDist)
			: m_knockbackExtraDamageChangePerDist;
	}

	public float GetKnockbackDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistMod.GetModifiedValue(m_knockbackDist)
			: m_knockbackDist;
	}

	public KnockbackType GetKnockbackType()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType)
			: m_knockbackType;
	}

	public int CalcExtraDamageForDashDist(BoardSquare startSquare, BoardSquare endSquare)
	{
		int damage = 0;
		if (GetKnockbackExtraDamageByDist() > 0 || GetKnockbackExtraDamageChangePerDist() != 0)
		{
			damage = GetKnockbackExtraDamageByDist();
			if (endSquare != null
			    && startSquare != null
			    && GetKnockbackExtraDamageChangePerDist() != 0
			    && KnockbackUtils.CanBuildStraightLineChargePath(ActorData, endSquare, startSquare, false, out int numSquaresInPath))
			{
				int factor = numSquaresInPath - 2;
				if (factor > 0)
				{
					damage += factor * GetKnockbackExtraDamageChangePerDist();
				}
			}
		}
		return damage;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		int damage = GetDashDamage();
		if (m_chainedStrike != null)
		{
			damage += GetKnockbackDamage();
		}
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, damage));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (Targeter is AbilityUtil_Targeter_SamuraiShowdown targeter && targeter.OrderedHitActors.Contains(targetActor))
		{
			Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
			bool isValidTarget = true;
			if (GetMaxDamageTargets() > 0)
			{
				int targetIndex = targeter.OrderedHitActors.IndexOf(targetActor);
				if (targetIndex >= GetMaxDamageTargets())
				{
					isValidTarget = false;
				}
			}
			if (isValidTarget)
			{
				int baseDamage = GetDashDamage();
				int damageReduction = GetDashLessDamagePerTarget();
				if (m_chainedStrike != null)
				{
					baseDamage += GetKnockbackDamage();
					damageReduction += m_knockbackLessDamagePerTarget;
				}
				int damage = Mathf.Max(0, baseDamage - damageReduction * (Targeter.GetNumActorsInRange() - 1));
				AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, damage);
				if (m_syncComponent != null)
				{
					// reactor
					symbolToValue[AbilityTooltipSymbol.Damage] += m_syncComponent.CalcExtraDamageFromSelfBuffAbility();
					// rogues (inlined)
					// int num4 = 0;
					// if (m_syncComponent.IsSelfBuffActive(ref num4))
					// {
					// 	symbolToValue[AbilityTooltipSymbol.Damage] += num4;
					// }
					// symbolToValue[AbilityTooltipSymbol.Damage] += m_syncComponent.GetExtraDamageFromQueuedSelfBuff();
				}
				BoardSquare targeterSquare = Board.Get().GetSquare(Targeter.LastUpdatingGridPos);
				BoardSquare casterSquare = ActorData.CurrentBoardSquare;
				symbolToValue[AbilityTooltipSymbol.Damage] += CalcExtraDamageForDashDist(casterSquare, targeterSquare);
			}
			else
			{
				symbolToValue[AbilityTooltipSymbol.Damage] = 0;
			}
			return symbolToValue;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		SamuraiAfterimageStrike chainAbility = null;
		Ability[] chainAbilities = m_chainAbilities;
		foreach (Ability ability in chainAbilities)
		{
			chainAbility = ability as SamuraiAfterimageStrike;
			if (chainAbility != null)
			{
				break;
			}
		}
		int baseDamage = m_dashDamage;
		int damageReduction = m_dashLessDamagePerTarget;
		if (chainAbility != null)
		{
			baseDamage += m_knockbackDamage;
			damageReduction += m_knockbackLessDamagePerTarget;
		}
		AddTokenInt(tokens, "DamageAmount", "includes chained AfterimageStrike if present", baseDamage);
		AddTokenInt(tokens, "LessDamagePerTarget", "includes chained AfterimageStrike if present", damageReduction);
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "MaxDamageTargets", string.Empty, m_maxDamageTargets);
		AbilityMod.AddToken_EffectInfo(tokens, m_dashEnemyHitEffect, "DashEnemyHitEffect", m_dashEnemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_dashSelfHitEffect, "DashSelfHitEffect", m_dashSelfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_markEffectInfo, "MarkEffectInfo", m_markEffectInfo);
		AddTokenInt(tokens, "KnockbackDamage", string.Empty, m_knockbackDamage);
		AddTokenInt(tokens, "KnockbackLessDamagePerTarget", string.Empty, m_knockbackLessDamagePerTarget);
		AddTokenInt(tokens, "KnockbackExtraDamageByDist", string.Empty, m_knockbackExtraDamageByDist);
		AddTokenInt(tokens, "KnockbackExtraDamageChangePerDist", string.Empty, m_knockbackExtraDamageChangePerDist);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		return targetSquare != null
		       && targetSquare.IsValidForGameplay()
		       && targetSquare != caster.GetCurrentBoardSquare()
		       && KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare, caster.GetCurrentBoardSquare(), false) != null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return CanMoveAfterEvade();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SamuraiSwordDash))
		{
			m_abilityMod = abilityMod as AbilityMod_SamuraiSwordDash;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// added in rogues
	public List<ActorData> GetLastGatheredHitActors()
	{
		return m_lastGatheredHitActors;
	}

	// added in rogues
	public override Vector3 GetChargeBestSquareTestVector(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		return -1f * base.GetChargeBestSquareTestVector(chargeSegments);
	}

	// added in rogues
	public override bool GetChargeThroughInvalidSquares()
	{
		return false;
	}

	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] GetChargePath(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		ServerEvadeUtils.ChargeSegment[] array = new ServerEvadeUtils.ChargeSegment[2];
		array[0] = new ServerEvadeUtils.ChargeSegment
		{
			m_pos = caster.GetSquareAtPhaseStart(),
			m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
			m_end = BoardSquarePathInfo.ChargeEndType.None
		};
		array[1] = new ServerEvadeUtils.ChargeSegment
		{
			m_pos = Board.Get().GetSquare(targets[0].GridPos),
			m_end = BoardSquarePathInfo.ChargeEndType.Impact
		};
		float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(array));
		array[0].m_segmentMovementSpeed = segmentMovementSpeed;
		array[1].m_segmentMovementSpeed = segmentMovementSpeed;
		return array;
	}
	
	// added in rogues
	public override BoardSquare GetModifiedMoveStartSquare(ActorData caster, List<AbilityTarget> targets)
	{
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		return square != null
			? square
			: base.GetModifiedMoveStartSquare(caster, targets);
	}

	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);
		m_syncComponent.m_afterimagePosition = caster.GetSquareAtPhaseStart();
		m_syncComponent.Networkm_afterimageX = m_syncComponent.m_afterimagePosition.x;
		m_syncComponent.Networkm_afterimageY = m_syncComponent.m_afterimagePosition.y;
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		Vector3 targetPos = caster.GetLoSCheckPos(Board.Get().GetSquare(targets[0].GridPos));
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				targetPos,
				Quaternion.LookRotation(targets[0].AimDirection),
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource),
			new ServerClientUtils.SequenceStartData(
				SequenceLookup.Get().GetSimpleHitSequencePrefab(),
				caster.GetSquareAtPhaseStart().ToVector3(),
				Quaternion.LookRotation(targets[0].AimDirection),
				null,
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = FindHitActors(
			targets,
			caster,
			out Dictionary<ActorData, Vector3> actorToDamageOrigin,
			nonActorTargetInfo,
			out List<ActorData> actorsInRangeInEvasion);
		if (abilityResults.IsReal)
		{
			m_lastGatheredHitActors = actorsInRangeInEvasion;
		}
		int damage = GetDashDamage() - (hitActors.Count - 1) * GetDashLessDamagePerTarget();
		damage = Mathf.Max(0, damage);
		foreach (ActorData actorData in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, actorToDamageOrigin[actorData]));
			actorHitResults.SetBaseDamage(damage);
			if (GetMarkEffectInfo().m_applyEffect)
			{
				actorHitResults.AddEffect(new SamuraiMarkEffect(
					AsEffectSource(),
					actorData.GetCurrentBoardSquare(),
					actorData,
					caster,
					GetMarkEffectInfo().m_effectData));
			}
			actorHitResults.AddStandardEffectInfo(GetDashEnemyHitEffect());
			abilityResults.StoreActorHit(actorHitResults);
		}
		foreach (ActorData actorData in actorsInRangeInEvasion)
		{
			if (!hitActors.Contains(actorData))
			{
				ActorHitResults actorHitResults = MakeActorHitRes(actorData, caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart()));
				actorHitResults.SetIgnoreTechpointInteractionForHit(true);
				abilityResults.StoreActorHit(actorHitResults);
			}
		}
		bool flag = false;
		if (hitActors.Count == 0
		    && GetEnergyRefundIfTargetDashedAway() > 0
		    && GameFlowData.Get().IsInResolveState())
		{
			Vector3 casterStartPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
			Vector3 targetPos = caster.GetLoSCheckPos(Board.Get().GetSquare(targets[0].GridPos));
			foreach (ActorData actorData in GameFlowData.Get().GetActors())
			{
				if (actorData != null
				    && actorData.GetTeam() != caster.GetTeam()
				    && AreaEffectUtils.IsActorTargetable(actorData)
				    && ServerActionBuffer.Get().ActorIsEvading(actorData))
				{
					BoardSquare squareAtPhaseStart = actorData.GetSquareAtPhaseStart();
					if (squareAtPhaseStart != null
					    && (AreaEffectUtils.IsSquareInBoxByActorRadius(squareAtPhaseStart, casterStartPos, targetPos, 2f * GetDamageRadius())
					        || AreaEffectUtils.IsSquareInConeByActorRadius(squareAtPhaseStart, casterStartPos, 0f, 360f, GetDamageRadiusAtStart(), 0f, PenetrateLineOfSight(), caster)
					        || AreaEffectUtils.IsSquareInConeByActorRadius(squareAtPhaseStart, targetPos, 0f, 360f, GetDamageRadiusAtEnd(), 0f, PenetrateLineOfSight(), caster)))
					{
						flag = true;
						break;
					}
				}
			}
		}
		if (GetDashSelfHitEffect().m_applyEffect || flag)
		{
			ActorHitResults actorHitResults3 = MakeActorHitRes(caster, caster.GetFreePos());
			if (GetDashSelfHitEffect().m_applyEffect)
			{
				actorHitResults3.AddStandardEffectInfo(GetDashSelfHitEffect());
			}
			if (flag)
			{
				if (m_energyRefundIgnoreBuff)
				{
					actorHitResults3.AddDirectTechPointGainOnCaster(GetEnergyRefundIfTargetDashedAway());
				}
				else
				{
					actorHitResults3.AddTechPointGainOnCaster(GetEnergyRefundIfTargetDashedAway());
				}
			}
			abilityResults.StoreActorHit(actorHitResults3);
		}
		PositionHitResults positionHitResults = MakePosHitRes(caster.GetSquareAtPhaseStart().ToVector3());
		
		// custom
		GroundEffectField groundEffectField = new GroundEffectField
		{
			persistentSequencePrefab = m_afterimageSequencePrefab,
			duration = 1,
			shape = AbilityAreaShape.SingleSquare,
			effectOnAllies = new StandardEffectInfo(),
			effectOnEnemies = new StandardEffectInfo()
		};
		// rogues
		// GroundEffectField groundEffectField = ScriptableObject.CreateInstance<GroundEffectField>();
		// groundEffectField.persistentSequencePrefab = m_afterimageSequencePrefab;
		// groundEffectField.duration = 1;
		// groundEffectField.shape = AbilityAreaShape.SingleSquare;
		// groundEffectField.effectOnAllies = new StandardEffectInfo();
		// groundEffectField.effectOnEnemies = new StandardEffectInfo();
		
		StandardGroundEffect standardGroundEffect = new StandardGroundEffect(
			AsEffectSource(),
			caster.GetSquareAtPhaseStart(),
			caster.GetSquareAtPhaseStart().ToVector3(),
			null,
			caster,
			groundEffectField);
		Vector3 vector = caster.GetCurrentBoardSquare().ToVector3() - caster.GetSquareAtPhaseStart().ToVector3();
		standardGroundEffect.SetSequenceOrientation(Quaternion.LookRotation(vector));
		positionHitResults.AddEffect(standardGroundEffect);
		abilityResults.StorePositionHit(positionHitResults);
		if (GameFlowData.Get().IsInResolveState() && !ServerActionBuffer.Get().GatheringFakeResults)
		{
			m_syncComponent.m_afterimageVfxEffect = standardGroundEffect;
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> FindHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		out Dictionary<ActorData, Vector3> actorToDamageOrigin,
		List<NonActorTargetInfo> nonActorTargetInfo,
		out List<ActorData> actorsInRangeInEvasion)
	{
		BoardSquare squareAtPhaseStart = caster.GetSquareAtPhaseStart();
		List<ActorData> list = new List<ActorData>();
		actorToDamageOrigin = new Dictionary<ActorData, Vector3>();
		Vector3 startPos = caster.GetLoSCheckPos(squareAtPhaseStart);
		Vector3 endPos = caster.GetLoSCheckPos(Board.Get().GetSquare(targets[0].GridPos));
		List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(
			startPos,
			endPos,
			GetDamageRadiusAtStart(),
			GetDamageRadiusAtEnd(),
			GetDamageRadius(),
			PenetrateLineOfSight(),
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInRadiusOfLine);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInRadiusOfLine, caster.GetFreePos(caster.GetSquareAtPhaseStart()));
		actorsInRangeInEvasion = new List<ActorData>(actorsInRadiusOfLine);
		TargeterUtils.LimitActorsToMaxNumber(ref actorsInRangeInEvasion, GetMaxTargets());
		TargeterUtils.LimitActorsToMaxNumber(ref actorsInRadiusOfLine, GetMaxDamageTargets());
		foreach (ActorData actorData in actorsInRadiusOfLine)
		{
			if (!list.Contains(actorData))
			{
				list.Add(actorData);
				actorToDamageOrigin[actorData] = startPos;
			}
		}
		return list;
	}

	// added in rogues
	public override void OnDodgedDamage(ActorData caster, int damageDodged)
	{
		caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SamuraiStats.DamageDealtAndDodged_Ult, damageDodged);
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SamuraiStats.DamageDealtAndDodged_Ult, results.FinalDamage);
		}
	}
#endif
}
