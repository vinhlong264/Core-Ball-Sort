﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tube : MonoBehaviour
{
    [SerializeField] private Transform topPos;
    [SerializeField] private List<Ball> ballsInTube = new List<Ball>();
    [SerializeField] private List<Ball> ballsCanMove = new List<Ball>();
    private Stack<Ball> StackBalls = new Stack<Ball>();
    private Vector2[] posArray;
    private bool canWin;
    private int maxBall;

    public Ball tmp { get; set; }
    private Vector2 prePos;

    public Vector2[] PosArray { get => posArray; }
    public Transform TopPos { get => topPos; }
    public List<Ball> BallsCanMove { get =>  ballsCanMove;}
    private void OnEnable()
    {
        maxBall = 4;
        InitPos();
    }

    private void InitPos()
    {
        posArray = new Vector2[4];
        float ofsetBall = 1f;
        float tubeHeight = transform.localScale.y;
        float tubeBottomY = transform.position.y - (tubeHeight / 2f);

        for (int i = 0; i < 4; i++)
        {
            float yPos = tubeBottomY + (i * ofsetBall) + (ofsetBall / 2);
            Vector2 newPos = new Vector2(transform.position.x, yPos);
            posArray[i] = newPos;
        }

    }

    public void SelectBall()
    {
        tmp = StackBalls.Pop();
        ballsInTube.Remove(tmp);
        ballsCanMove.Add(tmp);

        prePos = tmp.transform.position;
        tmp.Move(topPos.position);

        while(StackBalls.Count > 0)
        {
            if (!checkBallSameColor())
            {
                break;
            }

            Ball getBall = StackBalls.Pop();
            ballsCanMove.Add(getBall);
            ballsInTube.Remove(getBall);
        }
    }

    public void ReturnTube()
    {
        for (int i = ballsCanMove.Count - 1; i >= 0; i--)
        {
            addBall(ballsCanMove[i]);
        }

        ballsCanMove.Clear();

        tmp.Move(prePos);
        tmp = null;
    }

    public void MoveSenquence(Tube tubeDestination , float height)
    {
        StartCoroutine(SenquenceCroutine(tubeDestination, height));
    }

    IEnumerator SenquenceCroutine(Tube tubeDestination, float height)
    {
        Vector2[] newPos = new Vector2[]
        {
            topPos.position,
            tubeDestination.topPos.position
        };

        for(int i = 0; i < ballsCanMove.Count; i++)
        {
            int index = tubeDestination.GetEmptySlot();
            if (index != -1)
            {
                Vector2 targetPos = tubeDestination.PosArray[index];
                ballsCanMove[i].MoveDestination(newPos, targetPos, height);
                tubeDestination.addBall(ballsCanMove[i]);
                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                break;
            }
        }

        ballsCanMove.Clear();
    }

    public void addBall(Ball ball)
    {
        StackBalls.Push(ball);
        ballsInTube.Add(ball);

        checkFullColor();
    }

    public int GetEmptySlot()
    {
        Debug.Log(StackBalls.Count);
        return StackBalls.Count;
    }

    public int GetSpace()
    {
        return maxBall - StackBalls.Count;
    }

    public ColorType GetColorType()
    {
        return StackBalls.Peek().BallType;
    }

    public int GetAmountBall()
    {
        if(StackBalls.Count == 0) return 0;

        return StackBalls.Count;
    }

    private void checkFullColor()
    {
        if (StackBalls.Count == 0) return;

        if (ballsInTube.Count == 0) return;

        ColorType tmp = StackBalls.Peek().BallType;

        int count = 0;
        foreach (var i in ballsInTube)
        {
            if (i.BallType != tmp)
            {
                break;
            }

            count++;
        }

        if (count == maxBall)
        {
            canWin = true;
            Debug.Log($"Tube {this.gameObject.name} win, đã đủ số màu");
            GameManager.instance.OnYouWin?.Invoke();
        }
    }

    private bool checkBallSameColor()
    {
        if(StackBalls.Count == 0) return false;
        
        if(StackBalls.Peek().BallType == tmp.BallType) return true;

        return false;
    }
}
