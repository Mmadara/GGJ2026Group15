using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public enum Direction
{
    Left,
    Right
}

public class VisualObstacleController : MonoBehaviour
{
    [SerializeField] private Image _lightOff;
    [SerializeField] private Image _screenRightBlack;
    [SerializeField] private Image _screenLeftBlack;
    [SerializeField] private Image _blurryScreen;

    public void StartBlickingLight()
    {
        StartCoroutine(BlinkingLights());
    }

    public void StartLightOff()
    {
        StartCoroutine(StartGenericCoverScreen(_lightOff));
    }

    public void StartBlurry()
    {
        StartCoroutine(StartGenericCoverScreen(_blurryScreen));
    }

    public void CovertHalfTheScreen(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                StartCoroutine(StartGenericCoverScreen(_screenLeftBlack));
                break;

            case Direction.Right:
                StartCoroutine(StartGenericCoverScreen(_screenRightBlack));
                break;
        }
    }

    IEnumerator BlinkingLights()
    {
        for (int i = 0; i < 40; i++)
        {
            _lightOff.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            _lightOff.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
        }

    }

    IEnumerator StartGenericCoverScreen(Image image)
    {
        image.gameObject.SetActive(true);

        yield return new WaitForSeconds(Random.Range(3, 5));

        image.gameObject.SetActive(false);

    }

}
