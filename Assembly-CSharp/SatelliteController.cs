using System;
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
		for (int i = 0; i < this.m_satelliteModelDataPrefabs.Length; i++)
		{
			if (i < this.m_overrideSatelliteResourceLinks.Length)
			{
				if (this.m_overrideSatelliteResourceLinks[i] != null)
				{
					this.m_satelliteModelDataPrefabs[i] = this.m_overrideSatelliteResourceLinks[i].GetPrefab(false);
				}
			}
		}
	}

	private void CreateSatellites()
	{
		if (!this.m_createdSatellites)
		{
			this.m_satelliteInstances = new PersistentSatellite[this.m_satelliteModelDataPrefabs.Length];
			for (int i = 0; i < this.m_satelliteModelDataPrefabs.Length; i++)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_satelliteModelDataPrefabs[i]);
				if (gameObject)
				{
					this.m_satelliteInstances[i] = gameObject.GetComponent<PersistentSatellite>();
					if (this.m_satelliteInstances[i] != null)
					{
						int layer = LayerMask.NameToLayer("Actor");
						foreach (Transform transform in this.m_satelliteInstances[i].gameObject.GetComponentsInChildren<Transform>(true))
						{
							transform.gameObject.layer = layer;
						}
					}
				}
				if (this.m_satelliteInstances[i] != null)
				{
					this.m_satelliteInstances[i].Setup(this);
				}
			}
			this.m_createdSatellites = true;
		}
	}

	public PersistentSatellite GetSatellite(int index)
	{
		PersistentSatellite result = null;
		if (this.m_satelliteInstances != null)
		{
			if (index < this.m_satelliteInstances.Length)
			{
				result = this.m_satelliteInstances[index];
			}
		}
		return result;
	}

	public void OnAssignedToInitialBoardSquare()
	{
		if (this.m_createdSatellites)
		{
			for (int i = 0; i < this.m_satelliteInstances.Length; i++)
			{
				if (this.m_satelliteInstances[i] != null)
				{
					this.m_satelliteInstances[i].OnAssignedToInitialBoardSquare();
				}
			}
		}
		else
		{
			this.m_delayedOnAssignedToBoardSquare = true;
		}
	}

	private void OnRespawn()
	{
		if (this.m_satelliteInstances != null)
		{
			for (int i = 0; i < this.m_satelliteInstances.Length; i++)
			{
				if (this.m_satelliteInstances[i] != null)
				{
					this.m_satelliteInstances[i].OnRespawn();
				}
			}
		}
	}

	public void OnActorDeath()
	{
		if (this.m_satelliteInstances != null)
		{
			for (int i = 0; i < this.m_satelliteInstances.Length; i++)
			{
				if (this.m_satelliteInstances[i] != null)
				{
					this.m_satelliteInstances[i].OnActorDeath();
				}
			}
		}
	}

	private void Update()
	{
		if (this.m_delayedOnAssignedToBoardSquare && this.m_createdSatellites)
		{
			this.m_delayedOnAssignedToBoardSquare = false;
			this.OnAssignedToInitialBoardSquare();
		}
	}

	public override bool OnSerialize(NetworkWriter writer, bool initialState)
	{
		return this.OnSerializeHelper(new NetworkWriterAdapter(writer), initialState);
	}

	public override void OnDeserialize(NetworkReader reader, bool initialState)
	{
		uint num = uint.MaxValue;
		if (!initialState)
		{
			num = reader.ReadPackedUInt32();
		}
		if (num != 0U)
		{
			this.OnSerializeHelper(new NetworkReaderAdapter(reader), initialState);
		}
	}

	private bool OnSerializeHelper(IBitStream stream, bool initialState)
	{
		if (!initialState)
		{
			if (this.m_serializeHelper.ShouldReturnImmediately(ref stream))
			{
				return false;
			}
		}
		int num = 0;
		if (stream.isWriting)
		{
			num = this.m_overrideSatelliteResourceLinks.Length;
			stream.Serialize(ref num);
			for (int i = 0; i < num; i++)
			{
				PrefabResourceLink.Stream(stream, ref this.m_overrideSatelliteResourceLinks[i]);
			}
		}
		if (stream.isReading)
		{
			stream.Serialize(ref num);
			this.m_overrideSatelliteResourceLinks = new PrefabResourceLink[num];
			for (int j = 0; j < num; j++)
			{
				PrefabResourceLink.Stream(stream, ref this.m_overrideSatelliteResourceLinks[j]);
			}
			this.SetupPrefabsFromOverrides();
			this.CreateSatellites();
		}
		return this.m_serializeHelper.End(initialState, base.syncVarDirtyBits);
	}

	private void UNetVersion()
	{
	}
}
