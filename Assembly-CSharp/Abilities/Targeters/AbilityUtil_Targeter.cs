// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
using AbilityContextNamespace;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class AbilityUtil_Targeter
{
	public enum AffectsActor
	{
		Never,
		Possible,
		Always
	}

	public class ActorTarget
	{
		public ActorData m_actor;
		public Vector3 m_damageOrigin;
		public bool m_ignoreCoverMinDist;
		public List<AbilityTooltipSubject> m_subjectTypes;
	}

	public enum TargeterMovementType
	{
		Knockback,
		Movement,
		Attacking
	}

	public class ArrowList
	{
		public GameObject m_gameObject;
		public BoardSquarePathInfo m_pathInfo;
	}

	// removed in rogues
	private static readonly int[] materialColorProperties = new int[2]
	{
		Shader.PropertyToID("_Color"),
		Shader.PropertyToID("_TintColor")
	};

	protected Ability m_ability;
	protected bool m_useMultiTargetUpdate;
	protected List<GameObject> m_highlights;
	// removed in rogues
	protected TargeterTemplateFadeContainer m_highlightFadeContainer;
	protected Dictionary<ActorData, ActorHitContext> m_actorContextVars;
	protected ContextVars m_nonActorSpecificContext;
	protected bool m_shouldShowActorRadius;
	private PathStart m_gameObjectOnCaster;
	private PathEnd m_gameObjectOnCastTarget;
	private GameObject m_lineBetweenPath;
	protected bool m_showArcToShape;
	private GameObject m_targetingArcInstance;
	private GameObject m_targetingArcPulseInstance;
	private Vector3 m_arcEnd;
	private float m_arcTraveled;
	private Vector3 m_cameraForward;
	private Vector3 m_cameraPosition;
	private List<ActorTarget> m_actorsInRange = new List<ActorTarget>();
	// removed in rogues
	protected List<ActorData> m_actorsAddedSoFar = new List<ActorData>();
	protected List<ArrowList> m_arrows = new List<ArrowList>();
	protected HighlightUtils.CursorType m_cursorType = HighlightUtils.CursorType.NoCursorType;
	protected bool m_affectsEnemies = true;
	protected bool m_affectsAllies;
	protected bool m_affectsTargetingActor;
	private float m_lastAllyTargeterChange;
	private float m_confirmedTargetingStartTime;

	public GridPos LastUpdatingGridPos { get; set; }
	public Vector3 LastUpdateFreePos { get; set; }
	public Vector3 LastUpdateAimDir { get; set; }
	// removed in rogues
	public bool MarkedForForceUpdate { get; set; }

	public bool ShowArcToShape
	{
		get
		{
			return m_showArcToShape;
		}
		set
		{
			m_showArcToShape = value;
		}
	}

	protected GameObject Highlight
	{
		get
		{
			if (m_highlights != null && m_highlights.Count > 0)
			{
				return m_highlights[0];
			}
			return null;
		}
		set
		{
			if (m_highlights == null)
			{
				m_highlights = new List<GameObject>();
			}
			if (m_highlights.Count == 0)
			{
				m_highlights.Add(null);
			}
			if (m_highlights[0] != null)
			{
				DestroyObjectAndMaterials(m_highlights[0]);
				m_highlights[0] = null;
			}
			m_highlights[0] = value;
		}
	}

	public AbilityUtil_Targeter(Ability ability)
	{
		m_highlights = new List<GameObject>();

		// removed in rogues
		m_highlightFadeContainer = new TargeterTemplateFadeContainer();

		m_ability = ability;
		m_actorContextVars = new Dictionary<ActorData, ActorHitContext>();
		m_nonActorSpecificContext = new ContextVars();
	}

	private void OnDestroy()
	{
		DestroyTargetingArcMesh();
	}

	public void SetLastUpdateCursorState(AbilityTarget target)
	{
		LastUpdatingGridPos = target.GridPos;
		LastUpdateFreePos = target.FreePos;
		LastUpdateAimDir = target.AimDirection;
	}

	// removed in rogues
	public bool IsCursorStateSameAsLastUpdate(AbilityTarget target)
	{
		return LastUpdatingGridPos.CoordsEqual(target.GridPos)
			&& LastUpdateFreePos == target.FreePos
			&& LastUpdateAimDir == target.AimDirection;
	}

	public virtual void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
	}

	public virtual void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
	}

	public virtual void UpdateHighlightPosAfterClick(AbilityTarget target, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
	}

	public virtual bool IsUsingMultiTargetUpdate()
	{
		return m_useMultiTargetUpdate;
	}

	public void SetUseMultiTargetUpdate(bool value)
	{
		m_useMultiTargetUpdate = value;
	}

	public bool ShouldShowActorRadius()
	{
		return m_shouldShowActorRadius;
	}

	public void SetShowArcToShape(bool showArc)
	{
		ShowArcToShape = showArc;
	}

	public void TargeterAbilitySelected()
	{
		HighlightUtils.Get().SetCursorType(m_cursorType);
		DestroyTeleportStartObject();
		if (m_ability != null
			&& AbilityUtils.AbilityHasTag(m_ability, AbilityTags.UseTeleportUIEffect)
			&& GameFlowData.Get() != null
			&& GameFlowData.Get().activeOwnedActorData != null
			&& HighlightUtils.Get().m_teleportPrefab != null)
		{
			m_gameObjectOnCaster = Object.Instantiate(HighlightUtils.Get().m_teleportPrefab);
			m_gameObjectOnCaster.transform.position = GameFlowData.Get().activeOwnedActorData.transform.position;
			m_gameObjectOnCaster.SetColor(GetTeleportColor());
		}

		// removed in rogues
		MarkedForForceUpdate = true;
	}

	public Color GetTeleportColor()
	{
		Color result = HighlightUtils.Get().s_teleportMovementLineColor;
		if (m_ability.IsFlashAbility())
		{
			result = HighlightUtils.Get().s_flashMovementLineColor;
		}
		return result;
	}

	public virtual void AbilityCasted(GridPos startPosition, GridPos endPosition)
	{
		if (m_gameObjectOnCaster != null)
		{
			m_gameObjectOnCaster.AbilityCasted(startPosition, endPosition);
		}
	}

	public void UpdateTargetAreaEffect(AbilityTarget currentTarget, ActorData caster)
	{
		Color teleportColor = GetTeleportColor();
		if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != caster)
		{
			float num = 0.5f;
			teleportColor.a *= num;
			teleportColor.r *= num;
			teleportColor.g *= num;
			teleportColor.b *= num;
		}
		if (m_gameObjectOnCastTarget == null
			&& m_ability != null
			&& AbilityUtils.AbilityHasTag(m_ability, AbilityTags.UseTeleportUIEffect)
			&& HighlightUtils.Get().m_teleportPrefab != null)
		{
			m_gameObjectOnCastTarget = Object.Instantiate(HighlightUtils.Get().m_teleportEndPrefab);
		}
		if (m_gameObjectOnCastTarget != null)
		{
			List<Vector3> list = new List<Vector3>
			{
				Board.Get().GetSquare(caster.GetGridPos()).GetOccupantRefPos(),
				Board.Get().GetSquare(currentTarget.GridPos).GetOccupantRefPos()
			};
			Vector3 a = list[0] - list[1];
			Vector3 a2 = list[list.Count - 1] - list[list.Count - 2];
			a.Normalize();
			a2.Normalize();
			list[0] -= a * Board.Get().squareSize * 0.6f;
			list[list.Count - 1] -= a2 * Board.Get().squareSize * 0.4f;
			DestroyTeleportLine();
			m_lineBetweenPath = Targeting.GetTargeting().CreateLineMesh(list, 0.2f, teleportColor, true, HighlightUtils.Get().m_ArrowDottedLineMaterial);
			m_gameObjectOnCastTarget.transform.position = currentTarget.GetWorldGridPos();
			m_gameObjectOnCastTarget.AbilityCasted(caster.GetGridPos(), currentTarget.GridPos);
			m_gameObjectOnCastTarget.SetColor(teleportColor);
			m_gameObjectOnCastTarget.Setup();
		}
	}

	public void UpdateEffectOnCaster(AbilityTarget currentTarget, ActorData caster)
	{
		Color teleportColor = GetTeleportColor();
		if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != caster)
		{
			float num = 0.5f;
			teleportColor.a *= num;
			teleportColor.r *= num;
			teleportColor.g *= num;
			teleportColor.b *= num;
		}
		if (m_gameObjectOnCaster == null
			&& m_ability != null
			&& AbilityUtils.AbilityHasTag(m_ability, AbilityTags.UseTeleportUIEffect)
			&& HighlightUtils.Get().m_teleportPrefab != null)
		{
			m_gameObjectOnCaster = Object.Instantiate(HighlightUtils.Get().m_teleportPrefab);
		}
		if (m_gameObjectOnCaster != null)
		{
			m_gameObjectOnCaster.transform.position = caster.transform.position;
			m_gameObjectOnCaster.AbilityCasted(caster.GetGridPos(), currentTarget.GridPos);
			m_gameObjectOnCaster.SetColor(teleportColor);
		}
	}

	private void DestroyTeleportLine()
	{
		if (m_lineBetweenPath != null)
		{
			HighlightUtils.DestroyMeshesOnObject(m_lineBetweenPath);
			DestroyObjectAndMaterials(m_lineBetweenPath);
			m_lineBetweenPath = null;
		}
	}

	private void DestroyTeleportStartObject()
	{
		if (m_gameObjectOnCaster != null)
		{
			DestroyObjectAndMaterials(m_gameObjectOnCaster.gameObject);
			m_gameObjectOnCaster = null;
		}
	}

	private void DestroyTeleportEndObjet()
	{
		if (m_gameObjectOnCastTarget != null)
		{
			DestroyObjectAndMaterials(m_gameObjectOnCastTarget.gameObject);
			m_gameObjectOnCastTarget = null;
		}
	}

	public void InstantiateTeleportPathUIEffect()
	{
		if (m_gameObjectOnCaster == null && HighlightUtils.Get().m_teleportPrefab != null)
		{
			m_gameObjectOnCaster = Object.Instantiate(HighlightUtils.Get().m_teleportPrefab);
		}
		if (m_gameObjectOnCastTarget == null
			&& m_ability != null
			&& HighlightUtils.Get().m_teleportPrefab != null)
		{
			m_gameObjectOnCastTarget = Object.Instantiate(HighlightUtils.Get().m_teleportEndPrefab);
		}
	}

	internal void TargeterAbilityDeselected(int targetIndex)
	{
		bool flag = false;
		if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
		{
			AbilityData abilityData = GameFlowData.Get().activeOwnedActorData.GetAbilityData();
			if (abilityData != null)
			{
				flag = abilityData.HasQueuedAction(abilityData.GetActionTypeOfAbility(m_ability));  // , true in rogues
			}
		}
		if (!flag)
		{
			ClearActorsInRange();
			ClearArrows();
			ClearHighlightCursors();
			HideAllSquareIndicators();
			DestroyTeleportStartObject();
			DestroyTeleportLine();
			DestroyTeleportEndObjet();
		}
		if (flag
			&& !m_ability.BackupTargets.IsNullOrEmpty()
			&& targetIndex < m_ability.BackupTargets.Count)
		{
			AbilityTarget abilityTarget = m_ability.BackupTargets[targetIndex];
			ClearHighlightCursors();
			SetLastUpdateCursorState(abilityTarget);
			if (IsUsingMultiTargetUpdate())
			{
				UpdateTargetingMultiTargets(abilityTarget, GameFlowData.Get().activeOwnedActorData, targetIndex, m_ability.BackupTargets);
			}
			else
			{
				UpdateTargeting(abilityTarget, GameFlowData.Get().activeOwnedActorData);
			}

			// removed in rogues
			UpdateHighlightPosAfterClick(abilityTarget, GameFlowData.Get().activeOwnedActorData, targetIndex, m_ability.BackupTargets);
			SetupTargetingArc(GameFlowData.Get().activeOwnedActorData, false);
			// end removed
		}
		HighlightUtils.Get().SetCursorType(HighlightUtils.CursorType.MouseOverCursorType);

		// removed in rogues
		MarkedForForceUpdate = true;
	}

	public virtual void ResetTargeter(bool clearHighlightInstantly)  // no param in rogues
	{
		ClearActorsInRange();
		ClearArrows();
		ClearHighlightCursors(clearHighlightInstantly);
		HideAllSquareIndicators();
		DestroyTeleportStartObject();
		DestroyTeleportLine();
		DestroyTeleportEndObjet();
		m_confirmedTargetingStartTime = 0f;
		m_actorContextVars.Clear();
		m_nonActorSpecificContext.ClearData();

		// removed in rogues
		MarkedForForceUpdate = true;
	}

	public List<AbilityTooltipSubject> GetTooltipSubjectTypes(ActorData actor)
	{
		if (actor != null)
		{
			foreach (ActorTarget actorTarget in m_actorsInRange)
			{
				if (actorTarget.m_actor == actor)
				{
					return actorTarget.m_subjectTypes;
				}
			}
		}
		return null;
	}

	public void AppendToTooltipSubjectSet(ActorData actor, HashSet<AbilityTooltipSubject> subjectsToAppendTo)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = GetTooltipSubjectTypes(actor);
		if (tooltipSubjectTypes != null)
		{
			foreach (AbilityTooltipSubject current in tooltipSubjectTypes)
			{
				subjectsToAppendTo.Add(current);
			}
		}
	}

	public virtual bool IsActorInTargetRange(ActorData actor)
	{
		return IsActorInTargetRange(actor, out var inCover);  // (actor, out HitChanceBracketType hitChanceBracketType) in rogues
	}

	// reactor
	public virtual bool IsActorInTargetRange(ActorData actor, out bool inCover)
	{
		inCover = false;
		if (actor == null)
		{
			return false;
		}

		foreach (ActorTarget actorTarget in m_actorsInRange)
		{
			if (actorTarget.m_actor == actor)
			{
				ActorCover actorCover = actorTarget.m_actor.GetActorCover();
				if (actorCover != null)
				{
					ActorData activeOwnedActorData = GameFlowData.Get().POVActorData;
					bool isEnemy = activeOwnedActorData != null && activeOwnedActorData.GetTeam() != actor.GetTeam();
					bool isIgnoringCover = AbilityUtils.AbilityIgnoreCover(m_ability, actorTarget.m_actor);
					bool isVisibleToClient = actor.IsActorVisibleToClient();
					bool hasCover = actorTarget.m_ignoreCoverMinDist
						? actorCover.IsInCoverWrtDirectionOnly(actorTarget.m_damageOrigin, actorTarget.m_actor.GetCurrentBoardSquare())
						: actorCover.IsInCoverWrt(actorTarget.m_damageOrigin);
					inCover = isEnemy && !isIgnoringCover && isVisibleToClient && hasCover;
				}
				return true;
			}
		}
		return false;
	}
	// rogues
	//public virtual bool IsActorInTargetRange(ActorData actor, out HitChanceBracketType strongestCover)
	//{
	//	bool result = false;
	//	strongestCover = HitChanceBracketType.Default;
	//	if (actor != null)
	//	{
	//		int i = 0;
	//		while (i < m_actorsInRange.Count)
	//		{
	//			ActorTarget actorTarget = m_actorsInRange[i];
	//			if (actorTarget.m_actor == actor)
	//			{
	//				result = true;
	//				ActorCover actorCover = actorTarget.m_actor.GetActorCover();
	//				if (!(actorCover != null))
	//				{
	//					break;
	//				}
	//				ActorData povactorData = GameFlowData.Get().POVActorData;
	//				bool flag = povactorData != null && povactorData.GetTeam() != actor.GetTeam();
	//				bool flag2 = AbilityUtils.AbilityIgnoreCover(m_ability, actorTarget.m_actor);
	//				bool flag3 = actor.IsActorVisibleToClient();
	//				bool flag4 = actorTarget.m_ignoreCoverMinDist ? actorCover.IsInCoverWrtDirectionOnly(actorTarget.m_damageOrigin, actorTarget.m_actor.GetCurrentBoardSquare(), out strongestCover) : actorCover.IsInCoverWrt(actorTarget.m_damageOrigin, out strongestCover);
	//				if (!flag || flag2 || !flag3 || !flag4)
	//				{
	//					strongestCover = HitChanceBracketType.Default;
	//					break;
	//				}
	//				break;
	//			}
	//			else
	//			{
	//				i++;
	//			}
	//		}
	//	}
	//	return result;
	//}

	public List<ActorTarget> GetActorsInRange()
	{
		return m_actorsInRange;
	}

	public virtual int GetNumActorsInRange()
	{
		return m_actorsInRange.Count;
	}

	protected virtual List<ActorData> GetVisibleActorsInRange()
	{
		List<ActorData> list = new List<ActorData>();
		foreach (ActorTarget actorTarget in m_actorsInRange)
		{
			if (actorTarget.m_actor.IsActorVisibleToClient())
			{
				list.Add(actorTarget.m_actor);
			}
		}
		return list;
	}

	public virtual List<ActorData> GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject subject)
	{
		List<ActorData> list = new List<ActorData>();
		foreach (ActorTarget actorTarget in m_actorsInRange)
		{
			if (actorTarget.m_actor.IsActorVisibleToClient() && actorTarget.m_subjectTypes.Contains(subject))
			{
				list.Add(actorTarget.m_actor);
			}
		}
		return list;
	}

	public int GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject subject)
	{
		int num = 0;
		foreach (ActorTarget actorTarget in m_actorsInRange)
		{
			if (actorTarget.m_actor.IsActorVisibleToClient()
				&& actorTarget.m_subjectTypes.Contains(subject))
			{
				num++;
			}
		}
		return num;
	}

	public int GetTooltipSubjectCountOnActor(ActorData actor, AbilityTooltipSubject subject)
	{
		int num = 0;
		foreach (ActorTarget actorTarget in m_actorsInRange)
		{
			if (actorTarget.m_actor == actor)
			{
				foreach (AbilityTooltipSubject subjectType in actorTarget.m_subjectTypes)
				{
					if (subjectType == subject)
					{
						num++;
					}
				}
			}
		}
		return num;
	}

	public int GetTooltipSubjectCountTotalWithDuplicates(AbilityTooltipSubject subject)
	{
		int num = 0;
		foreach (ActorTarget actorTarget in m_actorsInRange)
		{
			foreach (AbilityTooltipSubject subjectType in actorTarget.m_subjectTypes)
			{
				if (subjectType == subject)
				{
					num++;
				}
			}
		}
		return num;
	}

	protected void AddActorsInRange(List<ActorData> actors, Vector3 damageOrigin, ActorData targetingActor, AbilityTooltipSubject subjectType = AbilityTooltipSubject.Primary, bool appendSubjectType = false)
	{
		foreach (ActorData actor in actors)
		{
			AddActorInRange(actor, damageOrigin, targetingActor, subjectType, appendSubjectType);
		}
	}

	protected void AddActorInRange(ActorData actor, Vector3 damageOrigin, ActorData targetingActor, AbilityTooltipSubject subjectType = AbilityTooltipSubject.Primary, bool appendSubjectType = false)
	{
		if (!IsActorInTargetRange(actor))
		{
			ActorTarget actorTarget = new ActorTarget
			{
				m_actor = actor,
				m_damageOrigin = damageOrigin,
				m_subjectTypes = new List<AbilityTooltipSubject>
				{
					subjectType
				}
			};
			if (actor == targetingActor)
			{
				actorTarget.m_subjectTypes.Add(AbilityTooltipSubject.Self);
			}
			else if (actor.GetTeam() == targetingActor.GetTeam())
			{
				actorTarget.m_subjectTypes.Add(AbilityTooltipSubject.Ally);
			}
			else
			{
				actorTarget.m_subjectTypes.Add(AbilityTooltipSubject.Enemy);
			}
			for (AbilityTooltipSubject i = AbilityTooltipSubject.FirstOneoffSubject; i <= AbilityTooltipSubject.LastOneoffSubject; i++)
			{
				if (DoesTargetActorMatchTooltipSubject(i, actor, damageOrigin, targetingActor))
				{
					actorTarget.m_subjectTypes.Add(i);
				}
			}
			m_actorsInRange.Add(actorTarget);

			// removed in rogues
			m_actorsAddedSoFar.Add(actor);

			if (!m_actorContextVars.ContainsKey(actor))
			{
				ActorHitContext actorHitContext = new ActorHitContext();
				actorHitContext.m_inRangeForTargeter = true;
				m_actorContextVars[actor] = actorHitContext;
			}
			else
			{
				m_actorContextVars[actor].m_inRangeForTargeter = true;
			}

			// added in rogues
#if SERVER
			int count = targetingActor.GetActorTurnSM().GetAbilityTargets().Count;
			m_actorContextVars[actor].m_contextVars.SetValue(ContextKeys.s_TargeterIndex.GetKey(), count);
#endif
		}
		else if (appendSubjectType)
		{
			foreach (ActorTarget item in m_actorsInRange)
			{
				if (item.m_actor == actor)
				{
					item.m_subjectTypes.Add(subjectType);
					break;
				}
			}
		}
	}

	public Dictionary<ActorData, ActorHitContext> GetActorContextVars()
	{
		return m_actorContextVars;
	}

	public ContextVars GetNonActorSpecificContext()
	{
		return m_nonActorSpecificContext;
	}

	protected void SetIgnoreCoverMinDist(ActorData actor, bool ignoreCoverMinDist)
	{
		foreach (ActorTarget actorTarget in m_actorsInRange)
		{
			if (actorTarget.m_actor == actor)
			{
				actorTarget.m_ignoreCoverMinDist = ignoreCoverMinDist;
				return;
			}
		}
	}

	protected virtual bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		return m_ability.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
	}

	public void ClearActorsInRange()
	{
		m_actorsInRange.Clear();

		// removed in rogues
		m_actorsAddedSoFar.Clear();

		foreach (ActorHitContext value in m_actorContextVars.Values)
		{
			value.m_inRangeForTargeter = false;
#if SERVER
			if (GameFlowData.Get() != null && GameFlowData.Get().POVActorData != null)  // no check for GameFlowData.Get().POVActorData != null in rogues
			{
				int index = GameFlowData.Get().POVActorData.GetActorTurnSM().GetAbilityTargets().Count - 1;
				value.m_contextVars.SetValue(ContextKeys.s_TargeterIndex.GetKey(), index);
			}
#endif
		}
#if SERVER
		SetMovementArrowEnabledFromIndex(0, false);
#endif
	}

	protected void AddMovementArrow(ActorData mover, BoardSquarePathInfo path, TargeterMovementType movementType, MovementPathStart previousLine = null, bool isChasing = false)
	{
		Color arrowColor;
		switch (movementType)
		{
			case TargeterMovementType.Knockback:
				arrowColor = HighlightUtils.Get().s_knockbackMovementLineColor;
				break;
			case TargeterMovementType.Movement:
				arrowColor = HighlightUtils.Get().s_dashMovementLineColor;
				break;
			case TargeterMovementType.Attacking:
				arrowColor = HighlightUtils.Get().s_attackMovementLineColor;
				break;
			default:
				arrowColor = HighlightUtils.Get().s_knockbackMovementLineColor;
				break;
		}
		AddMovementArrow(mover, path, arrowColor, previousLine, isChasing, movementType);
	}

	public void UpdateArrowsForUI()
	{
		bool flag = false;
		foreach (ArrowList arrow in m_arrows)
		{
			if (arrow.m_gameObject.activeSelf)
			{
				flag = true;
				List<Vector3> list = KnockbackUtils.BuildDrawablePath(arrow.m_pathInfo, false);
				MovementPathStart componentInChildren = arrow.m_gameObject.GetComponentInChildren<MovementPathStart>();
				Vector3 vector2D = list[list.Count - 1];
				BoardSquare boardSquare = Board.Get().GetSquareFromVec3(vector2D);
				componentInChildren.SetCharacterMovementPanel(boardSquare);
			}
		}
		if (!flag && m_arrows.Count > 0)
		{
			m_arrows[0].m_gameObject.GetComponentInChildren<MovementPathStart>().HideCharacterMovementPanel();
		}
	}

	protected void AddMovementArrow(ActorData mover, BoardSquarePathInfo path, Color arrowColor, MovementPathStart previousLine, bool isChasing, TargeterMovementType movementType)
	{
		if (path != null && mover != null && mover.IsActorVisibleToClient())
		{
			List<Vector3> points = KnockbackUtils.BuildDrawablePath(path, false);
			if (points.Count >= 2)
			{
				GameObject gameObject = Targeting.GetTargeting().CreateFancyArrowMesh(ref points, 0.2f, arrowColor, isChasing, mover, movementType, null, previousLine, false);
				bool flag = false;
				foreach (ArrowList arrow in m_arrows)
				{
					if (arrow.m_gameObject == gameObject)
					{
						flag = true;
						arrow.m_pathInfo = path;
						break;
					}
				}
				if (!flag && gameObject.GetComponentInChildren<MovementPathStart>() != null)
				{
					ArrowList arrowList = new ArrowList
					{
						m_gameObject = gameObject,
						m_pathInfo = path
					};
					m_arrows.Add(arrowList);
				}
			}
		}
	}

	protected int AddMovementArrowWithPrevious(ActorData mover, BoardSquarePathInfo path, TargeterMovementType movementType, int arrowIndex, bool isChasing = false)  // public in rogues
	{
		Color arrowColor;
		switch (movementType)
		{
			case TargeterMovementType.Knockback:
				arrowColor = HighlightUtils.Get().s_knockbackMovementLineColor;
				break;
			case TargeterMovementType.Movement:
				arrowColor = HighlightUtils.Get().s_dashMovementLineColor;
				break;
			case TargeterMovementType.Attacking:
				arrowColor = HighlightUtils.Get().s_attackMovementLineColor;
				break;
			default:
				arrowColor = HighlightUtils.Get().s_knockbackMovementLineColor;
				break;
		}
		return AddMovementArrowWithPrevious(mover, path, movementType, arrowColor, arrowIndex, isChasing);
	}

	protected int AddMovementArrowWithPrevious(ActorData mover, BoardSquarePathInfo path, TargeterMovementType movementType, Color arrowColor, int arrowIndex, bool isChasing = false)
	{
		if (CanCreateMovementArrows(path))
		{
			MovementPathStart previousLine = null;
			if (m_arrows.Count > arrowIndex)
			{
				previousLine = m_arrows[arrowIndex].m_gameObject.GetComponentInChildren<MovementPathStart>();
			}
			AddMovementArrow(mover, path, arrowColor, previousLine, isChasing, movementType);
			return arrowIndex + 1;
		}
		return arrowIndex;
	}

	protected void EnableAllMovementArrows()  // public in rogues
	{
		SetMovementArrowEnabledFromIndex(0, true);
	}

	protected void SetMovementArrowEnabledFromIndex(int fromIndex, bool enabled)  // public in rogues
	{
		for (int i = fromIndex; i < m_arrows.Count; i++)
		{
			m_arrows[i].m_gameObject.SetActive(enabled);
		}
	}

	protected bool CanCreateMovementArrows(BoardSquarePathInfo path)
	{
		if (path == null)
		{
			return false;
		}
		List<Vector3> list = KnockbackUtils.BuildDrawablePath(path, false);
		if (list.Count >= 2)
		{
			Vector3 vector = list[0] - list[1];
			Vector3 vector2 = list[list.Count - 1] - list[list.Count - 2];
			if (vector.sqrMagnitude > 0f && vector2.sqrMagnitude > 0f)
			{
				return true;
			}
		}
		return false;
	}

	public void ClearArrows()
	{
		foreach (ArrowList arrowList in m_arrows)
		{
			if (arrowList != null && arrowList.m_gameObject != null)
			{
				MovementPathStart componentInChildren = arrowList.m_gameObject.GetComponentInChildren<MovementPathStart>();
				if (componentInChildren != null)
				{
					componentInChildren.HideCharacterMovementPanel();
				}
				DestroyObjectAndMaterials(arrowList.m_gameObject);
			}
		}
		m_arrows.Clear();
	}

	public static GameObject CreateArrowFromGridPosPath(List<GridPos> gridPosPath, Color lineColor, bool isChasing, ActorData theActor)
	{
		GameObject result = null;
		if (gridPosPath.Count >= 2)
		{
			GridPos start = gridPosPath[0];
			GridPos end = gridPosPath[gridPosPath.Count - 1];
			result = CreateArrowFromGridPosPoints(start, end, lineColor, isChasing, theActor);
		}
		return result;
	}

	public static GameObject CreateArrowFromGridPosPoints(GridPos start, GridPos end, Color lineColor, bool isChasing, ActorData theActor)
	{
		List<Vector3> points = new List<Vector3>();
		Vector3 occupantRefPos = Board.Get().GetSquare(start).GetOccupantRefPos();
		occupantRefPos.y += 0.5f;
		points.Add(occupantRefPos);
		Vector3 occupantRefPos2 = Board.Get().GetSquare(end).GetOccupantRefPos();
		occupantRefPos2.y += 0.5f;
		points.Add(occupantRefPos2);
		return Targeting.GetTargeting().CreateFancyArrowMesh(ref points, 0.2f, lineColor, isChasing, theActor, TargeterMovementType.Movement, null, null, false);
	}

	public List<GameObject> GetHighlightCopies(bool setOpacity)
	{
		List<GameObject> list = new List<GameObject>();
		foreach (GameObject highlight in m_highlights)
		{
			if (highlight != null)
			{
				GameObject gameObject = HighlightUtils.Get().CloneTargeterHighlight(highlight, this);
				if (!highlight.activeSelf)
				{
					gameObject.SetActive(false);
				}
				list.Add(gameObject);
			}
		}
		if (setOpacity)
		{
			float opacityFromTargeterData = GetOpacityFromTargeterData(HighlightUtils.Get().m_allyTargeterOpacity, 100f);
			SetTargeterHighlightOpacity(list, opacityFromTargeterData);
		}
		return list;
	}

	protected virtual void ClearHighlightCursors(bool clearInstantly = true)
	{
		if (!clearInstantly && NetworkClient.active)  // unconditionally else in rogues
		{
			if (m_highlights.Count > 0)
			{
				m_highlightFadeContainer.TrackHighlights(m_highlights);
			}
		}
		else
		{
			foreach (GameObject gameObject in m_highlights)
			{
				if (gameObject != null)
				{
					DestroyObjectAndMaterials(gameObject);
				}
			}
		}
		m_highlights.Clear();
		DestroyTargetingArcMesh();
		if (m_targetingArcPulseInstance != null)
		{
			Object.Destroy(m_targetingArcPulseInstance);
			m_targetingArcPulseInstance = null;
		}
	}

	protected void DestroyObjectAndMaterials(GameObject highlightObj)
	{
		if (highlightObj != null)
		{
			HighlightUtils.DestroyObjectAndMaterials(highlightObj);
		}
	}

	protected virtual void DisableHighlightCursors()
	{
		if (m_highlights != null)
		{
			foreach (GameObject highlight in m_highlights)
			{
				if (highlight != null)
				{
					highlight.SetActive(false);
				}
			}
		}
	}

	public List<TargeterTemplateSwapData> GetTemplateSwapData()
	{
		if (m_ability != null)
		{
			return m_ability.m_targeterTemplateSwaps;
		}
		return null;
	}

	public HighlightUtils.CursorType GetCursorType()
	{
		return m_cursorType;
	}

	public void SetAffectedGroups(bool affectsEnemies, bool affectsAllies, bool affectsCaster)
	{
		m_affectsEnemies = affectsEnemies;
		m_affectsAllies = affectsAllies;
		m_affectsTargetingActor = affectsCaster;
	}

	protected bool GetAffectsTarget(ActorData potentialTarget, ActorData targeterOwner)
	{
		if (potentialTarget == null || targeterOwner == null)
		{
			return false;
		}
		if (targeterOwner.GetTeam() != potentialTarget.GetTeam())
		{
			return m_affectsEnemies;
		}
		if (targeterOwner == potentialTarget)
		{
			return m_affectsTargetingActor;
		}
		return m_affectsAllies;
	}

	protected List<Team> GetAffectedTeams()
	{
		return GetAffectedTeams(GameFlowData.Get().activeOwnedActorData);
	}

	public List<Team> GetAffectedTeams(ActorData targeterOwner)
	{
		List<Team> list = new List<Team>();
		if (targeterOwner != null)
		{
			if (m_affectsAllies)
			{
				list.Add(targeterOwner.GetTeam());
			}
			if (m_affectsEnemies)
			{
#if VANILLA
				list.Add(targeterOwner.GetEnemyTeam()); // reactor
#else
				list.AddRange(targeterOwner.GetOtherTeams()); // rogues
#endif
			}
		}
		return list;
	}

	protected virtual float GetCurrentRangeInSquares()
	{
		return AbilityUtils.GetCurrentRangeInSquares(m_ability, GameFlowData.Get().activeOwnedActorData, 0);
	}

	protected virtual Vector3 GetTargetingArcEndPosition(ActorData targetingActor)
	{
		if (Highlight != null)
		{
			return Highlight.transform.position;
		}
		if (targetingActor != null && targetingActor.GetCurrentBoardSquare() != null)
		{
			return targetingActor.GetCurrentBoardSquare().ToVector3();
		}
		return Vector3.zero;
	}

	public virtual void SetupTargetingArc(ActorData targetingActor, bool activatePulse)
	{
		if (m_showArcToShape && Highlight == null)
		{
			m_showArcToShape = false;
		}
		if (m_showArcToShape
			&& Highlight != null
			&& (GetTargetingArcEndPosition(targetingActor) - targetingActor.GetCurrentBoardSquare().ToVector3()).magnitude > HighlightUtils.Get().m_minDistForTargetingArc)
		{
			Vector3 loSCheckPos = targetingActor.GetLoSCheckPos();
			Vector3 vector = Camera.main.transform.rotation * Vector3.forward;
			bool flag = (vector - m_cameraForward).sqrMagnitude > 0.01f
				|| (Camera.main.transform.position - m_cameraPosition).sqrMagnitude > 0.01f;
			if (flag)
			{
				m_cameraForward = vector;
				m_cameraPosition = Camera.main.transform.position;
			}
			if (activatePulse)
			{
				if (m_targetingArcPulseInstance != null)
				{
					m_targetingArcPulseInstance.SetActive(false);
					Object.Destroy(m_targetingArcPulseInstance);
					m_targetingArcPulseInstance = null;
				}
				m_targetingArcPulseInstance = Object.Instantiate(HighlightUtils.Get().m_targetingArcForShape);
				m_targetingArcPulseInstance.SetActive(true);
				m_targetingArcPulseInstance.transform.position = loSCheckPos;
				m_arcTraveled = 0f;
			}
			Vector3 targetingArcEndPosition = GetTargetingArcEndPosition(targetingActor);
			if (m_arcTraveled < 1f || flag)
			{
				float targetingArcMaxHeight = HighlightUtils.Get().m_targetingArcMaxHeight;
				float num = 1f + (loSCheckPos.y - targetingArcEndPosition.y) / targetingArcMaxHeight;
				Vector3 vector2 = targetingArcEndPosition - loSCheckPos;
				vector2.y = 0f;
				if (vector2.magnitude <= 0.5f)
				{
					vector2 = new Vector3(0.5f * vector.x, 0f, 0.5f * vector.z);
				}
				float num2 = 0.5f * vector2.magnitude;
				float num3 = targetingArcMaxHeight / (num2 * num2);
				if (m_targetingArcPulseInstance != null)
				{
					m_arcTraveled += HighlightUtils.Get().m_targetingArcMovementSpeed * Time.deltaTime;
					float num4 = m_arcTraveled * vector2.magnitude - num2;
					float num5 = num3 * num4 * num4;
					if (num4 > 0f)
					{
						num5 *= num;
					}
					Vector3 position = loSCheckPos + vector2 * m_arcTraveled + Vector3.up * (targetingArcMaxHeight - num5);
					m_targetingArcPulseInstance.transform.position = position;
				}
				bool flag2 = false;
				if ((m_arcEnd - targetingArcEndPosition).sqrMagnitude > 0.1f)
				{
					m_arcEnd = targetingArcEndPosition;
					flag2 = true;
				}
				if (m_targetingArcInstance == null || flag || flag2)
				{
					List<Vector3> list = new List<Vector3>();
					for (int i = 1; i <= HighlightUtils.Get().m_targetingArcNumSegments; i++)
					{
						float num6 = i / (float)HighlightUtils.Get().m_targetingArcNumSegments;
						float num7 = num6 * vector2.magnitude - num2;
						float num8 = num3 * num7 * num7;
						if (num7 > 0f)
						{
							num8 *= num;
						}
						Vector3 item = loSCheckPos + vector2 * num6 + Vector3.up * (targetingArcMaxHeight - num8);
						list.Add(item);
					}
					Color color = HighlightUtils.Get().m_targetingArcColor;
					if (targetingActor != GameFlowData.Get().activeOwnedActorData)
					{
						color = HighlightUtils.Get().m_targetingArcColorAllies;
					}
					m_targetingArcInstance = Targeting.GetTargeting().CreateLineMesh(list, 0.2f, color, false, HighlightUtils.Get().m_targetingArcMaterial, m_targetingArcInstance, true);
				}
			}
			else
			{
				m_arcTraveled = 0f;
				if (m_targetingArcPulseInstance != null)
				{
					m_targetingArcPulseInstance.SetActive(false);
					Object.Destroy(m_targetingArcPulseInstance);
					m_targetingArcPulseInstance = null;
				}
			}
		}
		else
		{
			DestroyTargetingArcMesh();
			if (m_targetingArcPulseInstance != null)
			{
				m_targetingArcPulseInstance.SetActive(false);
				Object.Destroy(m_targetingArcPulseInstance);
				m_targetingArcPulseInstance = null;
			}
		}
	}

	private void DestroyTargetingArcMesh()
	{
		if (m_targetingArcInstance != null)
		{
			m_targetingArcInstance.SetActive(false);
			HighlightUtils.DestroyMeshesOnObject(m_targetingArcInstance);
			DestroyObjectAndMaterials(m_targetingArcInstance);
			m_targetingArcInstance = null;
		}
	}

	public virtual void AdjustOpacityWhileTargeting()
	{
		if (NetworkClient.active && HighlightUtils.Get().m_setTargeterOpacityWhileTargeting)
		{
			float opacity = Mathf.Clamp(HighlightUtils.Get().m_targeterOpacityWhileTargeting, 0.01f, 1f);
			foreach (GameObject gameObject in m_highlights)
			{
				foreach (MeshRenderer meshRenderer in gameObject.GetComponents<MeshRenderer>())
				{
					SetMaterialOpacity(meshRenderer.materials, opacity);
				}
				foreach (MeshRenderer meshRenderer in gameObject.GetComponentsInChildren<MeshRenderer>())
				{
					SetMaterialOpacity(meshRenderer.materials, opacity);
				}
			}
		}
	}

	public virtual void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		m_lastAllyTargeterChange = Time.time;
		m_confirmedTargetingStartTime = Time.time + HUD_UIResources.Get().m_confirmedTargetingDuration;
		foreach (ArrowList arrowList in m_arrows)
		{
			if (arrowList != null && arrowList.m_gameObject.activeSelf)
			{
				MovementPathStart componentInChildren = arrowList.m_gameObject.GetComponentInChildren<MovementPathStart>();
				if (componentInChildren != null)
				{
					componentInChildren.SetGlow(true);
				}
			}
		}
		if (GameFlowData.Get().activeOwnedActorData == targetingActor
			// removed in rogues
			&& targetingActor.GetActorTurnSM().CurrentState != TurnStateEnum.TARGETING_ACTION)
		{
			HideAllSquareIndicators();
		}
		SetupTargetingArc(targetingActor, true);
		if (Application.isEditor && m_highlights != null && m_ability != null)
		{
			foreach (GameObject gameObject in m_highlights)
			{
				if (gameObject != null && !gameObject.name.StartsWith("[Targeter]"))
				{
					gameObject.name = "[Targeter] " + m_ability.GetNameString() + ": " + gameObject.name;
				}
			}
		}
	}

	public virtual void UpdateConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float timeSinceChange = Time.time - m_lastAllyTargeterChange;
		HighlightUtils.TargeterOpacityData[] targeterOpacity =
			GameFlowData.Get().activeOwnedActorData == targetingActor
			? HighlightUtils.Get().m_confirmedTargeterOpacity
			: HighlightUtils.Get().m_allyTargeterOpacity;
		float opacityFromTargeterData = GetOpacityFromTargeterData(targeterOpacity, timeSinceChange);
		SetTargeterHighlightOpacity(m_highlights, opacityFromTargeterData);
		SetupTargetingArc(targetingActor, false);
	}

	// removed in rogues
	public void UpdateFadeOutHighlights(ActorData targetingActor)
	{
		m_highlightFadeContainer.UpdateFade(targetingActor, m_highlights.Count > 0);
	}

	public static void SetTargeterHighlightOpacity(List<GameObject> highlights, float opacity)
	{
		foreach (GameObject highlight in highlights)
		{
			foreach (MeshRenderer meshRenderer in highlight.GetComponents<MeshRenderer>())
			{
				SetMaterialOpacity(meshRenderer.materials, opacity);
			}
			foreach (MeshRenderer meshRenderer in highlight.GetComponentsInChildren<MeshRenderer>())
			{
				SetMaterialOpacity(meshRenderer.materials, opacity);
			}
			float opacity2 = Mathf.Clamp(opacity * HighlightUtils.Get().m_targeterParticleSystemOpacityMultiplier, 0f, 1f);
			foreach (ParticleSystemRenderer particleSystemRenderer in highlight.GetComponentsInChildren<ParticleSystemRenderer>())
			{
				SetMaterialOpacity(particleSystemRenderer.materials, opacity2);
			}
		}
	}

	// removed in rogues
	public static void SetTargeterHighlightColor(List<GameObject> highlights, Color color, bool keepOpacity = true, bool clearColorOverTime = true)
	{
		foreach (GameObject gameObject in highlights)
		{
			foreach (MeshRenderer meshRenderer in gameObject.GetComponents<MeshRenderer>())
			{
				SetMaterialColor(meshRenderer.materials, color, keepOpacity);
			}
			foreach (MeshRenderer meshRenderer2 in gameObject.GetComponentsInChildren<MeshRenderer>())
			{
				SetMaterialColor(meshRenderer2.materials, color, keepOpacity);
			}
			foreach (ParticleSystemRenderer particleSystemRenderer in gameObject.GetComponentsInChildren<ParticleSystemRenderer>())
			{
				if (clearColorOverTime)
				{
					ParticleSystem component = particleSystemRenderer.GetComponent<ParticleSystem>();
					if (component != null)
					{
						ParticleSystem.ColorOverLifetimeModule colorOverLifetime = component.colorOverLifetime;
						if (colorOverLifetime.enabled)
						{
							colorOverLifetime.enabled = false;
						}
					}
				}
				SetMaterialColor(particleSystemRenderer.materials, color, keepOpacity);
			}
		}
	}

	public static float GetOpacityFromTargeterData(HighlightUtils.TargeterOpacityData[] targeterOpacity, float timeSinceChange)
	{
		float result = 1f;
		for (int i = 0; i < targeterOpacity.Length; i++)
		{
			if (i == targeterOpacity.Length - 1)
			{
				result = targeterOpacity[i].m_alpha;
			}
			else if (targeterOpacity[i].m_timeSinceConfirmed <= timeSinceChange
				&& timeSinceChange <= targeterOpacity[i + 1].m_timeSinceConfirmed)
			{
				float alpha = targeterOpacity[i].m_alpha;
				float alpha2 = targeterOpacity[i + 1].m_alpha;
				float num2 = (timeSinceChange - targeterOpacity[i].m_timeSinceConfirmed) / Mathf.Max(0.01f, targeterOpacity[i + 1].m_timeSinceConfirmed - targeterOpacity[i].m_timeSinceConfirmed);
				result = alpha * (1f - num2) + alpha2 * num2;
				break;
			}
		}
		return result;
	}

	internal static void SetMaterialOpacity(Material[] materials, float opacity)
	{
		foreach (Material material in materials)
		{
			foreach (int nameID in materialColorProperties)
			{
				if (material.HasProperty(nameID))
				{
					Color color = material.GetColor(nameID);
					Color value = new Color(color.r, color.g, color.b, opacity);
					material.SetColor(nameID, value);
				}
			}
		}
	}

	internal static void SetMaterialColor(Material[] materials, Color newColor, bool keepOpacity = true)
	{
		foreach (Material material in materials)
		{
			foreach (int nameID in materialColorProperties)
			{
				if (material.HasProperty(nameID))
				{
					if (keepOpacity)
					{
						Color color = material.GetColor(nameID);
						newColor.a = color.a;
					}
					material.SetColor(nameID, newColor);
				}
			}
		}
	}

	public float GetConfirmedTargetingRemainingTime()
	{
		return Mathf.Max(m_confirmedTargetingStartTime - Time.time, 0f);
	}

	public void HideAllSquareIndicators()
	{
		if (HighlightUtils.GetHiddenSquaresContainer() != null)
		{
			HighlightUtils.GetHiddenSquaresContainer().HideAllSquareIndicators();
		}

		// removed in rogues
		if (HighlightUtils.GetAffectedSquaresContainer() != null)
		{
			HighlightUtils.GetAffectedSquaresContainer().HideAllSquareIndicators();
		}
	}

	public void ResetSquareIndicatorIndexToUse()
	{
		if (HighlightUtils.GetHiddenSquaresContainer() != null)
		{
			HighlightUtils.GetHiddenSquaresContainer().ResetNextIndicatorIndex();
		}

		// removed in rogues
		if (HighlightUtils.GetAffectedSquaresContainer() != null)
		{
			HighlightUtils.GetAffectedSquaresContainer().ResetNextIndicatorIndex();
		}
	}

	public void HideUnusedSquareIndicators()
	{
		if (HighlightUtils.GetHiddenSquaresContainer() != null)
		{
			HighlightUtils.GetHiddenSquaresContainer().HideAllSquareIndicators(GetNextHiddenSquareIndicatorIndex());
		}

		// removed in rogues
		if (HighlightUtils.GetAffectedSquaresContainer() != null)
		{
			HighlightUtils.GetAffectedSquaresContainer().HideAllSquareIndicators(GetNextAffectedSquareIndicatorIndex());
		}
	}

	public int GetNextHiddenSquareIndicatorIndex()
	{
		if (HighlightUtils.GetHiddenSquaresContainer() != null)
		{
			return HighlightUtils.GetHiddenSquaresContainer().GetNextIndicatorIndex();
		}
		return 0;
	}

	public int ShowHiddenSquareIndicatorForSquare(BoardSquare square)
	{
		if (HighlightUtils.GetHiddenSquaresContainer() != null)
		{
			return HighlightUtils.GetHiddenSquaresContainer().ShowIndicatorForSquare(square);
		}
		return 0;
	}

	private static Mesh CreateRectMesh(float halfWidth, float halfHeight)
	{
		Mesh mesh = new Mesh
		{
			vertices = new Vector3[]
			{
				new Vector3(-halfWidth, 0f, -halfHeight),
				new Vector3(halfWidth, 0f, -halfHeight),
				new Vector3(halfWidth, 0f, halfHeight),
				new Vector3(-halfWidth, 0f, halfHeight)
			},
			uv = new Vector2[]
			{
				new Vector2(0f, 0f),
				new Vector2(0f, 1f),
				new Vector2(1f, 1f),
				new Vector2(1f, 0f)
			},
			triangles = new int[]
			{
				0,
				1,
				2,
				0,
				2,
				3
			}
		};
		mesh.RecalculateNormals();
		return mesh;
	}

	// removed in rogues
	public int GetNextAffectedSquareIndicatorIndex()
	{
		if (HighlightUtils.GetAffectedSquaresContainer() != null)
		{
			return HighlightUtils.GetAffectedSquaresContainer().GetNextIndicatorIndex();
		}
		return 0;
	}

	// removed in rogues
	public int ShowAffectedSquareIndicatorForSquare(BoardSquare square)
	{
		if (HighlightUtils.GetAffectedSquaresContainer() != null)
		{
			return HighlightUtils.GetAffectedSquaresContainer().ShowIndicatorForSquare(square);
		}
		return 0;
	}

	public virtual void DrawGizmos(AbilityTarget currentTarget, ActorData targetingActor)
	{
	}
}
