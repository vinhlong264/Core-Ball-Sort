using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject YouWinnPanel;
    private Stack<StepMove> stepMoveBall = new Stack<StepMove>();

    public System.Action OnYouWin;
    private int countTube;

    public LevelManager LevelManager { get => levelManager; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnYouWin += OnYouWinHandler;
    }

    private void OnYouWinHandler()
    {
        countTube++;
        if (countTube == levelManager.MaxTube)
        {
            Debug.Log("you win");
            YouWinnPanel.SetActive(true);
        }
    }

    public void addStepBall(StepMove stepMove)
    {
        stepMoveBall.Push(stepMove);
    }

    public void UndoHandler()
    {
        StepMove getStep = stepMoveBall.Pop();

        StartCoroutine(Main(getStep.balls, getStep));
    }
    IEnumerator Main(List<Ball> balls, StepMove stepTmp)
    {
        yield return StartCoroutine(UndoMoveBall(balls, stepTmp));
        yield return new WaitForSeconds(0.3f);
        foreach (Ball b in balls)
        {
            stepTmp.tubeOld.addBall(b);
        }
        stepTmp.tubeOld.BallsCanMove.Clear();

    }

    IEnumerator UndoMoveBall(List<Ball> balls, StepMove stepTmp)
    {      
        for (int i = 0; i < balls.Count; i++)
        {
            Ball ball = balls[i];
            if (ball != null)
            {
                Vector2 startPos = stepTmp.points[i];
                stepTmp.tubeNew.removeBall(ball);

                stepTmp.tubeOld.BallsCanMove.Add(ball);
                Vector3[] newPos = new Vector3[]
                {
                    stepTmp.tubeNew.TopPos.position, stepTmp.tubeOld.TopPos.position
                };

                ball.MoveDestination(newPos, startPos, 0.8f);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}


