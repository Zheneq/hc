// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using CameraManagerInternal;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

public class CameraManager : MonoBehaviour, IGameEventListener
{
	internal enum AbilityCinematicState
	{
		Default,
		Never,
		Always
	}

	public enum CameraTargetReason
	{
		CameraCenterKeyHeld,
		AbilitySoftTargeting,
		ClientActorRespawned,
		MustSelectRespawnLoc,
		ChangedActiveActor,
		CtfTurninRegionSpawned,
		ReachedTargetObj,
		IsoCamEnabled,
		ForcingTransform,
		UserFocusingOnActor,
		SinglePlayerStateScriptCommand,
		CtfFlagTurnedIn,
		// MoveStart // added in rogues
	}

	public enum CameraShakeIntensity
	{
		Small,
		Large,
		None
	}

	public enum CameraLogType
	{
		None,
		Ability,
		Isometric,
		MergeBounds,
		SimilarBounds
	}

	public GameObject m_gameCameraPrefab;
	public GameObject m_faceCameraPrefab;  // removed in rogues
	public GameObject m_tauntBackgroundCameraPrefab;
	public bool m_useTauntBackground;
	// public GameObject m_live2DVignetteCameraPrefab;  // added in rogues

	internal AbilityCinematicState m_abilityCinematicState;

	private static CameraManager s_instance;
	
	private bool m_useAbilitiesCameraOutOfCinematics;
	private int m_abilityAnimationsBetweenCamEvents;
	private List<CameraShot.CharacterToAnimParamSetActions> m_animParamSettersOnTurnTick = new List<CameraShot.CharacterToAnimParamSetActions>();
	private bool m_useCameraToggleKey = true;
	private bool m_useRightClickToToggle;
	
	public const float c_cameraToggleGracePeriod = 1.5f;

	private Bounds m_savedMoveCamBound;
	private int m_savedMoveCamBoundTurn = -1;

	internal float DefaultFOV { get; private set; }
	public Camera FaceCamera { get; private set; }  // removed in rogues
	internal GameObject AudioListener { get; private set; }
	internal CameraShotSequence ShotSequence { get; private set; }
	internal CameraFaceShot FaceShot { get; private set; }  // removed in rogues
	internal Bounds CameraPositionBounds { get; private set; }
	internal TauntBackgroundCamera TauntBackgroundCamera { get; private set; }
	// internal PveLive2DVignettePortraitCamera Live2DVignetteCamera { get; private set; }
  // added in rogues
	internal float SecondsRemainingToPauseForUserControl { get; set; } = -1f;
	public bool UseCameraToggleKey => m_useCameraToggleKey;
	public static bool CamDebugTraceOn => false;
	
	// removed in rogues
	internal bool InFaceShot(ActorData actor)
	{
		return FaceShot != null && FaceShot.Actor == actor;
	}

	internal bool WillRespondToInput()
	{
		return GetIsometricCamera().enabled;
	}

	internal bool IsPlayingShotSequence()
	{
		return ShotSequence != null;
	}

	internal bool InCinematic()
	{
		return ShotSequence != null;
	}

	internal ActorData GetCinematicTargetActor()
	{
		return ShotSequence != null
			? ShotSequence.Actor
			: null;
	}

	internal int GetCinematicActionAnimIndex()
	{
		return ShotSequence != null
			? ShotSequence.m_animIndexTauntTrigger
			: -1;
	}

	internal bool ShouldHideBrushVfx()
	{
		return TauntBackgroundCamera != null && ShotSequence != null;
	}

	internal static CameraManager Get()
	{
		return s_instance;
	}

