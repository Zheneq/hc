using System.Collections.Generic;
using UnityEngine;

public class GremlinsBombingRun : Ability
{
	[Header("-- Targeter ------------------------------------")]
	public bool m_targeterMultiStep = true;
	public int m_squaresPerExplosion = 3;
	[Header("-- Multi-Step Targeter Only --")]
	public int m_maxSquaresPerStep = 6;
	public float m_maxAngleWithFirstStep = 45f;
	[Header("-- Explosion ------------------------------------")]
	public int m_explosionDamageAmount = 5;
	public AbilityAreaShape m_explosionShape = AbilityAreaShape.Three_x_Three;
	[Header("-- Sequences ------------------------------------")]
	public GameObject m_castSequencePrefab;
	public GameObject m_animListenerSequencePrefab;

	private AbilityMod_GremlinsBombingRun m_abilityMod;
	private int m_numSteps = 1;
	private GremlinsLandMineInfoComponent m_bombInfoComp;

	public AbilityMod_GremlinsBombingRun GetMod()
	{
		return m_abilityMod;
	}

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Bombing Run";
		}
		SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return (GetMaxSquaresPerJump() - 1) * m_numSteps;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount;
	}

	public int GetMinSquaresPerJump()
	{
		int squaresPerExplosion = m_abilityMod != null
			? m_abilityMod.m_minSquaresPerExplosionMod.GetModifiedValue(m_squaresPerExplosion)
			: m_squaresPerExplosion;
		return Mathf.Max(1, squaresPerExplosion);
	}

	public int GetMaxSquaresPerJump()
	{
		int maxSquaresPerStep = m_abilityMod != null
			? m_abilityMod.m_maxSquaresPerExplosionMod.GetModifiedValue(m_maxSquaresPerStep)
			: m_maxSquaresPerStep;
		if (maxSquaresPerStep > 0 && maxSquaresPerStep < GetMinSquaresPerJump())
		{
			maxSquaresPerStep = GetMinSquaresPerJump() + 1;
		}
		return maxSquaresPerStep;
	}

	public float GetMaxAngleWithFirstSegment()
	{
		return m_abilityMod != null
			? m_abilityMod.m_angleWithFirstStepMod.GetModifiedValue(m_maxAngleWithFirstStep)
			: m_maxAngleWithFirstStep;
	}

	public AbilityAreaShape GetExplosionShape()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionShapeMod.GetModifiedValue(m_explosionShape)
			: m_explosionShape;
	}

	public bool ShouldLeaveMinesAtTouchedSquares()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_shouldLeaveMinesAtTouchedSquares.GetModifiedValue(false);
	}

	private void SetupTargeter()
	{
		if (m_bombInfoComp == null)
		{
			m_bombInfoComp = GetComponent<GremlinsLandMineInfoComponent>();
		}
		m_numSteps = m_targeterMultiStep
			? Mathf.Max(GetNumTargets(), 1)
			: 1;
		if (m_numSteps < 2)
		{
			Targeter = new AbilityUtil_Targeter_BombingRun(this, GetExplosionShape(), GetMinSquaresPerJump());
		}
		else
		{
			ClearTargeters();
			for (int i = 0; i < m_numSteps; i++)
			{
				Targeters.Add(new AbilityUtil_Targeter_BombingRun(this, GetExplosionShape(), GetMinSquaresPerJump()));
				Targeters[i].SetUseMultiTargetUpdate(true);
			}
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		return numbers;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return m_numSteps;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool isValid = true;
		BoardSquare square = Board.Get().GetSquare(target.GridPos);
		if (square == null || !square.IsValidForGameplay())
		{
			return false;
		}
		if (m_numSteps < 2)
		{
			isValid = KnockbackUtils.CanBuildStraightLineChargePath(
				caster,
				square,
				caster.GetCurrentBoardSquare(),
				false,
				out _);
		}
		else
		{
			if (targetIndex > 0)
			{
				BoardSquare square2 = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos);
				if (square == square2)
				{
					isValid = false;
				}
				else if (GetMaxAngleWithFirstSegment() > 0f)
				{
					Vector3 b = targetIndex > 1
						? Board.Get().GetSquare(currentTargets[targetIndex - 2].GridPos).ToVector3()
						: caster.GetFreePos();
					Vector3 a = Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos).ToVector3();
					Vector3 from = a - b;
					from.y = 0f;
					Vector3 to = square.ToVector3() - square2.ToVector3();
					if (Mathf.RoundToInt(Vector3.Angle(from, to)) > Mathf.RoundToInt(GetMaxAngleWithFirstSegment()))
					{
						isValid = false;
					}
				}
			}
			if (isValid)
			{
				bool canJump;
				int numSquaresInPath;
				if (targetIndex == 0)
				{
					canJump = KnockbackUtils.CanBuildStraightLineChargePath(
						caster,
						square,
						caster.GetCurrentBoardSquare(),
						false,
						out numSquaresInPath);
				}
				else
				{
					canJump = KnockbackUtils.CanBuildStraightLineChargePath(
						caster,
						square,
						Board.Get().GetSquare(currentTargets[targetIndex - 1].GridPos),
						false,
						out numSquaresInPath);
				}

				isValid = canJump
				       && isValid
				       && numSquaresInPath > GetMinSquaresPerJump()
				       && (GetMaxSquaresPerJump() <= 0 || numSquaresInPath <= GetMaxSquaresPerJump());
				if (isValid && targetIndex < m_numSteps - 1)
				{
					List<BoardSquare> list = new List<BoardSquare>();
					list.Add(caster.GetCurrentBoardSquare());
					for (int i = 0; i < targetIndex; i++)
					{
						if (i >= currentTargets.Count)
						{
							break;
						}
						list.Add(Board.Get().GetSquare(currentTargets[i].GridPos));
					}
					isValid = CanTargetForNextClick(square, targetIndex + 1, list, caster);
				}
			}
		}
		return isValid;
	}

	private bool CanTargetForNextClick(
		BoardSquare fromSquare,
		int nextTargetIndex,
		List<BoardSquare> squaresAddedSoFar,
		ActorData caster)
	{
		if (nextTargetIndex >= m_numSteps)
		{
			return true;
		}
		if (nextTargetIndex <= 0 || squaresAddedSoFar.Count <= 0 || fromSquare == null)
		{
			return false;
		}
		bool isValid = false;
		Vector3 prevPos = squaresAddedSoFar[squaresAddedSoFar.Count - 1].ToVector3();
		Vector3 curPos = fromSquare.ToVector3();
		Vector3 prevJump = curPos - prevPos;
		prevJump.y = 0f;
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(prevJump);
		float coneWidthDegrees = Mathf.Clamp(2f * GetMaxAngleWithFirstSegment() + 10f, 0f, 360f);
		if (GetMaxAngleWithFirstSegment() <= 0f)
		{
			coneWidthDegrees = 360f;
		}
		float coneLengthRadiusInSquares = GetMaxSquaresPerJump() * 1.42f;
		List<BoardSquare> squaresInCone = AreaEffectUtils.GetSquaresInCone(
			fromSquare.ToVector3(),
			coneCenterAngleDegrees,
			coneWidthDegrees,
			coneLengthRadiusInSquares,
			0f,
			false,
			caster);
		float minDistance = GetMinSquaresPerJump() * Board.Get().squareSize;
		foreach (BoardSquare squareInRange in squaresInCone)
		{
			if (isValid)
			{
				break;
			}
			if (!squareInRange.IsValidForGameplay() || squareInRange == fromSquare)
			{
				continue;
			}
			Vector3 nextJump = squareInRange.ToVector3() - fromSquare.ToVector3();
			nextJump.y = 0f;
			if (nextJump.magnitude < minDistance
			    || Mathf.RoundToInt(Vector3.Angle(prevJump, nextJump)) > Mathf.RoundToInt(GetMaxAngleWithFirstSegment()))
			{
				continue;
			}
			bool canJump = KnockbackUtils.CanBuildStraightLineChargePath(
				caster,
				squareInRange,
				fromSquare,
				false,
				out var numSquaresInPath);

			if (canJump
			    && numSquaresInPath > GetMinSquaresPerJump()
			    && (GetMaxSquaresPerJump() <= 0 || numSquaresInPath <= GetMaxSquaresPerJump()))
			{
				isValid = true;
			}
		}
		return isValid;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_GremlinsBombingRun abilityMod_GremlinsBombingRun = modAsBase as AbilityMod_GremlinsBombingRun;
		AddTokenInt(tokens, "Damage", string.Empty, abilityMod_GremlinsBombingRun != null
			? abilityMod_GremlinsBombingRun.m_damageMod.GetModifiedValue(m_explosionDamageAmount)
			: m_explosionDamageAmount);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_GremlinsBombingRun))
		{
			m_abilityMod = abilityMod as AbilityMod_GremlinsBombingRun;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}
}
