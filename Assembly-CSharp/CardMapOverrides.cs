using System;
using UnityEngine;

public class CardMapOverrides : MonoBehaviour
{
	public CardMapOverride[] m_overrides;

	private static CardMapOverrides s_instance;

	private void Awake()
	{
		CardMapOverrides.s_instance = this;
	}

	private void OnDestroy()
	{
		CardMapOverrides.s_instance = null;
	}

	public static CardMapOverrides Get()
	{
		return CardMapOverrides.s_instance;
	}
}
