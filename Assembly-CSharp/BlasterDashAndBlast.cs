using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Dash and Blast";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		AbilityData component = base.GetComponent<AbilityData>();
		if (component != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.Setup()).MethodHandle;
			}
			this.m_overchargeAbility = (component.GetAbilityOfType(typeof(BlasterOvercharge)) as BlasterOvercharge);
			this.m_primaryAbility = (component.GetAbilityOfType(typeof(BlasterStretchingCone)) as BlasterStretchingCone);
			this.m_myActionType = component.GetActionTypeOfAbility(this);
		}
		int num = Mathf.Max(base.GetNumTargets() - 1, 1);
		int num2 = Mathf.Max(base.GetNumTargets() - num, 0);
		base.ClearTargeters();
		for (int i = 0; i < num; i++)
		{
			AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false);
			abilityUtil_Targeter_Charge.SetUseMultiTargetUpdate(true);
			StandardEffectInfo moddedEffectForSelf = base.GetModdedEffectForSelf();
			if (moddedEffectForSelf != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (moddedEffectForSelf.m_applyEffect)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					abilityUtil_Targeter_Charge.m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
				}
			}
			base.Targeters.Add(abilityUtil_Targeter_Charge);
		}
		for (int j = 0; j < num2; j++)
		{
			AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = new AbilityUtil_Targeter_StretchCone(this, this.GetMinLength(), this.GetMaxLength(), this.GetMinAngle(), this.GetMaxAngle(), this.m_stretchStyle, this.GetConeBackwardOffset(), this.PenetrateLineOfSight());
			abilityUtil_Targeter_StretchCone.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_StretchCone);
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public void OnPrimaryAttackModChange()
	{
		this.Setup();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetRangeInSquares(0) - 0.5f + this.GetMaxLength();
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			min = this.GetMinLength() * Board.Get().squareSize;
			max = this.GetMaxLength() * Board.Get().squareSize;
			return true;
		}
		return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	public unsafe override bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		if (targetIndex == 1)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.HasAimingOriginOverride(ActorData, int, List<AbilityTarget>, Vector3*)).MethodHandle;
			}
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(targetsSoFar[0].GridPos);
			overridePos = boardSquareSafe.GetWorldPosition();
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	public bool UseConeParamFromPrimary()
	{
		return this.m_useConeParamsFromPrimary && this.m_primaryAbility != null;
	}

	public bool UseHitPropertyFromPrimary()
	{
		bool result;
		if (this.m_useHitParamsFromPrimary)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.UseHitPropertyFromPrimary()).MethodHandle;
			}
			result = (this.m_primaryAbility != null);
		}
		else
		{
			result = false;
		}
		return result;
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedEnemyEffectNormal;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.SetCachedFields()).MethodHandle;
			}
			cachedEnemyEffectNormal = this.m_abilityMod.m_enemyEffectNormalMod.GetModifiedValue(this.m_enemyEffectNormal);
		}
		else
		{
			cachedEnemyEffectNormal = this.m_enemyEffectNormal;
		}
		this.m_cachedEnemyEffectNormal = cachedEnemyEffectNormal;
		StandardEffectInfo cachedEnemyEffectOvercharged;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedEnemyEffectOvercharged = this.m_abilityMod.m_enemyEffectOverchargedMod.GetModifiedValue(this.m_enemyEffectOvercharged);
		}
		else
		{
			cachedEnemyEffectOvercharged = this.m_enemyEffectOvercharged;
		}
		this.m_cachedEnemyEffectOvercharged = cachedEnemyEffectOvercharged;
		this.m_cachedSelfEffectOnCast = ((!this.m_abilityMod) ? this.m_selfEffectOnCast : this.m_abilityMod.m_selfEffectOnCastMod.GetModifiedValue(this.m_selfEffectOnCast));
	}

	public float GetMinLength()
	{
		if (this.UseConeParamFromPrimary())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.GetMinLength()).MethodHandle;
			}
			return this.m_primaryAbility.GetMinLength();
		}
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			result = this.m_abilityMod.m_minLengthMod.GetModifiedValue(this.m_minLength);
		}
		else
		{
			result = this.m_minLength;
		}
		return result;
	}

	public float GetMaxLength()
	{
		if (this.UseConeParamFromPrimary())
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.GetMaxLength()).MethodHandle;
			}
			return this.m_primaryAbility.GetMaxLength();
		}
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			result = this.m_abilityMod.m_maxLengthMod.GetModifiedValue(this.m_maxLength);
		}
		else
		{
			result = this.m_maxLength;
		}
		return result;
	}

	public float GetMinAngle()
	{
		if (this.UseConeParamFromPrimary())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.GetMinAngle()).MethodHandle;
			}
			return this.m_primaryAbility.GetMinAngle();
		}
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			result = this.m_abilityMod.m_minAngleMod.GetModifiedValue(this.m_minAngle);
		}
		else
		{
			result = this.m_minAngle;
		}
		return result;
	}

	public float GetMaxAngle()
	{
		if (this.UseConeParamFromPrimary())
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.GetMaxAngle()).MethodHandle;
			}
			return this.m_primaryAbility.GetMaxAngle();
		}
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			result = this.m_abilityMod.m_maxAngleMod.GetModifiedValue(this.m_maxAngle);
		}
		else
		{
			result = this.m_maxAngle;
		}
		return result;
	}

	public float GetConeBackwardOffset()
	{
		if (this.UseConeParamFromPrimary())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.GetConeBackwardOffset()).MethodHandle;
			}
			return this.m_primaryAbility.GetConeBackwardOffset();
		}
		float result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			result = this.m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(this.m_coneBackwardOffset);
		}
		else
		{
			result = this.m_coneBackwardOffset;
		}
		return result;
	}

	public bool PenetrateLineOfSight()
	{
		if (this.UseConeParamFromPrimary())
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.PenetrateLineOfSight()).MethodHandle;
			}
			return this.m_primaryAbility.PenetrateLineOfSight();
		}
		return (!this.m_abilityMod) ? this.m_penetrateLineOfSight : this.m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(this.m_penetrateLineOfSight);
	}

	public bool UseStockBasedEvadeDistance()
	{
		bool result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.UseStockBasedEvadeDistance()).MethodHandle;
			}
			result = this.m_abilityMod.m_useStockBasedEvadeDistanceMod.GetModifiedValue(this.m_useStockBasedEvadeDistance);
		}
		else
		{
			result = this.m_useStockBasedEvadeDistance;
		}
		return result;
	}

	public float GetDistancePerStock()
	{
		float a = 0.1f;
		float b;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.GetDistancePerStock()).MethodHandle;
			}
			b = this.m_abilityMod.m_distancePerStockMod.GetModifiedValue(this.m_distancePerStock);
		}
		else
		{
			b = this.m_distancePerStock;
		}
		return Mathf.Max(a, b);
	}

	public bool StockBasedDistUseSquareCoordDist()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.StockBasedDistUseSquareCoordDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_stockBasedDistUseSquareCoordDistMod.GetModifiedValue(this.m_stockBasedDistUseSquareCoordDist);
		}
		else
		{
			result = this.m_stockBasedDistUseSquareCoordDist;
		}
		return result;
	}

	public int GetStockBasedDistMaxSquareCoordDist()
	{
		return (!(this.m_abilityMod != null)) ? this.m_stockBasedDistMaxSquareCoordDist : this.m_abilityMod.m_stockBasedDistMaxSquareCoordDistMod.GetModifiedValue(this.m_stockBasedDistMaxSquareCoordDist);
	}

	public int GetDamageAmountNormal()
	{
		if (this.UseHitPropertyFromPrimary())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.GetDamageAmountNormal()).MethodHandle;
			}
			return this.m_primaryAbility.GetDamageAmountNormal();
		}
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			result = this.m_abilityMod.m_damageAmountNormalMod.GetModifiedValue(this.m_damageAmountNormal);
		}
		else
		{
			result = this.m_damageAmountNormal;
		}
		return result;
	}

	public int GetDamageAmountOvercharged()
	{
		return this.GetDamageAmountNormal() + this.m_overchargeAbility.GetExtraDamage() + this.GetMultiStackOverchargeDamage();
	}

	public int GetExtraDamageForSingleHit()
	{
		if (this.UseHitPropertyFromPrimary())
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.GetExtraDamageForSingleHit()).MethodHandle;
			}
			return this.m_primaryAbility.GetExtraDamageForSingleHit();
		}
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			result = this.m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(this.m_extraDamageForSingleHit);
		}
		else
		{
			result = this.m_extraDamageForSingleHit;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyEffectNormal()
	{
		if (this.UseHitPropertyFromPrimary())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.GetEnemyEffectNormal()).MethodHandle;
			}
			return this.m_primaryAbility.GetNormalEnemyEffect();
		}
		StandardEffectInfo result;
		if (this.m_cachedEnemyEffectNormal != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			result = this.m_cachedEnemyEffectNormal;
		}
		else
		{
			result = this.m_enemyEffectNormal;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyEffectOvercharged()
	{
		if (this.UseHitPropertyFromPrimary())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.GetEnemyEffectOvercharged()).MethodHandle;
			}
			return this.m_primaryAbility.GetOverchargedEnemyEffect();
		}
		StandardEffectInfo result;
		if (this.m_cachedEnemyEffectOvercharged != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			result = this.m_cachedEnemyEffectOvercharged;
		}
		else
		{
			result = this.m_enemyEffectOvercharged;
		}
		return result;
	}

	public StandardEffectInfo GetSelfEffectOnCast()
	{
		return (this.m_cachedSelfEffectOnCast == null) ? this.m_selfEffectOnCast : this.m_cachedSelfEffectOnCast;
	}

	public int GetCurrentModdedDamage()
	{
		if (this.AmOvercharged(base.ActorData))
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.GetCurrentModdedDamage()).MethodHandle;
			}
			return this.GetDamageAmountOvercharged() + this.GetMultiStackOverchargeDamage();
		}
		return this.GetDamageAmountNormal();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BlasterDashAndBlast))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_BlasterDashAndBlast);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	private bool AmOvercharged(ActorData caster)
	{
		if (this.m_syncComp == null)
		{
			this.m_syncComp = base.GetComponent<Blaster_SyncComponent>();
		}
		return this.m_syncComp.m_overchargeBuffs > 0;
	}

	private int GetMultiStackOverchargeDamage()
	{
		if (this.m_syncComp != null && this.m_syncComp.m_overchargeBuffs > 1 && this.m_overchargeAbility != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.GetMultiStackOverchargeDamage()).MethodHandle;
			}
			if (this.m_overchargeAbility.GetExtraDamageForMultiCast() > 0)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.m_overchargeAbility.GetExtraDamageForMultiCast();
			}
		}
		return 0;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex < base.GetNumTargets() - 1)
		{
			BoardSquare currentBoardSquare = caster.GetCurrentBoardSquare();
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
			if (boardSquareSafe != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
				}
				if (boardSquareSafe.IsBaselineHeight() && boardSquareSafe != currentBoardSquare)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					bool flag = true;
					if (this.UseStockBasedEvadeDistance())
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						int stocksRemaining = caster.GetAbilityData().GetStocksRemaining(this.m_myActionType);
						if (this.StockBasedDistUseSquareCoordDist())
						{
							int maxCoordDiff = this.GetMaxCoordDiff(currentBoardSquare, boardSquareSafe);
							int num = Mathf.Max(1, stocksRemaining);
							if (this.GetStockBasedDistMaxSquareCoordDist() > 0)
							{
								num = Mathf.Min(this.GetStockBasedDistMaxSquareCoordDist(), stocksRemaining);
							}
							flag = (maxCoordDiff <= num);
						}
						else
						{
							Vector3 vector = boardSquareSafe.ToVector3() - currentBoardSquare.ToVector3();
							vector.y = 0f;
							float magnitude = vector.magnitude;
							float num2 = (float)stocksRemaining * this.GetDistancePerStock() * Board.Get().squareSize + 0.05f;
							flag = (magnitude <= num2);
						}
					}
					bool result;
					if (flag)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						int num3;
						result = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, currentBoardSquare, false, out num3);
					}
					else
					{
						result = false;
					}
					return result;
				}
			}
			return false;
		}
		return true;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.m_damageAmountNormal);
		StandardEffectInfo moddedEffectForSelf = base.GetModdedEffectForSelf();
		if (moddedEffectForSelf != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			moddedEffectForSelf.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		}
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (i >= base.Targeters.Count)
			{
				break;
			}
			AbilityUtil_Targeter abilityUtil_Targeter = base.Targeters[i];
			if (abilityUtil_Targeter != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
				}
				if (abilityUtil_Targeter is AbilityUtil_Targeter_StretchCone)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = abilityUtil_Targeter as AbilityUtil_Targeter_StretchCone;
					List<AbilityTooltipSubject> tooltipSubjectTypes = abilityUtil_Targeter.GetTooltipSubjectTypes(targetActor);
					if (tooltipSubjectTypes != null)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
						{
							int visibleActorsCountByTooltipSubject = abilityUtil_Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
							int num = 0;
							if (this.m_primaryAbility != null)
							{
								num += this.m_primaryAbility.GetExtraDamageFromAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
								num += this.m_primaryAbility.GetExtraDamageFromRadius(abilityUtil_Targeter_StretchCone.LastConeRadiusInSquares);
							}
							int num2 = this.GetCurrentModdedDamage() + num;
							if (visibleActorsCountByTooltipSubject == 1)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								num2 += this.GetExtraDamageForSingleHit();
							}
							dictionary[AbilityTooltipSymbol.Damage] = num2;
							break;
						}
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterDashAndBlast abilityMod_BlasterDashAndBlast = modAsBase as AbilityMod_BlasterDashAndBlast;
		string name = "DamageAmountNormal";
		string empty = string.Empty;
		int val;
		if (abilityMod_BlasterDashAndBlast)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BlasterDashAndBlast.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_BlasterDashAndBlast.m_damageAmountNormalMod.GetModifiedValue(this.m_damageAmountNormal);
		}
		else
		{
			val = this.m_damageAmountNormal;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "ExtraDamageForSingleHit";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_BlasterDashAndBlast)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			val2 = abilityMod_BlasterDashAndBlast.m_extraDamageForSingleHitMod.GetModifiedValue(this.m_extraDamageForSingleHit);
		}
		else
		{
			val2 = this.m_extraDamageForSingleHit;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		base.AddTokenInt(tokens, "StockBasedDistMaxSquareCoordDist", string.Empty, this.m_stockBasedDistMaxSquareCoordDist, false);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_BlasterDashAndBlast) ? this.m_enemyEffectNormal : abilityMod_BlasterDashAndBlast.m_enemyEffectNormalMod.GetModifiedValue(this.m_enemyEffectNormal), "EnemyEffectNormal", this.m_enemyEffectNormal, true);
		StandardEffectInfo effectInfo;
		if (abilityMod_BlasterDashAndBlast)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo = abilityMod_BlasterDashAndBlast.m_enemyEffectOverchargedMod.GetModifiedValue(this.m_enemyEffectOvercharged);
		}
		else
		{
			effectInfo = this.m_enemyEffectOvercharged;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyEffectOvercharged", this.m_enemyEffectOvercharged, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_BlasterDashAndBlast)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo2 = abilityMod_BlasterDashAndBlast.m_selfEffectOnCastMod.GetModifiedValue(this.m_selfEffectOnCast);
		}
		else
		{
			effectInfo2 = this.m_selfEffectOnCast;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "SelfEffectOnCast", this.m_selfEffectOnCast, true);
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
