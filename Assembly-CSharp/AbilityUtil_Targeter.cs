using AbilityContextNamespace;
using System.Collections.Generic;
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

	private static readonly int[] materialColorProperties = new int[2]
	{
		Shader.PropertyToID("_Color"),
		Shader.PropertyToID("_TintColor")
	};

	protected Ability m_ability;
	protected bool m_useMultiTargetUpdate;
	protected List<GameObject> m_highlights;
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

	public bool IsCursorStateSameAsLastUpdate(AbilityTarget target)
	{
		if (LastUpdatingGridPos.CoordsEqual(target.GridPos) && LastUpdateFreePos == target.FreePos)
		{
			return LastUpdateAimDir == target.AimDirection;
		}
		return false;
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
		if (m_ability != null)
		{
			if (AbilityUtils.AbilityHasTag(m_ability, AbilityTags.UseTeleportUIEffect))
			{
				if (GameFlowData.Get() != null)
				{
					if (GameFlowData.Get().activeOwnedActorData != null && HighlightUtils.Get().m_teleportPrefab != null)
					{
						m_gameObjectOnCaster = Object.Instantiate(HighlightUtils.Get().m_teleportPrefab);
						m_gameObjectOnCaster.transform.position = GameFlowData.Get().activeOwnedActorData.transform.position;
						m_gameObjectOnCaster.SetColor(GetTeleportColor());
					}
				}
			}
		}
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
		if (!(m_gameObjectOnCaster != null))
		{
			return;
		}
		while (true)
		{
			m_gameObjectOnCaster.AbilityCasted(startPosition, endPosition);
			return;
		}
	}

	public void UpdateTargetAreaEffect(AbilityTarget currentTarget, ActorData caster)
	{
		Color teleportColor = GetTeleportColor();
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().activeOwnedActorData != caster)
			{
				float num = 0.5f;
				teleportColor.a *= num;
				teleportColor.r *= num;
				teleportColor.g *= num;
				teleportColor.b *= num;
			}
		}
		if (m_gameObjectOnCastTarget == null)
		{
			if (m_ability != null)
			{
				if (AbilityUtils.AbilityHasTag(m_ability, AbilityTags.UseTeleportUIEffect) && HighlightUtils.Get().m_teleportPrefab != null)
				{
					m_gameObjectOnCastTarget = Object.Instantiate(HighlightUtils.Get().m_teleportEndPrefab);
				}
			}
		}
		if (!(m_gameObjectOnCastTarget != null))
		{
			return;
		}
		while (true)
		{
			List<Vector3> list = new List<Vector3>();
			Vector3 worldPosition = Board.Get().GetSquare(caster.GetGridPosWithIncrementedHeight()).GetOccupantRefPos();
			list.Add(worldPosition);
			Vector3 worldPosition2 = Board.Get().GetSquare(currentTarget.GridPos).GetOccupantRefPos();
			list.Add(worldPosition2);
			Vector3 a = list[0] - list[1];
			Vector3 a2 = list[list.Count - 1] - list[list.Count - 2];
			a.Normalize();
			a2.Normalize();
			list[0] -= a * Board.Get().squareSize * 0.6f;
			list[list.Count - 1] -= a2 * Board.Get().squareSize * 0.4f;
			DestroyTeleportLine();
			m_lineBetweenPath = Targeting.GetTargeting().CreateLineMesh(list, 0.2f, teleportColor, true, HighlightUtils.Get().m_ArrowDottedLineMaterial);
			m_gameObjectOnCastTarget.transform.position = currentTarget.GetWorldGridPos();
			m_gameObjectOnCastTarget.AbilityCasted(caster.GetGridPosWithIncrementedHeight(), currentTarget.GridPos);
			m_gameObjectOnCastTarget.SetColor(teleportColor);
			m_gameObjectOnCastTarget.Setup();
			return;
		}
	}

	public void UpdateEffectOnCaster(AbilityTarget currentTarget, ActorData caster)
	{
		Color teleportColor = GetTeleportColor();
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().activeOwnedActorData != caster)
			{
				float num = 0.5f;
				teleportColor.a *= num;
				teleportColor.r *= num;
				teleportColor.g *= num;
				teleportColor.b *= num;
			}
		}
		if (m_gameObjectOnCaster == null && m_ability != null)
		{
			if (AbilityUtils.AbilityHasTag(m_ability, AbilityTags.UseTeleportUIEffect) && HighlightUtils.Get().m_teleportPrefab != null)
			{
				m_gameObjectOnCaster = Object.Instantiate(HighlightUtils.Get().m_teleportPrefab);
			}
		}
		if (!(m_gameObjectOnCaster != null))
		{
			return;
		}
		while (true)
		{
			m_gameObjectOnCaster.transform.position = caster.transform.position;
			m_gameObjectOnCaster.AbilityCasted(caster.GetGridPosWithIncrementedHeight(), currentTarget.GridPos);
			m_gameObjectOnCaster.SetColor(teleportColor);
			return;
		}
	}

	private void DestroyTeleportLine()
	{
		if (!(m_lineBetweenPath != null))
		{
			return;
		}
		while (true)
		{
			HighlightUtils.DestroyMeshesOnObject(m_lineBetweenPath);
			DestroyObjectAndMaterials(m_lineBetweenPath);
			m_lineBetweenPath = null;
			return;
		}
	}

	private void DestroyTeleportStartObject()
	{
		if (!(m_gameObjectOnCaster != null))
		{
			return;
		}
		while (true)
		{
			DestroyObjectAndMaterials(m_gameObjectOnCaster.gameObject);
			m_gameObjectOnCaster = null;
			return;
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
		if (m_gameObjectOnCaster == null)
		{
			if (HighlightUtils.Get().m_teleportPrefab != null)
			{
				m_gameObjectOnCaster = Object.Instantiate(HighlightUtils.Get().m_teleportPrefab);
			}
		}
		if (!(m_gameObjectOnCastTarget == null))
		{
			return;
		}
		while (true)
		{
			if (m_ability != null && HighlightUtils.Get().m_teleportPrefab != null)
			{
				m_gameObjectOnCastTarget = Object.Instantiate(HighlightUtils.Get().m_teleportEndPrefab);
			}
			return;
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
				flag = abilityData.HasQueuedAction(abilityData.GetActionTypeOfAbility(m_ability));
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
		if (flag)
		{
			if (!m_ability.BackupTargets.IsNullOrEmpty())
			{
				if (targetIndex < m_ability.BackupTargets.Count)
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
					UpdateHighlightPosAfterClick(abilityTarget, GameFlowData.Get().activeOwnedActorData, targetIndex, m_ability.BackupTargets);
					SetupTargetingArc(GameFlowData.Get().activeOwnedActorData, false);
				}
			}
		}
		HighlightUtils.Get().SetCursorType(HighlightUtils.CursorType.MouseOverCursorType);
		MarkedForForceUpdate = true;
	}

	public virtual void ResetTargeter(bool clearHighlightInstantly)
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
		if (tooltipSubjectTypes == null)
		{
			return;
		}
		foreach (AbilityTooltipSubject current in tooltipSubjectTypes)
		{
			subjectsToAppendTo.Add(current);
		}
	}

	public virtual bool IsActorInTargetRange(ActorData actor)
	{
		return IsActorInTargetRange(actor, out var inCover);
	}

	public virtual bool IsActorInTargetRange(ActorData actor, out bool inCover)
	{
		inCover = false;
		if (actor == null)
		{
			return false;
		}

		for (int num = 0; num < m_actorsInRange.Count; num++)
		{
			ActorTarget actorTarget = m_actorsInRange[num];
			if (actorTarget.m_actor == actor)
			{
				ActorCover actorCover = actorTarget.m_actor.GetActorCover();
				if (actorCover != null)
				{
					ActorData activeOwnedActorData = GameFlowData.Get().POVActorData;
					bool isEnemy = activeOwnedActorData != null && activeOwnedActorData.GetTeam() != actor.GetTeam();
					bool isIgnoringCover = AbilityUtils.AbilityIgnoreCover(m_ability, actorTarget.m_actor);
					bool isVisibleToClient = actor.IsVisibleToClient();
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
		using (List<ActorTarget>.Enumerator enumerator = m_actorsInRange.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorTarget current = enumerator.Current;
				if (current.m_actor.IsVisibleToClient())
				{
					list.Add(current.m_actor);
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
	}

	public virtual List<ActorData> GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject subject)
	{
		List<ActorData> list = new List<ActorData>();
		for (int i = 0; i < m_actorsInRange.Count; i++)
		{
			ActorTarget actorTarget = m_actorsInRange[i];
			if (actorTarget.m_actor.IsVisibleToClient() && actorTarget.m_subjectTypes.Contains(subject))
			{
				list.Add(actorTarget.m_actor);
			}
		}
		while (true)
		{
			return list;
		}
	}

	public int GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject subject)
	{
		int num = 0;
		for (int i = 0; i < m_actorsInRange.Count; i++)
		{
			ActorTarget actorTarget = m_actorsInRange[i];
			if (!actorTarget.m_actor.IsVisibleToClient())
			{
				continue;
			}
			if (actorTarget.m_subjectTypes.Contains(subject))
			{
				num++;
			}
		}
		while (true)
		{
			return num;
		}
	}

	public int GetTooltipSubjectCountOnActor(ActorData actor, AbilityTooltipSubject subject)
	{
		int num = 0;
		for (int i = 0; i < m_actorsInRange.Count; i++)
		{
			ActorTarget actorTarget = m_actorsInRange[i];
			if (!(actorTarget.m_actor == actor))
			{
				continue;
			}
			for (int j = 0; j < actorTarget.m_subjectTypes.Count; j++)
			{
				if (actorTarget.m_subjectTypes[j] == subject)
				{
					num++;
				}
			}
		}
		while (true)
		{
			return num;
		}
	}

	public int GetTooltipSubjectCountTotalWithDuplicates(AbilityTooltipSubject subject)
	{
		int num = 0;
		for (int i = 0; i < m_actorsInRange.Count; i++)
		{
			ActorTarget actorTarget = m_actorsInRange[i];
			for (int j = 0; j < actorTarget.m_subjectTypes.Count; j++)
			{
				if (actorTarget.m_subjectTypes[j] == subject)
				{
					num++;
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					goto end_IL_0055;
				}
				continue;
				end_IL_0055:
				break;
			}
		}
		while (true)
		{
			return num;
		}
	}

	protected void AddActorsInRange(List<ActorData> actors, Vector3 damageOrigin, ActorData targetingActor, AbilityTooltipSubject subjectType = AbilityTooltipSubject.Primary, bool appendSubjectType = false)
	{
		for (int i = 0; i < actors.Count; i++)
		{
			AddActorInRange(actors[i], damageOrigin, targetingActor, subjectType, appendSubjectType);
		}
	}

	protected void AddActorInRange(ActorData actor, Vector3 damageOrigin, ActorData targetingActor, AbilityTooltipSubject subjectType = AbilityTooltipSubject.Primary, bool appendSubjectType = false)
	{
		if (!IsActorInTargetRange(actor))
		{
			ActorTarget actorTarget = new ActorTarget();
			actorTarget.m_actor = actor;
			actorTarget.m_damageOrigin = damageOrigin;
			actorTarget.m_subjectTypes = new List<AbilityTooltipSubject>();
			actorTarget.m_subjectTypes.Add(subjectType);
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
			m_actorsAddedSoFar.Add(actor);
			if (!m_actorContextVars.ContainsKey(actor))
			{
				ActorHitContext actorHitContext = new ActorHitContext();
				actorHitContext.m_inRangeForTargeter = true;
				m_actorContextVars[actor] = actorHitContext;
				return;
			}
			else
			{
				m_actorContextVars[actor].m_inRangeForTargeter = true;
			}
		}
		else if (appendSubjectType)
		{
			foreach (ActorTarget item in m_actorsInRange)
			{
				if (item.m_actor == actor)
				{
					item.m_subjectTypes.Add(subjectType);
					return;
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
		for (int i = 0; i < m_actorsInRange.Count; i++)
		{
			if (m_actorsInRange[i].m_actor == actor)
			{
				m_actorsInRange[i].m_ignoreCoverMinDist = ignoreCoverMinDist;
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
		m_actorsAddedSoFar.Clear();
		foreach (KeyValuePair<ActorData, ActorHitContext> actorContextVar in m_actorContextVars)
		{
			actorContextVar.Value.m_inRangeForTargeter = false;
		}
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
		for (int i = 0; i < m_arrows.Count; i++)
		{
			if (m_arrows[i].m_gameObject.activeSelf)
			{
				flag = true;
				List<Vector3> list = KnockbackUtils.BuildDrawablePath(m_arrows[i].m_pathInfo, false);
				MovementPathStart componentInChildren = m_arrows[i].m_gameObject.GetComponentInChildren<MovementPathStart>();
				Vector3 vector2D = list[list.Count - 1];
				BoardSquare boardSquare = Board.Get().GetSquare(vector2D);
				componentInChildren.SetCharacterMovementPanel(boardSquare);
			}
		}
		while (true)
		{
			if (flag)
			{
				return;
			}
			while (true)
			{
				if (m_arrows.Count > 0)
				{
					while (true)
					{
						m_arrows[0].m_gameObject.GetComponentInChildren<MovementPathStart>().HideCharacterMovementPanel();
						return;
					}
				}
				return;
			}
		}
	}

	protected void AddMovementArrow(ActorData mover, BoardSquarePathInfo path, Color arrowColor, MovementPathStart previousLine, bool isChasing, TargeterMovementType movementType)
	{
		if (path == null)
		{
			return;
		}
		while (true)
		{
			if (!(mover != null) || !mover.IsVisibleToClient())
			{
				return;
			}
			List<Vector3> points = KnockbackUtils.BuildDrawablePath(path, false);
			if (points.Count < 2)
			{
				return;
			}
			GameObject gameObject = Targeting.GetTargeting().CreateFancyArrowMesh(ref points, 0.2f, arrowColor, isChasing, mover, movementType, null, previousLine, false);
			bool flag = false;
			int num = 0;
			while (true)
			{
				if (num < m_arrows.Count)
				{
					if (m_arrows[num].m_gameObject == gameObject)
					{
						flag = true;
						m_arrows[num].m_pathInfo = path;
						break;
					}
					num++;
					continue;
				}
				break;
			}
			if (flag)
			{
				return;
			}
			while (true)
			{
				if (gameObject.GetComponentInChildren<MovementPathStart>() != null)
				{
					while (true)
					{
						ArrowList arrowList = new ArrowList();
						arrowList.m_gameObject = gameObject;
						arrowList.m_pathInfo = path;
						m_arrows.Add(arrowList);
						return;
					}
				}
				return;
			}
		}
	}

	protected int AddMovementArrowWithPrevious(ActorData mover, BoardSquarePathInfo path, TargeterMovementType movementType, int arrowIndex, bool isChasing = false)
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					MovementPathStart previousLine = null;
					if (m_arrows.Count > arrowIndex)
					{
						previousLine = m_arrows[arrowIndex].m_gameObject.GetComponentInChildren<MovementPathStart>();
					}
					AddMovementArrow(mover, path, arrowColor, previousLine, isChasing, movementType);
					return arrowIndex + 1;
				}
				}
			}
		}
		return arrowIndex;
	}

	protected void EnableAllMovementArrows()
	{
		SetMovementArrowEnabledFromIndex(0, true);
	}

	protected void SetMovementArrowEnabledFromIndex(int fromIndex, bool enabled)
	{
		for (int i = fromIndex; i < m_arrows.Count; i++)
		{
			m_arrows[i].m_gameObject.SetActive(enabled);
		}
		while (true)
		{
			return;
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
			if (vector.sqrMagnitude > 0f)
			{
				if (vector2.sqrMagnitude > 0f)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void ClearArrows()
	{
		using (List<ArrowList>.Enumerator enumerator = m_arrows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ArrowList current = enumerator.Current;
				if (current != null)
				{
					if (current.m_gameObject != null)
					{
						MovementPathStart componentInChildren = current.m_gameObject.GetComponentInChildren<MovementPathStart>();
						if (componentInChildren != null)
						{
							componentInChildren.HideCharacterMovementPanel();
						}
						DestroyObjectAndMaterials(current.m_gameObject);
					}
				}
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
		Vector3 worldPosition = Board.Get().GetSquare(start).GetOccupantRefPos();
		worldPosition.y += 0.5f;
		points.Add(worldPosition);
		Vector3 worldPosition2 = Board.Get().GetSquare(end).GetOccupantRefPos();
		worldPosition2.y += 0.5f;
		points.Add(worldPosition2);
		return Targeting.GetTargeting().CreateFancyArrowMesh(ref points, 0.2f, lineColor, isChasing, theActor, TargeterMovementType.Movement, null, null, false);
	}

	public List<GameObject> GetHighlightCopies(bool setOpacity)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < m_highlights.Count; i++)
		{
			if (m_highlights[i] != null)
			{
				GameObject gameObject = HighlightUtils.Get().CloneTargeterHighlight(m_highlights[i], this);
				if (!m_highlights[i].activeSelf)
				{
					gameObject.SetActive(false);
				}
				list.Add(gameObject);
			}
		}
		while (true)
		{
			if (setOpacity)
			{
				HighlightUtils.TargeterOpacityData[] allyTargeterOpacity = HighlightUtils.Get().m_allyTargeterOpacity;
				float opacityFromTargeterData = GetOpacityFromTargeterData(allyTargeterOpacity, 100f);
				SetTargeterHighlightOpacity(list, opacityFromTargeterData);
			}
			return list;
		}
	}

	protected virtual void ClearHighlightCursors(bool clearInstantly = true)
	{
		if (!clearInstantly)
		{
			if (NetworkClient.active)
			{
				if (m_highlights.Count > 0)
				{
					m_highlightFadeContainer.TrackHighlights(m_highlights);
				}
				goto IL_00a3;
			}
		}
		using (List<GameObject>.Enumerator enumerator = m_highlights.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				if (current != null)
				{
					DestroyObjectAndMaterials(current);
				}
			}
		}
		goto IL_00a3;
		IL_00a3:
		m_highlights.Clear();
		DestroyTargetingArcMesh();
		if (!(m_targetingArcPulseInstance != null))
		{
			return;
		}
		while (true)
		{
			Object.Destroy(m_targetingArcPulseInstance);
			m_targetingArcPulseInstance = null;
			return;
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
		if (m_highlights == null)
		{
			return;
		}
		while (true)
		{
			foreach (GameObject highlight in m_highlights)
			{
				if (highlight != null)
				{
					highlight.SetActive(false);
				}
			}
			return;
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
		if (!(potentialTarget == null))
		{
			if (!(targeterOwner == null))
			{
				if (targeterOwner.GetTeam() == potentialTarget.GetTeam())
				{
					if (targeterOwner == potentialTarget)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								return m_affectsTargetingActor;
							}
						}
					}
					return m_affectsAllies;
				}
				return m_affectsEnemies;
			}
		}
		return false;
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
				list.Add(targeterOwner.GetEnemyTeam());
			}
		}
		return list;
	}

	protected virtual float GetCurrentRangeInSquares()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		return AbilityUtils.GetCurrentRangeInSquares(m_ability, activeOwnedActorData, 0);
	}

	protected virtual Vector3 GetTargetingArcEndPosition(ActorData targetingActor)
	{
		if (Highlight != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return Highlight.transform.position;
				}
			}
		}
		if (targetingActor != null)
		{
			if (targetingActor.GetCurrentBoardSquare() != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return targetingActor.GetCurrentBoardSquare().ToVector3();
					}
				}
			}
		}
		return Vector3.zero;
	}

	public virtual void SetupTargetingArc(ActorData targetingActor, bool activatePulse)
	{
		if (m_showArcToShape)
		{
			if (Highlight == null)
			{
				m_showArcToShape = false;
			}
		}
		if (m_showArcToShape)
		{
			if (Highlight != null)
			{
				if ((GetTargetingArcEndPosition(targetingActor) - targetingActor.GetCurrentBoardSquare().ToVector3()).magnitude > HighlightUtils.Get().m_minDistForTargetingArc)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
						{
							Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
							Vector3 vector = Camera.main.transform.rotation * Vector3.forward;
							int num;
							if (!((vector - m_cameraForward).sqrMagnitude > 0.01f))
							{
								num = (((Camera.main.transform.position - m_cameraPosition).sqrMagnitude > 0.01f) ? 1 : 0);
							}
							else
							{
								num = 1;
							}
							bool flag = (byte)num != 0;
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
								m_targetingArcPulseInstance.transform.position = travelBoardSquareWorldPositionForLos;
								m_arcTraveled = 0f;
							}
							Vector3 targetingArcEndPosition = GetTargetingArcEndPosition(targetingActor);
							if (!(m_arcTraveled < 1f))
							{
								if (!flag)
								{
									m_arcTraveled = 0f;
									if (m_targetingArcPulseInstance != null)
									{
										while (true)
										{
											switch (7)
											{
											case 0:
												break;
											default:
												m_targetingArcPulseInstance.SetActive(false);
												Object.Destroy(m_targetingArcPulseInstance);
												m_targetingArcPulseInstance = null;
												return;
											}
										}
									}
									return;
								}
							}
							float targetingArcMaxHeight = HighlightUtils.Get().m_targetingArcMaxHeight;
							float num2 = 1f + (travelBoardSquareWorldPositionForLos.y - targetingArcEndPosition.y) / targetingArcMaxHeight;
							Vector3 a = targetingArcEndPosition - travelBoardSquareWorldPositionForLos;
							a.y = 0f;
							if (a.magnitude <= 0.5f)
							{
								a = new Vector3(0.5f * vector.x, 0f, 0.5f * vector.z);
							}
							float num3 = 0.5f * a.magnitude;
							float num4 = targetingArcMaxHeight / (num3 * num3);
							if (m_targetingArcPulseInstance != null)
							{
								m_arcTraveled += HighlightUtils.Get().m_targetingArcMovementSpeed * Time.deltaTime;
								float num5 = m_arcTraveled * a.magnitude - num3;
								float num6 = num4 * num5 * num5;
								if (num5 > 0f)
								{
									num6 *= num2;
								}
								Vector3 position = travelBoardSquareWorldPositionForLos + a * m_arcTraveled + Vector3.up * (targetingArcMaxHeight - num6);
								m_targetingArcPulseInstance.transform.position = position;
							}
							bool flag2 = false;
							if ((m_arcEnd - targetingArcEndPosition).sqrMagnitude > 0.1f)
							{
								m_arcEnd = targetingArcEndPosition;
								flag2 = true;
							}
							if (!(m_targetingArcInstance == null) && !flag)
							{
								if (!flag2)
								{
									return;
								}
							}
							List<Vector3> list = new List<Vector3>();
							for (int i = 1; i <= HighlightUtils.Get().m_targetingArcNumSegments; i++)
							{
								float num7 = (float)i / (float)HighlightUtils.Get().m_targetingArcNumSegments;
								float num8 = num7 * a.magnitude - num3;
								float num9 = num4 * num8 * num8;
								if (num8 > 0f)
								{
									num9 *= num2;
								}
								Vector3 item = travelBoardSquareWorldPositionForLos + a * num7 + Vector3.up * (targetingArcMaxHeight - num9);
								list.Add(item);
							}
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
								{
									Color color = HighlightUtils.Get().m_targetingArcColor;
									if (targetingActor != GameFlowData.Get().activeOwnedActorData)
									{
										color = HighlightUtils.Get().m_targetingArcColorAllies;
									}
									m_targetingArcInstance = Targeting.GetTargeting().CreateLineMesh(list, 0.2f, color, false, HighlightUtils.Get().m_targetingArcMaterial, m_targetingArcInstance, true);
									return;
								}
								}
							}
						}
						}
					}
				}
			}
		}
		DestroyTargetingArcMesh();
		if (m_targetingArcPulseInstance != null)
		{
			m_targetingArcPulseInstance.SetActive(false);
			Object.Destroy(m_targetingArcPulseInstance);
			m_targetingArcPulseInstance = null;
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
		if (!NetworkClient.active || !HighlightUtils.Get().m_setTargeterOpacityWhileTargeting)
		{
			return;
		}
		while (true)
		{
			float opacity = Mathf.Clamp(HighlightUtils.Get().m_targeterOpacityWhileTargeting, 0.01f, 1f);
			using (List<GameObject>.Enumerator enumerator = m_highlights.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject current = enumerator.Current;
					MeshRenderer[] components = current.GetComponents<MeshRenderer>();
					MeshRenderer[] array = components;
					foreach (MeshRenderer meshRenderer in array)
					{
						SetMaterialOpacity(meshRenderer.materials, opacity);
					}
					MeshRenderer[] componentsInChildren = current.GetComponentsInChildren<MeshRenderer>();
					MeshRenderer[] array2 = componentsInChildren;
					foreach (MeshRenderer meshRenderer2 in array2)
					{
						SetMaterialOpacity(meshRenderer2.materials, opacity);
					}
				}
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	public virtual void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		m_lastAllyTargeterChange = Time.time;
		m_confirmedTargetingStartTime = Time.time + HUD_UIResources.Get().m_confirmedTargetingDuration;
		using (List<ArrowList>.Enumerator enumerator = m_arrows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ArrowList current = enumerator.Current;
				if (current != null)
				{
					if (current.m_gameObject.activeSelf)
					{
						MovementPathStart componentInChildren = current.m_gameObject.GetComponentInChildren<MovementPathStart>();
						if (componentInChildren != null)
						{
							componentInChildren.SetGlow(true);
						}
					}
				}
			}
		}
		if (GameFlowData.Get().activeOwnedActorData == targetingActor)
		{
			if (targetingActor.GetActorTurnSM().CurrentState != TurnStateEnum.TARGETING_ACTION)
			{
				HideAllSquareIndicators();
			}
		}
		SetupTargetingArc(targetingActor, true);
		if (!Application.isEditor)
		{
			return;
		}
		while (true)
		{
			if (m_highlights == null)
			{
				return;
			}
			while (true)
			{
				if (m_ability != null)
				{
					while (true)
					{
						using (List<GameObject>.Enumerator enumerator2 = m_highlights.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								GameObject current2 = enumerator2.Current;
								if (current2 != null)
								{
									if (!current2.name.StartsWith("[Targeter]"))
									{
										current2.name = "[Targeter] " + m_ability.GetNameString() + ": " + current2.name;
									}
								}
							}
							while (true)
							{
								switch (7)
								{
								default:
									return;
								case 0:
									break;
								}
							}
						}
					}
				}
				return;
			}
		}
	}

	public virtual void UpdateConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float timeSinceChange = Time.time - m_lastAllyTargeterChange;
		float num = 0f;
		HighlightUtils.TargeterOpacityData[] targeterOpacity = (!(GameFlowData.Get().activeOwnedActorData == targetingActor)) ? HighlightUtils.Get().m_allyTargeterOpacity : HighlightUtils.Get().m_confirmedTargeterOpacity;
		num = GetOpacityFromTargeterData(targeterOpacity, timeSinceChange);
		SetTargeterHighlightOpacity(m_highlights, num);
		SetupTargetingArc(targetingActor, false);
	}

	public void UpdateFadeOutHighlights(ActorData targetingActor)
	{
		m_highlightFadeContainer.UpdateFade(targetingActor, m_highlights.Count > 0);
	}

	public static void SetTargeterHighlightOpacity(List<GameObject> highlights, float opacity)
	{
		foreach (GameObject highlight in highlights)
		{
			MeshRenderer[] components = highlight.GetComponents<MeshRenderer>();
			MeshRenderer[] array = components;
			foreach (MeshRenderer meshRenderer in array)
			{
				SetMaterialOpacity(meshRenderer.materials, opacity);
			}
			MeshRenderer[] componentsInChildren = highlight.GetComponentsInChildren<MeshRenderer>();
			MeshRenderer[] array2 = componentsInChildren;
			foreach (MeshRenderer meshRenderer2 in array2)
			{
				SetMaterialOpacity(meshRenderer2.materials, opacity);
			}
			float opacity2 = Mathf.Clamp(opacity * HighlightUtils.Get().m_targeterParticleSystemOpacityMultiplier, 0f, 1f);
			ParticleSystemRenderer[] componentsInChildren2 = highlight.GetComponentsInChildren<ParticleSystemRenderer>();
			ParticleSystemRenderer[] array3 = componentsInChildren2;
			foreach (ParticleSystemRenderer particleSystemRenderer in array3)
			{
				SetMaterialOpacity(particleSystemRenderer.materials, opacity2);
			}
		}
	}

	public static void SetTargeterHighlightColor(List<GameObject> highlights, Color color, bool keepOpacity = true, bool clearColorOverTime = true)
	{
		using (List<GameObject>.Enumerator enumerator = highlights.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject current = enumerator.Current;
				MeshRenderer[] components = current.GetComponents<MeshRenderer>();
				MeshRenderer[] array = components;
				foreach (MeshRenderer meshRenderer in array)
				{
					SetMaterialColor(meshRenderer.materials, color, keepOpacity);
				}
				MeshRenderer[] componentsInChildren = current.GetComponentsInChildren<MeshRenderer>();
				MeshRenderer[] array2 = componentsInChildren;
				foreach (MeshRenderer meshRenderer2 in array2)
				{
					SetMaterialColor(meshRenderer2.materials, color, keepOpacity);
				}
				ParticleSystemRenderer[] componentsInChildren2 = current.GetComponentsInChildren<ParticleSystemRenderer>();
				ParticleSystemRenderer[] array3 = componentsInChildren2;
				foreach (ParticleSystemRenderer particleSystemRenderer in array3)
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
			while (true)
			{
				switch (2)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public static float GetOpacityFromTargeterData(HighlightUtils.TargeterOpacityData[] targeterOpacity, float timeSinceChange)
	{
		float result = 1f;
		int num = 0;
		while (true)
		{
			if (num < targeterOpacity.Length)
			{
				if (num == targeterOpacity.Length - 1)
				{
					result = targeterOpacity[num].m_alpha;
				}
				else if (targeterOpacity[num].m_timeSinceConfirmed <= timeSinceChange)
				{
					if (timeSinceChange <= targeterOpacity[num + 1].m_timeSinceConfirmed)
					{
						float alpha = targeterOpacity[num].m_alpha;
						float alpha2 = targeterOpacity[num + 1].m_alpha;
						float num2 = (timeSinceChange - targeterOpacity[num].m_timeSinceConfirmed) / Mathf.Max(0.01f, targeterOpacity[num + 1].m_timeSinceConfirmed - targeterOpacity[num].m_timeSinceConfirmed);
						result = alpha * (1f - num2) + alpha2 * num2;
						break;
					}
				}
				num++;
				continue;
			}
			break;
		}
		return result;
	}

	internal static void SetMaterialOpacity(Material[] materials, float opacity)
	{
		foreach (Material material in materials)
		{
			int[] array = materialColorProperties;
			foreach (int nameID in array)
			{
				if (material.HasProperty(nameID))
				{
					Color color = material.GetColor(nameID);
					Color value = new Color(color.r, color.g, color.b, opacity);
					material.SetColor(nameID, value);
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					goto end_IL_007a;
				}
				continue;
				end_IL_007a:
				break;
			}
		}
	}

	internal static void SetMaterialColor(Material[] materials, Color newColor, bool keepOpacity = true)
	{
		foreach (Material material in materials)
		{
			int[] array = materialColorProperties;
			foreach (int nameID in array)
			{
				if (!material.HasProperty(nameID))
				{
					continue;
				}
				if (keepOpacity)
				{
					Color color = material.GetColor(nameID);
					newColor.a = color.a;
				}
				material.SetColor(nameID, newColor);
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
		if (HighlightUtils.GetAffectedSquaresContainer() == null)
		{
			return;
		}
		while (true)
		{
			HighlightUtils.GetAffectedSquaresContainer().HideAllSquareIndicators();
			return;
		}
	}

	public void ResetSquareIndicatorIndexToUse()
	{
		if (HighlightUtils.GetHiddenSquaresContainer() != null)
		{
			HighlightUtils.GetHiddenSquaresContainer().ResetNextIndicatorIndex();
		}
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
		if (HighlightUtils.GetAffectedSquaresContainer() == null)
		{
			return;
		}
		while (true)
		{
			HighlightUtils.GetAffectedSquaresContainer().HideAllSquareIndicators(GetNextAffectedSquareIndicatorIndex());
			return;
		}
	}

	public int GetNextHiddenSquareIndicatorIndex()
	{
		if (HighlightUtils.GetHiddenSquaresContainer() != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return HighlightUtils.GetHiddenSquaresContainer().GetNextIndicatorIndex();
				}
			}
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
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[4]
		{
			new Vector3(0f - halfWidth, 0f, 0f - halfHeight),
			new Vector3(halfWidth, 0f, 0f - halfHeight),
			new Vector3(halfWidth, 0f, halfHeight),
			new Vector3(0f - halfWidth, 0f, halfHeight)
		};
		mesh.uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};
		mesh.triangles = new int[6]
		{
			0,
			1,
			2,
			0,
			2,
			3
		};
		mesh.RecalculateNormals();
		return mesh;
	}

	public int GetNextAffectedSquareIndicatorIndex()
	{
		if (HighlightUtils.GetAffectedSquaresContainer() != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return HighlightUtils.GetAffectedSquaresContainer().GetNextIndicatorIndex();
				}
			}
		}
		return 0;
	}

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
