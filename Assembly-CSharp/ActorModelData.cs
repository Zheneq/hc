using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class ActorModelData : MonoBehaviour, IGameEventListener
{
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

		internal bool IsExplosion => ExplosionRadius > 0f;

		internal float ExplosionRadius
		{
			get;
			set;
		}

		internal Vector3 ExplosionCenter => m_position;

		internal float ExplosionMagnitude
		{
			get;
			set;
		}

		internal Vector3 HitPosition => m_position;

		internal Vector3 HitImpulse
		{
			get;
			set;
		}

		internal ImpulseInfo(float explosionRadius, Vector3 explosionCenter)
		{
			ExplosionRadius = explosionRadius;
			m_position = explosionCenter;
			ExplosionMagnitude = TheatricsManager.GetRagdollImpactForce();
		}

		internal ImpulseInfo(Vector3 hitPosition, Vector3 hitDirection)
		{
			hitDirection.y = Mathf.Max(0.75f, hitDirection.y);
			float d = TheatricsManager.GetRagdollImpactForce();
			if (hitDirection.sqrMagnitude > 0f)
			{
				hitDirection.Normalize();
			}
			else
			{
				if (Application.isEditor)
				{
					Log.Warning("Ragdoll impulse has 0 vector as impulse direction");
				}
				hitDirection = Vector3.up;
				d = 0.1f;
			}
			m_position = hitPosition;
			HitImpulse = hitDirection * d;
		}

		public string GetDebugString()
		{
			string empty = string.Empty;
			string text;
			if (IsExplosion)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						text = empty;
						return text + "Explosion Type Impulse, Radius= " + ExplosionRadius + " | Magnitude = " + ExplosionMagnitude + " | Center= " + ExplosionCenter;
					}
				}
			}
			text = empty;
			return string.Concat(text, "Impulse FromPos= ", HitPosition, " | Magnitude= ", HitImpulse.magnitude, " | ImpulseDir= ", HitImpulse.normalized);
		}
	}

	public const int MIN_ANIMATION_INDEX = 1;

	public const int MAX_ANIMATION_INDEX = 20;

	public Sprite m_characterPortrait;

	internal ActorData m_parentActorData;

	public PersistentVFXInfo[] m_persistentVFX;

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
	public List<CullModeSettings> m_stealthShaderCullModeSettings;

	[HideInInspector]
	public float[] m_camStartEventDelays = new float[21];

	[HideInInspector]
	public float[] m_tauntCamStartEventDelays = new float[21];

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

	internal float Alpha
	{
		get;
		set;
	}

	internal Dictionary<int, string> GetAnimatorStateNameHashToNameMap()
	{
		return m_animatorStateNameHashToName;
	}

	private void Awake()
	{
		m_needsStandingIdleBoundingBox = true;
		m_modelAnimator = GetComponentInChildren<Animator>();
		AnimatorControllerParameter[] parameters = m_modelAnimator.parameters;
		for (int i = 0; i < parameters.Length; i++)
		{
			if (!m_hasRandomValueParameter)
			{
				if (parameters[i].name == "RandomValue")
				{
					m_hasRandomValueParameter = true;
				}
			}
			if (m_hasTurnStartParameter)
			{
				continue;
			}
			if (parameters[i].name == "TurnStart")
			{
				m_hasTurnStartParameter = true;
			}
		}
		while (true)
		{
			float[] savedTauntCamStartEventDelays;
			List<int> savedAnimatorStateNameHashes;
			List<string> savedAnimatorStateNames;
			if (m_savedAnimData != null)
			{
				float[] savedCamStartEventDelays = m_savedAnimData.m_savedCamStartEventDelays;
				savedTauntCamStartEventDelays = m_savedAnimData.m_savedTauntCamStartEventDelays;
				savedAnimatorStateNameHashes = m_savedAnimData.m_savedAnimatorStateNameHashes;
				savedAnimatorStateNames = m_savedAnimData.m_savedAnimatorStateNames;
				if (savedCamStartEventDelays != null)
				{
					if (savedCamStartEventDelays.Length == 21)
					{
						m_camStartEventDelays = new float[21];
						Array.Copy(savedCamStartEventDelays, m_camStartEventDelays, 21);
						goto IL_0153;
					}
				}
				Log.Error(base.name + " saved CamStartEventDelays is null or has mismatched number of entries");
				goto IL_0153;
			}
			goto IL_01c3;
			IL_01c3:
			Animator[] componentsInChildren = base.gameObject.GetComponentsInChildren<Animator>();
			Animator[] array = componentsInChildren;
			foreach (Animator animator in array)
			{
				animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
			}
			while (true)
			{
				m_animatorStateNameHashToName = new Dictionary<int, string>(m_animatorStateNameHashes.Count);
				for (int k = 0; k < m_animatorStateNameHashes.Count; k++)
				{
					m_animatorStateNameHashToName[m_animatorStateNameHashes[k]] = m_animatorStateNames[k];
				}
				while (true)
				{
					m_projector = base.gameObject.GetComponentInChildren<Projector>();
					PersistentVFXInfo[] persistentVFX = m_persistentVFX;
					foreach (PersistentVFXInfo persistentVFXInfo in persistentVFX)
					{
						if (persistentVFXInfo.m_vfxPrefab != null)
						{
							persistentVFXInfo.m_fxJoint.Initialize(base.gameObject);
							persistentVFXInfo.m_vfxInstance = UnityEngine.Object.Instantiate(persistentVFXInfo.m_vfxPrefab);
							persistentVFXInfo.m_vfxInstance.transform.parent = persistentVFXInfo.m_fxJoint.m_jointObject.transform;
							persistentVFXInfo.m_vfxInstance.transform.localPosition = Vector3.zero;
							persistentVFXInfo.m_vfxInstance.transform.localRotation = Quaternion.identity;
							persistentVFXInfo.m_vfxInstance.transform.localScale = Vector3.one;
						}
					}
					while (true)
					{
						m_renderers = base.gameObject.GetComponentsInChildren<Renderer>();
						if (s_stealthMaterialPlaneVectorIDs[0] == 0)
						{
							s_stealthMaterialPlaneVectorIDs[0] = Shader.PropertyToID("_StealthPlane0");
							s_stealthMaterialPlaneVectorIDs[1] = Shader.PropertyToID("_StealthPlane1");
							s_stealthMaterialPlaneVectorIDs[2] = Shader.PropertyToID("_StealthPlane2");
							s_stealthMaterialPlaneVectorIDs[3] = Shader.PropertyToID("_StealthPlane3");
							s_materialPropertyIDTeam = Shader.PropertyToID("_Team");
							s_materialPropertyIDStealthFade = Shader.PropertyToID("_StealthFade");
							s_materialPropertyIDBrush = Shader.PropertyToID("_StealthBrush");
							s_materialPropertyIDStealthMoving = Shader.PropertyToID("_StealthMoving");
							s_materialPropertyIDStealthBroken = Shader.PropertyToID("_StealthBroken");
							s_materialPropertyIDVisibleToClient = Shader.PropertyToID("_VisibleToClient");
						}
						s_materialPropertyIDCullMode = Shader.PropertyToID("_Cull");
						materialColorProperty = Shader.PropertyToID("_Color");
						materialOutlineProperty = Shader.PropertyToID("_Outline");
						materialOutlineColorProperty = Shader.PropertyToID("_OutlineColor");
						m_alphaUpdateMarkedDirty = true;
						return;
					}
				}
			}
			IL_0153:
			if (savedTauntCamStartEventDelays != null && savedTauntCamStartEventDelays.Length == 21)
			{
				m_tauntCamStartEventDelays = new float[21];
				Array.Copy(savedTauntCamStartEventDelays, m_tauntCamStartEventDelays, 21);
			}
			else
			{
				Log.Error(base.name + " saved Taunt CamStartEventDelays is null or has mismatched number of entries");
			}
			if (savedAnimatorStateNameHashes != null)
			{
				m_animatorStateNameHashes = new List<int>(savedAnimatorStateNameHashes);
			}
			if (savedAnimatorStateNames != null)
			{
				m_animatorStateNames = new List<string>(savedAnimatorStateNames);
			}
			goto IL_01c3;
		}
	}

	private void Start()
	{
		m_shroudInstances = GetComponentsInChildren<ShroudInstance>();
		if (m_shroudInstances.Length > 0)
		{
			m_dirtyRenderersCache = true;
		}
		else
		{
			CacheRenderers();
		}
		PKFxFX[] componentsInChildren = GetComponentsInChildren<PKFxFX>();
		if (componentsInChildren != null)
		{
			foreach (PKFxFX pKFxFX in componentsInChildren)
			{
				if (pKFxFX.m_PlayOnStart)
				{
					m_popcornFXPlayOnStartComponents.Add(pKFxFX);
				}
			}
		}
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TheatricsAbilitiesEnd);
		m_forceUpdateVisibility = true;
		if (GetComponentInChildren<AnimationEventReceiver>() == null)
		{
			string arg;
			if (m_parentActorData != null)
			{
				arg = m_parentActorData.name;
			}
			else
			{
				arg = string.Empty;
			}
			Log.Error($"{arg} does not have an Animation Event Receiver on its model.  Please update the prefab.");
		}
		SetMaterialFloatTeam();
		InitJointsAndRigidBodies();
		InitCachedJointForRagdoll();
	}

	private void SetMaterialFloatTeam()
	{
		float num;
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				if (m_parentActorData.GetOpposingTeam() == GameFlowData.Get().activeOwnedActorData.GetTeam())
				{
					num = 1f;
					goto IL_0076;
				}
			}
		}
		num = 0f;
		goto IL_0076;
		IL_0076:
		float value = num;
		SetMaterialFloat(s_materialPropertyIDTeam, value);
	}

	public void DelayEnablingOfShroudInstances()
	{
		ShroudInstance[] componentsInChildren = GetComponentsInChildren<ShroudInstance>();
		m_shroudInstancesToEnable = new bool[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			m_shroudInstancesToEnable[i] = componentsInChildren[i].enabled;
			componentsInChildren[i].enabled = false;
		}
		while (true)
		{
			return;
		}
	}

	public void ImpartWindImpulse(Vector3 direction)
	{
		for (int i = 0; i < m_shroudInstances.Length; i++)
		{
			m_shroudInstances[i].ImpartWindImpulse(direction);
		}
		while (true)
		{
			return;
		}
	}

	public void InitJointsAndRigidBodies()
	{
		Joint[] componentsInChildren = base.gameObject.GetComponentsInChildren<Joint>();
		Joint[] array = componentsInChildren;
		foreach (Joint joint in array)
		{
			joint.enablePreprocessing = m_enableRagdollPreprocessing;
		}
		while (true)
		{
			Rigidbody[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<Rigidbody>();
			Rigidbody[] array2 = componentsInChildren2;
			foreach (Rigidbody rigidbody in array2)
			{
				rigidbody.maxDepenetrationVelocity = m_maxDepenetrationVelocity;
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
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
				rigidbody = rigidbody2;
				num = rigidbody2.mass;
			}
		}
		if (!(rigidbody != null))
		{
			return;
		}
		while (true)
		{
			m_cachedRigidbodyForRagdollImpulse = rigidbody;
			return;
		}
	}

	private void OnDestroy()
	{
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TheatricsAbilitiesEnd);
		}
		if (m_appearanceNameToCachedRendererMaterials != null)
		{
			for (int i = 0; i < m_appearanceNameToCachedRendererMaterials.Count; i++)
			{
				List<Material[]> list = m_appearanceNameToCachedRendererMaterials.Values.ElementAt(i);
				for (int j = 0; j < list.Count; j++)
				{
					Material[] array = list[j];
					for (int k = 0; k < array.Length; k++)
					{
						UnityEngine.Object.Destroy(array[k]);
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							goto end_IL_0081;
						}
						continue;
						end_IL_0081:
						break;
					}
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						goto end_IL_0098;
					}
					continue;
					end_IL_0098:
					break;
				}
			}
			m_appearanceNameToCachedRendererMaterials.Clear();
		}
		m_parentActorData = null;
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.TheatricsAbilitiesEnd)
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
		SetCameraTransparency(1f, 1f, 0f);
	}

	internal void OnMovementAnimatorUpdate(BoardSquarePathInfo.ConnectionType movementType)
	{
		if (!(BrushCoordinator.Get() != null))
		{
			return;
		}
		if (!(GameFlowData.Get().activeOwnedActorData == null))
		{
			if (GameFlowData.Get().activeOwnedActorData.GetOpposingTeam() == m_parentActorData.GetTeam())
			{
				if (movementType == BoardSquarePathInfo.ConnectionType.Flight)
				{
					return;
				}
				if (movementType == BoardSquarePathInfo.ConnectionType.Teleport)
				{
					return;
				}
			}
		}
		bool flag = m_parentActorData.IsHiddenInBrush();
		BoardSquarePathInfo previousTravelBoardSquarePathInfo = m_parentActorData.GetActorMovement().GetPreviousTravelBoardSquarePathInfo();
		int num;
		if (previousTravelBoardSquarePathInfo != null)
		{
			num = ((previousTravelBoardSquarePathInfo.m_visibleToEnemies || previousTravelBoardSquarePathInfo.m_moverHasGameplayHitHere) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		if (num != 0 && flag && m_stealthBrushTransitionParameter.EaseFinished())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
				{
					m_stealthBrushTransitionParameter = new EasedOutFloatQuad(1f);
					Eased<float> stealthBrushTransitionParameter = m_stealthBrushTransitionParameter;
					float duration;
					if (m_parentActorData.GetActorMovement().IsPast2ndToLastSquare())
					{
						duration = m_stealthMoveStoppedHightlightFadeDuration;
					}
					else
					{
						duration = m_stealthMoveHightlightFadeDuration;
					}
					stealthBrushTransitionParameter.EaseTo(0f, duration);
					return;
				}
				}
			}
		}
		BoardSquarePathInfo nextTravelBoardSquarePathInfo = m_parentActorData.GetActorMovement().GetNextTravelBoardSquarePathInfo();
		object obj;
		if (nextTravelBoardSquarePathInfo == null)
		{
			obj = null;
		}
		else
		{
			obj = nextTravelBoardSquarePathInfo.square;
		}
		BoardSquare boardSquare = (BoardSquare)obj;
		int num2;
		if (boardSquare != null)
		{
			num2 = (BrushCoordinator.Get().IsRegionFunctioning(boardSquare.BrushRegion) ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool flag2 = (byte)num2 != 0;
		if (!flag || flag2)
		{
			return;
		}
		while (true)
		{
			if (m_stealthBrushTransitionParameter.GetEndValue() == 0f)
			{
				m_stealthBrushTransitionParameter = new EasedInFloatQuad(m_stealthBrushTransitionParameter);
				m_stealthBrushTransitionParameter.EaseTo(1f, m_stealthMoveHightlightFadeDuration);
			}
			return;
		}
	}

	internal void OnMovementAnimatorStop()
	{
		m_stealthBrushTransitionParameter.EaseTo(0f, m_stealthMoveStoppedHightlightFadeDuration);
	}

	public bool DiffForSyncCharacterPrefab(ActorModelData other, ref List<string> diffDescriptions)
	{
		bool result = true;
		if (m_characterPortrait != other.m_characterPortrait)
		{
			diffDescriptions.Add("\tCharacter Portrait different");
			result = false;
		}
		if (m_cameraVertOffset == other.m_cameraVertOffset)
		{
			if (m_cameraVertCoverOffset == other.m_cameraVertCoverOffset)
			{
				if (m_cameraHorzOffset == other.m_cameraHorzOffset)
				{
					if (m_stealthParameterStopPKFX == other.m_stealthParameterStopPKFX)
					{
						goto IL_00a4;
					}
				}
			}
		}
		diffDescriptions.Add("\tCamera Offsets different");
		result = false;
		goto IL_00a4;
		IL_00a4:
		if (m_persistentVFX.Length != other.m_persistentVFX.Length)
		{
			diffDescriptions.Add("\tPersistent VFX different");
			result = false;
		}
		else
		{
			int num = 0;
			while (true)
			{
				if (num < m_persistentVFX.Length)
				{
					if (!(m_persistentVFX[num].m_vfxPrefab.name != other.m_persistentVFX[num].m_vfxPrefab.name) && !(m_persistentVFX[num].m_fxJoint.m_joint != other.m_persistentVFX[num].m_fxJoint.m_joint))
					{
						if (!(m_persistentVFX[num].m_fxJoint.m_jointCharacter != other.m_persistentVFX[num].m_fxJoint.m_jointCharacter))
						{
							num++;
							continue;
						}
					}
					diffDescriptions.Add("\tPersistent VFX different");
					result = false;
				}
				else
				{
				}
				break;
			}
		}
		return result;
	}

	public bool IsVisibleToClient()
	{
		return m_visibleToClient;
	}

	public bool HasAnimatorControllerParamater(string paramName)
	{
		bool result = false;
		m_modelAnimator = GetComponentInChildren<Animator>();
		AnimatorControllerParameter[] parameters = m_modelAnimator.parameters;
		int num = 0;
		while (true)
		{
			if (num < parameters.Length)
			{
				if (parameters[num].name == paramName)
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

	public bool HasTurnStartParameter()
	{
		return m_hasTurnStartParameter;
	}

	private void CacheRenderers()
	{
		List<string> list = new List<string>();
		list.Add("Hydrogen/TGP/Toony");
		list.Add("Hydrogen/TGP/Toony_Metal");
		list.Add("Hydrogen/TGP/Toony_Alpha");
		list.Add("Hydrogen/DigitalSorceress");
		list.Add("Hydrogen/DigitalSorceressHair");
		list.Add("Hydrogen/Spark");
		list.Add("Hydrogen/Spark2");
		list.Add("Hydrogen/TGP/Environment_Metal_Alpha");
		list.Add("Hydrogen/Trickster");
		list.Add("Hydrogen/Trickster_Environment_Metal");
		list.Add("Hydrogen/Trickster_Environment_Metal_Alpha");
		List<string> list2 = list;
		m_dirtyRenderersCache = false;
		if (m_renderers == null)
		{
			m_renderers = base.gameObject.GetComponentsInChildren<Renderer>();
		}
		else
		{
			List<Renderer> list3 = new List<Renderer>();
			for (int i = 0; i < m_renderers.Length; i++)
			{
				list3.Add(m_renderers[i]);
			}
			Renderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<Renderer>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				if (!list3.Contains(componentsInChildren[j]))
				{
					list3.Add(componentsInChildren[j]);
				}
			}
			m_renderers = list3.ToArray();
		}
		m_rendererDefaultColors = new List<Color[]>(m_renderers.Length);
		List<Material[]> list4 = new List<Material[]>(m_renderers.Length);
		Renderer[] renderers = m_renderers;
		foreach (Renderer renderer in renderers)
		{
			if (renderer == null)
			{
				continue;
			}
			Color[] array = new Color[renderer.sharedMaterials.Length];
			for (int l = 0; l < renderer.sharedMaterials.Length; l++)
			{
				if (renderer.sharedMaterials[l] != null)
				{
					if (renderer.sharedMaterials[l].HasProperty(materialColorProperty))
					{
						array[l] = renderer.sharedMaterials[l].color;
					}
					else
					{
						array[l] = Color.white;
					}
					if (list2.Contains(renderer.sharedMaterials[l].shader.name))
					{
						continue;
					}
					object stealthReplacementShaderStr;
					if (base.gameObject.name.Contains("trickster"))
					{
						stealthReplacementShaderStr = "Hydrogen/Trickster";
					}
					else
					{
						stealthReplacementShaderStr = list2[0];
					}
					m_stealthReplacementShaderStr = (string)stealthReplacementShaderStr;
				}
				else
				{
					array[l] = Color.white;
				}
			}
			m_rendererDefaultColors.Add(array);
			Material[] array2 = new Material[renderer.sharedMaterials.Length];
			for (int m = 0; m < array2.Length; m++)
			{
				m_rendererDefaultMaterialsCacheKey = (array2[m] = new Material(renderer.sharedMaterials[m])).shader.name;
			}
			list4.Add(array2);
		}
		while (true)
		{
			m_appearanceNameToCachedRendererMaterials[m_rendererDefaultMaterialsCacheKey] = list4;
			if (!string.IsNullOrEmpty(m_stealthReplacementShaderStr))
			{
				m_stealthReplacementShader = Shader.Find(m_stealthReplacementShaderStr);
			}
			m_alphaUpdateMarkedDirty = true;
			return;
		}
	}

	internal void InitializeFaceActorModelData()
	{
		m_isFace = true;
		DestroyRagdoll();
		int layer = LayerMask.NameToLayer("Face");
		Transform[] componentsInChildren = base.gameObject.GetComponentsInChildren<Transform>(true);
		foreach (Transform transform in componentsInChildren)
		{
			transform.gameObject.layer = layer;
		}
		while (true)
		{
			SkinnedMeshRenderer[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren2)
			{
				skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
				skinnedMeshRenderer.receiveShadows = false;
				skinnedMeshRenderer.lightProbeUsage = LightProbeUsage.Off;
			}
			while (true)
			{
				base.gameObject.layer = layer;
				if (m_projector != null)
				{
					m_projector.enabled = false;
				}
				base.gameObject.SetActive(false);
				return;
			}
		}
	}

	public float GetCameraHorzOffset()
	{
		return m_cameraHorzOffset;
	}

	public float GetCameraVertOffset(bool forceStandingOffset)
	{
		float num = 0f;
		if (!forceStandingOffset)
		{
			if (m_parentActorData.GetActorCover().HasAnyCover())
			{
				return m_cameraVertCoverOffset;
			}
		}
		return m_cameraVertOffset;
	}

	public float GetModelSize()
	{
		if (m_renderers != null)
		{
			if (m_renderers.Length >= 1)
			{
				Bounds bounds = m_renderers[0].bounds;
				Renderer[] renderers = m_renderers;
				foreach (Renderer renderer in renderers)
				{
					if (renderer != null)
					{
						bounds.Encapsulate(renderer.bounds);
					}
				}
				while (true)
				{
					Vector3 size = bounds.size;
					return Mathf.Max(size.x, Mathf.Max(size.y, size.z));
				}
			}
		}
		return 2f;
	}

	private void DestroyRagdoll()
	{
		Joint[] componentsInChildren = base.gameObject.GetComponentsInChildren<Joint>();
		Joint[] array = componentsInChildren;
		foreach (Joint obj in array)
		{
			UnityEngine.Object.Destroy(obj);
		}
		while (true)
		{
			Collider[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<Collider>();
			Collider[] array2 = componentsInChildren2;
			foreach (Collider obj2 in array2)
			{
				UnityEngine.Object.Destroy(obj2);
			}
			while (true)
			{
				Rigidbody[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<Rigidbody>();
				Rigidbody[] array3 = componentsInChildren3;
				foreach (Rigidbody obj3 in array3)
				{
					UnityEngine.Object.Destroy(obj3);
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
		}
	}

	internal string GetCurrentAnimatorStateName()
	{
		string result = "[UNKNOWN: Please save .controller]";
		Animator modelAnimator = GetModelAnimator();
		if (modelAnimator != null)
		{
			if (modelAnimator.layerCount >= 1)
			{
				AnimatorStateInfo currentAnimatorStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
				if (m_animatorStateNameHashToName.TryGetValue(currentAnimatorStateInfo.shortNameHash, out string value))
				{
					result = value;
				}
			}
		}
		return result;
	}

	internal int GetCurrentAnimatorStateHash()
	{
		Animator modelAnimator = GetModelAnimator();
		if (modelAnimator != null)
		{
			if (modelAnimator.layerCount >= 1)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return modelAnimator.GetCurrentAnimatorStateInfo(0).shortNameHash;
					}
				}
			}
		}
		return 0;
	}

	public string GetAnimatorHashToString(int hash)
	{
		if (m_animatorStateNameHashToName.TryGetValue(hash, out string value))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return value;
				}
			}
		}
		return "UNKNOWN";
	}

	internal bool IsPlayingAttackAnim()
	{
		bool endingAttack;
		return IsPlayingAttackAnim(out endingAttack);
	}

	public bool IsPlayingChargeEnd()
	{
		bool result = false;
		Animator modelAnimator = GetModelAnimator();
		if (modelAnimator != null)
		{
			if (modelAnimator.layerCount >= 1)
			{
				result = (modelAnimator.GetCurrentAnimatorStateInfo(0).tagHash == s_animHashChargeEnd);
			}
		}
		return result;
	}

	internal bool IsPlayingAttackAnim(out bool endingAttack)
	{
		endingAttack = false;
		Animator modelAnimator = GetModelAnimator();
		if (!(modelAnimator == null))
		{
			if (modelAnimator.layerCount >= 1)
			{
				AnimatorStateInfo currentAnimatorStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
				bool flag = modelAnimator.IsInTransition(0);
				if (currentAnimatorStateInfo.tagHash != s_animHashAttack)
				{
					if (currentAnimatorStateInfo.tagHash != s_animHashChargeEnd)
					{
						return false;
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
		Animator modelAnimator = GetModelAnimator();
		if (modelAnimator == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (modelAnimator.layerCount < 1)
		{
			return false;
		}
		AnimatorStateInfo currentAnimatorStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
		int result;
		if (currentAnimatorStateInfo.tagHash == s_animHashIdle || currentAnimatorStateInfo.shortNameHash == s_animHashIdle)
		{
			if (excludeCover)
			{
				result = ((!m_parentActorData.GetActorCover().HasAnyCover()) ? 1 : 0);
			}
			else
			{
				result = 1;
			}
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal bool IsPlayingDamageAnim()
	{
		Animator modelAnimator = GetModelAnimator();
		if (!(modelAnimator == null))
		{
			if (modelAnimator.layerCount >= 1)
			{
				AnimatorStateInfo currentAnimatorStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
				int result;
				if (currentAnimatorStateInfo.tagHash != s_animHashDamage && currentAnimatorStateInfo.shortNameHash != s_animHashDamage)
				{
					result = ((currentAnimatorStateInfo.tagHash == s_animHashDamageNoInterrupt) ? 1 : 0);
				}
				else
				{
					result = 1;
				}
				return (byte)result != 0;
			}
		}
		return false;
	}

	internal bool CanPlayDamageReactAnim()
	{
		Animator modelAnimator = GetModelAnimator();
		int result;
		if (!(modelAnimator == null))
		{
			if (modelAnimator.layerCount >= 1)
			{
				AnimatorStateInfo currentAnimatorStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
				int num;
				if (currentAnimatorStateInfo.tagHash != s_animHashIdle)
				{
					num = ((currentAnimatorStateInfo.shortNameHash == s_animHashIdle) ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				bool flag = (byte)num != 0;
				int num2;
				if (currentAnimatorStateInfo.tagHash != s_animHashDamage)
				{
					num2 = ((currentAnimatorStateInfo.shortNameHash == s_animHashDamage) ? 1 : 0);
				}
				else
				{
					num2 = 1;
				}
				bool flag2 = (byte)num2 != 0;
				if (!flag)
				{
					if (!flag2)
					{
						result = 0;
						goto IL_00c5;
					}
				}
				if (!flag2)
				{
					result = ((!modelAnimator.IsInTransition(0)) ? 1 : 0);
				}
				else
				{
					result = 1;
				}
				goto IL_00c5;
			}
		}
		return false;
		IL_00c5:
		return (byte)result != 0;
	}

	internal bool IsPlayingKnockdownAnim()
	{
		Animator modelAnimator = GetModelAnimator();
		if (!(modelAnimator == null))
		{
			if (modelAnimator.layerCount >= 1)
			{
				return modelAnimator.GetCurrentAnimatorStateInfo(0).tagHash == s_animHashKnockdown;
			}
		}
		return false;
	}

	private void StoreRigidBodyOffsets()
	{
		Rigidbody[] componentsInChildren = base.gameObject.GetComponentsInChildren<Rigidbody>();
		Rigidbody[] array = componentsInChildren;
		foreach (Rigidbody rigidbody in array)
		{
			m_initialBoneOffsetMap[rigidbody.gameObject] = rigidbody.transform.localPosition;
			m_initialBoneRotationOffsetMap[rigidbody.gameObject] = rigidbody.transform.localRotation;
			m_initialBoneScaleOffsetMap[rigidbody.gameObject] = rigidbody.transform.localScale;
		}
		while (true)
		{
			return;
		}
	}

	private void ResetRigidBodyOffsets()
	{
		Rigidbody[] componentsInChildren = base.gameObject.GetComponentsInChildren<Rigidbody>();
		Rigidbody[] array = componentsInChildren;
		foreach (Rigidbody rigidbody in array)
		{
			if (m_initialBoneOffsetMap.ContainsKey(rigidbody.gameObject))
			{
				rigidbody.transform.localPosition = m_initialBoneOffsetMap[rigidbody.gameObject];
				rigidbody.transform.localRotation = m_initialBoneRotationOffsetMap[rigidbody.gameObject];
				rigidbody.transform.localScale = m_initialBoneScaleOffsetMap[rigidbody.gameObject];
			}
		}
		while (true)
		{
			return;
		}
	}

	internal Renderer GetModelRenderer(int index = 0)
	{
		object result;
		if (m_renderers != null)
		{
			if (m_renderers.Length > index)
			{
				result = m_renderers[index];
				goto IL_0031;
			}
		}
		result = null;
		goto IL_0031;
		IL_0031:
		return (Renderer)result;
	}

	internal int GetNumModelRenderers()
	{
		int result;
		if (m_renderers == null)
		{
			result = 0;
		}
		else
		{
			result = m_renderers.Length;
		}
		return result;
	}

	public Animator GetModelAnimator()
	{
		if (m_modelAnimator == null)
		{
			if (NetworkClient.active)
			{
				Log.Error(string.Concat(this, " has a NULL model Animator"));
			}
		}
		else if (m_modelAnimator.layerCount == 0)
		{
			m_modelAnimator = GetComponentInChildren<Animator>();
			object str;
			if (m_parentActorData == null)
			{
				str = "NULL";
			}
			else
			{
				str = m_parentActorData.ToString();
			}
			object str2;
			if (m_modelAnimator == null)
			{
				str2 = "NULL";
			}
			else
			{
				str2 = m_modelAnimator.layerCount.ToString();
			}
			Log.Error((string)str + " model Animator had zero layers! Refreshing... layers: " + (string)str2);
		}
		return m_modelAnimator;
	}

	public void Setup(ActorData parentActorData)
	{
		Vector3 localScale = base.transform.localScale;
		m_parentActorData = parentActorData;
		base.transform.parent = parentActorData.transform;
		base.transform.localPosition = Vector3.zero;
		base.transform.localScale = localScale;
		GameObject gameObject = base.gameObject.FindInChildren("root_JNT");
		if (gameObject != null)
		{
			m_rootBoneTransform = gameObject.transform;
		}
		gameObject = base.gameObject.FindInChildren("hip_JNT");
		if (gameObject != null)
		{
			m_hipBoneTransform = gameObject.transform;
		}
		gameObject = base.gameObject.FindInChildren("floor_JNT");
		if (gameObject != null)
		{
			m_floorBoneTransform = gameObject.transform;
		}
		StoreRigidBodyOffsets();
	}

	private void UpdateFloorBone()
	{
		if (!(m_rootBoneTransform != null))
		{
			return;
		}
		while (true)
		{
			if (m_hipBoneTransform != null && m_floorBoneTransform != null)
			{
				while (true)
				{
					Vector3 position = m_floorBoneTransform.position;
					Vector3 position2 = m_rootBoneTransform.transform.position;
					float y = position2.y;
					Vector3 position3 = m_hipBoneTransform.transform.position;
					position.y = Mathf.Min(y, position3.y);
					m_floorBoneTransform.position = position;
					return;
				}
			}
			return;
		}
	}

	private void UpdateRandomValueForAnimator()
	{
		float num = 0.25f;
		if (!(m_modelAnimator != null) || !(m_lastAnimatorRandomSetTime + num < Time.time))
		{
			return;
		}
		while (true)
		{
			if (m_hasRandomValueParameter)
			{
				while (true)
				{
					m_lastAnimatorRandomSetTime = Time.time;
					m_modelAnimator.SetFloat("RandomValue", UnityEngine.Random.value);
					return;
				}
			}
			return;
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
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (CameraManager.Get().ShotSequence.Actor != m_parentActorData)
		{
			return false;
		}
		return true;
	}

	public void ForceUpdateVisibility()
	{
		UpdateVisibility();
		UpdateSelectionOutline();
	}

	private void Update()
	{
		bool flag = false;
		if (m_shroudInstancesToEnable != null)
		{
			for (int i = 0; i < m_shroudInstancesToEnable.Length; i++)
			{
				m_shroudInstances[i].enabled = m_shroudInstancesToEnable[i];
				flag = true;
			}
			m_shroudInstancesToEnable = null;
		}
		if (m_dirtyRenderersCache && !flag)
		{
			CacheRenderers();
		}
		if (m_parentActorData == null)
		{
			Log.Error("Setup was not called for ActorModelData on " + base.gameObject.name);
			return;
		}
		if (!m_attemptedToCreateBaseCircle)
		{
			ActorVFX actorVFX = m_parentActorData.GetActorVFX();
			if (actorVFX != null)
			{
				actorVFX.SpawnBaseCircles();
			}
			m_attemptedToCreateBaseCircle = true;
		}
		if (!m_parentActorData.IsModelAnimatorDisabled())
		{
			UpdateFloorBone();
		}
		UpdateRandomValueForAnimator();
		UpdateStealth();
		UpdateVisibility();
		UpdateSelectionOutline();
		if (!m_isFace)
		{
			UpdateCameraTransparency();
		}
		bool flag2 = IsPlayingIdleAnim(true);
		if (m_needsStandingIdleBoundingBox)
		{
			if (flag2)
			{
				goto IL_0176;
			}
		}
		if (m_rendererBoundsApprox.size == Vector3.zero)
		{
			goto IL_0176;
		}
		goto IL_0280;
		IL_0280:
		if (m_activeRigidbodies == null)
		{
			return;
		}
		while (true)
		{
			if (!(Camera.main != null))
			{
				return;
			}
			while (true)
			{
				Vector3 position = Camera.main.transform.position;
				float num = Camera.main.farClipPlane * Camera.main.farClipPlane;
				for (int j = 0; j < m_activeRigidbodies.Length; j++)
				{
					Rigidbody rigidbody = m_activeRigidbodies[j];
					if (!(rigidbody != null))
					{
						continue;
					}
					if (rigidbody.IsSleeping())
					{
						continue;
					}
					Vector3 position2 = rigidbody.position;
					if (float.IsNaN(position2.x))
					{
						rigidbody.isKinematic = true;
						rigidbody.detectCollisions = false;
						rigidbody.transform.localPosition = new Vector3(-10000f, 0f, 0f);
						rigidbody.Sleep();
						continue;
					}
					float num2 = Vector3.SqrMagnitude(rigidbody.position - position);
					if (num2 > num)
					{
						rigidbody.isKinematic = true;
						rigidbody.detectCollisions = false;
						rigidbody.Sleep();
					}
				}
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		IL_0176:
		if (GetNumModelRenderers() > 0)
		{
			m_needsStandingIdleBoundingBox = !flag2;
			float num3 = 0.95f * Board.Get().squareSize;
			float num4 = 4f;
			m_rendererBoundsApprox.size = new Vector3(num3, num4, num3);
			m_rendererBoundsApprox.center = new Vector3(0f, 0.5f * num4, 0f);
		}
		if (!m_isFace)
		{
			m_rendererBoundsApproxCollider = base.gameObject.AddComponent<BoxCollider>();
			m_rendererBoundsApproxCollider.center = m_rendererBoundsApprox.center;
			m_rendererBoundsApproxCollider.size = m_rendererBoundsApprox.size;
			Collider[] componentsInChildren = base.gameObject.GetComponentsInChildren<Collider>();
			Collider[] array = componentsInChildren;
			foreach (Collider collider in array)
			{
				collider.gameObject.layer = ActorData.Layer;
			}
		}
		goto IL_0280;
	}

	internal void SetMaterialShader(Shader shader, bool overrideDefault = false)
	{
		string name = shader.name;
		if (overrideDefault)
		{
			m_rendererDefaultMaterialsCacheKey = name;
		}
		if (m_appearanceNameToCachedRendererMaterials.ContainsKey(name))
		{
			List<Material[]> list = m_appearanceNameToCachedRendererMaterials[name];
			for (int i = 0; i < m_renderers.Length; i++)
			{
				Material[] array = list[i];
				Renderer renderer = m_renderers[i];
				for (int j = 0; j < renderer.materials.Length; j++)
				{
					array[j].shaderKeywords = renderer.materials[j].shaderKeywords;
					UnityEngine.Object.Destroy(renderer.materials[j]);
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						goto end_IL_00a5;
					}
					continue;
					end_IL_00a5:
					break;
				}
				renderer.materials = array;
			}
		}
		else
		{
			List<Material[]> list = new List<Material[]>(m_renderers.Length);
			int num = 0;
			while (num < m_renderers.Length)
			{
				Renderer renderer2 = m_renderers[num];
				Material[] array2 = new Material[renderer2.sharedMaterials.Length];
				for (int k = 0; k < array2.Length; k++)
				{
					Material material = new Material(renderer2.sharedMaterials[k]);
					material.shader = shader;
					material.shaderKeywords = renderer2.sharedMaterials[k].shaderKeywords;
					array2[k] = material;
				}
				while (true)
				{
					list.Add(array2);
					for (int l = 0; l < renderer2.materials.Length; l++)
					{
						UnityEngine.Object.Destroy(renderer2.materials[l]);
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							goto end_IL_018e;
						}
						continue;
						end_IL_018e:
						break;
					}
					renderer2.materials = array2;
					num++;
					goto IL_01a7;
				}
				IL_01a7:;
			}
			m_appearanceNameToCachedRendererMaterials[name] = list;
		}
		m_alphaUpdateMarkedDirty = true;
	}

	public void ResetMaterialsToDefaults()
	{
		if (m_appearanceNameToCachedRendererMaterials.ContainsKey(m_rendererDefaultMaterialsCacheKey))
		{
			List<Material[]> list = m_appearanceNameToCachedRendererMaterials[m_rendererDefaultMaterialsCacheKey];
			for (int i = 0; i < m_renderers.Length; i++)
			{
				Renderer renderer = m_renderers[i];
				if (renderer != null)
				{
					Material[] materials = list[i];
					for (int j = 0; j < renderer.materials.Length; j++)
					{
						UnityEngine.Object.Destroy(renderer.materials[j]);
					}
					renderer.materials = materials;
				}
			}
			DisableMaterialKeyword(c_stealthShaderKeyword);
			SetMaterialFloatTeam();
			if (m_stealthShaderCullModeSettings != null)
			{
				for (int k = 0; k < m_stealthShaderCullModeSettings.Count; k++)
				{
					CullModeSettings cullModeSettings = m_stealthShaderCullModeSettings[k];
					if (!cullModeSettings.m_forStealth)
					{
						SetMaterialFloatByNameMatch(s_materialPropertyIDCullMode, (float)cullModeSettings.m_desiredCullMode, cullModeSettings.m_targetMaterialSearchPtn);
					}
				}
			}
		}
		m_alphaUpdateMarkedDirty = true;
	}

	private void SetLayer(int newLayer, int oldLayer)
	{
		for (int i = 0; i < m_renderers.Length; i++)
		{
			Renderer renderer = m_renderers[i];
			if (renderer.gameObject.layer == oldLayer)
			{
				renderer.gameObject.layer = newLayer;
			}
		}
	}

	private void SetMaterialFloat(int propertyID, float value)
	{
		int num = 0;
		while (num < m_renderers.Length)
		{
			Renderer renderer = m_renderers[num];
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				material.SetFloat(propertyID, value);
			}
			while (true)
			{
				num++;
				goto IL_0047;
			}
			IL_0047:;
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void SetMaterialFloatByNameMatch(int propertyID, float value, string searchPtn)
	{
		for (int i = 0; i < m_renderers.Length; i++)
		{
			Renderer renderer = m_renderers[i];
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				if (!string.IsNullOrEmpty(searchPtn))
				{
					if (material.name.IndexOf(searchPtn) < 0)
					{
						continue;
					}
				}
				material.SetFloat(propertyID, value);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					goto end_IL_0061;
				}
				continue;
				end_IL_0061:
				break;
			}
		}
	}

	private void SetMinMaterialFloat(int propertyId, float value)
	{
		for (int i = 0; i < m_renderers.Length; i++)
		{
			Renderer renderer = m_renderers[i];
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				material.SetFloat(propertyId, Mathf.Max(value, material.GetFloat(propertyId)));
			}
		}
	}

	private void SetMaterialVector(int id, Vector4 value)
	{
		int num = 0;
		while (num < m_renderers.Length)
		{
			Renderer renderer = m_renderers[num];
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				material.SetVector(id, value);
			}
			while (true)
			{
				num++;
				goto IL_0049;
			}
			IL_0049:;
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	internal void EnableMaterialKeyword(string keyword)
	{
		int num = 0;
		while (num < m_renderers.Length)
		{
			Renderer renderer = m_renderers[num];
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				material.EnableKeyword(keyword);
			}
			while (true)
			{
				num++;
				goto IL_0046;
			}
			IL_0046:;
		}
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

	internal void DisableMaterialKeyword(string keyword)
	{
		int num = 0;
		while (num < m_renderers.Length)
		{
			Renderer renderer = m_renderers[num];
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				material.DisableKeyword(keyword);
			}
			while (true)
			{
				num++;
				goto IL_0046;
			}
			IL_0046:;
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

	internal void SetMaterialKeywordOnAllCachedMaterials(string keyword, bool enable)
	{
		foreach (KeyValuePair<string, List<Material[]>> appearanceNameToCachedRendererMaterial in m_appearanceNameToCachedRendererMaterials)
		{
			List<Material[]> value = appearanceNameToCachedRendererMaterial.Value;
			for (int i = 0; i < value.Count; i++)
			{
				Material[] array = value[i];
				foreach (Material material in array)
				{
					if (material != null)
					{
						if (enable)
						{
							material.EnableKeyword(keyword);
						}
						else
						{
							material.DisableKeyword(keyword);
						}
					}
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						goto end_IL_0082;
					}
					continue;
					end_IL_0082:
					break;
				}
			}
		}
	}

	private void SetMaterialRenderQueue(int renderQueue)
	{
		for (int i = 0; i < m_renderers.Length; i++)
		{
			Renderer renderer = m_renderers[i];
			Material[] materials = renderer.materials;
			for (int j = 0; j < materials.Length; j++)
			{
				Material material = materials[j];
				int renderQueue2;
				if (renderQueue < -1)
				{
					renderQueue2 = renderer.sharedMaterials[j].renderQueue;
				}
				else
				{
					renderQueue2 = renderQueue;
				}
				material.renderQueue = renderQueue2;
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

	private void ScaleMaterialColorToSDR(int propertyId)
	{
		for (int i = 0; i < m_renderers.Length; i++)
		{
			Renderer renderer = m_renderers[i];
			Material[] materials = renderer.materials;
			foreach (Material material in materials)
			{
				Color color = material.GetColor(propertyId);
				float maxColorComponent = color.maxColorComponent;
				if (maxColorComponent > 1f)
				{
					material.SetColor(propertyId, color * 1f / maxColorComponent);
				}
			}
		}
	}

	private void KillPopcornFXPlayOnStart()
	{
		m_stealthPKFXStopped = true;
		for (int i = 0; i < m_popcornFXPlayOnStartComponents.Count; i++)
		{
			PKFxFX pKFxFX = m_popcornFXPlayOnStartComponents.ElementAt(i);
			pKFxFX.KillEffect();
		}
	}

	public void RestartPopcornFXPlayOnStart()
	{
		if (!m_visibleToClient)
		{
			return;
		}
		while (true)
		{
			m_stealthPKFXStopped = false;
			for (int i = 0; i < m_popcornFXPlayOnStartComponents.Count; i++)
			{
				PKFxFX pKFxFX = m_popcornFXPlayOnStartComponents.ElementAt(i);
				pKFxFX.StartEffect();
			}
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
	}

	internal void EnableRendererAndUpdateVisibility()
	{
		Renderer[] renderers = m_renderers;
		foreach (Renderer renderer in renderers)
		{
			if (renderer != null)
			{
				renderer.enabled = true;
			}
		}
		if (m_shroudInstances != null)
		{
			ShroudInstance[] shroudInstances = m_shroudInstances;
			foreach (ShroudInstance shroudInstance in shroudInstances)
			{
				shroudInstance.enabled = true;
			}
		}
		m_visibleToClient = true;
		m_forceHideRenderers = false;
		UpdateVisibility();
		if (!m_visibleToClient)
		{
			return;
		}
		while (true)
		{
			SetPersistentVfxActive(true);
			return;
		}
	}

	internal void DisableAndHideRenderers()
	{
		Renderer[] renderers = m_renderers;
		foreach (Renderer renderer in renderers)
		{
			if (renderer != null)
			{
				renderer.enabled = false;
			}
		}
		while (true)
		{
			if (m_shroudInstances != null)
			{
				ShroudInstance[] shroudInstances = m_shroudInstances;
				foreach (ShroudInstance shroudInstance in shroudInstances)
				{
					shroudInstance.enabled = false;
				}
			}
			m_forceHideRenderers = true;
			return;
		}
	}

	internal void SetDefaultRendererAlpha(float alpha)
	{
		if (m_rendererDefaultColors == null)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_rendererDefaultColors.Count; i++)
			{
				for (int j = 0; j < m_rendererDefaultColors[i].Length; j++)
				{
					m_rendererDefaultColors[i][j].a = alpha;
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						goto end_IL_0057;
					}
					continue;
					end_IL_0057:
					break;
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

	internal void CacheDefaultRendererAlphas()
	{
		if (m_rendererDefaultColors == null)
		{
			return;
		}
		m_cachedRendererAlphas = new List<float>();
		int num = 0;
		while (num < m_rendererDefaultColors.Count)
		{
			for (int i = 0; i < m_rendererDefaultColors[num].Length; i++)
			{
				m_cachedRendererAlphas.Add(m_rendererDefaultColors[num][i].a);
			}
			while (true)
			{
				num++;
				goto IL_006e;
			}
			IL_006e:;
		}
	}

	internal void RestoreDefaultRendererAlphas()
	{
		if (m_rendererDefaultColors != null && m_cachedRendererAlphas != null)
		{
			int num = 0;
			for (int i = 0; i < m_rendererDefaultColors.Count; i++)
			{
				for (int j = 0; j < m_rendererDefaultColors[i].Length; j++)
				{
					m_rendererDefaultColors[i][j].a = m_cachedRendererAlphas[num++];
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						goto end_IL_0075;
					}
					continue;
					end_IL_0075:
					break;
				}
			}
		}
		m_cachedRendererAlphas = null;
	}

	internal void SetCameraTransparency(float startTransparency, float transparencyTime, float fadeStartDelayDuration)
	{
		m_cameraTransparency = startTransparency;
		m_cameraTransparencyStartValue = startTransparency;
		m_cameraTransparencyTime = transparencyTime;
		m_cameraTransparencyLastChangeSetTime = Time.time + fadeStartDelayDuration;
	}

	internal void SetMasterSkinVfxInst(GameObject vfxInst)
	{
		if (m_masterSkinVfxInst != null)
		{
			Log.Warning("Setting master skin vfx instance when there is existing entry");
		}
		m_masterSkinVfxInst = vfxInst;
	}

	private void UpdateVisibility()
	{
		bool flag = false;
		if (m_isFace)
		{
			int num;
			if (CameraManager.Get() != null)
			{
				num = (CameraManager.Get().InFaceShot(m_parentActorData) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			flag = ((byte)num != 0);
		}
		else
		{
			flag = (!m_forceHideRenderers && m_parentActorData.IsVisibleToClient() && m_cameraTransparency > 0f);
		}
		if (m_visibleToClient == flag)
		{
			if (!m_forceUpdateVisibility)
			{
				return;
			}
		}
		m_forceUpdateVisibility = false;
		m_visibleToClient = flag;
		if (m_shroudInstances != null)
		{
			ShroudInstance[] shroudInstances = m_shroudInstances;
			foreach (ShroudInstance shroudInstance in shroudInstances)
			{
				if (shroudInstance != null)
				{
					shroudInstance.enabled = flag;
				}
			}
		}
		if (m_renderers != null)
		{
			Renderer[] renderers = m_renderers;
			foreach (Renderer renderer in renderers)
			{
				if (renderer != null)
				{
					renderer.enabled = flag;
				}
			}
		}
		if (m_projector != null)
		{
			m_projector.enabled = flag;
		}
		ParticleSystem[] componentsInChildren = GetComponentsInChildren<ParticleSystem>();
		ParticleSystem[] array = componentsInChildren;
		foreach (ParticleSystem particleSystem in array)
		{
			if (!flag)
			{
				particleSystem.Clear();
			}
			ParticleSystem.EmissionModule emission = particleSystem.emission;
			emission.enabled = flag;
		}
		while (true)
		{
			SetPersistentVfxActive(flag);
			return;
		}
	}

	private void SetPersistentVfxActive(bool active)
	{
		PersistentVFXInfo[] persistentVFX = m_persistentVFX;
		foreach (PersistentVFXInfo persistentVFXInfo in persistentVFX)
		{
			if (persistentVFXInfo == null)
			{
				continue;
			}
			if (persistentVFXInfo.m_vfxInstance != null)
			{
				if (persistentVFXInfo.m_vfxInstance.activeSelf != active)
				{
					persistentVFXInfo.m_vfxInstance.SetActive(active);
				}
			}
		}
		if (!(m_masterSkinVfxInst != null))
		{
			return;
		}
		while (true)
		{
			m_masterSkinVfxInst.gameObject.SetActiveIfNeeded(active);
			return;
		}
	}

	private void UpdateStealth()
	{
		if (m_isFace)
		{
			return;
		}
		int num;
		if (!m_parentActorData.GetActorStatus().HasStatus(StatusType.Revealed, false))
		{
			if (!CaptureTheFlag.IsActorRevealedByFlag_Client(m_parentActorData))
			{
				num = (m_parentActorData.VisibleTillEndOfPhase ? 1 : 0);
				goto IL_005d;
			}
		}
		num = 1;
		goto IL_005d;
		IL_005d:
		bool flag = (byte)num != 0;
		bool flag2 = m_parentActorData.GetActorStatus().IsInvisibleToEnemies(false);
		List<Plane> list = null;
		if (!((float)m_stealthBrushTransitionParameter > 0f))
		{
			if (m_stealthBrushTransitionParameter.GetEndValue() != 1f)
			{
				goto IL_014a;
			}
		}
		if (BrushCoordinator.Get() != null)
		{
			BoardSquare travelBoardSquare = m_parentActorData.GetTravelBoardSquare();
			Vector3 vector;
			if ((bool)travelBoardSquare)
			{
				vector = travelBoardSquare.ToVector3();
			}
			else
			{
				vector = base.transform.position;
			}
			Vector3 center = vector;
			Bounds bounds = new Bounds(center, new Vector3(0.1f, 0.1f, 0.1f));
			bounds.Encapsulate(m_parentActorData.PreviousBoardSquarePosition);
			list = BrushCoordinator.Get().CalcIntersectingBrushSidePlanes(bounds);
		}
		goto IL_014a;
		IL_05ac:
		bool flag3;
		if (m_stealthPKFXStopped && !flag3)
		{
			if (m_stealthBrushTransitionParameter.GetEndValue() != 1f)
			{
				if (m_stealthBrokenParameter.GetEndValue() != 1f)
				{
					goto IL_0734;
				}
			}
			if (1f - Mathf.Max(m_stealthBrushTransitionParameter, m_stealthBrokenParameter) <= m_stealthParameterStartPKFX)
			{
				RestartPopcornFXPlayOnStart();
			}
		}
		goto IL_0734;
		IL_0734:
		if (!m_stealthShaderEnabled)
		{
			return;
		}
		int num2;
		while (true)
		{
			SetMaterialFloat(s_materialPropertyIDStealthFade, m_stealthFadeParameter);
			SetMaterialFloat(s_materialPropertyIDBrush, m_stealthBrushParameter);
			SetMaterialFloat(s_materialPropertyIDStealthMoving, m_stealthBrushTransitionParameter);
			SetMaterialFloat(s_materialPropertyIDStealthBroken, m_stealthBrokenParameter);
			int propertyID = s_materialPropertyIDVisibleToClient;
			float value;
			if (m_parentActorData.IsVisibleToClient())
			{
				value = 1f;
			}
			else
			{
				value = 0f;
			}
			SetMaterialFloat(propertyID, value);
			for (int i = 0; i < s_stealthMaterialPlaneVectorIDs.Length; i++)
			{
				if (num2 > i)
				{
					Plane plane = list[i];
					int id = s_stealthMaterialPlaneVectorIDs[i];
					Vector3 normal = plane.normal;
					float x = normal.x;
					Vector3 normal2 = plane.normal;
					float y = normal2.y;
					Vector3 normal3 = plane.normal;
					SetMaterialVector(id, new Vector4(x, y, normal3.z, plane.distance - (1f - m_parentActorData.GetModelAnimator().speed) * m_stealthStoppedPlaneOffset));
				}
				else
				{
					SetMaterialVector(s_stealthMaterialPlaneVectorIDs[i], Vector4.zero);
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
		IL_03dd:
		int num3;
		if (m_stealthBrushParameter.GetEndValue() == 1f)
		{
			num3 = (((float)m_stealthBrokenParameter <= 0.99f) ? 1 : 0);
		}
		else
		{
			num3 = 0;
		}
		bool flag4 = (byte)num3 != 0;
		flag3 = (m_stealthFadeParameter.GetEndValue() == 1f);
		if (!flag3)
		{
			if (!flag4)
			{
				if (m_stealthShaderEnabled && m_stealthFadeParameter.GetEndValue() == 0f && m_stealthFadeParameter.EaseFinished())
				{
					if (m_stealthBrushParameter.GetEndValue() == 0f)
					{
						if (m_stealthBrushParameter.EaseFinished())
						{
							goto IL_06e4;
						}
					}
					if (m_stealthBrokenParameter.GetEndValue() == 1f)
					{
						if (m_stealthBrushParameter.EaseFinished())
						{
							goto IL_06e4;
						}
					}
				}
				goto IL_06f1;
			}
		}
		if (!m_stealthShaderEnabled)
		{
			if (m_stealthReplacementShader != null)
			{
				SetMaterialShader(m_stealthReplacementShader);
				ScaleMaterialColorToSDR(materialOutlineColorProperty);
				SetMinMaterialFloat(materialOutlineProperty, 1E-05f);
			}
			EnableMaterialKeyword(c_stealthShaderKeyword);
			SetMaterialRenderQueue(3000);
			SetMaterialFloatTeam();
			if (m_stealthShaderCullModeSettings != null)
			{
				for (int j = 0; j < m_stealthShaderCullModeSettings.Count; j++)
				{
					CullModeSettings cullModeSettings = m_stealthShaderCullModeSettings[j];
					if (cullModeSettings.m_forStealth)
					{
						SetMaterialFloatByNameMatch(s_materialPropertyIDCullMode, (float)cullModeSettings.m_desiredCullMode, cullModeSettings.m_targetMaterialSearchPtn);
					}
				}
			}
			m_stealthShaderEnabled = true;
		}
		if (Mathf.Max(m_stealthFadeParameter, 1f - (float)m_stealthBrushTransitionParameter) >= m_stealthParameterStopPKFX)
		{
			if (!flag3)
			{
				if (!m_parentActorData.IsHiddenInBrush())
				{
					goto IL_05ac;
				}
			}
			if (!m_stealthPKFXStopped)
			{
				KillPopcornFXPlayOnStart();
				goto IL_0734;
			}
		}
		goto IL_05ac;
		IL_06f1:
		if (Mathf.Min(m_stealthFadeParameter, 1f - (float)m_stealthBrushTransitionParameter) <= m_stealthParameterStartPKFX && m_stealthPKFXStopped)
		{
			RestartPopcornFXPlayOnStart();
		}
		goto IL_0734;
		IL_014a:
		int num4;
		if (list != null)
		{
			num4 = list.Count;
		}
		else
		{
			num4 = 0;
		}
		num2 = num4;
		int travelBoardSquareBrushRegion = m_parentActorData.GetTravelBoardSquareBrushRegion();
		bool flag5 = travelBoardSquareBrushRegion >= 0;
		bool flag6 = BrushCoordinator.Get() != null && m_parentActorData.IsHiddenInBrush();
		int num5;
		if (flag5 && BrushCoordinator.Get() != null)
		{
			num5 = ((!BrushCoordinator.Get().IsRegionFunctioning(travelBoardSquareBrushRegion)) ? 1 : 0);
		}
		else
		{
			num5 = 0;
		}
		bool flag7 = (byte)num5 != 0;
		int num6;
		if (!flag6)
		{
			num6 = ((num2 > 0) ? 1 : 0);
		}
		else
		{
			num6 = 1;
		}
		bool flag8 = (byte)num6 != 0;
		if (!flag)
		{
			if (flag2 || flag6)
			{
				if (m_stealthBrokenParameter.GetEndValue() == 1f)
				{
					m_stealthBrokenParameter.EaseTo(0f, 0.0166666675f);
				}
				if (flag2)
				{
					if (m_stealthFadeParameter.GetEndValue() == 0f)
					{
						m_stealthFadeParameter.EaseTo(1f, m_stealthFadeDuration);
					}
				}
				if (flag8)
				{
					if (m_stealthBrushParameter.GetEndValue() == 0f)
					{
						m_stealthBrushParameter.EaseTo(1f, 0.0166666675f);
					}
				}
				goto IL_03dd;
			}
		}
		if (m_stealthFadeParameter.GetEndValue() != 1f)
		{
			if (!flag7)
			{
				if (!flag8)
				{
					if (m_stealthBrushParameter.GetEndValue() == 1f)
					{
						m_stealthBrushParameter.EaseTo(0f, 0.0166666675f);
					}
				}
				goto IL_03dd;
			}
		}
		if (m_stealthShaderEnabled)
		{
			if (m_stealthBrokenParameter.GetEndValue() == 0f)
			{
				m_stealthBrokenParameter.EaseTo(1f, m_stealthBrokenEaseDuration);
				if (m_stealthFadeParameter.GetEndValue() == 1f)
				{
					m_stealthFadeParameter.EaseTo(0f, m_stealthBrokenEaseDuration);
				}
				if (m_stealthBrushParameter.GetEndValue() == 0f)
				{
					m_stealthBrushParameter.EaseTo(0f, m_stealthBrokenEaseDuration);
				}
			}
		}
		goto IL_03dd;
		IL_06e4:
		ResetMaterialsToDefaults();
		m_stealthShaderEnabled = false;
		goto IL_06f1;
	}

	private bool IsAnimatingStealthActivation()
	{
		int result;
		if (m_stealthShaderEnabled)
		{
			if (!m_parentActorData.IsHiddenInBrush() && m_stealthFadeParameter.GetEndValue() == 1f)
			{
				if (!Mathf.Approximately(m_stealthFadeParameter, 1f))
				{
					result = 1;
					goto IL_008e;
				}
			}
			result = ((!Mathf.Approximately(m_stealthBrushTransitionParameter, 0f)) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		goto IL_008e;
		IL_008e:
		return (byte)result != 0;
	}

	private void UpdateSelectionOutline()
	{
		bool flag = false;
		bool flag2 = m_parentActorData.IsDead();
		bool flag3 = m_parentActorData.IsVisibleToClient();
		bool updatingInConfirm = false;
		if (flag3)
		{
			if (!flag2)
			{
				if (m_renderers != null)
				{
					ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
					if (m_parentActorData.BeingTargetedByClientAbility(out bool _, out updatingInConfirm))
					{
						goto IL_009d;
					}
					if (activeOwnedActorData != null)
					{
						if (activeOwnedActorData.ShouldForceTargetOutlineForActor(m_parentActorData))
						{
							goto IL_009d;
						}
					}
				}
			}
		}
		goto IL_009f;
		IL_009d:
		flag = true;
		goto IL_009f;
		IL_009f:
		if (flag)
		{
			if (Camera.main != null)
			{
				PlayerSelectionEffect component = Camera.main.GetComponent<PlayerSelectionEffect>();
				if (component != null)
				{
					component.m_drawSelection = true;
					component.SetDrawingInConfirm(updatingInConfirm);
				}
			}
		}
		if (flag == m_showingOutline)
		{
			return;
		}
		while (true)
		{
			int num = LayerMask.NameToLayer("ActorSelected");
			int num2 = LayerMask.NameToLayer("Actor");
			if (flag)
			{
				SetMaterialFloatTeam();
				SetLayer(num, num2);
			}
			else
			{
				SetLayer(num2, num);
			}
			m_showingOutline = flag;
			object obj;
			if (Camera.main != null)
			{
				obj = Camera.main.GetComponent<PlayerSelectionEffect>();
			}
			else
			{
				obj = null;
			}
			PlayerSelectionEffect playerSelectionEffect = (PlayerSelectionEffect)obj;
			if (playerSelectionEffect != null)
			{
				while (true)
				{
					playerSelectionEffect.SetDrawingInConfirm(updatingInConfirm);
					return;
				}
			}
			return;
		}
	}

	private void UpdateAlphaForRenderer(Renderer curRenderer, int curRendererIndex)
	{
		if (curRenderer == null)
		{
			return;
		}
		while (true)
		{
			if (m_rendererDefaultColors == null)
			{
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
			Material[] materials = curRenderer.materials;
			if (curRenderer == null)
			{
				return;
			}
			for (int i = 0; i < materials.Length; i++)
			{
				if (m_rendererDefaultColors.Count <= curRendererIndex)
				{
					continue;
				}
				if (!(materials[i] != null))
				{
					continue;
				}
				if (!materials[i].HasProperty(materialColorProperty))
				{
					continue;
				}
				Color color = m_rendererDefaultColors[curRendererIndex][i];
				if (m_rendererDefaultColors[curRendererIndex][i].a == 1f)
				{
					color.a = Alpha;
				}
				else
				{
					color.a = m_rendererDefaultColors[curRendererIndex][i].a;
				}
				materials[i].color = color;
			}
			return;
		}
	}

	private void UpdateCameraTransparency()
	{
		if (Time.time >= m_cameraTransparencyLastChangeSetTime)
		{
			m_cameraTransparency += (1f - m_cameraTransparencyStartValue) * Time.deltaTime / m_cameraTransparencyTime;
			m_cameraTransparency = Mathf.Min(1f, m_cameraTransparency);
		}
		if (Alpha == m_cameraTransparency)
		{
			return;
		}
		while (true)
		{
			if (m_alphaUpdateMarkedDirty)
			{
				Alpha = m_cameraTransparency;
				m_alphaUpdateMarkedDirty = false;
				for (int i = 0; i < m_renderers.Length; i++)
				{
					UpdateAlphaForRenderer(m_renderers[i], i);
				}
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			return;
		}
	}

	internal float GetCamStartEventDelay(int animationIndex, bool useTauntCamAltTime)
	{
		float num = 0f;
		if (useTauntCamAltTime)
		{
			if (m_tauntCamStartEventDelays != null)
			{
				if (animationIndex >= 0)
				{
					if (animationIndex < m_tauntCamStartEventDelays.Length)
					{
						if (m_tauntCamStartEventDelays[animationIndex] > 0f)
						{
							return m_tauntCamStartEventDelays[animationIndex];
						}
					}
				}
			}
		}
		float result;
		if (animationIndex >= 0 && animationIndex < m_camStartEventDelays.Length)
		{
			result = m_camStartEventDelays[animationIndex];
		}
		else
		{
			result = 0f;
		}
		return result;
	}

	internal void EnableRagdoll(bool ragDollOn, ImpulseInfo impulseInfo = null)
	{
		Animator modelAnimator = GetModelAnimator();
		if (modelAnimator != null)
		{
			modelAnimator.enabled = !ragDollOn;
		}
		SkinnedMeshRenderer[] componentsInChildren = base.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer skinnedMeshRenderer in componentsInChildren)
		{
			if (skinnedMeshRenderer.enabled)
			{
				skinnedMeshRenderer.updateWhenOffscreen = ragDollOn;
			}
		}
		while (true)
		{
			int layer = LayerMask.NameToLayer("DeadActor");
			int layer2 = LayerMask.NameToLayer("Actor");
			Rigidbody[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<Rigidbody>();
			Rigidbody[] array = componentsInChildren2;
			foreach (Rigidbody rigidbody in array)
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
			object activeRigidbodies;
			if (ragDollOn)
			{
				activeRigidbodies = componentsInChildren2;
			}
			else
			{
				activeRigidbodies = null;
			}
			m_activeRigidbodies = (Rigidbody[])activeRigidbodies;
			Collider[] componentsInChildren3 = base.gameObject.GetComponentsInChildren<Collider>();
			Collider[] array2 = componentsInChildren3;
			foreach (Collider collider in array2)
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
			while (true)
			{
				if (m_rendererBoundsApproxCollider != null)
				{
					m_rendererBoundsApproxCollider.enabled = !ragDollOn;
				}
				if (ragDollOn)
				{
					ApplyImpulseOnRagdoll(impulseInfo, componentsInChildren2);
				}
				else
				{
					ResetRigidBodyOffsets();
				}
				return;
			}
		}
	}

	internal void ApplyImpulseOnRagdoll(ImpulseInfo impulseInfo, Rigidbody[] rigidBodies)
	{
		if (impulseInfo == null)
		{
			return;
		}
		while (true)
		{
			if (Application.isEditor)
			{
				Log.Info("Applying impulse on " + base.gameObject.name + ", impulse info:" + impulseInfo.GetDebugString());
			}
			if (rigidBodies == null)
			{
				rigidBodies = base.gameObject.GetComponentsInChildren<Rigidbody>();
			}
			if (impulseInfo.IsExplosion)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						if (TheatricsManager.RagdollOnlyApplyForceAtSingleJoint() && m_cachedRigidbodyForRagdollImpulse != null)
						{
							Vector3 a = m_parentActorData.GetHipJointRigidBodyPosition() - impulseInfo.ExplosionCenter;
							a.y = Mathf.Max(0.75f, a.y);
							float magnitude = a.magnitude;
							if (magnitude > 0.1f)
							{
								a.Normalize();
							}
							else
							{
								a = Vector3.up;
							}
							if (Application.isEditor)
							{
								Log.Info("Applying impulse on " + m_cachedRigidbodyForRagdollImpulse.name + ", impulse info:" + impulseInfo.GetDebugString());
							}
							m_cachedRigidbodyForRagdollImpulse.AddForce(impulseInfo.ExplosionMagnitude * a, ForceMode.Impulse);
							return;
						}
						float num;
						if (rigidBodies.Length > 0)
						{
							num = 1f / (float)rigidBodies.Length;
						}
						else
						{
							num = 1f;
						}
						float num2 = num;
						Rigidbody[] array = rigidBodies;
						foreach (Rigidbody rigidbody in array)
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
								sqrMagnitude = 0.1f;
								a2 = Vector3.up;
							}
							float num3 = 1f;
							float d = Mathf.Clamp(num2 * num3 * impulseInfo.ExplosionMagnitude, 1f, 3000f);
							rigidbody.AddForceAtPosition(a2 * d, vector, ForceMode.Impulse);
						}
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								if (Application.isEditor)
								{
									Log.Info("Applying impulse on ALL rigidbodies, Impulse on each = " + num2 * impulseInfo.ExplosionMagnitude + " (total impulse = " + impulseInfo.ExplosionMagnitude + ")");
								}
								return;
							}
						}
					}
					}
				}
			}
			if (TheatricsManager.RagdollOnlyApplyForceAtSingleJoint())
			{
				if (m_cachedRigidbodyForRagdollImpulse != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							if (Application.isEditor)
							{
								Log.Info("Applying impulse on " + m_cachedRigidbodyForRagdollImpulse.name + ", impulse info:" + impulseInfo.GetDebugString());
							}
							m_cachedRigidbodyForRagdollImpulse.AddForce(impulseInfo.HitImpulse, ForceMode.Impulse);
							return;
						}
					}
				}
			}
			float num4 = float.MaxValue;
			object obj;
			if (rigidBodies.Length > 0)
			{
				obj = rigidBodies[0];
			}
			else
			{
				obj = null;
			}
			Rigidbody rigidbody2 = (Rigidbody)obj;
			Vector3 position = impulseInfo.HitPosition;
			Vector3 hitPosition = impulseInfo.HitPosition;
			Rigidbody[] array2 = rigidBodies;
			foreach (Rigidbody rigidbody3 in array2)
			{
				Vector3 vector2 = rigidbody3.ClosestPointOnBounds(hitPosition);
				float sqrMagnitude2 = (vector2 - hitPosition).sqrMagnitude;
				if (sqrMagnitude2 < num4)
				{
					num4 = sqrMagnitude2;
					rigidbody2 = rigidbody3;
					position = vector2;
				}
			}
			if (!(rigidbody2 != null))
			{
				return;
			}
			while (true)
			{
				rigidbody2.AddForceAtPosition(impulseInfo.HitImpulse, position, ForceMode.Impulse);
				if (Application.isEditor)
				{
					Log.Info("Applying impulse on rigidbody " + rigidbody2.name + ", with impulse magnitude " + impulseInfo.HitImpulse.magnitude);
				}
				return;
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(m_rendererBoundsApprox.center + base.transform.position, m_rendererBoundsApprox.size);
		}
	}
}
