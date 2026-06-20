// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Passive_TricksterAfterImage : Passive
{
	[Header("-- Clone Info")]
	public GameObject m_afterImagePrefab;
	public CharacterResourceLink m_afterImageResourceLink;
	[Header("-- Duration and Despawning")]
	public int m_maxDuration = 2;
	public SpoilsSpawnData m_spoilSpawnOnDisappear;
	[Header("-- When to create clone on movement")]
	public bool m_cloneOnTeleport;
	public bool m_cloneOnKnockback;
	[Header("-- Animation")]
	public AbilityData.ActionType m_onEntryAnimIndex = AbilityData.ActionType.ABILITY_6;

	internal const int c_maxAfterImages = 2;

#if SERVER
	// added in rogues
	private TricksterAfterImageNetworkBehaviour m_syncComponent;
	private ActorAdditionalVisionProviders m_visionProviders;
	private TricksterMadeYouLook m_swapAbility;
	private BoardSquare m_tricksterSquareOnResolveStart;
	private Dictionary<int, int> m_actorIndexToTurnsSoFar;
	private List<ActorData> m_preAllocatedAfterImages = new List<ActorData>();
	private int m_numPreallocAvailable = c_maxAfterImages;
	private bool m_createdClonesFromMovement;
	private bool m_movedOnStartingSquare;
	private BoardSquare m_prevSquareInPath;
	private List<BoardSquare> m_cloneSquaresBeforeMovement = new List<BoardSquare>();
#endif

	public static Passive_TricksterAfterImage GetFromActor(ActorData actor)
	{
		if (actor == null)
		{
			return null;
		}
		PassiveData passiveData = actor.GetPassiveData();
		if (passiveData == null)
		{
			return null;
		}
		foreach (Passive passive in passiveData.m_passives)
		{
			if (passive != null && passive is Passive_TricksterAfterImage image)
			{
				return image;
			}
		}
		return null;
	}

#if SERVER
	// added in rogues
	public List<ActorData> GetValidAfterImages()
	{
		List<ActorData> afterImages = new List<ActorData>();
		foreach (int actorIndex in m_syncComponent.m_afterImages)
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
			if (actorData != null
			    && actorData.gameObject.activeSelf
			    && !actorData.IsDead())
			{
				afterImages.Add(actorData);
			}
		}
		return afterImages;
	}

	// added in rogues
	public List<ActorData> GetPreAllocatedActors()
	{
		return m_preAllocatedAfterImages.ToList();
	}

	// added in rogues
	protected override void OnStartup()
	{
		m_syncComponent = GetComponent<TricksterAfterImageNetworkBehaviour>();
		m_visionProviders = GetComponent<ActorAdditionalVisionProviders>();
		AbilityData abilityData = GetComponent<AbilityData>();
		if (abilityData != null)
		{
			m_swapAbility = abilityData.GetAbilityOfType(typeof(TricksterMadeYouLook)) as TricksterMadeYouLook;
		}
		
		// rogues
		// m_syncComponent.Networkm_maxAfterImageCount = c_maxAfterImages;
		// server
		m_syncComponent.m_maxAfterImageCount = c_maxAfterImages;
		
		if (NetworkServer.active)
		{
			PreAllocateAfterImages();
		}
	}

	// added in rogues
	private void OnDestroy()
	{
		if (GameFlowData.Get() == null
		    || !NetworkServer.active
		    || m_syncComponent == null)
		{
			return;
		}
		for (int i = m_syncComponent.m_afterImages.Count - 1; i >= 0; i--)
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(m_syncComponent.m_afterImages[i]);
			if (actorData != null)
			{
				m_visionProviders.RemoveVisionProviderOnActor(actorData.ActorIndex);
				GameplayUtils.DestroyActor(actorData.gameObject);
			}
		}
		m_syncComponent.m_afterImages.Clear();
	}

	// added in rogues
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		AdvanceSpawnedDurationCounter();
		FreezeUnusedAfterImages();
		foreach (ActorData actorData in m_preAllocatedAfterImages)
		{
			if (actorData != null)
			{
				actorData.OnTurnStart();
				if (GameFlowData.Get().CurrentTurn == 1)
				{
					actorData.GetActorStats().AddStatMod(StatType.IncomingDamage, ModType.Multiplier, 0f);
					actorData.GetActorStatus().AddStatus(StatusType.CantCollectPowerups, 0);
				}
			}
		}
		m_tricksterSquareOnResolveStart = null;
		m_cloneSquaresBeforeMovement.Clear();
		m_createdClonesFromMovement = false;
		m_movedOnStartingSquare = false;
		m_prevSquareInPath = null;
	}

	// added in rogues
	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		if (m_createdClonesFromMovement
		    || m_cloneSquaresBeforeMovement.Count <= 0
		    || Owner.IsDead())
		{
			return;
		}
		foreach (BoardSquare cloneSquareBeforeMovement in m_cloneSquaresBeforeMovement)
		{
			bool afterImageCreated = false;
			for (int i = 0; i <= 3; i++)
			{
				BoardSquare center = cloneSquareBeforeMovement;
				if (m_movedOnStartingSquare && m_prevSquareInPath != null)
				{
					center = m_prevSquareInPath;
				}
				foreach (BoardSquare square in AreaEffectUtils.GetSquaresInBorderLayer(center, i, true))
				{
					if (square.IsValidForGameplay() && square.OccupantActor == null)
					{
						CreateAfterImageOnSquare(square, Owner.transform.forward);
						afterImageCreated = true;
						break;
					}
				}
				if (afterImageCreated)
				{
					break;
				}
			}
			if (m_movedOnStartingSquare)
			{
				break;
			}
		}
	}

	// added in rogues
	public override void OnDied(List<UnresolvedHealthChange> killers)
	{
		ClearAllAfterImages(true);
	}

	// added in rogues
	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		if (phase == AbilityPriority.Prep_Defense)
		{
			m_tricksterSquareOnResolveStart = !Owner.IsDead() && Owner.GetCurrentBoardSquare() != null
				? Owner.GetCurrentBoardSquare()
				: null;
		}
	}

	// added in rogues
	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		if (phase == AbilityPriority.Combat_Final)
		{
			CleanUpInvalidAfterImages();
			m_cloneSquaresBeforeMovement.Clear();
			float num = Owner.GetActorMovement().CalculateMaxHorizontalMovement();
			if (ServerActionBuffer.Get().HasUnresolvedMovementRequest(Owner) && num > 0f)
			{
				for (int i = 0; i < m_syncComponent.m_afterImages.Count; i++)
				{
					ActorData actorData = GameFlowData.Get().FindActorByActorIndex(m_syncComponent.m_afterImages[i]);
					if (actorData != null && actorData.GetCurrentBoardSquare() != null)
					{
						m_cloneSquaresBeforeMovement.Add(actorData.GetCurrentBoardSquare());
					}
				}
				ClearAllAfterImages(true);
			}
		}
	}

	// added in rogues
	public override void OnServerLastKnownPosUpdateBegin()
	{
		// TODO TRICKSTER vision
		m_visionProviders.AddVisionProviderOnGridPos(
			m_tricksterSquareOnResolveStart.GetGridPos(),
			Owner.GetSightRange(),
			false, // custom
			VisionProviderInfo.BrushRevealType.BaseOnCenterPosition,
			false,
			true, // custom
			BoardSquare.VisibilityFlags.Team);
	}

	// added in rogues
	public override void OnServerLastKnownPosUpdateEnd()
	{
		// TODO TRICKSTER vision
		m_visionProviders.RemoveVisionProviderOnGridPos(
			m_tricksterSquareOnResolveStart.GetGridPos(),
			Owner.GetSightRange(),
			false, // custom
			VisionProviderInfo.BrushRevealType.BaseOnCenterPosition,
			false,
			true, // custom
			BoardSquare.VisibilityFlags.Team);
	}

	// added in rogues
	public override void OnMovementResultsGathered(MovementCollection stabilizedMovements)
	{
		foreach (MovementInstance movementInstance in stabilizedMovements.m_movementInstances)
		{
			if (movementInstance.m_mover != Owner || movementInstance.m_path == null)
			{
				continue;
			}
			BoardSquare square = movementInstance.m_path.square;
			BoardSquarePathInfo pathInfo = movementInstance.m_path;
			while (pathInfo != null && pathInfo.next != null)
			{
				m_prevSquareInPath = pathInfo.square;
				pathInfo = pathInfo.next;
			}
			if (pathInfo.square == square)
			{
				m_movedOnStartingSquare = true;
			}
		}
	}

	// added in rogues
	public override void OnMovementStart(BoardSquare startSquare, BoardSquarePathInfo gameplayPathRemaining, ActorData.MovementType movementType)
	{
		if (gameplayPathRemaining == null)
		{
			return;
		}
		
		BoardSquarePathInfo pathInfo = gameplayPathRemaining;
		while (pathInfo != null && pathInfo.next != null)
		{
			pathInfo = pathInfo.next;
		}
		
		bool resetAfterImages = false;
		if (pathInfo.square != startSquare)
		{
			bool isDashStored = ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Owner, typeof(TricksterMadeYouLook));
			bool isNormalMovement = movementType == ActorData.MovementType.Normal;
			bool isTeleport = movementType == ActorData.MovementType.Teleport;
			bool isKnockback = movementType == ActorData.MovementType.Knockback;
			resetAfterImages = isNormalMovement
			       || (m_cloneOnKnockback && isKnockback)
			       || (m_cloneOnTeleport && isTeleport && !isDashStored);
		}
		if (resetAfterImages)
		{
			ClearAllAfterImages(true);
			CreateAfterImageOnSquare(startSquare, Owner.transform.forward);
			m_createdClonesFromMovement = true;
		}
	}

	// added in rogues
	public override void AddInvalidEvadeDestinations(List<ServerEvadeUtils.EvadeInfo> evades, List<BoardSquare> invalidSquares)
	{
		if ((m_cloneOnTeleport && ServerActionBuffer.Get().ActorIsEvading(Owner))
		    || ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Owner, typeof(TricksterMadeYouLook)))
		{
			invalidSquares.Add(Owner.GetCurrentBoardSquare());
		}
	}

	// added in rogues
	public override void AddInvalidKnockbackDestinations(
		Dictionary<ActorData, ServerKnockbackManager.KnockbackHits> incomingKnockbacks,
		List<BoardSquare> invalidSquares)
	{
		if (m_cloneOnKnockback
		    && incomingKnockbacks.ContainsKey(Owner)
		    && Owner.GetCurrentBoardSquare() != null)
		{
			invalidSquares.Add(Owner.GetCurrentBoardSquare());
		}
	}

	// added in rogues
	public BoardSquare GetTricksterSquareOnResolveStart()
	{
		return m_tricksterSquareOnResolveStart;
	}

	// added in rogues
	public void TurnToPosition(ActorData actor, Vector3 position)
	{
		if (NetworkServer.active && actor != null)
		{
			actor.TurnToPosition(position);
			m_syncComponent.CallRpcTurnToPosition(actor.ActorIndex, position);
		}
	}

	// added in rogues
	private SpoilsSpawnData GetSpoilsSpawnDataOnDisappear()
	{
		if (m_swapAbility != null)
		{
			return m_swapAbility.GetSpoilsSpawnDataOnDisappear(m_spoilSpawnOnDisappear);
		}
		return m_spoilSpawnOnDisappear;
	}

	// added in rogues
	public void CreateAfterImageOnSquare(BoardSquare square, Vector3 forwardDirection)
	{
		if (!NetworkServer.active)
		{
			return;
		}

		if (m_numPreallocAvailable <= 0 || m_numPreallocAvailable > c_maxAfterImages)
		{
			Debug.LogError("Trying to create more than available or mismatched available count");
			return;
		}
		
		ActorData actorData = m_preAllocatedAfterImages[c_maxAfterImages - m_numPreallocAvailable];
		UnfreezeActor(actorData);
		BoardSquare squareFromIndex = Board.Get().GetSquareFromIndex(0, 0);
		actorData.TeleportToBoardSquare(
			square,
			Owner.transform.forward,
			ActorData.TeleportType.TricksterAfterImage,
			null,
			20f,
			ActorData.MovementType.Teleport,
			GameEventManager.EventType.Invalid,
			squareFromIndex);
		m_syncComponent.m_afterImages.Add(actorData.ActorIndex);
		m_numPreallocAvailable--;
	}

	// added in rogues
	public void CreateAfterImageOnSquareWithoutTeleport(BoardSquare square, Vector3 forwardDirection, bool setPose, bool enableRenderer)
	{
		if (!NetworkServer.active)
		{
			return;
		}

		if (m_numPreallocAvailable <= 0 || m_numPreallocAvailable > c_maxAfterImages)
		{
			Debug.LogError("Trying to create more than available or mismatched available count");
			return;
		}
		
		ActorData actorData = m_preAllocatedAfterImages[c_maxAfterImages - m_numPreallocAvailable];
		UnfreezeActor(actorData);
		if (setPose)
		{
			actorData.SetTransformPositionToSquare(square);
			actorData.transform.rotation = Quaternion.LookRotation(forwardDirection);
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
			}
			TricksterAfterImageNetworkBehaviour.SetMaterialEnabledForAfterImage(m_owner, actorData, enableRenderer);
			m_syncComponent.CallRpcSetPose(actorData.ActorIndex, actorData.transform.position, forwardDirection, enableRenderer);
		}
		
		m_syncComponent.m_afterImages.Add(actorData.ActorIndex);
		m_numPreallocAvailable--;
	}

	// added in rogues
	public void ClearAllAfterImages(bool doTeleport)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		for (int i = m_syncComponent.m_afterImages.Count - 1; i >= 0; i--)
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(m_syncComponent.m_afterImages[i]);
			if (actorData == null)
			{
				continue;
			}
			BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
			FreezeActor(actorData);
			if (doTeleport)
			{
				BoardSquare squareFromIndex = Board.Get().GetSquareFromIndex(0, 0);
				actorData.TeleportToBoardSquare(
					squareFromIndex,
					Owner.transform.forward,
					ActorData.TeleportType.TricksterAfterImage,
					null,
					20f,
					ActorData.MovementType.Teleport,
					GameEventManager.EventType.Invalid,
					currentBoardSquare != null
						? currentBoardSquare
						: squareFromIndex);
			}
			if (currentBoardSquare != null)
			{
				SpoilsSpawnData spoilsSpawnDataOnDisappear = GetSpoilsSpawnDataOnDisappear();
				if (currentBoardSquare != null && spoilsSpawnDataOnDisappear != null)
				{
					spoilsSpawnDataOnDisappear.SpawnSpoilsAroundSquare(currentBoardSquare, Owner.GetTeam());
				}
			}
		}
		m_numPreallocAvailable = c_maxAfterImages;
		m_syncComponent.m_afterImages.Clear();
	}

	// added in rogues
	private void AdvanceSpawnedDurationCounter()
	{
		if (!NetworkServer.active || m_maxDuration <= 0)
		{
			return;
		}
		for (int i = 0; i < m_syncComponent.m_afterImages.Count; i++)
		{
			int key = m_syncComponent.m_afterImages[i];
			m_actorIndexToTurnsSoFar[key]++;
		}
	}

	// added in rogues
	public void ReduceSpawnedDurationCounter(int actorIndex)
	{
		if (NetworkServer.active
		    && m_maxDuration > 0
		    && m_syncComponent.m_afterImages.Contains(actorIndex)
		    && m_actorIndexToTurnsSoFar.ContainsKey(actorIndex))
		{
			m_actorIndexToTurnsSoFar[actorIndex]--;
		}
	}
	
	// added in rogues
	private void CleanUpInvalidAfterImages()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		int numAfterImages = ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Owner, typeof(TricksterCatchMeIfYouCan)) ? c_maxAfterImages : 1;
		for (int i = m_syncComponent.m_afterImages.Count - 1; i >= 0; i--)
		{
			ActorData actorData = GameFlowData.Get().FindActorByActorIndex(m_syncComponent.m_afterImages[i]);
			if (actorData == null)
			{
				Debug.LogError("Null actor in list of clones for Trickster");
				m_syncComponent.m_afterImages.RemoveAt(i);
			}
			else if ((m_maxDuration > 0 && m_actorIndexToTurnsSoFar[actorData.ActorIndex] > m_maxDuration) || i >= numAfterImages)
			{
				BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
				FreezeActor(actorData);
				m_syncComponent.m_afterImages.RemoveAt(i);
				m_numPreallocAvailable++;
				BoardSquare squareFromIndex = Board.Get().GetSquareFromIndex(0, 0);
				actorData.TeleportToBoardSquare(
					squareFromIndex,
					Owner.transform.forward,
					ActorData.TeleportType.TricksterAfterImage,
					null,
					20f,
					ActorData.MovementType.Teleport,
					GameEventManager.EventType.Invalid,
					currentBoardSquare);
				SpoilsSpawnData spoilsSpawnDataOnDisappear = GetSpoilsSpawnDataOnDisappear();
				if (currentBoardSquare != null && spoilsSpawnDataOnDisappear != null)
				{
					spoilsSpawnDataOnDisappear.SpawnSpoilsAroundSquare(currentBoardSquare, Owner.GetTeam());
				}
			}
		}
	}
	
	// added in rogues
	private void FreezeUnusedAfterImages()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		foreach (ActorData afterImage in m_preAllocatedAfterImages)
		{
			if (!m_syncComponent.m_afterImages.Contains(afterImage.ActorIndex))
			{
				FreezeActor(afterImage);
			}
		}
	}

	// added in rogues
	private void PreAllocateAfterImages()
	{
		m_actorIndexToTurnsSoFar = new Dictionary<int, int>();
		for (int i = 0; i < c_maxAfterImages; i++)
		{
			GameObject afterImageGameObject = Instantiate(m_afterImagePrefab, Vector3.zero, Quaternion.identity);
			if (!afterImageGameObject)
			{
				continue;
			}
			
			ActorData actorData = afterImageGameObject.GetComponent<ActorData>();
			PrefabResourceLink actorSkinPrefabLink = Owner.m_actorSkinPrefabLink;
			
			// rogues
			// actorData.Networkm_visualInfo = Owner.m_visualInfo;
			// actorData.Networkm_abilityVfxSwapInfo = Owner.m_abilityVfxSwapInfo;
			// custom
			actorData.m_visualInfo = Owner.m_visualInfo;
			actorData.m_abilityVfxSwapInfo = Owner.m_abilityVfxSwapInfo;

			actorData.Initialize(actorSkinPrefabLink, false); // no addMasterSkinVfx in rogues
			
			// TODO BOTS why does afterimage even need a brain?
			// rogues
			// actorData.gameObject.AddComponent<BotController>();
			// actorData.HasBotController = true;
			// actorData.gameObject.AddComponent<NPCBrain>();
			
			ActorBehavior actorBehavior = afterImageGameObject.GetComponent<ActorBehavior>();
			if (actorBehavior)
			{
				actorBehavior.OnTurnStart();
			}
			actorData.SetTeam(Owner.GetTeam());
			actorData.InitActorNetworkVisibilityObjects();
			// custom
			actorData.m_displayName = "FT";
			// rogues
			// actorData.Networkm_displayName = "FT";
			actorData.SetHitPoints(Owner.HitPoints);
			if (actorData.GetActorModelData() != null)
			{
				actorData.GetActorModelData().DisableAndHideRenderers();
			}
			NetworkServer.Spawn(afterImageGameObject);
			FreezeActor(actorData);
			m_preAllocatedAfterImages.Add(actorData);
			m_actorIndexToTurnsSoFar[actorData.ActorIndex] = 0;
		}
		Owner.OccupyCurrentBoardSquare();
	}

	// added in rogues
	private void FreezeActor(ActorData actor)
	{
		if (!NetworkServer.active || actor == null)
		{
			return;
		}
		if (TricksterDebugTraceOn)
		{
			LogForDebugging("Hiding: " + actor.DebugNameString());
		}
		if (actor.GetCurrentBoardSquare() != null)
		{
			actor.UnoccupyCurrentBoardSquare();
			actor.ClearCurrentBoardSquare();
		}
		ActorStatus actorStatus = actor.GetActorStatus();
		if (actorStatus != null && !actorStatus.HasStatus(StatusType.IsolateVisionFromAllies))
		{
			actorStatus.AddStatus(StatusType.IsolateVisionFromAllies, 0);
		}
		ServerActionBuffer.Get().CancelMovementRequests(actor);
		actor.SetTransformPositionToVector(new Vector3(10000f, -100f, 10000f));
		if (actor.GetActorModelData() != null)
		{
			actor.GetActorModelData().DisableAndHideRenderers();
		}
		m_visionProviders.RemoveVisionProviderOnActor(actor.ActorIndex);
		actor.SetNameplateAlwaysInvisible(true);
		actor.IgnoreForEnergyOnHit = true;
		actor.IgnoreForAbilityHits = true;
		m_syncComponent.CallRpcFreezeActor(actor.ActorIndex);
		m_actorIndexToTurnsSoFar[actor.ActorIndex] = 0;
	}

	// added in rogues
	private void UnfreezeActor(ActorData actor)
	{
		if (!NetworkServer.active || actor == null)
		{
			return;
		}
		if (TricksterDebugTraceOn)
		{
			LogForDebugging("Restoring: " + actor.DebugNameString());
		}
		actor.EnableRendererAndUpdateVisibility();
		ActorStatus actorStatus = actor.GetActorStatus();
		if (actorStatus != null && actorStatus.HasStatus(StatusType.IsolateVisionFromAllies))
		{
			actorStatus.RemoveStatus(StatusType.IsolateVisionFromAllies);
		}
		// TODO TRICKSTER vision
		m_visionProviders.AddVisionProviderOnActor(
			actor.ActorIndex,
			actor.GetSightRange(),
			false, // custom
			VisionProviderInfo.BrushRevealType.BaseOnCenterPosition,
			false, // custom
			true, // custom
			BoardSquare.VisibilityFlags.Team);
		m_syncComponent.CallRpcUnfreezeActor(actor.ActorIndex, (int)m_onEntryAnimIndex);
		m_actorIndexToTurnsSoFar[actor.ActorIndex] = 1;
		Animator modelAnimator = actor.GetModelAnimator();
		if (modelAnimator != null)
		{
			modelAnimator.SetInteger("Attack", (int)m_onEntryAnimIndex);
			modelAnimator.SetInteger("IdleType", 0);
			modelAnimator.SetBool("CinematicCam", false);
			modelAnimator.SetTrigger("StartAttack");
		}
		if (NetworkClient.active)
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (actor.GetActorModelData() != null
			    && activeOwnedActorData != null
			    && Owner != null)
			{
				bool sameTeamAsClientActor = activeOwnedActorData.GetTeam() == Owner.GetTeam();
				TricksterAfterImageNetworkBehaviour component = Owner.GetComponent<TricksterAfterImageNetworkBehaviour>();
				if (component != null)
				{
					TricksterAfterImageNetworkBehaviour.InitializeAfterImageMaterial(
						actor.GetActorModelData(),
						sameTeamAsClientActor,
						component.m_afterImageAlpha,
						component.m_afterImageShader,
						true);
				}
			}
		}
	}

	// added in rogues
	public bool TricksterDebugTraceOn => false;

	// added in rogues
	public void LogForDebugging(string input)
	{
		Debug.LogWarning("<color=white>Trickster: </color>" + input);
	}

	// added in rogues
	private void PrintDebugLog()
	{
		string text = "~~~ AfterImage passive debug log start: \n";
		text += "afterImageCount: "+m_syncComponent.m_afterImages.Count+" \n";
		List<ActorData> validAfterImages = m_syncComponent.GetValidAfterImages();
		text += "num valid: " + validAfterImages.Count + "\n"
		        + "num prealloc available: " + m_numPreallocAvailable + "\n";
		foreach (ActorData actorData in validAfterImages)
		{
			text += actorData.DisplayName + " " + actorData.GetAllyTeamName() + "\n";
		}
		Debug.LogWarning(text);
	}
#endif
}
