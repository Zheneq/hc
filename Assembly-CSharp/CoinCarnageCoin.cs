using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class CoinCarnageCoin : NetworkBehaviour
{
	private BoardSquare m_boardSquare;

	[SyncVar(hook = "HookSetPickedUp")]
	private bool m_pickedUp;

	public void Initialize(BoardSquare square)
	{
		this.m_boardSquare = square;
	}

	public BoardSquare GetSquare()
	{
		return this.m_boardSquare;
	}

	public bool IsPickedUp()
	{
		return this.m_pickedUp;
	}

	[Server]
	public void PickUp(ActorData actor)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageCoin.PickUp(ActorData)).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void CoinCarnageCoin::PickUp(ActorData)' called on client");
			return;
		}
		if (!this.m_pickedUp)
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
			this.Networkm_pickedUp = true;
		}
	}

	private void HookSetPickedUp(bool value)
	{
		this.Networkm_pickedUp = value;
		if (value)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageCoin.HookSetPickedUp(bool)).MethodHandle;
			}
			base.gameObject.SetActive(false);
		}
	}

	[Server]
	public void Destroy()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageCoin.Destroy()).MethodHandle;
			}
			Debug.LogWarning("[Server] function 'System.Void CoinCarnageCoin::Destroy()' called on client");
			return;
		}
		NetworkServer.Destroy(base.gameObject);
	}

	private void UNetVersion()
	{
	}

	public bool Networkm_pickedUp
	{
		get
		{
			return this.m_pickedUp;
		}
		[param: In]
		set
		{
			uint dirtyBit = 1U;
			if (NetworkServer.localClientActive && !base.syncVarHookGuard)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageCoin.set_Networkm_pickedUp(bool)).MethodHandle;
				}
				base.syncVarHookGuard = true;
				this.HookSetPickedUp(value);
				base.syncVarHookGuard = false;
			}
			base.SetSyncVar<bool>(value, ref this.m_pickedUp, dirtyBit);
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageCoin.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			writer.Write(this.m_pickedUp);
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_pickedUp);
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
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoinCarnageCoin.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_pickedUp = reader.ReadBoolean();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.HookSetPickedUp(reader.ReadBoolean());
		}
	}
}
