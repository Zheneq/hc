using UnityEngine;

public class CardMapOverrides : MonoBehaviour
{
	public CardMapOverride[] m_overrides;

	private static CardMapOverrides s_instance;

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public static CardMapOverrides Get()
	{
		return s_instance;
	}
}
