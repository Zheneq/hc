using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SequenceSource
{
	internal delegate void ActorDelegate(ActorData target);

	internal delegate void Vector3Delegate(Vector3 position);

	private static uint s_nextID = 1u;

	private static Dictionary<uint, List<SequenceSource>> s_idsToSrcs = new Dictionary<uint, List<SequenceSource>>();

	private uint _rootID;

	private ActorDelegate m_onHitActor;

	private Vector3Delegate m_onHitPosition;

	private HashSet<Vector3> m_hitPositions = new HashSet<Vector3>();

	private HashSet<ActorData> m_hitActors = new HashSet<ActorData>();

	private int m_hitTurn = -1;

	private AbilityPriority m_hitPhase = AbilityPriority.INVALID;

	internal uint RootID
	{
		get
		{
			return _rootID;
		}
		private set
		{
			if (_rootID == 0)
			{
				if (value != 0)
				{
					List<SequenceSource> list;
					if (s_idsToSrcs.ContainsKey(value))
					{
						list = s_idsToSrcs[value];
					}
					else
					{
						list = new List<SequenceSource>();
					}
					List<SequenceSource> list2 = list;
					list2.Add(this);
					s_idsToSrcs[value] = list2;
				}
			}
			_rootID = value;
		}
	}

	internal bool RemoveAtEndOfTurn
	{
		get;
		set;
	}

	internal bool WaitForClientEnable
	{
		get;
		private set;
	}

	internal SequenceSource()
	{
	}

	internal SequenceSource(ActorDelegate onHitActor, Vector3Delegate onHitPosition, bool removeAtEndOfTurn = true, SequenceSource parentSource = null, IBitStream stream = null)
	{
		m_onHitActor = onHitActor;
		m_onHitPosition = onHitPosition;
		RemoveAtEndOfTurn = removeAtEndOfTurn;
		WaitForClientEnable = false;
		if (stream == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					RootID = ((!(parentSource == null)) ? parentSource.RootID : AllocateID());
					return;
				}
			}
		}
		OnSerializeHelper(stream);
	}

	internal SequenceSource(ActorDelegate onHitActor, Vector3Delegate onHitPosition, uint rootID, bool removeAtEndOfTurn)
	{
		m_onHitActor = onHitActor;
		m_onHitPosition = onHitPosition;
		RootID = rootID;
		RemoveAtEndOfTurn = removeAtEndOfTurn;
	}

	private static uint AllocateID()
	{
		if (!NetworkServer.active)
		{
			if (NetworkClient.active)
			{
				Log.Error("Code Error: SequenceSource IDs should only be allocated on the server");
			}
		}
		return s_nextID++;
	}

	internal void SetWaitForClientEnable(bool value)
	{
		WaitForClientEnable = value;
	}

	internal SequenceSource GetShallowCopy()
	{
		return (SequenceSource)MemberwiseClone();
	}

	~SequenceSource()
	{
		if (s_idsToSrcs.ContainsKey(_rootID))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					List<SequenceSource> list = s_idsToSrcs[_rootID];
					list.Remove(this);
					return;
				}
				}
			}
		}
	}

	internal static void ClearStaticData()
	{
		s_idsToSrcs.Clear();
	}

	internal void OnSerializeHelper(NetworkWriter stream)
	{
		OnSerializeHelper(new NetworkWriterAdapter(stream));
	}

	public string Json()
	{
		return $"{{ \"rootID\": {RootID}, \"removeAtEndOfTurn\": {RemoveAtEndOfTurn}, \"waitForClientEnable\": {WaitForClientEnable}}}";
	}

	internal void OnSerializeHelper(IBitStream stream)
	{
		uint value = RootID;
		bool value2 = RemoveAtEndOfTurn;
		bool value3 = WaitForClientEnable;
		stream.Serialize(ref value);
		stream.Serialize(ref value2);
		stream.Serialize(ref value3);
		if (RootID != value)
		{
			RootID = value;
		}
		if (RemoveAtEndOfTurn != value2)
		{
			RemoveAtEndOfTurn = value2;
		}
		if (WaitForClientEnable != value3)
		{
			WaitForClientEnable = value3;
		}
	}

	internal void OnSequenceHit(Sequence seq, ActorData target, ActorModelData.ImpulseInfo impulseInfo, ActorModelData.RagdollActivation ragdollActivation = ActorModelData.RagdollActivation.HealthBased, bool tryHitReactIfAlreadyHit = true)
	{
		AbilityPriority currentAbilityPhase = ServerClientUtils.GetCurrentAbilityPhase();
		if (m_hitTurn == GameFlowData.Get().CurrentTurn)
		{
			if (m_hitPhase == currentAbilityPhase)
			{
				goto IL_0073;
			}
		}
		m_hitTurn = GameFlowData.Get().CurrentTurn;
		m_hitPhase = currentAbilityPhase;
		m_hitPositions.Clear();
		m_hitActors.Clear();
		goto IL_0073;
		IL_0073:
		bool flag = false;
		if (!m_hitActors.Contains(target))
		{
			if (m_onHitActor != null)
			{
				m_onHitActor(target);
			}
		}
		else
		{
			flag = true;
		}
		m_hitActors.Add(target);
		if (seq != null)
		{
			if (!tryHitReactIfAlreadyHit)
			{
				if (flag)
				{
					goto IL_00f7;
				}
			}
			TheatricsManager.Get().OnSequenceHit(seq, target, impulseInfo, ragdollActivation);
		}
		goto IL_00f7;
		IL_00f7:
		if (SequenceManager.SequenceDebugTraceOn)
		{
			Debug.LogWarning(string.Concat("<color=yellow>Sequence Actor Hit: </color><<color=lightblue>", seq.gameObject.name, " | ", seq.GetType(), "</color>> \nhit on: ", target.DebugNameString("white"), " @time= ", Time.time));
		}
	}

	internal void OnSequenceHit(Sequence seq, Vector3 position, ActorModelData.ImpulseInfo impulseInfo = null)
	{
		AbilityPriority currentAbilityPhase = ServerClientUtils.GetCurrentAbilityPhase();
		if (m_hitTurn == GameFlowData.Get().CurrentTurn)
		{
			if (m_hitPhase == currentAbilityPhase)
			{
				goto IL_0067;
			}
		}
		m_hitTurn = GameFlowData.Get().CurrentTurn;
		m_hitPhase = currentAbilityPhase;
		m_hitPositions.Clear();
		m_hitActors.Clear();
		goto IL_0067;
		IL_0067:
		if (!m_hitPositions.Contains(position))
		{
			m_hitPositions.Add(position);
			if (m_onHitPosition != null)
			{
				m_onHitPosition(position);
			}
		}
		if (!SequenceManager.SequenceDebugTraceOn)
		{
			return;
		}
		while (true)
		{
			Debug.LogWarning(string.Concat("<color=yellow>Sequence Position Hit: </color><<color=lightblue>", seq.gameObject.name, " | ", seq.GetType(), "</color>> \nhit at: ", position.ToString(), " @time= ", Time.time));
			return;
		}
	}

	internal static bool DidSequenceHit(SequenceSource src, ActorData target)
	{
		if (s_idsToSrcs.ContainsKey(src.RootID))
		{
			List<SequenceSource> list = s_idsToSrcs[src.RootID];
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
		if (s_idsToSrcs.ContainsKey(src.RootID))
		{
			List<SequenceSource> list = s_idsToSrcs[src.RootID];
			for (int i = 0; i < list.Count; i++)
			{
				if (!list[i].m_hitPositions.Contains(position))
				{
					continue;
				}
				while (true)
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
		using (HashSet<ActorData>.Enumerator enumerator = m_hitActors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (current != null)
				{
					if (text.Length > 0)
					{
						text += " | ";
					}
					text += current.ActorIndex;
				}
			}
		}
		object str;
		if (text.Length > 0)
		{
			str = text;
		}
		else
		{
			str = "(none)";
		}
		return "Did Hit Actor IDs: " + (string)str;
	}

	public string GetHitPositionsString()
	{
		string text = string.Empty;
		using (HashSet<Vector3>.Enumerator enumerator = m_hitPositions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				text = text + "\t" + enumerator.Current.ToString() + "\n";
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return text;
					}
					/*OpCode not supported: LdMemberToken*/;
					return text;
				}
			}
		}
	}

	public override bool Equals(object obj)
	{
		SequenceSource sequenceSource = obj as SequenceSource;
		if ((object)sequenceSource == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return RootID == sequenceSource.RootID;
	}

	public bool Equals(SequenceSource p)
	{
		return RootID == p.RootID;
	}

	public static bool operator ==(SequenceSource a, SequenceSource b)
	{
		if (object.ReferenceEquals(a, b))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if ((object)a != null)
		{
			if ((object)b != null)
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
		return (int)RootID;
	}
}
