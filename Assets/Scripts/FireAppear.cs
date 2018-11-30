using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAppear : MonoBehaviour {
    private float firingInterval = 3.0f;
    private int appearNumX = 0;
    private int appearNumZ = 0;
    [SerializeField]
    private GameObject fire;
    private GameObject[,] fires = new GameObject[3, 3];

	private void Start () 
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Vector3 createPoint = new Vector3(-3.5f + j, 1.5f, 0.5f - i);
                GameObject f = Instantiate(fire, createPoint, Quaternion.identity);
                fires[i, j] = f;
                fires[i, j].SetActive(false);
            }
        }
        StartCoroutine( Firing() );
	}

    IEnumerator Firing()
    {
        while(true)
        {
            appearNumX = Random.Range(0, 3);
            appearNumZ = Random.Range(0, 3);
            fires[appearNumX, appearNumZ].SetActive(true);
            yield return new WaitForSeconds(firingInterval);
        }
    }
}
