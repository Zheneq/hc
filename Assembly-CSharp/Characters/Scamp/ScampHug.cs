using System.Collections.Generic;
using UnityEngine;

public class ScampHug : Ability
{
	public enum TargetingMode
	{
		Laser,
		OnSquare
	}

	[Separator("Knockback Targeting")]
	public TargetingMode m_targetingMode;
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;
	public float m_knockbackWidth = 1f;
	public float m_knockbackMaxRange = 10f;
	[Separator("On Hit")]
	public int m_knockbackDirectHitDamage = 20;
	public int m_knockbackAoeDamage = 10;
	public StandardEffectInfo m_knockbackDirectHitEffect;
	public StandardEffectInfo m_knockbackAoeHitEffect;
	[Header("-- Knockback distance and type")]
	public float m_enemyKnockbackDist;
	public KnockbackType m_enemyKnockbackType = KnockbackType.AwayFromSource;
	[Separator("Evasion Phase Params")]
	public int m_evadeCooldown = 2;
	public TargetData[] m_evadeTargetData;
	[Header("-- Evasion Phase Animation --")]
	public ActorModelData.ActionAnimationType m_evadeAnimType = ActorModelData.ActionAnimationType.Ability5;
	[Separator("Sequences")]
	public GameObject m_knockbackCastSequencePrefab;
	public GameObject m_evasionCastSequencePrefab;

	private Scamp_SyncComponent m_syncComp;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "ScampHug";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		Targeter = new AbilityUtil_Targeter_ScampHug(
			this,
			m_syncComp,
			m_targetingMode,
			m_knockbackWidth,
			m_knockbackMaxRange,
			m_aoeShape,
			true,
			m_enemyKnockbackDist,
			m_enemyKnockbackType);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_knockbackDirectHitDamage);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, m_knockbackAoeDamage);
		return numbers;
	}

	public bool IsInSuit()
	{
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (IsInSuit() && m_targetingMode == TargetingMode.Laser)
		{
			return true;
		}
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		return targetSquare != null
		       && targetSquare != caster.GetCurrentBoardSquare()
		       && targetSquare.IsValidForGameplay()
		       && KnockbackUtils.CanBuildStraightLineChargePath(caster, targetSquare, caster.GetCurrentBoardSquare(), false, out _);
	}

	public override AbilityPriority GetRunPriority()
	{
		return IsInSuit()
			? AbilityPriority.Combat_Knockback
			: AbilityPriority.Evasion;
	}

	public override TargetData[] GetTargetData()
	{
		return IsInSuit()
			? base.GetTargetData()
			: m_evadeTargetData;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return IsInSuit()
			? ActorData.MovementType.None
			: ActorData.MovementType.Charge;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		return IsInSuit()
			? base.GetActionAnimType()
			: m_evadeAnimType;
	}

	public override int GetBaseCooldown()
	{
		return IsInSuit()
			? base.GetBaseCooldown()
			: m_evadeCooldown;
	}

	public static void GetHitActorsAndKnockbackDestinationStatic(
		AbilityTarget currentTarget,
		ActorData caster,
		TargetingMode targetingMode,
		bool includeInvisibles,
		float knockbackWidth,
		float knockbackRange,
		AbilityAreaShape aoeShape,
		out ActorData firstHitActor,
		out List<ActorData> aoeHitActors,
		out BoardSquare knockbackDestSquare)
	{
		firstHitActor = null;
		aoeHitActors = new List<ActorData>();
		knockbackDestSquare = null;
		if (targetingMode == TargetingMode.Laser)
		{
			Vector3 start = caster.GetLoSCheckPos();
			Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(
				start,
				currentTarget.AimDirection,
				knockbackRange * Board.Get().squareSize,
				false,
				caster);
			BoardSquare lastValidBoardSquareInLine = KnockbackUtils.GetLastValidBoardSquareInLine(start, laserEndPoint);
			Vector3 vector = lastValidBoardSquareInLine.ToVector3() - start;
			vector.y = 0f;
			float distance = Mathf.Max(0.1f, vector.magnitude / Board.SquareSizeStatic);
			BoardSquarePathInfo knockbackPath = KnockbackUtils.BuildKnockbackPath(
				caster,
				KnockbackType.ForwardAlongAimDir,
				currentTarget.AimDirection,
				Vector3.zero,
				distance);
			BoardSquarePathInfo knockbackStep = knockbackPath.GetPathEndpoint();
			Vector3 lhs = knockbackStep.square.ToVector3() - start;
			lhs.y = 0f;
			float d = Vector3.Dot(lhs, currentTarget.AimDirection) + 0.5f;
			laserEndPoint = start + d * currentTarget.AimDirection;
			float magnitude = (laserEndPoint - start).magnitude;
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
				start,
				currentTarget.AimDirection,
				magnitude / Board.Get().squareSize,
				knockbackWidth,
				caster,
				caster.GetEnemyTeamAsList(),
				true,
				1,
				true,
				includeInvisibles,
				out _,
				null);
			if (actorsInLaser.Count > 0)
			{
				firstHitActor = actorsInLaser[0];
				BoardSquare firstTargetSquare = firstHitActor.GetCurrentBoardSquare();
				if (firstTargetSquare != null)
				{
					aoeHitActors = AreaEffectUtils.GetActorsInShape(
						aoeShape,
						firstTargetSquare.ToVector3(),
						firstTargetSquare,
						false,
						caster,
						caster.GetEnemyTeam(),
						null);
					if (!includeInvisibles)
					{
						TargeterUtils.RemoveActorsInvisibleToClient(ref aoeHitActors);
					}
					aoeHitActors.Remove(firstHitActor);
					laserEndPoint = firstTargetSquare.ToVector3();
				}
				Vector3 laserVector = laserEndPoint - start;
				laserVector.y = 0f;
				float num = Vector3.Dot(laserVector, currentTarget.AimDirection);
				while (knockbackStep != null)
				{
					if (knockbackStep.prev == null)
					{
						break;
					}
					Vector3 knockbackStepVector = knockbackStep.square.ToVector3() - start;
					knockbackStepVector.y = 0f;
					if (knockbackStepVector.magnitude <= num + 0.71f)
					{
						break;
					}
					knockbackStep.prev.next = null;
					knockbackStep = knockbackStep.prev;
				}
			}
			knockbackDestSquare = knockbackStep.square;
		}
		else
		{
			BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTarget.GridPos);
			if (boardSquareSafe != null)
			{
				aoeHitActors = AreaEffectUtils.GetActorsInShape(
					aoeShape,
					boardSquareSafe.ToVector3(),
					boardSquareSafe,
					false,
					caster,
					caster.GetEnemyTeam(),
					null);
				if (!includeInvisibles)
				{
					TargeterUtils.RemoveActorsInvisibleToClient(ref aoeHitActors);
				}
			}
			knockbackDestSquare = boardSquareSafe;
		}
	}
}
