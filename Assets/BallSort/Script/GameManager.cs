using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private LevelManager levelManager;
    [Header("Game Play")]
    [SerializeField] private LayerMask whatIsMask;
    private Tube tubeSelect;
    private Tube tubeDestination;
    private int count = 0; // biến đếm số tube đã đủ màu

    //Undo
    private Stack<StepMove> stepMoveStack = new Stack<StepMove>(); // Stack để lưu các bước di chuyển

    [Header("You win infor")]
    [SerializeField] private GameObject youWinPanel;
    public System.Action OnYouWinHandler; // delegate check người chơi win

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        OnYouWinHandler += OnCheckWinHandler;
    }

    void Update()
    {
        if (!levelManager.GameStart) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D touch = Physics2D.Raycast(mousePos, Vector2.zero, float.PositiveInfinity, whatIsMask);

            if (touch.collider == null) return; // kiểm tra xem có chạm vào tube không

            if (tubeSelect == null) // Lấy ra tubeSelect từ RaycastHit2D
            {
                TouchSelect(touch);
            }
            else if (tubeSelect != null && touch.collider.GetComponent<Tube>() != tubeSelect && tubeDestination == null) // Lấy ra TubeDestination từ RaycastHit2D
            {
                TouchDestination(touch);
                return;
            }
        }
    }

    #region Step Select and condition Game
    private void TouchSelect(RaycastHit2D touch)
    {
        tubeSelect = touch.collider.GetComponent<Tube>();
        tubeSelect.SelectBall();
    }
    private void TouchDestination(RaycastHit2D touch) // Hàm lấy ra điểm đến
    {
        tubeDestination = touch.collider.GetComponent<Tube>();


        Vector2 start = tubeSelect.TopPos.position;
        Vector2 end = tubeDestination.TopPos.position;


        if (canMoveBall())
        {
            foreach (var ball in tubeSelect.AmountCanMove) // add tất cả các ball đã được chọn vào tube điểm đến
            {
                if (ball != null)
                {
                    tubeDestination.addBall(ball);
                }
            }

            AddStepMove(tubeSelect.tmp, tubeSelect, tubeDestination , tubeDestination.TopPos, tubeSelect.TopPos); // thêm bước di chuyển vào stack

            tubeSelect.MoveSenquence(start, end, (start.y + end.y) - 0.7f); // di chuyển chúng tới đích
            ResetStepSelect();
        }
        else
        {
            Debug.Log("Add lại ball được lấy ra: " + tubeSelect.tmp);
            tubeSelect.addBall(tubeSelect.tmp); // add lại ball đã chọn

            tubeSelect.tmp.changeBodyRb();
            tubeSelect.ResetBallCanMove();


            ResetStepSelect();
        }
        return;
    }


    private bool canMoveBall() // hàm bool kiểm tra ball có thể di chuyển không
    {
        if (tubeSelect == null || tubeDestination == null) return false;

        if (tubeDestination.GetAmountBall() == 4) // TH số lượng bóng tại điểm đến đã đầy
        {
            Debug.Log("Tube không còn đủ sức chứa");
            return false;
        }

        if (tubeDestination.GetAmountBall() == 0) // TH tube rỗng
        {
            Debug.Log("Destina hiện đang rỗng, được phép di chuyển tới");
            return true;
        }

        int ballsToMove = tubeSelect.AmountCanMove.Count; // lấy ra số ball sẽ di chuyển
        int spaceAvailable = tubeDestination.GetSpace(); // lấy ra không gian chứa còn lại của điểm đến

        Ball tmp = tubeSelect.tmp;
        if (tmp.Type == tubeDestination.GetTypeBallTop()) // TH bóng tại điểm đến cùng màu
        {
            if (ballsToMove > spaceAvailable) //Kiểm tra số bóng di chuyển có vượt quá không gian trống không
            {
                Debug.Log("Có đủ chỗ trống");
                int excessBalls = ballsToMove - spaceAvailable; // tính ra số bóng tối đa mà destina có thể chứa
                for (int i = 0; i < excessBalls; i++)
                {
                    Ball excessBall = tubeSelect.AmountCanMove[tubeSelect.AmountCanMove.Count - 1];
                    tubeSelect.addBall(excessBall); // Trả bóng thừa về tubeSelect
                    tubeSelect.AmountCanMove.RemoveAt(tubeSelect.AmountCanMove.Count - 1); // Xóa khỏi AmountCanMove
                }
            }
            return true;
        }

        return false;
    }

    private void ResetStepSelect() // Reset các bước chọn
    {
        tubeSelect.tmp = null;
        tubeSelect = null;
        if (tubeDestination == null) return;

        tubeDestination = null;
    }
    #endregion


    private void OnCheckWinHandler() // Điều kiện win game
    {
        count++;
        Debug.Log("Số tube đã hoàn thành: " + count);
        if (count == levelManager.MaxTube)
        {
            Debug.Log("You win");
            youWinPanel.SetActive(true);
        }
    }
    private void AddStepMove(Ball ball, Tube tubeSelect, Tube tubeDestination , Transform start, Transform end) // hàm thêm các bước di chuyển để Undo
    {
        if (ball != null)
        {
            Debug.Log("Thêm di chuyển của: " + ball.name);
            StepMove stepMove = new StepMove(ball, tubeSelect,tubeDestination , start , end);
            stepMoveStack.Push(stepMove); // thêm bước di chuyển
        }
    }

    public StepMove GetMoveStep() // lấy ra bước di chuyển 
    {
        if(stepMoveStack.Count == 0) return null;

        return stepMoveStack.Pop();
    }
}

public enum GameState
{
    TOUCH,
    NOTOUCh
}
