using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BrushCoordinator : NetworkBehaviour, IGameEventListener
{
	private static BrushCoordinator s_instance;

	private bool m_setupBrush;

	private bool m_cameraCreated;

	private bool m_brushVisible;

	public BrushRegion[] m_regions;

	private SyncListInt m_regionsLastDisruptionTurn = new SyncListInt();

	private static int kListm_regionsLastDisruptionTurn;

	private static int kRpcRpcUpdateClientFog = -0x7AF0AA42;

	static BrushCoordinator()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(BrushCoordinator), BrushCoordinator.kRpcRpcUpdateClientFog, new NetworkBehaviour.CmdDelegate(BrushCoordinator.InvokeRpcRpcUpdateClientFog));
		BrushCoordinator.kListm_regionsLastDisruptionTurn = 0x65C405BB;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(BrushCoordinator), BrushCoordinator.kListm_regionsLastDisruptionTurn, new NetworkBehaviour.CmdDelegate(BrushCoordinator.InvokeSyncListm_regionsLastDisruptionTurn));
		NetworkCRC.RegisterBehaviour("BrushCoordinator", 0);
	}

	public static BrushCoordinator Get()
	{
		return BrushCoordinator.s_instance;
	}

	private void OnEnable()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.GameCameraCreated);
	}

	private void OnDisable()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.GameCameraCreated);
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.GameCameraCreated)
		{
			if (!(VisualsLoader.Get() == null))
			{
				if (!VisualsLoader.Get().LevelLoaded())
				{
					return;
				}
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
				}
			}
			this.m_cameraCreated = true;
		}
	}

	private void Awake()
	{
		BrushCoordinator.s_instance = this;
		this.m_regionsLastDisruptionTurn.InitializeBehaviour(this, BrushCoordinator.kListm_regionsLastDisruptionTurn);
	}

	private void OnDestroy()
	{
		BrushCoordinator.s_instance = null;
	}

	public override void OnStartClient()
	{
		this.m_regionsLastDisruptionTurn.Callback = new SyncList<int>.SyncListChanged(this.SyncListCallbackLastDisruptTurn);
	}

	public void EnableBrushVisibility()
	{
		this.m_brushVisible = true;
	}

	public bool DisableAllBrush()
	{
		if (SinglePlayerManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.DisableAllBrush()).MethodHandle;
			}
			if (!SinglePlayerManager.Get().EnableBrush())
			{
				return true;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		bool result;
		if (DebugParameters.Get() != null)
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
			result = DebugParameters.Get().GetParameterAsBool("DisableBrush");
		}
		else
		{
			result = false;
		}
		return result;
	}

	private void Start()
	{
		for (int i = 0; i < this.m_regions.Length; i++)
		{
			this.m_regions[i].Initialize();
			if (NetworkServer.active)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.Start()).MethodHandle;
				}
				this.m_regionsLastDisruptionTurn.Add(-1);
			}
		}
		this.TrySetupBrushSquares();
		this.\u000E();
	}

	private void TrySetupBrushSquares()
	{
		if (!this.m_setupBrush)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.TrySetupBrushSquares()).MethodHandle;
			}
			if (Board.\u000E() != null)
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
				if (GameFlowData.Get() != null && this.m_cameraCreated)
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
					this.SetupBrushSquares();
					FogOfWar.CalculateFogOfWarForTeam(Team.TeamA);
					FogOfWar.CalculateFogOfWarForTeam(Team.TeamB);
					this.m_setupBrush = true;
				}
			}
		}
	}

	private void \u000E()
	{
		if (Application.isEditor)
		{
			for (int i = 0; i < this.m_regions.Length; i++)
			{
				BrushRegion brushRegion = this.m_regions[i];
				if (brushRegion == null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.\u000E()).MethodHandle;
					}
					Debug.LogError("Brush Region at index " + i + " is null");
				}
				else
				{
					if (brushRegion.m_disruptedVFX != null)
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
						if (brushRegion.m_disruptedVFX.GetComponent<PKFxFX>() == null)
						{
							Debug.LogError("Brush Region at index " + i + " has Disrupted VFX field set but it doesn't have a PKFxFX component");
						}
					}
					if (brushRegion.m_functioningVFX != null)
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
						if (brushRegion.m_functioningVFX.GetComponent<PKFxFX>() == null)
						{
							Debug.LogError("Brush Region at index " + i + " has Functioning VFX field set but it doesn't have a PKFxFX component");
						}
					}
				}
			}
		}
	}

	private void SetupBrushSquares()
	{
		for (int i = 0; i < this.m_regions.Length; i++)
		{
			if (this.m_regions[i] == null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.SetupBrushSquares()).MethodHandle;
				}
				Log.Error(string.Format("Null brush region ({0}); fix brush coordinator's data.", i), new object[0]);
			}
			else
			{
				List<BoardSquare> list = this.m_regions[i].\u001D();
				foreach (BoardSquare boardSquare in list)
				{
					if (boardSquare.BrushRegion == -1)
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
						boardSquare.BrushRegion = i;
					}
					else
					{
						Log.Error(string.Format("Two brush regions ({0} and {1}) are claiming the same boardSquare ({2})", boardSquare.BrushRegion, i, boardSquare.name), new object[0]);
					}
				}
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void SyncListCallbackLastDisruptTurn(SyncList<int>.Operation op, int _incorrectIndexBugIn51And52)
	{
		if (FogOfWar.GetClientFog() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.SyncListCallbackLastDisruptTurn(SyncList<int>.Operation, int)).MethodHandle;
			}
			FogOfWar.GetClientFog().MarkForRecalculateVisibility();
		}
	}

	[ClientRpc]
	public void RpcUpdateClientFog()
	{
		if (FogOfWar.GetClientFog() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.RpcUpdateClientFog()).MethodHandle;
			}
			FogOfWar.GetClientFog().UpdateVisibilityOfSquares(true);
		}
	}

	private void Update()
	{
		this.TrySetupBrushSquares();
		bool flag = !this.DisableAllBrush();
		bool flag2 = this.m_brushVisible && (CameraManager.Get() == null || !CameraManager.Get().ShouldHideBrushVfx());
		if (this.m_setupBrush)
		{
			for (int i = 0; i < this.m_regions.Length; i++)
			{
				BrushRegion brushRegion = this.m_regions[i];
				bool flag3 = this.IsRegionFunctioning(i);
				if (brushRegion.m_disruptedVFX != null)
				{
					if (!flag)
					{
						goto IL_BE;
					}
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.Update()).MethodHandle;
					}
					if (flag3)
					{
						goto IL_BE;
					}
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!flag2)
					{
						goto IL_BE;
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
					brushRegion.m_disruptedVFX.GetComponent<PKFxFX>().StartEffect();
					goto IL_144;
					IL_BE:
					brushRegion.m_disruptedVFX.GetComponent<PKFxFX>().TerminateEffect();
				}
				else
				{
					int j = 0;
					while (j < brushRegion.m_perSquareDisruptedVFX.Count)
					{
						GameObject gameObject = brushRegion.m_perSquareDisruptedVFX[j];
						if (!flag || flag3)
						{
							goto IL_117;
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!flag2)
						{
							goto IL_117;
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						gameObject.GetComponent<PKFxFX>().StartEffect();
						IL_125:
						j++;
						continue;
						IL_117:
						gameObject.GetComponent<PKFxFX>().TerminateEffect();
						goto IL_125;
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				IL_144:
				if (brushRegion.m_functioningVFX != null)
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
					if (!flag3)
					{
						goto IL_183;
					}
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!flag2)
					{
						goto IL_183;
					}
					brushRegion.m_functioningVFX.GetComponent<PKFxFX>().StartEffect();
					goto IL_1F4;
					IL_183:
					brushRegion.m_functioningVFX.GetComponent<PKFxFX>().TerminateEffect();
				}
				else
				{
					for (int k = 0; k < brushRegion.m_perSquareFunctioningVFX.Count; k++)
					{
						GameObject gameObject2 = brushRegion.m_perSquareFunctioningVFX[k];
						if (flag && flag3 && flag2)
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
							gameObject2.GetComponent<PKFxFX>().StartEffect();
						}
						else
						{
							gameObject2.GetComponent<PKFxFX>().TerminateEffect();
						}
					}
				}
				IL_1F4:
				brushRegion.UpdateBorderVisibility(flag3);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	public List<ActorData> GetActorsInBrushRegion(int regionIndex)
	{
		List<ActorData> result = null;
		if (this.DisableAllBrush())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.GetActorsInBrushRegion(int)).MethodHandle;
			}
			return new List<ActorData>();
		}
		if (regionIndex >= 0)
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
			if (regionIndex < this.m_regions.Length)
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
				BrushRegion brushRegion = this.m_regions[regionIndex];
				result = brushRegion.\u001D();
			}
		}
		return result;
	}

	public bool IsRegionFunctioning(int regionIndex)
	{
		if (this.DisableAllBrush())
		{
			return false;
		}
		bool result;
		if (regionIndex >= 0 && regionIndex < this.m_regions.Length)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.IsRegionFunctioning(int)).MethodHandle;
			}
			int num = this.m_regionsLastDisruptionTurn[regionIndex];
			if (num <= 0)
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
				result = true;
			}
			else
			{
				int num2 = GameFlowData.Get().CurrentTurn - num;
				result = (num2 >= GameplayData.Get().m_brushDisruptionTurns);
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool IsSquareHiddenFrom(BoardSquare subjectSquare, BoardSquare observerSquare)
	{
		if (this.DisableAllBrush())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.IsSquareHiddenFrom(BoardSquare, BoardSquare)).MethodHandle;
			}
			return false;
		}
		bool flag;
		if (!(subjectSquare == null))
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
			if (observerSquare == null)
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
			}
			else
			{
				if (subjectSquare.BrushRegion == observerSquare.BrushRegion)
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
					flag = false;
					goto IL_D1;
				}
				if (subjectSquare.\u0012())
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
					int brushRegion = subjectSquare.BrushRegion;
					if (this.IsRegionFunctioning(brushRegion))
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
						float num = observerSquare.HorizontalDistanceOnBoardTo(subjectSquare);
						float distanceCanSeeIntoBrush = GameplayData.Get().m_distanceCanSeeIntoBrush;
						if (num > distanceCanSeeIntoBrush)
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
							flag = true;
						}
						else
						{
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
					goto IL_D1;
				}
				flag = false;
				goto IL_D1;
			}
		}
		flag = false;
		IL_D1:
		if (!flag)
		{
			return false;
		}
		bool flag2;
		if (!(subjectSquare == null))
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
			if (!(subjectSquare.OccupantActor == null))
			{
				ActorStatus actorStatus = subjectSquare.OccupantActor.\u000E();
				if (!actorStatus.HasStatus(StatusType.Revealed, true))
				{
					if (!actorStatus.HasStatus(StatusType.CantHideInBrush, true))
					{
						flag2 = false;
						goto IL_144;
					}
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				flag2 = true;
				goto IL_144;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		flag2 = false;
		IL_144:
		bool result;
		if (flag)
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
			result = !flag2;
		}
		else
		{
			result = false;
		}
		return result;
	}

	internal List<Plane> CalcIntersectingBrushSidePlanes(Bounds bounds)
	{
		List<Plane> list = new List<Plane>(4);
		Bounds u001D = new Bounds(new Vector3(bounds.center.x, 0f, bounds.center.z), bounds.size);
		List<BoardSquare> list2 = Board.\u000E().\u000E(u001D, null);
		for (int i = 0; i < list2.Count; i++)
		{
			BoardSquare boardSquare = list2[i];
			byte brushExteriorSideFlags = this.GetBrushExteriorSideFlags(boardSquare);
			for (byte b = 0; b < 4; b += 1)
			{
				byte b2 = (byte)(1 << (int)b);
				if ((b2 & brushExteriorSideFlags) != 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.CalcIntersectingBrushSidePlanes(Bounds)).MethodHandle;
					}
					if (boardSquare.CalcSideBounds((SideFlags)b2).Intersects(bounds))
					{
						list.Add(boardSquare.CalcSidePlane((SideFlags)b2));
					}
				}
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		return list;
	}

	internal byte GetBrushExteriorSideFlags(BoardSquare square)
	{
		if (this.IsRegionFunctioning(square.BrushRegion))
		{
			BrushRegion brushRegion = this.m_regions[square.BrushRegion];
			return brushRegion.GetExteriorSideFlags(square);
		}
		return 0;
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		if (this.m_setupBrush)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.OnDrawGizmos()).MethodHandle;
			}
			for (int i = 0; i < this.m_regions.Length; i++)
			{
				BrushRegion brushRegion = this.m_regions[i];
				bool functioning = this.IsRegionFunctioning(i);
				brushRegion.DrawOutlineGizmos(functioning);
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_regionsLastDisruptionTurn(NetworkBehaviour obj, NetworkReader reader)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.InvokeSyncListm_regionsLastDisruptionTurn(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("SyncList m_regionsLastDisruptionTurn called on server.");
			return;
		}
		((BrushCoordinator)obj).m_regionsLastDisruptionTurn.HandleMsg(reader);
	}

	protected static void InvokeRpcRpcUpdateClientFog(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.InvokeRpcRpcUpdateClientFog(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcUpdateClientFog called on server.");
			return;
		}
		((BrushCoordinator)obj).RpcUpdateClientFog();
	}

	public void CallRpcUpdateClientFog()
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.CallRpcUpdateClientFog()).MethodHandle;
			}
			Debug.LogError("RPC Function RpcUpdateClientFog called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)BrushCoordinator.kRpcRpcUpdateClientFog);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		this.SendRPCInternal(networkWriter, 0, "RpcUpdateClientFog");
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			SyncListInt.WriteInstance(writer, this.m_regionsLastDisruptionTurn);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, this.m_regionsLastDisruptionTurn);
		}
		if (!flag)
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
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BrushCoordinator.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			SyncListInt.ReadReference(reader, this.m_regionsLastDisruptionTurn);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
			SyncListInt.ReadReference(reader, this.m_regionsLastDisruptionTurn);
		}
	}
}
