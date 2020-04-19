using System;

[Serializable]
public class ConversationTemplate
{
	public CharacterType m_initiator;

	public CharacterType m_responder;

	public ConversationLine[] m_lines;
}
