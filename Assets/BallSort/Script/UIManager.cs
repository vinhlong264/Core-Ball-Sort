using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelMangaer;
    [SerializeField] private GameObject youWinPanel;

    [SerializeField] private Button ResetBtn;
    [SerializeField] private Button UndoBtn;
    void Start()
    {
        ResetBtn.onClick.AddListener(() => ResetGameHandler());
        UndoBtn.onClick.AddListener(() => UndoHandle());
    }

    private void ResetGameHandler()
    {
        levelMangaer.ResetLevel();
        youWinPanel.SetActive(false);
    }

    private void UndoHandle()
    {
        StepMove getStep = GameManager.instance.GetMoveStep();

        Debug.Log(getStep);
        if (getStep != null)
        {
            Ball newBall = getStep.ball;
            Tube tubeOld = getStep.tubeSelect;
            Tube tubeNew = getStep.tubeDestination;

            tubeOld.addBall(newBall);
            tubeNew.removeBall(newBall);

            Vector2 start = getStep.start.position;
            Vector2 end = getStep.end.position;

            newBall.MoveBallDestination(start, end, (start.y + end.y) - 0.7f);
        }
    }
    
}
