using System;
using UnityEngine;

public class GrydDetonateBomb : Ability
{
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GrydDetonateBomb.Start()).MethodHandle;
			}
			this.m_abilityName = "Detonate";
		}
	}
}
