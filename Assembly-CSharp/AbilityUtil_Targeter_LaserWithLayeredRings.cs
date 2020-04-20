using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_LaserWithLayeredRings : AbilityUtil_Targeter_LaserWithCone
{
	private List<RadiusToLayerIndex> m_coneRadiusList = new List<RadiusToLayerIndex>();

	private float m_outerConeRadius = 1f;

	public AbilityUtil_Targeter_LaserWithLayeredRings(Ability ability, float laserWidth, float laserRange, bool ignoreLos, int laserMaxTargets, bool clampConeCenterToCursor, bool onlyUseConeIfLaserHit, List<float> coneRadiusInput) : base(ability, laserWidth, laserRange, ignoreLos, false, 360f, 1f, 0f)
	{
		base.SetClampToCursorPos(clampConeCenterToCursor);
		base.SetExplodeOnPathEnd(!onlyUseConeIfLaserHit);
		base.SetExplodeOnEnvironmentHit(!onlyUseConeIfLaserHit);
		for (int i = 0; i < coneRadiusInput.Count; i++)
		{
			this.m_coneRadiusList.Add(new RadiusToLayerIndex(coneRadiusInput[i]));
		}
		if (this.m_coneRadiusList.Count == 0)
		{
			Log.Error("no radius list passed to " + base.GetType(), new object[0]);
			this.m_coneRadiusList.Add(new RadiusToLayerIndex(1f));
		}
		RadiusToLayerIndex.SortAndSetLayerIndex(this.m_coneRadiusList);
		this.m_outerConeRadius = this.m_coneRadiusList[this.m_coneRadiusList.Count - 1].m_radius;
	}

	public override float GetConeRadius()
	{
		return this.m_outerConeRadius;
	}

	protected override void AllocateConeHighlights()
	{
		if (this.m_highlights.Count == 1)
		{
			for (int i = 0; i < this.m_coneRadiusList.Count; i++)
			{
				bool flag = i == this.m_coneRadiusList.Count - 1;
				float radiusInWorld = this.m_coneRadiusList[i].m_radius * Board.SquareSizeStatic;
				GameObject gameObject = HighlightUtils.Get().CreateConeCursor(radiusInWorld, this.GetConeWidthAngle());
				if (!flag)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_LaserWithLayeredRings.AllocateConeHighlights()).MethodHandle;
					}
					UIDynamicCone component = gameObject.GetComponent<UIDynamicCone>();
					if (component != null)
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
						component.SetConeObjectActive(false);
					}
				}
				this.m_highlights.Add(gameObject);
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
	}

	public override void AddTargetedActor(ActorData actor, Vector3 damageOrigin, ActorData targetingActor, AbilityTooltipSubject subjectType = AbilityTooltipSubject.Primary)
	{
		if (this.IsActorInTargetRange(actor))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_LaserWithLayeredRings.AddTargetedActor(ActorData, Vector3, ActorData, AbilityTooltipSubject)).MethodHandle;
			}
			return;
		}
		base.AddTargetedActor(actor, damageOrigin, targetingActor, subjectType);
		ActorHitContext actorHitContext = this.m_actorContextVars[actor];
		if (subjectType == AbilityTooltipSubject.Primary)
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
			actorHitContext.\u0015.SetInt(ContextKeys.\u001A.GetHash(), 0);
		}
		else
		{
			RadiusToLayerIndex bestMatchingData = AbilityCommon_LayeredRings.GetBestMatchingData<RadiusToLayerIndex>(this.m_coneRadiusList, actor.GetCurrentBoardSquare(), damageOrigin, targetingActor, true);
			float value = VectorUtils.HorizontalPlaneDistInSquares(damageOrigin, actor.GetTravelBoardSquareWorldPosition());
			actorHitContext.\u0015.SetInt(ContextKeys.\u001A.GetHash(), 1);
			actorHitContext.\u0015.SetInt(ContextKeys.\u0003.GetHash(), bestMatchingData.m_index);
			actorHitContext.\u0015.SetFloat(ContextKeys.\u0018.GetHash(), value);
		}
	}
}
