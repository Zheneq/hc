using System.Collections.Generic;
using UnityEngine;

public class MartyrCrystalStockVFXController : CopyableVfxControllerComponent, IGameEventListener
{
	[AnimEventPicker]
	[Header("-- Anim Event for hiding crystals on client")]
	public Object m_hideCrystalsAnimEvent;

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
		if (!(m_martyrSyncComp != null) || eventType != GameEventManager.EventType.ActorDamaged_Client)
		{
			return;
		}
		while (true)
		{
			if (!(args is GameEventManager.ActorHitHealthChangeArgs))
			{
				return;
			}
			while (true)
			{
				GameEventManager.ActorHitHealthChangeArgs actorHitHealthChangeArgs = args as GameEventManager.ActorHitHealthChangeArgs;
				if (m_owner == actorHitHealthChangeArgs.m_target)
				{
					m_martyrSyncComp.m_clientDamageThisTurn += actorHitHealthChangeArgs.m_amount;
				}
				return;
			}
		}
	}

	private void Start()
	{
		Initialize();
	}

	private void Initialize()
	{
		m_actorModelData = GetComponent<ActorModelData>();
		m_spawnedCrystalVfxList = new List<AttachedActorVFXInfo>();
		m_incompleteCrystalVfxList = new List<AttachedActorVFXInfo>();
		if (m_actorModelData != null && m_actorModelData.m_parentActorData != null)
		{
			m_martyrSyncComp = m_actorModelData.m_parentActorData.GetComponent<Martyr_SyncComponent>();
			m_martyrPassive = m_actorModelData.m_parentActorData.GetComponent<Passive_Martyr>();
			m_owner = m_actorModelData.m_parentActorData;
			m_owner.OnAnimationEventDelegates += HandleAnimEvent;
			m_owner.OnTurnStartDelegates += HandleTurnTick;
			for (int i = 0; i < m_animEventToVfxDataList.Count; i++)
			{
				AnimEventToAttachedVfxData animEventToAttachedVfxData = m_animEventToVfxDataList[i];
				m_animDrivenVfxList.Add(new AnimEventToVfxContainer(animEventToAttachedVfxData.m_persistentVfxStartEvent, animEventToAttachedVfxData.m_persistentVfxStopEvent, animEventToAttachedVfxData.m_persistentVfxList, base.gameObject));
			}
		}
		int instanceLayer = LayerMask.NameToLayer("UIInWorld");
		for (int j = 0; j < m_jointToVfxList.Count; j++)
		{
			AdditionalAttachedActorVfx.JointToVfx jointToVfx = m_jointToVfxList[j];
			AttachedActorVFXInfo attachedActorVFXInfo = new AttachedActorVFXInfo(jointToVfx.m_vfxCommonPrefab, base.gameObject, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "MartyrCrystalVfx_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo.HasVfxInstance())
			{
				attachedActorVFXInfo.SetInstanceLocalPosition(jointToVfx.m_localOffset);
				if (m_martyrSyncComp == null)
				{
					attachedActorVFXInfo.SetInstanceLayer(instanceLayer);
				}
				m_spawnedCrystalVfxList.Add(attachedActorVFXInfo);
			}
			else if (Application.isEditor)
			{
				Debug.LogWarning("Failed to spawn vfx on joint in MartyrCrystalStockVFXController");
			}
			GameObject gameObject = m_incompleteCrystalVfxPrefab;
			if (m_incompleteCrystalVfxList != null && j < m_specificIncompleteCrystalVfxPrefabs.Count && m_specificIncompleteCrystalVfxPrefabs[j] != null)
			{
				gameObject = m_specificIncompleteCrystalVfxPrefabs[j];
			}
			if (!(gameObject != null))
			{
				continue;
			}
			AttachedActorVFXInfo attachedActorVFXInfo2 = new AttachedActorVFXInfo(gameObject, base.gameObject, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "MartyrCrystalVfx_Partial_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
			if (attachedActorVFXInfo2.HasVfxInstance())
			{
				attachedActorVFXInfo2.SetInstanceLocalPosition(jointToVfx.m_localOffset);
				m_incompleteCrystalVfxList.Add(attachedActorVFXInfo2);
			}
		}
		if (GameEventManager.Get() == null)
		{
			return;
		}
		while (true)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ActorDamaged_Client);
			return;
		}
	}

	private void Update()
	{
		object obj;
		if (m_actorModelData != null)
		{
			obj = m_actorModelData.m_parentActorData;
		}
		else
		{
			obj = null;
		}
		ActorData actorData = (ActorData)obj;
		int num;
		if (!(actorData == null))
		{
			if (actorData.IsVisibleToClient())
			{
				num = ((actorData.GetActorModelDataRenderer() == null || actorData.GetActorModelDataRenderer().enabled) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
		}
		else
		{
			num = 1;
		}
		bool flag = (byte)num != 0;
		int num2;
		if (actorData != null)
		{
			num2 = (actorData.IsModelAnimatorDisabled() ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		bool flag2 = (byte)num2 != 0;
		int num3;
		if (!(actorData == null))
		{
			if (GameFlowData.Get() != null && GameFlowData.Get().activeOwnedActorData != null)
			{
				num3 = ((GameFlowData.Get().activeOwnedActorData.GetTeam() == actorData.GetTeam()) ? 1 : 0);
			}
			else
			{
				num3 = 0;
			}
		}
		else
		{
			num3 = 1;
		}
		bool flag3 = (byte)num3 != 0;
		int numActive = 0;
		int numPartial = 0;
		if (m_martyrSyncComp == null)
		{
			numActive = m_spawnedCrystalVfxList.Count;
		}
		else
		{
			GetNumCrystals(actorData, out numActive, out numPartial);
		}
		AttachedActorVFXInfo attachedActorVFXInfo;
		int actorVisible;
		for (int i = 0; i < m_spawnedCrystalVfxList.Count; attachedActorVFXInfo.UpdateVisibility((byte)actorVisible != 0, flag3), i++)
		{
			attachedActorVFXInfo = m_spawnedCrystalVfxList[i];
			if (flag)
			{
				if (!flag2)
				{
					actorVisible = ((i < numActive) ? 1 : 0);
					continue;
				}
			}
			actorVisible = 0;
		}
		for (int j = 0; j < m_incompleteCrystalVfxList.Count; j++)
		{
			int num4;
			if (j >= numActive)
			{
				num4 = ((j < numActive + numPartial) ? 1 : 0);
			}
			else
			{
				num4 = 0;
			}
			bool flag4 = (byte)num4 != 0;
			AttachedActorVFXInfo attachedActorVFXInfo2 = m_incompleteCrystalVfxList[j];
			int actorVisible2;
			if (flag && !flag2)
			{
				actorVisible2 = (flag4 ? 1 : 0);
			}
			else
			{
				actorVisible2 = 0;
			}
			attachedActorVFXInfo2.UpdateVisibility((byte)actorVisible2 != 0, flag3);
		}
		while (true)
		{
			for (int k = 0; k < m_animDrivenVfxList.Count; k++)
			{
				AnimEventToVfxContainer animEventToVfxContainer = m_animDrivenVfxList[k];
				int actorVisible3;
				if (flag)
				{
					actorVisible3 = ((!flag2) ? 1 : 0);
				}
				else
				{
					actorVisible3 = 0;
				}
				animEventToVfxContainer.UpdateVisibilityForSpawnedVfx((byte)actorVisible3 != 0, flag3);
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private void OnDestroy()
	{
		if (m_spawnedCrystalVfxList != null)
		{
			for (int i = 0; i < m_spawnedCrystalVfxList.Count; i++)
			{
				m_spawnedCrystalVfxList[i].DestroyVfx();
			}
			m_spawnedCrystalVfxList.Clear();
		}
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ActorDamaged_Client);
		}
		if (!(m_owner != null))
		{
			return;
		}
		while (true)
		{
			m_owner.OnAnimationEventDelegates -= HandleAnimEvent;
			m_owner.OnTurnStartDelegates -= HandleTurnTick;
			return;
		}
	}

	private int GetNumCrystals(ActorData actor, out int numActive, out int numPartial)
	{
		numActive = 0;
		numPartial = 0;
		if (m_martyrSyncComp != null && m_martyrPassive != null)
		{
			while (true)
			{
				int num;
				switch (4)
				{
				case 0:
					break;
				default:
					{
						numActive = Mathf.Max(0, m_martyrSyncComp.DamageCrystals + m_martyrSyncComp.m_clientCrystalAdjustment);
						numActive = Mathf.Min(numActive, m_martyrPassive.m_maxCrystals);
						num = numActive;
						if (m_martyrPassive.m_crystalGainMode == Passive_Martyr.CrystalGainMode.ByDamageTaken)
						{
							if (m_martyrPassive.m_damageToCrystalConversion > 0f)
							{
								int num2 = Mathf.FloorToInt((float)m_martyrSyncComp.m_clientDamageThisTurn / m_martyrPassive.m_damageToCrystalConversion);
								if (m_martyrPassive.m_maxCrystalsGainedEachTurn > 0 && num2 > m_martyrPassive.m_maxCrystalsGainedEachTurn)
								{
									num2 = m_martyrPassive.m_maxCrystalsGainedEachTurn;
								}
								num = numActive + num2;
								goto IL_0161;
							}
						}
						if (m_martyrPassive.m_crystalGainMode == Passive_Martyr.CrystalGainMode.ByEnergy)
						{
							if (actor != null)
							{
								if (m_martyrPassive.m_energyToCrystalConversion > 0)
								{
									int energyToDisplay = actor.GetEnergyToDisplay();
									int num3 = energyToDisplay / m_martyrPassive.m_energyToCrystalConversion;
									if (num3 > num)
									{
										num = num3;
									}
								}
							}
						}
						goto IL_0161;
					}
					IL_0161:
					num = Mathf.Min(num, m_martyrPassive.m_maxCrystals);
					numPartial = num - numActive;
					return num;
				}
			}
		}
		return 0;
	}

	private void HandleTurnTick()
	{
		for (int i = 0; i < m_animDrivenVfxList.Count; i++)
		{
			if (m_animDrivenVfxList[i].m_turnOffOnTurnStart)
			{
				m_animDrivenVfxList[i].m_shouldShowPersistentVfx = false;
			}
		}
		if (!(m_martyrSyncComp != null))
		{
			return;
		}
		while (true)
		{
			m_martyrSyncComp.m_clientDamageThisTurn = 0;
			return;
		}
	}

	private void HandleAnimEvent(Object eventObj, GameObject sourceObject)
	{
		if (!(m_owner != null))
		{
			return;
		}
		for (int i = 0; i < m_animDrivenVfxList.Count; i++)
		{
			m_animDrivenVfxList[i].HandleAnimEvent(eventObj, sourceObject);
		}
		if (!(m_hideCrystalsAnimEvent != null))
		{
			return;
		}
		while (true)
		{
			if (!(eventObj == m_hideCrystalsAnimEvent))
			{
				return;
			}
			while (true)
			{
				if (m_martyrSyncComp != null)
				{
					while (true)
					{
						m_martyrSyncComp.OnClientCrystalConsumed();
						return;
					}
				}
				return;
			}
		}
	}
}
