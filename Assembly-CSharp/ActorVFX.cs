using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActorVFX : NetworkBehaviour
{
	private GameObject m_chaseMouseover;

	private GameObject m_moveMouseover;

	private GameObject m_concealedVFX;

	private List<ActorVFX.FootstepVFX> m_footstepsVFX;

	private List<ActorVFX.BrushMoveVFX> m_brushMoveVFX;

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
			gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
			gameObject.transform.parent = base.transform;
			gameObject.transform.rotation = Quaternion.identity;
			gameObject.SetActive(false);
		}
		return gameObject;
	}

	private void Awake()
	{
		this.m_chaseMouseover = this.InstantiateAndSetupVFX(HighlightUtils.Get().m_chaseMouseoverPrefab);
		this.m_moveMouseover = this.InstantiateAndSetupVFX(HighlightUtils.Get().m_moveSquareCursorPrefab);
		this.m_concealedVFX = this.InstantiateAndSetupVFX(HighlightUtils.Get().m_concealedVFXPrefab);
		if (HighlightUtils.Get().m_hitInCoverVFXPrefab != null)
		{
			this.m_hitInCoverVfxContainer = new DurationActorVFXInfo(HighlightUtils.Get().m_hitInCoverVFXPrefab, HighlightUtils.Get().m_hitInCoverVFXDuration, base.gameObject);
			this.m_durationBasedVfxList.Add(this.m_hitInCoverVfxContainer);
		}
		if (HighlightUtils.Get().m_onDeathVFXPrefab != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.Awake()).MethodHandle;
			}
			this.m_onDeathVfxContainer = new DurationActorVFXInfo(HighlightUtils.Get().m_onDeathVFXPrefab, HighlightUtils.Get().m_onDeathVFXDuration, null);
			this.m_durationBasedVfxList.Add(this.m_onDeathVfxContainer);
		}
		if (HighlightUtils.Get().m_onRespawnVFXPrefab != null)
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
			this.m_onRespawnVFXContainer = new DurationActorVFXInfo(HighlightUtils.Get().m_onRespawnVFXPrefab, HighlightUtils.Get().m_onRespawnVFXDuration, null);
			this.m_durationBasedVfxList.Add(this.m_onRespawnVFXContainer);
		}
		if (HighlightUtils.Get().m_knockbackHitWhileUnstoppableVFXPrefab != null)
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
			this.m_onKnockbackWhileUnstoppableContainer = new DurationActorVFXInfo(HighlightUtils.Get().m_knockbackHitWhileUnstoppableVFXPrefab, HighlightUtils.Get().m_knockbackHitWhileUnstoppableVFXDuration, base.gameObject);
			this.m_durationBasedVfxList.Add(this.m_onKnockbackWhileUnstoppableContainer);
		}
		this.m_footstepsVFX = new List<ActorVFX.FootstepVFX>();
		this.m_brushMoveVFX = new List<ActorVFX.BrushMoveVFX>();
		this.m_actorData = base.GetComponent<ActorData>();
		this.m_statusIndicatorVFXList = new List<AttachedActorVFXInfo>();
	}

	private void Start()
	{
		if (HighlightUtils.Get() != null)
		{
			if (HighlightUtils.Get().m_statusVfxPrefabToJoint != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.Start()).MethodHandle;
				}
				foreach (StatusVfxPrefabToJoint statusVfxPrefabToJoint2 in HighlightUtils.Get().m_statusVfxPrefabToJoint)
				{
					if (statusVfxPrefabToJoint2.m_status != StatusType.INVALID)
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
						if (statusVfxPrefabToJoint2.m_status != StatusType.NUM)
						{
							StatusIndicatorVFX statusIndicatorVFX = new StatusIndicatorVFX(statusVfxPrefabToJoint2.m_statusVfxPrefab, this.m_actorData, statusVfxPrefabToJoint2.m_status, statusVfxPrefabToJoint2.m_vfxJoint, statusVfxPrefabToJoint2.m_alignToRootOrientation, "AttachedStatusVfx_" + statusVfxPrefabToJoint2.m_status.ToString());
							if (statusIndicatorVFX.HasVfxInstance())
							{
								this.m_statusIndicatorVFXList.Add(statusIndicatorVFX);
							}
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
			}
			if (HighlightUtils.Get().m_knockedBackVFXPrefab != null)
			{
				KnockbackStatusIndicatorVFX knockbackStatusIndicatorVFX = new KnockbackStatusIndicatorVFX(HighlightUtils.Get().m_knockedBackVFXPrefab, this.m_actorData, HighlightUtils.Get().m_knockedBackVfxJoint, "AttachedStatusVfx_KnockedBack");
				if (knockbackStatusIndicatorVFX.HasVfxInstance())
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
					this.m_statusIndicatorVFXList.Add(knockbackStatusIndicatorVFX);
				}
			}
		}
	}

	private void LateUpdate()
	{
		ActorData actorData = this.m_actorData;
		bool sameTeamAsClientActor = false;
		if (GameFlowData.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.LateUpdate()).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				sameTeamAsClientActor = (GameFlowData.Get().activeOwnedActorData.\u000E() == this.m_actorData.\u000E());
			}
			else
			{
				sameTeamAsClientActor = (this.m_actorData.\u000E() == Team.TeamA);
			}
		}
		bool flag = actorData.\u000E();
		bool flag2 = actorData.\u0012();
		bool flag3 = actorData.\u0018();
		bool flag4;
		if (actorData.\u000E() != null)
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
			flag4 = actorData.\u000E().InChargeState();
		}
		else
		{
			flag4 = false;
		}
		bool flag5 = flag4;
		bool flag6;
		if (HUD_UI.Get() != null)
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
			flag6 = HUD_UI.Get().MainHUDElementsVisible();
		}
		else
		{
			flag6 = false;
		}
		bool flag7 = flag6;
		bool flag8;
		if (CameraManager.Get() != null)
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
			flag8 = CameraManager.Get().InCinematic();
		}
		else
		{
			flag8 = false;
		}
		bool flag9 = flag8;
		if (this.m_chaseMouseover != null)
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
				BoardSquare playerFreeSquare = Board.\u000E().PlayerFreeSquare;
				bool flag10 = playerFreeSquare == actorData.\u0012();
				if (flag10)
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
					if (ActorTurnSM.IsClientDecidingMovement())
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
						if (ActorData.WouldSquareBeChasedByClient(playerFreeSquare, false))
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
							this.m_chaseMouseover.SetActive(true);
							this.m_chaseMouseover.transform.position = actorData.\u000E("hip_JNT");
							goto IL_1E7;
						}
					}
				}
				this.m_chaseMouseover.SetActive(false);
			}
		}
		IL_1E7:
		if (this.m_moveMouseover != null)
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
			if (GameFlowData.Get().gameState != GameState.EndingGame)
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
				BoardSquare playerFreeSquare2 = Board.\u000E().PlayerFreeSquare;
				bool flag11 = playerFreeSquare2 == actorData.\u0012();
				if (!this.m_chaseMouseover.activeSelf)
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
					if (flag11)
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
						if (ActorTurnSM.IsClientDecidingMovement())
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
							if (ActorData.WouldSquareBeChasedByClient(playerFreeSquare2, true))
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
								this.m_moveMouseover.SetActive(true);
								this.m_moveMouseover.transform.position = actorData.\u000E("hip_JNT");
								goto IL_2CC;
							}
						}
					}
				}
				this.m_moveMouseover.SetActive(false);
			}
		}
		IL_2CC:
		if (this.m_concealedVFX != null)
		{
			if (actorData.\u0016())
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
				if (!this.CanBeSeenInBrush() && flag3)
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
						this.m_concealedVFX.SetActive(true);
						this.m_concealedVFX.transform.position = actorData.\u000E(0f);
						goto IL_3A7;
					}
				}
			}
			if (actorData.\u000E().IsInvisibleToEnemies(false))
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
				if (flag3)
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
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_concealedVFX.SetActive(true);
						this.m_concealedVFX.transform.position = actorData.\u000E(0f);
						goto IL_3A7;
					}
				}
			}
			this.m_concealedVFX.SetActive(false);
		}
		IL_3A7:
		for (int i = 0; i < this.m_footstepsVFX.Count; i++)
		{
			ActorVFX.FootstepVFX footstepVFX = this.m_footstepsVFX[i];
			bool active = FogOfWar.GetClientFog() != null && FogOfWar.GetClientFog().IsVisible(footstepVFX.m_square);
			if (footstepVFX.m_vfxObj != null)
			{
				footstepVFX.m_vfxObj.SetActive(active);
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
		for (int j = 0; j < this.m_statusIndicatorVFXList.Count; j++)
		{
			AttachedActorVFXInfo attachedActorVFXInfo = this.m_statusIndicatorVFXList[j];
			if (attachedActorVFXInfo != null)
			{
				if (!flag3)
				{
					goto IL_464;
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
				if (flag2 || flag9)
				{
					goto IL_464;
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
				bool flag12 = !flag5;
				IL_465:
				bool actorVisible = flag12;
				attachedActorVFXInfo.UpdateVisibility(actorVisible, true);
				goto IL_471;
				IL_464:
				flag12 = false;
				goto IL_465;
			}
			IL_471:;
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
		for (int k = 0; k < this.m_durationBasedVfxList.Count; k++)
		{
			DurationActorVFXInfo durationActorVFXInfo = this.m_durationBasedVfxList[k];
			if (durationActorVFXInfo != null)
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
				durationActorVFXInfo.OnUpdate();
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
		bool flag13;
		if (flag3)
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
					if (!flag5)
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
						if (!this.m_actorData.IgnoreForAbilityHits)
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
							flag13 = flag7;
							goto IL_52A;
						}
					}
				}
			}
		}
		flag13 = false;
		IL_52A:
		bool actorVisible2 = flag13;
		if (this.m_friendBaseCircleContainer != null)
		{
			this.m_friendBaseCircleContainer.UpdateVisibility(actorVisible2, sameTeamAsClientActor);
		}
		if (this.m_enemyBaseCircleContainer != null)
		{
			this.m_enemyBaseCircleContainer.UpdateVisibility(actorVisible2, sameTeamAsClientActor);
		}
	}

	public void OnTurnTick()
	{
		using (List<ActorVFX.FootstepVFX>.Enumerator enumerator = this.m_footstepsVFX.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorVFX.FootstepVFX footstepVFX = enumerator.Current;
				if (footstepVFX != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.OnTurnTick()).MethodHandle;
					}
					if (footstepVFX.m_vfxObj != null)
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
						UnityEngine.Object.Destroy(footstepVFX.m_vfxObj);
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
		this.m_footstepsVFX.Clear();
		int currentTurn = GameFlowData.Get().CurrentTurn;
		int i = this.m_brushMoveVFX.Count - 1;
		while (i >= 0)
		{
			ActorVFX.BrushMoveVFX brushMoveVFX = this.m_brushMoveVFX[i];
			if (brushMoveVFX.m_vfxObj == null)
			{
				goto IL_D9;
			}
			if (currentTurn > brushMoveVFX.m_spawnTurn + 1)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					goto IL_D9;
				}
			}
			IL_10C:
			i--;
			continue;
			IL_D9:
			if (brushMoveVFX.m_vfxObj != null)
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
				UnityEngine.Object.Destroy(brushMoveVFX.m_vfxObj);
			}
			this.m_brushMoveVFX.RemoveAt(i);
			goto IL_10C;
		}
		if (this.m_hitInCoverVfxContainer != null)
		{
			this.m_hitInCoverVfxContainer.HideVfx();
		}
	}

	public void SpawnBaseCircles()
	{
		if (!this.m_spawnedBaseCircles)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.SpawnBaseCircles()).MethodHandle;
			}
			float d = HighlightUtils.Get().m_baseCircleSizeInSquares * Board.\u000E().squareSize;
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(HighlightUtils.Get().m_friendBaseCirclePrefab, this.m_actorData, HighlightUtils.Get().m_baseCircleJoint, false, "BaseCircleVFX_Friendly", AttachedActorVFXInfo.FriendOrFoeVisibility.FriendlyOnly);
			if (attachedActorVFXInfo.HasVfxInstance())
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
				attachedActorVFXInfo.SetInstanceScale(d * Vector3.one);
				attachedActorVFXInfo.SetInstanceLocalPosition(HighlightUtils.Get().m_circlePrefabHeight * Vector3.up);
				this.m_friendBaseCircleContainer = attachedActorVFXInfo;
			}
			AttachedActorVFXInfo attachedActorVFXInfo2 = new AttachedActorVFXInfo(HighlightUtils.Get().m_enemyBaseCirclePrefab, this.m_actorData, HighlightUtils.Get().m_baseCircleJoint, false, "BaseCircleVFX_Enemy", AttachedActorVFXInfo.FriendOrFoeVisibility.EnemyOnly);
			if (attachedActorVFXInfo2.HasVfxInstance())
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
				attachedActorVFXInfo2.SetInstanceScale(d * Vector3.one);
				attachedActorVFXInfo2.SetInstanceLocalPosition(HighlightUtils.Get().m_circlePrefabHeight * Vector3.up);
				this.m_enemyBaseCircleContainer = attachedActorVFXInfo2;
			}
			this.m_spawnedBaseCircles = true;
		}
	}

	private void OnDestroy()
	{
		if (this.m_chaseMouseover != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.OnDestroy()).MethodHandle;
			}
			UnityEngine.Object.Destroy(this.m_chaseMouseover);
			this.m_chaseMouseover = null;
		}
		if (this.m_moveMouseover != null)
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
			UnityEngine.Object.Destroy(this.m_moveMouseover);
			this.m_moveMouseover = null;
		}
		if (this.m_concealedVFX != null)
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
			UnityEngine.Object.Destroy(this.m_concealedVFX);
			this.m_concealedVFX = null;
		}
		if (this.m_footstepsVFX != null)
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
			using (List<ActorVFX.FootstepVFX>.Enumerator enumerator = this.m_footstepsVFX.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorVFX.FootstepVFX footstepVFX = enumerator.Current;
					if (footstepVFX != null)
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
						UnityEngine.Object.Destroy(footstepVFX.m_vfxObj);
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
			this.m_footstepsVFX.Clear();
		}
		if (this.m_brushMoveVFX != null)
		{
			using (List<ActorVFX.BrushMoveVFX>.Enumerator enumerator2 = this.m_brushMoveVFX.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorVFX.BrushMoveVFX brushMoveVFX = enumerator2.Current;
					if (brushMoveVFX != null)
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
						if (brushMoveVFX.m_vfxObj != null)
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
							UnityEngine.Object.Destroy(brushMoveVFX.m_vfxObj);
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
			this.m_brushMoveVFX.Clear();
		}
		if (this.m_statusIndicatorVFXList != null)
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
			using (List<AttachedActorVFXInfo>.Enumerator enumerator3 = this.m_statusIndicatorVFXList.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					AttachedActorVFXInfo attachedActorVFXInfo = enumerator3.Current;
					if (attachedActorVFXInfo != null)
					{
						attachedActorVFXInfo.DestroyVfx();
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
			this.m_statusIndicatorVFXList.Clear();
		}
		for (int i = 0; i < this.m_durationBasedVfxList.Count; i++)
		{
			DurationActorVFXInfo durationActorVFXInfo = this.m_durationBasedVfxList[i];
			if (durationActorVFXInfo != null)
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
				durationActorVFXInfo.DestroyVfx();
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
		if (this.m_friendBaseCircleContainer != null)
		{
			this.m_friendBaseCircleContainer.DestroyVfx();
			this.m_friendBaseCircleContainer = null;
		}
		if (this.m_enemyBaseCircleContainer != null)
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
			this.m_enemyBaseCircleContainer.DestroyVfx();
			this.m_enemyBaseCircleContainer = null;
		}
	}

	private bool BeingChasedByClient()
	{
		bool result = false;
		GameFlowData gameFlowData = GameFlowData.Get();
		if (gameFlowData)
		{
			ActorData activeOwnedActorData = gameFlowData.activeOwnedActorData;
			if (activeOwnedActorData != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.BeingChasedByClient()).MethodHandle;
				}
				ActorData actorData = this.m_actorData;
				if (activeOwnedActorData.\u0012() == actorData)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.OnMove(BoardSquarePathInfo, BoardSquarePathInfo)).MethodHandle;
			}
			if (this.BeingChasedByClient())
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
				ActorData actorData = this.m_actorData;
				bool flag;
				if (actorData.\u000E(localPlayerData, false, false))
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
					flag = !actorData.\u000E(localPlayerData, false);
				}
				else
				{
					flag = false;
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
					ActorVFX.FootstepVFX item = new ActorVFX.FootstepVFX(square, this.m_actorData);
					this.m_footstepsVFX.Add(item);
				}
			}
		}
		BrushCoordinator brushCoordinator = BrushCoordinator.Get();
		if (square != null)
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
			if (prevNode != null && brushCoordinator && FogOfWar.GetClientFog() != null)
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
				BoardSquare square2 = prevNode.square;
				bool flag2 = brushCoordinator.IsRegionFunctioning(square.BrushRegion);
				bool flag3 = brushCoordinator.IsRegionFunctioning(square2.BrushRegion);
				if (flag2 != flag3)
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
					if (BoardSquarePathInfo.IsConnectionTypeConventional(prevNode.connectionType))
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
						if (!moveToNode.m_visibleToEnemies)
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
							if (!prevNode.m_visibleToEnemies)
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
								if (this.m_actorData.\u000E() != localPlayerData.GetTeamViewing())
								{
									goto IL_1A2;
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
						ActorVFX.BrushMoveVFX item2 = new ActorVFX.BrushMoveVFX(square2, square, flag2);
						this.m_brushMoveVFX.Add(item2);
					}
				}
			}
		}
		IL_1A2:
		if (this.m_hitInCoverVfxContainer != null)
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
			this.m_hitInCoverVfxContainer.HideVfx();
		}
	}

	private bool CanBeSeenInBrush()
	{
		ActorData actorData = this.m_actorData;
		bool flag;
		if (actorData.\u0016())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.CanBeSeenInBrush()).MethodHandle;
			}
			flag = !BrushCoordinator.Get().IsRegionFunctioning(actorData.\u0018());
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		ActorStatus component = base.GetComponent<ActorStatus>();
		bool flag3;
		if (!component.HasStatus(StatusType.CantHideInBrush, false))
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
			flag3 = component.HasStatus(StatusType.Revealed, false);
		}
		else
		{
			flag3 = true;
		}
		bool flag4 = flag3;
		bool flag5 = CaptureTheFlag.IsActorRevealedByFlag_Client(actorData);
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(actorData.\u0016(), GameplayData.Get().m_distanceCanSeeIntoBrush, true, null, actorData.\u0012(), null, false, default(Vector3));
		bool flag6;
		if (actorsInRadius.Count > 0)
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
			flag6 = true;
		}
		else
		{
			flag6 = false;
		}
		bool flag7 = false;
		List<ActorData> actorsInBrushRegion = BrushCoordinator.Get().GetActorsInBrushRegion(actorData.\u0018());
		if (actorsInBrushRegion != null)
		{
			foreach (ActorData actorData2 in actorsInBrushRegion)
			{
				if (actorData2.\u000E() != actorData.\u000E())
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
					flag7 = true;
					break;
				}
			}
		}
		bool result;
		if (!flag2 && !flag4 && !flag5 && !flag6)
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
			result = flag7;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public void ShowHitWhileInCoverVfx(Vector3 actorPos, Vector3 hitOrigin, ActorData caster)
	{
		this.ShowDurationVfxWithDirection(this.m_hitInCoverVfxContainer, actorPos, hitOrigin, caster);
	}

	public void ShowKnockbackWhileUnstoppableVfx(Vector3 actorPos, Vector3 hitOrigin, ActorData caster)
	{
		this.ShowDurationVfxWithDirection(this.m_onKnockbackWhileUnstoppableContainer, actorPos, hitOrigin, caster);
	}

	private void ShowDurationVfxWithDirection(DurationActorVFXInfo container, Vector3 actorPos, Vector3 hitOrigin, ActorData caster)
	{
		if (container != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.ShowDurationVfxWithDirection(DurationActorVFXInfo, Vector3, Vector3, ActorData)).MethodHandle;
			}
			Vector3 vector = hitOrigin - actorPos;
			vector.y = 0f;
			vector.Normalize();
			if (vector == Vector3.zero)
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
				if (caster != null)
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
					if (caster.\u0012() != null)
					{
						vector = caster.\u0016() - actorPos;
						vector.y = 0f;
						vector.Normalize();
					}
				}
				if (vector == Vector3.zero)
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
					vector = Vector3.forward;
				}
			}
			container.ShowVfx(this.m_actorData.\u0018(), vector);
		}
	}

	public void ShowOnDeathVfx(Vector3 actorPos)
	{
		if (this.m_onDeathVfxContainer != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.ShowOnDeathVfx(Vector3)).MethodHandle;
			}
			this.m_onDeathVfxContainer.ShowVfxAtPosition(actorPos, this.m_actorData.\u0018(), Vector3.zero);
		}
	}

	public void ShowOnRespawnVfx()
	{
		if (this.m_onRespawnVFXContainer != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.ShowOnRespawnVfx()).MethodHandle;
			}
			this.m_onRespawnVFXContainer.ShowVfxAtPosition(this.m_actorData.\u0016(), true, Vector3.zero);
			PlayerData playerData;
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
				playerData = GameFlowData.Get().LocalPlayerData;
			}
			else
			{
				playerData = null;
			}
			PlayerData playerData2 = playerData;
			if (playerData2 != null)
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
				bool flag = this.m_actorData.\u000E() == playerData2.GetTeamViewing();
				AudioManager.PostEvent((!flag) ? "ui/ingame/resurrection/enemy" : "ui/ingame/resurrection/ally", base.gameObject);
			}
		}
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		bool result;
		return result;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
	}

	private class FootstepVFX
	{
		public GameObject m_vfxObj;

		public BoardSquare m_square;

		public FootstepVFX(BoardSquare square, ActorData runner)
		{
			this.m_square = square;
			if (HighlightUtils.Get().m_footstepsVFXPrefab != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.FootstepVFX..ctor(BoardSquare, ActorData)).MethodHandle;
				}
				if (FogOfWar.GetClientFog() != null)
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
					this.m_vfxObj = UnityEngine.Object.Instantiate<GameObject>(HighlightUtils.Get().m_footstepsVFXPrefab);
					this.m_vfxObj.transform.position = square.\u001D() + new Vector3(0f, 0.1f, 0f);
					this.m_vfxObj.transform.rotation = runner.transform.rotation;
					bool active = FogOfWar.GetClientFog().IsVisible(this.m_square);
					this.m_vfxObj.SetActive(active);
					return;
				}
			}
			this.m_vfxObj = null;
		}
	}

	private class BrushMoveVFX
	{
		public GameObject m_vfxObj;

		public int m_spawnTurn;

		public BrushMoveVFX(BoardSquare prevSquare, BoardSquare moveToSquare, bool movingIntoBrush)
		{
			GameObject gameObject = (!movingIntoBrush) ? HighlightUtils.Get().m_moveOutOfBrushVfxPrefab : HighlightUtils.Get().m_moveIntoBrushVfxPrefab;
			this.m_vfxObj = null;
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorVFX.BrushMoveVFX..ctor(BoardSquare, BoardSquare, bool)).MethodHandle;
				}
				BoardSquare boardSquare = moveToSquare;
				BoardSquare boardSquare2 = prevSquare;
				if (!movingIntoBrush)
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
					boardSquare = prevSquare;
					boardSquare2 = moveToSquare;
				}
				this.m_spawnTurn = GameFlowData.Get().CurrentTurn;
				Vector3 vector = boardSquare.ToVector3();
				Vector3 vector2 = boardSquare2.ToVector3() - vector;
				vector2.y = 0f;
				vector2.Normalize();
				Vector3 position = vector + 0.5f * Board.SquareSizeStatic * vector2;
				this.m_vfxObj = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				if (this.m_vfxObj != null)
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
					this.m_vfxObj.transform.position = position;
					this.m_vfxObj.transform.rotation = Quaternion.LookRotation(vector2);
				}
			}
		}
	}
}
