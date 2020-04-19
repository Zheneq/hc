using System;

[Serializable]
public class QuestConfigOverride
{
	public int Index;

	public bool Enabled;

	public bool ShouldAbandon;

	public QuestConfigOverride Clone()
	{
		return (QuestConfigOverride)base.MemberwiseClone();
	}
}
