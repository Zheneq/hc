using AbilityContextNamespace;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Fireborg_SyncComponent : NetworkBehaviour
{
	[Separator("Ignited Effect", true)]
	public StandardActorEffectData m_ignitedEffectData;

	public int m_ignitedTriggerDamage = 5;

	public StandardEffectInfo m_ignitedTriggerEffect;

	public int m_ignitedTriggerEnergyOnCaster;

	[Separator("Ground Fire Effect", true)]
	public int m_groundFireDamageNormal = 6;

	public int m_groundFireDamageSuperheated = 8;

	public StandardEffectInfo m_groundFireEffect;

	public bool m_groundFireAddsIgniteIfSuperheated = true;

	[Separator("Sequences", true)]
	public GameObject m_groundFirePerSquareSeqPrefab;

	public GameObject m_groundFireOnHitSeqPrefab;

	[Header("-- Superheated versions")]
	public GameObject m_superheatedGroundFireSquareSeqPrefab;

	[SyncVar]
	internal int m_superheatLastCastTurn;

	internal SyncListUInt m_actorsInGroundFireOnTurnStart = new SyncListUInt();

	private AbilityData m_abilityData;

	private FireborgSuperheat m_superheatAbility;

	private AbilityData.ActionType m_superheatActionType = AbilityData.ActionType.INVALID_ACTION;

	public static ContextNameKeyPair s_cvarSuperheated;

	private HashSet<ActorData> m_ignitedActorsThisTurn = new HashSet<ActorData>();

	private static int kListm_actorsInGroundFireOnTurnStart;

	public int Networkm_superheatLastCastTurn
	{
		get
		{
			return m_superheatLastCastTurn;
		}
		[param: In]
		set
		{
			SetSyncVar(value, ref m_superheatLastCastTurn, 1u);
		}
	}

	static Fireborg_SyncComponent()
	{
		s_cvarSuperheated = new ContextNameKeyPair("Superheated");
		kListm_actorsInGroundFireOnTurnStart = 1427115255;
		NetworkBehaviour.RegisterSyncListDelegate(typeof(Fireborg_SyncComponent), kListm_actorsInGroundFireOnTurnStart, InvokeSyncListm_actorsInGroundFireOnTurnStart);
		NetworkCRC.RegisterBehaviour("Fireborg_SyncComponent", 0);
	}

	public void ResetIgnitedActorsTrackingThisTurn()
	{
		m_ignitedActorsThisTurn.Clear();
	}

	private void Start()
	{
		m_abilityData = GetComponent<AbilityData>();
		m_superheatAbility = m_abilityData.GetAbilityOfType<FireborgSuperheat>();
		if (!(m_superheatAbility != null))
		{
			return;
		}
		while (true)
		{
			m_superheatActionType = m_abilityData.GetActionTypeOfAbility(m_superheatAbility);
			return;
		}
	}

	public static string GetSuperheatedCvarUsage()
	{
		return ContextVars.GetDebugString(s_cvarSuperheated.GetName(), "1 if caster is in Superheated mode, 0 otherwise", false);
	}

	public bool InSuperheatMode()
	{
		if (m_superheatAbility != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					bool flag = false;
					int currentTurn = GameFlowData.Get().CurrentTurn;
					int superheatDuration = m_superheatAbility.GetSuperheatDuration();
					if (m_superheatLastCastTurn > 0)
					{
						flag = (currentTurn < m_superheatLastCastTurn + superheatDuration);
					}
					int result;
					if (!flag)
					{
						result = (m_abilityData.HasQueuedAction(m_superheatActionType) ? 1 : 0);
					}
					else
					{
						result = 1;
					}
					return (byte)result != 0;
				}
				}
			}
		}
		return false;
	}

	public void SetSuperheatedContextVar(ContextVars abilityContext)
	{
		bool flag = InSuperheatMode();
		abilityContext.SetInt(s_cvarSuperheated.GetHash(), flag ? 1 : 0);
	}

	public void AddGroundFireTargetingNumber(ActorData target, ActorData caster, TargetingNumberUpdateScratch results)
	{
		if (target.GetTeam() == caster.GetTeam())
		{
			return;
		}
		while (true)
		{
			int num;
			if (InSuperheatMode())
			{
				num = m_groundFireDamageSuperheated;
			}
			else
			{
				num = m_groundFireDamageNormal;
			}
			int num2 = num;
			if (num2 > 0)
			{
				while (true)
				{
					results.m_damage += num2;
					return;
				}
			}
			return;
		}
	}

	public string GetTargetPreviewAccessoryString(AbilityTooltipSymbol symbolType, Ability ability, ActorData targetActor, ActorData caster)
	{
		if (symbolType == AbilityTooltipSymbol.Damage)
		{
			int num;
			if (InSuperheatMode())
			{
				num = m_groundFireDamageSuperheated;
			}
			else
			{
				num = m_groundFireDamageNormal;
			}
			int num2 = num;
			if (num2 > 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return "\n+ " + AbilityUtils.CalculateDamageForTargeter(caster, targetActor, ability, num2, false);
					}
				}
			}
		}
		return null;
	}

	private void UNetVersion()
	{
	}

	protected static void InvokeSyncListm_actorsInGroundFireOnTurnStart(NetworkBehaviour obj, NetworkReader reader)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					Debug.LogError("SyncList m_actorsInGroundFireOnTurnStart called on server.");
					return;
				}
			}
		}
		((Fireborg_SyncComponent)obj).m_actorsInGroundFireOnTurnStart.HandleMsg(reader);
	}

	private void Awake()
	{
		m_actorsInGroundFireOnTurnStart.InitializeBehaviour(this, kListm_actorsInGroundFireOnTurnStart);
	}

	public override bool OnSerialize(NetworkWriter writer, bool forceAll)
	{
		if (forceAll)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					writer.WritePackedUInt32((uint)m_superheatLastCastTurn);
					SyncListUInt.WriteInstance(writer, m_actorsInGroundFireOnTurnStart);
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
			writer.WritePackedUInt32((uint)m_superheatLastCastTurn);
		}
		if ((base.syncVarDirtyBits & 2) != 0)
		{
			if (!flag)
			{
				writer.WritePackedUInt32(base.syncVarDirtyBits);
				flag = true;
			}
			SyncListUInt.WriteInstance(writer, m_actorsInGroundFireOnTurnStart);
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_superheatLastCastTurn = (int)reader.ReadPackedUInt32();
					SyncListUInt.ReadReference(reader, m_actorsInGroundFireOnTurnStart);
					return;
				}
			}
		}
		int num = (int)reader.ReadPackedUInt32();
		if ((num & 1) != 0)
		{
			m_superheatLastCastTurn = (int)reader.ReadPackedUInt32();
		}
		if ((num & 2) == 0)
		{
			return;
		}
		while (true)
		{
			SyncListUInt.ReadReference(reader, m_actorsInGroundFireOnTurnStart);
			return;
		}
	}
}
