using System;
using System.Collections.Generic;
using UnityEngine;

public class SoldierDashAndOverwatch : Ability
{
	[Separator("Targeting", true)]
	public bool m_onlyDashNextToCover = true;

	public float m_coneDistThreshold = 4f;

	[Header("  Targeting: For Cone")]
	public ConeTargetingInfo m_coneInfo;

	[Header("  Targeting: For Laser")]
	public LaserTargetingInfo m_laserInfo;

	[Separator("On Cast Hit", true)]
	public StandardEffectInfo m_selfHitEffect;

	public float m_onCastAllyHitRadiusAroundDest;

	public StandardEffectInfo m_onCastAllyHitEffect;

	[Separator("On Overwatch Hit", true)]
	public int m_coneDamage = 0x14;

	public int m_laserDamage = 0x22;

	public StandardEffectInfo m_overwatchHitEffect;

	public float m_nearDistThreshold;

	public int m_extraDamageForNearTargets;

	[Header("-- Extra Energy (per target hit)")]
	public int m_extraEnergyForCone;

	public int m_extraEnergyForLaser;

	public AbilityPriority m_hitPhase = AbilityPriority.Combat_Damage;

	[Separator("Overwatch Anim Indices (for animation in Combat phase)", true)]
	public int m_overwatchConeTriggerAnim = 0xB;

	public int m_overwatchLaserTriggerAnim = 0xC;

	[Separator("Sequences: for dash part, on self", true)]
	public GameObject m_castSequencePrefab;

	[Header("-- Sequences: for shooting in combat, assuming similar to basic attack cone")]
	public GameObject m_overwatchConeSequencePrefab;

	public GameObject m_overwatchLaserSequencePrefab;

	private AbilityMod_SoldierDashAndOverwatch m_abilityMod;

	private Soldier_SyncComponent m_syncComp;

	private AbilityData m_abilityData;

	private SoldierConeOrLaser m_primaryAbility;

	private SoldierStimPack m_stimAbility;

	private ConeTargetingInfo m_cachedConeInfo;

	private LaserTargetingInfo m_cachedLaserInfo;

	private StandardEffectInfo m_cachedSelfHitEffect;

	private StandardEffectInfo m_cachedOverwatchHitEffect;

