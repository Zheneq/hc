using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_LayerCones : AbilityUtil_Targeter
{
	public List<float> m_coneRadiusList;

	public float m_coneWidthAngle;

	public bool m_penetrateLoS;

	public float m_coneBackwardOffsetInSquares;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_LayerCones.IsAffectingCasterDelegate m_affectCasterDelegate;

	public AbilityUtil_Targeter_LayerCones.NumActiveLayerDelegate m_delegateNumActiveLayers;

	public AbilityUtil_Targeter_LayerCones(Ability ability, float coneWidthAngle, List<float> coneRadiusList, float coneBackwardOffsetInSquares, bool penetrateLoS) : base(ability)
	{
		this.m_coneWidthAngle = coneWidthAngle;
		this.m_coneRadiusList = coneRadiusList;
		this.m_penetrateLoS = penetrateLoS;
		this.m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public float GetMaxConeRadius()
	{
		float result = 0f;
		int numActiveLayers = this.GetNumActiveLayers();
		if (numActiveLayers > 0)
		{
			result = this.m_coneRadiusList[numActiveLayers - 1];
		}
		return result;
	}

	public float GetActiveLayerRadius()
	{
		return this.m_coneRadiusList[this.GetNumActiveLayers() - 1];
	}

	public int GetNumActiveLayers()
	{
		if (this.m_delegateNumActiveLayers != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_LayerCones.GetNumActiveLayers()).MethodHandle;
			}
			int count = this.m_coneRadiusList.Count;
			return Mathf.Min(count, this.m_delegateNumActiveLayers(count));
		}
		return this.m_coneRadiusList.Count;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector;
		if (currentTarget == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_LayerCones.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			vector = targetingActor.transform.forward;
		}
		else
		{
			vector = currentTarget.AimDirection;
		}
		Vector3 vector2 = vector;
		int numActiveLayers = this.GetNumActiveLayers();
		this.m_nonActorSpecificContext.SetInt(ContextKeys.\u000F.GetHash(), numActiveLayers);
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vector2);
		this.HandleConeCursorHighlights(travelBoardSquareWorldPositionForLos, vector2, numActiveLayers);
		List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, this.m_coneWidthAngle, this.GetMaxConeRadius(), this.m_coneBackwardOffsetInSquares, this.m_penetrateLoS, targetingActor, TargeterUtils.GetRelevantTeams(targetingActor, this.m_affectsAllies, this.m_affectsEnemies), null, false, default(Vector3));
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
		actorsInCone.Remove(targetingActor);
		if (this.m_affectsTargetingActor)
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
			if (this.m_affectCasterDelegate != null)
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
				if (!this.m_affectCasterDelegate(targetingActor, actorsInCone))
				{
					goto IL_10C;
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
			}
			actorsInCone.Add(targetingActor);
		}
		IL_10C:
		using (List<ActorData>.Enumerator enumerator = actorsInCone.GetEnumerator())
		{
			IL_1FB:
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (this.ShouldAddActor(actorData, targetingActor))
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
					base.AddActorInRange(actorData, travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Primary, false);
					int i = 0;
					while (i < this.m_coneRadiusList.Count)
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
						if (i >= numActiveLayers)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								goto IL_1FB;
							}
						}
						else
						{
							bool flag = AreaEffectUtils.IsSquareInConeByActorRadius(actorData.GetCurrentBoardSquare(), travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, this.m_coneWidthAngle, this.m_coneRadiusList[i], this.m_coneBackwardOffsetInSquares, true, targetingActor, false, default(Vector3));
							if (flag)
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
								ActorHitContext actorHitContext = this.m_actorContextVars[actorData];
								actorHitContext.\u0015.SetInt(ContextKeys.\u0003.GetHash(), i);
								break;
							}
							i++;
						}
					}
				}
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
		}
		this.DrawInvalidSquareIndicators(currentTarget, targetingActor);
	}

	private bool ShouldAddActor(ActorData actor, ActorData caster)
	{
		bool result = false;
		if (actor == caster)
		{
			result = this.m_affectsTargetingActor;
		}
		else
		{
			if (actor.GetTeam() == caster.GetTeam())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_LayerCones.ShouldAddActor(ActorData, ActorData)).MethodHandle;
				}
				if (this.m_affectsAllies)
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
					return true;
				}
			}
			if (actor.GetTeam() != caster.GetTeam())
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
				if (this.m_affectsEnemies)
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
					result = true;
				}
			}
		}
		return result;
	}

	public void HandleConeCursorHighlights(Vector3 casterPos, Vector3 centerAimDir, int numConesActive)
	{
		float d = this.m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = casterPos + new Vector3(0f, y, 0f) - centerAimDir * d;
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= this.m_coneRadiusList.Count)
			{
				goto IL_125;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_LayerCones.HandleConeCursorHighlights(Vector3, Vector3, int)).MethodHandle;
			}
		}
		this.m_highlights = new List<GameObject>();
		for (int i = 0; i < this.m_coneRadiusList.Count; i++)
		{
			float radiusInWorld = (this.m_coneRadiusList[i] + this.m_coneBackwardOffsetInSquares) * Board.Get().squareSize;
			GameObject gameObject = HighlightUtils.Get().CreateConeCursor(radiusInWorld, this.m_coneWidthAngle);
			UIDynamicCone component = gameObject.GetComponent<UIDynamicCone>();
			if (component != null)
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
				component.SetConeObjectActive(false);
			}
			this.m_highlights.Add(gameObject);
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
		IL_125:
		for (int j = 0; j < this.m_highlights.Count; j++)
		{
			if (j < numConesActive)
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
				this.m_highlights[j].transform.position = position;
				this.m_highlights[j].transform.rotation = Quaternion.LookRotation(centerAimDir);
				this.m_highlights[j].gameObject.SetActiveIfNeeded(true);
			}
			else
			{
				this.m_highlights[j].gameObject.SetActiveIfNeeded(false);
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			Vector3 vec = (currentTarget != null) ? currentTarget.AimDirection : targetingActor.transform.forward;
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, travelBoardSquareWorldPositionForLos, coneCenterAngleDegrees, this.m_coneWidthAngle, this.GetMaxConeRadius(), this.m_coneBackwardOffsetInSquares, targetingActor, this.m_penetrateLoS, null);
			base.HideUnusedSquareIndicators();
		}
	}

	public delegate bool IsAffectingCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	public delegate int NumActiveLayerDelegate(int maxLayers);
}
