using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class ActorModelData : MonoBehaviour, IGameEventListener
{
	public const int MIN_ANIMATION_INDEX = 1;

	public const int MAX_ANIMATION_INDEX = 0x14;

	public Sprite m_characterPortrait;

	internal ActorData m_parentActorData;

	public ActorModelData.PersistentVFXInfo[] m_persistentVFX;

	[HideInInspector]
	public TauntCameraSet m_abilityCameraShotSequences;

	[Header("--- Ragdoll ---")]
	public bool m_enableRagdollPreprocessing;

	public float m_maxDepenetrationVelocity;

	[Header("--- Camera Offsets ---")]
	public float m_cameraVertOffset = 1.2f;

	public float m_cameraVertCoverOffset = 0.4f;

	public float m_cameraHorzOffset = 1.5f;

	[Space(10f)]
	public float m_stealthParameterStartPKFX = 0.75f;

	public float m_stealthParameterStopPKFX = 0.25f;

	[Header("-- Reference to anim events and state info, saved by editor --")]
	public ActorModelAnimData m_savedAnimData;

	private string m_stealthReplacementShaderStr;

	private Shader m_stealthReplacementShader;

	private bool m_stealthPKFXStopped;

	private bool m_stealthShaderEnabled;

	private float m_stealthFadeDuration = 1.5f;

	public float m_stealthMoveHightlightFadeDuration = 0.3f;

	public float m_stealthMoveStoppedHightlightFadeDuration = 2f;

	public float m_stealthBrokenEaseDuration = 12f;

	private float m_stealthStoppedPlaneOffset = 0.75f;

	private EasedOutFloat m_stealthFadeParameter = new EasedOutFloat(0f);

	private EasedFloat m_stealthBrushParameter = new EasedFloat(0f);

	private EasedOutFloat m_stealthBrokenParameter = new EasedOutFloat(0f);

	private Eased<float> m_stealthBrushTransitionParameter = new EasedOutFloatQuad(0f);

	private string c_stealthShaderKeyword = "STEALTH_ON";

	private string m_rendererDefaultMaterialsCacheKey;

	[Header("-- for setting cull mode on particular materials when entering/exiting stealth modes")]
	public List<ActorModelData.CullModeSettings> m_stealthShaderCullModeSettings;

	[HideInInspector]
	public float[] m_camStartEventDelays = new float[0x15];

	[HideInInspector]
	public float[] m_tauntCamStartEventDelays = new float[0x15];

	[HideInInspector]
	public List<int> m_animatorStateNameHashes;

	[HideInInspector]
	public List<string> m_animatorStateNames;

	private Dictionary<int, string> m_animatorStateNameHashToName;

	private bool m_alphaUpdateMarkedDirty;

	private bool m_showingOutline;

	private Transform m_rootBoneTransform;

	private Transform m_floorBoneTransform;

	private Transform m_hipBoneTransform;

	private Dictionary<GameObject, Vector3> m_initialBoneOffsetMap = new Dictionary<GameObject, Vector3>();

	private Dictionary<GameObject, Quaternion> m_initialBoneRotationOffsetMap = new Dictionary<GameObject, Quaternion>();

	private Dictionary<GameObject, Vector3> m_initialBoneScaleOffsetMap = new Dictionary<GameObject, Vector3>();

	private Projector m_projector;

	private Renderer[] m_renderers;

	private bool[] m_shroudInstancesToEnable;

	private ShroudInstance[] m_shroudInstances;

	private List<PKFxFX> m_popcornFXPlayOnStartComponents = new List<PKFxFX>();

	private List<Color[]> m_rendererDefaultColors;

	private Dictionary<string, List<Material[]>> m_appearanceNameToCachedRendererMaterials = new Dictionary<string, List<Material[]>>();

	private Rigidbody[] m_activeRigidbodies;

	private bool m_dirtyRenderersCache;

	private bool m_attemptedToCreateBaseCircle;

	private float m_cameraTransparency = 1f;

	private float m_cameraTransparencyStartValue = 1f;

	private float m_cameraTransparencyTime = 1f;

	private float m_cameraTransparencyLastChangeSetTime;

	private Animator m_modelAnimator;

	private Bounds m_rendererBoundsApprox = default(Bounds);

	private BoxCollider m_rendererBoundsApproxCollider;

	private bool m_visibleToClient = true;

	private bool m_forceUpdateVisibility;

	private bool m_isFace;

	private bool m_hasRandomValueParameter;

	private bool m_hasTurnStartParameter;

	private bool m_needsStandingIdleBoundingBox = true;

	private float m_lastAnimatorRandomSetTime;

	private List<float> m_cachedRendererAlphas;

	private static int[] s_stealthMaterialPlaneVectorIDs = new int[4];

	internal static int s_materialPropertyIDTeam;

	private static int s_materialPropertyIDStealthFade;

	private static int s_materialPropertyIDBrush;

	private static int s_materialPropertyIDStealthBroken;

	private static int s_materialPropertyIDStealthMoving;

	private static int s_materialPropertyIDVisibleToClient;

	private static int s_materialPropertyIDCullMode;

	private static int materialColorProperty;

	private static int materialOutlineProperty;

	private static int materialOutlineColorProperty;

	private Rigidbody m_cachedRigidbodyForRagdollImpulse;

	private GameObject m_masterSkinVfxInst;

	private static readonly int s_animHashDamage = Animator.StringToHash("Damage");

	private static readonly int s_animHashDamageNoInterrupt = Animator.StringToHash("Damage_NoInterrupt");

	private static readonly int s_animHashChargeEnd = Animator.StringToHash("ChargeEnd");

	private static readonly int s_animHashAttack = Animator.StringToHash("Attack");

	private static readonly int s_animHashIdle = Animator.StringToHash("Idle");

	private static readonly int s_animHashKnockdown = Animator.StringToHash("Knockdown");

	private bool m_forceHideRenderers;

	internal Dictionary<int, string> GetAnimatorStateNameHashToNameMap()
	{
		return this.m_animatorStateNameHashToName;
	}

	internal float Alpha { get; set; }

	private void Awake()
	{
		this.m_needsStandingIdleBoundingBox = true;
		this.m_modelAnimator = base.GetComponentInChildren<Animator>();
		AnimatorControllerParameter[] parameters = this.m_modelAnimator.parameters;
		for (int i = 0; i < parameters.Length; i++)
		{
			if (!this.m_hasRandomValueParameter)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.Awake()).MethodHandle;
				}
				if (parameters[i].name == "RandomValue")
				{
					this.m_hasRandomValueParameter = true;
				}
			}
			if (!this.m_hasTurnStartParameter)
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
				if (parameters[i].name == "TurnStart")
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
					this.m_hasTurnStartParameter = true;
				}
			}
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
		if (this.m_savedAnimData != null)
		{
			float[] savedCamStartEventDelays = this.m_savedAnimData.m_savedCamStartEventDelays;
			float[] savedTauntCamStartEventDelays = this.m_savedAnimData.m_savedTauntCamStartEventDelays;
			List<int> savedAnimatorStateNameHashes = this.m_savedAnimData.m_savedAnimatorStateNameHashes;
			List<string> savedAnimatorStateNames = this.m_savedAnimData.m_savedAnimatorStateNames;
			if (savedCamStartEventDelays != null)
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
				if (savedCamStartEventDelays.Length == 0x15)
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
					this.m_camStartEventDelays = new float[0x15];
					Array.Copy(savedCamStartEventDelays, this.m_camStartEventDelays, 0x15);
					goto IL_153;
				}
			}
			Log.Error(base.name + " saved CamStartEventDelays is null or has mismatched number of entries", new object[0]);
			IL_153:
			if (savedTauntCamStartEventDelays != null && savedTauntCamStartEventDelays.Length == 0x15)
			{
				this.m_tauntCamStartEventDelays = new float[0x15];
				Array.Copy(savedTauntCamStartEventDelays, this.m_tauntCamStartEventDelays, 0x15);
			}
			else
			{
				Log.Error(base.name + " saved Taunt CamStartEventDelays is null or has mismatched number of entries", new object[0]);
			}
			if (savedAnimatorStateNameHashes != null)
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
				this.m_animatorStateNameHashes = new List<int>(savedAnimatorStateNameHashes);
			}
			if (savedAnimatorStateNames != null)
			{
				this.m_animatorStateNames = new List<string>(savedAnimatorStateNames);
			}
		}
		Animator[] componentsInChildren = base.gameObject.GetComponentsInChildren<Animator>();
		foreach (Animator animator in componentsInChildren)
		{
			animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
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
		this.m_animatorStateNameHashToName = new Dictionary<int, string>(this.m_animatorStateNameHashes.Count);
		for (int k = 0; k < this.m_animatorStateNameHashes.Count; k++)
		{
			this.m_animatorStateNameHashToName[this.m_animatorStateNameHashes[k]] = this.m_animatorStateNames[k];
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
		this.m_projector = base.gameObject.GetComponentInChildren<Projector>();
		foreach (ActorModelData.PersistentVFXInfo persistentVFXInfo in this.m_persistentVFX)
		{
			if (persistentVFXInfo.m_vfxPrefab != null)
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
				persistentVFXInfo.m_fxJoint.Initialize(base.gameObject);
				persistentVFXInfo.m_vfxInstance = UnityEngine.Object.Instantiate<GameObject>(persistentVFXInfo.m_vfxPrefab);
				persistentVFXInfo.m_vfxInstance.transform.parent = persistentVFXInfo.m_fxJoint.m_jointObject.transform;
				persistentVFXInfo.m_vfxInstance.transform.localPosition = Vector3.zero;
				persistentVFXInfo.m_vfxInstance.transform.localRotation = Quaternion.identity;
				persistentVFXInfo.m_vfxInstance.transform.localScale = Vector3.one;
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
		this.m_renderers = base.gameObject.GetComponentsInChildren<Renderer>();
		if (ActorModelData.s_stealthMaterialPlaneVectorIDs[0] == 0)
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
			ActorModelData.s_stealthMaterialPlaneVectorIDs[0] = Shader.PropertyToID("_StealthPlane0");
			ActorModelData.s_stealthMaterialPlaneVectorIDs[1] = Shader.PropertyToID("_StealthPlane1");
			ActorModelData.s_stealthMaterialPlaneVectorIDs[2] = Shader.PropertyToID("_StealthPlane2");
			ActorModelData.s_stealthMaterialPlaneVectorIDs[3] = Shader.PropertyToID("_StealthPlane3");
			ActorModelData.s_materialPropertyIDTeam = Shader.PropertyToID("_Team");
			ActorModelData.s_materialPropertyIDStealthFade = Shader.PropertyToID("_StealthFade");
			ActorModelData.s_materialPropertyIDBrush = Shader.PropertyToID("_StealthBrush");
			ActorModelData.s_materialPropertyIDStealthMoving = Shader.PropertyToID("_StealthMoving");
			ActorModelData.s_materialPropertyIDStealthBroken = Shader.PropertyToID("_StealthBroken");
			ActorModelData.s_materialPropertyIDVisibleToClient = Shader.PropertyToID("_VisibleToClient");
		}
		ActorModelData.s_materialPropertyIDCullMode = Shader.PropertyToID("_Cull");
		ActorModelData.materialColorProperty = Shader.PropertyToID("_Color");
		ActorModelData.materialOutlineProperty = Shader.PropertyToID("_Outline");
		ActorModelData.materialOutlineColorProperty = Shader.PropertyToID("_OutlineColor");
		this.m_alphaUpdateMarkedDirty = true;
	}

	private void Start()
	{
		this.m_shroudInstances = base.GetComponentsInChildren<ShroudInstance>();
		if (this.m_shroudInstances.Length > 0)
		{
			this.m_dirtyRenderersCache = true;
		}
		else
		{
			this.CacheRenderers();
		}
		PKFxFX[] componentsInChildren = base.GetComponentsInChildren<PKFxFX>();
		if (componentsInChildren != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.Start()).MethodHandle;
			}
			foreach (PKFxFX pkfxFX in componentsInChildren)
			{
				if (pkfxFX.m_PlayOnStart)
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
					this.m_popcornFXPlayOnStartComponents.Add(pkfxFX);
				}
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
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TheatricsAbilitiesEnd);
		this.m_forceUpdateVisibility = true;
		if (base.GetComponentInChildren<AnimationEventReceiver>() == null)
		{
			string format = "{0} does not have an Animation Event Receiver on its model.  Please update the prefab.";
			object arg;
			if (this.m_parentActorData != null)
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
				arg = this.m_parentActorData.name;
			}
			else
			{
				arg = string.Empty;
			}
			Log.Error(string.Format(format, arg), new object[0]);
		}
		this.SetMaterialFloatTeam();
		this.InitJointsAndRigidBodies();
		this.InitCachedJointForRagdoll();
	}

	private void SetMaterialFloatTeam()
	{
		float num;
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.SetMaterialFloatTeam()).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData != null)
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
				if (this.m_parentActorData.\u0012() == GameFlowData.Get().activeOwnedActorData.\u000E())
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
					num = 1f;
					goto IL_76;
				}
			}
		}
		num = 0f;
		IL_76:
		float value = num;
		this.SetMaterialFloat(ActorModelData.s_materialPropertyIDTeam, value);
	}

	public void DelayEnablingOfShroudInstances()
	{
		ShroudInstance[] componentsInChildren = base.GetComponentsInChildren<ShroudInstance>();
		this.m_shroudInstancesToEnable = new bool[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			this.m_shroudInstancesToEnable[i] = componentsInChildren[i].enabled;
			componentsInChildren[i].enabled = false;
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.DelayEnablingOfShroudInstances()).MethodHandle;
		}
	}

	public void ImpartWindImpulse(Vector3 direction)
	{
		for (int i = 0; i < this.m_shroudInstances.Length; i++)
		{
			this.m_shroudInstances[i].ImpartWindImpulse(direction);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.ImpartWindImpulse(Vector3)).MethodHandle;
		}
	}

	public void InitJointsAndRigidBodies()
	{
		Joint[] componentsInChildren = base.gameObject.GetComponentsInChildren<Joint>();
		foreach (Joint joint in componentsInChildren)
		{
			joint.enablePreprocessing = this.m_enableRagdollPreprocessing;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.InitJointsAndRigidBodies()).MethodHandle;
		}
		Rigidbody[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rigidbody in componentsInChildren2)
		{
			rigidbody.maxDepenetrationVelocity = this.m_maxDepenetrationVelocity;
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

	public void InitCachedJointForRagdoll()
	{
		float num = -1f;
		Rigidbody rigidbody = null;
		Rigidbody[] componentsInChildren = base.gameObject.GetComponentsInChildren<Rigidbody>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Rigidbody rigidbody2 = componentsInChildren[i];
			if (componentsInChildren[i].name.EqualsIgnoreCase("spine2_JNT"))
			{
				rigidbody = rigidbody2;
				break;
			}
			if (rigidbody2.mass > num)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.InitCachedJointForRagdoll()).MethodHandle;
				}
				rigidbody = rigidbody2;
				num = rigidbody2.mass;
			}
		}
		if (rigidbody != null)
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
			this.m_cachedRigidbodyForRagdollImpulse = rigidbody;
		}
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.OnDestroy()).MethodHandle;
			}
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TheatricsAbilitiesEnd);
		}
		if (this.m_appearanceNameToCachedRendererMaterials != null)
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
			for (int i = 0; i < this.m_appearanceNameToCachedRendererMaterials.Count; i++)
			{
				List<Material[]> list = this.m_appearanceNameToCachedRendererMaterials.Values.ElementAt(i);
				for (int j = 0; j < list.Count; j++)
				{
					Material[] array = list[j];
					for (int k = 0; k < array.Length; k++)
					{
						UnityEngine.Object.Destroy(array[k]);
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
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			this.m_appearanceNameToCachedRendererMaterials.Clear();
		}
		this.m_parentActorData = null;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.TheatricsAbilitiesEnd)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
		}
		else
		{
			this.SetCameraTransparency(1f, 1f, 0f);
		}
	}

	internal void OnMovementAnimatorUpdate(BoardSquarePathInfo.ConnectionType movementType)
	{
		if (BrushCoordinator.Get() != null)
		{
			if (!(GameFlowData.Get().activeOwnedActorData == null))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.OnMovementAnimatorUpdate(BoardSquarePathInfo.ConnectionType)).MethodHandle;
				}
				if (GameFlowData.Get().activeOwnedActorData.\u0012() == this.m_parentActorData.\u000E())
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
					if (movementType == BoardSquarePathInfo.ConnectionType.Flight)
					{
						return;
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
					if (movementType == BoardSquarePathInfo.ConnectionType.Teleport)
					{
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
			bool flag = this.m_parentActorData.\u0016();
			BoardSquarePathInfo previousTravelBoardSquarePathInfo = this.m_parentActorData.\u000E().GetPreviousTravelBoardSquarePathInfo();
			bool flag2;
			if (previousTravelBoardSquarePathInfo != null)
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
				flag2 = (previousTravelBoardSquarePathInfo.m_visibleToEnemies || previousTravelBoardSquarePathInfo.m_moverHasGameplayHitHere);
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			if (flag3 && flag && this.m_stealthBrushTransitionParameter.EaseFinished())
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
				this.m_stealthBrushTransitionParameter = new EasedOutFloatQuad(1f);
				Eased<float> stealthBrushTransitionParameter = this.m_stealthBrushTransitionParameter;
				float endValue = 0f;
				float duration;
				if (this.m_parentActorData.\u000E().IsPast2ndToLastSquare())
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
					duration = this.m_stealthMoveStoppedHightlightFadeDuration;
				}
				else
				{
					duration = this.m_stealthMoveHightlightFadeDuration;
				}
				stealthBrushTransitionParameter.EaseTo(endValue, duration);
			}
			else
			{
				BoardSquarePathInfo nextTravelBoardSquarePathInfo = this.m_parentActorData.\u000E().GetNextTravelBoardSquarePathInfo();
				BoardSquare boardSquare;
				if (nextTravelBoardSquarePathInfo == null)
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
					boardSquare = null;
				}
				else
				{
					boardSquare = nextTravelBoardSquarePathInfo.square;
				}
				BoardSquare boardSquare2 = boardSquare;
				bool flag4;
				if (boardSquare2 != null)
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
					flag4 = BrushCoordinator.Get().IsRegionFunctioning(boardSquare2.BrushRegion);
				}
				else
				{
					flag4 = false;
				}
				bool flag5 = flag4;
				if (flag && !flag5)
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
					if (this.m_stealthBrushTransitionParameter.GetEndValue() == 0f)
					{
						this.m_stealthBrushTransitionParameter = new EasedInFloatQuad(this.m_stealthBrushTransitionParameter);
						this.m_stealthBrushTransitionParameter.EaseTo(1f, this.m_stealthMoveHightlightFadeDuration);
					}
				}
			}
		}
	}

	internal void OnMovementAnimatorStop()
	{
		this.m_stealthBrushTransitionParameter.EaseTo(0f, this.m_stealthMoveStoppedHightlightFadeDuration);
	}

	public unsafe bool DiffForSyncCharacterPrefab(ActorModelData other, ref List<string> diffDescriptions)
	{
		bool result = true;
		if (this.m_characterPortrait != other.m_characterPortrait)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.DiffForSyncCharacterPrefab(ActorModelData, List<string>*)).MethodHandle;
			}
			diffDescriptions.Add("\tCharacter Portrait different");
			result = false;
		}
		if (this.m_cameraVertOffset == other.m_cameraVertOffset)
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
			if (this.m_cameraVertCoverOffset == other.m_cameraVertCoverOffset)
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
				if (this.m_cameraHorzOffset == other.m_cameraHorzOffset)
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
					if (this.m_stealthParameterStopPKFX == other.m_stealthParameterStopPKFX)
					{
						goto IL_A4;
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
		diffDescriptions.Add("\tCamera Offsets different");
		result = false;
		IL_A4:
		if (this.m_persistentVFX.Length != other.m_persistentVFX.Length)
		{
			diffDescriptions.Add("\tPersistent VFX different");
			result = false;
		}
		else
		{
			int i = 0;
			while (i < this.m_persistentVFX.Length)
			{
				if (!(this.m_persistentVFX[i].m_vfxPrefab.name != other.m_persistentVFX[i].m_vfxPrefab.name) && !(this.m_persistentVFX[i].m_fxJoint.m_joint != other.m_persistentVFX[i].m_fxJoint.m_joint))
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
					if (!(this.m_persistentVFX[i].m_fxJoint.m_jointCharacter != other.m_persistentVFX[i].m_fxJoint.m_jointCharacter))
					{
						i++;
						continue;
					}
				}
				diffDescriptions.Add("\tPersistent VFX different");
				return false;
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
		return result;
	}

	public bool IsVisibleToClient()
	{
		return this.m_visibleToClient;
	}

	public bool HasAnimatorControllerParamater(string paramName)
	{
		bool result = false;
		this.m_modelAnimator = base.GetComponentInChildren<Animator>();
		AnimatorControllerParameter[] parameters = this.m_modelAnimator.parameters;
		for (int i = 0; i < parameters.Length; i++)
		{
			if (parameters[i].name == paramName)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.HasAnimatorControllerParamater(string)).MethodHandle;
				}
				result = true;
				return result;
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			return result;
		}
	}

	public bool HasTurnStartParameter()
	{
		return this.m_hasTurnStartParameter;
	}

	private void CacheRenderers()
	{
		List<string> list = new List<string>
		{
			"Hydrogen/TGP/Toony",
			"Hydrogen/TGP/Toony_Metal",
			"Hydrogen/TGP/Toony_Alpha",
			"Hydrogen/DigitalSorceress",
			"Hydrogen/DigitalSorceressHair",
			"Hydrogen/Spark",
			"Hydrogen/Spark2",
			"Hydrogen/TGP/Environment_Metal_Alpha",
			"Hydrogen/Trickster",
			"Hydrogen/Trickster_Environment_Metal",
			"Hydrogen/Trickster_Environment_Metal_Alpha"
		};
		this.m_dirtyRenderersCache = false;
		if (this.m_renderers == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.CacheRenderers()).MethodHandle;
			}
			this.m_renderers = base.gameObject.GetComponentsInChildren<Renderer>();
		}
		else
		{
			List<Renderer> list2 = new List<Renderer>();
			for (int i = 0; i < this.m_renderers.Length; i++)
			{
				list2.Add(this.m_renderers[i]);
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
			Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (!list2.Contains(componentsInChildren[j]))
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
					list2.Add(componentsInChildren[j]);
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
			this.m_renderers = list2.ToArray();
		}
		this.m_rendererDefaultColors = new List<Color[]>(this.m_renderers.Length);
		List<Material[]> list3 = new List<Material[]>(this.m_renderers.Length);
		foreach (Renderer renderer in this.m_renderers)
		{
			if (renderer == null)
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
			}
			else
			{
				Color[] array = new Color[renderer.sharedMaterials.Length];
				for (int l = 0; l < renderer.sharedMaterials.Length; l++)
				{
					if (renderer.sharedMaterials[l] != null)
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
						if (renderer.sharedMaterials[l].HasProperty(ActorModelData.materialColorProperty))
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
							array[l] = renderer.sharedMaterials[l].color;
						}
						else
						{
							array[l] = Color.white;
						}
						if (!list.Contains(renderer.sharedMaterials[l].shader.name))
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
							string stealthReplacementShaderStr;
							if (base.gameObject.name.Contains("trickster"))
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
								stealthReplacementShaderStr = "Hydrogen/Trickster";
							}
							else
							{
								stealthReplacementShaderStr = list[0];
							}
							this.m_stealthReplacementShaderStr = stealthReplacementShaderStr;
						}
					}
					else
					{
						array[l] = Color.white;
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
				this.m_rendererDefaultColors.Add(array);
				Material[] array2 = new Material[renderer.sharedMaterials.Length];
				for (int m = 0; m < array2.Length; m++)
				{
					Material material = new Material(renderer.sharedMaterials[m]);
					array2[m] = material;
					this.m_rendererDefaultMaterialsCacheKey = material.shader.name;
				}
				list3.Add(array2);
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
		this.m_appearanceNameToCachedRendererMaterials[this.m_rendererDefaultMaterialsCacheKey] = list3;
		if (!string.IsNullOrEmpty(this.m_stealthReplacementShaderStr))
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
			this.m_stealthReplacementShader = Shader.Find(this.m_stealthReplacementShaderStr);
		}
		this.m_alphaUpdateMarkedDirty = true;
	}

	internal void InitializeFaceActorModelData()
	{
		this.m_isFace = true;
		this.DestroyRagdoll();
		int layer = LayerMask.NameToLayer("Face");
		foreach (Transform transform in base.gameObject.GetComponentsInChildren<Transform>(true))
		{
			transform.gameObject.layer = layer;
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.InitializeFaceActorModelData()).MethodHandle;
		}
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true))
		{
			skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			skinnedMeshRenderer.receiveShadows = false;
			skinnedMeshRenderer.lightProbeUsage = LightProbeUsage.Off;
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
		base.gameObject.layer = layer;
		if (this.m_projector != null)
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
			this.m_projector.enabled = false;
		}
		base.gameObject.SetActive(false);
	}

	public float GetCameraHorzOffset()
	{
		return this.m_cameraHorzOffset;
	}

	public float GetCameraVertOffset(bool forceStandingOffset)
	{
		if (!forceStandingOffset)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.GetCameraVertOffset(bool)).MethodHandle;
			}
			if (this.m_parentActorData.\u000E().HasAnyCover(false))
			{
				return this.m_cameraVertCoverOffset;
			}
		}
		return this.m_cameraVertOffset;
	}

	public float GetModelSize()
	{
		if (this.m_renderers != null)
		{
			if (this.m_renderers.Length >= 1)
			{
				Bounds bounds = this.m_renderers[0].bounds;
				foreach (Renderer renderer in this.m_renderers)
				{
					if (renderer != null)
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
						bounds.Encapsulate(renderer.bounds);
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
				Vector3 size = bounds.size;
				return Mathf.Max(size.x, Mathf.Max(size.y, size.z));
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.GetModelSize()).MethodHandle;
			}
		}
		return 2f;
	}

	private void DestroyRagdoll()
	{
		Joint[] componentsInChildren = base.gameObject.GetComponentsInChildren<Joint>();
		foreach (Joint obj in componentsInChildren)
		{
			UnityEngine.Object.Destroy(obj);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.DestroyRagdoll()).MethodHandle;
		}
		Collider[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<Collider>();
		foreach (Collider obj2 in componentsInChildren2)
		{
			UnityEngine.Object.Destroy(obj2);
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
		Rigidbody[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody obj3 in componentsInChildren3)
		{
			UnityEngine.Object.Destroy(obj3);
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

	internal string GetCurrentAnimatorStateName()
	{
		string result = "[UNKNOWN: Please save .controller]";
		Animator modelAnimator = this.GetModelAnimator();
		if (modelAnimator != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.GetCurrentAnimatorStateName()).MethodHandle;
			}
			if (modelAnimator.layerCount >= 1)
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
				AnimatorStateInfo currentAnimatorStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
				string text;
				if (this.m_animatorStateNameHashToName.TryGetValue(currentAnimatorStateInfo.shortNameHash, out text))
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
					result = text;
				}
			}
		}
		return result;
	}

	internal int GetCurrentAnimatorStateHash()
	{
		Animator modelAnimator = this.GetModelAnimator();
		if (modelAnimator != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.GetCurrentAnimatorStateHash()).MethodHandle;
			}
			if (modelAnimator.layerCount >= 1)
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
				return modelAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash;
			}
		}
		return 0;
	}

	public string GetAnimatorHashToString(int hash)
	{
		string result;
		if (this.m_animatorStateNameHashToName.TryGetValue(hash, out result))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.GetAnimatorHashToString(int)).MethodHandle;
			}
			return result;
		}
		return "UNKNOWN";
	}

	internal bool IsPlayingAttackAnim()
	{
		bool flag;
		return this.IsPlayingAttackAnim(out flag);
	}

	public bool IsPlayingChargeEnd()
	{
		bool result = false;
		Animator modelAnimator = this.GetModelAnimator();
		if (modelAnimator != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.IsPlayingChargeEnd()).MethodHandle;
			}
			if (modelAnimator.layerCount >= 1)
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
				result = (modelAnimator.GetCurrentAnimatorStateInfo(0).tagHash == ActorModelData.s_animHashChargeEnd);
			}
		}
		return result;
	}

	internal unsafe bool IsPlayingAttackAnim(out bool endingAttack)
	{
		endingAttack = false;
		Animator modelAnimator = this.GetModelAnimator();
		if (!(modelAnimator == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.IsPlayingAttackAnim(bool*)).MethodHandle;
			}
			if (modelAnimator.layerCount >= 1)
			{
				AnimatorStateInfo currentAnimatorStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
				bool flag = modelAnimator.IsInTransition(0);
				if (currentAnimatorStateInfo.tagHash != ActorModelData.s_animHashAttack)
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
					if (currentAnimatorStateInfo.tagHash != ActorModelData.s_animHashChargeEnd)
					{
						return false;
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
				endingAttack = flag;
				return true;
			}
		}
		return false;
	}

	internal bool IsPlayingIdleAnim(bool excludeCover = false)
	{
		Animator modelAnimator = this.GetModelAnimator();
		if (modelAnimator == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.IsPlayingIdleAnim(bool)).MethodHandle;
			}
			return true;
		}
		if (modelAnimator.layerCount < 1)
		{
			return false;
		}
		AnimatorStateInfo currentAnimatorStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
		bool result;
		if (currentAnimatorStateInfo.tagHash == ActorModelData.s_animHashIdle || currentAnimatorStateInfo.shortNameHash == ActorModelData.s_animHashIdle)
		{
			if (excludeCover)
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
				result = !this.m_parentActorData.\u000E().HasAnyCover(false);
			}
			else
			{
				result = true;
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal bool IsPlayingDamageAnim()
	{
		Animator modelAnimator = this.GetModelAnimator();
		if (!(modelAnimator == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.IsPlayingDamageAnim()).MethodHandle;
			}
			if (modelAnimator.layerCount >= 1)
			{
				AnimatorStateInfo currentAnimatorStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
				bool result;
				if (currentAnimatorStateInfo.tagHash != ActorModelData.s_animHashDamage && currentAnimatorStateInfo.shortNameHash != ActorModelData.s_animHashDamage)
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
					result = (currentAnimatorStateInfo.tagHash == ActorModelData.s_animHashDamageNoInterrupt);
				}
				else
				{
					result = true;
				}
				return result;
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
		return false;
	}

	internal bool CanPlayDamageReactAnim()
	{
		Animator modelAnimator = this.GetModelAnimator();
		if (!(modelAnimator == null))
		{
			if (modelAnimator.layerCount >= 1)
			{
				AnimatorStateInfo currentAnimatorStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
				bool flag;
				if (currentAnimatorStateInfo.tagHash != ActorModelData.s_animHashIdle)
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
					flag = (currentAnimatorStateInfo.shortNameHash == ActorModelData.s_animHashIdle);
				}
				else
				{
					flag = true;
				}
				bool flag2 = flag;
				bool flag3;
				if (currentAnimatorStateInfo.tagHash != ActorModelData.s_animHashDamage)
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
					flag3 = (currentAnimatorStateInfo.shortNameHash == ActorModelData.s_animHashDamage);
				}
				else
				{
					flag3 = true;
				}
				bool flag4 = flag3;
				if (!flag2)
				{
					if (!flag4)
					{
						return false;
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
				bool result;
				if (!flag4)
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
					result = !modelAnimator.IsInTransition(0);
				}
				else
				{
					result = true;
				}
				return result;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.CanPlayDamageReactAnim()).MethodHandle;
			}
		}
		return false;
	}

	internal bool IsPlayingKnockdownAnim()
	{
		Animator modelAnimator = this.GetModelAnimator();
		if (!(modelAnimator == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.IsPlayingKnockdownAnim()).MethodHandle;
			}
			if (modelAnimator.layerCount >= 1)
			{
				return modelAnimator.GetCurrentAnimatorStateInfo(0).tagHash == ActorModelData.s_animHashKnockdown;
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
		return false;
	}

	private void StoreRigidBodyOffsets()
	{
		Rigidbody[] componentsInChildren = base.gameObject.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rigidbody in componentsInChildren)
		{
			this.m_initialBoneOffsetMap[rigidbody.gameObject] = rigidbody.transform.localPosition;
			this.m_initialBoneRotationOffsetMap[rigidbody.gameObject] = rigidbody.transform.localRotation;
			this.m_initialBoneScaleOffsetMap[rigidbody.gameObject] = rigidbody.transform.localScale;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.StoreRigidBodyOffsets()).MethodHandle;
		}
	}

	private void ResetRigidBodyOffsets()
	{
		Rigidbody[] componentsInChildren = base.gameObject.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rigidbody in componentsInChildren)
		{
			if (this.m_initialBoneOffsetMap.ContainsKey(rigidbody.gameObject))
			{
				rigidbody.transform.localPosition = this.m_initialBoneOffsetMap[rigidbody.gameObject];
				rigidbody.transform.localRotation = this.m_initialBoneRotationOffsetMap[rigidbody.gameObject];
				rigidbody.transform.localScale = this.m_initialBoneScaleOffsetMap[rigidbody.gameObject];
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.ResetRigidBodyOffsets()).MethodHandle;
		}
	}

	internal Renderer GetModelRenderer(int index = 0)
	{
		if (this.m_renderers != null)
		{
			if (this.m_renderers.Length > index)
			{
				return this.m_renderers[index];
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.GetModelRenderer(int)).MethodHandle;
			}
		}
		return null;
	}

	internal int GetNumModelRenderers()
	{
		int result;
		if (this.m_renderers == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.GetNumModelRenderers()).MethodHandle;
			}
			result = 0;
		}
		else
		{
			result = this.m_renderers.Length;
		}
		return result;
	}

	public Animator GetModelAnimator()
	{
		if (this.m_modelAnimator == null)
		{
			if (NetworkClient.active)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.GetModelAnimator()).MethodHandle;
				}
				Log.Error(this + " has a NULL model Animator", new object[0]);
			}
		}
		else if (this.m_modelAnimator.layerCount == 0)
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
			this.m_modelAnimator = base.GetComponentInChildren<Animator>();
			string str;
			if (this.m_parentActorData == null)
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
				str = "NULL";
			}
			else
			{
				str = this.m_parentActorData.ToString();
			}
			string str2 = " model Animator had zero layers! Refreshing... layers: ";
			string str3;
			if (this.m_modelAnimator == null)
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
				str3 = "NULL";
			}
			else
			{
				str3 = this.m_modelAnimator.layerCount.ToString();
			}
			Log.Error(str + str2 + str3, new object[0]);
		}
		return this.m_modelAnimator;
	}

	public void Setup(ActorData parentActorData)
	{
		Vector3 localScale = base.transform.localScale;
		this.m_parentActorData = parentActorData;
		base.transform.parent = parentActorData.transform;
		base.transform.localPosition = Vector3.zero;
		base.transform.localScale = localScale;
		GameObject gameObject = base.gameObject.FindInChildren("root_JNT", 0);
		if (gameObject != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.Setup(ActorData)).MethodHandle;
			}
			this.m_rootBoneTransform = gameObject.transform;
		}
		gameObject = base.gameObject.FindInChildren("hip_JNT", 0);
		if (gameObject != null)
		{
			this.m_hipBoneTransform = gameObject.transform;
		}
		gameObject = base.gameObject.FindInChildren("floor_JNT", 0);
		if (gameObject != null)
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
			this.m_floorBoneTransform = gameObject.transform;
		}
		this.StoreRigidBodyOffsets();
	}

	private void UpdateFloorBone()
	{
		if (this.m_rootBoneTransform != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.UpdateFloorBone()).MethodHandle;
			}
			if (this.m_hipBoneTransform != null && this.m_floorBoneTransform != null)
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
				Vector3 position = this.m_floorBoneTransform.position;
				position.y = Mathf.Min(this.m_rootBoneTransform.transform.position.y, this.m_hipBoneTransform.transform.position.y);
				this.m_floorBoneTransform.position = position;
			}
		}
	}

	private void UpdateRandomValueForAnimator()
	{
		float num = 0.25f;
		if (this.m_modelAnimator != null && this.m_lastAnimatorRandomSetTime + num < Time.time)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.UpdateRandomValueForAnimator()).MethodHandle;
			}
			if (this.m_hasRandomValueParameter)
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
				this.m_lastAnimatorRandomSetTime = Time.time;
				this.m_modelAnimator.SetFloat("RandomValue", UnityEngine.Random.value);
			}
		}
	}

	public bool IsInCinematicCam()
	{
		if (CameraManager.Get() == null)
		{
			return false;
		}
		if (CameraManager.Get().ShotSequence == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.IsInCinematicCam()).MethodHandle;
			}
			return false;
		}
		return !(CameraManager.Get().ShotSequence.Actor != this.m_parentActorData);
	}

	public void ForceUpdateVisibility()
	{
		this.UpdateVisibility();
		this.UpdateSelectionOutline();
	}

	private void Update()
	{
		bool flag = false;
		if (this.m_shroudInstancesToEnable != null)
		{
			for (int i = 0; i < this.m_shroudInstancesToEnable.Length; i++)
			{
				this.m_shroudInstances[i].enabled = this.m_shroudInstancesToEnable[i];
				flag = true;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.Update()).MethodHandle;
			}
			this.m_shroudInstancesToEnable = null;
		}
		if (this.m_dirtyRenderersCache && !flag)
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
			this.CacheRenderers();
		}
		if (this.m_parentActorData == null)
		{
			Log.Error("Setup was not called for ActorModelData on " + base.gameObject.name, new object[0]);
			return;
		}
		if (!this.m_attemptedToCreateBaseCircle)
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
			ActorVFX actorVFX = this.m_parentActorData.\u000E();
			if (actorVFX != null)
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
				actorVFX.SpawnBaseCircles();
			}
			this.m_attemptedToCreateBaseCircle = true;
		}
		if (!this.m_parentActorData.\u0012())
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
			this.UpdateFloorBone();
		}
		this.UpdateRandomValueForAnimator();
		this.UpdateStealth();
		this.UpdateVisibility();
		this.UpdateSelectionOutline();
		if (!this.m_isFace)
		{
			this.UpdateCameraTransparency();
		}
		bool flag2 = this.IsPlayingIdleAnim(true);
		if (this.m_needsStandingIdleBoundingBox)
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
			if (flag2)
			{
				goto IL_176;
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
		if (!(this.m_rendererBoundsApprox.size == Vector3.zero))
		{
			goto IL_280;
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
		IL_176:
		if (this.GetNumModelRenderers() > 0)
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
			this.m_needsStandingIdleBoundingBox = !flag2;
			float num = 0.95f * Board.\u000E().squareSize;
			float num2 = 4f;
			this.m_rendererBoundsApprox.size = new Vector3(num, num2, num);
			this.m_rendererBoundsApprox.center = new Vector3(0f, 0.5f * num2, 0f);
		}
		if (!this.m_isFace)
		{
			this.m_rendererBoundsApproxCollider = base.gameObject.AddComponent<BoxCollider>();
			this.m_rendererBoundsApproxCollider.center = this.m_rendererBoundsApprox.center;
			this.m_rendererBoundsApproxCollider.size = this.m_rendererBoundsApprox.size;
			Collider[] componentsInChildren = base.gameObject.GetComponentsInChildren<Collider>();
			foreach (Collider collider in componentsInChildren)
			{
				collider.gameObject.layer = ActorData.Layer;
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
		IL_280:
		if (this.m_activeRigidbodies != null)
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
				Vector3 position = Camera.main.transform.position;
				float num3 = Camera.main.farClipPlane * Camera.main.farClipPlane;
				for (int k = 0; k < this.m_activeRigidbodies.Length; k++)
				{
					Rigidbody rigidbody = this.m_activeRigidbodies[k];
					if (rigidbody != null)
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
						if (!rigidbody.IsSleeping())
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
							if (float.IsNaN(rigidbody.position.x))
							{
								rigidbody.isKinematic = true;
								rigidbody.detectCollisions = false;
								rigidbody.transform.localPosition = new Vector3(-10000f, 0f, 0f);
								rigidbody.Sleep();
							}
							else
							{
								float num4 = Vector3.SqrMagnitude(rigidbody.position - position);
								if (num4 > num3)
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
									rigidbody.isKinematic = true;
									rigidbody.detectCollisions = false;
									rigidbody.Sleep();
								}
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
		}
	}

	internal void SetMaterialShader(Shader shader, bool overrideDefault = false)
	{
		string name = shader.name;
		if (overrideDefault)
		{
			this.m_rendererDefaultMaterialsCacheKey = name;
		}
		if (this.m_appearanceNameToCachedRendererMaterials.ContainsKey(name))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.SetMaterialShader(Shader, bool)).MethodHandle;
			}
			List<Material[]> list = this.m_appearanceNameToCachedRendererMaterials[name];
			for (int i = 0; i < this.m_renderers.Length; i++)
			{
				Material[] array = list[i];
				Renderer renderer = this.m_renderers[i];
				for (int j = 0; j < renderer.materials.Length; j++)
				{
					array[j].shaderKeywords = renderer.materials[j].shaderKeywords;
					UnityEngine.Object.Destroy(renderer.materials[j]);
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
				renderer.materials = array;
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
		else
		{
			List<Material[]> list = new List<Material[]>(this.m_renderers.Length);
			for (int k = 0; k < this.m_renderers.Length; k++)
			{
				Renderer renderer2 = this.m_renderers[k];
				Material[] array2 = new Material[renderer2.sharedMaterials.Length];
				for (int l = 0; l < array2.Length; l++)
				{
					array2[l] = new Material(renderer2.sharedMaterials[l])
					{
						shader = shader,
						shaderKeywords = renderer2.sharedMaterials[l].shaderKeywords
					};
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
				list.Add(array2);
				for (int m = 0; m < renderer2.materials.Length; m++)
				{
					UnityEngine.Object.Destroy(renderer2.materials[m]);
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
				renderer2.materials = array2;
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
			this.m_appearanceNameToCachedRendererMaterials[name] = list;
		}
		this.m_alphaUpdateMarkedDirty = true;
	}

	public void ResetMaterialsToDefaults()
	{
		if (this.m_appearanceNameToCachedRendererMaterials.ContainsKey(this.m_rendererDefaultMaterialsCacheKey))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.ResetMaterialsToDefaults()).MethodHandle;
			}
			List<Material[]> list = this.m_appearanceNameToCachedRendererMaterials[this.m_rendererDefaultMaterialsCacheKey];
			for (int i = 0; i < this.m_renderers.Length; i++)
			{
				Renderer renderer = this.m_renderers[i];
				if (renderer != null)
				{
					Material[] materials = list[i];
					for (int j = 0; j < renderer.materials.Length; j++)
					{
						UnityEngine.Object.Destroy(renderer.materials[j]);
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
					renderer.materials = materials;
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
			this.DisableMaterialKeyword(this.c_stealthShaderKeyword);
			this.SetMaterialFloatTeam();
			if (this.m_stealthShaderCullModeSettings != null)
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
				for (int k = 0; k < this.m_stealthShaderCullModeSettings.Count; k++)
				{
					ActorModelData.CullModeSettings cullModeSettings = this.m_stealthShaderCullModeSettings[k];
					if (!cullModeSettings.m_forStealth)
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
						this.SetMaterialFloatByNameMatch(ActorModelData.s_materialPropertyIDCullMode, (float)cullModeSettings.m_desiredCullMode, cullModeSettings.m_targetMaterialSearchPtn);
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
		this.m_alphaUpdateMarkedDirty = true;
	}

	private void SetLayer(int newLayer, int oldLayer)
	{
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			Renderer renderer = this.m_renderers[i];
			if (renderer.gameObject.layer == oldLayer)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.SetLayer(int, int)).MethodHandle;
				}
				renderer.gameObject.layer = newLayer;
			}
		}
	}

	private void SetMaterialFloat(int propertyID, float value)
	{
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			Renderer renderer = this.m_renderers[i];
			foreach (Material material in renderer.materials)
			{
				material.SetFloat(propertyID, value);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.SetMaterialFloat(int, float)).MethodHandle;
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

	private void SetMaterialFloatByNameMatch(int propertyID, float value, string searchPtn)
	{
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			Renderer renderer = this.m_renderers[i];
			Material[] materials = renderer.materials;
			int j = 0;
			while (j < materials.Length)
			{
				Material material = materials[j];
				if (string.IsNullOrEmpty(searchPtn))
				{
					goto IL_4E;
				}
				if (material.name.IndexOf(searchPtn) >= 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.SetMaterialFloatByNameMatch(int, float, string)).MethodHandle;
						goto IL_4E;
					}
					goto IL_4E;
				}
				IL_57:
				j++;
				continue;
				IL_4E:
				material.SetFloat(propertyID, value);
				goto IL_57;
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

	private void SetMinMaterialFloat(int propertyId, float value)
	{
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			Renderer renderer = this.m_renderers[i];
			foreach (Material material in renderer.materials)
			{
				material.SetFloat(propertyId, Mathf.Max(value, material.GetFloat(propertyId)));
			}
		}
	}

	private void SetMaterialVector(int id, Vector4 value)
	{
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			Renderer renderer = this.m_renderers[i];
			foreach (Material material in renderer.materials)
			{
				material.SetVector(id, value);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.SetMaterialVector(int, Vector4)).MethodHandle;
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

	internal void EnableMaterialKeyword(string keyword)
	{
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			Renderer renderer = this.m_renderers[i];
			foreach (Material material in renderer.materials)
			{
				material.EnableKeyword(keyword);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.EnableMaterialKeyword(string)).MethodHandle;
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

	internal void DisableMaterialKeyword(string keyword)
	{
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			Renderer renderer = this.m_renderers[i];
			foreach (Material material in renderer.materials)
			{
				material.DisableKeyword(keyword);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.DisableMaterialKeyword(string)).MethodHandle;
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

	internal void SetMaterialKeywordOnAllCachedMaterials(string keyword, bool enable)
	{
		foreach (KeyValuePair<string, List<Material[]>> keyValuePair in this.m_appearanceNameToCachedRendererMaterials)
		{
			List<Material[]> value = keyValuePair.Value;
			for (int i = 0; i < value.Count; i++)
			{
				foreach (Material material in value[i])
				{
					if (material != null)
					{
						if (enable)
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
								RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.SetMaterialKeywordOnAllCachedMaterials(string, bool)).MethodHandle;
							}
							material.EnableKeyword(keyword);
						}
						else
						{
							material.DisableKeyword(keyword);
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

	private void SetMaterialRenderQueue(int renderQueue)
	{
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			Renderer renderer = this.m_renderers[i];
			Material[] materials = renderer.materials;
			for (int j = 0; j < materials.Length; j++)
			{
				Material material = materials[j];
				Material material2 = material;
				int renderQueue2;
				if (renderQueue < -1)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.SetMaterialRenderQueue(int)).MethodHandle;
					}
					renderQueue2 = renderer.sharedMaterials[j].renderQueue;
				}
				else
				{
					renderQueue2 = renderQueue;
				}
				material2.renderQueue = renderQueue2;
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

	private void ScaleMaterialColorToSDR(int propertyId)
	{
		for (int i = 0; i < this.m_renderers.Length; i++)
		{
			Renderer renderer = this.m_renderers[i];
			foreach (Material material in renderer.materials)
			{
				Color color = material.GetColor(propertyId);
				float maxColorComponent = color.maxColorComponent;
				if (maxColorComponent > 1f)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.ScaleMaterialColorToSDR(int)).MethodHandle;
					}
					material.SetColor(propertyId, color * 1f / maxColorComponent);
				}
			}
		}
	}

	private void KillPopcornFXPlayOnStart()
	{
		this.m_stealthPKFXStopped = true;
		for (int i = 0; i < this.m_popcornFXPlayOnStartComponents.Count; i++)
		{
			PKFxFX pkfxFX = this.m_popcornFXPlayOnStartComponents.ElementAt(i);
			pkfxFX.KillEffect();
		}
	}

	public void RestartPopcornFXPlayOnStart()
	{
		if (this.m_visibleToClient)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.RestartPopcornFXPlayOnStart()).MethodHandle;
			}
			this.m_stealthPKFXStopped = false;
			for (int i = 0; i < this.m_popcornFXPlayOnStartComponents.Count; i++)
			{
				PKFxFX pkfxFX = this.m_popcornFXPlayOnStartComponents.ElementAt(i);
				pkfxFX.StartEffect();
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

	internal void EnableRendererAndUpdateVisibility()
	{
		foreach (Renderer renderer in this.m_renderers)
		{
			if (renderer != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.EnableRendererAndUpdateVisibility()).MethodHandle;
				}
				renderer.enabled = true;
			}
		}
		if (this.m_shroudInstances != null)
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
			foreach (ShroudInstance shroudInstance in this.m_shroudInstances)
			{
				shroudInstance.enabled = true;
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
		this.m_visibleToClient = true;
		this.m_forceHideRenderers = false;
		this.UpdateVisibility();
		if (this.m_visibleToClient)
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
			this.SetPersistentVfxActive(true);
		}
	}

	internal void DisableAndHideRenderers()
	{
		foreach (Renderer renderer in this.m_renderers)
		{
			if (renderer != null)
			{
				renderer.enabled = false;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.DisableAndHideRenderers()).MethodHandle;
		}
		if (this.m_shroudInstances != null)
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
			foreach (ShroudInstance shroudInstance in this.m_shroudInstances)
			{
				shroudInstance.enabled = false;
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
		this.m_forceHideRenderers = true;
	}

	internal void SetDefaultRendererAlpha(float alpha)
	{
		if (this.m_rendererDefaultColors != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.SetDefaultRendererAlpha(float)).MethodHandle;
			}
			for (int i = 0; i < this.m_rendererDefaultColors.Count; i++)
			{
				for (int j = 0; j < this.m_rendererDefaultColors[i].Length; j++)
				{
					this.m_rendererDefaultColors[i][j].a = alpha;
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

	internal void CacheDefaultRendererAlphas()
	{
		if (this.m_rendererDefaultColors != null)
		{
			this.m_cachedRendererAlphas = new List<float>();
			for (int i = 0; i < this.m_rendererDefaultColors.Count; i++)
			{
				for (int j = 0; j < this.m_rendererDefaultColors[i].Length; j++)
				{
					this.m_cachedRendererAlphas.Add(this.m_rendererDefaultColors[i][j].a);
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.CacheDefaultRendererAlphas()).MethodHandle;
				}
			}
		}
	}

	internal void RestoreDefaultRendererAlphas()
	{
		if (this.m_rendererDefaultColors != null && this.m_cachedRendererAlphas != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.RestoreDefaultRendererAlphas()).MethodHandle;
			}
			int num = 0;
			for (int i = 0; i < this.m_rendererDefaultColors.Count; i++)
			{
				for (int j = 0; j < this.m_rendererDefaultColors[i].Length; j++)
				{
					this.m_rendererDefaultColors[i][j].a = this.m_cachedRendererAlphas[num++];
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
		this.m_cachedRendererAlphas = null;
	}

	internal void SetCameraTransparency(float startTransparency, float transparencyTime, float fadeStartDelayDuration)
	{
		this.m_cameraTransparency = startTransparency;
		this.m_cameraTransparencyStartValue = startTransparency;
		this.m_cameraTransparencyTime = transparencyTime;
		this.m_cameraTransparencyLastChangeSetTime = Time.time + fadeStartDelayDuration;
	}

	internal void SetMasterSkinVfxInst(GameObject vfxInst)
	{
		if (this.m_masterSkinVfxInst != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.SetMasterSkinVfxInst(GameObject)).MethodHandle;
			}
			Log.Warning("Setting master skin vfx instance when there is existing entry", new object[0]);
		}
		this.m_masterSkinVfxInst = vfxInst;
	}

	private void UpdateVisibility()
	{
		bool flag2;
		if (this.m_isFace)
		{
			bool flag;
			if (CameraManager.Get() != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.UpdateVisibility()).MethodHandle;
				}
				flag = CameraManager.Get().InFaceShot(this.m_parentActorData);
			}
			else
			{
				flag = false;
			}
			flag2 = flag;
		}
		else
		{
			flag2 = (!this.m_forceHideRenderers && this.m_parentActorData.\u0018() && this.m_cameraTransparency > 0f);
		}
		if (this.m_visibleToClient == flag2)
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
			if (!this.m_forceUpdateVisibility)
			{
				return;
			}
		}
		this.m_forceUpdateVisibility = false;
		this.m_visibleToClient = flag2;
		if (this.m_shroudInstances != null)
		{
			foreach (ShroudInstance shroudInstance in this.m_shroudInstances)
			{
				if (shroudInstance != null)
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
					shroudInstance.enabled = flag2;
				}
			}
		}
		if (this.m_renderers != null)
		{
			foreach (Renderer renderer in this.m_renderers)
			{
				if (renderer != null)
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
					renderer.enabled = flag2;
				}
			}
		}
		if (this.m_projector != null)
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
			this.m_projector.enabled = flag2;
		}
		ParticleSystem[] componentsInChildren = base.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			if (!flag2)
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
				particleSystem.Clear();
			}
			particleSystem.emission.enabled = flag2;
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
		this.SetPersistentVfxActive(flag2);
	}

	private void SetPersistentVfxActive(bool active)
	{
		foreach (ActorModelData.PersistentVFXInfo persistentVFXInfo in this.m_persistentVFX)
		{
			if (persistentVFXInfo != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.SetPersistentVfxActive(bool)).MethodHandle;
				}
				if (persistentVFXInfo.m_vfxInstance != null)
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
					if (persistentVFXInfo.m_vfxInstance.activeSelf != active)
					{
						persistentVFXInfo.m_vfxInstance.SetActive(active);
					}
				}
			}
		}
		if (this.m_masterSkinVfxInst != null)
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
			this.m_masterSkinVfxInst.gameObject.SetActiveIfNeeded(active);
		}
	}

	private void UpdateStealth()
	{
		if (this.m_isFace)
		{
			return;
		}
		bool flag;
		if (!this.m_parentActorData.\u000E().HasStatus(StatusType.Revealed, false))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.UpdateStealth()).MethodHandle;
			}
			if (!CaptureTheFlag.IsActorRevealedByFlag_Client(this.m_parentActorData))
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
				flag = this.m_parentActorData.VisibleTillEndOfPhase;
				goto IL_5D;
			}
		}
		flag = true;
		IL_5D:
		bool flag2 = flag;
		bool flag3 = this.m_parentActorData.\u000E().IsInvisibleToEnemies(false);
		List<Plane> list = null;
		if (this.m_stealthBrushTransitionParameter <= 0f)
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
			if (this.m_stealthBrushTransitionParameter.GetEndValue() != 1f)
			{
				goto IL_14A;
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
		if (BrushCoordinator.Get() != null)
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
			BoardSquare boardSquare = this.m_parentActorData.\u000E();
			Vector3 vector;
			if (boardSquare)
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
				vector = boardSquare.ToVector3();
			}
			else
			{
				vector = base.transform.position;
			}
			Vector3 center = vector;
			Bounds bounds = new Bounds(center, new Vector3(0.1f, 0.1f, 0.1f));
			bounds.Encapsulate(this.m_parentActorData.PreviousBoardSquarePosition);
			list = BrushCoordinator.Get().CalcIntersectingBrushSidePlanes(bounds);
		}
		IL_14A:
		int num;
		if (list != null)
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
			num = list.Count;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		int num3 = this.m_parentActorData.\u0018();
		bool flag4 = num3 >= 0;
		bool flag5 = BrushCoordinator.Get() != null && this.m_parentActorData.\u0016();
		bool flag6;
		if (flag4 && BrushCoordinator.Get() != null)
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
			flag6 = !BrushCoordinator.Get().IsRegionFunctioning(num3);
		}
		else
		{
			flag6 = false;
		}
		bool flag7 = flag6;
		bool flag8;
		if (!flag5)
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
			flag8 = (num2 > 0);
		}
		else
		{
			flag8 = true;
		}
		bool flag9 = flag8;
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
			if (!flag3 && !flag5)
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
			}
			else
			{
				if (this.m_stealthBrokenParameter.GetEndValue() == 1f)
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
					this.m_stealthBrokenParameter.EaseTo(0f, 0.0166666675f);
				}
				if (flag3)
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
					if (this.m_stealthFadeParameter.GetEndValue() == 0f)
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
						this.m_stealthFadeParameter.EaseTo(1f, this.m_stealthFadeDuration);
					}
				}
				if (!flag9)
				{
					goto IL_3DD;
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
				if (this.m_stealthBrushParameter.GetEndValue() == 0f)
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
					this.m_stealthBrushParameter.EaseTo(1f, 0.0166666675f);
					goto IL_3DD;
				}
				goto IL_3DD;
			}
		}
		if (this.m_stealthFadeParameter.GetEndValue() != 1f)
		{
			if (flag7)
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
			}
			else
			{
				if (flag9)
				{
					goto IL_323;
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
				if (this.m_stealthBrushParameter.GetEndValue() == 1f)
				{
					this.m_stealthBrushParameter.EaseTo(0f, 0.0166666675f);
					goto IL_323;
				}
				goto IL_323;
			}
		}
		if (this.m_stealthShaderEnabled)
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
			if (this.m_stealthBrokenParameter.GetEndValue() == 0f)
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
				this.m_stealthBrokenParameter.EaseTo(1f, this.m_stealthBrokenEaseDuration);
				if (this.m_stealthFadeParameter.GetEndValue() == 1f)
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
					this.m_stealthFadeParameter.EaseTo(0f, this.m_stealthBrokenEaseDuration);
				}
				if (this.m_stealthBrushParameter.GetEndValue() == 0f)
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
					this.m_stealthBrushParameter.EaseTo(0f, this.m_stealthBrokenEaseDuration);
				}
			}
		}
		IL_323:
		IL_3DD:
		bool flag10;
		if (this.m_stealthBrushParameter.GetEndValue() == 1f)
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
			flag10 = (this.m_stealthBrokenParameter <= 0.99f);
		}
		else
		{
			flag10 = false;
		}
		bool flag11 = flag10;
		bool flag12 = this.m_stealthFadeParameter.GetEndValue() == 1f;
		if (!flag12)
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
			if (flag11)
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
			}
			else
			{
				if (this.m_stealthShaderEnabled && this.m_stealthFadeParameter.GetEndValue() == 0f && this.m_stealthFadeParameter.EaseFinished())
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
					if (this.m_stealthBrushParameter.GetEndValue() == 0f)
					{
						if (this.m_stealthBrushParameter.EaseFinished())
						{
							goto IL_6E4;
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
					if (this.m_stealthBrokenParameter.GetEndValue() != 1f)
					{
						goto IL_6F1;
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
					if (!this.m_stealthBrushParameter.EaseFinished())
					{
						goto IL_6F1;
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
					IL_6E4:
					this.ResetMaterialsToDefaults();
					this.m_stealthShaderEnabled = false;
				}
				IL_6F1:
				if (Mathf.Min(this.m_stealthFadeParameter, 1f - this.m_stealthBrushTransitionParameter) <= this.m_stealthParameterStartPKFX && this.m_stealthPKFXStopped)
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
					this.RestartPopcornFXPlayOnStart();
					goto IL_734;
				}
				goto IL_734;
			}
		}
		if (!this.m_stealthShaderEnabled)
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
			if (this.m_stealthReplacementShader != null)
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
				this.SetMaterialShader(this.m_stealthReplacementShader, false);
				this.ScaleMaterialColorToSDR(ActorModelData.materialOutlineColorProperty);
				this.SetMinMaterialFloat(ActorModelData.materialOutlineProperty, 1E-05f);
			}
			this.EnableMaterialKeyword(this.c_stealthShaderKeyword);
			this.SetMaterialRenderQueue(0xBB8);
			this.SetMaterialFloatTeam();
			if (this.m_stealthShaderCullModeSettings != null)
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
				for (int i = 0; i < this.m_stealthShaderCullModeSettings.Count; i++)
				{
					ActorModelData.CullModeSettings cullModeSettings = this.m_stealthShaderCullModeSettings[i];
					if (cullModeSettings.m_forStealth)
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
						this.SetMaterialFloatByNameMatch(ActorModelData.s_materialPropertyIDCullMode, (float)cullModeSettings.m_desiredCullMode, cullModeSettings.m_targetMaterialSearchPtn);
					}
				}
			}
			this.m_stealthShaderEnabled = true;
		}
		if (Mathf.Max(this.m_stealthFadeParameter, 1f - this.m_stealthBrushTransitionParameter) >= this.m_stealthParameterStopPKFX)
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
			if (!flag12)
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
				if (!this.m_parentActorData.\u0016())
				{
					goto IL_5AC;
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
			if (!this.m_stealthPKFXStopped)
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
				this.KillPopcornFXPlayOnStart();
				goto IL_63D;
			}
		}
		IL_5AC:
		if (this.m_stealthPKFXStopped && !flag12)
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
			if (this.m_stealthBrushTransitionParameter.GetEndValue() != 1f)
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
				if (this.m_stealthBrokenParameter.GetEndValue() != 1f)
				{
					goto IL_63D;
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
			if (1f - Mathf.Max(this.m_stealthBrushTransitionParameter, this.m_stealthBrokenParameter) <= this.m_stealthParameterStartPKFX)
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
				this.RestartPopcornFXPlayOnStart();
			}
		}
		IL_63D:
		IL_734:
		if (this.m_stealthShaderEnabled)
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
			this.SetMaterialFloat(ActorModelData.s_materialPropertyIDStealthFade, this.m_stealthFadeParameter);
			this.SetMaterialFloat(ActorModelData.s_materialPropertyIDBrush, this.m_stealthBrushParameter);
			this.SetMaterialFloat(ActorModelData.s_materialPropertyIDStealthMoving, this.m_stealthBrushTransitionParameter);
			this.SetMaterialFloat(ActorModelData.s_materialPropertyIDStealthBroken, this.m_stealthBrokenParameter);
			int propertyID = ActorModelData.s_materialPropertyIDVisibleToClient;
			float value;
			if (this.m_parentActorData.\u0018())
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
				value = 1f;
			}
			else
			{
				value = 0f;
			}
			this.SetMaterialFloat(propertyID, value);
			for (int j = 0; j < ActorModelData.s_stealthMaterialPlaneVectorIDs.Length; j++)
			{
				if (num2 > j)
				{
					Plane plane = list[j];
					this.SetMaterialVector(ActorModelData.s_stealthMaterialPlaneVectorIDs[j], new Vector4(plane.normal.x, plane.normal.y, plane.normal.z, plane.distance - (1f - this.m_parentActorData.\u000E().speed) * this.m_stealthStoppedPlaneOffset));
				}
				else
				{
					this.SetMaterialVector(ActorModelData.s_stealthMaterialPlaneVectorIDs[j], Vector4.zero);
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

	private bool IsAnimatingStealthActivation()
	{
		bool result;
		if (this.m_stealthShaderEnabled)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.IsAnimatingStealthActivation()).MethodHandle;
			}
			if (!this.m_parentActorData.\u0016() && this.m_stealthFadeParameter.GetEndValue() == 1f)
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
				if (!Mathf.Approximately(this.m_stealthFadeParameter, 1f))
				{
					result = true;
					goto IL_8B;
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
			result = !Mathf.Approximately(this.m_stealthBrushTransitionParameter, 0f);
			IL_8B:;
		}
		else
		{
			result = false;
		}
		return result;
	}

	private void UpdateSelectionOutline()
	{
		bool flag = false;
		bool flag2 = this.m_parentActorData.\u000E();
		bool flag3 = this.m_parentActorData.\u0018();
		bool drawingInConfirm = false;
		if (flag3)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.UpdateSelectionOutline()).MethodHandle;
			}
			if (!flag2)
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
				if (this.m_renderers != null)
				{
					ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
					bool flag4;
					if (!this.m_parentActorData.BeingTargetedByClientAbility(out flag4, out drawingInConfirm))
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
						if (!(activeOwnedActorData != null))
						{
							goto IL_9F;
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
						if (!activeOwnedActorData.ShouldForceTargetOutlineForActor(this.m_parentActorData))
						{
							goto IL_9F;
						}
					}
					flag = true;
				}
			}
		}
		IL_9F:
		if (flag)
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
				PlayerSelectionEffect component = Camera.main.GetComponent<PlayerSelectionEffect>();
				if (component != null)
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
					component.m_drawSelection = true;
					component.SetDrawingInConfirm(drawingInConfirm);
				}
			}
		}
		if (flag != this.m_showingOutline)
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
			int num = LayerMask.NameToLayer("ActorSelected");
			int num2 = LayerMask.NameToLayer("Actor");
			if (flag)
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
				this.SetMaterialFloatTeam();
				this.SetLayer(num, num2);
			}
			else
			{
				this.SetLayer(num2, num);
			}
			this.m_showingOutline = flag;
			PlayerSelectionEffect playerSelectionEffect;
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
				playerSelectionEffect = Camera.main.GetComponent<PlayerSelectionEffect>();
			}
			else
			{
				playerSelectionEffect = null;
			}
			PlayerSelectionEffect playerSelectionEffect2 = playerSelectionEffect;
			if (playerSelectionEffect2 != null)
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
				playerSelectionEffect2.SetDrawingInConfirm(drawingInConfirm);
			}
		}
	}

	private void UpdateAlphaForRenderer(Renderer curRenderer, int curRendererIndex)
	{
		if (!(curRenderer == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.UpdateAlphaForRenderer(Renderer, int)).MethodHandle;
			}
			if (this.m_rendererDefaultColors == null)
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
			}
			else
			{
				Material[] materials = curRenderer.materials;
				if (curRenderer == null)
				{
					return;
				}
				for (int i = 0; i < materials.Length; i++)
				{
					if (this.m_rendererDefaultColors.Count <= curRendererIndex)
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
					}
					else if (materials[i] != null)
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
						if (materials[i].HasProperty(ActorModelData.materialColorProperty))
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
							Color color = this.m_rendererDefaultColors[curRendererIndex][i];
							if (this.m_rendererDefaultColors[curRendererIndex][i].a == 1f)
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
								color.a = this.Alpha;
							}
							else
							{
								color.a = this.m_rendererDefaultColors[curRendererIndex][i].a;
							}
							materials[i].color = color;
						}
					}
				}
				return;
			}
		}
	}

	private void UpdateCameraTransparency()
	{
		if (Time.time >= this.m_cameraTransparencyLastChangeSetTime)
		{
			this.m_cameraTransparency += (1f - this.m_cameraTransparencyStartValue) * Time.deltaTime / this.m_cameraTransparencyTime;
			this.m_cameraTransparency = Mathf.Min(1f, this.m_cameraTransparency);
		}
		if (this.Alpha != this.m_cameraTransparency)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.UpdateCameraTransparency()).MethodHandle;
			}
			if (this.m_alphaUpdateMarkedDirty)
			{
				this.Alpha = this.m_cameraTransparency;
				this.m_alphaUpdateMarkedDirty = false;
				for (int i = 0; i < this.m_renderers.Length; i++)
				{
					this.UpdateAlphaForRenderer(this.m_renderers[i], i);
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

	internal float GetCamStartEventDelay(int animationIndex, bool useTauntCamAltTime)
	{
		if (useTauntCamAltTime)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.GetCamStartEventDelay(int, bool)).MethodHandle;
			}
			if (this.m_tauntCamStartEventDelays != null)
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
				if (animationIndex >= 0)
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
					if (animationIndex < this.m_tauntCamStartEventDelays.Length)
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
						if (this.m_tauntCamStartEventDelays[animationIndex] > 0f)
						{
							return this.m_tauntCamStartEventDelays[animationIndex];
						}
					}
				}
			}
		}
		float result;
		if (animationIndex >= 0 && animationIndex < this.m_camStartEventDelays.Length)
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
			result = this.m_camStartEventDelays[animationIndex];
		}
		else
		{
			result = 0f;
		}
		return result;
	}

	internal void EnableRagdoll(bool ragDollOn, ActorModelData.ImpulseInfo impulseInfo = null)
	{
		Animator modelAnimator = this.GetModelAnimator();
		if (modelAnimator != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.EnableRagdoll(bool, ActorModelData.ImpulseInfo)).MethodHandle;
			}
			modelAnimator.enabled = !ragDollOn;
		}
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
		{
			if (skinnedMeshRenderer.enabled)
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
				skinnedMeshRenderer.updateWhenOffscreen = ragDollOn;
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
		int layer = LayerMask.NameToLayer("DeadActor");
		int layer2 = LayerMask.NameToLayer("Actor");
		Rigidbody[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rigidbody in componentsInChildren2)
		{
			rigidbody.isKinematic = !ragDollOn;
			rigidbody.detectCollisions = ragDollOn;
			if (ragDollOn)
			{
				rigidbody.WakeUp();
			}
			else
			{
				rigidbody.Sleep();
			}
		}
		Rigidbody[] activeRigidbodies;
		if (ragDollOn)
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
			activeRigidbodies = componentsInChildren2;
		}
		else
		{
			activeRigidbodies = null;
		}
		this.m_activeRigidbodies = activeRigidbodies;
		Collider[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<Collider>();
		foreach (Collider collider in componentsInChildren3)
		{
			collider.enabled = ragDollOn;
			if (ragDollOn)
			{
				collider.gameObject.layer = layer;
			}
			else
			{
				collider.gameObject.layer = layer2;
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
		if (this.m_rendererBoundsApproxCollider != null)
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
			this.m_rendererBoundsApproxCollider.enabled = !ragDollOn;
		}
		if (ragDollOn)
		{
			this.ApplyImpulseOnRagdoll(impulseInfo, componentsInChildren2);
		}
		else
		{
			this.ResetRigidBodyOffsets();
		}
	}

	internal void ApplyImpulseOnRagdoll(ActorModelData.ImpulseInfo impulseInfo, Rigidbody[] rigidBodies)
	{
		if (impulseInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.ApplyImpulseOnRagdoll(ActorModelData.ImpulseInfo, Rigidbody[])).MethodHandle;
			}
			if (Application.isEditor)
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
				Log.Info("Applying impulse on " + base.gameObject.name + ", impulse info:" + impulseInfo.GetDebugString(), new object[0]);
			}
			if (rigidBodies == null)
			{
				rigidBodies = base.gameObject.GetComponentsInChildren<Rigidbody>();
			}
			if (impulseInfo.IsExplosion)
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
				if (TheatricsManager.RagdollOnlyApplyForceAtSingleJoint() && this.m_cachedRigidbodyForRagdollImpulse != null)
				{
					Vector3 a = this.m_parentActorData.\u0018() - impulseInfo.ExplosionCenter;
					a.y = Mathf.Max(0.75f, a.y);
					float magnitude = a.magnitude;
					if (magnitude > 0.1f)
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
						a.Normalize();
					}
					else
					{
						a = Vector3.up;
					}
					if (Application.isEditor)
					{
						Log.Info("Applying impulse on " + this.m_cachedRigidbodyForRagdollImpulse.name + ", impulse info:" + impulseInfo.GetDebugString(), new object[0]);
					}
					this.m_cachedRigidbodyForRagdollImpulse.AddForce(impulseInfo.ExplosionMagnitude * a, ForceMode.Impulse);
				}
				else
				{
					float num;
					if (rigidBodies.Length > 0)
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
						num = 1f / (float)rigidBodies.Length;
					}
					else
					{
						num = 1f;
					}
					float num2 = num;
					foreach (Rigidbody rigidbody in rigidBodies)
					{
						Vector3 vector = rigidbody.ClosestPointOnBounds(impulseInfo.ExplosionCenter);
						Vector3 a2 = vector - impulseInfo.ExplosionCenter;
						a2.y = Mathf.Max(0.75f, a2.y);
						float sqrMagnitude = a2.sqrMagnitude;
						if (sqrMagnitude > 0.1f)
						{
							a2.Normalize();
						}
						else
						{
							a2 = Vector3.up;
						}
						float num3 = 1f;
						float d = Mathf.Clamp(num2 * num3 * impulseInfo.ExplosionMagnitude, 1f, 3000f);
						rigidbody.AddForceAtPosition(a2 * d, vector, ForceMode.Impulse);
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
					if (Application.isEditor)
					{
						Log.Info(string.Concat(new object[]
						{
							"Applying impulse on ALL rigidbodies, Impulse on each = ",
							num2 * impulseInfo.ExplosionMagnitude,
							" (total impulse = ",
							impulseInfo.ExplosionMagnitude,
							")"
						}), new object[0]);
					}
				}
			}
			else
			{
				if (TheatricsManager.RagdollOnlyApplyForceAtSingleJoint())
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
					if (this.m_cachedRigidbodyForRagdollImpulse != null)
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
						if (Application.isEditor)
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
							Log.Info("Applying impulse on " + this.m_cachedRigidbodyForRagdollImpulse.name + ", impulse info:" + impulseInfo.GetDebugString(), new object[0]);
						}
						this.m_cachedRigidbodyForRagdollImpulse.AddForce(impulseInfo.HitImpulse, ForceMode.Impulse);
						return;
					}
				}
				float num4 = float.MaxValue;
				Rigidbody rigidbody2;
				if (rigidBodies.Length > 0)
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
					rigidbody2 = rigidBodies[0];
				}
				else
				{
					rigidbody2 = null;
				}
				Rigidbody rigidbody3 = rigidbody2;
				Vector3 position = impulseInfo.HitPosition;
				Vector3 hitPosition = impulseInfo.HitPosition;
				foreach (Rigidbody rigidbody4 in rigidBodies)
				{
					Vector3 vector2 = rigidbody4.ClosestPointOnBounds(hitPosition);
					float sqrMagnitude2 = (vector2 - hitPosition).sqrMagnitude;
					if (sqrMagnitude2 < num4)
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
						num4 = sqrMagnitude2;
						rigidbody3 = rigidbody4;
						position = vector2;
					}
				}
				if (rigidbody3 != null)
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
					rigidbody3.AddForceAtPosition(impulseInfo.HitImpulse, position, ForceMode.Impulse);
					if (Application.isEditor)
					{
						Log.Info(string.Concat(new object[]
						{
							"Applying impulse on rigidbody ",
							rigidbody3.name,
							", with impulse magnitude ",
							impulseInfo.HitImpulse.magnitude
						}), new object[0]);
					}
				}
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(this.m_rendererBoundsApprox.center + base.transform.position, this.m_rendererBoundsApprox.size);
	}

	[Serializable]
	public class PersistentVFXInfo
	{
		public GameObject m_vfxPrefab;

		[JointPopup("FX attach joint (or start position for projectiles).")]
		public JointPopupProperty m_fxJoint;

		[HideInInspector]
		public GameObject m_vfxInstance;
	}

	public enum ActionAnimationType
	{
		None,
		Ability1,
		Ability2,
		Ability3,
		Ability4,
		Ability5,
		Ability6,
		Ability7,
		Ability8,
		Ability9,
		Ability10,
		Item1,
		Item2,
		Item3,
		Item4,
		Item5,
		Item6,
		Item7,
		Item8,
		Item9,
		Item10
	}

	[Serializable]
	public class CullModeSettings
	{
		public string m_targetMaterialSearchPtn = string.Empty;

		public bool m_forStealth;

		public CullMode m_desiredCullMode;
	}

	internal enum RagdollActivation
	{
		None,
		HealthBased
	}

	internal class ImpulseInfo
	{
		private Vector3 m_position;

		internal ImpulseInfo(float explosionRadius, Vector3 explosionCenter)
		{
			this.ExplosionRadius = explosionRadius;
			this.m_position = explosionCenter;
			this.ExplosionMagnitude = TheatricsManager.GetRagdollImpactForce();
		}

		internal ImpulseInfo(Vector3 hitPosition, Vector3 hitDirection)
		{
			hitDirection.y = Mathf.Max(0.75f, hitDirection.y);
			float d = TheatricsManager.GetRagdollImpactForce();
			if (hitDirection.sqrMagnitude > 0f)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.ImpulseInfo..ctor(Vector3, Vector3)).MethodHandle;
				}
				hitDirection.Normalize();
			}
			else
			{
				if (Application.isEditor)
				{
					Log.Warning("Ragdoll impulse has 0 vector as impulse direction", new object[0]);
				}
				hitDirection = Vector3.up;
				d = 0.1f;
			}
			this.m_position = hitPosition;
			this.HitImpulse = hitDirection * d;
		}

		public string GetDebugString()
		{
			string text = string.Empty;
			if (this.IsExplosion)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorModelData.ImpulseInfo.GetDebugString()).MethodHandle;
				}
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"Explosion Type Impulse, Radius= ",
					this.ExplosionRadius,
					" | Magnitude = ",
					this.ExplosionMagnitude,
					" | Center= ",
					this.ExplosionCenter
				});
			}
			else
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"Impulse FromPos= ",
					this.HitPosition,
					" | Magnitude= ",
					this.HitImpulse.magnitude,
					" | ImpulseDir= ",
					this.HitImpulse.normalized
				});
			}
			return text;
		}

		internal bool IsExplosion
		{
			get
			{
				return this.ExplosionRadius > 0f;
			}
		}

		internal float ExplosionRadius { get; set; }

		internal Vector3 ExplosionCenter
		{
			get
			{
				return this.m_position;
			}
		}

		internal float ExplosionMagnitude { get; set; }

		internal Vector3 HitPosition
		{
			get
			{
				return this.m_position;
			}
		}

		internal Vector3 HitImpulse { get; set; }
	}
}
