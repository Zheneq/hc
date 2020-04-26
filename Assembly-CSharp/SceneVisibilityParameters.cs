public class SceneVisibilityParameters
{
	public bool TurnOffAllOtherScenesInCanvasLayer;

	public bool PlayTransitions;

	public SceneStateParameters StateParameters;

	public SceneVisibilityParameters()
	{
		TurnOffAllOtherScenesInCanvasLayer = false;
		PlayTransitions = true;
		StateParameters = new SceneStateParameters();
	}
}
