using System.Collections;
using Unity.VisualScripting;
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


    public void MoveDestination(Vector2[] destinationPos , Vector2 targetPos , float height)
    {
        StartCoroutine(MoveDestinationCroutine(destinationPos, height, () =>
        {
            StartCoroutine(MoveCroutine(targetPos));
        }));
    }

    private IEnumerator MoveDestinationCroutine(Vector2[] posDestination, float height, System.Action callBack)
    {
        float t = 0;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.position = GetPos(posDestination[0], posDestination[1], t/duration, height);
            yield return null;
        }
        transform.position = posDestination[1];

        callBack?.Invoke();
    }
    private IEnumerator MoveCroutine(Vector2 pos)
    {
        float t = 0;
        Vector2 startPos = transform.position;
        Vector2 endPos = pos;
        while(t < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }

    private Vector2 GetPos(Vector2 start , Vector2 end , float t , float height)
    {
        float x = Mathf.Lerp(start.x , end.x , t); // tính toán chiều ngang di chuyển
        float y = Mathf.Lerp(start .y , end.y , t) + height  * 4 * t * (1 - t); // tính toán chiều dọc di chuyển

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
    public Ball ball; // lưu trữ ball đã di chuyển của sau các bước chọn
    public Tube tubeSelect; // Vị trí Tube cũ của ball
    public Tube tubeDestination; // vị trí điểm đến mới của ball
    public Transform start; //Điểm bắt đầu di chuyển
    public Transform end; //Điểm kết thúc di chuyển

    public StepMove(Ball ball, Tube tubeSelect, Tube tubeDestination, Transform start, Transform end)
    {
        this.ball = ball;
        this.tubeSelect = tubeSelect;
        this.tubeDestination = tubeDestination;
        this.start = start;
        this.end = end;
    }
}