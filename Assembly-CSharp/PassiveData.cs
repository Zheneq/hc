using System;
using UnityEngine;

public class PassiveData : MonoBehaviour
{
	public Passive[] m_passives;
	public string m_toolTip;
	public string m_toolTipTitle = "Passive";

	public Passive GetPassiveOfType(Type passiveType)
	{
		foreach (Passive passive in m_passives)
		{
			if (passive != null && passive.GetType() == passiveType)
			{
				return passive;
			}
		}
		return null;
	}

	public T GetPassiveOfType<T>() where T : Passive
	{
		foreach (Passive passive in m_passives)
		{
			if (passive != null && passive.GetType() == typeof(T))
			{
				return passive as T;
			}
		}
		return null;
	}
}
