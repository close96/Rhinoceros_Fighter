using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private int digestionCount = 0;
	
	private void Update ()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) { this.transform.position += Vector3.forward * 2.0f; }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { this.transform.position += Vector3.back * 2.0f; }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { this.transform.position += Vector3.right * 2.0f; }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { this.transform.position += Vector3.left * 2.0f; }
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
            if (digestionCount == 10) { other.gameObject.SetActive(false); }
        }
    }
}
