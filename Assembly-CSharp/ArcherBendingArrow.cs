using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcherBendingArrow : Ability
{
	[Header("-- Targeting")]
	public float m_laserWidth = 0.6f;

	public float m_minRangeBeforeBend = 1f;

	public float m_maxRangeBeforeBend = 5.5f;

	public float m_maxTotalRange = 7.5f;

	public float m_maxBendAngle = 45f;

	public bool m_penetrateLoS;

	public int m_maxTargets = 1;

	public bool m_startTargeterFadeAtActorRadius = true;

	[Header("-- Damage")]
	public int m_laserDamageAmount = 5;

	public StandardEffectInfo m_laserHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ArcherBendingArrow m_abilityMod;

	private Archer_SyncComponent m_syncComp;

	private ArcherHealingDebuffArrow m_healArrowAbility;

	private AbilityData.ActionType m_healArrowActionType = AbilityData.ActionType.INVALID_ACTION;

	private ActorTargeting m_actorTargeting;

	private StandardEffectInfo m_cachedLaserHitEffect;

	private StandardEffectInfo m_cachedEffectToHealingDebuffTarget;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Bending Arrow";
		}
		m_syncComp = GetComponent<Archer_SyncComponent>();
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			m_healArrowAbility = (GetAbilityOfType(typeof(ArcherHealingDebuffArrow)) as ArcherHealingDebuffArrow);
			if (m_healArrowAbility != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_healArrowActionType = component.GetActionTypeOfAbility(m_healArrowAbility);
			}
		}
		m_actorTargeting = GetComponent<ActorTargeting>();
		base.Targeters.Clear();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_BendingLaser abilityUtil_Targeter_BendingLaser = new AbilityUtil_Targeter_BendingLaser(this, GetLaserWidth(), GetMinRangeBeforeBend(), GetMaxRangeBeforeBend(), GetMaxTotalRange(), GetMaxBendAngle(), PenetrateLoS(), GetMaxTargets());
			abilityUtil_Targeter_BendingLaser.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_BendingLaser.m_startFadeAtActorRadius = m_startTargeterFadeAtActorRadius;
			base.Targeters.Add(abilityUtil_Targeter_BendingLaser);
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

	public override int GetExpectedNumberOfTargeters()
	{
		if (!base.Targeters.IsNullOrEmpty())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityUtil_Targeter_BendingLaser abilityUtil_Targeter_BendingLaser = base.Targeters[0] as AbilityUtil_Targeter_BendingLaser;
			if (abilityUtil_Targeter_BendingLaser.DidStopShort())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return 1;
					}
				}
			}
		}
		return 2;
	}

	public override bool ShouldAutoConfirmIfTargetingOnEndTurn()
	{
		return true;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxTotalRange();
	}

	private Vector3 GetTargeterClampedAimDirection(Vector3 aimDir, List<AbilityTarget> targets)
	{
		aimDir.y = 0f;
		aimDir.Normalize();
		float maxBendAngle = GetMaxBendAngle();
		Vector3 aimDirection = targets[0].AimDirection;
		if (maxBendAngle > 0f && maxBendAngle < 360f)
		{
			aimDir = Vector3.RotateTowards(aimDirection, aimDir, (float)Math.PI / 180f * maxBendAngle, 0f);
		}
		return aimDir;
	}

	private float GetClampedRangeInSquares(ActorData targetingActor, AbilityTarget currentTarget)
	{
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		float magnitude = (currentTarget.FreePos - travelBoardSquareWorldPositionForLos).magnitude;
		if (magnitude < GetMinRangeBeforeBend() * Board.Get().squareSize)
		{
			return GetMinRangeBeforeBend();
		}
		if (magnitude > GetMaxRangeBeforeBend() * Board.Get().squareSize)
		{
			return GetMaxRangeBeforeBend();
		}
		return magnitude / Board.Get().squareSize;
	}

	private float GetDistanceRemaining(ActorData targetingActor, AbilityTarget previousTarget, out Vector3 bendPos)
	{
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		float clampedRangeInSquares = GetClampedRangeInSquares(targetingActor, previousTarget);
		bendPos = travelBoardSquareWorldPositionForLos + previousTarget.AimDirection * clampedRangeInSquares * Board.Get().squareSize;
		return GetMaxTotalRange() - clampedRangeInSquares;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_laserDamageAmount > 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		}
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int num = GetLaserDamageAmount();
		bool flag = currentTargeterIndex == 0;
		AbilityUtil_Targeter.ActorTarget actorTarget = base.Targeters[currentTargeterIndex].GetActorsInRange().Find((AbilityUtil_Targeter.ActorTarget t) => t.m_actor == targetActor);
		if (actorTarget == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (currentTargeterIndex > 0)
			{
				actorTarget = base.Targeters[0].GetActorsInRange().Find((AbilityUtil_Targeter.ActorTarget t) => t.m_actor == targetActor);
				flag = (actorTarget != null);
			}
		}
		if (actorTarget != null && !actorTarget.m_subjectTypes.Contains(AbilityTooltipSubject.Near))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			num = GetDamageAfterPierce();
		}
		if (IsReactionHealTarget(targetActor))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			num += GetExtraDamageToHealingDebuffTarget();
			num += m_healArrowAbility.GetExtraDamageToThisTargetFromCaster();
		}
		if (!flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			num += GetExtraDamageAfterBend();
		}
		dictionary[AbilityTooltipSymbol.Damage] = num;
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.Targeters[i].GetActorsInRange();
			using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityUtil_Targeter.ActorTarget current = enumerator.Current;
					if (IsReactionHealTarget(current.m_actor))
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								if (1 == 0)
								{
									/*OpCode not supported: LdMemberToken*/;
								}
								return m_healArrowAbility.GetTechPointsPerHeal();
							}
						}
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}

	private bool IsReactionHealTarget(ActorData targetActor)
	{
		if (m_syncComp.m_healReactionTargetActor == targetActor.ActorIndex)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_syncComp.ActorHasUsedHealReaction(base.ActorData))
			{
				while (true)
				{
					switch (6)
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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_actorTargeting != null)
			{
				List<AbilityTarget> abilityTargetsInRequest = m_actorTargeting.GetAbilityTargetsInRequest(m_healArrowActionType);
				if (abilityTargetsInRequest != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (abilityTargetsInRequest.Count > 0)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(abilityTargetsInRequest[0].GridPos);
						ActorData targetableActorOnSquare = AreaEffectUtils.GetTargetableActorOnSquare(boardSquareSafe, true, false, base.ActorData);
						if (targetableActorOnSquare == targetActor)
						{
							while (true)
							{
								switch (1)
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
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, m_laserDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffect, "LaserHitEffect", m_laserHitEffect);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					min = GetMinRangeBeforeBend() * Board.Get().squareSize;
					max = GetMaxRangeBeforeBend() * Board.Get().squareSize;
					return true;
				}
			}
		}
		if (targetIndex == 1)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					min = 1f;
					max = 1f;
					return true;
				}
			}
		}
		return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	public override bool HasRestrictedFreeAimDegrees(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (targetIndex == 1)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Vector3 aimDirection = targetsSoFar[0].AimDirection;
					float num = VectorUtils.HorizontalAngle_Deg(aimDirection);
					min = num - GetMaxBendAngle();
					max = num + GetMaxBendAngle();
					return true;
				}
				}
			}
		}
		return base.HasRestrictedFreeAimDegrees(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}

	public override bool HasAimingOriginOverride(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out Vector3 overridePos)
	{
		if (targetIndex == 1)
		{
			AbilityTarget abilityTarget = targetsSoFar[0];
			Vector3 travelBoardSquareWorldPositionForLos = aimingActor.GetTravelBoardSquareWorldPositionForLos();
			float magnitude = (abilityTarget.FreePos - travelBoardSquareWorldPositionForLos).magnitude;
			float d;
			if (magnitude < GetMinRangeBeforeBend() * Board.Get().squareSize)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				d = GetMinRangeBeforeBend() * Board.Get().squareSize;
			}
			else if (magnitude > GetMaxRangeBeforeBend() * Board.Get().squareSize)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				d = GetMaxRangeBeforeBend() * Board.Get().squareSize;
			}
			else
			{
				d = magnitude;
			}
			Vector3 vector = overridePos = travelBoardSquareWorldPositionForLos + abilityTarget.AimDirection * d;
			return true;
		}
		return base.HasAimingOriginOverride(aimingActor, targetIndex, targetsSoFar, out overridePos);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ArcherBendingArrow))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_ArcherBendingArrow);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedLaserHitEffect;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			cachedLaserHitEffect = m_abilityMod.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect);
		}
		else
		{
			cachedLaserHitEffect = m_laserHitEffect;
		}
		m_cachedLaserHitEffect = cachedLaserHitEffect;
		object cachedEffectToHealingDebuffTarget;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedEffectToHealingDebuffTarget = m_abilityMod.m_effectToHealingDebuffTarget.GetModifiedValue(null);
		}
		else
		{
			cachedEffectToHealingDebuffTarget = null;
		}
		m_cachedEffectToHealingDebuffTarget = (StandardEffectInfo)cachedEffectToHealingDebuffTarget;
	}

	public float GetLaserWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
		}
		return result;
	}

	public float GetMinRangeBeforeBend()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_minRangeBeforeBendMod.GetModifiedValue(m_minRangeBeforeBend);
		}
		else
		{
			result = m_minRangeBeforeBend;
		}
		return result;
	}

	public float GetMaxRangeBeforeBend()
	{
		return (!m_abilityMod) ? m_maxRangeBeforeBend : m_abilityMod.m_maxRangeBeforeBendMod.GetModifiedValue(m_maxRangeBeforeBend);
	}

	public float GetMaxTotalRange()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_maxTotalRangeMod.GetModifiedValue(m_maxTotalRange);
		}
		else
		{
			result = m_maxTotalRange;
		}
		return result;
	}

	public float GetMaxBendAngle()
	{
		return (!m_abilityMod) ? m_maxBendAngle : m_abilityMod.m_maxBendAngleMod.GetModifiedValue(m_maxBendAngle);
	}

	public bool PenetrateLoS()
	{
		return (!m_abilityMod) ? m_penetrateLoS : m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS);
	}

	public int GetMaxTargets()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			result = m_maxTargets;
		}
		return result;
	}

	public int GetLaserDamageAmount()
	{
		return (!m_abilityMod) ? m_laserDamageAmount : m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount);
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedLaserHitEffect != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_cachedLaserHitEffect;
		}
		else
		{
			result = m_laserHitEffect;
		}
		return result;
	}

	public int GetExtraAbsorbForShieldGeneratorPerHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_nextShieldGeneratorExtraAbsorbPerHit.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraAbsorbForShieldGeneratorMax()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_nextShieldGeneratorExtraAbsorbMax.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraDamageToHealingDebuffTarget()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_extraDamageToHealingDebuffTarget.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetExtraDamageAfterBend()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_extraDamageAfterBend.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetDamageAfterPierce()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_damageAfterPiercingMod.GetModifiedValue(GetLaserDamageAmount());
		}
		else
		{
			result = GetLaserDamageAmount();
		}
		return result;
	}

	public StandardEffectInfo GetEffectToHealingDebuffTarget()
	{
		return m_cachedEffectToHealingDebuffTarget;
	}

	public int GetExtraHealingFromHealingDebuffTarget()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_extraHealingFromHealingDebuffTarget.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}
}
