using System.Collections.Generic;
using UnityEngine;

public class NekoFlipDash : Ability
{
	[Separator("Targeting - Dash Range (please use larger value in TargetData")]
	public float m_dashTargetRange = 3.5f;
	[Separator("Targeting - (if actor/disc targeted) landing position")]
	public bool m_canTargetDiscs = true;
	public bool m_canTargetEnemies = true;
	public float m_maxDistanceFromTarget = 2.5f;
	public float m_minDistanceFromTarget;
	public float m_maxAngleChange = 120f;
	[Separator("Targeting - Thrown Disc targeting")]
	public float m_laserWidth = 1f;
	public float m_laserLength = 6.5f;
	public float m_aoeRadiusAtLaserEnd = 1f;
	[Header("-- Disc return end radius")]
	public float m_discReturnEndRadius;
	[Header("-- Dash options --")]
	public float m_chargeRadius;
	public float m_chargeRadiusAtStart;
	public float m_chargeRadiusAtEnd;
	public float m_explosionRadiusAtTargetedDisc = 2.5f;
	public bool m_continueToEndIfTargetEvades = true;
	public bool m_leaveDiscAtStartSquare = true;
	public bool m_throwDiscFromStart = true;
	public bool m_canMoveAfterEvade;
	public bool m_explodeTargetedDisc;
	public int m_discMaxTargets;
	public StandardEffectInfo m_effectOnSelf;
	[Separator("On Enemy Hit")]
	public int m_damage = 20;
	public int m_discDirectDamage = 25;
	public int m_discReturnTripDamage = 10;
	public int m_discReturnTripSubsequentHitDamage = 10;
	public bool m_returnTripIgnoreCover = true;
	public StandardEffectInfo m_enemyHitEffect;
	public int m_explodingTargetDiscDamage = 25;
	[Header("-- Other Abilities --")]
	public int m_discsReturningThisTurnExtraDamage;
	[Separator("Cooldown Reduction")]
	public int m_cdrIfHasReturnDiscHit;
	public int m_cdrOnEnlargeDiscIfCastSameTurn;
	[Separator("Sequences")]
	public GameObject m_throwDiscSequencePrefab;
	public GameObject m_onCastTauntSequencePrefab;
	public GameObject m_chargeSequencePrefab;
	public GameObject m_explosionAtTargetDiscSequencePrefab;
	public GameObject m_discReturnTripSequencePrefab;
	public GameObject m_discPersistentDiscSequencePrefab;
	public float m_recoveryTime = 1f;

	internal int m_throwDiscTargeterIndex = -1;
	internal int m_dashTargeterIndex;
	internal int m_landingTargeterIndex = 1;

