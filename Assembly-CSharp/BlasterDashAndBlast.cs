using System.Collections.Generic;
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
		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			m_overchargeAbility = (component.GetAbilityOfType(typeof(BlasterOvercharge)) as BlasterOvercharge);
			m_primaryAbility = (component.GetAbilityOfType(typeof(BlasterStretchingCone)) as BlasterStretchingCone);
			m_myActionType = component.GetActionTypeOfAbility(this);
		}
		int num = Mathf.Max(GetNumTargets() - 1, 1);
		int num2 = Mathf.Max(GetNumTargets() - num, 0);
		ClearTargeters();
		for (int i = 0; i < num; i++)
		{
			AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false);
			abilityUtil_Targeter_Charge.SetUseMultiTargetUpdate(true);
			StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
			if (moddedEffectForSelf != null)
			{
				if (moddedEffectForSelf.m_applyEffect)
				{
					abilityUtil_Targeter_Charge.m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
				}
			}
			base.Targeters.Add(abilityUtil_Targeter_Charge);
		}
		for (int j = 0; j < num2; j++)
		{
			AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = new AbilityUtil_Targeter_StretchCone(this, GetMinLength(), GetMaxLength(), GetMinAngle(), GetMaxAngle(), m_stretchStyle, GetConeBackwardOffset(), PenetrateLineOfSight());
			abilityUtil_Targeter_StretchCone.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_StretchCone);
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					BoardSquare boardSquareSafe = Board.Get().GetSquare(targetsSoFar[0].GridPos);
					overridePos = boardSquareSafe.GetWorldPosition();
					return true;
				}
				}
			}
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	public bool UseConeParamFromPrimary()
	{
		return m_useConeParamsFromPrimary && m_primaryAbility != null;
	}

	public bool UseHitPropertyFromPrimary()
	{
		int result;
		if (m_useHitParamsFromPrimary)
		{
			result = ((m_primaryAbility != null) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyEffectNormal;
		if ((bool)m_abilityMod)
		{
			cachedEnemyEffectNormal = m_abilityMod.m_enemyEffectNormalMod.GetModifiedValue(m_enemyEffectNormal);
		}
		else
		{
			cachedEnemyEffectNormal = m_enemyEffectNormal;
		}
		m_cachedEnemyEffectNormal = cachedEnemyEffectNormal;
		StandardEffectInfo cachedEnemyEffectOvercharged;
		if ((bool)m_abilityMod)
		{
			cachedEnemyEffectOvercharged = m_abilityMod.m_enemyEffectOverchargedMod.GetModifiedValue(m_enemyEffectOvercharged);
		}
		else
		{
			cachedEnemyEffectOvercharged = m_enemyEffectOvercharged;
		}
		m_cachedEnemyEffectOvercharged = cachedEnemyEffectOvercharged;
		m_cachedSelfEffectOnCast = ((!m_abilityMod) ? m_selfEffectOnCast : m_abilityMod.m_selfEffectOnCastMod.GetModifiedValue(m_selfEffectOnCast));
	}

	public float GetMinLength()
	{
		if (UseConeParamFromPrimary())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.GetMinLength();
				}
			}
		}
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_minLengthMod.GetModifiedValue(m_minLength);
		}
		else
		{
			result = m_minLength;
		}
		return result;
	}

	public float GetMaxLength()
	{
		if (UseConeParamFromPrimary())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.GetMaxLength();
				}
			}
		}
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxLengthMod.GetModifiedValue(m_maxLength);
		}
		else
		{
			result = m_maxLength;
		}
		return result;
	}

	public float GetMinAngle()
	{
		if (UseConeParamFromPrimary())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.GetMinAngle();
				}
			}
		}
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_minAngleMod.GetModifiedValue(m_minAngle);
		}
		else
		{
			result = m_minAngle;
		}
		return result;
	}

	public float GetMaxAngle()
	{
		if (UseConeParamFromPrimary())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.GetMaxAngle();
				}
			}
		}
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxAngleMod.GetModifiedValue(m_maxAngle);
		}
		else
		{
			result = m_maxAngle;
		}
		return result;
	}

	public float GetConeBackwardOffset()
	{
		if (UseConeParamFromPrimary())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.GetConeBackwardOffset();
				}
			}
		}
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset);
		}
		else
		{
			result = m_coneBackwardOffset;
		}
		return result;
	}

	public bool PenetrateLineOfSight()
	{
		if (UseConeParamFromPrimary())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.PenetrateLineOfSight();
				}
			}
		}
		return (!m_abilityMod) ? m_penetrateLineOfSight : m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight);
	}

	public bool UseStockBasedEvadeDistance()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_useStockBasedEvadeDistanceMod.GetModifiedValue(m_useStockBasedEvadeDistance);
		}
		else
		{
			result = m_useStockBasedEvadeDistance;
		}
		return result;
	}

	public float GetDistancePerStock()
	{
		float b;
		if ((bool)m_abilityMod)
		{
			b = m_abilityMod.m_distancePerStockMod.GetModifiedValue(m_distancePerStock);
		}
		else
		{
			b = m_distancePerStock;
		}
		return Mathf.Max(0.1f, b);
	}

	public bool StockBasedDistUseSquareCoordDist()
	{
		bool result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_stockBasedDistUseSquareCoordDistMod.GetModifiedValue(m_stockBasedDistUseSquareCoordDist);
		}
		else
		{
			result = m_stockBasedDistUseSquareCoordDist;
		}
		return result;
	}

	public int GetStockBasedDistMaxSquareCoordDist()
	{
		return (!(m_abilityMod != null)) ? m_stockBasedDistMaxSquareCoordDist : m_abilityMod.m_stockBasedDistMaxSquareCoordDistMod.GetModifiedValue(m_stockBasedDistMaxSquareCoordDist);
	}

	public int GetDamageAmountNormal()
	{
		if (UseHitPropertyFromPrimary())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.GetDamageAmountNormal();
				}
			}
		}
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal);
		}
		else
		{
			result = m_damageAmountNormal;
		}
		return result;
	}

	public int GetDamageAmountOvercharged()
	{
		return GetDamageAmountNormal() + m_overchargeAbility.GetExtraDamage() + GetMultiStackOverchargeDamage();
	}

	public int GetExtraDamageForSingleHit()
	{
		if (UseHitPropertyFromPrimary())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.GetExtraDamageForSingleHit();
				}
			}
		}
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit);
		}
		else
		{
			result = m_extraDamageForSingleHit;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyEffectNormal()
	{
		if (UseHitPropertyFromPrimary())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.GetNormalEnemyEffect();
				}
			}
		}
		StandardEffectInfo result;
		if (m_cachedEnemyEffectNormal != null)
		{
			result = m_cachedEnemyEffectNormal;
		}
		else
		{
			result = m_enemyEffectNormal;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyEffectOvercharged()
	{
		if (UseHitPropertyFromPrimary())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.GetOverchargedEnemyEffect();
				}
			}
		}
		StandardEffectInfo result;
		if (m_cachedEnemyEffectOvercharged != null)
		{
			result = m_cachedEnemyEffectOvercharged;
		}
		else
		{
			result = m_enemyEffectOvercharged;
		}
		return result;
	}

	public StandardEffectInfo GetSelfEffectOnCast()
	{
		return (m_cachedSelfEffectOnCast == null) ? m_selfEffectOnCast : m_cachedSelfEffectOnCast;
	}

	public int GetCurrentModdedDamage()
	{
		if (AmOvercharged(base.ActorData))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return GetDamageAmountOvercharged() + GetMultiStackOverchargeDamage();
				}
			}
		}
		return GetDamageAmountNormal();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BlasterDashAndBlast))
		{
			m_abilityMod = (abilityMod as AbilityMod_BlasterDashAndBlast);
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
		if (m_syncComp != null && m_syncComp.m_overchargeBuffs > 1 && m_overchargeAbility != null)
		{
			if (m_overchargeAbility.GetExtraDamageForMultiCast() > 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return m_overchargeAbility.GetExtraDamageForMultiCast();
					}
				}
			}
		}
		return 0;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex < GetNumTargets() - 1)
		{
			BoardSquare currentBoardSquare = caster.GetCurrentBoardSquare();
			BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
			if (boardSquareSafe != null)
			{
				if (boardSquareSafe.IsBaselineHeight() && boardSquareSafe != currentBoardSquare)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
						{
							bool flag = true;
							if (UseStockBasedEvadeDistance())
							{
								int stocksRemaining = caster.GetAbilityData().GetStocksRemaining(m_myActionType);
								if (StockBasedDistUseSquareCoordDist())
								{
									int maxCoordDiff = GetMaxCoordDiff(currentBoardSquare, boardSquareSafe);
									int num = Mathf.Max(1, stocksRemaining);
									if (GetStockBasedDistMaxSquareCoordDist() > 0)
									{
										num = Mathf.Min(GetStockBasedDistMaxSquareCoordDist(), stocksRemaining);
									}
									flag = (maxCoordDiff <= num);
								}
								else
								{
									Vector3 vector = boardSquareSafe.ToVector3() - currentBoardSquare.ToVector3();
									vector.y = 0f;
									float magnitude = vector.magnitude;
									float num2 = (float)stocksRemaining * GetDistancePerStock() * Board.Get().squareSize + 0.05f;
									flag = (magnitude <= num2);
								}
							}
							int result;
							if (flag)
							{
								result = (KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, currentBoardSquare, false, out int _) ? 1 : 0);
							}
							else
							{
								result = 0;
							}
							return (byte)result != 0;
						}
						}
					}
				}
			}
			return false;
		}
		return true;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damageAmountNormal);
		StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
		if (moddedEffectForSelf != null)
		{
			moddedEffectForSelf.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			if (i >= base.Targeters.Count)
			{
				break;
			}
			AbilityUtil_Targeter abilityUtil_Targeter = base.Targeters[i];
			if (abilityUtil_Targeter == null)
			{
				continue;
			}
			if (!(abilityUtil_Targeter is AbilityUtil_Targeter_StretchCone))
			{
				continue;
			}
			AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = abilityUtil_Targeter as AbilityUtil_Targeter_StretchCone;
			List<AbilityTooltipSubject> tooltipSubjectTypes = abilityUtil_Targeter.GetTooltipSubjectTypes(targetActor);
			if (tooltipSubjectTypes == null)
			{
				continue;
			}
			if (!tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				continue;
			}
			int visibleActorsCountByTooltipSubject = abilityUtil_Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			int num = 0;
			if (m_primaryAbility != null)
			{
				num += m_primaryAbility.GetExtraDamageFromAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
				num += m_primaryAbility.GetExtraDamageFromRadius(abilityUtil_Targeter_StretchCone.LastConeRadiusInSquares);
			}
			int num2 = GetCurrentModdedDamage() + num;
			if (visibleActorsCountByTooltipSubject == 1)
			{
				num2 += GetExtraDamageForSingleHit();
			}
			dictionary[AbilityTooltipSymbol.Damage] = num2;
			break;
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterDashAndBlast abilityMod_BlasterDashAndBlast = modAsBase as AbilityMod_BlasterDashAndBlast;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_BlasterDashAndBlast)
		{
			val = abilityMod_BlasterDashAndBlast.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal);
		}
		else
		{
			val = m_damageAmountNormal;
		}
		AddTokenInt(tokens, "DamageAmountNormal", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_BlasterDashAndBlast)
		{
			val2 = abilityMod_BlasterDashAndBlast.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit);
		}
		else
		{
			val2 = m_extraDamageForSingleHit;
		}
		AddTokenInt(tokens, "ExtraDamageForSingleHit", empty2, val2);
		AddTokenInt(tokens, "StockBasedDistMaxSquareCoordDist", string.Empty, m_stockBasedDistMaxSquareCoordDist);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_BlasterDashAndBlast) ? m_enemyEffectNormal : abilityMod_BlasterDashAndBlast.m_enemyEffectNormalMod.GetModifiedValue(m_enemyEffectNormal), "EnemyEffectNormal", m_enemyEffectNormal);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_BlasterDashAndBlast)
		{
			effectInfo = abilityMod_BlasterDashAndBlast.m_enemyEffectOverchargedMod.GetModifiedValue(m_enemyEffectOvercharged);
		}
		else
		{
			effectInfo = m_enemyEffectOvercharged;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyEffectOvercharged", m_enemyEffectOvercharged);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_BlasterDashAndBlast)
		{
			effectInfo2 = abilityMod_BlasterDashAndBlast.m_selfEffectOnCastMod.GetModifiedValue(m_selfEffectOnCast);
		}
		else
		{
			effectInfo2 = m_selfEffectOnCast;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "SelfEffectOnCast", m_selfEffectOnCast);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	private int GetMaxCoordDiff(BoardSquare a, BoardSquare b)
	{
		int a2 = Mathf.Abs(a.x - b.x);
		int b2 = Mathf.Abs(a.y - b.y);
		return Mathf.Max(a2, b2);
	}
}
