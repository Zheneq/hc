using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class TrackerDroneTrackerComponent : NetworkBehaviour
{
	[SyncVar]
	private bool m_droneActive;

	[SyncVar]
	private int m_boardX;

	[SyncVar]
	private int m_boardY;

	private SyncListInt m_trackedActorIndex = new SyncListInt();

	private static int kListm_trackedActorIndex;

	public bool Networkm_droneActive
	{
		get
		{
			return m_droneActive;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_droneActive, 1u);
		}
	}

	public int Networkm_boardX
	{
		get
		{
			return m_boardX;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_boardX, 2u);
		}
	}

	public int Networkm_boardY
	{
		get
		{
			return m_boardY;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_boardY, 4u);
		}
	}

	static TrackerDroneTrackerComponent()
	{
		kListm_trackedActorIndex = 1611176142;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(TrackerDroneTrackerComponent), kListm_trackedActorIndex, InvokeSyncListm_trackedActorIndex);
		NetworkCRC.RegisterBehaviour("TrackerDroneTrackerComponent", 0);
	}

	internal bool DroneIsActive()
	{
		BoardSquare boardSquare = Board.Get().GetSquareFromIndex(m_boardX, m_boardY);
		int result;
		if (m_droneActive)
		{
			result = ((boardSquare != null) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	internal int BoardX()
	{
		return m_boardX;
	}

	internal int BoardY()
	{
		return m_boardY;
	}

	internal bool IsTrackingActor(int index)
	{
		return m_trackedActorIndex.Contains(index);
	}

	[Server]
	internal void UpdateDroneActiveFlag(bool isActive)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TrackerDroneTrackerComponent::UpdateDroneActiveFlag(System.Boolean)' called on client");
		}
		else
		{
			Networkm_droneActive = isActive;
		}
	}

	[Server]
	internal void UpdateDroneBoardPos(int x, int y)
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
					Debug.LogWarning("[Server] function 'System.Void TrackerDroneTrackerComponent::UpdateDroneBoardPos(System.Int32,System.Int32)' called on client");
					return;
				}
			}
		}
		Networkm_boardX = x;
		Networkm_boardY = y;
	}

	[Server]
	internal void AddTrackedActorByIndex(int actorIndex)
	{
		if (!NetworkServer.active)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogWarning("[Server] function 'System.Void TrackerDroneTrackerComponent::AddTrackedActorByIndex(System.Int32)' called on client");
					return;
				}
			}
		}
		if (m_trackedActorIndex.Contains(actorIndex))
		{
			return;
		}
		while (true)
		{
			m_trackedActorIndex.Add(actorIndex);
			return;
		}
	}

	[Server]
	internal void RemoveTrackedActorByIndex(int actorIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TrackerDroneTrackerComponent::RemoveTrackedActorByIndex(System.Int32)' called on client");
		}
		else
		{
			m_trackedActorIndex.Remove(actorIndex);
		}
	}

	private void Update()
	{
		if (!GameFlowData.Get().IsInDecisionState())
		{
			return;
		}
		while (true)
		{
			SanityCheckTrackerDroneState();
			return;
		}
	}

	public void SanityCheckTrackerDroneState()
	{
		SatelliteController component = GetComponent<SatelliteController>();
		if (component == null)
		{
			return;
		}
		PersistentSatellite satellite = component.GetSatellite(0);
		if (satellite == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_droneActive)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					if (!satellite.IsVisible())
					{
						satellite.OverrideVisibility(true);
					}
					BoardSquare boardSquare = Board.Get().GetSquareFromTransform(satellite.transform);
					if (!(boardSquare == null))
					{
						if (boardSquare.x == BoardX())
						{
							if (boardSquare.y == BoardY())
							{
								return;
							}
						}
					}
					float x = (float)BoardX() * Board.Get().squareSize;
					float z = (float)BoardY() * Board.Get().squareSize;
					Vector3 position = satellite.transform.position;
					Vector3 targetPosition = new Vector3(x, position.y, z);
					satellite.TeleportToLocation(targetPosition);
					return;
				}
				}
			}
		}
		if (satellite.IsVisible())
		{
			satellite.OverrideVisibility(false);
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_trackedActorIndex(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_trackedActorIndex called on server.");
		}
		else
		{
			((TrackerDroneTrackerComponent)obj).m_trackedActorIndex.HandleMsg(reader);
		}
	}

	private void Awake()
	{
		m_trackedActorIndex.InitializeBehaviour(this, kListm_trackedActorIndex);
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
					writer.Write(m_droneActive);
					writer.WritePackedUInt32((uint)m_boardX);
					writer.WritePackedUInt32((uint)m_boardY);
					SyncListInt.WriteInstance(writer, m_trackedActorIndex);
					return true;
				}
			}
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_droneActive);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_boardX);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_boardY);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, m_trackedActorIndex);
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
			m_droneActive = reader.ReadBoolean();
			m_boardX = (int)reader.ReadPackedUInt32();
			m_boardY = (int)reader.ReadPackedUInt32();
			SyncListInt.ReadReference(reader, m_trackedActorIndex);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_droneActive = reader.ReadBoolean();
		}
		if ((num & 2) != 0)
		{
			m_boardX = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			m_boardY = (int)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			SyncListInt.ReadReference(reader, m_trackedActorIndex);
		}
	}
}
