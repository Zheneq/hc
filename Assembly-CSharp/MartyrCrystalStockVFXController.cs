using System;
using System.Collections.Generic;
using UnityEngine;

public class MartyrCrystalStockVFXController : CopyableVfxControllerComponent, IGameEventListener
{
	[AnimEventPicker]
	[Header("-- Anim Event for hiding crystals on client")]
	public UnityEngine.Object m_hideCrystalsAnimEvent;

	[Space(20f, order = 0)]
	[Header("-- Persistent vfx that can be started by anim event", order = 1)]
	public List<AnimEventToAttachedVfxData> m_animEventToVfxDataList;

	[Header("-- Incomplete Crystal Vfxs --")]
	public GameObject m_incompleteCrystalVfxPrefab;

	public List<GameObject> m_specificIncompleteCrystalVfxPrefabs;

	[Header("-- Full Crystal Vfxs --")]
	public List<AdditionalAttachedActorVfx.JointToVfx> m_jointToVfxList;

	private ActorModelData m_actorModelData;

	private List<AttachedActorVFXInfo> m_spawnedCrystalVfxList;

	private List<AttachedActorVFXInfo> m_incompleteCrystalVfxList;

	private Martyr_SyncComponent m_martyrSyncComp;

	private Passive_Martyr m_martyrPassive;

	private ActorData m_owner;

