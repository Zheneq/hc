using System;
using System.Collections.Generic;
using UnityEngine;

public class Ninja360Attack : Ability
{
	[Separator("Targeting", true)]
	public Ninja360Attack.TargetingMode m_targetingMode = Ninja360Attack.TargetingMode.Laser;

	public bool m_penetrateLineOfSight;

	[Header("  (( if using Laser ))")]
	public LaserTargetingInfo m_laserInfo;

	[Header("  (( if using Cone ))")]
	public ConeTargetingInfo m_coneInfo;

	public float m_innerConeAngle;

	[Header("  (( if using Shape ))")]
	public AbilityAreaShape m_targeterShape = AbilityAreaShape.Three_x_Three;

	[Separator("On Hit", true)]
	public int m_damageAmount = 0xF;

	[Header("-- Damage for Inner Area Hit --")]
	public int m_innerAreaDamage = 0x1E;

	[Space(10f)]
	public StandardEffectInfo m_enemyHitEffect;

	public bool m_useDifferentEffectForInnerCone;

	public StandardEffectInfo m_innerConeEnemyHitEffect;

	[Header("-- Energy Gain on Marked Target --")]
	public int m_energyGainOnMarkedHit;

	public int m_selfHealOnMarkedHit;

	[Separator("[Deathmark] Effect", "magenta")]
	public bool m_applyDeathmarkEffect = true;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private const int c_innerConeIdentifier = 1;

	private Ninja_SyncComponent m_syncComp;

	private AbilityMod_Ninja360Attack m_abilityMod;

	private LaserTargetingInfo m_cachedLaserInfo;

	private ConeTargetingInfo m_cachedConeInfo;

	private StandardEffectInfo m_cachedEnemyHitEffect;

