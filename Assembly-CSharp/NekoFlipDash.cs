using System.Collections.Generic;
using UnityEngine;

public class NekoFlipDash : Ability
{
	[Separator("Targeting - Dash Range (please use larger value in TargetData", true)]
	public float m_dashTargetRange = 3.5f;

	[Separator("Targeting - (if actor/disc targeted) landing position", true)]
	public bool m_canTargetDiscs = true;

	public bool m_canTargetEnemies = true;

	public float m_maxDistanceFromTarget = 2.5f;

	public float m_minDistanceFromTarget;

	public float m_maxAngleChange = 120f;

	[Separator("Targeting - Thrown Disc targeting", true)]
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

	[Separator("On Enemy Hit", true)]
	public int m_damage = 20;

	public int m_discDirectDamage = 25;

	public int m_discReturnTripDamage = 10;

	public int m_discReturnTripSubsequentHitDamage = 10;

	public bool m_returnTripIgnoreCover = true;

	public StandardEffectInfo m_enemyHitEffect;

	public int m_explodingTargetDiscDamage = 25;

	[Header("-- Other Abilities --")]
	public int m_discsReturningThisTurnExtraDamage;

	[Separator("Cooldown Reduction", true)]
	public int m_cdrIfHasReturnDiscHit;

	public int m_cdrOnEnlargeDiscIfCastSameTurn;

