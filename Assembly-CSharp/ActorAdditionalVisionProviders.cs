using System;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class ActorAdditionalVisionProviders : NetworkBehaviour
{
	private SyncListVisionProviderInfo m_visionProviders = new SyncListVisionProviderInfo();

	private ActorData m_actorData;

	private static int kListm_visionProviders = -0x20A7FD18;

	static ActorAdditionalVisionProviders()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(ActorAdditionalVisionProviders), ActorAdditionalVisionProviders.kListm_visionProviders, new NetworkBehaviour.CmdDelegate(ActorAdditionalVisionProviders.InvokeSyncListm_visionProviders));
		NetworkCRC.RegisterBehaviour("ActorAdditionalVisionProviders", 0);
	}

	private void Start()
	{
		this.m_actorData = base.GetComponent<ActorData>();
	}

	public override void OnStartClient()
	{
		this.m_visionProviders.Callback = new SyncList<VisionProviderInfo>.SyncListChanged(this.SyncListCallbackVisionProviders);
	}

	public SyncListVisionProviderInfo GetVisionProviders()
	{
		return this.m_visionProviders;
	}

	private void SyncListCallbackVisionProviders(SyncList<VisionProviderInfo>.Operation op, int _incorrectIndexBugIn51And52)
	{
		if (this.m_actorData != null)
		{
			this.m_actorData.GetFogOfWar().MarkForRecalculateVisibility();
		}
	}

	[Server]
	public void AddVisionProviderOnGridPos(GridPos gridPos, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::AddVisionProviderOnGridPos(GridPos,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
			return;
		}
		this.m_visionProviders.Add(new VisionProviderInfo(gridPos, radius, useSraightLineDist, brushRevealType, ignoreLos, canFunctionInGlobalBlind, flag));
		this.m_actorData.GetFogOfWar().MarkForRecalculateVisibility();
	}

	[Server]
	public void RemoveVisionProviderOnGridPos(GridPos gridPos, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnGridPos(GridPos,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
			return;
		}
		int num = -1;
		for (int i = 0; i < (int)this.m_visionProviders.Count; i++)
		{
			if (this.m_visionProviders[i].IsEqual(gridPos, radius, useSraightLineDist, brushRevealType, ignoreLos, flag, canFunctionInGlobalBlind))
			{
				num = i;
				IL_72:
				if (num >= 0)
				{
					this.m_visionProviders.RemoveAt(num);
				}
				return;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			goto IL_72;
		}
	}

	[Server]
	public void AddVisionProviderOnActor(int actorIndex, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::AddVisionProviderOnActor(System.Int32,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
			return;
		}
		this.m_visionProviders.Add(new VisionProviderInfo(actorIndex, radius, useSraightLineDist, brushRevealType, ignoreLos, canFunctionInGlobalBlind, flag));
		this.m_actorData.GetFogOfWar().MarkForRecalculateVisibility();
	}

	[Server]
	public void RemoveVisionProviderOnActor(int actorIndex, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnActor(System.Int32,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
			return;
		}
		int num = -1;
		for (int i = 0; i < (int)this.m_visionProviders.Count; i++)
		{
			if (this.m_visionProviders[i].IsEqual(actorIndex, radius, useSraightLineDist, brushRevealType, ignoreLos, flag, canFunctionInGlobalBlind))
			{
				num = i;
				IL_78:
				if (num >= 0)
				{
					this.m_visionProviders.RemoveAt(num);
				}
				return;
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			goto IL_78;
		}
	}

	[Server]
	public void RemoveVisionProviderOnActor(int actorIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnActor(System.Int32)' called on client");
			return;
		}
		for (int i = (int)(this.m_visionProviders.Count - 1); i >= 0; i--)
		{
			if (this.m_visionProviders[i].m_actorIndex == actorIndex)
			{
				if (this.m_visionProviders[i].m_satelliteIndex == -1)
				{
					this.m_visionProviders.RemoveAt(i);
				}
			}
		}
	}

	[Server]
	public void AddVisionProviderOnSatellite(int actorIndex, int satelliteIndex, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::AddVisionProviderOnSatellite(System.Int32,System.Int32,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
			return;
		}
		this.m_visionProviders.Add(new VisionProviderInfo(actorIndex, satelliteIndex, radius, useSraightLineDist, brushRevealType, ignoreLos, canFunctionInGlobalBlind, flag));
		this.m_actorData.GetFogOfWar().MarkForRecalculateVisibility();
	}

	[Server]
	public void RemoveVisionProviderOnSatellite(int actorIndex, int satelliteIndex, float radius, bool useSraightLineDist, VisionProviderInfo.BrushRevealType brushRevealType, bool ignoreLos, bool canFunctionInGlobalBlind, BoardSquare.VisibilityFlags flag)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnSatellite(System.Int32,System.Int32,System.Single,System.Boolean,VisionProviderInfo/BrushRevealType,System.Boolean,System.Boolean,BoardSquare/VisibilityFlags)' called on client");
			return;
		}
		int num = -1;
		for (int i = 0; i < (int)this.m_visionProviders.Count; i++)
		{
			if (this.m_visionProviders[i].IsEqual(actorIndex, satelliteIndex, radius, useSraightLineDist, brushRevealType, ignoreLos, flag, canFunctionInGlobalBlind))
			{
				num = i;
				break;
			}
		}
		if (num >= 0)
		{
			this.m_visionProviders.RemoveAt(num);
		}
	}

	[Server]
	public void RemoveVisionProviderOnSatellite(int actorIndex, int satelliteIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::RemoveVisionProviderOnSatellite(System.Int32,System.Int32)' called on client");
			return;
		}
		for (int i = (int)(this.m_visionProviders.Count - 1); i >= 0; i--)
		{
			if (this.m_visionProviders[i].m_actorIndex == actorIndex && this.m_visionProviders[i].m_satelliteIndex == satelliteIndex)
			{
				this.m_visionProviders.RemoveAt(i);
			}
		}
	}

	public bool HasVisionProviderOnSatellite(int actorIndex, int satelliteIndex)
	{
		for (int i = 0; i < (int)this.m_visionProviders.Count; i++)
		{
			if (this.m_visionProviders[i].m_actorIndex == actorIndex)
			{
				if (this.m_visionProviders[i].m_satelliteIndex == satelliteIndex)
				{
					return true;
				}
			}
		}
		return false;
	}

	public unsafe bool GetVisionProviderInfoOnSatellite(int actorIndex, int satelliteIndex, out VisionProviderInfo visionProviderInfo)
	{
		for (int i = 0; i < (int)this.m_visionProviders.Count; i++)
		{
			if (this.m_visionProviders[i].m_actorIndex == actorIndex && this.m_visionProviders[i].m_satelliteIndex == satelliteIndex)
			{
				visionProviderInfo = this.m_visionProviders[i];
				return true;
			}
		}
		visionProviderInfo = default(VisionProviderInfo);
		return false;
	}

	[Server]
	public void ClearVisionProviders()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void ActorAdditionalVisionProviders::ClearVisionProviders()' called on client");
			return;
		}
		this.m_visionProviders.Clear();
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_visionProviders(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_visionProviders called on server.");
			return;
		}
		((ActorAdditionalVisionProviders)obj).m_visionProviders.HandleMsg(reader);
	}

	private void Awake()
	{
		this.m_visionProviders.InitializeBehaviour(this, ActorAdditionalVisionProviders.kListm_visionProviders);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			GeneratedNetworkCode._WriteStructSyncListVisionProviderInfo_None(writer, this.m_visionProviders);
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
			GeneratedNetworkCode._WriteStructSyncListVisionProviderInfo_None(writer, this.m_visionProviders);
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
			GeneratedNetworkCode._ReadStructSyncListVisionProviderInfo_None(reader, this.m_visionProviders);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			GeneratedNetworkCode._ReadStructSyncListVisionProviderInfo_None(reader, this.m_visionProviders);
		}
	}
}
