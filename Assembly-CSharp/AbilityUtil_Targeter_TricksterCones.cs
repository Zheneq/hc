using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_TricksterCones : AbilityUtil_Targeter
{
	public ConeTargetingInfo m_coneInfo;

	private int m_maxCones;

	public bool m_showHitIndicatorLine;

	private bool m_useCasterPosForLoS;

	public Dictionary<ActorData, int> m_actorToHitCount = new Dictionary<ActorData, int>();

	public Dictionary<ActorData, int> m_actorToCoverCount = new Dictionary<ActorData, int>();

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	private AbilityUtil_Targeter_TricksterCones.GetCurrentNumberOfConesDelegate GetCurrentNumberOfCones;

	private AbilityUtil_Targeter_TricksterCones.GetClampedTargetPosDelegate GetClampedTargetPos;

	private AbilityUtil_Targeter_TricksterCones.GetConeInfoDelegate GetConeOrigins;

	private AbilityUtil_Targeter_TricksterCones.GetConeInfoDelegate GetConeDirections;

	public AbilityUtil_Targeter_TricksterCones.DamageOriginDelegate m_customDamageOriginDelegate;

	private Dictionary<ActorData, Vector3> m_tempActorToDamageOrigins = new Dictionary<ActorData, Vector3>();

	public AbilityUtil_Targeter_TricksterCones(Ability ability, ConeTargetingInfo coneTargetingInfo, int maxCones, AbilityUtil_Targeter_TricksterCones.GetCurrentNumberOfConesDelegate numConesDelegate, AbilityUtil_Targeter_TricksterCones.GetConeInfoDelegate coneOriginsDelegate, AbilityUtil_Targeter_TricksterCones.GetConeInfoDelegate coneDirectionsDelegate, AbilityUtil_Targeter_TricksterCones.GetClampedTargetPosDelegate clampedTargetPosDelegate, bool showHitIndicatorLine, bool useCasterPosForLoS) : base(ability)
	{
		this.m_coneInfo = coneTargetingInfo;
		this.m_maxCones = maxCones;
		this.m_useCasterPosForLoS = useCasterPosForLoS;
		this.GetCurrentNumberOfCones = numConesDelegate;
		this.GetClampedTargetPos = clampedTargetPosDelegate;
		this.GetConeOrigins = coneOriginsDelegate;
		this.GetConeDirections = coneDirectionsDelegate;
		base.SetAffectedGroups(this.m_coneInfo.m_affectsEnemies, this.m_coneInfo.m_affectsAllies, this.m_coneInfo.m_affectsCaster);
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		for (int i = 0; i < this.m_maxCones; i++)
		{
			this.m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
		}
		this.m_showHitIndicatorLine = showHitIndicatorLine;
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (this.m_highlights != null && this.m_highlights.Count == this.m_maxCones * 2)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_TricksterCones.StartConfirmedTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			for (int i = this.m_maxCones; i < this.m_highlights.Count; i++)
			{
				this.m_highlights[i].SetActive(false);
			}
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		float squareSize = Board.Get().squareSize;
		float radiusInWorld = (this.m_coneInfo.m_radiusInSquares + this.m_coneInfo.m_backwardsOffset) * squareSize;
		if (this.m_highlights != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_TricksterCones.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			if (this.m_highlights.Count >= this.m_maxCones * 2)
			{
				goto IL_FF;
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
		this.m_highlights = new List<GameObject>();
		for (int i = 0; i < this.m_maxCones; i++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateConeCursor(radiusInWorld, this.m_coneInfo.m_widthAngleDeg));
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
		for (int j = 0; j < this.m_maxCones; j++)
		{
			GameObject item = HighlightUtils.Get().CreateDynamicLineSegmentMesh(1f, 0.3f, true, Color.cyan);
			this.m_highlights.Add(item);
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
		IL_FF:
		bool flag;
		if (GameFlowData.Get() != null)
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
			flag = (GameFlowData.Get().activeOwnedActorData == targetingActor);
		}
		else
		{
			flag = false;
		}
		bool active = flag;
		int num = this.GetCurrentNumberOfCones();
		for (int k = 0; k < this.m_maxCones; k++)
		{
			if (k < num)
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
				this.m_highlights[k].SetActive(true);
				this.m_highlights[this.m_maxCones + k].SetActive(active);
			}
			else
			{
				this.m_highlights[k].SetActive(false);
				this.m_highlights[this.m_maxCones + k].SetActive(false);
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
		Vector3 freeTargetPos = this.GetClampedTargetPos(currentTarget, targetingActor);
		List<Vector3> list = this.GetConeOrigins(currentTarget, freeTargetPos, targetingActor);
		List<Vector3> list2 = this.GetConeDirections(currentTarget, freeTargetPos, targetingActor);
		this.m_tempActorToDamageOrigins.Clear();
		Dictionary<ActorData, Vector3> tempActorToDamageOrigins = this.m_tempActorToDamageOrigins;
		this.m_actorToHitCount.Clear();
		this.m_actorToCoverCount.Clear();
		int l = 0;
		while (l < num)
		{
			if (l >= this.m_maxCones)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					goto IL_6EE;
				}
			}
			else
			{
				Vector3 vector = list[l];
				Vector3 vector2 = list2[l];
				vector2.y = 0f;
				float magnitude = vector2.magnitude;
				vector2.Normalize();
				float num2 = VectorUtils.HorizontalAngle_Deg(vector2);
				List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(vector, num2, this.m_coneInfo.m_widthAngleDeg, this.m_coneInfo.m_radiusInSquares, this.m_coneInfo.m_backwardsOffset, this.m_coneInfo.m_penetrateLos, targetingActor, base.GetAffectedTeams(), null, false, default(Vector3));
				TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
				if (actorsInCone.Contains(targetingActor))
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
					actorsInCone.Remove(targetingActor);
				}
				foreach (ActorData actorData in actorsInCone)
				{
					ActorCover actorCover = actorData.GetActorCover();
					bool flag2 = actorCover.IsInCoverWrt(vector);
					if (tempActorToDamageOrigins.ContainsKey(actorData))
					{
						Dictionary<ActorData, int> dictionary;
						ActorData key;
						(dictionary = this.m_actorToHitCount)[key = actorData] = dictionary[key] + 1;
						Dictionary<ActorData, int> dictionary2 = dictionary = this.m_actorToCoverCount;
						ActorData key3;
						ActorData key2 = key3 = actorData;
						int num3 = dictionary[key3];
						int num4;
						if (flag2)
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
							num4 = 1;
						}
						else
						{
							num4 = 0;
						}
						dictionary2[key2] = num3 + num4;
						if (actorCover != null)
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
							if (!actorCover.IsInCoverWrt(tempActorToDamageOrigins[actorData]))
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
								if (actorCover.IsInCoverWrt(vector))
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
									tempActorToDamageOrigins[actorData] = vector;
								}
							}
						}
					}
					else
					{
						this.m_actorToHitCount[actorData] = 1;
						Dictionary<ActorData, int> actorToCoverCount = this.m_actorToCoverCount;
						ActorData key4 = actorData;
						int value;
						if (flag2)
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
							value = 1;
						}
						else
						{
							value = 0;
						}
						actorToCoverCount[key4] = value;
						tempActorToDamageOrigins[actorData] = vector;
					}
				}
				if (this.m_affectsTargetingActor)
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
					base.AddActorInRange(targetingActor, vector, targetingActor, AbilityTooltipSubject.Tertiary, false);
				}
				float d = this.m_coneInfo.m_backwardsOffset * squareSize;
				Vector3 position = vector - vector2 * d;
				position.y = HighlightUtils.GetHighlightHeight();
				this.m_highlights[l].transform.position = position;
				this.m_highlights[l].transform.rotation = Quaternion.LookRotation(vector2);
				if (this.m_showHitIndicatorLine)
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
					this.m_highlights[this.m_maxCones + l].transform.position = position;
					this.m_highlights[this.m_maxCones + l].transform.rotation = Quaternion.LookRotation(vector2);
				}
				else if (this.m_highlights[this.m_maxCones + l].activeSelf)
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
					this.m_highlights[this.m_maxCones + l].SetActive(false);
				}
				Color color;
				if (actorsInCone.Count > 0)
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
					color = Color.red;
				}
				else
				{
					color = Color.cyan;
				}
				Color color2 = color;
				HighlightUtils.Get().AdjustDynamicLineSegmentMesh(this.m_highlights[this.m_maxCones + l], magnitude / Board.Get().squareSize, color2);
				if (l != num - 1)
				{
					goto IL_671;
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
				if (l >= this.m_squarePosCheckerList.Count - 1)
				{
					goto IL_671;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int m = l; m < this.m_squarePosCheckerList.Count; m++)
				{
					SquareInsideChecker_Cone squareInsideChecker_Cone = this.m_squarePosCheckerList[m] as SquareInsideChecker_Cone;
					squareInsideChecker_Cone.UpdateConeProperties(vector, this.m_coneInfo.m_widthAngleDeg, this.m_coneInfo.m_radiusInSquares, this.m_coneInfo.m_backwardsOffset, num2, targetingActor);
					if (this.m_useCasterPosForLoS)
					{
						squareInsideChecker_Cone.SetLosPosOverride(true, targetingActor.GetTravelBoardSquareWorldPositionForLos(), true);
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
				IL_6CB:
				l++;
				continue;
				IL_671:
				SquareInsideChecker_Cone squareInsideChecker_Cone2 = this.m_squarePosCheckerList[l] as SquareInsideChecker_Cone;
				squareInsideChecker_Cone2.UpdateConeProperties(vector, this.m_coneInfo.m_widthAngleDeg, this.m_coneInfo.m_radiusInSquares, this.m_coneInfo.m_backwardsOffset, num2, targetingActor);
				if (this.m_useCasterPosForLoS)
				{
					squareInsideChecker_Cone2.SetLosPosOverride(true, targetingActor.GetTravelBoardSquareWorldPositionForLos(), true);
					goto IL_6CB;
				}
				goto IL_6CB;
			}
		}
		IL_6EE:
		foreach (KeyValuePair<ActorData, Vector3> keyValuePair in tempActorToDamageOrigins)
		{
			ActorData key5 = keyValuePair.Key;
			Vector3 vector3 = keyValuePair.Value;
			if (this.m_customDamageOriginDelegate != null)
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
				vector3 = this.m_customDamageOriginDelegate(currentTarget, vector3, key5, targetingActor);
			}
			for (int n = 0; n < this.m_actorToHitCount[key5]; n++)
			{
				AbilityTooltipSubject subjectType = (key5.GetTeam() != targetingActor.GetTeam()) ? AbilityTooltipSubject.Primary : AbilityTooltipSubject.Secondary;
				base.AddActorInRange(key5, vector3, targetingActor, subjectType, true);
				ActorHitContext actorHitContext = this.m_actorContextVars[key5];
				actorHitContext.\u0015.SetInt(ContextKeys.\u0019.GetHash(), this.m_actorToHitCount[key5]);
			}
		}
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
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
			base.ResetSquareIndicatorIndexToUse();
			for (int num5 = 0; num5 < num; num5++)
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
				if (num5 >= this.m_maxCones)
				{
					break;
				}
				Vector3 coneStart = list[num5];
				Vector3 vec = list2[num5];
				vec.y = 0f;
				vec.Normalize();
				float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
				AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, coneStart, coneCenterAngleDegrees, this.m_coneInfo.m_widthAngleDeg, this.m_coneInfo.m_radiusInSquares, this.m_coneInfo.m_backwardsOffset, targetingActor, this.m_coneInfo.m_penetrateLos, this.m_squarePosCheckerList);
			}
			base.HideUnusedSquareIndicators();
		}
	}

	public delegate int GetCurrentNumberOfConesDelegate();

	public delegate Vector3 GetClampedTargetPosDelegate(AbilityTarget currentTarget, ActorData caster);

	public delegate List<Vector3> GetConeInfoDelegate(AbilityTarget currentTarget, Vector3 freeTargetPos, ActorData caster);

	public delegate Vector3 DamageOriginDelegate(AbilityTarget currentTarget, Vector3 defaultOrigin, ActorData actorToAdd, ActorData caster);
}
