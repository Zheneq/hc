using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ChargeAoE : AbilityUtil_Targeter
{
	public delegate bool ShouldAddActorDelegate(ActorData actorToConsider, AbilityTarget abilityTarget, List<ActorData> hitActors, ActorData caster, Ability ability);

	public delegate bool ShouldAddCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	protected float m_radiusAroundStart = 2f;

	protected float m_radiusAroundEnd = 2f;

	protected float m_rangeFromLine = 2f;

	protected bool m_penetrateLoS;

	protected int m_maxTargets;

	public bool AllowChargeThroughInvalidSquares;

	public bool ShowTeleportLines;

	public bool SkipEvadeMovementLines;

	public bool ForceAddTargetingActor;

	public bool UseEndPosAsDamageOriginIfOverlap;

	public bool TrimPathOnTargetHit;

	public bool UseLineColorOverride;

	public Color LineColorOverride = Color.white;

	public ShouldAddActorDelegate m_shouldAddTargetDelegate;

	public ShouldAddCasterDelegate m_shouldAddCasterDelegate;

	public List<ActorData> OrderedHitActors = new List<ActorData>();

	protected OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_ChargeAoE(Ability ability, float radiusAroundStart, float radiusAroundEnd, float rangeFromDir, int maxTargets, bool ignoreTargetsCover, bool penetrateLoS)
		: base(ability)
	{
		m_radiusAroundStart = radiusAroundStart;
		m_radiusAroundEnd = radiusAroundEnd;
		m_rangeFromLine = rangeFromDir;
		m_penetrateLoS = penetrateLoS;
		m_maxTargets = maxTargets;
		m_cursorType = HighlightUtils.CursorType.MouseOverCursorType;
		int shouldShowActorRadius;
		if (!GameWideData.Get().UseActorRadiusForLaser())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			shouldShowActorRadius = (GameWideData.Get().UseActorRadiusForCone() ? 1 : 0);
		}
		else
		{
			shouldShowActorRadius = 1;
		}
		m_shouldShowActorRadius = ((byte)shouldShowActorRadius != 0);
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	protected virtual bool UseRadiusAroundLine(AbilityTarget currentTarget)
	{
		return m_rangeFromLine > 0f;
	}

	protected virtual bool UseRadiusAroundStart(AbilityTarget currentTarget)
	{
		return m_radiusAroundStart > 0f;
	}

	protected virtual bool UseRadiusAroundEnd(AbilityTarget currentTarget)
	{
		return m_radiusAroundEnd > 0f;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		BoardSquare boardSquare = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		ClearActorsInRange();
		OrderedHitActors.Clear();
		BoardSquarePathInfo boardSquarePathInfo = null;
		float startRadiusInSquares = (!UseRadiusAroundStart(currentTarget)) ? 0f : m_radiusAroundStart;
		float num;
		if (UseRadiusAroundEnd(currentTarget))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = m_radiusAroundEnd;
		}
		else
		{
			num = 0f;
		}
		float endRadiusInSquares = num;
		float num2 = (!UseRadiusAroundLine(currentTarget)) ? 0f : m_rangeFromLine;
		BoardSquare boardSquare2 = null;
		if (!(boardSquare != null))
		{
			goto IL_00c4;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		if (currentTargetIndex != 0 && targets != null)
		{
			if (IsUsingMultiTargetUpdate())
			{
				goto IL_00c4;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		boardSquare2 = targetingActor.GetCurrentBoardSquare();
		goto IL_00fa;
		IL_00fa:
		List<Team> affectedTeams = GetAffectedTeams();
		if (boardSquare2 != null && boardSquare != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (TrimPathOnTargetHit)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (num2 > 0f)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					bool collision;
					Vector3 collisionNormal;
					Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(targetingActor, boardSquare2.ToVector3(), boardSquare.ToVector3(), out collision, out collisionNormal);
					if (collision)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(boardSquare2.ToVector3(), abilityLineEndpoint);
					}
				}
			}
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare, boardSquare2, AllowChargeThroughInvalidSquares);
		}
		if (boardSquarePathInfo != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (boardSquarePathInfo.next != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (TrimPathOnTargetHit)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (num2 > 0f)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						TargetSelect_ChargeAoE.TrimChargePathOnActorHit(boardSquarePathInfo, boardSquare2, num2, targetingActor, affectedTeams, false, out BoardSquare _);
					}
				}
			}
		}
		if (!SkipEvadeMovementLines)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (ShowTeleportLines)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				InstantiateTeleportPathUIEffect();
				UpdateEffectOnCaster(currentTarget, targetingActor);
				UpdateTargetAreaEffect(currentTarget, targetingActor);
			}
			else if (UseLineColorOverride)
			{
				AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, LineColorOverride, 0);
			}
			else
			{
				AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, 0);
			}
		}
		List<ActorData> list = null;
		if (m_shouldAddCasterDelegate != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			list = new List<ActorData>();
		}
		if (boardSquarePathInfo != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			List<Vector3> list2 = KnockbackUtils.BuildDrawablePath(boardSquarePathInfo, true);
			if (list2.Count >= 2)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				Vector3 vector = list2[0];
				Vector3 vector2 = list2[list2.Count - 1];
				bool flag = UseRadiusAroundLine(currentTarget);
				bool flag2 = UseRadiusAroundStart(currentTarget);
				bool flag3 = UseRadiusAroundEnd(currentTarget);
				float widthInSquares = m_rangeFromLine * 2f;
				if (m_highlights.Count == 0)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					GameObject item = TargeterUtils.CreateLaserBoxHighlight(vector, vector2, widthInSquares, TargeterUtils.HeightAdjustType.FromPathArrow);
					m_highlights.Add(item);
					GameObject item2 = TargeterUtils.CreateCircleHighlight(vector, m_radiusAroundStart, TargeterUtils.HeightAdjustType.FromPathArrow, targetingActor == GameFlowData.Get().activeOwnedActorData);
					m_highlights.Add(item2);
					GameObject item3 = TargeterUtils.CreateCircleHighlight(vector2, m_radiusAroundEnd, TargeterUtils.HeightAdjustType.FromPathArrow, targetingActor == GameFlowData.Get().activeOwnedActorData);
					m_highlights.Add(item3);
				}
				if (flag)
				{
					if (vector == vector2)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_highlights.Count > 0)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (m_highlights[0] != null)
							{
								m_highlights[0].SetActive(false);
							}
						}
					}
					else
					{
						if (m_highlights.Count > 0)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (m_highlights[0] != null)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								m_highlights[0].SetActive(true);
							}
						}
						TargeterUtils.RefreshLaserBoxHighlight(m_highlights[0], vector, vector2, widthInSquares, TargeterUtils.HeightAdjustType.FromPathArrow);
					}
				}
				else if (m_highlights.Count > 0)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_highlights[0] != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						m_highlights[0].SetActive(false);
					}
				}
				if (flag2)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					m_highlights[1].SetActive(true);
					TargeterUtils.RefreshCircleHighlight(m_highlights[1], vector, TargeterUtils.HeightAdjustType.FromPathArrow);
				}
				else if (m_highlights.Count > 1 && m_highlights[1] != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					m_highlights[1].SetActive(false);
				}
				if (flag3)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					m_highlights[2].SetActive(true);
					TargeterUtils.RefreshCircleHighlight(m_highlights[2], vector2, TargeterUtils.HeightAdjustType.FromPathArrow);
				}
				else if (m_highlights.Count > 2)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_highlights[2] != null)
					{
						m_highlights[2].SetActive(false);
					}
				}
				List<ActorData> actors = AreaEffectUtils.GetActorsInRadiusOfLine(vector, vector2, startRadiusInSquares, endRadiusInSquares, num2, m_penetrateLoS, targetingActor, GetAffectedTeams(), null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
				TargeterUtils.SortActorsByDistanceToPos(ref actors, targetingActor.GetTravelBoardSquareWorldPosition());
				TargeterUtils.LimitActorsToMaxNumber(ref actors, m_maxTargets);
				using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData current = enumerator.Current;
						if (GetAffectsTarget(current, targetingActor))
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (m_shouldAddTargetDelegate != null)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!m_shouldAddTargetDelegate(current, currentTarget, actors, targetingActor, m_ability))
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									continue;
								}
							}
							Vector3 damageOrigin = vector;
							if (UseEndPosAsDamageOriginIfOverlap)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								Vector3 travelBoardSquareWorldPosition = current.GetTravelBoardSquareWorldPosition();
								travelBoardSquareWorldPosition.y = vector2.y;
								if ((travelBoardSquareWorldPosition - vector2).sqrMagnitude <= Mathf.Epsilon)
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									damageOrigin = vector2;
								}
							}
							AddActorInRange(current, damageOrigin, targetingActor);
							OrderedHitActors.Add(current);
							if (m_shouldAddCasterDelegate != null)
							{
								list.Add(current);
							}
							if (UseRadiusAroundStart(currentTarget))
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								BoardSquare currentBoardSquare = current.GetCurrentBoardSquare();
								if (AreaEffectUtils.IsSquareInConeByActorRadius(currentBoardSquare, vector, 0f, 360f, m_radiusAroundStart, 0f, m_penetrateLoS, targetingActor))
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									AddActorInRange(current, damageOrigin, targetingActor, AbilityTooltipSubject.Far, true);
								}
							}
							bool flag4 = false;
							if (UseRadiusAroundEnd(currentTarget))
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								BoardSquare currentBoardSquare2 = current.GetCurrentBoardSquare();
								if (AreaEffectUtils.IsSquareInConeByActorRadius(currentBoardSquare2, vector2, 0f, 360f, m_radiusAroundEnd, 0f, m_penetrateLoS, targetingActor))
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
									AddActorInRange(current, damageOrigin, targetingActor, AbilityTooltipSubject.Far, true);
									flag4 = true;
								}
							}
							ActorHitContext actorHitContext = m_actorContextVars[current];
							ContextVars contextVars = actorHitContext._0015;
							int hash = ContextKeys._0004.GetHash();
							int value;
							if (flag4)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								value = 1;
							}
							else
							{
								value = 0;
							}
							contextVars.SetInt(hash, value);
						}
					}
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (targetingActor == GameFlowData.Get().activeOwnedActorData)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					ResetSquareIndicatorIndexToUse();
					AreaEffectUtils.OperateOnSquaresInRadiusOfLine(m_indicatorHandler, vector, vector2, startRadiusInSquares, endRadiusInSquares, num2, m_penetrateLoS, targetingActor);
					HideUnusedSquareIndicators();
				}
			}
		}
		if (!ForceAddTargetingActor)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_shouldAddCasterDelegate == null)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!m_shouldAddCasterDelegate(targetingActor, list))
			{
				return;
			}
		}
		AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self);
		return;
		IL_00c4:
		if (boardSquare != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			boardSquare2 = Board.Get().GetBoardSquareSafe(targets[currentTargetIndex - 1].GridPos);
		}
		goto IL_00fa;
	}
}
