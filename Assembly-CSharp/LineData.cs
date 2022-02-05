// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LineData : NetworkBehaviour, IGameEventListener
{
	public class LineInstance
	{
		public List<GridPos> m_positions;
		public bool isChasing;
		public GameObject m_lineObject;
		// removed in rogues
		public MovementPathStart m_movePathStart;
		// removed in rogues
		public Color m_currentColor;

		public LineInstance()
		{
			m_positions = new List<GridPos>();
		}

		// removed in rogues
		public void DestroyLineObject()
		{
			m_movePathStart = null;
			if (m_lineObject != null)
			{
				Object.Destroy(m_lineObject);
				m_lineObject = null;
			}
		}

		public override string ToString()
		{
			string result = m_lineObject != null
				? "(created lineObject)\n"
				: "(no lineObject)\n";
			for (int i = 0; i < m_positions.Count; i++)
			{
				GridPos gridPos = m_positions[i];
				if (i == 0)
				{
					result += $"({gridPos.x}, {gridPos.y})";
				}
				else if (i == m_positions.Count - 1)
				{
					result += $",\n({gridPos.x}, {gridPos.y}) (end)";
				}
				else
				{
					result += $",\n({gridPos.x}, {gridPos.y})";
				}
			}
			return result;
		}
	}

	private static bool MovementLinesCanBeVisibleForHud = true;
	private LineInstance m_movementLine;
	private LineInstance m_movementSnaredLine;
	private LineInstance m_movementLostLine;
	private LineInstance m_mousePotentialMovementLine;
	private GridPos m_lastDrawnStartPreview;
	private GridPos m_lastDrawnEndPreview;
	private float m_lastPreviewDrawnTime;
	private LineInstance m_potentialMovementLine;
	private ActorData m_actor;
	public float m_lastAllyMovementChange;
	private EasedFloatCubic m_alphaEased = new EasedFloatCubic(1f);
	private bool m_waitingForNextMoveSquaresUpdate;
	// removed in rogues
	private float m_lastMoveSquareUpdatedTime = -1f;

	public static bool SpectatorHideMovementLines { get; set; }

	private static bool MovementLinesCanBeVisible()
	{
		if (MovementLinesCanBeVisibleForHud && !SpectatorHideMovementLines)
		{
			return true;
		}
		if (!MovementLinesCanBeVisibleForHud)
		{
			return false;
		}
		return ClientGameManager.Get() == null
			|| ClientGameManager.Get().PlayerInfo == null
			|| !ClientGameManager.Get().PlayerInfo.IsSpectator
			|| !SpectatorHideMovementLines;
	}

	public static void SetAllowMovementLinesVisibleForHud(bool visible)
	{
		MovementLinesCanBeVisibleForHud = visible;
		if (GameFlowData.Get() != null)
		{
			foreach (ActorData actor in GameFlowData.Get().GetActors())
			{
				LineData lineData = actor.GetComponent<LineData>();
				if (lineData != null)
				{
					lineData.m_lastAllyMovementChange = Time.time;
					bool isClient = actor == GameFlowData.Get().activeOwnedActorData;
					lineData.RefreshLineVisibility(MovementLinesCanBeVisible(), isClient);
				}
			}
		}
	}

	private static Color GetColor(bool chasing, bool potential, bool clientPlayer)
	{
		Color result;
		if (chasing)
		{
			if (potential)
			{
				result = HighlightUtils.Get().s_defaultPotentialChasingColor;
			}
			else
			{
				result = HighlightUtils.Get().s_defaultChasingColor;
			}
		}
		else if (potential)
		{
			result = HighlightUtils.Get().s_defaultPotentialMovementColor;
		}
		else
		{
			result = HighlightUtils.Get().s_defaultMovementColor;
		}
		if (!clientPlayer)
		{
			float s_defaultDimMultiplier = HighlightUtils.Get().s_defaultDimMultiplier;
			result.r *= s_defaultDimMultiplier;
			result.g *= s_defaultDimMultiplier;
			result.b *= s_defaultDimMultiplier;
			result.a *= s_defaultDimMultiplier;
		}
		return result;
	}

	// added in rogues
#if SERVER
	public LineData.LineInstance MovementLine
	{
		get
		{
			return m_movementLine;
		}
	}
#endif

	// added in rogues
#if SERVER
	public LineData.LineInstance MovementSnaredLine
	{
		get
		{
			return m_movementSnaredLine;
		}
	}
#endif

	private void Awake()
	{
		m_actor = GetComponent<ActorData>();
	}

	private void Start()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedDecision);
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedPrep);
		}
		GameFlowData.s_onGameStateChanged += OnGameStateChanged;
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedDecision);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedPrep);
		}
		GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState newState)
	{
		// reactor
		if (newState == GameState.BothTeams_Decision)
		// rogues
		//if (GameFlowData.IsDecisionStateEnum(newState))
		{
			m_alphaEased.EaseTo(1f, 0.01f);
		}
	}

	public void OnClientRequestedMovementChange()
	{
		if (Time.time > m_lastMoveSquareUpdatedTime)  // unconditional in rogues
		{
			m_waitingForNextMoveSquaresUpdate = true;
			ClearMovementPreviewLine();
		}
	}

	public void OnCanMoveToSquaresUpdated()
	{
		m_waitingForNextMoveSquaresUpdate = false;

		// removed in rogues
		m_lastMoveSquareUpdatedTime = Time.time;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.UIPhaseStartedDecision)
		{
			OnMovementPhaseEnd();
			m_alphaEased.EaseTo(1f, 0.01f);
		}
		if (eventType == GameEventManager.EventType.UIPhaseStartedPrep)
		{
			m_alphaEased.EaseTo(0f, 2f);
		}
	}

	private void Update()
	{
		DisplayLineDataDebugInfo();
		if (m_potentialMovementLine != null
			&& GameFlowData.Get() != null
			&& InterfaceManager.Get() != null
			&& GameFlowData.Get().activeOwnedActorData == m_actor
			&& (m_actor.GetActorTurnSM().CurrentState != TurnStateEnum.DECIDING_MOVEMENT || m_actor.HasQueuedMovement())) // removed in rogues
        {
            ClearLine(ref m_potentialMovementLine);
        }
        if (m_movementLine != null || m_movementSnaredLine != null || m_potentialMovementLine != null)
		{
			bool isShowingAllyAbilities = InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo);
			if (isShowingAllyAbilities)
			{
				m_alphaEased = new EasedFloatCubic(1f);
				m_alphaEased.EaseTo(1f, 0.1f);
			}
			bool isClient = GameFlowData.Get().activeOwnedActorData == m_actor
				|| GameFlowData.Get().activeOwnedActorData == null
				|| Board.Get().PlayerClampedSquare != null && Board.Get().PlayerClampedSquare.OccupantActor == m_actor
				|| isShowingAllyAbilities;
			RefreshLineVisibility(MovementLinesCanBeVisible(), isClient);
			if (isShowingAllyAbilities && GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
			{
				m_alphaEased.EaseTo(0f, 1f);
			}
		}
		bool areLinesVisible = MovementLinesCanBeVisible();

		// reactor
		bool isWaypointing = Options_UI.Get() != null
			&& Options_UI.Get().GetShiftClickForMovementWaypoints() == InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier);
		// rogues
		//bool isWaypointing = InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier);


		bool amWaitingForUpdate = m_waitingForNextMoveSquaresUpdate;
		if (m_actor.GetActorTurnSM() != null
			&& (m_actor.GetActorTurnSM().CurrentState == TurnStateEnum.VALIDATING_MOVE_REQUEST
				|| m_actor.GetActorTurnSM().CurrentState == TurnStateEnum.VALIDATING_ACTION_REQUEST))
		{
			amWaitingForUpdate = true;
		}
		if (HighlightUtils.Get().m_showMovementPreview
			&& m_actor.GetAbilityData().GetSelectedAbility() == null
			&& GameFlowData.Get().IsInDecisionState()
			&& !amWaitingForUpdate
			&& GameFlowData.Get().activeOwnedActorData == m_actor
			&& GameFlowData.Get().activeOwnedActorData != null
			&& !m_actor.IsDead()
			//&& m_actor.GetActorTurnSM().AmDecidingMovement() // added in rogues
			//&& m_actor.GetActorTurnSM().CurrentState != TurnStateEnum.EXECUTING_ACTION // added in rogues
			&& (areLinesVisible || !isWaypointing))
		{
			if (Board.Get() != null
				&& GameFlowData.Get() != null
				&& Time.time - m_lastPreviewDrawnTime >= 0.1f)
            {
                BoardSquare currentBoardSquare = GameFlowData.Get().activeOwnedActorData.CurrentBoardSquare;
                if (Board.Get().PlayerClampedSquare != null && currentBoardSquare != null)
                {
                    GridPos currentPos = currentBoardSquare.GetGridPos();
                    GridPos clampedPos = Board.Get().PlayerClampedSquare.GetGridPos();
                    if (m_movementLine != null && isWaypointing)
                    {
                        BoardSquare moveFromBoardSquare = GameFlowData.Get().activeOwnedActorData.MoveFromBoardSquare;
                        GridPos moveFromPos = moveFromBoardSquare.GetGridPos();
                        if (m_lastDrawnStartPreview.x != moveFromPos.x
							|| m_lastDrawnStartPreview.y != moveFromPos.y
							|| m_lastDrawnEndPreview.x != clampedPos.x
							|| m_lastDrawnEndPreview.y != clampedPos.y)
                        {
                            if (m_actor.RemainingHorizontalMovement > 0f)
                            {
                                BoardSquare clampedSquare = Board.Get().PlayerClampedSquare;
                                if (!m_actor.CanMoveToBoardSquare(clampedSquare))
                                {
                                    clampedSquare = m_actor.GetActorMovement().GetClosestMoveableSquareTo(clampedSquare, false);
                                }
                                BoardSquarePathInfo path2 = m_actor.GetActorMovement().BuildPathTo(moveFromBoardSquare, clampedSquare);
                                SetMouseOverMovementLine(GetGridPosPathForPath(path2, false, null), false, false);

								// reactor
								Color lineColor = HighlightUtils.Get().m_movementLinePreviewColor;
								// rogues
								//Color lineColor = (!actor.GetActorMovement().SquaresCanMoveToWithQueuedAbility.Contains(Board.Get().PlayerClampedSquare)) ? HighlightUtils.Get().m_movementLinePreviewColorSprint : HighlightUtils.Get().m_movementLinePreviewColor;

								ShowLine(m_mousePotentialMovementLine, lineColor, true, false);
                                m_lastDrawnStartPreview = moveFromPos;
                                m_lastDrawnEndPreview = clampedPos;
                                m_lastPreviewDrawnTime = Time.time;
                            }
							else
							{
								ClearLine(ref m_mousePotentialMovementLine);
							}
                        }
                    }
                    else if ((m_lastDrawnStartPreview.x != currentPos.x
							|| m_lastDrawnStartPreview.y != currentPos.y
							|| m_lastDrawnEndPreview.x != clampedPos.x
							|| m_lastDrawnEndPreview.y != clampedPos.y)
						//&& !GameFlowData.Get().GetPause()  // added in rogues
						)
                    {
                        BoardSquare boardSquare = Board.Get().PlayerClampedSquare;
                        if (!m_actor.CanMoveToBoardSquare(boardSquare))
                        {
                            boardSquare = m_actor.GetActorMovement().GetClosestMoveableSquareTo(boardSquare, false);
                        }
                        BoardSquare initialMoveStartSquare = GameFlowData.Get().activeOwnedActorData.InitialMoveStartSquare;
                        BoardSquarePathInfo path = m_actor.GetActorMovement().BuildPathTo(initialMoveStartSquare, boardSquare);
                        SetMouseOverMovementLine(GetGridPosPathForPath(path, false, null), false, false);

						// readctor
						Color lineColor = HighlightUtils.Get().m_movementLinePreviewColor;
						// rogues
						//Color lineColor = (!actor.GetActorMovement().SquaresCanMoveToWithQueuedAbility.Contains(boardSquare)) ? HighlightUtils.Get().m_movementLinePreviewColorSprint : HighlightUtils.Get().m_movementLinePreviewColor;
                        
						ShowLine(m_mousePotentialMovementLine, lineColor, true, false);
                        m_lastDrawnStartPreview = initialMoveStartSquare.GetGridPos();
                        m_lastDrawnEndPreview = clampedPos;
                        m_lastPreviewDrawnTime = Time.time;
                    }
                }
                else
                {
                    ClearLine(ref m_mousePotentialMovementLine);
                }
            }
        }
		else
		{
			ClearLine(ref m_mousePotentialMovementLine);
		}
	}

	private void DisplayLineDataDebugInfo()
	{
		if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("DisplayLineData"))
		{
			string text = "";
			if (m_movementLine != null)
			{
				text = text + "Movement:\n" + m_movementLine.ToString();
			}
			UIActorDebugPanel.Get().SetActorValue(m_actor, "DisplayLineData", text);
		}
	}

	public void OnDeserializedData(LineInstance movementLine, sbyte numNodesInSnaredLine)
	{
		ClearAllLines();
		m_movementLine = movementLine;
		OnDeSerializeNumNodesInSnaredLine(numNodesInSnaredLine);
		DrawLine(ref m_movementSnaredLine);
		DrawLine(ref m_movementLine);
		DrawLine(ref m_movementLostLine, 0.2f);
		HideAllyMovementLinesIfResolving();
		if (SinglePlayerManager.Get() != null
			&& SinglePlayerManager.Get().GetLocalPlayer() != null)
		{
			SinglePlayerManager.Get().OnActorMovementChanged(SinglePlayerManager.Get().GetLocalPlayer());
		}
	}

	public static void SerializeLine(LineInstance line, NetworkWriter writer)
	{
		sbyte num = (sbyte)line.m_positions.Count;
		writer.Write(num);
		bool isChasing = line.isChasing;
		writer.Write(isChasing);
		for (int i = 0; i < num; i++)
		{
			int x = line.m_positions[i].x;
			int y = line.m_positions[i].y;
			byte xs = 0;
			byte ys = 0;
			if (x >= 0 && x <= 255)
			{
				xs = (byte)x;
			}
			else if (Application.isEditor)
			{
				Debug.LogError("Trying to serialize invalid grid pos x for LineData");
			}
			if (y >= 0 && y <= 255)
			{
				ys = (byte)y;
			}
			else if (Application.isEditor)
			{
				Debug.LogError("Trying to serialize invalid grid pos y for LineData");
			}
			writer.Write(xs);
			writer.Write(ys);
		}
	}

	public static LineInstance DeSerializeLine(NetworkReader reader)
	{
		LineInstance lineInstance = new LineInstance();
		sbyte num = reader.ReadSByte();
        lineInstance.isChasing = reader.ReadBoolean();
		lineInstance.m_positions.Clear();
		for (int i = 0; i < num; i++)
		{
			byte x = reader.ReadByte();
			byte y = reader.ReadByte();
			GridPos item = default(GridPos);
			item.x = x;
			item.y = y;
			item.height = (int)Board.Get().GetHeightAt(item.x, item.y);
			lineInstance.m_positions.Add(item);
		}
		return lineInstance;
	}

	public void OnDeSerializeNumNodesInSnaredLine(sbyte numNodesInSnaredLine)
	{
		if (numNodesInSnaredLine > 0)
		{
			if (m_movementLine != null)
			{
				numNodesInSnaredLine = (sbyte)Mathf.Min(numNodesInSnaredLine, (sbyte)m_movementLine.m_positions.Count);
				if (m_movementSnaredLine == null)
				{
					m_movementSnaredLine = new LineInstance();
				}
				if (m_movementLostLine == null)
				{
					m_movementLostLine = new LineInstance();
				}
				m_movementSnaredLine.m_positions.Clear();
				m_movementSnaredLine.isChasing = m_movementLine.isChasing;
				m_movementLostLine.m_positions.Clear();
				m_movementLostLine.isChasing = m_movementLine.isChasing;
				for (int i = 0; i < numNodesInSnaredLine; i++)
				{
					GridPos item = m_movementLine.m_positions[i];
					m_movementSnaredLine.m_positions.Add(item);
				}
				int num = Mathf.Max(0, numNodesInSnaredLine - 1);
				for (int i = num; i < m_movementLine.m_positions.Count; i++)
				{
					GridPos item = m_movementLine.m_positions[i];
					m_movementLostLine.m_positions.Add(item);
				}
				ChangeLineColor(m_movementLine, Color.red);
			}
		}
		else
		{
			ClearLine(ref m_movementSnaredLine);
			if (m_movementLine != null)
			{
				if (m_movementLostLine == null)
				{
					m_movementLostLine = new LineInstance();
				}
				m_movementLostLine.m_positions.Clear();
				m_movementLostLine.isChasing = m_movementLine.isChasing;
				foreach (GridPos pos in m_movementLine.m_positions)
				{
					m_movementLostLine.m_positions.Add(pos);
				}
			}
			else
			{
				ClearLine(ref m_movementLostLine);
			}
		}
	}

	// server-only, empty in reactor
	private void MarkForSerializationUpdate()
	{
#if SERVER
		if (NetworkServer.active && m_actor != null && m_actor.TeamSensitiveData_friendly != null)
		{
			m_actor.TeamSensitiveData_friendly.MarkAsDirty(ActorTeamSensitiveData.DirtyBit.LineData);
		}
#endif
	}

	private void GetMovementLineVisibilityFromStatus(float colorMult, out bool fullMovement, out bool snaredMovement, out bool showLostMovement, out Color fullPathColor, out Color snaredColor)
	{
		fullMovement = true;
		snaredMovement = false;
		showLostMovement = false;
		fullPathColor = snaredColor = HighlightUtils.Get().s_defaultMovementColor * colorMult;

		if (m_actor != null && m_actor.GetActorStatus() != null)
		{
			ActorStatus actorStatus = m_actor.GetActorStatus();
			AbilityData abilityData = m_actor.GetAbilityData();
			bool isMovementDebuffImmune = actorStatus.IsMovementDebuffImmune();
			if (!isMovementDebuffImmune)
			{
				isMovementDebuffImmune = abilityData != null
					&& (abilityData.HasPendingStatusFromQueuedAbilities(StatusType.MovementDebuffImmunity)
						|| abilityData.HasPendingStatusFromQueuedAbilities(StatusType.Unstoppable));
			}
			bool isFullDebuff = !actorStatus.HasStatus(StatusType.MovementDebuffSuppression, false) && !isMovementDebuffImmune;
			bool isRooted = actorStatus.HasStatus(StatusType.Rooted, false);
			bool isKnockedBack = actorStatus.HasStatus(StatusType.KnockedBack, false);
			if (isKnockedBack)
			{
				fullPathColor.a = 0f;
				snaredColor.a = 0f;
			}
			if (isFullDebuff && (isRooted || isKnockedBack))
			{
				if (isKnockedBack)
				{
					fullMovement = false;
				}
				else if (isRooted)
				{
					fullPathColor = HighlightUtils.Get().s_defaultLostMovementColor * colorMult;
				}
			}
			else
			{
				if (isFullDebuff
					&& actorStatus.HasStatus(StatusType.Snared, false)
					&& !actorStatus.HasStatus(StatusType.Hasted, false)
					&& (abilityData == null || !abilityData.HasPendingStatusFromQueuedAbilities(StatusType.Hasted)))
				{
					snaredMovement = true;
					fullMovement = false;
					showLostMovement = true;
					fullPathColor = HighlightUtils.Get().s_defaultLostMovementColor * colorMult;
					snaredColor = HighlightUtils.Get().s_defaultMovementColor * colorMult;
				}
			}
		}
		fullPathColor.a *= m_alphaEased;
		snaredColor.a *= m_alphaEased;
	}

	public void OnMovementStatusGained()
	{
		if (GameFlowData.Get() != null
			&& GameFlowData.Get().gameState == GameState.BothTeams_Resolve
			&& GameFlowData.Get().activeOwnedActorData != null
			&& GameFlowData.Get().activeOwnedActorData == m_actor)
		{
			m_alphaEased = new EasedFloatCubic(1f);
			m_alphaEased.EaseTo(0f, 3f);
		}
	}

	public void ClearAllLines()
	{
		ClearLine(ref m_movementLine);
		ClearLine(ref m_movementSnaredLine);
		ClearLine(ref m_movementLostLine);
		ClearMovementPreviewLine();
	}

	private void ClearMovementPreviewLine()
	{
		ClearLine(ref m_potentialMovementLine);
		m_lastPreviewDrawnTime = Time.time;
		m_lastDrawnStartPreview = GridPos.s_invalid;
		m_lastDrawnEndPreview = GridPos.s_invalid;
	}

	private void ClearLine(ref LineInstance theLine)
	{
		if (theLine != null)
		{
			if (theLine == m_movementLine)
			{
				HideMovementDestIndicator(m_movementLine);
			}
			theLine.m_positions.Clear();
			if (theLine.m_lineObject != null)
			{
				// reactor
				theLine.DestroyLineObject();
				// rogues
				//Object.Destroy(theLine.m_lineObject);
				//theLine.m_lineObject = null;
			}
			theLine = null;
			if (NetworkServer.active)
			{
				MarkForSerializationUpdate();
			}
		}
	}

	private void RefreshLineVisibility(bool visible, bool isClient)
	{
		float timeSinceChange = Time.time - m_lastAllyMovementChange;
		if (visible)
		{
			float opacity = 1f;
			if (!isClient)
			{
				opacity = AbilityUtil_Targeter.GetOpacityFromTargeterData(HighlightUtils.Get().m_movementLineOpacity, timeSinceChange);
			}
			bool isChasing = m_movementLine != null
				&& m_movementSnaredLine == null
				&& m_movementLine.isChasing;
			GetMovementLineVisibilityFromStatus(opacity, out bool fullMovement, out bool snaredMovement, out bool showLostMovement, out Color fullPathColor, out Color snaredColor);
			if (fullPathColor.a > 0f && (fullMovement || isChasing))
			{
				ShowLine(m_movementLine, fullPathColor);
				ShowMovementDestIndicator(m_movementLine);
			}
			else
			{
				HideLine(m_movementLine);
				HideMovementDestIndicator(m_movementLine);
			}
			if (snaredColor.a > 0f && snaredMovement && !isChasing)
			{
				ShowLine(m_movementSnaredLine, snaredColor);
			}
			else
			{
				HideLine(m_movementSnaredLine);
			}
			if (fullPathColor.a > 0f && showLostMovement && !isChasing)
			{
				ShowLine(m_movementLostLine, fullPathColor, true);
			}
			else
			{
				HideLine(m_movementLostLine);
			}
			ShowLine(m_potentialMovementLine, HighlightUtils.Get().s_defaultPotentialMovementColor * opacity);
			if (fullMovement && m_potentialMovementLine != null)
			{
				ShowMovementDestIndicator(m_potentialMovementLine);
			}
		}
		else
		{
			HideLine(m_movementLine);
			HideMovementDestIndicator(m_movementLine);
			HideLine(m_movementSnaredLine);
			HideLine(m_potentialMovementLine);
			HideLine(m_movementLostLine);
		}
	}

	private void DrawLine(ref LineInstance theLine, float startOffset = 0.4f)
	{
		bool isTeamView = false;
		if (m_actor != null
			&& GameFlowData.Get() != null
			&& GameFlowData.Get().LocalPlayerData != null)
		{
			Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
			isTeamView = teamViewing == Team.Invalid || teamViewing == m_actor.GetTeam();
		}
		if (theLine != null && isTeamView)
		{
			Color color = GetColor(theLine.isChasing, false, m_actor == GameFlowData.Get().activeOwnedActorData);
			MovementPathStart previousLine = theLine.m_lineObject != null
				// reactor
				? theLine.m_movePathStart
				// rogues
				//? theLine.m_lineObject.GetComponentInChildren<MovementPathStart>(true)
				: null;
			theLine.m_lineObject = DrawLine(theLine.m_positions, color, previousLine, theLine.isChasing, m_actor, startOffset);

			// removed in rogues
			theLine.m_currentColor = color;
			theLine.m_movePathStart = theLine.m_lineObject?.GetComponentInChildren<MovementPathStart>(true);
			// end removed in rogues

			if (!MovementLinesCanBeVisible())
			{
				HideLine(theLine);
			}
		}
	}

	private void ActivatePotentialMovementLine()
	{
		if (m_potentialMovementLine != null
			&& m_actor != null
			&& GameFlowData.Get()
			&& GameFlowData.Get().activeOwnedActorData == m_actor)
		{
			Color color = GetColor(m_potentialMovementLine.isChasing, true, true);
			MovementPathStart previousLine = m_potentialMovementLine.m_lineObject != null
				// reactor
				? m_potentialMovementLine.m_movePathStart
				// rogues
				//? m_potentialMovementLine.m_lineObject.GetComponentInChildren<MovementPathStart>(true)
				: null;
			m_potentialMovementLine.m_lineObject = DrawLine(m_potentialMovementLine.m_positions, color, previousLine, m_potentialMovementLine.isChasing, m_actor);
			// removed in rogues
			m_potentialMovementLine.m_currentColor = color;
			m_potentialMovementLine.m_movePathStart = m_potentialMovementLine.m_lineObject?.GetComponentInChildren<MovementPathStart>(true);
			// end removed in rogues

			if (!MovementLinesCanBeVisible())
			{
				HideLine(m_potentialMovementLine);
			}
		}
	}

	private GameObject DrawLine(List<GridPos> targetSquares, Color lineColor, MovementPathStart previousLine, bool isChasing, ActorData theActor, float startOffset = 0.4f)
	{
		if (targetSquares.Count < 2)
		{
			return null;
		}
		m_lastAllyMovementChange = Time.time;
		List<Vector3> points = new List<Vector3>();
		foreach (GridPos targetPos in targetSquares)
		{
			Vector3 worldPosition = Board.Get().GetSquare(targetPos).GetOccupantRefPos();
			points.Add(new Vector3(worldPosition.x, worldPosition.y + 0.1f, worldPosition.z));
		}
		return Targeting.GetTargeting().CreateFancyArrowMesh(ref points, 0.2f, lineColor, isChasing, theActor, AbilityUtil_Targeter.TargeterMovementType.Movement, null, previousLine, true, startOffset);
	}

	public void OnTurnStart()
	{
		ClearAllLines();
	}

	public void OnMovementPhaseEnd()
	{
		ClearAllLines();
	}

	private List<GridPos> GetGridPosPathForPath(BoardSquarePathInfo path, bool isChasing, ActorData chaseTarget)
	{
		List<GridPos> list = null;
		if (isChasing)
		{
			list = new List<GridPos>
			{
				m_actor.MoveFromBoardSquare.GetGridPos(),
				chaseTarget.GetCurrentBoardSquare().GetGridPos()
			};
		}
		else if (path != null)
		{
			list = path.ToGridPosPath();
		}
		return list;
	}

	// server-only, empty in reactor
	public void OnMovementChanged(List<GridPos> fullPathInfo, List<GridPos> snaredPathInfo, bool isChasing, bool rebuildLine)
	{
#if SERVER
		ActorData actor = m_actor;
		if (NetworkServer.active && actor)
		{
			if (fullPathInfo != null)
			{
				SetMovementLine(fullPathInfo, isChasing, rebuildLine);
			}
			else
			{
				ClearLine(ref m_movementLine);
			}
			if (snaredPathInfo != null)
			{
				SetMovementSnaredLine(snaredPathInfo, isChasing, rebuildLine);
			}
			else
			{
				ClearLine(ref m_movementSnaredLine);
			}
			int numSnaredNodes = (snaredPathInfo != null) ? snaredPathInfo.Count : 0;
			if (fullPathInfo != null)
			{
				SetMovementLostLine(fullPathInfo, numSnaredNodes, isChasing, rebuildLine);
				return;
			}
			ClearLine(ref m_movementLostLine);
		}
#endif
	}

	public void OnResolveStart()
	{
		HideAllyMovementLinesIfResolving();
	}

	private void HideAllyMovementLinesIfResolving()
    {
        if (GameFlowData.Get() != null
			&& m_actor != GameFlowData.Get().activeOwnedActorData
			&& GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
        {
            HideLine(m_movementLine);
            HideLine(m_movementSnaredLine);
            HideLine(m_movementLostLine);
        }
    }

    public void OnDisable()
	{
		ClearLine(ref m_movementLine);
		ClearLine(ref m_movementSnaredLine);
	}

	private void ChangeLineColor(LineInstance theLine, Color newColor)
	{
		if (theLine != null
			&& theLine.m_lineObject != null
			 && theLine.m_currentColor != newColor)  // removed in rogues
		{
			// removed in rogues
			theLine.m_currentColor = newColor;

			foreach (MeshRenderer renderer in theLine.m_lineObject.GetComponentsInChildren<MeshRenderer>())
			{
				if (renderer.materials.Length > 0)
				{
					renderer.materials[0].SetColor("_TintColor", newColor);
				}
			}
		}
	}

	private void ShowLine(LineInstance theLine, Color lineColor, bool hideStart = false, bool setGlow = true)
	{
		ChangeLineColor(theLine, lineColor);
		if (theLine != null
			&& theLine.m_lineObject != null)
		{
			// reactor
			MovementPathStart movePathStart = theLine.m_movePathStart;
			// rogues
			//MovementPathStart movePathStart = theLine.m_lineObject.GetComponentInChildren<MovementPathStart>(true);
			
			if (movePathStart != null)
			{
				movePathStart.SetGlow(setGlow);
				if (hideStart)
				{
					movePathStart.m_movementContainer.SetActive(false);
				}
			}
		}
	}

	private void HideLine(LineInstance theLine)
	{
		ChangeLineColor(theLine, Color.clear);
		if (theLine != null
			&& theLine.m_lineObject != null)
		{
			// reactor
			MovementPathStart movePathStart = theLine.m_movePathStart;
			// rogues
			//MovementPathStart movePathStart = theLine.m_lineObject.GetComponentInChildren<MovementPathStart>(true);
			
			if (movePathStart != null)
			{
				movePathStart.SetGlow(false);
			}
		}
	}

	private void ShowMovementDestIndicator(LineInstance inst)
	{
		if (inst != null
			&& inst.m_lineObject != null
			 && inst.m_movePathStart != null // removed in rogues
			&& inst.m_positions.Count > 0)
		{
			// reactor
			MovementPathStart movePathStart = inst.m_movePathStart;
			// rogues
			//MovementPathStart movePathStart = inst.m_lineObject.GetComponentInChildren<MovementPathStart>(true);
			
			GridPos gridPos = inst.m_positions[inst.m_positions.Count - 1];
			BoardSquare square = Board.Get().GetSquare(gridPos);
			if (movePathStart != null)
			{
				movePathStart.SetCharacterMovementPanel(square);
			}
		}
	}

	private void HideMovementDestIndicator(LineInstance inst)
	{
		if (inst != null
			&& inst.m_lineObject != null)
		{
			// reactor
			MovementPathStart movePathStart = inst.m_movePathStart;
			// rogues
			//MovementPathStart movePathStart = inst.m_lineObject.GetComponentInChildren<MovementPathStart>(true);
			if (movePathStart != null)
			{
				movePathStart.HideCharacterMovementPanel();
			}
		}
	}

	private void SetMovementSnaredLine(List<GridPos> gridPosPath, bool isChasing, bool rebuildLine)
	{
		if (rebuildLine || isChasing)
		{
			ClearLine(ref m_movementSnaredLine);
		}
		if (gridPosPath.Count > 1)
		{
			if (rebuildLine || m_movementSnaredLine == null)
			{
				m_movementSnaredLine = new LineInstance();
			}
			m_movementSnaredLine.m_positions = gridPosPath;
			m_movementSnaredLine.isChasing = isChasing;
			DrawLine(ref m_movementSnaredLine);
			if (m_movementLine != null
				&& m_movementLine.m_lineObject != null)
			{
				// reactor
				MovementPathStart movePathStart = m_movementLine.m_movePathStart;
				// rogues
				//MovementPathStart movePathStart = m_movementLine.m_lineObject.GetComponentInChildren<MovementPathStart>(true);
				if (movePathStart != null)
				{
					movePathStart.SetGlow(false);
				}
			}
		}
		else if (gridPosPath.Count == 1
			&& m_movementLine != null
			&& m_movementLine.m_lineObject != null)
		{
			// reactor
			MovementPathStart movePathStart = m_movementLine.m_movePathStart;
			// rogues
			//MovementPathStart movePathStart = m_movementLine.m_lineObject.GetComponentInChildren<MovementPathStart>(true);
			if (movePathStart != null)
			{
				movePathStart.SetGlow(false);
			}
		}
		MarkForSerializationUpdate();
	}

	private void SetMovementLine(List<GridPos> gridPosPath, bool isChasing, bool rebuildLine)
	{
		if (rebuildLine || isChasing)
		{
			ClearLine(ref m_movementLine);
		}
		if (gridPosPath.Count > 1)
		{
			if (rebuildLine || m_movementLine == null)
			{
				m_movementLine = new LineInstance();
			}
			m_movementLine.m_positions = gridPosPath;
			m_movementLine.isChasing = isChasing;
			DrawLine(ref m_movementLine);
		}
		MarkForSerializationUpdate();
	}

	private void SetMouseOverMovementLine(List<GridPos> gridPosPath, bool isChasing, bool rebuildLine)
	{
		if (rebuildLine || isChasing)
		{
			ClearLine(ref m_mousePotentialMovementLine);
		}
		if (gridPosPath != null && gridPosPath.Count > 1)
		{
			if (rebuildLine || m_mousePotentialMovementLine == null)
			{
				m_mousePotentialMovementLine = new LineInstance();
			}
			m_mousePotentialMovementLine.m_positions = gridPosPath;
			m_mousePotentialMovementLine.isChasing = isChasing;
			DrawLine(ref m_mousePotentialMovementLine);
			if (m_mousePotentialMovementLine.m_lineObject != null)
			{
				// reactor
				MovementPathStart movePathStart = m_mousePotentialMovementLine.m_movePathStart;
				// rogues
				//MovementPathStart movePathStart = m_mousePotentialMovementLine.m_lineObject.GetComponentInChildren<MovementPathStart>(true);
				if (movePathStart != null)
				{
					movePathStart.SetGlow(false);
				}
			}
		}
		else if (m_mousePotentialMovementLine != null)
		{
			ClearLine(ref m_mousePotentialMovementLine);
		}
	}

	private void SetMovementLostLine(List<GridPos> fullGridPosPath, int numSnaredNodes, bool isChasing, bool rebuildLine)
	{
		if (rebuildLine || isChasing)
		{
			ClearLine(ref m_movementLostLine);
		}
		List<GridPos> list = new List<GridPos>();
		int num = Mathf.Max(0, numSnaredNodes - 1);
		for (int i = num; i < fullGridPosPath.Count; i++)
		{
			list.Add(fullGridPosPath[i]);
		}
		if (list.Count > 1)
		{
			if (rebuildLine || m_movementLostLine == null)
			{
				m_movementLostLine = new LineInstance();
			}
			m_movementLostLine.m_positions = list;
			m_movementLostLine.isChasing = isChasing;
			DrawLine(ref m_movementLostLine, 0.2f);
		}
		MarkForSerializationUpdate();
	}

	private void SetPotentialMovementLine(List<GridPos> gridPosPath, bool isChasing)
	{
		ClearLine(ref m_potentialMovementLine);
		if (gridPosPath.Count > 1)
		{
			m_potentialMovementLine = new LineInstance
			{
				m_positions = gridPosPath,
				isChasing = isChasing
			};
			ActivatePotentialMovementLine();
		}
	}

	public List<GridPos> GetGridPosPath()
	{
		if (m_movementLine != null)
		{
			return m_movementLine.m_positions;
		}
		if (m_potentialMovementLine != null)
		{
			return m_potentialMovementLine.m_positions;
		}
		return new List<GridPos>();
	}

	public bool GetIsChasing()
	{
		if (m_movementLine != null)
		{
			return m_movementLine.isChasing;
		}
		if (m_potentialMovementLine != null)
		{
			return m_potentialMovementLine.isChasing;
		}
		return false;
	}

	// reactor
	private void UNetVersion()
	{
	}
	// rogues
	//private void MirrorProcessed()
	//{
	//}

	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	// removed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
