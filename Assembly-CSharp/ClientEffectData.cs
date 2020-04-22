using System.Collections.Generic;

internal class ClientEffectData
{
	public List<Sequence> m_sequences;

	public ActorData m_target;

	public List<StatusType> m_statuses;

	public ClientEffectData(List<Sequence> sequences, ActorData target, List<StatusType> statuses)
	{
		m_sequences = sequences;
		m_target = target;
		m_statuses = statuses;
	}
}
