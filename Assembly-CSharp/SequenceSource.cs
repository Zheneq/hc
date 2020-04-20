using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SequenceSource
{
	private static uint s_nextID = 1U;

	private static Dictionary<uint, List<SequenceSource>> s_idsToSrcs = new Dictionary<uint, List<SequenceSource>>();

	private uint _rootID;

	private SequenceSource.ActorDelegate m_onHitActor;

	private SequenceSource.Vector3Delegate m_onHitPosition;

	private HashSet<Vector3> m_hitPositions = new HashSet<Vector3>();

	private HashSet<ActorData> m_hitActors = new HashSet<ActorData>();

	private int m_hitTurn = -1;

	private AbilityPriority m_hitPhase = AbilityPriority.INVALID;

	internal SequenceSource()
	{
	}

	internal SequenceSource(SequenceSource.ActorDelegate onHitActor, SequenceSource.Vector3Delegate onHitPosition, bool removeAtEndOfTurn = true, SequenceSource parentSource = null, IBitStream stream = null)
	{
		this.m_onHitActor = onHitActor;
		this.m_onHitPosition = onHitPosition;
		this.RemoveAtEndOfTurn = removeAtEndOfTurn;
		this.WaitForClientEnable = false;
		if (stream == null)
		{
			this.RootID = ((!(parentSource == null)) ? parentSource.RootID : SequenceSource.AllocateID());
		}
		else
		{
			this.OnSerializeHelper(stream);
		}
	}

	internal SequenceSource(SequenceSource.ActorDelegate onHitActor, SequenceSource.Vector3Delegate onHitPosition, uint rootID, bool removeAtEndOfTurn)
	{
		this.m_onHitActor = onHitActor;
		this.m_onHitPosition = onHitPosition;
		this.RootID = rootID;
		this.RemoveAtEndOfTurn = removeAtEndOfTurn;
	}

	private static uint AllocateID()
	{
		if (!NetworkServer.active)
		{
			if (NetworkClient.active)
			{
				Log.Error("Code Error: SequenceSource IDs should only be allocated on the server", new object[0]);
			}
		}
		return SequenceSource.s_nextID++;
	}

	internal uint RootID
	{
		get
		{
			return this._rootID;
		}
		private set
		{
			if (this._rootID == 0U)
			{
				if (value > 0U)
				{
					List<SequenceSource> list;
					if (SequenceSource.s_idsToSrcs.ContainsKey(value))
					{
						list = SequenceSource.s_idsToSrcs[value];
					}
					else
					{
						list = new List<SequenceSource>();
					}
					List<SequenceSource> list2 = list;
					list2.Add(this);
					SequenceSource.s_idsToSrcs[value] = list2;
				}
			}
			this._rootID = value;
		}
	}

	internal bool RemoveAtEndOfTurn { get; set; }

	internal bool WaitForClientEnable { get; private set; }

	internal void SetWaitForClientEnable(bool value)
	{
		this.WaitForClientEnable = value;
	}

	internal SequenceSource GetShallowCopy()
	{
		return (SequenceSource)base.MemberwiseClone();
	}

	~SequenceSource()
	{
		try
		{
			if (SequenceSource.s_idsToSrcs.ContainsKey(this._rootID))
			{
				List<SequenceSource> list = SequenceSource.s_idsToSrcs[this._rootID];
				list.Remove(this);
			}
		}
		finally
		{
		}
	}

	internal static void ClearStaticData()
	{
		SequenceSource.s_idsToSrcs.Clear();
	}

	internal void OnSerializeHelper(NetworkWriter stream)
	{
		this.OnSerializeHelper(new NetworkWriterAdapter(stream));
	}

	internal void OnSerializeHelper(IBitStream stream)
	{
		uint rootID = this.RootID;
		bool removeAtEndOfTurn = this.RemoveAtEndOfTurn;
		bool waitForClientEnable = this.WaitForClientEnable;
		stream.Serialize(ref rootID);
		stream.Serialize(ref removeAtEndOfTurn);
		stream.Serialize(ref waitForClientEnable);
		if (this.RootID != rootID)
		{
			this.RootID = rootID;
		}
		if (this.RemoveAtEndOfTurn != removeAtEndOfTurn)
		{
			this.RemoveAtEndOfTurn = removeAtEndOfTurn;
		}
		if (this.WaitForClientEnable != waitForClientEnable)
		{
			this.WaitForClientEnable = waitForClientEnable;
		}
	}

	internal void OnSequenceHit(Sequence seq, ActorData target, ActorModelData.ImpulseInfo impulseInfo, ActorModelData.RagdollActivation ragdollActivation = ActorModelData.RagdollActivation.HealthBased, bool tryHitReactIfAlreadyHit = true)
	{
		AbilityPriority currentAbilityPhase = ServerClientUtils.GetCurrentAbilityPhase();
		if (this.m_hitTurn == GameFlowData.Get().CurrentTurn)
		{
			if (this.m_hitPhase == currentAbilityPhase)
			{
				goto IL_73;
			}
		}
		this.m_hitTurn = GameFlowData.Get().CurrentTurn;
		this.m_hitPhase = currentAbilityPhase;
		this.m_hitPositions.Clear();
		this.m_hitActors.Clear();
		IL_73:
		bool flag = false;
		if (!this.m_hitActors.Contains(target))
		{
			if (this.m_onHitActor != null)
			{
				this.m_onHitActor(target);
			}
		}
		else
		{
			flag = true;
		}
		this.m_hitActors.Add(target);
		if (seq != null)
		{
			if (!tryHitReactIfAlreadyHit)
			{
				if (flag)
				{
					goto IL_F7;
				}
			}
			TheatricsManager.Get().OnSequenceHit(seq, target, impulseInfo, ragdollActivation);
		}
		IL_F7:
		if (SequenceManager.SequenceDebugTraceOn)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"<color=yellow>Sequence Actor Hit: </color><<color=lightblue>",
				seq.gameObject.name,
				" | ",
				seq.GetType(),
				"</color>> \nhit on: ",
				target.GetColoredDebugName("white"),
				" @time= ",
				Time.time
			}));
		}
	}

	internal void OnSequenceHit(Sequence seq, Vector3 position, ActorModelData.ImpulseInfo impulseInfo = null)
	{
		AbilityPriority currentAbilityPhase = ServerClientUtils.GetCurrentAbilityPhase();
		if (this.m_hitTurn == GameFlowData.Get().CurrentTurn)
		{
			if (this.m_hitPhase == currentAbilityPhase)
			{
				goto IL_67;
			}
		}
		this.m_hitTurn = GameFlowData.Get().CurrentTurn;
		this.m_hitPhase = currentAbilityPhase;
		this.m_hitPositions.Clear();
		this.m_hitActors.Clear();
		IL_67:
		if (!this.m_hitPositions.Contains(position))
		{
			this.m_hitPositions.Add(position);
			if (this.m_onHitPosition != null)
			{
				this.m_onHitPosition(position);
			}
		}
		if (SequenceManager.SequenceDebugTraceOn)
		{
			Debug.LogWarning(string.Concat(new object[]
			{
				"<color=yellow>Sequence Position Hit: </color><<color=lightblue>",
				seq.gameObject.name,
				" | ",
				seq.GetType(),
				"</color>> \nhit at: ",
				position.ToString(),
				" @time= ",
				Time.time
			}));
		}
	}

	internal static bool DidSequenceHit(SequenceSource src, ActorData target)
	{
		if (SequenceSource.s_idsToSrcs.ContainsKey(src.RootID))
		{
			List<SequenceSource> list = SequenceSource.s_idsToSrcs[src.RootID];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].m_hitActors.Contains(target))
				{
					return true;
				}
			}
		}
		return false;
	}

	internal static bool DidSequenceHit(SequenceSource src, Vector3 position)
	{
		if (SequenceSource.s_idsToSrcs.ContainsKey(src.RootID))
		{
			List<SequenceSource> list = SequenceSource.s_idsToSrcs[src.RootID];
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].m_hitPositions.Contains(position))
				{
					return true;
				}
			}
		}
		return false;
	}

	public string GetHitActorsString()
	{
		string text = string.Empty;
		using (HashSet<ActorData>.Enumerator enumerator = this.m_hitActors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData != null)
				{
					if (text.Length > 0)
					{
						text += " | ";
					}
					text += actorData.ActorIndex;
				}
			}
		}
		string str = "Did Hit Actor IDs: ";
		string str2;
		if (text.Length > 0)
		{
			str2 = text;
		}
		else
		{
			str2 = "(none)";
		}
		return str + str2;
	}

	public string GetHitPositionsString()
	{
		string text = string.Empty;
		using (HashSet<Vector3>.Enumerator enumerator = this.m_hitPositions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Vector3 vector = enumerator.Current;
				text = text + "\t" + vector.ToString() + "\n";
			}
		}
		return text;
	}

	public override bool Equals(object obj)
	{
		SequenceSource sequenceSource = obj as SequenceSource;
		if (sequenceSource == null)
		{
			return false;
		}
		return this.RootID == sequenceSource.RootID;
	}

	public bool Equals(SequenceSource p)
	{
		return this.RootID == p.RootID;
	}

	public static bool operator ==(SequenceSource a, SequenceSource b)
	{
		if (object.ReferenceEquals(a, b))
		{
			return true;
		}
		if (a != null)
		{
			if (b != null)
			{
				return a.RootID == b.RootID;
			}
		}
		return false;
	}

	public static bool operator !=(SequenceSource a, SequenceSource b)
	{
		return !(a == b);
	}

	public override int GetHashCode()
	{
		return (int)this.RootID;
	}

	internal delegate void ActorDelegate(ActorData target);

	internal delegate void Vector3Delegate(Vector3 position);
}
