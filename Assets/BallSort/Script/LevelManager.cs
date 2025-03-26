﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    //Level info
    [SerializeField] private Tube[] tubes;
    [SerializeField] private GameObject[] ballPrefabs;
    [SerializeField] private int maxBall; // số bóng tối đa ở mỗi tube
    [SerializeField] private Transform parent;

    [SerializeField] private List<int> ballColorDumps = new List<int>();
    private int maxTube; // số lượng Tube để đk win
    private int totalBalls; //Tổng số lượng bóng cần có
    private bool gameStart; // Quản lý việc khi nào game được bắt đầu

    public bool GameStart { get => gameStart; }
    public int MaxTube { get => maxTube; }

    void Start()
    {
        gameStart = false;
        tubes = GetComponentsInChildren<Tube>();
        maxTube = tubes.Length - 2; // số tube có bóng
        totalBalls = maxBall * maxTube; // tổng số bóng của cả level

        List<int> ballColors = GenerateBall();
        ballColorDumps = ballColors;

        StartCoroutine(InitBall(ballColors));
    }


    #region Init Ball
    IEnumerator InitBall(List<int> ballColors)
    {
        int count = 0;
        int ballIndex = 0;
        while (count < maxBall)
        {
            for (int i = 0; i < maxTube; i++)
            {
                if (ballIndex >= ballColors.Count) // dừng vòng lặp nếu không đủ màu
                {
                    break;
                }

                yield return new WaitForSeconds(0.75f);
                GameObject tmp = Instantiate(ballPrefabs[ballColors[ballIndex]], tubes[i].TopPos.position, Quaternion.identity);
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
                Debug.Log("index Ball: " + i);
                colors.Add(i);
            }
        }

        while (colors.Count < totalBalls) // kiểm tra xem đã đủ số lượng màu chưa
        {
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
            tubes[i].ResetTube();
        }

        StartCoroutine(InitBall(ballColorDumps));
    }
}
