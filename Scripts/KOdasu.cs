using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characelsect1
{
    public class KOdasu : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        public void KOhyouzi()
        {
            transform.GetChild(2).gameObject.SetActive(true);
        }
    }
}
