using UnityEngine;
using UnityEngine.Networking;

public class SatelliteController : NetworkBehaviour
{
	public GameObject[] m_satelliteModelDataPrefabs;

	private PrefabResourceLink[] m_overrideSatelliteResourceLinks;

	protected PersistentSatellite[] m_satelliteInstances;

	private bool m_createdSatellites;

	private bool m_delayedOnAssignedToBoardSquare;

	private SerializeHelper m_serializeHelper = new SerializeHelper();

	private void SetupPrefabsFromOverrides()
	{
		for (int i = 0; i < m_satelliteModelDataPrefabs.Length; i++)
		{
			if (i >= m_overrideSatelliteResourceLinks.Length)
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
			if (m_overrideSatelliteResourceLinks[i] != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				m_satelliteModelDataPrefabs[i] = m_overrideSatelliteResourceLinks[i].GetPrefab();
			}
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void CreateSatellites()
	{
		if (m_createdSatellites)
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
			m_satelliteInstances = new PersistentSatellite[m_satelliteModelDataPrefabs.Length];
			for (int i = 0; i < m_satelliteModelDataPrefabs.Length; i++)
			{
				GameObject gameObject = Object.Instantiate(m_satelliteModelDataPrefabs[i]);
				if ((bool)gameObject)
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
					m_satelliteInstances[i] = gameObject.GetComponent<PersistentSatellite>();
					if (m_satelliteInstances[i] != null)
					{
						int layer = LayerMask.NameToLayer("Actor");
						Transform[] componentsInChildren = m_satelliteInstances[i].gameObject.GetComponentsInChildren<Transform>(true);
						foreach (Transform transform in componentsInChildren)
						{
							transform.gameObject.layer = layer;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				if (m_satelliteInstances[i] != null)
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
					m_satelliteInstances[i].Setup(this);
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				m_createdSatellites = true;
				return;
			}
		}
	}

	public PersistentSatellite GetSatellite(int index)
	{
		PersistentSatellite result = null;
		if (m_satelliteInstances != null)
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
			if (index < m_satelliteInstances.Length)
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
				result = m_satelliteInstances[index];
			}
		}
		return result;
	}

	public void OnAssignedToInitialBoardSquare()
	{
		if (m_createdSatellites)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					for (int i = 0; i < m_satelliteInstances.Length; i++)
					{
						if (m_satelliteInstances[i] != null)
						{
							m_satelliteInstances[i].OnAssignedToInitialBoardSquare();
						}
					}
					while (true)
					{
						switch (1)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
				}
			}
		}
		m_delayedOnAssignedToBoardSquare = true;
	}

	private void OnRespawn()
	{
		if (m_satelliteInstances == null)
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
			for (int i = 0; i < m_satelliteInstances.Length; i++)
			{
				if (m_satelliteInstances[i] != null)
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
					m_satelliteInstances[i].OnRespawn();
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
	}

	public void OnActorDeath()
	{
		if (m_satelliteInstances == null)
		{
			return;
		}
		for (int i = 0; i < m_satelliteInstances.Length; i++)
		{
			if (m_satelliteInstances[i] != null)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_satelliteInstances[i].OnActorDeath();
			}
		}
	}

	private void Update()
	{
		if (!m_delayedOnAssignedToBoardSquare || !m_createdSatellites)
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
			m_delayedOnAssignedToBoardSquare = false;
			OnAssignedToInitialBoardSquare();
			return;
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		return OnSerializeHelper(new NetworkWriterAdapter(writer), initialState);
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint num = uint.MaxValue;
		if (!initialState)
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
			num = reader.ReadPackedUInt32();
		}
		if (num == 0)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			OnSerializeHelper(new NetworkReaderAdapter(reader), initialState);
			return;
		}
	}

	private bool OnSerializeHelper(IBitStream stream, bool initialState)
	{
		if (!initialState)
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
			if (m_serializeHelper.ShouldReturnImmediately(ref stream))
			{
				return false;
			}
		}
		int value = 0;
		if (stream.isWriting)
		{
			value = m_overrideSatelliteResourceLinks.Length;
			stream.Serialize(ref value);
			for (int i = 0; i < value; i++)
			{
				PrefabResourceLink.Stream(stream, ref m_overrideSatelliteResourceLinks[i]);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (stream.isReading)
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
			stream.Serialize(ref value);
			m_overrideSatelliteResourceLinks = new PrefabResourceLink[value];
			for (int j = 0; j < value; j++)
			{
				PrefabResourceLink.Stream(stream, ref m_overrideSatelliteResourceLinks[j]);
			}
			SetupPrefabsFromOverrides();
			CreateSatellites();
		}
		return m_serializeHelper.End(initialState, base.syncVarDirtyBits);
	}

	private void UNetVersion()
	{
	}
}
