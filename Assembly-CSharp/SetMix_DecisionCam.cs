using UnityEngine;

public class SetMix_DecisionCam : MonoBehaviour
{
	private void Start()
	{
		AudioManager.GetMixerSnapshotManager().SetMix_DecisionCam();
	}
}