	private StandardEffectInfo m_cachedInnerConeEnemyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.Start()).MethodHandle;
			}
			this.m_abilityName = "Ninja Basic Attack";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		if (this.m_syncComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.Setup()).MethodHandle;
			}
			this.m_syncComp = base.GetComponent<Ninja_SyncComponent>();
		}
		if (this.m_targetingMode == Ninja360Attack.TargetingMode.Laser)
		{
			LaserTargetingInfo laserInfo = this.GetLaserInfo();
			base.Targeter = new AbilityUtil_Targeter_Laser(this, laserInfo.width, laserInfo.range, this.PenetrateLineOfSight(), laserInfo.maxTargets, false, false);
		}
		else if (this.m_targetingMode == Ninja360Attack.TargetingMode.Cone)
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
			ConeTargetingInfo coneInfo = this.GetConeInfo();
			float radiusInSquares = coneInfo.m_radiusInSquares;
			List<AbilityUtil_Targeter_MultipleCones.ConeDimensions> list = new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>();
			list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneInfo.m_widthAngleDeg, radiusInSquares));
			if (this.GetInnerConeAngle() > 0f)
			{
				list.Add(new AbilityUtil_Targeter_MultipleCones.ConeDimensions(this.GetInnerConeAngle(), radiusInSquares));
			}
			AbilityUtil_Targeter_MultipleCones abilityUtil_Targeter_MultipleCones = new AbilityUtil_Targeter_MultipleCones(this, list, coneInfo.m_backwardsOffset, this.PenetrateLineOfSight(), true, true, false, this.GetSelfHealOnMarkedHit() > 0);
			if (this.GetSelfHealOnMarkedHit() > 0)
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
				abilityUtil_Targeter_MultipleCones.m_affectCasterDelegate = new AbilityUtil_Targeter_MultipleCones.IsAffectingCasterDelegate(this.IncludeCasterForTargeter);
			}
			base.Targeter = abilityUtil_Targeter_MultipleCones;
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_Shape(this, this.GetTargeterShape(), this.PenetrateLineOfSight(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
		}
	}

	private bool IncludeCasterForTargeter(ActorData caster, List<ActorData> addedSoFar)
	{
		if (this.GetSelfHealOnMarkedHit() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.IncludeCasterForTargeter(ActorData, List<ActorData>)).MethodHandle;
			}
			for (int i = 0; i < addedSoFar.Count; i++)
			{
				if (this.IsActorMarked(addedSoFar[i]))
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
					return true;
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Design --</color>\nPlease edit [Deathmark] info on Ninja sync component.\n<color=cyan>-- For Art --</color>\nOn Sequence, for HitActorGroupOnAnimEventSequence components, use:\n" + 1 + " for Inner cone group identifier\n";
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		if (this.m_targetingMode == Ninja360Attack.TargetingMode.Laser)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.GetTargetableRadiusInSquares(ActorData)).MethodHandle;
			}
			return this.GetLaserInfo().range;
		}
		if (this.m_targetingMode == Ninja360Attack.TargetingMode.Cone)
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
			return this.GetConeInfo().m_radiusInSquares;
		}
		return 0f;
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo cachedLaserInfo;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.SetCachedFields()).MethodHandle;
			}
			cachedLaserInfo = this.m_abilityMod.m_laserInfoMod.GetModifiedValue(this.m_laserInfo);
		}
		else
		{
			cachedLaserInfo = this.m_laserInfo;
		}
		this.m_cachedLaserInfo = cachedLaserInfo;
		this.m_cachedConeInfo = ((!this.m_abilityMod) ? this.m_coneInfo : this.m_abilityMod.m_coneInfoMod.GetModifiedValue(this.m_coneInfo));
		StandardEffectInfo cachedEnemyHitEffect;
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
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
		StandardEffectInfo cachedInnerConeEnemyHitEffect;
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
			cachedInnerConeEnemyHitEffect = this.m_abilityMod.m_innerConeEnemyHitEffectMod.GetModifiedValue(this.m_innerConeEnemyHitEffect);
		}
		else
		{
			cachedInnerConeEnemyHitEffect = this.m_innerConeEnemyHitEffect;
		}
		this.m_cachedInnerConeEnemyHitEffect = cachedInnerConeEnemyHitEffect;
	}

	public bool PenetrateLineOfSight()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.PenetrateLineOfSight()).MethodHandle;
			}
			result = this.m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(this.m_penetrateLineOfSight);
		}
		else
		{
			result = this.m_penetrateLineOfSight;
		}
		return result;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedLaserInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.GetLaserInfo()).MethodHandle;
			}
			result = this.m_cachedLaserInfo;
		}
		else
		{
			result = this.m_laserInfo;
		}
		return result;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return (this.m_cachedConeInfo == null) ? this.m_coneInfo : this.m_cachedConeInfo;
	}

	public float GetInnerConeAngle()
	{
		return (!this.m_abilityMod) ? this.m_innerConeAngle : this.m_abilityMod.m_innerConeAngleMod.GetModifiedValue(this.m_innerConeAngle);
	}

	public AbilityAreaShape GetTargeterShape()
	{
		AbilityAreaShape result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.GetTargeterShape()).MethodHandle;
			}
			result = this.m_abilityMod.m_targeterShapeMod.GetModifiedValue(this.m_targeterShape);
		}
		else
		{
			result = this.m_targeterShape;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.GetDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			result = this.m_damageAmount;
		}
		return result;
	}

	public int GetInnerAreaDamage()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.GetInnerAreaDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_innerAreaDamageMod.GetModifiedValue(this.m_innerAreaDamage);
		}
		else
		{
			result = this.m_innerAreaDamage;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.GetEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public bool UseDifferentEffectForInnerCone()
	{
		return (!this.m_abilityMod) ? this.m_useDifferentEffectForInnerCone : this.m_abilityMod.m_useDifferentEffectForInnerConeMod.GetModifiedValue(this.m_useDifferentEffectForInnerCone);
	}

	public StandardEffectInfo GetInnerConeEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedInnerConeEnemyHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.GetInnerConeEnemyHitEffect()).MethodHandle;
			}
			result = this.m_cachedInnerConeEnemyHitEffect;
		}
		else
		{
			result = this.m_innerConeEnemyHitEffect;
		}
		return result;
	}

	public int GetEnergyGainOnMarkedHit()
	{
		int result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.GetEnergyGainOnMarkedHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_energyGainOnMarkedHitMod.GetModifiedValue(this.m_energyGainOnMarkedHit);
		}
		else
		{
			result = this.m_energyGainOnMarkedHit;
		}
		return result;
	}

	public int GetSelfHealOnMarkedHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.GetSelfHealOnMarkedHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_selfHealOnMarkedHitMod.GetModifiedValue(this.m_selfHealOnMarkedHit);
		}
		else
		{
			result = this.m_selfHealOnMarkedHit;
		}
		return result;
	}

	public bool ApplyDeathmarkEffect()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.ApplyDeathmarkEffect()).MethodHandle;
			}
			result = this.m_abilityMod.m_applyDeathmarkEffectMod.GetModifiedValue(this.m_applyDeathmarkEffect);
		}
		else
		{
			result = this.m_applyDeathmarkEffect;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamageAmount());
		AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetSelfHealOnMarkedHit());
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (this.m_targetingMode == Ninja360Attack.TargetingMode.Cone)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
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
				int damage = this.GetDamageAmount();
				if (this.GetInnerAreaDamage() > 0 && this.GetInnerConeAngle() > 0f)
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
					ActorData actorData = base.ActorData;
					Vector3 lastUpdateAimDir = base.Targeter.LastUpdateAimDir;
					float coneForwardAngle = VectorUtils.HorizontalAngle_Deg(lastUpdateAimDir);
					bool flag = this.IsActorInInnerCone(targetActor, actorData, coneForwardAngle);
					if (flag)
					{
						damage = this.GetInnerAreaDamage();
					}
				}
				results.m_damage = damage;
				return true;
			}
			if (targetActor == base.ActorData && this.GetSelfHealOnMarkedHit() > 0)
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
				int num = 0;
				List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
				for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
				{
					ActorData targetActor2 = visibleActorsInRangeByTooltipSubject[i];
					if (this.IsActorMarked(targetActor2))
					{
						num += this.GetSelfHealOnMarkedHit();
					}
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
				results.m_healing = num;
				return true;
			}
		}
		return false;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.GetEnergyGainOnMarkedHit() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			int num = 0;
			List<ActorData> visibleActorsInRangeByTooltipSubject = base.Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
			for (int i = 0; i < visibleActorsInRangeByTooltipSubject.Count; i++)
			{
				ActorData targetActor = visibleActorsInRangeByTooltipSubject[i];
				if (this.IsActorMarked(targetActor))
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
					num += this.GetEnergyGainOnMarkedHit();
				}
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			return num;
		}
		return 0;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.GetAccessoryTargeterNumberString(ActorData, AbilityTooltipSymbol, int)).MethodHandle;
			}
			if (this.m_syncComp != null)
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
				if (this.m_syncComp.m_deathmarkOnTriggerDamage > 0)
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
					if (this.IsActorMarked(targetActor))
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
						return "\n+ " + AbilityUtils.CalculateDamageForTargeter(base.ActorData, targetActor, this, this.m_syncComp.m_deathmarkOnTriggerDamage, false).ToString();
					}
				}
			}
		}
		return null;
	}

	public bool IsActorInInnerCone(ActorData targetActor, ActorData caster, float coneForwardAngle)
	{
		if (this.m_targetingMode == Ninja360Attack.TargetingMode.Cone)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.IsActorInInnerCone(ActorData, ActorData, float)).MethodHandle;
			}
			if (this.GetInnerConeAngle() > 0f)
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
				ConeTargetingInfo coneInfo = this.GetConeInfo();
				return AreaEffectUtils.IsSquareInConeByActorRadius(targetActor.GetCurrentBoardSquare(), caster.GetTravelBoardSquareWorldPosition(), coneForwardAngle, this.GetInnerConeAngle(), coneInfo.m_radiusInSquares, coneInfo.m_backwardsOffset, this.PenetrateLineOfSight(), caster, false, default(Vector3));
			}
		}
		return false;
	}

	public bool IsActorMarked(ActorData targetActor)
	{
		bool result;
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Ninja360Attack.IsActorMarked(ActorData)).MethodHandle;
			}
			result = this.m_syncComp.ActorHasDeathmark(targetActor);
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
		base.AddTokenInt(tokens, "InnerAreaDamage", string.Empty, this.m_innerAreaDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyHitEffect", this.m_enemyHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_innerConeEnemyHitEffect, "InnerConeEnemyHitEffect", this.m_innerConeEnemyHitEffect, true);
		base.AddTokenInt(tokens, "EnergyGainOnMarkedHit", string.Empty, this.m_energyGainOnMarkedHit, false);
		base.AddTokenInt(tokens, "SelfHealOnMarkedHit", string.Empty, this.m_selfHealOnMarkedHit, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_Ninja360Attack))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_Ninja360Attack);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}

	public enum TargetingMode
	{
		Shape,
		Cone,
		Laser
	}
}
