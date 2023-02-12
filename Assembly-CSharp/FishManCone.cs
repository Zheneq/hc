// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class FishManCone : Ability
{
	public enum ConeTargetingMode
	{
		Static,
		MultiClick,
		Stretch
	}

	[Header("-- Cone Data")]
	public ConeTargetingMode m_coneMode = ConeTargetingMode.MultiClick;
	public float m_coneWidthAngle = 60f;
	public float m_coneWidthAngleMin = 5f;
	public float m_coneLength = 5f;
	public float m_coneBackwardOffset;
	public bool m_penetrateLineOfSight;
	public int m_maxTargets;
	public float m_multiClickConeEdgeWidth = 0.2f;
	[Header("-- (for stretch cone only)")]
	public bool m_useDiscreteAngleChange;
	public float m_stretchInterpMinDist = 2.5f;
	public float m_stretchInterpRange = 4f;
	[Header("-- On Hit Target")]
	public int m_damageToEnemies;
	public int m_damageToEnemiesMax;
	public StandardEffectInfo m_effectToEnemies;
	[Header("-- Ally Healing")]
	public int m_healingToAllies = 15;
	public int m_healingToAlliesMax = 30;
	public StandardEffectInfo m_effectToAllies;
	[Header("-- Self-Healing")]
	public int m_healToCasterOnCast;
	public int m_healToCasterPerEnemyHit;
	public int m_healToCasterPerAllyHit;
	[Header("-- Bonus Healing on Heal Cone ability")]
	public int m_extraHealPerEnemyHitForNextHealCone;
	[Header("-- Extra Energy")]
	public int m_extraEnergyForSingleEnemyHit;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_FishManCone m_abilityMod;
	private FishMan_SyncComponent m_syncComp;
	private AreaEffectUtils.StretchConeStyle m_stretchConeStyle;
	private StandardEffectInfo m_cachedEffectToEnemies;
	private StandardEffectInfo m_cachedEffectToAllies;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<FishMan_SyncComponent>();
		}
		switch (m_coneMode)
		{
			case ConeTargetingMode.MultiClick:
			{
				ClearTargeters();
				for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
				{
					AbilityUtil_Targeter_SweepMultiClickCone targeter = new AbilityUtil_Targeter_SweepMultiClickCone(
						this,
						GetConeWidthAngleMin(),
						GetConeWidthAngle(),
						GetConeLength(),
						GetConeBackwardOffset(),
						m_multiClickConeEdgeWidth,
						PenetrateLineOfSight(),
						GetMaxTargets());
					targeter.SetAffectedGroups(AffectsEnemies(), AffectsAllies(), AffectsCaster());
					Targeters.Add(targeter);
				}
				break;
			}
			case ConeTargetingMode.Stretch:
				Targeter = new AbilityUtil_Targeter_StretchCone(
					this,
					GetConeLength(),
					GetConeLength(),
					GetConeWidthAngleMin(),
					GetConeWidthAngle(),
					m_stretchConeStyle,
					GetConeBackwardOffset(),
					PenetrateLineOfSight())
				{
					// reactor
					m_includeEnemies = AffectsEnemies(),
					m_includeAllies = AffectsAllies(),
					m_includeCaster = AffectsCaster(),
					// rogues (down the line)
					// Targeter.SetAffectedGroups(AffectsEnemies(), AffectsAllies(), AffectsCaster());

					m_interpMinDistOverride = m_stretchInterpMinDist,
					m_interpRangeOverride = m_stretchInterpRange,
					m_discreteWidthAngleChange = m_useDiscreteAngleChange,
					m_numDiscreteWidthChanges = GetMaxDamageToEnemies() - GetDamageToEnemies()
				};
				break;
			default:
				Targeter = new AbilityUtil_Targeter_DirectionCone(
					this,
					GetConeWidthAngle(),
					GetConeLength(),
					GetConeBackwardOffset(),
					PenetrateLineOfSight(),
					true,
					AffectsEnemies(),
					AffectsAllies(),
					AffectsCaster(),
					GetMaxTargets());
				break;
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_coneMode == ConeTargetingMode.MultiClick ? 2 : 1;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeLength();
	}

	public override bool HasRestrictedFreePosDistance(
		ActorData aimingActor,
		int targetIndex,
		List<AbilityTarget> targetsSoFar,
		out float min,
		out float max)
	{
		if (m_coneMode == ConeTargetingMode.Stretch)
		{
			min = m_stretchInterpMinDist * Board.Get().squareSize;
			max = (m_stretchInterpMinDist + m_stretchInterpRange) * Board.Get().squareSize;
			return true;
		}
		return base.HasRestrictedFreeAimDegrees(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	private bool AffectsEnemies()
	{
		return GetDamageToEnemies() > 0 || GetEffectToEnemies().m_applyEffect;
	}

	private bool AffectsAllies()
	{
		return GetHealingToAllies() > 0 || GetEffectToAllies().m_applyEffect;
	}

	private bool AffectsCaster()
	{
		return GetHealToCasterOnCast() > 0
		       || GetHealToCasterPerAllyHit() > 0
		       || GetHealToCasterPerEnemyHit() > 0;
	}

	private void SetCachedFields()
	{
		m_cachedEffectToEnemies = m_abilityMod != null
			? m_abilityMod.m_effectToEnemiesMod.GetModifiedValue(m_effectToEnemies)
			: m_effectToEnemies;
		m_cachedEffectToAllies = m_abilityMod != null
			? m_abilityMod.m_effectToAlliesMod.GetModifiedValue(m_effectToAllies)
			: m_effectToAllies;
	}

	public float GetConeWidthAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public float GetConeWidthAngleMin()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthAngleMinMod.GetModifiedValue(m_coneWidthAngleMin)
			: m_coneWidthAngleMin;
	}

	public float GetConeLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength)
			: m_coneLength;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset)
			: m_coneBackwardOffset;
	}

	public bool PenetrateLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight)
			: m_penetrateLineOfSight;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public int GetDamageToEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageToEnemiesMod.GetModifiedValue(m_damageToEnemies)
			: m_damageToEnemies;
	}

	public int GetMaxDamageToEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageToEnemiesMaxMod.GetModifiedValue(m_damageToEnemiesMax)
			: m_damageToEnemiesMax;
	}

	public StandardEffectInfo GetEffectToEnemies()
	{
		return m_cachedEffectToEnemies ?? m_effectToEnemies;
	}

	public int GetHealingToAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healingToAlliesMod.GetModifiedValue(m_healingToAllies)
			: m_healingToAllies;
	}

	public int GetMaxHealingToAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healingToAlliesMaxMod.GetModifiedValue(m_healingToAlliesMax)
			: m_healingToAlliesMax;
	}

	public StandardEffectInfo GetEffectToAllies()
	{
		return m_cachedEffectToAllies ?? m_effectToAllies;
	}

	public int GetHealToCasterOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healToCasterOnCastMod.GetModifiedValue(m_healToCasterOnCast)
			: m_healToCasterOnCast;
	}

	public int GetHealToCasterPerEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healToCasterPerEnemyHitMod.GetModifiedValue(m_healToCasterPerEnemyHit)
			: m_healToCasterPerEnemyHit;
	}

	public int GetHealToCasterPerAllyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healToCasterPerAllyHitMod.GetModifiedValue(m_healToCasterPerAllyHit)
			: m_healToCasterPerAllyHit;
	}

	public int GetExtraHealPerEnemyHitForNextHealCone()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealPerEnemyHitForNextHealConeMod.GetModifiedValue(m_extraHealPerEnemyHitForNextHealCone)
			: m_extraHealPerEnemyHitForNextHealCone;
	}

	public int GetExtraEnergyForSingleEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraEnergyForSingleEnemyHitMod.GetModifiedValue(m_extraEnergyForSingleEnemyHit)
			: m_extraEnergyForSingleEnemyHit;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_FishManCone))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		
		m_abilityMod = abilityMod as AbilityMod_FishManCone;
		Setup();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManCone abilityMod_FishManCone = modAsBase as AbilityMod_FishManCone;
		AddTokenInt(tokens, "DamageToEnemies", string.Empty, abilityMod_FishManCone != null
			? abilityMod_FishManCone.m_damageToEnemiesMod.GetModifiedValue(m_damageToEnemies)
			: m_damageToEnemies);
		AddTokenInt(tokens, "DamageToEnemiesMax", string.Empty, abilityMod_FishManCone != null
			? abilityMod_FishManCone.m_damageToEnemiesMaxMod.GetModifiedValue(m_damageToEnemiesMax)
			: m_damageToEnemiesMax);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManCone != null
			? abilityMod_FishManCone.m_effectToEnemiesMod.GetModifiedValue(m_effectToEnemies)
			: m_effectToEnemies, "EffectToEnemies", m_effectToEnemies);
		AddTokenInt(tokens, "HealingToAllies", string.Empty, abilityMod_FishManCone != null
			? abilityMod_FishManCone.m_healingToAlliesMod.GetModifiedValue(m_healingToAllies)
			: m_healingToAllies);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManCone != null
			? abilityMod_FishManCone.m_effectToAlliesMod.GetModifiedValue(m_effectToAllies)
			: m_effectToAllies, "EffectToAllies", m_effectToAllies);
		AddTokenInt(tokens, "MaxTargets", string.Empty, abilityMod_FishManCone != null
			? abilityMod_FishManCone.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets);
		AddTokenInt(tokens, "HealToCasterOnCast", string.Empty, abilityMod_FishManCone != null
			? abilityMod_FishManCone.m_healToCasterOnCastMod.GetModifiedValue(m_healToCasterOnCast)
			: m_healToCasterOnCast);
		AddTokenInt(tokens, "HealToCasterPerEnemyHit", string.Empty, abilityMod_FishManCone != null
			? abilityMod_FishManCone.m_healToCasterPerEnemyHitMod.GetModifiedValue(m_healToCasterPerEnemyHit)
			: m_healToCasterPerEnemyHit);
		AddTokenInt(tokens, "HealToCasterPerAllyHit", string.Empty, abilityMod_FishManCone != null
			? abilityMod_FishManCone.m_healToCasterPerAllyHitMod.GetModifiedValue(m_healToCasterPerAllyHit)
			: m_healToCasterPerAllyHit);
		AddTokenInt(tokens, "ExtraHealPerEnemyHitForNextHealCone", string.Empty, abilityMod_FishManCone != null
			? abilityMod_FishManCone.m_extraHealPerEnemyHitForNextHealConeMod.GetModifiedValue(m_extraHealPerEnemyHitForNextHealCone)
			: m_extraHealPerEnemyHitForNextHealCone);
		AddTokenInt(tokens, "ExtraEnergyForSingleEnemyHit", string.Empty, abilityMod_FishManCone != null
			? abilityMod_FishManCone.m_extraEnergyForSingleEnemyHitMod.GetModifiedValue(m_extraEnergyForSingleEnemyHit)
			: m_extraEnergyForSingleEnemyHit);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, GetDamageToEnemies()),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, GetHealingToAllies())
		};
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageToEnemies());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetHealingToAllies());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetHealToCasterOnCast() + GetHealToCasterPerEnemyHit() + GetHealToCasterPerAllyHit());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (currentTargeterIndex <= 0 && m_coneMode == ConeTargetingMode.MultiClick)
		{
			return null;
		}
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		
		AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = Targeter as AbilityUtil_Targeter_StretchCone;
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			int enemyNum = Targeters[currentTargeterIndex].GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			int allyNum = Targeters[currentTargeterIndex].GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			int healing = GetHealToCasterOnCast() + GetHealToCasterPerEnemyHit() * enemyNum + GetHealToCasterPerAllyHit() * allyNum;
			dictionary[AbilityTooltipSymbol.Healing] = Mathf.RoundToInt(healing);
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
		{
			int damage = GetDamageToEnemies();
			if (m_coneMode == ConeTargetingMode.MultiClick)
			{
				AbilityUtil_Targeter_SweepMultiClickCone targeter = Targeters[currentTargeterIndex] as AbilityUtil_Targeter_SweepMultiClickCone;
				damage = GetDamageForSweepAngle(targeter.sweepAngle);
			}
			else if (m_coneMode == ConeTargetingMode.Stretch && abilityUtil_Targeter_StretchCone != null)
			{
				damage = GetDamageForSweepAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
			}
			dictionary[AbilityTooltipSymbol.Damage] = damage;
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
		{
			int healing = GetHealingToAllies();
			if (m_coneMode == ConeTargetingMode.MultiClick)
			{
				AbilityUtil_Targeter_SweepMultiClickCone targeter = Targeters[currentTargeterIndex] as AbilityUtil_Targeter_SweepMultiClickCone;
				healing = GetHealingForSweepAngle(targeter.sweepAngle);
			}
			else if (m_coneMode == ConeTargetingMode.Stretch && abilityUtil_Targeter_StretchCone != null)
			{
				healing = GetHealingForSweepAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
			}
			dictionary[AbilityTooltipSymbol.Healing] = healing;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetExtraEnergyForSingleEnemyHit() <= 0 ||
		    (currentTargeterIndex <= 0 && m_coneMode == ConeTargetingMode.MultiClick))
		{
			return 0;
		}
		AbilityUtil_Targeter targeter = Targeters[currentTargeterIndex];
		if (targeter != null)
		{
			int enemyNum = targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			if (enemyNum == 1)
			{
				return GetExtraEnergyForSingleEnemyHit();
			}
		}
		return 0;
	}

	private Vector3 GetTargeterClampedAimDirection(Vector3 startAimDirection, Vector3 endAimDirection, out float sweepAngle, out float coneCenterDegrees)
	{
		sweepAngle = Vector3.Angle(startAimDirection, endAimDirection);
		float coneWidthAngle = GetConeWidthAngle();
		float coneWidthAngleMin = GetConeWidthAngleMin();
		if (coneWidthAngle > 0f && sweepAngle > coneWidthAngle)
		{
			endAimDirection = Vector3.RotateTowards(
				endAimDirection, 
				startAimDirection,
				(float)Math.PI / 180f * (sweepAngle - coneWidthAngle),
				0f);
			sweepAngle = coneWidthAngle;
		}
		else if (coneWidthAngleMin > 0f && sweepAngle < coneWidthAngleMin)
		{
			endAimDirection = Vector3.RotateTowards(
				endAimDirection, 
				startAimDirection, 
				(float)Math.PI / 180f * (sweepAngle - coneWidthAngleMin),
				0f);
			sweepAngle = coneWidthAngleMin;
		}

		coneCenterDegrees = VectorUtils.HorizontalAngle_Deg(startAimDirection);
		Vector3 vector = Vector3.Cross(startAimDirection, endAimDirection);
		if (vector.y > 0f)
		{
			coneCenterDegrees -= sweepAngle * 0.5f;
		}
		else
		{
			coneCenterDegrees += sweepAngle * 0.5f;
		}
		return endAimDirection;
	}

	public override Vector3 GetRotateToTargetPos(List<AbilityTarget> targets, ActorData caster)
	{
		if (m_coneMode == ConeTargetingMode.MultiClick)
		{
			float sweepAngle = GetConeWidthAngleMin();
			float coneCenterDegrees = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
			if (targets.Count > 1)
			{
				GetTargeterClampedAimDirection(
					targets[0].AimDirection,
					targets[targets.Count - 1].AimDirection,
					out sweepAngle,
					out coneCenterDegrees);
			}
			return caster.GetFreePos() + VectorUtils.AngleDegreesToVector(coneCenterDegrees);
		}
		return base.GetRotateToTargetPos(targets, caster);
	}

