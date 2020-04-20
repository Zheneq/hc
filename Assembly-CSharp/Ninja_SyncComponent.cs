using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Ninja_SyncComponent : NetworkBehaviour
{
	[Separator("[Deathmark] Persistent Effect Data", true)]
	public StandardActorEffectData m_deathMarkEffectData;

	[Separator("[Deathmark] On Detonate", true)]
	public int m_deathmarkOnTriggerDamage = 2;

	public int m_deathmarkOnTriggerCasterHeal;

	public int m_deathmarkOnTriggerCasterEnergyGain = 5;

	[Header("-- Effects On Detonate --")]
	public StandardEffectInfo m_effectOnTargetOnDetonate;

	public StandardEffectInfo m_effectOnCasterOnDetonate;

	[Separator("[Deathmark] Blacklist of Abilities that shouldn't trigger detonation", true)]
	public List<AbilityData.ActionType> m_blacklistForDeathmark;

	[Separator("[Deathmark] Sequences", true)]
	public GameObject m_deathmarkOnTriggerSequencePrefab;

	public GameObject m_deathmarkPersistentSequencePrefab;

	[Header("-- For misc vfx indicators --")]
	public GameObject m_rewindIndicatorVfxPrefab;

	[SyncVar]
	internal short m_rewindHToHp;

	[SyncVar]
	internal short m_rewindToSquareX = -1;

	[SyncVar]
	internal short m_rewindToSquareY = -1;

	[SyncVar]
	internal bool m_shurikenDashingThisTurn;

	[SyncVar]
	private int m_totalDeathmarkDamage;

	private SyncListUInt m_deathmarkedActorIndices = new SyncListUInt();

	private GameObject m_rangeIndicatorObj;

	private ActorData m_owner;

	private GameObject m_rewindIndicatorObj;

	private FriendlyEnemyVFXSelector m_rewindIndicatorFoFComp;

	private NinjaShurikenOrDash m_shurikenOrDashAbility;

	private AbilityData.ActionType m_shurikenOrDashActionType;

	private NinjaRewind m_rewindAbility;

	private static int kListm_deathmarkedActorIndices = -0x12B1182F;

	static Ninja_SyncComponent()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Ninja_SyncComponent), Ninja_SyncComponent.kListm_deathmarkedActorIndices, new NetworkBehaviour.CmdDelegate(Ninja_SyncComponent.InvokeSyncListm_deathmarkedActorIndices));
		NetworkCRC.RegisterBehaviour("Ninja_SyncComponent", 0);
	}

	private void Start()
	{
		this.m_owner = base.GetComponent<ActorData>();
		if (this.m_owner != null && this.m_owner.GetAbilityData() != null)
		{
			this.m_shurikenOrDashAbility = (this.m_owner.GetAbilityData().GetAbilityOfType(typeof(NinjaShurikenOrDash)) as NinjaShurikenOrDash);
			this.m_shurikenOrDashActionType = this.m_owner.GetAbilityData().GetActionTypeOfAbility(this.m_shurikenOrDashAbility);
			this.m_rewindAbility = (this.m_owner.GetAbilityData().GetAbilityOfType(typeof(NinjaRewind)) as NinjaRewind);
		}
		if (HighlightUtils.Get() != null)
		{
			this.m_rangeIndicatorObj = HighlightUtils.Get().CreateDynamicConeMesh(1f, 360f, true, null);
			UIDynamicCone uidynamicCone = (!this.m_rangeIndicatorObj) ? null : this.m_rangeIndicatorObj.GetComponent<UIDynamicCone>();
			if (uidynamicCone != null)
			{
				HighlightUtils.Get().AdjustDynamicConeMesh(this.m_rangeIndicatorObj, 1f, 360f);
				uidynamicCone.SetConeObjectActive(false);
			}
			if (this.m_rewindIndicatorVfxPrefab != null)
			{
				if (this.m_rewindAbility != null)
				{
					this.m_rewindIndicatorObj = UnityEngine.Object.Instantiate<GameObject>(this.m_rewindIndicatorVfxPrefab, Vector3.zero, Quaternion.identity);
					this.m_rewindIndicatorObj.SetActive(false);
					this.m_rewindIndicatorFoFComp = this.m_rewindIndicatorObj.GetComponent<FriendlyEnemyVFXSelector>();
				}
			}
		}
	}

	public BoardSquare GetSquareForRewind()
	{
		return Board.Get().GetBoardSquare((int)this.m_rewindToSquareX, (int)this.m_rewindToSquareY);
	}

	public void ClearSquareForRewind()
	{
		this.Networkm_rewindToSquareX = -1;
		this.Networkm_rewindToSquareY = -1;
	}

	public bool ActorHasDeathmark(ActorData actor)
	{
		bool result;
		if (actor != null)
		{
			result = this.m_deathmarkedActorIndices.Contains((uint)actor.ActorIndex);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool HasDeathmarkActor()
	{
		return this.m_deathmarkedActorIndices.Count > 0;
	}

	public int GetTotalDeathmarkDamage()
	{
		return this.m_totalDeathmarkDamage;
	}

	private unsafe bool ShouldShowRangeIndicator(out float dashToUnmarkedRange)
	{
		bool result = false;
		dashToUnmarkedRange = 0f;
		if (NetworkClient.active)
		{
			if (this.m_shurikenOrDashAbility != null && GameFlowData.Get() != null)
			{
				if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
				{
					ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
					if (activeOwnedActorData != null)
					{
						if (this.m_owner.IsVisibleToClient())
						{
							bool flag;
							if (this.m_owner.GetTeam() == activeOwnedActorData.GetTeam())
							{
								flag = this.m_owner.GetAbilityData().ValidateActionIsRequestable(this.m_shurikenOrDashActionType);
							}
							else
							{
								flag = (this.m_owner.GetAbilityData().GetCooldownRemaining(this.m_shurikenOrDashActionType) <= 0);
							}
							if (flag)
							{
								dashToUnmarkedRange = this.m_shurikenOrDashAbility.GetDashToUnmarkedRange();
								result = (dashToUnmarkedRange > 0f);
							}
						}
					}
				}
			}
		}
		return result;
	}

	private bool ShouldShowRevindIndicator()
	{
		bool result = false;
		if (NetworkClient.active)
		{
			if (GameFlowData.Get() != null)
			{
				if (this.m_rewindAbility != null)
				{
					if (this.m_owner != null)
					{
						if (!this.m_owner.IsDead() && FogOfWar.GetClientFog() != null)
						{
							BoardSquare squareForRewind = this.GetSquareForRewind();
							if (squareForRewind != null)
							{
								if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
								{
									if (GameFlowData.Get().LocalPlayerData != null)
									{
										if (GameFlowData.Get().LocalPlayerData.GetTeamViewing() == this.m_owner.GetTeam())
										{
											goto IL_130;
										}
									}
									if (!FogOfWar.GetClientFog().IsVisible(squareForRewind))
									{
										return result;
									}
									IL_130:
									result = true;
								}
							}
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
			if (this.m_rangeIndicatorObj != null)
			{
				float radiusInSquares;
				bool flag = this.ShouldShowRangeIndicator(out radiusInSquares);
				if (flag)
				{
					if (!this.m_rangeIndicatorObj.activeSelf)
					{
						this.m_rangeIndicatorObj.SetActive(true);
						HighlightUtils.Get().AdjustDynamicConeMesh(this.m_rangeIndicatorObj, radiusInSquares, 360f);
						Vector3 travelBoardSquareWorldPosition = this.m_owner.GetTravelBoardSquareWorldPosition();
						travelBoardSquareWorldPosition.y = HighlightUtils.GetHighlightHeight();
						this.m_rangeIndicatorObj.transform.position = travelBoardSquareWorldPosition;
						goto IL_EE;
					}
				}
				if (!flag)
				{
					if (this.m_rangeIndicatorObj.activeSelf)
					{
						this.m_rangeIndicatorObj.SetActive(false);
					}
				}
			}
			IL_EE:
			if (this.m_rewindIndicatorObj != null)
			{
				if (this.m_rewindIndicatorFoFComp != null)
				{
					this.m_rewindIndicatorFoFComp.Setup(this.m_owner.GetTeam());
				}
				bool flag2 = this.ShouldShowRevindIndicator();
				if (flag2)
				{
					if (!this.m_rewindIndicatorObj.activeSelf)
					{
						BoardSquare squareForRewind = this.GetSquareForRewind();
						Vector3 position = squareForRewind.ToVector3();
						position.y = HighlightUtils.GetHighlightHeight();
						this.m_rewindIndicatorObj.transform.position = position;
						this.m_rewindIndicatorObj.SetActive(true);
						return;
					}
				}
				if (!flag2)
				{
					if (this.m_rewindIndicatorObj.activeSelf)
					{
						this.m_rewindIndicatorObj.SetActive(false);
					}
				}
			}
		}
	}

	private void UNetVersion()
	{
	}

	public short Networkm_rewindHToHp
	{
		get
		{
			return this.m_rewindHToHp;
		}
		[param: In]
		set
		{
			base.SetSyncVar<short>(value, ref this.m_rewindHToHp, 1U);
		}
	}

	public short Networkm_rewindToSquareX
	{
		get
		{
			return this.m_rewindToSquareX;
		}
		[param: In]
		set
		{
			base.SetSyncVar<short>(value, ref this.m_rewindToSquareX, 2U);
		}
	}

	public short Networkm_rewindToSquareY
	{
		get
		{
			return this.m_rewindToSquareY;
		}
		[param: In]
		set
		{
			base.SetSyncVar<short>(value, ref this.m_rewindToSquareY, 4U);
		}
	}

	public bool Networkm_shurikenDashingThisTurn
	{
		get
		{
			return this.m_shurikenDashingThisTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_shurikenDashingThisTurn, 8U);
		}
	}

	public int Networkm_totalDeathmarkDamage
	{
		get
		{
			return this.m_totalDeathmarkDamage;
		}
		[param: In]
		set
		{
			base.SetSyncVar<int>(value, ref this.m_totalDeathmarkDamage, 0x10U);
		}
	}

	protected static void InvokeSyncListm_deathmarkedActorIndices(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_deathmarkedActorIndices called on server.");
			return;
		}
		((Ninja_SyncComponent)obj).m_deathmarkedActorIndices.HandleMsg(reader);
	}

	private void Awake()
	{
		this.m_deathmarkedActorIndices.InitializeBehaviour(this, Ninja_SyncComponent.kListm_deathmarkedActorIndices);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)this.m_rewindHToHp);
			writer.WritePackedUInt32((uint)this.m_rewindToSquareX);
			writer.WritePackedUInt32((uint)this.m_rewindToSquareY);
			writer.Write(this.m_shurikenDashingThisTurn);
			writer.WritePackedUInt32((uint)this.m_totalDeathmarkDamage);
			SyncListUInt.WriteInstance(writer, this.m_deathmarkedActorIndices);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_rewindHToHp);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_rewindToSquareX);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_rewindToSquareY);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_shurikenDashingThisTurn);
		}
		if ((base.syncVarDirtyBits & 0x10U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_totalDeathmarkDamage);
		}
		if ((base.syncVarDirtyBits & 0x20U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, this.m_deathmarkedActorIndices);
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
			this.m_rewindHToHp = (short)reader.ReadPackedUInt32();
			this.m_rewindToSquareX = (short)reader.ReadPackedUInt32();
			this.m_rewindToSquareY = (short)reader.ReadPackedUInt32();
			this.m_shurikenDashingThisTurn = reader.ReadBoolean();
			this.m_totalDeathmarkDamage = (int)reader.ReadPackedUInt32();
			SyncListUInt.ReadReference(reader, this.m_deathmarkedActorIndices);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			this.m_rewindHToHp = (short)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			this.m_rewindToSquareX = (short)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			this.m_rewindToSquareY = (short)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			this.m_shurikenDashingThisTurn = reader.ReadBoolean();
		}
		if ((num & 0x10) != 0)
		{
			this.m_totalDeathmarkDamage = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x20) != 0)
		{
			SyncListUInt.ReadReference(reader, this.m_deathmarkedActorIndices);
		}
	}
}
