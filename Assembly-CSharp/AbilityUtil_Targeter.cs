using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;
using UnityEngine.Networking;

public class AbilityUtil_Targeter
{
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

	private List<AbilityUtil_Targeter.ActorTarget> m_actorsInRange = new List<AbilityUtil_Targeter.ActorTarget>();

	protected List<ActorData> m_actorsAddedSoFar = new List<ActorData>();

	protected List<AbilityUtil_Targeter.ArrowList> m_arrows = new List<AbilityUtil_Targeter.ArrowList>();

	protected HighlightUtils.CursorType m_cursorType = HighlightUtils.CursorType.NoCursorType;

	protected bool m_affectsEnemies = true;

	protected bool m_affectsAllies;

	protected bool m_affectsTargetingActor;

	private static readonly int[] materialColorProperties = new int[]
	{
		Shader.PropertyToID("_Color"),
		Shader.PropertyToID("_TintColor")
	};

	private float m_lastAllyTargeterChange;

	private float m_confirmedTargetingStartTime;

	public AbilityUtil_Targeter(Ability ability)
	{
		this.m_highlights = new List<GameObject>();
		this.m_highlightFadeContainer = new TargeterTemplateFadeContainer();
		this.m_ability = ability;
		this.m_actorContextVars = new Dictionary<ActorData, ActorHitContext>();
		this.m_nonActorSpecificContext = new ContextVars();
	}

	public GridPos LastUpdatingGridPos { get; set; }

	public Vector3 LastUpdateFreePos { get; set; }

	public Vector3 LastUpdateAimDir { get; set; }

	public bool MarkedForForceUpdate { get; set; }

	private void OnDestroy()
	{
		this.DestroyTargetingArcMesh();
	}

	public void SetLastUpdateCursorState(AbilityTarget target)
	{
		this.LastUpdatingGridPos = target.GridPos;
		this.LastUpdateFreePos = target.FreePos;
		this.LastUpdateAimDir = target.AimDirection;
	}

