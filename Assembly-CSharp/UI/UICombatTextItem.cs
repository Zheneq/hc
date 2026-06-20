using UnityEngine;
using UnityEngine.UI;

public class UICombatTextItem : MonoBehaviour
{
	public Text m_label;

	public string m_text;

	public CombatTextCategory m_category;

	public float m_textVisibleTime = 2f;

	public float m_alphaStartTime = 1.5f;

	public float m_firstLineSpacing = 0.15f;

	public float m_lineSpacing = 0.035f;

	public float m_firstLineMoveTime = 1.5f;

	public float m_moveTime = 0.75f;

	internal ActorData Actor
	{
		get;
		private set;
	}
}
