using System.Collections;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private Tube tubeSelect;
    [SerializeField] private Tube tubeDestination;



    private RaycastDectector raycastDectector;
    void Start()
    {
        raycastDectector = new RaycastDectector(mask);
    }

    void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            InputDesktopHandler();
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            InputMobileHandler();
        }
    }
    #region Input Handler
    private void InputDesktopHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TubeSelectHandler(raycastDectector.GetInforRaycast().collider);
        }
    }

    private void InputMobileHandler()
    {
        Touch touch = Input.GetTouch(0);
        if(touch.phase == TouchPhase.Began)
        {
            TubeSelectHandler(raycastDectector.GetInforRaycast().collider);
        }
    }
    #endregion

    #region Game Logic
    private void TubeSelectHandler(Collider2D colider)
    {
        if (colider == null) return;

        if (tubeSelect == null)
        {
            tubeSelect = colider.GetComponent<Tube>();
            tubeSelect.SelectBall();
        }
        else if (tubeSelect != null && tubeDestination == null)
        {
            tubeDestination = colider.GetComponent<Tube>();

            if (tubeDestination == tubeSelect)
            {
                tubeSelect.ReturnTube();
            }

            if (CanMoveBall())
            {
                tubeSelect.MoveSenquence(tubeDestination , 0.8f);
            }
            else
            {
                tubeSelect.ReturnTube();
            }


            tubeSelect.tmp = null;
            tubeSelect = null;
            tubeDestination = null;
        }
    }

    private bool CanMoveBall()
    {
        if(tubeDestination.GetAmountBall() == 0) return true; // TH Tube rỗng

        if(tubeDestination.GetAmountBall() == 4) return false; //TH Tube đầy

        int ballToMove = tubeSelect.BallsCanMove.Count;
        int spaceAvailable = tubeDestination.GetSpace();

        if (tubeSelect.tmp.BallType == tubeDestination.GetColorType()) // TH bóng trên đỉnh Tube có cùng màu
        {
            if(ballToMove > spaceAvailable)
            {
                Debug.Log("Còn đủ chỗ trống");
                int enoughBall = ballToMove - spaceAvailable;
                for(int i = 0; i < enoughBall; i++)
                {
                    Ball b = tubeSelect.BallsCanMove[tubeSelect.BallsCanMove.Count - 1];
                    tubeSelect.addBall(b);
                    tubeSelect.BallsCanMove.RemoveAt(tubeSelect.BallsCanMove.Count - 1);
                }
            }

            return true;
        }

        return false;
    }


    #endregion
}

[System.Serializable]
public class RaycastDectector
{
    private LayerMask whatIsMask;
    private Vector2 targetPos;

    public RaycastDectector(LayerMask _mask)
    {
        this.whatIsMask = _mask;
    }

    public RaycastHit2D GetInforRaycast()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            Touch touch = Input.GetTouch(0);
            targetPos = Camera.main.ScreenToWorldPoint(touch.position);
        }

        RaycastHit2D hit = Physics2D.Raycast(targetPos, Vector2.zero, float.PositiveInfinity, whatIsMask);

        return hit;
    }
}
