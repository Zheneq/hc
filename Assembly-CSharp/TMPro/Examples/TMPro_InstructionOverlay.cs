using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class TMPro_InstructionOverlay : MonoBehaviour
	{
		public TMPro_InstructionOverlay.FpsCounterAnchorPositions \u001D = TMPro_InstructionOverlay.FpsCounterAnchorPositions.\u000E;

		private const string \u000E = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";

		private TextMeshPro \u0012;

		private TextContainer \u0015;

		private Transform \u0016;

		private Camera \u0013;

		private void \u0018()
		{
			if (!base.enabled)
			{
				return;
			}
			this.\u0013 = Camera.main;
			GameObject gameObject = new GameObject("Frame Counter");
			this.\u0016 = gameObject.transform;
			this.\u0016.parent = this.\u0013.transform;
			this.\u0016.localRotation = Quaternion.identity;
			this.\u0012 = gameObject.AddComponent<TextMeshPro>();
			this.\u0012.font = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
			this.\u0012.fontSharedMaterial = (Resources.Load("Fonts & Materials/LiberationSans SDF - Overlay", typeof(Material)) as Material);
			this.\u0012.fontSize = 30f;
			this.\u0012.isOverlay = true;
			this.\u0015 = gameObject.GetComponent<TextContainer>();
			this.\u0018(this.\u001D);
			this.\u0012.text = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";
		}

		private void \u0018(TMPro_InstructionOverlay.FpsCounterAnchorPositions \u001D)
		{
			switch (\u001D)
			{
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.\u001D:
				this.\u0015.anchorPosition = TextContainerAnchors.TopLeft;
				this.\u0016.position = this.\u0013.ViewportToWorldPoint(new Vector3(0f, 1f, 100f));
				break;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.\u000E:
				this.\u0015.anchorPosition = TextContainerAnchors.BottomLeft;
				this.\u0016.position = this.\u0013.ViewportToWorldPoint(new Vector3(0f, 0f, 100f));
				break;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.\u0012:
				this.\u0015.anchorPosition = TextContainerAnchors.TopRight;
				this.\u0016.position = this.\u0013.ViewportToWorldPoint(new Vector3(1f, 1f, 100f));
				break;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.\u0015:
				this.\u0015.anchorPosition = TextContainerAnchors.BottomRight;
				this.\u0016.position = this.\u0013.ViewportToWorldPoint(new Vector3(1f, 0f, 100f));
				break;
			}
		}

		public enum FpsCounterAnchorPositions
		{
			\u001D,
			\u000E,
			\u0012,
			\u0015
		}
	}
}
