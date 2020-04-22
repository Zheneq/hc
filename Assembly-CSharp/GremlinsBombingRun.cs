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
		int result;
		if (m_abilityMod == null)
		{
			result = m_explosionDamageAmount;
		}
		else
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_explosionDamageAmount);
		}
		return result;
	}

	public int GetMinSquaresPerJump()
	{
		int b = (!(m_abilityMod == null)) ? m_abilityMod.m_minSquaresPerExplosionMod.GetModifiedValue(m_squaresPerExplosion) : m_squaresPerExplosion;
		return Mathf.Max(1, b);
	}

	public int GetMaxSquaresPerJump()
	{
		int num;
		if (m_abilityMod == null)
		{
			num = m_maxSquaresPerStep;
		}
		else
		{
			num = m_abilityMod.m_maxSquaresPerExplosionMod.GetModifiedValue(m_maxSquaresPerStep);
		}
		int num2 = num;
		if (num2 > 0 && num2 < GetMinSquaresPerJump())
		{
			num2 = GetMinSquaresPerJump() + 1;
		}
		return num2;
	}

	public float GetMaxAngleWithFirstSegment()
	{
		return (!(m_abilityMod == null)) ? m_abilityMod.m_angleWithFirstStepMod.GetModifiedValue(m_maxAngleWithFirstStep) : m_maxAngleWithFirstStep;
	}

	public AbilityAreaShape GetExplosionShape()
	{
		AbilityAreaShape result;
		if (m_abilityMod == null)
		{
			result = m_explosionShape;
		}
		else
		{
			result = m_abilityMod.m_explosionShapeMod.GetModifiedValue(m_explosionShape);
		}
		return result;
	}

	public bool ShouldLeaveMinesAtTouchedSquares()
	{
		int result;
		if (m_abilityMod == null)
		{
			result = 0;
		}
		else
		{
			result = (m_abilityMod.m_shouldLeaveMinesAtTouchedSquares.GetModifiedValue(false) ? 1 : 0);
		}
		return (byte)result != 0;
	}

	private void SetupTargeter()
	{
		if (m_bombInfoComp == null)
		{
			m_bombInfoComp = GetComponent<GremlinsLandMineInfoComponent>();
		}
		if (m_targeterMultiStep)
		{
			m_numSteps = Mathf.Max(GetNumTargets(), 1);
		}
		else
		{
			m_numSteps = 1;
		}
		if (m_numSteps < 2)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					base.Targeter = new AbilityUtil_Targeter_BombingRun(this, GetExplosionShape(), GetMinSquaresPerJump());
					return;
				}
			}
		}
		ClearTargeters();
		for (int i = 0; i < m_numSteps; i++)
		{
			base.Targeters.Add(new AbilityUtil_Targeter_BombingRun(this, GetExplosionShape(), GetMinSquaresPerJump()));
			base.Targeters[i].SetUseMultiTargetUpdate(true);
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
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
		bool flag = true;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				if (m_numSteps < 2)
				{
					int numSquaresInPath = 0;
					flag = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out numSquaresInPath);
				}
				else
				{
					if (targetIndex > 0)
					{
						BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[targetIndex - 1].GridPos);
						if (boardSquareSafe == boardSquareSafe2)
						{
							flag = false;
						}
						else if (GetMaxAngleWithFirstSegment() > 0f)
						{
							Vector3 vector;
							if (targetIndex > 1)
							{
								vector = Board.Get().GetBoardSquareSafe(currentTargets[targetIndex - 2].GridPos).ToVector3();
							}
							else
							{
								vector = caster.GetTravelBoardSquareWorldPosition();
							}
							Vector3 b = vector;
							Vector3 a = Board.Get().GetBoardSquareSafe(currentTargets[targetIndex - 1].GridPos).ToVector3();
							Vector3 from = a - b;
							from.y = 0f;
							Vector3 to = boardSquareSafe.ToVector3() - boardSquareSafe2.ToVector3();
							int num = Mathf.RoundToInt(Vector3.Angle(from, to));
							if (num > Mathf.RoundToInt(GetMaxAngleWithFirstSegment()))
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						bool flag2 = false;
						int numSquaresInPath2 = 0;
						if (targetIndex == 0)
						{
							flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out numSquaresInPath2);
						}
						else
						{
							flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, Board.Get().GetBoardSquareSafe(currentTargets[targetIndex - 1].GridPos), false, out numSquaresInPath2);
						}
						if (flag2)
						{
							int num2;
							if (flag)
							{
								num2 = ((numSquaresInPath2 > GetMinSquaresPerJump()) ? 1 : 0);
							}
							else
							{
								num2 = 0;
							}
							int num3;
							if (num2 != 0)
							{
								if (GetMaxSquaresPerJump() > 0)
								{
									num3 = ((numSquaresInPath2 <= GetMaxSquaresPerJump()) ? 1 : 0);
								}
								else
								{
									num3 = 1;
								}
							}
							else
							{
								num3 = 0;
							}
							flag = ((byte)num3 != 0);
						}
						else
						{
							flag = false;
						}
						if (flag)
						{
							if (targetIndex < m_numSteps - 1)
							{
								List<BoardSquare> list = new List<BoardSquare>();
								list.Add(caster.GetCurrentBoardSquare());
								for (int i = 0; i < targetIndex; i++)
								{
									if (i < currentTargets.Count)
									{
										list.Add(Board.Get().GetBoardSquareSafe(currentTargets[i].GridPos));
										continue;
									}
									break;
								}
								flag = CanTargetForNextClick(boardSquareSafe, targetIndex + 1, list, caster);
							}
						}
					}
				}
				return flag;
			}
		}
		return false;
	}

	private bool CanTargetForNextClick(BoardSquare fromSquare, int nextTargetIndex, List<BoardSquare> squaresAddedSoFar, ActorData caster)
	{
		if (nextTargetIndex >= m_numSteps)
		{
			return true;
		}
		bool flag = false;
		if (nextTargetIndex > 0)
		{
			if (squaresAddedSoFar.Count > 0)
			{
				if (fromSquare != null)
				{
					Vector3 b = squaresAddedSoFar[squaresAddedSoFar.Count - 1].ToVector3();
					Vector3 a = fromSquare.ToVector3();
					Vector3 vector = a - b;
					vector.y = 0f;
					float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vector);
					float coneWidthDegrees = Mathf.Clamp(2f * GetMaxAngleWithFirstSegment() + 10f, 0f, 360f);
					if (GetMaxAngleWithFirstSegment() <= 0f)
					{
						coneWidthDegrees = 360f;
					}
					float coneLengthRadiusInSquares = (float)GetMaxSquaresPerJump() * 1.42f;
					List<BoardSquare> squaresInCone = AreaEffectUtils.GetSquaresInCone(fromSquare.ToVector3(), coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, 0f, false, caster);
					float num = (float)GetMinSquaresPerJump() * Board.Get().squareSize;
					for (int i = 0; i < squaresInCone.Count; i++)
					{
						if (!flag)
						{
							BoardSquare boardSquare = squaresInCone[i];
							if (!boardSquare.IsBaselineHeight())
							{
								continue;
							}
							if (!(boardSquare != fromSquare))
							{
								continue;
							}
							Vector3 to = boardSquare.ToVector3() - fromSquare.ToVector3();
							to.y = 0f;
							if (to.magnitude < num)
							{
								continue;
							}
							int num2 = Mathf.RoundToInt(Vector3.Angle(vector, to));
							if (num2 > Mathf.RoundToInt(GetMaxAngleWithFirstSegment()))
							{
								continue;
							}
							bool flag2 = true;
							bool flag3 = false;
							int numSquaresInPath = 0;
							if (KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquare, fromSquare, false, out numSquaresInPath))
							{
								int num3;
								if (flag2 && numSquaresInPath > GetMinSquaresPerJump())
								{
									if (GetMaxSquaresPerJump() > 0)
									{
										num3 = ((numSquaresInPath <= GetMaxSquaresPerJump()) ? 1 : 0);
									}
									else
									{
										num3 = 1;
									}
								}
								else
								{
									num3 = 0;
								}
								flag2 = ((byte)num3 != 0);
							}
							else
							{
								flag2 = false;
							}
							if (flag2)
							{
								flag = true;
							}
							continue;
						}
						break;
					}
				}
			}
		}
		return flag;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_GremlinsBombingRun abilityMod_GremlinsBombingRun = modAsBase as AbilityMod_GremlinsBombingRun;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_GremlinsBombingRun)
		{
			val = abilityMod_GremlinsBombingRun.m_damageMod.GetModifiedValue(m_explosionDamageAmount);
		}
		else
		{
			val = m_explosionDamageAmount;
		}
		AddTokenInt(tokens, "Damage", empty, val);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_GremlinsBombingRun))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_GremlinsBombingRun);
			SetupTargeter();
			return;
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
