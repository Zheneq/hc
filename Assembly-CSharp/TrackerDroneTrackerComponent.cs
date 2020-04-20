using System;
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

	private static int kListm_trackedActorIndex = 0x600898CE;

	static TrackerDroneTrackerComponent()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(TrackerDroneTrackerComponent), TrackerDroneTrackerComponent.kListm_trackedActorIndex, new NetworkBehaviour.CmdDelegate(TrackerDroneTrackerComponent.InvokeSyncListm_trackedActorIndex));
		NetworkCRC.RegisterBehaviour("TrackerDroneTrackerComponent", 0);
	}

	internal bool DroneIsActive()
	{
		BoardSquare boardSquare = Board.Get().GetBoardSquare(this.m_boardX, this.m_boardY);
		bool result;
		if (this.m_droneActive)
		{
			result = (boardSquare != null);
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal int BoardX()
	{
		return this.m_boardX;
	}

	internal int BoardY()
	{
		return this.m_boardY;
	}

	internal bool IsTrackingActor(int index)
	{
		return this.m_trackedActorIndex.Contains(index);
	}

	[Server]
	internal void UpdateDroneActiveFlag(bool isActive)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TrackerDroneTrackerComponent::UpdateDroneActiveFlag(System.Boolean)' called on client");
			return;
		}
		this.Networkm_droneActive = isActive;
	}

	[Server]
	internal void UpdateDroneBoardPos(int x, int y)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TrackerDroneTrackerComponent::UpdateDroneBoardPos(System.Int32,System.Int32)' called on client");
			return;
		}
		this.Networkm_boardX = x;
		this.Networkm_boardY = y;
	}

	[Server]
	internal void AddTrackedActorByIndex(int actorIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TrackerDroneTrackerComponent::AddTrackedActorByIndex(System.Int32)' called on client");
			return;
		}
		if (!this.m_trackedActorIndex.Contains(actorIndex))
		{
			this.m_trackedActorIndex.Add(actorIndex);
		}
	}

	[Server]
	internal void RemoveTrackedActorByIndex(int actorIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void TrackerDroneTrackerComponent::RemoveTrackedActorByIndex(System.Int32)' called on client");
			return;
		}
		this.m_trackedActorIndex.Remove(actorIndex);
	}

	private void Update()
	{
		if (GameFlowData.Get().IsInDecisionState())
		{
			this.SanityCheckTrackerDroneState();
		}
	}

	public void SanityCheckTrackerDroneState()
	{
		SatelliteController component = base.GetComponent<SatelliteController>();
		if (component == null)
		{
			return;
		}
		PersistentSatellite satellite = component.GetSatellite(0);
		if (satellite == null)
		{
			return;
		}
		if (this.m_droneActive)
		{
			if (!satellite.IsVisible())
			{
				satellite.OverrideVisibility(true);
			}
			BoardSquare boardSquare = Board.Get().GetBoardSquare(satellite.transform);
			if (!(boardSquare == null))
			{
				if (boardSquare.x == this.BoardX())
				{
					if (boardSquare.y == this.BoardY())
					{
						goto IL_127;
					}
				}
			}
			float x = (float)this.BoardX() * Board.Get().squareSize;
			float z = (float)this.BoardY() * Board.Get().squareSize;
			Vector3 targetPosition = new Vector3(x, satellite.transform.position.y, z);
			satellite.TeleportToLocation(targetPosition);
			IL_127:;
		}
		else if (satellite.IsVisible())
		{
			satellite.OverrideVisibility(false);
		}
	}

	private void UNetVersion()
	{
	}

	public bool Networkm_droneActive
	{
		get
		{
			return this.m_droneActive;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_droneActive, 1U);
		}
	}

	public int Networkm_boardX
	{
		get
		{
			return this.m_boardX;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_boardX, 2U);
		}
	}

	public int Networkm_boardY
	{
		get
		{
			return this.m_boardY;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_boardY, 4U);
		}
	}

	protected static void InvokeSyncListm_trackedActorIndex(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_trackedActorIndex called on server.");
			return;
		}
		((TrackerDroneTrackerComponent)obj).m_trackedActorIndex.HandleMsg(reader);
	}

	private void Awake()
	{
		this.m_trackedActorIndex.InitializeBehaviour(this, TrackerDroneTrackerComponent.kListm_trackedActorIndex);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.Write(this.m_droneActive);
			writer.WritePackedUInt32((uint)this.m_boardX);
			writer.WritePackedUInt32((uint)this.m_boardY);
			SyncListInt.WriteInstance(writer, this.m_trackedActorIndex);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_droneActive);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_boardX);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_boardY);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, this.m_trackedActorIndex);
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
			this.m_droneActive = reader.ReadBoolean();
			this.m_boardX = (int)reader.ReadPackedUInt32();
			this.m_boardY = (int)reader.ReadPackedUInt32();
			SyncListInt.ReadReference(reader, this.m_trackedActorIndex);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_droneActive = reader.ReadBoolean();
		}
		if ((num & 2) != 0)
		{
			this.m_boardX = (int)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			this.m_boardY = (int)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			SyncListInt.ReadReference(reader, this.m_trackedActorIndex);
		}
	}
}
