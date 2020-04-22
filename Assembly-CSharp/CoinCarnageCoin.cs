using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class CoinCarnageCoin : NetworkBehaviour
{
	private BoardSquare m_boardSquare;

	[SyncVar(hook = "HookSetPickedUp")]
	private bool m_pickedUp;

	public bool Networkm_pickedUp
	{
		get
		{
			return m_pickedUp;
		}
		[param: In]
		set
		{
			ref bool pickedUp = ref m_pickedUp;
			if (NetworkServer.localClientActive && !base.syncVarHookGuard)
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
				base.syncVarHookGuard = true;
				HookSetPickedUp(value);
				base.syncVarHookGuard = false;
			}
			SetSyncVar(value, ref pickedUp, 1u);
		}
	}

	public void Initialize(BoardSquare square)
	{
		m_boardSquare = square;
	}

	public BoardSquare GetSquare()
	{
		return m_boardSquare;
	}

	public bool IsPickedUp()
	{
		return m_pickedUp;
	}

	[Server]
	public void PickUp(ActorData actor)
	{
		if (!NetworkServer.active)
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
					Debug.LogWarning("[Server] function 'System.Void CoinCarnageCoin::PickUp(ActorData)' called on client");
					return;
				}
			}
		}
		if (m_pickedUp)
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
			Networkm_pickedUp = true;
			return;
		}
	}

	private void HookSetPickedUp(bool value)
	{
		Networkm_pickedUp = value;
		if (!value)
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
			base.gameObject.SetActive(false);
			return;
		}
	}

	[Server]
	public void Destroy()
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
					Debug.LogWarning("[Server] function 'System.Void CoinCarnageCoin::Destroy()' called on client");
					return;
				}
			}
		}
		NetworkServer.Destroy(base.gameObject);
	}

	private void UNetVersion()
	{
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					writer.Write(m_pickedUp);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_pickedUp);
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
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
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
					m_pickedUp = reader.ReadBoolean();
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			HookSetPickedUp(reader.ReadBoolean());
		}
	}
}
