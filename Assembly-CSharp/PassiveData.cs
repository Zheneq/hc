using System;
using UnityEngine;

public class PassiveData : MonoBehaviour
{
	public Passive[] m_passives;

	public string m_toolTip;

	public string m_toolTipTitle = "Passive";

	public Passive GetPassiveOfType(Type passiveType)
	{
		Passive[] passives = m_passives;
		foreach (Passive passive in passives)
		{
			if (passive != null)
			{
				if (passive.GetType() == passiveType)
				{
					return passive;
				}
			}
		}
		while (true)
		{
			return null;
		}
	}

	public T GetPassiveOfType<T>() where T : Passive
	{
		Passive[] passives = m_passives;
		foreach (Passive passive in passives)
		{
			if (!(passive != null))
			{
				continue;
			}
			if (passive.GetType() != typeof(T))
			{
				continue;
			}
			while (true)
			{
				return passive as T;
			}
		}
		return (T)null;
	}
}
