using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class TricksterAfterImageNetworkBehaviour : NetworkBehaviour
{
	public float m_targeterFreePosMaxRange = 10f;
	public bool m_targeterFreePosUseAvgPos;
	public float m_afterImageAlpha = 0.5f;
	public Shader m_afterImageShader;
	public SyncListInt m_afterImages = new SyncListInt();
	
	private ActorData m_owner;
	private GameObject m_rangeIndicatorObj;
	private List<int> m_actorIndexToHideForClient = new List<int>();
	private float m_timeToEndCheck = -1f;

	private const string c_materialKeywordAfterImageNone = "_EMISSIONNOISEON_NONE";
	private const string c_materialKeywordAfterImageFriend = "_EMISSIONNOISEON_FRIEND";
	private const string c_materialKeywordAfterImageEnemy = "_EMISSIONNOISEON_ENEMY";

	private static readonly int animIdleType = Animator.StringToHash("IdleType");
	private static readonly int animAttack = Animator.StringToHash("Attack");
	private static readonly int animCinematicCam = Animator.StringToHash("CinematicCam");
	private static readonly int animStartAttack = Animator.StringToHash("StartAttack");

	[SyncVar]
	public int m_maxAfterImageCount = 2;

	private static int kListm_afterImages = 660840369;
	private static int kRpcRpcTurnToPosition = -535042778;
	private static int kRpcRpcSetPose = -691096658;
	private static int kRpcRpcFreezeActor = 2107190201;
	private static int kRpcRpcUnfreezeActor = -239134528;

	public int Networkm_maxAfterImageCount
	{
		get => m_maxAfterImageCount;
		[param: In]
		set => SetSyncVar(value, ref m_maxAfterImageCount, 2u);
	}

	static TricksterAfterImageNetworkBehaviour()
	{
		RegisterRpcDelegate(typeof(TricksterAfterImageNetworkBehaviour), kRpcRpcTurnToPosition, InvokeRpcRpcTurnToPosition);
		RegisterRpcDelegate(typeof(TricksterAfterImageNetworkBehaviour), kRpcRpcSetPose, InvokeRpcRpcSetPose);
		RegisterRpcDelegate(typeof(TricksterAfterImageNetworkBehaviour), kRpcRpcFreezeActor, InvokeRpcRpcFreezeActor);
		RegisterRpcDelegate(typeof(TricksterAfterImageNetworkBehaviour), kRpcRpcUnfreezeActor, InvokeRpcRpcUnfreezeActor);
		RegisterSyncListDelegate(typeof(TricksterAfterImageNetworkBehaviour), kListm_afterImages, InvokeSyncListm_afterImages);
		NetworkCRC.RegisterBehaviour("TricksterAfterImageNetworkBehaviour", 0);
	}

	public int GetMaxAfterImageCount()
	{
		return m_maxAfterImageCount;
	}

	private void Start()
	{
		m_owner = GetComponent<ActorData>();
		GameFlowData.s_onActiveOwnedActorChange += OnActiveOwnedActorChange;
		if (HighlightUtils.Get() != null && m_targeterFreePosMaxRange > 0f)
		{
			m_rangeIndicatorObj = HighlightUtils.Get().CreateDynamicConeMesh(m_targeterFreePosMaxRange, 360f, true);
			UIDynamicCone uIDynamicCone = m_rangeIndicatorObj != null
				? m_rangeIndicatorObj.GetComponent<UIDynamicCone>()
				: null;
			if (uIDynamicCone != null)
			{
				HighlightUtils.Get().AdjustDynamicConeMesh(m_rangeIndicatorObj, m_targeterFreePosMaxRange, 360f);
				uIDynamicCone.SetConeObjectActive(false);
			}
		}
	}

	private void OnDestroy()
	{
		GameFlowData.s_onActiveOwnedActorChange -= OnActiveOwnedActorChange;
	}

	private void OnActiveOwnedActorChange(ActorData activeActor)
	{
		if (activeActor == null || m_owner == null)
		{
			return;
		}
		foreach (int actorIndex in m_afterImages)
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
			if (actorData != null && actorData.GetActorModelData() != null)
			{
				InitializeAfterImageMaterial(
					actorData.GetActorModelData(),
					activeActor.GetTeam() == m_owner.GetTeam(),
					m_afterImageAlpha,
					m_afterImageShader,
					true);
			}
		}
	}

	public void CalcTargetingCenterAndAimAtPos(
		Vector3 inputFreePos,
		ActorData caster,
		List<ActorData> allTargetingActors,
		bool useCasterSquareAtResolveStart,
		out Vector3 centerOfFreePosLimit,
		out Vector3 freePosForAim)
	{
		freePosForAim = inputFreePos;
		centerOfFreePosLimit = CalcTargetingFreePosCenter(caster, allTargetingActors, useCasterSquareAtResolveStart);
		float num = m_targeterFreePosMaxRange * Board.Get().squareSize;
		if (num > 0f)
		{
			Vector3 vector = inputFreePos - centerOfFreePosLimit;
			vector.y = 0f;
			if (vector.magnitude > num)
			{
				Vector3 normalized = vector.normalized;
				freePosForAim = centerOfFreePosLimit + normalized * num;
			}
		}
	}

	public Vector3 CalcTargetingFreePosCenter(ActorData caster, List<ActorData> allTargetingActors, bool useSquareAtResolveStart)
	{
		Vector3 result = caster.GetFreePos();
		if (m_targeterFreePosUseAvgPos)
		{
			Vector3 zero = Vector3.zero;
			foreach (ActorData actor in allTargetingActors)
			{
				zero += actor.GetFreePos();
			}
			zero /= allTargetingActors.Count;
			zero.y = result.y;
			result = zero;
		}
		return result;
	}

	private bool ShouldShowRangeIndicator()
	{
		if (!NetworkClient.active)
		{
			return false;
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null
		    && m_owner == activeOwnedActorData
		    && m_owner.GetActorTurnSM().CurrentState == TurnStateEnum.TARGETING_ACTION)
		{
			AbilityData abilityData = m_owner.GetAbilityData();
			Ability ability = abilityData != null
				? abilityData.GetLastSelectedAbility()
				: null;
			return ability != null && (ability is TricksterBasicAttack || ability is TricksterCones);
		}
		return false;
	}

	private void Update()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		if (m_rangeIndicatorObj != null)
		{
			bool flag = ShouldShowRangeIndicator();
			if (flag && !m_rangeIndicatorObj.activeSelf)
			{
				m_rangeIndicatorObj.SetActive(true);
				List<ActorData> actorDatas = new List<ActorData>();
				actorDatas.Add(m_owner);
				actorDatas.AddRange(GetValidAfterImages());
				Vector3 position = CalcTargetingFreePosCenter(m_owner, actorDatas, false);
				position.y = HighlightUtils.GetHighlightHeight();
				m_rangeIndicatorObj.transform.position = position;
			}
			else if (!flag && m_rangeIndicatorObj.activeSelf)
			{
				m_rangeIndicatorObj.SetActive(false);
			}
		}
		if (m_timeToEndCheck > 0f && Time.time < m_timeToEndCheck)
		{
			for (int i = m_actorIndexToHideForClient.Count - 1; i >= 0; i--)
			{
				int actorIndex = m_actorIndexToHideForClient[i];
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				if (actorData != null)
				{
					HandleHideClone(actorData);
					m_actorIndexToHideForClient.RemoveAt(i);
				}
			}
		}
		else if (m_timeToEndCheck > 0f && Time.time >= m_timeToEndCheck)
		{
			m_timeToEndCheck = -1f;
		}
	}

	[ClientRpc]
	public void RpcTurnToPosition(int actorIndex, Vector3 position)
	{
		if (NetworkServer.active || !NetworkClient.active)
		{
			return;
		}
		ActorData afterImageByActorIndex = GetAfterImageByActorIndex(actorIndex);
		if (afterImageByActorIndex != null)
		{
			afterImageByActorIndex.TurnToPosition(position);
		}
	}

	[ClientRpc]
	public void RpcSetPose(int actorIndex, Vector3 position, Vector3 forward, bool enableRenderer)
	{
		if (NetworkServer.active || !NetworkClient.active)
		{
			return;
		}
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
		if (actorData != null)
		{
			actorData.transform.position = position;
			actorData.transform.rotation = Quaternion.LookRotation(forward);
			if (actorData.GetActorModelData() != null)
			{
				if (enableRenderer)
				{
					actorData.GetActorModelData().EnableRendererAndUpdateVisibility();
				}
				else
				{
					actorData.GetActorModelData().DisableAndHideRenderers();
				}
				SetMaterialEnabledForAfterImage(m_owner, actorData, enableRenderer);
			}
		}
	}

	[ClientRpc]
	public void RpcFreezeActor(int actorIndex)
	{
		if (NetworkServer.active || !NetworkClient.active)
		{
			return;
		}
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
		if (actorData != null)
		{
			HandleHideClone(actorData);
		}
		else
		{
			m_actorIndexToHideForClient.Add(actorIndex);
			if (m_timeToEndCheck < 0f)
			{
				m_timeToEndCheck = Time.time + 3f;
			}
		}
	}

	private void HandleHideClone(ActorData actor)
	{
		if (actor == null)
		{
			return;
		}
		if (actor.GetCurrentBoardSquare() != null)
		{
			actor.UnoccupyCurrentBoardSquare();
			actor.ClearCurrentBoardSquare();
		}
		actor.transform.position = new Vector3(10000f, -100f, 10000f);
		actor.GetActorModelData().DisableAndHideRenderers();
		actor.SetNameplateAlwaysInvisible(true);
		actor.IgnoreForEnergyOnHit = true;
		actor.IgnoreForAbilityHits = true;
	}

	[ClientRpc]
	public void RpcUnfreezeActor(int actorIndex, int unfreezeAnimIndex)
	{
		if (NetworkServer.active || !NetworkClient.active)
		{
			return;
		}
		ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
		if (actorData != null)
		{
			actorData.EnableRendererAndUpdateVisibility();
			Animator modelAnimator = actorData.GetModelAnimator();
			if (modelAnimator != null)
			{
				modelAnimator.SetInteger(animAttack, unfreezeAnimIndex);
				modelAnimator.SetInteger(animIdleType, 0);
				modelAnimator.SetBool(animCinematicCam, false);
				modelAnimator.SetTrigger(animStartAttack);
			}

			Team team = GameFlowData.Get().LocalPlayerData != null
				? GameFlowData.Get().LocalPlayerData.GetTeamViewing()
				: Team.Invalid;
			if (actorData.GetActorModelData() != null && m_owner != null)
			{
				bool sameTeamAsClientActor = team == m_owner.GetTeam();
				InitializeAfterImageMaterial(
					actorData.GetActorModelData(),
					sameTeamAsClientActor,
					m_afterImageAlpha,
					m_afterImageShader, true);
			}
		}
	}

	internal static void InitializeAfterImageMaterial(ActorModelData actorModelData, bool sameTeamAsClientActor, float alpha, Shader shader, bool isDefault)
	{
		actorModelData.CacheDefaultRendererAlphas();
		actorModelData.SetDefaultRendererAlpha(alpha);
		actorModelData.SetMaterialShader(shader, isDefault);
		SetMaterialKeywordsForTeam(actorModelData, sameTeamAsClientActor);
	}

	internal static void SetMaterialKeywordsForTeam(ActorModelData actorModelData, bool sameTeamAsClientActor)
	{
		if (sameTeamAsClientActor)
		{
			actorModelData.SetMaterialKeywordOnAllCachedMaterials(c_materialKeywordAfterImageNone, false);
			actorModelData.SetMaterialKeywordOnAllCachedMaterials(c_materialKeywordAfterImageEnemy, false);
			actorModelData.SetMaterialKeywordOnAllCachedMaterials(c_materialKeywordAfterImageFriend, true);
		}
		else
		{
			actorModelData.SetMaterialKeywordOnAllCachedMaterials(c_materialKeywordAfterImageNone, false);
			actorModelData.SetMaterialKeywordOnAllCachedMaterials(c_materialKeywordAfterImageFriend, false);
			actorModelData.SetMaterialKeywordOnAllCachedMaterials(c_materialKeywordAfterImageEnemy, true);
		}
	}

	internal static void DisableAfterImageMaterial(ActorModelData actorModelData)
	{
		if (actorModelData != null)
		{
			actorModelData.RestoreDefaultRendererAlphas();
			actorModelData.EnableMaterialKeyword(c_materialKeywordAfterImageNone);
			actorModelData.DisableMaterialKeyword(c_materialKeywordAfterImageEnemy);
			actorModelData.DisableMaterialKeyword(c_materialKeywordAfterImageFriend);
			actorModelData.ResetMaterialsToDefaults();
		}
	}

	internal static void SetMaterialEnabledForAfterImage(ActorData realTrickster, ActorData afterImage, bool desiredEnable)
	{
		if (afterImage == null)
		{
			return;
		}
		Team team = GameFlowData.Get().LocalPlayerData != null
			? GameFlowData.Get().LocalPlayerData.GetTeamViewing()
			: Team.Invalid;
		if (desiredEnable)
		{
			if (afterImage.GetActorModelData() != null && realTrickster != null)
			{
				bool sameTeamAsClientActor = team == realTrickster.GetTeam();
				TricksterAfterImageNetworkBehaviour component = realTrickster.GetComponent<TricksterAfterImageNetworkBehaviour>();
				if (component != null)
				{
					InitializeAfterImageMaterial(
						afterImage.GetActorModelData(),
						sameTeamAsClientActor,
						component.m_afterImageAlpha,
						component.m_afterImageShader,
						true);
				}
			}
		}
		else
		{
			DisableAfterImageMaterial(afterImage.GetActorModelData());
		}
	}

	public ActorData GetAfterImageByActorIndex(int actorIndex)
	{
		foreach (int afterImageIndex in m_afterImages)
		{
			if (afterImageIndex == actorIndex)
			{
				return GameFlowData.Get().FindActorByActorIndex(afterImageIndex);
			}
		}
		return null;
	}

	public List<ActorData> GetValidAfterImages(bool requireCurrentSquare = true)
	{
		List<ActorData> list = new List<ActorData>();
		foreach (int afterImageIndex in m_afterImages)
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(afterImageIndex);
			if (actorData != null
			    && actorData.gameObject.activeSelf
			    && !actorData.IsDead()
			    && (!requireCurrentSquare || actorData.GetCurrentBoardSquare() != null))
			{
				list.Add(actorData);
			}
		}
		return list;
	}

	public bool HasVaidAfterImages()
	{
		foreach (int afterImageIndex in m_afterImages)
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(afterImageIndex);
			if (actorData != null
			    && actorData.gameObject.activeSelf
			    && !actorData.IsDead()
			    && actorData.GetCurrentBoardSquare() != null)
			{
				return true;
			}
		}
		return false;
	}

	public void TurnToPosition(ActorData actor, Vector3 position)
	{
		if (NetworkServer.active && actor != null)
		{
			actor.TurnToPosition(position);
			CallRpcTurnToPosition(actor.ActorIndex, position);
		}
		else if (!NetworkServer.active && actor != null)
		{
			actor.TurnToPosition(position);
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_afterImages(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_afterImages called on server.");
			return;
		}
		((TricksterAfterImageNetworkBehaviour)obj).m_afterImages.HandleMsg(reader);
	}

	protected static void InvokeRpcRpcTurnToPosition(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcTurnToPosition called on server.");
			return;
		}
		((TricksterAfterImageNetworkBehaviour)obj).RpcTurnToPosition((int)reader.ReadPackedUInt32(), reader.ReadVector3());
	}

	protected static void InvokeRpcRpcSetPose(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcSetPose called on server.");
			return;
		}
		((TricksterAfterImageNetworkBehaviour)obj).RpcSetPose((int)reader.ReadPackedUInt32(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadBoolean());
	}

	protected static void InvokeRpcRpcFreezeActor(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcFreezeActor called on server.");
			return;
		}
		((TricksterAfterImageNetworkBehaviour)obj).RpcFreezeActor((int)reader.ReadPackedUInt32());
	}

	protected static void InvokeRpcRpcUnfreezeActor(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcUnfreezeActor called on server.");
			return;
		}
		((TricksterAfterImageNetworkBehaviour)obj).RpcUnfreezeActor((int)reader.ReadPackedUInt32(), (int)reader.ReadPackedUInt32());
	}

	public void CallRpcTurnToPosition(int actorIndex, Vector3 position)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcTurnToPosition called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcTurnToPosition);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndex);
		networkWriter.Write(position);
		SendRPCInternal(networkWriter, 0, "RpcTurnToPosition");
	}

	public void CallRpcSetPose(int actorIndex, Vector3 position, Vector3 forward, bool enableRenderer)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcSetPose called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcSetPose);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndex);
		networkWriter.Write(position);
		networkWriter.Write(forward);
		networkWriter.Write(enableRenderer);
		SendRPCInternal(networkWriter, 0, "RpcSetPose");
	}

	public void CallRpcFreezeActor(int actorIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcFreezeActor called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcFreezeActor);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndex);
		SendRPCInternal(networkWriter, 0, "RpcFreezeActor");
	}

	public void CallRpcUnfreezeActor(int actorIndex, int unfreezeAnimIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcUnfreezeActor called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write((short)0);
		networkWriter.Write((short)2);
		networkWriter.WritePackedUInt32((uint)kRpcRpcUnfreezeActor);
		networkWriter.Write(GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndex);
		networkWriter.WritePackedUInt32((uint)unfreezeAnimIndex);
		SendRPCInternal(networkWriter, 0, "RpcUnfreezeActor");
	}

	private void Awake()
	{
		m_afterImages.InitializeBehaviour(this, kListm_afterImages);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			SyncListInt.WriteInstance(writer, m_afterImages);
			writer.WritePackedUInt32((uint)m_maxAfterImageCount);
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
			SyncListInt.WriteInstance(writer, m_afterImages);
		}
		if ((syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_maxAfterImageCount);
		}
		if (!flag)
		{
			writer.WritePackedUInt32(syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListInt.ReadReference(reader, m_afterImages);
			m_maxAfterImageCount = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListInt.ReadReference(reader, m_afterImages);
		}
		if ((num & 2) != 0)
		{
			m_maxAfterImageCount = (int)reader.ReadPackedUInt32();
		}
	}
}
