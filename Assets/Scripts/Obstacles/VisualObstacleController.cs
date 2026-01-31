using UnityEngine;

public enum Direction
{
    Left,
    Right
}

public class VisualObstacleController : MonoBehaviour
{
    public void CoverHalfTheScreen(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                break;

            case Direction.Right:
                break;
        }
    }

    public void TurnOffTheLights()
    {

    }

    public void FlashingLights()
    {

    }
}