	[Separator("Sequences", true)]
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
			AbilityUtil_Targeter_NekoDisc item = new AbilityUtil_Targeter_NekoDisc(this, GetLaserWidth(), GetLaserLength(), GetLaserAoeRadius(), false, GetDiscMaxTargets());
			base.Targeters.Add(item);
		}
		float chargeRadiusAtStart = GetChargeRadiusAtStart();
		float radiusAroundEnd;
		if (GetExplosionRadiusAtTargetedDisc() > 0f && m_explodeTargetedDisc)
		{
			radiusAroundEnd = GetExplosionRadiusAtTargetedDisc();
		}
		else
		{
			radiusAroundEnd = GetChargeRadiusAtEnd();
		}
		AbilityUtil_Targeter_NekoCharge item2 = new AbilityUtil_Targeter_NekoCharge(this, chargeRadiusAtStart, radiusAroundEnd, GetChargeRadius(), 0, false, false);
		base.Targeters.Add(item2);
		AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, false);
		abilityUtil_Targeter_Charge.SetUseMultiTargetUpdate(true);
		base.Targeters.Add(abilityUtil_Targeter_Charge);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int max = GetTargetData().Length;
		int value;
		if (m_throwDiscFromStart)
		{
			value = 3;
		}
		else
		{
			value = 2;
		}
		return Mathf.Clamp(value, 1, max);
	}

	private void SetCachedFields()
	{
		m_cachedEffectOnSelf = ((!m_abilityMod) ? m_effectOnSelf : m_abilityMod.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf));
		StandardEffectInfo cachedEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedEnemyHitEffect = m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = m_enemyHitEffect;
		}
		m_cachedEnemyHitEffect = cachedEnemyHitEffect;
	}

	public float GetDashTargetRange()
	{
		return (!m_abilityMod) ? m_dashTargetRange : m_abilityMod.m_dashTargetRangeMod.GetModifiedValue(m_dashTargetRange);
	}

	public bool CanTargetDiscs()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canTargetDiscsMod.GetModifiedValue(m_canTargetDiscs);
		}
		else
		{
			result = m_canTargetDiscs;
		}
		return result;
	}

	public bool CanTargetEnemies()
	{
		return (!m_abilityMod) ? m_canTargetEnemies : m_abilityMod.m_canTargetEnemiesMod.GetModifiedValue(m_canTargetEnemies);
	}

	public float GetMaxDistanceFromTarget()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxDistanceFromTargetMod.GetModifiedValue(m_maxDistanceFromTarget);
		}
		else
		{
			result = m_maxDistanceFromTarget;
		}
		return result;
	}

	public float GetMinDistanceFromTarget()
	{
		return (!m_abilityMod) ? m_minDistanceFromTarget : m_abilityMod.m_minDistanceFromTargetMod.GetModifiedValue(m_minDistanceFromTarget);
	}

	public float GetMaxAngleChange()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxAngleChangeMod.GetModifiedValue(m_maxAngleChange);
		}
		else
		{
			result = m_maxAngleChange;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		return (!m_abilityMod) ? m_laserWidth : m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
	}

	public float GetLaserLength()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserLengthMod.GetModifiedValue(m_laserLength);
		}
		else
		{
			result = m_laserLength;
		}
		return result;
	}

	public float GetLaserAoeRadius()
	{
		return (!m_abilityMod) ? m_aoeRadiusAtLaserEnd : m_abilityMod.m_aoeRadiusAtLaserEndMod.GetModifiedValue(m_aoeRadiusAtLaserEnd);
	}

	public float GetDiscReturnEndRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_discReturnEndRadiusMod.GetModifiedValue(m_discReturnEndRadius);
		}
		else
		{
			result = m_discReturnEndRadius;
		}
		return result;
	}

	public float GetChargeRadius()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_chargeRadiusMod.GetModifiedValue(m_chargeRadius);
		}
		else
		{
			result = m_chargeRadius;
		}
		return result;
	}

	public float GetChargeRadiusAtStart()
	{
		return (!m_abilityMod) ? m_chargeRadiusAtStart : m_abilityMod.m_chargeRadiusAtStartMod.GetModifiedValue(m_chargeRadiusAtStart);
	}

	public float GetChargeRadiusAtEnd()
	{
		return (!m_abilityMod) ? m_chargeRadiusAtEnd : m_abilityMod.m_chargeRadiusAtEndMod.GetModifiedValue(m_chargeRadiusAtEnd);
	}

	public float GetExplosionRadiusAtTargetedDisc()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_explosionRadiusAtTargetedDiscMod.GetModifiedValue(m_explosionRadiusAtTargetedDisc);
		}
		else
		{
			result = m_explosionRadiusAtTargetedDisc;
		}
		return result;
	}

	public bool ContinueToEndIfTargetEvades()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_continueToEndIfTargetEvadesMod.GetModifiedValue(m_continueToEndIfTargetEvades);
		}
		else
		{
			result = m_continueToEndIfTargetEvades;
		}
		return result;
	}

	public bool LeaveDiscAtStartSquare()
	{
		return (!m_abilityMod) ? m_leaveDiscAtStartSquare : m_abilityMod.m_leaveDiscAtStartSquareMod.GetModifiedValue(m_leaveDiscAtStartSquare);
	}

	public bool ThrowDiscFromStart()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_throwDiscFromStartMod.GetModifiedValue(m_throwDiscFromStart);
		}
		else
		{
			result = m_throwDiscFromStart;
		}
		return result;
	}

	public bool CanMoveAfterEvade()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_canMoveAfterEvadeMod.GetModifiedValue(m_canMoveAfterEvade);
		}
		else
		{
			result = m_canMoveAfterEvade;
		}
		return result;
	}

	public bool ExplodeTargetedDisc()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_explodeTargetedDiscMod.GetModifiedValue(m_explodeTargetedDisc);
		}
		else
		{
			result = m_explodeTargetedDisc;
		}
		return result;
	}

	public int GetDiscMaxTargets()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_discMaxTargetsMod.GetModifiedValue(m_discMaxTargets);
		}
		else
		{
			result = m_discMaxTargets;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnSelf != null)
		{
			result = m_cachedEffectOnSelf;
		}
		else
		{
			result = m_effectOnSelf;
		}
		return result;
	}

	public int GetDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damage);
		}
		else
		{
			result = m_damage;
		}
		return result;
	}

	public int GetDiscDirectDamage()
	{
		return (!m_abilityMod) ? m_discDirectDamage : m_abilityMod.m_discDirectDamageMod.GetModifiedValue(m_discDirectDamage);
	}

	public int GetDiscReturnTripDamage()
	{
		return (!m_abilityMod) ? m_discReturnTripDamage : m_abilityMod.m_discReturnTripDamageMod.GetModifiedValue(m_discReturnTripDamage);
	}

	public int GetDiscReturnTripSubsequentHitDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_discReturnTripSubsequentHitDamageMod.GetModifiedValue(m_discReturnTripSubsequentHitDamage);
		}
		else
		{
			result = m_discReturnTripSubsequentHitDamage;
		}
		return result;
	}

	public bool ReturnTripIgnoreCover()
	{
		return (!m_abilityMod) ? m_returnTripIgnoreCover : m_abilityMod.m_returnTripIgnoreCoverMod.GetModifiedValue(m_returnTripIgnoreCover);
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyHitEffect != null)
		{
			result = m_cachedEnemyHitEffect;
		}
		else
		{
			result = m_enemyHitEffect;
		}
		return result;
	}

	public int GetExplodingTargetDiscDamage()
	{
		return (!m_abilityMod) ? m_explodingTargetDiscDamage : m_abilityMod.m_explodingTargetDiscDamageMod.GetModifiedValue(m_explodingTargetDiscDamage);
	}

	public int GetDiscsReturningThisTurnExtraDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_discsReturningThisTurnExtraDamageMod.GetModifiedValue(m_discsReturningThisTurnExtraDamage);
		}
		else
		{
			result = m_discsReturningThisTurnExtraDamage;
		}
		return result;
	}

	public int GetCdrIfHasReturnDiscHit()
	{
		return (!m_abilityMod) ? m_cdrIfHasReturnDiscHit : m_abilityMod.m_cdrIfHasReturnDiscHitMod.GetModifiedValue(m_cdrIfHasReturnDiscHit);
	}

	public int GetCdrOnEnlargeDiscIfCastSameTurn()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cdrOnEnlargeDiscIfCastSameTurnMod.GetModifiedValue(m_cdrOnEnlargeDiscIfCastSameTurn);
		}
		else
		{
			result = m_cdrOnEnlargeDiscIfCastSameTurn;
		}
		return result;
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
			using (List<BoardSquare>.Enumerator enumerator = activeDiscSquares.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoardSquare current = enumerator.Current;
					if (caster.GetActorMovement().CanMoveToBoardSquare(current))
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
			if (!CanTargetEnemies())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return false;
					}
				}
			}
		}
		if (CanTargetEnemies())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return HasTargetableActorsInDecision(caster, true, false, false, ValidateCheckPath.CanBuildPath, true, false);
				}
			}
		}
		return base.CustomCanCastValidation(caster);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		bool flag2 = false;
		BoardSquare boardSquareSafe;
		bool flag3;
		if (targetIndex == m_dashTargeterIndex)
		{
			boardSquareSafe = Board.Get().GetSquare(target.GridPos);
			if (CanTargetEnemies())
			{
				if (boardSquareSafe != null && boardSquareSafe.OccupantActor != null)
				{
					if (CanTargetActorInDecision(caster, boardSquareSafe.OccupantActor, true, false, false, ValidateCheckPath.CanBuildPath, true, false))
					{
						flag = true;
						flag2 = true;
						goto IL_033d;
					}
				}
			}
			if (CanTargetDiscs())
			{
				if (m_syncComp != null)
				{
					List<BoardSquare> activeDiscSquares = m_syncComp.GetActiveDiscSquares();
					if (activeDiscSquares.Contains(boardSquareSafe))
					{
						flag = true;
						float num = boardSquareSafe.HorizontalDistanceInSquaresTo(caster.GetCurrentBoardSquare());
						if (!(num > GetRangeInSquares(m_dashTargeterIndex)))
						{
							if (!(num < GetMinRangeInSquares(m_dashTargeterIndex)))
							{
								flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out int _);
								goto IL_033d;
							}
						}
						flag2 = false;
					}
					goto IL_033d;
				}
			}
			if (!CanTargetEnemies())
			{
				if (!CanTargetDiscs())
				{
					flag = true;
					flag3 = false;
					if (GetDashTargetRange() > 0f)
					{
						if (boardSquareSafe != null)
						{
							float num2 = boardSquareSafe.HorizontalDistanceInSquaresTo(caster.GetCurrentBoardSquare());
							flag3 = (num2 <= GetDashTargetRange());
							goto IL_01d2;
						}
					}
					flag3 = base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
					goto IL_01d2;
				}
			}
		}
		else
		{
			if (targetIndex != m_landingTargeterIndex)
			{
				return true;
			}
			flag = true;
			BoardSquare boardSquareSafe2 = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
			BoardSquare boardSquareSafe3 = Board.Get().GetSquare(target.GridPos);
			if (boardSquareSafe3 != null && boardSquareSafe3.IsValidForGameplay())
			{
				if (boardSquareSafe3 != boardSquareSafe2)
				{
					if (boardSquareSafe3 != caster.GetCurrentBoardSquare())
					{
						float num3 = boardSquareSafe3.HorizontalDistanceInSquaresTo(boardSquareSafe2);
						Vector3 to = boardSquareSafe3.ToVector3() - boardSquareSafe2.ToVector3();
						Vector3 from = boardSquareSafe2.ToVector3() - caster.GetCurrentBoardSquare().ToVector3();
						float num4 = Vector3.Angle(from, to);
						if (num3 >= m_minDistanceFromTarget)
						{
							if (num3 <= m_maxDistanceFromTarget && num4 <= m_maxAngleChange)
							{
								flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe3, boardSquareSafe2, false, out int _);
							}
						}
					}
				}
			}
		}
		goto IL_033d;
		IL_01d2:
		int num5;
		if (flag3)
		{
			num5 = (KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out int _) ? 1 : 0);
		}
		else
		{
			num5 = 0;
		}
		flag2 = ((byte)num5 != 0);
		goto IL_033d;
		IL_033d:
		return flag2 && flag;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return CanMoveAfterEvade();
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, GetExplodingTargetDiscDamage()));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.HighPower, GetDamage()));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetDiscDirectDamage()));
		return list;
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
		if (abilityMod.GetType() != typeof(AbilityMod_NekoFlipDash))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_NekoFlipDash);
			Setup();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
