using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcherDashAndShoot : Ability
{
	[Header("-- Targeting")]
	public float m_maxAngleForLaser = 30f;

	public float m_laserWidth = 0.5f;

	public float m_laserRange = 5.5f;

	public float m_aoeRadius = 1f;

	public bool m_aoePenetratesLoS;

	[Header("-- Enemy hits")]
	public int m_directDamage;

	public int m_aoeDamage;

	public StandardEffectInfo m_directTargetEffect;

	public StandardEffectInfo m_aoeTargetEffect;

	[Header("-- Sequences")]
	public GameObject m_dashSequencePrefab;

	public GameObject m_arrowProjectileSequencePrefab;

	private AbilityMod_ArcherDashAndShoot m_abilityMod;

	private ArcherShieldGeneratorArrow m_shieldGenAbility;

	private ArcherHealingDebuffArrow m_healArrowAbility;

	private AbilityData.ActionType m_healArrowActionType = AbilityData.ActionType.INVALID_ACTION;

	private ActorTargeting m_actorTargeting;

	private Archer_SyncComponent m_syncComp;

	private StandardEffectInfo m_cachedDirectTargetEffect;

	private StandardEffectInfo m_cachedAoeTargetEffect;

	private StandardEffectInfo m_cachedHealingDebuffTargetEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "ArcherDashAndShoot";
		}
		m_shieldGenAbility = (GetAbilityOfType(typeof(ArcherShieldGeneratorArrow)) as ArcherShieldGeneratorArrow);
		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			m_healArrowAbility = (GetAbilityOfType(typeof(ArcherHealingDebuffArrow)) as ArcherHealingDebuffArrow);
			if (m_healArrowAbility != null)
			{
				m_healArrowActionType = component.GetActionTypeOfAbility(m_healArrowAbility);
			}
		}
		m_actorTargeting = GetComponent<ActorTargeting>();
		m_syncComp = GetComponent<Archer_SyncComponent>();
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		base.Targeters.Clear();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_DashAndAim abilityUtil_Targeter_DashAndAim = new AbilityUtil_Targeter_DashAndAim(this, GetAoeRadius(), AoePenetratesLoS(), GetLaserWidth(), GetLaserRange(), GetMaxAngleForLaser(), GetClampedLaserDirection, true, GetMovementType() != ActorData.MovementType.Charge, 1);
			abilityUtil_Targeter_DashAndAim.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_DashAndAim);
		}
		while (true)
		{
			return;
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	internal Vector3 GetClampedLaserDirection(AbilityTarget dashTarget, AbilityTarget shootTarget, Vector3 neutralDir)
	{
		Vector3 vector = (shootTarget.FreePos - dashTarget.FreePos).normalized;
		float num = Vector3.Angle(neutralDir, vector);
		if (num > GetMaxAngleForLaser())
		{
			vector = Vector3.RotateTowards(vector, neutralDir, (num - GetMaxAngleForLaser()) * ((float)Math.PI / 180f), 0f);
		}
		return vector;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (targetIndex == 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
					if (boardSquareSafe != null && boardSquareSafe.IsValidForGameplay() && boardSquareSafe != caster.GetCurrentBoardSquare())
					{
						while (true)
						{
							switch (6)
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
					return false;
				}
				}
			}
		}
		return true;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_directDamage));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Secondary, m_aoeDamage));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (m_syncComp != null)
		{
			int num = GetDirectDamage();
			List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeters[1].GetTooltipSubjectTypes(targetActor);
			if (!tooltipSubjectTypes.IsNullOrEmpty())
			{
				if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
				{
					num = GetAoeDamage();
				}
			}
			if (IsReactionHealTarget(targetActor))
			{
				num += m_healArrowAbility.GetExtraDamageToThisTargetFromCaster();
			}
			dictionary[AbilityTooltipSymbol.Damage] = num;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[currentTargeterIndex].GetActorsInRange();
		using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityUtil_Targeter.ActorTarget current = enumerator.Current;
				if (IsReactionHealTarget(current.m_actor))
				{
					return m_healArrowAbility.GetTechPointsPerHeal();
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					goto end_IL_001f;
				}
			}
			end_IL_001f:;
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	private bool IsReactionHealTarget(ActorData targetActor)
	{
		if (m_syncComp.m_healReactionTargetActor == targetActor.ActorIndex)
		{
			if (!m_syncComp.ActorHasUsedHealReaction(base.ActorData))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		if (m_healArrowActionType != AbilityData.ActionType.INVALID_ACTION)
		{
			if (m_actorTargeting != null)
			{
				List<AbilityTarget> abilityTargetsInRequest = m_actorTargeting.GetAbilityTargetsInRequest(m_healArrowActionType);
				if (abilityTargetsInRequest != null)
				{
					if (abilityTargetsInRequest.Count > 0)
					{
						BoardSquare boardSquareSafe = Board.Get().GetSquare(abilityTargetsInRequest[0].GridPos);
						ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(boardSquareSafe, true, false, base.ActorData);
						if (targetableActorOnSquare == targetActor)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
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

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DirectDamage", string.Empty, m_directDamage);
		AddTokenInt(tokens, "AoeDamage", string.Empty, m_aoeDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_directTargetEffect, "DirectTargetEffect", m_directTargetEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_aoeTargetEffect, "AoeTargetEffect", m_aoeTargetEffect);
	}

	public override bool HasRestrictedFreeAimDegrees(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
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
					HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out Vector3 overridePos);
					Vector3 vec = aimingActor.GetTravelBoardSquareWorldPosition() - overridePos;
					vec.Normalize();
					float num = VectorUtils.HorizontalAngle_Deg(vec);
					min = num - GetMaxAngleForLaser();
					max = num + GetMaxAngleForLaser();
					return true;
				}
				}
			}
		}
		return base.HasRestrictedFreeAimDegrees(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			min = 1f;
			max = 1f;
			return true;
		}
		return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	public override bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		if (targetIndex == 1)
		{
			AbilityTarget abilityTarget = targetsSoFar[0];
			overridePos = aimingActor.GetSquareWorldPosition(Board.Get().GetSquare(abilityTarget.GridPos));
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ArcherDashAndShoot))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ArcherDashAndShoot);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private void SetCachedFields()
	{
		m_cachedDirectTargetEffect = ((!m_abilityMod) ? m_directTargetEffect : m_abilityMod.m_directTargetEffectMod.GetModifiedValue(m_directTargetEffect));
		StandardEffectInfo cachedAoeTargetEffect;
		if ((bool)m_abilityMod)
		{
			cachedAoeTargetEffect = m_abilityMod.m_aoeTargetEffectMod.GetModifiedValue(m_aoeTargetEffect);
		}
		else
		{
			cachedAoeTargetEffect = m_aoeTargetEffect;
		}
		m_cachedAoeTargetEffect = cachedAoeTargetEffect;
		object cachedHealingDebuffTargetEffect;
		if ((bool)m_abilityMod)
		{
			cachedHealingDebuffTargetEffect = m_abilityMod.m_healingDebuffTargetEffect.GetModifiedValue(null);
		}
		else
		{
			cachedHealingDebuffTargetEffect = null;
		}
		m_cachedHealingDebuffTargetEffect = (StandardEffectInfo)cachedHealingDebuffTargetEffect;
	}

	public float GetMaxAngleForLaser()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxAngleForLaserMod.GetModifiedValue(m_maxAngleForLaser);
		}
		else
		{
			result = m_maxAngleForLaser;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
		}
		return result;
	}

	public float GetLaserRange()
	{
		return (!m_abilityMod) ? m_laserRange : m_abilityMod.m_laserRangeMod.GetModifiedValue(m_laserRange);
	}

	public float GetAoeRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_aoeRadiusMod.GetModifiedValue(m_aoeRadius);
		}
		else
		{
			result = m_aoeRadius;
		}
		return result;
	}

	public bool AoePenetratesLoS()
	{
		return (!m_abilityMod) ? m_aoePenetratesLoS : m_abilityMod.m_aoePenetratesLoSMod.GetModifiedValue(m_aoePenetratesLoS);
	}

	public int GetDirectDamage()
	{
		return (!m_abilityMod) ? m_directDamage : m_abilityMod.m_directDamageMod.GetModifiedValue(m_directDamage);
	}

	public int GetAoeDamage()
	{
		return (!m_abilityMod) ? m_aoeDamage : m_abilityMod.m_aoeDamageMod.GetModifiedValue(m_aoeDamage);
	}

	public StandardEffectInfo GetDirectTargetEffect()
	{
		StandardEffectInfo result;
		if (m_cachedDirectTargetEffect != null)
		{
			result = m_cachedDirectTargetEffect;
		}
		else
		{
			result = m_directTargetEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAoeTargetEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAoeTargetEffect != null)
		{
			result = m_cachedAoeTargetEffect;
		}
		else
		{
			result = m_aoeTargetEffect;
		}
		return result;
	}

	public StandardEffectInfo GetHealingDebuffTargetEffect()
	{
		return m_cachedHealingDebuffTargetEffect;
	}

	public int GetCooldownAdjustmentEachTurnIfUnderHealthThreshold()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cooldownAdjustmentEachTurnUnderHealthThreshold.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public float GetHealthThresholdForCooldownOverride()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healthThresholdForCooldownOverride.GetModifiedValue(0f);
		}
		else
		{
			result = 0f;
		}
		return result;
	}
}
