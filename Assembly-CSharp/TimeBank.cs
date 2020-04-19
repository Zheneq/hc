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

	private static int kCmdCmdConsumableUsed = -0x72A53BD7;

	static TimeBank()
	{
		NetworkBehaviour.RegisterCommandDelegate(typeof(TimeBank), TimeBank.kCmdCmdConsumableUsed, new NetworkBehaviour.CmdDelegate(TimeBank.InvokeCmdCmdConsumableUsed));
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TimeBank.Awake()).MethodHandle;
					}
					int? initialTimeBankConsumables = gameConfig.InstanceSubType.GameOverrides.InitialTimeBankConsumables;
					if (initialTimeBankConsumables != null)
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
						int? initialTimeBankConsumables2 = gameConfig.InstanceSubType.GameOverrides.InitialTimeBankConsumables;
						networkm_consumablesRemaining = initialTimeBankConsumables2.Value;
					}
				}
			}
			else
			{
				Log.Error("Why does the GameManager's GameConfig not have a specific InstanceSubType? Where did it get it's GameConfig from? SubTypeBit is set to 0x{0:x4}", new object[]
				{
					gameConfig.InstanceSubTypeBit
				});
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		this.Networkm_reserveRemaining = GameWideData.Get().m_tbInitial;
		this.Networkm_consumablesRemaining = networkm_consumablesRemaining;
		this.ResetTurn();
	}

	[Command]
	private void CmdConsumableUsed()
	{
		this.m_clientConsumableUsed = true;
	}

	public void Update()
	{
		ActorTurnSM component = base.GetComponent<ActorTurnSM>();
		if (component.AmStillDeciding())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TimeBank.Update()).MethodHandle;
			}
			if (!(GameFlowData.Get() == null))
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
				if (GameFlowData.Get().GetTimeInState() >= 0.9f)
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
					if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
					{
						float num = this.TimeToDisplay();
						float num2;
						if (GameWideData.Get() != null)
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
							num2 = GameWideData.Get().m_tbGracePeriodBeforeConsuming;
						}
						else
						{
							num2 = 0f;
						}
						float num3 = num2;
						if (!this.m_clientConsumableUsed)
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
							if (num + num3 < 0f)
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
								if (!this.m_confirmed)
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
									this.m_clientConsumableUsed = true;
									if (base.isLocalPlayer)
									{
										this.CallCmdConsumableUsed();
									}
								}
							}
						}
						if (!this.AllowUnconfirm() && !this.m_clientEndTurnRequested && base.isLocalPlayer)
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
							if (!GameFlowData.Get().PreventAutoLockInOnTimeout())
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
								component.RequestEndTurn();
								this.m_clientEndTurnRequested = true;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TimeBank.TimeToDisplay()).MethodHandle;
			}
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
		bool result;
		if (this.m_reserveRemaining <= 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TimeBank.HasTimeSaved()).MethodHandle;
			}
			result = (this.m_consumablesRemaining > 0);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public float GetTimeSaved()
	{
		return this.m_reserveRemaining;
	}

	public int GetConsumablesRemaining()
	{
		return this.m_consumablesRemaining;
	}

	public bool GetConsumableUsed()
	{
		return this.m_clientConsumableUsed && !this.m_resolved;
	}

	public float GetPermittedOverflowTime()
	{
		float num = this.m_reserveRemaining;
		if (this.m_consumablesRemaining > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TimeBank.GetPermittedOverflowTime()).MethodHandle;
			}
			num += GameWideData.Get().m_tbConsumableDuration;
		}
		return num;
	}

	public bool AllowUnconfirm()
	{
		return -this.TimeToDisplay() < this.GetPermittedOverflowTime();
	}

	public void OnActionsConfirmed()
	{
		if (GameFlowData.Get())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TimeBank.OnActionsConfirmed()).MethodHandle;
			}
			this.m_reserveUsed = Mathf.Max(0f, -GameFlowData.Get().GetTimeRemainingInDecision());
		}
		this.m_confirmed = true;
	}

	public void OnActionsUnconfirmed()
	{
		this.m_reserveUsed = this.m_reserveRemaining + GameWideData.Get().m_tbConsumableDuration;
		this.m_confirmed = false;
		this.Update();
	}

	public void ResetTurn()
	{
		this.m_reserveUsed = this.m_reserveRemaining + GameWideData.Get().m_tbConsumableDuration;
		this.m_confirmed = false;
		this.m_clientConsumableUsed = false;
		this.Networkm_resolved = false;
		this.m_clientEndTurnRequested = false;
	}

	public void OnResolve(ActorData actorData)
	{
		if (NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TimeBank.OnResolve(ActorData)).MethodHandle;
			}
			if (!this.m_resolved)
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
				if (this.m_reserveUsed <= this.m_reserveRemaining)
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
					if (!this.m_clientConsumableUsed)
					{
						goto IL_99;
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
				if (this.m_consumablesRemaining > 0)
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
					if (this.m_clientConsumableUsed)
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
						this.Networkm_consumablesRemaining = Mathf.Max(this.m_consumablesRemaining - 1, 0);
					}
				}
				IL_99:
				this.Networkm_reserveRemaining = Mathf.Max(this.m_reserveRemaining - this.m_reserveUsed, 0f);
				float b = Mathf.Min(this.m_reserveRemaining + GameWideData.Get().m_tbRecharge, GameWideData.Get().m_tbRechargeCap);
				this.Networkm_reserveRemaining = Mathf.Max(this.m_reserveRemaining, b);
				this.Networkm_resolved = true;
			}
		}
	}

	private void UNetVersion()
	{
	}

	public float Networkm_reserveRemaining
	{
		get
		{
			return this.m_reserveRemaining;
		}
		[param: In]
		set
		{
			base.SetSyncVar<float>(value, ref this.m_reserveRemaining, 1U);
		}
	}

	public int Networkm_consumablesRemaining
	{
		get
		{
			return this.m_consumablesRemaining;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_consumablesRemaining, 2U);
		}
	}

	public bool Networkm_resolved
	{
		get
		{
			return this.m_resolved;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_resolved, 4U);
		}
	}

	protected static void InvokeCmdCmdConsumableUsed(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TimeBank.InvokeCmdCmdConsumableUsed(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("Command CmdConsumableUsed called on client.");
			return;
		}
		((TimeBank)obj).CmdConsumableUsed();
	}

	public void CallCmdConsumableUsed()
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TimeBank.CallCmdConsumableUsed()).MethodHandle;
			}
			Debug.LogError("Command function CmdConsumableUsed called on server.");
			return;
		}
		if (base.isServer)
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
			this.CmdConsumableUsed();
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)5));
		networkWriter.WritePackedUInt32((uint)TimeBank.kCmdCmdConsumableUsed);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		base.SendCommandInternal(networkWriter, 0, "CmdConsumableUsed");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write(this.m_reserveRemaining);
			writer.WritePackedUInt32((uint)this.m_consumablesRemaining);
			writer.Write(this.m_resolved);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TimeBank.OnSerialize(NetworkWriter, bool)).MethodHandle;
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
			writer.Write(this.m_reserveRemaining);
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_consumablesRemaining);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_resolved);
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
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			this.m_reserveRemaining = reader.ReadSingle();
			this.m_consumablesRemaining = (int)reader.ReadPackedUInt32();
			this.m_resolved = reader.ReadBoolean();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TimeBank.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_reserveRemaining = reader.ReadSingle();
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
			this.m_consumablesRemaining = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
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
			this.m_resolved = reader.ReadBoolean();
		}
	}
}
