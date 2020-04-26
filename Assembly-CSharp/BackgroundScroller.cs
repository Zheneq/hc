using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
	public GameObject[] m_backgroundBlockPrefabs;

	[Tooltip("The speed at which the blocks will translate forward (in their local space).")]
	public float m_backgroundSpeed;

	[Tooltip("Whether to randomly rotate the blocks 180 degrees")]
	public bool m_randomRotation;

	[Tooltip("Whether to hide the blocks when running in low graphics quality.")]
	public bool m_hideAtLowQuality;

	[Tooltip("Whether to hide the blocks when running in medium graphics quality.")]
	public bool m_hideAtMediumQuality;

	private List<GameObject> m_visibleBlocks = new List<GameObject>();

	private List<GameObject> m_inVisibleBlocks = new List<GameObject>();

	private List<GameObject> m_uninstantiatedPrefabs = new List<GameObject>();

	private List<GameObject> m_blocksToRetire = new List<GameObject>();

	private void Awake()
	{
		GameObject[] backgroundBlockPrefabs = m_backgroundBlockPrefabs;
		foreach (GameObject item in backgroundBlockPrefabs)
		{
			m_uninstantiatedPrefabs.Add(item);
		}
		while (true)
		{
			MakeNewVisibleBlock();
			return;
		}
	}

	private void Update()
	{
		BoxCollider component = GetComponent<BoxCollider>();
		if (component == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Log.Error($"BackgroundScroller {base.name} does not have a collider");
					return;
				}
			}
		}
		if (IsVisibleWithCurrentSettings())
		{
			bool flag = false;
			for (int i = 0; i < m_visibleBlocks.Count; i++)
			{
				GameObject gameObject = m_visibleBlocks[i];
				gameObject.SetActive(true);
				Transform transform = gameObject.transform;
				Vector3 localPosition = gameObject.transform.localPosition;
				transform.localPosition = new Vector3(localPosition.x + m_backgroundSpeed * Time.deltaTime, 0f, 0f);
				BoxCollider component2 = gameObject.GetComponent<BoxCollider>();
				if (component2 == null)
				{
					Log.Error($"Block {gameObject.name} in {base.name} does not have a collider");
					m_visibleBlocks.Remove(gameObject);
					return;
				}
				Vector3 localPosition2 = gameObject.transform.localPosition;
				float x = localPosition2.x;
				Vector3 center = component2.center;
				float num = x + center.x;
				Vector3 size = component2.size;
				float num2 = num - size.x * 0.5f;
				Vector3 localPosition3 = base.transform.localPosition;
				float x2 = localPosition3.x;
				Vector3 center2 = component.center;
				float num3 = x2 + center2.x;
				Vector3 size2 = component.size;
				float num4 = num3 - size2.x * 0.5f;
				Vector3 localPosition4 = base.transform.localPosition;
				float x3 = localPosition4.x;
				Vector3 center3 = component.center;
				float num5 = x3 + center3.x;
				Vector3 size3 = component.size;
				float num6 = num5 + size3.x * 0.5f;
				if (num2 > num6)
				{
					gameObject.SetActive(false);
					m_blocksToRetire.Add(gameObject);
				}
				if (i == m_visibleBlocks.Count - 1)
				{
					if (num2 > num4)
					{
						flag = true;
					}
				}
			}
			foreach (GameObject item in m_blocksToRetire)
			{
				m_visibleBlocks.Remove(item);
				m_inVisibleBlocks.Add(item);
			}
			m_blocksToRetire.Clear();
			if (!flag)
			{
				return;
			}
			while (true)
			{
				MakeNewVisibleBlock();
				return;
			}
		}
		using (List<GameObject>.Enumerator enumerator2 = m_visibleBlocks.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				GameObject current2 = enumerator2.Current;
				current2.SetActive(false);
			}
			while (true)
			{
				switch (4)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	private bool IsVisibleWithCurrentSettings()
	{
		Options_UI x = Options_UI.Get();
		if (x == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		GraphicsQuality currentGraphicsQuality = Options_UI.Get().GetCurrentGraphicsQuality();
		if (currentGraphicsQuality == GraphicsQuality.High)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (currentGraphicsQuality == GraphicsQuality.Medium && !m_hideAtMediumQuality)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return true;
				}
			}
		}
		if (currentGraphicsQuality == GraphicsQuality.Low)
		{
			if (!m_hideAtLowQuality)
			{
				return true;
			}
		}
		return false;
	}

	private GameObject MakeNewVisibleBlock()
	{
		GameObject gameObject = null;
		GameObject gameObject2 = null;
		if (m_uninstantiatedPrefabs.Count > 0)
		{
			int index = Random.Range(0, m_uninstantiatedPrefabs.Count);
			gameObject = m_uninstantiatedPrefabs[index];
			m_uninstantiatedPrefabs.RemoveAt(index);
		}
		else if (m_inVisibleBlocks.Count > 0)
		{
			int index2 = Random.Range(0, m_inVisibleBlocks.Count);
			gameObject2 = m_inVisibleBlocks[index2];
			m_inVisibleBlocks.RemoveAt(index2);
		}
		else if (m_visibleBlocks.Count > 0)
		{
			int index3 = Random.Range(0, m_visibleBlocks.Count);
			gameObject = m_visibleBlocks[index3];
		}
		if (gameObject != null)
		{
			gameObject2 = Object.Instantiate(gameObject);
			gameObject2.transform.SetParent(base.transform);
			gameObject2.transform.localPosition = Vector3.zero;
		}
		if (gameObject2 != null)
		{
			gameObject2.SetActive(IsVisibleWithCurrentSettings());
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localRotation = Quaternion.identity;
			if (m_randomRotation)
			{
				if (Random.value > 0.5f)
				{
					gameObject2.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
				}
			}
			if (m_visibleBlocks.Count > 0)
			{
				GameObject gameObject3 = m_visibleBlocks[m_visibleBlocks.Count - 1];
				Vector3 localPosition = gameObject3.transform.localPosition;
				float x = localPosition.x;
				Vector3 size = gameObject3.GetComponent<BoxCollider>().size;
				float num = x - size.x / 2f;
				Vector3 size2 = gameObject2.GetComponent<BoxCollider>().size;
				float num2 = num - size2.x / 2f;
				Vector3 center = gameObject3.GetComponent<BoxCollider>().center;
				float num3 = num2 + center.x;
				Vector3 center2 = gameObject2.GetComponent<BoxCollider>().center;
				float x2 = num3 - center2.x;
				gameObject2.transform.localPosition = new Vector3(x2, 0f, 0f);
			}
			m_visibleBlocks.Add(gameObject2);
		}
		return gameObject2;
	}
}
