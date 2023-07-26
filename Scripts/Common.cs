using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Characelsect1
{
    public class Common : MonoBehaviour
    {
        [SerializeField]
        GameObject transitionPrefab;

        Rigidbody2D rb;
        Animator anim;
        public HPBar hpBar;
        public int hp = 1000, powermax = 2000, power, side, powerprev, rstime, hitcount, tokitome, noko, airf, onaji;
        public int hits, hitt, gr, flagh, face, dash, syagami, dawn, guard, waza, pause, muteki, win, lose = 0;
        public float xx, yy;
        public float xcommon, ycommon;
        public GameObject KOdasu;
        public GameObject enemyObj;
        public MyGameManagerData myGameManagerData;
        void Start()
        {
            myGameManagerData = FindObjectOfType<MyGameManager>().GetMyGameManagerData();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            KOdasu = GameObject.Find("Canvas");
            if (this.gameObject.tag == "Player")
            {
                face = 1;
                hpBar = GameObject.Find("Canvas/HPBarP").GetComponent<HPBar>();
                side = 1;
            }
            else
            {
                face = -1;
                hpBar = GameObject.Find("Canvas/HPBar").GetComponent<HPBar>();
                side = 2;
            }
            power = myGameManagerData.GetPower(side);
            win = myGameManagerData.GetWin(this.GetComponent<Common>().side);
            hpBar.SetWin(win);
        }

        // Update is called once per frame
        void Update()
        {
            if(tokitome >= 1)
            {
                tokitome -= 1;
            }
            else
            {
                if (rstime < 360) rstime += 1;
                if (power > powermax) power = powermax;
                if (power != powerprev || power == 0) hpBar.UpdatePower(power);
                powerprev = power;
                Vector3 tmp = transform.position;
                if (tmp.x >= 2.765f) transform.Translate(2.765f - tmp.x, 0, 0);
                if (tmp.x <= -2.547f) transform.Translate(-2.547f - tmp.x, 0, 0);
                if (tmp.y <= -1.04f && dawn < 19 && (syagami != 19 || flagh == 19)) transform.Translate(0, -1.04f - tmp.y, 0);
                tmp = transform.position;
                xcommon = tmp.x;
                ycommon = tmp.y;
                if (ycommon >= -1.04f && flagh != 19)
                {
                    airf = 0;
                    onaji = 0;
                    rb.isKinematic = false;
                }
                else
                {
                    rb.isKinematic = true;
                }
                if (flagh == 19)
                {
                    if (pause > 0)
                    {
                        pause -= 1;
                    }
                    if (pause <= 0 && hits >= 1)
                    {
                        transform.Translate(xx, yy - (0.098f / 180) * gr, 0);
                        hits -= 1;
                        gr += 1;
                    }
                    //tmp = transform.position;
                    if (pause <= 0 && hits <= 0 && ycommon >= -1)
                    {
                        xx = 0;
                        yy -= (0.098f / 120);
                        transform.Translate(0, yy, 0);//(float)((9.8 / 120) * j)
                        gr += 1;
                    }
                    if (pause <= 0 && hits <= 0 && hitt > 0)
                    {
                        hitt -= 1;
                    }
                    tmp = transform.position;
                    if (pause <= 0 && hits <= 0 && ycommon <= -1 && (hitt <= 0 || yy != 0f))
                    {
                        gr = 0;
                        rb.velocity = new Vector2(0, 0);
                        xx = 0;
                        hpBar.ReColor();
                        var seq = DOTween.Sequence();
                        if (yy != 0 || airf == 19)
                        {
                            airf = 0;
                            rb.isKinematic = true;
                            yy = 0;
                            transform.Translate(0, -0.45f, 0);
                            dawn = 19;
                            muteki = 1;
                            if (hp <= 0) Die();
                            anim.SetInteger("Dawn", 19);
                            if (hp > 0)
                            {
                                myGameManagerData.SetPower(side, power);
                                seq.SetDelay(0.6f);
                                seq.AppendCallback(() => hitcount = 0);
                                seq.AppendCallback(() => anim.SetInteger("Dawn", 0));
                                seq.AppendCallback(() => muteki = 0);
                                seq.AppendCallback(() => dawn = 20);
                            }
                        }
                        flagh = 0;
                        seq.SetDelay(0.2f);
                        if (yy <= 0) seq.AppendCallback(() => hitcount = 0);
                        anim.SetInteger("nondamaged", 1);
                        anim.SetInteger("damage", 0);
                    }
                }
            }
        }

        IEnumerator Wait(double f)
        {
            //yield on a new YieldInstruction that waits for 5 seconds.
            yield return new WaitForSeconds((float)f);
        }

        public void OnDamage(int damage, float xh, float yh, int pausetime, int hitslide, int hittime)
        {
            hitcount += 1;
            anim.SetInteger("Tati", 0);
            anim.SetInteger("Jamp", 0);
            power += damage;
            anim.SetInteger("nondamaged", 0);
            dawn = 0;
            waza = 0;
            guard = 0;
            anim.SetInteger("Guard", 0);
            hp -= damage;
            if (hp <= 0 && noko != 0) hp = 1;
            hpBar.ChangeColor();
            hpBar.UpdateHP(damage);
            if (hp <= 0)
            {
                KOdasu.GetComponent<KOdasu>().KOhyouzi();
                //Die();
            }
            hits = hitslide;
            //if (onaji != enemyObj.GetComponent<Common>().waza)
            //{
                hitt = hittime + 20;
            //}
            //else
            //{
                hitt = hittime;
            //}
            //naji = enemyObj.GetComponent<Common>().waza;
            pause = pausetime;
            xx += xh;
            yy += yh;
            //if (ycommon > -1.04f && yy <= 0) yy = 0.01f;
            if (yy > 0) anim.SetInteger("Syagami", 0);
            if (hp <= 0 && yy <= 0.05f) yy = 0.05f;
            if (yy != 0 || airf == 19)
            {
                anim.SetInteger("damage", 2);
                anim.SetTrigger("damagedair");
                airf = 19;
            }
            else
            {
                anim.SetInteger("damage", 1);
                anim.SetTrigger("damaged");
            }
            if (yy > 0.01f) syagami = 0;
            flagh = 19;
            gr = 0;
        }

        public void OnGuard(int damage, int hittime)
        {
            hp -= damage;
            hpBar.ChangeColor();
            hpBar.UpdateHP(damage);
            if (hp <= 0)
            {
                OnDamage(0, 0, 0, 0, 0, 0);
            }
            hitt = hittime;
            flagh = 19;
            gr = 0;
        }

        void Die()
        {
            hp = 0;
            anim.SetTrigger("Die");
            var seq = DOTween.Sequence();
            seq.SetDelay(1.0f);
            seq.AppendCallback(() => ToNext());
        }

        void ToNext()
        {
            hitcount = 0;
            Instantiate(transitionPrefab);
            myGameManagerData.SetPower(this.GetComponent<Common>().side, this.GetComponent<Common>().power);
            var seq = DOTween.Sequence();
            seq.SetDelay(0.2f);
            if (lose <= 0) seq.AppendCallback(() => SceneManager.LoadScene("MainScene"));
            if (lose >= 1) seq.AppendCallback(() => SceneManager.LoadScene("Charaselsect1"));
        }
    }
}
