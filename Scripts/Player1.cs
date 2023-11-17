using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Characelsect1
{
    public class Player1 : MonoBehaviour
    {
        // Start is called before the first frame update
        public float moveSpeed = 10, kyorix, kyoriy;
        public double hitpause, hitback;
        public int hp = 1000, backf, atf, state, gr, win, rstime, hitcount, uketsuke, nyuryoku, tyouhit, tyouhitime;
        public Transform attackPoint;
        public Transform attackPointJJ;
        public Transform attackPointCJ;
        public Transform attackPointUP;
        public Transform attackPointTK;
        public float attackRadius;
        public LayerMask enemyLayer;
        public HPBar hpBar;
        public GameObject backPanel;
        public GameObject hiteffect1;
        public GameObject hiteffect2;
        public GameObject hiteffect3;
        public GameObject oneffectS;
        public GameObject hiteffectTyou;
        public GameObject hiteffectTyou2;
        public GameObject hiteffectTyou3;
        public GameObject guardeffect1;
        public GameObject enemyObj;
        Rigidbody2D rb;
        Animator anim;

        public AudioClip soundKopan;
        public AudioClip soundKyou;
        public AudioClip soundKiri;
        public AudioClip soundKopanHit;
        public AudioClip soundKyouHit;
        public AudioClip soundKiriHit;
        public AudioClip soundGuard;
        public AudioClip soundonS;
        AudioSource audioSource;

        public MyGameManagerData myGameManagerData;

        void Start()
        {
            //プレイヤーキャラの設定
            myGameManagerData = FindObjectOfType<MyGameManager>().GetMyGameManagerData();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            if (this.gameObject.tag == "Player")
            {
                var seq = DOTween.Sequence();
                seq.SetDelay(0.1f);
                seq.AppendCallback(() => enemyLayer = 1 << LayerMask.NameToLayer("Enemy"));
                hpBar = GameObject.Find("Canvas/HPBarP").GetComponent<HPBar>();
                hpBar.SetName("GUNKO");
                seq.SetDelay(0.1f);
                seq.AppendCallback(() => enemyObj = GameObject.FindGameObjectWithTag("Enemy"));
            }
            else
            {
                var seq = DOTween.Sequence();
                seq.SetDelay(0.1f);
                seq.AppendCallback(() => enemyLayer = 1 << LayerMask.NameToLayer("Player"));
                hpBar = GameObject.Find("Canvas/HPBar").GetComponent<HPBar>();
                hpBar.SetName("GUNKO");
                seq.SetDelay(0.1f);
                seq.AppendCallback(() => enemyObj = GameObject.FindGameObjectWithTag("Player"));
            }
            this.GetComponent<Common>().enemyObj = enemyObj;
        }

        // Update is called once per frame

        void Update()
        {
            //プレイヤーキャラの操作部分
            if (this.GetComponent<Common>().tokitome >= 1)
            {
                OnAttackHit(0.1f); 
                rb.velocity = new Vector2(0, 0);
            }
            else
            {

                if (this.GetComponent<Common>().flagh == 19)
                {
                    anim.SetInteger("damage", 2);
                }
                else
                {
                    //anim.SetInteger("damage", 0);
                }
                if (tyouhitime > 0)
                {
                    tyouhitime += 1;
                    if (tyouhitime == 120 || tyouhitime == 180 || tyouhitime == 240) Instantiate(hiteffectTyou2, attackPoint.position, transform.rotation);
                    if (tyouhitime == 90 || tyouhitime == 150 || tyouhitime == 210) Instantiate(hiteffectTyou, attackPoint.position, transform.rotation);
                    if (tyouhitime == 300)
                    {
                        enemyObj.GetComponent<Common>().noko = 0;
                        Instantiate(hiteffectTyou3, attackPoint.position, transform.rotation);
                    }
                    if (tyouhitime >= 320)
                    {
                        anim.SetInteger("Tyouhi", 2);
                        if(tyouhitime < 330)
                        { 
                            rb.isKinematic = true;
                            transform.Translate(0, -0.01f, 0);
                        }
                        if (tyouhitime >= 380)
                        {
                            anim.SetInteger("Tyouhi", 0);
                            anim.SetInteger("Tati", 0);
                            atf = 0;
                            transform.Translate(0, 0.1f, 0);
                            tyouhitime = 0;
                            rb.isKinematic = false;
                        }
                    }

                }
                if (hitcount != enemyObj.GetComponent<Common>().hitcount && hitcount >= 2)
                {
                    hpBar.SetHits(enemyObj.GetComponent<Common>().hitcount);
                }
                hitcount = enemyObj.GetComponent<Common>().hitcount;
                if (hitcount > 5) hitcount = 5;
                if (state == 0 && this.GetComponent<Common>().flagh != 19)
                {
                    rb.isKinematic = false;
                }
                else
                {
                    rb.isKinematic = true;
                }
                if (this.GetComponent<Common>().ycommon >= -1.04f && this.GetComponent<Common>().syagami == 19 && this.GetComponent<Common>().flagh != 19)
                {
                    rb.isKinematic = true;
                    transform.Translate(0, -0.15f, 0);
                }
                if (this.GetComponent<Common>().ycommon >= -1.04f && this.GetComponent<Common>().waza == 3 && this.GetComponent<Common>().flagh != 19 && this.GetComponent<Common>().tokitome <= 0)
                {
                    rb.isKinematic = true;
                    transform.Translate(0, -0.15f, 0);
                }
                if (atf == 0 && this.GetComponent<Common>().flagh != 19)
                {
                    if (this.GetComponent<Common>().face == 1 && this.GetComponent<Common>().xcommon > enemyObj.GetComponent<Common>().xcommon)
                    {
                        this.GetComponent<Common>().face = -1;
                        transform.localScale = new Vector3(-1, 1, 1);
                    }
                    if (this.GetComponent<Common>().face == -1 && this.GetComponent<Common>().xcommon < enemyObj.GetComponent<Common>().xcommon)
                    {
                        this.GetComponent<Common>().face = 1;
                        transform.localScale = new Vector3(1, 1, 1);
                    }
                }
                //Vector3 tmp = transform.position;
                //if (tmp.x >= 2.765f) transform.Translate(2.765f - tmp.x, 0, 0);
                //if (tmp.x <= -2.547f) transform.Translate(-2.547f - tmp.x, 0, 0);
                kyoriy = Mathf.Abs(this.GetComponent<Common>().ycommon - enemyObj.GetComponent<Common>().ycommon);
                kyorix = Mathf.Abs(this.GetComponent<Common>().xcommon - enemyObj.GetComponent<Common>().xcommon);
                if (backf > 0 && hitback != 0)
                {
                    backf -= 1;
                    if (this.GetComponent<Common>().waza != 3 && state != 1)
                    {
                        rb.velocity = new Vector2((float)-hitback, rb.velocity.y);
                        if (backf <= 0) rb.velocity = new Vector2(0, rb.velocity.y);
                    }
                    else
                    {
                        transform.Translate(0.01f * this.GetComponent<Common>().face * (float)(-hitback), 0, 0);
                    }
                }
                if (atf != 0)
                {
                    atf -= 1;
                    if (this.GetComponent<Common>().flagh == 19) atf = 0;
                    if (atf <= 0)
                    {
                        //if (this.GetComponent<Common>().waza == 1) transform.Translate(-0.05f* this.GetComponent<Common>().face, 0, 0);
                        this.GetComponent<Common>().waza = 0;
                        if (state == 0) anim.SetInteger("Tati", 0);
                        if (state == 1) anim.SetInteger("Jamp", 19);
                        if (state == 2) anim.SetInteger("Syagami", 19);
                    }
                }
                if (state == 1 && this.GetComponent<Common>().flagh == 0)
                {
                    float x = Input.GetAxisRaw("Horizontal");//方向キー横
                    rb.isKinematic = true;
                    //rb.velocity = new Vector2(rb.velocity.x, 0);
                    //rb.velocity = new Vector2(x*0.5f, (float)(5 -(9.8 / 120) * gr));
                    transform.Translate(x * 0.01f, (float)(0.034 - (0.098 / 120) * gr), 0);
                    gr += 1;
                    if (this.GetComponent<Common>().ycommon <= -1.04f && gr >= 30)
                    {
                        rb.isKinematic = false;
                        rb.velocity = new Vector2(0, 0);
                        state = 0;
                        gr = 0;
                        atf = 3;
                        anim.SetInteger("Jamp", 0);
                        anim.SetInteger("Guard", 0);
                        this.GetComponent<Common>().guard = 0;
                    }
                }
                anim.SetFloat("Speed", 0);
                if (enemyObj.GetComponent<Common>().hp <= 0 && win == 0)
                {
                    myGameManagerData.SetPower(this.GetComponent<Common>().side, this.GetComponent<Common>().power);
                    myGameManagerData.PlusWin(this.GetComponent<Common>().side);
                    win = myGameManagerData.GetWin(this.GetComponent<Common>().side);
                    hpBar.SetWin(win);
                    if (win >= 2)
                    {
                        myGameManagerData.Reset();
                        enemyObj.GetComponent<Common>().lose = 19;
                    }
                }
                if(uketsuke >= 1)
                {
                    uketsuke -= 1;
                    if (uketsuke == 0) nyuryoku = 0;
                }
                if (rstime < 360) rstime += 1;
                if (atf <= 0 && this.GetComponent<Common>().flagh == 0 && enemyObj.GetComponent<Common>().hp > 0 && rstime >= 360)
                {
                    anim.SetInteger("Guard", 0);
                    this.GetComponent<Common>().guard = 0;
                    //キーボード操作
                    if (Input.GetKeyDown(KeyCode.UpArrow) && state != 1) 
                    {
                        anim.SetInteger("Syagami", 0);
                        anim.SetInteger("Jamp", 19);
                        rb.isKinematic = true;
                        state = 1;
                        gr = 0;
                        this.GetComponent<Common>().syagami = 0;
                        //atf = 999;}
                    }
                    if (uketsuke == 0 && (Input.GetKeyDown(KeyCode.V) || Input.GetKeyDown(KeyCode.C)))
                    {
                        if (Input.GetKeyDown(KeyCode.C)) nyuryoku = 1;
                        if (Input.GetKeyDown(KeyCode.V)) nyuryoku = 2;
                        uketsuke = 6;
                    }
                        if ((Input.GetKeyDown(KeyCode.C) && nyuryoku == 2 || Input.GetKeyDown(KeyCode.V) && nyuryoku == 1) && state == 0)
                    {
                        this.GetComponent<Common>().waza = 3;
                        this.GetComponent<Common>().power -= 1000;
                        rb.velocity = new Vector2(0, rb.velocity.y);
                        var seq = DOTween.Sequence();
                        
                        anim.SetInteger("Tati", 29);
                        Instantiate(oneffectS, transform.position, transform.rotation);
                        Instantiate(backPanel);
                        audioSource.PlayOneShot(soundonS);
                        atf = 50;
                        backf = 24;
                        hitback = -5;
                        this.GetComponent<Common>().tokitome = 60;
                        enemyObj.GetComponent<Common>().tokitome = 60;
                        seq.SetDelay(0.4f);
                        seq.AppendCallback(() => audioSource.PlayOneShot(soundKyou));
                    }
                    if (Input.GetKeyDown(KeyCode.Z) && atf <= 0)

                    {
                        //transform.Translate(0.05f* this.GetComponent<Common>().face, 0, 0);
                        this.GetComponent<Common>().waza = 1;
                        this.GetComponent<Common>().power += 10;
                        if (state == 1)
                        {
                            anim.SetInteger("Jamp", 11);
                            atf = 999;
                            audioSource.PlayOneShot(soundKopan);
                            var seq = DOTween.Sequence();
                        }
                        if (state == 0)
                        {
                            rb.velocity = new Vector2(0, rb.velocity.y);
                            atf = 16;
                            anim.SetTrigger("kopan");
                            audioSource.PlayOneShot(soundKopan);
                        }
                        if (state == 2)
                        {
                            atf = 16;
                            anim.SetInteger("Syagami", 11);
                            var seq = DOTween.Sequence();
                            seq.SetDelay(0.05f);
                            seq.AppendCallback(() => audioSource.PlayOneShot(soundKopan));
                        }
                    }

                    if (Input.GetKeyDown(KeyCode.X) && atf <= 0)
                    {
                        //transform.Translate(0.05f* this.GetComponent<Common>().face, 0, 0);
                        this.GetComponent<Common>().waza = 1;
                        this.GetComponent<Common>().power += 20;
                        if (state == 1)
                        {
                            anim.SetInteger("Jamp", 12);
                            atf = 99;
                            audioSource.PlayOneShot(soundKyou);
                            // var seq = DOTween.Sequence();
                            //seq.SetDelay(0.1f);
                            //seq.AppendCallback(() => AttackJJaku());
                        }
                        if (state == 0)
                        {
                            rb.velocity = new Vector2(0, rb.velocity.y);
                            atf = 30;
                            anim.SetInteger("Tati", 12);
                            var seq = DOTween.Sequence();
                            seq.SetDelay(0.1f);
                            seq.AppendCallback(() => audioSource.PlayOneShot(soundKyou));
                        }
                        if (state == 2)
                        {
                            atf = 28;
                            anim.SetInteger("Syagami", 12);
                            var seq = DOTween.Sequence();
                            seq.SetDelay(0.1f);
                            seq.AppendCallback(() => audioSource.PlayOneShot(soundKyou));
                        }
                    }
                    if (nyuryoku == 1 && uketsuke == 1 && atf <= 0)
                    {
                        //transform.Translate(0.05f* this.GetComponent<Common>().face, 0, 0);
                        this.GetComponent<Common>().waza = 2;
                        this.GetComponent<Common>().power += 30;
                        if (state == 1)
                        {
                            anim.SetInteger("Jamp", 13);
                            atf = 99;
                            audioSource.PlayOneShot(soundKiri);
                            // var seq = DOTween.Sequence();
                            //seq.SetDelay(0.1f);
                            //seq.AppendCallback(() => AttackJJaku());
                        }
                        if (state == 0)
                        {
                            rb.velocity = new Vector2(0, rb.velocity.y);
                            atf = 60;
                            anim.SetInteger("Tati", 13);
                            var seq = DOTween.Sequence();
                            seq.SetDelay(0.1f);
                            seq.AppendCallback(() => audioSource.PlayOneShot(soundKiri));
                        }
                        if (state == 2)
                        {
                            atf = 60;
                            anim.SetInteger("Syagami", 13);
                            var seq = DOTween.Sequence();
                            seq.SetDelay(0.1f);
                            seq.AppendCallback(() => audioSource.PlayOneShot(soundKiri));
                        }
                    }
                    if (nyuryoku == 2 && uketsuke == 1 && atf <= 0 && state == 0)
                    {
                        float x = Input.GetAxisRaw("Horizontal");//方向キー横
                        this.GetComponent<Common>().waza = 3;
                        this.GetComponent<Common>().power += 30;
                        rb.velocity = new Vector2(0, rb.velocity.y);
                        var seq = DOTween.Sequence();
                        if (x > 0 && this.GetComponent<Common>().face == 1 || x < 0 && this.GetComponent<Common>().face == -1)
                        {
                            anim.SetInteger("Tati", 15);
                            atf = 60;
                            backf = 12;
                            hitback = -5;
                            seq.SetDelay(0.1f);
                            seq.AppendCallback(() => audioSource.PlayOneShot(soundKyou));
                        }
                        else
                        {
                            anim.SetInteger("Tati", 14);
                            atf = 77;
                            seq.SetDelay(0.1f);
                            seq.AppendCallback(() => audioSource.PlayOneShot(soundKiri));

                        }
                    }
                    if (Input.GetKeyDown(KeyCode.B) && atf <= 0)
                    {
                        anim.SetInteger("Tati", 16);
                        atf = 60;
                        var seq = DOTween.Sequence();
                        seq.SetDelay(0.1f);
                        seq.AppendCallback(() => audioSource.PlayOneShot(soundKyou));
                    }
                        if (state != 1 && atf <= 0) Movement();
                }
            }
        }

        public void OnAttackHit(double HitStopTime)
        {
            // モーションを止める
            anim.speed = 0f;


            var seq = DOTween.Sequence();
            hitpause = HitStopTime;
            seq.SetDelay((float)HitStopTime);
            // モーションを再開
            seq.AppendCallback(() => anim.speed = 1f);
        }

        public void Hitback(double Xback, double time)
        {
            rb.velocity = new Vector2((float)-Xback, rb.velocity.y);

        }

        public void TyouhiHit()
        {
            //技
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer); //ヒット判定

            if (enemyObj.GetComponent<Common>().muteki == 0)
            {
                foreach (Collider2D hitEnemy in hitEnemys)
                {
                    Debug.Log(hitEnemy.gameObject.name);
                    if (enemyObj.GetComponent<Common>().guard >= 1 && this.GetComponent<Common>().face != enemyObj.GetComponent<Common>().face)
                    {
                        hitEnemy.GetComponent<Common>().OnGuard(0, 6);
                        Instantiate(guardeffect1, attackPoint.position, transform.rotation);
                        audioSource.PlayOneShot(soundGuard);
                        backf = 4;
                        hitback = 3;
                    }
                    else
                    {
                        enemyObj.GetComponent<Common>().noko = 19;
                        hitEnemy.GetComponent<Common>().OnDamage(0, 0.009f * this.GetComponent<Common>().face, 0.03f, 12, 18, 14);
                        Instantiate(hiteffect1, attackPoint.position, transform.rotation);
                        audioSource.PlayOneShot(soundKyouHit);
                        OnAttackHit(0.1);
                        atf += 999;
                        anim.SetInteger("Tyouhi", 1);
                        tyouhitime = 1;
                    }
                }
            }
        }

        public void Attack1()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

            if (enemyObj.GetComponent<Common>().muteki == 0)
            {
                foreach (Collider2D hitEnemy in hitEnemys)
                {
                    Debug.Log(hitEnemy.gameObject.name);
                    if (enemyObj.GetComponent<Common>().guard >= 1 && this.GetComponent<Common>().face != enemyObj.GetComponent<Common>().face)
                    {
                        hitEnemy.GetComponent<Common>().OnGuard(0, 6);
                        Instantiate(guardeffect1, attackPoint.position, transform.rotation);
                        audioSource.PlayOneShot(soundGuard);
                        backf = 4;
                        hitback = 3;
                    }
                    else
                    {
                        hitEnemy.GetComponent<Common>().OnDamage((int)(10 * (10 - hitcount) / 10), 0, 0, 4, 8, 14);
                        Instantiate(hiteffect2, attackPoint.position, transform.rotation);
                        audioSource.PlayOneShot(soundKopanHit);
                        OnAttackHit(0.05);
                        backf = 4;
                        hitback = 3;
                        atf += 3;
                        this.GetComponent<Common>().power += 10;
                    }
                }
            }
        }

        public void AttackCJyaku()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPointCJ.position, attackRadius, enemyLayer);

            if (enemyObj.GetComponent<Common>().muteki == 0)
            {
                foreach (Collider2D hitEnemy in hitEnemys)
                {
                    Debug.Log(hitEnemy.gameObject.name);
                    if (enemyObj.GetComponent<Common>().guard >= 1 && this.GetComponent<Common>().face != enemyObj.GetComponent<Common>().face)
                    {
                        hitEnemy.GetComponent<Common>().OnGuard(0, 6);
                        Instantiate(guardeffect1, attackPointCJ.position, transform.rotation);
                        audioSource.PlayOneShot(soundGuard);
                        backf = 4;
                        hitback = 3;
                    }
                    else
                    {
                        hitEnemy.GetComponent<Common>().OnDamage((int)(10 * (10 - hitcount) / 10), 0.009f * this.GetComponent<Common>().face, 0, 4, 8, 14);
                        Instantiate(hiteffect2, attackPointCJ.position, transform.rotation);
                        audioSource.PlayOneShot(soundKopanHit);
                        OnAttackHit(0.05);
                        backf = 4;
                        hitback = 3;
                        atf += 3;
                        this.GetComponent<Common>().power += 10;
                    }
                }
            }
        }

        public void AttackCKYou()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPointCJ.position, attackRadius, enemyLayer);

            if (enemyObj.GetComponent<Common>().muteki == 0)
            {
                foreach (Collider2D hitEnemy in hitEnemys)
                {
                    Debug.Log(hitEnemy.gameObject.name);
                    if (enemyObj.GetComponent<Common>().guard >= 1 && this.GetComponent<Common>().face != enemyObj.GetComponent<Common>().face)
                    {
                        hitEnemy.GetComponent<Common>().OnGuard(0, 9);
                        Instantiate(guardeffect1, attackPointCJ.position, transform.rotation);
                        audioSource.PlayOneShot(soundGuard);
                        backf = 4;
                        hitback = 3;
                    }
                    else
                    {
                        hitEnemy.GetComponent<Common>().OnDamage((int)(35 * (10 - hitcount) / 10), 0.018f * this.GetComponent<Common>().face, 0, 6, 8, 24);
                        Instantiate(hiteffect1, attackPointCJ.position, transform.rotation);
                        audioSource.PlayOneShot(soundKyouHit);
                        OnAttackHit(0.1);
                        backf = 6;
                        hitback = 6;
                        atf += 6;
                        this.GetComponent<Common>().power += 20;
                    }
                }
            }
        }

        public void AttackUKiri()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPointUP.position, attackRadius, enemyLayer);

            if (enemyObj.GetComponent<Common>().muteki == 0)
            {
                foreach (Collider2D hitEnemy in hitEnemys)
                {
                    Debug.Log(hitEnemy.gameObject.name);
                    if (enemyObj.GetComponent<Common>().guard >= 1 && this.GetComponent<Common>().face != enemyObj.GetComponent<Common>().face)
                    {
                        hitEnemy.GetComponent<Common>().OnGuard(0, 9);
                        Instantiate(guardeffect1, attackPointUP.position, transform.rotation);
                        audioSource.PlayOneShot(soundGuard);
                        backf = 4;
                        hitback = 3;
                    }
                    else
                    {
                        hitEnemy.GetComponent<Common>().OnDamage((int)(70 * (10 - hitcount) / 10), 0.02f * this.GetComponent<Common>().face, 0.03f, 6, 18, 32);
                        Instantiate(hiteffect3, attackPointUP.position, transform.rotation);
                        audioSource.PlayOneShot(soundKiriHit);
                        OnAttackHit(0.1);
                        backf = 8;
                        hitback = 6;
                        atf += 6;
                        this.GetComponent<Common>().power += 35;
                    }
                }
            }
        }

        public void AttackSKiri()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

            if (enemyObj.GetComponent<Common>().muteki == 0)
            {
                foreach (Collider2D hitEnemy in hitEnemys)
                {
                    Debug.Log(hitEnemy.gameObject.name);
                    if (enemyObj.GetComponent<Common>().guard >= 1 && this.GetComponent<Common>().face != enemyObj.GetComponent<Common>().face)
                    {
                        hitEnemy.GetComponent<Common>().OnGuard(0, 9);
                        Instantiate(guardeffect1, attackPoint.position, transform.rotation);
                        audioSource.PlayOneShot(soundGuard);
                        backf = 4;
                        hitback = 3;
                    }
                    else
                    {
                        hitEnemy.GetComponent<Common>().OnDamage((int)(60 * (10 - hitcount) / 10), 0.02f * this.GetComponent<Common>().face, 0, 6, 20, 32);
                        Instantiate(hiteffect3, attackPoint.position, transform.rotation);
                        audioSource.PlayOneShot(soundKiriHit);
                        OnAttackHit(0.1);
                        backf = 8;
                        hitback = 6;
                        atf += 6;
                        this.GetComponent<Common>().power += 30;
                    }
                }
            }
        }

        public void AttackSTsuki()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPointTK.position, attackRadius, enemyLayer);

            if (enemyObj.GetComponent<Common>().muteki == 0)
            {
                foreach (Collider2D hitEnemy in hitEnemys)
                {
                    Debug.Log(hitEnemy.gameObject.name);
                    if (enemyObj.GetComponent<Common>().guard >= 1 && this.GetComponent<Common>().face != enemyObj.GetComponent<Common>().face)
                    {
                        hitEnemy.GetComponent<Common>().OnGuard(0, 9);
                        Instantiate(guardeffect1, attackPointTK.position, transform.rotation);
                        audioSource.PlayOneShot(soundGuard);
                        backf = 4;
                        hitback = 3;
                    }
                    else
                    {
                        hitEnemy.GetComponent<Common>().OnDamage((int)(60 * (10 - hitcount) / 10), 0.03f * this.GetComponent<Common>().face, 0, 12, 12, 24);
                        Instantiate(hiteffect1, attackPointTK.position, transform.rotation);
                        audioSource.PlayOneShot(soundKyouHit);
                        OnAttackHit(0.05);
                        backf = 6;
                        hitback = 9;
                        atf += 6;
                        this.GetComponent<Common>().power += 30;
                    }
                }
            }
        }

        public void AttackSKYou()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

            if (enemyObj.GetComponent<Common>().muteki == 0)
            {
                foreach (Collider2D hitEnemy in hitEnemys)
                {
                    Debug.Log(hitEnemy.gameObject.name);
                    if (enemyObj.GetComponent<Common>().guard >= 1 && this.GetComponent<Common>().face != enemyObj.GetComponent<Common>().face)
                    {
                        hitEnemy.GetComponent<Common>().OnGuard(0, 9);
                        Instantiate(guardeffect1, attackPoint.position, transform.rotation);
                        audioSource.PlayOneShot(soundGuard);
                        backf = 4;
                        hitback = 3;
                    }
                    else
                    {
                        hitEnemy.GetComponent<Common>().OnDamage((int)(40 * (10 - hitcount) / 10), 0.018f * this.GetComponent<Common>().face, 0, 6, 8, 28);
                        Instantiate(hiteffect1, attackPoint.position, transform.rotation);
                        audioSource.PlayOneShot(soundKyouHit);
                        OnAttackHit(0.05);
                        backf = 6;
                        hitback = 6;
                        atf += 6;
                        this.GetComponent<Common>().power += 20;
                    }
                }
            }
        }

        public void AttackCK()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

            if (enemyObj.GetComponent<Common>().muteki == 0)
            {
                foreach (Collider2D hitEnemy in hitEnemys)
                {
                    if (enemyObj.GetComponent<Common>().guard == 3 && this.GetComponent<Common>().face != enemyObj.GetComponent<Common>().face)
                    {
                        hitEnemy.GetComponent<Common>().OnGuard(0, 12);
                        Instantiate(guardeffect1, attackPoint.position, transform.rotation);
                        audioSource.PlayOneShot(soundGuard);
                        backf = 4;
                        hitback = 3;
                    }
                    else
                    {
                        hitEnemy.GetComponent<Common>().OnDamage((int)(50 * (10 - hitcount) / 10), 0.01f * this.GetComponent<Common>().face, 0.005f, 6, 12, 24);
                        Instantiate(hiteffect3, attackPoint.position, transform.rotation);
                        audioSource.PlayOneShot(soundKiriHit);
                        OnAttackHit(0.05);
                        backf = 4;
                        hitback = 3;
                        atf += 6;
                        this.GetComponent<Common>().power += 40;
                    }
                }
            }
        }

        public void AttackJJaku()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPointJJ.position, attackRadius, enemyLayer);

            if (enemyObj.GetComponent<Common>().muteki == 0)
            {
                foreach (Collider2D hitEnemy in hitEnemys)
                {
                    if (enemyObj.GetComponent<Common>().guard >= 1 && enemyObj.GetComponent<Common>().guard != 3 && this.GetComponent<Common>().face != enemyObj.GetComponent<Common>().face)
                    {
                        hitEnemy.GetComponent<Common>().OnGuard(0, 9);
                        Instantiate(guardeffect1, attackPointJJ.position, transform.rotation);
                        audioSource.PlayOneShot(soundGuard);
                        backf = 4;
                        hitback = 2;
                    }
                    else
                    {
                        hitEnemy.GetComponent<Common>().OnDamage((int)(25 * (10 - hitcount) / 10), 0.02f * this.GetComponent<Common>().face, 0, 6, 16, 32);
                        Instantiate(hiteffect2, attackPointJJ.position, transform.rotation);
                        audioSource.PlayOneShot(soundKopanHit);
                        OnAttackHit(0.1);
                        backf = 9;
                        hitback = 2;
                        transform.Translate(-0.05f, 0, 0);
                        atf += 12;
                        this.GetComponent<Common>().power += 20;
                    }
                }
            }
        }

        public void AttackJKyou()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPointJJ.position, attackRadius, enemyLayer);

            if (enemyObj.GetComponent<Common>().muteki == 0)
            {
                foreach (Collider2D hitEnemy in hitEnemys)
                {
                    if (enemyObj.GetComponent<Common>().guard >= 1 && enemyObj.GetComponent<Common>().guard != 3 && this.GetComponent<Common>().face != enemyObj.GetComponent<Common>().face)
                    {
                        hitEnemy.GetComponent<Common>().OnGuard(0, 16);
                        Instantiate(guardeffect1, attackPointJJ.position, transform.rotation);
                        audioSource.PlayOneShot(soundGuard);
                        backf = 6;
                        hitback = 4;
                    }
                    else
                    {
                        hitEnemy.GetComponent<Common>().OnDamage((int)(40 * (10 - hitcount) / 10), 0.05f * this.GetComponent<Common>().face, 0.01f, 6, 16, 32);
                        Instantiate(hiteffect1, attackPointJJ.position, transform.rotation);
                        audioSource.PlayOneShot(soundKyouHit);
                        OnAttackHit(0.1);
                        backf = 6;
                        hitback = 4;
                        transform.Translate(-0.05f, 0, 0);
                        atf += 12;
                        this.GetComponent<Common>().power += 20;
                    }
                }
            }
        }

        public void AttackJKiri()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPointJJ.position, attackRadius, enemyLayer);

            if (enemyObj.GetComponent<Common>().muteki == 0)
            {
                foreach (Collider2D hitEnemy in hitEnemys)
                {
                    Debug.Log(hitEnemy.gameObject.name);
                    if (enemyObj.GetComponent<Common>().guard >= 1 && this.GetComponent<Common>().face != enemyObj.GetComponent<Common>().face)
                    {
                        hitEnemy.GetComponent<Common>().OnGuard(0, 9);
                        Instantiate(guardeffect1, attackPointJJ.position, transform.rotation);
                        audioSource.PlayOneShot(soundGuard);
                        backf = 4;
                        hitback = 3;
                    }
                    else
                    {
                        hitEnemy.GetComponent<Common>().OnDamage((int)(60 * (10 - hitcount) / 10), 0.02f * this.GetComponent<Common>().face, 0, 6, 20, 32);
                        Instantiate(hiteffect3, attackPointJJ.position, transform.rotation);
                        audioSource.PlayOneShot(soundKiriHit);
                        OnAttackHit(0.1);
                        backf = 8;
                        hitback = 6;
                        atf += 6;
                        this.GetComponent<Common>().power += 30;
                    }
                }
            }
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }

        void Movement()
        {
            float x = Input.GetAxisRaw("Horizontal");//方向キー横
            float y = Input.GetAxisRaw("Vertical");
            if (x > 0)
            {
                if (state == 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    transform.GetComponent<Common>().face = 1;
                }
                if (enemyObj.GetComponent<Common>().face == 1)
                {
                    this.GetComponent<Common>().guard = 19;
                    if (enemyObj.GetComponent<Common>().waza != 0)
                    {
                        transform.localScale = new Vector3(-1, 1, 1);
                        transform.GetComponent<Common>().face = -1;
                        x = 0;
                        if (y >= 0 && state == 2) state = 0;
                        anim.SetInteger("Guard", state + 1);
                        this.GetComponent<Common>().guard = state + 1;
                    }
                }
            }
            if (x < 0)
            {
                if (state == 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    transform.GetComponent<Common>().face = -1;
                }
                if (enemyObj.GetComponent<Common>().face == -1)
                {
                    this.GetComponent<Common>().guard = 19;
                    if (enemyObj.GetComponent<Common>().waza != 0)
                    {
                        transform.localScale = new Vector3(1, 1, 1);
                        transform.GetComponent<Common>().face = 1;
                        x = 0;
                        if (y >= 0 && state == 2) state = 0;
                        anim.SetInteger("Guard", state + 1);
                        this.GetComponent<Common>().guard = state + 1;
                    }
                }
            }
            if (y < 0 && x == 0)
            {
                if (this.GetComponent<Common>().syagami == 0) anim.SetInteger("Syagami", 19);
                state = 2;
                this.GetComponent<Common>().syagami = 19;
                if (this.GetComponent<Common>().guard > 0) anim.SetInteger("Guard", 3);
            }
            else
            {
                anim.SetInteger("Syagami", 0);
                state = 0;
                this.GetComponent<Common>().syagami = 0;
            }
            if (state != 0) x = 0;
            anim.SetFloat("Speed", Mathf.Abs(x * 10));
            rb.velocity = new Vector2(x * (moveSpeed), rb.velocity.y);
        }
    }
}
