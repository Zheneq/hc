using System;
using UnityEngine;

namespace TMPro.Examples
{
	public class TMPro_InstructionOverlay : MonoBehaviour
	{
		public TMPro_InstructionOverlay.FpsCounterAnchorPositions symbol_001D = TMPro_InstructionOverlay.FpsCounterAnchorPositions.symbol_000E;

		private const string symbol_000E = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";

		private TextMeshPro symbol_0012;

		private TextContainer symbol_0015;

		private Transform symbol_0016;

		private Camera symbol_0013;

		private void symbol_0018()
		{
			if (!base.enabled)
			{
				return;
			}
			this.symbol_0013 = Camera.main;
			GameObject gameObject = new GameObject("Frame Counter");
			this.symbol_0016 = gameObject.transform;
			this.symbol_0016.parent = this.symbol_0013.transform;
			this.symbol_0016.localRotation = Quaternion.identity;
			this.symbol_0012 = gameObject.AddComponent<TextMeshPro>();
			this.symbol_0012.font = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
			this.symbol_0012.fontSharedMaterial = (Resources.Load("Fonts & Materials/LiberationSans SDF - Overlay", typeof(Material)) as Material);
			this.symbol_0012.fontSize = 30f;
			this.symbol_0012.isOverlay = true;
			this.symbol_0015 = gameObject.GetComponent<TextContainer>();
			this.symbol_0018(this.symbol_001D);
			this.symbol_0012.text = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";
		}

		private void symbol_0018(TMPro_InstructionOverlay.FpsCounterAnchorPositions symbol_001D)
		{
			switch (symbol_001D)
			{
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.symbol_001D:
				this.symbol_0015.anchorPosition = TextContainerAnchors.TopLeft;
				this.symbol_0016.position = this.symbol_0013.ViewportToWorldPoint(new Vector3(0f, 1f, 100f));
				break;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.symbol_000E:
				this.symbol_0015.anchorPosition = TextContainerAnchors.BottomLeft;
				this.symbol_0016.position = this.symbol_0013.ViewportToWorldPoint(new Vector3(0f, 0f, 100f));
				break;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.symbol_0012:
				this.symbol_0015.anchorPosition = TextContainerAnchors.TopRight;
				this.symbol_0016.position = this.symbol_0013.ViewportToWorldPoint(new Vector3(1f, 1f, 100f));
				break;
			case TMPro_InstructionOverlay.FpsCounterAnchorPositions.symbol_0015:
				this.symbol_0015.anchorPosition = TextContainerAnchors.BottomRight;
				this.symbol_0016.position = this.symbol_0013.ViewportToWorldPoint(new Vector3(1f, 0f, 100f));
				break;
			}
		}

		public enum FpsCounterAnchorPositions
		{
			symbol_001D,
			symbol_000E,
			symbol_0012,
			symbol_0015
		}
	}
}
