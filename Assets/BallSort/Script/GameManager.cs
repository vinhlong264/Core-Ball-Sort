using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private LevelManager levelManager;
    public System.Action OnYouWin;
    private int countTube;

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
        if(countTube == levelManager.MaxTube)
        {
            Debug.Log("you win");
        }
    }


}