	private List<AnimEventToVfxContainer> m_animDrivenVfxList = new List<AnimEventToVfxContainer>();

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (this.m_martyrSyncComp != null && eventType == GameEventManager.EventType.ActorDamaged_Client)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrCrystalStockVFXController.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
			}
			if (args is GameEventManager.ActorHitHealthChangeArgs)
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
				GameEventManager.ActorHitHealthChangeArgs actorHitHealthChangeArgs = args as GameEventManager.ActorHitHealthChangeArgs;
				if (this.m_owner == actorHitHealthChangeArgs.m_target)
				{
					this.m_martyrSyncComp.m_clientDamageThisTurn += actorHitHealthChangeArgs.m_amount;
				}
			}
		}
	}

	private void Start()
	{
		this.Initialize();
	}

	private void Initialize()
	{
		this.m_actorModelData = base.GetComponent<ActorModelData>();
		this.m_spawnedCrystalVfxList = new List<AttachedActorVFXInfo>();
		this.m_incompleteCrystalVfxList = new List<AttachedActorVFXInfo>();
		if (this.m_actorModelData != null && this.m_actorModelData.m_parentActorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrCrystalStockVFXController.Initialize()).MethodHandle;
			}
			this.m_martyrSyncComp = this.m_actorModelData.m_parentActorData.GetComponent<Martyr_SyncComponent>();
			this.m_martyrPassive = this.m_actorModelData.m_parentActorData.GetComponent<Passive_Martyr>();
			this.m_owner = this.m_actorModelData.m_parentActorData;
			this.m_owner.OnAnimationEventDelegates += this.HandleAnimEvent;
			this.m_owner.OnTurnStartDelegates += this.HandleTurnTick;
			for (int i = 0; i < this.m_animEventToVfxDataList.Count; i++)
			{
				AnimEventToAttachedVfxData animEventToAttachedVfxData = this.m_animEventToVfxDataList[i];
				this.m_animDrivenVfxList.Add(new AnimEventToVfxContainer(animEventToAttachedVfxData.m_persistentVfxStartEvent, animEventToAttachedVfxData.m_persistentVfxStopEvent, animEventToAttachedVfxData.m_persistentVfxList, base.gameObject));
			}
		}
		int instanceLayer = LayerMask.NameToLayer("UIInWorld");
		for (int j = 0; j < this.m_jointToVfxList.Count; j++)
		{
			AdditionalAttachedActorVfx.JointToVfx jointToVfx = this.m_jointToVfxList[j];
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(jointToVfx.m_vfxCommonPrefab, base.gameObject, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "MartyrCrystalVfx_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
			{
				attachedActorVFXInfo.SetInstanceLocalPosition(jointToVfx.m_localOffset);
				if (this.m_martyrSyncComp == null)
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
					attachedActorVFXInfo.SetInstanceLayer(instanceLayer);
				}
				this.m_spawnedCrystalVfxList.Add(attachedActorVFXInfo);
			}
			else if (Application.isEditor)
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
				Debug.LogWarning("Failed to spawn vfx on joint in MartyrCrystalStockVFXController");
			}
			GameObject gameObject = this.m_incompleteCrystalVfxPrefab;
			if (this.m_incompleteCrystalVfxList != null && j < this.m_specificIncompleteCrystalVfxPrefabs.Count && this.m_specificIncompleteCrystalVfxPrefabs[j] != null)
			{
				gameObject = this.m_specificIncompleteCrystalVfxPrefabs[j];
			}
			if (gameObject != null)
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
				AttachedActorVFXInfo attachedActorVFXInfo2 = new AttachedActorVFXInfo(gameObject, base.gameObject, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "MartyrCrystalVfx_Partial_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
				if (attachedActorVFXInfo2.HasVfxInstance())
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
					attachedActorVFXInfo2.SetInstanceLocalPosition(jointToVfx.m_localOffset);
					this.m_incompleteCrystalVfxList.Add(attachedActorVFXInfo2);
				}
			}
		}
		if (GameEventManager.Get() != null)
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
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ActorDamaged_Client);
		}
	}

	private void Update()
	{
		ActorData actorData;
		if (this.m_actorModelData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrCrystalStockVFXController.Update()).MethodHandle;
			}
			actorData = this.m_actorModelData.m_parentActorData;
		}
		else
		{
			actorData = null;
		}
		ActorData actorData2 = actorData;
		bool flag;
		if (!(actorData2 == null))
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
			if (actorData2.\u0018())
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
				flag = (actorData2.\u000E() == null || actorData2.\u000E().enabled);
			}
			else
			{
				flag = false;
			}
		}
		else
		{
			flag = true;
		}
		bool flag2 = flag;
		bool flag3;
		if (actorData2 != null)
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
			flag3 = actorData2.\u0012();
		}
		else
		{
			flag3 = false;
		}
		bool flag4 = flag3;
		bool flag5;
		if (!(actorData2 == null))
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
			if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
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
				flag5 = (GameFlowData.Get().activeOwnedActorData.\u000E() == actorData2.\u000E());
			}
			else
			{
				flag5 = false;
			}
		}
		else
		{
			flag5 = true;
		}
		bool flag6 = flag5;
		int num = 0;
		int num2 = 0;
		if (this.m_martyrSyncComp == null)
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
			num = this.m_spawnedCrystalVfxList.Count;
		}
		else
		{
			this.GetNumCrystals(actorData2, out num, out num2);
		}
		int i = 0;
		while (i < this.m_spawnedCrystalVfxList.Count)
		{
			AttachedActorVFXInfo attachedActorVFXInfo = this.m_spawnedCrystalVfxList[i];
			if (!flag2)
			{
				goto IL_180;
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
			if (flag4)
			{
				goto IL_180;
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
			bool actorVisible = i < num;
			IL_181:
			attachedActorVFXInfo.UpdateVisibility(actorVisible, flag6);
			i++;
			continue;
			IL_180:
			actorVisible = false;
			goto IL_181;
		}
		for (int j = 0; j < this.m_incompleteCrystalVfxList.Count; j++)
		{
			bool flag7;
			if (j >= num)
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
				flag7 = (j < num + num2);
			}
			else
			{
				flag7 = false;
			}
			bool flag8 = flag7;
			AttachedActorVFXInfo attachedActorVFXInfo2 = this.m_incompleteCrystalVfxList[j];
			bool actorVisible2;
			if (flag2 && !flag4)
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
				actorVisible2 = flag8;
			}
			else
			{
				actorVisible2 = false;
			}
			attachedActorVFXInfo2.UpdateVisibility(actorVisible2, flag6);
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
		for (int k = 0; k < this.m_animDrivenVfxList.Count; k++)
		{
			AnimEventToVfxContainer animEventToVfxContainer = this.m_animDrivenVfxList[k];
			bool actorVisible3;
			if (flag2)
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
				actorVisible3 = !flag4;
			}
			else
			{
				actorVisible3 = false;
			}
			animEventToVfxContainer.UpdateVisibilityForSpawnedVfx(actorVisible3, flag6);
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

	private void OnDestroy()
	{
		if (this.m_spawnedCrystalVfxList != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrCrystalStockVFXController.OnDestroy()).MethodHandle;
			}
			for (int i = 0; i < this.m_spawnedCrystalVfxList.Count; i++)
			{
				this.m_spawnedCrystalVfxList[i].DestroyVfx();
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
			this.m_spawnedCrystalVfxList.Clear();
		}
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ActorDamaged_Client);
		}
		if (this.m_owner != null)
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
			this.m_owner.OnAnimationEventDelegates -= this.HandleAnimEvent;
			this.m_owner.OnTurnStartDelegates -= this.HandleTurnTick;
		}
	}

	private unsafe int GetNumCrystals(ActorData actor, out int numActive, out int numPartial)
	{
		numActive = 0;
		numPartial = 0;
		if (this.m_martyrSyncComp != null && this.m_martyrPassive != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrCrystalStockVFXController.GetNumCrystals(ActorData, int*, int*)).MethodHandle;
			}
			numActive = Mathf.Max(0, this.m_martyrSyncComp.DamageCrystals + this.m_martyrSyncComp.m_clientCrystalAdjustment);
			numActive = Mathf.Min(numActive, this.m_martyrPassive.m_maxCrystals);
			int num = numActive;
			if (this.m_martyrPassive.m_crystalGainMode == Passive_Martyr.CrystalGainMode.ByDamageTaken)
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
				if (this.m_martyrPassive.m_damageToCrystalConversion > 0f)
				{
					int num2 = Mathf.FloorToInt((float)this.m_martyrSyncComp.m_clientDamageThisTurn / this.m_martyrPassive.m_damageToCrystalConversion);
					if (this.m_martyrPassive.m_maxCrystalsGainedEachTurn > 0 && num2 > this.m_martyrPassive.m_maxCrystalsGainedEachTurn)
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
						num2 = this.m_martyrPassive.m_maxCrystalsGainedEachTurn;
					}
					num = numActive + num2;
					goto IL_161;
				}
			}
			if (this.m_martyrPassive.m_crystalGainMode == Passive_Martyr.CrystalGainMode.ByEnergy)
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
				if (actor != null)
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
					if (this.m_martyrPassive.m_energyToCrystalConversion > 0)
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
						int num3 = actor.\u0019();
						int num4 = num3 / this.m_martyrPassive.m_energyToCrystalConversion;
						if (num4 > num)
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
							num = num4;
						}
					}
				}
			}
			IL_161:
			num = Mathf.Min(num, this.m_martyrPassive.m_maxCrystals);
			numPartial = num - numActive;
			return num;
		}
		return 0;
	}

	private void HandleTurnTick()
	{
		for (int i = 0; i < this.m_animDrivenVfxList.Count; i++)
		{
			if (this.m_animDrivenVfxList[i].m_turnOffOnTurnStart)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrCrystalStockVFXController.HandleTurnTick()).MethodHandle;
				}
				this.m_animDrivenVfxList[i].m_shouldShowPersistentVfx = false;
			}
		}
		if (this.m_martyrSyncComp != null)
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
			this.m_martyrSyncComp.m_clientDamageThisTurn = 0;
		}
	}

	private void HandleAnimEvent(UnityEngine.Object eventObj, GameObject sourceObject)
	{
		if (this.m_owner != null)
		{
			for (int i = 0; i < this.m_animDrivenVfxList.Count; i++)
			{
				this.m_animDrivenVfxList[i].HandleAnimEvent(eventObj, sourceObject);
			}
			if (this.m_hideCrystalsAnimEvent != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(MartyrCrystalStockVFXController.HandleAnimEvent(UnityEngine.Object, GameObject)).MethodHandle;
				}
				if (eventObj == this.m_hideCrystalsAnimEvent)
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
					if (this.m_martyrSyncComp != null)
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
						this.m_martyrSyncComp.OnClientCrystalConsumed();
					}
				}
			}
		}
	}
}
