using UnityEngine;
using System.Collections;

public enum KissedFurnitureBehavior
{
    None = 0,
    FollowPlayer = 1
}

public class KissableFurniture : MonoBehaviour
{
    [SerializeField] private KissedFurnitureBehavior kissedBehavior = KissedFurnitureBehavior.None;
    public Color kissedColor = new Color(255.0f / 255.0f, 192.0f / 255.0f, 203.0f / 255.0f);
    private bool isKissed = false;

    [SerializeField] private float minFollowDistance = 1.0f;
    [SerializeField] private float followSpeed = 3.5f;
    private Transform firstPlayerTransform;

#if UNITY_EDITOR
    public KeyCode KissKey = KeyCode.Alpha0;
#endif

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KissKey))
        {
            isKissed = !isKissed;

            if (isKissed)
            {
                KissFurniture();
            }
            else
            {
                UnkissFurniture();
            }
        }
#endif

        if (isKissed)
        {
            switch ((int)kissedBehavior)
            {
                case (int)KissedFurnitureBehavior.FollowPlayer:
                    Vector3 moveDir = firstPlayerTransform.position - transform.position;
                    float distanceFromPlayer = moveDir.magnitude;
                    moveDir.Normalize();

                    if (distanceFromPlayer > minFollowDistance)
                    {
                        GetComponent<Rigidbody2D>().velocity = moveDir * followSpeed;
                    }
                    else
                    {
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                    }
                    break;
            }
        }
    }

    public void KissFurniture()
    {
        isKissed = true;
        OnFurnitureKissed();
    }

    private void OnFurnitureKissed()
    {
        GetComponent<SpriteRenderer>().color = kissedColor;

        switch((int)kissedBehavior)
        {
            case (int)KissedFurnitureBehavior.FollowPlayer:
                foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))
                {
                    if (player.name != "Ghost")
                    {
                        firstPlayerTransform = player.transform;
                        break;
                    }
                }
                break;
        }
    }

    public void UnkissFurniture()
    {
        isKissed = false;
        OnFurnitureUnkissed();
    }

    private void OnFurnitureUnkissed()
    {
        GetComponent<SpriteRenderer>().color = Color.white;

        switch ((int)kissedBehavior)
        {
            case (int)KissedFurnitureBehavior.FollowPlayer:
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                break;
        }
    }
}
