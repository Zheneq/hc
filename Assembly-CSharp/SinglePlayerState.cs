using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SinglePlayerState
{
	public enum SinglePlayerTag
	{
		PauseTimer,
		RequireShootAndMove,
		DefaultCinematicCam,
		ForceOnCinematicCam,
		ShutdownOnExit,
		AdvanceOnNewTurn,
		RequireMaxPossibleAbilities,
		RequireMoveToAdvanceScriptDestination,
		ClearCooldownsOnEnter,
		DEPRECATED1,
		AdvanceOnMoveEntered,
		AdvanceOnAbilitySelected,
		AdvanceOnAbilityTargeted,
		AdvanceOnLockInEntered,
		DisableCancel,
		RequireChasing,
		RequireDash,
		TeleportToAdvanceScriptDestination,
		ResetHealthOnEnter,
		ResetEnergyOnEnter,
		EnableChatter,
		EnableAutoQueuedAbilitiesForNpcs,
		EnableHiddenMovementText,
		EnableBrush,
		DisableAdvanceTurn,
		AdvanceOnDecisionEnd,
		AdvanceOnResolutionEnd,
		EnableCooldownIndicators,
		ResetContributionOnEnter,
		AdvanceOnTutorialQueueEmpty
	}

	public enum ActorDiesTrigger
	{
		Never,
		AnyActor,
		SpawnedNPCs,
		Players,
		ClientActor,
		ClientAlly,
		ClientEnemy
	}

	public string m_stateTitle;

	[Tooltip("-1 to allow no abilities, 0-4 for freelancer abilities, 7-9 for catalysts")]
	public int[] m_allowedAbilities;

	public BoardRegion m_allowedDestinations;

	public BoardRegion m_advanceScriptDestinations;

	public BoardRegion m_respawnDestinations;

	public BoardRegion m_allowedTargets;

	public bool m_mustTargetNearCenter;

	public bool m_onlyAllowWaypointMovement;

	public BoardRegion m_rightClickHighlight;

	public string m_rightClickText;

	public float m_rightClickHeight;

	public BoardRegion m_shiftRightClickHighlight;

	public string m_shiftRightClickText;

	public float m_shiftRightClickHeight;

	public BoardRegion m_leftClickHighlight;

	public string m_leftClickText;

	public float m_leftClickHeight;

	public int m_minAbilityTargetsForAiming;

	public string m_bannerText;

	public TextRegion m_tutorialBoxText;

	public TextRegion m_tutorialBoxText2;

	public TextRegion m_tutorialBoxText3;

	public TextRegion m_tutorialCameraMovementText;

	public TextRegion m_tutorialCameraRotationText;

	public string m_errorStringOnForbiddenPath;

	public GameObject m_cameraRotationTarget;

	public float m_advanceAfterSeconds;

	public int m_minPlayerHitPoints;

	public int m_minAllyHitPoints;

	[AudioEvent(false)]
	public string m_audioEventOnPreEnter;

	[AudioEvent(false)]
	public string m_audioEventOnEnter;

	[AudioEvent(false)]
	public string m_audioEventOnExit;

	public SinglePlayerScriptedChat[] m_chatTextOnEnter;

	public string m_tutorialVideoPreviewOnEnter;

	[HideInInspector]
	public bool m_markedForAdvanceState;

	[HideInInspector]
	public int m_stateIndex = -1;

	[HideInInspector]
	public float m_startTime;

	public SinglePlayerTag[] m_tags;

	public ActivatableObject[] m_activationsOnEnter;

	public ActivatableObject[] m_activationsOnExit;

	public ActivatableUI[] m_uiActivationsOnEnter;

	public ActivatableUI[] m_uiActivationsOnExit;

	public string[] m_npcsToSpawnOnEnter;

	public string[] m_npcsToDespawnOnEnter;

	public ActorDiesTrigger m_advanceScriptIfActorDies;

	public int m_advanceScriptIfActorDiesCount;

	[HideInInspector]
	public int m_actorDeaths;

	private HashSet<BoardSquare> m_advanceDestinationsHashSet;

	public void Initialize()
	{
		m_allowedDestinations.Initialize();
		m_advanceScriptDestinations.Initialize();
		m_respawnDestinations.Initialize();
		m_allowedTargets.Initialize();
		m_rightClickHighlight.Initialize();
		m_shiftRightClickHighlight.Initialize();
		m_leftClickHighlight.Initialize();
	}

	public string GetBannerText()
	{
		if (m_bannerText.IsNullOrEmpty())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		return StringUtil.TR(m_bannerText);
	}

	public string GetRightClickText()
	{
		if (m_rightClickText.IsNullOrEmpty())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		return StringUtil.TR(m_rightClickText);
	}

	public string GetShiftRightClickText()
	{
		if (m_shiftRightClickText.IsNullOrEmpty())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		return StringUtil.TR(m_shiftRightClickText);
	}

	public string GetLeftClickText()
	{
		if (m_leftClickText.IsNullOrEmpty())
		{
			return string.Empty;
		}
		return StringUtil.TR(m_leftClickText);
	}

	public string GetTutorialBoxText()
	{
		if (m_tutorialBoxText.m_text.IsNullOrEmpty())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		return StringUtil.TR(m_tutorialBoxText.m_text);
	}

	public string GetTutorialBoxText2()
	{
		if (m_tutorialBoxText2.m_text.IsNullOrEmpty())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		return StringUtil.TR(m_tutorialBoxText2.m_text);
	}

	public string GetTutorialBoxText3()
	{
		if (m_tutorialBoxText3.m_text.IsNullOrEmpty())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		return StringUtil.TR(m_tutorialBoxText3.m_text);
	}

	public string GetTutorialCameraMovementText()
	{
		if (m_tutorialCameraMovementText.m_text.IsNullOrEmpty())
		{
			return string.Empty;
		}
		return StringUtil.TR(m_tutorialCameraMovementText.m_text);
	}

	public string GetTutorialCameraRotationText()
	{
		if (m_tutorialCameraRotationText.m_text.IsNullOrEmpty())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		return StringUtil.TR(m_tutorialCameraRotationText.m_text);
	}

	public string GetErrorStringOnForbiddenPath()
	{
		if (m_errorStringOnForbiddenPath.IsNullOrEmpty())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		return StringUtil.TR(m_errorStringOnForbiddenPath);
	}

	public float GetDuration()
	{
		return Time.unscaledTime - m_startTime;
	}

	public void OnEnteringState()
	{
		m_startTime = Time.unscaledTime;
		LogState("Entering", false);
		ActivatableObject[] activationsOnEnter = m_activationsOnEnter;
		foreach (ActivatableObject activatableObject in activationsOnEnter)
		{
			if (activatableObject == null)
			{
				Log.Error("Null activation-object in 'Activations On Enter' for state " + m_stateIndex + ".");
			}
			else if (activatableObject.m_sceneObject == null)
			{
				Log.Error("Activation-object has null scene-object in 'Activations On Enter' for state " + m_stateIndex + ".");
			}
			else
			{
				activatableObject.Activate();
			}
		}
		ActivatableUI[] uiActivationsOnEnter = m_uiActivationsOnEnter;
		foreach (ActivatableUI activatableUI in uiActivationsOnEnter)
		{
			activatableUI.Activate();
		}
		while (true)
		{
			if (m_audioEventOnPreEnter != string.Empty)
			{
				AudioManager.PostEvent(m_audioEventOnPreEnter);
			}
			if (m_audioEventOnEnter != string.Empty)
			{
				AudioManager.PostEvent(m_audioEventOnEnter);
			}
			if (m_chatTextOnEnter != null)
			{
				if (m_chatTextOnEnter.Length > 0)
				{
					SinglePlayerScriptedChat[] chatTextOnEnter = m_chatTextOnEnter;
					foreach (SinglePlayerScriptedChat singlePlayerScriptedChat in chatTextOnEnter)
					{
						UITutorialPanel.Get().QueueDialogue(singlePlayerScriptedChat.m_text, singlePlayerScriptedChat.m_audioEvent, singlePlayerScriptedChat.m_displaySeconds, singlePlayerScriptedChat.m_sender);
					}
				}
			}
			if (m_tutorialVideoPreviewOnEnter != string.Empty)
			{
				UITutorialDropdownlist.Get().DisplayPreviewVideo(m_tutorialVideoPreviewOnEnter);
			}
			string bannerText = GetBannerText();
			if (bannerText != string.Empty)
			{
				UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialBar, true);
				UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText, true);
				HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText.text = bannerText;
			}
			else
			{
				UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialBar, false);
				UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText, false);
			}
			ChatterManager.Get().EnableChatter = GetHasTag(SinglePlayerTag.EnableChatter);
			if (GetHasTag(SinglePlayerTag.DefaultCinematicCam))
			{
				CameraManager.Get().m_abilityCinematicState = CameraManager.AbilityCinematicState.Default;
			}
			else if (GetHasTag(SinglePlayerTag.ForceOnCinematicCam))
			{
				CameraManager.Get().m_abilityCinematicState = CameraManager.AbilityCinematicState.Always;
			}
			else
			{
				CameraManager.Get().m_abilityCinematicState = CameraManager.AbilityCinematicState.Never;
			}
			if (GetHasTag(SinglePlayerTag.ClearCooldownsOnEnter))
			{
				while (true)
				{
					GameFlowData.Get().ClearCooldowns();
					GameFlowData.Get().RefillStocks();
					return;
				}
			}
			return;
		}
	}

	public void OnExitingState()
	{
		LogState("Exiting", true);
		ActivatableObject[] activationsOnExit = m_activationsOnExit;
		foreach (ActivatableObject activatableObject in activationsOnExit)
		{
			if (activatableObject == null)
			{
				Log.Error("Null activation-object in 'Activations On Exit' for state " + m_stateIndex + ".  Tell Colin.");
			}
			else if (activatableObject.m_sceneObject == null)
			{
				Log.Error("Activation-object has null scene-object in 'Activations On Exit' for state " + m_stateIndex + ".  Tell Colin.");
			}
			else
			{
				activatableObject.Activate();
			}
		}
		while (true)
		{
			ActivatableUI[] uiActivationsOnExit = m_uiActivationsOnExit;
			foreach (ActivatableUI activatableUI in uiActivationsOnExit)
			{
				activatableUI.Activate();
			}
			while (true)
			{
				if (m_audioEventOnExit != string.Empty)
				{
					AudioManager.PostEvent(m_audioEventOnExit);
				}
				if ((bool)CameraManager.Get())
				{
					CameraManager.Get().m_abilityCinematicState = CameraManager.AbilityCinematicState.Default;
				}
				if (!GetHasTag(SinglePlayerTag.ShutdownOnExit))
				{
					return;
				}
				while (true)
				{
					GameManager.Get().StopGame();
					GameFlowData.Get().gameState = GameState.EndingGame;
					if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
					{
						UIGameOverScreen.Get().Setup(GameManager.Get().GameConfig.GameType, GameResult.TeamAWon, 1, 0);
					}
					if (AppState.GetCurrent() != AppState_GameTeardown.Get() && AppState.GetCurrent() != AppState_FrontendLoadingScreen.Get())
					{
						while (true)
						{
							AppState_GameTeardown.Get().Enter();
							return;
						}
					}
					return;
				}
			}
		}
	}

	public HashSet<BoardSquare> GetAdvanceDestinations()
	{
		if (m_advanceDestinationsHashSet == null && m_advanceScriptDestinations.m_quads.Length > 0)
		{
			m_advanceDestinationsHashSet = new HashSet<BoardSquare>();
			List<BoardSquare> squaresInRegion = m_advanceScriptDestinations.GetSquaresInRegion();
			using (List<BoardSquare>.Enumerator enumerator = squaresInRegion.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoardSquare current = enumerator.Current;
					m_advanceDestinationsHashSet.Add(current);
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						goto end_IL_003d;
					}
				}
				end_IL_003d:;
			}
		}
		return m_advanceDestinationsHashSet;
	}

	private void LogState(string action, bool eventLog)
	{
		string timeForLogging = MatchLogger.Get().GetTimeForLogging(false);
		int currentTurn = GameFlowData.Get().CurrentTurn;
		string name = SceneManager.GetActiveScene().name;
		string empty = string.Empty;
		int num = SinglePlayerCoordinator.Get().m_script.Length - 1;
		if (m_stateTitle != string.Empty)
		{
			empty = "state '" + m_stateTitle + "' (state " + m_stateIndex + " of " + num + ")";
		}
		else
		{
			empty = "state " + m_stateIndex + " of " + num;
		}
		Log.Info($"{HydrogenConfig.Get().Ticket.Handle} SinglePlayer level {name}: {action} {empty} on turn {currentTurn}.  Time elapsed: {timeForLogging}.");
	}

	public void OnTurnTick()
	{
		CheckMinHitPoints();
	}

	private void CheckMinHitPoints()
	{
	}

	public bool GetHasTag(SinglePlayerTag tag)
	{
		bool result = false;
		SinglePlayerTag[] tags = m_tags;
		int num = 0;
		while (true)
		{
			if (num < tags.Length)
			{
				SinglePlayerTag singlePlayerTag = tags[num];
				if (singlePlayerTag == tag)
				{
					result = true;
					break;
				}
				num++;
				continue;
			}
			break;
		}
		return result;
	}

	public Vector3 GetSpawnFacing(Vector3 spawnPosition)
	{
		Vector3 result = new Vector3(1f, 0f, 0f);
		if (m_cameraRotationTarget != null)
		{
			Vector3 vector = m_cameraRotationTarget.transform.position - spawnPosition;
			vector.y = 0f;
			if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
			{
				if (!(vector.x > 0f))
				{
					result = new Vector3(-1f, 0f, 0f);
				}
				else
				{
					result = new Vector3(1f, 0f, 0f);
				}
			}
			else if (!(vector.z > 0f))
			{
				result = new Vector3(0f, 0f, -1f);
			}
			else
			{
				result = new Vector3(0f, 0f, 1f);
			}
		}
		return result;
	}
}
