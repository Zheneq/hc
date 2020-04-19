using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActorStatus : NetworkBehaviour
{
	private SyncListUInt m_statusCounts = new SyncListUInt();

	private SyncListUInt m_statusDurations = new SyncListUInt();

	private int[] m_statusCountsPrevious;

	private int[] m_clientStatusCountAdjustments;

	private ActorData m_actorData;

	private List<Ability> m_passivePendingStatusSources = new List<Ability>();

	public const string STATUS_DEBUG_HEADER = "<color=cyan>ActorStatus</color>: ";

	private static int kListm_statusCounts = -0x6E592F;

	private static int kListm_statusDurations;

	static ActorStatus()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorStatus), ActorStatus.kListm_statusCounts, new NetworkBehaviour.CmdDelegate(ActorStatus.InvokeSyncListm_statusCounts));
		ActorStatus.kListm_statusDurations = 0x254A88B2;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorStatus), ActorStatus.kListm_statusDurations, new NetworkBehaviour.CmdDelegate(ActorStatus.InvokeSyncListm_statusDurations));
		NetworkCRC.RegisterBehaviour("ActorStatus", 0);
	}

	private void Awake()
	{
		this.m_statusCountsPrevious = new int[0x3A];
		this.m_clientStatusCountAdjustments = new int[0x3A];
		this.m_actorData = base.GetComponent<ActorData>();
		this.m_statusCounts.InitializeBehaviour(this, ActorStatus.kListm_statusCounts);
		this.m_statusDurations.InitializeBehaviour(this, ActorStatus.kListm_statusDurations);
	}

	public int GetDurationOfStatus(StatusType status)
	{
		return (int)this.m_statusDurations[(int)status];
	}

	public override void OnStartClient()
	{
		this.m_statusCounts.Callback = new SyncList<uint>.SyncListChanged(this.SyncListCallbackStatusCounts);
		this.m_statusDurations.Callback = new SyncList<uint>.SyncListChanged(this.SyncListCallbackStatusDuration);
	}

	private void SyncListCallbackStatusDuration(SyncList<uint>.Operation op, int i)
	{
		if (NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.SyncListCallbackStatusDuration(SyncList<uint>.Operation, int)).MethodHandle;
			}
			if (i >= 0)
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
				if (i < 0x3A)
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
					ActorData actorData = this.m_actorData;
					if (HUD_UI.Get() != null && actorData != null)
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
						HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.NotifyStatusDurationChange(actorData, (StatusType)i, (int)this.m_statusDurations[i]);
						if (actorData == GameFlowData.Get().activeOwnedActorData)
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
							HUD_UI.Get().m_mainScreenPanel.m_characterProfile.UpdateStatusDisplay(true);
						}
					}
				}
			}
		}
	}

	public override void OnStartServer()
	{
		for (int i = 0; i < this.m_statusCountsPrevious.Length; i++)
		{
			this.m_statusCounts.Add(0U);
			this.m_statusDurations.Add(0U);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.OnStartServer()).MethodHandle;
		}
		if (GameplayUtils.IsPlayerControlled(this.m_actorData))
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
			int num = GameplayData.Get().m_recentlySpawnedDuration + 1;
			for (int j = 0; j < num; j++)
			{
				this.AddStatus(StatusType.RecentlySpawned, 1);
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

	private void SyncListCallbackStatusCounts(SyncList<uint>.Operation op, int i)
	{
		if (!NetworkServer.active && i >= 0 && i < 0x3A)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.SyncListCallbackStatusCounts(SyncList<uint>.Operation, int)).MethodHandle;
			}
			int num = this.m_statusCountsPrevious[i];
			num += this.m_clientStatusCountAdjustments[i];
			this.m_clientStatusCountAdjustments[i] = 0;
			this.m_statusCountsPrevious[i] = (int)this.m_statusCounts[i];
			if ((ulong)this.m_statusCounts[i] != (ulong)((long)num))
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
				bool flag = num > 0;
				bool flag2 = this.HasStatus((StatusType)i, true);
				if (flag != flag2)
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
					this.OnStatusChanged((StatusType)i, flag2);
				}
			}
		}
	}

	public void UpdateStatusDuration(StatusType status, int newDuration)
	{
		if ((long)newDuration > (long)((ulong)this.m_statusDurations[(int)status]))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.UpdateStatusDuration(StatusType, int)).MethodHandle;
			}
			this.m_statusDurations[(int)status] = (uint)Mathf.Max(0, newDuration);
		}
	}

	[Server]
	public void AddStatus(StatusType status, int duration)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.AddStatus(StatusType, int)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void ActorStatus::AddStatus(StatusType,System.Int32)' called on client");
			return;
		}
		int num = (int)this.m_statusCounts[(int)status];
		this.m_statusCounts[(int)status] = (uint)(num + 1);
		if ((long)duration > (long)((ulong)this.m_statusDurations[(int)status]))
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
			this.m_statusDurations[(int)status] = (uint)Mathf.Max(0, duration);
		}
		num += this.m_clientStatusCountAdjustments[(int)status];
		this.m_clientStatusCountAdjustments[(int)status] = 0;
		if (ActorStatus.\u001D)
		{
			Log.Warning(string.Concat(new object[]
			{
				"<color=cyan>ActorStatus</color>: ADD ",
				ActorStatus.GetColoredStatusName(status, "yellow"),
				" to ",
				this.m_actorData.\u0012("white"),
				", Count = ",
				this.m_statusCounts[(int)status],
				", PrevCount = ",
				num
			}), new object[0]);
		}
		if (num == 0)
		{
			this.OnStatusChanged(status, true);
		}
	}

	[Server]
	public void RemoveStatus(StatusType status)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.RemoveStatus(StatusType)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void ActorStatus::RemoveStatus(StatusType)' called on client");
			return;
		}
		int num = (int)this.m_statusCounts[(int)status];
		if (num > 0)
		{
			this.m_statusCounts[(int)status] = (uint)Mathf.Max(0, num - 1);
			num += this.m_clientStatusCountAdjustments[(int)status];
			this.m_clientStatusCountAdjustments[(int)status] = 0;
			if (ActorStatus.\u001D)
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
				Log.Warning(string.Concat(new object[]
				{
					"<color=cyan>ActorStatus</color>: REMOVE ",
					ActorStatus.GetColoredStatusName(status, "yellow"),
					" from ",
					this.m_actorData.\u0012("white"),
					", Count = ",
					this.m_statusCounts[(int)status],
					", PrevCount: ",
					num
				}), new object[0]);
			}
			if (num == 1)
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
				this.OnStatusChanged(status, false);
			}
		}
		else
		{
			Log.Error(string.Format("Removing status '{0}' that was never added", status), new object[0]);
		}
	}

	[Client]
	public void ClientAddStatus(StatusType status)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ActorStatus::ClientAddStatus(StatusType)' called on server");
			return;
		}
		int num = this.m_clientStatusCountAdjustments[(int)status];
		this.m_clientStatusCountAdjustments[(int)status] = num + 1;
		if (ActorStatus.\u001D)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.ClientAddStatus(StatusType)).MethodHandle;
			}
			Log.Warning(string.Concat(new object[]
			{
				"<color=cyan>ActorStatus</color>: <color=cyan>CLIENT_ADD</color> ",
				ActorStatus.GetColoredStatusName(status, "yellow"),
				" to ",
				this.m_actorData.\u0012("white"),
				", ClientAdjust = ",
				this.m_clientStatusCountAdjustments[(int)status],
				", SyncCount = ",
				this.m_statusCounts[(int)status]
			}), new object[0]);
		}
		if ((ulong)this.m_statusCounts[(int)status] + (ulong)((long)this.m_clientStatusCountAdjustments[(int)status]) == 1UL)
		{
			this.OnStatusChanged(status, true);
		}
	}

	[Client]
	public void ClientRemoveStatus(StatusType status)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.ClientRemoveStatus(StatusType)).MethodHandle;
			}
			Debug.LogWarning("[Client] function 'System.Void ActorStatus::ClientRemoveStatus(StatusType)' called on server");
			return;
		}
		int num = this.m_clientStatusCountAdjustments[(int)status];
		this.m_clientStatusCountAdjustments[(int)status] = num - 1;
		if (this.m_clientStatusCountAdjustments[(int)status] < 0)
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
			this.m_clientStatusCountAdjustments[(int)status] = 0;
		}
		if (ActorStatus.\u001D)
		{
			Log.Warning(string.Concat(new object[]
			{
				"<color=cyan>ActorStatus</color>: <color=magenta>CLIENT_REMOVE</color> ",
				ActorStatus.GetColoredStatusName(status, "yellow"),
				" from ",
				this.m_actorData.\u0012("white"),
				", ClientAdjust = ",
				this.m_clientStatusCountAdjustments[(int)status],
				", SyncCount = ",
				this.m_statusCounts[(int)status]
			}), new object[0]);
		}
		if ((ulong)this.m_statusCounts[(int)status] + (ulong)((long)this.m_clientStatusCountAdjustments[(int)status]) == 0UL)
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
			this.OnStatusChanged(status, false);
		}
	}

	[Client]
	public void ClientClearAdjustments()
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.ClientClearAdjustments()).MethodHandle;
			}
			Debug.LogWarning("[Client] function 'System.Void ActorStatus::ClientClearAdjustments()' called on server");
			return;
		}
		for (int i = 0; i < this.m_clientStatusCountAdjustments.Length; i++)
		{
			if (this.m_clientStatusCountAdjustments[i] != 0)
			{
				this.m_clientStatusCountAdjustments[i] = 0;
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

	public bool HasStatus(StatusType status, bool includePending = true)
	{
		int num = (int)((status >= (StatusType)this.m_statusCounts.Count) ? 0U : this.m_statusCounts[(int)status]);
		num += this.m_clientStatusCountAdjustments[(int)status];
		bool flag = num > 0;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.HasStatus(StatusType, bool)).MethodHandle;
			}
			if (includePending)
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
				if (this.m_passivePendingStatusSources.Count > 0)
				{
					int i = 0;
					while (i < this.m_passivePendingStatusSources.Count)
					{
						if (flag)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								goto IL_C8;
							}
						}
						else
						{
							Ability ability = this.m_passivePendingStatusSources[i];
							bool flag2 = flag;
							bool flag3;
							if (ability != null)
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
								flag3 = ability.HasPassivePendingStatus(status, this.m_actorData);
							}
							else
							{
								flag3 = false;
							}
							flag = (flag2 || flag3);
							i++;
						}
					}
				}
			}
		}
		IL_C8:
		if (GameplayMutators.Get() != null)
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
			int currentTurn = GameFlowData.Get().CurrentTurn;
			if (!flag)
			{
				flag |= GameplayMutators.IsStatusActive(status, currentTurn, GameplayMutators.ActionPhaseCheckMode.Default);
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
				flag = !GameplayMutators.IsStatusSuppressed(status, currentTurn, GameplayMutators.ActionPhaseCheckMode.Default);
			}
		}
		return flag;
	}

	public void AddAbilityForPassivePendingStatus(Ability ability)
	{
		if (ability != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.AddAbilityForPassivePendingStatus(Ability)).MethodHandle;
			}
			if (!this.m_passivePendingStatusSources.Contains(ability))
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
				this.m_passivePendingStatusSources.Add(ability);
			}
		}
	}

	public void RemoveAbilityForPassivePendingStatus(Ability ability)
	{
		this.m_passivePendingStatusSources.Remove(ability);
	}

	public bool IsKnockbackImmune(bool checkPending = true)
	{
		bool flag;
		if (!this.HasStatus(StatusType.KnockbackImmune, true))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.IsKnockbackImmune(bool)).MethodHandle;
			}
			flag = this.HasStatus(StatusType.Unstoppable, true);
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		if (!flag2)
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
			if (this.m_actorData.\u000E() != null)
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
				if (checkPending)
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
					flag2 = (this.m_actorData.\u000E().HasPendingStatusFromQueuedAbilities(StatusType.Unstoppable) || this.m_actorData.\u000E().HasPendingStatusFromQueuedAbilities(StatusType.KnockbackImmune));
				}
			}
		}
		return flag2;
	}

	public bool IsMovementDebuffImmune(bool checkPending = true)
	{
		bool flag;
		if (!this.HasStatus(StatusType.MovementDebuffImmunity, true))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.IsMovementDebuffImmune(bool)).MethodHandle;
			}
			flag = this.HasStatus(StatusType.Unstoppable, true);
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
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
			if (this.m_actorData.\u000E() != null)
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
				if (checkPending)
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
					bool flag3;
					if (!this.m_actorData.\u000E().HasPendingStatusFromQueuedAbilities(StatusType.Unstoppable))
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
						flag3 = this.m_actorData.\u000E().HasPendingStatusFromQueuedAbilities(StatusType.MovementDebuffImmunity);
					}
					else
					{
						flag3 = true;
					}
					flag2 = flag3;
				}
			}
		}
		return flag2;
	}

	public bool IsEnergized(bool checkPending = true)
	{
		bool flag = this.HasStatus(StatusType.Energized, true);
		if (checkPending)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.IsEnergized(bool)).MethodHandle;
			}
			if (!flag && this.m_actorData.\u000E() != null)
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
				flag = this.m_actorData.\u000E().HasPendingStatusFromQueuedAbilities(StatusType.Energized);
			}
		}
		return flag;
	}

	public void OnStatusChanged(StatusType status, bool statusGained)
	{
		if (ActorStatus.\u001D)
		{
			string[] array = new string[5];
			array[0] = "<color=cyan>ActorStatus</color>: On Status Changed: <color=yellow>";
			array[1] = status.ToString();
			array[2] = "</color> ";
			int num = 3;
			string text;
			if (statusGained)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.OnStatusChanged(StatusType, bool)).MethodHandle;
				}
				text = "<color=cyan>Gained";
			}
			else
			{
				text = "<color=magenta>Lost";
			}
			array[num] = text;
			array[4] = "</color>";
			Log.Warning(string.Concat(array), new object[0]);
		}
		if (!statusGained)
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
			this.m_statusDurations[(int)status] = 0U;
		}
		ActorData actorData = this.m_actorData;
		if (HUD_UI.Get() != null)
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
			if (actorData != null)
			{
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.NotifyStatusChange(actorData, status, statusGained);
				if (actorData == GameFlowData.Get().activeOwnedActorData)
				{
					HUD_UI.Get().m_mainScreenPanel.m_characterProfile.UpdateStatusDisplay(true);
				}
			}
		}
		actorData.ForceUpdateIsVisibleToClientCache();
		if (statusGained)
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
			if (actorData.\u0018())
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
				if (status != StatusType.Snared)
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
					if (status != StatusType.Rooted)
					{
						goto IL_151;
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
				AudioManager.PostEvent("ablty/generic/snare", actorData.gameObject);
			}
		}
		IL_151:
		switch (status)
		{
		case StatusType.SeeThroughBrush:
			goto IL_310;
		case StatusType.CantCollectPowerups:
			goto IL_395;
		default:
			switch (status)
			{
			case StatusType.Revealed:
			case StatusType.CantHideInBrush:
				FogOfWar.CalculateFogOfWarForTeam(actorData.\u0012());
				goto IL_395;
			case StatusType.CantSprint_UnlessUnstoppable:
			case StatusType.CantSprint_Absolute:
			case StatusType.CrippledMovement:
				break;
			case StatusType.RecentlySpawned:
			case StatusType.RecentlyRespawned:
				goto IL_249;
			case StatusType.KnockbackImmune:
			case StatusType.KnockbackResistant:
			case StatusType.DamageImmune:
			case StatusType.HealImmune:
			case StatusType.CantBeTargeted:
			case StatusType.CantBeHelpedByTeam:
			case StatusType.EffectImmune:
				goto IL_395;
			case StatusType.BuffImmune:
			case StatusType.DebuffImmune:
				goto IL_235;
			case StatusType.IsolateVisionFromAllies:
				goto IL_310;
			case StatusType.LoseAllyVision:
				goto IL_2E7;
			case StatusType.InvisibleToEnemies:
				goto IL_30B;
			default:
				goto IL_395;
			}
			break;
		case StatusType.Blind:
			goto IL_2E7;
		case StatusType.AnchoredNoMovement:
		case StatusType.Rooted:
		case StatusType.Snared:
		case StatusType.KnockedBack:
			break;
		case StatusType.MovementDebuffSuppression:
		case StatusType.Hasted:
			goto IL_249;
		case StatusType.MovementDebuffImmunity:
		case StatusType.Unstoppable:
			goto IL_235;
		case StatusType.ProximityBasedInvisibility:
			goto IL_30B;
		case StatusType.Farsight:
			if (statusGained)
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
				base.GetComponent<ActorStats>().AddStatMod(StatType.SightRange, ModType.BaseAdd, 4f);
			}
			else
			{
				base.GetComponent<ActorStats>().RemoveStatMod(StatType.SightRange, ModType.BaseAdd, 4f);
			}
			base.GetComponent<FogOfWar>().MarkForRecalculateVisibility();
			goto IL_395;
		case StatusType.Myopic:
			if (statusGained)
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
				base.GetComponent<ActorStats>().AddStatMod(StatType.SightRange, ModType.BaseAdd, -4f);
			}
			else
			{
				base.GetComponent<ActorStats>().RemoveStatMod(StatType.SightRange, ModType.BaseAdd, -4f);
			}
			base.GetComponent<FogOfWar>().MarkForRecalculateVisibility();
			goto IL_395;
		case StatusType.DecreasedIncomingHealing:
			if (statusGained)
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
				base.GetComponent<ActorStats>().AddStatMod(StatType.IncomingHealing, ModType.Multiplier, 0.5f);
			}
			else
			{
				base.GetComponent<ActorStats>().RemoveStatMod(StatType.IncomingHealing, ModType.Multiplier, 0.5f);
			}
			goto IL_395;
		case StatusType.IncreasedIncomingHealing:
			if (statusGained)
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
				base.GetComponent<ActorStats>().AddStatMod(StatType.IncomingHealing, ModType.Multiplier, 1.25f);
			}
			else
			{
				base.GetComponent<ActorStats>().RemoveStatMod(StatType.IncomingHealing, ModType.Multiplier, 1.25f);
			}
			goto IL_395;
		}
		this.UpdateMovementForMovementStatus(statusGained);
		goto IL_395;
		IL_235:
		this.HandleStatusImmunityChangeForEffects(status, statusGained);
		this.UpdateMovementForMovementStatus(statusGained);
		goto IL_395;
		IL_249:
		this.UpdateMovementForMovementStatus(statusGained);
		goto IL_395;
		IL_2E7:
		base.GetComponent<FogOfWar>().MarkForRecalculateVisibility();
		IL_30B:
		goto IL_395;
		IL_310:
		FogOfWar.CalculateFogOfWarForTeam(actorData.\u000E());
		IL_395:
		if (Board.\u000E() != null)
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
			Board.\u000E().MarkForUpdateValidSquares(true);
		}
	}

	private void UpdateSquaresCanMoveTo()
	{
		if (this.m_actorData.\u000E() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.UpdateSquaresCanMoveTo()).MethodHandle;
			}
			this.m_actorData.\u000E().UpdateSquaresCanMoveTo();
		}
	}

	private void UpdateMovementForMovementStatus(bool gained)
	{
		this.UpdateSquaresCanMoveTo();
		if (NetworkClient.active && this.m_actorData != null && gained)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.UpdateMovementForMovementStatus(bool)).MethodHandle;
			}
			LineData component = this.m_actorData.GetComponent<LineData>();
			if (component != null)
			{
				component.OnMovementStatusGained();
			}
		}
	}

	private void HandleStatusImmunityChangeForEffects(StatusType status, bool gained)
	{
	}

	public bool IsImmuneToForcedChase()
	{
		return this.HasStatus(StatusType.Unstoppable, true);
	}

	public bool IsInvisibleToEnemies(bool includePendingStatus = true)
	{
		bool flag = this.HasStatus(StatusType.InvisibleToEnemies, includePendingStatus);
		ActorData actorData = this.m_actorData;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.IsInvisibleToEnemies(bool)).MethodHandle;
			}
			if (this.HasStatus(StatusType.ProximityBasedInvisibility, includePendingStatus))
			{
				bool flag2 = false;
				List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(actorData.\u0012());
				using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData2 = enumerator.Current;
						BoardSquare boardSquare = Board.\u000E().\u000E(actorData.\u0016());
						BoardSquare boardSquare2 = Board.\u000E().\u000E(actorData2.\u0016());
						if (!(boardSquare == null))
						{
							if (boardSquare2 == null)
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
								float num = boardSquare.HorizontalDistanceOnBoardTo(boardSquare2);
								if (num <= GameplayData.Get().m_proximityBasedInvisibilityMinDistance)
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
									if (!GameplayData.Get().m_blindEnemyBreaksProximityBasedInvisibility)
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
										if (!(actorData2.\u000E() != null) || actorData2.\u000E().HasStatus(StatusType.Blind, includePendingStatus))
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
									flag2 = true;
									goto IL_15A;
								}
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
				IL_15A:
				if (!flag2)
				{
					flag = true;
				}
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
			if (!actorData.ServerSuppressInvisibility)
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
				return !this.HasStatus(StatusType.SuppressInvisibility, includePendingStatus);
			}
		}
		return false;
	}

	public bool IsActionSilenced(AbilityData.ActionType action, bool checkPending = false)
	{
		bool flag;
		if (this.HasStatus(StatusType.SilencedAllAbilities, checkPending))
		{
			flag = true;
		}
		else
		{
			bool flag2;
			if (action >= AbilityData.ActionType.ABILITY_0)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.IsActionSilenced(AbilityData.ActionType, bool)).MethodHandle;
				}
				flag2 = (action <= AbilityData.ActionType.ABILITY_6);
			}
			else
			{
				flag2 = false;
			}
			bool flag3 = flag2;
			bool flag4;
			if (action >= AbilityData.ActionType.CARD_0)
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
				flag4 = (action <= AbilityData.ActionType.CARD_2);
			}
			else
			{
				flag4 = false;
			}
			bool flag5 = flag4;
			bool flag6 = action == AbilityData.ActionType.ABILITY_0;
			flag = false;
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
				if (this.HasStatus(StatusType.SilencedPlayerAbilities, checkPending))
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
					flag = (flag || flag3);
				}
			}
			if (flag3 && !flag6)
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
				if (this.HasStatus(StatusType.SilencedNonbasicPlayerAbilities, checkPending))
				{
					bool flag7 = flag;
					bool flag8;
					if (flag3)
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
						flag8 = !flag6;
					}
					else
					{
						flag8 = false;
					}
					flag = (flag7 || flag8);
				}
			}
			if (flag6 && this.HasStatus(StatusType.SilencedBasicPlayerAbility, checkPending))
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
				flag = (flag || flag6);
			}
			if (flag5 && this.HasStatus(StatusType.SilencedCardAbilities, checkPending))
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
				flag = (flag || flag5);
			}
			if (this.HasStatus(StatusType.SilencedEvasionAbilities, checkPending))
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
				Ability abilityOfActionType = base.GetComponent<AbilityData>().GetAbilityOfActionType(action);
				bool flag9;
				if (abilityOfActionType != null)
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
					flag9 = (abilityOfActionType.RunPriority == AbilityPriority.Evasion);
					foreach (Ability ability in abilityOfActionType.GetChainAbilities())
					{
						flag9 |= (ability.RunPriority == AbilityPriority.Evasion);
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
				else
				{
					flag9 = false;
				}
				flag = (flag || flag9);
			}
		}
		return flag;
	}

	public static bool IsDispellableMovementDebuff(StatusType status)
	{
		if (status != StatusType.Rooted)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.IsDispellableMovementDebuff(StatusType)).MethodHandle;
			}
			if (status != StatusType.Snared)
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
				if (status != StatusType.CrippledMovement)
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
					return status == StatusType.CantSprint_UnlessUnstoppable;
				}
			}
		}
		return true;
	}

	public static string GetColoredStatusName(StatusType status, string color)
	{
		return string.Concat(new string[]
		{
			"<color=",
			color,
			">[",
			status.ToString(),
			"]</color>"
		});
	}

	public static bool \u001D
	{
		get
		{
			return false;
		}
	}

	public int \u001D(StatusType \u001D)
	{
		if (\u001D >= StatusType.Revealed)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.\u001D(StatusType)).MethodHandle;
			}
			if (\u001D < (StatusType)this.m_statusCounts.Count)
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
				return (int)(this.m_statusCounts[(int)\u001D] + (uint)this.m_clientStatusCountAdjustments[(int)\u001D]);
			}
		}
		return 0;
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_statusCounts(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_statusCounts called on server.");
			return;
		}
		((ActorStatus)obj).m_statusCounts.HandleMsg(reader);
	}

	protected static void InvokeSyncListm_statusDurations(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.InvokeSyncListm_statusDurations(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("SyncList m_statusDurations called on server.");
			return;
		}
		((ActorStatus)obj).m_statusDurations.HandleMsg(reader);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ActorStatus.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			SyncListUInt.WriteInstance(writer, this.m_statusCounts);
			SyncListUInt.WriteInstance(writer, this.m_statusDurations);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
			SyncListUInt.WriteInstance(writer, this.m_statusCounts);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
			SyncListUInt.WriteInstance(writer, this.m_statusDurations);
		}
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
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListUInt.ReadReference(reader, this.m_statusCounts);
			SyncListUInt.ReadReference(reader, this.m_statusDurations);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListUInt.ReadReference(reader, this.m_statusCounts);
		}
		if ((num & 2) != 0)
		{
			SyncListUInt.ReadReference(reader, this.m_statusDurations);
		}
	}
}