	private AbilityMod_NekoFlipDash m_abilityMod;
	private Neko_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedEffectOnSelf;
	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Flip Dash";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		m_syncComp = GetComponent<Neko_SyncComponent>();
		m_throwDiscTargeterIndex = -1;
		m_dashTargeterIndex = 0;
		m_landingTargeterIndex = 1;
		if (m_throwDiscFromStart)
		{
			m_throwDiscTargeterIndex = 0;
			m_dashTargeterIndex = 1;
			m_landingTargeterIndex = 2;
			AbilityUtil_Targeter_NekoDisc discTargeter = new AbilityUtil_Targeter_NekoDisc(
				this, GetLaserWidth(), GetLaserLength(), GetLaserAoeRadius(), false, GetDiscMaxTargets());
			Targeters.Add(discTargeter);
		}
		Targeters.Add(new AbilityUtil_Targeter_NekoCharge(
			this,
			GetChargeRadiusAtStart(),
			GetExplosionRadiusAtTargetedDisc() > 0f && m_explodeTargetedDisc
				? GetExplosionRadiusAtTargetedDisc()
				: GetChargeRadiusAtEnd(),
			GetChargeRadius(),
			0,
			false,
			false));
		AbilityUtil_Targeter_Charge chargeTargeter = new AbilityUtil_Targeter_Charge(
			this,
			AbilityAreaShape.SingleSquare,
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos,
			false);
		chargeTargeter.SetUseMultiTargetUpdate(true);
		Targeters.Add(chargeTargeter);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return Mathf.Clamp(
			m_throwDiscFromStart ? 3 : 2, 
			1, 
			GetTargetData().Length);
	}

	private void SetCachedFields()
	{
		m_cachedEffectOnSelf = m_abilityMod != null ? m_abilityMod.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf) : m_effectOnSelf;
		m_cachedEnemyHitEffect = m_abilityMod != null ? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect) : m_enemyHitEffect;
	}

	public float GetDashTargetRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_dashTargetRangeMod.GetModifiedValue(m_dashTargetRange)
			: m_dashTargetRange;
	}

	public bool CanTargetDiscs()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canTargetDiscsMod.GetModifiedValue(m_canTargetDiscs)
			: m_canTargetDiscs;
	}

	public bool CanTargetEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canTargetEnemiesMod.GetModifiedValue(m_canTargetEnemies)
			: m_canTargetEnemies;
	}

	public float GetMaxDistanceFromTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxDistanceFromTargetMod.GetModifiedValue(m_maxDistanceFromTarget)
			: m_maxDistanceFromTarget;
	}

	public float GetMinDistanceFromTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minDistanceFromTargetMod.GetModifiedValue(m_minDistanceFromTarget)
			: m_minDistanceFromTarget;
	}

	public float GetMaxAngleChange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxAngleChangeMod.GetModifiedValue(m_maxAngleChange)
			: m_maxAngleChange;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public float GetLaserLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserLengthMod.GetModifiedValue(m_laserLength)
			: m_laserLength;
	}

	public float GetLaserAoeRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeRadiusAtLaserEndMod.GetModifiedValue(m_aoeRadiusAtLaserEnd)
			: m_aoeRadiusAtLaserEnd;
	}

	public float GetDiscReturnEndRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_discReturnEndRadiusMod.GetModifiedValue(m_discReturnEndRadius)
			: m_discReturnEndRadius;
	}

	public float GetChargeRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chargeRadiusMod.GetModifiedValue(m_chargeRadius)
			: m_chargeRadius;
	}

	public float GetChargeRadiusAtStart()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chargeRadiusAtStartMod.GetModifiedValue(m_chargeRadiusAtStart)
			: m_chargeRadiusAtStart;
	}

	public float GetChargeRadiusAtEnd()
	{
		return m_abilityMod != null
			? m_abilityMod.m_chargeRadiusAtEndMod.GetModifiedValue(m_chargeRadiusAtEnd)
			: m_chargeRadiusAtEnd;
	}

	public float GetExplosionRadiusAtTargetedDisc()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionRadiusAtTargetedDiscMod.GetModifiedValue(m_explosionRadiusAtTargetedDisc)
			: m_explosionRadiusAtTargetedDisc;
	}

	public bool ContinueToEndIfTargetEvades()
	{
		return m_abilityMod != null
			? m_abilityMod.m_continueToEndIfTargetEvadesMod.GetModifiedValue(m_continueToEndIfTargetEvades)
			: m_continueToEndIfTargetEvades;
	}

	public bool LeaveDiscAtStartSquare()
	{
		return m_abilityMod != null
			? m_abilityMod.m_leaveDiscAtStartSquareMod.GetModifiedValue(m_leaveDiscAtStartSquare)
			: m_leaveDiscAtStartSquare;
	}

	public bool ThrowDiscFromStart()
	{
		return m_abilityMod != null
			? m_abilityMod.m_throwDiscFromStartMod.GetModifiedValue(m_throwDiscFromStart)
			: m_throwDiscFromStart;
	}

	public bool CanMoveAfterEvade()
	{
		return m_abilityMod != null
			? m_abilityMod.m_canMoveAfterEvadeMod.GetModifiedValue(m_canMoveAfterEvade)
			: m_canMoveAfterEvade;
	}

	public bool ExplodeTargetedDisc()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explodeTargetedDiscMod.GetModifiedValue(m_explodeTargetedDisc)
			: m_explodeTargetedDisc;
	}

	public int GetDiscMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_discMaxTargetsMod.GetModifiedValue(m_discMaxTargets)
			: m_discMaxTargets;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		return m_cachedEffectOnSelf ?? m_effectOnSelf;
	}

	public int GetDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damage)
			: m_damage;
	}

	public int GetDiscDirectDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_discDirectDamageMod.GetModifiedValue(m_discDirectDamage)
			: m_discDirectDamage;
	}

	public int GetDiscReturnTripDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_discReturnTripDamageMod.GetModifiedValue(m_discReturnTripDamage)
			: m_discReturnTripDamage;
	}

	public int GetDiscReturnTripSubsequentHitDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_discReturnTripSubsequentHitDamageMod.GetModifiedValue(m_discReturnTripSubsequentHitDamage)
			: m_discReturnTripSubsequentHitDamage;
	}

	public bool ReturnTripIgnoreCover()
	{
		return m_abilityMod != null
			? m_abilityMod.m_returnTripIgnoreCoverMod.GetModifiedValue(m_returnTripIgnoreCover)
			: m_returnTripIgnoreCover;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public int GetExplodingTargetDiscDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explodingTargetDiscDamageMod.GetModifiedValue(m_explodingTargetDiscDamage)
			: m_explodingTargetDiscDamage;
	}

	public int GetDiscsReturningThisTurnExtraDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_discsReturningThisTurnExtraDamageMod.GetModifiedValue(m_discsReturningThisTurnExtraDamage)
			: m_discsReturningThisTurnExtraDamage;
	}

	public int GetCdrIfHasReturnDiscHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfHasReturnDiscHitMod.GetModifiedValue(m_cdrIfHasReturnDiscHit)
			: m_cdrIfHasReturnDiscHit;
	}

	public int GetCdrOnEnlargeDiscIfCastSameTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrOnEnlargeDiscIfCastSameTurnMod.GetModifiedValue(m_cdrOnEnlargeDiscIfCastSameTurn)
			: m_cdrOnEnlargeDiscIfCastSameTurn;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DiscMaxTargets", string.Empty, m_discMaxTargets);
		AbilityMod.AddToken_EffectInfo(tokens, m_effectOnSelf, "EffectOnSelf", m_effectOnSelf);
		AddTokenInt(tokens, "Damage", string.Empty, m_damage);
		AddTokenInt(tokens, "DiscDirectDamage", string.Empty, m_discDirectDamage);
		AddTokenInt(tokens, "DiscReturnTripDamage", string.Empty, m_discReturnTripDamage);
		AddTokenInt(tokens, "DiscReturnTripSubsequentHitDamage", string.Empty, m_discReturnTripSubsequentHitDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "ExplodingTargetDiscDamage", string.Empty, m_explodingTargetDiscDamage);
		AddTokenInt(tokens, "DiscsReturningThisTurnExtraDamage", string.Empty, m_discsReturningThisTurnExtraDamage);
		AddTokenInt(tokens, "CdrIfHasReturnDiscHit", string.Empty, m_cdrIfHasReturnDiscHit);
		AddTokenInt(tokens, "CdrOnEnlargeDiscIfCastSameTurn", string.Empty, m_cdrOnEnlargeDiscIfCastSameTurn);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (CanTargetDiscs() && m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
			foreach (BoardSquare current in activeDiscSquares)
			{
				if (caster.GetActorMovement().CanMoveToBoardSquare(current))
				{
					return true;
				}
			}
			if (!CanTargetEnemies())
			{
				return false;
			}
		}
		if (CanTargetEnemies())
		{
			return HasTargetableActorsInDecision(
				caster, true, false, false, ValidateCheckPath.CanBuildPath, true, false);
		}
		return base.CustomCanCastValidation(caster);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool isValidCast = false;
		bool isValidDash = false;
		if (targetIndex == m_dashTargeterIndex)
		{
			BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
			if (CanTargetEnemies()
			    && targetSquare != null
			    && targetSquare.OccupantActor != null
			    && CanTargetActorInDecision(caster, targetSquare.OccupantActor, true, false, false, ValidateCheckPath.CanBuildPath, true, false))
			{
				isValidCast = true;
				isValidDash = true;
			}
			else if (CanTargetDiscs() && m_syncComp != null)
			{
				List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
				if (activeDiscSquares.Contains(targetSquare))
				{
					isValidCast = true;
					float dist = targetSquare.HorizontalDistanceInSquaresTo(caster.GetCurrentBoardSquare());
					int foo;
					isValidDash = dist <= GetRangeInSquares(m_dashTargeterIndex) &&
					              dist >= GetMinRangeInSquares(m_dashTargeterIndex) &&
					              KnockbackUtils.CanBuildStraightLineChargePath(caster, targetSquare, caster.GetCurrentBoardSquare(), false, out foo);
				}
			}
			else if (!CanTargetEnemies() && !CanTargetDiscs())
			{
				isValidCast = true;
				bool isValidDashDist;
				if (GetDashTargetRange() > 0f && targetSquare != null)
				{
					float dist = targetSquare.HorizontalDistanceInSquaresTo(caster.GetCurrentBoardSquare());
					isValidDashDist = dist <= GetDashTargetRange();
				}
				else
				{
					isValidDashDist = base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
				}

				int foo;
				isValidDash = isValidDashDist
				              && KnockbackUtils.CanBuildStraightLineChargePath(caster, targetSquare, caster.GetCurrentBoardSquare(), false, out foo);
			}
		}
		else
		{
			if (targetIndex != m_landingTargeterIndex)
			{
				return true;
			}
			isValidCast = true;
			BoardSquare prevTargetSquare = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
			BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
			if (targetSquare != null
			    && targetSquare.IsValidForGameplay()
			    && targetSquare != prevTargetSquare
			    && targetSquare != caster.GetCurrentBoardSquare())
			{
				float discDist = targetSquare.HorizontalDistanceInSquaresTo(prevTargetSquare);
				Vector3 to = targetSquare.ToVector3() - prevTargetSquare.ToVector3();
				Vector3 from = prevTargetSquare.ToVector3() - caster.GetCurrentBoardSquare().ToVector3();
				float throwAngle = Vector3.Angle(from, to);
				if (discDist >= m_minDistanceFromTarget
				    && discDist <= m_maxDistanceFromTarget
				    && throwAngle <= m_maxAngleChange)
				{
					int foo;
					isValidDash = KnockbackUtils.CanBuildStraightLineChargePath(caster, targetSquare, prevTargetSquare, false, out foo);
				}
			}
		}
		return isValidDash && isValidCast;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return CanMoveAfterEvade();
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, GetExplodingTargetDiscDamage()),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.HighPower, GetDamage()),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetDiscDirectDamage())
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserLength() + GetLaserAoeRadius();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NekoFlipDash))
		{
			m_abilityMod = abilityMod as AbilityMod_NekoFlipDash;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
