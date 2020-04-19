using System;
using System.Collections.Generic;

internal class ClientEffectData
{
	public List<Sequence> m_sequences;

	public ActorData m_target;

	public List<StatusType> m_statuses;

	public ClientEffectData(List<Sequence> sequences, ActorData target, List<StatusType> statuses)
	{
		this.m_sequences = sequences;
		this.m_target = target;
		this.m_statuses = statuses;
	}
}
