using System;
using System.Collections.Generic;
using UnityEngine;

public class NinjaDarts : Ability
{
	[Separator("Targeting Properties", true)]
	public LaserTargetingInfo m_laserInfo;

	[Space(10f)]
	public int m_laserCount = 3;

	public float m_angleInBetween = 10f;

	public bool m_changeAngleByCursorDistance = true;

	public float m_targeterMinAngle;

	public float m_targeterMaxAngle = 180f;

	public float m_targeterMinInterpDistance = 0.5f;

	public float m_targeterMaxInterpDistance = 4f;

	[Separator("On Hit Stuff", true)]
	public int m_damage = 0xA;

	public int m_extraDamagePerSubseqHit;

	[Space(10f)]
	public StandardEffectInfo m_enemySingleHitEffect;

	public StandardEffectInfo m_enemyMultiHitEffect;

	[Header("-- For effect when hitting over certain number of lasers --")]
	public int m_enemyExtraEffectHitCount;

	public StandardEffectInfo m_enemyExtraHitEffectForHitCount;

	[Header("-- For Ally Hit --")]
	public StandardEffectInfo m_allySingleHitEffect;

	public StandardEffectInfo m_allyMultiHitEffect;

	[Separator("Energy per dart hit", true)]
	public int m_energyPerDartHit;

	[Separator("Cooldown Reduction", true)]
	public int m_cdrOnMiss;

	[Separator("[Deathmark] Effect", "magenta")]
	public bool m_applyDeathmarkEffect = true;

	public bool m_ignoreCoverOnTargets;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_NinjaDarts m_abilityMod;

	private Ninja_SyncComponent m_syncComp;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardEffectInfo m_cachedEnemySingleHitEffect;

	private StandardEffectInfo m_cachedEnemyMultiHitEffect;

	private StandardEffectInfo m_cachedEnemyExtraHitEffectForHitCount;

	private StandardEffectInfo m_cachedAllySingleHitEffect;

