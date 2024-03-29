using System.Linq;
using UnityEngine;

public class TauntCameraSet : ScriptableObject
{
	public ScriptableObjectList m_tauntCameraShotSequences = new ScriptableObjectList(typeof(CameraShotSequence), ".");

	public CameraShotSequence GetTauntCam(int uniqueId)
	{
		return m_tauntCameraShotSequences.values.FirstOrDefault((ScriptableObject css) => css != null && (css as CameraShotSequence).m_uniqueTauntID == uniqueId) as CameraShotSequence;
	}

	public void AddTauntCam(CameraShotSequence tauntCam)
	{
		m_tauntCameraShotSequences.AddItem(tauntCam);
	}
}
