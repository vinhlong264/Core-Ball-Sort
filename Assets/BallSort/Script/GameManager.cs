using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField] private LevelManager levelManager;
    [Header("Game Play")]
    [SerializeField] private LayerMask whatIsMask;
    private Tube tubeSelect;
    private Tube tubeDestination;
    private int count = 0;
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
        //if (!levelManager.GameStart) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D touch = Physics2D.Raycast(mousePos, Vector2.zero, float.PositiveInfinity, whatIsMask);

            if (touch.collider == null) return;

            if (tubeSelect == null)
            {
                tubeSelect = touch.collider.GetComponent<Tube>();
                tubeSelect.SelectBall();
            }
            else if (tubeSelect != null && touch.collider.GetComponent<Tube>() != tubeSelect && tubeDestination == null)
            {
                tubeDestination = touch.collider.GetComponent<Tube>();
                Vector2 start = tubeSelect.TopPos.position;
                Vector2 end = tubeDestination.TopPos.position;
                if (canMoveBall())
                {
                    tubeSelect.MoveSenquence(start, end, start.y + end.y);
                    tubeSelect.tmp = null;
                    tubeSelect = null;
                    tubeDestination = null;
                }
                else
                {
                    tubeSelect.tmp.changeBodyRb();
                    tubeSelect.ResetBallCanMove();
                    tubeSelect.tmp = null;
                    tubeSelect = null;
                    tubeDestination = null;
                }
                return;
            }

            Invoke("ResetSelect", 2f);
        }
    }

    private void OnCheckWinHandler()
    {
        count++;
        if(count == levelManager.MaxTube)
        {
            Debug.Log("You win");
        }
    }

    private void ResetSelect()
    {
        if (tubeDestination != null) return;


        if(tubeSelect == null) return;
        tubeSelect.tmp = null;
        tubeSelect.ResetBallCanMove();
        tubeSelect = null;
    }

    private bool canMoveBall()
    {
        if(tubeSelect == null || tubeDestination == null) return false;

        if (tubeDestination.GetAmountBall() == 4)
        {
            Debug.Log("Tube không còn đủ sức chứa");
            return false;
        }

        Ball tmp = tubeSelect.tmp;
        if(tmp.Type == tubeDestination.GetTypeBallTop())
        {
            Debug.Log("Bóng trên cùng của tube destination cùng màu, được phép di chuyển đến");
            return true;
        }

        if(tubeDestination.GetAmountBall() == 0)
        {
            Debug.Log("TubeDestination hiện đang rỗng, được phép di chuyển đến");
            return true;
        }

        return false;
    }
}
