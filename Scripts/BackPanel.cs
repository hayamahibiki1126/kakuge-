using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPanel : MonoBehaviour
{

    public int lefttime = 9999999;
    public GameObject panel;
    // Start is called before the first frame update
    void Start()
    {
        lefttime = 60;
    }

    public void OnSet()
    {
        lefttime = 360;
        panel.SetActive(true);
    }

    public void OnRemove()
    {
        //panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        lefttime -= 1;
        if (lefttime <= 0) DestroyImmediate(gameObject, true); ;

    }

}
