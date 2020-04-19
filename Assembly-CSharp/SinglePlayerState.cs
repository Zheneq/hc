using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class SinglePlayerState
{
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

	public SinglePlayerState.SinglePlayerTag[] m_tags;

	public ActivatableObject[] m_activationsOnEnter;

	public ActivatableObject[] m_activationsOnExit;

	public ActivatableUI[] m_uiActivationsOnEnter;

	public ActivatableUI[] m_uiActivationsOnExit;

	public string[] m_npcsToSpawnOnEnter;

	public string[] m_npcsToDespawnOnEnter;

	public SinglePlayerState.ActorDiesTrigger m_advanceScriptIfActorDies;

	public int m_advanceScriptIfActorDiesCount;

	[HideInInspector]
	public int m_actorDeaths;

	private HashSet<BoardSquare> m_advanceDestinationsHashSet;

	public void Initialize()
	{
		this.m_allowedDestinations.Initialize();
		this.m_advanceScriptDestinations.Initialize();
		this.m_respawnDestinations.Initialize();
		this.m_allowedTargets.Initialize();
		this.m_rightClickHighlight.Initialize();
		this.m_shiftRightClickHighlight.Initialize();
		this.m_leftClickHighlight.Initialize();
	}

	public string GetBannerText()
	{
		if (this.m_bannerText.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.GetBannerText()).MethodHandle;
			}
			return string.Empty;
		}
		return StringUtil.TR(this.m_bannerText);
	}

	public string GetRightClickText()
	{
		if (this.m_rightClickText.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.GetRightClickText()).MethodHandle;
			}
			return string.Empty;
		}
		return StringUtil.TR(this.m_rightClickText);
	}

	public string GetShiftRightClickText()
	{
		if (this.m_shiftRightClickText.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.GetShiftRightClickText()).MethodHandle;
			}
			return string.Empty;
		}
		return StringUtil.TR(this.m_shiftRightClickText);
	}

	public string GetLeftClickText()
	{
		if (this.m_leftClickText.IsNullOrEmpty())
		{
			return string.Empty;
		}
		return StringUtil.TR(this.m_leftClickText);
	}

	public string GetTutorialBoxText()
	{
		if (this.m_tutorialBoxText.m_text.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.GetTutorialBoxText()).MethodHandle;
			}
			return string.Empty;
		}
		return StringUtil.TR(this.m_tutorialBoxText.m_text);
	}

	public string GetTutorialBoxText2()
	{
		if (this.m_tutorialBoxText2.m_text.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.GetTutorialBoxText2()).MethodHandle;
			}
			return string.Empty;
		}
		return StringUtil.TR(this.m_tutorialBoxText2.m_text);
	}

	public string GetTutorialBoxText3()
	{
		if (this.m_tutorialBoxText3.m_text.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.GetTutorialBoxText3()).MethodHandle;
			}
			return string.Empty;
		}
		return StringUtil.TR(this.m_tutorialBoxText3.m_text);
	}

	public string GetTutorialCameraMovementText()
	{
		if (this.m_tutorialCameraMovementText.m_text.IsNullOrEmpty())
		{
			return string.Empty;
		}
		return StringUtil.TR(this.m_tutorialCameraMovementText.m_text);
	}

	public string GetTutorialCameraRotationText()
	{
		if (this.m_tutorialCameraRotationText.m_text.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.GetTutorialCameraRotationText()).MethodHandle;
			}
			return string.Empty;
		}
		return StringUtil.TR(this.m_tutorialCameraRotationText.m_text);
	}

	public string GetErrorStringOnForbiddenPath()
	{
		if (this.m_errorStringOnForbiddenPath.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.GetErrorStringOnForbiddenPath()).MethodHandle;
			}
			return string.Empty;
		}
		return StringUtil.TR(this.m_errorStringOnForbiddenPath);
	}

	public float GetDuration()
	{
		return Time.unscaledTime - this.m_startTime;
	}

	public void OnEnteringState()
	{
		this.m_startTime = Time.unscaledTime;
		this.LogState("Entering", false);
		foreach (ActivatableObject activatableObject in this.m_activationsOnEnter)
		{
			if (activatableObject == null)
			{
				Log.Error("Null activation-object in 'Activations On Enter' for state " + this.m_stateIndex + ".", new object[0]);
			}
			else if (activatableObject.m_sceneObject == null)
			{
				Log.Error("Activation-object has null scene-object in 'Activations On Enter' for state " + this.m_stateIndex + ".", new object[0]);
			}
			else
			{
				activatableObject.Activate();
			}
		}
		foreach (ActivatableUI activatableUI in this.m_uiActivationsOnEnter)
		{
			activatableUI.Activate();
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.OnEnteringState()).MethodHandle;
		}
		if (this.m_audioEventOnPreEnter != string.Empty)
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
			AudioManager.PostEvent(this.m_audioEventOnPreEnter, null);
		}
		if (this.m_audioEventOnEnter != string.Empty)
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
			AudioManager.PostEvent(this.m_audioEventOnEnter, null);
		}
		if (this.m_chatTextOnEnter != null)
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
			if (this.m_chatTextOnEnter.Length > 0)
			{
				foreach (SinglePlayerScriptedChat singlePlayerScriptedChat in this.m_chatTextOnEnter)
				{
					UITutorialPanel.Get().QueueDialogue(singlePlayerScriptedChat.m_text, singlePlayerScriptedChat.m_audioEvent, singlePlayerScriptedChat.m_displaySeconds, singlePlayerScriptedChat.m_sender);
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
		if (this.m_tutorialVideoPreviewOnEnter != string.Empty)
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
			UITutorialDropdownlist.Get().DisplayPreviewVideo(this.m_tutorialVideoPreviewOnEnter);
		}
		string bannerText = this.GetBannerText();
		if (bannerText != string.Empty)
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
			UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialBar, true, null);
			UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText, true, null);
			HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText.text = bannerText;
		}
		else
		{
			UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialBar, false, null);
			UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText, false, null);
		}
		ChatterManager.Get().EnableChatter = this.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableChatter);
		if (this.GetHasTag(SinglePlayerState.SinglePlayerTag.DefaultCinematicCam))
		{
			CameraManager.Get().m_abilityCinematicState = CameraManager.AbilityCinematicState.Default;
		}
		else if (this.GetHasTag(SinglePlayerState.SinglePlayerTag.ForceOnCinematicCam))
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
			CameraManager.Get().m_abilityCinematicState = CameraManager.AbilityCinematicState.Always;
		}
		else
		{
			CameraManager.Get().m_abilityCinematicState = CameraManager.AbilityCinematicState.Never;
		}
		if (this.GetHasTag(SinglePlayerState.SinglePlayerTag.ClearCooldownsOnEnter))
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
			GameFlowData.Get().ClearCooldowns();
			GameFlowData.Get().RefillStocks();
		}
	}

	public void OnExitingState()
	{
		this.LogState("Exiting", true);
		foreach (ActivatableObject activatableObject in this.m_activationsOnExit)
		{
			if (activatableObject == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.OnExitingState()).MethodHandle;
				}
				Log.Error("Null activation-object in 'Activations On Exit' for state " + this.m_stateIndex + ".  Tell Colin.", new object[0]);
			}
			else if (activatableObject.m_sceneObject == null)
			{
				Log.Error("Activation-object has null scene-object in 'Activations On Exit' for state " + this.m_stateIndex + ".  Tell Colin.", new object[0]);
			}
			else
			{
				activatableObject.Activate();
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
		foreach (ActivatableUI activatableUI in this.m_uiActivationsOnExit)
		{
			activatableUI.Activate();
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
		if (this.m_audioEventOnExit != string.Empty)
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
			AudioManager.PostEvent(this.m_audioEventOnExit, null);
		}
		if (CameraManager.Get())
		{
			CameraManager.Get().m_abilityCinematicState = CameraManager.AbilityCinematicState.Default;
		}
		if (this.GetHasTag(SinglePlayerState.SinglePlayerTag.ShutdownOnExit))
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
			GameManager.Get().StopGame(GameResult.NoResult);
			GameFlowData.Get().gameState = GameState.EndingGame;
			if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
			{
				UIGameOverScreen.Get().Setup(GameManager.Get().GameConfig.GameType, GameResult.TeamAWon, 1, 0);
			}
			if (AppState.GetCurrent() != AppState_GameTeardown.Get() && AppState.GetCurrent() != AppState_FrontendLoadingScreen.Get())
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
				AppState_GameTeardown.Get().Enter();
			}
		}
	}

	public HashSet<BoardSquare> GetAdvanceDestinations()
	{
		if (this.m_advanceDestinationsHashSet == null && this.m_advanceScriptDestinations.m_quads.Length > 0)
		{
			this.m_advanceDestinationsHashSet = new HashSet<BoardSquare>();
			List<BoardSquare> list = this.m_advanceScriptDestinations.\u001D();
			using (List<BoardSquare>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoardSquare item = enumerator.Current;
					this.m_advanceDestinationsHashSet.Add(item);
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.GetAdvanceDestinations()).MethodHandle;
				}
			}
		}
		return this.m_advanceDestinationsHashSet;
	}

	private void LogState(string action, bool eventLog)
	{
		string timeForLogging = MatchLogger.Get().GetTimeForLogging(false);
		int currentTurn = GameFlowData.Get().CurrentTurn;
		string name = SceneManager.GetActiveScene().name;
		string text = string.Empty;
		int num = SinglePlayerCoordinator.Get().m_script.Length - 1;
		if (this.m_stateTitle != string.Empty)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.LogState(string, bool)).MethodHandle;
			}
			text = string.Concat(new object[]
			{
				"state '",
				this.m_stateTitle,
				"' (state ",
				this.m_stateIndex,
				" of ",
				num,
				")"
			});
		}
		else
		{
			text = string.Concat(new object[]
			{
				"state ",
				this.m_stateIndex,
				" of ",
				num
			});
		}
		Log.Info(string.Format("{0} SinglePlayer level {1}: {2} {3} on turn {4}.  Time elapsed: {5}.", new object[]
		{
			HydrogenConfig.Get().Ticket.Handle,
			name,
			action,
			text,
			currentTurn,
			timeForLogging
		}), new object[0]);
	}

	public void OnTurnTick()
	{
		this.CheckMinHitPoints();
	}

	private void CheckMinHitPoints()
	{
	}

	public bool GetHasTag(SinglePlayerState.SinglePlayerTag tag)
	{
		bool result = false;
		foreach (SinglePlayerState.SinglePlayerTag singlePlayerTag in this.m_tags)
		{
			if (singlePlayerTag == tag)
			{
				result = true;
				return result;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.GetHasTag(SinglePlayerState.SinglePlayerTag)).MethodHandle;
			return result;
		}
		return result;
	}

	public Vector3 GetSpawnFacing(Vector3 spawnPosition)
	{
		Vector3 result = new Vector3(1f, 0f, 0f);
		if (this.m_cameraRotationTarget != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(SinglePlayerState.GetSpawnFacing(Vector3)).MethodHandle;
			}
			Vector3 vector = this.m_cameraRotationTarget.transform.position - spawnPosition;
			vector.y = 0f;
			if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
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
				if (vector.x > 0f)
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
					result = new Vector3(1f, 0f, 0f);
				}
				else
				{
					result = new Vector3(-1f, 0f, 0f);
				}
			}
			else if (vector.z > 0f)
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
				result = new Vector3(0f, 0f, 1f);
			}
			else
			{
				result = new Vector3(0f, 0f, -1f);
			}
		}
		return result;
	}

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
}
