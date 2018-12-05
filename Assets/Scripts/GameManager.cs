using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public static Dictionary<Vector3, string> canMoveArea = new Dictionary<Vector3, string>();
    public static Dictionary<Vector3, GameObject> objectPos = new Dictionary<Vector3, GameObject>();
    [SerializeField]
    private GameObject firePrefab;
    [SerializeField]
    private GameObject stonePrefab;
    [SerializeField]
    private GameObject woodPrefab;
    [SerializeField]
    private GameObject grassPrefab;

    private void Start()
    {
        for (int i = 0; i < lengthSquare; i++)
        {
            for (int j = 0; j < sideSquare; j++)
            {
                playArea[i, j] = new Vector3(GameManager.stageLeftLine + (j * distance), 1.4f, GameManager.stageTopLine - (i * distance));
                canMoveArea.Add(playArea[i, j], "true");
                objectPos.Add(playArea[i, j], null);
            }
        }
        canMoveArea[playArea[0, 2]] = "grass";
        canMoveArea[playArea[1, 2]] = "grass";
        canMoveArea[playArea[2, 0]] = "stone";
        canMoveArea[playArea[2, 1]] = "stone";
        canMoveArea[playArea[2, 2]] = "stone";
        canMoveArea[playArea[2, 3]] = "wood";
        canMoveArea[playArea[2, 4]] = "stone";
        canMoveArea[playArea[1, 0]] = "fire";
        canMoveArea[playArea[1, 3]] = "fire";
        canMoveArea[playArea[3, 2]] = "fire";

        for (int i = 0; i < lengthSquare; i++)
        {
            for(int j = 0; j < sideSquare; j++)
            {
                if (canMoveArea[playArea[i, j]] == "fire")
                {
                    objectPos[playArea[i, j]] = Instantiate(firePrefab, playArea[i, j], Quaternion.identity);
                }
                
                if (canMoveArea[playArea[i, j]] == "stone")
                {
                    objectPos[playArea[i, j]] = Instantiate(stonePrefab, playArea[i, j], stonePrefab.transform.rotation);
                }
                
                if (canMoveArea[playArea[i, j]] == "wood")
                {
                    objectPos[playArea[i, j]] = Instantiate(woodPrefab, playArea[i, j], woodPrefab.transform.rotation);
                }
                
                if (canMoveArea[playArea[i, j]] == "grass")
                {
                    objectPos[playArea[i, j]] = Instantiate(grassPrefab, playArea[i, j], grassPrefab.transform.rotation);
                }
            }
        }
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Stage2");
        }
    }
    
}