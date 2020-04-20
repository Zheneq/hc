using System;
using System.Collections.Generic;
using UnityEngine;

public class GrydPlaceOrMoveBomb : Ability
{
	[Header("-- Targeting")]
	public bool m_lockToCardinalDirsForPlace = true;

	public bool m_lockToCardinalDirsForMove = true;

	public int m_placeRange = 4;

	public int m_moveRange = 4;

	public bool m_moveIsFreeAction = true;

	[Header("-- Enemy direct hit")]
	public bool m_explodeThisTurnOnDirectHit;

	public bool m_explodeImmediatelyOnMove;

	[Header("-- Bomb explosion")]
	public int m_bombDuration;

	public int m_damageAmount;

	public float m_explosionLaserRange;

	public float m_explosionLaserWidth;

	public int m_cooldownAfterExplode = 2;

	[Header("-- Anims")]
	public ActorModelData.ActionAnimationType m_moveBombAnimIndex = ActorModelData.ActionAnimationType.Ability2;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_persistentBombSequencePrefab;

	public GameObject m_explodeBombSequencePrefab;

	public GameObject m_moveBombSequencePrefab;

	private Gryd_SyncComponent m_syncComp;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Place/Move Bomb";
		}
		this.m_syncComp = base.GetComponent<Gryd_SyncComponent>();
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_GrydBomb(this, (float)this.m_moveRange);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (this.HasPlacedBomb())
		{
			return true;
		}
		GridPos gridPosWithIncrementedHeight = caster.GetGridPosWithIncrementedHeight();
		if (this.m_lockToCardinalDirsForPlace && !this.CardinallyAligned(gridPosWithIncrementedHeight, target.GridPos))
		{
			return false;
		}
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (!(boardSquareSafe == null))
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				return Mathf.Abs(gridPosWithIncrementedHeight.x - target.GridPos.x) <= this.m_placeRange && Mathf.Abs(gridPosWithIncrementedHeight.y - target.GridPos.y) <= this.m_placeRange && base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
			}
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_damageAmount);
		return result;
	}

	public override bool IsFreeAction()
	{
		if (this.HasPlacedBomb())
		{
			return this.m_moveIsFreeAction;
		}
		return base.IsFreeAction();
	}

	public override ActorModelData.ActionAnimationType GetActionAnimType()
	{
		if (this.HasPlacedBomb())
		{
			return this.m_moveBombAnimIndex;
		}
		return base.GetActionAnimType();
	}

	private bool CardinallyAligned(GridPos start, GridPos end)
	{
		bool result;
		if (!start.CoordsEqual(end))
		{
			result = (start.x == end.x || start.y == end.y);
		}
		else
		{
			result = false;
		}
		return result;
	}

	private unsafe GridPos GetPushEndPos(Vector3 targetPos, out bool hitActor)
	{
		GridPos placedBomb = this.GetPlacedBomb();
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(placedBomb);
		Vector3 vector = boardSquareSafe.ToVector3();
		Vector3 vector2 = targetPos - vector;
		if (this.m_lockToCardinalDirsForMove)
		{
			vector2 = VectorUtils.HorizontalAngleToClosestCardinalDirection(Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vector2)));
		}
		hitActor = false;
		bool flag;
		Vector3 vector3;
		Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(base.ActorData, vector, vector + vector2 * (float)this.m_moveRange * Board.Get().squareSize, out flag, out vector3, null);
		BoardSquare boardSquare = null;
		if (flag)
		{
			boardSquare = Board.Get().GetBoardSquare(abilityLineEndpoint);
		}
		GridPos result = placedBomb;
		if (vector2.x > 0.1f)
		{
			int i = placedBomb.x + 1;
			while (i <= placedBomb.x + this.m_moveRange)
			{
				BoardSquare boardSquare2 = Board.Get().GetBoardSquare(i, placedBomb.y);
				if (!(boardSquare2 == null))
				{
					if (boardSquare2.IsBaselineHeight())
					{
						if (boardSquareSafe.symbol_0013(i, placedBomb.y))
						{
							if (boardSquare != null)
							{
								if (boardSquare.x < i)
								{
									for (;;)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										goto IL_18E;
									}
								}
							}
							result = boardSquare2.GetGridPos();
							if (boardSquare2.OccupantActor != null)
							{
								if (boardSquare2.OccupantActor.GetTeam() != base.ActorData.GetTeam())
								{
									hitActor = true;
									goto IL_201;
								}
							}
							i++;
							continue;
						}
					}
				}
				IL_18E:
				IL_201:
				return result;
			}
		}
		else if (vector2.x < -0.1f)
		{
			int j = placedBomb.x - 1;
			while (j >= placedBomb.x - this.m_moveRange)
			{
				BoardSquare boardSquare3 = Board.Get().GetBoardSquare(j, placedBomb.y);
				if (!(boardSquare3 == null))
				{
					if (boardSquare3.IsBaselineHeight())
					{
						if (boardSquareSafe.symbol_0013(j, placedBomb.y))
						{
							if (boardSquare != null)
							{
								if (boardSquare.x > j)
								{
									goto IL_2B7;
								}
							}
							result = boardSquare3.GetGridPos();
							if (boardSquare3.OccupantActor != null)
							{
								if (boardSquare3.OccupantActor.GetTeam() != base.ActorData.GetTeam())
								{
									hitActor = true;
									goto IL_337;
								}
							}
							j--;
							continue;
						}
					}
				}
				IL_2B7:
				IL_337:
				return result;
			}
		}
		else if (vector2.z > 0.1f)
		{
			int k = placedBomb.y + 1;
			while (k <= placedBomb.y + this.m_moveRange)
			{
				BoardSquare boardSquare4 = Board.Get().GetBoardSquare(placedBomb.x, k);
				if (!(boardSquare4 == null))
				{
					if (boardSquare4.IsBaselineHeight())
					{
						if (boardSquareSafe.symbol_0013(placedBomb.x, k))
						{
							if (boardSquare != null)
							{
								if (boardSquare.y < k)
								{
									for (;;)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										goto IL_3F1;
									}
								}
							}
							result = boardSquare4.GetGridPos();
							if (boardSquare4.OccupantActor != null)
							{
								if (boardSquare4.OccupantActor.GetTeam() != base.ActorData.GetTeam())
								{
									hitActor = true;
									goto IL_46D;
								}
							}
							k++;
							continue;
						}
					}
				}
				IL_3F1:
				IL_46D:
				return result;
			}
		}
		else if (vector2.z < -0.1f)
		{
			int l = placedBomb.y - 1;
			while (l >= placedBomb.y - this.m_moveRange)
			{
				BoardSquare boardSquare5 = Board.Get().GetBoardSquare(placedBomb.x, l);
				if (!(boardSquare5 == null) && boardSquare5.IsBaselineHeight() && boardSquareSafe.symbol_0013(placedBomb.x, l))
				{
					if (boardSquare != null && boardSquare.y > l)
					{
					}
					else
					{
						result = boardSquare5.GetGridPos();
						if (!(boardSquare5.OccupantActor != null) || boardSquare5.OccupantActor.GetTeam() == base.ActorData.GetTeam())
						{
							l--;
							continue;
						}
						hitActor = true;
					}
				}
				break;
			}
		}
		return result;
	}

	public bool HasPlacedBomb()
	{
		return this.GetPlacedBomb().x > 0 && this.GetPlacedBomb().y > 0;
	}

	public GridPos GetPlacedBomb()
	{
		GridPos result;
		if (this.m_syncComp != null)
		{
			result = this.m_syncComp.m_bombLocation;
		}
		else
		{
			result = GridPos.s_invalid;
		}
		return result;
	}
}
