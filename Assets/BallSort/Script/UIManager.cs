using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
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
        GameManager.instance.LevelManager.ResetLevel();
        youWinPanel.SetActive(false);
    }

    private void UndoHandle()
    {
        GameManager.instance.UndoHandler();
    }
    
}
