// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BrushCoordinator : NetworkBehaviour, IGameEventListener
{
	private static BrushCoordinator s_instance;

	private bool m_setupBrush;
	private bool m_cameraCreated;
	private bool m_brushVisible;

	// added in rogues
//#if SERVER
//	private bool m_updateHiddenVisionAreas;
//#endif

	public BrushRegion[] m_regions;
	private SyncListInt m_regionsLastDisruptionTurn = new SyncListInt();

#if SERVER
	// added in rogues
	//private List<GameObject[]> m_hiddenVisionAreaMeshObjects = new List<GameObject[]>();
	// added in rogues
	//private SyncListBool m_hiddenVisionAreasRevealed = new SyncListBool();
	// added in rogues
	private static Color s_notVisibleColor = new Color(1f, 1f, 1f, 1f);
	// added in rogues
	private static Color s_visibleColor = new Color(0f, 0f, 0f, 0f);
	// added in rogues -- cache for DisableAllBrush() -- not using it
	//public bool BrushDisabled { get; set; }
#endif

	// removed in rogues
	private static int kListm_regionsLastDisruptionTurn = 1707345339;
	// removed in rogues
	private static int kRpcRpcUpdateClientFog = -2062592578;

	static BrushCoordinator()
	{
		// reactor
		RegisterRpcDelegate(typeof(BrushCoordinator), kRpcRpcUpdateClientFog, InvokeRpcRpcUpdateClientFog);
		// rogues
		//RegisterRpcDelegate(typeof(BrushCoordinator), "RpcUpdateClientFog", new NetworkBehaviour.CmdDelegate(BrushCoordinator.InvokeRpcRpcUpdateClientFog));

		// reactor
		RegisterSyncListDelegate(typeof(BrushCoordinator), kListm_regionsLastDisruptionTurn, InvokeSyncListm_regionsLastDisruptionTurn);
		NetworkCRC.RegisterBehaviour("BrushCoordinator", 0);
	}

	// added in rogues -- instead of custom serialization?
	//public BrushCoordinator()
	//{
	//	base.InitSyncObject(this.m_regionsLastDisruptionTurn);
	//	base.InitSyncObject(this.m_hiddenVisionAreasRevealed);
	//}

	public static BrushCoordinator Get()
	{
		return s_instance;
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
		if (eventType == GameEventManager.EventType.GameCameraCreated
			&& (VisualsLoader.Get() == null || VisualsLoader.Get().LevelLoaded()))
		{
			m_cameraCreated = true;
		}
	}

	private void Awake()
	{
#if SERVER
		DontDestroyOnLoad(gameObject);  // added in rogues
#endif
		s_instance = this;
		m_regionsLastDisruptionTurn.InitializeBehaviour(this, kListm_regionsLastDisruptionTurn); // removed in rogues
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public override void OnStartClient()
	{
		// reactor
		m_regionsLastDisruptionTurn.Callback = SyncListCallbackLastDisruptTurn;
		// rogues
		//m_regionsLastDisruptionTurn.Callback += new SyncList<int>.SyncListChanged(this.SyncListCallbackLastDisruptTurn);
		//m_hiddenVisionAreasRevealed.Callback += new SyncList<bool>.SyncListChanged(this.SyncListCallbackVisionAreasRevealed);
	}

	public void EnableBrushVisibility()
	{
		m_brushVisible = true;
	}

	public bool DisableAllBrush()
	{
		return (SinglePlayerManager.Get() != null && !SinglePlayerManager.Get().EnableBrush())
			|| (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("DisableBrush"));
	}

	private void Start()
	{
		foreach (BrushRegion region in m_regions)
		{
			// reactor
			region.Initialize();
			// rogues
			//region.Initialize(base.gameObject.scene);

			if (NetworkServer.active)
			{
				m_regionsLastDisruptionTurn.Add(-1);
			}
		}
		TrySetupBrushSquares();
		DebugCheckRegions();
//#if SERVER
//		BrushDisabled = DisableAllBrush();  // added in rogues
//#endif
	}

	private void TrySetupBrushSquares()
	{
		if (!m_setupBrush
			&& Board.Get() != null
			&& GameFlowData.Get() != null
			&& m_cameraCreated)
		{
			SetupBrushSquares();
			FogOfWar.CalculateFogOfWarForTeam(Team.TeamA);
			FogOfWar.CalculateFogOfWarForTeam(Team.TeamB);
			m_setupBrush = true;
		}
	}

	private void DebugCheckRegions()
	{
		if (Application.isEditor)
		{
			for (int i = 0; i < m_regions.Length; i++)
			{
				BrushRegion brushRegion = m_regions[i];
				if (brushRegion != null)
				{
					if (brushRegion.m_disruptedVFX != null && brushRegion.m_disruptedVFX.GetComponent<PKFxFX>() == null)
					{
						Debug.LogError("Brush Region at index " + i + " has Disrupted VFX field set but it doesn't have a PKFxFX component");
					}
					if (brushRegion.m_functioningVFX != null && brushRegion.m_functioningVFX.GetComponent<PKFxFX>() == null)
					{
						Debug.LogError("Brush Region at index " + i + " has Functioning VFX field set but it doesn't have a PKFxFX component");
					}
				}
				else
				{
					Debug.LogError("Brush Region at index " + i + " is null");
				}
			}
		}
	}

	private void SetupBrushSquares()
	{
		for (int i = 0; i < m_regions.Length; i++)
		{
			if (m_regions[i] != null)
			{
				foreach (BoardSquare item in m_regions[i].GetSquaresInRegion())
				{
					if (item.BrushRegion == -1)
					{
						item.BrushRegion = i;
					}
					else
					{
						Log.Error($"Two brush regions ({item.BrushRegion} and {i}) are claiming the same boardSquare ({item.name})");
					}
				}
			}
			else
			{
				Log.Error($"Null brush region ({i}); fix brush coordinator's data.");
			}
		}
	}

	// added in rogues
//#if SERVER
//	public void ShowHiddenVisionArea(int index, bool show)
//	{
//		if (NetworkServer.active)
//		{
//			this.m_hiddenVisionAreasRevealed[index] = show;
//			GameObject[] array = this.m_hiddenVisionAreaMeshObjects[index];
//			for (int i = 0; i < array.Length; i++)
//			{
//				Debug.Log((show ? "Revealing" : "Hiding") + " VisionArea " + index);
//				array[i].SetActive(!show);
//				GameEventManager.Get().FireEvent(GameEventManager.EventType.BoardSquareVisibleShadeChanged, null);
//			}
//		}
//	}
//#endif

	// removed in rogues
	// TODO recheck this, looks a bit shady
	private void SyncListCallbackLastDisruptTurn(SyncList<int>.Operation op, int _incorrectIndexBugIn51And52)
	{
		if (FogOfWar.GetClientFog() != null)
		{
			FogOfWar.GetClientFog().MarkForRecalculateVisibility();
		}
	}

	// added in rogues
//#if SERVER
//	public void UpdateHiddenVisionAreas()
//	{
//		for (int i = 0; i < this.m_hiddenVisionAreaMeshObjects.Count; i++)
//		{
//			GameObject[] array = this.m_hiddenVisionAreaMeshObjects[i];
//			for (int j = 0; j < array.Length; j++)
//			{
//				Debug.Log((this.m_hiddenVisionAreasRevealed[i] ? "Revealing" : "Hiding") + " VisionArea " + i);
//				array[j].SetActive(!this.m_hiddenVisionAreasRevealed[i]);
//			}
//		}
//		GameEventManager.Get().FireEvent(GameEventManager.EventType.BoardSquareVisibleShadeChanged, null);
//	}
//#endif

	// added in rogues
//#if SERVER
//	public int RegisterVisionHiderArea(BoardRegion area, bool startOff)
//	{
//		// TODO LOW check material (commented out)
//		GameObject[] array = new GameObject[area.m_quads.Length];
//		//Material losmaterial = HighlightUtils.Get().m_LOSMaterial;
//		for (int i = 0; i < area.m_quads.Length; i++)
//		{
//			Bounds bounds = area.m_quads[i].GetBounds();  // GetBounds(base.gameObject.scene) in rogues
//			array[i] = GameObject.CreatePrimitive(PrimitiveType.Quad);
//			array[i].name = string.Format("Vision Hider Quad");
//			Object.DestroyImmediate(array[i].GetComponent<MeshCollider>());
//			array[i].transform.parent = base.transform;
//			array[i].transform.position = bounds.center;
//			array[i].transform.position += Vector3.up * 0.1f;
//			array[i].transform.Rotate(new Vector3(90f, 0f, 0f));
//			array[i].transform.localScale = new Vector3(bounds.extents.x * 2f, bounds.extents.z * 2f, 1f);
//			MeshRenderer component = array[i].GetComponent<MeshRenderer>();
//			//component.material = losmaterial;
//			component.enabled = true;
//			component.shadowCastingMode = 0;
//			component.receiveShadows = false;
//			array[i].layer = LayerMask.NameToLayer("FogOfWar");
//			Mesh mesh = array[i].GetComponent<MeshFilter>().mesh;
//			Vector3[] vertices = mesh.vertices;
//			Color32[] array2 = new Color32[vertices.Length];
//			int j = 0;
//			Color32 color = BrushCoordinator.s_notVisibleColor;
//			while (j < vertices.Length)
//			{
//				array2[j] = color;
//				j++;
//			}
//			mesh.colors32 = array2;
//			array[i].SetActive(!startOff);
//		}
//		this.m_hiddenVisionAreaMeshObjects.Add(array);
//		if (NetworkServer.active)
//		{
//			this.m_hiddenVisionAreasRevealed.Add(startOff);
//		}
//		return this.m_hiddenVisionAreaMeshObjects.Count - 1;
//	}
//#endif

	// added in rogues
	//private void SyncListCallbackLastDisruptTurn(SyncList<int>.Operation op, int index, int item)
	//{
	//	if (FogOfWar.GetClientFog() != null)
	//	{
	//		FogOfWar.GetClientFog().MarkForRecalculateVisibility();
	//	}
	//}

	// added in rogues
	//private void SyncListCallbackVisionAreasRevealed(SyncList<bool>.Operation op, int index, bool item)
	//{
	//	m_updateHiddenVisionAreas = true;
	//}

	[ClientRpc]
	public void RpcUpdateClientFog()
	{
		if (FogOfWar.GetClientFog() != null)
		{
			FogOfWar.GetClientFog().UpdateVisibilityOfSquares();
		}
	}

	// added in rogues
#if SERVER
	public void OnTurnEnd()
	{
		if (GameFlowData.Get() != null)
		{
			int currentTurn = GameFlowData.Get().CurrentTurn;
			for (int i = 0; i < this.m_regionsLastDisruptionTurn.Count; i++)
			{
				int num = this.m_regionsLastDisruptionTurn[i];
				if (num > 0 && currentTurn - num + 1 == GameplayData.Get().m_brushDisruptionTurns)
				{
					this.m_regionsLastDisruptionTurn[i] = -1;
				}
			}
			this.CallRpcUpdateClientFog();
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnDamaged_HandleConcealment(ActorData target, ActorData caster, DamageSource src, int appliedDamage, ServerCombatManager.DamageType damageType)
	{
		bool flag = damageType == ServerCombatManager.DamageType.Ability || (damageType == ServerCombatManager.DamageType.Effect && AbilityUtils.AbilityHasTag(src.Ability, AbilityTags.EffectDmgDisruptsCasterBrush));
		BoardSquare squareAtPhaseStart = caster.GetSquareAtPhaseStart();
		if (squareAtPhaseStart != null && flag)
		{
			int brushRegion = squareAtPhaseStart.BrushRegion;
			if (brushRegion >= 0 && this.IsRegionFunctioning(brushRegion))
			{
				this.DisruptBrush(brushRegion);
			}
		}
		if (target.IsInBrush())
		{
			int brushRegion2 = target.GetBrushRegion();
			this.DisruptBrush(brushRegion2);
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnEffect_HandleConcealment(Effect effect)
	{
		ActorData caster = effect.Caster;
		ActorData target = effect.Target;
		if (target != null && target.GetTeam() != caster.GetTeam() && target.IsInBrush())
		{
			int brushRegion = target.GetBrushRegion();
			this.DisruptBrush(brushRegion);
		}
	}
#endif

	// added in rogues
#if SERVER
	public void OnCast_HandleConcealment(ActorData caster, Ability ability, List<AbilityTarget> targets)
	{
		if (!AbilityUtils.AbilityHasTag(ability, AbilityTags.DontDisruptBrush))
		{
			if (caster.IsInBrush())
			{
				int brushRegion = caster.GetBrushRegion();
				this.DisruptBrush(brushRegion);
			}
			if (ability != null)
			{
				List<int> additionalBrushRegionsToDisrupt = ability.GetAdditionalBrushRegionsToDisrupt(caster, targets);
				if (additionalBrushRegionsToDisrupt != null)
				{
					for (int i = 0; i < additionalBrushRegionsToDisrupt.Count; i++)
					{
						this.DisruptBrush(additionalBrushRegionsToDisrupt[i]);
					}
				}
			}
		}
	}
#endif

	// added in rogues
#if SERVER
	[Server]
	public void DisruptBrush(int regionIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void BrushCoordinator::DisruptBrush(System.Int32)' called on client");
			return;
		}
		if (regionIndex >= 0 && regionIndex < this.m_regions.Length)
		{
			bool flag = false;
			if (this.IsRegionFunctioning(regionIndex))
			{
				flag = true;
			}
			if (this.m_regionsLastDisruptionTurn[regionIndex] != GameFlowData.Get().CurrentTurn)
			{
				this.m_regionsLastDisruptionTurn[regionIndex] = GameFlowData.Get().CurrentTurn;
			}
			if (flag)
			{
				ServerActionBuffer.Get().ImmediateUpdateAllFogOfWar();
				foreach (ActorData actorData in GameFlowData.Get().GetActors())
				{
					if (actorData != null && actorData.GetBrushRegion() == regionIndex)
					{
						actorData.CallRpcMarkForRecalculateClientVisibility();
						if (!actorData.IsDead() && actorData.GetCurrentBoardSquare() != null && actorData.ServerLastKnownPosSquare != actorData.GetCurrentBoardSquare() && actorData.IsActorVisibleToAnyEnemy())
						{
							actorData.SetServerLastKnownPosSquare(actorData.CurrentBoardSquare, "DisruptBrush");
						}
					}
				}
			}
		}
	}
#endif

	private void Update()
	{
		TrySetupBrushSquares();
		// reactor
		bool areBrushesEnabled = !DisableAllBrush();
		// rogues
		//BrushDisabled = DisableAllBrush();  // added in rogues
		//bool areBrushesEnabled = !BrushDisabled;
		bool areVfxVisible = m_brushVisible
			&& (CameraManager.Get() == null || !CameraManager.Get().ShouldHideBrushVfx());
//#if SERVER
//		if (m_updateHiddenVisionAreas)  // added in rogues
//		{
//			UpdateHiddenVisionAreas();
//			m_updateHiddenVisionAreas = false;
//		}
//#endif
		if (!m_setupBrush)
		{
			return;
		}
		for (int i = 0; i < m_regions.Length; i++)
		{
			BrushRegion brushRegion = m_regions[i];
			bool isRegionFunctioning = IsRegionFunctioning(i);
			if (brushRegion.m_disruptedVFX != null)
			{
				if (areBrushesEnabled && !isRegionFunctioning && areVfxVisible)
				{
					brushRegion.m_disruptedVFX.GetComponent<PKFxFX>().StartEffect();
				}
				else
				{
					brushRegion.m_disruptedVFX.GetComponent<PKFxFX>().TerminateEffect();
				}
			}
			else
			{
				foreach (GameObject vfx in brushRegion.m_perSquareDisruptedVFX)
				{
					if (areBrushesEnabled && !isRegionFunctioning && areVfxVisible)
					{
						vfx.GetComponent<PKFxFX>().StartEffect();
					}
					else
					{
						vfx.GetComponent<PKFxFX>().TerminateEffect();
					}
				}
			}
			if (brushRegion.m_functioningVFX != null)
			{
				if (isRegionFunctioning && areVfxVisible)
				{
					brushRegion.m_functioningVFX.GetComponent<PKFxFX>().StartEffect();
				}
				else
				{
					brushRegion.m_functioningVFX.GetComponent<PKFxFX>().TerminateEffect();
				}
			}
			else
			{
				foreach (GameObject vfx in brushRegion.m_perSquareFunctioningVFX)
				{
					if (areBrushesEnabled && isRegionFunctioning && areVfxVisible)
					{
						vfx.GetComponent<PKFxFX>().StartEffect();
					}
					else
					{
						vfx.GetComponent<PKFxFX>().TerminateEffect();
					}
				}
			}
			brushRegion.UpdateBorderVisibility(isRegionFunctioning);
		}
	}

	public List<ActorData> GetActorsInBrushRegion(int regionIndex)
	{
		if (DisableAllBrush())  // if BrushDisabled in rogues
		{
			return new List<ActorData>();
		}
		if (regionIndex >= 0 && regionIndex < m_regions.Length)
		{
			return m_regions[regionIndex].GetActorsInRegion();
		}
		return null;
	}

	public bool IsRegionFunctioning(int regionIndex)
	{
		if (DisableAllBrush())  // if BrushDisabled in rogues
		{
			return false;
		}
		if (regionIndex >= 0 && regionIndex < m_regions.Length)
		{
			// NOTE ROGUES
			// reactor
			int lastDisruptionTurn = m_regionsLastDisruptionTurn[regionIndex];
			// rogues
			//int lastDisruptionTurn = m_regionsLastDisruptionTurn.Count > regionIndex
			//	? m_regionsLastDisruptionTurn[regionIndex]  // unconditional get in reactor
			//	: -1;
			if (lastDisruptionTurn <= 0)
			{
				return true;
			}
			int turnsPassed = GameFlowData.Get().CurrentTurn - lastDisruptionTurn;
			if (turnsPassed >= GameplayData.Get().m_brushDisruptionTurns)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsSquareHiddenFrom(BoardSquare subjectSquare, BoardSquare observerSquare)
	{
		if (DisableAllBrush())  // if BrushDisabled in rogues
		{
			return false;
		}
		bool isHidden = false;
		if (subjectSquare != null
			&& observerSquare != null
			&& subjectSquare.BrushRegion != observerSquare.BrushRegion
			&& subjectSquare.IsInBrush()
			&& IsRegionFunctioning(subjectSquare.BrushRegion))
		{
			float distance = observerSquare.HorizontalDistanceOnBoardTo(subjectSquare);
			float distanceCanSeeIntoBrush = GameplayData.Get().m_distanceCanSeeIntoBrush;
			isHidden = distance > distanceCanSeeIntoBrush;
		}
		if (!isHidden)
		{
			return false;
		}
		bool isRevealed = false;
		if (subjectSquare != null && subjectSquare.OccupantActor != null)
		{
			ActorStatus actorStatus = subjectSquare.OccupantActor.GetActorStatus();
			isRevealed = actorStatus.HasStatus(StatusType.Revealed)
				|| actorStatus.HasStatus(StatusType.CantHideInBrush);
		}
		return isHidden && !isRevealed;
	}

	internal List<Plane> CalcIntersectingBrushSidePlanes(Bounds bounds)
	{
		List<Plane> result = new List<Plane>(4);
		Bounds bounds2 = new Bounds(new Vector3(bounds.center.x, 0f, bounds.center.z), bounds.size);
		foreach (BoardSquare boardSquare in Board.Get().GetSquaresInBox(bounds2))
		{
			byte brushExteriorSideFlags = GetBrushExteriorSideFlags(boardSquare);
			for (byte b = 0; b < 4; b++)
			{
				byte mask = (byte)(1 << b);
				if ((mask & brushExteriorSideFlags) != 0
					&& boardSquare.CalcSideBounds((SideFlags)mask).Intersects(bounds))
				{
					result.Add(boardSquare.CalcSidePlane((SideFlags)mask));
				}
			}
		}
		return result;
	}

	internal byte GetBrushExteriorSideFlags(BoardSquare square)
	{
		if (IsRegionFunctioning(square.BrushRegion))
		{
			return m_regions[square.BrushRegion].GetExteriorSideFlags(square);
		}
		return 0;
	}

	private void OnDrawGizmos()
	{
		if (CameraManager.ShouldDrawGizmosForCurrentCamera() && m_setupBrush)
		{
			for (int i = 0; i < m_regions.Length; i++)
			{
				m_regions[i].DrawOutlineGizmos(IsRegionFunctioning(i));
			}
		}
	}

	// reactor
	private void UNetVersion()
	{
	}
	// rogues
	//private void MirrorProcessed()
	//{
	//}

	// removed in rogues
	protected static void InvokeSyncListm_regionsLastDisruptionTurn(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_regionsLastDisruptionTurn called on server.");
			return;
		}
		((BrushCoordinator)obj).m_regionsLastDisruptionTurn.HandleMsg(reader);
	}

	protected static void InvokeRpcRpcUpdateClientFog(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcUpdateClientFog called on server.");
			return;
		}
		((BrushCoordinator)obj).RpcUpdateClientFog();
	}

	public void CallRpcUpdateClientFog()
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcUpdateClientFog called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcUpdateClientFog);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		SendRPCInternal(networkWriter, 0, "RpcUpdateClientFog");

		// rogues
		//NetworkWriter networkWriter = new NetworkWriter();
		//this.SendRPCInternal(typeof(BrushCoordinator), "RpcUpdateClientFog", networkWriter, 0);
	}

	// removed in rogues
	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListInt.WriteInstance(writer, m_regionsLastDisruptionTurn);
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
			SyncListInt.WriteInstance(writer, m_regionsLastDisruptionTurn);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	// removed in rogues
	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		int setBits = int.MaxValue;
		if (!initialState)
		{
			setBits = (int)reader.ReadPackedUInt32();
		}
		if ((setBits & 1) != 0)
		{
			SyncListInt.ReadReference(reader, m_regionsLastDisruptionTurn);
		}
	}
}
