using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FadeOnTimeScale1 : MonoBehaviour 
{
    private Text _Text;

	void Start () 
    {
        _Text = GetComponent<Text>();
	}
	
	void Update () 
    {
        _Text.CrossFadeAlpha(0, 1, false);

        if (_Text.color.a <= 0)
        {
            Destroy(gameObject);
        }
	}
}
