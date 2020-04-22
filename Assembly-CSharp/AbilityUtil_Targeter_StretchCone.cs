using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_StretchCone : AbilityUtil_Targeter
{
	public delegate bool UseExtraKnockbackDistDelegate(ActorData caster);

	public delegate float ConeLengthSquaresOverrideDelegate();

	public float m_minLengthSquares;

	public float m_maxLengthSquares;

	public float m_minAngleDegrees;

	public float m_maxAngleDegrees;

	public float m_coneBackwardOffsetInSquares;

	public bool m_penetrateLoS;

	public bool m_includeEnemies = true;

	public bool m_includeAllies;

	public bool m_includeCaster;

	public AreaEffectUtils.StretchConeStyle m_stretchStyle;

	public float m_knockbackDistance;

	public KnockbackType m_knockbackType;

	public float m_extraKnockbackDist;

	public float m_knockbackDistanceOnSelf;

	public KnockbackType m_knockbackTypeOnSelf;

	public float m_interpMinDistOverride = -1f;

	public float m_interpRangeOverride = -1f;

	public bool m_discreteWidthAngleChange;

	public int m_numDiscreteWidthChanges;

	private TargeterPart_Cone m_conePart;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public UseExtraKnockbackDistDelegate m_useExtraKnockbackDistDelegate;

	public ConeLengthSquaresOverrideDelegate m_coneLengthSquaresOverrideDelegate;

	public bool ForceHideSides;

	public float LastConeAngle
	{
		get;
		set;
	}

	public float LastConeRadiusInSquares
	{
		get;
		set;
	}

	public AbilityUtil_Targeter_StretchCone(Ability ability, float minLengthInSquares, float maxLengthInSquares, float minAngleDegrees, float maxAngleDegrees, AreaEffectUtils.StretchConeStyle stretchStyle, float coneBackwardOffsetInSquares, bool penetrateLoS)
		: base(ability)
	{
		m_minLengthSquares = minLengthInSquares;
		m_maxLengthSquares = maxLengthInSquares;
		m_minAngleDegrees = minAngleDegrees;
		m_maxAngleDegrees = maxAngleDegrees;
		m_stretchStyle = stretchStyle;
		m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		m_penetrateLoS = penetrateLoS;
		m_knockbackDistance = 0f;
		m_knockbackType = KnockbackType.ForwardAlongAimDir;
		m_conePart = new TargeterPart_Cone(m_maxAngleDegrees, m_maxLengthSquares, m_penetrateLoS, m_coneBackwardOffsetInSquares);
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		LastConeAngle = 0f;
		LastConeRadiusInSquares = 0f;
	}

	public void InitKnockbackData(float knockbackDistance, KnockbackType knockbackType, float knockbackDistanceOnSelf, KnockbackType knockbackTypeOnSelf)
	{
		m_knockbackDistance = knockbackDistance;
		m_knockbackType = knockbackType;
		m_knockbackDistanceOnSelf = knockbackDistanceOnSelf;
		m_knockbackTypeOnSelf = knockbackTypeOnSelf;
	}

	public void SetExtraKnockbackDist(float extraDist)
	{
		m_extraKnockbackDist = extraDist;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		BoardSquare currentBoardSquare = targetingActor.GetCurrentBoardSquare();
		UpdateTargetingAsIfFromSquare(currentTarget, targetingActor, currentBoardSquare);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		BoardSquare coneStartSquare;
		if (targets.Count >= 1 && currentTargetIndex >= 1)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (targets[currentTargetIndex - 1] != null)
			{
				AbilityTarget abilityTarget = targets[currentTargetIndex - 1];
				coneStartSquare = Board.Get().GetBoardSquareSafe(abilityTarget.GridPos);
				goto IL_0066;
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
		coneStartSquare = targetingActor.GetCurrentBoardSquare();
		goto IL_0066;
		IL_0066:
		UpdateTargetingAsIfFromSquare(currentTarget, targetingActor, coneStartSquare);
	}

	public void UpdateTargetingAsIfFromSquare(AbilityTarget currentTarget, ActorData targetingActor, BoardSquare coneStartSquare)
	{
		ClearActorsInRange();
		Vector3 worldPositionForLoS = coneStartSquare.GetWorldPositionForLoS();
		Vector3 freePos = currentTarget.FreePos;
		Vector3 vector = freePos - worldPositionForLoS;
		vector.y = 0f;
		vector.Normalize();
		float forwardDir_degrees = VectorUtils.HorizontalAngle_Deg(vector);
		AreaEffectUtils.GatherStretchConeDimensions(freePos, worldPositionForLoS, m_minLengthSquares, m_maxLengthSquares, m_minAngleDegrees, m_maxAngleDegrees, m_stretchStyle, out float lengthInSquares, out float angleInDegrees, m_discreteWidthAngleChange, m_numDiscreteWidthChanges, m_interpMinDistOverride, m_interpRangeOverride);
		if (m_coneLengthSquaresOverrideDelegate != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			lengthInSquares = m_coneLengthSquaresOverrideDelegate();
		}
		LastConeAngle = angleInDegrees;
		LastConeRadiusInSquares = lengthInSquares;
		m_conePart.UpdateDimensions(angleInDegrees, lengthInSquares);
		if (m_highlights != null)
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
			if (m_highlights.Count >= 1)
			{
				goto IL_010e;
			}
		}
		m_highlights = new List<GameObject>();
		GameObject item = m_conePart.CreateHighlightObject(this);
		m_highlights.Add(item);
		goto IL_010e;
		IL_010e:
		m_conePart.AdjustHighlight(m_highlights[0], worldPositionForLoS, vector);
		List<ActorData> hitActors = m_conePart.GetHitActors(worldPositionForLoS, vector, targetingActor, TargeterUtils.GetRelevantTeams(targetingActor, m_includeAllies, m_includeEnemies));
		if (!(m_knockbackDistance > 0f))
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
			if (!(m_knockbackDistanceOnSelf > 0f))
			{
				goto IL_02aa;
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
		int num = 0;
		EnableAllMovementArrows();
		Vector3 sourcePos = worldPositionForLoS;
		if (m_knockbackDistance > 0f)
		{
			float num2 = m_knockbackDistance;
			if (m_extraKnockbackDist > 0f)
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
				if (m_useExtraKnockbackDistDelegate != null)
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
					if (m_useExtraKnockbackDistDelegate(targetingActor))
					{
						num2 += m_extraKnockbackDist;
					}
				}
			}
			using (List<ActorData>.Enumerator enumerator = hitActors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (current.GetTeam() != targetingActor.GetTeam())
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
						BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(current, m_knockbackType, vector, sourcePos, num2);
						num = AddMovementArrowWithPrevious(current, path, TargeterMovementType.Knockback, num);
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
		}
		if (m_knockbackDistanceOnSelf > 0f)
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
			BoardSquarePathInfo path2 = KnockbackUtils.BuildKnockbackPath(targetingActor, m_knockbackTypeOnSelf, vector, sourcePos, m_knockbackDistanceOnSelf);
			num = AddMovementArrowWithPrevious(targetingActor, path2, TargeterMovementType.Knockback, num);
		}
		SetMovementArrowEnabledFromIndex(num, false);
		goto IL_02aa;
		IL_02aa:
		if (m_includeCaster)
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
			if (!hitActors.Contains(targetingActor))
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
				hitActors.Add(targetingActor);
			}
		}
		for (int i = 0; i < hitActors.Count; i++)
		{
			ActorData actor = hitActors[i];
			if (ShouldAddActor(actor, targetingActor))
			{
				AddActorInRange(actor, worldPositionForLoS, targetingActor);
			}
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			DrawInvalidSquareIndicators(targetingActor, worldPositionForLoS, forwardDir_degrees, lengthInSquares, angleInDegrees);
			return;
		}
	}

	private void DrawInvalidSquareIndicators(ActorData targetingActor, Vector3 coneStartPos, float forwardDir_degrees, float coneLengthSquares, float coneWidthDegrees)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			ResetSquareIndicatorIndexToUse();
			m_conePart.ShowHiddenSquares(m_indicatorHandler, coneStartPos, forwardDir_degrees, targetingActor, m_penetrateLoS);
			HideUnusedSquareIndicators();
		}
	}

	private bool ShouldAddActor(ActorData actor, ActorData caster)
	{
		bool result = false;
		if (actor == caster)
		{
			result = m_includeCaster;
		}
		else
		{
			if (actor.GetTeam() == caster.GetTeam())
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (m_includeAllies)
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
					result = true;
					goto IL_0081;
				}
			}
			if (actor.GetTeam() != caster.GetTeam())
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
				if (m_includeEnemies)
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
					result = true;
				}
			}
		}
		goto IL_0081;
		IL_0081:
		return result;
	}
}
