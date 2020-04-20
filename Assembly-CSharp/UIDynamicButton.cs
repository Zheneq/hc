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
		this.m_labels = new List<TextMeshProUGUI>();
		this.m_icons = new List<Image>();
		IEnumerator enumerator = base.transform.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				Transform transform = (Transform)obj;
				TextMeshProUGUI componentInChildren = transform.GetComponentInChildren<TextMeshProUGUI>();
				if (componentInChildren == null)
				{
				}
				else
				{
					this.m_labels.Add(componentInChildren);
					foreach (Image image in transform.GetComponentsInChildren<Image>(true))
					{
						if (image.gameObject.GetInstanceID() != transform.gameObject.GetInstanceID())
						{
							this.m_icons.Add(image);
						}
					}
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
		if (this.m_labels == null)
		{
			this.Initialize();
		}
		using (List<TextMeshProUGUI>.Enumerator enumerator = this.m_labels.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				TextMeshProUGUI textMeshProUGUI = enumerator.Current;
				textMeshProUGUI.text = "<sprite name=\"" + spriteName + "\"> " + text;
			}
		}
	}
}
