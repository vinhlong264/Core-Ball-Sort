using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LevelManager levelMangaer;
    [SerializeField] private GameObject youWinPanel;

    [SerializeField] private Button ResetBtn;
    void Start()
    {
        ResetBtn.onClick.AddListener(() => ResetGameHandler());
    }

    private void ResetGameHandler()
    {
        levelMangaer.ResetLevel();
        youWinPanel.SetActive(false);
    }
    
}
