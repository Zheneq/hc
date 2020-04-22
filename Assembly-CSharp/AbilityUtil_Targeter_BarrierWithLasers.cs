using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BarrierWithLasers : AbilityUtil_Targeter_Barrier
{
	public float m_laserRangeFront;

	public float m_laserRangeBack;

	public bool m_laserIgnoreLos;

	public bool m_laserLengthIgnoreLos;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private TargeterPart_Laser m_laserPartFront;

	private TargeterPart_Laser m_laserPartBack;

	public AbilityUtil_Targeter_BarrierWithLasers(Ability ability, float width, float laserRangeFront, float laserRangeBack, bool laserIgnoreLos, bool laserLengthIgnoreLos)
		: base(ability, width, true)
	{
		m_laserRangeFront = laserRangeFront;
		m_laserRangeBack = laserRangeBack;
		m_laserIgnoreLos = laserIgnoreLos;
		m_laserLengthIgnoreLos = laserLengthIgnoreLos;
		m_laserPartFront = new TargeterPart_Laser(m_width, m_laserRangeFront, m_laserIgnoreLos, -1);
		m_laserPartFront.m_lengthIgnoreWorldGeo = m_laserLengthIgnoreLos;
		m_laserPartFront.m_ignoreStartOffset = true;
		m_laserPartBack = new TargeterPart_Laser(m_width, m_laserRangeBack, m_laserIgnoreLos, -1);
		m_laserPartBack.m_lengthIgnoreWorldGeo = m_laserLengthIgnoreLos;
		m_laserPartBack.m_ignoreStartOffset = true;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
		int num = 1;
		if (m_snapToBorder)
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
			num = 2;
		}
		if (m_highlights.Count < num + 2)
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
			m_highlights.Add(m_laserPartFront.CreateHighlightObject(this));
			m_highlights.Add(m_laserPartBack.CreateHighlightObject(this));
		}
		GameObject gameObject = m_highlights[num];
		GameObject gameObject2 = m_highlights[num + 1];
		bool flag = GameFlowData.Get().activeOwnedActorData == targetingActor;
		if (flag)
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
			ResetSquareIndicatorIndexToUse();
		}
		Vector3 barrierCenterPos = m_barrierCenterPos;
		barrierCenterPos.y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, m_affectsAllies, m_affectsEnemies);
		if (m_laserRangeFront > 0f)
		{
			Vector3 endPos;
			List<ActorData> hitActors = m_laserPartFront.GetHitActors(barrierCenterPos, m_barrierDir, targetingActor, relevantTeams, out endPos);
			using (List<ActorData>.Enumerator enumerator = hitActors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					AddActorInRange(current, barrierCenterPos, targetingActor);
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
			if (flag)
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
				m_laserPartFront.ShowHiddenSquares(m_indicatorHandler, barrierCenterPos, endPos, targetingActor, m_laserIgnoreLos);
			}
			m_laserPartFront.AdjustHighlight(gameObject, barrierCenterPos, endPos, false);
		}
		else
		{
			gameObject.SetActiveIfNeeded(false);
		}
		if (m_laserRangeBack > 0f)
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
			Vector3 endPos2;
			List<ActorData> hitActors2 = m_laserPartBack.GetHitActors(barrierCenterPos, -1f * m_barrierDir, targetingActor, relevantTeams, out endPos2);
			using (List<ActorData>.Enumerator enumerator2 = hitActors2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData current2 = enumerator2.Current;
					AddActorInRange(current2, barrierCenterPos, targetingActor);
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
			}
			if (flag)
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
				m_laserPartBack.ShowHiddenSquares(m_indicatorHandler, barrierCenterPos, endPos2, targetingActor, m_laserIgnoreLos);
			}
			m_laserPartBack.AdjustHighlight(gameObject2, barrierCenterPos, endPos2, false);
		}
		else
		{
			gameObject2.SetActiveIfNeeded(false);
		}
		if (flag)
		{
			HideAllSquareIndicators();
		}
		if (!(m_laserRangeFront > 0f))
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
			if (!(m_laserRangeBack > 0f))
			{
				return;
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
		}
		m_highlights[0].gameObject.SetActiveIfNeeded(false);
	}
}
