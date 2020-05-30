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

	private static int kListm_deathmarkedActorIndices;

	public short Networkm_rewindHToHp
	{
		get
		{
			return m_rewindHToHp;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_rewindHToHp, 1u);
		}
	}

	public short Networkm_rewindToSquareX
	{
		get
		{
			return m_rewindToSquareX;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_rewindToSquareX, 2u);
		}
	}

	public short Networkm_rewindToSquareY
	{
		get
		{
			return m_rewindToSquareY;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_rewindToSquareY, 4u);
		}
	}

	public bool Networkm_shurikenDashingThisTurn
	{
		get
		{
			return m_shurikenDashingThisTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_shurikenDashingThisTurn, 8u);
		}
	}

	public int Networkm_totalDeathmarkDamage
	{
		get
		{
			return m_totalDeathmarkDamage;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_totalDeathmarkDamage, 16u);
		}
	}

	static Ninja_SyncComponent()
	{
		kListm_deathmarkedActorIndices = -313595951;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Ninja_SyncComponent), kListm_deathmarkedActorIndices, InvokeSyncListm_deathmarkedActorIndices);
		NetworkCRC.RegisterBehaviour("Ninja_SyncComponent", 0);
	}

	private void Start()
	{
		m_owner = GetComponent<ActorData>();
		if (m_owner != null && m_owner.GetAbilityData() != null)
		{
			m_shurikenOrDashAbility = (m_owner.GetAbilityData().GetAbilityOfType(typeof(NinjaShurikenOrDash)) as NinjaShurikenOrDash);
			m_shurikenOrDashActionType = m_owner.GetAbilityData().GetActionTypeOfAbility(m_shurikenOrDashAbility);
			m_rewindAbility = (m_owner.GetAbilityData().GetAbilityOfType(typeof(NinjaRewind)) as NinjaRewind);
		}
		if (!(HighlightUtils.Get() != null))
		{
			return;
		}
		m_rangeIndicatorObj = HighlightUtils.Get().CreateDynamicConeMesh(1f, 360f, true);
		UIDynamicCone uIDynamicCone = (!m_rangeIndicatorObj) ? null : m_rangeIndicatorObj.GetComponent<UIDynamicCone>();
		if (uIDynamicCone != null)
		{
			HighlightUtils.Get().AdjustDynamicConeMesh(m_rangeIndicatorObj, 1f, 360f);
			uIDynamicCone.SetConeObjectActive(false);
		}
		if (!(m_rewindIndicatorVfxPrefab != null))
		{
			return;
		}
		while (true)
		{
			if (m_rewindAbility != null)
			{
				while (true)
				{
					m_rewindIndicatorObj = Object.Instantiate(m_rewindIndicatorVfxPrefab, Vector3.zero, Quaternion.identity);
					m_rewindIndicatorObj.SetActive(false);
					m_rewindIndicatorFoFComp = m_rewindIndicatorObj.GetComponent<FriendlyEnemyVFXSelector>();
					return;
				}
			}
			return;
		}
	}

	public BoardSquare GetSquareForRewind()
	{
		return Board.Get().GetSquare(m_rewindToSquareX, m_rewindToSquareY);
	}

	public void ClearSquareForRewind()
	{
		Networkm_rewindToSquareX = -1;
		Networkm_rewindToSquareY = -1;
	}

	public bool ActorHasDeathmark(ActorData actor)
	{
		int result;
		if (actor != null)
		{
			result = (m_deathmarkedActorIndices.Contains((uint)actor.ActorIndex) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool HasDeathmarkActor()
	{
		return m_deathmarkedActorIndices.Count > 0;
	}

	public int GetTotalDeathmarkDamage()
	{
		return m_totalDeathmarkDamage;
	}

	private bool ShouldShowRangeIndicator(out float dashToUnmarkedRange)
	{
		bool result = false;
		dashToUnmarkedRange = 0f;
		if (NetworkClient.active)
		{
			if (m_shurikenOrDashAbility != null && GameFlowData.Get() != null)
			{
				if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
				{
					ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
					if (activeOwnedActorData != null)
					{
						if (m_owner.IsVisibleToClient())
						{
							bool flag = false;
							if (m_owner.GetTeam() == activeOwnedActorData.GetTeam())
							{
								flag = m_owner.GetAbilityData().ValidateActionIsRequestable(m_shurikenOrDashActionType);
							}
							else
							{
								flag = (m_owner.GetAbilityData().GetCooldownRemaining(m_shurikenOrDashActionType) <= 0);
							}
							if (flag)
							{
								dashToUnmarkedRange = m_shurikenOrDashAbility.GetDashToUnmarkedRange();
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
				if (m_rewindAbility != null)
				{
					if (m_owner != null)
					{
						if (!m_owner.IsDead() && FogOfWar.GetClientFog() != null)
						{
							BoardSquare squareForRewind = GetSquareForRewind();
							if (squareForRewind != null)
							{
								if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
								{
									if (GameFlowData.Get().LocalPlayerData != null)
									{
										if (GameFlowData.Get().LocalPlayerData.GetTeamViewing() == m_owner.GetTeam())
										{
											goto IL_0130;
										}
									}
									if (FogOfWar.GetClientFog().IsVisible(squareForRewind))
									{
										goto IL_0130;
									}
								}
							}
						}
					}
				}
			}
		}
		goto IL_0132;
		IL_0130:
		result = true;
		goto IL_0132;
		IL_0132:
		return result;
	}

	private void Update()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			if (m_rangeIndicatorObj != null)
			{
				float dashToUnmarkedRange;
				bool flag = ShouldShowRangeIndicator(out dashToUnmarkedRange);
				if (flag)
				{
					if (!m_rangeIndicatorObj.activeSelf)
					{
						m_rangeIndicatorObj.SetActive(true);
						HighlightUtils.Get().AdjustDynamicConeMesh(m_rangeIndicatorObj, dashToUnmarkedRange, 360f);
						Vector3 travelBoardSquareWorldPosition = m_owner.GetTravelBoardSquareWorldPosition();
						travelBoardSquareWorldPosition.y = HighlightUtils.GetHighlightHeight();
						m_rangeIndicatorObj.transform.position = travelBoardSquareWorldPosition;
						goto IL_00ee;
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
			goto IL_00ee;
			IL_00ee:
			if (!(m_rewindIndicatorObj != null))
			{
				return;
			}
			while (true)
			{
				if (m_rewindIndicatorFoFComp != null)
				{
					m_rewindIndicatorFoFComp.Setup(m_owner.GetTeam());
				}
				bool flag2 = ShouldShowRevindIndicator();
				if (flag2)
				{
					if (!m_rewindIndicatorObj.activeSelf)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
							{
								BoardSquare squareForRewind = GetSquareForRewind();
								Vector3 position = squareForRewind.ToVector3();
								position.y = HighlightUtils.GetHighlightHeight();
								m_rewindIndicatorObj.transform.position = position;
								m_rewindIndicatorObj.SetActive(true);
								return;
							}
							}
						}
					}
				}
				if (flag2)
				{
					return;
				}
				while (true)
				{
					if (m_rewindIndicatorObj.activeSelf)
					{
						m_rewindIndicatorObj.SetActive(false);
					}
					return;
				}
			}
		}
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_deathmarkedActorIndices(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Debug.LogError("SyncList m_deathmarkedActorIndices called on server.");
					return;
				}
			}
		}
		((Ninja_SyncComponent)obj).m_deathmarkedActorIndices.HandleMsg(reader);
	}

	private void Awake()
	{
		m_deathmarkedActorIndices.InitializeBehaviour(this, kListm_deathmarkedActorIndices);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			writer.WritePackedUInt32((uint)m_rewindHToHp);
			writer.WritePackedUInt32((uint)m_rewindToSquareX);
			writer.WritePackedUInt32((uint)m_rewindToSquareY);
			writer.Write(m_shurikenDashingThisTurn);
			writer.WritePackedUInt32((uint)m_totalDeathmarkDamage);
			SyncListUInt.WriteInstance(writer, m_deathmarkedActorIndices);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_rewindHToHp);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_rewindToSquareX);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_rewindToSquareY);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_shurikenDashingThisTurn);
		}
		if ((base.syncVarDirtyBits & 0x10) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_totalDeathmarkDamage);
		}
		if ((base.syncVarDirtyBits & 0x20) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_deathmarkedActorIndices);
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
			m_rewindHToHp = (short)reader.ReadPackedUInt32();
			m_rewindToSquareX = (short)reader.ReadPackedUInt32();
			m_rewindToSquareY = (short)reader.ReadPackedUInt32();
			m_shurikenDashingThisTurn = reader.ReadBoolean();
			m_totalDeathmarkDamage = (int)reader.ReadPackedUInt32();
			SyncListUInt.ReadReference(reader, m_deathmarkedActorIndices);
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_rewindHToHp = (short)reader.ReadPackedUInt32();
		}
		if ((num & 2) != 0)
		{
			m_rewindToSquareX = (short)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			m_rewindToSquareY = (short)reader.ReadPackedUInt32();
		}
		if ((num & 8) != 0)
		{
			m_shurikenDashingThisTurn = reader.ReadBoolean();
		}
		if ((num & 0x10) != 0)
		{
			m_totalDeathmarkDamage = (int)reader.ReadPackedUInt32();
		}
		if ((num & 0x20) == 0)
		{
			return;
		}
		while (true)
		{
			SyncListUInt.ReadReference(reader, m_deathmarkedActorIndices);
			return;
		}
	}
}
