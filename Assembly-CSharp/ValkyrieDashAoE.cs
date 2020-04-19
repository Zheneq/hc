using System;
using System.Collections.Generic;
using UnityEngine;

public class ValkyrieDashAoE : Ability
{
	[Header("-- Shield effect")]
	public StandardEffectInfo m_shieldEffectInfo;

	public int m_techPointGainPerCoveredHit = 5;

	public int m_techPointGainPerTooCloseForCoverHit;

	[Separator("Dash Target Mode", true)]
	public ValkyrieDashAoE.DashTargetingMode m_dashTargetingMode;

	[Header("-- Targeting")]
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;

	public bool m_aoePenetratesLoS;

	[Separator("Aim Shield and Cone", true)]
	public float m_coneWidthAngle = 110f;

	public float m_coneRadius = 2.5f;

	public int m_coverDuration = 1;

	[Header("-- Cover Ignore Min Dist?")]
	public bool m_coverIgnoreMinDist = true;

	[Header("-- Whether to put guard ability on cooldown")]
	public bool m_triggerCooldownOnGuardAbiity;

	[Separator("Enemy hits", true)]
	public int m_damage = 0x14;

	public StandardEffectInfo m_enemyDebuff;

	[Separator("Ally & self hits", true)]
	public int m_absorb = 0x14;

	public AbilityCooldownChangeInfo m_cooldownReductionIfDamagedThisTurn;

	public StandardEffectInfo m_allyBuff;

	public StandardEffectInfo m_selfBuff;

	[Separator("Sequences", true)]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ValkyrieDashAoE m_abilityMod;

	private StandardEffectInfo m_cachedShieldEffectInfo;

	private StandardEffectInfo m_cachedEnemyDebuff;

	private StandardEffectInfo m_cachedAllyBuff;

