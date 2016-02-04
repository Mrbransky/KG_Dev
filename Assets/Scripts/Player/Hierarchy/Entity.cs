using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour 
{
    public virtual Vector2 moveDir { get; set; }
    protected Rigidbody2D rigidBody;

    public float baseSpeed;
    public float currentSpeed;

    public virtual void Awake()
    {
        rigidBody = this.GetComponent<Rigidbody2D>();
    }
	
	public virtual void Update () 
    {
        if (moveDir != Vector2.zero && baseSpeed > 0)
            ApplyMovement();
	}

    protected void ApplyMovement()
    {
        Vector3 calc = new Vector3(moveDir.x, moveDir.y, 0) * currentSpeed * Time.deltaTime;
        this.rigidBody.transform.position += calc;
    }
}
