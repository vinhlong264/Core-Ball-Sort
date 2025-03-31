using UnityEngine;
using UnityEngine.UI;

public class LevelEditor : MonoBehaviour
{
    [SerializeField] private InputField input_Levels; //nhập tên levels
    [SerializeField] private InputField input_Bottles; // nhập số lượng Bottles
    [SerializeField] private Text status; // Trạng thái generate
    [SerializeField] private Button generateBtn; // Nút tạo level

    private void Start()
    {
        generateBtn.onClick.AddListener(() => GenerateLevel());
    }

    public void GenerateLevel()
    {
        if (string.IsNullOrEmpty(input_Levels.text) || string.IsNullOrEmpty(input_Bottles.text)) 
        {
            Debug.Log("Vui lòng không để trống trường nhập");
            return;
        }

        int levels = int.Parse(input_Levels.text);
        int bottles = int.Parse (input_Bottles.text);;

        if(levels <= 0)
        {
            Debug.Log("Level chỉ được phép nhập lớn hơn 0");
            return;
        }

        if(bottles <= 5)
        {
            Debug.Log("Số lượng bottles phải lớn hơn 5");
            return;
        }

        status.text = "tạo level thành công";
    }
}
