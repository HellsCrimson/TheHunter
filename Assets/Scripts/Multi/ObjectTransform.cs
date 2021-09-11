using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTransform : MonoBehaviour
{
    public string objectName;
    public bool open;
    [SerializeField] MeshRenderer graphics;
    
    public void Open()
    { 
        open = true; 
        gameObject.SetActive(open); 
    }
    
    public void Close()
    {
        open = false;
        gameObject.SetActive(open); 
    }

    public void DesactivateGraphics()
    {
        if (graphics == null)
        {
            var mesh = GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < mesh.Length; i++)
            {
                mesh[i].enabled = !mesh[i].enabled;
            }
        }
        else
            graphics.enabled = !graphics.enabled;
    }
}
