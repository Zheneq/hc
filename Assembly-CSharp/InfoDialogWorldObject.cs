using System;
using UnityEngine;

public class InfoDialogWorldObject : MonoBehaviour
{
	public UIInfoDialogPanel.Pivot m_dialog2DPosition = UIInfoDialogPanel.Pivot.TopRight;

	public bool m_useLine = true;

	public float m_lineLength = 200f;

	public string m_textToDisplay = "Change me!";

	public int fontSize = 0x18;

	private void Start()
	{
		if (UIInfoDialogPanel.Get() != null)
		{
			UIInfoDialogPanel.Get().AddInfoDialog(base.gameObject.transform.position, base.gameObject.transform.position, this.m_dialog2DPosition, this.m_lineLength, this.m_textToDisplay, this.fontSize, this.m_useLine, false, false);
		}
	}
}
