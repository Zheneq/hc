using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ControlPoint : NetworkBehaviour, IGameEventListener
{
	public string m_displayName = "Base Control Point";

	public BoardRegion m_region;

	[Separator("Progress Settings", true)]
	public ControlPoint.State m_startingState;

	public ControlPoint.ControlProgressType m_progressType;

	public bool m_stayControlledUntilOtherTeamCaptures;

	public bool m_resetProgressOnceCaptured;

	public bool m_resetProgressOnceDisabled = true;

	[Header("-- Progress For Team A")]
	public int m_progressNeededForTeamAToCapture = 0xA;

	public int m_maxTotalProgressForTeamA = 0xA;

	public int m_maxProgressForTeamAOnceControlled;

	[Header("-- Progress For Team B")]
	public int m_progressNeededForTeamBToCapture = 0xA;

	public int m_maxTotalProgressForTeamB = 0xA;

	public int m_maxProgressForTeamBOnceControlled;

	[Separator("Caps on Progress, Decay, Lock Duration", true)]
	public int m_maxProgressChangeInOneTurn = 0xA;

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
	public ControlPoint.ControlPointGameplay m_controllingTeamGameplay;

	public ControlPoint.ControlPointGameplay m_otherTeamGameplay;

	public int m_totalObjectivePointsToDispense = -1;

	public bool m_disableWhenDispensedLastObjectivePoint;

	[Separator("Other Control Points to Activate when Disabled", true)]
	public List<GameObject> m_controlPointsToActivateOnDisabled;

	[Header("-- (if <= 0, activate every control point in list) How many random control points to activate when this one is disabled")]
	public int m_numRandomControlPointsToActivate = -1;

	public int m_randomActivateTurnsLockedOverride = -1;

	public bool m_randomActivateIgnoreIfEverActivated;

	[Separator("Vision", true)]
	public ControlPoint.VisionGranting m_visionGranting;

	public bool m_visionSeeThroughBrush = true;

	[Header("-- Vision region override, used if there are valid entries")]
	public BoardRegion m_visionRegionOverride;

	[Separator("Healing", true)]
	public ControlPoint.VisionGranting m_whenToApplyHealing;

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
	public ControlPoint.CaptureMessage[] m_captureMessages;

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
	private ControlPoint.State m_currentControlPointState;

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

	public int CurrentProgressTugOfWar
	{
		get
		{
			return this.m_currentProgressTugOfWar;
		}
	}

	public int ProgressTeamA
	{
		get
		{
			return this.m_progressTeamA;
		}
	}

	public int ProgressTeamB
	{
		get
		{
			return this.m_progressTeamB;
		}
	}

	public ControlPoint.State CurrentControlPointState
	{
		get
		{
			return this.m_currentControlPointState;
		}
	}

	public void SetCurrentControlPointState(ControlPoint.State newState)
	{
		if (this.m_currentControlPointState != newState)
		{
			ControlPoint.State currentControlPointState = this.m_currentControlPointState;
			this.Networkm_currentControlPointState = newState;
			if (NetworkServer.active)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.SetCurrentControlPointState(ControlPoint.State)).MethodHandle;
				}
				if (newState == ControlPoint.State.Disabled)
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
					if (this.m_resetProgressOnceDisabled)
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
						this.Networkm_progressTeamA = 0;
						this.Networkm_progressTeamB = 0;
						this.Networkm_currentProgressTugOfWar = 0;
					}
				}
			}
		}
	}

	private void SetLockedTurnsRemaining(int lockedTurnsRemaining)
	{
		if (this.m_lockedTurnsRemaining != lockedTurnsRemaining)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.SetLockedTurnsRemaining(int)).MethodHandle;
			}
			this.Networkm_lockedTurnsRemaining = lockedTurnsRemaining;
			if (this.CurrentControlPointState == ControlPoint.State.Locked)
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
				if (lockedTurnsRemaining == 0)
				{
					this.SetCurrentControlPointState(ControlPoint.State.Enabled);
					return;
				}
			}
			if (this.CurrentControlPointState == ControlPoint.State.Enabled && lockedTurnsRemaining > 0)
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
				this.SetCurrentControlPointState(ControlPoint.State.Locked);
			}
		}
	}

	protected virtual void Start()
	{
		if (this.m_boundaryNeutral != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.Start()).MethodHandle;
			}
			this.m_boundaryNeutral.SetActive(false);
		}
		if (this.m_boundaryAllied != null)
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
			this.m_boundaryAllied.SetActive(false);
		}
		if (this.m_boundaryEnemy != null)
		{
			this.m_boundaryEnemy.SetActive(false);
		}
		if (this.m_boundaryDisabled != null)
		{
			this.m_boundaryDisabled.SetActive(false);
		}
		this.m_region.Initialize();
		this.m_visionRegionOverride.Initialize();
		this.GenerateBoundaryVisuals(false);
	}

	public static List<ControlPoint> GetAllControlPoints()
	{
		if (ControlPoint.s_controlPoints == null)
		{
			ControlPoint.s_controlPoints = new List<ControlPoint>();
		}
		return ControlPoint.s_controlPoints;
	}

	private void Awake()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.TurnTick);
		this.SetCurrentControlPointState(this.m_startingState);
		this.SetLockedTurnsRemaining(this.m_turnsLockedAfterActivated);
		if (ControlPoint.s_controlPoints == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.Awake()).MethodHandle;
			}
			ControlPoint.s_controlPoints = new List<ControlPoint>();
		}
		ControlPoint.s_controlPoints.Add(this);
	}

	private void OnDestroy()
	{
		ControlPoint.s_controlPoints.Remove(this);
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.TurnTick);
		if (this.m_autoBoundary != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.OnDestroy()).MethodHandle;
			}
			HighlightUtils.DestroyBoundaryHighlightObject(this.m_autoBoundary);
			this.m_autoBoundary = null;
		}
		if (HUD_UI.Get() != null)
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
			HUD_UI.Get().m_mainScreenPanel.m_controlPointNameplatePanel.RemoveControlPoint(this);
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemoveControlPoint(this);
		}
	}

	public void SetRegion(BoardRegion region)
	{
		this.m_region = region;
		this.m_region.Initialize();
		this.GenerateBoundaryVisuals(true);
	}

	public BoardRegion GetRegion()
	{
		return this.m_region;
	}

	public List<BoardSquare> GetSquaresForVision()
	{
		List<BoardSquare> list = this.m_visionRegionOverride.\u001D();
		if (list.Count > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.GetSquaresForVision()).MethodHandle;
			}
			return list;
		}
		return this.m_region.\u001D();
	}

	protected void HookSetCurrentState(ControlPoint.State newState)
	{
		this.Networkm_currentControlPointState = newState;
		this.RefreshBoundaryVFX();
	}

	private void Update()
	{
		if (HUD_UI.Get() != null)
		{
			if (!this.m_initializedControlPointHud)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.Update()).MethodHandle;
				}
				HUD_UI.Get().m_mainScreenPanel.m_controlPointNameplatePanel.AddControlPoint(this);
				HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddControlPoint(this);
				this.m_initializedControlPointHud = true;
			}
			if (this.m_boundaryToShow != null)
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
				if (this.m_boundaryVFX != this.m_boundaryToShow)
				{
					if (this.m_boundaryVFX != null)
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
						this.m_boundaryVFX.SetActive(false);
					}
					this.m_boundaryToShow.SetActive(true);
					this.m_boundaryVFX = this.m_boundaryToShow;
					HUD_UI.Get().m_mainScreenPanel.m_minimap.AddControlPoint(this);
					this.m_boundaryToShow = null;
				}
			}
		}
		this.GenerateBoundaryVisuals(false);
		if (this.m_autoBoundary != null)
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
			float num = (1f - Mathf.Cos(Time.time * this.m_boundaryOscillationSpeed)) / 2f;
			float num2 = num * this.m_boundaryOscillationHeight;
			this.m_autoBoundary.transform.position = new Vector3(this.m_autoBoundary.transform.position.x, this.m_autoBoundaryHeight + num2, this.m_autoBoundary.transform.position.z);
			if (GameFlowData.Get() != null)
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
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
					Team controllingTeam = this.GetControllingTeam();
					Color mainColor = this.GetMainColor(controllingTeam, teamViewing);
					bool flag;
					if (this.m_numTeamAPlayers > 0)
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
						flag = (this.m_numTeamBPlayers > 0);
					}
					else
					{
						flag = false;
					}
					bool contested = flag;
					bool flag2;
					bool flag3;
					if (this.m_canContributeProgressWhileContested)
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
						flag2 = (this.m_numTeamAPlayers > this.m_numTeamBPlayers);
						flag3 = (this.m_numTeamBPlayers > this.m_numTeamAPlayers);
					}
					else
					{
						bool flag4;
						if (this.m_numTeamAPlayers > 0)
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
							flag4 = (this.m_numTeamBPlayers == 0);
						}
						else
						{
							flag4 = false;
						}
						flag2 = flag4;
						flag3 = (this.m_numTeamBPlayers > 0 && this.m_numTeamAPlayers == 0);
					}
					bool alliedControlled = controllingTeam == teamViewing;
					bool enemyControlled = controllingTeam != teamViewing;
					bool flag5;
					if (flag2)
					{
						if (teamViewing == Team.TeamA)
						{
							flag5 = true;
							goto IL_2C1;
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
					flag5 = (flag3 && teamViewing == Team.TeamB);
					IL_2C1:
					bool alliedCapturing = flag5;
					bool flag6;
					if (flag2)
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
						if (teamViewing != Team.TeamA)
						{
							flag6 = true;
							goto IL_2F1;
						}
					}
					if (flag3)
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
						flag6 = (teamViewing != Team.TeamB);
					}
					else
					{
						flag6 = false;
					}
					IL_2F1:
					bool enemyCapturing = flag6;
					Color secondaryColor = this.GetSecondaryColor(alliedCapturing, alliedControlled, enemyCapturing, enemyControlled, contested, mainColor);
					float num3 = 1f - num * num;
					float num4 = num * num;
					Color boundaryColor = new Color(mainColor.r * num3 + secondaryColor.r * num4, mainColor.g * num3 + secondaryColor.g * num4, mainColor.b * num3 + secondaryColor.b * num4, mainColor.a * num3 + secondaryColor.a * num4);
					this.SetBoundaryColor(boundaryColor);
				}
			}
		}
	}

	private void GenerateBoundaryVisuals(bool forceRefreshBoundary = false)
	{
		if (this.m_autoBoundary != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.GenerateBoundaryVisuals(bool)).MethodHandle;
			}
			if (this.m_autoGenerateBoundaryVisuals)
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
				if (this.m_currentControlPointState != ControlPoint.State.Disabled)
				{
					if (!forceRefreshBoundary)
					{
						goto IL_5B;
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
			}
			HighlightUtils.DestroyBoundaryHighlightObject(this.m_autoBoundary);
			this.m_autoBoundary = null;
		}
		IL_5B:
		if (this.m_autoBoundary == null)
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
			if (this.m_autoGenerateBoundaryVisuals)
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
				if (this.m_currentControlPointState != ControlPoint.State.Disabled)
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
					this.m_autoBoundary = HighlightUtils.Get().CreateBoundaryHighlight(this.m_region.\u001D(), Color.yellow, false);
					this.m_autoBoundary.name = this.m_displayName + " Auto-Boundary";
					UnityEngine.Object.DontDestroyOnLoad(this.m_autoBoundary);
					this.m_autoBoundaryHeight = this.m_autoBoundary.transform.position.y;
				}
			}
		}
	}

	private void SetBoundaryColor(Color newColor)
	{
		if (this.m_autoBoundary != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.SetBoundaryColor(Color)).MethodHandle;
			}
			this.m_autoBoundary.GetComponent<Renderer>().material.SetColor("_TintColor", newColor);
		}
	}

	protected void RefreshBoundaryVFX()
	{
		this.m_boundaryToShow = null;
		if (this.m_autoGenerateBoundaryVisuals)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.RefreshBoundaryVFX()).MethodHandle;
			}
			this.m_boundaryToShow = this.m_autoBoundary;
		}
		else if (this.m_currentControlPointState == ControlPoint.State.Disabled)
		{
			this.m_boundaryToShow = this.m_boundaryDisabled;
			this.m_currentMinimapColor = this.m_miniMapColorDisabled;
		}
		else if (GameFlowData.Get() != null)
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
				Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				Team controllingTeam = this.GetControllingTeam();
				if (controllingTeam == Team.Invalid)
				{
					this.m_boundaryToShow = this.m_boundaryNeutral;
					this.m_currentMinimapColor = this.m_miniMapColorNeutral;
				}
				else if (controllingTeam == teamViewing)
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
					this.m_boundaryToShow = this.m_boundaryAllied;
					this.m_currentMinimapColor = this.m_miniMapColorAllied;
				}
				else
				{
					this.m_boundaryToShow = this.m_boundaryEnemy;
					this.m_currentMinimapColor = this.m_miniMapColorEnemy;
				}
			}
		}
	}

	private Color GetMainColor(Team controllingTeam, Team clientTeam)
	{
		Color result;
		if (controllingTeam == Team.Invalid)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.GetMainColor(Team, Team)).MethodHandle;
			}
			result = this.m_primaryColor_neutral;
		}
		else if (controllingTeam == clientTeam)
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
			result = this.m_primaryColor_friendly;
		}
		else
		{
			result = this.m_primaryColor_hostile;
		}
		return result;
	}

	private Color GetSecondaryColor(bool alliedCapturing, bool alliedControlled, bool enemyCapturing, bool enemyControlled, bool contested, Color mainColor)
	{
		Color result;
		if (alliedCapturing)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.GetSecondaryColor(bool, bool, bool, bool, bool, Color)).MethodHandle;
			}
			if (alliedControlled)
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
				result = mainColor;
			}
			else
			{
				result = this.m_secondaryColor_friendlyCapturing;
			}
		}
		else if (enemyCapturing)
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
			if (enemyControlled)
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
				result = mainColor;
			}
			else
			{
				result = this.m_secondaryColor_hostileCapturing;
			}
		}
		else if (contested)
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
			result = this.m_secondaryColor_contested;
		}
		else
		{
			result = mainColor;
		}
		return result;
	}

	public virtual Vector3 GetGUIPosition(float pixelsAbove)
	{
		Vector3 vector;
		if (this.m_nameplateOverridePosition != null)
		{
			vector = this.m_nameplateOverridePosition.transform.position;
		}
		else
		{
			vector = this.m_region.\u001D();
		}
		Vector3 b2;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.GetGUIPosition(float)).MethodHandle;
			}
			if (Camera.main.transform != null)
			{
				Vector3 a = Camera.main.WorldToScreenPoint(vector);
				Vector3 b = Camera.main.WorldToScreenPoint(vector + Camera.main.transform.up);
				Vector3 vector2 = a - b;
				vector2.z = 0f;
				float d = pixelsAbove / vector2.magnitude;
				b2 = Camera.main.transform.up * d;
				goto IL_EB;
			}
		}
		b2 = Vector3.zero;
		IL_EB:
		return vector + b2;
	}

	public unsafe virtual void SetupRectNameplate(ref TextMeshProUGUI controllerLabel, ref TextMeshProUGUI progressLabel, ref Slider bar)
	{
		if (!(GameFlowData.Get() == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.SetupRectNameplate(TextMeshProUGUI*, TextMeshProUGUI*, Slider*)).MethodHandle;
			}
			if (!(GameFlowData.Get().LocalPlayerData == null))
			{
				bool flag;
				bool flag2;
				bool flag3;
				bool flag4;
				bool flag5;
				bool flag6;
				bool flag7;
				bool flag8;
				bool flag9;
				this.CalcCurrentStatus(out flag, out flag2, out flag3, out flag4, out flag5, out flag6, out flag7, out flag8, out flag9);
				Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				Team controllingTeam = this.GetControllingTeam();
				Team winningTeam = this.GetWinningTeam();
				Color mainColor = this.GetMainColor(winningTeam, teamViewing);
				Color mainColor2 = this.GetMainColor(controllingTeam, teamViewing);
				if (this.m_progressType == ControlPoint.ControlProgressType.TugOfWar)
				{
					int num = Mathf.Abs(this.CurrentProgressTugOfWar);
					if (this.CurrentProgressTugOfWar > 0)
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
						bar.value = (float)num / (float)this.m_maxTotalProgressForTeamA;
					}
					else if (this.CurrentProgressTugOfWar < 0)
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
						bar.value = (float)num / (float)this.m_maxTotalProgressForTeamB;
					}
					else
					{
						bar.value = 0f;
					}
					bar.fillRect.GetComponent<Image>().color = mainColor;
					GameObject gameObject = bar.gameObject;
					bool active;
					if (num <= 0)
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
						active = (this.CurrentControlPointState == ControlPoint.State.Enabled);
					}
					else
					{
						active = true;
					}
					gameObject.SetActive(active);
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
					controllerLabel.color = this.m_uiTextColor_Empty;
				}
				if (this.CurrentControlPointState != ControlPoint.State.Enabled)
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
					progressLabel.color = this.m_uiTextColor_Locked;
				}
				else if (flag5)
				{
					progressLabel.color = this.m_secondaryColor_friendlyCapturing;
				}
				else if (flag7)
				{
					progressLabel.color = this.m_secondaryColor_hostileCapturing;
				}
				else if (flag9)
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
					progressLabel.color = this.m_secondaryColor_contested;
				}
				else
				{
					progressLabel.color = this.m_uiTextColor_Empty;
				}
				if (controllingTeam == Team.TeamA)
				{
					if (teamViewing == Team.TeamA)
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
						controllerLabel.text = "Friendly Controlled";
					}
					else
					{
						controllerLabel.text = "Enemy Controlled";
					}
				}
				else if (controllingTeam == Team.TeamB)
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
				if (this.m_totalObjectivePointsToDispense >= 0)
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
					string str = " (" + (this.m_totalObjectivePointsToDispense - this.m_numObjectivePointsDispensed).ToString() + " ObjPts)";
					TextMeshProUGUI textMeshProUGUI = controllerLabel;
					textMeshProUGUI.text += str;
				}
				string str2 = string.Empty;
				if (this.m_progressType == ControlPoint.ControlProgressType.IndependentProgress)
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
					int num2;
					int num3;
					if (teamViewing == Team.TeamA)
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
						num2 = this.m_progressTeamA;
						num3 = this.m_progressTeamB;
					}
					else
					{
						num2 = this.m_progressTeamB;
						num3 = this.m_progressTeamA;
					}
					str2 = string.Concat(new object[]
					{
						"\n(Progress: Ally ",
						num2,
						", Enemy ",
						num3,
						")"
					});
				}
				if (this.CurrentControlPointState == ControlPoint.State.Locked)
				{
					progressLabel.text = string.Format("Locked for {0} turns", this.m_lockedTurnsRemaining);
				}
				else if (this.CurrentControlPointState == ControlPoint.State.Disabled)
				{
					progressLabel.text = "Disabled";
				}
				else if (this.CurrentControlPointState == ControlPoint.State.Enabled)
				{
					if (flag5)
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
						if (flag6)
						{
							progressLabel.text = "Friendly Occupied";
						}
						else
						{
							progressLabel.text = "Friendly Capturing!";
						}
					}
					else if (flag7)
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
						if (flag8)
						{
							progressLabel.text = "Enemy Occupied";
						}
						else
						{
							progressLabel.text = "Enemy Capturing!";
						}
					}
					else if (!flag9)
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
						progressLabel.text = "Uncontested";
					}
					else
					{
						progressLabel.text = "Capturing Contested";
					}
					TextMeshProUGUI textMeshProUGUI2 = progressLabel;
					textMeshProUGUI2.text += str2;
				}
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

	public virtual void OnTurnStart_ControlPoint_Client()
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.OnTurnStart_ControlPoint_Client()).MethodHandle;
			}
			return;
		}
		this.RefreshBoundaryVFX();
		if (!this.m_initializedControlPointHud)
		{
			HUD_UI.Get().m_mainScreenPanel.m_controlPointNameplatePanel.AddControlPoint(this);
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.AddControlPoint(this);
			this.m_initializedControlPointHud = true;
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.TurnTick)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
			this.OnTurnStart_ControlPoint();
		}
	}

	public virtual void OnTurnStart_ControlPoint()
	{
		this.OnTurnStart_ControlPoint_Client();
	}

	public virtual bool ShouldControlPointEnd()
	{
		return false;
	}

	public Team GetControllingTeam()
	{
		return (Team)this.m_controllingTeam;
	}

	public Team GetWinningTeam()
	{
		if (this.CurrentProgressTugOfWar > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.GetWinningTeam()).MethodHandle;
			}
			return Team.TeamA;
		}
		if (this.CurrentProgressTugOfWar < 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.OnCapturedBy(Team, Team)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void ControlPoint::OnCapturedBy(Team,Team)' called on client");
			return;
		}
		this.SetLockedTurnsRemaining(this.m_turnsLockedAfterCapture);
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		foreach (ControlPoint.CaptureMessage captureMessage in this.m_captureMessages)
		{
			if (!string.IsNullOrEmpty(captureMessage.message))
			{
				if (captureMessage.condition == ControlPoint.CaptureMessageCondition.OnFriendlyCapture)
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
					if (activeOwnedActorData.\u000E() == capturedByTeam)
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
						InterfaceManager.Get().DisplayAlert(captureMessage.message, captureMessage.color, 7f, false, 0);
						goto IL_260;
					}
				}
				if (captureMessage.condition == ControlPoint.CaptureMessageCondition.OnEnemyCapture && activeOwnedActorData.\u000E() != capturedByTeam)
				{
					InterfaceManager.Get().DisplayAlert(captureMessage.message, captureMessage.color, 7f, false, 0);
				}
				else
				{
					if (captureMessage.condition == ControlPoint.CaptureMessageCondition.OnFriendlyTeamACapture)
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
						if (activeOwnedActorData.\u000E() == capturedByTeam)
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
							if (capturedByTeam == Team.TeamA)
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
								InterfaceManager.Get().DisplayAlert(captureMessage.message, captureMessage.color, 7f, false, 0);
								goto IL_260;
							}
						}
					}
					if (captureMessage.condition == ControlPoint.CaptureMessageCondition.OnEnemyTeamACapture)
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
						if (activeOwnedActorData.\u000E() != capturedByTeam)
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
							if (capturedByTeam == Team.TeamA)
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
								InterfaceManager.Get().DisplayAlert(captureMessage.message, captureMessage.color, 7f, false, 0);
								goto IL_260;
							}
						}
					}
					if (captureMessage.condition == ControlPoint.CaptureMessageCondition.OnFriendlyTeamBCapture)
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
						if (activeOwnedActorData.\u000E() == capturedByTeam)
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
							if (capturedByTeam == Team.TeamB)
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
								InterfaceManager.Get().DisplayAlert(captureMessage.message, captureMessage.color, 7f, false, 0);
								goto IL_260;
							}
						}
					}
					if (captureMessage.condition != ControlPoint.CaptureMessageCondition.OnEnemyTeamBCapture || activeOwnedActorData.\u000E() == capturedByTeam)
					{
						goto IL_249;
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
					if (capturedByTeam != Team.TeamB)
					{
						goto IL_249;
					}
					InterfaceManager.Get().DisplayAlert(captureMessage.message, captureMessage.color, 7f, false, 0);
				}
				IL_260:
				GameEventManager.Get().FireEvent(GameEventManager.EventType.MatchObjectiveEvent, new GameEventManager.MatchObjectiveEventArgs
				{
					objective = GameEventManager.MatchObjectiveEventArgs.ObjectiveType.ControlPointCaptured,
					activatingActor = null,
					team = capturedByTeam
				});
				return;
			}
			IL_249:;
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			goto IL_260;
		}
	}

	public unsafe virtual void CalcCurrentStatus(out bool teamACapturing, out bool teamAControlled, out bool teamBCapturing, out bool teamBControlled, out bool alliedCapturing, out bool alliedControlled, out bool enemyCapturing, out bool enemyControlled, out bool contested)
	{
		Team controllingTeam = this.GetControllingTeam();
		if (controllingTeam == Team.TeamA)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.CalcCurrentStatus(bool*, bool*, bool*, bool*, bool*, bool*, bool*, bool*, bool*)).MethodHandle;
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
		bool flag;
		if (this.m_numTeamAPlayers > 0)
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
			flag = (this.m_numTeamBPlayers > 0);
		}
		else
		{
			flag = false;
		}
		contested = flag;
		if (this.m_canContributeProgressWhileContested)
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
			teamACapturing = (this.m_numTeamAPlayers > this.m_numTeamBPlayers);
			teamBCapturing = (this.m_numTeamBPlayers > this.m_numTeamAPlayers);
		}
		else
		{
			bool flag2;
			if (this.m_numTeamAPlayers > 0)
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
				flag2 = (this.m_numTeamBPlayers == 0);
			}
			else
			{
				flag2 = false;
			}
			teamACapturing = flag2;
			bool flag3;
			if (this.m_numTeamBPlayers > 0)
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
				flag3 = (this.m_numTeamAPlayers == 0);
			}
			else
			{
				flag3 = false;
			}
			teamBCapturing = flag3;
		}
		if (GameFlowData.Get() != null)
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
			if (GameFlowData.Get().LocalPlayerData != null)
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
				Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				bool flag4;
				if (controllingTeam == Team.TeamA)
				{
					if (teamViewing == Team.TeamA)
					{
						flag4 = true;
						goto IL_157;
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
				if (controllingTeam == Team.TeamB)
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
					flag4 = (teamViewing == Team.TeamB);
				}
				else
				{
					flag4 = false;
				}
				IL_157:
				alliedControlled = flag4;
				bool flag5;
				if (controllingTeam == Team.TeamA)
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
					if (teamViewing == Team.TeamB)
					{
						flag5 = true;
						goto IL_18D;
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
				if (controllingTeam == Team.TeamB)
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
					flag5 = (teamViewing == Team.TeamA);
				}
				else
				{
					flag5 = false;
				}
				IL_18D:
				enemyControlled = flag5;
				bool flag6;
				if (teamACapturing)
				{
					if (teamViewing == Team.TeamA)
					{
						flag6 = true;
						goto IL_1AF;
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
				flag6 = (teamBCapturing && teamViewing == Team.TeamB);
				IL_1AF:
				alliedCapturing = flag6;
				bool flag7;
				if (teamACapturing)
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
					if (teamViewing != Team.TeamA)
					{
						flag7 = true;
						goto IL_1DE;
					}
				}
				if (teamBCapturing)
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
					flag7 = (teamViewing != Team.TeamB);
				}
				else
				{
					flag7 = false;
				}
				IL_1DE:
				enemyCapturing = flag7;
				return;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.IsGrantingVisionForTeam(Team)).MethodHandle;
			}
			return this.m_grantingVisionForTeamA;
		}
		if (team == Team.TeamB)
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
			return this.m_grantingVisionForTeamB;
		}
		return false;
	}

	private void HookSetGrantingVisionForTeamA(bool grantingVisionForTeamA)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.HookSetGrantingVisionForTeamA(bool)).MethodHandle;
			}
			if (this.m_grantingVisionForTeamA != grantingVisionForTeamA)
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
				this.Networkm_grantingVisionForTeamA = grantingVisionForTeamA;
				List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(Team.TeamA);
				using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						actorData.\u000E().MarkForRecalculateVisibility();
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
	}

	private void HookSetGrantingVisionForTeamB(bool grantingVisionForTeamB)
	{
		if (NetworkClient.active && this.m_grantingVisionForTeamB != grantingVisionForTeamB)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.HookSetGrantingVisionForTeamB(bool)).MethodHandle;
			}
			this.Networkm_grantingVisionForTeamB = grantingVisionForTeamB;
			List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(Team.TeamB);
			foreach (ActorData actorData in allTeamMembers)
			{
				actorData.\u000E().MarkForRecalculateVisibility();
			}
		}
	}

	private void UNetVersion()
	{
	}

	public int Networkm_currentProgressTugOfWar
	{
		get
		{
			return this.m_currentProgressTugOfWar;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_currentProgressTugOfWar, 1U);
		}
	}

	public int Networkm_progressTeamA
	{
		get
		{
			return this.m_progressTeamA;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_progressTeamA, 2U);
		}
	}

	public int Networkm_progressTeamB
	{
		get
		{
			return this.m_progressTeamB;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_progressTeamB, 4U);
		}
	}

	public int Networkm_controllingTeam
	{
		get
		{
			return this.m_controllingTeam;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_controllingTeam, 8U);
		}
	}

	public int Networkm_numObjectivePointsDispensed
	{
		get
		{
			return this.m_numObjectivePointsDispensed;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_numObjectivePointsDispensed, 0x10U);
		}
	}

	public ControlPoint.State Networkm_currentControlPointState
	{
		get
		{
			return this.m_currentControlPointState;
		}
		[param: In]
		set
		{
			uint dirtyBit = 0x20U;
			if (NetworkServer.localClientActive)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.set_Networkm_currentControlPointState(ControlPoint.State)).MethodHandle;
				}
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetCurrentState(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<ControlPoint.State>(value, ref this.m_currentControlPointState, dirtyBit);
		}
	}

	public int Networkm_lockedTurnsRemaining
	{
		get
		{
			return this.m_lockedTurnsRemaining;
		}
		[param: In]
		set
		{
			uint dirtyBit = 0x40U;
			if (NetworkServer.localClientActive)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.set_Networkm_lockedTurnsRemaining(int)).MethodHandle;
				}
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.SetLockedTurnsRemaining(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.m_lockedTurnsRemaining, dirtyBit);
		}
	}

	public int Networkm_numTeamAPlayers
	{
		get
		{
			return this.m_numTeamAPlayers;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_numTeamAPlayers, 0x80U);
		}
	}

	public int Networkm_numTeamBPlayers
	{
		get
		{
			return this.m_numTeamBPlayers;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_numTeamBPlayers, 0x100U);
		}
	}

	public bool Networkm_grantingVisionForTeamA
	{
		get
		{
			return this.m_grantingVisionForTeamA;
		}
		[param: In]
		set
		{
			uint dirtyBit = 0x200U;
			if (NetworkServer.localClientActive)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.set_Networkm_grantingVisionForTeamA(bool)).MethodHandle;
				}
				if (!base.syncVarHookGuard)
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
					base.syncVarHookGuard = true;
					this.HookSetGrantingVisionForTeamA(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<bool>(value, ref this.m_grantingVisionForTeamA, dirtyBit);
		}
	}

	public bool Networkm_grantingVisionForTeamB
	{
		get
		{
			return this.m_grantingVisionForTeamB;
		}
		[param: In]
		set
		{
			uint dirtyBit = 0x400U;
			if (NetworkServer.localClientActive)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.set_Networkm_grantingVisionForTeamB(bool)).MethodHandle;
				}
				if (!base.syncVarHookGuard)
				{
					base.syncVarHookGuard = true;
					this.HookSetGrantingVisionForTeamB(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<bool>(value, ref this.m_grantingVisionForTeamB, dirtyBit);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)this.m_currentProgressTugOfWar);
			writer.WritePackedUInt32((uint)this.m_progressTeamA);
			writer.WritePackedUInt32((uint)this.m_progressTeamB);
			writer.WritePackedUInt32((uint)this.m_controllingTeam);
			writer.WritePackedUInt32((uint)this.m_numObjectivePointsDispensed);
			writer.Write((int)this.m_currentControlPointState);
			writer.WritePackedUInt32((uint)this.m_lockedTurnsRemaining);
			writer.WritePackedUInt32((uint)this.m_numTeamAPlayers);
			writer.WritePackedUInt32((uint)this.m_numTeamBPlayers);
			writer.Write(this.m_grantingVisionForTeamA);
			writer.Write(this.m_grantingVisionForTeamB);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_currentProgressTugOfWar);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_progressTeamA);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_progressTeamB);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
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
				for (;;)
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
			writer.WritePackedUInt32((uint)this.m_controllingTeam);
		}
		if ((base.syncVarDirtyBits & 0x10U) != 0U)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_numObjectivePointsDispensed);
		}
		if ((base.syncVarDirtyBits & 0x20U) != 0U)
		{
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)this.m_currentControlPointState);
		}
		if ((base.syncVarDirtyBits & 0x40U) != 0U)
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
				for (;;)
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
			writer.WritePackedUInt32((uint)this.m_lockedTurnsRemaining);
		}
		if ((base.syncVarDirtyBits & 0x80U) != 0U)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_numTeamAPlayers);
		}
		if ((base.syncVarDirtyBits & 0x100U) != 0U)
		{
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_numTeamBPlayers);
		}
		if ((base.syncVarDirtyBits & 0x200U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_grantingVisionForTeamA);
		}
		if ((base.syncVarDirtyBits & 0x400U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_grantingVisionForTeamB);
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
			this.m_currentProgressTugOfWar = (int)reader.ReadPackedUInt32();
			this.m_progressTeamA = (int)reader.ReadPackedUInt32();
			this.m_progressTeamB = (int)reader.ReadPackedUInt32();
			this.m_controllingTeam = (int)reader.ReadPackedUInt32();
			this.m_numObjectivePointsDispensed = (int)reader.ReadPackedUInt32();
			this.m_currentControlPointState = (ControlPoint.State)reader.ReadInt32();
			this.m_lockedTurnsRemaining = (int)reader.ReadPackedUInt32();
			this.m_numTeamAPlayers = (int)reader.ReadPackedUInt32();
			this.m_numTeamBPlayers = (int)reader.ReadPackedUInt32();
			this.m_grantingVisionForTeamA = reader.ReadBoolean();
			this.m_grantingVisionForTeamB = reader.ReadBoolean();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_currentProgressTugOfWar = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ControlPoint.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_progressTeamA = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
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
			this.m_progressTeamB = (int)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
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
			this.m_controllingTeam = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x10) != 0)
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
			this.m_numObjectivePointsDispensed = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x20) != 0)
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
			this.HookSetCurrentState((ControlPoint.State)reader.ReadInt32());
		}
		if ((num & 0x40) != 0)
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
			this.SetLockedTurnsRemaining((int)reader.ReadPackedUInt32());
		}
		if ((num & 0x80) != 0)
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
			this.m_numTeamAPlayers = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x100) != 0)
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
			this.m_numTeamBPlayers = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x200) != 0)
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
			this.HookSetGrantingVisionForTeamA(reader.ReadBoolean());
		}
		if ((num & 0x400) != 0)
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
			this.HookSetGrantingVisionForTeamB(reader.ReadBoolean());
		}
	}

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
		public ControlPoint.CaptureMessageCondition condition;

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
}
