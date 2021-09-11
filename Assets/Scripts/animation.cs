using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animation : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Vertical") > 0)
        {
            anim.SetBool("run", true);
        }
        else
        {
           anim.SetBool("run", false); 
        }
    }
}

