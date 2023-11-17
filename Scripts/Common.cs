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
            rb = GetComponent<Rigidbody2D>(); //
            anim = GetComponent<Animator>();
            KOdasu = GameObject.Find("Canvas"); //画面描画を参照
            if (this.gameObject.tag == "Player") //キャラの立ち位置で処理分岐
            {
                face = 1;
                hpBar = GameObject.Find("Canvas/HPBarP").GetComponent<HPBar>(); //HPbarのオブジェクトを参照
                side = 1;
            }
            else
            {
                face = -1;
                hpBar = GameObject.Find("Canvas/HPBar").GetComponent<HPBar>(); 
                side = 2;
            }
            power = myGameManagerData.GetPower(side); //パワーゲージ参照
            win = myGameManagerData.GetWin(this.GetComponent<Common>().side); //win数表示を参照
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
                if (power > powermax) power = powermax; //powermaxを越えないように
                if (power != powerprev || power == 0) hpBar.UpdatePower(power); //パワーゲージに反映
                powerprev = power;
                Vector3 tmp = transform.position;
                if (tmp.x >= 2.765f) transform.Translate(2.765f - tmp.x, 0, 0); //位置修正
                if (tmp.x <= -2.547f) transform.Translate(-2.547f - tmp.x, 0, 0);
                if (tmp.y <= -1.04f && dawn < 19 && (syagami != 19 || flagh == 19)) transform.Translate(0, -1.04f - tmp.y, 0);
                tmp = transform.position; //今の位置を記録
                xcommon = tmp.x;
                ycommon = tmp.y;
                if (ycommon >= -1.04f && flagh != 19) //ジャンプ中
                {
                    airf = 0;
                    onaji = 0;
                    rb.isKinematic = false; //物理演算を切る
                }
                else
                {
                    rb.isKinematic = true;
                }
                if (flagh == 19) //喰らい中処理
                {
                    if (pause > 0) //被弾硬直
                    {
                        pause -= 1;
                    }
                    if (pause <= 0 && hits >= 1) //硬直してない移動時間
                    {
                        transform.Translate(xx, yy - (0.098f / 180) * gr, 0); //xy方向に動かす
                        hits -= 1;
                        gr += 1;
                    }
                    //tmp = transform.position;
                    if (pause <= 0 && hits <= 0 && ycommon >= -1) //空中被弾
                    {
                        xx = 0;
                        yy -= (0.098f / 120);
                        transform.Translate(0, yy, 0);//y方向だけ動かす
                        gr += 1;
                    }
                    if (pause <= 0 && hits <= 0 && hitt > 0) //移動時間過ぎた後の喰らい時間
                    {
                        hitt -= 1;
                    }
                    tmp = transform.position;
                    if (pause <= 0 && hits <= 0 && ycommon <= -1 && (hitt <= 0 || yy != 0f)) /ダウン処理
                    {
                        gr = 0;
                        rb.velocity = new Vector2(0, 0); //重力リセット
                        xx = 0;
                        hpBar.ReColor();
                        var seq = DOTween.Sequence(); //待機時間用
                        if (yy != 0 || airf == 19)
                        {
                            airf = 0;
                            rb.isKinematic = true; //物理演算オン
                            yy = 0; 
                            transform.Translate(0, -0.45f, 0); //位置ずらす
                            dawn = 19;
                            muteki = 1;
                            if (hp <= 0) Die(); //hp0なら死亡処理へ
                            anim.SetInteger("Dawn", 19); //ダウンアニメへ
                            if (hp > 0)
                            {
                                myGameManagerData.SetPower(side, power); //パワーゲージ修正
                                seq.SetDelay(0.6f); //0.6f待機時間発生
                                seq.AppendCallback(() => hitcount = 0); //待機後復帰処理で変数リセット＆ダウンアニメ解除
                                seq.AppendCallback(() => anim.SetInteger("Dawn", 0));
                                seq.AppendCallback(() => muteki = 0);
                                seq.AppendCallback(() => dawn = 20);
                            }
                        }
                        flagh = 0;
                        seq.SetDelay(0.2f); //0.2f待機
                        if (yy <= 0) seq.AppendCallback(() => hitcount = 0); //ヒットカウントリセット
                        anim.SetInteger("nondamaged", 1);
                        anim.SetInteger("damage", 0);
                    }
                }
            }
        }

        IEnumerator Wait(double f)
        {
            yield return new WaitForSeconds((float)f); //指定f待機
        }

        public void OnDamage(int damage, float xh, float yh, int pausetime, int hitslide, int hittime)
        {
            hitcount += 1; //ヒット数加算
            anim.SetInteger("Tati", 0); //攻撃アニメ解除
            anim.SetInteger("Jamp", 0);
            anim.SetInteger("waza", 0);
            power += damage; //ダメージ分パワー増加
            anim.SetInteger("nondamaged", 0); //被弾アニメ移行
            dawn = 0; //行動フラグ解除
            waza = 0;
            guard = 0;
            anim.SetInteger("Guard", 0); //ガード解除
            hp -= damage; //ダメージ処理
            if (hp <= 0 && noko != 0) hp = 1; //nokoフラグ立っいるときは1で耐える
            hpBar.ChangeColor(); //hpの色を変更
            hpBar.UpdateHP(damage); //hpバーにダメージ反映
            if (hp <= 0)
            {
                KOdasu.GetComponent<KOdasu>().KOhyouzi(); //hp0ならKO表示
                //Die();
            }
            hits = hitslide;
            //if (onaji != enemyObj.GetComponent<Common>().waza)
            //{
            //hitt = hittime + 20; 
            //}
            //else
            //{
            hitt = hittime; //喰らい時間
            //}
            //naji = enemyObj.GetComponent<Common>().waza;
            pause = pausetime; //硬直時間
            xx += xh; //喰らい中移動速度(x)
            yy += yh; //(y)
            //if (ycommon > -1.04f && yy <= 0) yy = 0.01f;
            if (hp <= 0 && yy <= 0.05f) yy = 0.05f; //死ぬとき上方向に
            if (ycommon >= -1 && yy <= 0.01f) yy = 0.01f; //飛んでるなら上方向に飛ばす
            if (yy != 0 || airf == 19) //空中被弾かどうか
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
            if (yy > 0) anim.SetInteger("Syagami", 0); //上に飛ばされる時しゃがみ解除
            if (yy > 0.01f) syagami = 0; //しゃがみ解除
            flagh = 19; //被弾フラグ
            gr = 0;
        }

        public void OnGuard(int damage, int hittime) //ガード処理
        {
            hp -= damage;
            hpBar.ChangeColor();
            hpBar.UpdateHP(damage);
            if (hp <= 0)
            {
                OnDamage(0, 0, 0, 0, 0, 0); //死んだときのみ被弾処理に
            }
            hitt = hittime;
            flagh = 19;
            gr = 0;
        }

        void Die() //死亡処理
        {
            hp = 0;
            anim.SetTrigger("Die"); //死亡アニメ
            var seq = DOTween.Sequence(); 
            seq.SetDelay(1.0f); //1.0f待機
            seq.AppendCallback(() => ToNext()); //次の試合へ移行
        }

        void ToNext() //次の試合へ
        {
            hitcount = 0;
            Instantiate(transitionPrefab); //試合移行演出
            myGameManagerData.SetPower(this.GetComponent<Common>().side, this.GetComponent<Common>().power); //引継ぎ処理セット
            var seq = DOTween.Sequence();
            seq.SetDelay(0.2f); //0.2f待機
            if (lose <= 0) seq.AppendCallback(() => SceneManager.LoadScene("MainScene")); //まだ試合が続くなら次の試合場面へ
            if (lose >= 1) seq.AppendCallback(() => SceneManager.LoadScene("Charaselsect1")); //決着着いたならキャラセレ画面へ
        }
    }
}
