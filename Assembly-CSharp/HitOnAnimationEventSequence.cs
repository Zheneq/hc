using System;
using System.Collections.Generic;
using UnityEngine;

public class HitOnAnimationEventSequence : Sequence
{
	[AnimEventPicker]
	public UnityEngine.Object m_hitEvent;

	private int m_hitsSoFar;

	private List<List<ActorData>> m_hitTargetsList;

	private List<Vector3> m_hitPositionsList;

	internal override void Initialize(Sequence.IExtraSequenceParams[] extraParams)
	{
		foreach (Sequence.IExtraSequenceParams extraSequenceParams in extraParams)
		{
			HitOnAnimationEventSequence.ExtraParams extraParams2 = extraSequenceParams as HitOnAnimationEventSequence.ExtraParams;
			if (extraParams2 != null)
			{
				this.m_hitTargetsList = extraParams2.hitTargetsList;
				this.m_hitPositionsList = extraParams2.hitPositionsList;
			}
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(HitOnAnimationEventSequence.Initialize(Sequence.IExtraSequenceParams[])).MethodHandle;
		}
	}

	protected override void OnAnimationEvent(UnityEngine.Object parameter, GameObject sourceObject)
	{
		if (this.m_hitEvent != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HitOnAnimationEventSequence.OnAnimationEvent(UnityEngine.Object, GameObject)).MethodHandle;
			}
			if (this.m_hitEvent == parameter)
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
				this.HandleAnimationEvent();
			}
		}
	}

	private void HandleAnimationEvent()
	{
		if (this.m_hitPositionsList == null || this.m_hitTargetsList == null)
		{
			return;
		}
		if (this.m_hitPositionsList.Count != this.m_hitTargetsList.Count)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(HitOnAnimationEventSequence.HandleAnimationEvent()).MethodHandle;
			}
			Debug.LogError("HitOnAnimationEventSequences: mismatch between targets list and positions list");
		}
		if (this.m_hitsSoFar < this.m_hitPositionsList.Count)
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
			if (this.m_hitsSoFar < this.m_hitTargetsList.Count)
			{
				base.Source.OnSequenceHit(this, this.m_hitPositionsList[this.m_hitsSoFar], null);
				using (List<ActorData>.Enumerator enumerator = this.m_hitTargetsList[this.m_hitsSoFar].GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						base.Source.OnSequenceHit(this, actorData, Sequence.CreateImpulseInfoWithActorForward(actorData), ActorModelData.RagdollActivation.HealthBased, true);
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				this.m_hitsSoFar++;
			}
		}
	}

	public class ExtraParams : Sequence.IExtraSequenceParams
	{
		public List<List<ActorData>> hitTargetsList;

		public List<Vector3> hitPositionsList;

		public override void XSP_SerializeToStream(IBitStream stream)
		{
			int num;
			if (this.hitTargetsList != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(HitOnAnimationEventSequence.ExtraParams.XSP_SerializeToStream(IBitStream)).MethodHandle;
				}
				num = this.hitTargetsList.Count;
			}
			else
			{
				num = 0;
			}
			int num2 = num;
			stream.Serialize(ref num2);
			for (int i = 0; i < num2; i++)
			{
				List<ActorData> list = this.hitTargetsList[i];
				int num3;
				if (list != null)
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
					num3 = list.Count;
				}
				else
				{
					num3 = 0;
				}
				int num4 = num3;
				stream.Serialize(ref num4);
				for (int j = 0; j < num4; j++)
				{
					ActorData actorData = list[j];
					int num5;
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
						num5 = actorData.ActorIndex;
					}
					else
					{
						num5 = ActorData.s_invalidActorIndex;
					}
					int num6 = num5;
					stream.Serialize(ref num6);
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
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			int num7;
			if (this.hitPositionsList != null)
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
				num7 = this.hitPositionsList.Count;
			}
			else
			{
				num7 = 0;
			}
			int num8 = num7;
			stream.Serialize(ref num8);
			for (int k = 0; k < num8; k++)
			{
				Vector3 vector = this.hitPositionsList[k];
				stream.Serialize(ref vector);
			}
		}

		public override void XSP_DeserializeFromStream(IBitStream stream)
		{
			int num = 0;
			stream.Serialize(ref num);
			this.hitTargetsList = new List<List<ActorData>>(num);
			for (int i = 0; i < num; i++)
			{
				int num2 = 0;
				stream.Serialize(ref num2);
				List<ActorData> list = new List<ActorData>(num2);
				for (int j = 0; j < num2; j++)
				{
					int s_invalidActorIndex = ActorData.s_invalidActorIndex;
					stream.Serialize(ref s_invalidActorIndex);
					ActorData item = GameFlowData.Get().FindActorByActorIndex(s_invalidActorIndex);
					list.Add(item);
				}
				this.hitTargetsList.Add(list);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HitOnAnimationEventSequence.ExtraParams.XSP_DeserializeFromStream(IBitStream)).MethodHandle;
			}
			int num3 = 0;
			stream.Serialize(ref num3);
			this.hitPositionsList = new List<Vector3>(num3);
			for (int k = 0; k < num3; k++)
			{
				Vector3 zero = Vector3.zero;
				stream.Serialize(ref zero);
				this.hitPositionsList.Add(zero);
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
	}
}
