using AbilityContextNamespace;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityUtil_Targeter_LaserWithLayeredRings : AbilityUtil_Targeter_LaserWithCone
{
	private List<RadiusToLayerIndex> m_coneRadiusList = new List<RadiusToLayerIndex>();

	private float m_outerConeRadius = 1f;

	public AbilityUtil_Targeter_LaserWithLayeredRings(Ability ability, float laserWidth, float laserRange, bool ignoreLos, int laserMaxTargets, bool clampConeCenterToCursor, bool onlyUseConeIfLaserHit, List<float> coneRadiusInput)
		: base(ability, laserWidth, laserRange, ignoreLos, false, 360f, 1f, 0f)
	{
		SetClampToCursorPos(clampConeCenterToCursor);
		SetExplodeOnPathEnd(!onlyUseConeIfLaserHit);
		SetExplodeOnEnvironmentHit(!onlyUseConeIfLaserHit);
		for (int i = 0; i < coneRadiusInput.Count; i++)
		{
			m_coneRadiusList.Add(new RadiusToLayerIndex(coneRadiusInput[i]));
		}
		if (m_coneRadiusList.Count == 0)
		{
			Log.Error(new StringBuilder().Append("no radius list passed to ").Append(GetType()).ToString());
			m_coneRadiusList.Add(new RadiusToLayerIndex(1f));
		}
		RadiusToLayerIndex.SortAndSetLayerIndex(m_coneRadiusList);
		m_outerConeRadius = m_coneRadiusList[m_coneRadiusList.Count - 1].m_radius;
	}

	public override float GetConeRadius()
	{
		return m_outerConeRadius;
	}

	protected override void AllocateConeHighlights()
	{
		if (m_highlights.Count != 1)
		{
			return;
		}
		for (int i = 0; i < m_coneRadiusList.Count; i++)
		{
			bool flag = i == m_coneRadiusList.Count - 1;
			float radiusInWorld = m_coneRadiusList[i].m_radius * Board.SquareSizeStatic;
			GameObject gameObject = HighlightUtils.Get().CreateConeCursor(radiusInWorld, GetConeWidthAngle());
			if (!flag)
			{
				UIDynamicCone component = gameObject.GetComponent<UIDynamicCone>();
				if (component != null)
				{
					component.SetConeObjectActive(false);
				}
			}
			m_highlights.Add(gameObject);
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public override void AddTargetedActor(ActorData actor, Vector3 damageOrigin, ActorData targetingActor, AbilityTooltipSubject subjectType = AbilityTooltipSubject.Primary)
	{
		if (IsActorInTargetRange(actor))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		base.AddTargetedActor(actor, damageOrigin, targetingActor, subjectType);
		ActorHitContext actorHitContext = m_actorContextVars[actor];
		if (subjectType == AbilityTooltipSubject.Primary)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					actorHitContext.m_contextVars.SetValue(ContextKeys.s_InAoe.GetKey(), 0);
					return;
				}
			}
		}
		RadiusToLayerIndex bestMatchingData = AbilityCommon_LayeredRings.GetBestMatchingData(m_coneRadiusList, actor.GetCurrentBoardSquare(), damageOrigin, targetingActor, true);
		float value = VectorUtils.HorizontalPlaneDistInSquares(damageOrigin, actor.GetFreePos());
		actorHitContext.m_contextVars.SetValue(ContextKeys.s_InAoe.GetKey(), 1);
		actorHitContext.m_contextVars.SetValue(ContextKeys.s_Layer.GetKey(), bestMatchingData.m_index);
		actorHitContext.m_contextVars.SetValue(ContextKeys.s_DistFromStart.GetKey(), value);
	}
}
