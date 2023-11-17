using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitefeect2 : MonoBehaviour
{
    public float lefttime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lefttime);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0.05f, 0, 0);

    }
}
