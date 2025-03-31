using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Level info
    [SerializeField] private Tube[] tubes;
    [SerializeField] private GameObject[] ballPrefabs;
    [SerializeField] private Transform parent;
    [SerializeField] private int numberTubeEmpty;

    [SerializeField] private List<int> ballColorDumps = new List<int>();
    private int totalColor; // số bóng tối đa ở mỗi tube
    private int maxTube; // số lượng Tube để đk win
    private int totalBalls; //Tổng số lượng bóng cần có
    private bool gameStart; // Quản lý việc khi nào game được bắt đầu

    public bool GameStart { get => gameStart; }
    public int MaxTube { get => maxTube; }

    void Start()
    {
        gameStart = false;
        tubes = GetComponentsInChildren<Tube>();
        maxTube = tubes.Length - numberTubeEmpty; // số tube có bóng
        totalColor = ballPrefabs.Length;
        totalBalls = totalColor * maxTube; // tổng số bóng của cả level

        List<int> ballColors = GenerateBall();
        ballColorDumps = ballColors;

        StartCoroutine(InitBall(ballColors));
    }


    #region Init Ball
    IEnumerator InitBall(List<int> ballColors)
    {
        int count = 0;
        int ballIndex = 0;
        while (count < totalColor)
        {
            for (int i = 0; i < maxTube; i++)
            {
                if (ballIndex >= ballColors.Count) // dừng vòng lặp nếu không đủ màu
                {
                    break;
                }

                yield return new WaitForSeconds(1f);
                GameObject tmp = Instantiate(ballPrefabs[ballColors[ballIndex]], tubes[i].TopPos.position, Quaternion.identity);
                tmp.GetComponent<Ball>().Move(tubes[i].PosArray[count]);


                tmp.transform.parent = parent;
                tubes[i].addBall(tmp.GetComponent<Ball>());
                ballIndex++;
            }
            count++;
        }

        gameStart = true;
    }

    private List<int> GenerateBall()
    {
        List<int> colors = new List<int>();
        int numberColors = ballPrefabs.Length; // Số lượng màu bóng đã có sẵn
        int ballPerColors = totalBalls / numberColors; // tống số màu của mỗi bóng

        for (int i = 0; i < numberColors; i++)
        {
            for (int j = 0; j < ballPerColors; j++)
            {
                Debug.Log("Color tye "+i+ ":" +i);
                colors.Add(i);
            }
        }

        while (colors.Count < totalBalls) // kiểm tra xem đã đủ số lượng màu chưa
        {
            Debug.Log("Add color còn thiếu trong tube");
            colors.Add(Random.Range(0, numberColors));
        }


        for (int i = colors.Count - 1; i > 0; i--) // trộn lại các màu
        {
            int randomIndex = Random.Range(0, i + 1);
            int temp = colors[i];
            colors[i] = colors[randomIndex];
            colors[randomIndex] = temp;
        }
        return colors;
    }

    #endregion

    public void ResetLevel() // Hàm Reset Game
    {
        if (parent.childCount == 0) return;

        for (int i = 0; i < parent.childCount; i++)
        {
            Destroy(parent.GetChild(i).gameObject);
        }

        for (int i = 0; i < tubes.Length; i++) // Reset lại cấu hình ban đầu
        {
            //tubes[i].ResetTube();
        }

        StartCoroutine(InitBall(ballColorDumps));
    }
}
