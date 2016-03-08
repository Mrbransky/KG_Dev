using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewCameraBehavior : MonoBehaviour {

	private bool isOrthographic;
    public List<GameObject> targets;
    public float currentDistance;
    public float largestDistance;
    public Camera theCamera;
    public float height = 5.0f;
    Vector3 avgDistance;
    public float distance = 0.0f;                    // Default Distance 
    public int heightSpeed = 1;
    public float smoothDampSpeed = 1.0f;
    public float offset;
    private float yVelocity = 0.0f;
    private float xVelocity = 0.0f;
    private float dVelocity = 0.0f;
	void Start () {
	        
        targets.AddRange(GameObject.FindGameObjectsWithTag("Player")); 


    if(theCamera)
    {
    isOrthographic = theCamera.orthographic;
    }
 	//var globalControlScript = GameObject.Find("GlobalControl").GetComponent("GlobalControlScript");
 	theCamera.orthographicSize = 1 + (6f*targets.Count);
 
	}

 
    void LateUpdate()
    {

        //targets = GameObject.FindGameObjectsWithTag("Player");


        //if (!GameObject.FindWithTag("Player"))
        //    return;



        Vector3 sum = new Vector3(0, 0, 0);

        for (int n = 0; n < targets.Count; n++)
        {

            sum += targets[n].transform.position;

        }

        Vector3 avgDistance = sum / targets.Count;

        //    Debug.Log(avgDistance);

        float largestDifference = returnLargestDifference();

        height = Mathf.Lerp(height, largestDifference, Time.deltaTime * heightSpeed);

        if (targets.Count > 1)
        {
            if (isOrthographic)
            {

                float targetSize = Mathf.SmoothDamp(theCamera.orthographicSize, largestDifference, ref dVelocity, smoothDampSpeed);
                theCamera.orthographicSize = targetSize;
                if (theCamera.orthographicSize >= 10f)
                { theCamera.orthographicSize = 10f; }
                if (theCamera.orthographicSize <= 3f)
                { theCamera.orthographicSize = 3f; }
				float newYDampedPos = Mathf.SmoothDamp(theCamera.transform.position.y, avgDistance.y, ref yVelocity, smoothDampSpeed);
				float newXDampedPos = Mathf.SmoothDamp(theCamera.transform.position.x, avgDistance.x, ref xVelocity, smoothDampSpeed);
				theCamera.transform.position = new Vector3(newXDampedPos, newYDampedPos, theCamera.transform.position.z);
                


                //theCamera.transform.LookAt(avgDistance);

            }
            else
            {

                theCamera.transform.position = new Vector3(avgDistance.x, theCamera.transform.position.y, theCamera.transform.position.z);

                theCamera.transform.position = new Vector3(theCamera.transform.position.x, theCamera.transform.position.y, avgDistance.z - distance + largestDifference);

                theCamera.transform.position = new Vector3(theCamera.transform.position.x, height, theCamera.transform.position.z);

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


        for (int i = 0; i < targets.Count; i++)
        {

            for (int j = 0; j < targets.Count; j++)
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
