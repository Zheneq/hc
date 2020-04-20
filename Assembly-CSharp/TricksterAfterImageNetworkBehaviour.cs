using System;
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

	private static int kListm_afterImages;

	private static int kRpcRpcTurnToPosition = -0x1FE41ADA;

	private static int kRpcRpcSetPose;

	private static int kRpcRpcFreezeActor;

	private static int kRpcRpcUnfreezeActor;

	static TricksterAfterImageNetworkBehaviour()
	{
		NetworkBehaviour.RegisterRpcDelegate(typeof(TricksterAfterImageNetworkBehaviour), TricksterAfterImageNetworkBehaviour.kRpcRpcTurnToPosition, new NetworkBehaviour.CmdDelegate(TricksterAfterImageNetworkBehaviour.InvokeRpcRpcTurnToPosition));
		TricksterAfterImageNetworkBehaviour.kRpcRpcSetPose = -0x29314C52;
		NetworkBehaviour.RegisterRpcDelegate(typeof(TricksterAfterImageNetworkBehaviour), TricksterAfterImageNetworkBehaviour.kRpcRpcSetPose, new NetworkBehaviour.CmdDelegate(TricksterAfterImageNetworkBehaviour.InvokeRpcRpcSetPose));
		TricksterAfterImageNetworkBehaviour.kRpcRpcFreezeActor = 0x7D992BB9;
		NetworkBehaviour.RegisterRpcDelegate(typeof(TricksterAfterImageNetworkBehaviour), TricksterAfterImageNetworkBehaviour.kRpcRpcFreezeActor, new NetworkBehaviour.CmdDelegate(TricksterAfterImageNetworkBehaviour.InvokeRpcRpcFreezeActor));
		TricksterAfterImageNetworkBehaviour.kRpcRpcUnfreezeActor = -0xE40E740;
		NetworkBehaviour.RegisterRpcDelegate(typeof(TricksterAfterImageNetworkBehaviour), TricksterAfterImageNetworkBehaviour.kRpcRpcUnfreezeActor, new NetworkBehaviour.CmdDelegate(TricksterAfterImageNetworkBehaviour.InvokeRpcRpcUnfreezeActor));
		TricksterAfterImageNetworkBehaviour.kListm_afterImages = 0x27639FB1;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(TricksterAfterImageNetworkBehaviour), TricksterAfterImageNetworkBehaviour.kListm_afterImages, new NetworkBehaviour.CmdDelegate(TricksterAfterImageNetworkBehaviour.InvokeSyncListm_afterImages));
		NetworkCRC.RegisterBehaviour("TricksterAfterImageNetworkBehaviour", 0);
	}

	public int GetMaxAfterImageCount()
	{
		return this.m_maxAfterImageCount;
	}

	private void Start()
	{
		this.m_owner = base.GetComponent<ActorData>();
		GameFlowData.s_onActiveOwnedActorChange += this.OnActiveOwnedActorChange;
		if (HighlightUtils.Get() != null && this.m_targeterFreePosMaxRange > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.Start()).MethodHandle;
			}
			this.m_rangeIndicatorObj = HighlightUtils.Get().CreateDynamicConeMesh(this.m_targeterFreePosMaxRange, 360f, true, null);
			UIDynamicCone uidynamicCone;
			if (this.m_rangeIndicatorObj)
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
				uidynamicCone = this.m_rangeIndicatorObj.GetComponent<UIDynamicCone>();
			}
			else
			{
				uidynamicCone = null;
			}
			UIDynamicCone uidynamicCone2 = uidynamicCone;
			if (uidynamicCone2 != null)
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
				HighlightUtils.Get().AdjustDynamicConeMesh(this.m_rangeIndicatorObj, this.m_targeterFreePosMaxRange, 360f);
				uidynamicCone2.SetConeObjectActive(false);
			}
		}
	}

	private void OnDestroy()
	{
		GameFlowData.s_onActiveOwnedActorChange -= this.OnActiveOwnedActorChange;
	}

	private void OnActiveOwnedActorChange(ActorData activeActor)
	{
		if (activeActor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.OnActiveOwnedActorChange(ActorData)).MethodHandle;
			}
			if (this.m_owner != null)
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
				for (int i = 0; i < this.m_afterImages.Count; i++)
				{
					int actorIndex = this.m_afterImages[i];
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
					if (actorData != null && actorData.GetActorModelData() != null)
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
						TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(actorData.GetActorModelData(), activeActor.GetTeam() == this.m_owner.GetTeam(), this.m_afterImageAlpha, this.m_afterImageShader, true);
					}
				}
			}
		}
	}

	public unsafe void CalcTargetingCenterAndAimAtPos(Vector3 inputFreePos, ActorData caster, List<ActorData> allTargetingActors, bool useCasterSquareAtResolveStart, out Vector3 centerOfFreePosLimit, out Vector3 freePosForAim)
	{
		freePosForAim = inputFreePos;
		centerOfFreePosLimit = this.CalcTargetingFreePosCenter(caster, allTargetingActors, useCasterSquareAtResolveStart);
		float num = this.m_targeterFreePosMaxRange * Board.Get().squareSize;
		if (num > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.CalcTargetingCenterAndAimAtPos(Vector3, ActorData, List<ActorData>, bool, Vector3*, Vector3*)).MethodHandle;
			}
			Vector3 vector = inputFreePos - centerOfFreePosLimit;
			vector.y = 0f;
			float magnitude = vector.magnitude;
			if (magnitude > num)
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
				Vector3 normalized = vector.normalized;
				freePosForAim = centerOfFreePosLimit + normalized * num;
			}
		}
	}

	public Vector3 CalcTargetingFreePosCenter(ActorData caster, List<ActorData> allTargetingActors, bool useSquareAtResolveStart)
	{
		Vector3 result = caster.GetTravelBoardSquareWorldPosition();
		if (this.m_targeterFreePosUseAvgPos)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.CalcTargetingFreePosCenter(ActorData, List<ActorData>, bool)).MethodHandle;
			}
			Vector3 vector = Vector3.zero;
			using (List<ActorData>.Enumerator enumerator = allTargetingActors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					vector += actorData.GetTravelBoardSquareWorldPosition();
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
			vector /= (float)allTargetingActors.Count;
			vector.y = result.y;
			result = vector;
		}
		return result;
	}

	private bool ShouldShowRangeIndicator()
	{
		bool result = false;
		if (NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.ShouldShowRangeIndicator()).MethodHandle;
			}
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null)
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
				if (this.m_owner == activeOwnedActorData)
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
					ActorTurnSM actorTurnSM = this.m_owner.GetActorTurnSM();
					if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
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
						AbilityData abilityData = this.m_owner.GetAbilityData();
						Ability ability;
						if (abilityData)
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
							ability = abilityData.GetLastSelectedAbility();
						}
						else
						{
							ability = null;
						}
						Ability ability2 = ability;
						if (ability2 != null)
						{
							if (!(ability2 is TricksterBasicAttack))
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
								if (!(ability2 is TricksterCones))
								{
									return result;
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
							result = true;
						}
					}
				}
			}
		}
		return result;
	}

	private void Update()
	{
		if (NetworkClient.active)
		{
			GameFlowData gameFlowData = GameFlowData.Get();
			if (this.m_rangeIndicatorObj != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.Update()).MethodHandle;
				}
				bool flag = this.ShouldShowRangeIndicator();
				if (flag)
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
					if (!this.m_rangeIndicatorObj.activeSelf)
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
						this.m_rangeIndicatorObj.SetActive(true);
						List<ActorData> list = new List<ActorData>();
						list.Add(this.m_owner);
						list.AddRange(this.GetValidAfterImages(true));
						Vector3 position = this.CalcTargetingFreePosCenter(this.m_owner, list, false);
						position.y = HighlightUtils.GetHighlightHeight();
						this.m_rangeIndicatorObj.transform.position = position;
						goto IL_F7;
					}
				}
				if (!flag)
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
					if (this.m_rangeIndicatorObj.activeSelf)
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
						this.m_rangeIndicatorObj.SetActive(false);
					}
				}
			}
			IL_F7:
			if (this.m_timeToEndCheck > 0f)
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
				if (Time.time < this.m_timeToEndCheck)
				{
					for (int i = this.m_actorIndexToHideForClient.Count - 1; i >= 0; i--)
					{
						int actorIndex = this.m_actorIndexToHideForClient[i];
						ActorData actorData = gameFlowData.FindActorByActorIndex(actorIndex);
						if (actorData != null)
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
							this.HandleHideClone(actorData);
							this.m_actorIndexToHideForClient.RemoveAt(i);
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
					return;
				}
			}
			if (this.m_timeToEndCheck > 0f && Time.time >= this.m_timeToEndCheck)
			{
				this.m_timeToEndCheck = -1f;
			}
		}
	}

	[ClientRpc]
	public void RpcTurnToPosition(int actorIndex, Vector3 position)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.RpcTurnToPosition(int, Vector3)).MethodHandle;
			}
			if (NetworkClient.active)
			{
				ActorData afterImageByActorIndex = this.GetAfterImageByActorIndex(actorIndex);
				if (afterImageByActorIndex != null)
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
					afterImageByActorIndex.TurnToPosition(position, 0.2f);
				}
			}
		}
	}

	[ClientRpc]
	public void RpcSetPose(int actorIndex, Vector3 position, Vector3 forward, bool enableRenderer)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.RpcSetPose(int, Vector3, Vector3, bool)).MethodHandle;
			}
			if (NetworkClient.active)
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
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				if (actorData != null)
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
					actorData.transform.position = position;
					actorData.transform.rotation = Quaternion.LookRotation(forward);
					if (actorData.GetActorModelData() != null)
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
						if (enableRenderer)
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
							actorData.GetActorModelData().EnableRendererAndUpdateVisibility();
						}
						else
						{
							actorData.GetActorModelData().DisableAndHideRenderers();
						}
						TricksterAfterImageNetworkBehaviour.SetMaterialEnabledForAfterImage(this.m_owner, actorData, enableRenderer);
					}
				}
			}
		}
	}

	[ClientRpc]
	public void RpcFreezeActor(int actorIndex)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.RpcFreezeActor(int)).MethodHandle;
			}
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
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				if (actorData != null)
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
					this.HandleHideClone(actorData);
				}
				else
				{
					this.m_actorIndexToHideForClient.Add(actorIndex);
					if (this.m_timeToEndCheck < 0f)
					{
						this.m_timeToEndCheck = Time.time + 3f;
					}
				}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.RpcUnfreezeActor(int, int)).MethodHandle;
			}
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
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				if (actorData != null)
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
					actorData.EnableRendererAndUpdateVisibility();
					Animator modelAnimator = actorData.GetModelAnimator();
					if (modelAnimator != null)
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
						modelAnimator.SetInteger(TricksterAfterImageNetworkBehaviour.animAttack, unfreezeAnimIndex);
						modelAnimator.SetInteger(TricksterAfterImageNetworkBehaviour.animIdleType, 0);
						modelAnimator.SetBool(TricksterAfterImageNetworkBehaviour.animCinematicCam, false);
						modelAnimator.SetTrigger(TricksterAfterImageNetworkBehaviour.animStartAttack);
					}
					Team team;
					if (GameFlowData.Get().LocalPlayerData != null)
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
						team = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
					}
					else
					{
						team = Team.Invalid;
					}
					Team team2 = team;
					if (actorData.GetActorModelData() != null)
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
						if (this.m_owner != null)
						{
							bool sameTeamAsClientActor = team2 == this.m_owner.GetTeam();
							TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(actorData.GetActorModelData(), sameTeamAsClientActor, this.m_afterImageAlpha, this.m_afterImageShader, true);
						}
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
		TricksterAfterImageNetworkBehaviour.SetMaterialKeywordsForTeam(actorModelData, sameTeamAsClientActor);
	}

	internal static void SetMaterialKeywordsForTeam(ActorModelData actorModelData, bool sameTeamAsClientActor)
	{
		if (sameTeamAsClientActor)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.SetMaterialKeywordsForTeam(ActorModelData, bool)).MethodHandle;
			}
			actorModelData.SetMaterialKeywordOnAllCachedMaterials("_EMISSIONNOISEON_NONE", false);
			actorModelData.SetMaterialKeywordOnAllCachedMaterials("_EMISSIONNOISEON_ENEMY", false);
			actorModelData.SetMaterialKeywordOnAllCachedMaterials("_EMISSIONNOISEON_FRIEND", true);
		}
		else
		{
			actorModelData.SetMaterialKeywordOnAllCachedMaterials("_EMISSIONNOISEON_NONE", false);
			actorModelData.SetMaterialKeywordOnAllCachedMaterials("_EMISSIONNOISEON_FRIEND", false);
			actorModelData.SetMaterialKeywordOnAllCachedMaterials("_EMISSIONNOISEON_ENEMY", true);
		}
	}

	internal static void DisableAfterImageMaterial(ActorModelData actorModelData)
	{
		if (actorModelData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.DisableAfterImageMaterial(ActorModelData)).MethodHandle;
			}
			actorModelData.RestoreDefaultRendererAlphas();
			actorModelData.EnableMaterialKeyword("_EMISSIONNOISEON_NONE");
			actorModelData.DisableMaterialKeyword("_EMISSIONNOISEON_ENEMY");
			actorModelData.DisableMaterialKeyword("_EMISSIONNOISEON_FRIEND");
			actorModelData.ResetMaterialsToDefaults();
		}
	}

	internal static void SetMaterialEnabledForAfterImage(ActorData realTrickster, ActorData afterImage, bool desiredEnable)
	{
		if (afterImage != null)
		{
			Team team;
			if (GameFlowData.Get().LocalPlayerData != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.SetMaterialEnabledForAfterImage(ActorData, ActorData, bool)).MethodHandle;
				}
				team = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
			}
			else
			{
				team = Team.Invalid;
			}
			Team team2 = team;
			if (desiredEnable)
			{
				if (afterImage.GetActorModelData() != null)
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
					if (realTrickster != null)
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
						bool sameTeamAsClientActor = team2 == realTrickster.GetTeam();
						TricksterAfterImageNetworkBehaviour component = realTrickster.GetComponent<TricksterAfterImageNetworkBehaviour>();
						if (component != null)
						{
							TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(afterImage.GetActorModelData(), sameTeamAsClientActor, component.m_afterImageAlpha, component.m_afterImageShader, true);
						}
					}
				}
			}
			else
			{
				TricksterAfterImageNetworkBehaviour.DisableAfterImageMaterial(afterImage.GetActorModelData());
			}
		}
	}

	public ActorData GetAfterImageByActorIndex(int actorIndex)
	{
		IEnumerator<int> enumerator = this.m_afterImages.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				int num = enumerator.Current;
				if (num == actorIndex)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.GetAfterImageByActorIndex(int)).MethodHandle;
					}
					return GameFlowData.Get().FindActorByActorIndex(num);
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
		finally
		{
			if (enumerator != null)
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
				enumerator.Dispose();
			}
		}
		return null;
	}

	public List<ActorData> GetValidAfterImages(bool requireCurrentSquare = true)
	{
		List<ActorData> list = new List<ActorData>();
		using (IEnumerator<int> enumerator = this.m_afterImages.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				int actorIndex = enumerator.Current;
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				if (actorData != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.GetValidAfterImages(bool)).MethodHandle;
					}
					if (actorData.gameObject.activeSelf)
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
						if (!actorData.IsDead())
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
							if (!requireCurrentSquare || actorData.GetCurrentBoardSquare() != null)
							{
								list.Add(actorData);
							}
						}
					}
				}
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
		return list;
	}

	public bool HasVaidAfterImages()
	{
		IEnumerator<int> enumerator = this.m_afterImages.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				int actorIndex = enumerator.Current;
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				if (actorData != null)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.HasVaidAfterImages()).MethodHandle;
					}
					if (actorData.gameObject.activeSelf)
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
						if (!actorData.IsDead())
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
							if (actorData.GetCurrentBoardSquare() != null)
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
								return true;
							}
						}
					}
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
		finally
		{
			if (enumerator != null)
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
				enumerator.Dispose();
			}
		}
		return false;
	}

	public void TurnToPosition(ActorData actor, Vector3 position)
	{
		if (NetworkServer.active && actor != null)
		{
			actor.TurnToPosition(position, 0.2f);
			this.CallRpcTurnToPosition(actor.ActorIndex, position);
		}
		else if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.TurnToPosition(ActorData, Vector3)).MethodHandle;
			}
			if (actor != null)
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
				actor.TurnToPosition(position, 0.2f);
			}
		}
	}

	private void UNetVersion()
	{
	}

	public int Networkm_maxAfterImageCount
	{
		get
		{
			return this.m_maxAfterImageCount;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_maxAfterImageCount, 2U);
		}
	}

	protected static void InvokeSyncListm_afterImages(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.InvokeSyncListm_afterImages(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("SyncList m_afterImages called on server.");
			return;
		}
		((TricksterAfterImageNetworkBehaviour)obj).m_afterImages.HandleMsg(reader);
	}

	protected static void InvokeRpcRpcTurnToPosition(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.InvokeRpcRpcTurnToPosition(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
			Debug.LogError("RPC RpcTurnToPosition called on server.");
			return;
		}
		((TricksterAfterImageNetworkBehaviour)obj).RpcTurnToPosition((int)reader.ReadPackedUInt32(), reader.ReadVector3());
	}

	protected static void InvokeRpcRpcSetPose(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.InvokeRpcRpcSetPose(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.InvokeRpcRpcUnfreezeActor(NetworkBehaviour, NetworkReader)).MethodHandle;
			}
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
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)TricksterAfterImageNetworkBehaviour.kRpcRpcTurnToPosition);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndex);
		networkWriter.Write(position);
		this.SendRPCInternal(networkWriter, 0, "RpcTurnToPosition");
	}

	public void CallRpcSetPose(int actorIndex, Vector3 position, Vector3 forward, bool enableRenderer)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.CallRpcSetPose(int, Vector3, Vector3, bool)).MethodHandle;
			}
			Debug.LogError("RPC Function RpcSetPose called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)TricksterAfterImageNetworkBehaviour.kRpcRpcSetPose);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndex);
		networkWriter.Write(position);
		networkWriter.Write(forward);
		networkWriter.Write(enableRenderer);
		this.SendRPCInternal(networkWriter, 0, "RpcSetPose");
	}

	public void CallRpcFreezeActor(int actorIndex)
	{
		if (!NetworkServer.active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.CallRpcFreezeActor(int)).MethodHandle;
			}
			Debug.LogError("RPC Function RpcFreezeActor called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)TricksterAfterImageNetworkBehaviour.kRpcRpcFreezeActor);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndex);
		this.SendRPCInternal(networkWriter, 0, "RpcFreezeActor");
	}

	public void CallRpcUnfreezeActor(int actorIndex, int unfreezeAnimIndex)
	{
		if (!NetworkServer.active)
		{
			Debug.LogError("RPC Function RpcUnfreezeActor called on client.");
			return;
		}
		NetworkWriter networkWriter = new NetworkWriter();
		networkWriter.Write(0);
		networkWriter.Write((short)((ushort)2));
		networkWriter.WritePackedUInt32((uint)TricksterAfterImageNetworkBehaviour.kRpcRpcUnfreezeActor);
		networkWriter.Write(base.GetComponent<NetworkIdentity>().netId);
		networkWriter.WritePackedUInt32((uint)actorIndex);
		networkWriter.WritePackedUInt32((uint)unfreezeAnimIndex);
		this.SendRPCInternal(networkWriter, 0, "RpcUnfreezeActor");
	}

	private void Awake()
	{
		this.m_afterImages.InitializeBehaviour(this, TricksterAfterImageNetworkBehaviour.kListm_afterImages);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			SyncListInt.WriteInstance(writer, this.m_afterImages);
			writer.WritePackedUInt32((uint)this.m_maxAfterImageCount);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListInt.WriteInstance(writer, this.m_afterImages);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_maxAfterImageCount);
		}
		if (!flag)
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
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListInt.ReadReference(reader, this.m_afterImages);
			this.m_maxAfterImageCount = (int)reader.ReadPackedUInt32();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TricksterAfterImageNetworkBehaviour.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			SyncListInt.ReadReference(reader, this.m_afterImages);
		}
		if ((num & 2) != 0)
		{
			this.m_maxAfterImageCount = (int)reader.ReadPackedUInt32();
		}
	}
}
