using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private ColorType ballType;
    private float duration = 1f;
    public ColorType BallType { get => ballType; }

    public void Move(Vector2 posDestination) // di chuyển Ball lên vị trí đỉnh Tube
    {
        StartCoroutine(MoveCroutine(posDestination));
    }


    public void MoveDestination(Vector3[] destinationPos, Vector2 targetPos, float height)
    {
        StartCoroutine(MoveDestinationCroutine(destinationPos, height, () =>
        {
            StartCoroutine(MoveCroutine(targetPos));
        }));
    }

    #region Croutine

    private IEnumerator MoveDestinationCroutine(Vector3[] posDestination, float height, System.Action callBack)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = GetPos(posDestination[0], posDestination[1], t / duration, height);
            yield return new WaitForEndOfFrame();
        }
        transform.position = posDestination[1];

        callBack?.Invoke();
    }
    private IEnumerator MoveCroutine(Vector2 pos)
    {
        float t = 0;
        Vector2 startPos = transform.position;
        Vector2 endPos = pos;
        while (t < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.position = endPos;
    }

    #endregion

    private Vector2 GetPos(Vector2 start, Vector2 end, float t, float height)
    {
        float x = Mathf.Lerp(start.x, end.x, t); // tính toán chiều ngang di chuyển
        float y = Mathf.Lerp(start.y, end.y, t) + height * 4 * t * (1 - t); // tính toán chiều dọc di chuyển

        return new Vector2(x, y);
    }


}

public enum ColorType // enum quản lý các loại ball
{
    RED, BLUE, GREEN, YELLOW, NONE
}

[System.Serializable]
public class StepMove
{
    public List<Ball> balls;
    public Tube tubeOld;
    public Tube tubeNew;
    public List<Vector2> points;

    public StepMove(List<Ball> balls, Tube tubeOld, Tube tubeNew, List<Vector2> points)
    {
        this.balls = new List<Ball>(balls);
        this.tubeOld = tubeOld;
        this.tubeNew = tubeNew;
        this.points = new List<Vector2>(points);
    }
}