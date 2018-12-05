using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Scripting.Pipeline;

public class Player : MonoBehaviour
{
    private int digestionCount = 0;
    private int destroyCount = 0;
    private Vector3 targetPos;

    private void Start()
    {
        targetPos = this.transform.position;
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
                moveCheckPos.z = moveCheckPos.z + GameManager.distance;
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                
                if (moveCheckPos.z <= GameManager.stageTopLine &&
                    (GameManager.objectPos[moveCheckPos] == null ||
                     !(GameManager.objectPos[moveCheckPos].CompareTag("Stone") ||
                       GameManager.objectPos[moveCheckPos].CompareTag("Wood"))))
                {
                    targetPos.z = moveCheckPos.z;
                }

                if (GameManager.objectPos[moveCheckPos].CompareTag("Wood"))
                {
                    WoodDestroy(moveCheckPos);
                }
            }
        
            // 右に移動
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                moveCheckPos.x = moveCheckPos.x + GameManager.distance;
                this.transform.rotation = Quaternion.Euler(0, 90.0f, 0);
                
                if (moveCheckPos.x <= GameManager.stageRightLine &&
                    (GameManager.objectPos[moveCheckPos] == null ||
                     !(GameManager.objectPos[moveCheckPos].CompareTag("Stone") ||
                       GameManager.objectPos[moveCheckPos].CompareTag("Wood"))))
                {
                    targetPos.x = moveCheckPos.x;
                }
                
                if (GameManager.objectPos[moveCheckPos].CompareTag("Wood"))
                {
                    WoodDestroy(moveCheckPos);
                }
            }
        
            // 下に移動
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                moveCheckPos.z = moveCheckPos.z - GameManager.distance;
                this.transform.rotation = Quaternion.Euler(0, 180.0f, 0);
                
                if (GameManager.stageBottomLine <= moveCheckPos.z &&
                    (GameManager.objectPos[moveCheckPos] == null ||
                     !(GameManager.objectPos[moveCheckPos].CompareTag("Stone") ||
                       GameManager.objectPos[moveCheckPos].CompareTag("Wood"))))
                {
                    targetPos.z = moveCheckPos.z;
                }
                
                if (GameManager.objectPos[moveCheckPos].CompareTag("Wood"))
                {
                    WoodDestroy(moveCheckPos);
                }
            }
        
            // 左に移動
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                moveCheckPos.x = moveCheckPos.x - GameManager.distance;
                this.transform.rotation = Quaternion.Euler(0, 270.0f, 0);
                
                if (GameManager.stageLeftLine <= moveCheckPos.x &&
                    (GameManager.objectPos[moveCheckPos] == null ||
                     !(GameManager.objectPos[moveCheckPos].CompareTag("Stone") ||
                       GameManager.objectPos[moveCheckPos].CompareTag("Wood"))))
                {
                    targetPos.x = moveCheckPos.x;
                }
                
                if (GameManager.objectPos[moveCheckPos].CompareTag("Wood"))
                {
                    WoodDestroy(moveCheckPos);
                }
            }
            
        }

        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, Time.deltaTime * 7.5f);
    }

    private void WoodDestroy(Vector3 _moveCheckPos)
    {
        destroyCount++;

        if (destroyCount == 10)
        {
            GameManager.objectPos[_moveCheckPos].SetActive(false);
            GameManager.objectPos[_moveCheckPos] = null;
            destroyCount = 0;
        }
    }

}
