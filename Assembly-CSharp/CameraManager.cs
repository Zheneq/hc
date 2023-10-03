using CameraManagerInternal;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
		CtfFlagTurnedIn
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

	public GameObject m_faceCameraPrefab;

	public GameObject m_tauntBackgroundCameraPrefab;

	public bool m_useTauntBackground;

	internal AbilityCinematicState m_abilityCinematicState;

	private static CameraManager s_instance;

	private bool m_useAbilitiesCameraOutOfCinematics;

	private int m_abilityAnimationsBetweenCamEvents;

	private float m_secondsRemaingUnderUserControl = -1f;

	private List<CameraShot.CharacterToAnimParamSetActions> m_animParamSettersOnTurnTick = new List<CameraShot.CharacterToAnimParamSetActions>();

	private bool m_useCameraToggleKey = true;

	private bool m_useRightClickToToggle;

	public const float c_cameraToggleGracePeriod = 1.5f;

	private Bounds m_savedMoveCamBound;

	private int m_savedMoveCamBoundTurn = -1;

	internal float DefaultFOV
	{
		get;
		private set;
	}

	public Camera FaceCamera
	{
		get;
		private set;
	}

	internal GameObject AudioListener
	{
		get;
		private set;
	}

	internal CameraShotSequence ShotSequence
	{
		get;
		private set;
	}

	internal CameraFaceShot FaceShot
	{
		get;
		private set;
	}

	internal Bounds CameraPositionBounds
	{
		get;
		private set;
	}

	internal TauntBackgroundCamera TauntBackgroundCamera
	{
		get;
		private set;
	}

	internal float SecondsRemainingToPauseForUserControl
	{
		get
		{
			return m_secondsRemaingUnderUserControl;
		}
		set
		{
			m_secondsRemaingUnderUserControl = value;
		}
	}

	public bool UseCameraToggleKey => m_useCameraToggleKey;

	public static bool CamDebugTraceOn => false;

	internal bool InFaceShot(ActorData actor)
	{
		int result;
		if (FaceShot != null)
		{
			result = ((FaceShot.Actor == actor) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
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
		object result;
		if (ShotSequence == null)
		{
			result = null;
		}
		else
		{
			result = ShotSequence.Actor;
		}
		return (ActorData)result;
	}

	internal int GetCinematicActionAnimIndex()
	{
		return (!(ShotSequence == null)) ? ShotSequence.m_animIndexTauntTrigger : (-1);
	}

	internal bool ShouldHideBrushVfx()
	{
		int result;
		if (TauntBackgroundCamera != null)
		{
			result = ((ShotSequence != null) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
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
		if ((bool)GameFlowData.Get() && (bool)VisualsLoader.Get() && VisualsLoader.Get().LevelLoaded())
		{
			OnVisualSceneLoaded();
		}
		else
		{
			base.enabled = false;
		}
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.VisualSceneLoaded);
	}

	protected void Start()
	{
		if (!(Camera.main != null))
		{
			return;
		}
		while (true)
		{
			DefaultFOV = Camera.main.fieldOfView;
			return;
		}
	}

	private void ClearCameras()
	{
		if (Camera.main != null)
		{
			UnityEngine.Object.DestroyImmediate(Camera.main.gameObject);
		}
		if (FaceCamera != null)
		{
			UnityEngine.Object.DestroyImmediate(FaceCamera.gameObject);
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
		if (eventType != GameEventManager.EventType.VisualSceneLoaded)
		{
			return;
		}
		while (true)
		{
			OnVisualSceneLoaded();
			return;
		}
	}

	public void EnableOffFogOfWarEffect(bool enable)
	{
		if (!(Camera.main != null))
		{
			return;
		}
		while (true)
		{
			if (Camera.main.gameObject.GetComponent<FogOfWarEffect>() != null)
			{
				Camera.main.gameObject.GetComponent<FogOfWarEffect>().enabled = enable;
			}
			return;
		}
	}

	private void OnVisualSceneLoaded()
	{
		object obj;
		if (Camera.main == null)
		{
			obj = null;
		}
		else
		{
			obj = Camera.main.gameObject;
		}
		GameObject gameObject = (GameObject)obj;
		if (gameObject != null)
		{
			if (gameObject.GetComponent<IsometricCamera>() == null)
			{
				if (NetworkClient.active)
				{
					Log.Error("Environment scene is missing an instance of GameCamera.prefab, bloom may be active when loading into the level on Graphics Quality: Low until an instance is put in the environment scene.");
				}
				if (m_gameCameraPrefab != null)
				{
					UnityEngine.Object.DestroyImmediate(gameObject);
					UnityEngine.Object.Instantiate(m_gameCameraPrefab);
					gameObject = ((!(Camera.main == null)) ? Camera.main.gameObject : null);
				}
			}
		}
		if (gameObject == null)
		{
			if (NetworkClient.active)
			{
				Log.Error("Environment scene is missing an instance of GameCamera.prefab, bloom may be active when loading into the level on Graphics Quality: Low until an instance is put in the environment scene.");
			}
			if (m_gameCameraPrefab == null)
			{
				while (true)
				{
					throw new ApplicationException("There is no game camera prefab assigned in the CameraManager!");
				}
			}
			GameObject y = UnityEngine.Object.Instantiate(m_gameCameraPrefab);
			if (Camera.main == null)
			{
				Log.Error("Failed to switch to game camera; main camera is null");
			}
			else if (Camera.main.gameObject != y)
			{
				Log.Error("Failed to switch to game camera; main camera is '{0}'", Camera.main);
			}
		}
		UnityEngine.Object.DontDestroyOnLoad(Camera.main.gameObject);
		if (FaceCamera == null)
		{
			GameObject gameObject2 = UnityEngine.Object.Instantiate(m_faceCameraPrefab);
			UnityEngine.Object.DontDestroyOnLoad(gameObject2);
			object faceCamera;
			if (gameObject2 == null)
			{
				faceCamera = null;
			}
			else
			{
				faceCamera = gameObject2.GetComponent<Camera>();
			}
			FaceCamera = (Camera)faceCamera;
			FaceCamera.gameObject.SetActive(false);
		}
		if (Camera.main != null)
		{
			DefaultFOV = Camera.main.fieldOfView;
			RenderSettings.fog = false;
			if (NetworkClient.active)
			{
				if (m_tauntBackgroundCameraPrefab != null && m_useTauntBackground)
				{
					GameObject gameObject3 = UnityEngine.Object.Instantiate(m_tauntBackgroundCameraPrefab);
					TauntBackgroundCamera = gameObject3.GetComponent<TauntBackgroundCamera>();
					if (TauntBackgroundCamera == null)
					{
						Debug.LogError("Did not find taunt background camera component");
						UnityEngine.Object.Destroy(gameObject3);
					}
					else
					{
						gameObject3.SetActive(false);
					}
				}
			}
		}
		Board board = Board.Get();
		BoardSquare boardSquare = Board.Get().GetSquareFromIndex(Board.Get().GetMaxX() / 2, Board.Get().GetMaxY() / 2);
		if (boardSquare != null)
		{
			IsometricCamera isometricCamera = GetIsometricCamera();
			if (isometricCamera != null)
			{
				if (isometricCamera.enabled)
				{
					Vector3 position = boardSquare.gameObject.transform.position;
					float x = position.x;
					float y2 = Board.Get().BaselineHeight;
					Vector3 position2 = boardSquare.gameObject.transform.position;
					Vector3 pos = new Vector3(x, y2, position2.z);
					isometricCamera.SetTargetPosition(pos, 0f);
				}
			}
		}
		BoardSquare boardSquare2 = board.GetSquareFromIndex(0, 0);
		BoardSquare boardSquare3 = board.GetSquareFromIndex(board.GetMaxX() - 1, board.GetMaxY() - 1);
		Bounds cameraBounds = boardSquare2.CameraBounds;
		Bounds cameraBounds2 = boardSquare3.CameraBounds;
		Bounds cameraPositionBounds = cameraBounds;
		cameraPositionBounds.Encapsulate(cameraBounds2);
		CameraPositionBounds = cameraPositionBounds;
		if (AudioListener == null)
		{
			if (AudioListenerController.Get() != null)
			{
				AudioListener = AudioListenerController.Get().gameObject;
			}
		}
		base.enabled = true;
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
		object result;
		if (Camera.main != null)
		{
			result = Camera.main.GetComponent<FlyThroughCamera>();
		}
		else
		{
			result = null;
		}
		return (FlyThroughCamera)result;
	}

	internal DebugCamera GetDebugCamera()
	{
		object result;
		if (Camera.main != null)
		{
			result = Camera.main.GetComponent<DebugCamera>();
		}
		else
		{
			result = null;
		}
		return (DebugCamera)result;
	}

	internal IsometricCamera GetIsometricCamera()
	{
		return (!(Camera.main != null)) ? null : Camera.main.GetComponent<IsometricCamera>();
	}

	internal AbilitiesCamera GetAbilitiesCamera()
	{
		return (!(Camera.main != null)) ? null : Camera.main.GetComponent<AbilitiesCamera>();
	}

	internal FadeObjectsCameraComponent GetFadeObjectsCamera()
	{
		object result;
		if (Camera.main != null)
		{
			result = Camera.main.GetComponent<FadeObjectsCameraComponent>();
		}
		else
		{
			result = null;
		}
		return (FadeObjectsCameraComponent)result;
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
		if (!isometricCamera.enabled)
		{
			return;
		}
		while (true)
		{
			isometricCamera.OnTransitionOut();
			isometricCamera.enabled = false;
			return;
		}
	}

	internal void OnSpecialCameraShotBehaviorDisable(CameraTransitionType transitionInType)
	{
		Camera.main.fieldOfView = Get().DefaultFOV;
		m_useAbilitiesCameraOutOfCinematics = ShouldUseAbilitiesCameraOutOfCinematics();
		if (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("DebugCamera"))
		{
			return;
		}
		if (m_useAbilitiesCameraOutOfCinematics)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					EnableAbilitiesCamera(transitionInType);
					return;
				}
			}
		}
		EnableIsometricCamera(transitionInType);
	}

	public bool ShouldAutoCameraMove()
	{
		if (AccountPreferences.Get() != null)
		{
			if (AccountPreferences.Get().GetBool(BoolPreference.AutoCameraCenter))
			{
				goto IL_00c0;
			}
		}
		if (GameManager.Get() != null && GameManager.Get().GameConfig != null)
		{
			if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
			{
				goto IL_00c0;
			}
		}
		int result;
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			result = ((GameFlowData.Get().activeOwnedActorData.GetActorTurnSM().CurrentState == TurnStateEnum.PICKING_RESPAWN) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		goto IL_00c1;
		IL_00c1:
		return (byte)result != 0;
		IL_00c0:
		result = 1;
		goto IL_00c1;
	}

	internal void OnActionPhaseChange(ActionBufferPhase newPhase, bool requestAbilityCamera)
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
			Log.Warning("Camera manager: phase change to {0} with {1} abilities between camera start and end tags. Expected zero", newPhase.ToString(), m_abilityAnimationsBetweenCamEvents);
			m_abilityAnimationsBetweenCamEvents = 0;
		}
	}

	internal void SaveMovementCameraBound(Bounds bound)
	{
		m_savedMoveCamBound = bound;
		m_savedMoveCamBoundTurn = GameFlowData.Get().CurrentTurn;
	}

	internal void SetTargetForMovementIfNeeded()
	{
		if (!ShouldSetCameraForMovement() || GameFlowData.Get().CurrentTurn != m_savedMoveCamBoundTurn)
		{
			return;
		}
		while (true)
		{
			SetTarget(m_savedMoveCamBound);
			return;
		}
	}

	internal void SaveMovementCameraBoundForSpectator(Bounds bound)
	{
		if (m_savedMoveCamBoundTurn == GameFlowData.Get().CurrentTurn)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_savedMoveCamBound.Encapsulate(bound);
					return;
				}
			}
		}
		m_savedMoveCamBound = bound;
		m_savedMoveCamBoundTurn = GameFlowData.Get().CurrentTurn;
	}

	internal void SwitchCameraForMovement()
	{
		if (!ShouldSetCameraForMovement())
		{
			return;
		}
		while (true)
		{
			if (!(SecondsRemainingToPauseForUserControl <= 0f))
			{
				return;
			}
			while (true)
			{
				if (Get().ShouldAutoCameraMove())
				{
					while (true)
					{
						Get().OnActionPhaseChange(ActionBufferPhase.Movement, true);
						return;
					}
				}
				return;
			}
		}
	}

	private bool ShouldSetCameraForMovement()
	{
		int result;
		if (GameManager.Get() != null)
		{
			result = ((GameManager.Get().GameConfig.GameType != GameType.Tutorial) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal void AddAnimParamSetActions(CameraShot.CharacterToAnimParamSetActions animParamSetActions)
	{
		if (animParamSetActions == null)
		{
			return;
		}
		while (true)
		{
			m_animParamSettersOnTurnTick.Add(animParamSetActions);
			return;
		}
	}

	internal void OnTurnTick()
	{
		if (NetworkClient.active)
		{
			using (List<CameraShot.CharacterToAnimParamSetActions>.Enumerator enumerator = m_animParamSettersOnTurnTick.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CameraShot.CharacterToAnimParamSetActions current = enumerator.Current;
					if (current != null)
					{
						if (current.m_actor != null)
						{
							CameraShot.SetAnimParamsForActor(current.m_actor, current.m_animSetActions);
						}
					}
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
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
		if (abilitiesCamera.enabled)
		{
			return;
		}
		while (true)
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
			return;
		}
	}

	private void EnableIsometricCamera(CameraTransitionType transitionInType = CameraTransitionType.Move)
	{
		if (Camera.main == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
		IsometricCamera isometricCamera = GetIsometricCamera();
		DebugCamera debugCamera = GetDebugCamera();
		FlyThroughCamera flyThroughCamera = GetFlyThroughCamera();
		if (debugCamera != null)
		{
			debugCamera.enabled = false;
		}
		if (flyThroughCamera != null)
		{
			flyThroughCamera.enabled = false;
		}
		if (abilitiesCamera == null)
		{
			abilitiesCamera = Camera.main.gameObject.AddComponent<AbilitiesCamera>();
			abilitiesCamera.enabled = false;
			Log.Warning("Missing IsometricCamera component on main camera. Generating dynamically for now.");
		}
		m_useAbilitiesCameraOutOfCinematics = false;
		if (isometricCamera.enabled)
		{
			return;
		}
		while (true)
		{
			if (!(GameFlowData.Get().LocalPlayerData != null))
			{
				return;
			}
			while (true)
			{
				if (CamDebugTraceOn)
				{
					LogForDebugging("<color=white>Enable Isometric Camera</color>, transition type: " + transitionInType);
				}
				bool flag = false;
				if (abilitiesCamera.enabled)
				{
					flag = (abilitiesCamera.GetSecondsRemainingToPauseForUserControl() > 0f);
					abilitiesCamera.OnTransitionOut();
					abilitiesCamera.enabled = false;
				}
				isometricCamera.enabled = true;
				isometricCamera.OnTransitionIn(transitionInType);
				if (!(GameFlowData.Get().activeOwnedActorData != null))
				{
					return;
				}
				while (true)
				{
					GameObject targetObject;
					if (!abilitiesCamera.IsDisabledUntilSetTarget)
					{
						if (!flag)
						{
							targetObject = GameFlowData.Get().activeOwnedActorData.gameObject;
							goto IL_01ae;
						}
					}
					targetObject = null;
					goto IL_01ae;
					IL_01ae:
					isometricCamera.SetTargetObject(targetObject, CameraTargetReason.IsoCamEnabled);
					return;
				}
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
		if (SecondsRemainingToPauseForUserControl <= 0f)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					SetTargetObject(actor.gameObject, CameraTargetReason.ChangedActiveActor);
					return;
				}
			}
		}
		SetTargetObject(null, CameraTargetReason.ChangedActiveActor);
	}

	internal void OnActorMoved(ActorData actor)
	{
		FadeObjectsCameraComponent fadeObjectsCamera = GetFadeObjectsCamera();
		if (!(fadeObjectsCamera != null))
		{
			return;
		}
		while (true)
		{
			fadeObjectsCamera.MarkForResetVisibleObjects();
			return;
		}
	}

	internal void OnSelectedAbilityChanged(Ability ability)
	{
		FadeObjectsCameraComponent fadeObjectsCamera = GetFadeObjectsCamera();
		if (!(fadeObjectsCamera != null))
		{
			return;
		}
		while (true)
		{
			fadeObjectsCamera.ResetDesiredVisibleObjects();
			return;
		}
	}

	internal void OnNewTurnSMState()
	{
		FadeObjectsCameraComponent fadeObjectsCamera = GetFadeObjectsCamera();
		if (!(fadeObjectsCamera != null))
		{
			return;
		}
		while (true)
		{
			fadeObjectsCamera.ResetDesiredVisibleObjects();
			return;
		}
	}

	internal bool IsOnMainCamera(ActorData a)
	{
		if (a == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		Bounds cameraBounds = a.GetTravelBoardSquare().CameraBounds;
		return GeometryUtility.TestPlanesAABB(planes, cameraBounds);
	}

	internal void SetTargetObject(GameObject target, CameraTargetReason reason)
	{
		IsometricCamera isometricCamera = GetIsometricCamera();
		if (!(isometricCamera != null))
		{
			return;
		}
		while (true)
		{
			if (isometricCamera.enabled)
			{
				while (true)
				{
					isometricCamera.SetTargetObject(target, reason, false);
					return;
				}
			}
			return;
		}
	}

	internal void SetTargetObjectToMouse(GameObject target, CameraTargetReason reason)
	{
		if (CamDebugTraceOn)
		{
			object str;
			if (target != null)
			{
				str = target.name;
			}
			else
			{
				str = "NULL";
			}
			LogForDebugging("CameraManager.SetTargetObjectToMouse " + (string)str);
		}
		IsometricCamera isometricCamera = GetIsometricCamera();
		if (!(isometricCamera != null))
		{
			return;
		}
		while (true)
		{
			if (isometricCamera.enabled)
			{
				while (true)
				{
					isometricCamera.SetTargetObject(target, reason, true);
					return;
				}
			}
			return;
		}
	}

	public void SetTargetPosition(Vector3 pos, float easeInTime = 0f)
	{
		IsometricCamera isometricCamera = GetIsometricCamera();
		if (!(isometricCamera != null))
		{
			return;
		}
		while (true)
		{
			if (isometricCamera.enabled)
			{
				while (true)
				{
					isometricCamera.SetTargetPosition(pos, easeInTime);
					isometricCamera.SetTargetObject(null, CameraTargetReason.ReachedTargetObj);
					return;
				}
			}
			return;
		}
	}

	internal void SetTarget(Bounds bounds, bool quickerTransition = false, bool useLowPosition = false)
	{
		if (CamDebugTraceOn)
		{
			LogForDebugging("CameraManager.SetTarget " + bounds.ToString() + " | quicker transition: " + quickerTransition + " | useLowPosition: " + useLowPosition);
		}
		if (Camera.main == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
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
		while (true)
		{
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
				break;
			}
			while (true)
			{
				Camera.main.gameObject.GetComponent<CameraShake>().Play(0.3f, 0.1f, 0.75f);
				return;
			}
		}
	}

	internal bool AllowCameraShake()
	{
		DebugCamera debugCamera = GetDebugCamera();
        if (debugCamera != null && debugCamera.enabled && !debugCamera.AllowCameraShake())
        {
            return false;
        }
        IsometricCamera isometricCamera = GetIsometricCamera();
        if (isometricCamera != null && isometricCamera.enabled && !isometricCamera.AllowCameraShake())
        {
            return false;
        }
        AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
        if (abilitiesCamera != null && abilitiesCamera.enabled && abilitiesCamera.IsMovingAutomatically())
        {
            return m_abilityAnimationsBetweenCamEvents > 0;
        }
        return true;
	}

	public void OnAnimationEvent(ActorData animatedActor, UnityEngine.Object eventObject)
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
		bool result = false;
		int num;
		if (DebugParameters.Get() != null)
		{
			num = (DebugParameters.Get().GetParameterAsBool("DebugCamera") ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		if (ShotSequence == null)
		{
			if (!flag)
			{
				if (requestCinematicCam)
				{
					if (m_abilityCinematicState == AbilityCinematicState.Default)
					{
						goto IL_0098;
					}
				}
				if (m_abilityCinematicState == AbilityCinematicState.Always)
				{
					goto IL_0098;
				}
			}
		}
		goto IL_0235;
		IL_0098:
		TauntCameraSet tauntCamSetData = animatedActor.m_tauntCamSetData;
		CameraShotSequence cameraShotSequence = null;
		CameraShot[] array = null;
		int altCamShotIndex = -1;
		if (tauntCamSetData != null)
		{
			for (int i = 0; i < tauntCamSetData.m_tauntCameraShotSequences.Length; i++)
			{
				CameraShotSequence cameraShotSequence2 = tauntCamSetData.m_tauntCameraShotSequences[i] as CameraShotSequence;
				if (!(cameraShotSequence2 != null))
				{
					continue;
				}
				if (cameraShotSequence2.m_tauntNumber != cinematicRequested)
				{
					continue;
				}
				if (cameraShotSequence2.m_animIndexTauntTrigger == animationIndex)
				{
					cameraShotSequence = cameraShotSequence2;
					array = cameraShotSequence2.m_cameraShots;
					break;
				}
				if (cameraShotSequence2.m_alternateCameraShots == null || cameraShotSequence2.m_alternateCameraShots.Length <= 0)
				{
					continue;
				}
				for (int j = 0; j < cameraShotSequence2.m_alternateCameraShots.Length; j++)
				{
					if (cameraShotSequence2.m_alternateCameraShots[j].m_altAnimIndexTauntTrigger == animationIndex)
					{
						cameraShotSequence = cameraShotSequence2;
						array = cameraShotSequence2.m_alternateCameraShots[j].m_altCameraShots;
						altCamShotIndex = j;
						break;
					}
				}
			}
		}
		if (cameraShotSequence != null)
		{
			if (array != null)
			{
				if (array.Length > 0)
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
			}
		}
		goto IL_0235;
		IL_0235:
		return result;
	}

	internal void BeginFaceShot(CameraFaceShot faceShot, ActorData actor)
	{
		if (faceShot == null)
		{
			return;
		}
		while (true)
		{
			if (actor != null && FaceCamera != null)
			{
				FaceShot = faceShot;
				faceShot.Begin(actor, FaceCamera);
			}
			return;
		}
	}

	private bool IsCameraCenterKeyHeld()
	{
		bool flag = false;
		if (GameFlowData.Get() != null && GameFlowData.Get().gameState == GameState.EndingGame)
		{
			flag = true;
		}
		int result;
		if (!flag)
		{
			result = (InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraCenterOnAction) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal bool DoesAnimIndexTriggerTauntCamera(ActorData actor, int animIndex, int tauntNumber)
	{
		TauntCameraSet tauntCamSetData = actor.m_tauntCamSetData;
		if (tauntCamSetData != null)
		{
			if (tauntCamSetData.m_tauntCameraShotSequences != null)
			{
				for (int i = 0; i < tauntCamSetData.m_tauntCameraShotSequences.Length; i++)
				{
					CameraShotSequence cameraShotSequence = tauntCamSetData.m_tauntCameraShotSequences[i] as CameraShotSequence;
					if (!(cameraShotSequence != null))
					{
						continue;
					}
					if (cameraShotSequence.m_tauntNumber != tauntNumber)
					{
						continue;
					}
					if (cameraShotSequence.m_animIndexTauntTrigger == animIndex)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
					if (cameraShotSequence.m_alternateCameraShots == null)
					{
						continue;
					}
					if (cameraShotSequence.m_alternateCameraShots.Length <= 0)
					{
						continue;
					}
					for (int j = 0; j < cameraShotSequence.m_alternateCameraShots.Length; j++)
					{
						if (cameraShotSequence.m_alternateCameraShots[j].m_altAnimIndexTauntTrigger != animIndex)
						{
							continue;
						}
						while (true)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	internal void OnPlayerMovedCamera()
	{
		if (!(UIMainScreenPanel.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (UIMainScreenPanel.Get().m_autoCameraButton != null)
			{
				while (true)
				{
					UIMainScreenPanel.Get().m_autoCameraButton.OnPlayerMovedCamera();
					return;
				}
			}
			return;
		}
	}

	public void Update()
	{
		int num;
		if (DebugParameters.Get() != null)
		{
			num = (DebugParameters.Get().GetParameterAsBool("DebugCamera") ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		if (flag)
		{
			if (!GetDebugCamera().enabled)
			{
				EnableDebugCamera();
				goto IL_012a;
			}
		}
		if (AppState.GetCurrent() == AppState_InGameDeployment.Get())
		{
			if (GetFlyThroughCamera() != null)
			{
				EnableFlyThroughCamera();
				goto IL_012a;
			}
		}
		if (!flag)
		{
			if (GetDebugCamera() != null)
			{
				if (GetDebugCamera().enabled)
				{
					goto IL_0123;
				}
			}
		}
		if (!(GetFlyThroughCamera() == null))
		{
			if (!GetFlyThroughCamera().enabled)
			{
				goto IL_012a;
			}
		}
		goto IL_0123;
		IL_012a:
		if (SecondsRemainingToPauseForUserControl > 0f)
		{
			SecondsRemainingToPauseForUserControl -= Time.deltaTime;
		}
		if (GameFlowData.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (Camera.main == null)
			{
				return;
			}
			while (true)
			{
				if (flag)
				{
					return;
				}
				if (GetFlyThroughCamera().enabled)
				{
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
				if (ShotSequence != null)
				{
					if (!ShotSequence.Update())
					{
						ShotSequence = null;
						HUD_UI.Get().SetHUDVisibility(true, true);
						HUD_UI.Get().SetTauntBannerVisibility(false);
					}
				}
				if (FaceShot != null && !FaceShot.Update(FaceCamera))
				{
					FaceCamera.gameObject.SetActive(false);
					FaceShot = null;
				}
				AccountPreferences accountPreferences = AccountPreferences.Get();
				bool flag2;
				int num3;
				if (UIMainScreenPanel.Get() != null && UIMainScreenPanel.Get().m_autoCameraButton != null && GameManager.Get().GameConfig.GameType != GameType.Tutorial)
				{
					int num2;
					if (m_useCameraToggleKey)
					{
						num2 = (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.CameraToggleAutoCenter) ? 1 : 0);
					}
					else
					{
						num2 = 0;
					}
					flag2 = ((byte)num2 != 0);
					if (m_useRightClickToToggle)
					{
						if (!flag2)
						{
							if (InterfaceManager.Get() != null)
							{
								if (InterfaceManager.Get().ShouldHandleMouseClick())
								{
									if (Input.GetMouseButtonUp(1))
									{
										if (GameFlowData.Get() != null)
										{
											if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
											{
												num3 = ((GameFlowData.Get().GetPause() || GameFlowData.Get().GetTimeInState() >= 1.5f) ? 1 : 0);
												goto IL_036b;
											}
										}
									}
									num3 = 0;
									goto IL_036b;
								}
							}
						}
					}
					goto IL_036c;
				}
				goto IL_03cf;
				IL_03cf:
				if (ShotSequence == null)
				{
					m_useAbilitiesCameraOutOfCinematics = ShouldUseAbilitiesCameraOutOfCinematics();
					if (!flag)
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
						object target;
						if (activeOwnedActorData == null)
						{
							target = null;
						}
						else
						{
							target = activeOwnedActorData.gameObject;
						}
						cameraManager.SetTargetObject((GameObject)target, CameraTargetReason.CameraCenterKeyHeld);
						if (ControlpadGameplay.Get() != null)
						{
							ControlpadGameplay.Get().OnCameraCenteredOnActor(activeOwnedActorData);
						}
					}
				}
				if (!(AudioListener != null))
				{
					return;
				}
				if (ShotSequence != null)
				{
					if (ShotSequence.Actor != null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								AudioListener.transform.position = ShotSequence.Actor.transform.position;
								return;
							}
						}
					}
				}
				Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
				float num4 = Vector3.Dot(Vector3.down, Camera.main.transform.forward);
				if (num4 < 0.258819f)
				{
					Vector3 direction = Quaternion.AngleAxis(-75f, Camera.main.transform.right) * Vector3.down;
					ray = new Ray(Camera.main.transform.position, direction);
				}
				if (!new Plane(Vector3.up, -Board.Get().BaselineHeight).Raycast(ray, out float enter))
				{
					enter = 3f;
				}
				AudioListener.transform.position = ray.GetPoint(enter);
				return;
				IL_036c:
				if (flag2)
				{
					bool flag3 = !accountPreferences.GetBool(BoolPreference.AutoCameraCenter);
					accountPreferences.SetBool(BoolPreference.AutoCameraCenter, flag3);
					UIMainScreenPanel.Get().m_autoCameraButton.RefreshAutoCameraButton();
					AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
					if (flag3)
					{
						if (abilitiesCamera != null)
						{
							abilitiesCamera.OnAutoCenterCameraPreferenceSet();
						}
					}
				}
				goto IL_03cf;
				IL_036b:
				flag2 = ((byte)num3 != 0);
				goto IL_036c;
			}
		}
		IL_0123:
		EnableIsometricCamera();
		goto IL_012a;
	}

	private bool ShouldUseAbilitiesCameraOutOfCinematics()
	{
		if (Camera.main == null)
		{
			return false;
		}
		AbilitiesCamera abilitiesCamera = GetAbilitiesCamera();
		if (abilitiesCamera != null && abilitiesCamera.IsDisabledUntilSetTarget)
		{
			return false;
		}
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
	}

	public static bool BoundSidesWithinDistance(Bounds currentBound, Bounds compareToBound, float mergeSizeThresh, out Vector3 maxBoundDiff, out Vector3 minBoundDiff)
	{
		bool result = false;
		Vector3 a = currentBound.center + currentBound.extents;
		Vector3 a2 = currentBound.center - currentBound.extents;
		Vector3 b = compareToBound.center + compareToBound.extents;
		Vector3 b2 = compareToBound.center - compareToBound.extents;
		maxBoundDiff = a - b;
		minBoundDiff = a2 - b2;
		if (Mathf.Abs(maxBoundDiff.x) <= mergeSizeThresh)
		{
			if (Mathf.Abs(maxBoundDiff.z) <= mergeSizeThresh)
			{
				if (Mathf.Abs(minBoundDiff.x) <= mergeSizeThresh)
				{
					if (Mathf.Abs(minBoundDiff.z) <= mergeSizeThresh)
					{
						result = true;
					}
				}
			}
		}
		return result;
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
		if (!(AudioListener != null))
		{
			return;
		}
		while (true)
		{
			Gizmos.DrawWireSphere(AudioListener.transform.position, 1f);
			return;
		}
	}

	public static bool ShouldDrawGizmosForCurrentCamera()
	{
		if (Application.isEditor)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					LayerMask mask = 1 << LayerMask.NameToLayer("Default");
					return (Camera.current.cullingMask & (int)mask) != 0;
				}
				}
			}
		}
		return false;
	}

	public static void LogForDebugging(string str, CameraLogType cameraType = CameraLogType.None)
	{
		string text;
		if (cameraType == CameraLogType.None)
		{
			text = string.Empty;
		}
		else
		{
			text = "[" + cameraType.ToString() + "]";
		}
		string text2 = text;
		Debug.LogWarning("<color=magenta>Camera " + text2 + " | </color>" + str + "\n@time= " + Time.time);
	}
}
