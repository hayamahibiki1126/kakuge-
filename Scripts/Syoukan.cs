using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characelsect1
{
    public class Syoukan : MonoBehaviour
    {
        public MyGameManagerData myGameManagerData;
        public GameObject char1;
        public GameObject Player1;
        // Start is called before the first frame update
        void Start()
        {
            if (FindObjectOfType<MyGameManager>() != null)
            {
                myGameManagerData = FindObjectOfType<MyGameManager>().GetMyGameManagerData();
                char1 = myGameManagerData.GetCharacter();
                if (char1 != null)
                {
                    Instantiate(char1);
                }
                else
                {
                    Instantiate(Player1);
                }
            }
            else
            {
                Instantiate(Player1);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

