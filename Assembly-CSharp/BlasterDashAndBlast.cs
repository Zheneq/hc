﻿using System.Collections.Generic;
using UnityEngine;

public class BlasterDashAndBlast : Ability
{
	public bool m_useConeParamsFromPrimary = true;
	[Header("-- (if not taking values from primary cone ability) Cone Limits")]
	public float m_minLength;
	public float m_maxLength;
	public float m_minAngle;
	public float m_maxAngle;
	public AreaEffectUtils.StretchConeStyle m_stretchStyle = AreaEffectUtils.StretchConeStyle.DistanceSquared;
	public float m_coneBackwardOffset;
	public bool m_penetrateLineOfSight;
	[Header("-- Stock based Evade distance")]
	public bool m_useStockBasedEvadeDistance;
	public float m_distancePerStock = 1.01f;
	[Header("-- Whether to use square coordinate distance to limit stock-based evade distance")]
	public bool m_stockBasedDistUseSquareCoordDist = true;
	[Header("-- If <= 0, dist only limited by stock remaining")]
	public int m_stockBasedDistMaxSquareCoordDist;
	[Header("-- On Hit")]
	public bool m_useHitParamsFromPrimary = true;
	public int m_damageAmountNormal;
	public int m_extraDamageForSingleHit;
	public bool m_removeOverchargeEffectOnCast;
	[Space(10f)]
	public StandardEffectInfo m_enemyEffectNormal;
	public StandardEffectInfo m_enemyEffectOvercharged;
	[Space(10f)]
	public StandardEffectInfo m_selfEffectOnCast;
	[Header("-- Sequences")]
	public GameObject m_dashSequencePrefab;
	public GameObject m_coneSequencePrefab;
	public GameObject m_overchargedConeSequencePrefab;
	private AbilityMod_BlasterDashAndBlast m_abilityMod;
	private BlasterOvercharge m_overchargeAbility;
	private BlasterStretchingCone m_primaryAbility;
	private Blaster_SyncComponent m_syncComp;
	private AbilityData.ActionType m_myActionType = AbilityData.ActionType.INVALID_ACTION;
	private StandardEffectInfo m_cachedEnemyEffectNormal;
	private StandardEffectInfo m_cachedEnemyEffectOvercharged;
	private StandardEffectInfo m_cachedSelfEffectOnCast;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Dash and Blast";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		AbilityData abilityData = GetComponent<AbilityData>();
		if (abilityData != null)
		{
			m_overchargeAbility = abilityData.GetAbilityOfType(typeof(BlasterOvercharge)) as BlasterOvercharge;
			m_primaryAbility = abilityData.GetAbilityOfType(typeof(BlasterStretchingCone)) as BlasterStretchingCone;
			m_myActionType = abilityData.GetActionTypeOfAbility(this);
		}
		int dashTargeterNum = Mathf.Max(GetNumTargets() - 1, 1);
		int blastTargeterNum = Mathf.Max(GetNumTargets() - dashTargeterNum, 0);
		ClearTargeters();
		for (int i = 0; i < dashTargeterNum; i++)
		{
			AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false);
			abilityUtil_Targeter_Charge.SetUseMultiTargetUpdate(true);
			StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
			if (moddedEffectForSelf != null && moddedEffectForSelf.m_applyEffect)
			{
				abilityUtil_Targeter_Charge.m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
			}
			Targeters.Add(abilityUtil_Targeter_Charge);
		}
		for (int i = 0; i < blastTargeterNum; i++)
		{
			AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = new AbilityUtil_Targeter_StretchCone(this, GetMinLength(), GetMaxLength(), GetMinAngle(), GetMaxAngle(), m_stretchStyle, GetConeBackwardOffset(), PenetrateLineOfSight());
			abilityUtil_Targeter_StretchCone.SetUseMultiTargetUpdate(true);
			Targeters.Add(abilityUtil_Targeter_StretchCone);
		}
	}

	public void OnPrimaryAttackModChange()
	{
		Setup();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetRangeInSquares(0) - 0.5f + GetMaxLength();
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			min = GetMinLength() * Board.Get().squareSize;
			max = GetMaxLength() * Board.Get().squareSize;
			return true;
		}
		return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	public override bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		if (targetIndex == 1)
		{
			BoardSquare boardSquareSafe = Board.Get().GetSquare(targetsSoFar[0].GridPos);
			overridePos = boardSquareSafe.GetOccupantRefPos();
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	public bool UseConeParamFromPrimary()
	{
		return m_useConeParamsFromPrimary && m_primaryAbility != null;
	}

	public bool UseHitPropertyFromPrimary()
	{
		return m_useHitParamsFromPrimary && m_primaryAbility != null;
	}

	private void SetCachedFields()
	{
		m_cachedEnemyEffectNormal = m_abilityMod != null
			? m_abilityMod.m_enemyEffectNormalMod.GetModifiedValue(m_enemyEffectNormal)
			: m_enemyEffectNormal;
		m_cachedEnemyEffectOvercharged = m_abilityMod != null
			? m_abilityMod.m_enemyEffectOverchargedMod.GetModifiedValue(m_enemyEffectOvercharged)
			: m_enemyEffectOvercharged;
		m_cachedSelfEffectOnCast = m_abilityMod != null
			? m_abilityMod.m_selfEffectOnCastMod.GetModifiedValue(m_selfEffectOnCast)
			: m_selfEffectOnCast;
	}

	public float GetMinLength()
	{
		if (UseConeParamFromPrimary())
		{
			return m_primaryAbility.GetMinLength();
		}
		return m_abilityMod != null 
			? m_abilityMod.m_minLengthMod.GetModifiedValue(m_minLength) 
			: m_minLength;
	}

	public float GetMaxLength()
	{
		if (UseConeParamFromPrimary())
		{
			return m_primaryAbility.GetMaxLength();
		}
		return m_abilityMod != null 
			? m_abilityMod.m_maxLengthMod.GetModifiedValue(m_maxLength) 
			: m_maxLength;
	}

	public float GetMinAngle()
	{
		if (UseConeParamFromPrimary())
		{
			return m_primaryAbility.GetMinAngle();
		}
		return m_abilityMod != null 
			? m_abilityMod.m_minAngleMod.GetModifiedValue(m_minAngle) 
			: m_minAngle;
	}

	public float GetMaxAngle()
	{
		if (UseConeParamFromPrimary())
		{
			return m_primaryAbility.GetMaxAngle();
		}
		return m_abilityMod != null 
			? m_abilityMod.m_maxAngleMod.GetModifiedValue(m_maxAngle) 
			: m_maxAngle;
	}

	public float GetConeBackwardOffset()
	{
		if (UseConeParamFromPrimary())
		{
			return m_primaryAbility.GetConeBackwardOffset();
		}
		return m_abilityMod != null 
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset) 
			: m_coneBackwardOffset;
	}

	public bool PenetrateLineOfSight()
	{
		if (UseConeParamFromPrimary())
		{
			return m_primaryAbility.PenetrateLineOfSight();
		}
		return m_abilityMod != null 
			? m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight)
			: m_penetrateLineOfSight;
	}

	public bool UseStockBasedEvadeDistance()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_useStockBasedEvadeDistanceMod.GetModifiedValue(m_useStockBasedEvadeDistance) 
			: m_useStockBasedEvadeDistance;
	}

	public float GetDistancePerStock()
	{
		float distancePerStock = m_abilityMod != null
			? m_abilityMod.m_distancePerStockMod.GetModifiedValue(m_distancePerStock)
			: m_distancePerStock;
		return Mathf.Max(0.1f, distancePerStock);
	}

	public bool StockBasedDistUseSquareCoordDist()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_stockBasedDistUseSquareCoordDistMod.GetModifiedValue(m_stockBasedDistUseSquareCoordDist) 
			: m_stockBasedDistUseSquareCoordDist;
	}

	public int GetStockBasedDistMaxSquareCoordDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_stockBasedDistMaxSquareCoordDistMod.GetModifiedValue(m_stockBasedDistMaxSquareCoordDist)
			: m_stockBasedDistMaxSquareCoordDist;
	}

	public int GetDamageAmountNormal()
	{
		if (UseHitPropertyFromPrimary())
		{
			return m_primaryAbility.GetDamageAmountNormal();
		}
		return m_abilityMod != null 
			? m_abilityMod.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal) 
			: m_damageAmountNormal;
	}

	public int GetDamageAmountOvercharged()
	{
		return GetDamageAmountNormal() + m_overchargeAbility.GetExtraDamage() + GetMultiStackOverchargeDamage();
	}

	public int GetExtraDamageForSingleHit()
	{
		if (UseHitPropertyFromPrimary())
		{
			return m_primaryAbility.GetExtraDamageForSingleHit();
		}
		return m_abilityMod != null 
			? m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit) 
			: m_extraDamageForSingleHit;
	}

	public StandardEffectInfo GetEnemyEffectNormal()
	{
		if (UseHitPropertyFromPrimary())
		{
			return m_primaryAbility.GetNormalEnemyEffect();
		}
		return m_cachedEnemyEffectNormal ?? m_enemyEffectNormal;
	}

	public StandardEffectInfo GetEnemyEffectOvercharged()
	{
		if (UseHitPropertyFromPrimary())
		{
			return m_primaryAbility.GetOverchargedEnemyEffect();
		}
		return m_cachedEnemyEffectOvercharged ?? m_enemyEffectOvercharged;
	}

	public StandardEffectInfo GetSelfEffectOnCast()
	{
		return m_cachedSelfEffectOnCast ?? m_selfEffectOnCast;
	}

	public int GetCurrentModdedDamage()
	{
		if (AmOvercharged(ActorData))
		{
			return GetDamageAmountOvercharged() + GetMultiStackOverchargeDamage();
		}
		return GetDamageAmountNormal();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BlasterDashAndBlast))
		{
			m_abilityMod = abilityMod as AbilityMod_BlasterDashAndBlast;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	private bool AmOvercharged(ActorData caster)
	{
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Blaster_SyncComponent>();
		}
		return m_syncComp.m_overchargeBuffs > 0;
	}

	private int GetMultiStackOverchargeDamage()
	{
		if (m_syncComp != null
		    && m_syncComp.m_overchargeBuffs > 1
		    && m_overchargeAbility != null
		    && m_overchargeAbility.GetExtraDamageForMultiCast() > 0)
		{
			return m_overchargeAbility.GetExtraDamageForMultiCast();
		}
		return 0;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex >= GetNumTargets() - 1)
		{
			return true;
		}
		BoardSquare currentBoardSquare = caster.GetCurrentBoardSquare();
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		if (targetSquare == null || !targetSquare.IsValidForGameplay() || targetSquare == currentBoardSquare)
		{
			return false;
		}
		bool isValidTarget = true;
		if (UseStockBasedEvadeDistance())
		{
			int stocksRemaining = caster.GetAbilityData().GetStocksRemaining(m_myActionType);
			if (StockBasedDistUseSquareCoordDist())
			{
				int maxCoordDiff = GetMaxCoordDiff(currentBoardSquare, targetSquare);
				int maxDistance = Mathf.Max(1, stocksRemaining);
				if (GetStockBasedDistMaxSquareCoordDist() > 0)
				{
					maxDistance = Mathf.Min(GetStockBasedDistMaxSquareCoordDist(), stocksRemaining);
				}
				isValidTarget = maxCoordDiff <= maxDistance;
			}
			else
			{
				Vector3 vector = targetSquare.ToVector3() - currentBoardSquare.ToVector3();
				vector.y = 0f;
				float maxDistance = stocksRemaining * GetDistancePerStock() * Board.Get().squareSize + 0.05f;
				isValidTarget = vector.magnitude <= maxDistance;
			}
		}

		int foo;
		return isValidTarget
		       && KnockbackUtils.CanBuildStraightLineChargePath(caster, targetSquare, currentBoardSquare, false, out foo);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damageAmountNormal);
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		if (moddedEffectForSelf != null) moddedEffectForSelf.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		for (int i = 0; i <= currentTargeterIndex && i < Targeters.Count; i++)
		{
			AbilityUtil_Targeter abilityUtil_Targeter = Targeters[i];
			if (abilityUtil_Targeter != null
			    && abilityUtil_Targeter is AbilityUtil_Targeter_StretchCone)
			{
				AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = abilityUtil_Targeter as AbilityUtil_Targeter_StretchCone;
				List<AbilityTooltipSubject> tooltipSubjectTypes = abilityUtil_Targeter.GetTooltipSubjectTypes(targetActor);
				if (tooltipSubjectTypes != null
				    && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
				{
					int visibleActorsCountByTooltipSubject = abilityUtil_Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
					int baseDamage = 0;
					if (m_primaryAbility != null)
					{
						baseDamage += m_primaryAbility.GetExtraDamageFromAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
						baseDamage += m_primaryAbility.GetExtraDamageFromRadius(abilityUtil_Targeter_StretchCone.LastConeRadiusInSquares);
					}

					int damage = GetCurrentModdedDamage() + baseDamage;
					if (visibleActorsCountByTooltipSubject == 1)
					{
						damage += GetExtraDamageForSingleHit();
					}

					dictionary[AbilityTooltipSymbol.Damage] = damage;
					break;
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterDashAndBlast abilityMod_BlasterDashAndBlast = modAsBase as AbilityMod_BlasterDashAndBlast;
		AddTokenInt(tokens, "DamageAmountNormal", string.Empty, abilityMod_BlasterDashAndBlast != null
			? abilityMod_BlasterDashAndBlast.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal)
			: m_damageAmountNormal);
		AddTokenInt(tokens, "ExtraDamageForSingleHit", string.Empty, abilityMod_BlasterDashAndBlast != null
			? abilityMod_BlasterDashAndBlast.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit)
			: m_extraDamageForSingleHit);
		AddTokenInt(tokens, "StockBasedDistMaxSquareCoordDist", string.Empty, m_stockBasedDistMaxSquareCoordDist);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterDashAndBlast != null
			? abilityMod_BlasterDashAndBlast.m_enemyEffectNormalMod.GetModifiedValue(m_enemyEffectNormal)
			: m_enemyEffectNormal, "EnemyEffectNormal", m_enemyEffectNormal);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterDashAndBlast != null
			? abilityMod_BlasterDashAndBlast.m_enemyEffectOverchargedMod.GetModifiedValue(m_enemyEffectOvercharged)
			: m_enemyEffectOvercharged, "EnemyEffectOvercharged", m_enemyEffectOvercharged);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterDashAndBlast != null
			? abilityMod_BlasterDashAndBlast.m_selfEffectOnCastMod.GetModifiedValue(m_selfEffectOnCast)
			: m_selfEffectOnCast, "SelfEffectOnCast", m_selfEffectOnCast);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	private int GetMaxCoordDiff(BoardSquare a, BoardSquare b)
	{
		int dx = Mathf.Abs(a.x - b.x);
		int dy = Mathf.Abs(a.y - b.y);
		return Mathf.Max(dx, dy);
	}
}
