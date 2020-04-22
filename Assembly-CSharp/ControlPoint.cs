using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ControlPoint : NetworkBehaviour, IGameEventListener
{
	public enum State
	{
		Enabled,
		Locked,
		Disabled
	}

	public enum ControlProgressType
	{
		TugOfWar,
		IndependentProgress
	}

	public enum VisionGranting
	{
		Never,
		WhenControlled_ToEveryone,
		WhenControlled_ToControllers,
		WhenControlled_ToOthers,
		AlwaysWhenUnlocked,
		AlwaysIncludingLocked
	}

	public enum CaptureMessageCondition
	{
		OnFriendlyCapture,
		OnEnemyCapture,
		OnEnemyTeamACapture,
		OnFriendlyTeamACapture,
		OnEnemyTeamBCapture,
		OnFriendlyTeamBCapture
	}

	[Serializable]
	public struct CaptureMessage
	{
		public CaptureMessageCondition condition;

		public string message;

		public Color color;
	}

	[Serializable]
	public class ControlPointGameplay
	{
		public int m_objPoints_uncontested_vacant;

		public int m_objPoints_uncontested_alliesPresent;

		public int m_objPoints_uncontested_enemiesPresent;

		public int m_objPoints_contested_alliesOutnumberEnemies;

		public int m_objPoints_contested_equalEnemiesAndAllies;

		public int m_objPoints_contested_enemiesOutnumberAllies;

		public int m_objPoints_pointsPerAllyOutnumberingEnemy;

		public int m_objPoints_pointsPerEnemyOutnumberingAlly;
	}

	public string m_displayName = "Base Control Point";

	public BoardRegion m_region;

	[Separator("Progress Settings", true)]
	public State m_startingState;

	public ControlProgressType m_progressType;

	public bool m_stayControlledUntilOtherTeamCaptures;

	public bool m_resetProgressOnceCaptured;

	public bool m_resetProgressOnceDisabled = true;

	[Header("-- Progress For Team A")]
	public int m_progressNeededForTeamAToCapture = 10;

	public int m_maxTotalProgressForTeamA = 10;

	public int m_maxProgressForTeamAOnceControlled;

	[Header("-- Progress For Team B")]
	public int m_progressNeededForTeamBToCapture = 10;

	public int m_maxTotalProgressForTeamB = 10;

	public int m_maxProgressForTeamBOnceControlled;

	[Separator("Caps on Progress, Decay, Lock Duration", true)]
	public int m_maxProgressChangeInOneTurn = 10;

	public int m_progressDecayPerTurn = 5;

	public int m_numVacantTurnsUntilProgressDecays;

	[Header("-- Whether allow vacancy decay to happen when single team is vacant")]
	public bool m_allowIndependentVacancyDecay;

	[Header("-- Lock Duration")]
	public int m_turnsLockedAfterCapture;

	public int m_turnsLockedAfterActivated;

	public bool m_canContributeProgressWhileContested;

	[Header("-- Team A progress is positive; Team B is negative.  To make this start neutral, set to 0.")]
	public int m_startingProgress;

	[Separator("Objective Points", true)]
	public ControlPointGameplay m_controllingTeamGameplay;

	public ControlPointGameplay m_otherTeamGameplay;

	public int m_totalObjectivePointsToDispense = -1;

	public bool m_disableWhenDispensedLastObjectivePoint;

	[Separator("Other Control Points to Activate when Disabled", true)]
	public List<GameObject> m_controlPointsToActivateOnDisabled;

	[Header("-- (if <= 0, activate every control point in list) How many random control points to activate when this one is disabled")]
	public int m_numRandomControlPointsToActivate = -1;

	public int m_randomActivateTurnsLockedOverride = -1;

	public bool m_randomActivateIgnoreIfEverActivated;

	[Separator("Vision", true)]
	public VisionGranting m_visionGranting;

	public bool m_visionSeeThroughBrush = true;

	[Header("-- Vision region override, used if there are valid entries")]
	public BoardRegion m_visionRegionOverride;

	[Separator("Healing", true)]
	public VisionGranting m_whenToApplyHealing;

	public int m_healPerTurn;

	public GameObject m_healHitSequencePrefab;

	[Separator("NPC Spawners", true)]
	[Tooltip("If true, will activate only the spawners that match the team that took control of the point.  If false it will change the team of the spawner to the controlling team before activating.")]
	public bool m_spawnForMatchingTeams;

	public NPCSpawner[] m_spawnersForController;

	[Separator("Boundary/Visuals", true)]
	public bool m_autoGenerateBoundaryVisuals = true;

	public float m_boundaryOscillationSpeed = 3.14159f;

	public float m_boundaryOscillationHeight = 0.05f;

	public GameObject m_nameplateOverridePosition;

	private GameObject m_autoBoundary;

	private float m_autoBoundaryHeight;

	public GameObject m_boundaryNeutral;

	public GameObject m_boundaryAllied;

	public GameObject m_boundaryEnemy;

	public GameObject m_boundaryDisabled;

	[Header("-- Colors")]
	public Color m_primaryColor_friendly = Color.blue;

	public Color m_primaryColor_hostile = Color.red;

	public Color m_primaryColor_neutral = Color.gray;

	public Color m_secondaryColor_contested = Color.yellow;

	public Color m_secondaryColor_friendlyCapturing = Color.green;

	public Color m_secondaryColor_hostileCapturing = Color.magenta;

	public Color m_uiTextColor_Empty = new Color(0.8f, 0.8f, 0.8f, 1f);

	public Color m_uiTextColor_Locked = Color.yellow;

	public Sprite m_icon;

	[Header("-- Minimap")]
	public Color m_miniMapColorNeutral = Color.gray;

	public Color m_miniMapColorAllied = Color.blue;

	public Color m_miniMapColorEnemy = new Color(1f, 0.5f, 0f, 1f);

	public Color m_miniMapColorDisabled = new Color(0.5f, 0.5f, 0.5f, 0.2f);

	[Tooltip("Icon for this control point on the minimap, colored and scaled automatically")]
	public Sprite m_miniMapImage;

	[HideInInspector]
	public Color m_currentMinimapColor;

	[Header("Misc")]
	public CaptureMessage[] m_captureMessages;

	[SyncVar]
	private int m_currentProgressTugOfWar;

	[SyncVar]
	private int m_progressTeamA;

	[SyncVar]
	private int m_progressTeamB;

	[SyncVar]
	private int m_controllingTeam = -1;

	[SyncVar]
	private int m_numObjectivePointsDispensed;

	[SyncVar(hook = "HookSetCurrentState")]
	private State m_currentControlPointState;

	[SyncVar(hook = "SetLockedTurnsRemaining")]
	private int m_lockedTurnsRemaining;

	[SyncVar]
	private int m_numTeamAPlayers;

	[SyncVar]
	private int m_numTeamBPlayers;

	[SyncVar(hook = "HookSetGrantingVisionForTeamA")]
	private bool m_grantingVisionForTeamA;

	[SyncVar(hook = "HookSetGrantingVisionForTeamB")]
	private bool m_grantingVisionForTeamB;

	protected GameObject m_boundaryVFX;

	private bool m_initializedControlPointHud;

	private GameObject m_boundaryToShow;

	private static List<ControlPoint> s_controlPoints;

	public int CurrentProgressTugOfWar => m_currentProgressTugOfWar;

	public int ProgressTeamA => m_progressTeamA;

	public int ProgressTeamB => m_progressTeamB;

	public State CurrentControlPointState => m_currentControlPointState;

	public int Networkm_currentProgressTugOfWar
	{
		get
		{
			return m_currentProgressTugOfWar;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_currentProgressTugOfWar, 1u);
		}
	}

	public int Networkm_progressTeamA
	{
		get
		{
			return m_progressTeamA;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_progressTeamA, 2u);
		}
	}

	public int Networkm_progressTeamB
	{
		get
		{
			return m_progressTeamB;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_progressTeamB, 4u);
		}
	}

	public int Networkm_controllingTeam
	{
		get
		{
			return m_controllingTeam;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_controllingTeam, 8u);
		}
	}

	public int Networkm_numObjectivePointsDispensed
	{
		get
		{
			return m_numObjectivePointsDispensed;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_numObjectivePointsDispensed, 16u);
		}
	}

	public State Networkm_currentControlPointState
	{
		get
		{
			return m_currentControlPointState;
		}
		[param: In]
		set
		{
			ref State currentControlPointState = ref m_currentControlPointState;
			if (NetworkServer.localClientActive)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetCurrentState(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref currentControlPointState, 32u);
		}
	}

	public int Networkm_lockedTurnsRemaining
	{
		get
		{
			return m_lockedTurnsRemaining;
		}
		[param: In]
		set
		{
			ref int lockedTurnsRemaining = ref m_lockedTurnsRemaining;
			if (NetworkServer.localClientActive)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					SetLockedTurnsRemaining(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref lockedTurnsRemaining, 64u);
		}
	}

	public int Networkm_numTeamAPlayers
	{
		get
		{
			return m_numTeamAPlayers;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_numTeamAPlayers, 128u);
		}
	}

	public int Networkm_numTeamBPlayers
	{
		get
		{
			return m_numTeamBPlayers;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_numTeamBPlayers, 256u);
		}
	}

	public bool Networkm_grantingVisionForTeamA
	{
		get
		{
			return m_grantingVisionForTeamA;
		}
		[param: In]
		set
		{
			ref bool grantingVisionForTeamA = ref m_grantingVisionForTeamA;
			if (NetworkServer.localClientActive)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!base.syncVarHookGuard)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					base.syncVarHookGuard = true;
					HookSetGrantingVisionForTeamA(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref grantingVisionForTeamA, 512u);
		}
	}

	public bool Networkm_grantingVisionForTeamB
	{
		get
		{
			return m_grantingVisionForTeamB;
		}
		[param: In]
		set
		{
			ref bool grantingVisionForTeamB = ref m_grantingVisionForTeamB;
			if (NetworkServer.localClientActive)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					HookSetGrantingVisionForTeamB(value);
					base.syncVarHookGuard = false;
				}
			}
			SetSyncVar(value, ref grantingVisionForTeamB, 1024u);
		}
	}

	public void SetCurrentControlPointState(State newState)
	{
		if (m_currentControlPointState == newState)
		{
			return;
		}
		State currentControlPointState = m_currentControlPointState;
		Networkm_currentControlPointState = newState;
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (newState != State.Disabled)
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (m_resetProgressOnceDisabled)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						Networkm_progressTeamA = 0;
						Networkm_progressTeamB = 0;
						Networkm_currentProgressTugOfWar = 0;
						return;
					}
				}
				return;
			}
		}
	}

	private void SetLockedTurnsRemaining(int lockedTurnsRemaining)
	{
		if (m_lockedTurnsRemaining == lockedTurnsRemaining)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Networkm_lockedTurnsRemaining = lockedTurnsRemaining;
			if (CurrentControlPointState == State.Locked)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (lockedTurnsRemaining == 0)
				{
					SetCurrentControlPointState(State.Enabled);
					return;
				}
			}
			if (CurrentControlPointState == State.Enabled && lockedTurnsRemaining > 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					SetCurrentControlPointState(State.Locked);
					return;
				}
			}
			return;
		}
	}

	protected virtual void Start()
	{
		if (m_boundaryNeutral != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_boundaryNeutral.SetActive(false);
		}
		if (m_boundaryAllied != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			m_boundaryAllied.SetActive(false);
		}
		if (m_boundaryEnemy != null)
		{
			m_boundaryEnemy.SetActive(false);
		}
		if (m_boundaryDisabled != null)
		{
			m_boundaryDisabled.SetActive(false);
		}
		m_region.Initialize();
		m_visionRegionOverride.Initialize();
		GenerateBoundaryVisuals();
	}

	public static List<ControlPoint> GetAllControlPoints()
	{
		if (s_controlPoints == null)
		{
			s_controlPoints = new List<ControlPoint>();
		}
		return s_controlPoints;
	}

	private void Awake()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TurnTick);
		SetCurrentControlPointState(m_startingState);
		SetLockedTurnsRemaining(m_turnsLockedAfterActivated);
		if (s_controlPoints == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			s_controlPoints = new List<ControlPoint>();
		}
		s_controlPoints.Add(this);
	}

	private void OnDestroy()
	{
		s_controlPoints.Remove(this);
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TurnTick);
		if (m_autoBoundary != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary);
			m_autoBoundary = null;
		}
		if (!(HUD_UI.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			HUD_UI.Get().m_mainScreenPanel.m_controlPointNameplatePanel.RemoveControlPoint(this);
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveControlPoint(this);
			return;
		}
	}

	public void SetRegion(BoardRegion region)
	{
		m_region = region;
		m_region.Initialize();
		GenerateBoundaryVisuals(true);
	}

	public BoardRegion GetRegion()
	{
		return m_region;
	}

	public List<BoardSquare> GetSquaresForVision()
	{
		List<BoardSquare> squaresInRegion = m_visionRegionOverride.GetSquaresInRegion();
		if (squaresInRegion.Count > 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return squaresInRegion;
				}
			}
		}
		return m_region.GetSquaresInRegion();
	}

	protected void HookSetCurrentState(State newState)
	{
		Networkm_currentControlPointState = newState;
		RefreshBoundaryVFX();
	}

	private void Update()
	{
		if (HUD_UI.Get() != null)
		{
			if (!m_initializedControlPointHud)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				HUD_UI.Get().m_mainScreenPanel.m_controlPointNameplatePanel.AddControlPoint(this);
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddControlPoint(this);
				m_initializedControlPointHud = true;
			}
			if (m_boundaryToShow != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_boundaryVFX != m_boundaryToShow)
				{
					if (m_boundaryVFX != null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						m_boundaryVFX.SetActive(false);
					}
					m_boundaryToShow.SetActive(true);
					m_boundaryVFX = m_boundaryToShow;
					HUD_UI.Get().m_mainScreenPanel.m_minimap.AddControlPoint(this);
					m_boundaryToShow = null;
				}
			}
		}
		GenerateBoundaryVisuals();
		if (!(m_autoBoundary != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			float num = (1f - Mathf.Cos(Time.time * m_boundaryOscillationSpeed)) / 2f;
			float num2 = num * m_boundaryOscillationHeight;
			Transform transform = m_autoBoundary.transform;
			Vector3 position = m_autoBoundary.transform.position;
			float x = position.x;
			float y = m_autoBoundaryHeight + num2;
			Vector3 position2 = m_autoBoundary.transform.position;
			transform.position = new Vector3(x, y, position2.z);
			if (!(GameFlowData.Get() != null))
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (!(GameFlowData.Get().LocalPlayerData != null))
				{
					return;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
					Team controllingTeam = GetControllingTeam();
					Color mainColor = GetMainColor(controllingTeam, teamViewing);
					int num3;
					if (m_numTeamAPlayers > 0)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						num3 = ((m_numTeamBPlayers > 0) ? 1 : 0);
					}
					else
					{
						num3 = 0;
					}
					bool contested = (byte)num3 != 0;
					bool flag;
					bool flag2;
					if (m_canContributeProgressWhileContested)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						flag = (m_numTeamAPlayers > m_numTeamBPlayers);
						flag2 = (m_numTeamBPlayers > m_numTeamAPlayers);
					}
					else
					{
						int num4;
						if (m_numTeamAPlayers > 0)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							num4 = ((m_numTeamBPlayers == 0) ? 1 : 0);
						}
						else
						{
							num4 = 0;
						}
						flag = ((byte)num4 != 0);
						flag2 = (m_numTeamBPlayers > 0 && m_numTeamAPlayers == 0);
					}
					bool alliedControlled = controllingTeam == teamViewing;
					bool enemyControlled = controllingTeam != teamViewing;
					int num5;
					if (flag)
					{
						if (teamViewing == Team.TeamA)
						{
							num5 = 1;
							goto IL_02c1;
						}
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					num5 = ((flag2 && teamViewing == Team.TeamB) ? 1 : 0);
					goto IL_02c1;
					IL_02c1:
					bool alliedCapturing = (byte)num5 != 0;
					int num6;
					if (flag)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (teamViewing != 0)
						{
							num6 = 1;
							goto IL_02f1;
						}
					}
					if (flag2)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						num6 = ((teamViewing != Team.TeamB) ? 1 : 0);
					}
					else
					{
						num6 = 0;
					}
					goto IL_02f1;
					IL_02f1:
					bool enemyCapturing = (byte)num6 != 0;
					Color secondaryColor = GetSecondaryColor(alliedCapturing, alliedControlled, enemyCapturing, enemyControlled, contested, mainColor);
					float num7 = 1f - num * num;
					float num8 = num * num;
					Color boundaryColor = new Color(mainColor.r * num7 + secondaryColor.r * num8, mainColor.g * num7 + secondaryColor.g * num8, mainColor.b * num7 + secondaryColor.b * num8, mainColor.a * num7 + secondaryColor.a * num8);
					SetBoundaryColor(boundaryColor);
					return;
				}
			}
		}
	}

	private void GenerateBoundaryVisuals(bool forceRefreshBoundary = false)
	{
		if (m_autoBoundary != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_autoGenerateBoundaryVisuals)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_currentControlPointState != State.Disabled)
				{
					if (!forceRefreshBoundary)
					{
						goto IL_005b;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary);
			m_autoBoundary = null;
		}
		goto IL_005b;
		IL_005b:
		if (!(m_autoBoundary == null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (!m_autoGenerateBoundaryVisuals)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (m_currentControlPointState != State.Disabled)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						m_autoBoundary = HighlightUtils.Get().CreateBoundaryHighlight(m_region.GetSquaresInRegion(), Color.yellow);
						m_autoBoundary.name = m_displayName + " Auto-Boundary";
						UnityEngine.Object.DontDestroyOnLoad(m_autoBoundary);
						Vector3 position = m_autoBoundary.transform.position;
						m_autoBoundaryHeight = position.y;
						return;
					}
				}
				return;
			}
		}
	}

	private void SetBoundaryColor(Color newColor)
	{
		if (!(m_autoBoundary != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_autoBoundary.GetComponent<Renderer>().material.SetColor("_TintColor", newColor);
			return;
		}
	}

	protected void RefreshBoundaryVFX()
	{
		m_boundaryToShow = null;
		if (m_autoGenerateBoundaryVisuals)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_boundaryToShow = m_autoBoundary;
					return;
				}
			}
		}
		if (m_currentControlPointState == State.Disabled)
		{
			m_boundaryToShow = m_boundaryDisabled;
			m_currentMinimapColor = m_miniMapColorDisabled;
		}
		else
		{
			if (!(GameFlowData.Get() != null))
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (!(GameFlowData.Get().activeOwnedActorData != null))
				{
					return;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
					Team controllingTeam = GetControllingTeam();
					if (controllingTeam == Team.Invalid)
					{
						m_boundaryToShow = m_boundaryNeutral;
						m_currentMinimapColor = m_miniMapColorNeutral;
						return;
					}
					if (controllingTeam == teamViewing)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								m_boundaryToShow = m_boundaryAllied;
								m_currentMinimapColor = m_miniMapColorAllied;
								return;
							}
						}
					}
					m_boundaryToShow = m_boundaryEnemy;
					m_currentMinimapColor = m_miniMapColorEnemy;
					return;
				}
			}
		}
	}

	private Color GetMainColor(Team controllingTeam, Team clientTeam)
	{
		if (controllingTeam == Team.Invalid)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_primaryColor_neutral;
				}
			}
		}
		if (controllingTeam == clientTeam)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return m_primaryColor_friendly;
				}
			}
		}
		return m_primaryColor_hostile;
	}

	private Color GetSecondaryColor(bool alliedCapturing, bool alliedControlled, bool enemyCapturing, bool enemyControlled, bool contested, Color mainColor)
	{
		if (alliedCapturing)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (alliedControlled)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return mainColor;
							}
						}
					}
					return m_secondaryColor_friendlyCapturing;
				}
			}
		}
		if (enemyCapturing)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (enemyControlled)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								return mainColor;
							}
						}
					}
					return m_secondaryColor_hostileCapturing;
				}
			}
		}
		if (contested)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_secondaryColor_contested;
				}
			}
		}
		return mainColor;
	}

	public virtual Vector3 GetGUIPosition(float pixelsAbove)
	{
		Vector3 vector = (!(m_nameplateOverridePosition != null)) ? m_region.GetCenter() : m_nameplateOverridePosition.transform.position;
		Vector3 b2;
		if (Camera.main != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (Camera.main.transform != null)
			{
				Vector3 a = Camera.main.WorldToScreenPoint(vector);
				Vector3 b = Camera.main.WorldToScreenPoint(vector + Camera.main.transform.up);
				Vector3 vector2 = a - b;
				vector2.z = 0f;
				float d = pixelsAbove / vector2.magnitude;
				b2 = Camera.main.transform.up * d;
				goto IL_00eb;
			}
		}
		b2 = Vector3.zero;
		goto IL_00eb;
		IL_00eb:
		return vector + b2;
	}

	public virtual void SetupRectNameplate(ref TextMeshProUGUI controllerLabel, ref TextMeshProUGUI progressLabel, ref Slider bar)
	{
		if (GameFlowData.Get() == null)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameFlowData.Get().LocalPlayerData == null)
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
			CalcCurrentStatus(out bool _, out bool _, out bool _, out bool _, out bool alliedCapturing, out bool alliedControlled, out bool enemyCapturing, out bool enemyControlled, out bool contested);
			Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
			Team controllingTeam = GetControllingTeam();
			Team winningTeam = GetWinningTeam();
			Color mainColor = GetMainColor(winningTeam, teamViewing);
			Color mainColor2 = GetMainColor(controllingTeam, teamViewing);
			int num = 0;
			if (m_progressType == ControlProgressType.TugOfWar)
			{
				num = Mathf.Abs(CurrentProgressTugOfWar);
				if (CurrentProgressTugOfWar > 0)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					bar.value = (float)num / (float)m_maxTotalProgressForTeamA;
				}
				else if (CurrentProgressTugOfWar < 0)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					bar.value = (float)num / (float)m_maxTotalProgressForTeamB;
				}
				else
				{
					bar.value = 0f;
				}
				bar.fillRect.GetComponent<Image>().color = mainColor;
				GameObject gameObject = bar.gameObject;
				int active;
				if (num <= 0)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					active = ((CurrentControlPointState == State.Enabled) ? 1 : 0);
				}
				else
				{
					active = 1;
				}
				gameObject.SetActive((byte)active != 0);
			}
			else
			{
				bar.gameObject.SetActive(false);
			}
			if (controllingTeam != Team.Invalid)
			{
				controllerLabel.color = mainColor2;
			}
			else
			{
				controllerLabel.color = m_uiTextColor_Empty;
			}
			if (CurrentControlPointState != 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				progressLabel.color = m_uiTextColor_Locked;
			}
			else if (alliedCapturing)
			{
				progressLabel.color = m_secondaryColor_friendlyCapturing;
			}
			else if (enemyCapturing)
			{
				progressLabel.color = m_secondaryColor_hostileCapturing;
			}
			else if (contested)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				progressLabel.color = m_secondaryColor_contested;
			}
			else
			{
				progressLabel.color = m_uiTextColor_Empty;
			}
			if (controllingTeam == Team.TeamA)
			{
				if (teamViewing == Team.TeamA)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					controllerLabel.text = "Friendly Controlled";
				}
				else
				{
					controllerLabel.text = "Enemy Controlled";
				}
			}
			else if (controllingTeam == Team.TeamB)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (teamViewing == Team.TeamB)
				{
					controllerLabel.text = "Friendly Controlled";
				}
				else
				{
					controllerLabel.text = "Enemy Controlled";
				}
			}
			else
			{
				controllerLabel.text = "Uncontrolled";
			}
			if (m_totalObjectivePointsToDispense >= 0)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				string text = " (" + (m_totalObjectivePointsToDispense - m_numObjectivePointsDispensed) + " ObjPts)";
				controllerLabel.text += text;
			}
			string text2 = string.Empty;
			if (m_progressType == ControlProgressType.IndependentProgress)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				int num2 = 0;
				int num3 = 0;
				if (teamViewing == Team.TeamA)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					num2 = m_progressTeamA;
					num3 = m_progressTeamB;
				}
				else
				{
					num2 = m_progressTeamB;
					num3 = m_progressTeamA;
				}
				text2 = "\n(Progress: Ally " + num2 + ", Enemy " + num3 + ")";
			}
			if (CurrentControlPointState == State.Locked)
			{
				progressLabel.text = $"Locked for {m_lockedTurnsRemaining} turns";
				return;
			}
			if (CurrentControlPointState == State.Disabled)
			{
				progressLabel.text = "Disabled";
				return;
			}
			if (CurrentControlPointState != 0)
			{
				return;
			}
			if (alliedCapturing)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (alliedControlled)
				{
					progressLabel.text = "Friendly Occupied";
				}
				else
				{
					progressLabel.text = "Friendly Capturing!";
				}
			}
			else if (enemyCapturing)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (enemyControlled)
				{
					progressLabel.text = "Enemy Occupied";
				}
				else
				{
					progressLabel.text = "Enemy Capturing!";
				}
			}
			else if (!contested)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				progressLabel.text = "Uncontested";
			}
			else
			{
				progressLabel.text = "Capturing Contested";
			}
			progressLabel.text += text2;
			return;
		}
	}

	public virtual void OnTurnStart_ControlPoint_Client()
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		RefreshBoundaryVFX();
		if (!m_initializedControlPointHud)
		{
			HUD_UI.Get().m_mainScreenPanel.m_controlPointNameplatePanel.AddControlPoint(this);
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddControlPoint(this);
			m_initializedControlPointHud = true;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.TurnTick)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			OnTurnStart_ControlPoint();
			return;
		}
	}

	public virtual void OnTurnStart_ControlPoint()
	{
		OnTurnStart_ControlPoint_Client();
	}

	public virtual bool ShouldControlPointEnd()
	{
		return false;
	}

	public Team GetControllingTeam()
	{
		return (Team)m_controllingTeam;
	}

	public Team GetWinningTeam()
	{
		if (CurrentProgressTugOfWar > 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return Team.TeamA;
				}
			}
		}
		if (CurrentProgressTugOfWar < 0)
		{
			return Team.TeamB;
		}
		return Team.Invalid;
	}

	[Server]
	protected virtual void OnCapturedBy(Team capturedByTeam, Team previousControllingTeam)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogWarning("[Server] function 'System.Void ControlPoint::OnCapturedBy(Team,Team)' called on client");
					return;
				}
			}
		}
		SetLockedTurnsRemaining(m_turnsLockedAfterCapture);
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		CaptureMessage[] captureMessages = m_captureMessages;
		int num = 0;
		while (true)
		{
			if (num < captureMessages.Length)
			{
				CaptureMessage captureMessage = captureMessages[num];
				if (!string.IsNullOrEmpty(captureMessage.message))
				{
					if (captureMessage.condition == CaptureMessageCondition.OnFriendlyCapture)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (activeOwnedActorData.GetTeam() == capturedByTeam)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							InterfaceManager.Get().DisplayAlert(captureMessage.message, captureMessage.color, 7f);
							break;
						}
					}
					if (captureMessage.condition == CaptureMessageCondition.OnEnemyCapture && activeOwnedActorData.GetTeam() != capturedByTeam)
					{
						InterfaceManager.Get().DisplayAlert(captureMessage.message, captureMessage.color, 7f);
						break;
					}
					if (captureMessage.condition == CaptureMessageCondition.OnFriendlyTeamACapture)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (activeOwnedActorData.GetTeam() == capturedByTeam)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (capturedByTeam == Team.TeamA)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								InterfaceManager.Get().DisplayAlert(captureMessage.message, captureMessage.color, 7f);
								break;
							}
						}
					}
					if (captureMessage.condition == CaptureMessageCondition.OnEnemyTeamACapture)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (activeOwnedActorData.GetTeam() != capturedByTeam)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (capturedByTeam == Team.TeamA)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								InterfaceManager.Get().DisplayAlert(captureMessage.message, captureMessage.color, 7f);
								break;
							}
						}
					}
					if (captureMessage.condition == CaptureMessageCondition.OnFriendlyTeamBCapture)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (activeOwnedActorData.GetTeam() == capturedByTeam)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (capturedByTeam == Team.TeamB)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								InterfaceManager.Get().DisplayAlert(captureMessage.message, captureMessage.color, 7f);
								break;
							}
						}
					}
					if (captureMessage.condition == CaptureMessageCondition.OnEnemyTeamBCapture && activeOwnedActorData.GetTeam() != capturedByTeam)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (capturedByTeam == Team.TeamB)
						{
							InterfaceManager.Get().DisplayAlert(captureMessage.message, captureMessage.color, 7f);
							break;
						}
					}
				}
				num++;
				continue;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
		}
		GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchObjectiveEvent, new GameEventManager.MatchObjectiveEventArgs
		{
			objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.ControlPointCaptured,
			activatingActor = null,
			team = capturedByTeam
		});
	}

	public virtual void CalcCurrentStatus(out bool teamACapturing, out bool teamAControlled, out bool teamBCapturing, out bool teamBControlled, out bool alliedCapturing, out bool alliedControlled, out bool enemyCapturing, out bool enemyControlled, out bool contested)
	{
		Team controllingTeam = GetControllingTeam();
		if (controllingTeam == Team.TeamA)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			teamAControlled = true;
			teamBControlled = false;
		}
		else if (controllingTeam == Team.TeamB)
		{
			teamAControlled = false;
			teamBControlled = true;
		}
		else
		{
			teamAControlled = false;
			teamBControlled = false;
		}
		int num;
		if (m_numTeamAPlayers > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			num = ((m_numTeamBPlayers > 0) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		contested = ((byte)num != 0);
		if (m_canContributeProgressWhileContested)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			teamACapturing = (m_numTeamAPlayers > m_numTeamBPlayers);
			teamBCapturing = (m_numTeamBPlayers > m_numTeamAPlayers);
		}
		else
		{
			int num2;
			if (m_numTeamAPlayers > 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num2 = ((m_numTeamBPlayers == 0) ? 1 : 0);
			}
			else
			{
				num2 = 0;
			}
			teamACapturing = ((byte)num2 != 0);
			int num3;
			if (m_numTeamBPlayers > 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				num3 = ((m_numTeamAPlayers == 0) ? 1 : 0);
			}
			else
			{
				num3 = 0;
			}
			teamBCapturing = ((byte)num3 != 0);
		}
		if (GameFlowData.Get() != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (GameFlowData.Get().LocalPlayerData != null)
			{
				while (true)
				{
					Team teamViewing;
					int num4;
					int num5;
					int num6;
					int num7;
					switch (3)
					{
					case 0:
						break;
					default:
						{
							teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
							if (controllingTeam == Team.TeamA)
							{
								if (teamViewing == Team.TeamA)
								{
									num4 = 1;
									goto IL_0157;
								}
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							if (controllingTeam == Team.TeamB)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								num4 = ((teamViewing == Team.TeamB) ? 1 : 0);
							}
							else
							{
								num4 = 0;
							}
							goto IL_0157;
						}
						IL_01af:
						alliedCapturing = ((byte)num5 != 0);
						if (teamACapturing)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (teamViewing != 0)
							{
								num6 = 1;
								goto IL_01de;
							}
						}
						if (teamBCapturing)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							num6 = ((teamViewing != Team.TeamB) ? 1 : 0);
						}
						else
						{
							num6 = 0;
						}
						goto IL_01de;
						IL_018d:
						enemyControlled = ((byte)num7 != 0);
						if (teamACapturing)
						{
							if (teamViewing == Team.TeamA)
							{
								num5 = 1;
								goto IL_01af;
							}
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						num5 = ((teamBCapturing && teamViewing == Team.TeamB) ? 1 : 0);
						goto IL_01af;
						IL_0157:
						alliedControlled = ((byte)num4 != 0);
						if (controllingTeam == Team.TeamA)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (teamViewing == Team.TeamB)
							{
								num7 = 1;
								goto IL_018d;
							}
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						if (controllingTeam == Team.TeamB)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							num7 = ((teamViewing == Team.TeamA) ? 1 : 0);
						}
						else
						{
							num7 = 0;
						}
						goto IL_018d;
						IL_01de:
						enemyCapturing = ((byte)num6 != 0);
						return;
					}
				}
			}
		}
		alliedControlled = false;
		enemyControlled = false;
		alliedCapturing = false;
		enemyCapturing = false;
	}

	public bool IsGrantingVisionForTeam(Team team)
	{
		if (team == Team.Spectator)
		{
			return true;
		}
		if (team == Team.TeamA)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_grantingVisionForTeamA;
				}
			}
		}
		if (team == Team.TeamB)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_grantingVisionForTeamB;
				}
			}
		}
		return false;
	}

	private void HookSetGrantingVisionForTeamA(bool grantingVisionForTeamA)
	{
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_grantingVisionForTeamA != grantingVisionForTeamA)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					Networkm_grantingVisionForTeamA = grantingVisionForTeamA;
					List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(Team.TeamA);
					using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData current = enumerator.Current;
							current.GetFogOfWar().MarkForRecalculateVisibility();
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
			}
			return;
		}
	}

	private void HookSetGrantingVisionForTeamB(bool grantingVisionForTeamB)
	{
		if (!NetworkClient.active || m_grantingVisionForTeamB == grantingVisionForTeamB)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Networkm_grantingVisionForTeamB = grantingVisionForTeamB;
			List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(Team.TeamB);
			foreach (ActorData item in allTeamMembers)
			{
				item.GetFogOfWar().MarkForRecalculateVisibility();
			}
			return;
		}
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)m_currentProgressTugOfWar);
			writer.WritePackedUInt32((uint)m_progressTeamA);
			writer.WritePackedUInt32((uint)m_progressTeamB);
			writer.WritePackedUInt32((uint)m_controllingTeam);
			writer.WritePackedUInt32((uint)m_numObjectivePointsDispensed);
			writer.Write((int)m_currentControlPointState);
			writer.WritePackedUInt32((uint)m_lockedTurnsRemaining);
			writer.WritePackedUInt32((uint)m_numTeamAPlayers);
			writer.WritePackedUInt32((uint)m_numTeamBPlayers);
			writer.Write(m_grantingVisionForTeamA);
			writer.Write(m_grantingVisionForTeamB);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!flag)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_currentProgressTugOfWar);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_progressTeamA);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_progressTeamB);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			while (true)
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
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_controllingTeam);
		}
		if ((base.syncVarDirtyBits & 0x10) != 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_numObjectivePointsDispensed);
		}
		if ((base.syncVarDirtyBits & 0x20) != 0)
		{
			if (!flag)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)m_currentControlPointState);
		}
		if ((base.syncVarDirtyBits & 0x40) != 0)
		{
			while (true)
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
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_lockedTurnsRemaining);
		}
		if ((base.syncVarDirtyBits & 0x80) != 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_numTeamAPlayers);
		}
		if ((base.syncVarDirtyBits & 0x100) != 0)
		{
			if (!flag)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_numTeamBPlayers);
		}
		if ((base.syncVarDirtyBits & 0x200) != 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_grantingVisionForTeamA);
		}
		if ((base.syncVarDirtyBits & 0x400) != 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_grantingVisionForTeamB);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			m_currentProgressTugOfWar = (int)reader.ReadPackedUInt32();
			m_progressTeamA = (int)reader.ReadPackedUInt32();
			m_progressTeamB = (int)reader.ReadPackedUInt32();
			m_controllingTeam = (int)reader.ReadPackedUInt32();
			m_numObjectivePointsDispensed = (int)reader.ReadPackedUInt32();
			m_currentControlPointState = (State)reader.ReadInt32();
			m_lockedTurnsRemaining = (int)reader.ReadPackedUInt32();
			m_numTeamAPlayers = (int)reader.ReadPackedUInt32();
			m_numTeamBPlayers = (int)reader.ReadPackedUInt32();
			m_grantingVisionForTeamA = reader.ReadBoolean();
			m_grantingVisionForTeamB = reader.ReadBoolean();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_currentProgressTugOfWar = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_progressTeamA = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			m_progressTeamB = (int)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			m_controllingTeam = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x10) != 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			m_numObjectivePointsDispensed = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x20) != 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			HookSetCurrentState((State)reader.ReadInt32());
		}
		if ((num & 0x40) != 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			SetLockedTurnsRemaining((int)reader.ReadPackedUInt32());
		}
		if ((num & 0x80) != 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			m_numTeamAPlayers = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x100) != 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			m_numTeamBPlayers = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x200) != 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			HookSetGrantingVisionForTeamA(reader.ReadBoolean());
		}
		if ((num & 0x400) == 0)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			HookSetGrantingVisionForTeamB(reader.ReadBoolean());
			return;
		}
	}
}
