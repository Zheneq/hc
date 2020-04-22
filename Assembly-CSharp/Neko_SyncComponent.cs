using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Neko_SyncComponent : NetworkBehaviour, IForceActorOutlineChecker
{
	[Header("Return disc animation")]
	public int m_animIndexForStartOfDiscReturn;

	public int m_animIndexForPoweredUpDiscReturn;

	[Header("Return disc targeting")]
	public float m_discReturnTripLaserWidthInSquares = 1f;

	public float m_discReturnTripAoeRadiusAtlaserStart = 1f;

	public bool m_showTargetedGlowOnActorsInReturnDiscPath = true;

	public const int c_maxDiscLaserTemplates = 10;

	[Header("Indicator Colors")]
	public Color m_allyDiscIndicatorColor = Color.blue;

	public Color m_enemyDiscIndicatorColor = Color.red;

	public Color m_enlargedDiscEndpointColor = Color.blue;

	public Color m_returnDiscLineColor = Color.white;

	public Color m_fadeoutNonEnlargedDiscLineColor = Color.white;

	private SyncListInt m_boardX = new SyncListInt();

	private SyncListInt m_boardY = new SyncListInt();

	private float m_timeToWaitForValidationRequest;

	private const float c_waitDurationForValidation = 0.3f;

	[SyncVar]
	internal int m_homingActorIndex = -1;

	[SyncVar]
	internal bool m_superDiscActive;

	[SyncVar]
	internal int m_superDiscBoardX;

	[SyncVar]
	internal int m_superDiscBoardY;

	[SyncVar]
	internal int m_numUltConsecUsedTurns;

	internal int m_clientLastDiscBuffTurn = -1;

	internal BoardSquare m_clientDiscBuffTargetSquare;

	private const bool c_homingDiscStartFromCaster = false;

	private ActorData m_actorData;

	private AbilityData m_abilityData;

	private NekoBoomerangDisc m_primaryAbility;

	private NekoHomingDisc m_homingDiscAbility;

	private NekoEnlargeDisc m_enlargeDiscAbility;

	private AbilityData.ActionType m_enlargeDiscActionType = AbilityData.ActionType.INVALID_ACTION;

	private ActorTargeting m_actorTargeting;

	private bool m_showingTargeterTemplate;

	private List<Blaster_SyncComponent.HitAreaIndicatorHighlight> m_laserRangeMarkerForAlly;

	private List<GameObject> m_aoeRadiusMarkers;

	private GameObject m_endAoeMarker;

	private List<MeshRenderer[]> m_aoeMarkerRenderers;

	private List<ActorData> m_actorsTargetedByReturningDiscs = new List<ActorData>();

	private bool m_markedForForceUpdate;

	private bool m_setCasterPosLastFrame;

	private Vector3 m_lastCasterPos = Vector3.zero;

	private static int kListm_boardX;

	private static int kListm_boardY;

	public int Networkm_homingActorIndex
	{
		get
		{
			return m_homingActorIndex;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_homingActorIndex, 4u);
		}
	}

	public bool Networkm_superDiscActive
	{
		get
		{
			return m_superDiscActive;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_superDiscActive, 8u);
		}
	}

	public int Networkm_superDiscBoardX
	{
		get
		{
			return m_superDiscBoardX;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_superDiscBoardX, 16u);
		}
	}

	public int Networkm_superDiscBoardY
	{
		get
		{
			return m_superDiscBoardY;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_superDiscBoardY, 32u);
		}
	}

	public int Networkm_numUltConsecUsedTurns
	{
		get
		{
			return m_numUltConsecUsedTurns;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_numUltConsecUsedTurns, 64u);
		}
	}

	static Neko_SyncComponent()
	{
		kListm_boardX = 1782002628;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Neko_SyncComponent), kListm_boardX, InvokeSyncListm_boardX);
		kListm_boardY = 1782002629;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Neko_SyncComponent), kListm_boardY, InvokeSyncListm_boardY);
		NetworkCRC.RegisterBehaviour("Neko_SyncComponent", 0);
	}

	public static bool HomingDiscStartFromCaster()
	{
		return false;
	}

	public int GetNumActiveDiscs()
	{
		return m_boardX.Count;
	}

	public List<BoardSquare> GetActiveDiscSquares()
	{
		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = 0; i < m_boardX.Count; i++)
		{
			list.Add(GetSquareForDisc(i));
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
			return list;
		}
	}

	private BoardSquare GetSquareForDisc(int index)
	{
		return Board.Get().GetBoardSquare(m_boardX[index], m_boardY[index]);
	}

	public bool ShouldForceShowOutline(ActorData forActor)
	{
		if (NetworkClient.active && GameFlowData.Get().activeOwnedActorData == m_actorData)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					ActorTurnSM actorTurnSM = m_actorData.GetActorTurnSM();
					bool flag = true;
					if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
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
						flag = false;
						Ability selectedAbility = m_abilityData.GetSelectedAbility();
						if (selectedAbility != null)
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
							if (selectedAbility.GetRunPriority() == AbilityPriority.Evasion)
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
								if (selectedAbility is NekoFlipDash)
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
									NekoFlipDash nekoFlipDash = selectedAbility as NekoFlipDash;
									if (actorTurnSM.GetAbilityTargets().Count >= (nekoFlipDash.ThrowDiscFromStart() ? 1 : 0))
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
										flag = true;
									}
								}
								else
								{
									flag = true;
								}
							}
						}
					}
					int result;
					if (flag)
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
						result = (IsActorTargetedByReturningDiscs(forActor) ? 1 : 0);
					}
					else
					{
						result = 0;
					}
					return (byte)result != 0;
				}
				}
			}
		}
		return false;
	}

	private void Start()
	{
		m_actorData = GetComponent<ActorData>();
		m_abilityData = m_actorData.GetAbilityData();
		if (m_abilityData != null)
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
			m_primaryAbility = m_abilityData.GetAbilityOfType<NekoBoomerangDisc>();
			m_homingDiscAbility = m_abilityData.GetAbilityOfType<NekoHomingDisc>();
			m_enlargeDiscAbility = m_abilityData.GetAbilityOfType<NekoEnlargeDisc>();
			m_enlargeDiscActionType = m_abilityData.GetActionTypeOfAbility(m_enlargeDiscAbility);
		}
		m_actorTargeting = m_actorData.GetActorTargeting();
		if (!NetworkClient.active)
		{
			return;
		}
		m_laserRangeMarkerForAlly = new List<Blaster_SyncComponent.HitAreaIndicatorHighlight>(10);
		m_aoeRadiusMarkers = new List<GameObject>(10);
		m_aoeMarkerRenderers = new List<MeshRenderer[]>();
		for (int i = 0; i < 10; i++)
		{
			Blaster_SyncComponent.HitAreaIndicatorHighlight hitAreaIndicatorHighlight = Blaster_SyncComponent.CreateHitAreaTemplate(m_discReturnTripLaserWidthInSquares, m_returnDiscLineColor, false, 0.15f);
			hitAreaIndicatorHighlight.m_parentObj.SetActive(false);
			m_laserRangeMarkerForAlly.Add(hitAreaIndicatorHighlight);
			GameObject gameObject = HighlightUtils.Get().CreateDynamicConeMesh(m_discReturnTripAoeRadiusAtlaserStart, 360f, true);
			MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
			gameObject.SetActive(false);
			m_aoeRadiusMarkers.Add(gameObject);
			m_aoeMarkerRenderers.Add(componentsInChildren);
		}
		m_endAoeMarker = HighlightUtils.Get().CreateDynamicConeMesh(m_discReturnTripAoeRadiusAtlaserStart, 360f, true);
		MeshRenderer[] componentsInChildren2 = m_endAoeMarker.GetComponentsInChildren<MeshRenderer>();
		MeshRenderer[] array = componentsInChildren2;
		foreach (MeshRenderer meshRenderer in array)
		{
			AbilityUtil_Targeter.SetMaterialColor(meshRenderer.materials, m_enlargedDiscEndpointColor, false);
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (m_actorData != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					m_actorData.OnClientQueuedActionChangedDelegates += MarkForForceUpdate;
					m_actorData.OnSelectedAbilityChangedDelegates += OnSelectedAbilityChanged;
					m_actorData.AddForceShowOutlineChecker(this);
					GameFlowData.s_onActiveOwnedActorChange += OnActiveOwnedActorChange;
					return;
				}
			}
			return;
		}
	}

	private void OnDestroy()
	{
		if (!(m_actorData != null))
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
			m_actorData.OnClientQueuedActionChangedDelegates -= MarkForForceUpdate;
			m_actorData.OnSelectedAbilityChangedDelegates -= OnSelectedAbilityChanged;
			m_actorData.RemoveForceShowOutlineChecker(this);
			GameFlowData.s_onActiveOwnedActorChange -= OnActiveOwnedActorChange;
			return;
		}
	}

	private Vector3 GetDiscPos(int index)
	{
		if (index < m_boardX.Count)
		{
			BoardSquare squareForDisc = GetSquareForDisc(index);
			if (squareForDisc != null)
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
						return squareForDisc.ToVector3();
					}
				}
			}
		}
		return Vector3.zero;
	}

	private Vector3 GetCasterPos(out bool hasQueuedEvades)
	{
		hasQueuedEvades = false;
		if (m_actorData != null)
		{
			BoardSquare boardSquare = null;
			if (m_actorData.GetCurrentBoardSquare() != null)
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
				boardSquare = m_actorData.GetCurrentBoardSquare();
				List<AbilityData.AbilityEntry> queuedOrAimingAbilitiesForPhase = m_abilityData.GetQueuedOrAimingAbilitiesForPhase(UIQueueListPanel.UIPhase.Evasion);
				using (List<AbilityData.AbilityEntry>.Enumerator enumerator = queuedOrAimingAbilitiesForPhase.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityData.AbilityEntry current = enumerator.Current;
						AbilityData.ActionType actionTypeOfAbility = m_abilityData.GetActionTypeOfAbility(current.ability);
						List<AbilityTarget> abilityTargetsInRequest = m_actorTargeting.GetAbilityTargetsInRequest(actionTypeOfAbility);
						if (!abilityTargetsInRequest.IsNullOrEmpty())
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
							boardSquare = Board.Get().GetBoardSquareSafe(abilityTargetsInRequest[abilityTargetsInRequest.Count - 1].GridPos);
						}
						else if (m_actorData.GetActorTurnSM().GetAbilityTargets().Count == current.ability.GetNumTargets() - 1)
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
							AbilityTarget abilityTargetForTargeterUpdate = AbilityTarget.GetAbilityTargetForTargeterUpdate();
							boardSquare = Board.Get().GetBoardSquareSafe(abilityTargetForTargeterUpdate.GridPos);
						}
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
				hasQueuedEvades = (queuedOrAimingAbilitiesForPhase.Count > 0);
			}
			else if ((bool)m_actorData.GetMostResetDeathSquare())
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
				boardSquare = m_actorData.GetMostResetDeathSquare();
			}
			if (boardSquare != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return boardSquare.ToVector3();
					}
				}
			}
		}
		return Vector3.zero;
	}

	private Vector3 GetHomingActorPos()
	{
		ActorData actorData = m_actorData;
		if (m_homingActorIndex > 0)
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
			actorData = GameFlowData.Get().FindActorByActorIndex(m_homingActorIndex);
		}
		if (actorData != null)
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
			BoardSquare boardSquare = actorData.GetCurrentBoardSquare();
			if (actorData.IsDead())
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
				boardSquare = actorData.GetMostResetDeathSquare();
			}
			if (boardSquare != null)
			{
				return boardSquare.ToVector3();
			}
		}
		return Vector3.zero;
	}

	private bool IsDiscAtPosEnlarged(int discX, int discY, out bool enlargeDiscUsed)
	{
		bool result = false;
		enlargeDiscUsed = false;
		if (m_abilityData != null)
		{
			AbilityTarget abilityTarget = null;
			if (m_abilityData.HasQueuedAction(m_enlargeDiscActionType))
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
				List<AbilityTarget> abilityTargetsInRequest = m_actorTargeting.GetAbilityTargetsInRequest(m_enlargeDiscActionType);
				if (abilityTargetsInRequest != null)
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
					if (abilityTargetsInRequest.Count > 0)
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
						abilityTarget = abilityTargetsInRequest[0];
					}
				}
			}
			else
			{
				Ability selectedAbility = m_abilityData.GetSelectedAbility();
				if (selectedAbility != null && selectedAbility is NekoEnlargeDisc)
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
					abilityTarget = AbilityTarget.GetAbilityTargetForTargeterUpdate();
				}
			}
			if (abilityTarget != null)
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
				enlargeDiscUsed = true;
				if (m_homingActorIndex > 0)
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
					if (HomingDiscStartFromCaster())
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
						result = true;
						goto IL_0120;
					}
				}
				result = (abilityTarget.GridPos.x == discX && abilityTarget.GridPos.y == discY);
			}
		}
		goto IL_0120;
		IL_0120:
		return result;
	}

	private void Update()
	{
		if (!NetworkClient.active)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			bool setCasterPosLastFrame = m_setCasterPosLastFrame;
			m_setCasterPosLastFrame = false;
			bool flag = false;
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
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
				if (m_actorData.GetCurrentBoardSquare() != null)
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
					if (m_boardX.Count > 0 && GameFlowData.Get() != null && GameFlowData.Get().gameState == GameState.BothTeams_Decision)
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
						flag = true;
					}
				}
			}
			int count;
			float y;
			bool flag2;
			Vector3 vector;
			bool flag3;
			if (flag)
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
				count = m_boardX.Count;
				y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
				flag2 = (activeOwnedActorData.GetTeam() == m_actorData.GetTeam());
				vector = GetCasterPos(out bool hasQueuedEvades);
				flag3 = (m_actorData.GetActorTurnSM().CurrentState == TurnStateEnum.VALIDATING_ACTION_REQUEST);
				if (flag3)
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
					if (setCasterPosLastFrame)
					{
						vector = m_lastCasterPos;
						m_timeToWaitForValidationRequest = 0.3f;
						goto IL_01a4;
					}
				}
				if (m_timeToWaitForValidationRequest > 0f)
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
					m_timeToWaitForValidationRequest -= Time.unscaledDeltaTime;
				}
				if (!hasQueuedEvades)
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
					if (m_timeToWaitForValidationRequest > 0f)
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
						if (setCasterPosLastFrame)
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
							vector = m_lastCasterPos;
						}
					}
				}
				goto IL_01a4;
			}
			using (List<Blaster_SyncComponent.HitAreaIndicatorHighlight>.Enumerator enumerator = m_laserRangeMarkerForAlly.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Blaster_SyncComponent.HitAreaIndicatorHighlight current = enumerator.Current;
					current.SetVisible(false);
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
			foreach (GameObject aoeRadiusMarker in m_aoeRadiusMarkers)
			{
				aoeRadiusMarker.SetActive(false);
			}
			m_endAoeMarker.SetActive(false);
			m_actorsTargetedByReturningDiscs.Clear();
			m_timeToWaitForValidationRequest = 0f;
			goto IL_0803;
			IL_0803:
			m_showingTargeterTemplate = flag;
			return;
			IL_01a4:
			m_setCasterPosLastFrame = true;
			m_lastCasterPos = vector;
			Ability selectedAbility = m_abilityData.GetSelectedAbility();
			int num;
			if (selectedAbility != null)
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
				num = ((selectedAbility.GetRunPriority() == AbilityPriority.Evasion) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag4 = (byte)num != 0;
			int num2;
			if (selectedAbility != null)
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
				num2 = ((selectedAbility is NekoEnlargeDisc) ? 1 : 0);
			}
			else
			{
				num2 = 0;
			}
			bool flag5 = (byte)num2 != 0;
			ActorData actorData;
			if (m_homingActorIndex < 0)
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
				actorData = m_actorData;
			}
			else
			{
				actorData = GameplayUtils.GetActorOfActorIndex(m_homingActorIndex);
			}
			ActorData actorData2 = actorData;
			bool flag6 = actorData2 != null;
			if (actorData2 != null)
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
				if (actorData2.GetTeam() != m_actorData.GetTeam())
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
					int num3;
					if (actorData2.GetCurrentBoardSquare() != null)
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
						num3 = (actorData2.IsVisibleToClient() ? 1 : 0);
					}
					else
					{
						num3 = 0;
					}
					flag6 = ((byte)num3 != 0);
				}
			}
			if (m_showingTargeterTemplate)
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
				if (m_showingTargeterTemplate == flag6)
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
					if (!m_markedForForceUpdate)
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
						if (!flag3 && !flag4)
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
							if (!flag5)
							{
								goto IL_0803;
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
				}
			}
			m_markedForForceUpdate = false;
			m_actorsTargetedByReturningDiscs.Clear();
			for (int i = 0; i < m_laserRangeMarkerForAlly.Count; i++)
			{
				Blaster_SyncComponent.HitAreaIndicatorHighlight hitAreaIndicatorHighlight = m_laserRangeMarkerForAlly[i];
				GameObject gameObject = m_aoeRadiusMarkers[i];
				MeshRenderer[] array = m_aoeMarkerRenderers[i];
				int visible;
				if (flag2)
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
					visible = ((i < m_boardX.Count) ? 1 : 0);
				}
				else
				{
					visible = 0;
				}
				hitAreaIndicatorHighlight.SetVisible((byte)visible != 0);
				int active;
				if (HomingDiscStartFromCaster())
				{
					if (m_homingActorIndex > 0)
					{
						active = 0;
						goto IL_039c;
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				active = ((i < m_boardX.Count) ? 1 : 0);
				goto IL_039c;
				IL_0703:
				bool flag7 = flag7 && flag2;
				if (flag7 != m_endAoeMarker.activeSelf)
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
					m_endAoeMarker.SetActive(flag7);
				}
				continue;
				IL_039c:
				gameObject.SetActive((byte)active != 0);
				if (i >= m_boardX.Count)
				{
					continue;
				}
				Vector3 vector2;
				Vector3 vector3;
				if (m_homingActorIndex > 0)
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
					if (HomingDiscStartFromCaster())
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
						vector2 = vector;
					}
					else
					{
						vector2 = GetDiscPos(i);
					}
					vector2.y = y;
					vector3 = GetHomingActorPos();
					vector3.y = y;
				}
				else
				{
					vector2 = GetDiscPos(i);
					vector2.y = y;
					vector3 = vector;
					vector3.y = y;
				}
				Vector3 position = vector2;
				position.y = HighlightUtils.GetHighlightHeight();
				if (!flag2)
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
					position.y += 0.01f;
				}
				gameObject.transform.position = position;
				MeshRenderer[] array2 = array;
				foreach (MeshRenderer meshRenderer in array2)
				{
					AbilityUtil_Targeter.SetMaterialColor(meshRenderer.materials, (!flag2) ? m_enemyDiscIndicatorColor : m_allyDiscIndicatorColor, false);
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				flag7 = false;
				float num4;
				float aoeStartRadius;
				float num5;
				bool enlargeDiscUsed;
				Vector3 adjustedStartPosWithOffset;
				Vector3 vector4;
				float num6;
				int num7;
				if (flag6)
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
					num4 = m_discReturnTripLaserWidthInSquares;
					aoeStartRadius = m_discReturnTripAoeRadiusAtlaserStart;
					num5 = 0f;
					if (m_homingActorIndex > 0 && m_homingDiscAbility != null)
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
						num5 = m_homingDiscAbility.GetDiscReturnEndRadius();
					}
					else if (m_homingActorIndex < 0)
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
						if (m_primaryAbility != null)
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
							num5 = m_primaryAbility.GetDiscReturnEndRadius();
						}
					}
					bool flag8 = IsDiscAtPosEnlarged(m_boardX[i], m_boardY[i], out enlargeDiscUsed);
					if (flag8)
					{
						num4 = m_enlargeDiscAbility.GetLaserWidth();
						aoeStartRadius = m_enlargeDiscAbility.GetAoeRadius();
						num5 = Mathf.Max(num5, m_enlargeDiscAbility.GetReturnEndAoeRadius());
					}
					if (num5 > 0f)
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
						Vector3 position2 = vector3;
						position2.y = HighlightUtils.GetHighlightHeight();
						m_endAoeMarker.transform.position = position2;
						HighlightUtils.Get().AdjustDynamicConeMesh(m_endAoeMarker, num5, 360f);
						flag7 = true;
					}
					float laserInitialOffsetInSquares = GameWideData.Get().m_laserInitialOffsetInSquares;
					adjustedStartPosWithOffset = VectorUtils.GetAdjustedStartPosWithOffset(vector2, vector3, laserInitialOffsetInSquares);
					adjustedStartPosWithOffset.y = (float)Board.Get().BaselineHeight + 0.01f;
					vector4 = vector3 - vector2;
					vector4.y = 0f;
					num6 = vector4.magnitude / Board.Get().squareSize;
					if (count > 1)
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
						if (enlargeDiscUsed)
						{
							num7 = ((!flag8) ? 1 : 0);
							goto IL_0681;
						}
					}
					num7 = 0;
					goto IL_0681;
				}
				hitAreaIndicatorHighlight.SetVisible(false);
				goto IL_0703;
				IL_0681:
				if (num7 != 0)
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
					hitAreaIndicatorHighlight.m_color = m_fadeoutNonEnlargedDiscLineColor;
				}
				else
				{
					hitAreaIndicatorHighlight.m_color = m_returnDiscLineColor;
				}
				if (num6 > 0f)
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
					hitAreaIndicatorHighlight.SetPose(adjustedStartPosWithOffset, vector4.normalized);
					hitAreaIndicatorHighlight.AdjustSize(num4, num6);
				}
				else
				{
					hitAreaIndicatorHighlight.SetVisible(false);
				}
				UpdateActorsInDiscPath(vector2, vector3, num4, aoeStartRadius, num5, enlargeDiscUsed);
				goto IL_0703;
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
			goto IL_0803;
		}
	}

	private void UpdateActorsInDiscPath(Vector3 startLosPos, Vector3 endLosPos, float laserWidth, float aoeStartRadius, float aoeEndRadius, bool usingEnlargeDiscAbility)
	{
		if (!m_showTargetedGlowOnActorsInReturnDiscPath)
		{
			return;
		}
		List<Team> opposingTeams = m_actorData.GetOpposingTeams();
		if (usingEnlargeDiscAbility)
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
			if (m_enlargeDiscAbility != null)
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
				if (m_enlargeDiscAbility.CanIncludeAlliesOnReturn())
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
					opposingTeams.Add(m_actorData.GetTeam());
				}
			}
		}
		Vector3 dir = endLosPos - startLosPos;
		dir.y = 0f;
		float num = dir.magnitude / Board.Get().squareSize;
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(startLosPos, aoeStartRadius, true, m_actorData, opposingTeams, null);
		using (List<ActorData>.Enumerator enumerator = actorsInRadius.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (!m_actorsTargetedByReturningDiscs.Contains(current))
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
					m_actorsTargetedByReturningDiscs.Add(current);
				}
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
		if (!(num > 0f))
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
			Vector3 laserEndPos;
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(startLosPos, dir, num, laserWidth, m_actorData, opposingTeams, true, 0, true, false, out laserEndPos, null);
			foreach (ActorData item in actorsInLaser)
			{
				if (!m_actorsTargetedByReturningDiscs.Contains(item))
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
					m_actorsTargetedByReturningDiscs.Add(item);
				}
			}
			if (aoeEndRadius > 0f)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					List<ActorData> actorsInRadius2 = AreaEffectUtils.GetActorsInRadius(endLosPos, aoeEndRadius, true, m_actorData, opposingTeams, null);
					foreach (ActorData item2 in actorsInRadius2)
					{
						if (!m_actorsTargetedByReturningDiscs.Contains(item2))
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
							m_actorsTargetedByReturningDiscs.Add(item2);
						}
					}
					return;
				}
			}
			return;
		}
	}

	public bool IsActorTargetedByReturningDiscs(ActorData actor)
	{
		return m_actorsTargetedByReturningDiscs.Contains(actor);
	}

	public void MarkForForceUpdate()
	{
		m_markedForForceUpdate = true;
		m_timeToWaitForValidationRequest = 0f;
	}

	public void OnSelectedAbilityChanged(Ability ability)
	{
		m_markedForForceUpdate = true;
	}

	private void OnActiveOwnedActorChange(ActorData activeActor)
	{
		m_markedForForceUpdate = true;
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_boardX(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
					Debug.LogError("SyncList m_boardX called on server.");
					return;
				}
			}
		}
		((Neko_SyncComponent)obj).m_boardX.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_boardY(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_boardY called on server.");
		}
		else
		{
			((Neko_SyncComponent)obj).m_boardY.HandleMsg(reader);
		}
	}

	private void Awake()
	{
		m_boardX.InitializeBehaviour(this, kListm_boardX);
		m_boardY.InitializeBehaviour(this, kListm_boardY);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
					SyncListInt.WriteInstance(writer, m_boardX);
					SyncListInt.WriteInstance(writer, m_boardY);
					writer.WritePackedUInt32((uint)m_homingActorIndex);
					writer.Write(m_superDiscActive);
					writer.WritePackedUInt32((uint)m_superDiscBoardX);
					writer.WritePackedUInt32((uint)m_superDiscBoardY);
					writer.WritePackedUInt32((uint)m_numUltConsecUsedTurns);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
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
			SyncListInt.WriteInstance(writer, m_boardX);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
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
			SyncListInt.WriteInstance(writer, m_boardY);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
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
			writer.WritePackedUInt32((uint)m_homingActorIndex);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
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
			writer.Write(m_superDiscActive);
		}
		if ((base.syncVarDirtyBits & 0x10) != 0)
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
			writer.WritePackedUInt32((uint)m_superDiscBoardX);
		}
		if ((base.syncVarDirtyBits & 0x20) != 0)
		{
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
			writer.WritePackedUInt32((uint)m_superDiscBoardY);
		}
		if ((base.syncVarDirtyBits & 0x40) != 0)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_numUltConsecUsedTurns);
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
					SyncListInt.ReadReference(reader, m_boardX);
					SyncListInt.ReadReference(reader, m_boardY);
					m_homingActorIndex = (int)reader.ReadPackedUInt32();
					m_superDiscActive = reader.ReadBoolean();
					m_superDiscBoardX = (int)reader.ReadPackedUInt32();
					m_superDiscBoardY = (int)reader.ReadPackedUInt32();
					m_numUltConsecUsedTurns = (int)reader.ReadPackedUInt32();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
			SyncListInt.ReadReference(reader, m_boardX);
		}
		if ((num & 2) != 0)
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
			SyncListInt.ReadReference(reader, m_boardY);
		}
		if ((num & 4) != 0)
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
			m_homingActorIndex = (int)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			m_superDiscActive = reader.ReadBoolean();
		}
		if ((num & 0x10) != 0)
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
			m_superDiscBoardX = (int)reader.ReadPackedUInt32();
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
			m_superDiscBoardY = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x40) == 0)
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
			m_numUltConsecUsedTurns = (int)reader.ReadPackedUInt32();
			return;
		}
	}
}