	private StandardEffectInfo m_cachedOnCastAllyHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Dash And Overwatch";
		}
		this.Setup();
	}

	private void Setup()
	{
		if (this.m_abilityData == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.Setup()).MethodHandle;
			}
			this.m_abilityData = base.GetComponent<AbilityData>();
		}
		if (this.m_abilityData != null)
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
			this.m_stimAbility = (base.GetAbilityOfType(typeof(SoldierStimPack)) as SoldierStimPack);
			this.m_primaryAbility = (base.GetAbilityOfType(typeof(SoldierConeOrLaser)) as SoldierConeOrLaser);
		}
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
			this.m_syncComp = base.GetComponent<Soldier_SyncComponent>();
		}
		this.SetCachedFields();
		base.ClearTargeters();
		AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, 0f, this.GetOnCastAllyHitRadiusAroundDest(), 0f, -1, false, false);
		abilityUtil_Targeter_ChargeAoE.SetAffectedGroups(false, true, false);
		abilityUtil_Targeter_ChargeAoE.ForceAddTargetingActor = this.GetSelfHitEffect().m_applyEffect;
		AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE2 = abilityUtil_Targeter_ChargeAoE;
		if (SoldierDashAndOverwatch.<>f__am$cache0 == null)
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
			SoldierDashAndOverwatch.<>f__am$cache0 = ((ActorData actorToConsider, AbilityTarget abilityTarget, List<ActorData> hitActors, ActorData caster, Ability ability) => actorToConsider != caster);
		}
		abilityUtil_Targeter_ChargeAoE2.m_shouldAddTargetDelegate = SoldierDashAndOverwatch.<>f__am$cache0;
		base.Targeters.Add(abilityUtil_Targeter_ChargeAoE);
		AbilityUtil_Targeter_ConeOrLaser abilityUtil_Targeter_ConeOrLaser = new AbilityUtil_Targeter_ConeOrLaser(this, this.GetConeInfo(), this.GetLaserInfo(), this.m_coneDistThreshold);
		abilityUtil_Targeter_ConeOrLaser.SetUseMultiTargetUpdate(true);
		base.Targeters.Add(abilityUtil_Targeter_ConeOrLaser);
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	private void SetCachedFields()
	{
		ConeTargetingInfo cachedConeInfo;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.SetCachedFields()).MethodHandle;
			}
			cachedConeInfo = this.m_abilityMod.m_coneInfoMod.GetModifiedValue(this.m_coneInfo);
		}
		else
		{
			cachedConeInfo = this.m_coneInfo;
		}
		this.m_cachedConeInfo = cachedConeInfo;
		this.m_cachedLaserInfo = ((!this.m_abilityMod) ? this.m_laserInfo : this.m_abilityMod.m_laserInfoMod.GetModifiedValue(this.m_laserInfo));
		StandardEffectInfo cachedSelfHitEffect;
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
			cachedSelfHitEffect = this.m_abilityMod.m_selfHitEffectMod.GetModifiedValue(this.m_selfHitEffect);
		}
		else
		{
			cachedSelfHitEffect = this.m_selfHitEffect;
		}
		this.m_cachedSelfHitEffect = cachedSelfHitEffect;
		StandardEffectInfo cachedOverwatchHitEffect;
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
			cachedOverwatchHitEffect = this.m_abilityMod.m_overwatchHitEffectMod.GetModifiedValue(this.m_overwatchHitEffect);
		}
		else
		{
			cachedOverwatchHitEffect = this.m_overwatchHitEffect;
		}
		this.m_cachedOverwatchHitEffect = cachedOverwatchHitEffect;
		this.m_cachedOnCastAllyHitEffect = ((!this.m_abilityMod) ? this.m_onCastAllyHitEffect : this.m_abilityMod.m_onCastAllyHitEffectMod.GetModifiedValue(this.m_onCastAllyHitEffect));
	}

	public bool OnlyDashNextToCover()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.OnlyDashNextToCover()).MethodHandle;
			}
			result = this.m_abilityMod.m_onlyDashNextToCoverMod.GetModifiedValue(this.m_onlyDashNextToCover);
		}
		else
		{
			result = this.m_onlyDashNextToCover;
		}
		return result;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return (this.m_cachedConeInfo == null) ? this.m_coneInfo : this.m_cachedConeInfo;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.GetLaserInfo()).MethodHandle;
			}
			result = this.m_cachedLaserInfo;
		}
		else
		{
			result = this.m_laserInfo;
		}
		return result;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedSelfHitEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.GetSelfHitEffect()).MethodHandle;
			}
			result = this.m_cachedSelfHitEffect;
		}
		else
		{
			result = this.m_selfHitEffect;
		}
		return result;
	}

	public float GetOnCastAllyHitRadiusAroundDest()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.GetOnCastAllyHitRadiusAroundDest()).MethodHandle;
			}
			result = this.m_abilityMod.m_onCastAllyHitRadiusAroundDestMod.GetModifiedValue(this.m_onCastAllyHitRadiusAroundDest);
		}
		else
		{
			result = this.m_onCastAllyHitRadiusAroundDest;
		}
		return result;
	}

	public StandardEffectInfo GetOnCastAllyHitEffect()
	{
		return (this.m_cachedOnCastAllyHitEffect == null) ? this.m_onCastAllyHitEffect : this.m_cachedOnCastAllyHitEffect;
	}

	public int GetConeDamage()
	{
		if (this.m_primaryAbility != null && this.m_primaryAbility.HasConeDamageMod())
		{
			return this.m_primaryAbility.m_abilityMod.m_coneDamageMod.GetModifiedValue(this.m_coneDamage);
		}
		return (!this.m_abilityMod) ? this.m_coneDamage : this.m_abilityMod.m_overwatchDamageMod.GetModifiedValue(this.m_coneDamage);
	}

	public int GetLaserDamage()
	{
		if (this.m_primaryAbility != null && this.m_primaryAbility.HasLaserDamageMod())
		{
			return this.m_primaryAbility.m_abilityMod.m_laserDamageMod.GetModifiedValue(this.m_laserDamage);
		}
		return this.m_laserDamage;
	}

	public StandardEffectInfo GetOverwatchHitEffect()
	{
		return (this.m_cachedOverwatchHitEffect == null) ? this.m_overwatchHitEffect : this.m_cachedOverwatchHitEffect;
	}

	public float GetNearDistThreshold()
	{
		if (this.m_primaryAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.GetNearDistThreshold()).MethodHandle;
			}
			if (this.m_primaryAbility.HasNearDistThresholdMod())
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
				return this.m_primaryAbility.m_abilityMod.m_closeDistThresholdMod.GetModifiedValue(this.m_nearDistThreshold);
			}
		}
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
			result = this.m_abilityMod.m_nearDistThresholdMod.GetModifiedValue(this.m_nearDistThreshold);
		}
		else
		{
			result = this.m_nearDistThreshold;
		}
		return result;
	}

	public int GetExtraDamageForNearTargets()
	{
		if (this.m_primaryAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.GetExtraDamageForNearTargets()).MethodHandle;
			}
			if (this.m_primaryAbility.HasExtraDamageForNearTargetMod())
			{
				return this.m_primaryAbility.m_abilityMod.m_extraDamageForNearTargetMod.GetModifiedValue(this.m_extraDamageForNearTargets);
			}
		}
		return (!this.m_abilityMod) ? this.m_extraDamageForNearTargets : this.m_abilityMod.m_extraDamageForNearTargetsMod.GetModifiedValue(this.m_extraDamageForNearTargets);
	}

	public int GetExtraDamageToEvaders()
	{
		if (this.m_primaryAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.GetExtraDamageToEvaders()).MethodHandle;
			}
			return this.m_primaryAbility.GetExtraDamageToEvaders();
		}
		return 0;
	}

	public int GetExtraDamageForAlternating()
	{
		if (this.m_primaryAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.GetExtraDamageForAlternating()).MethodHandle;
			}
			return this.m_primaryAbility.GetExtraDamageForAlternating();
		}
		return 0;
	}

	public int GetExtraEnergyForCone()
	{
		if (this.m_primaryAbility != null && this.m_primaryAbility.HasConeEnergyMod())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.GetExtraEnergyForCone()).MethodHandle;
			}
			return this.m_primaryAbility.m_abilityMod.m_extraEnergyForConeMod.GetModifiedValue(this.m_extraEnergyForCone);
		}
		return this.m_extraEnergyForCone;
	}

	public int GetExtraEnergyForLaser()
	{
		if (this.m_primaryAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.GetExtraEnergyForLaser()).MethodHandle;
			}
			if (this.m_primaryAbility.HasLaserEnergyMod())
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
				return this.m_primaryAbility.m_abilityMod.m_extraEnergyForLaserMod.GetModifiedValue(this.m_extraEnergyForLaser);
			}
		}
		return this.m_extraEnergyForLaser;
	}

	public int GetExtraDamageForFromCover()
	{
		if (this.m_primaryAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.GetExtraDamageForFromCover()).MethodHandle;
			}
			return this.m_primaryAbility.GetExtraDamageForFromCover();
		}
		return 0;
	}

	public override bool ForceIgnoreCover(ActorData targetActor)
	{
		if (this.m_abilityData != null && this.m_stimAbility != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.ForceIgnoreCover(ActorData)).MethodHandle;
			}
			if (this.m_stimAbility.BasicAttackIgnoreCover())
			{
				return this.m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack));
			}
		}
		return false;
	}

	public override bool ForceReduceCoverEffectiveness(ActorData targetActor)
	{
		if (this.m_abilityData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.ForceReduceCoverEffectiveness(ActorData)).MethodHandle;
			}
			if (this.m_stimAbility != null)
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
				if (this.m_stimAbility.BasicAttackReduceCoverEffectiveness())
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
					return this.m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack));
				}
			}
		}
		return false;
	}

	private bool ShouldUseCone(Vector3 cursorFreePos, Vector3 startPos)
	{
		Vector3 vector = cursorFreePos - startPos;
		vector.y = 0f;
		float magnitude = vector.magnitude;
		return magnitude <= this.m_coneDistThreshold;
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			min = this.m_coneDistThreshold - 0.1f;
			max = this.m_coneDistThreshold + 0.1f;
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
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.HasAimingOriginOverride(ActorData, int, List<AbilityTarget>, Vector3*)).MethodHandle;
			}
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(targetsSoFar[0].GridPos);
			overridePos = boardSquareSafe.GetWorldPosition();
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetConeDamage());
		this.GetSelfHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		this.GetOnCastAllyHitEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		return result;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		ActorData actorData = base.ActorData;
		if (actorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.GetCustomTargeterNumbers(ActorData, int, TargetingNumberUpdateScratch)).MethodHandle;
			}
			if (currentTargeterIndex == 1)
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
				if (currentTargeterIndex < base.Targeters.Count)
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
					AbilityUtil_Targeter abilityUtil_Targeter = base.Targeters[0];
					AbilityUtil_Targeter abilityUtil_Targeter2 = base.Targeters[currentTargeterIndex];
					BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(abilityUtil_Targeter.LastUpdatingGridPos);
					if (abilityUtil_Targeter2.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
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
						int num;
						if (abilityUtil_Targeter2.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
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
							num = this.GetConeDamage();
							if (this.GetExtraDamageForAlternating() > 0)
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
								if (this.m_syncComp)
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
									if ((int)this.m_syncComp.m_lastPrimaryUsedMode == 2)
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
										num += this.GetExtraDamageForAlternating();
									}
								}
							}
						}
						else
						{
							num = this.GetLaserDamage();
							if (this.GetExtraDamageForAlternating() > 0)
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
								if (this.m_syncComp)
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
									if ((int)this.m_syncComp.m_lastPrimaryUsedMode == 1)
									{
										num += this.GetExtraDamageForAlternating();
									}
								}
							}
						}
						if (this.GetExtraDamageForNearTargets() > 0)
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
							if (this.GetNearDistThreshold() > 0f)
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
								Vector3 vector = boardSquareSafe.ToVector3() - targetActor.GetTravelBoardSquareWorldPosition();
								vector.y = 0f;
								if (vector.magnitude <= this.GetNearDistThreshold() * Board.Get().squareSize)
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
									num += this.GetExtraDamageForNearTargets();
								}
							}
						}
						if (this.GetExtraDamageForFromCover() > 0)
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
							if (this.OnlyDashNextToCover())
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
								num += this.GetExtraDamageForFromCover();
							}
						}
						results.m_damage = num;
						return true;
					}
				}
			}
		}
		return false;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (currentTargeterIndex > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			if (base.Targeters.Count > 1)
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
				if (this.GetExtraEnergyForCone() <= 0)
				{
					if (this.GetExtraEnergyForLaser() <= 0)
					{
						return 0;
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				AbilityUtil_Targeter abilityUtil_Targeter = base.Targeters[1];
				if (abilityUtil_Targeter is AbilityUtil_Targeter_ConeOrLaser)
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
					AbilityUtil_Targeter_ConeOrLaser abilityUtil_Targeter_ConeOrLaser = abilityUtil_Targeter as AbilityUtil_Targeter_ConeOrLaser;
					int visibleActorsCountByTooltipSubject = abilityUtil_Targeter_ConeOrLaser.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
					if (abilityUtil_Targeter_ConeOrLaser.m_updatingWithCone)
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
						return visibleActorsCountByTooltipSubject * this.GetExtraEnergyForCone();
					}
					return visibleActorsCountByTooltipSubject * this.GetExtraEnergyForLaser();
				}
			}
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, this.m_selfHitEffect, "SelfHitEffect", this.m_selfHitEffect, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_onCastAllyHitEffect, "OnCastAllyHitEffect", this.m_onCastAllyHitEffect, true);
		base.AddTokenInt(tokens, "ExtraDamageForNearTargets", string.Empty, this.m_extraDamageForNearTargets, false);
		base.AddTokenInt(tokens, "OverwatchDamage", string.Empty, this.m_coneDamage, false);
		base.AddTokenInt(tokens, "LaserDamage", string.Empty, this.m_laserDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_overwatchHitEffect, "OverwatchHitEffect", this.m_overwatchHitEffect, true);
		base.AddTokenInt(tokens, "ExtraEnergyForCone", string.Empty, this.m_extraEnergyForCone, false);
		base.AddTokenInt(tokens, "ExtraEnergyForLaser", string.Empty, this.m_extraEnergyForLaser, false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex == 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SoldierDashAndOverwatch.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
			if (boardSquareSafe != null && boardSquareSafe.IsBaselineHeight())
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
				if (boardSquareSafe != caster.GetCurrentBoardSquare())
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
					bool flag = !this.OnlyDashNextToCover();
					if (this.OnlyDashNextToCover())
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
						bool[] array;
						ActorCover.CalcCoverLevelGeoOnly(out array, boardSquareSafe);
						for (int i = 0; i < array.Length; i++)
						{
							if (array[i])
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
								flag = true;
								break;
							}
						}
					}
					if (flag)
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
						int num;
						return KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out num);
					}
				}
			}
			return false;
		}
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SoldierDashAndOverwatch))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SoldierDashAndOverwatch);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
