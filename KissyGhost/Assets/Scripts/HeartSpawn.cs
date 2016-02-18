using UnityEngine;
using System.Collections;

public class HeartSpawn : MonoBehaviour {

    public GameObject Hearts;
    float timer;
	// Update is called once per frame
	void Update () {


        timer += Time.deltaTime;

        if (timer >= 1)
        {
            GameObject curHearts = Instantiate(Hearts, transform.position, Quaternion.identity) as GameObject;
            curHearts.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5);
            timer = 0;
        }
	
	}
}
