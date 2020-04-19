using System;
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
		foreach (GameObject item in this.m_backgroundBlockPrefabs)
		{
			this.m_uninstantiatedPrefabs.Add(item);
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(BackgroundScroller.Awake()).MethodHandle;
		}
		this.MakeNewVisibleBlock();
	}

	private void Update()
	{
		BoxCollider component = base.GetComponent<BoxCollider>();
		if (component == null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BackgroundScroller.Update()).MethodHandle;
			}
			Log.Error(string.Format("BackgroundScroller {0} does not have a collider", base.name), new object[0]);
			return;
		}
		if (this.IsVisibleWithCurrentSettings())
		{
			bool flag = false;
			for (int i = 0; i < this.m_visibleBlocks.Count; i++)
			{
				GameObject gameObject = this.m_visibleBlocks[i];
				gameObject.SetActive(true);
				gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x + this.m_backgroundSpeed * Time.deltaTime, 0f, 0f);
				BoxCollider component2 = gameObject.GetComponent<BoxCollider>();
				if (component2 == null)
				{
					Log.Error(string.Format("Block {0} in {1} does not have a collider", gameObject.name, base.name), new object[0]);
					this.m_visibleBlocks.Remove(gameObject);
					return;
				}
				float num = gameObject.transform.localPosition.x + component2.center.x - component2.size.x * 0.5f;
				float num2 = base.transform.localPosition.x + component.center.x - component.size.x * 0.5f;
				float num3 = base.transform.localPosition.x + component.center.x + component.size.x * 0.5f;
				if (num > num3)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					gameObject.SetActive(false);
					this.m_blocksToRetire.Add(gameObject);
				}
				if (i == this.m_visibleBlocks.Count - 1)
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
					if (num > num2)
					{
						flag = true;
					}
				}
			}
			foreach (GameObject item in this.m_blocksToRetire)
			{
				this.m_visibleBlocks.Remove(item);
				this.m_inVisibleBlocks.Add(item);
			}
			this.m_blocksToRetire.Clear();
			if (flag)
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
				this.MakeNewVisibleBlock();
			}
		}
		else
		{
			using (List<GameObject>.Enumerator enumerator2 = this.m_visibleBlocks.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					GameObject gameObject2 = enumerator2.Current;
					gameObject2.SetActive(false);
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BackgroundScroller.IsVisibleWithCurrentSettings()).MethodHandle;
			}
			return true;
		}
		GraphicsQuality currentGraphicsQuality = Options_UI.Get().GetCurrentGraphicsQuality();
		if (currentGraphicsQuality == GraphicsQuality.High)
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
			return true;
		}
		if (currentGraphicsQuality == GraphicsQuality.Medium && !this.m_hideAtMediumQuality)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			return true;
		}
		if (currentGraphicsQuality == GraphicsQuality.Low)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!this.m_hideAtLowQuality)
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
		if (this.m_uninstantiatedPrefabs.Count > 0)
		{
			int index = UnityEngine.Random.Range(0, this.m_uninstantiatedPrefabs.Count);
			gameObject = this.m_uninstantiatedPrefabs[index];
			this.m_uninstantiatedPrefabs.RemoveAt(index);
		}
		else if (this.m_inVisibleBlocks.Count > 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(BackgroundScroller.MakeNewVisibleBlock()).MethodHandle;
			}
			int index2 = UnityEngine.Random.Range(0, this.m_inVisibleBlocks.Count);
			gameObject2 = this.m_inVisibleBlocks[index2];
			this.m_inVisibleBlocks.RemoveAt(index2);
		}
		else if (this.m_visibleBlocks.Count > 0)
		{
			int index3 = UnityEngine.Random.Range(0, this.m_visibleBlocks.Count);
			gameObject = this.m_visibleBlocks[index3];
		}
		if (gameObject != null)
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
			gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
			gameObject2.transform.SetParent(base.transform);
			gameObject2.transform.localPosition = Vector3.zero;
		}
		if (gameObject2 != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			gameObject2.SetActive(this.IsVisibleWithCurrentSettings());
			gameObject2.transform.localPosition = Vector3.zero;
			gameObject2.transform.localRotation = Quaternion.identity;
			if (this.m_randomRotation)
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
				if (UnityEngine.Random.value > 0.5f)
				{
					gameObject2.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
				}
			}
			if (this.m_visibleBlocks.Count > 0)
			{
				GameObject gameObject3 = this.m_visibleBlocks[this.m_visibleBlocks.Count - 1];
				float x = gameObject3.transform.localPosition.x - gameObject3.GetComponent<BoxCollider>().size.x / 2f - gameObject2.GetComponent<BoxCollider>().size.x / 2f + gameObject3.GetComponent<BoxCollider>().center.x - gameObject2.GetComponent<BoxCollider>().center.x;
				gameObject2.transform.localPosition = new Vector3(x, 0f, 0f);
			}
			this.m_visibleBlocks.Add(gameObject2);
		}
		return gameObject2;
	}
}
