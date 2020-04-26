using UnityEngine;

namespace TMPro.Examples
{
	public class TMPro_InstructionOverlay : MonoBehaviour
	{
		public enum FpsCounterAnchorPositions
		{
			_001D,
			_000E,
			_0012,
			_0015
		}

		public FpsCounterAnchorPositions _001D = FpsCounterAnchorPositions._000E;

		private const string _000E = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";

		private TextMeshPro _0012;

		private TextContainer _0015;

		private Transform _0016;

		private Camera _0013;

		private void _0018()
		{
			if (base.enabled)
			{
				_0013 = Camera.main;
				GameObject gameObject = new GameObject("Frame Counter");
				_0016 = gameObject.transform;
				_0016.parent = _0013.transform;
				_0016.localRotation = Quaternion.identity;
				_0012 = gameObject.AddComponent<TextMeshPro>();
				_0012.font = (Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset);
				_0012.fontSharedMaterial = (Resources.Load("Fonts & Materials/LiberationSans SDF - Overlay", typeof(Material)) as Material);
				_0012.fontSize = 30f;
				_0012.isOverlay = true;
				_0015 = gameObject.GetComponent<TextContainer>();
				_0018(_001D);
				_0012.text = "Camera Control - <#ffff00>Shift + RMB\n</color>Zoom - <#ffff00>Mouse wheel.";
			}
		}

		private void _0018(FpsCounterAnchorPositions _001D)
		{
			switch (_001D)
			{
			case FpsCounterAnchorPositions._001D:
				_0015.anchorPosition = TextContainerAnchors.TopLeft;
				_0016.position = _0013.ViewportToWorldPoint(new Vector3(0f, 1f, 100f));
				break;
			case FpsCounterAnchorPositions._000E:
				_0015.anchorPosition = TextContainerAnchors.BottomLeft;
				_0016.position = _0013.ViewportToWorldPoint(new Vector3(0f, 0f, 100f));
				break;
			case FpsCounterAnchorPositions._0012:
				_0015.anchorPosition = TextContainerAnchors.TopRight;
				_0016.position = _0013.ViewportToWorldPoint(new Vector3(1f, 1f, 100f));
				break;
			case FpsCounterAnchorPositions._0015:
				_0015.anchorPosition = TextContainerAnchors.BottomRight;
				_0016.position = _0013.ViewportToWorldPoint(new Vector3(1f, 0f, 100f));
				break;
			}
		}
	}
}
