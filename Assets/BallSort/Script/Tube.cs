using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    public Ball tmp { get; set; }

    private Stack<Ball> balls = new Stack<Ball>(); // Stack chứa các ball trong tube
    [SerializeField] private List<Ball> ballsInTube = new List<Ball>(); // List chứa các ball trong tube
    [SerializeField] private List<Ball> amountBallCanMove = new List<Ball>();

    [SerializeField] private Transform topPos;
    private bool canWin; //biến kiểm tra xem ông này đã đủ đk thắng chưa
    private int maxBall = 4;

    public Transform TopPos { get => topPos; }
    public bool CanWin { get => canWin; }

    public List<Ball> AmountCanMove { get => amountBallCanMove; }

    public void SelectBall(System.Action callBack = null) //Chọn ball ở đỉnh
    {
        if (balls.Count == 0) return;

        tmp = balls.Pop();
        amountBallCanMove.Add(tmp);

        ballsInTube.Remove(tmp);
        tmp.MoveBallTop((Vector2)topPos.position);

        while(balls.Count > 0) // Lấy ra tất cả các ball cùng màu với ball Tmp(đã được lấy ra)
        {
            if (!BallSameColor())
            {
                break;
            }
            Ball ball = balls.Pop();
            amountBallCanMove.Add(ball);
            ballsInTube.Remove(ball);
        }

        callBack?.Invoke();
    }

    private bool BallSameColor() // kiểm tra màu ball
    {
        if (balls.Count == 0) return false; // nếu ball hiện tại đang rỗng sẽ trả về false

        if(balls.Peek().Type == tmp.Type) // nếu màu của ball ở đỉnh hiện tại cùng loại với ball đã được lấy ra trước đó thì trả về true
        {
            return true;
        }
        return false;
    }

    public void ResetTube()
    {
        balls.Clear();
        ballsInTube.Clear();
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
            yield return new WaitForSeconds(0.6f);
            amountBallCanMove[i].MoveBallDestination(startPos, endPos, height);
        }

        ResetBallCanMove();
        Debug.Log(amountBallCanMove.Count);
    }
    public void addBall(Ball newBall) // addBall
    {
        if(balls.Count == maxBall) return;

        balls.Push(newBall);
        ballsInTube.Add(newBall);
        TubeFullColor();
    }

    public void ResetBallCanMove() // reset các ball đã được di chuyển đi
    {
        amountBallCanMove.Clear();
    }

    public int AmountBallCanMove()
    {
        if (ballsInTube.Count == 0) return 0;

        return balls.Count;
    }

    public int GetAmountBall() // lấy ra số lượng ball hiện tại
    {
        if (balls.Count == 0)
        {
            return 0;
        }

        return balls.Count;
    }

    public int GetSpace() //Tính toán khoảng trống cho đủ để các ball di chuyển tới không
    {
        return maxBall - balls.Count;
    }

    public ColorType GetTypeBallTop() // lấy ra loại màu ball ở đỉnh
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
}
