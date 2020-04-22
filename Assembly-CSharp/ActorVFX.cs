using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActorVFX : NetworkBehaviour
{
	private class FootstepVFX
	{
		public GameObject m_vfxObj;

		public BoardSquare m_square;

		public FootstepVFX(BoardSquare square, ActorData runner)
		{
			m_square = square;
			if (HighlightUtils.Get().m_footstepsVFXPrefab != null)
			{
				if (FogOfWar.GetClientFog() != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
						{
							m_vfxObj = Object.Instantiate(HighlightUtils.Get().m_footstepsVFXPrefab);
							m_vfxObj.transform.position = square.GetWorldPosition() + new Vector3(0f, 0.1f, 0f);
							m_vfxObj.transform.rotation = runner.transform.rotation;
							bool active = FogOfWar.GetClientFog().IsVisible(m_square);
							m_vfxObj.SetActive(active);
							return;
						}
						}
					}
				}
			}
			m_vfxObj = null;
		}
	}

	private class BrushMoveVFX
	{
		public GameObject m_vfxObj;

		public int m_spawnTurn;

		public BrushMoveVFX(BoardSquare prevSquare, BoardSquare moveToSquare, bool movingIntoBrush)
		{
			GameObject gameObject = (!movingIntoBrush) ? HighlightUtils.Get().m_moveOutOfBrushVfxPrefab : HighlightUtils.Get().m_moveIntoBrushVfxPrefab;
			m_vfxObj = null;
			if (!(gameObject != null))
			{
				return;
			}
			while (true)
			{
				BoardSquare boardSquare = moveToSquare;
				BoardSquare boardSquare2 = prevSquare;
				if (!movingIntoBrush)
				{
					boardSquare = prevSquare;
					boardSquare2 = moveToSquare;
				}
				m_spawnTurn = GameFlowData.Get().CurrentTurn;
				Vector3 vector = boardSquare.ToVector3();
				Vector3 vector2 = boardSquare2.ToVector3() - vector;
				vector2.y = 0f;
				vector2.Normalize();
				Vector3 position = vector + 0.5f * Board.SquareSizeStatic * vector2;
				m_vfxObj = Object.Instantiate(gameObject);
				if (m_vfxObj != null)
				{
					while (true)
					{
						m_vfxObj.transform.position = position;
						m_vfxObj.transform.rotation = Quaternion.LookRotation(vector2);
						return;
					}
				}
				return;
			}
		}
	}

	private GameObject m_chaseMouseover;

	private GameObject m_moveMouseover;

	private GameObject m_concealedVFX;

	private List<FootstepVFX> m_footstepsVFX;

	private List<BrushMoveVFX> m_brushMoveVFX;

	private List<AttachedActorVFXInfo> m_statusIndicatorVFXList;

	private AttachedActorVFXInfo m_friendBaseCircleContainer;

	private AttachedActorVFXInfo m_enemyBaseCircleContainer;

	private bool m_spawnedBaseCircles;

	private List<DurationActorVFXInfo> m_durationBasedVfxList = new List<DurationActorVFXInfo>();

	private DurationActorVFXInfo m_hitInCoverVfxContainer;

	private DurationActorVFXInfo m_onDeathVfxContainer;

	private DurationActorVFXInfo m_onRespawnVFXContainer;

	private DurationActorVFXInfo m_onKnockbackWhileUnstoppableContainer;

	private ActorData m_actorData;

	private const string c_respawnAudioEventAlly = "ui/ingame/resurrection/ally";

	private const string c_respawnAudioEventEnemy = "ui/ingame/resurrection/enemy";

	public GameObject InstantiateAndSetupVFX(GameObject prefab)
	{
		GameObject gameObject = null;
		if (prefab != null)
		{
			gameObject = Object.Instantiate(prefab);
			gameObject.transform.parent = base.transform;
			gameObject.transform.rotation = Quaternion.identity;
			gameObject.SetActive(false);
		}
		return gameObject;
	}

	private void Awake()
	{
		m_chaseMouseover = InstantiateAndSetupVFX(HighlightUtils.Get().m_chaseMouseoverPrefab);
		m_moveMouseover = InstantiateAndSetupVFX(HighlightUtils.Get().m_moveSquareCursorPrefab);
		m_concealedVFX = InstantiateAndSetupVFX(HighlightUtils.Get().m_concealedVFXPrefab);
		if (HighlightUtils.Get().m_hitInCoverVFXPrefab != null)
		{
			m_hitInCoverVfxContainer = new DurationActorVFXInfo(HighlightUtils.Get().m_hitInCoverVFXPrefab, HighlightUtils.Get().m_hitInCoverVFXDuration, base.gameObject);
			m_durationBasedVfxList.Add(m_hitInCoverVfxContainer);
		}
		if (HighlightUtils.Get().m_onDeathVFXPrefab != null)
		{
			m_onDeathVfxContainer = new DurationActorVFXInfo(HighlightUtils.Get().m_onDeathVFXPrefab, HighlightUtils.Get().m_onDeathVFXDuration, null);
			m_durationBasedVfxList.Add(m_onDeathVfxContainer);
		}
		if (HighlightUtils.Get().m_onRespawnVFXPrefab != null)
		{
			m_onRespawnVFXContainer = new DurationActorVFXInfo(HighlightUtils.Get().m_onRespawnVFXPrefab, HighlightUtils.Get().m_onRespawnVFXDuration, null);
			m_durationBasedVfxList.Add(m_onRespawnVFXContainer);
		}
		if (HighlightUtils.Get().m_knockbackHitWhileUnstoppableVFXPrefab != null)
		{
			m_onKnockbackWhileUnstoppableContainer = new DurationActorVFXInfo(HighlightUtils.Get().m_knockbackHitWhileUnstoppableVFXPrefab, HighlightUtils.Get().m_knockbackHitWhileUnstoppableVFXDuration, base.gameObject);
			m_durationBasedVfxList.Add(m_onKnockbackWhileUnstoppableContainer);
		}
		m_footstepsVFX = new List<FootstepVFX>();
		m_brushMoveVFX = new List<BrushMoveVFX>();
		m_actorData = GetComponent<ActorData>();
		m_statusIndicatorVFXList = new List<AttachedActorVFXInfo>();
	}

	private void Start()
	{
		if (!(HighlightUtils.Get() != null))
		{
			return;
		}
		if (HighlightUtils.Get().m_statusVfxPrefabToJoint != null)
		{
			StatusVfxPrefabToJoint[] statusVfxPrefabToJoint = HighlightUtils.Get().m_statusVfxPrefabToJoint;
			foreach (StatusVfxPrefabToJoint statusVfxPrefabToJoint2 in statusVfxPrefabToJoint)
			{
				if (statusVfxPrefabToJoint2.m_status == StatusType.INVALID)
				{
					continue;
				}
				if (statusVfxPrefabToJoint2.m_status != StatusType.NUM)
				{
					StatusIndicatorVFX statusIndicatorVFX = new StatusIndicatorVFX(statusVfxPrefabToJoint2.m_statusVfxPrefab, m_actorData, statusVfxPrefabToJoint2.m_status, statusVfxPrefabToJoint2.m_vfxJoint, statusVfxPrefabToJoint2.m_alignToRootOrientation, "AttachedStatusVfx_" + statusVfxPrefabToJoint2.m_status);
					if (statusIndicatorVFX.HasVfxInstance())
					{
						m_statusIndicatorVFXList.Add(statusIndicatorVFX);
					}
				}
			}
		}
		if (!(HighlightUtils.Get().m_knockedBackVFXPrefab != null))
		{
			return;
		}
		KnockbackStatusIndicatorVFX knockbackStatusIndicatorVFX = new KnockbackStatusIndicatorVFX(HighlightUtils.Get().m_knockedBackVFXPrefab, m_actorData, HighlightUtils.Get().m_knockedBackVfxJoint, "AttachedStatusVfx_KnockedBack");
		if (!knockbackStatusIndicatorVFX.HasVfxInstance())
		{
			return;
		}
		while (true)
		{
			m_statusIndicatorVFXList.Add(knockbackStatusIndicatorVFX);
			return;
		}
	}

	private void LateUpdate()
	{
		ActorData actorData = m_actorData;
		bool sameTeamAsClientActor = false;
		if (GameFlowData.Get() != null)
		{
			sameTeamAsClientActor = ((!(GameFlowData.Get().activeOwnedActorData != null)) ? (m_actorData.GetTeam() == Team.TeamA) : (GameFlowData.Get().activeOwnedActorData.GetTeam() == m_actorData.GetTeam()));
		}
		bool flag = actorData.IsDead();
		bool flag2 = actorData.IsModelAnimatorDisabled();
		bool flag3 = actorData.IsVisibleToClient();
		int num;
		if (actorData.GetActorMovement() != null)
		{
			num = (actorData.GetActorMovement().InChargeState() ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag4 = (byte)num != 0;
		int num2;
		if (HUD_UI.Get() != null)
		{
			num2 = (HUD_UI.Get().MainHUDElementsVisible() ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool flag5 = (byte)num2 != 0;
		int num3;
		if (CameraManager.Get() != null)
		{
			num3 = (CameraManager.Get().InCinematic() ? 1 : 0);
		}
		else
		{
			num3 = 0;
		}
		bool flag6 = (byte)num3 != 0;
		if (m_chaseMouseover != null)
		{
			if (GameFlowData.Get().gameState != GameState.EndingGame)
			{
				BoardSquare playerFreeSquare = Board.Get().PlayerFreeSquare;
				if (playerFreeSquare == actorData.GetCurrentBoardSquare())
				{
					if (ActorTurnSM.IsClientDecidingMovement())
					{
						if (ActorData.WouldSquareBeChasedByClient(playerFreeSquare))
						{
							m_chaseMouseover.SetActive(true);
							m_chaseMouseover.transform.position = actorData.GetBonePosition("hip_JNT");
							goto IL_01e7;
						}
					}
				}
				m_chaseMouseover.SetActive(false);
			}
		}
		goto IL_01e7;
		IL_03a7:
		for (int i = 0; i < m_footstepsVFX.Count; i++)
		{
			FootstepVFX footstepVFX = m_footstepsVFX[i];
			bool active = FogOfWar.GetClientFog() != null && FogOfWar.GetClientFog().IsVisible(footstepVFX.m_square);
			if (footstepVFX.m_vfxObj != null)
			{
				footstepVFX.m_vfxObj.SetActive(active);
			}
		}
		while (true)
		{
			for (int j = 0; j < m_statusIndicatorVFXList.Count; j++)
			{
				AttachedActorVFXInfo attachedActorVFXInfo = m_statusIndicatorVFXList[j];
				if (attachedActorVFXInfo == null)
				{
					continue;
				}
				int num4;
				if (flag3)
				{
					if (!flag2 && !flag6)
					{
						num4 = ((!flag4) ? 1 : 0);
						goto IL_0465;
					}
				}
				num4 = 0;
				goto IL_0465;
				IL_0465:
				bool actorVisible = (byte)num4 != 0;
				attachedActorVFXInfo.UpdateVisibility(actorVisible, true);
			}
			while (true)
			{
				for (int k = 0; k < m_durationBasedVfxList.Count; k++)
				{
					DurationActorVFXInfo durationActorVFXInfo = m_durationBasedVfxList[k];
					if (durationActorVFXInfo != null)
					{
						durationActorVFXInfo.OnUpdate();
					}
				}
				while (true)
				{
					int num5;
					if (flag3)
					{
						if (!flag)
						{
							if (!flag2)
							{
								if (!flag4)
								{
									if (!m_actorData.IgnoreForAbilityHits)
									{
										num5 = (flag5 ? 1 : 0);
										goto IL_052a;
									}
								}
							}
						}
					}
					num5 = 0;
					goto IL_052a;
					IL_052a:
					bool actorVisible2 = (byte)num5 != 0;
					if (m_friendBaseCircleContainer != null)
					{
						m_friendBaseCircleContainer.UpdateVisibility(actorVisible2, sameTeamAsClientActor);
					}
					if (m_enemyBaseCircleContainer != null)
					{
						m_enemyBaseCircleContainer.UpdateVisibility(actorVisible2, sameTeamAsClientActor);
					}
					return;
				}
			}
		}
		IL_02cc:
		if (m_concealedVFX != null)
		{
			if (actorData.IsHiddenInBrush())
			{
				if (!CanBeSeenInBrush() && flag3)
				{
					if (!flag)
					{
						m_concealedVFX.SetActive(true);
						m_concealedVFX.transform.position = actorData.GetNameplatePosition(0f);
						goto IL_03a7;
					}
				}
			}
			if (actorData.GetActorStatus().IsInvisibleToEnemies(false))
			{
				if (flag3)
				{
					if (!flag)
					{
						m_concealedVFX.SetActive(true);
						m_concealedVFX.transform.position = actorData.GetNameplatePosition(0f);
						goto IL_03a7;
					}
				}
			}
			m_concealedVFX.SetActive(false);
		}
		goto IL_03a7;
		IL_01e7:
		if (m_moveMouseover != null)
		{
			if (GameFlowData.Get().gameState != GameState.EndingGame)
			{
				BoardSquare playerFreeSquare2 = Board.Get().PlayerFreeSquare;
				bool flag7 = playerFreeSquare2 == actorData.GetCurrentBoardSquare();
				if (!m_chaseMouseover.activeSelf)
				{
					if (flag7)
					{
						if (ActorTurnSM.IsClientDecidingMovement())
						{
							if (ActorData.WouldSquareBeChasedByClient(playerFreeSquare2, true))
							{
								m_moveMouseover.SetActive(true);
								m_moveMouseover.transform.position = actorData.GetBonePosition("hip_JNT");
								goto IL_02cc;
							}
						}
					}
				}
				m_moveMouseover.SetActive(false);
			}
		}
		goto IL_02cc;
	}

	public void OnTurnTick()
	{
		using (List<FootstepVFX>.Enumerator enumerator = m_footstepsVFX.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FootstepVFX current = enumerator.Current;
				if (current != null)
				{
					if (current.m_vfxObj != null)
					{
						Object.Destroy(current.m_vfxObj);
					}
				}
			}
		}
		m_footstepsVFX.Clear();
		int currentTurn = GameFlowData.Get().CurrentTurn;
		for (int num = m_brushMoveVFX.Count - 1; num >= 0; num--)
		{
			BrushMoveVFX brushMoveVFX = m_brushMoveVFX[num];
			if (!(brushMoveVFX.m_vfxObj == null))
			{
				if (currentTurn <= brushMoveVFX.m_spawnTurn + 1)
				{
					continue;
				}
			}
			if (brushMoveVFX.m_vfxObj != null)
			{
				Object.Destroy(brushMoveVFX.m_vfxObj);
			}
			m_brushMoveVFX.RemoveAt(num);
		}
		if (m_hitInCoverVfxContainer != null)
		{
			m_hitInCoverVfxContainer.HideVfx();
		}
	}

	public void SpawnBaseCircles()
	{
		if (m_spawnedBaseCircles)
		{
			return;
		}
		while (true)
		{
			float d = HighlightUtils.Get().m_baseCircleSizeInSquares * Board.Get().squareSize;
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(HighlightUtils.Get().m_friendBaseCirclePrefab, m_actorData, HighlightUtils.Get().m_baseCircleJoint, false, "BaseCircleVFX_Friendly", AttachedActorVFXInfo.FriendOrFoeVisibility.FriendlyOnly);
			if (attachedActorVFXInfo.HasVfxInstance())
			{
				attachedActorVFXInfo.SetInstanceScale(d * Vector3.one);
				attachedActorVFXInfo.SetInstanceLocalPosition(HighlightUtils.Get().m_circlePrefabHeight * Vector3.up);
				m_friendBaseCircleContainer = attachedActorVFXInfo;
			}
			AttachedActorVFXInfo attachedActorVFXInfo2 = new AttachedActorVFXInfo(HighlightUtils.Get().m_enemyBaseCirclePrefab, m_actorData, HighlightUtils.Get().m_baseCircleJoint, false, "BaseCircleVFX_Enemy", AttachedActorVFXInfo.FriendOrFoeVisibility.EnemyOnly);
			if (attachedActorVFXInfo2.HasVfxInstance())
			{
				attachedActorVFXInfo2.SetInstanceScale(d * Vector3.one);
				attachedActorVFXInfo2.SetInstanceLocalPosition(HighlightUtils.Get().m_circlePrefabHeight * Vector3.up);
				m_enemyBaseCircleContainer = attachedActorVFXInfo2;
			}
			m_spawnedBaseCircles = true;
			return;
		}
	}

	private void OnDestroy()
	{
		if (m_chaseMouseover != null)
		{
			Object.Destroy(m_chaseMouseover);
			m_chaseMouseover = null;
		}
		if (m_moveMouseover != null)
		{
			Object.Destroy(m_moveMouseover);
			m_moveMouseover = null;
		}
		if (m_concealedVFX != null)
		{
			Object.Destroy(m_concealedVFX);
			m_concealedVFX = null;
		}
		if (m_footstepsVFX != null)
		{
			using (List<FootstepVFX>.Enumerator enumerator = m_footstepsVFX.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FootstepVFX current = enumerator.Current;
					if (current != null)
					{
						Object.Destroy(current.m_vfxObj);
					}
				}
			}
			m_footstepsVFX.Clear();
		}
		if (m_brushMoveVFX != null)
		{
			using (List<BrushMoveVFX>.Enumerator enumerator2 = m_brushMoveVFX.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					BrushMoveVFX current2 = enumerator2.Current;
					if (current2 != null)
					{
						if (current2.m_vfxObj != null)
						{
							Object.Destroy(current2.m_vfxObj);
						}
					}
				}
			}
			m_brushMoveVFX.Clear();
		}
		if (m_statusIndicatorVFXList != null)
		{
			using (List<AttachedActorVFXInfo>.Enumerator enumerator3 = m_statusIndicatorVFXList.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					enumerator3.Current?.DestroyVfx();
				}
			}
			m_statusIndicatorVFXList.Clear();
		}
		for (int i = 0; i < m_durationBasedVfxList.Count; i++)
		{
			DurationActorVFXInfo durationActorVFXInfo = m_durationBasedVfxList[i];
			if (durationActorVFXInfo != null)
			{
				durationActorVFXInfo.DestroyVfx();
			}
		}
		while (true)
		{
			if (m_friendBaseCircleContainer != null)
			{
				m_friendBaseCircleContainer.DestroyVfx();
				m_friendBaseCircleContainer = null;
			}
			if (m_enemyBaseCircleContainer != null)
			{
				while (true)
				{
					m_enemyBaseCircleContainer.DestroyVfx();
					m_enemyBaseCircleContainer = null;
					return;
				}
			}
			return;
		}
	}

	private bool BeingChasedByClient()
	{
		bool result = false;
		GameFlowData gameFlowData = GameFlowData.Get();
		if ((bool)gameFlowData)
		{
			ActorData activeOwnedActorData = gameFlowData.activeOwnedActorData;
			if (activeOwnedActorData != null)
			{
				ActorData actorData = m_actorData;
				if (activeOwnedActorData.GetQueuedChaseTarget() == actorData)
				{
					result = true;
				}
			}
		}
		return result;
	}

	public void OnMove(BoardSquarePathInfo moveToNode, BoardSquarePathInfo prevNode)
	{
		BoardSquare square = moveToNode.square;
		PlayerData localPlayerData = GameFlowData.Get().LocalPlayerData;
		if (localPlayerData != null)
		{
			if (BeingChasedByClient())
			{
				ActorData actorData = m_actorData;
				int num;
				if (actorData.SomeVisibilityCheckB_zq(localPlayerData, false))
				{
					num = ((!actorData.SomeVisibilityCheckA_zq(localPlayerData, false)) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				if (num == 0)
				{
					FootstepVFX item = new FootstepVFX(square, m_actorData);
					m_footstepsVFX.Add(item);
				}
			}
		}
		BrushCoordinator brushCoordinator = BrushCoordinator.Get();
		if (square != null)
		{
			if (prevNode != null && (bool)brushCoordinator && FogOfWar.GetClientFog() != null)
			{
				BoardSquare square2 = prevNode.square;
				bool flag = brushCoordinator.IsRegionFunctioning(square.BrushRegion);
				bool flag2 = brushCoordinator.IsRegionFunctioning(square2.BrushRegion);
				if (flag != flag2)
				{
					if (BoardSquarePathInfo.IsConnectionTypeConventional(prevNode.connectionType))
					{
						if (!moveToNode.m_visibleToEnemies)
						{
							if (!prevNode.m_visibleToEnemies)
							{
								if (m_actorData.GetTeam() != localPlayerData.GetTeamViewing())
								{
									goto IL_01a2;
								}
							}
						}
						BrushMoveVFX item2 = new BrushMoveVFX(square2, square, flag);
						m_brushMoveVFX.Add(item2);
					}
				}
			}
		}
		goto IL_01a2;
		IL_01a2:
		if (m_hitInCoverVfxContainer == null)
		{
			return;
		}
		while (true)
		{
			m_hitInCoverVfxContainer.HideVfx();
			return;
		}
	}

	private bool CanBeSeenInBrush()
	{
		ActorData actorData = m_actorData;
		int num;
		if (actorData.IsHiddenInBrush())
		{
			num = ((!BrushCoordinator.Get().IsRegionFunctioning(actorData.GetTravelBoardSquareBrushRegion())) ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		ActorStatus component = GetComponent<ActorStatus>();
		int num2;
		if (!component.HasStatus(StatusType.CantHideInBrush, false))
		{
			num2 = (component.HasStatus(StatusType.Revealed, false) ? 1 : 0);
		}
		else
		{
			num2 = 1;
		}
		bool flag2 = (byte)num2 != 0;
		bool flag3 = CaptureTheFlag.IsActorRevealedByFlag_Client(actorData);
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(actorData.GetTravelBoardSquareWorldPosition(), GameplayData.Get().m_distanceCanSeeIntoBrush, true, null, actorData.GetOpposingTeam(), null);
		bool flag4;
		if (actorsInRadius.Count > 0)
		{
			flag4 = true;
		}
		else
		{
			flag4 = false;
		}
		bool flag5 = false;
		List<ActorData> actorsInBrushRegion = BrushCoordinator.Get().GetActorsInBrushRegion(actorData.GetTravelBoardSquareBrushRegion());
		if (actorsInBrushRegion != null)
		{
			foreach (ActorData item in actorsInBrushRegion)
			{
				if (item.GetTeam() != actorData.GetTeam())
				{
					flag5 = true;
				}
			}
		}
		int result;
		if (!flag && !flag2 && !flag3 && !flag4)
		{
			result = (flag5 ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public void ShowHitWhileInCoverVfx(Vector3 actorPos, Vector3 hitOrigin, ActorData caster)
	{
		ShowDurationVfxWithDirection(m_hitInCoverVfxContainer, actorPos, hitOrigin, caster);
	}

	public void ShowKnockbackWhileUnstoppableVfx(Vector3 actorPos, Vector3 hitOrigin, ActorData caster)
	{
		ShowDurationVfxWithDirection(m_onKnockbackWhileUnstoppableContainer, actorPos, hitOrigin, caster);
	}

	private void ShowDurationVfxWithDirection(DurationActorVFXInfo container, Vector3 actorPos, Vector3 hitOrigin, ActorData caster)
	{
		if (container == null)
		{
			return;
		}
		while (true)
		{
			Vector3 vector = hitOrigin - actorPos;
			vector.y = 0f;
			vector.Normalize();
			if (vector == Vector3.zero)
			{
				if (caster != null)
				{
					if (caster.GetCurrentBoardSquare() != null)
					{
						vector = caster.GetTravelBoardSquareWorldPosition() - actorPos;
						vector.y = 0f;
						vector.Normalize();
					}
				}
				if (vector == Vector3.zero)
				{
					vector = Vector3.forward;
				}
			}
			container.ShowVfx(m_actorData.IsVisibleToClient(), vector);
			return;
		}
	}

	public void ShowOnDeathVfx(Vector3 actorPos)
	{
		if (m_onDeathVfxContainer == null)
		{
			return;
		}
		while (true)
		{
			m_onDeathVfxContainer.ShowVfxAtPosition(actorPos, m_actorData.IsVisibleToClient(), Vector3.zero);
			return;
		}
	}

	public void ShowOnRespawnVfx()
	{
		if (m_onRespawnVFXContainer == null)
		{
			return;
		}
		while (true)
		{
			m_onRespawnVFXContainer.ShowVfxAtPosition(m_actorData.GetTravelBoardSquareWorldPosition(), true, Vector3.zero);
			object obj;
			if (GameFlowData.Get() != null)
			{
				obj = GameFlowData.Get().LocalPlayerData;
			}
			else
			{
				obj = null;
			}
			PlayerData playerData = (PlayerData)obj;
			if (playerData != null)
			{
				while (true)
				{
					AudioManager.PostEvent((m_actorData.GetTeam() != playerData.GetTeamViewing()) ? "ui/ingame/resurrection/enemy" : "ui/ingame/resurrection/ally", base.gameObject);
					return;
				}
			}
			return;
		}
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result = default(bool);
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}
}
