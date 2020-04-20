using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HighlightUtils : MonoBehaviour, IGameEventListener
{
	private static HighlightUtils s_instance;

	public Color s_defaultMovementColor = new Color(1f, 1f, 1f, 0.5f);

	public Color s_defaultPotentialMovementColor = new Color(0f, 1f, 1f, 0.5f);

	public Color s_defaultChasingColor = new Color(1f, 1f, 1f, 0.75f);

	public Color s_defaultPotentialChasingColor = new Color(0f, 0.4f, 0.4f, 0.5f);

	public Color s_defaultLostMovementColor = new Color(0.5f, 0f, 0f, 0.5f);

	public Color s_knockbackMovementLineColor = new Color(1f, 0.9f, 0.2f, 0.75f);

	public Color s_attackMovementLineColor = new Color(1f, 0f, 0f, 0.75f);

	public Color s_dashMovementLineColor = new Color(1f, 0f, 0f, 0.75f);

	public Color s_teleportMovementLineColor = new Color(0f, 1f, 1f, 0.5f);

	public Color s_flashMovementLineColor = Color.white * 0.5f;

	public float s_defaultDimMultiplier = 0.33f;

	[Separator("Movement Line", true)]
	public Material m_ArrowLineMaterial;

	public Material m_ArrowChaseLineMaterial;

	public Material m_ArrowDottedLineMaterial;

	public MovementPathStart m_ArrowStartPrefab;

	public MovementPathEnd m_ArrowEndPrefab;

	public TeleportPathStart m_teleportPrefab;

	public TeleportPathEnd m_teleportEndPrefab;

	[Separator("Valid Squares Selection Highlight", true)]
	public Material m_highlightMaterial;

	public Material m_dottedHighlightMaterial;

	public GameObject m_movementGlowObject;

	public GameObject m_innerCornerPiece;

	public GameObject m_horizontalSidePiece;

	public GameObject m_verticalSidePiece;

	public GameObject m_outerCornerPiece;

	public GameObject m_highlightFloodFillSquare;

	[Separator("Mouseover Cursors", true)]
	public GameObject m_targetingCursorPrefab;

	public GameObject m_mouseOverCursorPrefab;

	public GameObject m_freeCursorPrefab;

	public GameObject m_movementPrefab;

	public GameObject m_abilityTargetPrefab;

	public GameObject m_IdleTargetPrefab;

	public GameObject m_cornerCursorPrefab;

	public GameObject m_sprintHighlightPrefab;

	public GameObject m_chaseSquareCursorPrefab;

	public GameObject m_chaseSquareCursorSecondaryPrefab;

	public GameObject m_respawnSelectionCursorPrefab;

	private GameObject m_targetingCursor;

	[Separator("Cover", true)]
	public GameObject m_coverIndicatorPrefab;

	public GameObject m_coverShieldOnlyPrefab;

	[Header("-- Mouse-over Cover Indicators --")]
	public GameObject m_mouseoverCoverShieldPrefab;

	public bool m_showMouseoverCoverIndicators = true;

	public float m_mouseoverCoverAreaRadius = 1.5f;

	public float m_mouseoverCoverIconAlpha = 0.1f;

	public float m_mouseoverHeightOffset = 2f;

	public HighlightUtils.CoverDirIndicatorParams m_mouseoverCoverDirParams;

	[Header("-- Move Into Cover Indicators --")]
	public bool m_showMoveIntoCoverIndicators;

	public HighlightUtils.MoveIntoCoverIndicatorTiming m_coverDirIndicatorTiming = HighlightUtils.MoveIntoCoverIndicatorTiming.ShowOnTurnStart;

	public float m_coverDirIndicatorRadiusInSquares = 3f;

	[Space(5f)]
	public float m_coverDirIndicatorDuration = 3f;

	public float m_coverDirIndicatorStartDelay = 0.5f;

	public float m_coverDirFadeoutStartDelay = 1f;

	[Space(5f)]
	public Color m_coverDirIndicatorColor = new Color(1f, 0.75f, 0.25f);

	public float m_coverDirIndicatorInitialOpacity = 0.08f;

	public float m_coverDirParticleInitialOpacity = 0.5f;

	[Separator("Ability Range Indicator", true)]
	public bool m_showAbilityRangeIndicator = true;

	public Color m_abilityRangeIndicatorColor = new Color(1f, 1f, 1f, 0.15f);

	public float m_abilityRangeIndicatorWidth = 0.075f;

	[Separator("Movement Line Preview", true)]
	public bool m_showMovementPreview = true;

	public Color m_movementLinePreviewColor = new Color(0f, 0.3f, 0.8f, 0.15f);

	[Separator("For Actor VFX", true)]
	public GameObject m_chaseMouseoverPrefab;

	public GameObject m_moveSquareCursorPrefab;

	public bool m_useActorVfxForMovementDebuff;

	public GameObject m_concealedVFXPrefab;

	public GameObject m_footstepsVFXPrefab;

	[Header("-- Generic Status VFX on Actor --")]
	public StatusVfxPrefabToJoint[] m_statusVfxPrefabToJoint;

	[Header("-- Knocked Down Status VFX --")]
	public GameObject m_knockedBackVFXPrefab;

	[JointPopup("KnockedDown Status VFX Attach Joint")]
	public JointPopupProperty m_knockedBackVfxJoint;

	[Separator("For Brush Regions", true)]
	public GameObject m_brushFunctioningSquarePrefab;

	public GameObject m_brushDisruptedSquarePrefab;

	[Space(5f)]
	public GameObject m_brushFunctioningBorderPrefab;

	public GameObject m_brushDisruptedBorderPrefab;

	public float m_brushBorderHeightOffset;

	[Header("-- for moving in/out of brush --")]
	public GameObject m_moveIntoBrushVfxPrefab;

	public GameObject m_moveOutOfBrushVfxPrefab;

	[Separator("On Death VFX", true)]
	public GameObject m_onDeathVFXPrefab;

	public float m_onDeathVFXDuration = 3f;

	[Separator("On Respawn VFX", true)]
	public GameObject m_onRespawnVFXPrefab;

	public float m_onRespawnVFXDuration = 3f;

	[Tooltip("On first turn of death, when you first pick the location")]
	public GameObject m_respawnPositionFlareFriendlyVFXPrefab;

	[Tooltip("On first turn of death, when you first pick the location")]
	public GameObject m_respawnPositionFlareEnemyVFXPrefab;

	[Tooltip("On second turn of death, when you are about to respawn in the location")]
	public GameObject m_respawnPositionFinalFriendlyVFXPrefab;

	[Tooltip("On second turn of death, when you are about to respawn in the location")]
	public GameObject m_respawnPositionFinalEnemyVFXPrefab;

	public Shader m_recentlySpawnedShader;

	[Separator("Hit in Cover VFX", true)]
	public GameObject m_hitInCoverVFXPrefab;

	public float m_hitInCoverVFXDuration = 2f;

	[Separator("Knockback Hit while Unstoppable", true)]
	public GameObject m_knockbackHitWhileUnstoppableVFXPrefab;

	public float m_knockbackHitWhileUnstoppableVFXDuration = 3f;

	[Space(10f)]
	public float m_targeterBoundaryLineLength = 3.75f;

	public Texture2D m_scrollCursorEdge;

	public Texture2D m_scrollCursorCorner;

	[Space(20f)]
	[Header("-- Targeter VFX: Aoe, Laser --")]
	public GameObject m_AoECursorPrefab;

	public GameObject m_AoECursorAllyPrefab;

	public GameObject m_rectangleCursorPrefab;

	public GameObject m_bouncingLaserTargeterPrefab;

	public float m_AoECursorRadius = 2.25f;

	[Space(10f)]
	public GameObject m_teamAPersistentAoEPrefab;

	public GameObject m_teamBPersistentAoEPrefab;

	public GameObject m_allyPersistentAoEPrefab;

	public GameObject m_allyPersistentHelpfulAoEPrefab;

	public GameObject m_enemyPersistentAoEPrefab;

	public GameObject m_enemyPersistentHelpfulAoEPrefab;

	[Header("-- Targeter VFX: Boundary Lines --")]
	public GameObject m_targeterBoundaryLineInner;

	public GameObject m_targeterBoundaryLineOuter;

	[Header("-- Targeter VFX: Shapes --")]
	public HighlightUtils.AreaShapePrefab[] m_areaShapePrefabs;

	public bool m_showTargetingArcsForShapes = true;

	public float m_minDistForTargetingArc = 5f;

	public float m_targetingArcMovementSpeed = 5f;

	public float m_targetingArcMaxHeight = 2.5f;

	public int m_targetingArcNumSegments = 0x10;

	public Color m_targetingArcColor = Color.cyan;

	public Color m_targetingArcColorAllies = Color.gray;

	public GameObject m_targetingArcForShape;

	public Material m_targetingArcMaterial;

	[Header("-- Targeter VFX: Cones --")]
	public GameObject[] m_conePrefabs;

	public GameObject m_dynamicConePrefab;

	[Header("-- Targeter VFX: Dynamic Line Segment --")]
	public GameObject m_dynamicLineSegmentPrefab;

	[Header("-- Targeter Opacity --")]
	public bool m_setTargeterOpacityWhileTargeting;

	public float m_targeterOpacityWhileTargeting = 0.15f;

	public float m_targeterParticleSystemOpacityMultiplier = 3f;

	public HighlightUtils.TargeterOpacityData[] m_confirmedTargeterOpacity;

	public HighlightUtils.TargeterOpacityData[] m_allyTargeterOpacity;

	[Header("-- Color/Opacity for removing targeter")]
	public HighlightUtils.TargeterOpacityData[] m_targeterRemoveFadeOpacity;

	public Color m_targeterRemoveColor = Color.red;

	[Header("-- Opacity for movement")]
	public HighlightUtils.TargeterOpacityData[] m_movementLineOpacity;

	[Separator("Ability Icon Color", true)]
	public Color m_allyTargetedAbilityIconColor = Color.yellow;

	public Color m_allyCooldownAbilityIconColor = Color.gray;

	public Color m_allyAvailableAbilityIconColor = Color.white;

	public Color m_allyTargetingAbilityIconColor = Color.white;

	[Separator("Hidden/Affected Square Indicators (while targeting)", true)]
	public GameObject m_hiddenSquarePrefab;

	public GameObject m_affectedSquarePrefab;

	public bool m_showAffectedSquaresWhileTargeting;

	[Separator("For Base Circle Indicator", true)]
	public GameObject m_friendBaseCirclePrefab;

	public GameObject m_enemyBaseCirclePrefab;

	public float m_circlePrefabHeight;

	[JointPopup("Base Circle VFX Attach Joint")]
	public JointPopupProperty m_baseCircleJoint;

	public float m_baseCircleSizeInSquares = 1f;

	[Separator("Nameplate Setting", true)]
	public bool m_enableAccumulatedAllyNumbers;

	private bool m_startCalled;

	private bool m_hideCursor;

	private HighlightUtils.CursorType m_currentCursorType = HighlightUtils.CursorType.NoCursorType;

	private bool m_isCursorSet;

	private List<GameObject> m_highlightCursors = new List<GameObject>();

	private MouseoverCoverManager m_mouseoverCoverManager;

	private GameObject m_mouseoverCoverParent;

	private HighlightUtils.HighlightPiece[] m_highlightPieces = new HighlightUtils.HighlightPiece[0x10];

	private GameObject m_surroundedSquaresParent;

	private SquareIndicators m_surroundedSquareIndicators;

	private GameObject m_hiddenSquareIndicatorParent;

	private SquareIndicators m_hiddenSquareIndicators;

	private GameObject m_affectedSquareIndicatorParent;

	private SquareIndicators m_affectedSquareIndicators;

	private GameObject m_abilityRangeIndicatorHighlight;

	private List<bool> m_rangeIndicatorMouseOverFlags;

	private float m_lastRangeIndicatorRadius;

	public bool m_cachedShouldShowAffectedSquares;

	[CompilerGenerated]
	private static SquareIndicators.CreateIndicatorDelegate <>f__mg$cache0;

	[CompilerGenerated]
	private static SquareIndicators.CreateIndicatorDelegate <>f__mg$cache1;

	[CompilerGenerated]
	private static SquareIndicators.CreateIndicatorDelegate <>f__mg$cache2;

	public HighlightUtils()
	{
		if (HighlightUtils.<>f__mg$cache0 == null)
		{
			HighlightUtils.<>f__mg$cache0 = new SquareIndicators.CreateIndicatorDelegate(HighlightUtils.CreateSurroundedSquareObject);
		}
		this.m_surroundedSquareIndicators = new SquareIndicators(HighlightUtils.<>f__mg$cache0, 0xF, 0xA, 0.12f);
		if (HighlightUtils.<>f__mg$cache1 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils..ctor()).MethodHandle;
			}
			HighlightUtils.<>f__mg$cache1 = new SquareIndicators.CreateIndicatorDelegate(HighlightUtils.CreateHiddenSquareIndicatorObject);
		}
		this.m_hiddenSquareIndicators = new SquareIndicators(HighlightUtils.<>f__mg$cache1, 0xF, 0xA, 0.09f);
		if (HighlightUtils.<>f__mg$cache2 == null)
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
			HighlightUtils.<>f__mg$cache2 = new SquareIndicators.CreateIndicatorDelegate(HighlightUtils.CreateAffectedSquareIndicatorObject);
		}
		this.m_affectedSquareIndicators = new SquareIndicators(HighlightUtils.<>f__mg$cache2, 0xF, 0xA, 0.09f);
		this.m_rangeIndicatorMouseOverFlags = new List<bool>();
		this.m_lastRangeIndicatorRadius = -1f;
		base..ctor();
	}

	public static HighlightUtils Get()
	{
		return HighlightUtils.s_instance;
	}

	internal GameObject ClampedMouseOverCursor { get; private set; }

	internal GameObject FreeMouseOverCursor { get; private set; }

	internal GameObject CornerMouseOverCursor { get; private set; }

	public GameObject MovementMouseOverCursor { get; private set; }

	internal GameObject AbilityTargetMouseOverCursor { get; private set; }

	internal GameObject IdleMouseOverCursor { get; private set; }

	public GameObject SprintMouseOverCursor { get; private set; }

	internal GameObject ChaseSquareCursor { get; private set; }

	internal GameObject ChaseSquareCursorAlt { get; private set; }

	internal GameObject RespawnSelectionCursor { get; private set; }

	internal Dictionary<HighlightUtils.ScrollCursorDirection, Texture2D> ScrollCursors { get; private set; }

	internal Dictionary<HighlightUtils.ScrollCursorDirection, Vector2> ScrollCursorsHotspot { get; private set; }

	public static void DestroyBoundaryHighlightObject(GameObject highlightObject)
	{
		if (highlightObject != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.DestroyBoundaryHighlightObject(GameObject)).MethodHandle;
			}
			foreach (Renderer renderer in highlightObject.GetComponentsInChildren<Renderer>(true))
			{
				if (renderer.material != null)
				{
					UnityEngine.Object.Destroy(renderer.material);
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
			foreach (MeshFilter meshFilter in highlightObject.GetComponents<MeshFilter>())
			{
				UnityEngine.Object.Destroy(meshFilter.mesh);
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
			UnityEngine.Object.DestroyImmediate(highlightObject);
		}
		if (HighlightUtils.s_instance != null)
		{
			HighlightUtils.s_instance.m_surroundedSquareIndicators.HideAllSquareIndicators(0);
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.VisualSceneLoaded)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
			this.m_surroundedSquareIndicators.Initialize();
			this.m_surroundedSquareIndicators.HideAllSquareIndicators(0);
			this.m_hiddenSquareIndicators.Initialize();
			this.m_hiddenSquareIndicators.HideAllSquareIndicators(0);
			this.m_mouseoverCoverManager.Initialize(this.m_mouseoverCoverParent);
			this.CreateRangeIndicatorHighlight();
		}
	}

	public static void DestroyMeshesOnObject(GameObject obj)
	{
		if (obj != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.DestroyMeshesOnObject(GameObject)).MethodHandle;
			}
			MeshFilter[] components = obj.GetComponents<MeshFilter>();
			foreach (MeshFilter meshFilter in components)
			{
				UnityEngine.Object.Destroy(meshFilter.mesh);
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
			MeshFilter[] componentsInChildren = obj.GetComponentsInChildren<MeshFilter>(true);
			foreach (MeshFilter meshFilter2 in componentsInChildren)
			{
				UnityEngine.Object.Destroy(meshFilter2.mesh);
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

	public static void DestroyMaterials(Material[] materials)
	{
		if (materials != null)
		{
			foreach (Material material in materials)
			{
				if (material != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.DestroyMaterials(Material[])).MethodHandle;
					}
					UnityEngine.Object.Destroy(material);
				}
			}
		}
	}

	public static void SetParticleSystemScale(GameObject particleObject, float scale)
	{
		ParticleSystem[] componentsInChildren = particleObject.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem particleSystem in componentsInChildren)
		{
			ParticleSystem.MainModule main = particleSystem.main;
			ParticleSystem.MinMaxCurve startSize = main.startSize;
			startSize.constant *= scale;
			main.startSize = startSize;
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.SetParticleSystemScale(GameObject, float)).MethodHandle;
		}
	}

	public static void DestroyObjectAndMaterials(GameObject highlightObj)
	{
		if (highlightObj != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.DestroyObjectAndMaterials(GameObject)).MethodHandle;
			}
			Renderer[] components = highlightObj.GetComponents<Renderer>();
			foreach (Renderer renderer in components)
			{
				HighlightUtils.DestroyMaterials(renderer.materials);
			}
			Renderer[] componentsInChildren = highlightObj.GetComponentsInChildren<Renderer>(true);
			foreach (Renderer renderer2 in componentsInChildren)
			{
				HighlightUtils.DestroyMaterials(renderer2.materials);
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
			UnityEngine.Object.Destroy(highlightObj);
		}
	}

	public bool HideCursor
	{
		get
		{
			return this.m_hideCursor;
		}
		set
		{
			this.m_hideCursor = value;
			this.RefreshCursorVisibility();
		}
	}

	private void Awake()
	{
		this.InitializeHighlightPieces();
		HighlightUtils.s_instance = this;
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.identity;
		base.transform.localScale = Vector3.one;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.VisualSceneLoaded);
		}
		this.m_mouseoverCoverParent = new GameObject("MouseoverCoverIndicators");
		UnityEngine.Object.DontDestroyOnLoad(this.m_mouseoverCoverParent);
		this.m_mouseoverCoverManager = new MouseoverCoverManager();
	}

	private void OnDestroy()
	{
		HighlightUtils.s_instance = null;
		if (GameEventManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.OnDestroy()).MethodHandle;
			}
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.VisualSceneLoaded);
		}
		this.m_surroundedSquareIndicators.ClearAllSquareIndicators();
		if (this.m_surroundedSquaresParent != null)
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
			UnityEngine.Object.Destroy(this.m_surroundedSquaresParent);
			this.m_surroundedSquaresParent = null;
		}
		this.m_hiddenSquareIndicators.ClearAllSquareIndicators();
		if (this.m_hiddenSquareIndicatorParent != null)
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
			UnityEngine.Object.Destroy(this.m_hiddenSquareIndicatorParent);
			this.m_hiddenSquareIndicatorParent = null;
		}
		this.m_affectedSquareIndicators.ClearAllSquareIndicators();
		if (this.m_affectedSquareIndicatorParent != null)
		{
			UnityEngine.Object.Destroy(this.m_affectedSquareIndicatorParent);
			this.m_affectedSquareIndicatorParent = null;
		}
		if (this.m_mouseoverCoverParent != null)
		{
			UnityEngine.Object.Destroy(this.m_mouseoverCoverParent);
			this.m_mouseoverCoverParent = null;
		}
		this.DestroyRangeIndicatorHighlight();
	}

	private void Initialize2DScrollCursors()
	{
		if (!(this.m_scrollCursorEdge != null))
		{
			if (!(this.m_scrollCursorCorner != null))
			{
				return;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.Initialize2DScrollCursors()).MethodHandle;
			}
		}
		this.ScrollCursors = new Dictionary<HighlightUtils.ScrollCursorDirection, Texture2D>();
		this.ScrollCursorsHotspot = new Dictionary<HighlightUtils.ScrollCursorDirection, Vector2>();
		IEnumerator enumerator = Enum.GetValues(typeof(HighlightUtils.ScrollCursorDirection)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				HighlightUtils.ScrollCursorDirection scrollCursorDirection = (HighlightUtils.ScrollCursorDirection)obj;
				switch (scrollCursorDirection)
				{
				case HighlightUtils.ScrollCursorDirection.N:
				case HighlightUtils.ScrollCursorDirection.S:
					if (this.m_scrollCursorEdge != null)
					{
						this.ScrollCursors[scrollCursorDirection] = new Texture2D(this.m_scrollCursorEdge.height, this.m_scrollCursorEdge.width);
						int num = this.m_scrollCursorEdge.height / 2;
						int num2 = (scrollCursorDirection != HighlightUtils.ScrollCursorDirection.N) ? (this.m_scrollCursorEdge.width - 1) : 0;
						this.ScrollCursorsHotspot[scrollCursorDirection] = new Vector2((float)num, (float)num2);
					}
					break;
				case HighlightUtils.ScrollCursorDirection.NE:
				case HighlightUtils.ScrollCursorDirection.SE:
				case HighlightUtils.ScrollCursorDirection.SW:
				case HighlightUtils.ScrollCursorDirection.NW:
					if (this.m_scrollCursorCorner != null)
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
						this.ScrollCursors[scrollCursorDirection] = new Texture2D(this.m_scrollCursorCorner.width, this.m_scrollCursorCorner.height);
						if (scrollCursorDirection == HighlightUtils.ScrollCursorDirection.NW)
						{
							goto IL_211;
						}
						int num3;
						if (scrollCursorDirection == HighlightUtils.ScrollCursorDirection.SW)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								goto IL_211;
							}
						}
						else
						{
							num3 = this.m_scrollCursorCorner.width - 1;
						}
						IL_223:
						int num4 = num3;
						if (scrollCursorDirection == HighlightUtils.ScrollCursorDirection.NW)
						{
							goto IL_241;
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
						int num5;
						if (scrollCursorDirection == HighlightUtils.ScrollCursorDirection.NE)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								goto IL_241;
							}
						}
						else
						{
							num5 = this.m_scrollCursorCorner.height - 1;
						}
						IL_253:
						int num6 = num5;
						this.ScrollCursorsHotspot[scrollCursorDirection] = new Vector2((float)num4, (float)num6);
						break;
						IL_241:
						num5 = 0;
						goto IL_253;
						IL_211:
						num3 = 0;
						goto IL_223;
					}
					break;
				case HighlightUtils.ScrollCursorDirection.E:
				case HighlightUtils.ScrollCursorDirection.W:
					if (this.m_scrollCursorEdge != null)
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
						this.ScrollCursors[scrollCursorDirection] = new Texture2D(this.m_scrollCursorEdge.width, this.m_scrollCursorEdge.height);
						int num7;
						if (scrollCursorDirection == HighlightUtils.ScrollCursorDirection.W)
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
							num7 = 0;
						}
						else
						{
							num7 = this.m_scrollCursorEdge.width - 1;
						}
						int num8 = num7;
						int num9 = this.m_scrollCursorEdge.height / 2;
						this.ScrollCursorsHotspot[scrollCursorDirection] = new Vector2((float)num8, (float)num9);
					}
					break;
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
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
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
				disposable.Dispose();
			}
		}
		for (int i = 0; i < this.m_scrollCursorEdge.width; i++)
		{
			for (int j = 0; j < this.m_scrollCursorEdge.height; j++)
			{
				if (this.m_scrollCursorEdge != null)
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
					this.ScrollCursors[HighlightUtils.ScrollCursorDirection.W].SetPixel(i, j, this.m_scrollCursorEdge.GetPixel(i, j));
					this.ScrollCursors[HighlightUtils.ScrollCursorDirection.E].SetPixel(i, j, this.m_scrollCursorEdge.GetPixel(this.m_scrollCursorEdge.width - (i + 1), j));
					this.ScrollCursors[HighlightUtils.ScrollCursorDirection.N].SetPixel(i, j, this.m_scrollCursorEdge.GetPixel(this.m_scrollCursorEdge.height - (j + 1), i));
					this.ScrollCursors[HighlightUtils.ScrollCursorDirection.S].SetPixel(i, j, this.m_scrollCursorEdge.GetPixel(j, i));
				}
				if (this.m_scrollCursorCorner != null)
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
					this.ScrollCursors[HighlightUtils.ScrollCursorDirection.NW].SetPixel(i, j, this.m_scrollCursorCorner.GetPixel(i, j));
					this.ScrollCursors[HighlightUtils.ScrollCursorDirection.SW].SetPixel(i, j, this.m_scrollCursorCorner.GetPixel(i, this.m_scrollCursorCorner.height - (j + 1)));
					this.ScrollCursors[HighlightUtils.ScrollCursorDirection.NE].SetPixel(i, j, this.m_scrollCursorCorner.GetPixel(this.m_scrollCursorCorner.width - (i + 1), j));
					this.ScrollCursors[HighlightUtils.ScrollCursorDirection.SE].SetPixel(i, j, this.m_scrollCursorCorner.GetPixel(this.m_scrollCursorCorner.width - (i + 1), this.m_scrollCursorCorner.height - (j + 1)));
				}
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

	private void Start()
	{
		this.m_highlightCursors.Clear();
		this.m_targetingCursor = UnityEngine.Object.Instantiate<GameObject>(this.m_targetingCursorPrefab);
		this.m_targetingCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(this.m_targetingCursor, false, null);
		this.m_highlightCursors.Add(this.m_targetingCursor);
		this.ClampedMouseOverCursor = UnityEngine.Object.Instantiate<GameObject>(this.m_mouseOverCursorPrefab);
		this.ClampedMouseOverCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(this.ClampedMouseOverCursor, false, null);
		this.m_highlightCursors.Add(this.ClampedMouseOverCursor);
		this.FreeMouseOverCursor = UnityEngine.Object.Instantiate<GameObject>(this.m_freeCursorPrefab);
		this.FreeMouseOverCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(this.FreeMouseOverCursor, false, null);
		this.m_highlightCursors.Add(this.FreeMouseOverCursor);
		this.CornerMouseOverCursor = UnityEngine.Object.Instantiate<GameObject>(this.m_cornerCursorPrefab);
		this.CornerMouseOverCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(this.CornerMouseOverCursor, false, null);
		this.m_highlightCursors.Add(this.CornerMouseOverCursor);
		this.MovementMouseOverCursor = UnityEngine.Object.Instantiate<GameObject>(this.m_movementPrefab);
		this.MovementMouseOverCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(this.MovementMouseOverCursor, false, null);
		this.m_highlightCursors.Add(this.MovementMouseOverCursor);
		this.AbilityTargetMouseOverCursor = UnityEngine.Object.Instantiate<GameObject>(this.m_abilityTargetPrefab);
		this.AbilityTargetMouseOverCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(this.AbilityTargetMouseOverCursor, false, null);
		this.m_highlightCursors.Add(this.AbilityTargetMouseOverCursor);
		this.IdleMouseOverCursor = UnityEngine.Object.Instantiate<GameObject>(this.m_IdleTargetPrefab);
		this.IdleMouseOverCursor.transform.parent = base.transform;
		UIManager.SetGameObjectActive(this.IdleMouseOverCursor, false, null);
		this.m_highlightCursors.Add(this.IdleMouseOverCursor);
		if (this.m_respawnSelectionCursorPrefab != null)
		{
			this.RespawnSelectionCursor = UnityEngine.Object.Instantiate<GameObject>(this.m_respawnSelectionCursorPrefab);
			this.RespawnSelectionCursor.transform.parent = base.transform;
			UIManager.SetGameObjectActive(this.RespawnSelectionCursor, false, null);
			this.m_highlightCursors.Add(this.RespawnSelectionCursor);
		}
		if (this.m_chaseSquareCursorPrefab != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.Start()).MethodHandle;
			}
			this.ChaseSquareCursor = UnityEngine.Object.Instantiate<GameObject>(this.m_chaseSquareCursorPrefab);
			this.ChaseSquareCursor.transform.parent = base.transform;
			UIManager.SetGameObjectActive(this.ChaseSquareCursor, false, null);
			this.m_highlightCursors.Add(this.ChaseSquareCursor);
		}
		if (this.m_chaseSquareCursorSecondaryPrefab != null)
		{
			this.ChaseSquareCursorAlt = UnityEngine.Object.Instantiate<GameObject>(this.m_chaseSquareCursorSecondaryPrefab);
			this.ChaseSquareCursorAlt.transform.parent = base.transform;
			UIManager.SetGameObjectActive(this.ChaseSquareCursorAlt, false, null);
			this.m_highlightCursors.Add(this.ChaseSquareCursorAlt);
		}
		this.Initialize2DScrollCursors();
		this.m_startCalled = true;
		this.RefreshCursorVisibility();
	}

	public void HideCursorHighlights()
	{
		for (int i = 0; i < this.m_highlightCursors.Count; i++)
		{
			GameObject gameObject = this.m_highlightCursors[i];
			if (gameObject != null)
			{
				UIManager.SetGameObjectActive(gameObject, false, null);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.HideCursorHighlights()).MethodHandle;
		}
	}

	public GameObject CloneTargeterHighlight(GameObject source, AbilityUtil_Targeter targeter)
	{
		if (!(source != null))
		{
			return null;
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CloneTargeterHighlight(GameObject, AbilityUtil_Targeter)).MethodHandle;
		}
		UIDynamicCone component = source.GetComponent<UIDynamicCone>();
		if (component != null)
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
			GameObject gameObject = this.CreateDynamicConeMesh(component.m_currentRadiusInWorld / Board.Get().squareSize, component.m_currentAngleInWorld, component.m_forceHideSides, targeter.GetTemplateSwapData());
			gameObject.transform.position = source.transform.position;
			gameObject.transform.rotation = source.transform.rotation;
			gameObject.transform.localScale = source.transform.localScale;
			return gameObject;
		}
		UIDynamicLineSegment component2 = source.GetComponent<UIDynamicLineSegment>();
		if (component2 != null)
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
			GameObject gameObject2 = this.CreateDynamicLineSegmentMesh(component2.m_currentLengthInWorld / Board.Get().squareSize, component2.m_currentWidthInWorld, component2.m_dotted, component2.m_currentColor);
			gameObject2.transform.position = source.transform.position;
			gameObject2.transform.rotation = source.transform.rotation;
			gameObject2.transform.localScale = source.transform.localScale;
			return gameObject2;
		}
		return UnityEngine.Object.Instantiate<GameObject>(source);
	}

	public GameObject GetTargeterTemplatePrefabToUse(GameObject input, TargeterTemplateSwapData.TargeterTemplateType inputTemplateType, List<TargeterTemplateSwapData> templateSwaps)
	{
		GameObject result = input;
		if (input != null && templateSwaps != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.GetTargeterTemplatePrefabToUse(GameObject, TargeterTemplateSwapData.TargeterTemplateType, List<TargeterTemplateSwapData>)).MethodHandle;
			}
			if (templateSwaps.Count > 0)
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
				for (int i = 0; i < templateSwaps.Count; i++)
				{
					if (templateSwaps[i].m_templateToReplace == inputTemplateType)
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
						if (templateSwaps[i].m_prefabToUse != null)
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
							return templateSwaps[i].m_prefabToUse;
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
		}
		return result;
	}

	public GameObject CreateRectangularCursor(float widthInWorld, float lengthInWorld, List<TargeterTemplateSwapData> templateSwapData = null)
	{
		GameObject gameObject = this.m_rectangleCursorPrefab;
		if (templateSwapData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateRectangularCursor(float, float, List<TargeterTemplateSwapData>)).MethodHandle;
			}
			gameObject = this.GetTargeterTemplatePrefabToUse(gameObject, TargeterTemplateSwapData.TargeterTemplateType.Laser, templateSwapData);
		}
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
		UIRectangleCursor component = gameObject2.GetComponent<UIRectangleCursor>();
		if (component != null)
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
			component.OnDimensionsChanged(widthInWorld, lengthInWorld);
		}
		return gameObject2;
	}

	public void ResizeRectangularCursor(float widthInWorld, float lengthInWorld, GameObject highlight)
	{
		highlight.GetComponent<UIRectangleCursor>().OnDimensionsChanged(widthInWorld, lengthInWorld);
	}

	public void RotateAndResizeRectangularCursor(GameObject highlightObj, Vector3 startPos, Vector3 endPos, float widthInSquares)
	{
		startPos.y = HighlightUtils.GetHighlightHeight();
		endPos.y = startPos.y;
		float widthInWorld = widthInSquares * Board.Get().squareSize;
		Vector3 vector = endPos - startPos;
		float magnitude = vector.magnitude;
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, highlightObj);
		Vector3 normalized = vector.normalized;
		highlightObj.transform.position = startPos;
		highlightObj.transform.rotation = Quaternion.LookRotation(normalized);
	}

	public GameObject CreateConeCursor(float radiusInWorld, float arcDegrees)
	{
		return this.CreateDynamicConeMesh(radiusInWorld / Board.Get().squareSize, arcDegrees, false, null);
	}

	public GameObject CreateConeCursor_FixedSize(float radiusInWorld, float arcDegrees)
	{
		GameObject original = null;
		float num = float.MaxValue;
		foreach (GameObject gameObject in this.m_conePrefabs)
		{
			UIConeCursor component = gameObject.GetComponent<UIConeCursor>();
			if (component == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateConeCursor_FixedSize(float, float)).MethodHandle;
				}
				Log.Error("HighlightUtils cone prefabs has a cone without a UIConeCursor component.", new object[0]);
			}
			else
			{
				if (component.m_arcDegrees >= 0f)
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
					if (component.m_arcDegrees > 360f)
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
						float num2 = Mathf.Abs(arcDegrees - component.m_arcDegrees);
						if (num2 < num)
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
							num = num2;
							original = gameObject;
							goto IL_D8;
						}
						goto IL_D8;
					}
				}
				Log.Error("HighlightUtils cone prefabs has a cone " + gameObject.name + " with an invalid arcDegrees.  Valid degrees are in (0, 360).", new object[0]);
			}
			IL_D8:;
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
		GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(original);
		UIConeCursor component2 = gameObject2.GetComponent<UIConeCursor>();
		component2.OnRadiusChanged(radiusInWorld);
		return gameObject2;
	}

	public GameObject CreateDynamicConeMesh(float initialRadiusInSquares, float initialAngle, bool forceHideSides, List<TargeterTemplateSwapData> templateSwapData = null)
	{
		GameObject gameObject = null;
		GameObject gameObject2 = this.m_dynamicConePrefab;
		if (templateSwapData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateDynamicConeMesh(float, float, bool, List<TargeterTemplateSwapData>)).MethodHandle;
			}
			gameObject2 = this.GetTargeterTemplatePrefabToUse(gameObject2, TargeterTemplateSwapData.TargeterTemplateType.DynamicCone, templateSwapData);
		}
		if (gameObject2 != null)
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
			gameObject = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
			if (gameObject != null)
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
				UIDynamicCone component = gameObject.GetComponent<UIDynamicCone>();
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
					component.InitCone();
					component.SetForceHideSides(forceHideSides);
					this.AdjustDynamicConeMesh(gameObject, initialRadiusInSquares, initialAngle);
				}
			}
		}
		return gameObject;
	}

	public void AdjustDynamicConeMesh(GameObject highlight, float radiusInSquares, float angleDeg)
	{
		if (highlight != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.AdjustDynamicConeMesh(GameObject, float, float)).MethodHandle;
			}
			UIDynamicCone component = highlight.GetComponent<UIDynamicCone>();
			if (component != null)
			{
				component.AdjustConeMeshVertices(angleDeg, radiusInSquares * Board.Get().squareSize);
			}
		}
	}

	public void SetDynamicConeMeshBorderActive(GameObject highlight, bool borderActive)
	{
		if (highlight != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.SetDynamicConeMeshBorderActive(GameObject, bool)).MethodHandle;
			}
			UIDynamicCone component = highlight.GetComponent<UIDynamicCone>();
			if (component != null)
			{
				component.SetBorderActive(borderActive);
			}
		}
	}

	public GameObject CreateDynamicLineSegmentMesh(float lengthInSquares, float widthInWorld, bool dotted, Color color)
	{
		GameObject gameObject;
		if (this.m_dynamicLineSegmentPrefab != null)
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_dynamicLineSegmentPrefab);
			UIDynamicLineSegment component = gameObject.GetComponent<UIDynamicLineSegment>();
			if (component != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateDynamicLineSegmentMesh(float, float, bool, Color)).MethodHandle;
				}
				float lengthInWorld = lengthInSquares * Board.Get().squareSize;
				component.CreateSegmentMesh(widthInWorld, dotted, color);
				component.AdjustDynamicLineSegmentMesh(lengthInWorld, color);
			}
		}
		else
		{
			Log.Error("no dynamic line segment prefab", new object[0]);
			gameObject = new GameObject("Error_NoDynamicLineSegmentPrefab");
		}
		return gameObject;
	}

	public void AdjustDynamicLineSegmentMesh(GameObject highlight, float lengthInSquares, Color color)
	{
		if (highlight != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.AdjustDynamicLineSegmentMesh(GameObject, float, Color)).MethodHandle;
			}
			UIDynamicLineSegment component = highlight.GetComponent<UIDynamicLineSegment>();
			if (component != null)
			{
				component.AdjustDynamicLineSegmentMesh(lengthInSquares * Board.Get().squareSize, color);
			}
		}
	}

	public void AdjustDynamicLineSegmentLength(GameObject highlight, float lengthInSquares)
	{
		if (highlight != null)
		{
			UIDynamicLineSegment component = highlight.GetComponent<UIDynamicLineSegment>();
			if (component != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.AdjustDynamicLineSegmentLength(GameObject, float)).MethodHandle;
				}
				component.AdjustSegmentLength(lengthInSquares * Board.Get().squareSize);
			}
		}
	}

	public GameObject CreateBouncingLaserCursor(Vector3 originalStart, List<Vector3> laserAnglePoints, float width)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_bouncingLaserTargeterPrefab);
		UIBouncingLaserCursor component = gameObject.GetComponent<UIBouncingLaserCursor>();
		component.OnUpdated(originalStart, laserAnglePoints, width);
		return gameObject;
	}

	public GameObject CreateAoECursor(float radiusInWorld, bool isForLocalPlayer)
	{
		if (GameFlowData.Get().activeOwnedActorData == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateAoECursor(float, bool)).MethodHandle;
			}
			isForLocalPlayer = true;
		}
		GameObject gameObject;
		if (isForLocalPlayer)
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
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_AoECursorPrefab);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_AoECursorAllyPrefab);
		}
		float scale = radiusInWorld / this.m_AoECursorRadius;
		HighlightUtils.SetParticleSystemScale(gameObject, scale);
		return gameObject;
	}

	public GameObject CreateShapeCursor(AbilityAreaShape shape, bool isForLocalPlayer)
	{
		GameObject gameObject = null;
		if (GameFlowData.Get().activeOwnedActorData == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateShapeCursor(AbilityAreaShape, bool)).MethodHandle;
			}
			isForLocalPlayer = true;
		}
		foreach (HighlightUtils.AreaShapePrefab areaShapePrefab in this.m_areaShapePrefabs)
		{
			if (areaShapePrefab.m_shape == shape)
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
				gameObject = UnityEngine.Object.Instantiate<GameObject>(areaShapePrefab.m_prefab);
				break;
			}
		}
		if (gameObject == null)
		{
			float num = (float)shape;
			gameObject = this.CreateAoECursor((num + 1.5f) * 0.4f, isForLocalPlayer);
		}
		return gameObject;
	}

	public GameObject CreateBoundaryLine(float lengthInSquares, bool innerVersion, bool openingDirection)
	{
		GameObject gameObject;
		if (innerVersion)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateBoundaryLine(float, bool, bool)).MethodHandle;
			}
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_targeterBoundaryLineInner);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_targeterBoundaryLineOuter);
		}
		float x;
		if (openingDirection)
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
			x = 1f;
		}
		else
		{
			x = -1f;
		}
		float z = lengthInSquares * Board.Get().squareSize / this.m_targeterBoundaryLineLength;
		gameObject.transform.localScale = new Vector3(x, 1f, z);
		return gameObject;
	}

	public void ResizeBoundaryLine(float lengthInSquares, GameObject highlight)
	{
		float z = lengthInSquares * Board.Get().squareSize / this.m_targeterBoundaryLineLength;
		highlight.transform.localScale = new Vector3(highlight.transform.localScale.x, 1f, z);
	}

	public GameObject CreateGridPatternHighlight(AbilityGridPattern pattern, float scale)
	{
		GameObject gameObject = new GameObject("Targeter parent: " + pattern.ToString());
		List<GameObject> list;
		if (pattern != AbilityGridPattern.Plus_Two_x_Two)
		{
			if (pattern != AbilityGridPattern.Plus_Four_x_Four)
			{
				list = new List<GameObject>();
			}
			else
			{
				list = this.CreatePlusPatternObjects(scale * Board.Get().squareSize * 4f);
			}
		}
		else
		{
			list = this.CreatePlusPatternObjects(scale * Board.Get().squareSize * 2f);
		}
		using (List<GameObject>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject gameObject2 = enumerator.Current;
				gameObject2.transform.parent = gameObject.transform;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateGridPatternHighlight(AbilityGridPattern, float)).MethodHandle;
			}
		}
		return gameObject;
	}

	private List<GameObject> CreatePlusPatternObjects(float scale)
	{
		List<GameObject> list = new List<GameObject>();
		list.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, true));
		list.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, false));
		List<GameObject> list2 = new List<GameObject>();
		list2.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, true));
		list2.Add(HighlightUtils.Get().CreateBoundaryLine(1f, false, false));
		Vector3 vector = new Vector3(1f, 0f, 0f);
		Vector3 vector2 = new Vector3(0f, 0f, 1f);
		List<GameObject> list3 = new List<GameObject>();
		list3.AddRange(list);
		list3.AddRange(list2);
		using (List<GameObject>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameObject gameObject = enumerator.Current;
				gameObject.transform.rotation = Quaternion.LookRotation(vector, Vector3.up);
				gameObject.transform.localPosition += 0.5f * scale * vector;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreatePlusPatternObjects(float)).MethodHandle;
			}
		}
		foreach (GameObject gameObject2 in list2)
		{
			gameObject2.transform.rotation = Quaternion.LookRotation(vector2, Vector3.up);
			gameObject2.transform.localPosition += 0.5f * scale * vector2;
		}
		using (List<GameObject>.Enumerator enumerator3 = list3.GetEnumerator())
		{
			while (enumerator3.MoveNext())
			{
				GameObject highlight = enumerator3.Current;
				HighlightUtils.Get().ResizeBoundaryLine(scale / Board.Get().squareSize, highlight);
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
		return list3;
	}

	public GameObject CreateAoEPersistentVFX(float radius, Team team, Vector3 position)
	{
		GameObject gameObject = null;
		if (team == Team.TeamA)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateAoEPersistentVFX(float, Team, Vector3)).MethodHandle;
			}
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_teamAPersistentAoEPrefab);
		}
		else if (team == Team.TeamB)
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
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_teamBPersistentAoEPrefab);
		}
		else
		{
			Log.Error("Someone not on team A or team B doing a firebomb, which has no appropriate VFX.", new object[0]);
		}
		if (gameObject != null)
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
			float scale = radius / this.m_AoECursorRadius;
			HighlightUtils.SetParticleSystemScale(gameObject, scale);
			gameObject.transform.position = position;
		}
		return gameObject;
	}

	public GameObject CreateAoEPersistentVFX(float radius, bool allied, bool helpful, Vector3 position)
	{
		GameObject gameObject;
		if (allied)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateAoEPersistentVFX(float, bool, bool, Vector3)).MethodHandle;
			}
			if (helpful)
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
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_allyPersistentHelpfulAoEPrefab);
			}
			else
			{
				gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_allyPersistentAoEPrefab);
			}
		}
		else if (helpful)
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
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_enemyPersistentHelpfulAoEPrefab);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_enemyPersistentAoEPrefab);
		}
		if (gameObject != null)
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
			float scale = radius / this.m_AoECursorRadius;
			HighlightUtils.SetParticleSystemScale(gameObject, scale);
			gameObject.transform.position = position;
		}
		return gameObject;
	}

	public HighlightUtils.CursorType GetCurrentCursorType()
	{
		return this.m_currentCursorType;
	}

	public void SetCursorType(HighlightUtils.CursorType cursorType)
	{
		this.m_currentCursorType = cursorType;
		this.RefreshCursorVisibility();
	}

	private bool ShouldShowSprintingCursor()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		ActorMovement actorMovement;
		if (activeOwnedActorData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.ShouldShowSprintingCursor()).MethodHandle;
			}
			actorMovement = activeOwnedActorData.GetActorMovement();
		}
		else
		{
			actorMovement = null;
		}
		ActorMovement actorMovement2 = actorMovement;
		ActorTurnSM actorTurnSM;
		if (activeOwnedActorData)
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
			actorTurnSM = activeOwnedActorData.GetActorTurnSM();
		}
		else
		{
			actorTurnSM = null;
		}
		ActorTurnSM actorTurnSM2 = actorTurnSM;
		if (!(activeOwnedActorData == null))
		{
			if (actorMovement2 == null)
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
				if (actorMovement2.SquaresCanMoveToWithQueuedAbility.Contains(Board.Get().PlayerClampedSquare))
				{
					return false;
				}
				if (activeOwnedActorData.RemainingHorizontalMovement <= 0f)
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
					return false;
				}
				if (activeOwnedActorData.MoveFromBoardSquare == Board.Get().PlayerClampedSquare)
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
					return false;
				}
				if (!actorTurnSM2.AmStillDeciding())
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
					return false;
				}
				if (activeOwnedActorData.IsDead())
				{
					return false;
				}
				if (actorTurnSM2.CurrentState == TurnStateEnum.PICKING_RESPAWN)
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
				if (!activeOwnedActorData.GetAbilityData().GetQueuedAbilitiesAllowSprinting())
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
				if (UITutorialFullscreenPanel.Get().IsAnyPanelVisible())
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
				if (UIGameStatsWindow.Get().m_container.gameObject.activeSelf)
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
					return false;
				}
				return true;
			}
		}
		return false;
	}

	private void RefreshCursorVisibility()
	{
		if (this.m_startCalled)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.RefreshCursorVisibility()).MethodHandle;
			}
			if (!(Board.Get() == null) && !(GameFlowData.Get() == null))
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
				if (!(HUD_UI.Get() == null))
				{
					ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
					ActorMovement actorMovement;
					if (activeOwnedActorData)
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
						actorMovement = activeOwnedActorData.GetActorMovement();
					}
					else
					{
						actorMovement = null;
					}
					ActorMovement exists = actorMovement;
					bool flag = ActorTurnSM.IsClientDecidingMovement();
					bool flag2;
					if (flag)
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
						flag2 = ActorData.WouldSquareBeChasedByClient(Board.Get().PlayerFreeSquare, false);
					}
					else
					{
						flag2 = false;
					}
					bool flag3 = flag2;
					bool flag4;
					if (Board.Get().PlayerClampedSquare != null)
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
						flag4 = Board.Get().PlayerClampedSquare.IsBaselineHeight();
					}
					else
					{
						flag4 = false;
					}
					bool flag5 = flag4;
					bool flag6 = true;
					bool flag7 = true;
					bool flag8 = false;
					bool flag9 = false;
					bool flag10 = false;
					bool flag11 = false;
					bool flag12 = false;
					bool flag13 = false;
					bool flag14 = false;
					bool flag15 = false;
					if (GameFlowData.Get().gameState != GameState.EndingGame)
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
						if (GameFlowData.Get().gameState != GameState.Deployment)
						{
							goto IL_148;
						}
					}
					flag6 = false;
					flag7 = false;
					flag3 = false;
					flag5 = false;
					flag14 = false;
					flag15 = false;
					IL_148:
					if (!this.HideCursor)
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
						if (UIUtils.IsMouseOnGUI())
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
						}
						else
						{
							switch (this.m_currentCursorType)
							{
							case HighlightUtils.CursorType.MouseOverCursorType:
								flag6 = false;
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
										flag11 = true;
										goto IL_21D;
									}
								}
								flag11 = false;
								IL_21D:
								if (flag3)
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
									flag14 = true;
								}
								else
								{
									flag14 = false;
								}
								if (Board.Get().PlayerFreeSquare != null)
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
									flag7 = true;
								}
								else
								{
									flag7 = false;
								}
								flag12 = false;
								if (exists)
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
									if (this.ShouldShowSprintingCursor())
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
										flag13 = true;
									}
									if (activeOwnedActorData.RemainingHorizontalMovement != 0f)
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
										if (activeOwnedActorData.GetActorTurnSM().AmStillDeciding())
										{
											break;
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
									flag11 = false;
								}
								break;
							case HighlightUtils.CursorType.TabTargetingCursorType:
								flag6 = true;
								flag11 = false;
								flag7 = false;
								flag12 = false;
								flag14 = false;
								break;
							case HighlightUtils.CursorType.ShapeTargetingSquareCursorType:
								flag6 = false;
								flag11 = false;
								flag7 = true;
								flag12 = false;
								flag14 = false;
								break;
							case HighlightUtils.CursorType.ShapeTargetingCornerCursorType:
								flag6 = false;
								flag11 = false;
								flag7 = false;
								flag12 = true;
								flag14 = false;
								break;
							case HighlightUtils.CursorType.NoCursorType:
								flag6 = false;
								flag11 = false;
								flag7 = false;
								flag12 = false;
								flag14 = false;
								break;
							}
							if (!(activeOwnedActorData != null))
							{
								goto IL_367;
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
							ActorCover actorCover = activeOwnedActorData.GetActorCover();
							if (!(actorCover != null))
							{
								goto IL_367;
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
							if (this.m_currentCursorType == HighlightUtils.CursorType.MouseOverCursorType)
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
								actorCover.UpdateCoverHighlights(Board.Get().PlayerFreeSquare);
								goto IL_367;
							}
							actorCover.UpdateCoverHighlights(null);
							goto IL_367;
						}
					}
					flag6 = false;
					flag11 = false;
					flag7 = false;
					flag12 = false;
					flag14 = false;
					flag15 = false;
					if (GameFlowData.Get().activeOwnedActorData != null)
					{
						ActorCover actorCover2 = GameFlowData.Get().activeOwnedActorData.GetActorCover();
						if (actorCover2 != null)
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
							actorCover2.UpdateCoverHighlights(null);
						}
					}
					IL_367:
					if (activeOwnedActorData != null)
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
						if (activeOwnedActorData.GetAbilityData().GetSelectedAbility() != null)
						{
							if (activeOwnedActorData.GetAbilityData().GetSelectedAbility().Targeter is AbilityUtil_Targeter_Shape)
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
								flag7 = false;
								flag8 = true;
								flag9 = false;
								flag10 = false;
								flag13 = false;
								flag14 = false;
							}
							else
							{
								flag7 = false;
								flag8 = false;
								flag9 = false;
								flag10 = false;
								flag13 = false;
								flag14 = false;
							}
						}
						else if (activeOwnedActorData.GetActorTurnSM().CurrentState == TurnStateEnum.PICKING_RESPAWN)
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
							flag15 = true;
							flag7 = false;
							flag8 = false;
							flag9 = false;
							flag10 = false;
							flag13 = false;
						}
						else
						{
							if (activeOwnedActorData.RemainingHorizontalMovement <= 0f)
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
								if (activeOwnedActorData.HasQueuedChase())
								{
									flag7 = false;
									flag8 = false;
									flag9 = false;
									flag10 = true;
									flag13 = false;
									goto IL_452;
								}
							}
							flag7 = false;
							flag8 = false;
							flag9 = true;
							flag10 = false;
						}
						IL_452:
						if (flag5)
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
								flag7 = false;
								flag8 = false;
								flag9 = false;
								flag10 = false;
								flag13 = false;
								flag14 = true;
							}
						}
					}
					if (!flag12)
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
						if (!flag11)
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
							flag7 = false;
							flag8 = false;
							flag9 = false;
							flag10 = false;
							flag13 = false;
						}
					}
					UIManager.SetGameObjectActive(this.MovementMouseOverCursor, flag9, null);
					UIManager.SetGameObjectActive(this.AbilityTargetMouseOverCursor, flag8, null);
					UIManager.SetGameObjectActive(this.IdleMouseOverCursor, flag10, null);
					UIManager.SetGameObjectActive(this.m_targetingCursor, flag6, null);
					UIManager.SetGameObjectActive(this.FreeMouseOverCursor, flag7, null);
					UIManager.SetGameObjectActive(this.ClampedMouseOverCursor, flag11, null);
					UIManager.SetGameObjectActive(this.CornerMouseOverCursor, flag12, null);
					if (this.ChaseSquareCursor != null)
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
						UIManager.SetGameObjectActive(this.ChaseSquareCursor, flag14, null);
					}
					if (this.ChaseSquareCursorAlt != null)
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
						UIManager.SetGameObjectActive(this.ChaseSquareCursorAlt, flag14, null);
					}
					if (this.SprintMouseOverCursor != null)
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
						bool flag16;
						if (!(UIScreenManager.Get() == null))
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
							flag16 = UIScreenManager.Get().GetHideHUDCompletely();
						}
						else
						{
							flag16 = true;
						}
						bool flag17 = flag16;
						GameObject sprintMouseOverCursor = this.SprintMouseOverCursor;
						bool doActive;
						if (flag13)
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
							doActive = !flag17;
						}
						else
						{
							doActive = false;
						}
						UIManager.SetGameObjectActive(sprintMouseOverCursor, doActive, null);
					}
					if (this.RespawnSelectionCursor != null)
					{
						UIManager.SetGameObjectActive(this.RespawnSelectionCursor, flag15, null);
					}
					if (ActorDebugUtils.Get() != null)
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
						if (ActorDebugUtils.Get().ShowingCategory(ActorDebugUtils.DebugCategory.CursorState, true))
						{
							ActorDebugUtils.DebugCategoryInfo debugCategoryInfo = ActorDebugUtils.Get().GetDebugCategoryInfo(ActorDebugUtils.DebugCategory.CursorState);
							string stringToDisplay = string.Format("CursorType: " + this.m_currentCursorType.ToString() + "\n{0} \tMovementMouseOverCursor \n{1} \tAbilityTargetMouseOverCursor \n{2} \tIdleMouseOverCursor \n{3} \tm_targetingCursor \n{4} \tFreeMouseOverCursor \n{5} \tClampedMouseOverCursor \n{6} \tCornerMouseOverCursor\n{7} \tshowChaseCursor \n{8} \tshowSprintCursor \n{9} \tchaseMouseover \n{10} \trespawnCursor \n", new object[]
							{
								flag9,
								flag8,
								flag10,
								flag6,
								flag7,
								flag11,
								flag12,
								flag14,
								flag13,
								flag3,
								flag15
							});
							debugCategoryInfo.m_stringToDisplay = stringToDisplay;
						}
					}
					return;
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

	public void UpdateCursorPositions()
	{
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		Vector3 vector3 = Board.Get().PlayerFreeCornerPos;
		if (Board.Get().PlayerClampedSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.UpdateCursorPositions()).MethodHandle;
			}
			vector = Board.Get().PlayerClampedSquare.ToVector3();
			if (Board.Get().PlayerClampedSquare.height < 0)
			{
				vector.y = (float)Board.Get().BaselineHeight;
			}
		}
		if (Board.Get().PlayerFreeSquare != null)
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
			vector2 = Board.Get().PlayerFreeSquare.ToVector3();
			if (Board.Get().PlayerFreeSquare.height < 0)
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
				vector2.y = (float)Board.Get().BaselineHeight;
			}
		}
		if (this.RespawnSelectionCursor != null)
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
			this.RespawnSelectionCursor.transform.position = vector;
		}
		Vector3 b = new Vector3(0f, 0.1f, 0f);
		vector += b;
		vector2 += b;
		vector3 += b;
		this.ClampedMouseOverCursor.transform.position = vector;
		this.FreeMouseOverCursor.transform.position = vector2;
		this.CornerMouseOverCursor.transform.position = vector3;
		this.ChaseSquareCursor.transform.position = vector2;
		this.ChaseSquareCursorAlt.transform.position = vector2;
		this.m_targetingCursor.transform.position = vector;
		this.MovementMouseOverCursor.transform.position = vector;
		this.AbilityTargetMouseOverCursor.transform.position = vector;
		this.IdleMouseOverCursor.transform.position = vector;
		Canvas canvas;
		if (HUD_UI.Get() != null)
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
			canvas = HUD_UI.Get().GetTopLevelCanvas();
		}
		else
		{
			canvas = null;
		}
		Canvas canvas2 = canvas;
		if (canvas2 != null)
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
			if (this.m_sprintHighlightPrefab != null)
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
				if (this.SprintMouseOverCursor == null)
				{
					this.SprintMouseOverCursor = UnityEngine.Object.Instantiate<GameObject>(this.m_sprintHighlightPrefab);
					UIManager.SetGameObjectActive(this.SprintMouseOverCursor, false, null);
					this.SprintMouseOverCursor.transform.SetParent(canvas2.transform, false);
					this.SprintMouseOverCursor.GetComponent<Canvas>().sortingOrder += this.m_sprintHighlightPrefab.GetComponent<Canvas>().sortingOrder;
				}
			}
			if (this.SprintMouseOverCursor != null)
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
				RectTransform rectTransform = canvas2.transform as RectTransform;
				Vector2 vector4 = Camera.main.WorldToViewportPoint(vector);
				Vector2 v = new Vector2(vector4.x * rectTransform.sizeDelta.x, vector4.y * rectTransform.sizeDelta.y);
				(this.SprintMouseOverCursor.transform as RectTransform).anchoredPosition3D = v;
				this.SprintMouseOverCursor.transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		this.RefreshCursorVisibility();
	}

	private unsafe void ProcessUpperLeftCorner(BoardSquare square, HashSet<BoardSquare> squaresSet, HashSet<BoardSquare> borderSet, ref HighlightUtils.HighlightMesh highlighMesh, bool includeInternalBorders)
	{
		Board board = Board.Get();
		BoardSquare boardSquare = board.GetBoardSquare(square.x, square.y + 1);
		bool flag = this.IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetBoardSquare(square.x - 1, square.y);
		bool flag2 = this.IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetBoardSquare(square.x - 1, square.y + 1);
		bool flag3 = this.IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		if (!flag && !flag2)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.ProcessUpperLeftCorner(BoardSquare, HashSet<BoardSquare>, HashSet<BoardSquare>, HighlightUtils.HighlightMesh*, bool)).MethodHandle;
			}
			highlighMesh.AddPiece(HighlightUtils.PieceType.UpperLeftOuterCorner, square);
		}
		else
		{
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
				if (flag2)
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
					highlighMesh.AddPiece(HighlightUtils.PieceType.UpperLeftHorizontal, square);
					return;
				}
			}
			if (flag)
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
				if (!flag2)
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
					highlighMesh.AddPiece(HighlightUtils.PieceType.UpperLeftVertical, square);
					return;
				}
			}
			if (flag && flag2)
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
					highlighMesh.AddPiece(HighlightUtils.PieceType.UpperLeftInnerCorner, square);
				}
			}
		}
	}

	private unsafe void ProcessUpperRightCorner(BoardSquare square, HashSet<BoardSquare> squaresSet, HashSet<BoardSquare> borderSet, ref HighlightUtils.HighlightMesh highlighMesh, bool includeInternalBorders)
	{
		Board board = Board.Get();
		BoardSquare boardSquare = board.GetBoardSquare(square.x, square.y + 1);
		bool flag = this.IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetBoardSquare(square.x + 1, square.y);
		bool flag2 = this.IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetBoardSquare(square.x + 1, square.y + 1);
		bool flag3 = this.IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		if (!flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.ProcessUpperRightCorner(BoardSquare, HashSet<BoardSquare>, HashSet<BoardSquare>, HighlightUtils.HighlightMesh*, bool)).MethodHandle;
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
				highlighMesh.AddPiece(HighlightUtils.PieceType.UpperRightOuterCorner, square);
				return;
			}
		}
		if (!flag)
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
			if (flag2)
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
				highlighMesh.AddPiece(HighlightUtils.PieceType.UpperRightHorizontal, square);
				return;
			}
		}
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
				highlighMesh.AddPiece(HighlightUtils.PieceType.UpperRightVertical, square);
				return;
			}
		}
		if (flag && flag2)
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
			if (!flag3)
			{
				highlighMesh.AddPiece(HighlightUtils.PieceType.UpperRightInnerCorner, square);
			}
		}
	}

	private unsafe void ProcessLowerLeftCorner(BoardSquare square, HashSet<BoardSquare> squaresSet, HashSet<BoardSquare> borderSet, ref HighlightUtils.HighlightMesh highlighMesh, bool includeInternalBorders)
	{
		Board board = Board.Get();
		BoardSquare boardSquare = board.GetBoardSquare(square.x, square.y - 1);
		bool flag = this.IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetBoardSquare(square.x - 1, square.y);
		bool flag2 = this.IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetBoardSquare(square.x - 1, square.y - 1);
		bool flag3 = this.IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.ProcessLowerLeftCorner(BoardSquare, HashSet<BoardSquare>, HashSet<BoardSquare>, HighlightUtils.HighlightMesh*, bool)).MethodHandle;
			}
			if (!flag2)
			{
				highlighMesh.AddPiece(HighlightUtils.PieceType.LowerLeftOuterCorner, square);
				return;
			}
		}
		if (!flag && flag2)
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
			highlighMesh.AddPiece(HighlightUtils.PieceType.LowerLeftHorizontal, square);
		}
		else
		{
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
					highlighMesh.AddPiece(HighlightUtils.PieceType.LowerLeftVertical, square);
					return;
				}
			}
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
				if (flag2)
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
					if (!flag3)
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
						highlighMesh.AddPiece(HighlightUtils.PieceType.LowerLeftInnerCorner, square);
					}
				}
			}
		}
	}

	private unsafe void ProcessLowerRightCorner(BoardSquare square, HashSet<BoardSquare> squaresSet, HashSet<BoardSquare> borderSet, ref HighlightUtils.HighlightMesh highlighMesh, bool includeInternalBorders)
	{
		Board board = Board.Get();
		BoardSquare boardSquare = board.GetBoardSquare(square.x, square.y - 1);
		bool flag = this.IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetBoardSquare(square.x + 1, square.y);
		bool flag2 = this.IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		boardSquare = board.GetBoardSquare(square.x + 1, square.y - 1);
		bool flag3 = this.IsSquareConnected(square, boardSquare, squaresSet, borderSet, includeInternalBorders);
		if (!flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.ProcessLowerRightCorner(BoardSquare, HashSet<BoardSquare>, HashSet<BoardSquare>, HighlightUtils.HighlightMesh*, bool)).MethodHandle;
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
				highlighMesh.AddPiece(HighlightUtils.PieceType.LowerRightOuterCorner, square);
				return;
			}
		}
		if (!flag)
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
			if (flag2)
			{
				highlighMesh.AddPiece(HighlightUtils.PieceType.LowerRightHorizontal, square);
				return;
			}
		}
		if (flag && !flag2)
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
			highlighMesh.AddPiece(HighlightUtils.PieceType.LowerRightVertical, square);
		}
		else if (flag)
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
			if (flag2 && !flag3)
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
				highlighMesh.AddPiece(HighlightUtils.PieceType.LowerRightInnerCorner, square);
			}
		}
	}

	private bool IsSquareOnSameSideOfAnyBorder(BoardSquare square, BoardSquare testSquare, HashSet<BoardSquare> squaresSet)
	{
		bool result;
		if (!squaresSet.Contains(testSquare))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.IsSquareOnSameSideOfAnyBorder(BoardSquare, BoardSquare, HashSet<BoardSquare>)).MethodHandle;
			}
			result = !squaresSet.Contains(square);
		}
		else
		{
			result = true;
		}
		return result;
	}

	private bool IsSquareConnected(BoardSquare square, BoardSquare testSquare, HashSet<BoardSquare> squaresSet, HashSet<BoardSquare> borderSet, bool includeInternalBorders)
	{
		if (includeInternalBorders)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.IsSquareConnected(BoardSquare, BoardSquare, HashSet<BoardSquare>, HashSet<BoardSquare>, bool)).MethodHandle;
			}
			return this.IsSquareOnSameSideOfAnyBorder(square, testSquare, squaresSet);
		}
		bool result;
		if (!squaresSet.Contains(testSquare))
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
			result = !borderSet.Contains(testSquare);
		}
		else
		{
			result = true;
		}
		return result;
	}

	private bool BoardSquareIsInSet(int x, int y, HashSet<BoardSquare> squaresSet)
	{
		BoardSquare boardSquare = Board.Get().GetBoardSquare(x, y);
		return squaresSet.Contains(boardSquare);
	}

	private bool IsSquareSurrounded(BoardSquare square, HashSet<BoardSquare> squaresSet)
	{
		if (this.BoardSquareIsInSet(square.x - 1, square.y - 1, squaresSet) && this.BoardSquareIsInSet(square.x, square.y - 1, squaresSet))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.IsSquareSurrounded(BoardSquare, HashSet<BoardSquare>)).MethodHandle;
			}
			if (this.BoardSquareIsInSet(square.x + 1, square.y - 1, squaresSet))
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
				if (this.BoardSquareIsInSet(square.x - 1, square.y, squaresSet))
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
					if (this.BoardSquareIsInSet(square.x + 1, square.y, squaresSet))
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
						if (this.BoardSquareIsInSet(square.x - 1, square.y + 1, squaresSet))
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
							if (this.BoardSquareIsInSet(square.x, square.y + 1, squaresSet))
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
								return this.BoardSquareIsInSet(square.x + 1, square.y + 1, squaresSet);
							}
						}
					}
				}
			}
		}
		return false;
	}

	private HashSet<BoardSquare> CalculateOuterBorder(HashSet<BoardSquare> squaresSet, HashSet<BoardSquare> additionalFloodFillSquares)
	{
		HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
		int num = int.MaxValue;
		int num2 = int.MinValue;
		int num3 = int.MaxValue;
		int num4 = int.MinValue;
		using (HashSet<BoardSquare>.Enumerator enumerator = squaresSet.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				BoardSquare boardSquare = enumerator.Current;
				if (boardSquare.x < num)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CalculateOuterBorder(HashSet<BoardSquare>, HashSet<BoardSquare>)).MethodHandle;
					}
					num = boardSquare.x;
				}
				if (boardSquare.x > num2)
				{
					num2 = boardSquare.x;
				}
				if (boardSquare.y < num3)
				{
					num3 = boardSquare.y;
				}
				if (boardSquare.y > num4)
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
					num4 = boardSquare.y;
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
		num--;
		num2++;
		num3--;
		num4++;
		Board board = Board.Get();
		for (int i = num; i <= num2; i++)
		{
			BoardSquare boardSquare2 = board.GetBoardSquare(i, num3);
			if (boardSquare2 != null)
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
				if (!squaresSet.Contains(boardSquare2))
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
					hashSet.Add(boardSquare2);
				}
			}
			BoardSquare boardSquare3 = board.GetBoardSquare(i, num4);
			if (boardSquare3 != null)
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
				if (!squaresSet.Contains(boardSquare3))
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
					hashSet.Add(boardSquare3);
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
		for (int j = num3; j <= num4; j++)
		{
			BoardSquare boardSquare4 = board.GetBoardSquare(num, j);
			if (boardSquare4 != null)
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
				if (!squaresSet.Contains(boardSquare4))
				{
					hashSet.Add(boardSquare4);
				}
			}
			BoardSquare boardSquare5 = board.GetBoardSquare(num2, j);
			if (boardSquare5 != null)
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
				if (!squaresSet.Contains(boardSquare5))
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
					hashSet.Add(boardSquare5);
				}
			}
		}
		List<BoardSquare> list = new List<BoardSquare>();
		using (HashSet<BoardSquare>.Enumerator enumerator2 = hashSet.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				BoardSquare item = enumerator2.Current;
				list.Add(item);
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
		if (additionalFloodFillSquares != null)
		{
			foreach (BoardSquare item2 in additionalFloodFillSquares)
			{
				if (!list.Contains(item2))
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
					list.Add(item2);
				}
			}
		}
		while (list.Count > 0)
		{
			BoardSquare boardSquare6 = list[0];
			list.RemoveAt(0);
			for (int k = -1; k < 2; k++)
			{
				for (int l = -1; l < 2; l++)
				{
					int num5 = boardSquare6.x + k;
					int num6 = boardSquare6.y + l;
					if (num5 <= num2)
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
						if (num5 >= num)
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
							if (num6 <= num4)
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
								if (num6 >= num3)
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
									BoardSquare boardSquare7 = board.GetBoardSquare(num5, num6);
									if (boardSquare7 != null)
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
										if (!squaresSet.Contains(boardSquare7))
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
											if (!hashSet.Contains(boardSquare7))
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
												list.Add(boardSquare7);
												hashSet.Add(boardSquare7);
											}
										}
									}
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
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		return hashSet;
	}

	public GameObject CreateBoundaryHighlight(HashSet<BoardSquare> squaresSet, Color borderColor, bool dotted = false, HashSet<BoardSquare> additionalFloodFillSquares = null, bool includeInternalBorders = false)
	{
		Material material;
		if (dotted)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateBoundaryHighlight(HashSet<BoardSquare>, Color, bool, HashSet<BoardSquare>, bool)).MethodHandle;
			}
			material = this.m_dottedHighlightMaterial;
		}
		else
		{
			material = this.m_highlightMaterial;
		}
		Material borderMaterial = material;
		GameObject gameObject = null;
		HighlightUtils.HighlightMesh highlightMesh = new HighlightUtils.HighlightMesh();
		if (squaresSet.Count > 0)
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
			HashSet<BoardSquare> borderSet = null;
			if (!includeInternalBorders)
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
				borderSet = this.CalculateOuterBorder(squaresSet, additionalFloodFillSquares);
			}
			this.m_surroundedSquareIndicators.ResetNextIndicatorIndex();
			foreach (BoardSquare square in squaresSet)
			{
				if (!this.IsSquareSurrounded(square, squaresSet))
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
					this.ProcessUpperLeftCorner(square, squaresSet, borderSet, ref highlightMesh, includeInternalBorders);
					this.ProcessUpperRightCorner(square, squaresSet, borderSet, ref highlightMesh, includeInternalBorders);
					this.ProcessLowerLeftCorner(square, squaresSet, borderSet, ref highlightMesh, includeInternalBorders);
					this.ProcessLowerRightCorner(square, squaresSet, borderSet, ref highlightMesh, includeInternalBorders);
				}
				if (includeInternalBorders)
				{
					this.m_surroundedSquareIndicators.ShowIndicatorForSquare(square);
				}
			}
			this.m_surroundedSquareIndicators.HideAllSquareIndicators(this.m_surroundedSquareIndicators.GetNextIndicatorIndex());
			gameObject = highlightMesh.CreateMeshObject(borderMaterial);
			borderColor.a = 1f;
			gameObject.GetComponent<Renderer>().material.SetColor("_TintColor", borderColor);
		}
		return gameObject;
	}

	public GameObject CreateBoundaryHighlight(List<BoardSquare> squares, Color borderColor, bool includeInternalBorders = false)
	{
		HashSet<BoardSquare> squaresSet = new HashSet<BoardSquare>(squares);
		return this.CreateBoundaryHighlight(squaresSet, borderColor, false, null, includeInternalBorders);
	}

	public static void AddPieceTypeToMask(ref int mask, HighlightUtils.PieceType pieceType)
	{
		mask |= 1 << (int)pieceType;
	}

	public static bool IsPieceTypeInMask(int mask, HighlightUtils.PieceType pieceType)
	{
		return (mask & 1 << (int)pieceType) != 0;
	}

	private Vector2[] HorizontalFlip(Vector2[] uvs)
	{
		Vector2[] array = new Vector2[]
		{
			uvs[3],
			default(Vector2),
			default(Vector2),
			uvs[0]
		};
		array[1] = uvs[2];
		array[2] = uvs[1];
		return array;
	}

	private Vector2[] VerticalFlip(Vector2[] uvs)
	{
		Vector2[] array = new Vector2[4];
		array[0] = uvs[2];
		array[2] = uvs[0];
		array[1] = uvs[3];
		array[3] = uvs[1];
		return array;
	}

	private void InitializeHighlightPieces()
	{
		for (int i = 0; i < this.m_highlightPieces.Length; i++)
		{
			this.m_highlightPieces[i] = new HighlightUtils.HighlightPiece();
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.InitializeHighlightPieces()).MethodHandle;
		}
		Vector3[] pos = new Vector3[]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(0.75f, 0f, -0.75f),
			new Vector3(0f, 0f, -0.75f),
			new Vector3(0.75f, 0f, 0f)
		};
		this.m_highlightPieces[0].m_pos = pos;
		this.m_highlightPieces[1].m_pos = pos;
		this.m_highlightPieces[2].m_pos = pos;
		this.m_highlightPieces[3].m_pos = pos;
		Vector3[] pos2 = new Vector3[]
		{
			new Vector3(0f, 0f, 0.75f),
			new Vector3(0.75f, 0f, 0f),
			new Vector3(0f, 0f, 0f),
			new Vector3(0.75f, 0f, 0.75f)
		};
		this.m_highlightPieces[4].m_pos = pos2;
		this.m_highlightPieces[5].m_pos = pos2;
		this.m_highlightPieces[6].m_pos = pos2;
		this.m_highlightPieces[7].m_pos = pos2;
		Vector3[] pos3 = new Vector3[]
		{
			new Vector3(-0.75f, 0f, 0.75f),
			new Vector3(0f, 0f, 0f),
			new Vector3(-0.75f, 0f, 0f),
			new Vector3(0f, 0f, 0.75f)
		};
		this.m_highlightPieces[8].m_pos = pos3;
		this.m_highlightPieces[9].m_pos = pos3;
		this.m_highlightPieces[0xA].m_pos = pos3;
		this.m_highlightPieces[0xB].m_pos = pos3;
		Vector3[] pos4 = new Vector3[]
		{
			new Vector3(-0.75f, 0f, 0f),
			new Vector3(0f, 0f, -0.75f),
			new Vector3(-0.75f, 0f, -0.75f),
			new Vector3(0f, 0f, 0f)
		};
		this.m_highlightPieces[0xC].m_pos = pos4;
		this.m_highlightPieces[0xD].m_pos = pos4;
		this.m_highlightPieces[0xE].m_pos = pos4;
		this.m_highlightPieces[0xF].m_pos = pos4;
		Vector2[] array = new Vector2[]
		{
			new Vector2(0.333333343f, 0f),
			new Vector2(0.6666667f, 1f),
			new Vector2(0.333333343f, 1f),
			new Vector2(0.6666667f, 0f)
		};
		this.m_highlightPieces[1].m_uv = array;
		this.m_highlightPieces[5].m_uv = this.VerticalFlip(array);
		this.m_highlightPieces[9].m_uv = this.VerticalFlip(array);
		this.m_highlightPieces[0xD].m_uv = array;
		Vector2[] array2 = new Vector2[]
		{
			new Vector2(0.333333343f, 0f),
			new Vector2(0.6666667f, 1f),
			new Vector2(0.6666667f, 0f),
			new Vector2(0.333333343f, 1f)
		};
		this.m_highlightPieces[0].m_uv = array2;
		this.m_highlightPieces[4].m_uv = array2;
		this.m_highlightPieces[8].m_uv = this.HorizontalFlip(array2);
		this.m_highlightPieces[0xC].m_uv = this.HorizontalFlip(array2);
		Vector2[] array3 = new Vector2[]
		{
			new Vector2(1f, 0f),
			new Vector2(0.6666667f, 1f),
			new Vector2(1f, 1f),
			new Vector2(0.6666667f, 0f)
		};
		this.m_highlightPieces[2].m_uv = array3;
		this.m_highlightPieces[6].m_uv = this.VerticalFlip(array3);
		this.m_highlightPieces[0xA].m_uv = this.HorizontalFlip(this.VerticalFlip(array3));
		this.m_highlightPieces[0xE].m_uv = this.HorizontalFlip(array3);
		Vector2[] array4 = new Vector2[]
		{
			new Vector2(0.333333343f, 0f),
			new Vector2(0f, 1f),
			new Vector2(0.333333343f, 1f),
			new Vector2(0f, 0f)
		};
		this.m_highlightPieces[3].m_uv = array4;
		this.m_highlightPieces[7].m_uv = this.VerticalFlip(array4);
		this.m_highlightPieces[0xB].m_uv = this.HorizontalFlip(this.VerticalFlip(array4));
		this.m_highlightPieces[0xF].m_uv = this.HorizontalFlip(array4);
	}

	public static float GetHighlightHeight()
	{
		return (float)Board.Get().BaselineHeight + 0.1f;
	}

	public void SetScrollCursor(HighlightUtils.ScrollCursorDirection direction)
	{
		if (Application.isEditor && CameraControls.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.SetScrollCursor(HighlightUtils.ScrollCursorDirection)).MethodHandle;
			}
			if (!CameraControls.Get().m_mouseMoveFringeInEditor)
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
				return;
			}
		}
		if (this.m_scrollCursorEdge)
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
			if (direction == HighlightUtils.ScrollCursorDirection.Undefined)
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
				Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
				this.m_isCursorSet = false;
			}
			else if (this.ScrollCursors.ContainsKey(direction))
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
				Texture2D texture2D = this.ScrollCursors[direction];
				if (texture2D != null)
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
					Vector2 vector;
					if (this.ScrollCursorsHotspot.ContainsKey(direction))
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
						vector = this.ScrollCursorsHotspot[direction];
					}
					else
					{
						vector = new Vector2((float)(texture2D.width / 2), (float)(texture2D.height / 2));
					}
					Vector2 hotspot = vector;
					Cursor.SetCursor(texture2D, hotspot, CursorMode.Auto);
					this.m_isCursorSet = true;
				}
			}
		}
	}

	public void ResetCursor()
	{
		if (this.m_isCursorSet)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.ResetCursor()).MethodHandle;
			}
			Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
			this.m_isCursorSet = false;
		}
	}

	private static GameObject CreateSurroundedSquareObject()
	{
		if (HighlightUtils.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateSurroundedSquareObject()).MethodHandle;
			}
			if (HighlightUtils.s_instance.m_surroundedSquaresParent == null)
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
				HighlightUtils.s_instance.m_surroundedSquaresParent = new GameObject("SurroundedSquaresParent");
			}
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(HighlightUtils.Get().m_highlightFloodFillSquare);
		gameObject.transform.localScale *= Board.Get().squareSize / 1.5f;
		if (HighlightUtils.s_instance != null)
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
			gameObject.transform.parent = HighlightUtils.s_instance.m_surroundedSquaresParent.transform;
		}
		return gameObject;
	}

	private static GameObject CreateHiddenSquareIndicatorObject()
	{
		if (HighlightUtils.s_instance == null)
		{
			return null;
		}
		if (HighlightUtils.s_instance.m_hiddenSquareIndicatorParent == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateHiddenSquareIndicatorObject()).MethodHandle;
			}
			HighlightUtils.s_instance.m_hiddenSquareIndicatorParent = new GameObject("HiddenSquareIndicatorParent");
		}
		if (HighlightUtils.s_instance.m_hiddenSquarePrefab == null)
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
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(HighlightUtils.s_instance.m_hiddenSquarePrefab);
		gameObject.transform.localScale *= Board.Get().squareSize / 1.5f;
		gameObject.transform.parent = HighlightUtils.s_instance.m_hiddenSquareIndicatorParent.transform;
		return gameObject;
	}

	public static SquareIndicators GetHiddenSquaresContainer()
	{
		if (HighlightUtils.s_instance != null)
		{
			return HighlightUtils.s_instance.m_hiddenSquareIndicators;
		}
		return null;
	}

	private static GameObject CreateAffectedSquareIndicatorObject()
	{
		if (HighlightUtils.s_instance == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateAffectedSquareIndicatorObject()).MethodHandle;
			}
			return null;
		}
		if (HighlightUtils.s_instance.m_affectedSquareIndicatorParent == null)
		{
			HighlightUtils.s_instance.m_affectedSquareIndicatorParent = new GameObject("AffectedSquareIndicatorParent");
		}
		if (HighlightUtils.s_instance.m_affectedSquarePrefab == null)
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
			return null;
		}
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(HighlightUtils.s_instance.m_affectedSquarePrefab);
		gameObject.transform.localScale *= Board.Get().squareSize / 1.5f;
		gameObject.transform.parent = HighlightUtils.s_instance.m_affectedSquareIndicatorParent.transform;
		return gameObject;
	}

	public static SquareIndicators GetAffectedSquaresContainer()
	{
		if (HighlightUtils.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.GetAffectedSquaresContainer()).MethodHandle;
			}
			return HighlightUtils.s_instance.m_affectedSquareIndicators;
		}
		return null;
	}

	private static bool ShouldShowAffectedSquares()
	{
		if (HighlightUtils.s_instance != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.ShouldShowAffectedSquares()).MethodHandle;
			}
			if (HighlightUtils.s_instance.m_showAffectedSquaresWhileTargeting)
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
				if (HighlightUtils.s_instance.m_affectedSquarePrefab != null)
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
					return InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowAllyAbilityInfo);
				}
			}
		}
		return false;
	}

	private void CreateRangeIndicatorHighlight()
	{
		this.m_rangeIndicatorMouseOverFlags.Clear();
		for (int i = 0; i < 0xE; i++)
		{
			this.m_rangeIndicatorMouseOverFlags.Add(false);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.CreateRangeIndicatorHighlight()).MethodHandle;
		}
		if (this.m_abilityRangeIndicatorHighlight == null)
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
			this.m_abilityRangeIndicatorHighlight = HighlightUtils.Get().CreateDynamicConeMesh(1f, 360f, true, null);
			this.m_abilityRangeIndicatorHighlight.name = "AbilityRangeIndicatorObject";
			this.m_abilityRangeIndicatorHighlight.transform.parent = base.transform;
			UIDynamicCone uidynamicCone;
			if (this.m_abilityRangeIndicatorHighlight)
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
				uidynamicCone = this.m_abilityRangeIndicatorHighlight.GetComponent<UIDynamicCone>();
			}
			else
			{
				uidynamicCone = null;
			}
			UIDynamicCone uidynamicCone2 = uidynamicCone;
			uidynamicCone2.m_borderThickness = this.m_abilityRangeIndicatorWidth;
			uidynamicCone2.SetConeObjectActive(false);
			MeshRenderer[] componentsInChildren = this.m_abilityRangeIndicatorHighlight.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer meshRenderer in componentsInChildren)
			{
				if (HighlightUtils.Get() != null)
				{
					AbilityUtil_Targeter.SetMaterialColor(meshRenderer.materials, this.m_abilityRangeIndicatorColor, true);
				}
				AbilityUtil_Targeter.SetMaterialOpacity(meshRenderer.materials, this.m_abilityRangeIndicatorColor.a);
			}
			UIManager.SetGameObjectActive(this.m_abilityRangeIndicatorHighlight, false, null);
		}
	}

	private void DestroyRangeIndicatorHighlight()
	{
		if (this.m_abilityRangeIndicatorHighlight != null)
		{
			HighlightUtils.DestroyObjectAndMaterials(this.m_abilityRangeIndicatorHighlight);
			this.m_abilityRangeIndicatorHighlight = null;
		}
	}

	private void AdjustRangeIndicatorHighlight(float radiusInSquares)
	{
		if (this.m_abilityRangeIndicatorHighlight != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.AdjustRangeIndicatorHighlight(float)).MethodHandle;
			}
			this.m_lastRangeIndicatorRadius = radiusInSquares;
			HighlightUtils.Get().AdjustDynamicConeMesh(this.m_abilityRangeIndicatorHighlight, radiusInSquares, 360f);
		}
	}

	public void SetRangeIndicatorMouseOverFlag(int actionTypeInt, bool isMousingOver)
	{
		if (actionTypeInt >= 0 && actionTypeInt < this.m_rangeIndicatorMouseOverFlags.Count)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.SetRangeIndicatorMouseOverFlag(int, bool)).MethodHandle;
			}
			this.m_rangeIndicatorMouseOverFlags[actionTypeInt] = isMousingOver;
		}
	}

	public void UpdateRangeIndicatorHighlight()
	{
		ActorData actorData;
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.UpdateRangeIndicatorHighlight()).MethodHandle;
			}
			actorData = GameFlowData.Get().activeOwnedActorData;
		}
		else
		{
			actorData = null;
		}
		ActorData actorData2 = actorData;
		if (this.m_showAbilityRangeIndicator && this.m_abilityRangeIndicatorHighlight != null)
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
			if (actorData2 != null && actorData2.GetAbilityData() != null)
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
				if (actorData2.GetActorTurnSM() != null)
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
					AbilityData abilityData = actorData2.GetAbilityData();
					bool flag = false;
					if (!actorData2.IsDead())
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
						if (actorData2.GetActorTurnSM().CurrentState == TurnStateEnum.TARGETING_ACTION)
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
							if (abilityData.GetSelectedAbility() != null)
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
								if (abilityData.GetSelectedAbility().ShowTargetableRadiusWhileTargeting())
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
									int selectedActionTypeForTargeting = (int)abilityData.GetSelectedActionTypeForTargeting();
									float targetableRadius = abilityData.GetTargetableRadius(selectedActionTypeForTargeting, actorData2);
									if (targetableRadius > 0f)
									{
										this.AdjustAndShowRangeIndicator(actorData2.GetTravelBoardSquareWorldPosition(), targetableRadius);
										flag = true;
									}
								}
							}
							goto IL_1D7;
						}
					}
					if (!actorData2.IsDead())
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
						if (actorData2.GetActorTurnSM().CurrentState == TurnStateEnum.DECIDING)
						{
							List<bool> rangeIndicatorMouseOverFlags = this.m_rangeIndicatorMouseOverFlags;
							for (int i = 0; i < rangeIndicatorMouseOverFlags.Count; i++)
							{
								if (rangeIndicatorMouseOverFlags[i])
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
									float targetableRadius2 = abilityData.GetTargetableRadius(i, actorData2);
									if (targetableRadius2 > 0f)
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
										this.AdjustAndShowRangeIndicator(actorData2.GetTravelBoardSquareWorldPosition(), targetableRadius2);
										flag = true;
									}
									break;
								}
							}
						}
					}
					IL_1D7:
					if (!flag && this.m_abilityRangeIndicatorHighlight.activeSelf)
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
						UIManager.SetGameObjectActive(this.m_abilityRangeIndicatorHighlight, false, null);
					}
				}
			}
		}
	}

	public void AdjustAndShowRangeIndicator(Vector3 centerPos, float radiusInSquares)
	{
		if (this.m_lastRangeIndicatorRadius != radiusInSquares)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.AdjustAndShowRangeIndicator(Vector3, float)).MethodHandle;
			}
			this.AdjustRangeIndicatorHighlight(radiusInSquares);
		}
		centerPos.y = HighlightUtils.GetHighlightHeight();
		this.m_abilityRangeIndicatorHighlight.transform.position = centerPos;
		if (!this.m_abilityRangeIndicatorHighlight.activeSelf)
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
			UIManager.SetGameObjectActive(this.m_abilityRangeIndicatorHighlight, true, null);
		}
	}

	public void UpdateMouseoverCoverHighlight()
	{
		ActorData actorData;
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.UpdateMouseoverCoverHighlight()).MethodHandle;
			}
			actorData = GameFlowData.Get().activeOwnedActorData;
		}
		else
		{
			actorData = null;
		}
		ActorData actorData2 = actorData;
		if (actorData2 != null)
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
			if (actorData2.GetActorCover() != null)
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
				ActorCover actorCover = actorData2.GetActorCover();
				if (actorCover != null)
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
					if (this.m_currentCursorType == HighlightUtils.CursorType.MouseOverCursorType)
					{
						this.m_mouseoverCoverManager.UpdateCoverAroundSquare(Board.Get().PlayerFreeSquare);
					}
					else
					{
						this.m_mouseoverCoverManager.UpdateCoverAroundSquare(null);
					}
				}
			}
		}
	}

	public void UpdateShowAffectedSquareFlag()
	{
		this.m_cachedShouldShowAffectedSquares = HighlightUtils.ShouldShowAffectedSquares();
	}

	[Serializable]
	public class CoverDirIndicatorParams
	{
		public float m_radiusInSquares = 3f;

		public Color m_color = new Color(1f, 0.75f, 0.25f);

		public HighlightUtils.TargeterOpacityData[] m_opacity;
	}

	public enum MoveIntoCoverIndicatorTiming
	{
		ShowOnMoveEnd,
		ShowOnTurnStart
	}

	[Serializable]
	public struct AreaShapePrefab
	{
		public AbilityAreaShape m_shape;

		public GameObject m_prefab;
	}

	[Serializable]
	public struct TargeterOpacityData
	{
		public float m_timeSinceConfirmed;

		public float m_alpha;
	}

	public enum CursorType
	{
		MouseOverCursorType,
		TabTargetingCursorType,
		ShapeTargetingSquareCursorType,
		ShapeTargetingCornerCursorType,
		NoCursorType
	}

	public enum ScrollCursorDirection
	{
		N,
		NE,
		E,
		SE,
		S,
		SW,
		W,
		NW,
		Undefined
	}

	public class HighlightMesh
	{
		public List<Vector3> m_pos = new List<Vector3>();

		public List<Vector2> m_uv = new List<Vector2>();

		public Dictionary<BoardSquare, int> m_borderSquares = new Dictionary<BoardSquare, int>();

		private unsafe void ApplySlope(HighlightUtils.PieceType pieceType, ref Vector3[] positions, BoardSquare square)
		{
			float num = square.GetVerticesAtCorner_zq(BoardSquare.CornerType.UpperRight).y;
			float num2 = square.GetVerticesAtCorner_zq(BoardSquare.CornerType.UpperLeft).y;
			float num3 = square.GetVerticesAtCorner_zq(BoardSquare.CornerType.LowerRight).y;
			float num4 = square.GetVerticesAtCorner_zq(BoardSquare.CornerType.LowerLeft).y;
			if (!square.IsBaselineHeight())
			{
				float num5 = (float)Board.Get().BaselineHeight;
				float num6 = num5 + square.GetHighlightOffset();
				num = num6;
				num2 = num6;
				num3 = num6;
				num4 = num6;
			}
			float y = (num2 + num4) * 0.5f;
			float y2 = (num + num3) * 0.5f;
			float num7 = (num2 + num) * 0.5f;
			float num8 = (num4 + num3) * 0.5f;
			float y3 = (num7 + num8) * 0.5f;
			if (pieceType != HighlightUtils.PieceType.LowerLeftHorizontal)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.HighlightMesh.ApplySlope(HighlightUtils.PieceType, Vector3[]*, BoardSquare)).MethodHandle;
				}
				if (pieceType != HighlightUtils.PieceType.LowerLeftVertical)
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
					if (pieceType != HighlightUtils.PieceType.LowerLeftInnerCorner)
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
						if (pieceType != HighlightUtils.PieceType.LowerLeftOuterCorner)
						{
							if (pieceType != HighlightUtils.PieceType.LowerRightHorizontal && pieceType != HighlightUtils.PieceType.LowerRightVertical)
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
								if (pieceType != HighlightUtils.PieceType.LowerRightInnerCorner)
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
									if (pieceType != HighlightUtils.PieceType.LowerRightOuterCorner)
									{
										if (pieceType != HighlightUtils.PieceType.UpperLeftHorizontal)
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
											if (pieceType != HighlightUtils.PieceType.UpperLeftVertical && pieceType != HighlightUtils.PieceType.UpperLeftInnerCorner)
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
												if (pieceType != HighlightUtils.PieceType.UpperLeftOuterCorner)
												{
													if (pieceType != HighlightUtils.PieceType.UpperRightHorizontal)
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
														if (pieceType != HighlightUtils.PieceType.UpperRightVertical && pieceType != HighlightUtils.PieceType.UpperRightInnerCorner)
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
															if (pieceType != HighlightUtils.PieceType.UpperRightOuterCorner)
															{
																return;
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
													positions[0].y = num7;
													positions[1].y = y2;
													positions[2].y = y3;
													positions[3].y = num;
													return;
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
										positions[0].y = num2;
										positions[1].y = y3;
										positions[2].y = y;
										positions[3].y = num7;
										return;
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
							positions[0].y = y3;
							positions[1].y = num3;
							positions[2].y = num8;
							positions[3].y = y2;
							return;
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
			positions[0].y = y;
			positions[1].y = num8;
			positions[2].y = num4;
			positions[3].y = y3;
		}

		public void AddPiece(HighlightUtils.PieceType pieceType, BoardSquare square)
		{
			if (square != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.HighlightMesh.AddPiece(HighlightUtils.PieceType, BoardSquare)).MethodHandle;
				}
				if (!this.m_borderSquares.ContainsKey(square))
				{
					this.m_borderSquares[square] = 0;
				}
				int value = this.m_borderSquares[square];
				HighlightUtils.AddPieceTypeToMask(ref value, pieceType);
				this.m_borderSquares[square] = value;
				Vector3 b = square.ToVector3();
				b.y = 0.1f;
				HighlightUtils.HighlightPiece highlightPiece = HighlightUtils.Get().m_highlightPieces[(int)pieceType];
				Vector3[] pos = highlightPiece.m_pos;
				this.ApplySlope(pieceType, ref pos, square);
				foreach (Vector3 a in pos)
				{
					this.m_pos.Add(a + b);
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
				foreach (Vector2 item in highlightPiece.m_uv)
				{
					this.m_uv.Add(item);
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

		public void AddVerticalConnector(Vector3 pt0, Vector3 pt1, Vector3 pt2, Vector3 pt3, bool left)
		{
			this.m_pos.Add(pt0);
			this.m_pos.Add(pt1);
			this.m_pos.Add(pt2);
			this.m_pos.Add(pt3);
			if (left)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.HighlightMesh.AddVerticalConnector(Vector3, Vector3, Vector3, Vector3, bool)).MethodHandle;
				}
				this.m_uv.Add(new Vector2(0.6666667f, 1f));
				this.m_uv.Add(new Vector2(0.333333343f, 0f));
				this.m_uv.Add(new Vector2(0.333333343f, 1f));
				this.m_uv.Add(new Vector2(0.6666667f, 0f));
			}
			else
			{
				this.m_uv.Add(new Vector2(0.6666667f, 0f));
				this.m_uv.Add(new Vector2(0.333333343f, 1f));
				this.m_uv.Add(new Vector2(0.333333343f, 0f));
				this.m_uv.Add(new Vector2(0.6666667f, 1f));
			}
		}

		public void ProcessVerticalConnectors()
		{
			float num = Board.Get().squareSize * 0.5f;
			float num2 = num - 0.1f;
			float num3 = 0.5f;
			using (Dictionary<BoardSquare, int>.Enumerator enumerator = this.m_borderSquares.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<BoardSquare, int> keyValuePair = enumerator.Current;
					BoardSquare key = keyValuePair.Key;
					BoardSquare boardSquare = Board.Get().GetBoardSquare(key.x, key.y + 1);
					int value = keyValuePair.Value;
					if (boardSquare != null)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.HighlightMesh.ProcessVerticalConnectors()).MethodHandle;
						}
						if (this.m_borderSquares.ContainsKey(boardSquare))
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
							if ((float)(boardSquare.height - key.height) >= num3)
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
								if (HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.UpperLeftVertical) || HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.UpperLeftInnerCorner))
								{
									Vector3 pt = new Vector3(key.worldX - num, (float)boardSquare.height, key.worldY + num2);
									Vector3 pt2 = new Vector3(key.worldX, (float)key.height, key.worldY + num2);
									Vector3 pt3 = new Vector3(key.worldX - num, (float)key.height, key.worldY + num2);
									Vector3 pt4 = new Vector3(key.worldX, (float)boardSquare.height, key.worldY + num2);
									this.AddVerticalConnector(pt, pt2, pt3, pt4, true);
								}
								if (!HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.UpperRightVertical))
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
									if (!HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.UpperRightInnerCorner))
									{
										goto IL_26A;
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
								Vector3 pt5 = new Vector3(key.worldX, (float)boardSquare.height, key.worldY + num2);
								Vector3 pt6 = new Vector3(key.worldX + num, (float)key.height, key.worldY + num2);
								Vector3 pt7 = new Vector3(key.worldX, (float)key.height, key.worldY + num2);
								Vector3 pt8 = new Vector3(key.worldX + num, (float)boardSquare.height, key.worldY + num2);
								this.AddVerticalConnector(pt5, pt6, pt7, pt8, false);
							}
						}
					}
					IL_26A:
					BoardSquare boardSquare2 = Board.Get().GetBoardSquare(key.x - 1, key.y);
					if (boardSquare2 != null && this.m_borderSquares.ContainsKey(boardSquare2))
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
						if ((float)(boardSquare2.height - key.height) >= num3)
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
							if (HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.UpperLeftHorizontal))
							{
								goto IL_30C;
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
							if (HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.UpperLeftInnerCorner))
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									goto IL_30C;
								}
							}
							IL_3B1:
							if (!HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.LowerLeftHorizontal))
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
								if (!HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.LowerLeftInnerCorner))
								{
									goto IL_485;
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
							Vector3 pt9 = new Vector3(key.worldX - num2, (float)boardSquare2.height, key.worldY - num);
							Vector3 pt10 = new Vector3(key.worldX - num2, (float)key.height, key.worldY);
							Vector3 pt11 = new Vector3(key.worldX - num2, (float)key.height, key.worldY - num);
							Vector3 pt12 = new Vector3(key.worldX - num2, (float)boardSquare2.height, key.worldY);
							this.AddVerticalConnector(pt9, pt10, pt11, pt12, true);
							goto IL_485;
							IL_30C:
							Vector3 pt13 = new Vector3(key.worldX - num2, (float)boardSquare2.height, key.worldY);
							Vector3 pt14 = new Vector3(key.worldX - num2, (float)key.height, key.worldY + num);
							Vector3 pt15 = new Vector3(key.worldX - num2, (float)key.height, key.worldY);
							Vector3 pt16 = new Vector3(key.worldX - num2, (float)boardSquare2.height, key.worldY + num);
							this.AddVerticalConnector(pt13, pt14, pt15, pt16, false);
							goto IL_3B1;
						}
					}
					IL_485:
					BoardSquare boardSquare3 = Board.Get().GetBoardSquare(key.x, key.y - 1);
					if (boardSquare3 != null)
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
						if (this.m_borderSquares.ContainsKey(boardSquare3))
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
							if ((float)(boardSquare3.height - key.height) >= num3)
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
								if (HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.LowerLeftVertical) || HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.LowerLeftInnerCorner))
								{
									Vector3 pt17 = new Vector3(key.worldX - num, (float)boardSquare3.height, key.worldY - num2);
									Vector3 pt18 = new Vector3(key.worldX, (float)key.height, key.worldY - num2);
									Vector3 pt19 = new Vector3(key.worldX - num, (float)key.height, key.worldY - num2);
									Vector3 pt20 = new Vector3(key.worldX, (float)boardSquare3.height, key.worldY - num2);
									this.AddVerticalConnector(pt17, pt18, pt19, pt20, true);
								}
								if (!HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.LowerRightVertical))
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
									if (!HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.LowerRightInnerCorner))
									{
										goto IL_67C;
									}
								}
								Vector3 pt21 = new Vector3(key.worldX, (float)boardSquare3.height, key.worldY - num2);
								Vector3 pt22 = new Vector3(key.worldX + num, (float)key.height, key.worldY - num2);
								Vector3 pt23 = new Vector3(key.worldX, (float)key.height, key.worldY - num2);
								Vector3 pt24 = new Vector3(key.worldX + num, (float)boardSquare3.height, key.worldY - num2);
								this.AddVerticalConnector(pt21, pt22, pt23, pt24, false);
							}
						}
					}
					IL_67C:
					BoardSquare boardSquare4 = Board.Get().GetBoardSquare(key.x + 1, key.y);
					if (boardSquare4 != null)
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
						if (this.m_borderSquares.ContainsKey(boardSquare4))
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
							if ((float)(boardSquare4.height - key.height) >= num3)
							{
								if (HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.UpperRightHorizontal))
								{
									goto IL_71C;
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
								if (HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.UpperRightInnerCorner))
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										goto IL_71C;
									}
								}
								IL_7BD:
								if (!HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.LowerRightHorizontal))
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
									if (!HighlightUtils.IsPieceTypeInMask(value, HighlightUtils.PieceType.LowerRightInnerCorner))
									{
										continue;
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
								Vector3 pt25 = new Vector3(key.worldX + num2, (float)boardSquare4.height, key.worldY);
								Vector3 pt26 = new Vector3(key.worldX + num2, (float)key.height, key.worldY - num);
								Vector3 pt27 = new Vector3(key.worldX + num2, (float)key.height, key.worldY);
								Vector3 pt28 = new Vector3(key.worldX + num2, (float)boardSquare4.height, key.worldY - num);
								this.AddVerticalConnector(pt25, pt26, pt27, pt28, false);
								continue;
								IL_71C:
								Vector3 pt29 = new Vector3(key.worldX + num2, (float)boardSquare4.height, key.worldY + num);
								Vector3 pt30 = new Vector3(key.worldX + num2, (float)key.height, key.worldY);
								Vector3 pt31 = new Vector3(key.worldX + num2, (float)key.height, key.worldY + num);
								Vector3 pt32 = new Vector3(key.worldX + num2, (float)boardSquare4.height, key.worldY);
								this.AddVerticalConnector(pt29, pt30, pt31, pt32, true);
								goto IL_7BD;
							}
						}
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
		}

		public GameObject CreateMeshObject(Material borderMaterial)
		{
			GameObject gameObject = new GameObject("HighlightMesh");
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
			Mesh mesh = meshFilter.mesh;
			mesh.vertices = this.m_pos.ToArray();
			mesh.uv = this.m_uv.ToArray();
			Vector3[] array = new Vector3[mesh.vertices.Length];
			for (int i = 0; i < mesh.normals.Length; i++)
			{
				array[i] = Vector3.up;
			}
			mesh.normals = array;
			int[] array2 = new int[this.m_pos.Count / 4 * 6];
			for (int j = 0; j < this.m_pos.Count / 4; j++)
			{
				array2[j * 6] = j * 4;
				array2[j * 6 + 1] = j * 4 + 1;
				array2[j * 6 + 2] = j * 4 + 2;
				array2[j * 6 + 3] = j * 4;
				array2[j * 6 + 4] = j * 4 + 3;
				array2[j * 6 + 5] = j * 4 + 1;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightUtils.HighlightMesh.CreateMeshObject(Material)).MethodHandle;
			}
			mesh.triangles = array2;
			gameObject.GetComponent<Renderer>().material = borderMaterial;
			return gameObject;
		}
	}

	public class HighlightPiece
	{
		public Vector3[] m_pos = new Vector3[4];

		public Vector2[] m_uv = new Vector2[4];

		public int[] m_indices = new int[]
		{
			0,
			1,
			2,
			0,
			3,
			1
		};

		public Vector3[] m_nrm = new Vector3[]
		{
			Vector3.up,
			Vector3.up,
			Vector3.up,
			Vector3.up
		};
	}

	public enum PieceType
	{
		LowerRightVertical,
		LowerRightHorizontal,
		LowerRightInnerCorner,
		LowerRightOuterCorner,
		UpperRightVertical,
		UpperRightHorizontal,
		UpperRightInnerCorner,
		UpperRightOuterCorner,
		UpperLeftVertical,
		UpperLeftHorizontal,
		UpperLeftInnerCorner,
		UpperLeftOuterCorner,
		LowerLeftVertical,
		LowerLeftHorizontal,
		LowerLeftInnerCorner,
		LowerLeftOuterCorner,
		NumTypes
	}
}
