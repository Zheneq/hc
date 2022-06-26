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
		if (m_martyrSyncComp != null
			&& eventType == GameEventManager.EventType.ActorDamaged_Client
			&& args is GameEventManager.ActorHitHealthChangeArgs)
		{
			GameEventManager.ActorHitHealthChangeArgs actorHitHealthChangeArgs = args as GameEventManager.ActorHitHealthChangeArgs;
			if (m_owner == actorHitHealthChangeArgs.m_target)
			{
				m_martyrSyncComp.m_clientDamageThisTurn += actorHitHealthChangeArgs.m_amount;
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
			foreach (AnimEventToAttachedVfxData animEventToAttachedVfxData in m_animEventToVfxDataList)
			{
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
			if (gameObject != null)
			{
				AttachedActorVFXInfo attachedActorVFXInfo2 = new AttachedActorVFXInfo(gameObject, base.gameObject, jointToVfx.m_joint, jointToVfx.m_alignToRootOrientation, "MartyrCrystalVfx_Partial_" + jointToVfx.m_name, AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
				if (attachedActorVFXInfo2.HasVfxInstance())
				{
					attachedActorVFXInfo2.SetInstanceLocalPosition(jointToVfx.m_localOffset);
					m_incompleteCrystalVfxList.Add(attachedActorVFXInfo2);
				}
			}
		}
		if (GameEventManager.Get() != null)
		{
			GameEventManager.Get().AddListener(this, GameEventManager.EventType.ActorDamaged_Client);
		}
	}

	private void Update()
	{
		ActorData actorData = m_actorModelData?.m_parentActorData;
		bool isRendered = actorData == null
			|| actorData.IsActorVisibleToClient()
				&& (actorData.GetModelRenderer() == null || actorData.GetModelRenderer().enabled);
		bool isInRagdoll = actorData != null && actorData.IsInRagdoll();
		bool sameTeamAsClientActor = actorData == null
			|| GameFlowData.Get() != null
				&& GameFlowData.Get().activeOwnedActorData != null
				&& GameFlowData.Get().activeOwnedActorData.GetTeam() == actorData.GetTeam();
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
		for (int i = 0; i < m_spawnedCrystalVfxList.Count; i++)
		{
			bool actorVisible = isRendered && !isInRagdoll && i < numActive;
			m_spawnedCrystalVfxList[i].UpdateVisibility(actorVisible, sameTeamAsClientActor);
		}
		for (int j = 0; j < m_incompleteCrystalVfxList.Count; j++)
		{
			bool flag4 = j >= numActive && j < numActive + numPartial;
			AttachedActorVFXInfo attachedActorVFXInfo2 = m_incompleteCrystalVfxList[j];
			bool actorVisible = isRendered && !isInRagdoll && flag4;
			attachedActorVFXInfo2.UpdateVisibility(actorVisible, sameTeamAsClientActor);
		}
		foreach (AnimEventToVfxContainer container in m_animDrivenVfxList)
		{
			bool actorVisible = isRendered && !isInRagdoll;
			container.UpdateVisibilityForSpawnedVfx(actorVisible, sameTeamAsClientActor);
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
		if (m_owner != null)
		{
			m_owner.OnAnimationEventDelegates -= HandleAnimEvent;
			m_owner.OnTurnStartDelegates -= HandleTurnTick;
		}
	}

	private int GetNumCrystals(ActorData actor, out int numActive, out int numPartial)
	{
		numActive = 0;
		numPartial = 0;
		if (m_martyrSyncComp == null || m_martyrPassive == null)
		{
			return 0;
		}
		numActive = Mathf.Max(0, m_martyrSyncComp.DamageCrystals + m_martyrSyncComp.m_clientCrystalAdjustment);
		numActive = Mathf.Min(numActive, m_martyrPassive.m_maxCrystals);
		int result = numActive;
		if (m_martyrPassive.m_crystalGainMode == Passive_Martyr.CrystalGainMode.ByDamageTaken
			&& m_martyrPassive.m_damageToCrystalConversion > 0f)
		{
			int crystalGainedThisTurn = Mathf.FloorToInt(m_martyrSyncComp.m_clientDamageThisTurn / m_martyrPassive.m_damageToCrystalConversion);
			if (m_martyrPassive.m_maxCrystalsGainedEachTurn > 0 && crystalGainedThisTurn > m_martyrPassive.m_maxCrystalsGainedEachTurn)
			{
				crystalGainedThisTurn = m_martyrPassive.m_maxCrystalsGainedEachTurn;
			}
			result = numActive + crystalGainedThisTurn;
		}
		else if (m_martyrPassive.m_crystalGainMode == Passive_Martyr.CrystalGainMode.ByEnergy
			&& actor != null
			&& m_martyrPassive.m_energyToCrystalConversion > 0)
		{
			int energyToDisplay = actor.GetTechPointsToDisplay();
			int crystalsByEnergy = energyToDisplay / m_martyrPassive.m_energyToCrystalConversion;
			if (crystalsByEnergy > result)
			{
				result = crystalsByEnergy;
			}
		}
		result = Mathf.Min(result, m_martyrPassive.m_maxCrystals);
		numPartial = result - numActive;
		return result;
	}

	private void HandleTurnTick()
	{
		foreach (AnimEventToVfxContainer container in m_animDrivenVfxList)
		{
			if (container.m_turnOffOnTurnStart)
			{
				container.m_shouldShowPersistentVfx = false;
			}
		}
		if (m_martyrSyncComp != null)
		{
			m_martyrSyncComp.m_clientDamageThisTurn = 0;
		}
	}

	private void HandleAnimEvent(Object eventObj, GameObject sourceObject)
	{
		if (m_owner == null)
		{
			return;
		}
		foreach (AnimEventToVfxContainer container in m_animDrivenVfxList)
		{
			container.HandleAnimEvent(eventObj, sourceObject);
		}
		if (m_hideCrystalsAnimEvent != null
			&& eventObj == m_hideCrystalsAnimEvent
			&& m_martyrSyncComp != null)
		{
			m_martyrSyncComp.OnClientCrystalConsumed();
		}
	}
}