	protected void Awake()
	{
		if (s_instance != null)
		{
			Debug.LogError("CameraManager instance was not null on Awake(), please check to make sure there is only 1 instance of GameSceneSingletons object");
		}
		s_instance = this;
		if (GameFlowData.Get() != null
		    && VisualsLoader.Get() != null
		    && VisualsLoader.Get().LevelLoaded())
		{
			OnVisualSceneLoaded();
		}
		else
		{
			enabled = false;
		}
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.VisualSceneLoaded);
	}

	protected void Start()
	{
		if (Camera.main != null)
		{
			DefaultFOV = Camera.main.fieldOfView;
		}
		// added in rogues
		// if (NetworkClient.active && GameEventManager.Get() != null)
		// {
		// 	GameEventManager.Get().FireEvent(GameEventManager.EventType.GameSceneSingletonStartOnClient, null);
		// }
	}

	private void ClearCameras()
	{
		if (Camera.main != null)
		{
			DestroyImmediate(Camera.main.gameObject);
		}
		
		// removed in rogues
		if (FaceCamera != null)
		{
			DestroyImmediate(FaceCamera.gameObject);
		}
	}

	private void OnDestroy()
	{
		ClearCameras();
		s_instance = null;
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.VisualSceneLoaded);
		}
		s_instance = null;
	}

	void IGameEventListener.OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.VisualSceneLoaded)
		{
			OnVisualSceneLoaded();
		}
	}

	public void EnableOffFogOfWarEffect(bool enable)
	{
		if (Camera.main != null && Camera.main.gameObject.GetComponent<FogOfWarEffect>() != null)
		{
			Camera.main.gameObject.GetComponent<FogOfWarEffect>().enabled = enable;
		}
	}

	private void OnVisualSceneLoaded()
	{
		GameObject cameraObject = Camera.main == null ? null : Camera.main.gameObject;
		
		// removed in rogues
		if (cameraObject != null && cameraObject.GetComponent<IsometricCamera>() == null)
		{
			if (NetworkClient.active)
			{
				Log.Error("Environment scene is missing an instance of GameCamera.prefab, bloom may be active when loading into the level on Graphics Quality: Low until an instance is put in the environment scene.");
			}
			if (m_gameCameraPrefab != null)
			{
				DestroyImmediate(cameraObject);
				Instantiate(m_gameCameraPrefab);
				cameraObject = !(Camera.main == null) ? Camera.main.gameObject : null;
			}
		}
		// end removed in rogues
		
		if (cameraObject == null)
		{
			if (NetworkClient.active)
			{
				Log.Error("Environment scene is missing an instance of GameCamera.prefab, bloom may be active when loading into the level on Graphics Quality: Low until an instance is put in the environment scene.");
			}
			if (m_gameCameraPrefab == null)
			{
				throw new ApplicationException("There is no game camera prefab assigned in the CameraManager!");
			}
			GameObject y = Instantiate(m_gameCameraPrefab);
			if (Camera.main == null)
			{
				Log.Error("Failed to switch to game camera; main camera is null");
			}
			else if (Camera.main.gameObject != y)
			{
				Log.Error("Failed to switch to game camera; main camera is '{0}'", Camera.main);
			}
		}
		DontDestroyOnLoad(Camera.main.gameObject);
		
		// removed in rogues
		if (FaceCamera == null)
		{
			GameObject faceCameraObject = Instantiate(m_faceCameraPrefab);
			DontDestroyOnLoad(faceCameraObject);
			FaceCamera = faceCameraObject != null
				? faceCameraObject.GetComponent<Camera>()
				: null;
			FaceCamera.gameObject.SetActive(false);
		}
		// end removed in rogues
		
		if (Camera.main != null)
		{
			DefaultFOV = Camera.main.fieldOfView;
			RenderSettings.fog = false;  // removed in rogues
			if (NetworkClient.active
			    && m_tauntBackgroundCameraPrefab != null
			    && m_useTauntBackground)
			{
				GameObject tauntCameraObject = Instantiate(m_tauntBackgroundCameraPrefab);
				TauntBackgroundCamera = tauntCameraObject.GetComponent<TauntBackgroundCamera>();
				if (TauntBackgroundCamera == null)
				{
					Debug.LogError("Did not find taunt background camera component");
					Destroy(tauntCameraObject);
				}
				else
				{
					tauntCameraObject.SetActive(false);
				}
			}
			
			// added in rogues
			// if (NetworkClient.active && m_live2DVignetteCameraPrefab != null)
			// {
			// 	GameObject vignetteObject = Instantiate<GameObject>(m_live2DVignetteCameraPrefab);
			// 	Live2DVignetteCamera = vignetteObject.GetComponent<PveLive2DVignettePortraitCamera>();
			// 	if (Live2DVignetteCamera == null)
			// 	{
			// 		Destroy(vignetteObject);
			// 	}
			// }
		}

		BoardSquare centerSquare = Board.Get().GetSquareFromIndex(Board.Get().GetMaxX() / 2, Board.Get().GetMaxY() / 2);
		if (centerSquare != null)
		{
			IsometricCamera isometricCamera = GetIsometricCamera();
			if (isometricCamera != null && isometricCamera.enabled)
			{
				Vector3 pos = new Vector3(
					centerSquare.gameObject.transform.position.x, 
					Board.Get().BaselineHeight,
					centerSquare.gameObject.transform.position.z);
				isometricCamera.SetTargetPosition(pos, 0f);
			}
		}
		BoardSquare squareA = Board.Get().GetSquareFromIndex(0, 0);
		BoardSquare squareB = Board.Get().GetSquareFromIndex(Board.Get().GetMaxX() - 1, Board.Get().GetMaxY() - 1);
		
		Bounds cameraPositionBounds = squareA.CameraBounds;
		cameraPositionBounds.Encapsulate(squareB.CameraBounds);
		CameraPositionBounds = cameraPositionBounds;
		
		if (AudioListener == null && AudioListenerController.Get() != null)
		{
			AudioListener = AudioListenerController.Get().gameObject;
		}
		enabled = true;
		GameEventManager.Get().FireEvent(GameEventManager.EventType.GameCameraCreatedPre, null);
		GameEventManager.Get().FireEvent(GameEventManager.EventType.GameCameraCreated, null);
		GameEventManager.Get().FireEvent(GameEventManager.EventType.GameCameraCreatedPost, null);
	}

	internal bool IsOnCamera(Bounds bounds)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		return GeometryUtility.TestPlanesAABB(planes, bounds);
	}

	internal FlyThroughCamera GetFlyThroughCamera()
	{
		return Camera.main != null
			? Camera.main.GetComponent<FlyThroughCamera>()
			: null;
	}

	internal DebugCamera GetDebugCamera()
	{
		return Camera.main != null
			? Camera.main.GetComponent<DebugCamera>()
			: null;
	}

	internal IsometricCamera GetIsometricCamera()
	{
		return Camera.main != null
			? Camera.main.GetComponent<IsometricCamera>()
			: null;
	}

	internal AbilitiesCamera GetAbilitiesCamera()
	{
		return Camera.main != null
			? Camera.main.GetComponent<AbilitiesCamera>()
			: null;
	}

	internal FadeObjectsCameraComponent GetFadeObjectsCamera()
	{
		return Camera.main != null
			? Camera.main.GetComponent<FadeObjectsCameraComponent>()
			: null;
	}

	internal void OnSpecialCameraShotBehaviorEnable(CameraTransitionType transitionInType)
	{
		IsometricCamera isometricCamera = GetIsometricCamera();
		AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
		if (abilitiesCamera.enabled)
		{
			abilitiesCamera.OnTransitionOut();
			abilitiesCamera.enabled = false;
		}
		if (isometricCamera.enabled)
		{
			isometricCamera.OnTransitionOut();
			isometricCamera.enabled = false;
		}
	}

	internal void OnSpecialCameraShotBehaviorDisable(CameraTransitionType transitionInType)
	{
		Camera.main.fieldOfView = Get().DefaultFOV;
		m_useAbilitiesCameraOutOfCinematics = ShouldUseAbilitiesCameraOutOfCinematics();
		if (DebugParameters.Get() == null
		    || !DebugParameters.Get().GetParameterAsBool("DebugCamera"))
		{
			if (m_useAbilitiesCameraOutOfCinematics)
			{
				EnableAbilitiesCamera(transitionInType);
			}
			else
			{
				EnableIsometricCamera(transitionInType);
			}
		}
	}

	public bool ShouldAutoCameraMove()
	{
		return AccountPreferences.Get() != null
		       && AccountPreferences.Get().GetBool(BoolPreference.AutoCameraCenter)
		    // removed in rogues
		    || GameManager.Get() != null
		       && GameManager.Get().GameConfig != null
		       && GameManager.Get().GameConfig.GameType == GameType.Tutorial
		    // end removed in rogues
		    || GameFlowData.Get().activeOwnedActorData != null
		       && GameFlowData.Get().activeOwnedActorData.GetActorTurnSM().CurrentState == TurnStateEnum.PICKING_RESPAWN;
	}

	internal void OnActionPhaseChange(ActionBufferPhase newPhase, bool requestAbilityCamera)  // no newPhase in rogues
	{
		if (DebugParameters.Get() == null || !DebugParameters.Get().GetParameterAsBool("DebugCamera"))
		{
			if (requestAbilityCamera && ShouldUseAbilitiesCameraOutOfCinematics())
			{
				EnableAbilitiesCamera();
			}
			else
			{
				EnableIsometricCamera();
			}
		}
		FadeObjectsCameraComponent fadeObjectsCamera = GetFadeObjectsCamera();
		if (fadeObjectsCamera != null)
		{
			fadeObjectsCamera.ResetDesiredVisibleObjects();
		}
		if (m_abilityAnimationsBetweenCamEvents > 0)
		{
			Log.Warning("Camera manager: phase change to {0} with {1} abilities between camera start and end tags. Expected zero",
				newPhase.ToString(), m_abilityAnimationsBetweenCamEvents);  // removed in rogues
			m_abilityAnimationsBetweenCamEvents = 0;
		}
	}

	internal void SaveMovementCameraBound(Bounds bound)
	{
		m_savedMoveCamBound = bound;
		m_savedMoveCamBoundTurn = GameFlowData.Get().CurrentTurn;
	}

	// empty in rogues
	internal void SetTargetForMovementIfNeeded()
	{
		if (ShouldSetCameraForMovement() && GameFlowData.Get().CurrentTurn == m_savedMoveCamBoundTurn)
		{
			SetTarget(m_savedMoveCamBound);
		}
	}

	internal void SaveMovementCameraBoundForSpectator(Bounds bound)
	{
		if (m_savedMoveCamBoundTurn == GameFlowData.Get().CurrentTurn)
		{
			m_savedMoveCamBound.Encapsulate(bound);
		}
		else
		{
			m_savedMoveCamBound = bound;
			m_savedMoveCamBoundTurn = GameFlowData.Get().CurrentTurn;
		}
	}

	internal void SwitchCameraForMovement()
	{
		// reactor
		if (ShouldSetCameraForMovement()
		    && SecondsRemainingToPauseForUserControl <= 0f
		    && Get().ShouldAutoCameraMove())
		{
			Get().OnActionPhaseChange(ActionBufferPhase.Movement, true);
		}
		// rogues
		// if (SecondsRemainingToPauseForUserControl <= 0f)
		// {
		// 	Get().ShouldAutoCameraMove();
		// }
	}

	// removed in rogues
	private bool ShouldSetCameraForMovement()
	{
		return GameManager.Get() != null && GameManager.Get().GameConfig.GameType != GameType.Tutorial;
	}

	internal void AddAnimParamSetActions(CameraShot.CharacterToAnimParamSetActions animParamSetActions)
	{
		if (animParamSetActions != null)
		{
			m_animParamSettersOnTurnTick.Add(animParamSetActions);
		}
	}

	internal void OnTurnTick()
	{
		if (NetworkClient.active)
		{
			foreach (CameraShot.CharacterToAnimParamSetActions actions in m_animParamSettersOnTurnTick)
			{
				if (actions != null && actions.m_actor != null)
				{
					CameraShot.SetAnimParamsForActor(actions.m_actor, actions.m_animSetActions);
				}
			}
		}
		m_animParamSettersOnTurnTick.Clear();
	}

	private void EnableFlyThroughCamera()
	{
		AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
		IsometricCamera isometricCamera = GetIsometricCamera();
		DebugCamera debugCamera = GetDebugCamera();
		FlyThroughCamera flyThroughCamera = GetFlyThroughCamera();
		if (!flyThroughCamera.enabled)
		{
			abilitiesCamera.enabled = false;
			isometricCamera.enabled = false;
			debugCamera.enabled = false;
			flyThroughCamera.enabled = true;
		}
	}

	private void EnableDebugCamera()
	{
		AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
		IsometricCamera isometricCamera = GetIsometricCamera();
		DebugCamera debugCamera = GetDebugCamera();
		FlyThroughCamera flyThroughCamera = GetFlyThroughCamera();
		if (!debugCamera.enabled)
		{
			abilitiesCamera.enabled = false;
			isometricCamera.enabled = false;
			flyThroughCamera.enabled = false;
			debugCamera.enabled = true;
		}
	}

	private void EnableAbilitiesCamera(CameraTransitionType transitionInType = CameraTransitionType.Move)
	{
		if (Camera.main == null)
		{
			return;
		}
		AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
		IsometricCamera isometricCamera = GetIsometricCamera();
		DebugCamera debugCamera = GetDebugCamera();
		FlyThroughCamera flyThroughCamera = GetFlyThroughCamera();
		if (abilitiesCamera == null)
		{
			abilitiesCamera = Camera.main.gameObject.AddComponent<AbilitiesCamera>();
			abilitiesCamera.enabled = false;
			Log.Warning("Missing AbilitiesCamera component on main camera. Generating dynamically for now.");
		}
		m_useAbilitiesCameraOutOfCinematics = true;
		debugCamera.enabled = false;
		flyThroughCamera.enabled = false;
		if (!abilitiesCamera.enabled)
		{
			if (CamDebugTraceOn)
			{
				LogForDebugging("<color=white>Enable Abilities Camera</color>, transition type: " + transitionInType);
			}

			if (isometricCamera.enabled)
			{
				isometricCamera.OnTransitionOut();
				isometricCamera.enabled = false;
			}

			abilitiesCamera.enabled = true;
			abilitiesCamera.OnTransitionIn(transitionInType);
		}
	}

	private void EnableIsometricCamera(CameraTransitionType transitionInType = CameraTransitionType.Move)
	{
		if (Camera.main == null)
		{
			return;
		}
		AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
		IsometricCamera isometricCamera = GetIsometricCamera();
		DebugCamera debugCamera = GetDebugCamera();
		FlyThroughCamera flyThroughCamera = GetFlyThroughCamera();
		if (debugCamera != null)  // no check in rogues
		{
			debugCamera.enabled = false;
		}
		if (flyThroughCamera != null)  // no check in rogues
		{
			flyThroughCamera.enabled = false;
		}
		if (abilitiesCamera == null)
		{
			abilitiesCamera = Camera.main.gameObject.AddComponent<AbilitiesCamera>();
			abilitiesCamera.enabled = false;
#if SERVER
			Log.Warning("Missing AbilitiesCamera component on main camera. Generating dynamically for now."); // custom, was IsometricCamera in reactor
		}
		// custom
		// TODO HACK do we really need cameras on the server?
		if (isometricCamera == null)
		{
			isometricCamera = Camera.main.gameObject.AddComponent<IsometricCamera>();
			isometricCamera.enabled = false;
			
#endif
			Log.Warning("Missing IsometricCamera component on main camera. Generating dynamically for now.");
		}
		m_useAbilitiesCameraOutOfCinematics = false;
		if (!isometricCamera.enabled && GameFlowData.Get().LocalPlayerData != null)
		{
			if (CamDebugTraceOn)
			{
				LogForDebugging("<color=white>Enable Isometric Camera</color>, transition type: " + transitionInType);
			}

			bool flag = false;
			if (abilitiesCamera.enabled)
			{
				flag = abilitiesCamera.GetSecondsRemainingToPauseForUserControl() > 0f;
				abilitiesCamera.OnTransitionOut();
				abilitiesCamera.enabled = false;
			}

			isometricCamera.enabled = true;
			isometricCamera.OnTransitionIn(transitionInType);
			if (GameFlowData.Get().activeOwnedActorData != null
			    // && GameFlowData.Get().ActingTeam == GameFlowData.Get().activeOwnedActorData.GetTeam()  // added in rogues
			    )
			{
				GameObject targetObject = !abilitiesCamera.IsDisabledUntilSetTarget && !flag
					? GameFlowData.Get().activeOwnedActorData.gameObject
					: null;
				isometricCamera.SetTargetObject(targetObject, CameraTargetReason.IsoCamEnabled);
			}
		}
	}

	internal void OnActiveOwnedActorChange(ActorData actor)
	{
		FadeObjectsCameraComponent fadeObjectsCamera = GetFadeObjectsCamera();
		if (fadeObjectsCamera != null)
		{
			fadeObjectsCamera.ResetDesiredVisibleObjects();
		}

		GameObject targetObject = SecondsRemainingToPauseForUserControl <= 0f ? actor.gameObject : null;
		SetTargetObject(targetObject, CameraTargetReason.ChangedActiveActor);
	}

	internal void OnActorMoved(ActorData actor)
	{
		FadeObjectsCameraComponent fadeObjectsCamera = GetFadeObjectsCamera();
		if (fadeObjectsCamera != null)
		{
			fadeObjectsCamera.MarkForResetVisibleObjects();
		}
	}

	internal void OnSelectedAbilityChanged(Ability ability)
	{
		FadeObjectsCameraComponent fadeObjectsCamera = GetFadeObjectsCamera();
		if (fadeObjectsCamera != null)
		{
			fadeObjectsCamera.ResetDesiredVisibleObjects();
		}
	}

	internal void OnNewTurnSMState()
	{
		FadeObjectsCameraComponent fadeObjectsCamera = GetFadeObjectsCamera();
		if (fadeObjectsCamera != null)
		{
			fadeObjectsCamera.ResetDesiredVisibleObjects();
		}
	}

	internal bool IsOnMainCamera(ActorData a)
	{
		if (a == null)
		{
			return false;
		}
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		Bounds cameraBounds = a.GetTravelBoardSquare().CameraBounds;
		return GeometryUtility.TestPlanesAABB(planes, cameraBounds);
	}

	internal void SetTargetObject(GameObject target, CameraTargetReason reason)
	{
		IsometricCamera isometricCamera = GetIsometricCamera();
		if (isometricCamera != null && isometricCamera.enabled)
		{
			isometricCamera.SetTargetObject(target, reason, false);
		}
	}

	internal void SetTargetObjectToMouse(GameObject target, CameraTargetReason reason)
	{
		if (CamDebugTraceOn)
		{
			LogForDebugging("CameraManager.SetTargetObjectToMouse " + (target != null ? target.name : "NULL"));
		}
		IsometricCamera isometricCamera = GetIsometricCamera();
		if (isometricCamera != null && isometricCamera.enabled)
		{
			isometricCamera.SetTargetObject(target, reason, true);
		}
	}

	public void SetTargetPosition(Vector3 pos, float easeInTime = 0f)
	{
		IsometricCamera isometricCamera = GetIsometricCamera();
		if (isometricCamera != null && isometricCamera.enabled)
		{
			isometricCamera.SetTargetPosition(pos, easeInTime);
			isometricCamera.SetTargetObject(null, CameraTargetReason.ReachedTargetObj);
		}
	}

	internal void SetTarget(Bounds bounds, bool quickerTransition = false, bool useLowPosition = false)
	{
		if (CamDebugTraceOn)
		{
			LogForDebugging("CameraManager.SetTarget " + bounds + " | quicker transition: " + quickerTransition + " | useLowPosition: " + useLowPosition);
		}
		if (Camera.main == null)
		{
			return;
		}
		AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
		if (abilitiesCamera == null)
		{
			abilitiesCamera = Camera.main.gameObject.AddComponent<AbilitiesCamera>();
			abilitiesCamera.enabled = false;
		}
		abilitiesCamera.SetTarget(bounds, quickerTransition, useLowPosition);
	}

	internal Bounds GetTarget()
	{
		AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
		if (abilitiesCamera == null)
		{
			abilitiesCamera = Camera.main.gameObject.AddComponent<AbilitiesCamera>();
			abilitiesCamera.enabled = false;
		}
		return abilitiesCamera.GetTarget();
	}

	public void PlayCameraShake(CameraShakeIntensity intensity)
	{
		if (intensity == CameraShakeIntensity.None)
		{
			return;
		}
		if (Camera.main.gameObject.GetComponent<CameraShake>() == null)
		{
			Camera.main.gameObject.AddComponent<CameraShake>();
		}
		switch (intensity)
		{
			default:
				return;
			case CameraShakeIntensity.Small:
				Camera.main.gameObject.GetComponent<CameraShake>().Play(0.1f, 0.025f, 0.5f);
				return;
			case CameraShakeIntensity.Large:
				Camera.main.gameObject.GetComponent<CameraShake>().Play(0.3f, 0.1f, 0.75f);
				return;
		}
	}

	internal bool AllowCameraShake()
	{
		DebugCamera debugCamera = GetDebugCamera();
        if (debugCamera != null
            && debugCamera.enabled
            && !debugCamera.AllowCameraShake())
        {
            return false;
        }
        IsometricCamera isometricCamera = GetIsometricCamera();
        if (isometricCamera != null
            && isometricCamera.enabled
            && !isometricCamera.AllowCameraShake())
        {
            return false;
        }
        AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
        if (abilitiesCamera != null
            && abilitiesCamera.enabled
            && abilitiesCamera.IsMovingAutomatically())
        {
            return m_abilityAnimationsBetweenCamEvents > 0;
        }
        return true;
	}

	public void OnAnimationEvent(ActorData animatedActor, Object eventObject)
	{
        switch (eventObject.name)
        {
            case "CameraShakeSmallEvent":
                PlayCameraShake(CameraShakeIntensity.Small);
                break;
            case "CameraShakeLargeEvent":
                PlayCameraShake(CameraShakeIntensity.Large);
                break;
            case "CamStartEvent":
                m_abilityAnimationsBetweenCamEvents++;
                break;
            case "CamEndEvent":
                m_abilityAnimationsBetweenCamEvents--;
                if (m_abilityAnimationsBetweenCamEvents < 0)
                {
                    Log.Warning("Camera manger: ability animation CamStart CamEnd count  mismatch");
                    m_abilityAnimationsBetweenCamEvents = 0;
                }
                break;
        }
    }

	public bool OnAbilityAnimationStart(ActorData animatedActor, int animationIndex, Vector3 targetPos, bool requestCinematicCam, int cinematicRequested)
	{
		if (ShotSequence != null
		    || DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("DebugCamera")
		    || ((!requestCinematicCam || m_abilityCinematicState != AbilityCinematicState.Default) && m_abilityCinematicState != AbilityCinematicState.Always))
		{
			return false;
		}

		bool result = false;
		TauntCameraSet tauntCamSetData = animatedActor.m_tauntCamSetData;
		CameraShotSequence cameraShotSequence = null;
		CameraShot[] array = null;
		int altCamShotIndex = -1;
		if (tauntCamSetData != null)
		{
			for (int i = 0; i < tauntCamSetData.m_tauntCameraShotSequences.Length; i++)
			{
				CameraShotSequence tauntSequence = tauntCamSetData.m_tauntCameraShotSequences[i] as CameraShotSequence;
				if (tauntSequence == null
				    || tauntSequence.m_tauntNumber != cinematicRequested)
				{
					continue;
				}
				if (tauntSequence.m_animIndexTauntTrigger == animationIndex)
				{
					cameraShotSequence = tauntSequence;
					array = tauntSequence.m_cameraShots;
					break;
				}

				if (tauntSequence.m_alternateCameraShots == null
				    || tauntSequence.m_alternateCameraShots.Length <= 0)
				{
					continue;
				}

				for (int j = 0; j < tauntSequence.m_alternateCameraShots.Length; j++)
				{
					if (tauntSequence.m_alternateCameraShots[j].m_altAnimIndexTauntTrigger == animationIndex)
					{
						cameraShotSequence = tauntSequence;
						array = tauntSequence.m_alternateCameraShots[j].m_altCameraShots;
						altCamShotIndex = j;
						break;
					}
				}
			}
		}
		if (cameraShotSequence != null
		    && array != null
		    && array.Length > 0)
		{
			ShotSequence = cameraShotSequence;
			ShotSequence.Begin(animatedActor, altCamShotIndex);
			result = true;
			HUD_UI.Get().SetHUDVisibility(false, false);
			if (animatedActor.IsHumanControlled())
			{
				HUD_UI.Get().SetupTauntBanner(animatedActor);
			}
			HUD_UI.Get().SetTauntBannerVisibility(animatedActor.IsHumanControlled());
		}

		return result;
	}

	// removed in rogues
	internal void BeginFaceShot(CameraFaceShot faceShot, ActorData actor)
	{
		if (faceShot != null
		    && actor != null
		    && FaceCamera != null)
		{
			FaceShot = faceShot;
			faceShot.Begin(actor, FaceCamera);
		}
	}

	private bool IsCameraCenterKeyHeld()
	{
		return (GameFlowData.Get() == null || GameFlowData.Get().gameState != GameState.EndingGame)
		       && InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraCenterOnAction);
	}

	internal bool DoesAnimIndexTriggerTauntCamera(ActorData actor, int animIndex, int tauntNumber)
	{
		TauntCameraSet tauntCamSetData = actor.m_tauntCamSetData;
		if (tauntCamSetData == null || tauntCamSetData.m_tauntCameraShotSequences == null)
		{
			return false;
		}
		for (int i = 0; i < tauntCamSetData.m_tauntCameraShotSequences.Length; i++)
		{
			CameraShotSequence cameraShotSequence = tauntCamSetData.m_tauntCameraShotSequences[i] as CameraShotSequence;
			if (cameraShotSequence == null || cameraShotSequence.m_tauntNumber != tauntNumber)
			{
				continue;
			}
			if (cameraShotSequence.m_animIndexTauntTrigger == animIndex)
			{
				return true;
			}
			if (cameraShotSequence.m_alternateCameraShots == null || cameraShotSequence.m_alternateCameraShots.Length <= 0)
			{
				continue;
			}
			foreach (CameraShotSequence.AlternativeCamShotData data in cameraShotSequence.m_alternateCameraShots)
			{
				if (data.m_altAnimIndexTauntTrigger == animIndex)
				{
					return true;
				}
			}
		}
		return false;
	}

	internal void OnPlayerMovedCamera()
	{
		if (UIMainScreenPanel.Get() != null && UIMainScreenPanel.Get().m_autoCameraButton != null)
		{
			UIMainScreenPanel.Get().m_autoCameraButton.OnPlayerMovedCamera();
		}
	}

	public void Update()
	{
		// TODO HACK check if it is still needed
#if !SERVER
		bool isDebugCamera = DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("DebugCamera");
		if (isDebugCamera && !GetDebugCamera().enabled)
		{
			EnableDebugCamera();
		}
		else if (AppState.GetCurrent() == AppState_InGameDeployment.Get()
		         && GetFlyThroughCamera() != null) // removed in rogues
		{
			EnableFlyThroughCamera();
		}
		else if (!isDebugCamera && GetDebugCamera() != null && GetDebugCamera().enabled  // && GetDebugCamera() != null removed in rogues
		         || GetFlyThroughCamera() == null  // removed in rogues
		         || GetFlyThroughCamera().enabled)
		{
			EnableIsometricCamera();
		}

		if (SecondsRemainingToPauseForUserControl > 0f)
		{
			SecondsRemainingToPauseForUserControl -= Time.deltaTime;
		}
		if (GameFlowData.Get() == null
		    || Camera.main == null
		    || isDebugCamera
		    || GetFlyThroughCamera().enabled)
		{
			return;
		}
		if (ShotSequence != null && !ShotSequence.Update())
		{
			ShotSequence = null;
			HUD_UI.Get().SetHUDVisibility(true, true);
			HUD_UI.Get().SetTauntBannerVisibility(false);
		}
		
		// removed in rogues
		if (FaceShot != null && !FaceShot.Update(FaceCamera))
		{
			FaceCamera.gameObject.SetActive(false);
			FaceShot = null;
		}
		// end removed in rogues
		
		AccountPreferences accountPreferences = AccountPreferences.Get();
		if (UIMainScreenPanel.Get() != null
		    && UIMainScreenPanel.Get().m_autoCameraButton != null
		    && GameManager.Get().GameConfig.GameType != GameType.Tutorial)  // removed in rogues
		{
			bool toggleAutoCameraCenter = m_useCameraToggleKey && InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.CameraToggleAutoCenter);
			if (m_useRightClickToToggle
			    && !toggleAutoCameraCenter
			    && InterfaceManager.Get() != null
			    && InterfaceManager.Get().ShouldHandleMouseClick())
			{
				toggleAutoCameraCenter = Input.GetMouseButtonUp(1)
				        && GameFlowData.Get() != null
				        && GameFlowData.Get().gameState == GameState.BothTeams_Resolve
				        && (GameFlowData.Get().GetPause() || GameFlowData.Get().GetTimeInState() >= 1.5f);
			}
			if (toggleAutoCameraCenter)
			{
				bool autoCameraCenter = !accountPreferences.GetBool(BoolPreference.AutoCameraCenter);
				accountPreferences.SetBool(BoolPreference.AutoCameraCenter, autoCameraCenter);
				UIMainScreenPanel.Get().m_autoCameraButton.RefreshAutoCameraButton();
				AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
				if (autoCameraCenter && abilitiesCamera != null)
				{
					abilitiesCamera.OnAutoCenterCameraPreferenceSet();
				}
			}
		}

		if (ShotSequence == null)
		{
			m_useAbilitiesCameraOutOfCinematics = ShouldUseAbilitiesCameraOutOfCinematics();
			if (!isDebugCamera)
			{
				if (m_useAbilitiesCameraOutOfCinematics)
				{
					EnableAbilitiesCamera();
				}
				else
				{
					EnableIsometricCamera();
				}
			}
			if (IsCameraCenterKeyHeld())
			{
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				CameraManager cameraManager = Get();
				GameObject target = activeOwnedActorData != null
					? activeOwnedActorData.gameObject
					: null;
				cameraManager.SetTargetObject(target, CameraTargetReason.CameraCenterKeyHeld);
				if (ControlpadGameplay.Get() != null)
				{
					ControlpadGameplay.Get().OnCameraCenteredOnActor(activeOwnedActorData);
				}
			}
		}

		if (AudioListener != null)
		{
			if (ShotSequence != null && ShotSequence.Actor != null)
			{
				AudioListener.transform.position = ShotSequence.Actor.transform.position;
				return;
			}

			Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
			float num4 = Vector3.Dot(Vector3.down, Camera.main.transform.forward);
			if (num4 < 0.258819f)
			{
				Vector3 direction = Quaternion.AngleAxis(-75f, Camera.main.transform.right) * Vector3.down;
				ray = new Ray(Camera.main.transform.position, direction);
			}

			// float enter;
			if (!new Plane(Vector3.up, -Board.Get().BaselineHeight).Raycast(ray, out float enter))  // (ray, ref num) in rogues
			{
				enter = 3f;
			}

			AudioListener.transform.position = ray.GetPoint(enter);
		}
