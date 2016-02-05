using UnityEngine;
using System.Collections;

public class NewCameraBehavior : MonoBehaviour {

	private bool isOrthographic;
    public GameObject[] targets;
    public float currentDistance;
    public float largestDistance;
    public Camera theCamera;
    public float height = 5.0f;
    Vector3 avgDistance;
    float distance = 0.0f;                    // Default Distance 
    float speed = 1;
    float offset;
	void Start () {
	
    targets = GameObject.FindGameObjectsWithTag("Player"); 
 
    if(theCamera)
    {
    isOrthographic = theCamera.orthographic;
    }
 	//var globalControlScript = GameObject.Find("GlobalControl").GetComponent("GlobalControlScript");
 	theCamera.orthographicSize = 1 + (6f*targets.Length);
 
	}

    void LateUpdate()
    {

        targets = GameObject.FindGameObjectsWithTag("Player");


        if (!GameObject.FindWithTag("Player"))
            return;



        Vector3 sum =new Vector3(0, 0, 0);

        for (int n = 0; n < targets.Length; n++)
        {

            sum += targets[n].transform.position;

        }

        Vector3 avgDistance = sum / targets.Length;

        //    Debug.Log(avgDistance);

        float largestDifference = returnLargestDifference();

        height = Mathf.Lerp(height, largestDifference, Time.deltaTime * speed);

        if (targets.Length > 1)
        {
            if (isOrthographic)
            {

                theCamera.transform.Translate(avgDistance.x, 0, 0);

                theCamera.orthographicSize = largestDifference;
                if (theCamera.orthographicSize >= 10f)
                { theCamera.orthographicSize = 10f; }
                if (theCamera.orthographicSize <= 3f)
                { theCamera.orthographicSize = 3f; }

                theCamera.transform.Translate(0,avgDistance.y,0);

                theCamera.transform.LookAt(avgDistance);

            }
            else
            {

                theCamera.transform.Translate(avgDistance.x, 0, 0);

                theCamera.transform.Translate(0,0,avgDistance.z - distance + largestDifference);

                theCamera.transform.Translate(0, height,0);

                theCamera.transform.LookAt(avgDistance);

            }
            //var shakeScript = gameObject.GetComponent("CameraShakeScript");
            //shakeScript.Shake();

        }


    }
    float returnLargestDifference()
    {

        currentDistance = 0.0f;

        largestDistance = 0.0f;

        for (int i = 0; i < targets.Length; i++)
        {

            for (int j = 0; j < targets.Length; j++)
            {

                currentDistance = Vector3.Distance(targets[i].transform.position, targets[j].transform.position);

                if (currentDistance > largestDistance)
                {

                    largestDistance = currentDistance;

                }

            }

        }

        return largestDistance;

    }
}
