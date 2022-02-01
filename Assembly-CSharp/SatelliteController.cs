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
			if (i < m_overrideSatelliteResourceLinks.Length
				&& m_overrideSatelliteResourceLinks[i] != null)
			{
				m_satelliteModelDataPrefabs[i] = m_overrideSatelliteResourceLinks[i].GetPrefab();
			}
		}
	}

	private void CreateSatellites()
	{
		if (m_createdSatellites)
		{
			return;
		}
		m_satelliteInstances = new PersistentSatellite[m_satelliteModelDataPrefabs.Length];
		for (int i = 0; i < m_satelliteModelDataPrefabs.Length; i++)
		{
			GameObject gameObject = Object.Instantiate(m_satelliteModelDataPrefabs[i]);
			if (gameObject != null)
			{
				m_satelliteInstances[i] = gameObject.GetComponent<PersistentSatellite>();
				if (m_satelliteInstances[i] != null)
				{
					int layer = LayerMask.NameToLayer("Actor");
					Transform[] componentsInChildren = m_satelliteInstances[i].gameObject.GetComponentsInChildren<Transform>(true);
					foreach (Transform transform in componentsInChildren)
					{
						transform.gameObject.layer = layer;
					}
				}
			}
			if (m_satelliteInstances[i] != null)
			{
				m_satelliteInstances[i].Setup(this);
			}
		}
		m_createdSatellites = true;
	}

	public PersistentSatellite GetSatellite(int index)
	{
		if (m_satelliteInstances != null && index < m_satelliteInstances.Length)
		{
			return m_satelliteInstances[index];
		}
		return null;
	}

	public void OnAssignedToInitialBoardSquare()
	{
		if (m_createdSatellites)
		{
			foreach (PersistentSatellite satellite in m_satelliteInstances)
			{
				if (satellite != null)
				{
					satellite.OnAssignedToInitialBoardSquare();
				}
			}
		}
		else
		{
			m_delayedOnAssignedToBoardSquare = true;
		}
	}

	private void OnRespawn()
	{
		if (m_satelliteInstances != null)
		{
			foreach (PersistentSatellite satellite in m_satelliteInstances)
			{
				if (satellite != null)
				{
					satellite.OnRespawn();
				}
			}
		}
	}

	public void OnActorDeath()
	{
		if (m_satelliteInstances != null)
		{
			foreach (PersistentSatellite satellite in m_satelliteInstances)
			{
				if (satellite != null)
				{
					satellite.OnActorDeath();
				}
			}
		}
	}

	private void Update()
	{
		if (m_delayedOnAssignedToBoardSquare && m_createdSatellites)
		{
			m_delayedOnAssignedToBoardSquare = false;
			OnAssignedToInitialBoardSquare();
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		return OnSerializeHelper(new NetworkWriterAdapter(writer), initialState);
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint flags = uint.MaxValue;
		if (!initialState)
		{
			flags = reader.ReadPackedUInt32();
		}
		if (flags != 0)
		{
			OnSerializeHelper(new NetworkReaderAdapter(reader), initialState);
		}
	}

	private bool OnSerializeHelper(IBitStream stream, bool initialState)
	{
		if (!initialState && m_serializeHelper.ShouldReturnImmediately(ref stream))
		{
			return false;
		}
		int num = 0;
		if (stream.isWriting)
		{
			num = m_overrideSatelliteResourceLinks.Length;
			stream.Serialize(ref num);
			for (int i = 0; i < num; i++)
			{
				PrefabResourceLink.Stream(stream, ref m_overrideSatelliteResourceLinks[i]);
			}
		}
		if (stream.isReading)
		{
			stream.Serialize(ref num);
			m_overrideSatelliteResourceLinks = new PrefabResourceLink[num];
			for (int j = 0; j < num; j++)
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
