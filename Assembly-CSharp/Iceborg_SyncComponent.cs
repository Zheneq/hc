using AbilityContextNamespace;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Iceborg_SyncComponent : NetworkBehaviour
{
	public static ContextNameKeyPair s_cvarNovaCenter;

	public static ContextNameKeyPair s_cvarHasNova;

	[Separator("Delayed Aoe (Nova) Effect", true)]
	public float m_delayedAoeRadius = 1.5f;

	[Header("-- (for combat phase abilities, will add 1 to duration and only trigger next turn)")]
	public int m_delayedAoeDuration = 1;

	public bool m_delayedAoeTriggerOnReact = true;

	public bool m_delayedAoeCanReactToIndirectHits = true;

	[Separator("(Normal) Nova Trigger On Hit Data. Context Var [NovaCenter] is 1 if enemy has core, 0 otherwise", "yellow")]
	public OnHitAuthoredData m_delayedAoeOnHitData;

	[Separator("Shield per delayed aoe hit", true)]
	public int m_delayedAoeShieldPerEnemyHit;

	public int m_delayedAoeShieldPerExplosion;

	public int m_delayedAoeShieldDuration = 1;

	[Separator("Energy Gain on Delayed Aoe Trigger", true)]
	public int m_delayedAoeEnergyPerEnemyHit;

	public int m_delayedAoeEnergyPerExplosion;

	[Separator("Sequences for delayed Aoe Effect", true)]
	public GameObject m_delayedAoePersistentSeqPrefab;

	public GameObject m_delayedAoeTriggerSeqPrefab;

	public GameObject m_empoweredDelayedAoeTriggerSeqPrefab;

	[Header("-- for when shielding is applied to caster on beginning of turn")]
	public GameObject m_delayedAoeShieldApplySeqPrefab;

	internal int m_clientDetonateNovaUsedTurn = -1;

	private int m_cachedNovaTriggerDamage;

	private SyncListUInt m_actorsWithNovaCore = new SyncListUInt();

	[SyncVar]
	internal short m_damageFieldLastCastTurn = -1;

	[SyncVar]
	internal bool m_damageAreaCanMoveThisTurn;

	[SyncVar]
	internal short m_damageAreaCenterX = -1;

	[SyncVar]
	internal short m_damageAreaCenterY = -1;

	[SyncVar]
	internal Vector3 m_damageAreaFreePos;

	[SyncVar]
	internal short m_numNovaEffectsOnTurnStart;

	[SyncVar]
	internal bool m_selfShieldLowHealthOnTurnStart;

	private static int kListm_actorsWithNovaCore;

	public short Networkm_damageFieldLastCastTurn
	{
		get
		{
			return m_damageFieldLastCastTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_damageFieldLastCastTurn, 2u);
		}
	}

	public bool Networkm_damageAreaCanMoveThisTurn
	{
		get
		{
			return m_damageAreaCanMoveThisTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_damageAreaCanMoveThisTurn, 4u);
		}
	}

	public short Networkm_damageAreaCenterX
	{
		get
		{
			return m_damageAreaCenterX;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_damageAreaCenterX, 8u);
		}
	}

	public short Networkm_damageAreaCenterY
	{
		get
		{
			return m_damageAreaCenterY;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_damageAreaCenterY, 16u);
		}
	}

	public Vector3 Networkm_damageAreaFreePos
	{
		get
		{
			return m_damageAreaFreePos;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_damageAreaFreePos, 32u);
		}
	}

	public short Networkm_numNovaEffectsOnTurnStart
	{
		get
		{
			return m_numNovaEffectsOnTurnStart;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_numNovaEffectsOnTurnStart, 64u);
		}
	}

	public bool Networkm_selfShieldLowHealthOnTurnStart
	{
		get
		{
			return m_selfShieldLowHealthOnTurnStart;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_selfShieldLowHealthOnTurnStart, 128u);
		}
	}

	static Iceborg_SyncComponent()
	{
		s_cvarNovaCenter = new ContextNameKeyPair("NovaCenter");
		s_cvarHasNova = new ContextNameKeyPair("HasNova");
		kListm_actorsWithNovaCore = 1707706451;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Iceborg_SyncComponent), kListm_actorsWithNovaCore, InvokeSyncListm_actorsWithNovaCore);
		NetworkCRC.RegisterBehaviour("Iceborg_SyncComponent", 0);
	}

	private void Start()
	{
		m_cachedNovaTriggerDamage = m_delayedAoeOnHitData.GetFirstDamageValue();
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens)
	{
		m_delayedAoeOnHitData.AddTooltipTokens(tokens);
		if (m_delayedAoeShieldPerEnemyHit > 0)
		{
			AbilityMod.AddToken_IntDiff(tokens, "NovaShieldPerHit", string.Empty, m_delayedAoeShieldPerEnemyHit, false, 0);
		}
		if (m_delayedAoeShieldPerExplosion > 0)
		{
			AbilityMod.AddToken_IntDiff(tokens, "NovaShieldPerExplosion", string.Empty, m_delayedAoeShieldPerExplosion, false, 0);
		}
		if (m_delayedAoeEnergyPerEnemyHit > 0)
		{
			AbilityMod.AddToken_IntDiff(tokens, "NovaEnergyPerHit", string.Empty, m_delayedAoeEnergyPerEnemyHit, false, 0);
		}
		if (m_delayedAoeEnergyPerExplosion <= 0)
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_IntDiff(tokens, "NovaEnergyPerExplosion", string.Empty, m_delayedAoeEnergyPerExplosion, false, 0);
			return;
		}
	}

	public bool HasNovaCore(ActorData actor)
	{
		return actor != null && m_actorsWithNovaCore.Contains((uint)actor.ActorIndex);
	}

	public void SetHasCoreContext_Client(Dictionary<ActorData, ActorHitContext> actorHitContext, ActorData targetActor, ActorData caster)
	{
		if (!actorHitContext.ContainsKey(targetActor) || caster.GetTeam() == targetActor.GetTeam())
		{
			return;
		}
		while (true)
		{
			bool flag = HasNovaCore(targetActor);
			ContextVars contextVars = actorHitContext[targetActor].context;
			int hash = s_cvarHasNova.GetKey();
			int value;
			if (flag)
			{
				value = 1;
			}
			else
			{
				value = 0;
			}
			contextVars.SetInt(hash, value);
			return;
		}
	}

	public int GetTurnsSinceInitialCast()
	{
		int result = 0;
		if (m_damageFieldLastCastTurn > 0)
		{
			result = Mathf.Max(0, GameFlowData.Get().CurrentTurn - m_damageFieldLastCastTurn);
		}
		return result;
	}

	public int GetNovaCoreTriggerDamage()
	{
		return m_cachedNovaTriggerDamage;
	}

	public string GetTargetPreviewAccessoryString(AbilityTooltipSymbol symbolType, Ability ability, ActorData targetActor, ActorData caster)
	{
		return null;
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_actorsWithNovaCore(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_actorsWithNovaCore called on server.");
		}
		else
		{
			((Iceborg_SyncComponent)obj).m_actorsWithNovaCore.HandleMsg(reader);
		}
	}

	private void Awake()
	{
		m_actorsWithNovaCore.InitializeBehaviour(this, kListm_actorsWithNovaCore);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					SyncListUInt.WriteInstance(writer, m_actorsWithNovaCore);
					writer.WritePackedUInt32((uint)m_damageFieldLastCastTurn);
					writer.Write(m_damageAreaCanMoveThisTurn);
					writer.WritePackedUInt32((uint)m_damageAreaCenterX);
					writer.WritePackedUInt32((uint)m_damageAreaCenterY);
					writer.Write(m_damageAreaFreePos);
					writer.WritePackedUInt32((uint)m_numNovaEffectsOnTurnStart);
					writer.Write(m_selfShieldLowHealthOnTurnStart);
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
			SyncListUInt.WriteInstance(writer, m_actorsWithNovaCore);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_damageFieldLastCastTurn);
		}
		if ((base.syncVarDirtyBits & 4) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_damageAreaCanMoveThisTurn);
		}
		if ((base.syncVarDirtyBits & 8) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_damageAreaCenterX);
		}
		if ((base.syncVarDirtyBits & 0x10) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_damageAreaCenterY);
		}
		if ((base.syncVarDirtyBits & 0x20) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_damageAreaFreePos);
		}
		if ((base.syncVarDirtyBits & 0x40) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)m_numNovaEffectsOnTurnStart);
		}
		if ((base.syncVarDirtyBits & 0x80) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(m_selfShieldLowHealthOnTurnStart);
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
			SyncListUInt.ReadReference(reader, m_actorsWithNovaCore);
			m_damageFieldLastCastTurn = (short)reader.ReadPackedUInt32();
			m_damageAreaCanMoveThisTurn = reader.ReadBoolean();
			m_damageAreaCenterX = (short)reader.ReadPackedUInt32();
			m_damageAreaCenterY = (short)reader.ReadPackedUInt32();
			m_damageAreaFreePos = reader.ReadVector3();
			m_numNovaEffectsOnTurnStart = (short)reader.ReadPackedUInt32();
			m_selfShieldLowHealthOnTurnStart = reader.ReadBoolean();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			SyncListUInt.ReadReference(reader, m_actorsWithNovaCore);
		}
		if ((num & 2) != 0)
		{
			m_damageFieldLastCastTurn = (short)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
		{
			m_damageAreaCanMoveThisTurn = reader.ReadBoolean();
		}
		if ((num & 8) != 0)
		{
			m_damageAreaCenterX = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x10) != 0)
		{
			m_damageAreaCenterY = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x20) != 0)
		{
			m_damageAreaFreePos = reader.ReadVector3();
		}
		if ((num & 0x40) != 0)
		{
			m_numNovaEffectsOnTurnStart = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x80) == 0)
		{
			return;
		}
		while (true)
		{
			m_selfShieldLowHealthOnTurnStart = reader.ReadBoolean();
			return;
		}
	}
}
