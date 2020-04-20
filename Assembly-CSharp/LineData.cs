﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LineData : NetworkBehaviour, IGameEventListener
{
	private static bool MovementLinesCanBeVisibleForHud = true;

	private LineData.LineInstance m_movementLine;

	private LineData.LineInstance m_movementSnaredLine;

	private LineData.LineInstance m_movementLostLine;

	private LineData.LineInstance m_mousePotentialMovementLine;

	private GridPos m_lastDrawnStartPreview;

	private GridPos m_lastDrawnEndPreview;

	private float m_lastPreviewDrawnTime;

	private LineData.LineInstance m_potentialMovementLine;

	private ActorData m_actor;

	public float m_lastAllyMovementChange;

	private EasedFloatCubic m_alphaEased = new EasedFloatCubic(1f);

	private bool m_waitingForNextMoveSquaresUpdate;

	private float m_lastMoveSquareUpdatedTime = -1f;

	private static bool MovementLinesCanBeVisible()
	{
		if (LineData.MovementLinesCanBeVisibleForHud)
		{
			if (!LineData.SpectatorHideMovementLines)
			{
				return true;
			}
		}
		if (!LineData.MovementLinesCanBeVisibleForHud)
		{
			return false;
		}
		bool flag;
		if (ClientGameManager.Get() != null)
		{
			if (ClientGameManager.Get().PlayerInfo != null)
			{
				flag = ClientGameManager.Get().PlayerInfo.IsSpectator;
				goto IL_85;
			}
		}
		flag = false;
		IL_85:
		bool flag2 = flag;
		bool result;
		if (flag2)
		{
			result = !LineData.SpectatorHideMovementLines;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public static void SetAllowMovementLinesVisibleForHud(bool visible)
	{
		LineData.MovementLinesCanBeVisibleForHud = visible;
		if (GameFlowData.Get() != null)
		{
			List<ActorData> actors = GameFlowData.Get().GetActors();
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					LineData component = actorData.GetComponent<LineData>();
					if (component != null)
					{
						component.m_lastAllyMovementChange = Time.time;
						component.RefreshLineVisibility(LineData.MovementLinesCanBeVisible(), actorData == GameFlowData.Get().activeOwnedActorData);
					}
				}
			}
		}
	}

	public static bool SpectatorHideMovementLines { get; set; }

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
		this.m_actor = base.GetComponent<ActorData>();
	}

	private void Start()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedDecision);
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.UIPhaseStartedPrep);
		}
		GameFlowData.s_onGameStateChanged += this.OnGameStateChanged;
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedDecision);
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.UIPhaseStartedPrep);
		}
		GameFlowData.s_onGameStateChanged -= this.OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState newState)
	{
		if (newState == GameState.BothTeams_Decision)
		{
			this.m_alphaEased.EaseTo(1f, 0.01f);
		}
	}

	public void OnClientRequestedMovementChange()
	{
		if (Time.time > this.m_lastMoveSquareUpdatedTime)
		{
			this.m_waitingForNextMoveSquaresUpdate = true;
			this.ClearMovementPreviewLine();
		}
	}

	public void OnCanMoveToSquaresUpdated()
	{
		this.m_waitingForNextMoveSquaresUpdate = false;
		this.m_lastMoveSquareUpdatedTime = Time.time;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.UIPhaseStartedDecision)
		{
			this.OnMovementPhaseEnd();
			this.m_alphaEased.EaseTo(1f, 0.01f);
		}
		if (eventType == GameEventManager.EventType.UIPhaseStartedPrep)
		{
			this.m_alphaEased.EaseTo(0f, 2f);
		}
	}

	private void Update()
	{
		this.DisplayLineDataDebugInfo();
		ActorData actor = this.m_actor;
		GameFlowData gameFlowData = GameFlowData.Get();
		ActorTurnSM actorTurnSM = actor.GetActorTurnSM();
		if (this.m_potentialMovementLine != null)
		{
			InterfaceManager exists = InterfaceManager.Get();
			if (gameFlowData)
			{
				if (exists)
				{
					if (gameFlowData.activeOwnedActorData == actor)
					{
						if (actorTurnSM.CurrentState == TurnStateEnum.DECIDING_MOVEMENT)
						{
							if (!actor.HasQueuedMovement())
							{
								goto IL_AE;
							}
						}
						this.ClearLine(ref this.m_potentialMovementLine);
					}
				}
			}
		}
		IL_AE:
		if (this.m_movementLine == null)
		{
			if (this.m_movementSnaredLine == null)
			{
				if (this.m_potentialMovementLine == null)
				{
					goto IL_1E4;
				}
			}
		}
		bool flag = InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo);
		if (flag)
		{
			this.m_alphaEased = new EasedFloatCubic(1f);
			this.m_alphaEased.EaseTo(1f, 0.1f);
		}
		bool flag2;
		if (!(gameFlowData.activeOwnedActorData == actor))
		{
			if (!(gameFlowData.activeOwnedActorData == null))
			{
				if (!(Board.Get().PlayerClampedSquare != null) || !(Board.Get().PlayerClampedSquare.OccupantActor == actor))
				{
					flag2 = flag;
					goto IL_195;
				}
			}
		}
		flag2 = true;
		IL_195:
		bool isClient = flag2;
		this.RefreshLineVisibility(LineData.MovementLinesCanBeVisible(), isClient);
		if (flag)
		{
			if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
			{
				this.m_alphaEased.EaseTo(0f, 1f);
			}
		}
		IL_1E4:
		bool flag3 = LineData.MovementLinesCanBeVisible();
		bool flag4;
		if (Options_UI.Get() != null)
		{
			flag4 = (Options_UI.Get().GetShiftClickForMovementWaypoints() == InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier));
		}
		else
		{
			flag4 = false;
		}
		bool flag5 = flag4;
		bool flag6 = this.m_waitingForNextMoveSquaresUpdate;
		if (actorTurnSM != null)
		{
			if (actorTurnSM.CurrentState != TurnStateEnum.VALIDATING_MOVE_REQUEST)
			{
				if (actorTurnSM.CurrentState != TurnStateEnum.VALIDATING_ACTION_REQUEST)
				{
					goto IL_272;
				}
			}
			flag6 = true;
		}
		IL_272:
		bool flag7;
		if (HighlightUtils.Get().m_showMovementPreview && this.m_actor.GetAbilityData().GetSelectedAbility() == null)
		{
			if (GameFlowData.Get().IsInDecisionState() && !flag6)
			{
				if (GameFlowData.Get().activeOwnedActorData == this.m_actor && GameFlowData.Get().activeOwnedActorData != null)
				{
					flag7 = !this.m_actor.IsDead();
					goto IL_309;
				}
			}
		}
		flag7 = false;
		IL_309:
		bool flag8 = flag7;
		if (flag8)
		{
			if (!flag3)
			{
				if (flag5)
				{
					goto IL_6AF;
				}
			}
			if (Board.Get() != null)
			{
				if (GameFlowData.Get() != null && Time.time - this.m_lastPreviewDrawnTime >= 0.1f)
				{
					BoardSquare currentBoardSquare = GameFlowData.Get().activeOwnedActorData.CurrentBoardSquare;
					if (Board.Get().PlayerClampedSquare != null)
					{
						if (currentBoardSquare != null)
						{
							GridPos gridPos = currentBoardSquare.GetGridPos();
							GridPos gridPos2 = Board.Get().PlayerClampedSquare.GetGridPos();
							if (this.m_movementLine != null)
							{
								if (flag5)
								{
									BoardSquare moveFromBoardSquare = GameFlowData.Get().activeOwnedActorData.MoveFromBoardSquare;
									GridPos gridPos3 = moveFromBoardSquare.GetGridPos();
									bool flag9;
									if (this.m_lastDrawnStartPreview.x == gridPos3.x)
									{
										if (this.m_lastDrawnStartPreview.y == gridPos3.y && this.m_lastDrawnEndPreview.x == gridPos2.x)
										{
											flag9 = (this.m_lastDrawnEndPreview.y != gridPos2.y);
											goto IL_498;
										}
									}
									flag9 = true;
									IL_498:
									bool flag10 = flag9;
									if (flag10)
									{
										if (actor.RemainingHorizontalMovement > 0f)
										{
											BoardSquare boardSquare = Board.Get().PlayerClampedSquare;
											if (!actor.CanMoveToBoardSquare(boardSquare))
											{
												boardSquare = actor.GetActorMovement().GetClosestMoveableSquareTo(boardSquare, false);
											}
											BoardSquarePathInfo path = this.m_actor.GetActorMovement().BuildPathTo(moveFromBoardSquare, boardSquare);
											this.SetMouseOverMovementLine(this.GetGridPosPathForPath(path, false, null), false, false);
											this.ShowLine(this.m_mousePotentialMovementLine, HighlightUtils.Get().m_movementLinePreviewColor, true, false);
											this.m_lastDrawnStartPreview = gridPos3;
											this.m_lastDrawnEndPreview = gridPos2;
											this.m_lastPreviewDrawnTime = Time.time;
										}
										else
										{
											this.ClearLine(ref this.m_mousePotentialMovementLine);
										}
									}
									goto IL_69F;
								}
							}
							bool flag11;
							if (this.m_lastDrawnStartPreview.x == gridPos.x)
							{
								if (this.m_lastDrawnStartPreview.y == gridPos.y)
								{
									if (this.m_lastDrawnEndPreview.x == gridPos2.x)
									{
										flag11 = (this.m_lastDrawnEndPreview.y != gridPos2.y);
										goto IL_5E7;
									}
								}
							}
							flag11 = true;
							IL_5E7:
							bool flag12 = flag11;
							if (flag12)
							{
								BoardSquare boardSquare2 = Board.Get().PlayerClampedSquare;
								if (!actor.CanMoveToBoardSquare(boardSquare2))
								{
									boardSquare2 = actor.GetActorMovement().GetClosestMoveableSquareTo(boardSquare2, false);
								}
								BoardSquare initialMoveStartSquare = GameFlowData.Get().activeOwnedActorData.InitialMoveStartSquare;
								BoardSquarePathInfo path2 = this.m_actor.GetActorMovement().BuildPathTo(initialMoveStartSquare, boardSquare2);
								this.SetMouseOverMovementLine(this.GetGridPosPathForPath(path2, false, null), false, false);
								this.ShowLine(this.m_mousePotentialMovementLine, HighlightUtils.Get().m_movementLinePreviewColor, true, false);
								this.m_lastDrawnStartPreview = initialMoveStartSquare.GetGridPos();
								this.m_lastDrawnEndPreview = gridPos2;
								this.m_lastPreviewDrawnTime = Time.time;
							}
							IL_69F:
							goto IL_6AD;
						}
					}
					this.ClearLine(ref this.m_mousePotentialMovementLine);
				}
			}
			IL_6AD:
			return;
		}
		IL_6AF:
		this.ClearLine(ref this.m_mousePotentialMovementLine);
	}

	private void DisplayLineDataDebugInfo()
	{
		if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("DisplayLineData"))
		{
			string text = string.Empty;
			if (this.m_movementLine != null)
			{
				text = text + "Movement:\n" + this.m_movementLine.ToString();
			}
			UIActorDebugPanel.Get().SetActorValue(this.m_actor, "DisplayLineData", text);
		}
	}

	public void OnDeserializedData(LineData.LineInstance movementLine, sbyte numNodesInSnaredLine)
	{
		this.ClearAllLines();
		this.m_movementLine = movementLine;
		this.OnDeSerializeNumNodesInSnaredLine(numNodesInSnaredLine);
		this.DrawLine(ref this.m_movementSnaredLine, 0.4f);
		this.DrawLine(ref this.m_movementLine, 0.4f);
		this.DrawLine(ref this.m_movementLostLine, 0.2f);
		this.HideAllyMovementLinesIfResolving();
		if (SinglePlayerManager.Get() != null)
		{
			if (SinglePlayerManager.Get().GetLocalPlayer() != null)
			{
				SinglePlayerManager.Get().OnActorMovementChanged(SinglePlayerManager.Get().GetLocalPlayer());
			}
		}
	}

	public static void SerializeLine(LineData.LineInstance line, NetworkWriter writer)
	{
		sbyte b = (sbyte)line.m_positions.Count;
		writer.Write(b);
		bool isChasing = line.isChasing;
		writer.Write(isChasing);
		int i = 0;
		while (i < (int)b)
		{
			int x = line.m_positions[i].x;
			int y = line.m_positions[i].y;
			byte value = 0;
			byte value2 = 0;
			if (x < 0)
			{
				goto IL_93;
			}
			if (x > 0xFF)
			{
				goto IL_93;
			}
			value = (byte)x;
			IL_B0:
			if (y < 0)
			{
				goto IL_CF;
			}
			if (y > 0xFF)
			{
				goto IL_CF;
			}
			value2 = (byte)y;
			IL_EC:
			writer.Write(value);
			writer.Write(value2);
			i++;
			continue;
			IL_CF:
			if (Application.isEditor)
			{
				Debug.LogError("Trying to serialize invalid grid pos y for LineData");
				goto IL_EC;
			}
			goto IL_EC;
			IL_93:
			if (Application.isEditor)
			{
				Debug.LogError("Trying to serialize invalid grid pos x for LineData");
				goto IL_B0;
			}
			goto IL_B0;
		}
	}

	public static LineData.LineInstance DeSerializeLine(NetworkReader reader)
	{
		LineData.LineInstance lineInstance = new LineData.LineInstance();
		sbyte b = reader.ReadSByte();
		bool isChasing = reader.ReadBoolean();
		lineInstance.isChasing = isChasing;
		lineInstance.m_positions.Clear();
		for (int i = 0; i < (int)b; i++)
		{
			byte x = reader.ReadByte();
			byte y = reader.ReadByte();
			GridPos item = default(GridPos);
			item.x = (int)x;
			item.y = (int)y;
			item.height = (int)Board.Get().GetSquareHeight(item.x, item.y);
			lineInstance.m_positions.Add(item);
		}
		return lineInstance;
	}

	public void OnDeSerializeNumNodesInSnaredLine(sbyte numNodesInSnaredLine)
	{
		if ((int)numNodesInSnaredLine > 0)
		{
			if (this.m_movementLine != null)
			{
				numNodesInSnaredLine = (sbyte)Mathf.Min((int)numNodesInSnaredLine, (int)((sbyte)this.m_movementLine.m_positions.Count));
				if (this.m_movementSnaredLine == null)
				{
					this.m_movementSnaredLine = new LineData.LineInstance();
				}
				if (this.m_movementLostLine == null)
				{
					this.m_movementLostLine = new LineData.LineInstance();
				}
				this.m_movementSnaredLine.m_positions.Clear();
				this.m_movementSnaredLine.isChasing = this.m_movementLine.isChasing;
				this.m_movementLostLine.m_positions.Clear();
				this.m_movementLostLine.isChasing = this.m_movementLine.isChasing;
				for (int i = 0; i < (int)numNodesInSnaredLine; i++)
				{
					GridPos item = this.m_movementLine.m_positions[i];
					this.m_movementSnaredLine.m_positions.Add(item);
				}
				int num = Mathf.Max(0, (int)numNodesInSnaredLine - 1);
				for (int j = num; j < this.m_movementLine.m_positions.Count; j++)
				{
					GridPos item2 = this.m_movementLine.m_positions[j];
					this.m_movementLostLine.m_positions.Add(item2);
				}
				this.ChangeLineColor(this.m_movementLine, Color.red);
			}
		}
		else
		{
			this.ClearLine(ref this.m_movementSnaredLine);
			if (this.m_movementLine != null)
			{
				if (this.m_movementLostLine == null)
				{
					this.m_movementLostLine = new LineData.LineInstance();
				}
				this.m_movementLostLine.m_positions.Clear();
				this.m_movementLostLine.isChasing = this.m_movementLine.isChasing;
				for (int k = 0; k < this.m_movementLine.m_positions.Count; k++)
				{
					GridPos item3 = this.m_movementLine.m_positions[k];
					this.m_movementLostLine.m_positions.Add(item3);
				}
			}
			else
			{
				this.ClearLine(ref this.m_movementLostLine);
			}
		}
	}

	private void MarkForSerializationUpdate()
	{
	}

	private unsafe void GetMovementLineVisibilityFromStatus(float colorMult, out bool fullMovement, out bool snaredMovement, out bool showLostMovement, out Color fullPathColor, out Color snaredColor)
	{
		fullMovement = true;
		snaredMovement = false;
		showLostMovement = false;
		fullPathColor = (snaredColor = HighlightUtils.Get().s_defaultMovementColor * colorMult);
		if (this.m_actor != null && this.m_actor.GetActorStatus() != null)
		{
			ActorStatus actorStatus = this.m_actor.GetActorStatus();
			AbilityData abilityData = this.m_actor.GetAbilityData();
			bool flag = actorStatus.IsMovementDebuffImmune(true);
			if (!flag)
			{
				flag = (abilityData != null && (abilityData.HasPendingStatusFromQueuedAbilities(StatusType.MovementDebuffImmunity) || abilityData.HasPendingStatusFromQueuedAbilities(StatusType.Unstoppable)));
			}
			bool flag2;
			if (!actorStatus.HasStatus(StatusType.MovementDebuffSuppression, false))
			{
				flag2 = !flag;
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			bool flag4 = actorStatus.HasStatus(StatusType.Rooted, false);
			bool flag5 = actorStatus.HasStatus(StatusType.KnockedBack, false);
			if (flag5)
			{
				fullPathColor.a = 0f;
				snaredColor.a = 0f;
			}
			if (flag3)
			{
				if (!flag4)
				{
					if (!flag5)
					{
						goto IL_179;
					}
				}
				if (flag5)
				{
					fullMovement = false;
				}
				else if (flag4)
				{
					fullPathColor = HighlightUtils.Get().s_defaultLostMovementColor * colorMult;
				}
				goto IL_209;
			}
			IL_179:
			if (flag3)
			{
				if (actorStatus.HasStatus(StatusType.Snared, false))
				{
					if (!actorStatus.HasStatus(StatusType.Hasted, false))
					{
						if (!(abilityData == null))
						{
							if (abilityData.HasPendingStatusFromQueuedAbilities(StatusType.Hasted))
							{
								goto IL_209;
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
		}
		IL_209:
		fullPathColor.a *= this.m_alphaEased;
		snaredColor.a *= this.m_alphaEased;
	}

	public void OnMovementStatusGained()
	{
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
			{
				if (GameFlowData.Get().activeOwnedActorData != null && GameFlowData.Get().activeOwnedActorData == this.m_actor)
				{
					this.m_alphaEased = new EasedFloatCubic(1f);
					this.m_alphaEased.EaseTo(0f, 3f);
				}
			}
		}
	}

	public void ClearAllLines()
	{
		this.ClearLine(ref this.m_movementLine);
		this.ClearLine(ref this.m_movementSnaredLine);
		this.ClearLine(ref this.m_movementLostLine);
		this.ClearMovementPreviewLine();
	}

	private void ClearMovementPreviewLine()
	{
		this.ClearLine(ref this.m_potentialMovementLine);
		this.m_lastPreviewDrawnTime = Time.time;
		this.m_lastDrawnStartPreview = GridPos.s_invalid;
		this.m_lastDrawnEndPreview = GridPos.s_invalid;
	}

	private unsafe void ClearLine(ref LineData.LineInstance theLine)
	{
		if (theLine != null)
		{
			if (theLine == this.m_movementLine)
			{
				this.HideMovementDestIndicator(this.m_movementLine);
			}
			theLine.m_positions.Clear();
			if (theLine.m_lineObject != null)
			{
				theLine.DestroyLineObject();
			}
			theLine = null;
			if (NetworkServer.active)
			{
				this.MarkForSerializationUpdate();
			}
		}
	}

	private void RefreshLineVisibility(bool visible, bool isClient)
	{
		float timeSinceChange = Time.time - this.m_lastAllyMovementChange;
		if (visible)
		{
			float num = 1f;
			if (!isClient)
			{
				num = AbilityUtil_Targeter.GetOpacityFromTargeterData(HighlightUtils.Get().m_movementLineOpacity, timeSinceChange);
			}
			bool flag;
			if (this.m_movementLine != null)
			{
				if (this.m_movementSnaredLine == null)
				{
					flag = this.m_movementLine.isChasing;
					goto IL_82;
				}
			}
			flag = false;
			IL_82:
			bool flag2 = flag;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			Color lineColor;
			Color lineColor2;
			this.GetMovementLineVisibilityFromStatus(num, out flag3, out flag4, out flag5, out lineColor, out lineColor2);
			if (lineColor.a > 0f)
			{
				if (!flag3)
				{
					if (!flag2)
					{
						goto IL_E2;
					}
				}
				this.ShowLine(this.m_movementLine, lineColor, false, true);
				this.ShowMovementDestIndicator(this.m_movementLine);
				goto IL_FA;
			}
			IL_E2:
			this.HideLine(this.m_movementLine);
			this.HideMovementDestIndicator(this.m_movementLine);
			IL_FA:
			if (lineColor2.a > 0f && flag4 && !flag2)
			{
				this.ShowLine(this.m_movementSnaredLine, lineColor2, false, true);
			}
			else
			{
				this.HideLine(this.m_movementSnaredLine);
			}
			if (lineColor.a > 0f)
			{
				if (flag5)
				{
					if (!flag2)
					{
						this.ShowLine(this.m_movementLostLine, lineColor, true, true);
						goto IL_188;
					}
				}
			}
			this.HideLine(this.m_movementLostLine);
			IL_188:
			this.ShowLine(this.m_potentialMovementLine, HighlightUtils.Get().s_defaultPotentialMovementColor * num, false, true);
			if (flag3)
			{
				if (this.m_potentialMovementLine != null)
				{
					this.ShowMovementDestIndicator(this.m_potentialMovementLine);
				}
			}
		}
		else
		{
			this.HideLine(this.m_movementLine);
			this.HideMovementDestIndicator(this.m_movementLine);
			this.HideLine(this.m_movementSnaredLine);
			this.HideLine(this.m_potentialMovementLine);
			this.HideLine(this.m_movementLostLine);
		}
	}

	private unsafe void DrawLine(ref LineData.LineInstance theLine, float startOffset = 0.4f)
	{
		bool flag = false;
		ActorData actor = this.m_actor;
		GameFlowData gameFlowData = GameFlowData.Get();
		if (actor && gameFlowData != null)
		{
			if (gameFlowData.LocalPlayerData != null)
			{
				Team teamViewing = gameFlowData.LocalPlayerData.GetTeamViewing();
				bool flag2;
				if (teamViewing != Team.Invalid)
				{
					flag2 = (teamViewing == actor.GetTeam());
				}
				else
				{
					flag2 = true;
				}
				flag = flag2;
			}
		}
		if (theLine != null)
		{
			if (flag)
			{
				Color color = LineData.GetColor(theLine.isChasing, false, actor == GameFlowData.Get().activeOwnedActorData);
				MovementPathStart previousLine = null;
				if (theLine.m_lineObject != null)
				{
					previousLine = theLine.m_movePathStart;
				}
				theLine.m_lineObject = this.CreateLineObject(theLine.m_positions, color, previousLine, theLine.isChasing, actor, startOffset);
				theLine.m_currentColor = color;
				LineData.LineInstance lineInstance = theLine;
				MovementPathStart movePathStart;
				if (theLine.m_lineObject != null)
				{
					movePathStart = theLine.m_lineObject.GetComponentInChildren<MovementPathStart>(true);
				}
				else
				{
					movePathStart = null;
				}
				lineInstance.m_movePathStart = movePathStart;
				if (!LineData.MovementLinesCanBeVisible())
				{
					this.HideLine(theLine);
				}
			}
		}
	}

	private void ActivatePotentialMovementLine()
	{
		ActorData actor = this.m_actor;
		GameFlowData gameFlowData = GameFlowData.Get();
		if (this.m_potentialMovementLine != null)
		{
			if (actor)
			{
				if (gameFlowData)
				{
					if (gameFlowData.activeOwnedActorData == actor)
					{
						Color color = LineData.GetColor(this.m_potentialMovementLine.isChasing, true, true);
						MovementPathStart previousLine = null;
						if (this.m_potentialMovementLine.m_lineObject != null)
						{
							previousLine = this.m_potentialMovementLine.m_movePathStart;
						}
						this.m_potentialMovementLine.m_lineObject = this.CreateLineObject(this.m_potentialMovementLine.m_positions, color, previousLine, this.m_potentialMovementLine.isChasing, actor, 0.4f);
						this.m_potentialMovementLine.m_currentColor = color;
						LineData.LineInstance potentialMovementLine = this.m_potentialMovementLine;
						MovementPathStart movePathStart;
						if (this.m_potentialMovementLine.m_lineObject != null)
						{
							movePathStart = this.m_potentialMovementLine.m_lineObject.GetComponentInChildren<MovementPathStart>(true);
						}
						else
						{
							movePathStart = null;
						}
						potentialMovementLine.m_movePathStart = movePathStart;
						if (!LineData.MovementLinesCanBeVisible())
						{
							this.HideLine(this.m_potentialMovementLine);
						}
					}
				}
			}
		}
	}

	private GameObject CreateLineObject(List<GridPos> targetSquares, Color lineColor, MovementPathStart previousLine, bool isChasing, ActorData theActor, float startOffset = 0.4f)
	{
		if (targetSquares.Count < 2)
		{
			return null;
		}
		this.m_lastAllyMovementChange = Time.time;
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < targetSquares.Count; i++)
		{
			Vector3 worldPosition = Board.Get().GetBoardSquareSafe(targetSquares[i]).GetWorldPosition();
			list.Add(new Vector3(worldPosition.x, worldPosition.y + 0.1f, worldPosition.z));
		}
		return Targeting.GetTargeting().CreateFancyArrowMesh(ref list, 0.2f, lineColor, isChasing, theActor, AbilityUtil_Targeter.TargeterMovementType.Movement, null, previousLine, true, startOffset, 0.4f);
	}

	public void OnTurnStart()
	{
		this.ClearAllLines();
	}

	public void OnMovementPhaseEnd()
	{
		this.ClearAllLines();
	}

	private List<GridPos> GetGridPosPathForPath(BoardSquarePathInfo path, bool isChasing, ActorData chaseTarget)
	{
		List<GridPos> list = null;
		if (isChasing)
		{
			list = new List<GridPos>();
			list.Add(this.m_actor.MoveFromBoardSquare.GetGridPos());
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
		this.HideAllyMovementLinesIfResolving();
	}

	private void HideAllyMovementLinesIfResolving()
	{
		bool flag;
		if (!(GameFlowData.Get() == null))
		{
			if (!(this.m_actor == GameFlowData.Get().activeOwnedActorData))
			{
				flag = (GameFlowData.Get().gameState != GameState.BothTeams_Resolve);
				goto IL_5C;
			}
		}
		flag = true;
		IL_5C:
		if (!flag)
		{
			this.HideLine(this.m_movementLine);
			this.HideLine(this.m_movementSnaredLine);
			this.HideLine(this.m_movementLostLine);
		}
	}

	public void OnDisable()
	{
		this.ClearLine(ref this.m_movementLine);
		this.ClearLine(ref this.m_movementSnaredLine);
	}

	private void ChangeLineColor(LineData.LineInstance theLine, Color newColor)
	{
		if (theLine != null)
		{
			if (theLine.m_lineObject != null && theLine.m_currentColor != newColor)
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
			}
		}
	}

	private void ShowLine(LineData.LineInstance theLine, Color lineColor, bool hideStart = false, bool setGlow = true)
	{
		this.ChangeLineColor(theLine, lineColor);
		if (theLine != null)
		{
			if (theLine.m_lineObject != null)
			{
				MovementPathStart movePathStart = theLine.m_movePathStart;
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
	}

	private void HideLine(LineData.LineInstance theLine)
	{
		this.ChangeLineColor(theLine, Color.clear);
		if (theLine != null && theLine.m_lineObject != null)
		{
			MovementPathStart movePathStart = theLine.m_movePathStart;
			if (movePathStart != null)
			{
				movePathStart.SetGlow(false);
			}
		}
	}

	private void ShowMovementDestIndicator(LineData.LineInstance inst)
	{
		if (inst != null)
		{
			if (inst.m_lineObject != null && inst.m_movePathStart != null)
			{
				if (inst.m_positions.Count > 0)
				{
					MovementPathStart movePathStart = inst.m_movePathStart;
					GridPos gridPos = inst.m_positions[inst.m_positions.Count - 1];
					BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(gridPos);
					if (movePathStart != null)
					{
						movePathStart.SetCharacterMovementPanel(boardSquareSafe);
					}
				}
			}
		}
	}

	private void HideMovementDestIndicator(LineData.LineInstance inst)
	{
		if (inst != null)
		{
			if (inst.m_lineObject != null)
			{
				MovementPathStart movePathStart = inst.m_movePathStart;
				if (movePathStart != null)
				{
					movePathStart.HideCharacterMovementPanel();
				}
			}
		}
	}

	private void SetMovementSnaredLine(List<GridPos> gridPosPath, bool isChasing, bool rebuildLine)
	{
		if (!rebuildLine)
		{
			if (!isChasing)
			{
				goto IL_2F;
			}
		}
		this.ClearLine(ref this.m_movementSnaredLine);
		IL_2F:
		if (gridPosPath.Count > 1)
		{
			if (!rebuildLine)
			{
				if (this.m_movementSnaredLine != null)
				{
					goto IL_71;
				}
			}
			this.m_movementSnaredLine = new LineData.LineInstance();
			IL_71:
			this.m_movementSnaredLine.m_positions = gridPosPath;
			this.m_movementSnaredLine.isChasing = isChasing;
			this.DrawLine(ref this.m_movementSnaredLine, 0.4f);
			if (this.m_movementLine != null)
			{
				if (this.m_movementLine.m_lineObject != null)
				{
					MovementPathStart movePathStart = this.m_movementLine.m_movePathStart;
					if (movePathStart != null)
					{
						movePathStart.SetGlow(false);
					}
				}
			}
		}
		else if (gridPosPath.Count == 1)
		{
			if (this.m_movementLine != null)
			{
				if (this.m_movementLine.m_lineObject != null)
				{
					MovementPathStart movePathStart2 = this.m_movementLine.m_movePathStart;
					if (movePathStart2 != null)
					{
						movePathStart2.SetGlow(false);
					}
				}
			}
		}
		this.MarkForSerializationUpdate();
	}

	private void SetMovementLine(List<GridPos> gridPosPath, bool isChasing, bool rebuildLine)
	{
		if (!rebuildLine)
		{
			if (!isChasing)
			{
				goto IL_25;
			}
		}
		this.ClearLine(ref this.m_movementLine);
		IL_25:
		if (gridPosPath.Count > 1)
		{
			if (!rebuildLine)
			{
				if (this.m_movementLine != null)
				{
					goto IL_5A;
				}
			}
			this.m_movementLine = new LineData.LineInstance();
			IL_5A:
			this.m_movementLine.m_positions = gridPosPath;
			this.m_movementLine.isChasing = isChasing;
			this.DrawLine(ref this.m_movementLine, 0.4f);
		}
		this.MarkForSerializationUpdate();
	}

	private void SetMouseOverMovementLine(List<GridPos> gridPosPath, bool isChasing, bool rebuildLine)
	{
		if (!rebuildLine)
		{
			if (!isChasing)
			{
				goto IL_25;
			}
		}
		this.ClearLine(ref this.m_mousePotentialMovementLine);
		IL_25:
		if (gridPosPath != null && gridPosPath.Count > 1)
		{
			if (!rebuildLine)
			{
				if (this.m_mousePotentialMovementLine != null)
				{
					goto IL_59;
				}
			}
			this.m_mousePotentialMovementLine = new LineData.LineInstance();
			IL_59:
			this.m_mousePotentialMovementLine.m_positions = gridPosPath;
			this.m_mousePotentialMovementLine.isChasing = isChasing;
			this.DrawLine(ref this.m_mousePotentialMovementLine, 0.4f);
			if (this.m_mousePotentialMovementLine.m_lineObject != null)
			{
				MovementPathStart movePathStart = this.m_mousePotentialMovementLine.m_movePathStart;
				if (movePathStart != null)
				{
					movePathStart.SetGlow(false);
				}
			}
		}
		else if (this.m_mousePotentialMovementLine != null)
		{
			this.ClearLine(ref this.m_mousePotentialMovementLine);
		}
	}

	private void SetMovementLostLine(List<GridPos> fullGridPosPath, int numSnaredNodes, bool isChasing, bool rebuildLine)
	{
		if (!rebuildLine)
		{
			if (!isChasing)
			{
				goto IL_26;
			}
		}
		this.ClearLine(ref this.m_movementLostLine);
		IL_26:
		List<GridPos> list = new List<GridPos>();
		int num = Mathf.Max(0, numSnaredNodes - 1);
		for (int i = num; i < fullGridPosPath.Count; i++)
		{
			list.Add(fullGridPosPath[i]);
		}
		if (list.Count > 1)
		{
			if (!rebuildLine)
			{
				if (this.m_movementLostLine != null)
				{
					goto IL_96;
				}
			}
			this.m_movementLostLine = new LineData.LineInstance();
			IL_96:
			this.m_movementLostLine.m_positions = list;
			this.m_movementLostLine.isChasing = isChasing;
			this.DrawLine(ref this.m_movementLostLine, 0.2f);
		}
		this.MarkForSerializationUpdate();
	}

	private void SetPotentialMovementLine(List<GridPos> gridPosPath, bool isChasing)
	{
		this.ClearLine(ref this.m_potentialMovementLine);
		if (gridPosPath.Count > 1)
		{
			this.m_potentialMovementLine = new LineData.LineInstance();
			this.m_potentialMovementLine.m_positions = gridPosPath;
			this.m_potentialMovementLine.isChasing = isChasing;
			this.ActivatePotentialMovementLine();
		}
	}

	public List<GridPos> GetGridPosPath()
	{
		if (this.m_movementLine != null)
		{
			return this.m_movementLine.m_positions;
		}
		if (this.m_potentialMovementLine != null)
		{
			return this.m_potentialMovementLine.m_positions;
		}
		return new List<GridPos>();
	}

	public bool GetIsChasing()
	{
		if (this.m_movementLine != null)
		{
			return this.m_movementLine.isChasing;
		}
		return this.m_potentialMovementLine != null && this.m_potentialMovementLine.isChasing;
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		return false;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}

	public class LineInstance
	{
		public List<GridPos> m_positions;

		public bool isChasing;

		public GameObject m_lineObject;

		public MovementPathStart m_movePathStart;

		public Color m_currentColor;

		public LineInstance()
		{
			this.m_positions = new List<GridPos>();
		}

		public void DestroyLineObject()
		{
			this.m_movePathStart = null;
			if (this.m_lineObject != null)
			{
				UnityEngine.Object.Destroy(this.m_lineObject);
				this.m_lineObject = null;
			}
		}

		public override string ToString()
		{
			string text = string.Empty;
			if (this.m_lineObject == null)
			{
				text += "(no lineObject)\n";
			}
			else
			{
				text += "(created lineObject)\n";
			}
			for (int i = 0; i < this.m_positions.Count; i++)
			{
				GridPos gridPos = this.m_positions[i];
				if (i == 0)
				{
					text += string.Format("({0}, {1})", gridPos.x, gridPos.y);
				}
				else if (i == this.m_positions.Count - 1)
				{
					text += string.Format(",\n({0}, {1}) (end)", gridPos.x, gridPos.y);
				}
				else
				{
					text += string.Format(",\n({0}, {1})", gridPos.x, gridPos.y);
				}
			}
			return text;
		}
	}
}
