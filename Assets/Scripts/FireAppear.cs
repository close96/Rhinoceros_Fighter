using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAppear : MonoBehaviour {
    [SerializeField]
    private GameObject fireAppear;
    [SerializeField]
    private GameObject[] fire;
    private float firingInterval = 3.0f;
    private int beforeNum = 0;
    private int appearNum = 0;

	private void Start () 
    {
        foreach (Transform item in fireAppear.transform)
        {
            item.gameObject.SetActive(false);
        }
        StartCoroutine( Firing() );
	}

    IEnumerator Firing()
    {
        while(true)
        {
            while (appearNum == beforeNum)
            {
                appearNum = Random.Range(0, 9);
            }
            fire[appearNum].SetActive(true);
            beforeNum = appearNum;
            yield return new WaitForSeconds(firingInterval);
        }
    }
}
