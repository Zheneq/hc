using System.Collections.Generic;
using UnityEngine;

public class ScampHug : Ability
{
	public enum TargetingMode
	{
		Laser,
		OnSquare
	}

	[Separator("Knockback Targeting", true)]
	public TargetingMode m_targetingMode;

	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;

	public float m_knockbackWidth = 1f;

	public float m_knockbackMaxRange = 10f;

	[Separator("On Hit", true)]
	public int m_knockbackDirectHitDamage = 20;

	public int m_knockbackAoeDamage = 10;

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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "ScampHug";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
		base.Targeter = new AbilityUtil_Targeter_ScampHug(this, m_syncComp, m_targetingMode, m_knockbackWidth, m_knockbackMaxRange, m_aoeShape, true, m_enemyKnockbackDist, m_enemyKnockbackType);
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
		int result;
		if (m_syncComp != null)
		{
			result = (m_syncComp.m_suitWasActiveOnTurnStart ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (IsInSuit())
		{
			if (m_targetingMode == TargetingMode.Laser)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe != caster.GetCurrentBoardSquare() && boardSquareSafe.IsBaselineHeight())
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
		return false;
	}

	public override AbilityPriority GetRunPriority()
	{
		if (IsInSuit())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return AbilityPriority.Combat_Knockback;
				}
			}
		}
		return AbilityPriority.Evasion;
	}

	public override TargetData[] GetTargetData()
	{
		if (IsInSuit())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return base.GetTargetData();
				}
			}
		}
		return m_evadeTargetData;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		if (IsInSuit())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return ActorData.MovementType.None;
				}
			}
		}
		return ActorData.MovementType.Charge;
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (IsInSuit())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return base.GetActionAnimType();
				}
			}
		}
		return m_evadeAnimType;
	}

	public override int GetBaseCooldown()
	{
		if (!IsInSuit())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_evadeCooldown;
				}
			}
		}
		return base.GetBaseCooldown();
	}

	public static void GetHitActorsAndKnockbackDestinationStatic(AbilityTarget currentTarget, ActorData caster, TargetingMode targetingMode, bool includeInvisibles, float knockbackWidth, float knockbackRange, AbilityAreaShape aoeShape, out ActorData firstHitActor, out List<ActorData> aoeHitActors, out BoardSquare knockbackDestSquare)
	{
		firstHitActor = null;
		aoeHitActors = new List<ActorData>();
		knockbackDestSquare = null;
		if (targetingMode == TargetingMode.Laser)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
					Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, knockbackRange * Board.Get().squareSize, false, caster);
					BoardSquare lastValidBoardSquareInLine = KnockbackUtils.GetLastValidBoardSquareInLine(travelBoardSquareWorldPositionForLos, laserEndPoint);
					Vector3 vector = lastValidBoardSquareInLine.ToVector3() - travelBoardSquareWorldPositionForLos;
					vector.y = 0f;
					float distance = Mathf.Max(0.1f, vector.magnitude / Board.SquareSizeStatic);
					BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildKnockbackPath(caster, KnockbackType.ForwardAlongAimDir, currentTarget.AimDirection, Vector3.zero, distance);
					BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo.GetPathEndpoint();
					Vector3 lhs = boardSquarePathInfo2.square.ToVector3() - travelBoardSquareWorldPositionForLos;
					lhs.y = 0f;
					float d = Vector3.Dot(lhs, currentTarget.AimDirection) + 0.5f;
					laserEndPoint = travelBoardSquareWorldPositionForLos + d * currentTarget.AimDirection;
					float magnitude = (laserEndPoint - travelBoardSquareWorldPositionForLos).magnitude;
					Vector3 laserEndPos;
					List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, magnitude / Board.Get().squareSize, knockbackWidth, caster, caster.GetEnemyTeams(), true, 1, true, includeInvisibles, out laserEndPos, null);
					if (actorsInLaser.Count > 0)
					{
						firstHitActor = actorsInLaser[0];
						BoardSquare currentBoardSquare = firstHitActor.GetCurrentBoardSquare();
						if (currentBoardSquare != null)
						{
							aoeHitActors = AreaEffectUtils.GetActorsInShape(aoeShape, currentBoardSquare.ToVector3(), currentBoardSquare, false, caster, caster.GetEnemyTeam(), null);
							if (!includeInvisibles)
							{
								TargeterUtils.RemoveActorsInvisibleToClient(ref aoeHitActors);
							}
							aoeHitActors.Remove(firstHitActor);
							laserEndPoint = currentBoardSquare.ToVector3();
						}
						Vector3 lhs2 = laserEndPoint - travelBoardSquareWorldPositionForLos;
						lhs2.y = 0f;
						float num = Vector3.Dot(lhs2, currentTarget.AimDirection);
						while (boardSquarePathInfo2 != null)
						{
							if (boardSquarePathInfo2.prev == null)
							{
								break;
							}
							Vector3 vector2 = boardSquarePathInfo2.square.ToVector3() - travelBoardSquareWorldPositionForLos;
							vector2.y = 0f;
							if (!(vector2.magnitude > num + 0.71f))
							{
								break;
							}
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									goto end_IL_025a;
								}
								continue;
								end_IL_025a:
								break;
							}
							boardSquarePathInfo2.prev.next = null;
							boardSquarePathInfo2 = boardSquarePathInfo2.prev;
						}
					}
					knockbackDestSquare = boardSquarePathInfo2.square;
					return;
				}
				}
			}
		}
		BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTarget.GridPos);
		if (boardSquareSafe != null)
		{
			aoeHitActors = AreaEffectUtils.GetActorsInShape(aoeShape, boardSquareSafe.ToVector3(), boardSquareSafe, false, caster, caster.GetEnemyTeam(), null);
			if (!includeInvisibles)
			{
				TargeterUtils.RemoveActorsInvisibleToClient(ref aoeHitActors);
			}
		}
		knockbackDestSquare = boardSquareSafe;
	}
}
