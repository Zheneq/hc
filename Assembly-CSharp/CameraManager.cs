using System;
using System.Collections.Generic;
using CameraManagerInternal;
using UnityEngine;
using UnityEngine.Networking;

public class CameraManager : MonoBehaviour, IGameEventListener
{
	public GameObject m_gameCameraPrefab;

	public GameObject m_faceCameraPrefab;

	public GameObject m_tauntBackgroundCameraPrefab;

	public bool m_useTauntBackground;

	internal CameraManager.AbilityCinematicState m_abilityCinematicState;

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

	internal float DefaultFOV { get; private set; }

	public Camera FaceCamera { get; private set; }

	internal GameObject AudioListener { get; private set; }

	internal CameraShotSequence ShotSequence { get; private set; }

	internal CameraFaceShot FaceShot { get; private set; }

	internal bool InFaceShot(ActorData actor)
	{
		bool result;
		if (this.FaceShot != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.InFaceShot(ActorData)).MethodHandle;
			}
			result = (this.FaceShot.Actor == actor);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal bool WillRespondToInput()
	{
		return this.GetIsometricCamera().enabled;
	}

	internal bool IsPlayingShotSequence()
	{
		return this.ShotSequence != null;
	}

	internal bool InCinematic()
	{
		return this.ShotSequence != null;
	}

