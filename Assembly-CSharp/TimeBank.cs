// ROGUES
// SERVER
using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class TimeBank : NetworkBehaviour
{
	[SyncVar]
	private float m_reserveRemaining;
	[SyncVar]
	private int m_consumablesRemaining;
	private float m_reserveUsed;
	private bool m_confirmed;
	private bool m_clientConsumableUsed;
	[SyncVar]
	private bool m_resolved;
	private bool m_clientEndTurnRequested;

	// removed in rogues
	private static int kCmdCmdConsumableUsed = -1923431383;

	public float Networkm_reserveRemaining
	{
		get => m_reserveRemaining;
		[param: In]
		set => SetSyncVar(value, ref m_reserveRemaining, 1u);
	}

	public int Networkm_consumablesRemaining
	{
		get => m_consumablesRemaining;
		[param: In]
		set => SetSyncVar(value, ref m_consumablesRemaining, 2u);
	}

	public bool Networkm_resolved
	{
		get => m_resolved;
		[param: In]
		set => SetSyncVar(value, ref m_resolved, 4u);
	}

	static TimeBank()
	{
		// reactor
		RegisterCommandDelegate(typeof(TimeBank), kCmdCmdConsumableUsed, InvokeCmdCmdConsumableUsed);
		NetworkCRC.RegisterBehaviour("TimeBank", 0);
		// rogues
		// NetworkBehaviour.RegisterCommandDelegate(typeof(TimeBank), "CmdConsumableUsed", new CmdDelegate(InvokeCmdCmdConsumableUsed));
	}

	private void Awake()
	{
		int networkm_consumablesRemaining = GameWideData.Get().m_tbConsumables;
		
		// removed in rogues
		try
		{
			LobbyGameConfig gameConfig = GameManager.Get().GameConfig;
			if (gameConfig.HasSelectedSubType)
			{
				int? initialTimeBankConsumables = gameConfig.InstanceSubType.GameOverrides?.InitialTimeBankConsumables;
				if (initialTimeBankConsumables.HasValue)
				{
					networkm_consumablesRemaining = initialTimeBankConsumables.Value;
				}
			}
			else
			{
				Log.Error("Why does the GameManager's GameConfig not have a specific InstanceSubType? Where did it get it's GameConfig from? SubTypeBit is set to 0x{0:x4}", gameConfig.InstanceSubTypeBit);
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		// end removed in rogues
		
		Networkm_reserveRemaining = GameWideData.Get().m_tbInitial;
		Networkm_consumablesRemaining = networkm_consumablesRemaining;
		ResetTurn();
	}

	[Command]
	private void CmdConsumableUsed()
	{
		m_clientConsumableUsed = true;
	}

	public void Update()
	{
		ActorTurnSM component = GetComponent<ActorTurnSM>();
		if (!component.AmStillDeciding()
		    || GameFlowData.Get() == null
		    || GameFlowData.Get().GetTimeInState() < 0.9f
		    || GameFlowData.Get().gameState != GameState.BothTeams_Decision)
		{
			return;
		}
		float gracePeriod = GameWideData.Get() != null ? GameWideData.Get().m_tbGracePeriodBeforeConsuming : 0f;
		if (!m_clientConsumableUsed && TimeToDisplay() + gracePeriod < 0f && !m_confirmed)
		{
			m_clientConsumableUsed = true;
			if (isLocalPlayer)
			{
				CallCmdConsumableUsed();
			}
		}
		if (!AllowUnconfirm()
		    && !m_clientEndTurnRequested
		    && isLocalPlayer
		    && !GameFlowData.Get().PreventAutoLockInOnTimeout())
		{
			component.RequestEndTurn();  // RequestEndTurn(true) in rogues
			m_clientEndTurnRequested = true;
		}
	}

	public float TimeToDisplay()
	{
		return GameFlowData.Get() != null ? GameFlowData.Get().GetTimeRemainingInDecision() : 0f;
	}

	public bool HasTimeSaved()
	{
		return m_reserveRemaining > 0f || m_consumablesRemaining > 0;
	}

	public float GetTimeSaved()
	{
		return m_reserveRemaining;
	}

	public int GetConsumablesRemaining()
	{
		return m_consumablesRemaining;
	}

	public bool GetConsumableUsed()
	{
		return m_clientConsumableUsed && !m_resolved;
	}

	public float GetPermittedOverflowTime()
	{
		float num = m_reserveRemaining;
		if (m_consumablesRemaining > 0)
		{
			num += GameWideData.Get().m_tbConsumableDuration;
		}
		return num;
	}

	public bool AllowUnconfirm()
	{
		return 0f - TimeToDisplay() < GetPermittedOverflowTime();
	}

	public void OnActionsConfirmed()
	{
		if ((bool)GameFlowData.Get())
		{
			m_reserveUsed = Mathf.Max(0f, 0f - GameFlowData.Get().GetTimeRemainingInDecision());
		}
		m_confirmed = true;
	}

	public void OnActionsUnconfirmed()
	{
		m_reserveUsed = m_reserveRemaining + GameWideData.Get().m_tbConsumableDuration;
		m_confirmed = false;
		Update();
	}

	public void ResetTurn()
	{
		m_reserveUsed = m_reserveRemaining + GameWideData.Get().m_tbConsumableDuration;
		m_confirmed = false;
		m_clientConsumableUsed = false;
		Networkm_resolved = false;
		m_clientEndTurnRequested = false;
	}

	public void OnResolve(ActorData actorData)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		if (m_resolved)
		{
			return;
		}
		Log.Debug($"Timebank {actorData.m_displayName} before resolve: " +
		         $"m_reserveUsed={m_reserveUsed} " +
		         $"m_reserveRemaining={m_reserveRemaining} " +
		         $"m_clientConsumableUsed={m_clientConsumableUsed} " +
		         $"m_consumablesRemaining={m_consumablesRemaining}");
		if ((m_reserveUsed > m_reserveRemaining || m_clientConsumableUsed)
		    && m_consumablesRemaining > 0)
		{
			Log.Debug($"Timebank {actorData.m_displayName} used");
			if (m_clientConsumableUsed)
			{
				Networkm_consumablesRemaining = Mathf.Max(m_consumablesRemaining - 1, 0);
				Log.Debug($"Timebank {actorData.m_displayName} decreased");
			}
#if SERVER
			// added in rogues
			GameplayMetricHelper.RecordTimebankUsed(actorData);
#endif
		}
		Networkm_reserveRemaining = Mathf.Max(m_reserveRemaining - m_reserveUsed, 0f);
		float recharge = Mathf.Min(m_reserveRemaining + GameWideData.Get().m_tbRecharge, GameWideData.Get().m_tbRechargeCap);
		Networkm_reserveRemaining = Mathf.Max(m_reserveRemaining, recharge);
		Networkm_resolved = true;
		Log.Debug($"Timebank {actorData.m_displayName} after resolve: " +
		          $"m_reserveUsed={m_reserveUsed} " +
		          $"m_reserveRemaining={m_reserveRemaining} " +
		          $"m_clientConsumableUsed={m_clientConsumableUsed} " +
		          $"m_consumablesRemaining={m_consumablesRemaining}");
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeCmdCmdConsumableUsed(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("Command CmdConsumableUsed called on client.");
			return;
		}
		((TimeBank)obj).CmdConsumableUsed();
	}

	public void CallCmdConsumableUsed()
	{
		// removed in rogues
		if (!NetworkClient.active)
		{
			Debug.LogError("Command function CmdConsumableUsed called on server.");
			return;
		}
		// end removed in rogues
		
		if (isServer)
		{
			CmdConsumableUsed();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		// reactor
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdConsumableUsed);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, "CmdConsumableUsed");
		// rogues
		// base.SendCommandInternal(typeof(TimeBank), "CmdConsumableUsed", networkWriter, 0);
	}

	// different in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write(m_reserveRemaining);
			writer.WritePackedUInt32((uint)m_consumablesRemaining);
			writer.Write(m_resolved);
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
			writer.Write(m_reserveRemaining);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_consumablesRemaining);
		}
		if ((syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_resolved);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	// different in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			m_reserveRemaining = reader.ReadSingle();
			m_consumablesRemaining = (int)reader.ReadPackedUInt32();
			m_resolved = reader.ReadBoolean();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_reserveRemaining = reader.ReadSingle();
		}
		if ((num & 2) != 0)
		{
			m_consumablesRemaining = (int)reader.ReadPackedUInt32();
		}
        if ((num & 4) != 0)
        {
            m_resolved = reader.ReadBoolean();
        }
    }
}
