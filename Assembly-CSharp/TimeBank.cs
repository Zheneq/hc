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

	private static int kCmdCmdConsumableUsed;

	public float Networkm_reserveRemaining
	{
		get
		{
			return m_reserveRemaining;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_reserveRemaining, 1u);
		}
	}

	public int Networkm_consumablesRemaining
	{
		get
		{
			return m_consumablesRemaining;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_consumablesRemaining, 2u);
		}
	}

	public bool Networkm_resolved
	{
		get
		{
			return m_resolved;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_resolved, 4u);
		}
	}

	static TimeBank()
	{
		kCmdCmdConsumableUsed = -1923431383;
		NetworkBehaviour.RegisterCommandDelegate(typeof(TimeBank), kCmdCmdConsumableUsed, InvokeCmdCmdConsumableUsed);
		NetworkCRC.RegisterBehaviour("TimeBank", 0);
	}

	private void Awake()
	{
		int networkm_consumablesRemaining = GameWideData.Get().m_tbConsumables;
		try
		{
			LobbyGameConfig gameConfig = GameManager.Get().GameConfig;
			if (gameConfig.HasSelectedSubType)
			{
				if (gameConfig.InstanceSubType.GameOverrides != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
						{
							int? initialTimeBankConsumables = gameConfig.InstanceSubType.GameOverrides.InitialTimeBankConsumables;
							if (initialTimeBankConsumables.HasValue)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
									{
										int? initialTimeBankConsumables2 = gameConfig.InstanceSubType.GameOverrides.InitialTimeBankConsumables;
										networkm_consumablesRemaining = initialTimeBankConsumables2.Value;
										goto end_IL_000d;
									}
									}
								}
							}
							goto end_IL_000d;
						}
						}
					}
				}
			}
			else
			{
				Log.Error("Why does the GameManager's GameConfig not have a specific InstanceSubType? Where did it get it's GameConfig from? SubTypeBit is set to 0x{0:x4}", gameConfig.InstanceSubTypeBit);
			}
			end_IL_000d:;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
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
		if (!component.AmStillDeciding())
		{
			return;
		}
		while (true)
		{
			if (GameFlowData.Get() == null)
			{
				return;
			}
			while (true)
			{
				if (GameFlowData.Get().GetTimeInState() < 0.9f)
				{
					return;
				}
				while (true)
				{
					if (GameFlowData.Get().gameState != GameState.BothTeams_Decision)
					{
						return;
					}
					float num = TimeToDisplay();
					float num2;
					if (GameWideData.Get() != null)
					{
						num2 = GameWideData.Get().m_tbGracePeriodBeforeConsuming;
					}
					else
					{
						num2 = 0f;
					}
					float num3 = num2;
					if (!m_clientConsumableUsed)
					{
						if (num + num3 < 0f)
						{
							if (!m_confirmed)
							{
								m_clientConsumableUsed = true;
								if (base.isLocalPlayer)
								{
									CallCmdConsumableUsed();
								}
							}
						}
					}
					if (AllowUnconfirm() || m_clientEndTurnRequested || !base.isLocalPlayer)
					{
						return;
					}
					while (true)
					{
						if (!GameFlowData.Get().PreventAutoLockInOnTimeout())
						{
							while (true)
							{
								component.RequestEndTurn();
								m_clientEndTurnRequested = true;
								return;
							}
						}
						return;
					}
				}
			}
		}
	}

	public float TimeToDisplay()
	{
		float result;
		if (GameFlowData.Get() != null)
		{
			result = GameFlowData.Get().GetTimeRemainingInDecision();
		}
		else
		{
			result = 0f;
		}
		return result;
	}

	public bool HasTimeSaved()
	{
		int result;
		if (!(m_reserveRemaining > 0f))
		{
			result = ((m_consumablesRemaining > 0) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
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
		while (true)
		{
			if (m_resolved)
			{
				return;
			}
			while (true)
			{
				if (!(m_reserveUsed > m_reserveRemaining))
				{
					if (!m_clientConsumableUsed)
					{
						goto IL_0099;
					}
				}
				if (m_consumablesRemaining > 0)
				{
					if (m_clientConsumableUsed)
					{
						Networkm_consumablesRemaining = Mathf.Max(m_consumablesRemaining - 1, 0);
					}
				}
				goto IL_0099;
				IL_0099:
				Networkm_reserveRemaining = Mathf.Max(m_reserveRemaining - m_reserveUsed, 0f);
				float b = Mathf.Min(m_reserveRemaining + GameWideData.Get().m_tbRecharge, GameWideData.Get().m_tbRechargeCap);
				Networkm_reserveRemaining = Mathf.Max(m_reserveRemaining, b);
				Networkm_resolved = true;
				return;
			}
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeCmdCmdConsumableUsed(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command CmdConsumableUsed called on client.");
					return;
				}
			}
		}
		((TimeBank)obj).CmdConsumableUsed();
	}

	public void CallCmdConsumableUsed()
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Debug.LogError("Command function CmdConsumableUsed called on server.");
					return;
				}
			}
		}
		if (base.isServer)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					CmdConsumableUsed();
					return;
				}
			}
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)5);
		networkWriter.WritePackedUInt32((uint)kCmdCmdConsumableUsed);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendCommandInternal(networkWriter, 0, "CmdConsumableUsed");
	}

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
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_reserveRemaining);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_consumablesRemaining);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_resolved);
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
		if ((num & 4) == 0)
		{
			return;
		}
		while (true)
		{
			m_resolved = reader.ReadBoolean();
			return;
		}
	}
}