#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_syncComp == null)
		{
			return;
		}
		sbyte num = 0;
		List<ActorData> hitActors = additionalData.m_abilityResults.HitActorList();
		foreach (ActorData hitActor in hitActors)
		{
			if (hitActor.GetTeam() != caster.GetTeam())
			{
				num += 1;
			}
		}
		if (m_syncComp.m_lastBasicAttackEnemyHitCount != num)
		{
			m_syncComp.Networkm_lastBasicAttackEnemyHitCount = num;
		}
	}

	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		Sequence.IExtraSequenceParams[] array = null;
		if (m_coneMode == ConeTargetingMode.MultiClick)
		{
			BlasterStretchConeSequence.ExtraParams extraParams = new BlasterStretchConeSequence.ExtraParams();
			GetTargeterClampedAimDirection(
				targets[0].AimDirection,
				targets[1].AimDirection,
				out extraParams.angleInDegrees,
				out extraParams.forwardAngle);
			extraParams.lengthInSquares = GetConeLength();
			array = new Sequence.IExtraSequenceParams[]
			{
				extraParams
			};
		}
		else if (m_coneMode == ConeTargetingMode.Stretch)
		{
			Vector3 loSCheckPos = caster.GetLoSCheckPos();
			AreaEffectUtils.GatherStretchConeDimensions(
				targets[0].FreePos,
				loSCheckPos,
				GetConeLength(),
				GetConeLength(),
				GetConeWidthAngleMin(),
				GetConeWidthAngle(),
				m_stretchConeStyle,
				out _,
				out var angleInDegrees,
				m_useDiscreteAngleChange,
				GetMaxDamageToEnemies() - GetDamageToEnemies(),
				m_stretchInterpMinDist,
				m_stretchInterpRange);
			array = new Sequence.IExtraSequenceParams[]
			{
				new BlasterStretchConeSequence.ExtraParams
				{
					angleInDegrees = angleInDegrees,
					forwardAngle = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection),
					lengthInSquares = GetConeLength()
				}
			};
		}

		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			caster.GetCurrentBoardSquare(),
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource,
			array);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = FindHitActors(targets, caster, nonActorTargetInfo, out int baseDamage, out int baseHealing);
		Vector3 casterPos = caster.GetLoSCheckPos();
		int healToCaster = GetHealToCasterOnCast();
		int hitEnemiesNum = 0;
		foreach (ActorData hitActor in hitActors)
		{
			if (hitActor.GetTeam() != caster.GetTeam())
			{
				hitEnemiesNum++;
			}
		}
		foreach (ActorData actorData in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, casterPos));
			if (actorData != caster)
			{
				if (actorData.GetTeam() != caster.GetTeam())
				{
					actorHitResults.SetBaseDamage(baseDamage);
					actorHitResults.AddStandardEffectInfo(GetEffectToEnemies());
					healToCaster += GetHealToCasterPerEnemyHit();
					if (hitEnemiesNum == 1 && GetExtraEnergyForSingleEnemyHit() > 0)
					{
						actorHitResults.SetTechPointGainOnCaster(GetExtraEnergyForSingleEnemyHit());
					}
				}
				else
				{
					actorHitResults.SetBaseHealing(baseHealing);
					actorHitResults.AddStandardEffectInfo(GetEffectToAllies());
					healToCaster += GetHealToCasterPerAllyHit();
				}
				abilityResults.StoreActorHit(actorHitResults);
			}
		}
		if (healToCaster > 0)
		{
			ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			casterHitResults.SetBaseHealing(healToCaster);
			abilityResults.StoreActorHit(casterHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> FindHitActors(
		List<AbilityTarget> targets,
		ActorData caster,
		List<NonActorTargetInfo> nonActorTargetInfo,
		out int damageAmount,
		out int healAmount)
	{
		Vector3 aimDirection = targets[0].AimDirection;
		Vector3 casterPos = caster.GetLoSCheckPos();
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDirection);
		List<Team> affectedTeams = new List<Team>();
		if (AffectsAllies())
		{
			affectedTeams.Add(caster.GetTeam());
		}
		if (AffectsEnemies())
		{
			affectedTeams.AddRange(caster.GetOtherTeams());
		}
		List<ActorData> actorsInCone;
		switch (m_coneMode)
		{
			case ConeTargetingMode.MultiClick:
			{
				float coneWidthAngleMin = GetConeWidthAngleMin();
				float coneCenterAngleDegrees2 = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
				if (targets.Count > 1)
				{
					GetTargeterClampedAimDirection(
						targets[0].AimDirection,
						targets[targets.Count - 1].AimDirection,
						out coneWidthAngleMin,
						out coneCenterAngleDegrees2);
				}
				Vector3 aimDirection2 = targets[targets.Count - 1].AimDirection;
				List<NonActorTargetInfo> nonActorTargets = new List<NonActorTargetInfo>();
				List<NonActorTargetInfo> nonActorTargets2 = new List<NonActorTargetInfo>();
				List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
					caster.GetLoSCheckPos(),
					aimDirection,
					GetConeLength(),
					m_multiClickConeEdgeWidth,
					caster,
					affectedTeams,
					PenetrateLineOfSight(),
					0,
					PenetrateLineOfSight(),
					true,
					out _,
					nonActorTargets);
				List<ActorData> actorsInLaser2 = AreaEffectUtils.GetActorsInLaser(
					caster.GetLoSCheckPos(),
					aimDirection2,
					GetConeLength(),
					m_multiClickConeEdgeWidth,
					caster,
					affectedTeams,
					PenetrateLineOfSight(),
					0,
					PenetrateLineOfSight(),
					true,
					out _,
					nonActorTargets2);
				actorsInCone = AreaEffectUtils.GetActorsInCone(
					caster.GetFreePos(),
					coneCenterAngleDegrees2,
					coneWidthAngleMin,
					GetConeLength(),
					GetConeBackwardOffset(),
					PenetrateLineOfSight(),
					caster,
					affectedTeams,
					nonActorTargetInfo);
				foreach (ActorData actor in actorsInLaser)
				{
					if (!actorsInCone.Contains(actor))
					{
						actorsInCone.Add(actor);
					}
				}
				foreach (ActorData actor in actorsInLaser2)
				{
					if (!actorsInCone.Contains(actor))
					{
						actorsInCone.Add(actor);
					}
				}
				damageAmount = GetDamageForSweepAngle(coneWidthAngleMin);
				healAmount = GetHealingForSweepAngle(coneWidthAngleMin);
				break;
			}
			case ConeTargetingMode.Stretch:
				AreaEffectUtils.GatherStretchConeDimensions(
					targets[0].FreePos,
					casterPos,
					GetConeLength(),
					GetConeLength(),
					GetConeWidthAngleMin(),
					GetConeWidthAngle(),
					m_stretchConeStyle,
					out _,
					out var num2,
					m_useDiscreteAngleChange,
					GetMaxDamageToEnemies() - GetDamageToEnemies(),
					m_stretchInterpMinDist,
					m_stretchInterpRange);
				actorsInCone = AreaEffectUtils.GetActorsInCone(
					casterPos,
					coneCenterAngleDegrees,
					num2,
					GetConeLength(),
					GetConeBackwardOffset(),
					PenetrateLineOfSight(),
					caster,
					affectedTeams,
					nonActorTargetInfo);
				damageAmount = GetDamageForSweepAngle(num2);
				healAmount = GetHealingForSweepAngle(num2);
				break;
			default:
				actorsInCone = AreaEffectUtils.GetActorsInCone(
					casterPos,
					coneCenterAngleDegrees,
					GetConeWidthAngle(),
					GetConeLength(),
					GetConeBackwardOffset(),
					PenetrateLineOfSight(),
					caster,
					affectedTeams,
					nonActorTargetInfo);
				damageAmount = GetDamageToEnemies();
				healAmount = GetHealingToAllies();
				break;
		}
		if (m_maxTargets > 0)
		{
			TargeterUtils.SortActorsByDistanceToPos(ref actorsInCone, casterPos);
			TargeterUtils.LimitActorsToMaxNumber(ref actorsInCone, m_maxTargets);
		}
		return actorsInCone;
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		float angle = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
		if (targets.Count > 1)
		{
			GetTargeterClampedAimDirection(
				targets[0].AimDirection,
				targets[targets.Count - 1].AimDirection,
				out _, 
				out angle);
		}

		return new List<Vector3>
		{
			caster.GetFreePos(),
			caster.GetFreePos() + GetConeLength() * Board.Get().squareSize * VectorUtils.AngleDegreesToVector(angle)
		};
	}
#endif
	
	private int GetDamageForSweepAngle(float sweepAngle)
	{
		float damageRange = GetMaxDamageToEnemies() - GetDamageToEnemies();
		float angleRange = GetConeWidthAngle() - GetConeWidthAngleMin();
		float share = angleRange > 0f
			? 1f - (sweepAngle - GetConeWidthAngleMin()) / angleRange
			: 1f;
		share = Mathf.Clamp(share, 0f, 1f);
		return GetDamageToEnemies() + Mathf.RoundToInt(damageRange * share);
	}

	private int GetHealingForSweepAngle(float sweepAngle)
	{
		float healingRange = GetMaxHealingToAllies() - GetHealingToAllies();
		float angleRange = GetConeWidthAngle() - GetConeWidthAngleMin();
		float share = angleRange > 0f
			? 1f - (sweepAngle - GetConeWidthAngleMin()) / angleRange
			: 1f;
		share = Mathf.Clamp(share, 0f, 1f);
		return GetHealingToAllies() + Mathf.RoundToInt(healingRange * share);
	}
}