	internal ActorData GetCinematicTargetActor()
	{
		ActorData result;
		if (this.ShotSequence == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.GetCinematicTargetActor()).MethodHandle;
			}
			result = null;
		}
		else
		{
			result = this.ShotSequence.Actor;
		}
		return result;
	}

	internal int GetCinematicActionAnimIndex()
	{
		return (!(this.ShotSequence == null)) ? this.ShotSequence.m_animIndexTauntTrigger : -1;
	}

	internal Bounds CameraPositionBounds { get; private set; }

	internal TauntBackgroundCamera TauntBackgroundCamera { get; private set; }

	internal bool ShouldHideBrushVfx()
	{
		bool result;
		if (this.TauntBackgroundCamera != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.ShouldHideBrushVfx()).MethodHandle;
			}
			result = (this.ShotSequence != null);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal static CameraManager Get()
	{
		return CameraManager.s_instance;
	}

	internal float SecondsRemainingToPauseForUserControl
	{
		get
		{
			return this.m_secondsRemaingUnderUserControl;
		}
		set
		{
			this.m_secondsRemaingUnderUserControl = value;
		}
	}

	public bool UseCameraToggleKey
	{
		get
		{
			return this.m_useCameraToggleKey;
		}
	}

	protected void Awake()
	{
		if (CameraManager.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.Awake()).MethodHandle;
			}
			Debug.LogError("CameraManager instance was not null on Awake(), please check to make sure there is only 1 instance of GameSceneSingletons object");
		}
		CameraManager.s_instance = this;
		if (GameFlowData.Get() && VisualsLoader.Get() && VisualsLoader.Get().LevelLoaded())
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
			this.OnVisualSceneLoaded();
		}
		else
		{
			base.enabled = false;
		}
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.VisualSceneLoaded);
	}

	protected void Start()
	{
		if (Camera.main != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.Start()).MethodHandle;
			}
			this.DefaultFOV = Camera.main.fieldOfView;
		}
	}

	private void ClearCameras()
	{
		if (Camera.main != null)
		{
			UnityEngine.Object.DestroyImmediate(Camera.main.gameObject);
		}
		if (this.FaceCamera != null)
		{
			UnityEngine.Object.DestroyImmediate(this.FaceCamera.gameObject);
		}
	}

	private void OnDestroy()
	{
		this.ClearCameras();
		CameraManager.s_instance = null;
		if (GameEventManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnDestroy()).MethodHandle;
			}
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.VisualSceneLoaded);
		}
		CameraManager.s_instance = null;
	}

	void IGameEventListener.OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.VisualSceneLoaded)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.IGameEventListener.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
			this.OnVisualSceneLoaded();
		}
	}

	public void EnableOffFogOfWarEffect(bool enable)
	{
		if (Camera.main != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.EnableOffFogOfWarEffect(bool)).MethodHandle;
			}
			if (Camera.main.gameObject.GetComponent<FogOfWarEffect>() != null)
			{
				Camera.main.gameObject.GetComponent<FogOfWarEffect>().enabled = enable;
			}
		}
	}

	private void OnVisualSceneLoaded()
	{
		GameObject gameObject;
		if (Camera.main == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnVisualSceneLoaded()).MethodHandle;
			}
			gameObject = null;
		}
		else
		{
			gameObject = Camera.main.gameObject;
		}
		GameObject gameObject2 = gameObject;
		if (gameObject2 != null)
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
			if (gameObject2.GetComponent<IsometricCamera>() == null)
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
				if (NetworkClient.active)
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
					Log.Error("Environment scene is missing an instance of GameCamera.prefab, bloom may be active when loading into the level on Graphics Quality: Low until an instance is put in the environment scene.", new object[0]);
				}
				if (this.m_gameCameraPrefab != null)
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
					UnityEngine.Object.DestroyImmediate(gameObject2);
					UnityEngine.Object.Instantiate<GameObject>(this.m_gameCameraPrefab);
					gameObject2 = ((!(Camera.main == null)) ? Camera.main.gameObject : null);
				}
			}
		}
		if (gameObject2 == null)
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
			if (NetworkClient.active)
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
				Log.Error("Environment scene is missing an instance of GameCamera.prefab, bloom may be active when loading into the level on Graphics Quality: Low until an instance is put in the environment scene.", new object[0]);
			}
			if (this.m_gameCameraPrefab == null)
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
				throw new ApplicationException("There is no game camera prefab assigned in the CameraManager!");
			}
			GameObject y = UnityEngine.Object.Instantiate<GameObject>(this.m_gameCameraPrefab);
			if (Camera.main == null)
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
				Log.Error("Failed to switch to game camera; main camera is null", new object[0]);
			}
			else if (Camera.main.gameObject != y)
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
				Log.Error("Failed to switch to game camera; main camera is '{0}'", new object[]
				{
					Camera.main
				});
			}
		}
		UnityEngine.Object.DontDestroyOnLoad(Camera.main.gameObject);
		if (this.FaceCamera == null)
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
			GameObject gameObject3 = UnityEngine.Object.Instantiate<GameObject>(this.m_faceCameraPrefab);
			UnityEngine.Object.DontDestroyOnLoad(gameObject3);
			Camera faceCamera;
			if (gameObject3 == null)
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
				faceCamera = null;
			}
			else
			{
				faceCamera = gameObject3.GetComponent<Camera>();
			}
			this.FaceCamera = faceCamera;
			this.FaceCamera.gameObject.SetActive(false);
		}
		if (Camera.main != null)
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
			this.DefaultFOV = Camera.main.fieldOfView;
			RenderSettings.fog = false;
			if (NetworkClient.active)
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
				if (this.m_tauntBackgroundCameraPrefab != null && this.m_useTauntBackground)
				{
					GameObject gameObject4 = UnityEngine.Object.Instantiate<GameObject>(this.m_tauntBackgroundCameraPrefab);
					this.TauntBackgroundCamera = gameObject4.GetComponent<TauntBackgroundCamera>();
					if (this.TauntBackgroundCamera == null)
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
						Debug.LogError("Did not find taunt background camera component");
						UnityEngine.Object.Destroy(gameObject4);
					}
					else
					{
						gameObject4.SetActive(false);
					}
				}
			}
		}
		Board board = Board.Get();
		BoardSquare boardSquare = Board.Get().GetBoardSquare(Board.Get().GetMaxX() / 2, Board.Get().GetMaxY() / 2);
		if (boardSquare != null)
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
			IsometricCamera isometricCamera = this.GetIsometricCamera();
			if (isometricCamera != null)
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
				if (isometricCamera.enabled)
				{
					Vector3 pos = new Vector3(boardSquare.gameObject.transform.position.x, (float)Board.Get().BaselineHeight, boardSquare.gameObject.transform.position.z);
					isometricCamera.SetTargetPosition(pos, 0f);
				}
			}
		}
		BoardSquare boardSquare2 = board.GetBoardSquare(0, 0);
		BoardSquare boardSquare3 = board.GetBoardSquare(board.GetMaxX() - 1, board.GetMaxY() - 1);
		Bounds cameraBounds = boardSquare2.CameraBounds;
		Bounds cameraBounds2 = boardSquare3.CameraBounds;
		Bounds cameraPositionBounds = cameraBounds;
		cameraPositionBounds.Encapsulate(cameraBounds2);
		this.CameraPositionBounds = cameraPositionBounds;
		if (this.AudioListener == null)
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
			if (AudioListenerController.Get() != null)
			{
				this.AudioListener = AudioListenerController.Get().gameObject;
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
		FlyThroughCamera result;
		if (Camera.main != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.GetFlyThroughCamera()).MethodHandle;
			}
			result = Camera.main.GetComponent<FlyThroughCamera>();
		}
		else
		{
			result = null;
		}
		return result;
	}

	internal DebugCamera GetDebugCamera()
	{
		DebugCamera result;
		if (Camera.main != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.GetDebugCamera()).MethodHandle;
			}
			result = Camera.main.GetComponent<DebugCamera>();
		}
		else
		{
			result = null;
		}
		return result;
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
		FadeObjectsCameraComponent result;
		if (Camera.main != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.GetFadeObjectsCamera()).MethodHandle;
			}
			result = Camera.main.GetComponent<FadeObjectsCameraComponent>();
		}
		else
		{
			result = null;
		}
		return result;
	}

	internal void OnSpecialCameraShotBehaviorEnable(CameraTransitionType transitionInType)
	{
		IsometricCamera isometricCamera = this.GetIsometricCamera();
		AbilitiesCamera abilitiesCamera = this.GetAbilitiesCamera();
		if (abilitiesCamera.enabled)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnSpecialCameraShotBehaviorEnable(CameraTransitionType)).MethodHandle;
			}
			abilitiesCamera.OnTransitionOut();
			abilitiesCamera.enabled = false;
		}
		if (isometricCamera.enabled)
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
			isometricCamera.OnTransitionOut();
			isometricCamera.enabled = false;
		}
	}

	internal void OnSpecialCameraShotBehaviorDisable(CameraTransitionType transitionInType)
	{
		Camera.main.fieldOfView = CameraManager.Get().DefaultFOV;
		this.m_useAbilitiesCameraOutOfCinematics = this.ShouldUseAbilitiesCameraOutOfCinematics();
		if (DebugParameters.Get() == null || !DebugParameters.Get().GetParameterAsBool("DebugCamera"))
		{
			if (this.m_useAbilitiesCameraOutOfCinematics)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnSpecialCameraShotBehaviorDisable(CameraTransitionType)).MethodHandle;
				}
				this.EnableAbilitiesCamera(transitionInType);
			}
			else
			{
				this.EnableIsometricCamera(transitionInType);
			}
		}
	}

	public bool ShouldAutoCameraMove()
	{
		if (AccountPreferences.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.ShouldAutoCameraMove()).MethodHandle;
			}
			if (AccountPreferences.Get().GetBool(BoolPreference.AutoCameraCenter))
			{
				goto IL_C0;
			}
		}
		if (GameManager.Get() != null && GameManager.Get().GameConfig != null)
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
			if (GameManager.Get().GameConfig.GameType == GameType.Tutorial)
			{
				goto IL_C0;
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
		int result;
		if (GameFlowData.Get().activeOwnedActorData != null)
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
			result = ((GameFlowData.Get().activeOwnedActorData.GetActorTurnSM().CurrentState == TurnStateEnum.PICKING_RESPAWN) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return result != 0;
		IL_C0:
		result = 1;
		return result != 0;
	}

	internal void OnActionPhaseChange(ActionBufferPhase newPhase, bool requestAbilityCamera)
	{
		if (DebugParameters.Get() == null || !DebugParameters.Get().GetParameterAsBool("DebugCamera"))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnActionPhaseChange(ActionBufferPhase, bool)).MethodHandle;
			}
			if (requestAbilityCamera && this.ShouldUseAbilitiesCameraOutOfCinematics())
			{
				this.EnableAbilitiesCamera(CameraTransitionType.Move);
			}
			else
			{
				this.EnableIsometricCamera(CameraTransitionType.Move);
			}
		}
		FadeObjectsCameraComponent fadeObjectsCamera = this.GetFadeObjectsCamera();
		if (fadeObjectsCamera != null)
		{
			fadeObjectsCamera.ResetDesiredVisibleObjects();
		}
		if (this.m_abilityAnimationsBetweenCamEvents > 0)
		{
			Log.Warning("Camera manager: phase change to {0} with {1} abilities between camera start and end tags. Expected zero", new object[]
			{
				newPhase.ToString(),
				this.m_abilityAnimationsBetweenCamEvents
			});
			this.m_abilityAnimationsBetweenCamEvents = 0;
		}
	}

	internal void SaveMovementCameraBound(Bounds bound)
	{
		this.m_savedMoveCamBound = bound;
		this.m_savedMoveCamBoundTurn = GameFlowData.Get().CurrentTurn;
	}

	internal void SetTargetForMovementIfNeeded()
	{
		if (this.ShouldSetCameraForMovement() && GameFlowData.Get().CurrentTurn == this.m_savedMoveCamBoundTurn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.SetTargetForMovementIfNeeded()).MethodHandle;
			}
			this.SetTarget(this.m_savedMoveCamBound, false, false);
		}
	}

	internal void SaveMovementCameraBoundForSpectator(Bounds bound)
	{
		if (this.m_savedMoveCamBoundTurn == GameFlowData.Get().CurrentTurn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.SaveMovementCameraBoundForSpectator(Bounds)).MethodHandle;
			}
			this.m_savedMoveCamBound.Encapsulate(bound);
		}
		else
		{
			this.m_savedMoveCamBound = bound;
			this.m_savedMoveCamBoundTurn = GameFlowData.Get().CurrentTurn;
		}
	}

	internal void SwitchCameraForMovement()
	{
		if (this.ShouldSetCameraForMovement())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.SwitchCameraForMovement()).MethodHandle;
			}
			if (this.SecondsRemainingToPauseForUserControl <= 0f)
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
				if (CameraManager.Get().ShouldAutoCameraMove())
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
					CameraManager.Get().OnActionPhaseChange(ActionBufferPhase.Movement, true);
				}
			}
		}
	}

	private bool ShouldSetCameraForMovement()
	{
		bool result;
		if (GameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.ShouldSetCameraForMovement()).MethodHandle;
			}
			result = (GameManager.Get().GameConfig.GameType != GameType.Tutorial);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal void AddAnimParamSetActions(CameraShot.CharacterToAnimParamSetActions animParamSetActions)
	{
		if (animParamSetActions != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.AddAnimParamSetActions(CameraShot.CharacterToAnimParamSetActions)).MethodHandle;
			}
			this.m_animParamSettersOnTurnTick.Add(animParamSetActions);
		}
	}

	internal void OnTurnTick()
	{
		if (NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnTurnTick()).MethodHandle;
			}
			using (List<CameraShot.CharacterToAnimParamSetActions>.Enumerator enumerator = this.m_animParamSettersOnTurnTick.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CameraShot.CharacterToAnimParamSetActions characterToAnimParamSetActions = enumerator.Current;
					if (characterToAnimParamSetActions != null)
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
						if (characterToAnimParamSetActions.m_actor != null)
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
							CameraShot.SetAnimParamsForActor(characterToAnimParamSetActions.m_actor, characterToAnimParamSetActions.m_animSetActions);
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
		}
		this.m_animParamSettersOnTurnTick.Clear();
	}

	private void EnableFlyThroughCamera()
	{
		AbilitiesCamera abilitiesCamera = this.GetAbilitiesCamera();
		IsometricCamera isometricCamera = this.GetIsometricCamera();
		DebugCamera debugCamera = this.GetDebugCamera();
		FlyThroughCamera flyThroughCamera = this.GetFlyThroughCamera();
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
		AbilitiesCamera abilitiesCamera = this.GetAbilitiesCamera();
		IsometricCamera isometricCamera = this.GetIsometricCamera();
		DebugCamera debugCamera = this.GetDebugCamera();
		FlyThroughCamera flyThroughCamera = this.GetFlyThroughCamera();
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.EnableAbilitiesCamera(CameraTransitionType)).MethodHandle;
			}
			return;
		}
		AbilitiesCamera abilitiesCamera = this.GetAbilitiesCamera();
		IsometricCamera isometricCamera = this.GetIsometricCamera();
		DebugCamera debugCamera = this.GetDebugCamera();
		FlyThroughCamera flyThroughCamera = this.GetFlyThroughCamera();
		if (abilitiesCamera == null)
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
			abilitiesCamera = Camera.main.gameObject.AddComponent<AbilitiesCamera>();
			abilitiesCamera.enabled = false;
			Log.Warning("Missing AbilitiesCamera component on main camera. Generating dynamically for now.", new object[0]);
		}
		this.m_useAbilitiesCameraOutOfCinematics = true;
		debugCamera.enabled = false;
		flyThroughCamera.enabled = false;
		if (!abilitiesCamera.enabled)
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
			if (CameraManager.CamDebugTraceOn)
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
				CameraManager.LogForDebugging("<color=white>Enable Abilities Camera</color>, transition type: " + transitionInType, CameraManager.CameraLogType.None);
			}
			if (isometricCamera.enabled)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.EnableIsometricCamera(CameraTransitionType)).MethodHandle;
			}
			return;
		}
		AbilitiesCamera abilitiesCamera = this.GetAbilitiesCamera();
		IsometricCamera isometricCamera = this.GetIsometricCamera();
		DebugCamera debugCamera = this.GetDebugCamera();
		FlyThroughCamera flyThroughCamera = this.GetFlyThroughCamera();
		if (debugCamera != null)
		{
			debugCamera.enabled = false;
		}
		if (flyThroughCamera != null)
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
			flyThroughCamera.enabled = false;
		}
		if (abilitiesCamera == null)
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
			abilitiesCamera = Camera.main.gameObject.AddComponent<AbilitiesCamera>();
			abilitiesCamera.enabled = false;
			Log.Warning("Missing IsometricCamera component on main camera. Generating dynamically for now.", new object[0]);
		}
		this.m_useAbilitiesCameraOutOfCinematics = false;
		if (!isometricCamera.enabled)
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
			if (GameFlowData.Get().LocalPlayerData != null)
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
				if (CameraManager.CamDebugTraceOn)
				{
					CameraManager.LogForDebugging("<color=white>Enable Isometric Camera</color>, transition type: " + transitionInType, CameraManager.CameraLogType.None);
				}
				bool flag = false;
				if (abilitiesCamera.enabled)
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
					flag = (abilitiesCamera.GetSecondsRemainingToPauseForUserControl() > 0f);
					abilitiesCamera.OnTransitionOut();
					abilitiesCamera.enabled = false;
				}
				isometricCamera.enabled = true;
				isometricCamera.OnTransitionIn(transitionInType);
				if (GameFlowData.Get().activeOwnedActorData != null)
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
					GameObject targetObject;
					if (!abilitiesCamera.IsDisabledUntilSetTarget)
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
						if (!flag)
						{
							targetObject = GameFlowData.Get().activeOwnedActorData.gameObject;
							goto IL_1AE;
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
					targetObject = null;
					IL_1AE:
					isometricCamera.SetTargetObject(targetObject, CameraManager.CameraTargetReason.IsoCamEnabled);
				}
			}
		}
	}

	internal void OnActiveOwnedActorChange(ActorData actor)
	{
		FadeObjectsCameraComponent fadeObjectsCamera = this.GetFadeObjectsCamera();
		if (fadeObjectsCamera != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnActiveOwnedActorChange(ActorData)).MethodHandle;
			}
			fadeObjectsCamera.ResetDesiredVisibleObjects();
		}
		if (this.SecondsRemainingToPauseForUserControl <= 0f)
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
			this.SetTargetObject(actor.gameObject, CameraManager.CameraTargetReason.ChangedActiveActor);
		}
		else
		{
			this.SetTargetObject(null, CameraManager.CameraTargetReason.ChangedActiveActor);
		}
	}

	internal void OnActorMoved(ActorData actor)
	{
		FadeObjectsCameraComponent fadeObjectsCamera = this.GetFadeObjectsCamera();
		if (fadeObjectsCamera != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnActorMoved(ActorData)).MethodHandle;
			}
			fadeObjectsCamera.MarkForResetVisibleObjects();
		}
	}

	internal void OnSelectedAbilityChanged(Ability ability)
	{
		FadeObjectsCameraComponent fadeObjectsCamera = this.GetFadeObjectsCamera();
		if (fadeObjectsCamera != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnSelectedAbilityChanged(Ability)).MethodHandle;
			}
			fadeObjectsCamera.ResetDesiredVisibleObjects();
		}
	}

	internal void OnNewTurnSMState()
	{
		FadeObjectsCameraComponent fadeObjectsCamera = this.GetFadeObjectsCamera();
		if (fadeObjectsCamera != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnNewTurnSMState()).MethodHandle;
			}
			fadeObjectsCamera.ResetDesiredVisibleObjects();
		}
	}

	internal bool IsOnMainCamera(ActorData a)
	{
		if (a == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.IsOnMainCamera(ActorData)).MethodHandle;
			}
			return false;
		}
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		Bounds cameraBounds = a.GetTravelBoardSquare().CameraBounds;
		return GeometryUtility.TestPlanesAABB(planes, cameraBounds);
	}

	internal void SetTargetObject(GameObject target, CameraManager.CameraTargetReason reason)
	{
		IsometricCamera isometricCamera = this.GetIsometricCamera();
		if (isometricCamera != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.SetTargetObject(GameObject, CameraManager.CameraTargetReason)).MethodHandle;
			}
			if (isometricCamera.enabled)
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
				isometricCamera.SetTargetObject(target, reason, false);
			}
		}
	}

	internal void SetTargetObjectToMouse(GameObject target, CameraManager.CameraTargetReason reason)
	{
		if (CameraManager.CamDebugTraceOn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.SetTargetObjectToMouse(GameObject, CameraManager.CameraTargetReason)).MethodHandle;
			}
			string str = "CameraManager.SetTargetObjectToMouse ";
			string str2;
			if (target != null)
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
				str2 = target.name;
			}
			else
			{
				str2 = "NULL";
			}
			CameraManager.LogForDebugging(str + str2, CameraManager.CameraLogType.None);
		}
		IsometricCamera isometricCamera = this.GetIsometricCamera();
		if (isometricCamera != null)
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
			if (isometricCamera.enabled)
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
				isometricCamera.SetTargetObject(target, reason, true);
			}
		}
	}

	public void SetTargetPosition(Vector3 pos, float easeInTime = 0f)
	{
		IsometricCamera isometricCamera = this.GetIsometricCamera();
		if (isometricCamera != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.SetTargetPosition(Vector3, float)).MethodHandle;
			}
			if (isometricCamera.enabled)
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
				isometricCamera.SetTargetPosition(pos, easeInTime);
				isometricCamera.SetTargetObject(null, CameraManager.CameraTargetReason.ReachedTargetObj);
			}
		}
	}

	internal void SetTarget(Bounds bounds, bool quickerTransition = false, bool useLowPosition = false)
	{
		if (CameraManager.CamDebugTraceOn)
		{
			CameraManager.LogForDebugging(string.Concat(new object[]
			{
				"CameraManager.SetTarget ",
				bounds.ToString(),
				" | quicker transition: ",
				quickerTransition,
				" | useLowPosition: ",
				useLowPosition
			}), CameraManager.CameraLogType.None);
		}
		if (Camera.main == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.SetTarget(Bounds, bool, bool)).MethodHandle;
			}
			return;
		}
		AbilitiesCamera abilitiesCamera = this.GetAbilitiesCamera();
		if (abilitiesCamera == null)
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
			abilitiesCamera = Camera.main.gameObject.AddComponent<AbilitiesCamera>();
			abilitiesCamera.enabled = false;
		}
		abilitiesCamera.SetTarget(bounds, quickerTransition, useLowPosition);
	}

	internal Bounds GetTarget()
	{
		AbilitiesCamera abilitiesCamera = this.GetAbilitiesCamera();
		if (abilitiesCamera == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.GetTarget()).MethodHandle;
			}
			abilitiesCamera = Camera.main.gameObject.AddComponent<AbilitiesCamera>();
			abilitiesCamera.enabled = false;
		}
		return abilitiesCamera.GetTarget();
	}

	public void PlayCameraShake(CameraManager.CameraShakeIntensity intensity)
	{
		if (intensity != CameraManager.CameraShakeIntensity.None)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.PlayCameraShake(CameraManager.CameraShakeIntensity)).MethodHandle;
			}
			if (Camera.main.gameObject.GetComponent<CameraShake>() == null)
			{
				Camera.main.gameObject.AddComponent<CameraShake>();
			}
			if (intensity == CameraManager.CameraShakeIntensity.Small)
			{
				Camera.main.gameObject.GetComponent<CameraShake>().Play(0.1f, 0.025f, 0.5f);
			}
			else if (intensity == CameraManager.CameraShakeIntensity.Large)
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
				Camera.main.gameObject.GetComponent<CameraShake>().Play(0.3f, 0.1f, 0.75f);
			}
		}
	}

	internal bool AllowCameraShake()
	{
		DebugCamera debugCamera = this.GetDebugCamera();
		if (debugCamera != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.AllowCameraShake()).MethodHandle;
			}
			if (debugCamera.enabled && !debugCamera.AllowCameraShake())
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
				return false;
			}
		}
		IsometricCamera isometricCamera = this.GetIsometricCamera();
		if (isometricCamera != null)
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
			if (isometricCamera.enabled)
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
				if (!isometricCamera.AllowCameraShake())
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
					return false;
				}
			}
		}
		AbilitiesCamera abilitiesCamera = this.GetAbilitiesCamera();
		if (abilitiesCamera != null)
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
			if (abilitiesCamera.enabled && abilitiesCamera.IsMovingAutomatically())
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
				return this.m_abilityAnimationsBetweenCamEvents > 0;
			}
		}
		return true;
	}

	public void OnAnimationEvent(ActorData animatedActor, UnityEngine.Object eventObject)
	{
		if (eventObject.name == "CameraShakeSmallEvent")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnAnimationEvent(ActorData, UnityEngine.Object)).MethodHandle;
			}
			this.PlayCameraShake(CameraManager.CameraShakeIntensity.Small);
		}
		else if (eventObject.name == "CameraShakeLargeEvent")
		{
			this.PlayCameraShake(CameraManager.CameraShakeIntensity.Large);
		}
		else if (eventObject.name == "CamStartEvent")
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
			this.m_abilityAnimationsBetweenCamEvents++;
		}
		else if (eventObject.name == "CamEndEvent")
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
			this.m_abilityAnimationsBetweenCamEvents--;
			if (this.m_abilityAnimationsBetweenCamEvents < 0)
			{
				Log.Warning("Camera manger: ability animation CamStart CamEnd count  mismatch", new object[0]);
				this.m_abilityAnimationsBetweenCamEvents = 0;
			}
		}
	}

	public bool OnAbilityAnimationStart(ActorData animatedActor, int animationIndex, Vector3 targetPos, bool requestCinematicCam, int cinematicRequested)
	{
		bool result = false;
		bool flag;
		if (DebugParameters.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnAbilityAnimationStart(ActorData, int, Vector3, bool, int)).MethodHandle;
			}
			flag = DebugParameters.Get().GetParameterAsBool("DebugCamera");
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (this.ShotSequence == null)
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
			if (!flag2)
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
				if (requestCinematicCam)
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
					if (this.m_abilityCinematicState == CameraManager.AbilityCinematicState.Default)
					{
						goto IL_98;
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
				if (this.m_abilityCinematicState != CameraManager.AbilityCinematicState.Always)
				{
					return result;
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
				IL_98:
				TauntCameraSet tauntCamSetData = animatedActor.m_tauntCamSetData;
				CameraShotSequence cameraShotSequence = null;
				CameraShot[] array = null;
				int altCamShotIndex = -1;
				if (tauntCamSetData != null)
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
					for (int i = 0; i < tauntCamSetData.m_tauntCameraShotSequences.Length; i++)
					{
						CameraShotSequence cameraShotSequence2 = tauntCamSetData.m_tauntCameraShotSequences[i] as CameraShotSequence;
						if (cameraShotSequence2 != null)
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
							if (cameraShotSequence2.m_tauntNumber == cinematicRequested)
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
								if (cameraShotSequence2.m_animIndexTauntTrigger == animationIndex)
								{
									cameraShotSequence = cameraShotSequence2;
									array = cameraShotSequence2.m_cameraShots;
									break;
								}
								if (cameraShotSequence2.m_alternateCameraShots != null && cameraShotSequence2.m_alternateCameraShots.Length > 0)
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
						}
					}
				}
				if (cameraShotSequence != null)
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
					if (array != null)
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
						if (array.Length > 0)
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
							this.ShotSequence = cameraShotSequence;
							this.ShotSequence.Begin(animatedActor, altCamShotIndex);
							result = true;
							HUD_UI.Get().SetHUDVisibility(false, false);
							if (animatedActor.GetIsHumanControlled())
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
								HUD_UI.Get().SetupTauntBanner(animatedActor);
							}
							HUD_UI.Get().SetTauntBannerVisibility(animatedActor.GetIsHumanControlled());
						}
					}
				}
			}
		}
		return result;
	}

	internal void BeginFaceShot(CameraFaceShot faceShot, ActorData actor)
	{
		if (faceShot != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.BeginFaceShot(CameraFaceShot, ActorData)).MethodHandle;
			}
			if (actor != null && this.FaceCamera != null)
			{
				this.FaceShot = faceShot;
				faceShot.Begin(actor, this.FaceCamera);
			}
		}
	}

	private bool IsCameraCenterKeyHeld()
	{
		bool flag = false;
		if (GameFlowData.Get() != null && GameFlowData.Get().gameState == GameState.EndingGame)
		{
			flag = true;
		}
		bool result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.IsCameraCenterKeyHeld()).MethodHandle;
			}
			result = InputManager.Get().IsKeyBindingHeld(KeyPreference.CameraCenterOnAction);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal bool DoesAnimIndexTriggerTauntCamera(ActorData actor, int animIndex, int tauntNumber)
	{
		TauntCameraSet tauntCamSetData = actor.m_tauntCamSetData;
		if (tauntCamSetData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.DoesAnimIndexTriggerTauntCamera(ActorData, int, int)).MethodHandle;
			}
			if (tauntCamSetData.m_tauntCameraShotSequences != null)
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
				for (int i = 0; i < tauntCamSetData.m_tauntCameraShotSequences.Length; i++)
				{
					CameraShotSequence cameraShotSequence = tauntCamSetData.m_tauntCameraShotSequences[i] as CameraShotSequence;
					if (cameraShotSequence != null)
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
						if (cameraShotSequence.m_tauntNumber == tauntNumber)
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
							if (cameraShotSequence.m_animIndexTauntTrigger == animIndex)
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
								return true;
							}
							if (cameraShotSequence.m_alternateCameraShots != null)
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
								if (cameraShotSequence.m_alternateCameraShots.Length > 0)
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
									for (int j = 0; j < cameraShotSequence.m_alternateCameraShots.Length; j++)
									{
										if (cameraShotSequence.m_alternateCameraShots[j].m_altAnimIndexTauntTrigger == animIndex)
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
											return true;
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
								}
							}
						}
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
			}
		}
		return false;
	}

	internal void OnPlayerMovedCamera()
	{
		if (UIMainScreenPanel.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnPlayerMovedCamera()).MethodHandle;
			}
			if (UIMainScreenPanel.Get().m_autoCameraButton != null)
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
				UIMainScreenPanel.Get().m_autoCameraButton.OnPlayerMovedCamera();
			}
		}
	}

	public void Update()
	{
		bool flag;
		if (DebugParameters.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.Update()).MethodHandle;
			}
			flag = DebugParameters.Get().GetParameterAsBool("DebugCamera");
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (flag2)
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
			if (!this.GetDebugCamera().enabled)
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
				this.EnableDebugCamera();
				goto IL_12A;
			}
		}
		if (AppState.GetCurrent() == AppState_InGameDeployment.Get())
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
			if (this.GetFlyThroughCamera() != null)
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
				this.EnableFlyThroughCamera();
				goto IL_12A;
			}
		}
		if (!flag2)
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
			if (this.GetDebugCamera() != null)
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
				if (this.GetDebugCamera().enabled)
				{
					goto IL_123;
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
		if (!(this.GetFlyThroughCamera() == null))
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
			if (!this.GetFlyThroughCamera().enabled)
			{
				goto IL_12A;
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
		IL_123:
		this.EnableIsometricCamera(CameraTransitionType.Move);
		IL_12A:
		if (this.SecondsRemainingToPauseForUserControl > 0f)
		{
			this.SecondsRemainingToPauseForUserControl -= Time.deltaTime;
		}
		if (!(GameFlowData.Get() == null))
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
			if (!(Camera.main == null))
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
				if (!flag2)
				{
					if (!this.GetFlyThroughCamera().enabled)
					{
						if (this.ShotSequence != null)
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
							if (!this.ShotSequence.Update())
							{
								this.ShotSequence = null;
								HUD_UI.Get().SetHUDVisibility(true, true);
								HUD_UI.Get().SetTauntBannerVisibility(false);
							}
						}
						if (this.FaceShot != null && !this.FaceShot.Update(this.FaceCamera))
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
							this.FaceCamera.gameObject.SetActive(false);
							this.FaceShot = null;
						}
						AccountPreferences accountPreferences = AccountPreferences.Get();
						if (UIMainScreenPanel.Get() != null && UIMainScreenPanel.Get().m_autoCameraButton != null && GameManager.Get().GameConfig.GameType != GameType.Tutorial)
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
							bool flag3;
							if (this.m_useCameraToggleKey)
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
								flag3 = InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.CameraToggleAutoCenter);
							}
							else
							{
								flag3 = false;
							}
							bool flag4 = flag3;
							if (this.m_useRightClickToToggle)
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
								if (!flag4)
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
									if (InterfaceManager.Get() != null)
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
										if (InterfaceManager.Get().ShouldHandleMouseClick())
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
											bool flag5;
											if (Input.GetMouseButtonUp(1))
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
														switch (6)
														{
														case 0:
															continue;
														}
														break;
													}
													if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve)
													{
														flag5 = (GameFlowData.Get().GetPause() || GameFlowData.Get().GetTimeInState() >= 1.5f);
														goto IL_36B;
													}
												}
											}
											flag5 = false;
											IL_36B:
											flag4 = flag5;
										}
									}
								}
							}
							if (flag4)
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
								bool flag6 = !accountPreferences.GetBool(BoolPreference.AutoCameraCenter);
								accountPreferences.SetBool(BoolPreference.AutoCameraCenter, flag6);
								UIMainScreenPanel.Get().m_autoCameraButton.RefreshAutoCameraButton();
								AbilitiesCamera abilitiesCamera = this.GetAbilitiesCamera();
								if (flag6)
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
									if (abilitiesCamera != null)
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
										abilitiesCamera.OnAutoCenterCameraPreferenceSet();
									}
								}
							}
						}
						if (this.ShotSequence == null)
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
							this.m_useAbilitiesCameraOutOfCinematics = this.ShouldUseAbilitiesCameraOutOfCinematics();
							if (!flag2)
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
								if (this.m_useAbilitiesCameraOutOfCinematics)
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
									this.EnableAbilitiesCamera(CameraTransitionType.Move);
								}
								else
								{
									this.EnableIsometricCamera(CameraTransitionType.Move);
								}
							}
							if (this.IsCameraCenterKeyHeld())
							{
								ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
								CameraManager cameraManager = CameraManager.Get();
								GameObject target;
								if (activeOwnedActorData == null)
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
									target = null;
								}
								else
								{
									target = activeOwnedActorData.gameObject;
								}
								cameraManager.SetTargetObject(target, CameraManager.CameraTargetReason.CameraCenterKeyHeld);
								if (ControlpadGameplay.Get() != null)
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
									ControlpadGameplay.Get().OnCameraCenteredOnActor(activeOwnedActorData);
								}
							}
						}
						if (this.AudioListener != null)
						{
							if (this.ShotSequence != null)
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
								if (this.ShotSequence.Actor != null)
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
									this.AudioListener.transform.position = this.ShotSequence.Actor.transform.position;
									return;
								}
							}
							Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
							float num = Vector3.Dot(Vector3.down, Camera.main.transform.forward);
							if (num < 0.258819f)
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
								Vector3 direction = Quaternion.AngleAxis(-75f, Camera.main.transform.right) * Vector3.down;
								ray = new Ray(Camera.main.transform.position, direction);
							}
							Plane plane = new Plane(Vector3.up, (float)(-(float)Board.Get().BaselineHeight));
							float distance;
							if (!plane.Raycast(ray, out distance))
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
								distance = 3f;
							}
							this.AudioListener.transform.position = ray.GetPoint(distance);
						}
						return;
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

	private bool ShouldUseAbilitiesCameraOutOfCinematics()
	{
		if (Camera.main == null)
		{
			return false;
		}
		AbilitiesCamera abilitiesCamera = this.GetAbilitiesCamera();
		if (abilitiesCamera != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.ShouldUseAbilitiesCameraOutOfCinematics()).MethodHandle;
			}
			if (abilitiesCamera.IsDisabledUntilSetTarget)
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
				return false;
			}
		}
		bool flag = GameFlowData.Get().gameState == GameState.BothTeams_Resolve;
		ActionBufferPhase currentActionPhase = ServerClientUtils.GetCurrentActionPhase();
		bool flag2;
		if (currentActionPhase != ActionBufferPhase.AbilitiesWait)
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
			if (currentActionPhase != ActionBufferPhase.Movement)
			{
				flag2 = (currentActionPhase == ActionBufferPhase.MovementChase);
				goto IL_86;
			}
		}
		flag2 = true;
		IL_86:
		bool flag3 = flag2;
		bool flag4 = !TheatricsManager.Get().AbilityPhaseHasNoAnimations();
		bool result;
		if (flag)
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
			if (!flag3)
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
				if (!flag4)
				{
					goto IL_173;
				}
			}
			bool flag5 = GameManager.Get() != null && GameManager.Get().GameConfig.GameType == GameType.Tutorial;
			if (!this.ShouldAutoCameraMove())
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
				if (!CameraManager.Get().GetAbilitiesCamera().enabled)
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
					result = false;
					goto IL_171;
				}
			}
			if (flag5)
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
				bool flag6;
				if (currentActionPhase != ActionBufferPhase.MovementWait)
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
					flag6 = (currentActionPhase == ActionBufferPhase.Done);
				}
				else
				{
					flag6 = true;
				}
				bool flag7 = flag6;
				bool flag8;
				if (flag4)
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
					if (!flag3)
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
						flag8 = !flag7;
						goto IL_16C;
					}
				}
				flag8 = false;
				IL_16C:
				result = flag8;
			}
			else
			{
				result = true;
			}
			IL_171:
			return result;
		}
		IL_173:
		result = false;
		return result;
	}

	public unsafe static bool BoundSidesWithinDistance(Bounds currentBound, Bounds compareToBound, float mergeSizeThresh, out Vector3 maxBoundDiff, out Vector3 minBoundDiff)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.BoundSidesWithinDistance(Bounds, Bounds, float, Vector3*, Vector3*)).MethodHandle;
			}
			if (Mathf.Abs(maxBoundDiff.z) <= mergeSizeThresh)
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
				if (Mathf.Abs(minBoundDiff.x) <= mergeSizeThresh)
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
					if (Mathf.Abs(minBoundDiff.z) <= mergeSizeThresh)
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
						result = true;
					}
				}
			}
		}
		return result;
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireCube(this.CameraPositionBounds.center, this.CameraPositionBounds.size);
		Gizmos.DrawWireSphere(this.CameraPositionBounds.center, 1f);
		if (this.AudioListener != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.OnDrawGizmos()).MethodHandle;
			}
			Gizmos.DrawWireSphere(this.AudioListener.transform.position, 1f);
		}
	}

	public static bool ShouldDrawGizmosForCurrentCamera()
	{
		if (Application.isEditor)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.ShouldDrawGizmosForCurrentCamera()).MethodHandle;
			}
			LayerMask mask = 1 << LayerMask.NameToLayer("Default");
			return (Camera.current.cullingMask & mask) != 0;
		}
		return false;
	}

	public static void LogForDebugging(string str, CameraManager.CameraLogType cameraType = CameraManager.CameraLogType.None)
	{
		string text;
		if (cameraType == CameraManager.CameraLogType.None)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CameraManager.LogForDebugging(string, CameraManager.CameraLogType)).MethodHandle;
			}
			text = string.Empty;
		}
		else
		{
			text = "[" + cameraType.ToString() + "]";
		}
		string text2 = text;
		Debug.LogWarning(string.Concat(new object[]
		{
			"<color=magenta>Camera ",
			text2,
			" | </color>",
			str,
			"\n@time= ",
			Time.time
		}));
	}

	public static bool CamDebugTraceOn
	{
		get
		{
			return false;
		}
	}

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
}
