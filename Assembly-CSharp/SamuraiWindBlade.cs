using System;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiWindBlade : Ability
{
	[Header("-- Targeting")]
	public float m_laserWidth = 0.6f;

	public float m_minRangeBeforeBend = 1f;

	public float m_maxRangeBeforeBend = 5.5f;

	public float m_maxTotalRange = 7.5f;

	public float m_maxBendAngle = 45f;

	public bool m_penetrateLoS;

	public int m_maxTargets = 1;

	[Header("-- Damage")]
	public int m_laserDamageAmount = 5;

	public int m_damageChangePerTarget;

	public StandardEffectInfo m_laserHitEffect;

	[Header("-- Shielding per enemy hit on start of Next Turn")]
	public int m_shieldingPerEnemyHitNextTurn;

	public int m_shieldingDuration = 1;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_SamuraiWindBlade m_abilityMod;

	private Samurai_SyncComponent m_syncComponent;

	private StandardEffectInfo m_cachedLaserHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
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
			m_abilityName = "Wind Blade";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		m_syncComponent = base.ActorData.GetComponent<Samurai_SyncComponent>();
		ClearTargeters();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_BendingLaser abilityUtil_Targeter_BendingLaser = new AbilityUtil_Targeter_BendingLaser(this, GetLaserWidth(), GetMinRangeBeforeBend(), GetMaxRangeBeforeBend(), GetMaxTotalRange(), GetMaxBendAngle(), PenetrateLoS(), GetMaxTargets());
			abilityUtil_Targeter_BendingLaser.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_BendingLaser);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (!base.Targeters.IsNullOrEmpty())
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
			AbilityUtil_Targeter_BendingLaser abilityUtil_Targeter_BendingLaser = base.Targeters[0] as AbilityUtil_Targeter_BendingLaser;
			if (abilityUtil_Targeter_BendingLaser.DidStopShort())
			{
				while (true)
				{
					switch (1)
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

	private void SetCachedFields()
	{
		StandardEffectInfo cachedLaserHitEffect;
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
			cachedLaserHitEffect = m_abilityMod.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect);
		}
		else
		{
			cachedLaserHitEffect = m_laserHitEffect;
		}
		m_cachedLaserHitEffect = cachedLaserHitEffect;
	}

	public float GetLaserWidth()
	{
		float result;
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
			result = m_abilityMod.m_maxRangeBeforeBendMod.GetModifiedValue(m_maxRangeBeforeBend);
		}
		else
		{
			result = m_maxRangeBeforeBend;
		}
		return result;
	}

	public float GetMaxTotalRange()
	{
		return (!m_abilityMod) ? m_maxTotalRange : m_abilityMod.m_maxTotalRangeMod.GetModifiedValue(m_maxTotalRange);
	}

	public float GetMaxBendAngle()
	{
		float result;
		if ((bool)m_abilityMod)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_abilityMod.m_maxBendAngleMod.GetModifiedValue(m_maxBendAngle);
		}
		else
		{
			result = m_maxBendAngle;
		}
		return result;
	}

	public bool PenetrateLoS()
	{
		return (!m_abilityMod) ? m_penetrateLoS : m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS);
	}

	public int GetMaxTargets()
	{
		return (!m_abilityMod) ? m_maxTargets : m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
	}

	public int GetLaserDamageAmount()
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
			result = m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount);
		}
		else
		{
			result = m_laserDamageAmount;
		}
		return result;
	}

	public int GetDamageChangePerTarget()
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
			result = m_abilityMod.m_damageChangePerTargetMod.GetModifiedValue(m_damageChangePerTarget);
		}
		else
		{
			result = m_damageChangePerTarget;
		}
		return result;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedLaserHitEffect != null)
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
			result = m_cachedLaserHitEffect;
		}
		else
		{
			result = m_laserHitEffect;
		}
		return result;
	}

	public int GetShieldingPerEnemyHitNextTurn()
	{
		return (!m_abilityMod) ? m_shieldingPerEnemyHitNextTurn : m_abilityMod.m_shieldingPerEnemyHitNextTurnMod.GetModifiedValue(m_shieldingPerEnemyHitNextTurn);
	}

	public int GetShieldingDuration()
	{
		return (!m_abilityMod) ? m_shieldingDuration : m_abilityMod.m_shieldingDurationMod.GetModifiedValue(m_shieldingDuration);
	}

	public int CalcDamage(int hitOrder)
	{
		int num = GetLaserDamageAmount();
		if (GetDamageChangePerTarget() > 0)
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
			if (hitOrder > 0)
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
				num += GetDamageChangePerTarget() * hitOrder;
			}
		}
		return num;
	}

	public int GetHitOrderIndexFromTargeters(ActorData actor, int currentTargetIndex)
	{
		int num = 0;
		if (base.Targeters != null)
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
			for (int i = 0; i < base.Targeters.Count && i <= currentTargetIndex; i++)
			{
				AbilityUtil_Targeter_BendingLaser abilityUtil_Targeter_BendingLaser = base.Targeters[i] as AbilityUtil_Targeter_BendingLaser;
				if (abilityUtil_Targeter_BendingLaser != null)
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
					using (List<ActorData>.Enumerator enumerator = abilityUtil_Targeter_BendingLaser.m_ordererdHitActors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData current = enumerator.Current;
							if (current == actor)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
										return num;
									}
								}
							}
							num++;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
			}
		}
		return -1;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, m_laserDamageAmount);
		AddTokenInt(tokens, "DamageChangePerTarget", string.Empty, m_damageChangePerTarget);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffect, "LaserHitEffect", m_laserHitEffect);
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "ShieldingPerEnemyHitNextTurn", string.Empty, m_shieldingPerEnemyHitNextTurn);
		AddTokenInt(tokens, "ShieldingDuration", string.Empty, m_shieldingDuration);
	}

	private Vector3 GetTargeterClampedAimDirection(Vector3 aimDir, List<AbilityTarget> targets)
	{
		aimDir.y = 0f;
		aimDir.Normalize();
		float maxBendAngle = GetMaxBendAngle();
		Vector3 aimDirection = targets[0].AimDirection;
		if (maxBendAngle > 0f && maxBendAngle < 360f)
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return GetMinRangeBeforeBend();
				}
			}
		}
		if (magnitude > GetMaxRangeBeforeBend() * Board.Get().squareSize)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return GetMaxRangeBeforeBend();
				}
			}
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
				switch (6)
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
		if (m_laserDamageAmount > 0)
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
			int num = GetLaserDamageAmount();
			if (GetDamageChangePerTarget() > 0)
			{
				int hitOrderIndexFromTargeters = GetHitOrderIndexFromTargeters(targetActor, currentTargeterIndex);
				num = CalcDamage(hitOrderIndexFromTargeters);
			}
			if (m_syncComponent != null)
			{
				num += m_syncComponent.CalcExtraDamageFromSelfBuffAbility();
			}
			dictionary[AbilityTooltipSymbol.Damage] = num;
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SamuraiWindBlade))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityMod = (abilityMod as AbilityMod_SamuraiWindBlade);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
