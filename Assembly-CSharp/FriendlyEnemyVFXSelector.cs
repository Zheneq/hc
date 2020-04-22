using UnityEngine;

public class FriendlyEnemyVFXSelector : MonoBehaviour
{
	public GameObject m_friendlyVFX;

	public GameObject m_enemyVFX;

	public GameObject m_sharedVFX;

	public bool m_hideByMoveOffScreen;

	private Team m_casterTeam;

	private Team m_cachedLocalPlayerTeam;

	private bool m_friendly = true;

	private bool m_initialized;

	private GameObject m_allyFxInstance;

	private GameObject m_enemyFxInstance;

	private GameObject m_currentSharedVFX;

	private void UpdateVFX()
	{
		if (m_friendly == IsFriendly(m_casterTeam, m_cachedLocalPlayerTeam))
		{
			if (m_initialized)
			{
				return;
			}
			while (true)
			{
				switch (4)
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
		}
		m_initialized = true;
		m_friendly = IsFriendly(m_casterTeam, m_cachedLocalPlayerTeam);
		if (m_hideByMoveOffScreen)
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
			if (m_friendly)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_enemyFxInstance != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					HideFxObject(m_enemyFxInstance);
				}
			}
			else if (m_allyFxInstance != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				HideFxObject(m_allyFxInstance);
			}
		}
		else if (m_friendly)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_enemyFxInstance != null)
			{
				Object.Destroy(m_enemyFxInstance);
				m_enemyFxInstance = null;
			}
		}
		else if (m_allyFxInstance != null)
		{
			Object.Destroy(m_allyFxInstance);
			m_allyFxInstance = null;
		}
		GameObject gameObject;
		if (m_friendly)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			gameObject = m_friendlyVFX;
		}
		else
		{
			gameObject = m_enemyVFX;
		}
		GameObject gameObject2 = gameObject;
		if (!(gameObject2 != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (m_friendly)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						CreateInstanceIfNeeded(gameObject2, ref m_allyFxInstance);
						return;
					}
				}
			}
			CreateInstanceIfNeeded(gameObject2, ref m_enemyFxInstance);
			return;
		}
	}

	private void HideFxObject(GameObject obj)
	{
		if (!(obj != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			obj.transform.localPosition = new Vector3(-10000f, -10000f, -10000f);
			return;
		}
	}

	private void CreateInstanceIfNeeded(GameObject fxPrefab, ref GameObject instance)
	{
		if (instance == null)
		{
			while (true)
			{
				switch (1)
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
			if (fxPrefab != null)
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
				instance = Object.Instantiate(fxPrefab);
				instance.transform.parent = base.transform;
				instance.gameObject.SetLayerRecursively(LayerMask.NameToLayer("DynamicLit"));
			}
		}
		if (!(instance != null))
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
			instance.transform.localPosition = Vector3.zero;
			instance.transform.localRotation = Quaternion.identity;
			return;
		}
	}

	public void Setup(Team casterTeam)
	{
		m_casterTeam = casterTeam;
		int cachedLocalPlayerTeam;
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			while (true)
			{
				switch (5)
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
			cachedLocalPlayerTeam = (int)GameFlowData.Get().activeOwnedActorData.GetTeam();
		}
		else
		{
			cachedLocalPlayerTeam = 0;
		}
		m_cachedLocalPlayerTeam = (Team)cachedLocalPlayerTeam;
		if (m_currentSharedVFX == null && m_sharedVFX != null)
		{
			m_currentSharedVFX = Object.Instantiate(m_sharedVFX);
			m_currentSharedVFX.transform.parent = base.transform;
			m_currentSharedVFX.transform.localPosition = Vector3.zero;
			m_currentSharedVFX.transform.localRotation = Quaternion.identity;
			m_currentSharedVFX.gameObject.SetLayerRecursively(LayerMask.NameToLayer("DynamicLit"));
		}
		UpdateVFX();
	}

	public void SetAttribute(string attributeName, float value)
	{
		GameObject currentActiveVfxInstance = GetCurrentActiveVfxInstance();
		if (!(currentActiveVfxInstance != null))
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
			Sequence.SetAttribute(currentActiveVfxInstance, attributeName, value);
			return;
		}
	}

	private GameObject GetCurrentActiveVfxInstance()
	{
		GameObject result;
		if (m_friendly)
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
			result = m_allyFxInstance;
		}
		else
		{
			result = m_enemyFxInstance;
		}
		return result;
	}

	private void Update()
	{
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			int num;
			if (GameFlowData.Get().activeOwnedActorData != null)
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
				num = (int)GameFlowData.Get().activeOwnedActorData.GetTeam();
			}
			else
			{
				num = 0;
			}
			Team team = (Team)num;
			if (team != m_cachedLocalPlayerTeam)
			{
				m_cachedLocalPlayerTeam = team;
				UpdateVFX();
			}
			return;
		}
	}

	private static bool IsFriendly(Team team0, Team team1)
	{
		int result;
		if (team0 != team1)
		{
			if (team0 != Team.Invalid)
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
				result = ((team1 == Team.Invalid) ? 1 : 0);
			}
			else
			{
				result = 1;
			}
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}
}
