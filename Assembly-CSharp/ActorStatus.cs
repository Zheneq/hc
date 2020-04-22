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

	private static int kListm_statusCounts;

	private static int kListm_statusDurations;

	public static bool DebugLog => false;

	static ActorStatus()
	{
		kListm_statusCounts = -7231791;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorStatus), kListm_statusCounts, InvokeSyncListm_statusCounts);
		kListm_statusDurations = 625641650;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorStatus), kListm_statusDurations, InvokeSyncListm_statusDurations);
		NetworkCRC.RegisterBehaviour("ActorStatus", 0);
	}

	private void Awake()
	{
		m_statusCountsPrevious = new int[58];
		m_clientStatusCountAdjustments = new int[58];
		m_actorData = GetComponent<ActorData>();
		m_statusCounts.InitializeBehaviour(this, kListm_statusCounts);
		m_statusDurations.InitializeBehaviour(this, kListm_statusDurations);
	}

	public int GetDurationOfStatus(StatusType status)
	{
		return (int)m_statusDurations[(int)status];
	}

	public override void OnStartClient()
	{
		m_statusCounts.Callback = SyncListCallbackStatusCounts;
		m_statusDurations.Callback = SyncListCallbackStatusDuration;
	}

	private void SyncListCallbackStatusDuration(SyncList<uint>.Operation op, int i)
	{
		if (!NetworkClient.active)
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
			if (i < 0)
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
				if (i >= 58)
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
					ActorData actorData = m_actorData;
					if (!(HUD_UI.Get() != null) || !(actorData != null))
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
						HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.NotifyStatusDurationChange(actorData, (StatusType)i, (int)m_statusDurations[i]);
						if (actorData == GameFlowData.Get().activeOwnedActorData)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								HUD_UI.Get().m_mainScreenPanel.m_characterProfile.UpdateStatusDisplay(true);
								return;
							}
						}
						return;
					}
				}
			}
		}
	}

	public override void OnStartServer()
	{
		for (int i = 0; i < m_statusCountsPrevious.Length; i++)
		{
			m_statusCounts.Add(0u);
			m_statusDurations.Add(0u);
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
			if (!GameplayUtils.IsPlayerControlled(m_actorData))
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
				int num = GameplayData.Get().m_recentlySpawnedDuration + 1;
				for (int j = 0; j < num; j++)
				{
					AddStatus(StatusType.RecentlySpawned, 1);
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

	private void SyncListCallbackStatusCounts(SyncList<uint>.Operation op, int i)
	{
		if (NetworkServer.active || i < 0 || i >= 58)
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
			int num = m_statusCountsPrevious[i];
			num += m_clientStatusCountAdjustments[i];
			m_clientStatusCountAdjustments[i] = 0;
			m_statusCountsPrevious[i] = (int)m_statusCounts[i];
			if (m_statusCounts[i] == num)
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
				bool flag = num > 0;
				bool flag2 = HasStatus((StatusType)i);
				if (flag != flag2)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						OnStatusChanged((StatusType)i, flag2);
						return;
					}
				}
				return;
			}
		}
	}

	public void UpdateStatusDuration(StatusType status, int newDuration)
	{
		if (newDuration <= m_statusDurations[(int)status])
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
			m_statusDurations[(int)status] = (uint)Mathf.Max(0, newDuration);
			return;
		}
	}

	[Server]
	public void AddStatus(StatusType status, int duration)
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
					Debug.LogWarning("[Server] function 'System.Void ActorStatus::AddStatus(StatusType,System.Int32)' called on client");
					return;
				}
			}
		}
		int num = (int)m_statusCounts[(int)status];
		m_statusCounts[(int)status] = (uint)(num + 1);
		if (duration > m_statusDurations[(int)status])
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
			m_statusDurations[(int)status] = (uint)Mathf.Max(0, duration);
		}
		num += m_clientStatusCountAdjustments[(int)status];
		m_clientStatusCountAdjustments[(int)status] = 0;
		if (DebugLog)
		{
			Log.Warning("<color=cyan>ActorStatus</color>: ADD " + GetColoredStatusName(status, "yellow") + " to " + m_actorData.GetColoredDebugName("white") + ", Count = " + m_statusCounts[(int)status] + ", PrevCount = " + num);
		}
		if (num == 0)
		{
			OnStatusChanged(status, true);
		}
	}

	[Server]
	public void RemoveStatus(StatusType status)
	{
		if (!NetworkServer.active)
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
					Debug.LogWarning("[Server] function 'System.Void ActorStatus::RemoveStatus(StatusType)' called on client");
					return;
				}
			}
		}
		int num = (int)m_statusCounts[(int)status];
		if (num > 0)
		{
			m_statusCounts[(int)status] = (uint)Mathf.Max(0, num - 1);
			num += m_clientStatusCountAdjustments[(int)status];
			m_clientStatusCountAdjustments[(int)status] = 0;
			if (DebugLog)
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
				Log.Warning("<color=cyan>ActorStatus</color>: REMOVE " + GetColoredStatusName(status, "yellow") + " from " + m_actorData.GetColoredDebugName("white") + ", Count = " + m_statusCounts[(int)status] + ", PrevCount: " + num);
			}
			if (num != 1)
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
				OnStatusChanged(status, false);
				return;
			}
		}
		Log.Error($"Removing status '{status}' that was never added");
	}

	[Client]
	public void ClientAddStatus(StatusType status)
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ActorStatus::ClientAddStatus(StatusType)' called on server");
			return;
		}
		int num = m_clientStatusCountAdjustments[(int)status];
		m_clientStatusCountAdjustments[(int)status] = num + 1;
		if (DebugLog)
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
			Log.Warning("<color=cyan>ActorStatus</color>: <color=cyan>CLIENT_ADD</color> " + GetColoredStatusName(status, "yellow") + " to " + m_actorData.GetColoredDebugName("white") + ", ClientAdjust = " + m_clientStatusCountAdjustments[(int)status] + ", SyncCount = " + m_statusCounts[(int)status]);
		}
		if (m_statusCounts[(int)status] + m_clientStatusCountAdjustments[(int)status] == 1)
		{
			OnStatusChanged(status, true);
		}
	}

	[Client]
	public void ClientRemoveStatus(StatusType status)
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
					Debug.LogWarning("[Client] function 'System.Void ActorStatus::ClientRemoveStatus(StatusType)' called on server");
					return;
				}
			}
		}
		int num = m_clientStatusCountAdjustments[(int)status];
		m_clientStatusCountAdjustments[(int)status] = num - 1;
		if (m_clientStatusCountAdjustments[(int)status] < 0)
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
			m_clientStatusCountAdjustments[(int)status] = 0;
		}
		if (DebugLog)
		{
			Log.Warning("<color=cyan>ActorStatus</color>: <color=magenta>CLIENT_REMOVE</color> " + GetColoredStatusName(status, "yellow") + " from " + m_actorData.GetColoredDebugName("white") + ", ClientAdjust = " + m_clientStatusCountAdjustments[(int)status] + ", SyncCount = " + m_statusCounts[(int)status]);
		}
		if (m_statusCounts[(int)status] + m_clientStatusCountAdjustments[(int)status] != 0)
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
			OnStatusChanged(status, false);
			return;
		}
	}

	[Client]
	public void ClientClearAdjustments()
	{
		if (!NetworkClient.active)
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
					Debug.LogWarning("[Client] function 'System.Void ActorStatus::ClientClearAdjustments()' called on server");
					return;
				}
			}
		}
		for (int i = 0; i < m_clientStatusCountAdjustments.Length; i++)
		{
			if (m_clientStatusCountAdjustments[i] != 0)
			{
				m_clientStatusCountAdjustments[i] = 0;
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

	public bool HasStatus(StatusType status, bool includePending = true)
	{
		int num = (int)(((int)status < m_statusCounts.Count) ? m_statusCounts[(int)status] : 0);
		num += m_clientStatusCountAdjustments[(int)status];
		bool flag = num > 0;
		if (!flag)
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
			if (includePending)
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
				if (m_passivePendingStatusSources.Count > 0)
				{
					for (int i = 0; i < m_passivePendingStatusSources.Count; i++)
					{
						if (!flag)
						{
							Ability ability = m_passivePendingStatusSources[i];
							bool num2 = flag;
							int num3;
							if (ability != null)
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
								num3 = (ability.HasPassivePendingStatus(status, m_actorData) ? 1 : 0);
							}
							else
							{
								num3 = 0;
							}
							flag = ((byte)((num2 ? 1 : 0) | num3) != 0);
							continue;
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
						break;
					}
				}
			}
		}
		if (GameplayMutators.Get() != null)
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
			int currentTurn = GameFlowData.Get().CurrentTurn;
			if (!flag)
			{
				flag |= GameplayMutators.IsStatusActive(status, currentTurn);
			}
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
				flag = !GameplayMutators.IsStatusSuppressed(status, currentTurn);
			}
		}
		return flag;
	}

	public void AddAbilityForPassivePendingStatus(Ability ability)
	{
		if (!(ability != null))
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
			if (!m_passivePendingStatusSources.Contains(ability))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					m_passivePendingStatusSources.Add(ability);
					return;
				}
			}
			return;
		}
	}

	public void RemoveAbilityForPassivePendingStatus(Ability ability)
	{
		m_passivePendingStatusSources.Remove(ability);
	}

	public bool IsKnockbackImmune(bool checkPending = true)
	{
		int num;
		if (!HasStatus(StatusType.KnockbackImmune))
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
			num = (HasStatus(StatusType.Unstoppable) ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
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
			if (m_actorData.GetAbilityData() != null)
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
				if (checkPending)
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
					flag = (m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Unstoppable) || m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.KnockbackImmune));
				}
			}
		}
		return flag;
	}

	public bool IsMovementDebuffImmune(bool checkPending = true)
	{
		int num;
		if (!HasStatus(StatusType.MovementDebuffImmunity))
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
			num = (HasStatus(StatusType.Unstoppable) ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
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
			if (m_actorData.GetAbilityData() != null)
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
				if (checkPending)
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
					int num2;
					if (!m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Unstoppable))
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
						num2 = (m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.MovementDebuffImmunity) ? 1 : 0);
					}
					else
					{
						num2 = 1;
					}
					flag = ((byte)num2 != 0);
				}
			}
		}
		return flag;
	}

	public bool IsEnergized(bool checkPending = true)
	{
		bool flag = HasStatus(StatusType.Energized);
		if (checkPending)
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
			if (!flag && m_actorData.GetAbilityData() != null)
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
				flag = m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Energized);
			}
		}
		return flag;
	}

	public void OnStatusChanged(StatusType status, bool statusGained)
	{
		if (DebugLog)
		{
			string[] obj = new string[5]
			{
				"<color=cyan>ActorStatus</color>: On Status Changed: <color=yellow>",
				status.ToString(),
				"</color> ",
				null,
				null
			};
			object obj2;
			if (statusGained)
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
				obj2 = "<color=cyan>Gained";
			}
			else
			{
				obj2 = "<color=magenta>Lost";
			}
			obj[3] = (string)obj2;
			obj[4] = "</color>";
			Log.Warning(string.Concat(obj));
		}
		if (!statusGained)
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
			m_statusDurations[(int)status] = 0u;
		}
		ActorData actorData = m_actorData;
		if (HUD_UI.Get() != null)
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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (actorData.IsVisibleToClient())
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
				if (status != StatusType.Snared)
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
					if (status != StatusType.Rooted)
					{
						goto IL_0151;
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
				AudioManager.PostEvent("ablty/generic/snare", actorData.gameObject);
			}
		}
		goto IL_0151;
		IL_0151:
		switch (status)
		{
		case StatusType.CantSprint_UnlessUnstoppable:
		case StatusType.CantSprint_Absolute:
		case StatusType.CrippledMovement:
		case StatusType.AnchoredNoMovement:
		case StatusType.Rooted:
		case StatusType.Snared:
		case StatusType.KnockedBack:
			UpdateMovementForMovementStatus(statusGained);
			break;
		case StatusType.BuffImmune:
		case StatusType.DebuffImmune:
		case StatusType.MovementDebuffImmunity:
		case StatusType.Unstoppable:
			HandleStatusImmunityChangeForEffects(status, statusGained);
			UpdateMovementForMovementStatus(statusGained);
			break;
		case StatusType.RecentlySpawned:
		case StatusType.RecentlyRespawned:
		case StatusType.MovementDebuffSuppression:
		case StatusType.Hasted:
			UpdateMovementForMovementStatus(statusGained);
			break;
		case StatusType.Farsight:
			if (statusGained)
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
				GetComponent<ActorStats>().AddStatMod(StatType.SightRange, ModType.BaseAdd, 4f);
			}
			else
			{
				GetComponent<ActorStats>().RemoveStatMod(StatType.SightRange, ModType.BaseAdd, 4f);
			}
			GetComponent<FogOfWar>().MarkForRecalculateVisibility();
			break;
		case StatusType.Myopic:
			if (statusGained)
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
				GetComponent<ActorStats>().AddStatMod(StatType.SightRange, ModType.BaseAdd, -4f);
			}
			else
			{
				GetComponent<ActorStats>().RemoveStatMod(StatType.SightRange, ModType.BaseAdd, -4f);
			}
			GetComponent<FogOfWar>().MarkForRecalculateVisibility();
			break;
		case StatusType.LoseAllyVision:
		case StatusType.Blind:
			GetComponent<FogOfWar>().MarkForRecalculateVisibility();
			break;
		case StatusType.Revealed:
		case StatusType.CantHideInBrush:
			FogOfWar.CalculateFogOfWarForTeam(actorData.GetOpposingTeam());
			break;
		case StatusType.IsolateVisionFromAllies:
		case StatusType.SeeThroughBrush:
			FogOfWar.CalculateFogOfWarForTeam(actorData.GetTeam());
			break;
		case StatusType.DecreasedIncomingHealing:
			if (statusGained)
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
				GetComponent<ActorStats>().AddStatMod(StatType.IncomingHealing, ModType.Multiplier, 0.5f);
			}
			else
			{
				GetComponent<ActorStats>().RemoveStatMod(StatType.IncomingHealing, ModType.Multiplier, 0.5f);
			}
			break;
		case StatusType.IncreasedIncomingHealing:
			if (statusGained)
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
				GetComponent<ActorStats>().AddStatMod(StatType.IncomingHealing, ModType.Multiplier, 1.25f);
			}
			else
			{
				GetComponent<ActorStats>().RemoveStatMod(StatType.IncomingHealing, ModType.Multiplier, 1.25f);
			}
			break;
		}
		if (!(Board.Get() != null))
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
			Board.Get().MarkForUpdateValidSquares();
			return;
		}
	}

	private void UpdateSquaresCanMoveTo()
	{
		if (!(m_actorData.GetActorMovement() != null))
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
			m_actorData.GetActorMovement().UpdateSquaresCanMoveTo();
			return;
		}
	}

	private void UpdateMovementForMovementStatus(bool gained)
	{
		UpdateSquaresCanMoveTo();
		if (!NetworkClient.active || !(m_actorData != null) || !gained)
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
			LineData component = m_actorData.GetComponent<LineData>();
			if (component != null)
			{
				component.OnMovementStatusGained();
			}
			return;
		}
	}

	private void HandleStatusImmunityChangeForEffects(StatusType status, bool gained)
	{
	}

	public bool IsImmuneToForcedChase()
	{
		return HasStatus(StatusType.Unstoppable);
	}

	public bool IsInvisibleToEnemies(bool includePendingStatus = true)
	{
		bool flag = HasStatus(StatusType.InvisibleToEnemies, includePendingStatus);
		ActorData actorData = m_actorData;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (HasStatus(StatusType.ProximityBasedInvisibility, includePendingStatus))
			{
				bool flag2 = false;
				List<ActorData> allTeamMembers = GameFlowData.Get().GetAllTeamMembers(actorData.GetOpposingTeam());
				using (List<ActorData>.Enumerator enumerator = allTeamMembers.GetEnumerator())
				{
					while (true)
					{
						if (!enumerator.MoveNext())
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
							break;
						}
						ActorData current = enumerator.Current;
						BoardSquare boardSquare = Board.Get().GetBoardSquare(actorData.GetTravelBoardSquareWorldPosition());
						BoardSquare boardSquare2 = Board.Get().GetBoardSquare(current.GetTravelBoardSquareWorldPosition());
						if (!(boardSquare == null))
						{
							if (boardSquare2 == null)
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
							}
							else
							{
								float num = boardSquare.HorizontalDistanceOnBoardTo(boardSquare2);
								if (num <= GameplayData.Get().m_proximityBasedInvisibilityMinDistance)
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
									if (!GameplayData.Get().m_blindEnemyBreaksProximityBasedInvisibility)
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
										if (!(current.GetActorStatus() != null) || current.GetActorStatus().HasStatus(StatusType.Blind, includePendingStatus))
										{
											continue;
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
									flag2 = true;
									break;
								}
							}
						}
					}
				}
				if (!flag2)
				{
					flag = true;
				}
			}
		}
		int result;
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
			if (!actorData.ServerSuppressInvisibility)
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
				result = ((!HasStatus(StatusType.SuppressInvisibility, includePendingStatus)) ? 1 : 0);
				goto IL_0191;
			}
		}
		result = 0;
		goto IL_0191;
		IL_0191:
		return (byte)result != 0;
	}

	public bool IsActionSilenced(AbilityData.ActionType action, bool checkPending = false)
	{
		bool flag;
		if (HasStatus(StatusType.SilencedAllAbilities, checkPending))
		{
			flag = true;
		}
		else
		{
			int num;
			if (action >= AbilityData.ActionType.ABILITY_0)
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
				num = ((action <= AbilityData.ActionType.ABILITY_6) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag2 = (byte)num != 0;
			int num2;
			if (action >= AbilityData.ActionType.CARD_0)
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
				num2 = ((action <= AbilityData.ActionType.CARD_2) ? 1 : 0);
			}
			else
			{
				num2 = 0;
			}
			bool flag3 = (byte)num2 != 0;
			bool flag4 = action == AbilityData.ActionType.ABILITY_0;
			flag = false;
			if (flag2)
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
				if (HasStatus(StatusType.SilencedPlayerAbilities, checkPending))
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
					flag = (flag || flag2);
				}
			}
			if (flag2 && !flag4)
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
				if (HasStatus(StatusType.SilencedNonbasicPlayerAbilities, checkPending))
				{
					bool num3 = flag;
					int num4;
					if (flag2)
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
					}
					else
					{
						num4 = 0;
					}
					flag = ((byte)((num3 ? 1 : 0) | num4) != 0);
				}
			}
			if (flag4 && HasStatus(StatusType.SilencedBasicPlayerAbility, checkPending))
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
				flag = (flag || flag4);
			}
			if (flag3 && HasStatus(StatusType.SilencedCardAbilities, checkPending))
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
				flag = (flag || flag3);
			}
			if (HasStatus(StatusType.SilencedEvasionAbilities, checkPending))
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
				Ability abilityOfActionType = GetComponent<AbilityData>().GetAbilityOfActionType(action);
				bool flag5;
				if (abilityOfActionType != null)
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
					flag5 = (abilityOfActionType.RunPriority == AbilityPriority.Evasion);
					Ability[] chainAbilities = abilityOfActionType.GetChainAbilities();
					foreach (Ability ability in chainAbilities)
					{
						flag5 |= (ability.RunPriority == AbilityPriority.Evasion);
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
				else
				{
					flag5 = false;
				}
				flag = (flag || flag5);
			}
		}
		return flag;
	}

	public static bool IsDispellableMovementDebuff(StatusType status)
	{
		int result;
		if (status != StatusType.Rooted)
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
			if (status != StatusType.Snared)
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
				if (status != StatusType.CrippledMovement)
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
					result = ((status == StatusType.CantSprint_UnlessUnstoppable) ? 1 : 0);
					goto IL_003c;
				}
			}
		}
		result = 1;
		goto IL_003c;
		IL_003c:
		return (byte)result != 0;
	}

	public static string GetColoredStatusName(StatusType status, string color)
	{
		return "<color=" + color + ">[" + status.ToString() + "]</color>";
	}

	public int _001D(StatusType _001D)
	{
		if (_001D >= StatusType.Revealed)
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
			if ((int)_001D < m_statusCounts.Count)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						return (int)m_statusCounts[(int)_001D] + m_clientStatusCountAdjustments[(int)_001D];
					}
				}
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
		}
		else
		{
			((ActorStatus)obj).m_statusCounts.HandleMsg(reader);
		}
	}

	protected static void InvokeSyncListm_statusDurations(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Debug.LogError("SyncList m_statusDurations called on server.");
					return;
				}
			}
		}
		((ActorStatus)obj).m_statusDurations.HandleMsg(reader);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
					SyncListUInt.WriteInstance(writer, m_statusCounts);
					SyncListUInt.WriteInstance(writer, m_statusDurations);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
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
			SyncListUInt.WriteInstance(writer, m_statusCounts);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
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
			SyncListUInt.WriteInstance(writer, m_statusDurations);
		}
		if (!flag)
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
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListUInt.ReadReference(reader, m_statusCounts);
			SyncListUInt.ReadReference(reader, m_statusDurations);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListUInt.ReadReference(reader, m_statusCounts);
		}
		if ((num & 2) != 0)
		{
			SyncListUInt.ReadReference(reader, m_statusDurations);
		}
	}
}
