using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptIntro : MonoBehaviour
{
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        Player.SetActive(false);   
    }

    // Update is called once per frame
    public void FinAnim()
    {
        Player.SetActive(true);
        GetComponent<Camera>().depth = -1;
    }
}
