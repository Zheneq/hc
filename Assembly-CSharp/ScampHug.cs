using System;
using System.Collections.Generic;
using UnityEngine;

public class ScampHug : Ability
{
	[Separator("Knockback Targeting", true)]
	public ScampHug.TargetingMode m_targetingMode;

	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;

	public float m_knockbackWidth = 1f;

	public float m_knockbackMaxRange = 10f;

	[Separator("On Hit", true)]
	public int m_knockbackDirectHitDamage = 0x14;

	public int m_knockbackAoeDamage = 0xA;

	public StandardEffectInfo m_knockbackDirectHitEffect;

	public StandardEffectInfo m_knockbackAoeHitEffect;

	[Header("-- Knockback distance and type")]
	public float m_enemyKnockbackDist;

	public KnockbackType m_enemyKnockbackType = KnockbackType.AwayFromSource;

	[Separator("Evasion Phase Params", true)]
	public int m_evadeCooldown = 2;

	public TargetData[] m_evadeTargetData;

	[Header("-- Evasion Phase Animation --")]
	public ActorModelData.ActionAnimationType m_evadeAnimType = ActorModelData.ActionAnimationType.Ability5;

	[Separator("Sequences", true)]
	public GameObject m_knockbackCastSequencePrefab;

	public GameObject m_evasionCastSequencePrefab;

	private Scamp_SyncComponent m_syncComp;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "ScampHug";
		}
		this.Setup();
	}

	private void Setup()
	{
		this.m_syncComp = base.GetComponent<Scamp_SyncComponent>();
		base.Targeter = new AbilityUtil_Targeter_ScampHug(this, this.m_syncComp, this.m_targetingMode, this.m_knockbackWidth, this.m_knockbackMaxRange, this.m_aoeShape, true, this.m_enemyKnockbackDist, this.m_enemyKnockbackType);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_knockbackDirectHitDamage);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_knockbackAoeDamage);
		return result;
	}

	public bool IsInSuit()
	{
		bool result;
		if (this.m_syncComp != null)
		{
			result = this.m_syncComp.m_suitWasActiveOnTurnStart;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (this.IsInSuit())
		{
			if (this.m_targetingMode == ScampHug.TargetingMode.Laser)
			{
				return true;
			}
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe != caster.GetCurrentBoardSquare() && boardSquareSafe.IsBaselineHeight())
			{
				int num;
				return KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out num);
			}
		}
		return false;
	}

	public override AbilityPriority GetRunPriority()
	{
		if (this.IsInSuit())
		{
			return AbilityPriority.Combat_Knockback;
		}
		return AbilityPriority.Evasion;
	}

	public override TargetData[] GetTargetData()
	{
		if (this.IsInSuit())
		{
			return base.GetTargetData();
		}
		return this.m_evadeTargetData;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		if (this.IsInSuit())
		{
			return ActorData.MovementType.None;
		}
		return ActorData.MovementType.Charge;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (this.IsInSuit())
		{
			return base.GetActionAnimType();
		}
		return this.m_evadeAnimType;
	}

	public override int GetBaseCooldown()
	{
		if (!this.IsInSuit())
		{
			return this.m_evadeCooldown;
		}
		return base.GetBaseCooldown();
	}

	public unsafe static void GetHitActorsAndKnockbackDestinationStatic(AbilityTarget currentTarget, ActorData caster, ScampHug.TargetingMode targetingMode, bool includeInvisibles, float knockbackWidth, float knockbackRange, AbilityAreaShape aoeShape, out ActorData firstHitActor, out List<ActorData> aoeHitActors, out BoardSquare knockbackDestSquare)
	{
		firstHitActor = null;
		aoeHitActors = new List<ActorData>();
		knockbackDestSquare = null;
		if (targetingMode == ScampHug.TargetingMode.Laser)
		{
			Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
			Vector3 vector = VectorUtils.GetLaserEndPoint(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, knockbackRange * Board.Get().squareSize, false, caster, null, true);
			BoardSquare lastValidBoardSquareInLine = KnockbackUtils.GetLastValidBoardSquareInLine(travelBoardSquareWorldPositionForLos, vector, false, false, float.MaxValue);
			Vector3 vector2 = lastValidBoardSquareInLine.ToVector3() - travelBoardSquareWorldPositionForLos;
			vector2.y = 0f;
			float distance = Mathf.Max(0.1f, vector2.magnitude / Board.SquareSizeStatic);
			BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildKnockbackPath(caster, KnockbackType.ForwardAlongAimDir, currentTarget.AimDirection, Vector3.zero, distance);
			BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo.GetPathEndpoint();
			Vector3 lhs = boardSquarePathInfo2.square.ToVector3() - travelBoardSquareWorldPositionForLos;
			lhs.y = 0f;
			float d = Vector3.Dot(lhs, currentTarget.AimDirection) + 0.5f;
			vector = travelBoardSquareWorldPositionForLos + d * currentTarget.AimDirection;
			float magnitude = (vector - travelBoardSquareWorldPositionForLos).magnitude;
			Vector3 vector3;
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, magnitude / Board.Get().squareSize, knockbackWidth, caster, caster.GetOpposingTeams(), true, 1, true, includeInvisibles, out vector3, null, null, false, true);
			if (actorsInLaser.Count > 0)
			{
				firstHitActor = actorsInLaser[0];
				BoardSquare currentBoardSquare = firstHitActor.GetCurrentBoardSquare();
				if (currentBoardSquare != null)
				{
					aoeHitActors = AreaEffectUtils.GetActorsInShape(aoeShape, currentBoardSquare.ToVector3(), currentBoardSquare, false, caster, caster.GetOpposingTeam(), null);
					if (!includeInvisibles)
					{
						TargeterUtils.RemoveActorsInvisibleToClient(ref aoeHitActors);
					}
					aoeHitActors.Remove(firstHitActor);
					vector = currentBoardSquare.ToVector3();
				}
				Vector3 lhs2 = vector - travelBoardSquareWorldPositionForLos;
				lhs2.y = 0f;
				float num = Vector3.Dot(lhs2, currentTarget.AimDirection);
				while (boardSquarePathInfo2 != null)
				{
					if (boardSquarePathInfo2.prev == null)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							goto IL_29F;
						}
					}
					else
					{
						Vector3 vector4 = boardSquarePathInfo2.square.ToVector3() - travelBoardSquareWorldPositionForLos;
						vector4.y = 0f;
						if (vector4.magnitude <= num + 0.71f)
						{
							break;
						}
						boardSquarePathInfo2.prev.next = null;
						boardSquarePathInfo2 = boardSquarePathInfo2.prev;
					}
				}
			}
			IL_29F:
			knockbackDestSquare = boardSquarePathInfo2.square;
		}
		else
		{
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
			if (boardSquareSafe != null)
			{
				aoeHitActors = AreaEffectUtils.GetActorsInShape(aoeShape, boardSquareSafe.ToVector3(), boardSquareSafe, false, caster, caster.GetOpposingTeam(), null);
				if (!includeInvisibles)
				{
					TargeterUtils.RemoveActorsInvisibleToClient(ref aoeHitActors);
				}
			}
			knockbackDestSquare = boardSquareSafe;
		}
	}

	public enum TargetingMode
	{
		Laser,
		OnSquare
	}
}