	private StandardEffectInfo m_cachedAllyMultiHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.Start()).MethodHandle;
			}
			this.m_abilityName = "NinjaDarts";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		if (this.m_syncComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.SetupTargeter()).MethodHandle;
			}
			this.m_syncComp = base.GetComponent<Ninja_SyncComponent>();
		}
		AbilityUtil_Targeter abilityUtil_Targeter = AbilityCommon_FanLaser.CreateTargeter_SingleClick(this, this.GetLaserCount(), this.GetLaserInfo(), this.GetAngleInBetween(), this.ChangeAngleByCursorDistance(), this.GetTargeterMinAngle(), this.GetTargeterMaxAngle(), this.GetTargeterMinInterpDistance(), this.GetTargeterMaxInterpDistance());
		if (this.ChangeAngleByCursorDistance())
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
			if (abilityUtil_Targeter is AbilityUtil_Targeter_ThiefFanLaser)
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
				AbilityUtil_Targeter_ThiefFanLaser abilityUtil_Targeter_ThiefFanLaser = abilityUtil_Targeter as AbilityUtil_Targeter_ThiefFanLaser;
				abilityUtil_Targeter_ThiefFanLaser.m_customDamageOriginDelegate = new AbilityUtil_Targeter_ThiefFanLaser.CustomDamageOriginDelegate(this.GetCustomDamageOriginForTargeter);
			}
		}
		base.Targeter = abilityUtil_Targeter;
	}

	private Vector3 GetCustomDamageOriginForTargeter(ActorData potentialActor, ActorData caster, Vector3 defaultPos)
	{
		if (this.IgnoreCoverOnTargets())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetCustomDamageOriginForTargeter(ActorData, ActorData, Vector3)).MethodHandle;
			}
			if (this.ActorIsMarked(potentialActor))
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
				return potentialActor.GetTravelBoardSquareWorldPosition();
			}
		}
		return defaultPos;
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Design --</color>\nPlease edit [Deathmark] info on Ninja sync component.";
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserInfo().range;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "LaserCount", string.Empty, this.m_laserCount, false);
		base.AddTokenInt(tokens, "Damage", string.Empty, this.m_damage, false);
		base.AddTokenInt(tokens, "ExtraDamagePerSubseqHit", string.Empty, this.m_extraDamagePerSubseqHit, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemySingleHitEffect, "EnemySingleHitEffect", this.m_enemySingleHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyMultiHitEffect, "EnemyMultiHitEffect", this.m_enemyMultiHitEffect, true);
		base.AddTokenInt(tokens, "EnemyExtraEffectHitCount", string.Empty, this.m_enemyExtraEffectHitCount, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyExtraHitEffectForHitCount, "EnemyExtraHitEffectForHitCount", this.m_enemyExtraHitEffectForHitCount, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allySingleHitEffect, "AllySingleHitEffect", this.m_allySingleHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_allyMultiHitEffect, "AllyMultiHitEffect", this.m_allyMultiHitEffect, true);
		base.AddTokenInt(tokens, "EnergyPerDartHit", string.Empty, this.m_energyPerDartHit, false);
		base.AddTokenInt(tokens, "CdrOnMiss", string.Empty, this.m_cdrOnMiss, false);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.SetCachedFields()).MethodHandle;
			}
			cachedLaserInfo = this.m_abilityMod.m_laserInfoMod.GetModifiedValue(this.m_laserInfo);
		}
		else
		{
			cachedLaserInfo = this.m_laserInfo;
		}
		this.m_cachedLaserInfo = cachedLaserInfo;
		StandardEffectInfo cachedEnemySingleHitEffect;
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
			cachedEnemySingleHitEffect = this.m_abilityMod.m_enemySingleHitEffectMod.GetModifiedValue(this.m_enemySingleHitEffect);
		}
		else
		{
			cachedEnemySingleHitEffect = this.m_enemySingleHitEffect;
		}
		this.m_cachedEnemySingleHitEffect = cachedEnemySingleHitEffect;
		StandardEffectInfo cachedEnemyMultiHitEffect;
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
			cachedEnemyMultiHitEffect = this.m_abilityMod.m_enemyMultiHitEffectMod.GetModifiedValue(this.m_enemyMultiHitEffect);
		}
		else
		{
			cachedEnemyMultiHitEffect = this.m_enemyMultiHitEffect;
		}
		this.m_cachedEnemyMultiHitEffect = cachedEnemyMultiHitEffect;
		StandardEffectInfo cachedEnemyExtraHitEffectForHitCount;
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
			cachedEnemyExtraHitEffectForHitCount = this.m_abilityMod.m_enemyExtraHitEffectForHitCountMod.GetModifiedValue(this.m_enemyExtraHitEffectForHitCount);
		}
		else
		{
			cachedEnemyExtraHitEffectForHitCount = this.m_enemyExtraHitEffectForHitCount;
		}
		this.m_cachedEnemyExtraHitEffectForHitCount = cachedEnemyExtraHitEffectForHitCount;
		StandardEffectInfo cachedAllySingleHitEffect;
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
			cachedAllySingleHitEffect = this.m_abilityMod.m_allySingleHitEffectMod.GetModifiedValue(this.m_allySingleHitEffect);
		}
		else
		{
			cachedAllySingleHitEffect = this.m_allySingleHitEffect;
		}
		this.m_cachedAllySingleHitEffect = cachedAllySingleHitEffect;
		this.m_cachedAllyMultiHitEffect = ((!this.m_abilityMod) ? this.m_allyMultiHitEffect : this.m_abilityMod.m_allyMultiHitEffectMod.GetModifiedValue(this.m_allyMultiHitEffect));
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedLaserInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetLaserInfo()).MethodHandle;
			}
			result = this.m_cachedLaserInfo;
		}
		else
		{
			result = this.m_laserInfo;
		}
		return result;
	}

	public int GetLaserCount()
	{
		int num;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetLaserCount()).MethodHandle;
			}
			num = this.m_abilityMod.m_laserCountMod.GetModifiedValue(this.m_laserCount);
		}
		else
		{
			num = this.m_laserCount;
		}
		int b = num;
		return Mathf.Max(1, b);
	}

	public float GetAngleInBetween()
	{
		return (!this.m_abilityMod) ? this.m_angleInBetween : this.m_abilityMod.m_angleInBetweenMod.GetModifiedValue(this.m_angleInBetween);
	}

	public bool ChangeAngleByCursorDistance()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.ChangeAngleByCursorDistance()).MethodHandle;
			}
			result = this.m_abilityMod.m_changeAngleByCursorDistanceMod.GetModifiedValue(this.m_changeAngleByCursorDistance);
		}
		else
		{
			result = this.m_changeAngleByCursorDistance;
		}
		return result;
	}

	public float GetTargeterMinAngle()
	{
		float num;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetTargeterMinAngle()).MethodHandle;
			}
			num = this.m_abilityMod.m_targeterMinAngleMod.GetModifiedValue(this.m_targeterMinAngle);
		}
		else
		{
			num = this.m_targeterMinAngle;
		}
		float a = num;
		return Mathf.Max(a, 0f);
	}

	public float GetTargeterMaxAngle()
	{
		float b = (!this.m_abilityMod) ? this.m_targeterMaxAngle : this.m_abilityMod.m_targeterMaxAngleMod.GetModifiedValue(this.m_targeterMaxAngle);
		return Mathf.Max(1f, b);
	}

	public float GetTargeterMinInterpDistance()
	{
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetTargeterMinInterpDistance()).MethodHandle;
			}
			result = this.m_abilityMod.m_targeterMinInterpDistanceMod.GetModifiedValue(this.m_targeterMinInterpDistance);
		}
		else
		{
			result = this.m_targeterMinInterpDistance;
		}
		return result;
	}

	public float GetTargeterMaxInterpDistance()
	{
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetTargeterMaxInterpDistance()).MethodHandle;
			}
			result = this.m_abilityMod.m_targeterMaxInterpDistanceMod.GetModifiedValue(this.m_targeterMaxInterpDistance);
		}
		else
		{
			result = this.m_targeterMaxInterpDistance;
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
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetDamage()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damage);
		}
		else
		{
			result = this.m_damage;
		}
		return result;
	}

	public int GetExtraDamagePerSubseqHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetExtraDamagePerSubseqHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamagePerSubseqHitMod.GetModifiedValue(this.m_extraDamagePerSubseqHit);
		}
		else
		{
			result = this.m_extraDamagePerSubseqHit;
		}
		return result;
	}

	public StandardEffectInfo GetEnemySingleHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemySingleHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetEnemySingleHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemySingleHitEffect;
		}
		else
		{
			result = this.m_enemySingleHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyMultiHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetEnemyMultiHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyMultiHitEffect;
		}
		else
		{
			result = this.m_enemyMultiHitEffect;
		}
		return result;
	}

	public int GetEnemyExtraEffectHitCount()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetEnemyExtraEffectHitCount()).MethodHandle;
			}
			result = this.m_abilityMod.m_enemyExtraEffectHitCountMod.GetModifiedValue(this.m_enemyExtraEffectHitCount);
		}
		else
		{
			result = this.m_enemyExtraEffectHitCount;
		}
		return result;
	}

	public StandardEffectInfo GetEnemyExtraHitEffectForHitCount()
	{
		return (this.m_cachedEnemyExtraHitEffectForHitCount == null) ? this.m_enemyExtraHitEffectForHitCount : this.m_cachedEnemyExtraHitEffectForHitCount;
	}

	public StandardEffectInfo GetAllySingleHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllySingleHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetAllySingleHitEffect()).MethodHandle;
			}
			result = this.m_cachedAllySingleHitEffect;
		}
		else
		{
			result = this.m_allySingleHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAllyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyMultiHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetAllyMultiHitEffect()).MethodHandle;
			}
			result = this.m_cachedAllyMultiHitEffect;
		}
		else
		{
			result = this.m_allyMultiHitEffect;
		}
		return result;
	}

	public int GetEnergyPerDartHit()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetEnergyPerDartHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_energyPerDartHitMod.GetModifiedValue(this.m_energyPerDartHit);
		}
		else
		{
			result = this.m_energyPerDartHit;
		}
		return result;
	}

	public int GetCdrOnMiss()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetCdrOnMiss()).MethodHandle;
			}
			result = this.m_abilityMod.m_cdrOnMissMod.GetModifiedValue(this.m_cdrOnMiss);
		}
		else
		{
			result = this.m_cdrOnMiss;
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
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.ApplyDeathmarkEffect()).MethodHandle;
			}
			result = this.m_abilityMod.m_applyDeathmarkEffectMod.GetModifiedValue(this.m_applyDeathmarkEffect);
		}
		else
		{
			result = this.m_applyDeathmarkEffect;
		}
		return result;
	}

	public bool IgnoreCoverOnTargets()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.IgnoreCoverOnTargets()).MethodHandle;
			}
			result = this.m_abilityMod.m_ignoreCoverOnTargetsMod.GetModifiedValue(this.m_ignoreCoverOnTargets);
		}
		else
		{
			result = this.m_ignoreCoverOnTargets;
		}
		return result;
	}

	public int CalcDamageFromNumHits(int numHits)
	{
		if (numHits > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.CalcDamageFromNumHits(int)).MethodHandle;
			}
			int num = this.GetDamage();
			if (this.GetExtraDamagePerSubseqHit() > 0 && numHits > 1)
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
				num += this.GetExtraDamagePerSubseqHit() * (numHits - 1);
			}
			return num;
		}
		return 0;
	}

	public bool ActorIsMarked(ActorData actor)
	{
		bool result;
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.ActorIsMarked(ActorData)).MethodHandle;
			}
			result = this.m_syncComp.ActorHasDeathmark(actor);
		}
		else
		{
			result = false;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDamage());
		this.GetEnemySingleHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		this.GetEnemyMultiHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		int tooltipSubjectCountOnActor = base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary);
		int damage = this.CalcDamageFromNumHits(tooltipSubjectCountOnActor);
		results.m_damage = damage;
		return true;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.GetEnergyPerDartHit() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			int tooltipSubjectCountTotalWithDuplicates = base.Targeter.GetTooltipSubjectCountTotalWithDuplicates(AbilityTooltipSubject.Primary);
			return tooltipSubjectCountTotalWithDuplicates * this.GetEnergyPerDartHit();
		}
		return 0;
	}

	public override string GetAccessoryTargeterNumberString(ActorData targetActor, AbilityTooltipSymbol symbolType, int baseValue)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.GetAccessoryTargeterNumberString(ActorData, AbilityTooltipSymbol, int)).MethodHandle;
			}
			if (this.m_syncComp != null)
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
				if (this.m_syncComp.m_deathmarkOnTriggerDamage > 0 && this.m_syncComp.ActorHasDeathmark(targetActor))
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
					return "\n+ " + AbilityUtils.CalculateDamageForTargeter(base.ActorData, targetActor, this, this.m_syncComp.m_deathmarkOnTriggerDamage, false).ToString();
				}
			}
		}
		return null;
	}

	public float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees)
	{
		return AbilityCommon_FanLaser.CalculateDistanceFromFanAngleDegrees(fanAngleDegrees, this.GetTargeterMaxAngle(), this.GetTargeterMinInterpDistance(), this.GetTargeterMaxInterpDistance());
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = this.GetTargeterMinInterpDistance() * Board.Get().squareSize;
		max = this.GetTargeterMaxInterpDistance() * Board.Get().squareSize;
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NinjaDarts))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NinjaDarts.OnApplyAbilityMod(AbilityMod)).MethodHandle;
			}
			this.m_abilityMod = (abilityMod as AbilityMod_NinjaDarts);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
