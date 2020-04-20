using System;
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
	public int m_damage = 0x14;

	public int m_discDirectDamage = 0x19;

	public int m_discReturnTripDamage = 0xA;

	public int m_discReturnTripSubsequentHitDamage = 0xA;

	public bool m_returnTripIgnoreCover = true;

	public StandardEffectInfo m_enemyHitEffect;

	public int m_explodingTargetDiscDamage = 0x19;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Flip Dash";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.SetCachedFields();
		this.m_syncComp = base.GetComponent<Neko_SyncComponent>();
		this.m_throwDiscTargeterIndex = -1;
		this.m_dashTargeterIndex = 0;
		this.m_landingTargeterIndex = 1;
		if (this.m_throwDiscFromStart)
		{
			this.m_throwDiscTargeterIndex = 0;
			this.m_dashTargeterIndex = 1;
			this.m_landingTargeterIndex = 2;
			AbilityUtil_Targeter_NekoDisc item = new AbilityUtil_Targeter_NekoDisc(this, this.GetLaserWidth(), this.GetLaserLength(), this.GetLaserAoeRadius(), false, this.GetDiscMaxTargets(), false, false);
			base.Targeters.Add(item);
		}
		float chargeRadiusAtStart = this.GetChargeRadiusAtStart();
		float radiusAroundEnd;
		if (this.GetExplosionRadiusAtTargetedDisc() > 0f && this.m_explodeTargetedDisc)
		{
			radiusAroundEnd = this.GetExplosionRadiusAtTargetedDisc();
		}
		else
		{
			radiusAroundEnd = this.GetChargeRadiusAtEnd();
		}
		AbilityUtil_Targeter_NekoCharge item2 = new AbilityUtil_Targeter_NekoCharge(this, chargeRadiusAtStart, radiusAroundEnd, this.GetChargeRadius(), 0, false, false);
		base.Targeters.Add(item2);
		AbilityUtil_Targeter_Charge abilityUtil_Targeter_Charge = new AbilityUtil_Targeter_Charge(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CasterPos, false, false);
		abilityUtil_Targeter_Charge.SetUseMultiTargetUpdate(true);
		base.Targeters.Add(abilityUtil_Targeter_Charge);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		int max = this.GetTargetData().Length;
		int value;
		if (this.m_throwDiscFromStart)
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
		this.m_cachedEffectOnSelf = ((!this.m_abilityMod) ? this.m_effectOnSelf : this.m_abilityMod.m_effectOnSelfMod.GetModifiedValue(this.m_effectOnSelf));
		StandardEffectInfo cachedEnemyHitEffect;
		if (this.m_abilityMod)
		{
			cachedEnemyHitEffect = this.m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			cachedEnemyHitEffect = this.m_enemyHitEffect;
		}
		this.m_cachedEnemyHitEffect = cachedEnemyHitEffect;
	}

	public float GetDashTargetRange()
	{
		return (!this.m_abilityMod) ? this.m_dashTargetRange : this.m_abilityMod.m_dashTargetRangeMod.GetModifiedValue(this.m_dashTargetRange);
	}

	public bool CanTargetDiscs()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_canTargetDiscsMod.GetModifiedValue(this.m_canTargetDiscs);
		}
		else
		{
			result = this.m_canTargetDiscs;
		}
		return result;
	}

	public bool CanTargetEnemies()
	{
		return (!this.m_abilityMod) ? this.m_canTargetEnemies : this.m_abilityMod.m_canTargetEnemiesMod.GetModifiedValue(this.m_canTargetEnemies);
	}

	public float GetMaxDistanceFromTarget()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxDistanceFromTargetMod.GetModifiedValue(this.m_maxDistanceFromTarget);
		}
		else
		{
			result = this.m_maxDistanceFromTarget;
		}
		return result;
	}

	public float GetMinDistanceFromTarget()
	{
		return (!this.m_abilityMod) ? this.m_minDistanceFromTarget : this.m_abilityMod.m_minDistanceFromTargetMod.GetModifiedValue(this.m_minDistanceFromTarget);
	}

	public float GetMaxAngleChange()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxAngleChangeMod.GetModifiedValue(this.m_maxAngleChange);
		}
		else
		{
			result = this.m_maxAngleChange;
		}
		return result;
	}

	public float GetLaserWidth()
	{
		return (!this.m_abilityMod) ? this.m_laserWidth : this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_laserWidth);
	}

	public float GetLaserLength()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_laserLengthMod.GetModifiedValue(this.m_laserLength);
		}
		else
		{
			result = this.m_laserLength;
		}
		return result;
	}

	public float GetLaserAoeRadius()
	{
		return (!this.m_abilityMod) ? this.m_aoeRadiusAtLaserEnd : this.m_abilityMod.m_aoeRadiusAtLaserEndMod.GetModifiedValue(this.m_aoeRadiusAtLaserEnd);
	}

	public float GetDiscReturnEndRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_discReturnEndRadiusMod.GetModifiedValue(this.m_discReturnEndRadius);
		}
		else
		{
			result = this.m_discReturnEndRadius;
		}
		return result;
	}

	public float GetChargeRadius()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_chargeRadiusMod.GetModifiedValue(this.m_chargeRadius);
		}
		else
		{
			result = this.m_chargeRadius;
		}
		return result;
	}

	public float GetChargeRadiusAtStart()
	{
		return (!this.m_abilityMod) ? this.m_chargeRadiusAtStart : this.m_abilityMod.m_chargeRadiusAtStartMod.GetModifiedValue(this.m_chargeRadiusAtStart);
	}

	public float GetChargeRadiusAtEnd()
	{
		return (!this.m_abilityMod) ? this.m_chargeRadiusAtEnd : this.m_abilityMod.m_chargeRadiusAtEndMod.GetModifiedValue(this.m_chargeRadiusAtEnd);
	}

	public float GetExplosionRadiusAtTargetedDisc()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_explosionRadiusAtTargetedDiscMod.GetModifiedValue(this.m_explosionRadiusAtTargetedDisc);
		}
		else
		{
			result = this.m_explosionRadiusAtTargetedDisc;
		}
		return result;
	}

	public bool ContinueToEndIfTargetEvades()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_continueToEndIfTargetEvadesMod.GetModifiedValue(this.m_continueToEndIfTargetEvades);
		}
		else
		{
			result = this.m_continueToEndIfTargetEvades;
		}
		return result;
	}

	public bool LeaveDiscAtStartSquare()
	{
		return (!this.m_abilityMod) ? this.m_leaveDiscAtStartSquare : this.m_abilityMod.m_leaveDiscAtStartSquareMod.GetModifiedValue(this.m_leaveDiscAtStartSquare);
	}

	public bool ThrowDiscFromStart()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_throwDiscFromStartMod.GetModifiedValue(this.m_throwDiscFromStart);
		}
		else
		{
			result = this.m_throwDiscFromStart;
		}
		return result;
	}

	public bool CanMoveAfterEvade()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_canMoveAfterEvadeMod.GetModifiedValue(this.m_canMoveAfterEvade);
		}
		else
		{
			result = this.m_canMoveAfterEvade;
		}
		return result;
	}

	public bool ExplodeTargetedDisc()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_explodeTargetedDiscMod.GetModifiedValue(this.m_explodeTargetedDisc);
		}
		else
		{
			result = this.m_explodeTargetedDisc;
		}
		return result;
	}

	public int GetDiscMaxTargets()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_discMaxTargetsMod.GetModifiedValue(this.m_discMaxTargets);
		}
		else
		{
			result = this.m_discMaxTargets;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnSelf != null)
		{
			result = this.m_cachedEffectOnSelf;
		}
		else
		{
			result = this.m_effectOnSelf;
		}
		return result;
	}

	public int GetDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damage);
		}
		else
		{
			result = this.m_damage;
		}
		return result;
	}

	public int GetDiscDirectDamage()
	{
		return (!this.m_abilityMod) ? this.m_discDirectDamage : this.m_abilityMod.m_discDirectDamageMod.GetModifiedValue(this.m_discDirectDamage);
	}

	public int GetDiscReturnTripDamage()
	{
		return (!this.m_abilityMod) ? this.m_discReturnTripDamage : this.m_abilityMod.m_discReturnTripDamageMod.GetModifiedValue(this.m_discReturnTripDamage);
	}

	public int GetDiscReturnTripSubsequentHitDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_discReturnTripSubsequentHitDamageMod.GetModifiedValue(this.m_discReturnTripSubsequentHitDamage);
		}
		else
		{
			result = this.m_discReturnTripSubsequentHitDamage;
		}
		return result;
	}

	public bool ReturnTripIgnoreCover()
	{
		return (!this.m_abilityMod) ? this.m_returnTripIgnoreCover : this.m_abilityMod.m_returnTripIgnoreCoverMod.GetModifiedValue(this.m_returnTripIgnoreCover);
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyHitEffect != null)
		{
			result = this.m_cachedEnemyHitEffect;
		}
		else
		{
			result = this.m_enemyHitEffect;
		}
		return result;
	}

	public int GetExplodingTargetDiscDamage()
	{
		return (!this.m_abilityMod) ? this.m_explodingTargetDiscDamage : this.m_abilityMod.m_explodingTargetDiscDamageMod.GetModifiedValue(this.m_explodingTargetDiscDamage);
	}

	public int GetDiscsReturningThisTurnExtraDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_discsReturningThisTurnExtraDamageMod.GetModifiedValue(this.m_discsReturningThisTurnExtraDamage);
		}
		else
		{
			result = this.m_discsReturningThisTurnExtraDamage;
		}
		return result;
	}

	public int GetCdrIfHasReturnDiscHit()
	{
		return (!this.m_abilityMod) ? this.m_cdrIfHasReturnDiscHit : this.m_abilityMod.m_cdrIfHasReturnDiscHitMod.GetModifiedValue(this.m_cdrIfHasReturnDiscHit);
	}

	public int GetCdrOnEnlargeDiscIfCastSameTurn()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_cdrOnEnlargeDiscIfCastSameTurnMod.GetModifiedValue(this.m_cdrOnEnlargeDiscIfCastSameTurn);
		}
		else
		{
			result = this.m_cdrOnEnlargeDiscIfCastSameTurn;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "DiscMaxTargets", string.Empty, this.m_discMaxTargets, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnSelf, "EffectOnSelf", this.m_effectOnSelf, true);
		base.AddTokenInt(tokens, "Damage", string.Empty, this.m_damage, false);
		base.AddTokenInt(tokens, "DiscDirectDamage", string.Empty, this.m_discDirectDamage, false);
		base.AddTokenInt(tokens, "DiscReturnTripDamage", string.Empty, this.m_discReturnTripDamage, false);
		base.AddTokenInt(tokens, "DiscReturnTripSubsequentHitDamage", string.Empty, this.m_discReturnTripSubsequentHitDamage, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffect, "EnemyHitEffect", this.m_enemyHitEffect, true);
		base.AddTokenInt(tokens, "ExplodingTargetDiscDamage", string.Empty, this.m_explodingTargetDiscDamage, false);
		base.AddTokenInt(tokens, "DiscsReturningThisTurnExtraDamage", string.Empty, this.m_discsReturningThisTurnExtraDamage, false);
		base.AddTokenInt(tokens, "CdrIfHasReturnDiscHit", string.Empty, this.m_cdrIfHasReturnDiscHit, false);
		base.AddTokenInt(tokens, "CdrOnEnlargeDiscIfCastSameTurn", string.Empty, this.m_cdrOnEnlargeDiscIfCastSameTurn, false);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.CanTargetDiscs() && this.m_syncComp != null)
		{
			List<BoardSquare> activeDiscSquares = this.m_syncComp.GetActiveDiscSquares();
			using (List<BoardSquare>.Enumerator enumerator = activeDiscSquares.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoardSquare dest = enumerator.Current;
					if (caster.GetActorMovement().CanMoveToBoardSquare(dest))
					{
						return true;
					}
				}
			}
			if (!this.CanTargetEnemies())
			{
				return false;
			}
		}
		if (this.CanTargetEnemies())
		{
			return base.HasTargetableActorsInDecision(caster, true, false, false, Ability.ValidateCheckPath.CanBuildPath, true, false, false);
		}
		return base.CustomCanCastValidation(caster);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = false;
		bool flag2 = false;
		if (targetIndex == this.m_dashTargeterIndex)
		{
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
			if (this.CanTargetEnemies())
			{
				if (boardSquareSafe != null && boardSquareSafe.OccupantActor != null)
				{
					if (base.CanTargetActorInDecision(caster, boardSquareSafe.OccupantActor, true, false, false, Ability.ValidateCheckPath.CanBuildPath, true, false, false))
					{
						flag = true;
						flag2 = true;
						goto IL_1F8;
					}
				}
			}
			if (this.CanTargetDiscs())
			{
				if (this.m_syncComp != null)
				{
					List<BoardSquare> activeDiscSquares = this.m_syncComp.GetActiveDiscSquares();
					if (activeDiscSquares.Contains(boardSquareSafe))
					{
						flag = true;
						float num = boardSquareSafe.HorizontalDistanceInSquaresTo(caster.GetCurrentBoardSquare());
						if (num <= this.GetRangeInSquares(this.m_dashTargeterIndex))
						{
							if (num >= base.GetMinRangeInSquares(this.m_dashTargeterIndex))
							{
								int num2;
								flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out num2);
								goto IL_150;
							}
						}
						flag2 = false;
					}
					IL_150:
					goto IL_1F8;
				}
			}
			if (!this.CanTargetEnemies())
			{
				if (!this.CanTargetDiscs())
				{
					flag = true;
					bool flag3;
					if (this.GetDashTargetRange() > 0f)
					{
						if (boardSquareSafe != null)
						{
							float num3 = boardSquareSafe.HorizontalDistanceInSquaresTo(caster.GetCurrentBoardSquare());
							flag3 = (num3 <= this.GetDashTargetRange());
							goto IL_1D2;
						}
					}
					flag3 = base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
					IL_1D2:
					bool flag4;
					if (flag3)
					{
						int num4;
						flag4 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out num4);
					}
					else
					{
						flag4 = false;
					}
					flag2 = flag4;
				}
			}
			IL_1F8:;
		}
		else
		{
			if (targetIndex != this.m_landingTargeterIndex)
			{
				return true;
			}
			flag = true;
			BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[targetIndex - 1].GridPos);
			BoardSquare boardSquareSafe3 = Board.Get().GetBoardSquareSafe(target.GridPos);
			if (boardSquareSafe3 != null && boardSquareSafe3.IsBaselineHeight())
			{
				if (boardSquareSafe3 != boardSquareSafe2)
				{
					if (boardSquareSafe3 != caster.GetCurrentBoardSquare())
					{
						float num5 = boardSquareSafe3.HorizontalDistanceInSquaresTo(boardSquareSafe2);
						Vector3 to = boardSquareSafe3.ToVector3() - boardSquareSafe2.ToVector3();
						Vector3 from = boardSquareSafe2.ToVector3() - caster.GetCurrentBoardSquare().ToVector3();
						float num6 = Vector3.Angle(from, to);
						if (num5 >= this.m_minDistanceFromTarget)
						{
							if (num5 <= this.m_maxDistanceFromTarget && num6 <= this.m_maxAngleChange)
							{
								int num7;
								flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe3, boardSquareSafe2, false, out num7);
							}
						}
					}
				}
			}
		}
		return flag2 && flag;
	}

	public override bool CanOverrideMoveStartSquare()
	{
		return this.CanMoveAfterEvade();
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, this.GetExplodingTargetDiscDamage()),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.HighPower, this.GetDamage()),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.GetDiscDirectDamage())
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserLength() + this.GetLaserAoeRadius();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_NekoFlipDash))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_NekoFlipDash);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
