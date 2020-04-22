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
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				BoardSquare boardSquare = moveToSquare;
				BoardSquare boardSquare2 = prevSquare;
				if (!movingIntoBrush)
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
						switch (1)
						{
						case 0:
							continue;
						}
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
			m_onDeathVfxContainer = new DurationActorVFXInfo(HighlightUtils.Get().m_onDeathVFXPrefab, HighlightUtils.Get().m_onDeathVFXDuration, null);
			m_durationBasedVfxList.Add(m_onDeathVfxContainer);
		}
		if (HighlightUtils.Get().m_onRespawnVFXPrefab != null)
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
			m_onRespawnVFXContainer = new DurationActorVFXInfo(HighlightUtils.Get().m_onRespawnVFXPrefab, HighlightUtils.Get().m_onRespawnVFXDuration, null);
			m_durationBasedVfxList.Add(m_onRespawnVFXContainer);
		}
		if (HighlightUtils.Get().m_knockbackHitWhileUnstoppableVFXPrefab != null)
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
			StatusVfxPrefabToJoint[] statusVfxPrefabToJoint = HighlightUtils.Get().m_statusVfxPrefabToJoint;
			foreach (StatusVfxPrefabToJoint statusVfxPrefabToJoint2 in statusVfxPrefabToJoint)
			{
				if (statusVfxPrefabToJoint2.m_status == StatusType.INVALID)
				{
					continue;
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
				if (statusVfxPrefabToJoint2.m_status != StatusType.NUM)
				{
					StatusIndicatorVFX statusIndicatorVFX = new StatusIndicatorVFX(statusVfxPrefabToJoint2.m_statusVfxPrefab, m_actorData, statusVfxPrefabToJoint2.m_status, statusVfxPrefabToJoint2.m_vfxJoint, statusVfxPrefabToJoint2.m_alignToRootOrientation, "AttachedStatusVfx_" + statusVfxPrefabToJoint2.m_status);
					if (statusIndicatorVFX.HasVfxInstance())
					{
						m_statusIndicatorVFXList.Add(statusIndicatorVFX);
					}
				}
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
			switch (7)
			{
			case 0:
				continue;
			}
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
			sameTeamAsClientActor = ((!(GameFlowData.Get().activeOwnedActorData != null)) ? (m_actorData.GetTeam() == Team.TeamA) : (GameFlowData.Get().activeOwnedActorData.GetTeam() == m_actorData.GetTeam()));
		}
		bool flag = actorData.IsDead();
		bool flag2 = actorData.IsModelAnimatorDisabled();
		bool flag3 = actorData.IsVisibleToClient();
		int num;
		if (actorData.GetActorMovement() != null)
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			num3 = (CameraManager.Get().InCinematic() ? 1 : 0);
		}
		else
		{
			num3 = 0;
		}
		bool flag6 = (byte)num3 != 0;
		if (m_chaseMouseover != null)
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
			if (GameFlowData.Get().gameState != GameState.EndingGame)
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
				BoardSquare playerFreeSquare = Board.Get().PlayerFreeSquare;
				if (playerFreeSquare == actorData.GetCurrentBoardSquare())
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
					if (ActorTurnSM.IsClientDecidingMovement())
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
						if (ActorData.WouldSquareBeChasedByClient(playerFreeSquare))
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
			switch (5)
			{
			case 0:
				continue;
			}
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
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!flag2 && !flag6)
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
				switch (6)
				{
				case 0:
					continue;
				}
				for (int k = 0; k < m_durationBasedVfxList.Count; k++)
				{
					DurationActorVFXInfo durationActorVFXInfo = m_durationBasedVfxList[k];
					if (durationActorVFXInfo != null)
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
						durationActorVFXInfo.OnUpdate();
					}
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					int num5;
					if (flag3)
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
							if (!flag2)
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
								if (!flag4)
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
									if (!m_actorData.IgnoreForAbilityHits)
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
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!CanBeSeenInBrush() && flag3)
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
						m_concealedVFX.SetActive(true);
						m_concealedVFX.transform.position = actorData.GetNameplatePosition(0f);
						goto IL_03a7;
					}
				}
			}
			if (actorData.GetActorStatus().IsInvisibleToEnemies(false))
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
				if (flag3)
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
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (GameFlowData.Get().gameState != GameState.EndingGame)
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
				BoardSquare playerFreeSquare2 = Board.Get().PlayerFreeSquare;
				bool flag7 = playerFreeSquare2 == actorData.GetCurrentBoardSquare();
				if (!m_chaseMouseover.activeSelf)
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
					if (flag7)
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
						if (ActorTurnSM.IsClientDecidingMovement())
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
							if (ActorData.WouldSquareBeChasedByClient(playerFreeSquare2, true))
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
					if (current.m_vfxObj != null)
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
						Object.Destroy(current.m_vfxObj);
					}
				}
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
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (brushMoveVFX.m_vfxObj != null)
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
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			float d = HighlightUtils.Get().m_baseCircleSizeInSquares * Board.Get().squareSize;
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(HighlightUtils.Get().m_friendBaseCirclePrefab, m_actorData, HighlightUtils.Get().m_baseCircleJoint, false, "BaseCircleVFX_Friendly", AttachedActorVFXInfo.FriendOrFoeVisibility.FriendlyOnly);
			if (attachedActorVFXInfo.HasVfxInstance())
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
				attachedActorVFXInfo.SetInstanceScale(d * Vector3.one);
				attachedActorVFXInfo.SetInstanceLocalPosition(HighlightUtils.Get().m_circlePrefabHeight * Vector3.up);
				m_friendBaseCircleContainer = attachedActorVFXInfo;
			}
			AttachedActorVFXInfo attachedActorVFXInfo2 = new AttachedActorVFXInfo(HighlightUtils.Get().m_enemyBaseCirclePrefab, m_actorData, HighlightUtils.Get().m_baseCircleJoint, false, "BaseCircleVFX_Enemy", AttachedActorVFXInfo.FriendOrFoeVisibility.EnemyOnly);
			if (attachedActorVFXInfo2.HasVfxInstance())
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
			Object.Destroy(m_chaseMouseover);
			m_chaseMouseover = null;
		}
		if (m_moveMouseover != null)
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
			Object.Destroy(m_moveMouseover);
			m_moveMouseover = null;
		}
		if (m_concealedVFX != null)
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
			Object.Destroy(m_concealedVFX);
			m_concealedVFX = null;
		}
		if (m_footstepsVFX != null)
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
			using (List<FootstepVFX>.Enumerator enumerator = m_footstepsVFX.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					FootstepVFX current = enumerator.Current;
					if (current != null)
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
						Object.Destroy(current.m_vfxObj);
					}
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
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (current2.m_vfxObj != null)
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
							Object.Destroy(current2.m_vfxObj);
						}
					}
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
			m_brushMoveVFX.Clear();
		}
		if (m_statusIndicatorVFXList != null)
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
			using (List<AttachedActorVFXInfo>.Enumerator enumerator3 = m_statusIndicatorVFXList.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					enumerator3.Current?.DestroyVfx();
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
			m_statusIndicatorVFXList.Clear();
		}
		for (int i = 0; i < m_durationBasedVfxList.Count; i++)
		{
			DurationActorVFXInfo durationActorVFXInfo = m_durationBasedVfxList[i];
			if (durationActorVFXInfo != null)
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
				durationActorVFXInfo.DestroyVfx();
			}
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (m_friendBaseCircleContainer != null)
			{
				m_friendBaseCircleContainer.DestroyVfx();
				m_friendBaseCircleContainer = null;
			}
			if (m_enemyBaseCircleContainer != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
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
				ActorData actorData = m_actorData;
				if (activeOwnedActorData.GetQueuedChaseTarget() == actorData)
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
			if (BeingChasedByClient())
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
				ActorData actorData = m_actorData;
				int num;
				if (actorData.SomeVisibilityCheckB_zq(localPlayerData, false))
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
					num = ((!actorData.SomeVisibilityCheckA_zq(localPlayerData, false)) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				if (num == 0)
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
					FootstepVFX item = new FootstepVFX(square, m_actorData);
					m_footstepsVFX.Add(item);
				}
			}
		}
		BrushCoordinator brushCoordinator = BrushCoordinator.Get();
		if (square != null)
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
			if (prevNode != null && (bool)brushCoordinator && FogOfWar.GetClientFog() != null)
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
				BoardSquare square2 = prevNode.square;
				bool flag = brushCoordinator.IsRegionFunctioning(square.BrushRegion);
				bool flag2 = brushCoordinator.IsRegionFunctioning(square2.BrushRegion);
				if (flag != flag2)
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
					if (BoardSquarePathInfo.IsConnectionTypeConventional(prevNode.connectionType))
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
						if (!moveToNode.m_visibleToEnemies)
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
							if (!prevNode.m_visibleToEnemies)
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
								if (m_actorData.GetTeam() != localPlayerData.GetTeamViewing())
								{
									goto IL_01a2;
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
			switch (7)
			{
			case 0:
				continue;
			}
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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
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
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							flag5 = true;
							goto end_IL_00ec;
						}
					}
				}
			}
		}
		int result;
		if (!flag && !flag2 && !flag3 && !flag4)
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
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Vector3 vector = hitOrigin - actorPos;
			vector.y = 0f;
			vector.Normalize();
			if (vector == Vector3.zero)
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
				if (caster != null)
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
					if (caster.GetCurrentBoardSquare() != null)
					{
						vector = caster.GetTravelBoardSquareWorldPosition() - actorPos;
						vector.y = 0f;
						vector.Normalize();
					}
				}
				if (vector == Vector3.zero)
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
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_onRespawnVFXContainer.ShowVfxAtPosition(m_actorData.GetTravelBoardSquareWorldPosition(), true, Vector3.zero);
			object obj;
			if (GameFlowData.Get() != null)
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
					switch (1)
					{
					case 0:
						continue;
					}
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
