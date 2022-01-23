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

	private static readonly int animIdleType;

	private static readonly int animAttack;

	private static readonly int animCinematicCam;

	private static readonly int animStartAttack;

	[SyncVar]
	public int m_maxAfterImageCount = 2;

	private static int kListm_afterImages;

	private static int kRpcRpcTurnToPosition;

	private static int kRpcRpcSetPose;

	private static int kRpcRpcFreezeActor;

	private static int kRpcRpcUnfreezeActor;

	public int Networkm_maxAfterImageCount
	{
		get
		{
			return m_maxAfterImageCount;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_maxAfterImageCount, 2u);
		}
	}

	static TricksterAfterImageNetworkBehaviour()
	{
		animIdleType = Animator.StringToHash("IdleType");
		animAttack = Animator.StringToHash("Attack");
		animCinematicCam = Animator.StringToHash("CinematicCam");
		animStartAttack = Animator.StringToHash("StartAttack");
		kRpcRpcTurnToPosition = -535042778;
		NetworkBehaviour.RegisterRpcDelegate(typeof(TricksterAfterImageNetworkBehaviour), kRpcRpcTurnToPosition, InvokeRpcRpcTurnToPosition);
		kRpcRpcSetPose = -691096658;
		NetworkBehaviour.RegisterRpcDelegate(typeof(TricksterAfterImageNetworkBehaviour), kRpcRpcSetPose, InvokeRpcRpcSetPose);
		kRpcRpcFreezeActor = 2107190201;
		NetworkBehaviour.RegisterRpcDelegate(typeof(TricksterAfterImageNetworkBehaviour), kRpcRpcFreezeActor, InvokeRpcRpcFreezeActor);
		kRpcRpcUnfreezeActor = -239134528;
		NetworkBehaviour.RegisterRpcDelegate(typeof(TricksterAfterImageNetworkBehaviour), kRpcRpcUnfreezeActor, InvokeRpcRpcUnfreezeActor);
		kListm_afterImages = 660840369;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(TricksterAfterImageNetworkBehaviour), kListm_afterImages, InvokeSyncListm_afterImages);
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
		if (!(HighlightUtils.Get() != null) || !(m_targeterFreePosMaxRange > 0f))
		{
			return;
		}
		while (true)
		{
			m_rangeIndicatorObj = HighlightUtils.Get().CreateDynamicConeMesh(m_targeterFreePosMaxRange, 360f, true);
			object obj;
			if ((bool)m_rangeIndicatorObj)
			{
				obj = m_rangeIndicatorObj.GetComponent<UIDynamicCone>();
			}
			else
			{
				obj = null;
			}
			UIDynamicCone uIDynamicCone = (UIDynamicCone)obj;
			if (uIDynamicCone != null)
			{
				while (true)
				{
					HighlightUtils.Get().AdjustDynamicConeMesh(m_rangeIndicatorObj, m_targeterFreePosMaxRange, 360f);
					uIDynamicCone.SetConeObjectActive(false);
					return;
				}
			}
			return;
		}
	}

	private void OnDestroy()
	{
		GameFlowData.s_onActiveOwnedActorChange -= OnActiveOwnedActorChange;
	}

	private void OnActiveOwnedActorChange(ActorData activeActor)
	{
		if (!(activeActor != null))
		{
			return;
		}
		while (true)
		{
			if (!(m_owner != null))
			{
				return;
			}
			while (true)
			{
				for (int i = 0; i < m_afterImages.Count; i++)
				{
					int actorIndex = m_afterImages[i];
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
					if (actorData != null && actorData.GetActorModelData() != null)
					{
						InitializeAfterImageMaterial(actorData.GetActorModelData(), activeActor.GetTeam() == m_owner.GetTeam(), m_afterImageAlpha, m_afterImageShader, true);
					}
				}
				return;
			}
		}
	}

	public void CalcTargetingCenterAndAimAtPos(Vector3 inputFreePos, ActorData caster, List<ActorData> allTargetingActors, bool useCasterSquareAtResolveStart, out Vector3 centerOfFreePosLimit, out Vector3 freePosForAim)
	{
		freePosForAim = inputFreePos;
		centerOfFreePosLimit = CalcTargetingFreePosCenter(caster, allTargetingActors, useCasterSquareAtResolveStart);
		float num = m_targeterFreePosMaxRange * Board.Get().squareSize;
		if (!(num > 0f))
		{
			return;
		}
		while (true)
		{
			Vector3 vector = inputFreePos - centerOfFreePosLimit;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (magnitude > num)
			{
				while (true)
				{
					Vector3 normalized = vector.normalized;
					freePosForAim = centerOfFreePosLimit + normalized * num;
					return;
				}
			}
			return;
		}
	}

	public Vector3 CalcTargetingFreePosCenter(ActorData caster, List<ActorData> allTargetingActors, bool useSquareAtResolveStart)
	{
		Vector3 result = caster.GetFreePos();
		if (m_targeterFreePosUseAvgPos)
		{
			Vector3 zero = Vector3.zero;
			using (List<ActorData>.Enumerator enumerator = allTargetingActors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					zero += current.GetFreePos();
				}
			}
			zero /= (float)allTargetingActors.Count;
			zero.y = result.y;
			result = zero;
		}
		return result;
	}

	private bool ShouldShowRangeIndicator()
	{
		bool result = false;
		if (NetworkClient.active)
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
			{
				if (m_owner == activeOwnedActorData)
				{
					ActorTurnSM actorTurnSM = m_owner.GetActorTurnSM();
					if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
					{
						AbilityData abilityData = m_owner.GetAbilityData();
						object obj;
						if ((bool)abilityData)
						{
							obj = abilityData.GetLastSelectedAbility();
						}
						else
						{
							obj = null;
						}
						Ability ability = (Ability)obj;
						if (ability != null)
						{
							if (!(ability is TricksterBasicAttack))
							{
								if (!(ability is TricksterCones))
								{
									goto IL_00e7;
								}
							}
							result = true;
						}
					}
				}
			}
		}
		goto IL_00e7;
		IL_00e7:
		return result;
	}

	private void Update()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		GameFlowData gameFlowData = GameFlowData.Get();
		if (m_rangeIndicatorObj != null)
		{
			bool flag = ShouldShowRangeIndicator();
			if (flag)
			{
				if (!m_rangeIndicatorObj.activeSelf)
				{
					m_rangeIndicatorObj.SetActive(true);
					List<ActorData> list = new List<ActorData>();
					list.Add(m_owner);
					list.AddRange(GetValidAfterImages());
					Vector3 position = CalcTargetingFreePosCenter(m_owner, list, false);
					position.y = HighlightUtils.GetHighlightHeight();
					m_rangeIndicatorObj.transform.position = position;
					goto IL_00f7;
				}
			}
			if (!flag)
			{
				if (m_rangeIndicatorObj.activeSelf)
				{
					m_rangeIndicatorObj.SetActive(false);
				}
			}
		}
		goto IL_00f7;
		IL_00f7:
		if (m_timeToEndCheck > 0f)
		{
			if (Time.time < m_timeToEndCheck)
			{
				for (int num = m_actorIndexToHideForClient.Count - 1; num >= 0; num--)
				{
					int actorIndex = m_actorIndexToHideForClient[num];
					ActorData actorData = gameFlowData.FindActorByActorIndex(actorIndex);
					if (actorData != null)
					{
						HandleHideClone(actorData);
						m_actorIndexToHideForClient.RemoveAt(num);
					}
				}
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
		if (m_timeToEndCheck > 0f && Time.time >= m_timeToEndCheck)
		{
			m_timeToEndCheck = -1f;
		}
	}

	[ClientRpc]
	public void RpcTurnToPosition(int actorIndex, Vector3 position)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (NetworkClient.active)
			{
				ActorData afterImageByActorIndex = GetAfterImageByActorIndex(actorIndex);
				if (afterImageByActorIndex != null)
				{
					while (true)
					{
						afterImageByActorIndex.TurnToPosition(position);
						return;
					}
				}
				return;
			}
			return;
		}
	}

	[ClientRpc]
	public void RpcSetPose(int actorIndex, Vector3 position, Vector3 forward, bool enableRenderer)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (!NetworkClient.active)
			{
				return;
			}
			while (true)
			{
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				if (!(actorData != null))
				{
					return;
				}
				while (true)
				{
					actorData.transform.position = position;
					actorData.transform.rotation = Quaternion.LookRotation(forward);
					if (!(actorData.GetActorModelData() != null))
					{
						return;
					}
					while (true)
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
						return;
					}
				}
			}
		}
	}

	[ClientRpc]
	public void RpcFreezeActor(int actorIndex)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (!NetworkClient.active)
			{
				return;
			}
			while (true)
			{
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				if (actorData != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							HandleHideClone(actorData);
							return;
						}
					}
				}
				m_actorIndexToHideForClient.Add(actorIndex);
				if (m_timeToEndCheck < 0f)
				{
					m_timeToEndCheck = Time.time + 3f;
				}
				return;
			}
		}
	}

	private void HandleHideClone(ActorData actor)
	{
		if (actor != null)
		{
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
	}

	[ClientRpc]
	public void RpcUnfreezeActor(int actorIndex, int unfreezeAnimIndex)
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (!NetworkClient.active)
			{
				return;
			}
			while (true)
			{
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				if (!(actorData != null))
				{
					return;
				}
				while (true)
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
					int num;
					if (GameFlowData.Get().LocalPlayerData != null)
					{
						num = (int)GameFlowData.Get().LocalPlayerData.GetTeamViewing();
					}
					else
					{
						num = -1;
					}
					Team team = (Team)num;
					if (!(actorData.GetActorModelData() != null))
					{
						return;
					}
					while (true)
					{
						if (m_owner != null)
						{
							bool sameTeamAsClientActor = team == m_owner.GetTeam();
							InitializeAfterImageMaterial(actorData.GetActorModelData(), sameTeamAsClientActor, m_afterImageAlpha, m_afterImageShader, true);
						}
						return;
					}
				}
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
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					actorModelData.SetMaterialKeywordOnAllCachedMaterials("_EMISSIONNOISEON_NONE", false);
					actorModelData.SetMaterialKeywordOnAllCachedMaterials("_EMISSIONNOISEON_ENEMY", false);
					actorModelData.SetMaterialKeywordOnAllCachedMaterials("_EMISSIONNOISEON_FRIEND", true);
					return;
				}
			}
		}
		actorModelData.SetMaterialKeywordOnAllCachedMaterials("_EMISSIONNOISEON_NONE", false);
		actorModelData.SetMaterialKeywordOnAllCachedMaterials("_EMISSIONNOISEON_FRIEND", false);
		actorModelData.SetMaterialKeywordOnAllCachedMaterials("_EMISSIONNOISEON_ENEMY", true);
	}

	internal static void DisableAfterImageMaterial(ActorModelData actorModelData)
	{
		if (!(actorModelData != null))
		{
			return;
		}
		while (true)
		{
			actorModelData.RestoreDefaultRendererAlphas();
			actorModelData.EnableMaterialKeyword("_EMISSIONNOISEON_NONE");
			actorModelData.DisableMaterialKeyword("_EMISSIONNOISEON_ENEMY");
			actorModelData.DisableMaterialKeyword("_EMISSIONNOISEON_FRIEND");
			actorModelData.ResetMaterialsToDefaults();
			return;
		}
	}

	internal static void SetMaterialEnabledForAfterImage(ActorData realTrickster, ActorData afterImage, bool desiredEnable)
	{
		if (!(afterImage != null))
		{
			return;
		}
		int num;
		if (GameFlowData.Get().LocalPlayerData != null)
		{
			num = (int)GameFlowData.Get().LocalPlayerData.GetTeamViewing();
		}
		else
		{
			num = -1;
		}
		Team team = (Team)num;
		if (desiredEnable)
		{
			if (!(afterImage.GetActorModelData() != null))
			{
				return;
			}
			while (true)
			{
				if (!(realTrickster != null))
				{
					return;
				}
				while (true)
				{
					bool sameTeamAsClientActor = team == realTrickster.GetTeam();
					TricksterAfterImageNetworkBehaviour component = realTrickster.GetComponent<TricksterAfterImageNetworkBehaviour>();
					if (component != null)
					{
						InitializeAfterImageMaterial(afterImage.GetActorModelData(), sameTeamAsClientActor, component.m_afterImageAlpha, component.m_afterImageShader, true);
					}
					return;
				}
			}
		}
		DisableAfterImageMaterial(afterImage.GetActorModelData());
	}

	public ActorData GetAfterImageByActorIndex(int actorIndex)
	{
		IEnumerator<int> enumerator = m_afterImages.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				int current = enumerator.Current;
				if (current == actorIndex)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return GameFlowData.Get().FindActorByActorIndex(current);
						}
					}
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_0056;
					}
				}
			}
			end_IL_0056:;
		}
		return null;
	}

	public List<ActorData> GetValidAfterImages(bool requireCurrentSquare = true)
	{
		List<ActorData> list = new List<ActorData>();
		using (IEnumerator<int> enumerator = m_afterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int current = enumerator.Current;
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(current);
				if (actorData != null)
				{
					if (actorData.gameObject.activeSelf)
					{
						if (!actorData.IsDead())
						{
							if (!requireCurrentSquare || actorData.GetCurrentBoardSquare() != null)
							{
								list.Add(actorData);
							}
						}
					}
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
	}

	public bool HasVaidAfterImages()
	{
		IEnumerator<int> enumerator = m_afterImages.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				int current = enumerator.Current;
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(current);
				if (actorData != null)
				{
					if (actorData.gameObject.activeSelf)
					{
						if (!actorData.IsDead())
						{
							if (actorData.GetCurrentBoardSquare() != null)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										return true;
									}
								}
							}
						}
					}
				}
			}
		}
		finally
		{
			if (enumerator != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						enumerator.Dispose();
						goto end_IL_00aa;
					}
				}
			}
			end_IL_00aa:;
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
		else
		{
			if (NetworkServer.active)
			{
				return;
			}
			while (true)
			{
				if (actor != null)
				{
					while (true)
					{
						actor.TurnToPosition(position);
						return;
					}
				}
				return;
			}
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_afterImages(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Debug.LogError("SyncList m_afterImages called on server.");
					return;
				}
			}
		}
		((TricksterAfterImageNetworkBehaviour)obj).m_afterImages.HandleMsg(reader);
	}

	protected static void InvokeRpcRpcTurnToPosition(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC RpcTurnToPosition called on server.");
					return;
				}
			}
		}
		((TricksterAfterImageNetworkBehaviour)obj).RpcTurnToPosition((int)reader.ReadPackedUInt32(), reader.ReadVector3());
	}

	protected static void InvokeRpcRpcSetPose(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC RpcSetPose called on server.");
					return;
				}
			}
		}
		((TricksterAfterImageNetworkBehaviour)obj).RpcSetPose((int)reader.ReadPackedUInt32(), reader.ReadVector3(), reader.ReadVector3(), reader.ReadBoolean());
	}

	protected static void InvokeRpcRpcFreezeActor(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("RPC RpcFreezeActor called on server.");
		}
		else
		{
			((TricksterAfterImageNetworkBehaviour)obj).RpcFreezeActor((int)reader.ReadPackedUInt32());
		}
	}

	protected static void InvokeRpcRpcUnfreezeActor(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC RpcUnfreezeActor called on server.");
					return;
				}
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC Function RpcSetPose called on client.");
					return;
				}
			}
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
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Debug.LogError("RPC Function RpcFreezeActor called on client.");
					return;
				}
			}
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					SyncListInt.WriteInstance(writer, m_afterImages);
					writer.WritePackedUInt32((uint)m_maxAfterImageCount);
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
			SyncListInt.WriteInstance(writer, m_afterImages);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_maxAfterImageCount);
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
