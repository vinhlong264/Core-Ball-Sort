using System.Drawing;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Tube tubeSelect;
    [SerializeField] private Tube tubeDestination;

    [Header("Input")]
    [SerializeField] private LayerMask layerMask;

    // Update is called once per frame

    #region Input Game
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector2 mousPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousPos, Vector2.zero, float.PositiveInfinity, layerMask);

            if (hit.collider != null)
            {
                if (tubeSelect == null)
                {
                    tubeSelect = hit.collider.GetComponent<Tube>();
                    tubeSelect.SelectBall();
                }
                else if(tubeSelect != null && tubeDestination == null && hit.collider.GetComponent<Tube>() != tubeSelect)
                {
                    tubeDestination = hit.collider.GetComponent<Tube>();
                    int size = tubeDestination.GetBallInTube().Count;


                    if (canMoveBall())
                    {
                        Vector3[] points = new Vector3[]
                        {
                            tubeSelect.TopPos.position,
                            tubeDestination.TopPos.position,
                        };

                        tubeSelect.MoveAllBall(points, () =>
                        {
                            Debug.Log("Call back");
                            //CheckWin();


                            tubeSelect.tmp = null;
                            tubeSelect = null;
                            tubeDestination = null;
                        });
                    }
                    else
                    {
                        tubeSelect.tmp.changeBody();
                        tubeSelect.tmp = null;
                        tubeDestination = null;
                        tubeSelect = null;
                    }
                }
            }
        }
    }
    #endregion


    private void CheckWin()
    {
        if (tubeDestination == null) return;

        int winCondition = 0;

        for(int i = 0; i < tubeDestination.GetBallInTube().Count; i++)
        {
            if(tubeDestination.GetBallInTube()[i].Type == tubeDestination.GetBallInTube()[i + 1].Type)
            {
                winCondition++;
            }
        }

        if(winCondition == 4)
        {
            Debug.Log("You win");
        }
    }


    private bool canMoveBall()
    {
        if(tubeDestination == null) return false;


        int size = tubeDestination.GetBallInTube().Count;
        if (tubeSelect.tmp.Type == tubeDestination.GetBallInTube()[size - 1].Type
            && tubeSelect.GetAmountBallToMove().Count < tubeDestination.MaxBall)
        {
            return true;
        }

        return false;
    }
}
