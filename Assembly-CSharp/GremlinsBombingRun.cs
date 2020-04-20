using System;
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
		return this.m_abilityMod;
	}

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Bombing Run";
		}
		this.SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return (float)((this.GetMaxSquaresPerJump() - 1) * this.m_numSteps);
	}

	public int GetDamageAmount()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_explosionDamageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_explosionDamageAmount);
		}
		return result;
	}

	public int GetMinSquaresPerJump()
	{
		int b = (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_minSquaresPerExplosionMod.GetModifiedValue(this.m_squaresPerExplosion) : this.m_squaresPerExplosion;
		return Mathf.Max(1, b);
	}

	public int GetMaxSquaresPerJump()
	{
		int num;
		if (this.m_abilityMod == null)
		{
			num = this.m_maxSquaresPerStep;
		}
		else
		{
			num = this.m_abilityMod.m_maxSquaresPerExplosionMod.GetModifiedValue(this.m_maxSquaresPerStep);
		}
		int num2 = num;
		if (num2 > 0 && num2 < this.GetMinSquaresPerJump())
		{
			num2 = this.GetMinSquaresPerJump() + 1;
		}
		return num2;
	}

	public float GetMaxAngleWithFirstSegment()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_angleWithFirstStepMod.GetModifiedValue(this.m_maxAngleWithFirstStep) : this.m_maxAngleWithFirstStep;
	}

	public AbilityAreaShape GetExplosionShape()
	{
		AbilityAreaShape result;
		if (this.m_abilityMod == null)
		{
			result = this.m_explosionShape;
		}
		else
		{
			result = this.m_abilityMod.m_explosionShapeMod.GetModifiedValue(this.m_explosionShape);
		}
		return result;
	}

	public bool ShouldLeaveMinesAtTouchedSquares()
	{
		bool result;
		if (this.m_abilityMod == null)
		{
			result = false;
		}
		else
		{
			result = this.m_abilityMod.m_shouldLeaveMinesAtTouchedSquares.GetModifiedValue(false);
		}
		return result;
	}

	private void SetupTargeter()
	{
		if (this.m_bombInfoComp == null)
		{
			this.m_bombInfoComp = base.GetComponent<GremlinsLandMineInfoComponent>();
		}
		if (this.m_targeterMultiStep)
		{
			this.m_numSteps = Mathf.Max(base.GetNumTargets(), 1);
		}
		else
		{
			this.m_numSteps = 1;
		}
		if (this.m_numSteps < 2)
		{
			base.Targeter = new AbilityUtil_Targeter_BombingRun(this, this.GetExplosionShape(), this.GetMinSquaresPerJump());
		}
		else
		{
			base.ClearTargeters();
			for (int i = 0; i < this.m_numSteps; i++)
			{
				base.Targeters.Add(new AbilityUtil_Targeter_BombingRun(this, this.GetExplosionShape(), this.GetMinSquaresPerJump()));
				base.Targeters[i].SetUseMultiTargetUpdate(true);
			}
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDamageAmount());
		return result;
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return this.m_numSteps;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		bool flag = true;
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				if (this.m_numSteps < 2)
				{
					int num = 0;
					flag = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out num);
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
						else if (this.GetMaxAngleWithFirstSegment() > 0f)
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
							int num2 = Mathf.RoundToInt(Vector3.Angle(from, to));
							if (num2 > Mathf.RoundToInt(this.GetMaxAngleWithFirstSegment()))
							{
								flag = false;
							}
						}
					}
					if (flag)
					{
						int num3 = 0;
						bool flag2;
						if (targetIndex == 0)
						{
							flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, caster.GetCurrentBoardSquare(), false, out num3);
						}
						else
						{
							flag2 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquareSafe, Board.Get().GetBoardSquareSafe(currentTargets[targetIndex - 1].GridPos), false, out num3);
						}
						if (flag2)
						{
							bool flag3;
							if (flag)
							{
								flag3 = (num3 > this.GetMinSquaresPerJump());
							}
							else
							{
								flag3 = false;
							}
							flag = flag3;
							bool flag4;
							if (flag)
							{
								if (this.GetMaxSquaresPerJump() > 0)
								{
									flag4 = (num3 <= this.GetMaxSquaresPerJump());
								}
								else
								{
									flag4 = true;
								}
							}
							else
							{
								flag4 = false;
							}
							flag = flag4;
						}
						else
						{
							flag = false;
						}
						if (flag)
						{
							if (targetIndex < this.m_numSteps - 1)
							{
								List<BoardSquare> list = new List<BoardSquare>();
								list.Add(caster.GetCurrentBoardSquare());
								int i = 0;
								while (i < targetIndex)
								{
									if (i >= currentTargets.Count)
									{
										for (;;)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											goto IL_2CF;
										}
									}
									else
									{
										list.Add(Board.Get().GetBoardSquareSafe(currentTargets[i].GridPos));
										i++;
									}
								}
								IL_2CF:
								flag = this.CanTargetForNextClick(boardSquareSafe, targetIndex + 1, list, caster);
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
		if (nextTargetIndex >= this.m_numSteps)
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
					float coneWidthDegrees = Mathf.Clamp(2f * this.GetMaxAngleWithFirstSegment() + 10f, 0f, 360f);
					if (this.GetMaxAngleWithFirstSegment() <= 0f)
					{
						coneWidthDegrees = 360f;
					}
					float coneLengthRadiusInSquares = (float)this.GetMaxSquaresPerJump() * 1.42f;
					List<BoardSquare> squaresInCone = AreaEffectUtils.GetSquaresInCone(fromSquare.ToVector3(), coneCenterAngleDegrees, coneWidthDegrees, coneLengthRadiusInSquares, 0f, false, caster);
					float num = (float)this.GetMinSquaresPerJump() * Board.Get().squareSize;
					int i = 0;
					while (i < squaresInCone.Count)
					{
						if (flag)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								return flag;
							}
						}
						else
						{
							BoardSquare boardSquare = squaresInCone[i];
							if (boardSquare.IsBaselineHeight())
							{
								if (boardSquare != fromSquare)
								{
									Vector3 to = boardSquare.ToVector3() - fromSquare.ToVector3();
									to.y = 0f;
									if (to.magnitude < num)
									{
									}
									else
									{
										int num2 = Mathf.RoundToInt(Vector3.Angle(vector, to));
										if (num2 <= Mathf.RoundToInt(this.GetMaxAngleWithFirstSegment()))
										{
											bool flag2 = true;
											int num3 = 0;
											bool flag3 = KnockbackUtils.CanBuildStraightLineChargePath(caster, boardSquare, fromSquare, false, out num3);
											if (flag3)
											{
												flag2 = (flag2 && num3 > this.GetMinSquaresPerJump());
												bool flag4;
												if (flag2)
												{
													if (this.GetMaxSquaresPerJump() > 0)
													{
														flag4 = (num3 <= this.GetMaxSquaresPerJump());
													}
													else
													{
														flag4 = true;
													}
												}
												else
												{
													flag4 = false;
												}
												flag2 = flag4;
											}
											else
											{
												flag2 = false;
											}
											if (flag2)
											{
												flag = true;
											}
										}
									}
								}
							}
							i++;
						}
					}
				}
			}
		}
		return flag;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_GremlinsBombingRun abilityMod_GremlinsBombingRun = modAsBase as AbilityMod_GremlinsBombingRun;
		string name = "Damage";
		string empty = string.Empty;
		int val;
		if (abilityMod_GremlinsBombingRun)
		{
			val = abilityMod_GremlinsBombingRun.m_damageMod.GetModifiedValue(this.m_explosionDamageAmount);
		}
		else
		{
			val = this.m_explosionDamageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_GremlinsBombingRun))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_GremlinsBombingRun);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}
}
