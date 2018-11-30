using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Scripting.Pipeline;

public class Player : MonoBehaviour {
    private int digestionCount = 0;
    private Vector3 targetPos;
    private static float stageLeftLine = -3.5f;
    private static float stageRightLine = 0.5f;
    private static float stageTopLine = 0.5f;
    private static float stageBottomLine = -3.5f;
    private float distance = 1.0f;
    private Vector3[,] playArea = new Vector3[5, 5];
    private Dictionary<Vector3, string> canMoveArea = new Dictionary<Vector3, string>();

    private void Start()
    {
        targetPos = this.transform.position;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                playArea[i, j] = new Vector3(stageLeftLine + (j * distance), 1.4f, stageTopLine - (i * distance));
                canMoveArea.Add(playArea[i, j], "true");
            }
        }
    }
    
	private void Update ()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space)) { digestionCount++; }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Fire") { digestionCount = 0; }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Fire")
        {
            // 10回ボタンを押したら火を消す
            if (digestionCount == 10) { other.gameObject.SetActive(false); }
        }
    }

    private void Move()
    {
        Vector3 moveCheckPos = this.transform.position;
            
        if (this.transform.position == targetPos)
        {
            // 上に移動
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                moveCheckPos.z = moveCheckPos.z + distance;
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                
                if (moveCheckPos.z <= stageTopLine && canMoveArea[moveCheckPos] == "true")
                {
                    targetPos.z = moveCheckPos.z;
                }
            }
        
            // 右に移動
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                moveCheckPos.x = moveCheckPos.x + distance;
                this.transform.rotation = Quaternion.Euler(0, 90.0f, 0);
                
                if (moveCheckPos.x <= stageRightLine && canMoveArea[moveCheckPos] == "true")
                {
                    targetPos.x = moveCheckPos.x;
                }
            }
        
            // 下に移動
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                moveCheckPos.z = moveCheckPos.z - distance;
                this.transform.rotation = Quaternion.Euler(0, 180.0f, 0);
                
                if (stageBottomLine <= moveCheckPos.z && canMoveArea[moveCheckPos] == "true")
                {
                    targetPos.z = moveCheckPos.z;
                }
            }
        
            // 左に移動
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                moveCheckPos.x = moveCheckPos.x - distance;
                this.transform.rotation = Quaternion.Euler(0, 270.0f, 0);
                
                if (stageLeftLine <= moveCheckPos.x && canMoveArea[moveCheckPos] == "true")
                {
                    targetPos.x = moveCheckPos.x;
                }
            }
            
        }

        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, Time.deltaTime * 7.5f);
        
    }

}
