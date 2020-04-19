using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class PowerUp : NetworkBehaviour
{
	private static int s_nextPowerupGuid;

	public Ability m_ability;

	public string m_powerUpName;

	public string m_powerUpToolTip;

	[AudioEvent(false)]
	public string m_audioEventPickUp = "ui_pickup_health";

	public GameObject m_sequencePrefab;

	public bool m_restrictPickupByTeam;

	public PowerUp.PowerUpCategory m_chatterCategory;

	[SyncVar]
	private Team m_pickupTeam = Team.Objects;

	[SyncVar(hook = "HookSetGuid")]
	private int m_guid = -1;

	[SyncVar]
	private uint m_sequenceSourceId;

	private List<int> m_clientSequenceIds = new List<int>();

	private BoardSquare m_boardSquare;

	[SyncVar]
	public bool m_isSpoil;

	[SyncVar]
	public bool m_ignoreSpawnSplineForSequence;

	public int m_duration;

	private int m_age;

	private bool m_pickedUp;

	private bool m_stolen;

	private ActorTag m_tags;

	private bool m_markedForRemoval;

	private SequenceSource _sequenceSource;

	private static int kRpcRpcOnPickedUp = -0x19A229B0;

	private static int kRpcRpcOnSteal;

	static PowerUp()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(PowerUp), PowerUp.kRpcRpcOnPickedUp, new NetworkBehaviour.CmdDelegate(PowerUp.InvokeRpcRpcOnPickedUp));
		PowerUp.kRpcRpcOnSteal = 0x7269CE5A;
		NetworkBehaviour.RegisterRpcDelegate(typeof(PowerUp), PowerUp.kRpcRpcOnSteal, new NetworkBehaviour.CmdDelegate(PowerUp.InvokeRpcRpcOnSteal));
		NetworkCRC.RegisterBehaviour("PowerUp", 0);
	}

	public int Guid
	{
		get
		{
			return this.m_guid;
		}
		private set
		{
		}
	}

	public PowerUp.IPowerUpListener powerUpListener { get; set; }

	public BoardSquare boardSquare
	{
		get
		{
			return this.m_boardSquare;
		}
	}

	public Team PickupTeam
	{
		get
		{
			return this.m_pickupTeam;
		}
		set
		{
		}
	}

	public void SetPickupTeam(Team value)
	{
		this.Networkm_pickupTeam = value;
	}

	public void AddTag(string powerupTag)
	{
		if (this.m_tags == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.AddTag(string)).MethodHandle;
			}
			this.m_tags = base.gameObject.GetComponent<ActorTag>();
			if (this.m_tags == null)
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
				this.m_tags = base.gameObject.AddComponent<ActorTag>();
			}
		}
		this.m_tags.AddTag(powerupTag);
	}

	public bool HasTag(string powerupTag)
	{
		if (this.m_tags != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.HasTag(string)).MethodHandle;
			}
			return this.m_tags.HasTag(powerupTag);
		}
		return false;
	}

	internal SequenceSource SequenceSource
	{
		get
		{
			if (this._sequenceSource == null)
			{
				this._sequenceSource = new SequenceSource(null, null, false, null, null);
			}
			return this._sequenceSource;
		}
	}

	public void ClientSpawnSequences()
	{
		if (NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.ClientSpawnSequences()).MethodHandle;
			}
			if (this.m_clientSequenceIds.Count == 0)
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
				BoardSquare targetSquare = Board.\u000E().\u0012(base.transform.position.x, base.transform.position.z);
				PowerUp.ExtraParams extraParams = new PowerUp.ExtraParams();
				if (this.m_restrictPickupByTeam)
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
					extraParams.m_pickupTeamAsInt = (int)this.m_pickupTeam;
				}
				else
				{
					extraParams.m_pickupTeamAsInt = 2;
				}
				extraParams.m_ignoreSpawnSpline = this.m_ignoreSpawnSplineForSequence;
				Sequence.IExtraSequenceParams[] extraParams2 = new Sequence.IExtraSequenceParams[]
				{
					extraParams
				};
				if (this._sequenceSource == null)
				{
					this._sequenceSource = new SequenceSource(null, null, this.m_sequenceSourceId, false);
				}
				Sequence[] array = SequenceManager.Get().CreateClientSequences(this.m_sequencePrefab, targetSquare, null, null, this.SequenceSource, extraParams2);
				bool flag = false;
				if (array != null)
				{
					for (int i = 0; i < array.Length; i++)
					{
						array[i].RemoveAtTurnEnd = false;
						this.m_clientSequenceIds.Add(array[i].Id);
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
							if (PowerUpManager.Get() != null)
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
								array[i].transform.parent = PowerUpManager.Get().GetSpawnedPersistentSequencesRoot().transform;
								flag = true;
							}
						}
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
			}
		}
	}

	public void CalculateBoardSquare()
	{
		this.m_boardSquare = Board.\u000E().\u0012(base.transform.position.x, base.transform.position.z);
	}

	public void SetDuration(int duration)
	{
		this.m_duration = duration;
	}

	private void Awake()
	{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.Awake()).MethodHandle;
			}
			this.Networkm_guid = PowerUp.s_nextPowerupGuid++;
			SequenceSource sequenceSource = this.SequenceSource;
			this.Networkm_sequenceSourceId = sequenceSource.RootID;
		}
	}

	private void HookSetGuid(int guid)
	{
		this.Networkm_guid = guid;
		PowerUpManager.Get().SetPowerUpGuid(this, guid);
	}

	private void Start()
	{
		if (this.m_ability == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.Start()).MethodHandle;
			}
			Log.Error("PowerUp " + this + " needs a valid Ability assigned in the inspector for its prefab", new object[0]);
		}
		base.transform.parent = PowerUpManager.Get().GetSpawnedPowerupsRoot().transform;
		base.tag = "powerup";
		Transform[] componentsInChildren = base.GetComponentsInChildren<Transform>();
		int layer = LayerMask.NameToLayer("PowerUp");
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].gameObject.layer = layer;
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
		if (this.m_boardSquare == null)
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
			this.CalculateBoardSquare();
		}
		if (NetworkClient.active)
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
			if (PowerUpManager.Get() != null)
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
				PowerUpManager.Get().TrackClientPowerUp(this);
			}
			this.ClientSpawnSequences();
		}
	}

	[Server]
	public void CheckForPickupOnSpawn()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PowerUp::CheckForPickupOnSpawn()' called on client");
			return;
		}
	}

	public void CheckForPickupOnTurnStart()
	{
	}

	public void MarkSequencesForRemoval()
	{
		this.m_markedForRemoval = true;
		if (SequenceManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.MarkSequencesForRemoval()).MethodHandle;
			}
			for (int i = 0; i < this.m_clientSequenceIds.Count; i++)
			{
				Sequence sequence = SequenceManager.Get().FindSequence(this.m_clientSequenceIds[i]);
				if (sequence != null)
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
					sequence.MarkForRemoval();
				}
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

	public bool WasMarkedForRemoval()
	{
		return this.m_markedForRemoval;
	}

	public void SetHideSequence(bool hide)
	{
		for (int i = 0; i < this.m_clientSequenceIds.Count; i++)
		{
			Sequence sequence = SequenceManager.Get().FindSequence(this.m_clientSequenceIds[i]);
			if (sequence != null)
			{
				Transform transform = sequence.gameObject.transform;
				Vector3 localPosition;
				if (hide)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.SetHideSequence(bool)).MethodHandle;
					}
					localPosition = new Vector3(0f, -100f, 0f);
				}
				else
				{
					localPosition = Vector3.zero;
				}
				transform.localPosition = localPosition;
			}
		}
	}

	[ClientRpc]
	private void RpcOnPickedUp(int pickedUpByActorIndex)
	{
		this.Client_OnPickedUp(pickedUpByActorIndex);
	}

	public void Client_OnPickedUp(int pickedUpByActorIndex)
	{
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(pickedUpByActorIndex);
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.Client_OnPickedUp(int)).MethodHandle;
			}
			if (clientFog.IsVisible(this.boardSquare))
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
				string audioEventPickUp = this.m_audioEventPickUp;
				GameObject gameObject;
				if (actorData == null)
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
					gameObject = base.gameObject;
				}
				else
				{
					gameObject = actorData.gameObject;
				}
				AudioManager.PostEvent(audioEventPickUp, gameObject);
			}
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			Transform child = base.transform.GetChild(i);
			child.gameObject.SetActive(false);
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
		GameEventManager.Get().FireEvent(GameEventManager.EventType.PowerUpActivated, new GameEventManager.PowerUpActivatedArgs
		{
			byActor = actorData,
			powerUp = this
		});
		this.MarkSequencesForRemoval();
	}

	[ClientRpc]
	private void RpcOnSteal(int actorIndexFor3DAudio)
	{
		this.Client_OnSteal(actorIndexFor3DAudio);
	}

	public void Client_OnSteal(int actorIndexFor3DAudio)
	{
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (clientFog != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.Client_OnSteal(int)).MethodHandle;
			}
			if (clientFog.IsVisible(this.boardSquare))
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
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndexFor3DAudio);
				string audioEventPickUp = this.m_audioEventPickUp;
				GameObject gameObject;
				if (actorData == null)
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
					gameObject = base.gameObject;
				}
				else
				{
					gameObject = actorData.gameObject;
				}
				AudioManager.PostEvent(audioEventPickUp, gameObject);
			}
		}
		this.MarkSequencesForRemoval();
	}

	[Server]
	internal void Destroy()
	{
		if (!NetworkServer.active)
		{
			Debug.LogWarning("[Server] function 'System.Void PowerUp::Destroy()' called on client");
			return;
		}
		if (this.powerUpListener != null)
		{
			this.powerUpListener.OnPowerUpDestroyed(this);
		}
		NetworkServer.Destroy(base.gameObject);
	}

	private void OnDestroy()
	{
		if (NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.OnDestroy()).MethodHandle;
			}
			if (PowerUpManager.Get() != null)
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
				PowerUpManager.Get().UntrackClientPowerUp(this);
			}
			this.MarkSequencesForRemoval();
			this.m_clientSequenceIds.Clear();
		}
		if (PowerUpManager.Get() != null)
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
			PowerUpManager.Get().OnPowerUpDestroy(this);
		}
	}

	public bool TeamAllowedForPickUp(Team team)
	{
		bool result;
		if (this.m_restrictPickupByTeam)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.TeamAllowedForPickUp(Team)).MethodHandle;
			}
			result = (team == this.PickupTeam);
		}
		else
		{
			result = true;
		}
		return result;
	}

	internal void OnTurnTick()
	{
	}

	public bool CanBeStolen()
	{
		return true;
	}

	private void UNetVersion()
	{
	}

	public Team Networkm_pickupTeam
	{
		get
		{
			return this.m_pickupTeam;
		}
		[param: In]
		set
		{
			base.SetSyncVar<Team>(value, ref this.m_pickupTeam, 1U);
		}
	}

	public int Networkm_guid
	{
		get
		{
			return this.m_guid;
		}
		[param: In]
		set
		{
			uint dirtyBit = 2U;
			if (NetworkServer.localClientActive)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.set_Networkm_guid(int)).MethodHandle;
				}
				if (!base.syncVarHookGuard)
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
					base.syncVarHookGuard = true;
					this.HookSetGuid(value);
					base.syncVarHookGuard = false;
				}
			}
			base.SetSyncVar<int>(value, ref this.m_guid, dirtyBit);
		}
	}

	public uint Networkm_sequenceSourceId
	{
		get
		{
			return this.m_sequenceSourceId;
		}
		[param: In]
		set
		{
			base.SetSyncVar<uint>(value, ref this.m_sequenceSourceId, 4U);
		}
	}

	public bool Networkm_isSpoil
	{
		get
		{
			return this.m_isSpoil;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_isSpoil, 8U);
		}
	}

	public bool Networkm_ignoreSpawnSplineForSequence
	{
		get
		{
			return this.m_ignoreSpawnSplineForSequence;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_ignoreSpawnSplineForSequence, 0x10U);
		}
	}

	protected static void InvokeRpcRpcOnPickedUp(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcOnPickedUp called on server.");
			return;
		}
		((PowerUp)obj).RpcOnPickedUp((int)reader.ReadPackedUInt32());
	}

	protected static void InvokeRpcRpcOnSteal(NetworkBehaviour obj, NetworkReader reader)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.InvokeRpcRpcOnSteal(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcOnSteal called on server.");
			return;
		}
		((PowerUp)obj).RpcOnSteal((int)reader.ReadPackedUInt32());
	}

	public void CallRpcOnPickedUp(int pickedUpByActorIndex)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.CallRpcOnPickedUp(int)).MethodHandle;
			}
			Debug.LogError("RPC Function RpcOnPickedUp called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)PowerUp.kRpcRpcOnPickedUp);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)pickedUpByActorIndex);
		this.SendRPCInternal(networkWriter, 0, "RpcOnPickedUp");
	}

	public void CallRpcOnSteal(int actorIndexFor3DAudio)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcOnSteal called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)PowerUp.kRpcRpcOnSteal);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndexFor3DAudio);
		this.SendRPCInternal(networkWriter, 0, "RpcOnSteal");
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			writer.Write((int)this.m_pickupTeam);
			writer.WritePackedUInt32((uint)this.m_guid);
			writer.WritePackedUInt32(this.m_sequenceSourceId);
			writer.Write(this.m_isSpoil);
			writer.Write(this.m_ignoreSpawnSplineForSequence);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write((int)this.m_pickupTeam);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_guid);
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32(this.m_sequenceSourceId);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
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
				flag = true;
			}
			writer.Write(this.m_isSpoil);
		}
		if ((base.syncVarDirtyBits & 0x10U) != 0U)
		{
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
				flag = true;
			}
			writer.Write(this.m_ignoreSpawnSplineForSequence);
		}
		if (!flag)
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
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			this.m_pickupTeam = (Team)reader.ReadInt32();
			this.m_guid = (int)reader.ReadPackedUInt32();
			this.m_sequenceSourceId = reader.ReadPackedUInt32();
			this.m_isSpoil = reader.ReadBoolean();
			this.m_ignoreSpawnSplineForSequence = reader.ReadBoolean();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PowerUp.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			this.m_pickupTeam = (Team)reader.ReadInt32();
		}
		if ((num & 2) != 0)
		{
			this.HookSetGuid((int)reader.ReadPackedUInt32());
		}
		if ((num & 4) != 0)
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
			this.m_sequenceSourceId = reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
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
			this.m_isSpoil = reader.ReadBoolean();
		}
		if ((num & 0x10) != 0)
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
			this.m_ignoreSpawnSplineForSequence = reader.ReadBoolean();
		}
	}

	public enum PowerUpCategory
	{
		NoCategory,
		Healing,
		Might,
		Movement,
		Energy
	}

	public interface IPowerUpListener
	{
		void OnPowerUpDestroyed(PowerUp powerUp);

		PowerUp[] GetActivePowerUps();

		void SetSpawningEnabled(bool enabled);

		void OnTurnTick();

		bool IsPowerUpSpawnPoint(BoardSquare square);

		void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor);
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public int m_pickupTeamAsInt;

		public bool m_ignoreSpawnSpline;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			sbyte b = (sbyte)this.m_pickupTeamAsInt;
			stream.Serialize(ref b);
			stream.Serialize(ref this.m_ignoreSpawnSpline);
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			sbyte b = (sbyte)this.m_pickupTeamAsInt;
			stream.Serialize(ref b);
			this.m_pickupTeamAsInt = (int)b;
			stream.Serialize(ref this.m_ignoreSpawnSpline);
		}
	}
}
