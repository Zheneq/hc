using UnityEngine;

public class StopPkFxOnDestroy : MonoBehaviour
{
	public bool m_restartOnEnable;

	public bool m_killEffect;

	private bool s_quitting;

	private bool m_hasStarted;

	private void OnApplicationQuit()
	{
		s_quitting = true;
	}

	private void OnDisable()
	{
		if (s_quitting)
		{
			return;
		}
		PKFxFX[] componentsInChildren = GetComponentsInChildren<PKFxFX>(true);
		PKFxFX[] array = componentsInChildren;
		foreach (PKFxFX pKFxFX in array)
		{
			if (!(pKFxFX != null))
			{
				continue;
			}
			while (true)
			{
				switch (2)
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
			if (m_killEffect)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				pKFxFX.KillEffect();
			}
			else
			{
				pKFxFX.TerminateEffect();
			}
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void Start()
	{
		m_hasStarted = true;
	}

	private void StartEffects()
	{
		PKFxFX[] componentsInChildren = GetComponentsInChildren<PKFxFX>(true);
		PKFxFX[] array = componentsInChildren;
		foreach (PKFxFX pKFxFX in array)
		{
			if (pKFxFX != null)
			{
				while (true)
				{
					switch (3)
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
				pKFxFX.StartEffect();
			}
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void OnEnable()
	{
		if (!m_restartOnEnable)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_hasStarted)
			{
				StartEffects();
			}
			return;
		}
	}
}
