using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Scripting.Pipeline;

public class Player : MonoBehaviour {
    private int digestionCount = 0;
    private Vector3 pos;
    private Vector3 targetPos;
    private static float stageLeftLine = -3.5f;
    private static float stageRightLine = 0.5f;
    private static float stageTopLine = 0.5f;
    private static float stageBottomLine = -3.5f;

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
        float distance = 1.0f;
        
        // 上に移動
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            targetPos.z = this.transform.position.z + distance;
        }
        
        // 右に移動
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.transform.rotation = Quaternion.Euler(0, 90.0f, 0);
            targetPos.x = this.transform.position.x + distance;
        }
        
        // 下に移動
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.transform.rotation = Quaternion.Euler(0, 180.0f, 0);
            targetPos.z = this.transform.position.z - distance;
        }
        
        // 左に移動
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.transform.rotation = Quaternion.Euler(0, 270.0f, 0);
            targetPos.x = this.transform.position.x - distance;
        }

        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, Time.deltaTime * 5);
        
        // エリア設定
        pos = this.transform.position;

        this.transform.position = new Vector3(Mathf.Clamp(pos.x, stageLeftLine, stageRightLine), pos.y, Mathf.Clamp(pos.z, stageBottomLine, stageTopLine));
    }

}