	private StandardEffectInfo m_cachedSelfBuff;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.Start()).MethodHandle;
			}
			this.m_abilityName = "Valkyrie Dash AoE";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		base.Targeters.Clear();
		if (this.m_dashTargetingMode == ValkyrieDashAoE.DashTargetingMode.Aoe)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.SetupTargeter()).MethodHandle;
			}
			base.Targeter = new AbilityUtil_Targeter_BattleMonkUltimate(this, this.GetAoeShape(), this.AoePenetratesLoS(), this.GetAoeShape(), this.AoePenetratesLoS(), true);
			bool affectsEnemies = this.IncludeEnemies();
			bool affectsAllies = this.IncludeAllies();
			bool affectsCaster = this.IncludeSelf();
			base.Targeter.SetAffectedGroups(affectsEnemies, affectsAllies, affectsCaster);
		}
		else
		{
			AbilityUtil_Targeter_Charge item = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false);
			AbilityUtil_Targeter_ValkyrieGuard abilityUtil_Targeter_ValkyrieGuard = new AbilityUtil_Targeter_ValkyrieGuard(this, 1f, true, false, false);
			abilityUtil_Targeter_ValkyrieGuard.SetConeParams(true, this.GetConeWidthAngle(), this.GetConeRadius(), false);
			abilityUtil_Targeter_ValkyrieGuard.SetAffectedGroups(this.IncludeEnemies(), this.IncludeAllies(), this.IncludeSelf());
			abilityUtil_Targeter_ValkyrieGuard.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(item);
			base.Targeters.Add(abilityUtil_Targeter_ValkyrieGuard);
		}
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedShieldEffectInfo;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.SetCachedFields()).MethodHandle;
			}
			cachedShieldEffectInfo = this.m_abilityMod.m_shieldEffectInfoMod.GetModifiedValue(this.m_shieldEffectInfo);
		}
		else
		{
			cachedShieldEffectInfo = this.m_shieldEffectInfo;
		}
		this.m_cachedShieldEffectInfo = cachedShieldEffectInfo;
		StandardEffectInfo cachedEnemyDebuff;
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
			cachedEnemyDebuff = this.m_abilityMod.m_enemyDebuffMod.GetModifiedValue(this.m_enemyDebuff);
		}
		else
		{
			cachedEnemyDebuff = this.m_enemyDebuff;
		}
		this.m_cachedEnemyDebuff = cachedEnemyDebuff;
		StandardEffectInfo cachedAllyBuff;
		if (this.m_abilityMod)
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
			cachedAllyBuff = this.m_abilityMod.m_allyBuffMod.GetModifiedValue(this.m_allyBuff);
		}
		else
		{
			cachedAllyBuff = this.m_allyBuff;
		}
		this.m_cachedAllyBuff = cachedAllyBuff;
		StandardEffectInfo cachedSelfBuff;
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
			cachedSelfBuff = this.m_abilityMod.m_selfBuffMod.GetModifiedValue(this.m_selfBuff);
		}
		else
		{
			cachedSelfBuff = this.m_selfBuff;
		}
		this.m_cachedSelfBuff = cachedSelfBuff;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "Damage", string.Empty, this.m_damage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyDebuff, "EnemyDebuff", this.m_enemyDebuff, true);
		base.AddTokenInt(tokens, "Absorb", string.Empty, this.m_absorb, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyBuff, "AllyBuff", this.m_allyBuff, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_selfBuff, "SelfBuff", this.m_selfBuff, true);
		base.AddTokenInt(tokens, "CoverDuration", string.Empty, this.m_coverDuration, false);
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (this.m_dashTargetingMode == ValkyrieDashAoE.DashTargetingMode.Aoe)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.GetExpectedNumberOfTargeters()).MethodHandle;
			}
			return 1;
		}
		return Mathf.Min(this.GetTargetData().Length, 2);
	}

	public bool IncludeEnemies()
	{
		bool result;
		if (this.GetDamage() <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.IncludeEnemies()).MethodHandle;
			}
			result = this.m_enemyDebuff.m_applyEffect;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public bool IncludeAllies()
	{
		return this.m_allyBuff.m_applyEffect;
	}

	public bool IncludeSelf()
	{
		return this.m_selfBuff.m_applyEffect;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		if (this.m_damage != 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.m_damage));
		}
		if (this.m_absorb != 0)
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
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Ally, this.m_absorb));
		}
		this.m_enemyDebuff.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Enemy);
		this.m_allyBuff.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Ally);
		this.m_selfBuff.ReportAbilityTooltipNumbers(ref list, AbilityTooltipSubject.Self);
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor.\u000E() == base.ActorData.\u000E())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			int absorb = this.GetAbsorb();
			dictionary[AbilityTooltipSymbol.Absorb] = absorb;
		}
		else
		{
			int damage = this.GetDamage();
			dictionary[AbilityTooltipSymbol.Damage] = damage;
		}
		return dictionary;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (targetIndex == 0)
		{
			if (boardSquare != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
				}
				if (boardSquare.\u0016())
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
					if (boardSquare != caster.\u0012())
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
						return KnockbackUtils.BuildStraightLineChargePath(caster, boardSquare) != null;
					}
				}
			}
			return false;
		}
		return true;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		bool result;
		if (caster != null && caster.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.CustomCanCastValidation(ActorData)).MethodHandle;
			}
			result = !caster.\u000E().HasQueuedAbilityOfType(typeof(ValkyrieGuard));
		}
		else
		{
			result = false;
		}
		return result;
	}

	public StandardEffectInfo GetShieldEffectInfo()
	{
		return (this.m_cachedShieldEffectInfo == null) ? this.m_shieldEffectInfo : this.m_cachedShieldEffectInfo;
	}

	public AbilityAreaShape GetAoeShape()
	{
		return (!this.m_abilityMod) ? this.m_aoeShape : this.m_abilityMod.m_aoeShapeMod.GetModifiedValue(this.m_aoeShape);
	}

	public bool AoePenetratesLoS()
	{
		bool result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.AoePenetratesLoS()).MethodHandle;
			}
			result = this.m_abilityMod.m_aoePenetratesLoSMod.GetModifiedValue(this.m_aoePenetratesLoS);
		}
		else
		{
			result = this.m_aoePenetratesLoS;
		}
		return result;
	}

	public float GetConeWidthAngle()
	{
		float result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.GetConeWidthAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(this.m_coneWidthAngle);
		}
		else
		{
			result = this.m_coneWidthAngle;
		}
		return result;
	}

	public float GetConeRadius()
	{
		float result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.GetConeRadius()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneRadiusMod.GetModifiedValue(this.m_coneRadius);
		}
		else
		{
			result = this.m_coneRadius;
		}
		return result;
	}

	public int GetCoverDuration()
	{
		return (!this.m_abilityMod) ? this.m_coverDuration : this.m_abilityMod.m_coverDurationMod.GetModifiedValue(this.m_coverDuration);
	}

	public bool CoverIgnoreMinDist()
	{
		bool result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.CoverIgnoreMinDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_coverIgnoreMinDistMod.GetModifiedValue(this.m_coverIgnoreMinDist);
		}
		else
		{
			result = this.m_coverIgnoreMinDist;
		}
		return result;
	}

	public bool TriggerCooldownOnGuardAbiity()
	{
		bool result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.TriggerCooldownOnGuardAbiity()).MethodHandle;
			}
			result = this.m_abilityMod.m_triggerCooldownOnGuardAbiityMod.GetModifiedValue(this.m_triggerCooldownOnGuardAbiity);
		}
		else
		{
			result = this.m_triggerCooldownOnGuardAbiity;
		}
		return result;
	}

	public int GetTechPointGainPerCoveredHit()
	{
		int result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.GetTechPointGainPerCoveredHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_techPointGainPerCoveredHitMod.GetModifiedValue(this.m_techPointGainPerCoveredHit);
		}
		else
		{
			result = this.m_techPointGainPerCoveredHit;
		}
		return result;
	}

	public int GetTechPointGainPerTooCloseForCoverHit()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.GetTechPointGainPerTooCloseForCoverHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_techPointGainPerTooCloseForCoverHitMod.GetModifiedValue(this.m_techPointGainPerTooCloseForCoverHit);
		}
		else
		{
			result = this.m_techPointGainPerTooCloseForCoverHit;
		}
		return result;
	}

	public int GetDamage()
	{
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.GetDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damage);
		}
		else
		{
			result = this.m_damage;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyDebuff()
	{
		return (this.m_cachedEnemyDebuff == null) ? this.m_enemyDebuff : this.m_cachedEnemyDebuff;
	}

	public int GetAbsorb()
	{
		int result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.GetAbsorb()).MethodHandle;
			}
			result = this.m_abilityMod.m_absorbMod.GetModifiedValue(this.m_absorb);
		}
		else
		{
			result = this.m_absorb;
		}
		return result;
	}

	public StandardEffectInfo GetAllyBuff()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyBuff != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.GetAllyBuff()).MethodHandle;
			}
			result = this.m_cachedAllyBuff;
		}
		else
		{
			result = this.m_allyBuff;
		}
		return result;
	}

	public StandardEffectInfo GetSelfBuff()
	{
		return (this.m_cachedSelfBuff == null) ? this.m_selfBuff : this.m_cachedSelfBuff;
	}

	public int GetCooldownReductionOnHitAmount()
	{
		int num = this.m_cooldownReductionIfDamagedThisTurn.cooldownAddAmount;
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.GetCooldownReductionOnHitAmount()).MethodHandle;
			}
			num = this.m_abilityMod.m_cooldownReductionIfDamagedThisTurnMod.GetModifiedValue(num);
		}
		return num;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyrieDashAoE))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_ValkyrieDashAoE);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public unsafe override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.HasRestrictedFreePosDistance(ActorData, int, List<AbilityTarget>, float*, float*)).MethodHandle;
			}
			min = 1f;
			max = 1f;
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
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ValkyrieDashAoE.HasAimingOriginOverride(ActorData, int, List<AbilityTarget>, Vector3*)).MethodHandle;
			}
			BoardSquare boardSquare = Board.\u000E().\u000E(targetsSoFar[0].GridPos);
			overridePos = boardSquare.ToVector3();
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	public enum DashTargetingMode
	{
		Aoe,
		AimShieldCone
	}
}
