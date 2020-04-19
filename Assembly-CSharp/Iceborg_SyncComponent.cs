using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AbilityContextNamespace;
using UnityEngine;
using UnityEngine.Networking;

public class Iceborg_SyncComponent : NetworkBehaviour
{
	public static ContextNameKeyPair s_cvarNovaCenter = new ContextNameKeyPair("NovaCenter");

	public static ContextNameKeyPair s_cvarHasNova = new ContextNameKeyPair("HasNova");

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

	private static int kListm_actorsWithNovaCore = 0x65C98853;

	static Iceborg_SyncComponent()
	{
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Iceborg_SyncComponent), Iceborg_SyncComponent.kListm_actorsWithNovaCore, new NetworkBehaviour.CmdDelegate(Iceborg_SyncComponent.InvokeSyncListm_actorsWithNovaCore));
		NetworkCRC.RegisterBehaviour("Iceborg_SyncComponent", 0);
	}

	private void Start()
	{
		this.m_cachedNovaTriggerDamage = this.m_delayedAoeOnHitData.GetFirstDamageValue();
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens)
	{
		this.m_delayedAoeOnHitData.AddTooltipTokens(tokens);
		if (this.m_delayedAoeShieldPerEnemyHit > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Iceborg_SyncComponent.AddTooltipTokens(List<TooltipTokenEntry>)).MethodHandle;
			}
			AbilityMod.AddToken_IntDiff(tokens, "NovaShieldPerHit", string.Empty, this.m_delayedAoeShieldPerEnemyHit, false, 0);
		}
		if (this.m_delayedAoeShieldPerExplosion > 0)
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
			AbilityMod.AddToken_IntDiff(tokens, "NovaShieldPerExplosion", string.Empty, this.m_delayedAoeShieldPerExplosion, false, 0);
		}
		if (this.m_delayedAoeEnergyPerEnemyHit > 0)
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
			AbilityMod.AddToken_IntDiff(tokens, "NovaEnergyPerHit", string.Empty, this.m_delayedAoeEnergyPerEnemyHit, false, 0);
		}
		if (this.m_delayedAoeEnergyPerExplosion > 0)
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
			AbilityMod.AddToken_IntDiff(tokens, "NovaEnergyPerExplosion", string.Empty, this.m_delayedAoeEnergyPerExplosion, false, 0);
		}
	}

	public bool HasNovaCore(ActorData actor)
	{
		return actor != null && this.m_actorsWithNovaCore.Contains((uint)actor.ActorIndex);
	}

	public void SetHasCoreContext_Client(Dictionary<ActorData, ActorHitContext> actorHitContext, ActorData targetActor, ActorData caster)
	{
		if (actorHitContext.ContainsKey(targetActor) && caster.\u000E() != targetActor.\u000E())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Iceborg_SyncComponent.SetHasCoreContext_Client(Dictionary<ActorData, ActorHitContext>, ActorData, ActorData)).MethodHandle;
			}
			bool flag = this.HasNovaCore(targetActor);
			ContextVars u = actorHitContext[targetActor].\u0015;
			int u001D = Iceborg_SyncComponent.s_cvarHasNova.\u0012();
			int u000E;
			if (flag)
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
				u000E = 1;
			}
			else
			{
				u000E = 0;
			}
			u.\u0016(u001D, u000E);
		}
	}

	public int GetTurnsSinceInitialCast()
	{
		int result = 0;
		if (this.m_damageFieldLastCastTurn > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Iceborg_SyncComponent.GetTurnsSinceInitialCast()).MethodHandle;
			}
			result = Mathf.Max(0, GameFlowData.Get().CurrentTurn - (int)this.m_damageFieldLastCastTurn);
		}
		return result;
	}

	public int GetNovaCoreTriggerDamage()
	{
		return this.m_cachedNovaTriggerDamage;
	}

	public string GetTargetPreviewAccessoryString(AbilityTooltipSymbol symbolType, Ability ability, ActorData targetActor, ActorData caster)
	{
		return null;
	}

	private void UNetVersion()
	{
	}

	public short Networkm_damageFieldLastCastTurn
	{
		get
		{
			return this.m_damageFieldLastCastTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<short>(value, ref this.m_damageFieldLastCastTurn, 2U);
		}
	}

	public bool Networkm_damageAreaCanMoveThisTurn
	{
		get
		{
			return this.m_damageAreaCanMoveThisTurn;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_damageAreaCanMoveThisTurn, 4U);
		}
	}

	public short Networkm_damageAreaCenterX
	{
		get
		{
			return this.m_damageAreaCenterX;
		}
		[param: In]
		set
		{
			base.SetSyncVar<short>(value, ref this.m_damageAreaCenterX, 8U);
		}
	}

	public short Networkm_damageAreaCenterY
	{
		get
		{
			return this.m_damageAreaCenterY;
		}
		[param: In]
		set
		{
			base.SetSyncVar<short>(value, ref this.m_damageAreaCenterY, 0x10U);
		}
	}

	public Vector3 Networkm_damageAreaFreePos
	{
		get
		{
			return this.m_damageAreaFreePos;
		}
		[param: In]
		set
		{
			base.SetSyncVar<Vector3>(value, ref this.m_damageAreaFreePos, 0x20U);
		}
	}

	public short Networkm_numNovaEffectsOnTurnStart
	{
		get
		{
			return this.m_numNovaEffectsOnTurnStart;
		}
		[param: In]
		set
		{
			base.SetSyncVar<short>(value, ref this.m_numNovaEffectsOnTurnStart, 0x40U);
		}
	}

	public bool Networkm_selfShieldLowHealthOnTurnStart
	{
		get
		{
			return this.m_selfShieldLowHealthOnTurnStart;
		}
		[param: In]
		set
		{
			base.SetSyncVar<bool>(value, ref this.m_selfShieldLowHealthOnTurnStart, 0x80U);
		}
	}

	protected static void InvokeSyncListm_actorsWithNovaCore(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			Debug.LogError("SyncList m_actorsWithNovaCore called on server.");
			return;
		}
		((Iceborg_SyncComponent)obj).m_actorsWithNovaCore.HandleMsg(reader);
	}

	private void Awake()
	{
		this.m_actorsWithNovaCore.InitializeBehaviour(this, Iceborg_SyncComponent.kListm_actorsWithNovaCore);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Iceborg_SyncComponent.OnSerialize(NetworkWriter, bool)).MethodHandle;
			}
			SyncListUInt.WriteInstance(writer, this.m_actorsWithNovaCore);
			writer.WritePackedUInt32((uint)this.m_damageFieldLastCastTurn);
			writer.Write(this.m_damageAreaCanMoveThisTurn);
			writer.WritePackedUInt32((uint)this.m_damageAreaCenterX);
			writer.WritePackedUInt32((uint)this.m_damageAreaCenterY);
			writer.Write(this.m_damageAreaFreePos);
			writer.WritePackedUInt32((uint)this.m_numNovaEffectsOnTurnStart);
			writer.Write(this.m_selfShieldLowHealthOnTurnStart);
			return true;
		}
		bool flag = false;
		if ((base.syncVarDirtyBits & 1U) != 0U)
		{
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
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, this.m_actorsWithNovaCore);
		}
		if ((base.syncVarDirtyBits & 2U) != 0U)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_damageFieldLastCastTurn);
		}
		if ((base.syncVarDirtyBits & 4U) != 0U)
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
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_damageAreaCanMoveThisTurn);
		}
		if ((base.syncVarDirtyBits & 8U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_damageAreaCenterX);
		}
		if ((base.syncVarDirtyBits & 0x10U) != 0U)
		{
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_damageAreaCenterY);
		}
		if ((base.syncVarDirtyBits & 0x20U) != 0U)
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
				flag = true;
			}
			writer.Write(this.m_damageAreaFreePos);
		}
		if ((base.syncVarDirtyBits & 0x40U) != 0U)
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
			if (!flag)
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
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.WritePackedUInt32((uint)this.m_numNovaEffectsOnTurnStart);
		}
		if ((base.syncVarDirtyBits & 0x80U) != 0U)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			writer.Write(this.m_selfShieldLowHealthOnTurnStart);
		}
		if (!flag)
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
			writer.WritePackedUInt32(base.syncVarDirtyBits);
		}
		return flag;
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		if (initialState)
		{
			SyncListUInt.ReadReference(reader, this.m_actorsWithNovaCore);
			this.m_damageFieldLastCastTurn = (short)reader.ReadPackedUInt32();
			this.m_damageAreaCanMoveThisTurn = reader.ReadBoolean();
			this.m_damageAreaCenterX = (short)reader.ReadPackedUInt32();
			this.m_damageAreaCenterY = (short)reader.ReadPackedUInt32();
			this.m_damageAreaFreePos = reader.ReadVector3();
			this.m_numNovaEffectsOnTurnStart = (short)reader.ReadPackedUInt32();
			this.m_selfShieldLowHealthOnTurnStart = reader.ReadBoolean();
			return;
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Iceborg_SyncComponent.OnDeserialize(NetworkReader, bool)).MethodHandle;
			}
			SyncListUInt.ReadReference(reader, this.m_actorsWithNovaCore);
		}
		if ((num & 2) != 0)
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
			this.m_damageFieldLastCastTurn = (short)reader.ReadPackedUInt32();
		}
		if ((num & 4) != 0)
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
			this.m_damageAreaCanMoveThisTurn = reader.ReadBoolean();
		}
		if ((num & 8) != 0)
		{
			this.m_damageAreaCenterX = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x10) != 0)
		{
			this.m_damageAreaCenterY = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x20) != 0)
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
			this.m_damageAreaFreePos = reader.ReadVector3();
		}
		if ((num & 0x40) != 0)
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
			this.m_numNovaEffectsOnTurnStart = (short)reader.ReadPackedUInt32();
		}
		if ((num & 0x80) != 0)
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
			this.m_selfShieldLowHealthOnTurnStart = reader.ReadBoolean();
		}
	}
}
