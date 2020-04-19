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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydPlaceOrMoveBomb.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			return true;
		}
		GridPos start = caster.\u000E();
		if (this.m_lockToCardinalDirsForPlace && !this.CardinallyAligned(start, target.GridPos))
		{
			return false;
		}
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (!(boardSquare == null))
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (boardSquare.\u0016())
			{
				return Mathf.Abs(start.x - target.GridPos.x) <= this.m_placeRange && Mathf.Abs(start.y - target.GridPos.y) <= this.m_placeRange && base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydPlaceOrMoveBomb.IsFreeAction()).MethodHandle;
			}
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydPlaceOrMoveBomb.CardinallyAligned(GridPos, GridPos)).MethodHandle;
			}
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
		BoardSquare boardSquare = Board.\u000E().\u000E(placedBomb);
		Vector3 vector = boardSquare.ToVector3();
		Vector3 vector2 = targetPos - vector;
		if (this.m_lockToCardinalDirsForMove)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydPlaceOrMoveBomb.GetPushEndPos(Vector3, bool*)).MethodHandle;
			}
			vector2 = VectorUtils.HorizontalAngleToClosestCardinalDirection(Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vector2)));
		}
		hitActor = false;
		bool flag;
		Vector3 vector3;
		Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(base.ActorData, vector, vector + vector2 * (float)this.m_moveRange * Board.\u000E().squareSize, out flag, out vector3, null);
		BoardSquare boardSquare2 = null;
		if (flag)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			boardSquare2 = Board.\u000E().\u000E(abilityLineEndpoint);
		}
		GridPos result = placedBomb;
		if (vector2.x > 0.1f)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			int i = placedBomb.x + 1;
			while (i <= placedBomb.x + this.m_moveRange)
			{
				BoardSquare boardSquare3 = Board.\u000E().\u0016(i, placedBomb.y);
				if (!(boardSquare3 == null))
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (boardSquare3.\u0016())
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (boardSquare.\u0013(i, placedBomb.y))
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (boardSquare2 != null)
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (boardSquare2.x < i)
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
							result = boardSquare3.\u001D();
							if (boardSquare3.OccupantActor != null)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								if (boardSquare3.OccupantActor.\u000E() != base.ActorData.\u000E())
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		else if (vector2.x < -0.1f)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			int j = placedBomb.x - 1;
			while (j >= placedBomb.x - this.m_moveRange)
			{
				BoardSquare boardSquare4 = Board.\u000E().\u0016(j, placedBomb.y);
				if (!(boardSquare4 == null))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (boardSquare4.\u0016())
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (boardSquare.\u0013(j, placedBomb.y))
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (boardSquare2 != null)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								if (boardSquare2.x > j)
								{
									goto IL_2B7;
								}
							}
							result = boardSquare4.\u001D();
							if (boardSquare4.OccupantActor != null)
							{
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								if (boardSquare4.OccupantActor.\u000E() != base.ActorData.\u000E())
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		else if (vector2.z > 0.1f)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			int k = placedBomb.y + 1;
			while (k <= placedBomb.y + this.m_moveRange)
			{
				BoardSquare boardSquare5 = Board.\u000E().\u0016(placedBomb.x, k);
				if (!(boardSquare5 == null))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (boardSquare5.\u0016())
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (boardSquare.\u0013(placedBomb.x, k))
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (boardSquare2 != null)
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (boardSquare2.y < k)
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
							result = boardSquare5.\u001D();
							if (boardSquare5.OccupantActor != null)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (boardSquare5.OccupantActor.\u000E() != base.ActorData.\u000E())
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		else if (vector2.z < -0.1f)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			int l = placedBomb.y - 1;
			while (l >= placedBomb.y - this.m_moveRange)
			{
				BoardSquare boardSquare6 = Board.\u000E().\u0016(placedBomb.x, l);
				if (!(boardSquare6 == null) && boardSquare6.\u0016() && boardSquare.\u0013(placedBomb.x, l))
				{
					if (boardSquare2 != null && boardSquare2.y > l)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					else
					{
						result = boardSquare6.\u001D();
						if (!(boardSquare6.OccupantActor != null) || boardSquare6.OccupantActor.\u000E() == base.ActorData.\u000E())
						{
							l--;
							continue;
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydPlaceOrMoveBomb.GetPlacedBomb()).MethodHandle;
			}
			result = this.m_syncComp.m_bombLocation;
		}
		else
		{
			result = GridPos.s_invalid;
		}
		return result;
	}
}
