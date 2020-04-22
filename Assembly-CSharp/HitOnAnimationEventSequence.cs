using System.Collections.Generic;
using UnityEngine;

public class HitOnAnimationEventSequence : Sequence
{
	public class ExtraParams : IExtraSequenceParams
	{
		public List<List<ActorData>> hitTargetsList;

		public List<Vector3> hitPositionsList;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			int num;
			if (hitTargetsList != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num = hitTargetsList.Count;
			}
			else
			{
				num = 0;
			}
			int value = num;
			stream.Serialize(ref value);
			for (int i = 0; i < value; i++)
			{
				List<ActorData> list = hitTargetsList[i];
				int num2;
				if (list != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					num2 = list.Count;
				}
				else
				{
					num2 = 0;
				}
				int value2 = num2;
				stream.Serialize(ref value2);
				for (int j = 0; j < value2; j++)
				{
					ActorData actorData = list[j];
					int num3;
					if (actorData != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						num3 = actorData.ActorIndex;
					}
					else
					{
						num3 = ActorData.s_invalidActorIndex;
					}
					int value3 = num3;
					stream.Serialize(ref value3);
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						goto end_IL_00b3;
					}
					continue;
					end_IL_00b3:
					break;
				}
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				int num4;
				if (hitPositionsList != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					num4 = hitPositionsList.Count;
				}
				else
				{
					num4 = 0;
				}
				int value4 = num4;
				stream.Serialize(ref value4);
				for (int k = 0; k < value4; k++)
				{
					Vector3 value5 = hitPositionsList[k];
					stream.Serialize(ref value5);
				}
				return;
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			int value = 0;
			stream.Serialize(ref value);
			hitTargetsList = new List<List<ActorData>>(value);
			for (int i = 0; i < value; i++)
			{
				int value2 = 0;
				stream.Serialize(ref value2);
				List<ActorData> list = new List<ActorData>(value2);
				for (int j = 0; j < value2; j++)
				{
					int value3 = ActorData.s_invalidActorIndex;
					stream.Serialize(ref value3);
					ActorData item = GameFlowData.Get().FindActorByActorIndex(value3);
					list.Add(item);
				}
				hitTargetsList.Add(list);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				int value4 = 0;
				stream.Serialize(ref value4);
				hitPositionsList = new List<Vector3>(value4);
				for (int k = 0; k < value4; k++)
				{
					Vector3 value5 = Vector3.zero;
					stream.Serialize(ref value5);
					hitPositionsList.Add(value5);
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
	}

	[AnimEventPicker]
	public Object m_hitEvent;

	private int m_hitsSoFar;

	private List<List<ActorData>> m_hitTargetsList;

	private List<Vector3> m_hitPositionsList;

	internal override void Initialize(IExtraSequenceParams[] extraParams)
	{
		foreach (IExtraSequenceParams extraSequenceParams in extraParams)
		{
			ExtraParams extraParams2 = extraSequenceParams as ExtraParams;
			if (extraParams2 != null)
			{
				m_hitTargetsList = extraParams2.hitTargetsList;
				m_hitPositionsList = extraParams2.hitPositionsList;
			}
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
	{
		if (!(m_hitEvent != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_hitEvent == parameter)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					HandleAnimationEvent();
					return;
				}
			}
			return;
		}
	}

	private void HandleAnimationEvent()
	{
		if (m_hitPositionsList == null || m_hitTargetsList == null)
		{
			return;
		}
		if (m_hitPositionsList.Count != m_hitTargetsList.Count)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Debug.LogError("HitOnAnimationEventSequences: mismatch between targets list and positions list");
		}
		if (m_hitsSoFar >= m_hitPositionsList.Count)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (m_hitsSoFar < m_hitTargetsList.Count)
			{
				base.Source.OnSequenceHit(this, m_hitPositionsList[m_hitsSoFar]);
				using (List<ActorData>.Enumerator enumerator = m_hitTargetsList[m_hitsSoFar].GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData current = enumerator.Current;
						base.Source.OnSequenceHit(this, current, Sequence.CreateImpulseInfoWithActorForward(current));
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				m_hitsSoFar++;
			}
			return;
		}
	}
}
