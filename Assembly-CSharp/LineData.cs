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

		public MovementPathStart m_movePathStart;

		public Color m_currentColor;

		public LineInstance()
		{
			m_positions = new List<GridPos>();
		}

		public void DestroyLineObject()
		{
			m_movePathStart = null;
			if (!(m_lineObject != null))
			{
				return;
			}
			while (true)
			{
				Object.Destroy(m_lineObject);
				m_lineObject = null;
				return;
			}
		}

		public override string ToString()
		{
			string empty = string.Empty;
			empty = ((!(m_lineObject == null)) ? (empty + "(created lineObject)\n") : (empty + "(no lineObject)\n"));
			for (int i = 0; i < m_positions.Count; i++)
			{
				GridPos gridPos = m_positions[i];
				if (i == 0)
				{
					empty += $"({gridPos.x}, {gridPos.y})";
				}
				else if (i == m_positions.Count - 1)
				{
					empty += $",\n({gridPos.x}, {gridPos.y}) (end)";
				}
				else
				{
					empty += $",\n({gridPos.x}, {gridPos.y})";
				}
			}
			while (true)
			{
				return empty;
			}
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

	private float m_lastMoveSquareUpdatedTime = -1f;

	public static bool SpectatorHideMovementLines
	{
		get;
		set;
	}

	private static bool MovementLinesCanBeVisible()
	{
		if (MovementLinesCanBeVisibleForHud)
		{
			if (!SpectatorHideMovementLines)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		if (!MovementLinesCanBeVisibleForHud)
		{
			while (true)
			{
				return false;
			}
		}
		int num;
		if (ClientGameManager.Get() != null)
		{
			if (ClientGameManager.Get().PlayerInfo != null)
			{
				num = (ClientGameManager.Get().PlayerInfo.IsSpectator ? 1 : 0);
				goto IL_0085;
			}
		}
		num = 0;
		goto IL_0085;
		IL_0085:
		int result;
		if (num != 0)
		{
			result = ((!SpectatorHideMovementLines) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public static void SetAllowMovementLinesVisibleForHud(bool visible)
	{
		MovementLinesCanBeVisibleForHud = visible;
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			List<ActorData> actors = GameFlowData.Get().GetActors();
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					LineData component = current.GetComponent<LineData>();
					if (component != null)
					{
						component.m_lastAllyMovementChange = Time.time;
						component.RefreshLineVisibility(MovementLinesCanBeVisible(), current == GameFlowData.Get().activeOwnedActorData);
					}
				}
				while (true)
				{
					switch (6)
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

	private static Color GetColor(bool chasing, bool potential, bool clientPlayer)
	{
		Color result = Color.clear;
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
		if (newState == GameState.BothTeams_Decision)
		{
			m_alphaEased.EaseTo(1f, 0.01f);
		}
	}

	public void OnClientRequestedMovementChange()
	{
		if (!(Time.time > m_lastMoveSquareUpdatedTime))
		{
			return;
		}
		while (true)
		{
			m_waitingForNextMoveSquaresUpdate = true;
			ClearMovementPreviewLine();
			return;
		}
	}

	public void OnCanMoveToSquaresUpdated()
	{
		m_waitingForNextMoveSquaresUpdate = false;
		m_lastMoveSquareUpdatedTime = Time.time;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.UIPhaseStartedDecision)
		{
			OnMovementPhaseEnd();
			m_alphaEased.EaseTo(1f, 0.01f);
		}
		if (eventType != GameEventManager.EventType.UIPhaseStartedPrep)
		{
			return;
		}
		while (true)
		{
			m_alphaEased.EaseTo(0f, 2f);
			return;
		}
	}

	private void Update()
	{
		DisplayLineDataDebugInfo();
		ActorData actor = m_actor;
		GameFlowData gameFlowData = GameFlowData.Get();
		ActorTurnSM actorTurnSM = actor.GetActorTurnSM();
		if (m_potentialMovementLine != null)
		{
			InterfaceManager exists = InterfaceManager.Get();
			if ((bool)gameFlowData)
			{
				if ((bool)exists)
				{
					if (gameFlowData.activeOwnedActorData == actor)
					{
						if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING_MOVEMENT)
						{
							if (!actor.HasQueuedMovement())
							{
								goto IL_00ae;
							}
						}
						ClearLine(ref m_potentialMovementLine);
					}
				}
			}
		}
		goto IL_00ae;
		IL_01e4:
		bool flag = MovementLinesCanBeVisible();
		int num;
		if (Options_UI.Get() != null)
		{
			num = ((Options_UI.Get().GetShiftClickForMovementWaypoints() == InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier)) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag2 = (byte)num != 0;
		bool flag3 = m_waitingForNextMoveSquaresUpdate;
		if (actorTurnSM != null)
		{
			if (actorTurnSM.CurrentState != TurnStateEnum.VALIDATING_MOVE_REQUEST)
			{
				if (actorTurnSM.CurrentState != TurnStateEnum.VALIDATING_ACTION_REQUEST)
				{
					goto IL_0272;
				}
			}
			flag3 = true;
		}
		goto IL_0272;
		IL_00ae:
		if (m_movementLine == null)
		{
			if (m_movementSnaredLine == null)
			{
				if (m_potentialMovementLine == null)
				{
					goto IL_01e4;
				}
			}
		}
		bool flag4 = InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo);
		if (flag4)
		{
			m_alphaEased = new EasedFloatCubic(1f);
			m_alphaEased.EaseTo(1f, 0.1f);
		}
		int num2;
		if (!(gameFlowData.activeOwnedActorData == actor))
		{
			if (!(gameFlowData.activeOwnedActorData == null))
			{
				if (!(Board.Get().PlayerClampedSquare != null) || !(Board.Get().PlayerClampedSquare.OccupantActor == actor))
				{
					num2 = (flag4 ? 1 : 0);
					goto IL_0195;
				}
			}
		}
		num2 = 1;
		goto IL_0195;
		IL_06af:
		ClearLine(ref m_mousePotentialMovementLine);
		return;
		IL_0195:
		bool isClient = (byte)num2 != 0;
		RefreshLineVisibility(MovementLinesCanBeVisible(), isClient);
		if (flag4)
		{
			if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
			{
				m_alphaEased.EaseTo(0f, 1f);
			}
		}
		goto IL_01e4;
		IL_0272:
		int num3;
		if (HighlightUtils.Get().m_showMovementPreview && m_actor.GetAbilityData().GetSelectedAbility() == null)
		{
			if (GameFlowData.Get().IsInDecisionState() && !flag3)
			{
				if (GameFlowData.Get().activeOwnedActorData == m_actor && GameFlowData.Get().activeOwnedActorData != null)
				{
					num3 = ((!m_actor.IsDead()) ? 1 : 0);
					goto IL_0309;
				}
			}
		}
		num3 = 0;
		goto IL_0309;
		IL_0309:
		if (num3 != 0)
		{
			if (!flag)
			{
				if (flag2)
				{
					goto IL_06af;
				}
			}
			if (!(Board.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (!(GameFlowData.Get() != null) || !(Time.time - m_lastPreviewDrawnTime >= 0.1f))
				{
					return;
				}
				while (true)
				{
					BoardSquare currentBoardSquare = GameFlowData.Get().activeOwnedActorData.CurrentBoardSquare;
					if (Board.Get().PlayerClampedSquare != null)
					{
						if (currentBoardSquare != null)
						{
							while (true)
							{
								GridPos gridPos2;
								BoardSquare moveFromBoardSquare;
								GridPos gridPos3;
								int num4;
								int num5;
								switch (4)
								{
								case 0:
									break;
								default:
									{
										GridPos gridPos = currentBoardSquare.GetGridPos();
										gridPos2 = Board.Get().PlayerClampedSquare.GetGridPos();
										if (m_movementLine != null)
										{
											if (flag2)
											{
												moveFromBoardSquare = GameFlowData.Get().activeOwnedActorData.MoveFromBoardSquare;
												gridPos3 = moveFromBoardSquare.GetGridPos();
												if (m_lastDrawnStartPreview.x == gridPos3.x)
												{
													if (m_lastDrawnStartPreview.y == gridPos3.y && m_lastDrawnEndPreview.x == gridPos2.x)
													{
														num4 = ((m_lastDrawnEndPreview.y != gridPos2.y) ? 1 : 0);
														goto IL_0498;
													}
												}
												num4 = 1;
												goto IL_0498;
											}
										}
										if (m_lastDrawnStartPreview.x == gridPos.x)
										{
											if (m_lastDrawnStartPreview.y == gridPos.y)
											{
												if (m_lastDrawnEndPreview.x == gridPos2.x)
												{
													num5 = ((m_lastDrawnEndPreview.y != gridPos2.y) ? 1 : 0);
													goto IL_05e7;
												}
											}
										}
										num5 = 1;
										goto IL_05e7;
									}
									IL_05e7:
									if (num5 != 0)
									{
										BoardSquare boardSquare = Board.Get().PlayerClampedSquare;
										if (!actor.CanMoveToBoardSquare(boardSquare))
										{
											boardSquare = actor.GetActorMovement().GetClosestMoveableSquareTo(boardSquare, false);
										}
										BoardSquare initialMoveStartSquare = GameFlowData.Get().activeOwnedActorData.InitialMoveStartSquare;
										BoardSquarePathInfo path = m_actor.GetActorMovement().BuildPathTo(initialMoveStartSquare, boardSquare);
										SetMouseOverMovementLine(GetGridPosPathForPath(path, false, null), false, false);
										ShowLine(m_mousePotentialMovementLine, HighlightUtils.Get().m_movementLinePreviewColor, true, false);
										m_lastDrawnStartPreview = initialMoveStartSquare.GetGridPos();
										m_lastDrawnEndPreview = gridPos2;
										m_lastPreviewDrawnTime = Time.time;
									}
									return;
									IL_0498:
									if (num4 != 0)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												break;
											default:
												if (actor.RemainingHorizontalMovement > 0f)
												{
													while (true)
													{
														switch (6)
														{
														case 0:
															break;
														default:
														{
															BoardSquare boardSquare2 = Board.Get().PlayerClampedSquare;
															if (!actor.CanMoveToBoardSquare(boardSquare2))
															{
																boardSquare2 = actor.GetActorMovement().GetClosestMoveableSquareTo(boardSquare2, false);
															}
															BoardSquarePathInfo path2 = m_actor.GetActorMovement().BuildPathTo(moveFromBoardSquare, boardSquare2);
															SetMouseOverMovementLine(GetGridPosPathForPath(path2, false, null), false, false);
															ShowLine(m_mousePotentialMovementLine, HighlightUtils.Get().m_movementLinePreviewColor, true, false);
															m_lastDrawnStartPreview = gridPos3;
															m_lastDrawnEndPreview = gridPos2;
															m_lastPreviewDrawnTime = Time.time;
															return;
														}
														}
													}
												}
												ClearLine(ref m_mousePotentialMovementLine);
												return;
											}
										}
									}
									return;
								}
							}
						}
					}
					ClearLine(ref m_mousePotentialMovementLine);
					return;
				}
			}
		}
		goto IL_06af;
	}

	private void DisplayLineDataDebugInfo()
	{
		if (DebugParameters.Get() == null || !DebugParameters.Get().GetParameterAsBool("DisplayLineData"))
		{
			return;
		}
		while (true)
		{
			string text = string.Empty;
			if (m_movementLine != null)
			{
				text = text + "Movement:\n" + m_movementLine.ToString();
			}
			UIActorDebugPanel.Get().SetActorValue(m_actor, "DisplayLineData", text);
			return;
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
		if (!(SinglePlayerManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (SinglePlayerManager.Get().GetLocalPlayer() != null)
			{
				while (true)
				{
					SinglePlayerManager.Get().OnActorMovementChanged(SinglePlayerManager.Get().GetLocalPlayer());
					return;
				}
			}
			return;
		}
	}

	public static void SerializeLine(LineInstance line, NetworkWriter writer)
	{
		sbyte b = (sbyte)line.m_positions.Count;
		writer.Write(b);
		bool isChasing = line.isChasing;
		writer.Write(isChasing);
		byte value;
		byte value2;
		for (int i = 0; i < b; writer.Write(value), writer.Write(value2), i++)
		{
			int x = line.m_positions[i].x;
			int y = line.m_positions[i].y;
			value = 0;
			value2 = 0;
			if (x >= 0)
			{
				if (x <= 255)
				{
					value = (byte)x;
					goto IL_00b0;
				}
			}
			if (Application.isEditor)
			{
				Debug.LogError("Trying to serialize invalid grid pos x for LineData");
			}
			goto IL_00b0;
			IL_00b0:
			if (y >= 0)
			{
				if (y <= 255)
				{
					value2 = (byte)y;
					continue;
				}
			}
			if (Application.isEditor)
			{
				Debug.LogError("Trying to serialize invalid grid pos y for LineData");
			}
		}
		while (true)
		{
			switch (5)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public static LineInstance DeSerializeLine(NetworkReader reader)
	{
		LineInstance lineInstance = new LineInstance();
		sbyte b = reader.ReadSByte();
		bool flag = lineInstance.isChasing = reader.ReadBoolean();
		lineInstance.m_positions.Clear();
		for (int i = 0; i < b; i++)
		{
			byte x = reader.ReadByte();
			byte y = reader.ReadByte();
			GridPos item = default(GridPos);
			item.x = x;
			item.y = y;
			item.height = (int)Board.Get().GetSquareHeight(item.x, item.y);
			lineInstance.m_positions.Add(item);
		}
		while (true)
		{
			return lineInstance;
		}
	}

	public void OnDeSerializeNumNodesInSnaredLine(sbyte numNodesInSnaredLine)
	{
		if (numNodesInSnaredLine > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (m_movementLine != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
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
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
									{
										int num = Mathf.Max(0, numNodesInSnaredLine - 1);
										for (int j = num; j < m_movementLine.m_positions.Count; j++)
										{
											GridPos item2 = m_movementLine.m_positions[j];
											m_movementLostLine.m_positions.Add(item2);
										}
										while (true)
										{
											switch (4)
											{
											case 0:
												break;
											default:
												ChangeLineColor(m_movementLine, Color.red);
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
					return;
				}
			}
		}
		ClearLine(ref m_movementSnaredLine);
		if (m_movementLine != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					if (m_movementLostLine == null)
					{
						m_movementLostLine = new LineInstance();
					}
					m_movementLostLine.m_positions.Clear();
					m_movementLostLine.isChasing = m_movementLine.isChasing;
					for (int k = 0; k < m_movementLine.m_positions.Count; k++)
					{
						GridPos item3 = m_movementLine.m_positions[k];
						m_movementLostLine.m_positions.Add(item3);
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
			}
		}
		ClearLine(ref m_movementLostLine);
	}

	private void MarkForSerializationUpdate()
	{
	}

	private void GetMovementLineVisibilityFromStatus(float colorMult, out bool fullMovement, out bool snaredMovement, out bool showLostMovement, out Color fullPathColor, out Color snaredColor)
	{
		fullMovement = true;
		snaredMovement = false;
		showLostMovement = false;
		fullPathColor = (snaredColor = HighlightUtils.Get().s_defaultMovementColor * colorMult);
		ActorStatus actorStatus;
		AbilityData abilityData;
		bool flag2;
		if (m_actor != null && m_actor.GetActorStatus() != null)
		{
			actorStatus = m_actor.GetActorStatus();
			abilityData = m_actor.GetAbilityData();
			bool flag = actorStatus.IsMovementDebuffImmune();
			if (!flag)
			{
				flag = (abilityData != null && (abilityData.HasPendingStatusFromQueuedAbilities(StatusType.MovementDebuffImmunity) || abilityData.HasPendingStatusFromQueuedAbilities(StatusType.Unstoppable)));
			}
			int num;
			if (!actorStatus.HasStatus(StatusType.MovementDebuffSuppression, false))
			{
				num = ((!flag) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			flag2 = ((byte)num != 0);
			bool flag3 = actorStatus.HasStatus(StatusType.Rooted, false);
			bool flag4 = actorStatus.HasStatus(StatusType.KnockedBack, false);
			if (flag4)
			{
				fullPathColor.a = 0f;
				snaredColor.a = 0f;
			}
			if (!flag2)
			{
				goto IL_0179;
			}
			if (!flag3)
			{
				if (!flag4)
				{
					goto IL_0179;
				}
			}
			if (flag4)
			{
				fullMovement = false;
			}
			else if (flag3)
			{
				fullPathColor = HighlightUtils.Get().s_defaultLostMovementColor * colorMult;
			}
		}
		goto IL_0209;
		IL_0209:
		fullPathColor.a *= m_alphaEased;
		snaredColor.a *= m_alphaEased;
		return;
		IL_0179:
		if (flag2)
		{
			if (actorStatus.HasStatus(StatusType.Snared, false))
			{
				if (!actorStatus.HasStatus(StatusType.Hasted, false))
				{
					if (!(abilityData == null))
					{
						if (abilityData.HasPendingStatusFromQueuedAbilities(StatusType.Hasted))
						{
							goto IL_0209;
						}
					}
					snaredMovement = true;
					fullMovement = false;
					showLostMovement = true;
					fullPathColor = HighlightUtils.Get().s_defaultLostMovementColor * colorMult;
					snaredColor = HighlightUtils.Get().s_defaultMovementColor * colorMult;
				}
			}
		}
		goto IL_0209;
	}

	public void OnMovementStatusGained()
	{
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (GameFlowData.Get().gameState != GameState.BothTeams_Resolve)
			{
				return;
			}
			while (true)
			{
				if (GameFlowData.Get().activeOwnedActorData != null && GameFlowData.Get().activeOwnedActorData == m_actor)
				{
					m_alphaEased = new EasedFloatCubic(1f);
					m_alphaEased.EaseTo(0f, 3f);
				}
				return;
			}
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
		if (theLine == null)
		{
			return;
		}
		while (true)
		{
			if (theLine == m_movementLine)
			{
				HideMovementDestIndicator(m_movementLine);
			}
			theLine.m_positions.Clear();
			if (theLine.m_lineObject != null)
			{
				theLine.DestroyLineObject();
			}
			theLine = null;
			if (NetworkServer.active)
			{
				while (true)
				{
					MarkForSerializationUpdate();
					return;
				}
			}
			return;
		}
	}

	private void RefreshLineVisibility(bool visible, bool isClient)
	{
		float timeSinceChange = Time.time - m_lastAllyMovementChange;
		if (visible)
		{
			while (true)
			{
				float num;
				int num2;
				bool flag;
				bool fullMovement;
				bool snaredMovement;
				bool showLostMovement;
				Color fullPathColor;
				Color snaredColor;
				switch (3)
				{
				case 0:
					break;
				default:
					{
						num = 1f;
						if (!isClient)
						{
							num = AbilityUtil_Targeter.GetOpacityFromTargeterData(HighlightUtils.Get().m_movementLineOpacity, timeSinceChange);
						}
						if (m_movementLine != null)
						{
							if (m_movementSnaredLine == null)
							{
								num2 = (m_movementLine.isChasing ? 1 : 0);
								goto IL_0082;
							}
						}
						num2 = 0;
						goto IL_0082;
					}
					IL_0082:
					flag = ((byte)num2 != 0);
					fullMovement = false;
					snaredMovement = false;
					showLostMovement = false;
					GetMovementLineVisibilityFromStatus(num, out fullMovement, out snaredMovement, out showLostMovement, out fullPathColor, out snaredColor);
					if (!(fullPathColor.a > 0f))
					{
						goto IL_00e2;
					}
					if (!fullMovement)
					{
						if (!flag)
						{
							goto IL_00e2;
						}
					}
					ShowLine(m_movementLine, fullPathColor);
					ShowMovementDestIndicator(m_movementLine);
					goto IL_00fa;
					IL_0188:
					ShowLine(m_potentialMovementLine, HighlightUtils.Get().s_defaultPotentialMovementColor * num);
					if (fullMovement)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								if (m_potentialMovementLine != null)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											break;
										default:
											ShowMovementDestIndicator(m_potentialMovementLine);
											return;
										}
									}
								}
								return;
							}
						}
					}
					return;
					IL_00e2:
					HideLine(m_movementLine);
					HideMovementDestIndicator(m_movementLine);
					goto IL_00fa;
					IL_00fa:
					if (snaredColor.a > 0f && snaredMovement && !flag)
					{
						ShowLine(m_movementSnaredLine, snaredColor);
					}
					else
					{
						HideLine(m_movementSnaredLine);
					}
					if (fullPathColor.a > 0f)
					{
						if (showLostMovement)
						{
							if (!flag)
							{
								ShowLine(m_movementLostLine, fullPathColor, true);
								goto IL_0188;
							}
						}
					}
					HideLine(m_movementLostLine);
					goto IL_0188;
				}
			}
		}
		HideLine(m_movementLine);
		HideMovementDestIndicator(m_movementLine);
		HideLine(m_movementSnaredLine);
		HideLine(m_potentialMovementLine);
		HideLine(m_movementLostLine);
	}

	private void DrawLine(ref LineInstance theLine, float startOffset = 0.4f)
	{
		bool flag = false;
		ActorData actor = m_actor;
		GameFlowData gameFlowData = GameFlowData.Get();
		if ((bool)actor && gameFlowData != null)
		{
			if (gameFlowData.LocalPlayerData != null)
			{
				Team teamViewing = gameFlowData.LocalPlayerData.GetTeamViewing();
				int num;
				if (teamViewing != Team.Invalid)
				{
					num = ((teamViewing == actor.GetTeam()) ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				flag = ((byte)num != 0);
			}
		}
		if (theLine == null)
		{
			return;
		}
		while (true)
		{
			if (!flag)
			{
				return;
			}
			while (true)
			{
				Color color = GetColor(theLine.isChasing, false, actor == GameFlowData.Get().activeOwnedActorData);
				MovementPathStart previousLine = null;
				if (theLine.m_lineObject != null)
				{
					previousLine = theLine.m_movePathStart;
				}
				theLine.m_lineObject = CreateLineObject(theLine.m_positions, color, previousLine, theLine.isChasing, actor, startOffset);
				theLine.m_currentColor = color;
				LineInstance obj = theLine;
				object movePathStart;
				if (theLine.m_lineObject != null)
				{
					movePathStart = theLine.m_lineObject.GetComponentInChildren<MovementPathStart>(true);
				}
				else
				{
					movePathStart = null;
				}
				obj.m_movePathStart = (MovementPathStart)movePathStart;
				if (!MovementLinesCanBeVisible())
				{
					HideLine(theLine);
				}
				return;
			}
		}
	}

	private void ActivatePotentialMovementLine()
	{
		ActorData actor = m_actor;
		GameFlowData gameFlowData = GameFlowData.Get();
		if (m_potentialMovementLine == null)
		{
			return;
		}
		while (true)
		{
			if (!actor)
			{
				return;
			}
			while (true)
			{
				if (!gameFlowData)
				{
					return;
				}
				while (true)
				{
					if (!(gameFlowData.activeOwnedActorData == actor))
					{
						return;
					}
					while (true)
					{
						Color color = GetColor(m_potentialMovementLine.isChasing, true, true);
						MovementPathStart previousLine = null;
						if (m_potentialMovementLine.m_lineObject != null)
						{
							previousLine = m_potentialMovementLine.m_movePathStart;
						}
						m_potentialMovementLine.m_lineObject = CreateLineObject(m_potentialMovementLine.m_positions, color, previousLine, m_potentialMovementLine.isChasing, actor);
						m_potentialMovementLine.m_currentColor = color;
						LineInstance potentialMovementLine = m_potentialMovementLine;
						object movePathStart;
						if (m_potentialMovementLine.m_lineObject != null)
						{
							movePathStart = m_potentialMovementLine.m_lineObject.GetComponentInChildren<MovementPathStart>(true);
						}
						else
						{
							movePathStart = null;
						}
						potentialMovementLine.m_movePathStart = (MovementPathStart)movePathStart;
						if (!MovementLinesCanBeVisible())
						{
							while (true)
							{
								HideLine(m_potentialMovementLine);
								return;
							}
						}
						return;
					}
				}
			}
		}
	}

	private GameObject CreateLineObject(List<GridPos> targetSquares, Color lineColor, MovementPathStart previousLine, bool isChasing, ActorData theActor, float startOffset = 0.4f)
	{
		if (targetSquares.Count < 2)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		m_lastAllyMovementChange = Time.time;
		List<Vector3> points = new List<Vector3>();
		for (int i = 0; i < targetSquares.Count; i++)
		{
			Vector3 worldPosition = Board.Get().GetSquare(targetSquares[i]).GetWorldPosition();
			points.Add(new Vector3(worldPosition.x, worldPosition.y + 0.1f, worldPosition.z));
		}
		while (true)
		{
			return Targeting.GetTargeting().CreateFancyArrowMesh(ref points, 0.2f, lineColor, isChasing, theActor, AbilityUtil_Targeter.TargeterMovementType.Movement, null, previousLine, true, startOffset);
		}
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
			list = new List<GridPos>();
			list.Add(m_actor.MoveFromBoardSquare.GetGridPos());
			list.Add(chaseTarget.GetCurrentBoardSquare().GetGridPos());
		}
		else if (path != null)
		{
			list = path.ToGridPosPath();
		}
		return list;
	}

	public void OnMovementChanged(List<GridPos> fullPathInfo, List<GridPos> snaredPathInfo, bool isChasing, bool rebuildLine)
	{
	}

	public void OnResolveStart()
	{
		HideAllyMovementLinesIfResolving();
	}

	private void HideAllyMovementLinesIfResolving()
	{
		int num;
		if (!(GameFlowData.Get() == null))
		{
			if (!(m_actor == GameFlowData.Get().activeOwnedActorData))
			{
				num = ((GameFlowData.Get().gameState != GameState.BothTeams_Resolve) ? 1 : 0);
				goto IL_005c;
			}
		}
		num = 1;
		goto IL_005c;
		IL_005c:
		if (num == 0)
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
		if (theLine == null)
		{
			return;
		}
		while (true)
		{
			if (!(theLine.m_lineObject != null) || !(theLine.m_currentColor != newColor))
			{
				return;
			}
			while (true)
			{
				theLine.m_currentColor = newColor;
				MeshRenderer[] componentsInChildren = theLine.m_lineObject.GetComponentsInChildren<MeshRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (componentsInChildren[i].materials.Length > 0)
					{
						componentsInChildren[i].materials[0].SetColor("_TintColor", newColor);
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

	private void ShowLine(LineInstance theLine, Color lineColor, bool hideStart = false, bool setGlow = true)
	{
		ChangeLineColor(theLine, lineColor);
		if (theLine == null)
		{
			return;
		}
		while (true)
		{
			if (!(theLine.m_lineObject != null))
			{
				return;
			}
			while (true)
			{
				MovementPathStart movePathStart = theLine.m_movePathStart;
				if (!(movePathStart != null))
				{
					return;
				}
				movePathStart.SetGlow(setGlow);
				if (hideStart)
				{
					while (true)
					{
						movePathStart.m_movementContainer.SetActive(false);
						return;
					}
				}
				return;
			}
		}
	}

	private void HideLine(LineInstance theLine)
	{
		ChangeLineColor(theLine, Color.clear);
		if (theLine == null || !(theLine.m_lineObject != null))
		{
			return;
		}
		while (true)
		{
			MovementPathStart movePathStart = theLine.m_movePathStart;
			if (movePathStart != null)
			{
				movePathStart.SetGlow(false);
			}
			return;
		}
	}

	private void ShowMovementDestIndicator(LineInstance inst)
	{
		if (inst == null)
		{
			return;
		}
		while (true)
		{
			if (!(inst.m_lineObject != null) || !(inst.m_movePathStart != null))
			{
				return;
			}
			while (true)
			{
				if (inst.m_positions.Count <= 0)
				{
					return;
				}
				while (true)
				{
					MovementPathStart movePathStart = inst.m_movePathStart;
					GridPos gridPos = inst.m_positions[inst.m_positions.Count - 1];
					BoardSquare boardSquareSafe = Board.Get().GetSquare(gridPos);
					if (movePathStart != null)
					{
						movePathStart.SetCharacterMovementPanel(boardSquareSafe);
					}
					return;
				}
			}
		}
	}

	private void HideMovementDestIndicator(LineInstance inst)
	{
		if (inst == null)
		{
			return;
		}
		while (true)
		{
			if (inst.m_lineObject != null)
			{
				MovementPathStart movePathStart = inst.m_movePathStart;
				if (movePathStart != null)
				{
					while (true)
					{
						movePathStart.HideCharacterMovementPanel();
						return;
					}
				}
				return;
			}
			return;
		}
	}

	private void SetMovementSnaredLine(List<GridPos> gridPosPath, bool isChasing, bool rebuildLine)
	{
		if (!rebuildLine)
		{
			if (!isChasing)
			{
				goto IL_002f;
			}
		}
		ClearLine(ref m_movementSnaredLine);
		goto IL_002f;
		IL_0159:
		MarkForSerializationUpdate();
		return;
		IL_002f:
		if (gridPosPath.Count > 1)
		{
			if (!rebuildLine)
			{
				if (m_movementSnaredLine != null)
				{
					goto IL_0071;
				}
			}
			m_movementSnaredLine = new LineInstance();
			goto IL_0071;
		}
		if (gridPosPath.Count == 1)
		{
			if (m_movementLine != null)
			{
				if (m_movementLine.m_lineObject != null)
				{
					MovementPathStart movePathStart = m_movementLine.m_movePathStart;
					if (movePathStart != null)
					{
						movePathStart.SetGlow(false);
					}
				}
			}
		}
		goto IL_0159;
		IL_0071:
		m_movementSnaredLine.m_positions = gridPosPath;
		m_movementSnaredLine.isChasing = isChasing;
		DrawLine(ref m_movementSnaredLine);
		if (m_movementLine != null)
		{
			if (m_movementLine.m_lineObject != null)
			{
				MovementPathStart movePathStart2 = m_movementLine.m_movePathStart;
				if (movePathStart2 != null)
				{
					movePathStart2.SetGlow(false);
				}
			}
		}
		goto IL_0159;
	}

	private void SetMovementLine(List<GridPos> gridPosPath, bool isChasing, bool rebuildLine)
	{
		if (!rebuildLine)
		{
			if (!isChasing)
			{
				goto IL_0025;
			}
		}
		ClearLine(ref m_movementLine);
		goto IL_0025;
		IL_005a:
		m_movementLine.m_positions = gridPosPath;
		m_movementLine.isChasing = isChasing;
		DrawLine(ref m_movementLine);
		goto IL_0083;
		IL_0025:
		if (gridPosPath.Count > 1)
		{
			if (!rebuildLine)
			{
				if (m_movementLine != null)
				{
					goto IL_005a;
				}
			}
			m_movementLine = new LineInstance();
			goto IL_005a;
		}
		goto IL_0083;
		IL_0083:
		MarkForSerializationUpdate();
	}

	private void SetMouseOverMovementLine(List<GridPos> gridPosPath, bool isChasing, bool rebuildLine)
	{
		if (!rebuildLine)
		{
			if (!isChasing)
			{
				goto IL_0025;
			}
		}
		ClearLine(ref m_mousePotentialMovementLine);
		goto IL_0025;
		IL_0059:
		m_mousePotentialMovementLine.m_positions = gridPosPath;
		m_mousePotentialMovementLine.isChasing = isChasing;
		DrawLine(ref m_mousePotentialMovementLine);
		if (!(m_mousePotentialMovementLine.m_lineObject != null))
		{
			return;
		}
		MovementPathStart movePathStart = m_mousePotentialMovementLine.m_movePathStart;
		if (!(movePathStart != null))
		{
			return;
		}
		while (true)
		{
			movePathStart.SetGlow(false);
			return;
		}
		IL_0025:
		if (gridPosPath != null && gridPosPath.Count > 1)
		{
			if (!rebuildLine)
			{
				if (m_mousePotentialMovementLine != null)
				{
					goto IL_0059;
				}
			}
			m_mousePotentialMovementLine = new LineInstance();
			goto IL_0059;
		}
		if (m_mousePotentialMovementLine == null)
		{
			return;
		}
		while (true)
		{
			ClearLine(ref m_mousePotentialMovementLine);
			return;
		}
	}

	private void SetMovementLostLine(List<GridPos> fullGridPosPath, int numSnaredNodes, bool isChasing, bool rebuildLine)
	{
		if (!rebuildLine)
		{
			if (!isChasing)
			{
				goto IL_0026;
			}
		}
		ClearLine(ref m_movementLostLine);
		goto IL_0026;
		IL_0026:
		List<GridPos> list = new List<GridPos>();
		int num = Mathf.Max(0, numSnaredNodes - 1);
		for (int i = num; i < fullGridPosPath.Count; i++)
		{
			list.Add(fullGridPosPath[i]);
		}
		while (true)
		{
			if (list.Count > 1)
			{
				if (!rebuildLine)
				{
					if (m_movementLostLine != null)
					{
						goto IL_0096;
					}
				}
				m_movementLostLine = new LineInstance();
				goto IL_0096;
			}
			goto IL_00bf;
			IL_00bf:
			MarkForSerializationUpdate();
			return;
			IL_0096:
			m_movementLostLine.m_positions = list;
			m_movementLostLine.isChasing = isChasing;
			DrawLine(ref m_movementLostLine, 0.2f);
			goto IL_00bf;
		}
	}

	private void SetPotentialMovementLine(List<GridPos> gridPosPath, bool isChasing)
	{
		ClearLine(ref m_potentialMovementLine);
		if (gridPosPath.Count <= 1)
		{
			return;
		}
		while (true)
		{
			m_potentialMovementLine = new LineInstance();
			m_potentialMovementLine.m_positions = gridPosPath;
			m_potentialMovementLine.isChasing = isChasing;
			ActivatePotentialMovementLine();
			return;
		}
	}

	public List<GridPos> GetGridPosPath()
	{
		if (m_movementLine != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_movementLine.m_positions;
				}
			}
		}
		if (m_potentialMovementLine != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_potentialMovementLine.m_positions;
				}
			}
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

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
