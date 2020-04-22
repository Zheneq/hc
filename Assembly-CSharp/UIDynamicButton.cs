using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDynamicButton : MonoBehaviour
{
	private List<TextMeshProUGUI> m_labels;

	private List<Image> m_icons;

	private void Initialize()
	{
		m_labels = new List<TextMeshProUGUI>();
		m_icons = new List<Image>();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Transform transform = (Transform)enumerator.Current;
				TextMeshProUGUI componentInChildren = transform.GetComponentInChildren<TextMeshProUGUI>();
				if (componentInChildren == null)
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
				}
				else
				{
					m_labels.Add(componentInChildren);
					Image[] componentsInChildren = transform.GetComponentsInChildren<Image>(true);
					foreach (Image image in componentsInChildren)
					{
						if (image.gameObject.GetInstanceID() != transform.gameObject.GetInstanceID())
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
							m_icons.Add(image);
						}
					}
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
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	public void SetText(string text, string spriteName)
	{
		if (m_labels == null)
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
			Initialize();
		}
		using (List<TextMeshProUGUI>.Enumerator enumerator = m_labels.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TextMeshProUGUI current = enumerator.Current;
				current.text = "<sprite name=\"" + spriteName + "\"> " + text;
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
	}
}
