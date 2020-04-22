using UnityEngine;

public class GrydDetonateBomb : Ability
{
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (!(m_abilityName == "Base Ability"))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Detonate";
			return;
		}
	}
}
