// ROGUES
// SERVER
using UnityEngine;

// same in rogues & reactor
public class PowerUpLocation : MonoBehaviour
{
    private BoardSquare m_boardSquare;

    public BoardSquare boardSquare => m_boardSquare;

    public void Initialize()
    {
        Vector3 position = transform.position;
        m_boardSquare = Board.Get().GetSquareFromPos(position.x, position.z);
    }

    private void OnDrawGizmos()
    {
        if (CameraManager.ShouldDrawGizmosForCurrentCamera())
        {
            Gizmos.DrawIcon(transform.position, "icon_PowerUp.png");
        }
    }
}