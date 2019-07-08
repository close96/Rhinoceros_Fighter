using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private int digestionCount = 0;
    private int destroyCount = 0;
    private Vector3 targetPos;
    private float moveSpeed = 7.5f;

    private void Start()
    {
        targetPos = this.transform.position;
    }
    
	private void Update ()
    {
        Move();

        if (GameManager.objectPos[this.transform.position] != null &&
            GameManager.objectPos[this.transform.position].CompareTag("Fire"))
        {
            if (Input.GetButtonDown(DDR.Circle.ToDescription()) ||
                Input.GetButtonDown(DDR.Cross.ToDescription()) ||
                Input.GetButtonDown(DDR.Triangle.ToDescription()) ||
                Input.GetButtonDown(DDR.Square.ToDescription()))
            {
                FireExtinction();
            }
        }
        
        if (GameManager.objectPos[this.transform.position] != null &&
            GameManager.objectPos[this.transform.position].CompareTag("Grass"))
        {
            float effectJudge = Random.Range(0.0f, 100.0f);

            if (effectJudge <= 50.0f)
            {
                this.moveSpeed = this.moveSpeed / 2f;
            }

            else
            {
                this.moveSpeed = this.moveSpeed * 2f;
            }
            
            GameManager.objectPos[this.transform.position].SetActive(false);
            GameManager.objectPos[this.transform.position] = null;
            AudioManager.Instance.PlaySe(SoundEffectEnum.grassIn);
        }
	}
    
    private void Move()
    {
        Vector3 moveCheckPos = this.transform.position;
            
        if (this.transform.position == targetPos)
        {
            // 上に移動
            if (Input.GetButtonDown(DDR.Up.ToDescription()))
            {
                moveCheckPos.z = moveCheckPos.z + GameManager.distance;
                this.transform.rotation = Quaternion.Euler(0, 0, 0);
                
                if (moveCheckPos.z <= GameManager.stageTopLine &&
                    (GameManager.objectPos[moveCheckPos] == null ||
                     !(GameManager.objectPos[moveCheckPos].CompareTag("Stone") ||
                       GameManager.objectPos[moveCheckPos].CompareTag("Wood"))))
                {
                    targetPos.z = moveCheckPos.z;
                    digestionCount = 0;
                    destroyCount = 0;
                }

                if (GameManager.objectPos[moveCheckPos] != null &&
                    GameManager.objectPos[moveCheckPos].CompareTag("Wood"))
                {
                    WoodDestroy(moveCheckPos);
                }
            }
        
            // 右に移動
            if (Input.GetButtonDown(DDR.Right.ToDescription()))
            {
                moveCheckPos.x = moveCheckPos.x + GameManager.distance;
                this.transform.rotation = Quaternion.Euler(0, 90.0f, 0);
                
                if (moveCheckPos.x <= GameManager.stageRightLine &&
                    (GameManager.objectPos[moveCheckPos] == null ||
                     !(GameManager.objectPos[moveCheckPos].CompareTag("Stone") ||
                       GameManager.objectPos[moveCheckPos].CompareTag("Wood"))))
                {
                    targetPos.x = moveCheckPos.x;
                    digestionCount = 0;
                    destroyCount = 0;
                }
                
                if (GameManager.objectPos[moveCheckPos] != null &&
                    GameManager.objectPos[moveCheckPos].CompareTag("Wood"))
                {
                    WoodDestroy(moveCheckPos);
                }
            }
        
            // 下に移動
            if (Input.GetButtonDown(DDR.Down.ToDescription()))
            {
                moveCheckPos.z = moveCheckPos.z - GameManager.distance;
                this.transform.rotation = Quaternion.Euler(0, 180.0f, 0);
                
                if (GameManager.stageBottomLine <= moveCheckPos.z &&
                    (GameManager.objectPos[moveCheckPos] == null ||
                     !(GameManager.objectPos[moveCheckPos].CompareTag("Stone") ||
                       GameManager.objectPos[moveCheckPos].CompareTag("Wood"))))
                {
                    targetPos.z = moveCheckPos.z;
                    digestionCount = 0;
                    destroyCount = 0;
                }
                
                if (GameManager.objectPos[moveCheckPos] != null &&
                    GameManager.objectPos[moveCheckPos].CompareTag("Wood"))
                {
                    WoodDestroy(moveCheckPos);
                }
            }
        
            // 左に移動
            if (Input.GetButtonDown(DDR.Left.ToDescription()))
            {
                moveCheckPos.x = moveCheckPos.x - GameManager.distance;
                this.transform.rotation = Quaternion.Euler(0, 270.0f, 0);
                
                if (GameManager.stageLeftLine <= moveCheckPos.x &&
                    (GameManager.objectPos[moveCheckPos] == null ||
                     !(GameManager.objectPos[moveCheckPos].CompareTag("Stone") ||
                       GameManager.objectPos[moveCheckPos].CompareTag("Wood"))))
                {
                    targetPos.x = moveCheckPos.x;
                    digestionCount = 0;
                    destroyCount = 0;
                }
                
                if (GameManager.objectPos[moveCheckPos] != null &&
                    GameManager.objectPos[moveCheckPos].CompareTag("Wood"))
                {
                    WoodDestroy(moveCheckPos);
                }
            }
            
        }

        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, Time.deltaTime * moveSpeed);
    }

    private void FireExtinction()
    {
        digestionCount++;
        AudioManager.Instance.PlaySe(SoundEffectEnum.earthquake);
        
        if (digestionCount == 10)
        {
            GameManager.objectPos[this.transform.position].SetActive(false);
            GameManager.objectPos[this.transform.position] = null;
            digestionCount = 0;
            GameManager.fireCount--;
            AudioManager.Instance.PlaySe(SoundEffectEnum.digestion);
        }

        if (GameManager.fireCount == 0)
        {
            GameManager.currentStageNum++;
            GameManager.stageLoadFlag = true;
        }
        
        // 火を消す時にぴょんぴょんする
        Vector3 jumpPos = this.transform.position;
        jumpPos.y = this.transform.position.y + 2;
        this.transform.position = Vector3.MoveTowards(this.transform.position, jumpPos, Time.deltaTime * moveSpeed);
    }

    private void WoodDestroy(Vector3 _moveCheckPos)
    {
        destroyCount++;
        AudioManager.Instance.PlaySe(SoundEffectEnum.woodAtack);

        if (destroyCount == 5)
        {
            GameManager.objectPos[_moveCheckPos].SetActive(false);
            GameManager.objectPos[_moveCheckPos] = null;
            destroyCount = 0;
            AudioManager.Instance.PlaySe(SoundEffectEnum.woodDestroy);
        }
    }

}
