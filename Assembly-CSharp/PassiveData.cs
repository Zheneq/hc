using System;
using UnityEngine;

public class PassiveData : MonoBehaviour
{
	public Passive[] m_passives;

	public string m_toolTip;

	public string m_toolTipTitle = "Passive";

	public Passive GetPassiveOfType(Type passiveType)
	{
		foreach (Passive passive in this.m_passives)
		{
			if (passive != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(PassiveData.GetPassiveOfType(Type)).MethodHandle;
				}
				if (passive.GetType() == passiveType)
				{
					return passive;
				}
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		return null;
	}

	public T GetPassiveOfType<T>() where T : Passive
	{
		foreach (Passive passive in this.m_passives)
		{
			if (passive != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(PassiveData.GetPassiveOfType()).MethodHandle;
				}
				if (passive.GetType() == typeof(T))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					return passive as T;
				}
			}
		}
		return (T)((object)null);
	}
}
