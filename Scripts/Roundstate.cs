using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characelsect1
{
    public class Roundstate : MonoBehaviour
    {
        public int time, win1, win2;
        public MyGameManagerData myGameManagerData;
        public GameObject oya;
        // Start is called before the first frame update
        void Start()
        {
            myGameManagerData = FindObjectOfType<MyGameManager>().GetMyGameManagerData();
            oya = GameObject.Find("Canvas/HPBarP");
        }

        // Update is called once per frame
        void Update()
        {
            time += 1;
            if(time == 60)
            {
                transform.Translate(0, 0.2f, 0);
                win1 = myGameManagerData.GetWin(1);
                win2 = myGameManagerData.GetWin(2);
                switch(win1 + win2)
                {
                    case 0:
                        oya.GetComponent<HPBar>()._roundstate.text = string.Format("Round 1");
                        break;
                    case 1:
                        oya.GetComponent<HPBar>()._roundstate.text = string.Format("Round 2");
                        break;
                    case 2:
                        oya.GetComponent<HPBar>()._roundstate.text = string.Format("Round 3");
                        break;
                    default:
                        oya.GetComponent<HPBar>()._roundstate.text = string.Format("Round X");
                        break;
                }
            }
            if (time == 180 || time == 20) oya.GetComponent<HPBar>()._roundstate.text = string.Format("");
            if (time == 240)
            {
                transform.Translate(0, 0.4f, 0);
                oya.GetComponent<HPBar>()._roundstate.fontSize += 40;
                oya.GetComponent<HPBar>()._roundstate.text = string.Format("FIGHT!");
            }
            if (time >= 60 && time <= 80 || time >= 160 && time <= 180 || time >= 240 && time <= 260 || time >= 340 && time <= 360)
            {
                transform.Translate(0, -0.01f, 0);
            }
            if (time == 360)
            {
                oya.GetComponent<HPBar>()._roundstate.text = string.Format("");
                Destroy(gameObject);
            }
        }
    }
}