	public bool IsCursorStateSameAsLastUpdate(AbilityTarget target)
	{
		if (this.LastUpdatingGridPos.CoordsEqual(target.GridPos))
		{
			if (this.LastUpdateFreePos == target.FreePos)
			{
				return this.LastUpdateAimDir == target.AimDirection;
			}
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
		return this.m_useMultiTargetUpdate;
	}

	public void SetUseMultiTargetUpdate(bool value)
	{
		this.m_useMultiTargetUpdate = value;
	}

	public bool ShouldShowActorRadius()
	{
		return this.m_shouldShowActorRadius;
	}

	public void SetShowArcToShape(bool showArc)
	{
		this.ShowArcToShape = showArc;
	}

	public bool ShowArcToShape
	{
		get
		{
			return this.m_showArcToShape;
		}
		set
		{
			this.m_showArcToShape = value;
		}
	}

	public void TargeterAbilitySelected()
	{
		HighlightUtils.Get().SetCursorType(this.m_cursorType);
		this.DestroyTeleportStartObject();
		if (this.m_ability != null)
		{
			if (AbilityUtils.AbilityHasTag(this.m_ability, AbilityTags.UseTeleportUIEffect))
			{
				if (GameFlowData.Get() != null)
				{
					if (GameFlowData.Get().activeOwnedActorData != null && HighlightUtils.Get().m_teleportPrefab != null)
					{
						this.m_gameObjectOnCaster = UnityEngine.Object.Instantiate<TeleportPathStart>(HighlightUtils.Get().m_teleportPrefab);
						this.m_gameObjectOnCaster.transform.position = GameFlowData.Get().activeOwnedActorData.transform.position;
						this.m_gameObjectOnCaster.SetColor(this.GetTeleportColor());
					}
				}
			}
		}
		this.MarkedForForceUpdate = true;
	}

	public Color GetTeleportColor()
	{
		Color result = HighlightUtils.Get().s_teleportMovementLineColor;
		if (this.m_ability.IsFlashAbility())
		{
			result = HighlightUtils.Get().s_flashMovementLineColor;
		}
		return result;
	}

	public virtual void AbilityCasted(GridPos startPosition, GridPos endPosition)
	{
		if (this.m_gameObjectOnCaster != null)
		{
			this.m_gameObjectOnCaster.AbilityCasted(startPosition, endPosition);
		}
	}

	public void UpdateTargetAreaEffect(AbilityTarget currentTarget, ActorData caster)
	{
		Color teleportColor = this.GetTeleportColor();
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
		if (this.m_gameObjectOnCastTarget == null)
		{
			if (this.m_ability != null)
			{
				if (AbilityUtils.AbilityHasTag(this.m_ability, AbilityTags.UseTeleportUIEffect) && HighlightUtils.Get().m_teleportPrefab != null)
				{
					this.m_gameObjectOnCastTarget = UnityEngine.Object.Instantiate<TeleportPathEnd>(HighlightUtils.Get().m_teleportEndPrefab);
				}
			}
		}
		if (this.m_gameObjectOnCastTarget != null)
		{
			List<Vector3> list = new List<Vector3>();
			Vector3 worldPosition = Board.Get().GetBoardSquareSafe(caster.GetGridPosWithIncrementedHeight()).GetWorldPosition();
			list.Add(worldPosition);
			Vector3 worldPosition2 = Board.Get().GetBoardSquareSafe(currentTarget.GridPos).GetWorldPosition();
			list.Add(worldPosition2);
			Vector3 a = list[0] - list[1];
			Vector3 a2 = list[list.Count - 1] - list[list.Count - 2];
			a.Normalize();
			a2.Normalize();
			List<Vector3> list2;
			(list2 = list)[0] = list2[0] - a * Board.Get().squareSize * 0.6f;
			int index;
			(list2 = list)[index = list.Count - 1] = list2[index] - a2 * Board.Get().squareSize * 0.4f;
			this.DestroyTeleportLine();
			this.m_lineBetweenPath = Targeting.GetTargeting().CreateLineMesh(list, 0.2f, teleportColor, true, HighlightUtils.Get().m_ArrowDottedLineMaterial, null, false);
			this.m_gameObjectOnCastTarget.transform.position = currentTarget.GetWorldGridPos();
			this.m_gameObjectOnCastTarget.AbilityCasted(caster.GetGridPosWithIncrementedHeight(), currentTarget.GridPos);
			this.m_gameObjectOnCastTarget.SetColor(teleportColor);
			this.m_gameObjectOnCastTarget.Setup();
		}
	}

	public void UpdateEffectOnCaster(AbilityTarget currentTarget, ActorData caster)
	{
		Color teleportColor = this.GetTeleportColor();
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
		if (this.m_gameObjectOnCaster == null && this.m_ability != null)
		{
			if (AbilityUtils.AbilityHasTag(this.m_ability, AbilityTags.UseTeleportUIEffect) && HighlightUtils.Get().m_teleportPrefab != null)
			{
				this.m_gameObjectOnCaster = UnityEngine.Object.Instantiate<TeleportPathStart>(HighlightUtils.Get().m_teleportPrefab);
			}
		}
		if (this.m_gameObjectOnCaster != null)
		{
			this.m_gameObjectOnCaster.transform.position = caster.transform.position;
			this.m_gameObjectOnCaster.AbilityCasted(caster.GetGridPosWithIncrementedHeight(), currentTarget.GridPos);
			this.m_gameObjectOnCaster.SetColor(teleportColor);
		}
	}

	private void DestroyTeleportLine()
	{
		if (this.m_lineBetweenPath != null)
		{
			HighlightUtils.DestroyMeshesOnObject(this.m_lineBetweenPath);
			this.DestroyObjectAndMaterials(this.m_lineBetweenPath);
			this.m_lineBetweenPath = null;
		}
	}

	private void DestroyTeleportStartObject()
	{
		if (this.m_gameObjectOnCaster != null)
		{
			this.DestroyObjectAndMaterials(this.m_gameObjectOnCaster.gameObject);
			this.m_gameObjectOnCaster = null;
		}
	}

	private void DestroyTeleportEndObjet()
	{
		if (this.m_gameObjectOnCastTarget != null)
		{
			this.DestroyObjectAndMaterials(this.m_gameObjectOnCastTarget.gameObject);
			this.m_gameObjectOnCastTarget = null;
		}
	}

	public void InstantiateTeleportPathUIEffect()
	{
		if (this.m_gameObjectOnCaster == null)
		{
			if (HighlightUtils.Get().m_teleportPrefab != null)
			{
				this.m_gameObjectOnCaster = UnityEngine.Object.Instantiate<TeleportPathStart>(HighlightUtils.Get().m_teleportPrefab);
			}
		}
		if (this.m_gameObjectOnCastTarget == null)
		{
			if (this.m_ability != null && HighlightUtils.Get().m_teleportPrefab != null)
			{
				this.m_gameObjectOnCastTarget = UnityEngine.Object.Instantiate<TeleportPathEnd>(HighlightUtils.Get().m_teleportEndPrefab);
			}
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
				flag = abilityData.HasQueuedAction(abilityData.GetActionTypeOfAbility(this.m_ability));
			}
		}
		if (!flag)
		{
			this.ClearActorsInRange();
			this.ClearArrows();
			this.ClearHighlightCursors(true);
			this.HideAllSquareIndicators();
			this.DestroyTeleportStartObject();
			this.DestroyTeleportLine();
			this.DestroyTeleportEndObjet();
		}
		if (flag)
		{
			if (!this.m_ability.BackupTargets.IsNullOrEmpty<AbilityTarget>())
			{
				if (targetIndex < this.m_ability.BackupTargets.Count)
				{
					AbilityTarget abilityTarget = this.m_ability.BackupTargets[targetIndex];
					this.ClearHighlightCursors(true);
					this.SetLastUpdateCursorState(abilityTarget);
					if (this.IsUsingMultiTargetUpdate())
					{
						this.UpdateTargetingMultiTargets(abilityTarget, GameFlowData.Get().activeOwnedActorData, targetIndex, this.m_ability.BackupTargets);
					}
					else
					{
						this.UpdateTargeting(abilityTarget, GameFlowData.Get().activeOwnedActorData);
					}
					this.UpdateHighlightPosAfterClick(abilityTarget, GameFlowData.Get().activeOwnedActorData, targetIndex, this.m_ability.BackupTargets);
					this.SetupTargetingArc(GameFlowData.Get().activeOwnedActorData, false);
				}
			}
		}
		HighlightUtils.Get().SetCursorType(HighlightUtils.CursorType.MouseOverCursorType);
		this.MarkedForForceUpdate = true;
	}

	public virtual void ResetTargeter(bool clearHighlightInstantly)
	{
		this.ClearActorsInRange();
		this.ClearArrows();
		this.ClearHighlightCursors(clearHighlightInstantly);
		this.HideAllSquareIndicators();
		this.DestroyTeleportStartObject();
		this.DestroyTeleportLine();
		this.DestroyTeleportEndObjet();
		this.m_confirmedTargetingStartTime = 0f;
		this.m_actorContextVars.Clear();
		this.m_nonActorSpecificContext.Clear();
		this.MarkedForForceUpdate = true;
	}

