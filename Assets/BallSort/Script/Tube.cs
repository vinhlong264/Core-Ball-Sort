using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    public Ball tmp { get; set; }

    private Stack<Ball> balls = new Stack<Ball>();
    [SerializeField] private List<Ball> amountBallCanMove = new List<Ball>();
    [SerializeField] private List<Ball> ballsInTube = new List<Ball>();

    [SerializeField] private Transform topPos;
    private bool canWin; //biến kiểm tra xem ông này đã đủ đk thắng chưa

    public Transform TopPos { get => topPos; }
    public bool CanWin { get => canWin; }

    public void SelectBall(System.Action callBack = null)
    {
        if (balls.Count == 0) return;

        tmp = balls.Pop();
        amountBallCanMove.Add(tmp);

        ballsInTube.Remove(tmp);
        tmp.MoveBallTop((Vector2)topPos.position);

        while(balls.Count > 0)
        {
            if (!BallSameColor())
            {
                break;
            }
            Ball ball = balls.Pop();
            Debug.Log("Ball take: "+ball.name);
            amountBallCanMove.Add(ball);
            ballsInTube.Remove(ball);
        }
        callBack?.Invoke();
    }

    private bool BallSameColor()
    {
        if (balls.Count == 0) return false;

        if(balls.Peek().Type == tmp.Type)
        {
            return true;
        }

        return false;
    }

    public void MoveSenquence(Vector2 startPos , Vector2 endPos , float height)
    {
        if(amountBallCanMove.Count == 1) // nếu số lượng ball có thể di chuyển chỉ có 1 thì di chuyển duy nhất ball đó
        {
            amountBallCanMove[0].MoveBallDestination(startPos, endPos, height);
            ResetBallCanMove();
        }
        else if(amountBallCanMove.Count > 1)
        {
            StartCoroutine(MoveSenquenceCroutine(startPos, endPos, height));
        }
    }

    IEnumerator MoveSenquenceCroutine(Vector2 startPos, Vector2 endPos, float height) // Hàm di chuyển 1 lúc nhiều ball
    {
        for(int i = 0; i < amountBallCanMove.Count; i++)
        {
            yield return new WaitForSeconds(0.75f);
            yield return null;
            amountBallCanMove[i].MoveBallDestination(startPos, endPos, height);
        }
        ResetBallCanMove();
    }

    public void addBall(Ball newBall, System.Action callback = null)
    {
        balls.Push(newBall);
        ballsInTube.Add(newBall);
        TubeFullColor();

        callback?.Invoke();
    }

    public void ResetBallCanMove()
    {
        if(amountBallCanMove.Count == 0) return;
        amountBallCanMove.Clear();
    }

    public int GetAmountBall()
    {
        if (balls.Count == 0)
        {
            return 0;
        }

        return balls.Count;
    }

    public ColorType GetTypeBallTop()
    {
        if(canWin) return ColorType.NONE;

        if (balls.Count > 0)
        {
            return balls.Peek().Type;
        }

        return ColorType.NONE;
    }

    private void TubeFullColor()
    {
        if (balls.Count == 0) return;
        if (ballsInTube.Count == 0) return;

        ColorType tmp = balls.Peek().Type; // lấy ra màu ở đỉnh tube
        int count = 0;

        foreach (var i in ballsInTube)
        {
            if (i.Type != tmp)
            {
                continue;
            }

            count++;
        }

        if (count == 4)
        {
            canWin = true;
            Debug.Log($"Tube {this.gameObject.name} win, đã đủ số màu");
            GameManager.instance.OnYouWinHandler?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ball"))
        {
            addBall(collision.GetComponent<Ball>());
            
        }
    }
}