#endif
	}

	private bool ShouldUseAbilitiesCameraOutOfCinematics()
	{
#if SERVER
		// TODO HACK check if it is still needed
		return false;
#else
		if (Camera.main == null)
		{
			return false;
		}
		AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
		if (abilitiesCamera != null && abilitiesCamera.IsDisabledUntilSetTarget)
		{
			return false;
		}
		
		// reactor
		bool isInResolve = GameFlowData.Get().gameState == GameState.BothTeams_Resolve;
		ActionBufferPhase currentActionPhase = ServerClientUtils.GetCurrentActionPhase();
		bool abilitiesDone = currentActionPhase == ActionBufferPhase.AbilitiesWait
		             || currentActionPhase == ActionBufferPhase.Movement
		             || currentActionPhase == ActionBufferPhase.MovementChase;
		bool hasAnimationsInPhase = !TheatricsManager.Get().AbilityPhaseHasNoAnimations();
		if (!isInResolve)
		{
			return false;
		}
		if (!abilitiesDone && !hasAnimationsInPhase)
		{
			return false;
		}
		bool isTutorial = GameManager.Get() != null && GameManager.Get().GameConfig.GameType == GameType.Tutorial;
		if (!ShouldAutoCameraMove() && !Get().GetAbilitiesCamera().enabled)
		{
			return false;
		}
		if (isTutorial)
		{
			bool isMovementDone = currentActionPhase == ActionBufferPhase.MovementWait
			             || currentActionPhase == ActionBufferPhase.Done;
			return hasAnimationsInPhase && !abilitiesDone && !isMovementDone ;
		}
		return true;
		// rogues
		// bool flag = GameFlowData.Get().PlayerActionState > PlayerActionStateMachine.StateFlag.WaitingForInput;
		// bool hasAnimationsThisPhase = !TheatricsManager.Get().AbilityPhaseHasNoAnimations();
		// bool flag3 = hasAnimationsThisPhase && TheatricsManager.Get().GetPhaseToUpdate() != AbilityPriority.INVALID;
		// return flag && flag3 && TheatricsManager.Get().GetSetBoundCount() > 0 && (ShouldAutoCameraMove() || Get().GetAbilitiesCamera().enabled);
#endif
	}

	public static bool BoundSidesWithinDistance(
		Bounds currentBound,
		Bounds compareToBound,
		float mergeSizeThresh,
		out Vector3 maxBoundDiff,
		out Vector3 minBoundDiff)
	{
		Vector3 a = currentBound.center + currentBound.extents;
		Vector3 a2 = currentBound.center - currentBound.extents;
		Vector3 b = compareToBound.center + compareToBound.extents;
		Vector3 b2 = compareToBound.center - compareToBound.extents;
		maxBoundDiff = a - b;
		minBoundDiff = a2 - b2;
		return Mathf.Abs(maxBoundDiff.x) <= mergeSizeThresh
		       && Mathf.Abs(maxBoundDiff.z) <= mergeSizeThresh
		       && Mathf.Abs(minBoundDiff.x) <= mergeSizeThresh
		       && Mathf.Abs(minBoundDiff.z) <= mergeSizeThresh;
	}

	private void OnDrawGizmos()
	{
		if (!ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(CameraPositionBounds.center, CameraPositionBounds.size);
		Gizmos.DrawWireSphere(CameraPositionBounds.center, 1f);
		if (AudioListener != null)
		{
			Gizmos.DrawWireSphere(AudioListener.transform.position, 1f);
		}
	}

	public static bool ShouldDrawGizmosForCurrentCamera()
	{
		if (Application.isEditor)
		{
			LayerMask mask = 1 << LayerMask.NameToLayer("Default");
			return (Camera.current.cullingMask & mask) != 0;
		}
		return false;
	}

	public static void LogForDebugging(string str, CameraLogType cameraType = CameraLogType.None)
	{
		Debug.LogWarning(
			"<color=magenta>Camera " 
			+ (cameraType == CameraLogType.None ? string.Empty : "[" + cameraType + "]")
			+ " | </color>" + str + "\n@time= " + Time.time);
	}
}
