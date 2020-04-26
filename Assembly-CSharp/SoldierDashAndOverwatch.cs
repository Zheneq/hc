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
	public int m_coneDamage = 20;

	public int m_laserDamage = 34;

	public StandardEffectInfo m_overwatchHitEffect;

	public float m_nearDistThreshold;

	public int m_extraDamageForNearTargets;

	[Header("-- Extra Energy (per target hit)")]
	public int m_extraEnergyForCone;

	public int m_extraEnergyForLaser;

	public AbilityPriority m_hitPhase = AbilityPriority.Combat_Damage;

	[Separator("Overwatch Anim Indices (for animation in Combat phase)", true)]
	public int m_overwatchConeTriggerAnim = 11;

	public int m_overwatchLaserTriggerAnim = 12;

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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Dash And Overwatch";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_abilityData == null)
		{
			m_abilityData = GetComponent<AbilityData>();
		}
		if (m_abilityData != null)
		{
			m_stimAbility = (GetAbilityOfType(typeof(SoldierStimPack)) as SoldierStimPack);
			m_primaryAbility = (GetAbilityOfType(typeof(SoldierConeOrLaser)) as SoldierConeOrLaser);
		}
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Soldier_SyncComponent>();
		}
		SetCachedFields();
		ClearTargeters();
		AbilityUtil_Targeter_ChargeAoE abilityUtil_Targeter_ChargeAoE = new AbilityUtil_Targeter_ChargeAoE(this, 0f, GetOnCastAllyHitRadiusAroundDest(), 0f, -1, false, false);
		abilityUtil_Targeter_ChargeAoE.SetAffectedGroups(false, true, false);
		abilityUtil_Targeter_ChargeAoE.ForceAddTargetingActor = GetSelfHitEffect().m_applyEffect;
		
		abilityUtil_Targeter_ChargeAoE.m_shouldAddTargetDelegate = ((ActorData actorToConsider, AbilityTarget abilityTarget, List<ActorData> hitActors, ActorData caster, Ability ability) => actorToConsider != caster);
		base.Targeters.Add(abilityUtil_Targeter_ChargeAoE);
		AbilityUtil_Targeter_ConeOrLaser abilityUtil_Targeter_ConeOrLaser = new AbilityUtil_Targeter_ConeOrLaser(this, GetConeInfo(), GetLaserInfo(), m_coneDistThreshold);
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
		if ((bool)m_abilityMod)
		{
			cachedConeInfo = m_abilityMod.m_coneInfoMod.GetModifiedValue(m_coneInfo);
		}
		else
		{
			cachedConeInfo = m_coneInfo;
		}
		m_cachedConeInfo = cachedConeInfo;
		m_cachedLaserInfo = ((!m_abilityMod) ? m_laserInfo : m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo));
		StandardEffectInfo cachedSelfHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedSelfHitEffect = m_abilityMod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect);
		}
		else
		{
			cachedSelfHitEffect = m_selfHitEffect;
		}
		m_cachedSelfHitEffect = cachedSelfHitEffect;
		StandardEffectInfo cachedOverwatchHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedOverwatchHitEffect = m_abilityMod.m_overwatchHitEffectMod.GetModifiedValue(m_overwatchHitEffect);
		}
		else
		{
			cachedOverwatchHitEffect = m_overwatchHitEffect;
		}
		m_cachedOverwatchHitEffect = cachedOverwatchHitEffect;
		m_cachedOnCastAllyHitEffect = ((!m_abilityMod) ? m_onCastAllyHitEffect : m_abilityMod.m_onCastAllyHitEffectMod.GetModifiedValue(m_onCastAllyHitEffect));
	}

	public bool OnlyDashNextToCover()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_onlyDashNextToCoverMod.GetModifiedValue(m_onlyDashNextToCover);
		}
		else
		{
			result = m_onlyDashNextToCover;
		}
		return result;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return (m_cachedConeInfo == null) ? m_coneInfo : m_cachedConeInfo;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedLaserInfo != null)
		{
			result = m_cachedLaserInfo;
		}
		else
		{
			result = m_laserInfo;
		}
		return result;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedSelfHitEffect != null)
		{
			result = m_cachedSelfHitEffect;
		}
		else
		{
			result = m_selfHitEffect;
		}
		return result;
	}

	public float GetOnCastAllyHitRadiusAroundDest()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_onCastAllyHitRadiusAroundDestMod.GetModifiedValue(m_onCastAllyHitRadiusAroundDest);
		}
		else
		{
			result = m_onCastAllyHitRadiusAroundDest;
		}
		return result;
	}

	public StandardEffectInfo GetOnCastAllyHitEffect()
	{
		return (m_cachedOnCastAllyHitEffect == null) ? m_onCastAllyHitEffect : m_cachedOnCastAllyHitEffect;
	}

	public int GetConeDamage()
	{
		if (m_primaryAbility != null && m_primaryAbility.HasConeDamageMod())
		{
			return m_primaryAbility.m_abilityMod.m_coneDamageMod.GetModifiedValue(m_coneDamage);
		}
		return (!m_abilityMod) ? m_coneDamage : m_abilityMod.m_overwatchDamageMod.GetModifiedValue(m_coneDamage);
	}

	public int GetLaserDamage()
	{
		if (m_primaryAbility != null && m_primaryAbility.HasLaserDamageMod())
		{
			return m_primaryAbility.m_abilityMod.m_laserDamageMod.GetModifiedValue(m_laserDamage);
		}
		return m_laserDamage;
	}

	public StandardEffectInfo GetOverwatchHitEffect()
	{
		return (m_cachedOverwatchHitEffect == null) ? m_overwatchHitEffect : m_cachedOverwatchHitEffect;
	}

	public float GetNearDistThreshold()
	{
		if (m_primaryAbility != null)
		{
			if (m_primaryAbility.HasNearDistThresholdMod())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return m_primaryAbility.m_abilityMod.m_closeDistThresholdMod.GetModifiedValue(m_nearDistThreshold);
					}
				}
			}
		}
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_nearDistThresholdMod.GetModifiedValue(m_nearDistThreshold);
		}
		else
		{
			result = m_nearDistThreshold;
		}
		return result;
	}

	public int GetExtraDamageForNearTargets()
	{
		if (m_primaryAbility != null)
		{
			if (m_primaryAbility.HasExtraDamageForNearTargetMod())
			{
				return m_primaryAbility.m_abilityMod.m_extraDamageForNearTargetMod.GetModifiedValue(m_extraDamageForNearTargets);
			}
		}
		return (!m_abilityMod) ? m_extraDamageForNearTargets : m_abilityMod.m_extraDamageForNearTargetsMod.GetModifiedValue(m_extraDamageForNearTargets);
	}

	public int GetExtraDamageToEvaders()
	{
		if (m_primaryAbility != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.GetExtraDamageToEvaders();
				}
			}
		}
		return 0;
	}

	public int GetExtraDamageForAlternating()
	{
		if (m_primaryAbility != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.GetExtraDamageForAlternating();
				}
			}
		}
		return 0;
	}

	public int GetExtraEnergyForCone()
	{
		if (m_primaryAbility != null && m_primaryAbility.HasConeEnergyMod())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.m_abilityMod.m_extraEnergyForConeMod.GetModifiedValue(m_extraEnergyForCone);
				}
			}
		}
		return m_extraEnergyForCone;
	}

	public int GetExtraEnergyForLaser()
	{
		if (m_primaryAbility != null)
		{
			if (m_primaryAbility.HasLaserEnergyMod())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return m_primaryAbility.m_abilityMod.m_extraEnergyForLaserMod.GetModifiedValue(m_extraEnergyForLaser);
					}
				}
			}
		}
		return m_extraEnergyForLaser;
	}

	public int GetExtraDamageForFromCover()
	{
		if (m_primaryAbility != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_primaryAbility.GetExtraDamageForFromCover();
				}
			}
		}
		return 0;
	}

	public override bool ForceIgnoreCover(ActorData targetActor)
	{
		if (m_abilityData != null && m_stimAbility != null)
		{
			if (m_stimAbility.BasicAttackIgnoreCover())
			{
				return m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack));
			}
		}
		return false;
	}

	public override bool ForceReduceCoverEffectiveness(ActorData targetActor)
	{
		if (m_abilityData != null)
		{
			if (m_stimAbility != null)
			{
				if (m_stimAbility.BasicAttackReduceCoverEffectiveness())
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return m_abilityData.HasQueuedAbilityOfType(typeof(SoldierStimPack));
						}
					}
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
		return magnitude <= m_coneDistThreshold;
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			min = m_coneDistThreshold - 0.1f;
			max = m_coneDistThreshold + 0.1f;
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
				switch (6)
				{
				case 0:
					break;
				default:
				{
					BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(targetsSoFar[0].GridPos);
					overridePos = boardSquareSafe.GetWorldPosition();
					return true;
				}
				}
			}
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetConeDamage());
		GetSelfHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		GetOnCastAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		ActorData actorData = base.ActorData;
		if (actorData != null)
		{
			if (currentTargeterIndex == 1)
			{
				if (currentTargeterIndex < base.Targeters.Count)
				{
					AbilityUtil_Targeter abilityUtil_Targeter = base.Targeters[0];
					AbilityUtil_Targeter abilityUtil_Targeter2 = base.Targeters[currentTargeterIndex];
					BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(abilityUtil_Targeter.LastUpdatingGridPos);
					if (abilityUtil_Targeter2.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
							{
								int num = 0;
								if (abilityUtil_Targeter2.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary) > 0)
								{
									num = GetConeDamage();
									if (GetExtraDamageForAlternating() > 0)
									{
										if ((bool)m_syncComp)
										{
											if (m_syncComp.m_lastPrimaryUsedMode == 2)
											{
												num += GetExtraDamageForAlternating();
											}
										}
									}
								}
								else
								{
									num = GetLaserDamage();
									if (GetExtraDamageForAlternating() > 0)
									{
										if ((bool)m_syncComp)
										{
											if (m_syncComp.m_lastPrimaryUsedMode == 1)
											{
												num += GetExtraDamageForAlternating();
											}
										}
									}
								}
								if (GetExtraDamageForNearTargets() > 0)
								{
									if (GetNearDistThreshold() > 0f)
									{
										Vector3 vector = boardSquareSafe.ToVector3() - targetActor.GetTravelBoardSquareWorldPosition();
										vector.y = 0f;
										if (vector.magnitude <= GetNearDistThreshold() * Board.Get().squareSize)
										{
											num += GetExtraDamageForNearTargets();
										}
									}
								}
								if (GetExtraDamageForFromCover() > 0)
								{
									if (OnlyDashNextToCover())
									{
										num += GetExtraDamageForFromCover();
									}
								}
								results.m_damage = num;
								return true;
							}
							}
						}
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
			if (base.Targeters.Count > 1)
			{
				if (GetExtraEnergyForCone() <= 0)
				{
					if (GetExtraEnergyForLaser() <= 0)
					{
						goto IL_00b5;
					}
				}
				AbilityUtil_Targeter abilityUtil_Targeter = base.Targeters[1];
				if (abilityUtil_Targeter is AbilityUtil_Targeter_ConeOrLaser)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
						{
							AbilityUtil_Targeter_ConeOrLaser abilityUtil_Targeter_ConeOrLaser = abilityUtil_Targeter as AbilityUtil_Targeter_ConeOrLaser;
							int visibleActorsCountByTooltipSubject = abilityUtil_Targeter_ConeOrLaser.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
							if (abilityUtil_Targeter_ConeOrLaser.m_updatingWithCone)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										return visibleActorsCountByTooltipSubject * GetExtraEnergyForCone();
									}
								}
							}
							return visibleActorsCountByTooltipSubject * GetExtraEnergyForLaser();
						}
						}
					}
				}
			}
		}
		goto IL_00b5;
		IL_00b5:
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, m_selfHitEffect, "SelfHitEffect", m_selfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_onCastAllyHitEffect, "OnCastAllyHitEffect", m_onCastAllyHitEffect);
		AddTokenInt(tokens, "ExtraDamageForNearTargets", string.Empty, m_extraDamageForNearTargets);
		AddTokenInt(tokens, "OverwatchDamage", string.Empty, m_coneDamage);
		AddTokenInt(tokens, "LaserDamage", string.Empty, m_laserDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_overwatchHitEffect, "OverwatchHitEffect", m_overwatchHitEffect);
		AddTokenInt(tokens, "ExtraEnergyForCone", string.Empty, m_extraEnergyForCone);
		AddTokenInt(tokens, "ExtraEnergyForLaser", string.Empty, m_extraEnergyForLaser);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex == 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
					if (boardSquareSafe != null && boardSquareSafe.IsBaselineHeight())
					{
						if (boardSquareSafe != caster.GetCurrentBoardSquare())
						{
							bool flag = !OnlyDashNextToCover();
							if (OnlyDashNextToCover())
							{
								ActorCover.CalcCoverLevelGeoOnly(out bool[] hasCover, boardSquareSafe);
								for (int i = 0; i < hasCover.Length; i++)
								{
									if (hasCover[i])
									{
										flag = true;
										break;
									}
								}
							}
							if (flag)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										break;
									default:
									{
										int numSquaresInPath;
										return KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out numSquaresInPath);
									}
									}
								}
							}
						}
					}
					return false;
				}
				}
			}
		}
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SoldierDashAndOverwatch))
		{
			m_abilityMod = (abilityMod as AbilityMod_SoldierDashAndOverwatch);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
