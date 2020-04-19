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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.IsCursorStateSameAsLastUpdate(AbilityTarget)).MethodHandle;
			}
			if (this.LastUpdateFreePos == target.FreePos)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.TargeterAbilitySelected()).MethodHandle;
			}
			if (AbilityUtils.AbilityHasTag(this.m_ability, AbilityTags.UseTeleportUIEffect))
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
				if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetTeleportColor()).MethodHandle;
			}
			result = HighlightUtils.Get().s_flashMovementLineColor;
		}
		return result;
	}

	public virtual void AbilityCasted(GridPos startPosition, GridPos endPosition)
	{
		if (this.m_gameObjectOnCaster != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.AbilityCasted(GridPos, GridPos)).MethodHandle;
			}
			this.m_gameObjectOnCaster.AbilityCasted(startPosition, endPosition);
		}
	}

	public void UpdateTargetAreaEffect(AbilityTarget currentTarget, ActorData caster)
	{
		Color teleportColor = this.GetTeleportColor();
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.UpdateTargetAreaEffect(AbilityTarget, ActorData)).MethodHandle;
			}
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_ability != null)
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
				if (AbilityUtils.AbilityHasTag(this.m_ability, AbilityTags.UseTeleportUIEffect) && HighlightUtils.Get().m_teleportPrefab != null)
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
					this.m_gameObjectOnCastTarget = UnityEngine.Object.Instantiate<TeleportPathEnd>(HighlightUtils.Get().m_teleportEndPrefab);
				}
			}
		}
		if (this.m_gameObjectOnCastTarget != null)
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
			List<Vector3> list = new List<Vector3>();
			Vector3 item = Board.\u000E().\u000E(caster.\u000E()).\u001D();
			list.Add(item);
			Vector3 item2 = Board.\u000E().\u000E(currentTarget.GridPos).\u001D();
			list.Add(item2);
			Vector3 a = list[0] - list[1];
			Vector3 a2 = list[list.Count - 1] - list[list.Count - 2];
			a.Normalize();
			a2.Normalize();
			List<Vector3> list2;
			(list2 = list)[0] = list2[0] - a * Board.\u000E().squareSize * 0.6f;
			int index;
			(list2 = list)[index = list.Count - 1] = list2[index] - a2 * Board.\u000E().squareSize * 0.4f;
			this.DestroyTeleportLine();
			this.m_lineBetweenPath = Targeting.GetTargeting().CreateLineMesh(list, 0.2f, teleportColor, true, HighlightUtils.Get().m_ArrowDottedLineMaterial, null, false);
			this.m_gameObjectOnCastTarget.transform.position = currentTarget.GetWorldGridPos();
			this.m_gameObjectOnCastTarget.AbilityCasted(caster.\u000E(), currentTarget.GridPos);
			this.m_gameObjectOnCastTarget.SetColor(teleportColor);
			this.m_gameObjectOnCastTarget.Setup();
		}
	}

	public void UpdateEffectOnCaster(AbilityTarget currentTarget, ActorData caster)
	{
		Color teleportColor = this.GetTeleportColor();
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.UpdateEffectOnCaster(AbilityTarget, ActorData)).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData != caster)
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
				float num = 0.5f;
				teleportColor.a *= num;
				teleportColor.r *= num;
				teleportColor.g *= num;
				teleportColor.b *= num;
			}
		}
		if (this.m_gameObjectOnCaster == null && this.m_ability != null)
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
			if (AbilityUtils.AbilityHasTag(this.m_ability, AbilityTags.UseTeleportUIEffect) && HighlightUtils.Get().m_teleportPrefab != null)
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
				this.m_gameObjectOnCaster = UnityEngine.Object.Instantiate<TeleportPathStart>(HighlightUtils.Get().m_teleportPrefab);
			}
		}
		if (this.m_gameObjectOnCaster != null)
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
			this.m_gameObjectOnCaster.transform.position = caster.transform.position;
			this.m_gameObjectOnCaster.AbilityCasted(caster.\u000E(), currentTarget.GridPos);
			this.m_gameObjectOnCaster.SetColor(teleportColor);
		}
	}

	private void DestroyTeleportLine()
	{
		if (this.m_lineBetweenPath != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.DestroyTeleportLine()).MethodHandle;
			}
			HighlightUtils.DestroyMeshesOnObject(this.m_lineBetweenPath);
			this.DestroyObjectAndMaterials(this.m_lineBetweenPath);
			this.m_lineBetweenPath = null;
		}
	}

	private void DestroyTeleportStartObject()
	{
		if (this.m_gameObjectOnCaster != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.DestroyTeleportStartObject()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.InstantiateTeleportPathUIEffect()).MethodHandle;
			}
			if (HighlightUtils.Get().m_teleportPrefab != null)
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
				this.m_gameObjectOnCaster = UnityEngine.Object.Instantiate<TeleportPathStart>(HighlightUtils.Get().m_teleportPrefab);
			}
		}
		if (this.m_gameObjectOnCastTarget == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.TargeterAbilityDeselected(int)).MethodHandle;
			}
			AbilityData abilityData = GameFlowData.Get().activeOwnedActorData.\u000E();
			if (abilityData != null)
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!this.m_ability.BackupTargets.IsNullOrEmpty<AbilityTarget>())
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
				if (targetIndex < this.m_ability.BackupTargets.Count)
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
					AbilityTarget abilityTarget = this.m_ability.BackupTargets[targetIndex];
					this.ClearHighlightCursors(true);
					this.SetLastUpdateCursorState(abilityTarget);
					if (this.IsUsingMultiTargetUpdate())
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
		this.m_nonActorSpecificContext.\u0015();
		this.MarkedForForceUpdate = true;
	}

	public List<AbilityTooltipSubject> GetTooltipSubjectTypes(ActorData actor)
	{
		List<AbilityTooltipSubject> result = null;
		if (actor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetTooltipSubjectTypes(ActorData)).MethodHandle;
			}
			using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = this.m_actorsInRange.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityUtil_Targeter.ActorTarget actorTarget = enumerator.Current;
					if (actorTarget.m_actor == actor)
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
						return actorTarget.m_subjectTypes;
					}
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
			}
		}
		return result;
	}

	public void AppendToTooltipSubjectSet(ActorData actor, HashSet<AbilityTooltipSubject> subjectsToAppendTo)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = this.GetTooltipSubjectTypes(actor);
		if (tooltipSubjectTypes != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.AppendToTooltipSubjectSet(ActorData, HashSet<AbilityTooltipSubject>)).MethodHandle;
			}
			using (List<AbilityTooltipSubject>.Enumerator enumerator = tooltipSubjectTypes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityTooltipSubject item = enumerator.Current;
					subjectsToAppendTo.Add(item);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.IsActorInTargetRange(ActorData, bool*)).MethodHandle;
			}
			for (int i = 0; i < this.m_actorsInRange.Count; i++)
			{
				AbilityUtil_Targeter.ActorTarget actorTarget = this.m_actorsInRange[i];
				if (actorTarget.m_actor == actor)
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
					ActorCover actorCover = actorTarget.m_actor.\u000E();
					if (actorCover != null)
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
						ActorData povactorData = GameFlowData.Get().POVActorData;
						bool flag;
						if (povactorData != null)
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
							flag = (povactorData.\u000E() != actor.\u000E());
						}
						else
						{
							flag = false;
						}
						bool flag2 = flag;
						bool flag3 = AbilityUtils.AbilityIgnoreCover(this.m_ability, actorTarget.m_actor);
						bool flag4 = actor.\u0018();
						bool flag5 = (!actorTarget.m_ignoreCoverMinDist) ? actorCover.IsInCoverWrt(actorTarget.m_damageOrigin) : actorCover.IsInCoverWrtDirectionOnly(actorTarget.m_damageOrigin, actorTarget.m_actor.\u0012());
						if (flag2)
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
							if (!flag3 && flag4)
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
								if (flag5)
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
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
				if (actorTarget.m_actor.\u0018())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetVisibleActorsInRange()).MethodHandle;
					}
					list.Add(actorTarget.m_actor);
				}
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
		}
		return list;
	}

	public virtual List<ActorData> GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject subject)
	{
		List<ActorData> list = new List<ActorData>();
		for (int i = 0; i < this.m_actorsInRange.Count; i++)
		{
			AbilityUtil_Targeter.ActorTarget actorTarget = this.m_actorsInRange[i];
			if (actorTarget.m_actor.\u0018() && actorTarget.m_subjectTypes.Contains(subject))
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject)).MethodHandle;
				}
				list.Add(actorTarget.m_actor);
			}
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
		return list;
	}

	public int GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject subject)
	{
		int num = 0;
		for (int i = 0; i < this.m_actorsInRange.Count; i++)
		{
			AbilityUtil_Targeter.ActorTarget actorTarget = this.m_actorsInRange[i];
			if (actorTarget.m_actor.\u0018())
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject)).MethodHandle;
				}
				if (actorTarget.m_subjectTypes.Contains(subject))
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
					num++;
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
			break;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetTooltipSubjectCountOnActor(ActorData, AbilityTooltipSubject)).MethodHandle;
				}
				for (int j = 0; j < actorTarget.m_subjectTypes.Count; j++)
				{
					if (actorTarget.m_subjectTypes[j] == subject)
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
						num++;
					}
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetTooltipSubjectCountTotalWithDuplicates(AbilityTooltipSubject)).MethodHandle;
					}
					num++;
				}
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
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.AddActorInRange(ActorData, Vector3, ActorData, AbilityTooltipSubject, bool)).MethodHandle;
			}
			AbilityUtil_Targeter.ActorTarget actorTarget = new AbilityUtil_Targeter.ActorTarget();
			actorTarget.m_actor = actor;
			actorTarget.m_damageOrigin = damageOrigin;
			actorTarget.m_subjectTypes = new List<AbilityTooltipSubject>();
			actorTarget.m_subjectTypes.Add(subjectType);
			if (actor == targetingActor)
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
				actorTarget.m_subjectTypes.Add(AbilityTooltipSubject.Self);
			}
			else if (actor.\u000E() == targetingActor.\u000E())
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_actorsInRange.Add(actorTarget);
			this.m_actorsAddedSoFar.Add(actor);
			if (!this.m_actorContextVars.ContainsKey(actor))
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
				ActorHitContext actorHitContext = new ActorHitContext();
				actorHitContext.\u0012 = true;
				this.m_actorContextVars[actor] = actorHitContext;
			}
			else
			{
				this.m_actorContextVars[actor].\u0012 = true;
			}
		}
		else if (appendSubjectType)
		{
			foreach (AbilityUtil_Targeter.ActorTarget actorTarget2 in this.m_actorsInRange)
			{
				if (actorTarget2.m_actor == actor)
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
			keyValuePair.Value.\u0012 = false;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.UpdateArrowsForUI()).MethodHandle;
				}
				flag = true;
				List<Vector3> list = KnockbackUtils.BuildDrawablePath(this.m_arrows[i].m_pathInfo, false);
				MovementPathStart componentInChildren = this.m_arrows[i].m_gameObject.GetComponentInChildren<MovementPathStart>();
				Vector3 u001D = list[list.Count - 1];
				BoardSquare characterMovementPanel = Board.\u000E().\u000E(u001D);
				componentInChildren.SetCharacterMovementPanel(characterMovementPanel);
			}
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
		if (!flag)
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
			if (this.m_arrows.Count > 0)
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
				this.m_arrows[0].m_gameObject.GetComponentInChildren<MovementPathStart>().HideCharacterMovementPanel();
			}
		}
	}

	protected void AddMovementArrow(ActorData mover, BoardSquarePathInfo path, Color arrowColor, MovementPathStart previousLine, bool isChasing, AbilityUtil_Targeter.TargeterMovementType movementType)
	{
		if (path != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.AddMovementArrow(ActorData, BoardSquarePathInfo, Color, MovementPathStart, bool, AbilityUtil_Targeter.TargeterMovementType)).MethodHandle;
			}
			if (mover != null && mover.\u0018())
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
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							flag = true;
							this.m_arrows[i].m_pathInfo = path;
							IL_D7:
							if (flag)
							{
								return;
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
							if (gameObject.GetComponentInChildren<MovementPathStart>() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.AddMovementArrowWithPrevious(ActorData, BoardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType, Color, int, bool)).MethodHandle;
			}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.SetMovementArrowEnabledFromIndex(int, bool)).MethodHandle;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.CanCreateMovementArrows(BoardSquarePathInfo)).MethodHandle;
			}
			Vector3 vector = list[0] - list[1];
			Vector3 vector2 = list[list.Count - 1] - list[list.Count - 2];
			if (vector.sqrMagnitude > 0f)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.ClearArrows()).MethodHandle;
					}
					if (arrowList.m_gameObject != null)
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
						MovementPathStart componentInChildren = arrowList.m_gameObject.GetComponentInChildren<MovementPathStart>();
						if (componentInChildren != null)
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
							componentInChildren.HideCharacterMovementPanel();
						}
						this.DestroyObjectAndMaterials(arrowList.m_gameObject);
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
		this.m_arrows.Clear();
	}

	public static GameObject CreateArrowFromGridPosPath(List<GridPos> gridPosPath, Color lineColor, bool isChasing, ActorData theActor)
	{
		GameObject result = null;
		if (gridPosPath.Count >= 2)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.CreateArrowFromGridPosPath(List<GridPos>, Color, bool, ActorData)).MethodHandle;
			}
			GridPos start = gridPosPath[0];
			GridPos end = gridPosPath[gridPosPath.Count - 1];
			result = AbilityUtil_Targeter.CreateArrowFromGridPosPoints(start, end, lineColor, isChasing, theActor);
		}
		return result;
	}

	public static GameObject CreateArrowFromGridPosPoints(GridPos start, GridPos end, Color lineColor, bool isChasing, ActorData theActor)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 item = Board.\u000E().\u000E(start).\u001D();
		item.y += 0.5f;
		list.Add(item);
		Vector3 item2 = Board.\u000E().\u000E(end).\u001D();
		item2.y += 0.5f;
		list.Add(item2);
		return Targeting.GetTargeting().CreateFancyArrowMesh(ref list, 0.2f, lineColor, isChasing, theActor, AbilityUtil_Targeter.TargeterMovementType.Movement, null, null, false, 0.4f, 0.4f);
	}

	public List<GameObject> GetHighlightCopies(bool setOpacity)
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < this.m_highlights.Count; i++)
		{
			if (this.m_highlights[i] != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetHighlightCopies(bool)).MethodHandle;
				}
				GameObject gameObject = HighlightUtils.Get().CloneTargeterHighlight(this.m_highlights[i], this);
				if (!this.m_highlights[i].activeSelf)
				{
					gameObject.SetActive(false);
				}
				list.Add(gameObject);
			}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.ClearHighlightCursors(bool)).MethodHandle;
			}
			if (NetworkClient.active)
			{
				if (this.m_highlights.Count > 0)
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
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.DestroyObjectAndMaterials(gameObject);
				}
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
		IL_A3:
		this.m_highlights.Clear();
		this.DestroyTargetingArcMesh();
		if (this.m_targetingArcPulseInstance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.DisableHighlightCursors()).MethodHandle;
			}
			foreach (GameObject gameObject in this.m_highlights)
			{
				if (gameObject != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.get_Highlight()).MethodHandle;
				}
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.set_Highlight(GameObject)).MethodHandle;
				}
				this.m_highlights = new List<GameObject>();
			}
			if (this.m_highlights.Count == 0)
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
				this.m_highlights.Add(null);
			}
			if (this.m_highlights[0] != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetAffectsTarget(ActorData, ActorData)).MethodHandle;
			}
			if (targeterOwner == null)
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
			}
			else
			{
				if (targeterOwner.\u000E() == potentialTarget.\u000E())
				{
					if (targeterOwner == potentialTarget)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetAffectedTeams(ActorData)).MethodHandle;
			}
			if (this.m_affectsAllies)
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
				list.Add(targeterOwner.\u000E());
			}
			if (this.m_affectsEnemies)
			{
				list.Add(targeterOwner.\u0012());
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetTargetingArcEndPosition(ActorData)).MethodHandle;
			}
			return this.Highlight.transform.position;
		}
		if (targetingActor != null)
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
			if (targetingActor.\u0012() != null)
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
				return targetingActor.\u0012().ToVector3();
			}
		}
		return Vector3.zero;
	}

	public virtual void SetupTargetingArc(ActorData targetingActor, bool activatePulse)
	{
		if (this.m_showArcToShape)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.SetupTargetingArc(ActorData, bool)).MethodHandle;
			}
			if (this.Highlight == null)
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
				this.m_showArcToShape = false;
			}
		}
		if (this.m_showArcToShape)
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
			if (this.Highlight != null)
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
				if ((this.GetTargetingArcEndPosition(targetingActor) - targetingActor.\u0012().ToVector3()).magnitude > HighlightUtils.Get().m_minDistForTargetingArc)
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
					Vector3 vector = targetingActor.\u0015();
					Vector3 vector2 = Camera.main.transform.rotation * Vector3.forward;
					bool flag;
					if ((vector2 - this.m_cameraForward).sqrMagnitude <= 0.01f)
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
						flag = ((Camera.main.transform.position - this.m_cameraPosition).sqrMagnitude > 0.01f);
					}
					else
					{
						flag = true;
					}
					bool flag2 = flag;
					if (flag2)
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
						this.m_cameraForward = vector2;
						this.m_cameraPosition = Camera.main.transform.position;
					}
					if (activatePulse)
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
						if (this.m_targetingArcPulseInstance != null)
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
							this.m_targetingArcPulseInstance.SetActive(false);
							UnityEngine.Object.Destroy(this.m_targetingArcPulseInstance);
							this.m_targetingArcPulseInstance = null;
						}
						this.m_targetingArcPulseInstance = UnityEngine.Object.Instantiate<GameObject>(HighlightUtils.Get().m_targetingArcForShape);
						this.m_targetingArcPulseInstance.SetActive(true);
						this.m_targetingArcPulseInstance.transform.position = vector;
						this.m_arcTraveled = 0f;
					}
					Vector3 targetingArcEndPosition = this.GetTargetingArcEndPosition(targetingActor);
					if (this.m_arcTraveled >= 1f)
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
						if (flag2)
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
						}
						else
						{
							this.m_arcTraveled = 0f;
							if (this.m_targetingArcPulseInstance != null)
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
								this.m_targetingArcPulseInstance.SetActive(false);
								UnityEngine.Object.Destroy(this.m_targetingArcPulseInstance);
								this.m_targetingArcPulseInstance = null;
								goto IL_518;
							}
							goto IL_518;
						}
					}
					float targetingArcMaxHeight = HighlightUtils.Get().m_targetingArcMaxHeight;
					float num = 1f + (vector.y - targetingArcEndPosition.y) / targetingArcMaxHeight;
					Vector3 a = targetingArcEndPosition - vector;
					a.y = 0f;
					if (a.magnitude <= 0.5f)
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
						a = new Vector3(0.5f * vector2.x, 0f, 0.5f * vector2.z);
					}
					float num2 = 0.5f * a.magnitude;
					float num3 = targetingArcMaxHeight / (num2 * num2);
					if (this.m_targetingArcPulseInstance != null)
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
						this.m_arcTraveled += HighlightUtils.Get().m_targetingArcMovementSpeed * Time.deltaTime;
						float num4 = this.m_arcTraveled * a.magnitude - num2;
						float num5 = num3 * num4 * num4;
						if (num4 > 0f)
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
							num5 *= num;
						}
						Vector3 position = vector + a * this.m_arcTraveled + Vector3.up * (targetingArcMaxHeight - num5);
						this.m_targetingArcPulseInstance.transform.position = position;
					}
					bool flag3 = false;
					if ((this.m_arcEnd - targetingArcEndPosition).sqrMagnitude > 0.1f)
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
						this.m_arcEnd = targetingArcEndPosition;
						flag3 = true;
					}
					if (!(this.m_targetingArcInstance == null) && !flag2)
					{
						if (!flag3)
						{
							goto IL_4D3;
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
						Vector3 item = vector + a * num6 + Vector3.up * (targetingArcMaxHeight - num8);
						list.Add(item);
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
					Color color = HighlightUtils.Get().m_targetingArcColor;
					if (targetingActor != GameFlowData.Get().activeOwnedActorData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.AdjustOpacityWhileTargeting()).MethodHandle;
			}
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
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
					foreach (MeshRenderer meshRenderer2 in componentsInChildren)
					{
						AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer2.materials, opacity);
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.StartConfirmedTargeting(AbilityTarget, ActorData)).MethodHandle;
					}
					if (arrowList.m_gameObject.activeSelf)
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
						MovementPathStart componentInChildren = arrowList.m_gameObject.GetComponentInChildren<MovementPathStart>();
						if (componentInChildren != null)
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
							componentInChildren.SetGlow(true);
						}
					}
				}
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
		}
		if (GameFlowData.Get().activeOwnedActorData == targetingActor)
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
			if (targetingActor.\u000E().CurrentState != TurnStateEnum.TARGETING_ACTION)
			{
				this.HideAllSquareIndicators();
			}
		}
		this.SetupTargetingArc(targetingActor, true);
		if (Application.isEditor)
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
			if (this.m_highlights != null)
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
				if (this.m_ability != null)
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
					using (List<GameObject>.Enumerator enumerator2 = this.m_highlights.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							GameObject gameObject = enumerator2.Current;
							if (gameObject != null)
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
								if (!gameObject.name.StartsWith("[Targeter]"))
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
									gameObject.name = "[Targeter] " + this.m_ability.GetNameString() + ": " + gameObject.name;
								}
							}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.SetTargeterHighlightOpacity(List<GameObject>, float)).MethodHandle;
			}
			MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer meshRenderer2 in componentsInChildren)
			{
				AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer2.materials, opacity);
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
			float opacity2 = Mathf.Clamp(opacity * HighlightUtils.Get().m_targeterParticleSystemOpacityMultiplier, 0f, 1f);
			ParticleSystemRenderer[] componentsInChildren2 = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();
			foreach (ParticleSystemRenderer particleSystemRenderer in componentsInChildren2)
			{
				AbilityUtil_Targeter.SetMaterialOpacity(particleSystemRenderer.materials, opacity2);
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.SetTargeterHighlightColor(List<GameObject>, Color, bool, bool)).MethodHandle;
				}
				MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
				foreach (MeshRenderer meshRenderer2 in componentsInChildren)
				{
					AbilityUtil_Targeter.SetMaterialColor(meshRenderer2.materials, color, keepOpacity);
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
				ParticleSystemRenderer[] componentsInChildren2 = gameObject.GetComponentsInChildren<ParticleSystemRenderer>();
				foreach (ParticleSystemRenderer particleSystemRenderer in componentsInChildren2)
				{
					if (clearColorOverTime)
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
						ParticleSystem component = particleSystemRenderer.GetComponent<ParticleSystem>();
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
							ParticleSystem.ColorOverLifetimeModule colorOverLifetime = component.colorOverLifetime;
							if (colorOverLifetime.enabled)
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
								colorOverLifetime.enabled = false;
							}
						}
					}
					AbilityUtil_Targeter.SetMaterialColor(particleSystemRenderer.materials, color, keepOpacity);
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
	}

	public static float GetOpacityFromTargeterData(HighlightUtils.TargeterOpacityData[] targeterOpacity, float timeSinceChange)
	{
		float result = 1f;
		for (int i = 0; i < targeterOpacity.Length; i++)
		{
			if (i == targeterOpacity.Length - 1)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetOpacityFromTargeterData(HighlightUtils.TargeterOpacityData[], float)).MethodHandle;
				}
				result = targeterOpacity[i].m_alpha;
			}
			else if (targeterOpacity[i].m_timeSinceConfirmed <= timeSinceChange)
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
				if (timeSinceChange <= targeterOpacity[i + 1].m_timeSinceConfirmed)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.SetMaterialOpacity(Material[], float)).MethodHandle;
					}
					Color color = material.GetColor(nameID);
					Color value = new Color(color.r, color.g, color.b, opacity);
					material.SetColor(nameID, value);
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
	}

	internal static void SetMaterialColor(Material[] materials, Color newColor, bool keepOpacity = true)
	{
		foreach (Material material in materials)
		{
			foreach (int nameID in AbilityUtil_Targeter.materialColorProperties)
			{
				if (material.HasProperty(nameID))
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.SetMaterialColor(Material[], Color, bool)).MethodHandle;
					}
					if (keepOpacity)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.HideAllSquareIndicators()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.HideUnusedSquareIndicators()).MethodHandle;
			}
			HighlightUtils.GetHiddenSquaresContainer().HideAllSquareIndicators(this.GetNextHiddenSquareIndicatorIndex());
		}
		if (HighlightUtils.GetAffectedSquaresContainer() != null)
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
			HighlightUtils.GetAffectedSquaresContainer().HideAllSquareIndicators(this.GetNextAffectedSquareIndicatorIndex());
		}
	}

	public int GetNextHiddenSquareIndicatorIndex()
	{
		if (HighlightUtils.GetHiddenSquaresContainer() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetNextHiddenSquareIndicatorIndex()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter.GetNextAffectedSquareIndicatorIndex()).MethodHandle;
			}
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
