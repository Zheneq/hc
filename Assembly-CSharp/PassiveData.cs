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
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (passive.GetType() == passiveType)
				{
					return passive;
				}
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (passive.GetType() != typeof(T))
			{
				continue;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				return passive as T;
			}
		}
		return (T)null;
	}
}