	public List<AbilityTooltipSubject> GetTooltipSubjectTypes(ActorData actor)
	{
		List<AbilityTooltipSubject> result = null;
		if (actor != null)
		{
			using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = this.m_actorsInRange.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityUtil_Targeter.ActorTarget actorTarget = enumerator.Current;
					if (actorTarget.m_actor == actor)
					{
						return actorTarget.m_subjectTypes;
					}
				}
			}
		}
		return result;
	}

	public void AppendToTooltipSubjectSet(ActorData actor, HashSet<AbilityTooltipSubject> subjectsToAppendTo)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = this.GetTooltipSubjectTypes(actor);
		if (tooltipSubjectTypes != null)
		{
			using (List<AbilityTooltipSubject>.Enumerator enumerator = tooltipSubjectTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityTooltipSubject item = enumerator.Current;
					subjectsToAppendTo.Add(item);
				}
			}
		}
	}

	public virtual bool IsActorInTargetRange(ActorData actor)
	{
		bool flag;
		return this.IsActorInTargetRange(actor, out flag);
	}

	public unsafe virtual bool IsActorInTargetRange(ActorData actor, out bool inCover)
	{
		bool result = false;
		inCover = false;
		if (actor != null)
		{
			for (int i = 0; i < this.m_actorsInRange.Count; i++)
			{
				AbilityUtil_Targeter.ActorTarget actorTarget = this.m_actorsInRange[i];
				if (actorTarget.m_actor == actor)
				{
					result = true;
					ActorCover actorCover = actorTarget.m_actor.GetActorCover();
					if (actorCover != null)
					{
						ActorData povactorData = GameFlowData.Get().POVActorData;
						bool flag;
						if (povactorData != null)
						{
							flag = (povactorData.GetTeam() != actor.GetTeam());
						}
						else
						{
							flag = false;
						}
						bool flag2 = flag;
						bool flag3 = AbilityUtils.AbilityIgnoreCover(this.m_ability, actorTarget.m_actor);
						bool flag4 = actor.IsVisibleToClient();
						bool flag5 = (!actorTarget.m_ignoreCoverMinDist) ? actorCover.IsInCoverWrt(actorTarget.m_damageOrigin) : actorCover.IsInCoverWrtDirectionOnly(actorTarget.m_damageOrigin, actorTarget.m_actor.GetCurrentBoardSquare());
						if (flag2)
						{
							if (!flag3 && flag4)
							{
								if (flag5)
								{
									inCover = true;
									goto IL_140;
								}
							}
						}
						inCover = false;
					}
					IL_140:
					return result;
				}
			}
		}
		return result;
	}

	public List<AbilityUtil_Targeter.ActorTarget> GetActorsInRange()
	{
		return this.m_actorsInRange;
	}

	public virtual int GetNumActorsInRange()
	{
		return this.m_actorsInRange.Count;
	}

	protected virtual List<ActorData> GetVisibleActorsInRange()
	{
		List<ActorData> list = new List<ActorData>();
		using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = this.m_actorsInRange.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityUtil_Targeter.ActorTarget actorTarget = enumerator.Current;
				if (actorTarget.m_actor.IsVisibleToClient())
				{
					list.Add(actorTarget.m_actor);
				}
			}
		}
		return list;
	}

	public virtual List<ActorData> GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject subject)
	{
		List<ActorData> list = new List<ActorData>();
		for (int i = 0; i < this.m_actorsInRange.Count; i++)
		{
			AbilityUtil_Targeter.ActorTarget actorTarget = this.m_actorsInRange[i];
			if (actorTarget.m_actor.IsVisibleToClient() && actorTarget.m_subjectTypes.Contains(subject))
			{
				list.Add(actorTarget.m_actor);
			}
		}
		return list;
	}

	public int GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject subject)
	{
		int num = 0;
		for (int i = 0; i < this.m_actorsInRange.Count; i++)
		{
			AbilityUtil_Targeter.ActorTarget actorTarget = this.m_actorsInRange[i];
			if (actorTarget.m_actor.IsVisibleToClient())
			{
				if (actorTarget.m_subjectTypes.Contains(subject))
				{
					num++;
				}
			}
		}
		return num;
	}

	public int GetTooltipSubjectCountOnActor(ActorData actor, AbilityTooltipSubject subject)
	{
		int num = 0;
		for (int i = 0; i < this.m_actorsInRange.Count; i++)
		{
			AbilityUtil_Targeter.ActorTarget actorTarget = this.m_actorsInRange[i];
			if (actorTarget.m_actor == actor)
			{
				for (int j = 0; j < actorTarget.m_subjectTypes.Count; j++)
				{
					if (actorTarget.m_subjectTypes[j] == subject)
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
		for (int i = 0; i < this.m_actorsInRange.Count; i++)
		{
			AbilityUtil_Targeter.ActorTarget actorTarget = this.m_actorsInRange[i];
			for (int j = 0; j < actorTarget.m_subjectTypes.Count; j++)
			{
				if (actorTarget.m_subjectTypes[j] == subject)
				{
					num++;
				}
			}
		}
		return num;
	}

	protected void AddActorsInRange(List<ActorData> actors, Vector3 damageOrigin, ActorData targetingActor, AbilityTooltipSubject subjectType = AbilityTooltipSubject.Primary, bool appendSubjectType = false)
	{
		for (int i = 0; i < actors.Count; i++)
		{
			this.AddActorInRange(actors[i], damageOrigin, targetingActor, subjectType, appendSubjectType);
		}
	}

	protected void AddActorInRange(ActorData actor, Vector3 damageOrigin, ActorData targetingActor, AbilityTooltipSubject subjectType = AbilityTooltipSubject.Primary, bool appendSubjectType = false)
	{
		if (!this.IsActorInTargetRange(actor))
		{
			AbilityUtil_Targeter.ActorTarget actorTarget = new AbilityUtil_Targeter.ActorTarget();
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
			for (int i = 8; i <= 0xF; i++)
			{
				AbilityTooltipSubject abilityTooltipSubject = (AbilityTooltipSubject)i;
				if (this.DoesTargetActorMatchTooltipSubject(abilityTooltipSubject, actor, damageOrigin, targetingActor))
				{
					actorTarget.m_subjectTypes.Add(abilityTooltipSubject);
				}
			}
			this.m_actorsInRange.Add(actorTarget);
			this.m_actorsAddedSoFar.Add(actor);
			if (!this.m_actorContextVars.ContainsKey(actor))
			{
				ActorHitContext actorHitContext = new ActorHitContext();
				actorHitContext.symbol_0012 = true;
				this.m_actorContextVars[actor] = actorHitContext;
			}
			else
			{
				this.m_actorContextVars[actor].symbol_0012 = true;
			}
		}
		else if (appendSubjectType)
		{
			foreach (AbilityUtil_Targeter.ActorTarget actorTarget2 in this.m_actorsInRange)
			{
				if (actorTarget2.m_actor == actor)
				{
					actorTarget2.m_subjectTypes.Add(subjectType);
					break;
				}
			}
		}
	}

	public Dictionary<ActorData, ActorHitContext> GetActorContextVars()
	{
		return this.m_actorContextVars;
	}

	public ContextVars GetNonActorSpecificContext()
	{
		return this.m_nonActorSpecificContext;
	}

	protected void SetIgnoreCoverMinDist(ActorData actor, bool ignoreCoverMinDist)
	{
		for (int i = 0; i < this.m_actorsInRange.Count; i++)
		{
			if (this.m_actorsInRange[i].m_actor == actor)
			{
				this.m_actorsInRange[i].m_ignoreCoverMinDist = ignoreCoverMinDist;
				break;
			}
		}
	}

	protected virtual bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		return this.m_ability.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
	}

	public void ClearActorsInRange()
	{
		this.m_actorsInRange.Clear();
		this.m_actorsAddedSoFar.Clear();
		foreach (KeyValuePair<ActorData, ActorHitContext> keyValuePair in this.m_actorContextVars)
		{
			keyValuePair.Value.symbol_0012 = false;
		}
	}

	protected void AddMovementArrow(ActorData mover, BoardSquarePathInfo path, AbilityUtil_Targeter.TargeterMovementType movementType, MovementPathStart previousLine = null, bool isChasing = false)
	{
		Color arrowColor;
		switch (movementType)
		{
		case AbilityUtil_Targeter.TargeterMovementType.Knockback:
			arrowColor = HighlightUtils.Get().s_knockbackMovementLineColor;
			break;
		case AbilityUtil_Targeter.TargeterMovementType.Movement:
			arrowColor = HighlightUtils.Get().s_dashMovementLineColor;
			break;
		case AbilityUtil_Targeter.TargeterMovementType.Attacking:
			arrowColor = HighlightUtils.Get().s_attackMovementLineColor;
			break;
		default:
			arrowColor = HighlightUtils.Get().s_knockbackMovementLineColor;
			break;
		}
		this.AddMovementArrow(mover, path, arrowColor, previousLine, isChasing, movementType);
	}

	public void UpdateArrowsForUI()
	{
		bool flag = false;
		for (int i = 0; i < this.m_arrows.Count; i++)
		{
			if (this.m_arrows[i].m_gameObject.activeSelf)
			{
				flag = true;
				List<Vector3> list = KnockbackUtils.BuildDrawablePath(this.m_arrows[i].m_pathInfo, false);
				MovementPathStart componentInChildren = this.m_arrows[i].m_gameObject.GetComponentInChildren<MovementPathStart>();
				Vector3 vector2D = list[list.Count - 1];
				BoardSquare boardSquare = Board.Get().GetBoardSquare(vector2D);
				componentInChildren.SetCharacterMovementPanel(boardSquare);
			}
		}
		if (!flag)
		{
			if (this.m_arrows.Count > 0)
			{
				this.m_arrows[0].m_gameObject.GetComponentInChildren<MovementPathStart>().HideCharacterMovementPanel();
			}
		}
	}

	protected void AddMovementArrow(ActorData mover, BoardSquarePathInfo path, Color arrowColor, MovementPathStart previousLine, bool isChasing, AbilityUtil_Targeter.TargeterMovementType movementType)
	{
		if (path != null)
		{
			if (mover != null && mover.IsVisibleToClient())
			{
				List<Vector3> list = KnockbackUtils.BuildDrawablePath(path, false);
				if (list.Count >= 2)
				{
					GameObject gameObject = Targeting.GetTargeting().CreateFancyArrowMesh(ref list, 0.2f, arrowColor, isChasing, mover, movementType, null, previousLine, false, 0.4f, 0.4f);
					bool flag = false;
					int i = 0;
					while (i < this.m_arrows.Count)
					{
						if (this.m_arrows[i].m_gameObject == gameObject)
						{
							flag = true;
							this.m_arrows[i].m_pathInfo = path;
							IL_D7:
							if (flag)
							{
								return;
							}
							if (gameObject.GetComponentInChildren<MovementPathStart>() != null)
							{
								AbilityUtil_Targeter.ArrowList arrowList = new AbilityUtil_Targeter.ArrowList();
								arrowList.m_gameObject = gameObject;
								arrowList.m_pathInfo = path;
								this.m_arrows.Add(arrowList);
								return;
							}
							return;
						}
						else
						{
							i++;
						}
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						goto IL_D7;
					}
				}
			}
		}
	}

	protected int AddMovementArrowWithPrevious(ActorData mover, BoardSquarePathInfo path, AbilityUtil_Targeter.TargeterMovementType movementType, int arrowIndex, bool isChasing = false)
	{
		Color arrowColor;
		switch (movementType)
		{
		case AbilityUtil_Targeter.TargeterMovementType.Knockback:
			arrowColor = HighlightUtils.Get().s_knockbackMovementLineColor;
			break;
		case AbilityUtil_Targeter.TargeterMovementType.Movement:
			arrowColor = HighlightUtils.Get().s_dashMovementLineColor;
			break;
		case AbilityUtil_Targeter.TargeterMovementType.Attacking:
			arrowColor = HighlightUtils.Get().s_attackMovementLineColor;
			break;
		default:
			arrowColor = HighlightUtils.Get().s_knockbackMovementLineColor;
			break;
		}
		return this.AddMovementArrowWithPrevious(mover, path, movementType, arrowColor, arrowIndex, isChasing);
	}

	protected int AddMovementArrowWithPrevious(ActorData mover, BoardSquarePathInfo path, AbilityUtil_Targeter.TargeterMovementType movementType, Color arrowColor, int arrowIndex, bool isChasing = false)
	{
		if (this.CanCreateMovementArrows(path))
		{
			MovementPathStart previousLine = null;
			if (this.m_arrows.Count > arrowIndex)
			{
				previousLine = this.m_arrows[arrowIndex].m_gameObject.GetComponentInChildren<MovementPathStart>();
			}
			this.AddMovementArrow(mover, path, arrowColor, previousLine, isChasing, movementType);
			return arrowIndex + 1;
		}
		return arrowIndex;
	}

	protected void EnableAllMovementArrows()
	{
		this.SetMovementArrowEnabledFromIndex(0, true);
	}

	protected void SetMovementArrowEnabledFromIndex(int fromIndex, bool enabled)
	{
		for (int i = fromIndex; i < this.m_arrows.Count; i++)
		{
			this.m_arrows[i].m_gameObject.SetActive(enabled);
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
		using (List<AbilityUtil_Targeter.ArrowList>.Enumerator enumerator = this.m_arrows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityUtil_Targeter.ArrowList arrowList = enumerator.Current;
				if (arrowList != null)
				{
					if (arrowList.m_gameObject != null)
					{
						MovementPathStart componentInChildren = arrowList.m_gameObject.GetComponentInChildren<MovementPathStart>();
						if (componentInChildren != null)
						{
							componentInChildren.HideCharacterMovementPanel();
						}
						this.DestroyObjectAndMaterials(arrowList.m_gameObject);
					}
				}
			}
		}
		this.m_arrows.Clear();
	}

	public static GameObject CreateArrowFromGridPosPath(List<GridPos> gridPosPath, Color lineColor, bool isChasing, ActorData theActor)
	{
		GameObject result = null;
		if (gridPosPath.Count >= 2)
		{
			GridPos start = gridPosPath[0];
			GridPos end = gridPosPath[gridPosPath.Count - 1];
			result = AbilityUtil_Targeter.CreateArrowFromGridPosPoints(start, end, lineColor, isChasing, theActor);
		}
		return result;
	}

	public static GameObject CreateArrowFromGridPosPoints(GridPos start, GridPos end, Color lineColor, bool isChasing, ActorData theActor)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 worldPosition = Board.Get().GetBoardSquareSafe(start).GetWorldPosition();
		worldPosition.y += 0.5f;
		list.Add(worldPosition);
		Vector3 worldPosition2 = Board.Get().GetBoardSquareSafe(end).GetWorldPosition();
		worldPosition2.y += 0.5f;
		list.Add(worldPosition2);
		return Targeting.GetTargeting().CreateFancyArrowMesh(ref list, 0.2f, lineColor, isChasing, theActor, AbilityUtil_Targeter.TargeterMovementType.Movement, null, null, false, 0.4f, 0.4f);
	}

	public List<GameObject> GetHighlightCopies(bool setOpacity)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.m_highlights.Count; i++)
		{
			if (this.m_highlights[i] != null)
			{
				GameObject gameObject = HighlightUtils.Get().CloneTargeterHighlight(this.m_highlights[i], this);
				if (!this.m_highlights[i].activeSelf)
				{
					gameObject.SetActive(false);
				}
				list.Add(gameObject);
			}
		}
		if (setOpacity)
		{
			HighlightUtils.TargeterOpacityData[] allyTargeterOpacity = HighlightUtils.Get().m_allyTargeterOpacity;
			float opacityFromTargeterData = AbilityUtil_Targeter.GetOpacityFromTargeterData(allyTargeterOpacity, 100f);
			AbilityUtil_Targeter.SetTargeterHighlightOpacity(list, opacityFromTargeterData);
		}
		return list;
	}

	protected virtual void ClearHighlightCursors(bool clearInstantly = true)
	{
		if (!clearInstantly)
		{
			if (NetworkClient.active)
			{
				if (this.m_highlights.Count > 0)
				{
					this.m_highlightFadeContainer.TrackHighlights(this.m_highlights);
					goto IL_A3;
				}
				goto IL_A3;
			}
		}
		using (List<GameObject>.Enumerator enumerator = this.m_highlights.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject gameObject = enumerator.Current;
				if (gameObject != null)
				{
					this.DestroyObjectAndMaterials(gameObject);
				}
			}
		}
		IL_A3:
		this.m_highlights.Clear();
		this.DestroyTargetingArcMesh();
		if (this.m_targetingArcPulseInstance != null)
		{
			UnityEngine.Object.Destroy(this.m_targetingArcPulseInstance);
			this.m_targetingArcPulseInstance = null;
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
		if (this.m_highlights != null)
		{
			foreach (GameObject gameObject in this.m_highlights)
			{
				if (gameObject != null)
				{
					gameObject.SetActive(false);
				}
			}
		}
	}

	protected GameObject Highlight
	{
		get
		{
			if (this.m_highlights != null)
			{
				if (this.m_highlights.Count > 0)
				{
					return this.m_highlights[0];
				}
			}
			return null;
		}
		set
		{
			if (this.m_highlights == null)
			{
				this.m_highlights = new List<GameObject>();
			}
			if (this.m_highlights.Count == 0)
			{
				this.m_highlights.Add(null);
			}
			if (this.m_highlights[0] != null)
			{
				this.DestroyObjectAndMaterials(this.m_highlights[0]);
				this.m_highlights[0] = null;
			}
			this.m_highlights[0] = value;
		}
	}

	public List<TargeterTemplateSwapData> GetTemplateSwapData()
	{
		if (this.m_ability != null)
		{
			return this.m_ability.m_targeterTemplateSwaps;
		}
		return null;
	}

	public HighlightUtils.CursorType GetCursorType()
	{
		return this.m_cursorType;
	}

	public void SetAffectedGroups(bool affectsEnemies, bool affectsAllies, bool affectsCaster)
	{
		this.m_affectsEnemies = affectsEnemies;
		this.m_affectsAllies = affectsAllies;
		this.m_affectsTargetingActor = affectsCaster;
	}

	protected bool GetAffectsTarget(ActorData potentialTarget, ActorData targeterOwner)
	{
		bool result;
		if (!(potentialTarget == null))
		{
			if (targeterOwner == null)
			{
			}
			else
			{
				if (targeterOwner.GetTeam() == potentialTarget.GetTeam())
				{
					if (targeterOwner == potentialTarget)
					{
						result = this.m_affectsTargetingActor;
					}
					else
					{
						result = this.m_affectsAllies;
					}
					return result;
				}
				return this.m_affectsEnemies;
			}
		}
		result = false;
		return result;
	}

	protected List<Team> GetAffectedTeams()
	{
		return this.GetAffectedTeams(GameFlowData.Get().activeOwnedActorData);
	}

	public List<Team> GetAffectedTeams(ActorData targeterOwner)
	{
		List<Team> list = new List<Team>();
		if (targeterOwner != null)
		{
			if (this.m_affectsAllies)
			{
				list.Add(targeterOwner.GetTeam());
			}
			if (this.m_affectsEnemies)
			{
				list.Add(targeterOwner.GetOpposingTeam());
			}
		}
		return list;
	}

	protected virtual float GetCurrentRangeInSquares()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		return AbilityUtils.GetCurrentRangeInSquares(this.m_ability, activeOwnedActorData, 0);
	}

	protected virtual Vector3 GetTargetingArcEndPosition(ActorData targetingActor)
	{
		if (this.Highlight != null)
		{
			return this.Highlight.transform.position;
		}
		if (targetingActor != null)
		{
			if (targetingActor.GetCurrentBoardSquare() != null)
			{
				return targetingActor.GetCurrentBoardSquare().ToVector3();
			}
		}
		return Vector3.zero;
	}

	public virtual void SetupTargetingArc(ActorData targetingActor, bool activatePulse)
	{
		if (this.m_showArcToShape)
		{
			if (this.Highlight == null)
			{
				this.m_showArcToShape = false;
			}
		}
		if (this.m_showArcToShape)
		{
			if (this.Highlight != null)
			{
				if ((this.GetTargetingArcEndPosition(targetingActor) - targetingActor.GetCurrentBoardSquare().ToVector3()).magnitude > HighlightUtils.Get().m_minDistForTargetingArc)
				{
					Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
					Vector3 vector = Camera.main.transform.rotation * Vector3.forward;
					bool flag;
					if ((vector - this.m_cameraForward).sqrMagnitude <= 0.01f)
					{
						flag = ((Camera.main.transform.position - this.m_cameraPosition).sqrMagnitude > 0.01f);
					}
					else
					{
						flag = true;
					}
					bool flag2 = flag;
					if (flag2)
					{
						this.m_cameraForward = vector;
						this.m_cameraPosition = Camera.main.transform.position;
					}
					if (activatePulse)
					{
						if (this.m_targetingArcPulseInstance != null)
						{
							this.m_targetingArcPulseInstance.SetActive(false);
							UnityEngine.Object.Destroy(this.m_targetingArcPulseInstance);
							this.m_targetingArcPulseInstance = null;
						}
						this.m_targetingArcPulseInstance = UnityEngine.Object.Instantiate<GameObject>(HighlightUtils.Get().m_targetingArcForShape);
						this.m_targetingArcPulseInstance.SetActive(true);
						this.m_targetingArcPulseInstance.transform.position = travelBoardSquareWorldPositionForLos;
						this.m_arcTraveled = 0f;
					}
					Vector3 targetingArcEndPosition = this.GetTargetingArcEndPosition(targetingActor);
					if (this.m_arcTraveled >= 1f)
					{
						if (flag2)
						{
						}
						else
						{
							this.m_arcTraveled = 0f;
							if (this.m_targetingArcPulseInstance != null)
							{
								this.m_targetingArcPulseInstance.SetActive(false);
								UnityEngine.Object.Destroy(this.m_targetingArcPulseInstance);
								this.m_targetingArcPulseInstance = null;
								goto IL_518;
							}
							goto IL_518;
						}
					}
					float targetingArcMaxHeight = HighlightUtils.Get().m_targetingArcMaxHeight;
					float num = 1f + (travelBoardSquareWorldPositionForLos.y - targetingArcEndPosition.y) / targetingArcMaxHeight;
					Vector3 a = targetingArcEndPosition - travelBoardSquareWorldPositionForLos;
					a.y = 0f;
					if (a.magnitude <= 0.5f)
					{
						a = new Vector3(0.5f * vector.x, 0f, 0.5f * vector.z);
					}
					float num2 = 0.5f * a.magnitude;
					float num3 = targetingArcMaxHeight / (num2 * num2);
					if (this.m_targetingArcPulseInstance != null)
					{
						this.m_arcTraveled += HighlightUtils.Get().m_targetingArcMovementSpeed * Time.deltaTime;
						float num4 = this.m_arcTraveled * a.magnitude - num2;
						float num5 = num3 * num4 * num4;
						if (num4 > 0f)
						{
							num5 *= num;
						}
						Vector3 position = travelBoardSquareWorldPositionForLos + a * this.m_arcTraveled + Vector3.up * (targetingArcMaxHeight - num5);
						this.m_targetingArcPulseInstance.transform.position = position;
					}
					bool flag3 = false;
					if ((this.m_arcEnd - targetingArcEndPosition).sqrMagnitude > 0.1f)
					{
						this.m_arcEnd = targetingArcEndPosition;
						flag3 = true;
					}
					if (!(this.m_targetingArcInstance == null) && !flag2)
					{
						if (!flag3)
						{
							goto IL_4D3;
						}
					}
					List<Vector3> list = new List<Vector3>();
					for (int i = 1; i <= HighlightUtils.Get().m_targetingArcNumSegments; i++)
					{
						float num6 = (float)i / (float)HighlightUtils.Get().m_targetingArcNumSegments;
						float num7 = num6 * a.magnitude - num2;
						float num8 = num3 * num7 * num7;
						if (num7 > 0f)
						{
							num8 *= num;
						}
						Vector3 item = travelBoardSquareWorldPositionForLos + a * num6 + Vector3.up * (targetingArcMaxHeight - num8);
						list.Add(item);
					}
					Color color = HighlightUtils.Get().m_targetingArcColor;
					if (targetingActor != GameFlowData.Get().activeOwnedActorData)
					{
						color = HighlightUtils.Get().m_targetingArcColorAllies;
					}
					this.m_targetingArcInstance = Targeting.GetTargeting().CreateLineMesh(list, 0.2f, color, false, HighlightUtils.Get().m_targetingArcMaterial, this.m_targetingArcInstance, true);
					IL_4D3:
					IL_518:
					return;
				}
			}
		}
		this.DestroyTargetingArcMesh();
		if (this.m_targetingArcPulseInstance != null)
		{
			this.m_targetingArcPulseInstance.SetActive(false);
			UnityEngine.Object.Destroy(this.m_targetingArcPulseInstance);
			this.m_targetingArcPulseInstance = null;
		}
	}

	private void DestroyTargetingArcMesh()
	{
		if (this.m_targetingArcInstance != null)
		{
			this.m_targetingArcInstance.SetActive(false);
			HighlightUtils.DestroyMeshesOnObject(this.m_targetingArcInstance);
			this.DestroyObjectAndMaterials(this.m_targetingArcInstance);
			this.m_targetingArcInstance = null;
		}
	}

	public virtual void AdjustOpacityWhileTargeting()
	{
		if (NetworkClient.active && HighlightUtils.Get().m_setTargeterOpacityWhileTargeting)
		{
			float opacity = Mathf.Clamp(HighlightUtils.Get().m_targeterOpacityWhileTargeting, 0.01f, 1f);
			using (List<GameObject>.Enumerator enumerator = this.m_highlights.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameObject gameObject = enumerator.Current;
					MeshRenderer[] components = gameObject.GetComponents<MeshRenderer>();
					foreach (MeshRenderer meshRenderer in components)
					{
						AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer.materials, opacity);
					}
					MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
					foreach (MeshRenderer meshRenderer2 in componentsInChildren)
					{
						AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer2.materials, opacity);
					}
				}
			}
		}
	}

	public virtual void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.m_lastAllyTargeterChange = Time.time;
		this.m_confirmedTargetingStartTime = Time.time + HUD_UIResources.Get().m_confirmedTargetingDuration;
		using (List<AbilityUtil_Targeter.ArrowList>.Enumerator enumerator = this.m_arrows.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityUtil_Targeter.ArrowList arrowList = enumerator.Current;
				if (arrowList != null)
				{
					if (arrowList.m_gameObject.activeSelf)
					{
						MovementPathStart componentInChildren = arrowList.m_gameObject.GetComponentInChildren<MovementPathStart>();
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
				this.HideAllSquareIndicators();
			}
		}
		this.SetupTargetingArc(targetingActor, true);
		if (Application.isEditor)
		{
			if (this.m_highlights != null)
			{
				if (this.m_ability != null)
				{
					using (List<GameObject>.Enumerator enumerator2 = this.m_highlights.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							GameObject gameObject = enumerator2.Current;
							if (gameObject != null)
							{
								if (!gameObject.name.StartsWith("[Targeter]"))
								{
									gameObject.name = "[Targeter] " + this.m_ability.GetNameString() + ": " + gameObject.name;
								}
							}
						}
					}
				}
			}
		}
	}

	public virtual void UpdateConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float timeSinceChange = Time.time - this.m_lastAllyTargeterChange;
		HighlightUtils.TargeterOpacityData[] targeterOpacity;
		if (GameFlowData.Get().activeOwnedActorData == targetingActor)
		{
			targeterOpacity = HighlightUtils.Get().m_confirmedTargeterOpacity;
		}
		else
		{
			targeterOpacity = HighlightUtils.Get().m_allyTargeterOpacity;
		}
		float opacityFromTargeterData = AbilityUtil_Targeter.GetOpacityFromTargeterData(targeterOpacity, timeSinceChange);
		AbilityUtil_Targeter.SetTargeterHighlightOpacity(this.m_highlights, opacityFromTargeterData);
		this.SetupTargetingArc(targetingActor, false);
	}

	public void UpdateFadeOutHighlights(ActorData targetingActor)
	{
		this.m_highlightFadeContainer.UpdateFade(targetingActor, this.m_highlights.Count > 0);
	}

	public static void SetTargeterHighlightOpacity(List<GameObject> highlights, float opacity)
	{
		foreach (GameObject gameObject in highlights)
		{
			MeshRenderer[] components = gameObject.GetComponents<MeshRenderer>();
			foreach (MeshRenderer meshRenderer in components)
			{
				AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer.materials, opacity);
			}
			MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer meshRenderer2 in componentsInChildren)
			{
				AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer2.materials, opacity);
			}
			float opacity2 = Mathf.Clamp(opacity * HighlightUtils.Get().m_targeterParticleSystemOpacityMultiplier, 0f, 1f);
			ParticleSystemRenderer[] componentsInChildren2 = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();
			foreach (ParticleSystemRenderer particleSystemRenderer in componentsInChildren2)
			{
				AbilityUtil_Targeter.SetMaterialOpacity(particleSystemRenderer.materials, opacity2);
			}
		}
	}

	public static void SetTargeterHighlightColor(List<GameObject> highlights, Color color, bool keepOpacity = true, bool clearColorOverTime = true)
	{
		using (List<GameObject>.Enumerator enumerator = highlights.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject gameObject = enumerator.Current;
				MeshRenderer[] components = gameObject.GetComponents<MeshRenderer>();
				foreach (MeshRenderer meshRenderer in components)
				{
					AbilityUtil_Targeter.SetMaterialColor(meshRenderer.materials, color, keepOpacity);
				}
				MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
				foreach (MeshRenderer meshRenderer2 in componentsInChildren)
				{
					AbilityUtil_Targeter.SetMaterialColor(meshRenderer2.materials, color, keepOpacity);
				}
				ParticleSystemRenderer[] componentsInChildren2 = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();
				foreach (ParticleSystemRenderer particleSystemRenderer in componentsInChildren2)
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
					AbilityUtil_Targeter.SetMaterialColor(particleSystemRenderer.materials, color, keepOpacity);
				}
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
			else if (targeterOpacity[i].m_timeSinceConfirmed <= timeSinceChange)
			{
				if (timeSinceChange <= targeterOpacity[i + 1].m_timeSinceConfirmed)
				{
					float alpha = targeterOpacity[i].m_alpha;
					float alpha2 = targeterOpacity[i + 1].m_alpha;
					float num = (timeSinceChange - targeterOpacity[i].m_timeSinceConfirmed) / Mathf.Max(0.01f, targeterOpacity[i + 1].m_timeSinceConfirmed - targeterOpacity[i].m_timeSinceConfirmed);
					result = alpha * (1f - num) + alpha2 * num;
					return result;
				}
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return result;
		}
	}

	internal static void SetMaterialOpacity(Material[] materials, float opacity)
	{
		foreach (Material material in materials)
		{
			foreach (int nameID in AbilityUtil_Targeter.materialColorProperties)
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
			foreach (int nameID in AbilityUtil_Targeter.materialColorProperties)
			{
				if (material.HasProperty(nameID))
				{
					if (keepOpacity)
					{
						newColor.a = material.GetColor(nameID).a;
					}
					material.SetColor(nameID, newColor);
				}
			}
		}
	}

	public float GetConfirmedTargetingRemainingTime()
	{
		return Mathf.Max(this.m_confirmedTargetingStartTime - Time.time, 0f);
	}

	public void HideAllSquareIndicators()
	{
		if (HighlightUtils.GetHiddenSquaresContainer() != null)
		{
			HighlightUtils.GetHiddenSquaresContainer().HideAllSquareIndicators(0);
		}
		if (HighlightUtils.GetAffectedSquaresContainer() != null)
		{
			HighlightUtils.GetAffectedSquaresContainer().HideAllSquareIndicators(0);
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
			HighlightUtils.GetHiddenSquaresContainer().HideAllSquareIndicators(this.GetNextHiddenSquareIndicatorIndex());
		}
		if (HighlightUtils.GetAffectedSquaresContainer() != null)
		{
			HighlightUtils.GetAffectedSquaresContainer().HideAllSquareIndicators(this.GetNextAffectedSquareIndicatorIndex());
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
		Mesh mesh = new Mesh();
		mesh.vertices = new Vector3[]
		{
			new Vector3(-halfWidth, 0f, -halfHeight),
			new Vector3(halfWidth, 0f, -halfHeight),
			new Vector3(halfWidth, 0f, halfHeight),
			new Vector3(-halfWidth, 0f, halfHeight)
		};
		mesh.uv = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f)
		};
		mesh.triangles = new int[]
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
			return HighlightUtils.GetAffectedSquaresContainer().GetNextIndicatorIndex();
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
}
