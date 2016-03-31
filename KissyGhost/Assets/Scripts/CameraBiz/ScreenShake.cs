using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour {

    public float shake = 0;
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    Vector3 Hvec = Vector3.zero;

	void Update () {
	    if(shake > 0)
        {
            this.transform.localPosition = Vector3.SmoothDamp(transform.position, Random.insideUnitCircle * shakeAmount, ref Hvec,0.5f);
            //----
            this.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -10f);
            //----
            shake -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shake = 0.0f;
        }
	}
}
