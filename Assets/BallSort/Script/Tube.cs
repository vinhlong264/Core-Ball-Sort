using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    private Stack<Ball> balls = new Stack<Ball>();
    [SerializeField] private List<Ball> amountBallCanMove = new List<Ball>();
    [SerializeField] private List<Ball> ballsInTube = new List<Ball>();
    public Ball tmp { get; set; }
    [SerializeField] private Transform topPos;
    [SerializeField] private Transform bottomPos;
    public Transform TopPos { get => topPos; }
    private int maxBall;
    public Transform BottomPos { get => bottomPos; }
    public int MaxBall { get => maxBall; }


    private void Start()
    {
        maxBall = 4;
        Debug.Log(this.name + ": "+maxBall);
    }


    public void SelectBall()
    {
        tmp = balls.Pop();
        tmp.MoveBallStart(topPos.position);
        amountBallCanMove.Add(tmp);
        ballsInTube.Remove(tmp);

        while(balls.Count > 0)
        {
            Ball dump = balls.Pop();
            Debug.Log(dump.name);
            if(dump != null && dump.Type == tmp.Type)
            {
                amountBallCanMove.Add(dump);
                ballsInTube.Remove(dump);
            }
            else
            {
                ballsInTube.Remove(dump);
                ballsInTube.Add(dump);
                break;
            }
        }   
    }

    public void MoveAllBall(Vector3[] Pos , System.Action callBack = null)
    {
        StartCoroutine(Main(Pos));
        callBack?.Invoke();
    }

    IEnumerator Main(Vector3[] Pos)
    {
        yield return StartCoroutine(MoveSequence(Pos));
        amountBallCanMove.Clear();
    }

    IEnumerator MoveSequence(Vector3[] Pos)
    {
        foreach (var b in amountBallCanMove)
        {
            yield return new WaitForSeconds(0.5f);
            b.MoveDestination(Pos);
        }
    }

    public List<Ball> GetAmountBallToMove()
    {
        return amountBallCanMove;
    }

    public List<Ball> GetBallInTube()
    {
        return ballsInTube;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Ball>() != null)
        {
            balls.Push(collision.GetComponent<Ball>());
            ballsInTube.Add(collision.GetComponent<Ball>());
        }
    }
}
