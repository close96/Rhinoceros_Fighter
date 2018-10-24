using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private int digestionCount = 0;
    private Vector3 pos;
    private static float stageLeftLine = -3.5f;
    private static float stageRightLine = 0.5f;
    private static float stageTopLine = 0.5f;
    private static float stageBottomLine = -3.5f;
	
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
        Vector3 targetPos;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            this.transform.position += Vector3.forward * 1.0f;
            //targetPos = this.transform.position + Vector3.forward * 1.0f;
            //if (!CanMoveCheck(this.transform.position, targetPos - this.transform.position)) return;

            //this.transform.position = targetPos;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.transform.rotation = Quaternion.Euler(0, 90.0f, 0);
            this.transform.position += Vector3.right * 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.transform.rotation = Quaternion.Euler(0, 180.0f, 0);
            this.transform.position += Vector3.back * 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.transform.rotation = Quaternion.Euler(0, 270.0f, 0);
            this.transform.position += Vector3.left * 1.0f;
        }

        pos = this.transform.position;

        this.transform.position = new Vector3(Mathf.Clamp(pos.x, stageLeftLine, stageRightLine), pos.y, Mathf.Clamp(pos.z, stageBottomLine, stageTopLine));
    }

    private bool CanMoveCheck(Vector3 _nowPos, Vector3 _direction)
    {
        Debug.Log(_direction);
        RaycastHit hit;
        Physics.Raycast(_nowPos, _direction, out hit, 1);
        if (hit.collider.gameObject.tag == "wood")
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
