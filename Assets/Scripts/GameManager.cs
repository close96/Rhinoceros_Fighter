using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static float stageLeftLine = -3.5f;
    public static float stageRightLine = 0.5f;
    public static float stageTopLine = 0.5f;
    public static float stageBottomLine = -3.5f;
    public static float distance = 1.0f;
    private static int lengthSquare = 5;
    private static int sideSquare = 5;
    public static Vector3[,] playArea = new Vector3[lengthSquare, sideSquare];
    public static Dictionary<Vector3, GameObject> objectPos = new Dictionary<Vector3, GameObject>();
    
    [SerializeField]
    private GameObject firePrefab;
    [SerializeField]
    private GameObject stonePrefab;
    [SerializeField]
    private GameObject woodPrefab;
    [SerializeField]
    private GameObject grassPrefab;
    
    public static int currentStageNum = 0;
    public static int fireCount;
    public static bool stageLoadFlag = false;
    [SerializeField]
    private GameObject stageCanvas;
    [SerializeField]
    private Text timeLimit;
    private float remainingTime = 60;
    [SerializeField]
    private Text stageName;
    [SerializeField]
    private GameObject resultCanvas;
    [SerializeField]
    private Text resultMessage;
    [SerializeField]
    private Text score;
    private bool gameOverFlag = false;
    
    

    private void Awake()
    {
        // オブジェクト座標の用意
        for (int i = 0; i < lengthSquare; i++)
        {
            for (int j = 0; j < sideSquare; j++)
            {
                playArea[i, j] = new Vector3(stageLeftLine + (j * distance), 1.4f, stageTopLine - (i * distance));
                objectPos.Add(playArea[i, j], null);
            }
        }
        
        DontDestroyOnLoad(this);
        DontDestroyOnLoad(stageCanvas);
        DontDestroyOnLoad(resultCanvas);
        stageCanvas.SetActive(false);
        resultCanvas.SetActive(false);
    }

    private void Start()
    {
        stageLoadFlag = true;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        // タイトル画面でマットを踏んだらゲームスタート
        if (currentStageNum == 0)
        {
            if (Input.GetButtonDown(DDR.Start.ToDescription()) ||
                Input.GetButtonDown(DDR.Select.ToDescription()) ||
                Input.GetButtonDown(DDR.Up.ToDescription()) ||
                Input.GetButtonDown(DDR.Right.ToDescription()) ||
                Input.GetButtonDown(DDR.Down.ToDescription()) ||
                Input.GetButtonDown(DDR.Left.ToDescription()) ||
                Input.GetButtonDown(DDR.Circle.ToDescription()) ||
                Input.GetButtonDown(DDR.Cross.ToDescription()) ||
                Input.GetButtonDown(DDR.Square.ToDescription()) ||
                Input.GetButtonDown(DDR.Triangle.ToDescription()))
            {
                currentStageNum++;
                stageLoadFlag = true;
            }
        }

        // 制限時間のカウントダウンとタイムアップ処理
        if (currentStageNum != 0 &&
            currentStageNum != 1 &&
            currentStageNum != 4)
        {
            remainingTime -= Time.deltaTime;
            timeLimit.text = "じかん " + ((int) remainingTime + 1);
            if (remainingTime + 1 <= 1)
            {
                currentStageNum = 4;
                stageLoadFlag = true;
                gameOverFlag = true;
            }
        }
        
        // リザルトからタイトルへ
        if (currentStageNum == 4)
        {
            if (Input.GetButtonDown(DDR.Select.ToDescription()))
            {
                currentStageNum = 0;
                stageLoadFlag = true;
            }
        }
        
        // ステージ遷移
        if (stageLoadFlag)
        {
            switch (currentStageNum)
            {
                case 0:
                    SceneManager.LoadScene("Title");
                    stageLoadFlag = false;
                    gameOverFlag = false;
                    remainingTime = 60;
                    break;
                
                case 1:
                    SceneManager.LoadScene("Stage1");
                    stageLoadFlag = false;
                    break;
                
                case 2:
                    SceneManager.LoadScene("Stage2");
                    stageLoadFlag = false;
                    break;
                
                case 3:
                    SceneManager.LoadScene("Stage3");
                    stageLoadFlag = false;
                    break;
                
                case 4:
                    SceneManager.LoadScene("Result");
                    stageCanvas.SetActive(false);
                    stageLoadFlag = false;
                    break;
            }
        }
        
        // テスト用のステージスキップ
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentStageNum++;
            stageLoadFlag = true;
        }
    }
    
    private void OnSceneLoaded( Scene scene, LoadSceneMode mode )
    {
        switch (currentStageNum)
        {
            // タイトル画面
            case 0:
                resultCanvas.SetActive(false);
                break;
            
            // ステージ１のオブジェクト配置
            case 1:
                AudioManager.Instance.PlayBgm(BackGroundMusicEnum.GameBGM);
        
                fireCount = 3;
                // 草
                objectPos[playArea[0, 2]] = Instantiate(grassPrefab, playArea[0, 2], grassPrefab.transform.rotation);
                objectPos[playArea[1, 2]] = Instantiate(grassPrefab, playArea[1, 2], grassPrefab.transform.rotation);
                // 岩
                objectPos[playArea[2, 0]] = Instantiate(stonePrefab, playArea[2, 0], stonePrefab.transform.rotation);
                objectPos[playArea[2, 1]] = Instantiate(stonePrefab, playArea[2, 1], stonePrefab.transform.rotation);
                objectPos[playArea[2, 2]] = Instantiate(stonePrefab, playArea[2, 2], stonePrefab.transform.rotation);
                objectPos[playArea[2, 4]] = Instantiate(stonePrefab, playArea[2, 4], stonePrefab.transform.rotation);
                // 木
                objectPos[playArea[2, 3]] = Instantiate(woodPrefab, playArea[2, 3], woodPrefab.transform.rotation);
                // 炎
                objectPos[playArea[1, 0]] = Instantiate(firePrefab, playArea[1, 0], Quaternion.identity);
                objectPos[playArea[1, 3]] = Instantiate(firePrefab, playArea[1, 3], Quaternion.identity);
                objectPos[playArea[3, 2]] = Instantiate(firePrefab, playArea[3, 2], Quaternion.identity);
                
                stageCanvas.SetActive(true);
                timeLimit.gameObject.SetActive(false);
                break;
            
            // ステージ２のオブジェクト配置
            case 2:
                fireCount = 5;
            
                // 草 
                objectPos[playArea[1, 1]] = Instantiate(grassPrefab, playArea[1, 1], grassPrefab.transform.rotation);
                objectPos[playArea[3, 4]] = Instantiate(grassPrefab, playArea[3, 4], grassPrefab.transform.rotation);
                // 岩
                objectPos[playArea[0, 3]] = Instantiate(stonePrefab, playArea[0, 3], stonePrefab.transform.rotation);
                objectPos[playArea[2, 2]] = Instantiate(stonePrefab, playArea[2, 2], stonePrefab.transform.rotation);
                objectPos[playArea[2, 3]] = Instantiate(stonePrefab, playArea[2, 3], stonePrefab.transform.rotation);
                objectPos[playArea[4, 1]] = Instantiate(stonePrefab, playArea[4, 1], stonePrefab.transform.rotation);
                // 木
                objectPos[playArea[0, 1]] = Instantiate(woodPrefab, playArea[0, 1], woodPrefab.transform.rotation);
                objectPos[playArea[1, 0]] = Instantiate(woodPrefab, playArea[1, 0], woodPrefab.transform.rotation);
                // 炎
                objectPos[playArea[0, 0]] = Instantiate(firePrefab, playArea[0, 0], Quaternion.identity);
                objectPos[playArea[0, 4]] = Instantiate(firePrefab, playArea[0, 4], Quaternion.identity);
                objectPos[playArea[2, 1]] = Instantiate(firePrefab, playArea[2, 1], Quaternion.identity);
                objectPos[playArea[3, 3]] = Instantiate(firePrefab, playArea[3, 3], Quaternion.identity);
                objectPos[playArea[4, 0]] = Instantiate(firePrefab, playArea[4, 0], Quaternion.identity);
                
                timeLimit.gameObject.SetActive(true);
                break;
            
            // ステージ３のオブジェクト配置
            case 3:
                fireCount = 7;
            
                // 草
                objectPos[playArea[1, 0]] = Instantiate(grassPrefab, playArea[1, 0], grassPrefab.transform.rotation);
                objectPos[playArea[2, 3]] = Instantiate(grassPrefab, playArea[2, 3], grassPrefab.transform.rotation);
                objectPos[playArea[3, 2]] = Instantiate(grassPrefab, playArea[3, 2], grassPrefab.transform.rotation);
                // 岩
                objectPos[playArea[2, 0]] = Instantiate(stonePrefab, playArea[2, 0], stonePrefab.transform.rotation);
                objectPos[playArea[3, 1]] = Instantiate(stonePrefab, playArea[3, 1], stonePrefab.transform.rotation);
                objectPos[playArea[3, 3]] = Instantiate(stonePrefab, playArea[3, 3], stonePrefab.transform.rotation);
                // 木
                objectPos[playArea[0, 1]] = Instantiate(woodPrefab, playArea[0, 1], woodPrefab.transform.rotation);
                objectPos[playArea[0, 2]] = Instantiate(woodPrefab, playArea[0, 2], woodPrefab.transform.rotation);
                objectPos[playArea[1, 4]] = Instantiate(woodPrefab, playArea[1, 4], woodPrefab.transform.rotation);
                // 炎
                objectPos[playArea[0, 0]] = Instantiate(firePrefab, playArea[0, 0], Quaternion.identity);
                objectPos[playArea[0, 4]] = Instantiate(firePrefab, playArea[0, 4], Quaternion.identity);
                objectPos[playArea[1, 3]] = Instantiate(firePrefab, playArea[1, 3], Quaternion.identity);
                objectPos[playArea[2, 1]] = Instantiate(firePrefab, playArea[2, 1], Quaternion.identity);
                objectPos[playArea[3, 0]] = Instantiate(firePrefab, playArea[3, 0], Quaternion.identity);
                objectPos[playArea[3, 4]] = Instantiate(firePrefab, playArea[3, 4], Quaternion.identity);
                objectPos[playArea[4, 3]] = Instantiate(firePrefab, playArea[4, 3], Quaternion.identity);
                break;
            
            // リザルト画面
            case 4:
                AudioManager.Instance.StopBgm();
                resultCanvas.SetActive(true);
                
                if (gameOverFlag)
                {
                    resultMessage.text = "ざんねん！じかんないに\n" +
                                         "火をぜんぶ けせなかった…";
                }
                else
                {
                    resultMessage.text = "おめでとう！じかんないに\n" +
                                         "火をぜんぶ けせたよ！";
                }

                // score.text = "のこりじかん " + (int)(remainingTime + 1) + "びょう";
                break;
        } 
        
        stageName.text = "ステージ" + currentStageNum;
    }
    
}