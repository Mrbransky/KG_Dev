using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

    public float shake = 0;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    public float smoothAmount = 0.5f;
    Vector3 Hvec = Vector3.zero;

	void Update () {

	    if(shake > 0)
        {
            Vector2 rand = Random.insideUnitCircle * shakeAmount;
            transform.position = Vector3.SmoothDamp(transform.position, new Vector3(rand.x, rand.y,10f)+transform.position, ref Hvec, smoothAmount);
            //----
            transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
            //----
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0.0f;
        }

	}
}
