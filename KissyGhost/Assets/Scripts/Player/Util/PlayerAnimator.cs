using UnityEngine;
using System.Collections;

public class PlayerAnimator : MonoBehaviour 
{
    protected Animator anim;
    private Player.MoveAnim playerMoveAnim;

	void Awake () 
    {
        anim = this.GetComponent<Animator>();
        playerMoveAnim = this.GetComponent<Player>().MoveAnimation;
	}
	
    public void UpdateAnimation(Player.MoveAnim animation)
    {
        playerMoveAnim = animation;

        switch(animation)
        {
            case Player.MoveAnim.Idle:
                anim.SetBool("isMoving", false);
                break;

            case Player.MoveAnim.Walking:
                anim.SetBool("isMoving", true);
                break;
            case Player.MoveAnim.Kicking:
                anim.SetTrigger("Kick");
                break;
            case Player.MoveAnim.notKicking:
                anim.SetTrigger("StopKick");
                break;
        }
    }
}
