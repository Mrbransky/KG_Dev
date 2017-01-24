using UnityEngine;
using System.Collections;

public class ShootThatStar : MonoBehaviour {

    Animator anim;
    float TimeUntilAnim;

    bool IsShooting
    {
        get { return anim.GetBool("IsShooting"); }
        set { anim.SetBool("IsShooting", value); }
    }

    bool TimeSet
    {
        get { return anim.GetBool("TimeSet"); }
        set { anim.SetBool("TimeSet", value); }
    }

    void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	void Update ()
    {
        if (TimeUntilAnim <= 0 && !IsShooting)
            TimeUntilAnim = ShuffleBag.shuffle.StarListNext();

        else
        {
            if (TimeUntilAnim > 0 && !TimeSet)
                TimeSet = true;

            else if (TimeUntilAnim > 0)
            {
                TimeUntilAnim -= Time.deltaTime;

                if (TimeUntilAnim <= 0 && !IsShooting)
                    IsShooting = true;
            }

            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("EndShooting"))
                IsShooting = false;

            else if (IsShooting)
                TimeSet = false;

            
        }


        //if (TimeSet == false)
        //{
        //    TimeUntilAnim = ShuffleBag.shuffle.StarListNext();
        //    IsShooting = false;
        //    TimeSet = true;

        //    Debug.Log(TimeUntilAnim);
        //}

        //if (TimeUntilAnim > 0 && !IsShooting)
        //    TimeUntilAnim -= Time.deltaTime;
        //else
        //    IsShooting = true;

        //if(TimeUntilAnim <= 0)
        //    TimeSet = false;
        
        
	}
}
