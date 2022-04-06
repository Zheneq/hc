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
	public const int c_num = 58;

	private static int kListm_statusCounts = -7231791;
	private static int kListm_statusDurations = 625641650;

	public static bool DebugTraceOn => true;

	static ActorStatus()
	{
		RegisterSyncListDelegate(typeof(ActorStatus), kListm_statusCounts, InvokeSyncListm_statusCounts);
		RegisterSyncListDelegate(typeof(ActorStatus), kListm_statusDurations, InvokeSyncListm_statusDurations);
		NetworkCRC.RegisterBehaviour("ActorStatus", 0);
	}

	private void Awake()
	{
		m_statusCountsPrevious = new int[c_num];
		m_clientStatusCountAdjustments = new int[c_num];
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
		if (NetworkClient.active && i >= 0 && i < c_num)
		{
			ActorData actorData = m_actorData;
			if (HUD_UI.Get() != null && actorData != null)
			{
				HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.NotifyStatusDurationChange(actorData, (StatusType)i, (int)m_statusDurations[i]);
				if (actorData == GameFlowData.Get().activeOwnedActorData)
				{
					HUD_UI.Get().m_mainScreenPanel.m_characterProfile.UpdateStatusDisplay(true);
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
		if (GameplayUtils.IsPlayerControlled(m_actorData))
		{
			int num = GameplayData.Get().m_recentlySpawnedDuration + 1;
			for (int j = 0; j < num; j++)
			{
				AddStatus(StatusType.RecentlySpawned, 1);
			}
		}
	}

	private void SyncListCallbackStatusCounts(SyncList<uint>.Operation op, int i) // ?
	{
		if (!NetworkServer.active && i >= 0 && i < c_num)
		{
			int statusCount = m_statusCountsPrevious[i] + m_clientStatusCountAdjustments[i];
			m_clientStatusCountAdjustments[i] = 0;
			m_statusCountsPrevious[i] = (int)m_statusCounts[i];
			if (m_statusCounts[i] != statusCount)
			{
				bool hadStatus = statusCount > 0;
				bool flag2 = HasStatus((StatusType)i);
				if (hadStatus != flag2)
				{
					OnStatusChanged((StatusType)i, flag2);
				}
			}
		}
	}

	public void UpdateStatusDuration(StatusType status, int newDuration)
	{
		if (newDuration > m_statusDurations[(int)status])
		{
			m_statusDurations[(int)status] = (uint)Mathf.Max(0, newDuration);
		}
	}

	[Server]
	public void AddStatus(StatusType status, int duration)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorStatus::AddStatus(StatusType,System.Int32)' called on client");
			return;
		}
		int prevCount = (int)m_statusCounts[(int)status];
		m_statusCounts[(int)status] = (uint)(prevCount + 1);
		if (duration > m_statusDurations[(int)status])
		{
			m_statusDurations[(int)status] = (uint)Mathf.Max(0, duration);
		}
		prevCount += m_clientStatusCountAdjustments[(int)status];
		m_clientStatusCountAdjustments[(int)status] = 0;
		if (DebugTraceOn)
		{
			Log.Warning("<color=cyan>ActorStatus</color>: ADD " + GetColoredStatusName(status, "yellow")
				+ " to " + m_actorData.DebugNameString("white") + ", Count = " + m_statusCounts[(int)status]
				+ ", PrevCount = " + prevCount);
		}
		if (prevCount == 0)
		{
			OnStatusChanged(status, true);
		}
	}

	[Server]
	public void RemoveStatus(StatusType status)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorStatus::RemoveStatus(StatusType)' called on client");
			return;
		}
		int prevCount = (int)m_statusCounts[(int)status];
		if (prevCount > 0)
		{
			m_statusCounts[(int)status] = (uint)Mathf.Max(0, prevCount - 1);
			prevCount += m_clientStatusCountAdjustments[(int)status];
			m_clientStatusCountAdjustments[(int)status] = 0;
			if (DebugTraceOn)
			{
				Log.Warning("<color=cyan>ActorStatus</color>: REMOVE " + GetColoredStatusName(status, "yellow")
					+ " from " + m_actorData.DebugNameString("white") + ", Count = " + m_statusCounts[(int)status]
					+ ", PrevCount: " + prevCount);
			}
			if (prevCount == 1)
			{
				OnStatusChanged(status, false);
			}
		}
		else
		{
			Log.Error($"Removing status '{status}' that was never added");
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
		int prevCount = m_clientStatusCountAdjustments[(int)status];
		m_clientStatusCountAdjustments[(int)status] = prevCount + 1;
		if (DebugTraceOn)
		{
			Log.Warning("<color=cyan>ActorStatus</color>: <color=cyan>CLIENT_ADD</color> "
				+ GetColoredStatusName(status, "yellow") + " to " + m_actorData.DebugNameString("white")
				+ ", ClientAdjust = " + m_clientStatusCountAdjustments[(int)status]
				+ ", SyncCount = " + m_statusCounts[(int)status]);
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
			Debug.LogWarning("[Client] function 'System.Void ActorStatus::ClientRemoveStatus(StatusType)' called on server");
			return;
		}
		int prevCount = m_clientStatusCountAdjustments[(int)status];
		m_clientStatusCountAdjustments[(int)status] = prevCount - 1;
		if (m_clientStatusCountAdjustments[(int)status] < 0)
		{
			m_clientStatusCountAdjustments[(int)status] = 0;
		}
		if (DebugTraceOn)
		{
			Log.Warning("<color=cyan>ActorStatus</color>: <color=magenta>CLIENT_REMOVE</color> "
				+ GetColoredStatusName(status, "yellow") + " from " + m_actorData.DebugNameString("white")
				+ ", ClientAdjust = " + m_clientStatusCountAdjustments[(int)status]
				+ ", SyncCount = " + m_statusCounts[(int)status]);
		}
		if (m_statusCounts[(int)status] + m_clientStatusCountAdjustments[(int)status] == 0)
		{
			OnStatusChanged(status, false);
		}
	}

	[Client]
	public void ClientClearAdjustments()
	{
		if (!NetworkClient.active)
		{
			Debug.LogWarning("[Client] function 'System.Void ActorStatus::ClientClearAdjustments()' called on server");
			return;
		}
		for (int i = 0; i < m_clientStatusCountAdjustments.Length; i++)
		{
			if (m_clientStatusCountAdjustments[i] != 0)
			{
				m_clientStatusCountAdjustments[i] = 0;
			}
		}
	}

	public bool HasStatus(StatusType status, bool includePending = true)
	{
		int count = ((int)status < m_statusCounts.Count) ? (int)m_statusCounts[(int)status] : 0;
		count += m_clientStatusCountAdjustments[(int)status];
		bool hasStatus = count > 0;
		if (!hasStatus && includePending && m_passivePendingStatusSources.Count > 0)
		{
			for (int i = 0; i < m_passivePendingStatusSources.Count; i++)
			{
				if (hasStatus)
				{
					break;
				}
				Ability ability = m_passivePendingStatusSources[i];
				hasStatus |= ability != null && ability.HasPassivePendingStatus(status, m_actorData);
			}
		}
		if (GameplayMutators.Get() != null)
		{
			int currentTurn = GameFlowData.Get().CurrentTurn;
			if (!hasStatus)
			{
				hasStatus |= GameplayMutators.IsStatusActive(status, currentTurn);
			}
			if (hasStatus)
			{
				hasStatus = !GameplayMutators.IsStatusSuppressed(status, currentTurn);
			}
		}
		return hasStatus;
	}

	public void AddAbilityForPassivePendingStatus(Ability ability)
	{
		if (ability != null && !m_passivePendingStatusSources.Contains(ability))
		{
			m_passivePendingStatusSources.Add(ability);
		}
	}

	public void RemoveAbilityForPassivePendingStatus(Ability ability)
	{
		m_passivePendingStatusSources.Remove(ability);
	}

	public bool IsKnockbackImmune(bool checkPending = true)
	{
		bool isImmune = HasStatus(StatusType.KnockbackImmune) || HasStatus(StatusType.Unstoppable);
		if (!isImmune && m_actorData.GetAbilityData() != null && checkPending)
		{
			isImmune = m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Unstoppable)
				|| m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.KnockbackImmune);
		}
		return isImmune;
	}

	public bool IsMovementDebuffImmune(bool checkPending = true)
	{
		bool isImmune = HasStatus(StatusType.MovementDebuffImmunity) || HasStatus(StatusType.Unstoppable);
		if (!isImmune && m_actorData.GetAbilityData() != null && checkPending)
		{
			isImmune = m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Unstoppable)
				|| m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.MovementDebuffImmunity);
		}
		return isImmune;
	}

	public bool IsEnergized(bool checkPending = true)
	{
		bool isEnergized = HasStatus(StatusType.Energized);
		if (checkPending && !isEnergized && m_actorData.GetAbilityData() != null)
		{
			isEnergized = m_actorData.GetAbilityData().HasPendingStatusFromQueuedAbilities(StatusType.Energized);
		}
		return isEnergized;
	}

	public void OnStatusChanged(StatusType status, bool statusGained)
	{
		if (DebugTraceOn)
		{
			Log.Warning($"<color=cyan>ActorStatus</color>: On Status Changed: <color=yellow>{status}</color>" +
				$" {(statusGained ? "<color=cyan>Gained" : " < color = magenta > Lost")}</color>");
		}
		if (!statusGained)
		{
			m_statusDurations[(int)status] = 0u;
		}
		ActorData actorData = m_actorData;
		if (HUD_UI.Get() != null && actorData != null)
		{
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.NotifyStatusChange(actorData, status, statusGained);
			if (actorData == GameFlowData.Get().activeOwnedActorData)
			{
				HUD_UI.Get().m_mainScreenPanel.m_characterProfile.UpdateStatusDisplay(true);
			}
		}
		actorData.ForceUpdateIsVisibleToClientCache();
		if (statusGained
			&& actorData.IsActorVisibleToClient()
			&& (status == StatusType.Snared || status == StatusType.Rooted))
		{
			AudioManager.PostEvent("ablty/generic/snare", actorData.gameObject);
		}
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
				FogOfWar.CalculateFogOfWarForTeam(actorData.GetEnemyTeam());
				break;
			case StatusType.IsolateVisionFromAllies:
			case StatusType.SeeThroughBrush:
				FogOfWar.CalculateFogOfWarForTeam(actorData.GetTeam());
				break;
			case StatusType.DecreasedIncomingHealing:
				if (statusGained)
				{
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
					GetComponent<ActorStats>().AddStatMod(StatType.IncomingHealing, ModType.Multiplier, 1.25f);
				}
				else
				{
					GetComponent<ActorStats>().RemoveStatMod(StatType.IncomingHealing, ModType.Multiplier, 1.25f);
				}
				break;
		}
		if (Board.Get() != null)
		{
			Board.Get().MarkForUpdateValidSquares();
		}
	}

	private void UpdateSquaresCanMoveTo()
	{
		if (m_actorData.GetActorMovement() != null)
		{
			m_actorData.GetActorMovement().UpdateSquaresCanMoveTo();
		}
	}

	private void UpdateMovementForMovementStatus(bool gained)
	{
		UpdateSquaresCanMoveTo();
		if (NetworkClient.active && m_actorData != null && gained)
		{
			LineData component = m_actorData.GetComponent<LineData>();
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
		return HasStatus(StatusType.Unstoppable);
	}

	public bool IsInvisibleToEnemies(bool includePendingStatus = true)
	{
		bool isInvisible = HasStatus(StatusType.InvisibleToEnemies, includePendingStatus);
		ActorData actorData = m_actorData;
		if (!isInvisible && HasStatus(StatusType.ProximityBasedInvisibility, includePendingStatus))
		{
			bool isVisibleInProximity = false;
			foreach (ActorData enemy in GameFlowData.Get().GetAllTeamMembers(actorData.GetEnemyTeam()))
			{
				BoardSquare mySquare = Board.Get().GetSquareFromVec3(actorData.GetFreePos());
				BoardSquare enemySquare = Board.Get().GetSquareFromVec3(enemy.GetFreePos());
				if (mySquare != null && enemySquare != null)
				{
					float dist = mySquare.HorizontalDistanceOnBoardTo(enemySquare);
					if (dist <= GameplayData.Get().m_proximityBasedInvisibilityMinDistance
						&& (GameplayData.Get().m_blindEnemyBreaksProximityBasedInvisibility
							|| enemy.GetActorStatus() != null && !enemy.GetActorStatus().HasStatus(StatusType.Blind, includePendingStatus)))
					{
						isVisibleInProximity = true;
						break;
					}
				}
			}
			if (!isVisibleInProximity)
			{
				isInvisible = true;
			}
		}
		return isInvisible
			&& !actorData.ServerSuppressInvisibility
			&& !HasStatus(StatusType.SuppressInvisibility, includePendingStatus);
	}

	public bool IsActionSilenced(AbilityData.ActionType action, bool checkPending = false)
	{
		if (HasStatus(StatusType.SilencedAllAbilities, checkPending))
		{
			return true;
		}

		bool isAbility = action >= AbilityData.ActionType.ABILITY_0
			&& action <= AbilityData.ActionType.ABILITY_6;
		bool isCard = action >= AbilityData.ActionType.CARD_0
			&& action <= AbilityData.ActionType.CARD_2;
		bool isBasicAbility = action == AbilityData.ActionType.ABILITY_0;

		bool isSilenced = false;
		if (isAbility && HasStatus(StatusType.SilencedPlayerAbilities, checkPending))
		{
			isSilenced |= isAbility;
		}
		if (isAbility && !isBasicAbility && HasStatus(StatusType.SilencedNonbasicPlayerAbilities, checkPending))
		{
			isSilenced |= isAbility && !isBasicAbility;
		}
		if (isBasicAbility && HasStatus(StatusType.SilencedBasicPlayerAbility, checkPending))
		{
			isSilenced |= isBasicAbility;
		}
		if (isCard && HasStatus(StatusType.SilencedCardAbilities, checkPending))
		{
			isSilenced |= isCard;
		}
		if (HasStatus(StatusType.SilencedEvasionAbilities, checkPending))
		{
			Ability abilityOfActionType = GetComponent<AbilityData>().GetAbilityOfActionType(action);
			bool isEvasionAbility;
			if (abilityOfActionType != null)
			{
				isEvasionAbility = abilityOfActionType.RunPriority == AbilityPriority.Evasion;
				Ability[] chainAbilities = abilityOfActionType.GetChainAbilities();
				foreach (Ability ability in chainAbilities)
				{
					isEvasionAbility |= ability.RunPriority == AbilityPriority.Evasion;
				}
			}
			else
			{
				isEvasionAbility = false;
			}
			isSilenced |= isEvasionAbility;
		}
		return isSilenced;
	}

	public static bool IsDispellableMovementDebuff(StatusType status)
	{
		return status == StatusType.Rooted
			|| status == StatusType.Snared
			|| status == StatusType.CrippledMovement
			|| status == StatusType.CantSprint_UnlessUnstoppable;
	}

	public static string GetColoredStatusName(StatusType status, string color)
	{
		return $"<color={color}>[{status}]</color>";
	}

	public int DebugGetStatusCount(StatusType status)
	{
		if (status >= StatusType.Revealed && (int)status < m_statusCounts.Count)
		{
			return (int)m_statusCounts[(int)status] + m_clientStatusCountAdjustments[(int)status];
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
		Log.Info($"[JSON] {{\"statusCounts\":{DefaultJsonSerializer.Serialize(((ActorStatus)obj).m_statusCounts)}}}");
	}

	protected static void InvokeSyncListm_statusDurations(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_statusDurations called on server.");
			return;
		}
		((ActorStatus)obj).m_statusDurations.HandleMsg(reader);
		Log.Info($"[JSON] {{\"statusDurations\":{DefaultJsonSerializer.Serialize(((ActorStatus)obj).m_statusDurations)}}}");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListUInt.WriteInstance(writer, m_statusCounts);
			SyncListUInt.WriteInstance(writer, m_statusDurations);
			return true;
		}
		bool flag = false;
		if ((syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_statusCounts);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_statusDurations);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListUInt.ReadReference(reader, m_statusCounts);
			SyncListUInt.ReadReference(reader, m_statusDurations);
			LogJson();
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
		LogJson(num);
	}

	private void LogJson(int mask = System.Int32.MaxValue)
	{
		var jsonLog = new List<string>();
		if ((mask & 1) != 0)
		{
			jsonLog.Add($"\"statusCounts\":{DefaultJsonSerializer.Serialize(m_statusCounts)}");
		}
		if ((mask & 2) != 0)
		{
			jsonLog.Add($"\"statusDurations\":{DefaultJsonSerializer.Serialize(m_statusDurations)}");
		}

		Log.Info($"[JSON] {{\"actorStatus\":{{{System.String.Join(",", jsonLog.ToArray())}}}}}");
	}
}
