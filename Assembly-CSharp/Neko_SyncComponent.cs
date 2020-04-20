using System;
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

	public const int c_maxDiscLaserTemplates = 0xA;

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

	private static int kListm_boardX = 0x6A3733C4;

	private static int kListm_boardY;

	static Neko_SyncComponent()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Neko_SyncComponent), Neko_SyncComponent.kListm_boardX, new NetworkBehaviour.CmdDelegate(Neko_SyncComponent.InvokeSyncListm_boardX));
		Neko_SyncComponent.kListm_boardY = 0x6A3733C5;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Neko_SyncComponent), Neko_SyncComponent.kListm_boardY, new NetworkBehaviour.CmdDelegate(Neko_SyncComponent.InvokeSyncListm_boardY));
		NetworkCRC.RegisterBehaviour("Neko_SyncComponent", 0);
	}

	public static bool HomingDiscStartFromCaster()
	{
		return false;
	}

	public int GetNumActiveDiscs()
	{
		return this.m_boardX.Count;
	}

	public List<BoardSquare> GetActiveDiscSquares()
	{
		List<BoardSquare> list = new List<BoardSquare>();
		for (int i = 0; i < this.m_boardX.Count; i++)
		{
			list.Add(this.GetSquareForDisc(i));
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_SyncComponent.GetActiveDiscSquares()).MethodHandle;
		}
		return list;
	}

	private BoardSquare GetSquareForDisc(int index)
	{
		return Board.Get().GetBoardSquare(this.m_boardX[index], this.m_boardY[index]);
	}

	public bool ShouldForceShowOutline(ActorData forActor)
	{
		if (NetworkClient.active && GameFlowData.Get().activeOwnedActorData == this.m_actorData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_SyncComponent.ShouldForceShowOutline(ActorData)).MethodHandle;
			}
			ActorTurnSM actorTurnSM = this.m_actorData.GetActorTurnSM();
			bool flag = true;
			if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
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
				flag = false;
				Ability selectedAbility = this.m_abilityData.GetSelectedAbility();
				if (selectedAbility != null)
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
					if (selectedAbility.GetRunPriority() == AbilityPriority.Evasion)
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
						if (selectedAbility is NekoFlipDash)
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
							NekoFlipDash nekoFlipDash = selectedAbility as NekoFlipDash;
							if (actorTurnSM.GetAbilityTargets().Count >= ((!nekoFlipDash.ThrowDiscFromStart()) ? 0 : 1))
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
				result = this.IsActorTargetedByReturningDiscs(forActor);
			}
			else
			{
				result = false;
			}
			return result;
		}
		return false;
	}

	private void Start()
	{
		this.m_actorData = base.GetComponent<ActorData>();
		this.m_abilityData = this.m_actorData.GetAbilityData();
		if (this.m_abilityData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_SyncComponent.Start()).MethodHandle;
			}
			this.m_primaryAbility = this.m_abilityData.GetAbilityOfType<NekoBoomerangDisc>();
			this.m_homingDiscAbility = this.m_abilityData.GetAbilityOfType<NekoHomingDisc>();
			this.m_enlargeDiscAbility = this.m_abilityData.GetAbilityOfType<NekoEnlargeDisc>();
			this.m_enlargeDiscActionType = this.m_abilityData.GetActionTypeOfAbility(this.m_enlargeDiscAbility);
		}
		this.m_actorTargeting = this.m_actorData.GetActorTargeting();
		if (NetworkClient.active)
		{
			this.m_laserRangeMarkerForAlly = new List<Blaster_SyncComponent.HitAreaIndicatorHighlight>(0xA);
			this.m_aoeRadiusMarkers = new List<GameObject>(0xA);
			this.m_aoeMarkerRenderers = new List<MeshRenderer[]>();
			for (int i = 0; i < 0xA; i++)
			{
				Blaster_SyncComponent.HitAreaIndicatorHighlight hitAreaIndicatorHighlight = Blaster_SyncComponent.CreateHitAreaTemplate(this.m_discReturnTripLaserWidthInSquares, this.m_returnDiscLineColor, false, 0.15f);
				hitAreaIndicatorHighlight.m_parentObj.SetActive(false);
				this.m_laserRangeMarkerForAlly.Add(hitAreaIndicatorHighlight);
				GameObject gameObject = HighlightUtils.Get().CreateDynamicConeMesh(this.m_discReturnTripAoeRadiusAtlaserStart, 360f, true, null);
				MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
				gameObject.SetActive(false);
				this.m_aoeRadiusMarkers.Add(gameObject);
				this.m_aoeMarkerRenderers.Add(componentsInChildren);
			}
			this.m_endAoeMarker = HighlightUtils.Get().CreateDynamicConeMesh(this.m_discReturnTripAoeRadiusAtlaserStart, 360f, true, null);
			MeshRenderer[] componentsInChildren2 = this.m_endAoeMarker.GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer meshRenderer in componentsInChildren2)
			{
				AbilityUtil_Targeter.SetMaterialColor(meshRenderer.materials, this.m_enlargedDiscEndpointColor, false);
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
			if (this.m_actorData != null)
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
				this.m_actorData.OnClientQueuedActionChangedDelegates += this.MarkForForceUpdate;
				this.m_actorData.OnSelectedAbilityChangedDelegates += this.OnSelectedAbilityChanged;
				this.m_actorData.AddForceShowOutlineChecker(this);
				GameFlowData.s_onActiveOwnedActorChange += this.OnActiveOwnedActorChange;
			}
		}
	}

	private void OnDestroy()
	{
		if (this.m_actorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_SyncComponent.OnDestroy()).MethodHandle;
			}
			this.m_actorData.OnClientQueuedActionChangedDelegates -= this.MarkForForceUpdate;
			this.m_actorData.OnSelectedAbilityChangedDelegates -= this.OnSelectedAbilityChanged;
			this.m_actorData.RemoveForceShowOutlineChecker(this);
			GameFlowData.s_onActiveOwnedActorChange -= this.OnActiveOwnedActorChange;
		}
	}

	private Vector3 GetDiscPos(int index)
	{
		if (index < this.m_boardX.Count)
		{
			BoardSquare squareForDisc = this.GetSquareForDisc(index);
			if (squareForDisc != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_SyncComponent.GetDiscPos(int)).MethodHandle;
				}
				return squareForDisc.ToVector3();
			}
		}
		return Vector3.zero;
	}

	private unsafe Vector3 GetCasterPos(out bool hasQueuedEvades)
	{
		hasQueuedEvades = false;
		if (this.m_actorData != null)
		{
			BoardSquare boardSquare = null;
			if (this.m_actorData.GetCurrentBoardSquare() != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_SyncComponent.GetCasterPos(bool*)).MethodHandle;
				}
				boardSquare = this.m_actorData.GetCurrentBoardSquare();
				List<AbilityData.AbilityEntry> queuedOrAimingAbilitiesForPhase = this.m_abilityData.GetQueuedOrAimingAbilitiesForPhase(UIQueueListPanel.UIPhase.Evasion);
				using (List<AbilityData.AbilityEntry>.Enumerator enumerator = queuedOrAimingAbilitiesForPhase.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						AbilityData.AbilityEntry abilityEntry = enumerator.Current;
						AbilityData.ActionType actionTypeOfAbility = this.m_abilityData.GetActionTypeOfAbility(abilityEntry.ability);
						List<AbilityTarget> abilityTargetsInRequest = this.m_actorTargeting.GetAbilityTargetsInRequest(actionTypeOfAbility);
						if (!abilityTargetsInRequest.IsNullOrEmpty<AbilityTarget>())
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
							boardSquare = Board.Get().GetBoardSquareSafe(abilityTargetsInRequest[abilityTargetsInRequest.Count - 1].GridPos);
						}
						else if (this.m_actorData.GetActorTurnSM().GetAbilityTargets().Count == abilityEntry.ability.GetNumTargets() - 1)
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
							AbilityTarget abilityTargetForTargeterUpdate = AbilityTarget.GetAbilityTargetForTargeterUpdate();
							boardSquare = Board.Get().GetBoardSquareSafe(abilityTargetForTargeterUpdate.GridPos);
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
				hasQueuedEvades = (queuedOrAimingAbilitiesForPhase.Count > 0);
			}
			else if (this.m_actorData.GetMostResetDeathSquare())
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
				boardSquare = this.m_actorData.GetMostResetDeathSquare();
			}
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
				return boardSquare.ToVector3();
			}
		}
		return Vector3.zero;
	}

	private Vector3 GetHomingActorPos()
	{
		ActorData actorData = this.m_actorData;
		if (this.m_homingActorIndex > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_SyncComponent.GetHomingActorPos()).MethodHandle;
			}
			actorData = GameFlowData.Get().FindActorByActorIndex(this.m_homingActorIndex);
		}
		if (actorData != null)
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
			BoardSquare boardSquare = actorData.GetCurrentBoardSquare();
			if (actorData.IsDead())
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
				boardSquare = actorData.GetMostResetDeathSquare();
			}
			if (boardSquare != null)
			{
				return boardSquare.ToVector3();
			}
		}
		return Vector3.zero;
	}

	private unsafe bool IsDiscAtPosEnlarged(int discX, int discY, out bool enlargeDiscUsed)
	{
		bool result = false;
		enlargeDiscUsed = false;
		if (this.m_abilityData != null)
		{
			AbilityTarget abilityTarget = null;
			bool flag = this.m_abilityData.HasQueuedAction(this.m_enlargeDiscActionType);
			if (flag)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_SyncComponent.IsDiscAtPosEnlarged(int, int, bool*)).MethodHandle;
				}
				List<AbilityTarget> abilityTargetsInRequest = this.m_actorTargeting.GetAbilityTargetsInRequest(this.m_enlargeDiscActionType);
				if (abilityTargetsInRequest != null)
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
					if (abilityTargetsInRequest.Count > 0)
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
						abilityTarget = abilityTargetsInRequest[0];
					}
				}
			}
			else
			{
				Ability selectedAbility = this.m_abilityData.GetSelectedAbility();
				if (selectedAbility != null && selectedAbility is NekoEnlargeDisc)
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
					abilityTarget = AbilityTarget.GetAbilityTargetForTargeterUpdate();
				}
			}
			if (abilityTarget != null)
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
				enlargeDiscUsed = true;
				if (this.m_homingActorIndex > 0)
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
					if (Neko_SyncComponent.HomingDiscStartFromCaster())
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
				}
				result = (abilityTarget.GridPos.x == discX && abilityTarget.GridPos.y == discY);
			}
		}
		return result;
	}

	private void Update()
	{
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_SyncComponent.Update()).MethodHandle;
			}
			bool setCasterPosLastFrame = this.m_setCasterPosLastFrame;
			this.m_setCasterPosLastFrame = false;
			bool flag = false;
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
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
				if (this.m_actorData.GetCurrentBoardSquare() != null)
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
					if (this.m_boardX.Count > 0 && GameFlowData.Get() != null && GameFlowData.Get().gameState == GameState.BothTeams_Decision)
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
						flag = true;
					}
				}
			}
			if (flag)
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
				int count = this.m_boardX.Count;
				float y = (float)Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
				bool flag2 = activeOwnedActorData.GetTeam() == this.m_actorData.GetTeam();
				bool flag3;
				Vector3 vector = this.GetCasterPos(out flag3);
				bool flag4 = this.m_actorData.GetActorTurnSM().CurrentState == TurnStateEnum.VALIDATING_ACTION_REQUEST;
				if (flag4)
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
					if (setCasterPosLastFrame)
					{
						vector = this.m_lastCasterPos;
						this.m_timeToWaitForValidationRequest = 0.3f;
						goto IL_1A4;
					}
				}
				if (this.m_timeToWaitForValidationRequest > 0f)
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
					this.m_timeToWaitForValidationRequest -= Time.unscaledDeltaTime;
				}
				if (!flag3)
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
					if (this.m_timeToWaitForValidationRequest > 0f)
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
						if (setCasterPosLastFrame)
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
							vector = this.m_lastCasterPos;
						}
					}
				}
				IL_1A4:
				this.m_setCasterPosLastFrame = true;
				this.m_lastCasterPos = vector;
				Ability selectedAbility = this.m_abilityData.GetSelectedAbility();
				bool flag5;
				if (selectedAbility != null)
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
					flag5 = (selectedAbility.GetRunPriority() == AbilityPriority.Evasion);
				}
				else
				{
					flag5 = false;
				}
				bool flag6 = flag5;
				bool flag7;
				if (selectedAbility != null)
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
					flag7 = (selectedAbility is NekoEnlargeDisc);
				}
				else
				{
					flag7 = false;
				}
				bool flag8 = flag7;
				ActorData actorData;
				if (this.m_homingActorIndex < 0)
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
					actorData = this.m_actorData;
				}
				else
				{
					actorData = GameplayUtils.GetActorOfActorIndex(this.m_homingActorIndex);
				}
				ActorData actorData2 = actorData;
				bool flag9 = actorData2 != null;
				if (actorData2 != null)
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
					if (actorData2.GetTeam() != this.m_actorData.GetTeam())
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
						bool flag10;
						if (actorData2.GetCurrentBoardSquare() != null)
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
							flag10 = actorData2.IsVisibleToClient();
						}
						else
						{
							flag10 = false;
						}
						flag9 = flag10;
					}
				}
				if (this.m_showingTargeterTemplate)
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
					if (this.m_showingTargeterTemplate == flag9)
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
						if (!this.m_markedForForceUpdate)
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
							if (!flag4 && !flag6)
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
								if (!flag8)
								{
									goto IL_754;
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
					}
				}
				this.m_markedForForceUpdate = false;
				this.m_actorsTargetedByReturningDiscs.Clear();
				int i = 0;
				while (i < this.m_laserRangeMarkerForAlly.Count)
				{
					Blaster_SyncComponent.HitAreaIndicatorHighlight hitAreaIndicatorHighlight = this.m_laserRangeMarkerForAlly[i];
					GameObject gameObject = this.m_aoeRadiusMarkers[i];
					MeshRenderer[] array = this.m_aoeMarkerRenderers[i];
					Blaster_SyncComponent.HitAreaIndicatorHighlight hitAreaIndicatorHighlight2 = hitAreaIndicatorHighlight;
					bool visible;
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
						visible = (i < this.m_boardX.Count);
					}
					else
					{
						visible = false;
					}
					hitAreaIndicatorHighlight2.SetVisible(visible);
					GameObject gameObject2 = gameObject;
					if (!Neko_SyncComponent.HomingDiscStartFromCaster())
					{
						goto IL_38A;
					}
					bool active;
					if (this.m_homingActorIndex <= 0)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							goto IL_38A;
						}
					}
					else
					{
						active = false;
					}
					IL_39C:
					gameObject2.SetActive(active);
					if (i < this.m_boardX.Count)
					{
						Vector3 vector2;
						Vector3 vector3;
						if (this.m_homingActorIndex > 0)
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
							if (Neko_SyncComponent.HomingDiscStartFromCaster())
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
								vector2 = vector;
							}
							else
							{
								vector2 = this.GetDiscPos(i);
							}
							vector2.y = y;
							vector3 = this.GetHomingActorPos();
							vector3.y = y;
						}
						else
						{
							vector2 = this.GetDiscPos(i);
							vector2.y = y;
							vector3 = vector;
							vector3.y = y;
						}
						Vector3 position = vector2;
						position.y = HighlightUtils.GetHighlightHeight();
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
							position.y += 0.01f;
						}
						gameObject.transform.position = position;
						foreach (MeshRenderer meshRenderer in array)
						{
							AbilityUtil_Targeter.SetMaterialColor(meshRenderer.materials, (!flag2) ? this.m_enemyDiscIndicatorColor : this.m_allyDiscIndicatorColor, false);
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
						bool flag11 = false;
						if (flag9)
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
							float num = this.m_discReturnTripLaserWidthInSquares;
							float aoeStartRadius = this.m_discReturnTripAoeRadiusAtlaserStart;
							float num2 = 0f;
							if (this.m_homingActorIndex > 0 && this.m_homingDiscAbility != null)
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
								num2 = this.m_homingDiscAbility.GetDiscReturnEndRadius();
							}
							else if (this.m_homingActorIndex < 0)
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
								if (this.m_primaryAbility != null)
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
									num2 = this.m_primaryAbility.GetDiscReturnEndRadius();
								}
							}
							bool flag13;
							bool flag12 = this.IsDiscAtPosEnlarged(this.m_boardX[i], this.m_boardY[i], out flag13);
							if (flag12)
							{
								num = this.m_enlargeDiscAbility.GetLaserWidth();
								aoeStartRadius = this.m_enlargeDiscAbility.GetAoeRadius();
								num2 = Mathf.Max(num2, this.m_enlargeDiscAbility.GetReturnEndAoeRadius());
							}
							if (num2 > 0f)
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
								Vector3 position2 = vector3;
								position2.y = HighlightUtils.GetHighlightHeight();
								this.m_endAoeMarker.transform.position = position2;
								HighlightUtils.Get().AdjustDynamicConeMesh(this.m_endAoeMarker, num2, 360f);
								flag11 = true;
							}
							float laserInitialOffsetInSquares = GameWideData.Get().m_laserInitialOffsetInSquares;
							Vector3 adjustedStartPosWithOffset = VectorUtils.GetAdjustedStartPosWithOffset(vector2, vector3, laserInitialOffsetInSquares);
							adjustedStartPosWithOffset.y = (float)Board.Get().BaselineHeight + 0.01f;
							Vector3 vector4 = vector3 - vector2;
							vector4.y = 0f;
							float num3 = vector4.magnitude / Board.Get().squareSize;
							if (count <= 1)
							{
								goto IL_680;
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
							if (!flag13)
							{
								goto IL_680;
							}
							bool flag14 = !flag12;
							IL_681:
							bool flag15 = flag14;
							if (flag15)
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
								hitAreaIndicatorHighlight.m_color = this.m_fadeoutNonEnlargedDiscLineColor;
							}
							else
							{
								hitAreaIndicatorHighlight.m_color = this.m_returnDiscLineColor;
							}
							if (num3 > 0f)
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
								hitAreaIndicatorHighlight.SetPose(adjustedStartPosWithOffset, vector4.normalized);
								hitAreaIndicatorHighlight.AdjustSize(num, num3);
							}
							else
							{
								hitAreaIndicatorHighlight.SetVisible(false);
							}
							this.UpdateActorsInDiscPath(vector2, vector3, num, aoeStartRadius, num2, flag13);
							goto IL_703;
							IL_680:
							flag14 = false;
							goto IL_681;
						}
						hitAreaIndicatorHighlight.SetVisible(false);
						IL_703:
						flag11 = (flag11 && flag2);
						if (flag11 != this.m_endAoeMarker.activeSelf)
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
							this.m_endAoeMarker.SetActive(flag11);
						}
					}
					i++;
					continue;
					IL_38A:
					active = (i < this.m_boardX.Count);
					goto IL_39C;
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
				IL_754:;
			}
			else
			{
				using (List<Blaster_SyncComponent.HitAreaIndicatorHighlight>.Enumerator enumerator = this.m_laserRangeMarkerForAlly.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Blaster_SyncComponent.HitAreaIndicatorHighlight hitAreaIndicatorHighlight3 = enumerator.Current;
						hitAreaIndicatorHighlight3.SetVisible(false);
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
				foreach (GameObject gameObject3 in this.m_aoeRadiusMarkers)
				{
					gameObject3.SetActive(false);
				}
				this.m_endAoeMarker.SetActive(false);
				this.m_actorsTargetedByReturningDiscs.Clear();
				this.m_timeToWaitForValidationRequest = 0f;
			}
			this.m_showingTargeterTemplate = flag;
		}
	}

	private void UpdateActorsInDiscPath(Vector3 startLosPos, Vector3 endLosPos, float laserWidth, float aoeStartRadius, float aoeEndRadius, bool usingEnlargeDiscAbility)
	{
		if (this.m_showTargetedGlowOnActorsInReturnDiscPath)
		{
			List<Team> opposingTeams = this.m_actorData.GetOpposingTeams();
			if (usingEnlargeDiscAbility)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_SyncComponent.UpdateActorsInDiscPath(Vector3, Vector3, float, float, float, bool)).MethodHandle;
				}
				if (this.m_enlargeDiscAbility != null)
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
					if (this.m_enlargeDiscAbility.CanIncludeAlliesOnReturn())
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
						opposingTeams.Add(this.m_actorData.GetTeam());
					}
				}
			}
			Vector3 dir = endLosPos - startLosPos;
			dir.y = 0f;
			float num = dir.magnitude / Board.Get().squareSize;
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(startLosPos, aoeStartRadius, true, this.m_actorData, opposingTeams, null, false, default(Vector3));
			using (List<ActorData>.Enumerator enumerator = actorsInRadius.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData item = enumerator.Current;
					if (!this.m_actorsTargetedByReturningDiscs.Contains(item))
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
						this.m_actorsTargetedByReturningDiscs.Add(item);
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
			if (num > 0f)
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
				Vector3 vector;
				List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(startLosPos, dir, num, laserWidth, this.m_actorData, opposingTeams, true, 0, true, false, out vector, null, null, false, true);
				foreach (ActorData item2 in actorsInLaser)
				{
					if (!this.m_actorsTargetedByReturningDiscs.Contains(item2))
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
						this.m_actorsTargetedByReturningDiscs.Add(item2);
					}
				}
				if (aoeEndRadius > 0f)
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
					List<ActorData> actorsInRadius2 = AreaEffectUtils.GetActorsInRadius(endLosPos, aoeEndRadius, true, this.m_actorData, opposingTeams, null, false, default(Vector3));
					foreach (ActorData item3 in actorsInRadius2)
					{
						if (!this.m_actorsTargetedByReturningDiscs.Contains(item3))
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
							this.m_actorsTargetedByReturningDiscs.Add(item3);
						}
					}
				}
			}
		}
	}

	public bool IsActorTargetedByReturningDiscs(ActorData actor)
	{
		return this.m_actorsTargetedByReturningDiscs.Contains(actor);
	}

	public void MarkForForceUpdate()
	{
		this.m_markedForForceUpdate = true;
		this.m_timeToWaitForValidationRequest = 0f;
	}

	public void OnSelectedAbilityChanged(Ability ability)
	{
		this.m_markedForForceUpdate = true;
	}

	private void OnActiveOwnedActorChange(ActorData activeActor)
	{
		this.m_markedForForceUpdate = true;
	}

	private void UNetVersion()
	{
	}

	public int Networkm_homingActorIndex
	{
		get
		{
			return this.m_homingActorIndex;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_homingActorIndex, 4U);
		}
	}

	public bool Networkm_superDiscActive
	{
		get
		{
			return this.m_superDiscActive;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_superDiscActive, 8U);
		}
	}

	public int Networkm_superDiscBoardX
	{
		get
		{
			return this.m_superDiscBoardX;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_superDiscBoardX, 0x10U);
		}
	}

	public int Networkm_superDiscBoardY
	{
		get
		{
			return this.m_superDiscBoardY;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_superDiscBoardY, 0x20U);
		}
	}

	public int Networkm_numUltConsecUsedTurns
	{
		get
		{
			return this.m_numUltConsecUsedTurns;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_numUltConsecUsedTurns, 0x40U);
		}
	}

	protected static void InvokeSyncListm_boardX(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_SyncComponent.InvokeSyncListm_boardX(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("SyncList m_boardX called on server.");
			return;
		}
		((Neko_SyncComponent)obj).m_boardX.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_boardY(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_boardY called on server.");
			return;
		}
		((Neko_SyncComponent)obj).m_boardY.HandleMsg(reader);
	}

	private void Awake()
	{
		this.m_boardX.InitializeBehaviour(this, Neko_SyncComponent.kListm_boardX);
		this.m_boardY.InitializeBehaviour(this, Neko_SyncComponent.kListm_boardY);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_SyncComponent.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			SyncListInt.WriteInstance(writer, this.m_boardX);
			SyncListInt.WriteInstance(writer, this.m_boardY);
			writer.WritePackedUInt32((uint)this.m_homingActorIndex);
			writer.Write(this.m_superDiscActive);
			writer.WritePackedUInt32((uint)this.m_superDiscBoardX);
			writer.WritePackedUInt32((uint)this.m_superDiscBoardY);
			writer.WritePackedUInt32((uint)this.m_numUltConsecUsedTurns);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
			SyncListInt.WriteInstance(writer, this.m_boardX);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
			SyncListInt.WriteInstance(writer, this.m_boardY);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
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
			writer.WritePackedUInt32((uint)this.m_homingActorIndex);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
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
			writer.Write(this.m_superDiscActive);
		}
		if ((base.syncVarDirtyBits & 0x10U) != 0U)
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
			writer.WritePackedUInt32((uint)this.m_superDiscBoardX);
		}
		if ((base.syncVarDirtyBits & 0x20U) != 0U)
		{
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
			writer.WritePackedUInt32((uint)this.m_superDiscBoardY);
		}
		if ((base.syncVarDirtyBits & 0x40U) != 0U)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_numUltConsecUsedTurns);
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Neko_SyncComponent.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			SyncListInt.ReadReference(reader, this.m_boardX);
			SyncListInt.ReadReference(reader, this.m_boardY);
			this.m_homingActorIndex = (int)reader.ReadPackedUInt32();
			this.m_superDiscActive = reader.ReadBoolean();
			this.m_superDiscBoardX = (int)reader.ReadPackedUInt32();
			this.m_superDiscBoardY = (int)reader.ReadPackedUInt32();
			this.m_numUltConsecUsedTurns = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
			SyncListInt.ReadReference(reader, this.m_boardX);
		}
		if ((num & 2) != 0)
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
			SyncListInt.ReadReference(reader, this.m_boardY);
		}
		if ((num & 4) != 0)
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
			this.m_homingActorIndex = (int)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			this.m_superDiscActive = reader.ReadBoolean();
		}
		if ((num & 0x10) != 0)
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
			this.m_superDiscBoardX = (int)reader.ReadPackedUInt32();
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
			this.m_superDiscBoardY = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x40) != 0)
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
			this.m_numUltConsecUsedTurns = (int)reader.ReadPackedUInt32();
		}
	}
}
