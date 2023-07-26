using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Characelsect1
{
    public class Player2 : MonoBehaviour
    {
        [SerializeField]
        GameObject transitionPrefab;
        // Start is called before the first frame update
        public float moveSpeed = 7;
        public double hitpause, hitback, hitcount;
        public int hp = 1000, backf, atf, waza, state, gr, dash, dflag;
        public Transform attackPoint;
        public Transform attackPointJJ;
        public float attackRadius;
        public LayerMask enemyLayer;
        public HPBar hpBar;
        public GameObject hiteffect1;
        public GameObject hiteffect2;
        public GameObject guardeffect1;
        public GameObject enemyObj;
        Rigidbody2D rb;
        Animator anim;

        public AudioClip soundKopan;
        public AudioClip soundKopanHit;
        public AudioClip soundGuard;
        AudioSource audioSource;

        public MyGameManagerData myGameManagerData;

        void Start()
        {
            myGameManagerData = FindObjectOfType<MyGameManager>().GetMyGameManagerData();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            if (this.gameObject.tag == "Player")
            {
                var seq = DOTween.Sequence();
                seq.SetDelay(0.1f);
                seq.AppendCallback(() => enemyLayer = 1 << LayerMask.NameToLayer("Enemy"));
                enemyObj = GameObject.FindGameObjectWithTag("Enemy");
                this.GetComponent<Common>().face = 1;
            }
            else
            {
                var seq = DOTween.Sequence();
                seq.SetDelay(0.1f);
                seq.AppendCallback(() => enemyLayer = 1 << LayerMask.NameToLayer("Player"));
                enemyObj = GameObject.FindGameObjectWithTag("Player");
                this.GetComponent<Common>().face = -1;
            }
            this.GetComponent<Common>().enemyObj = enemyObj;
        }

        // Update is called once per frame

        void Update()
        {
            //if (dash == 0 && atf == 0 && this.GetComponent<Common>().flagh != 19 && this.GetComponent<Common>().face == 1 && this.GetComponent<Common>().xcommon > enemyObj.GetComponent<Common>().xcommon)
            //{
            //    this.GetComponent<Common>().face = -1;
            //     transform.localScale = new Vector3(-1, 1, 1);
            //}
            //if (dash == 0 && atf == 0 && this.GetComponent<Common>().flagh != 19 && this.GetComponent<Common>().face == -1 && this.GetComponent<Common>().xcommon < enemyObj.GetComponent<Common>().xcommon)
            // {
            //     this.GetComponent<Common>().face = 1;
            //      transform.localScale = new Vector3(1, 1, 1);
            // }
            //  dash = 0;

            if (this.GetComponent<Common>().tokitome >= 1)
            {
                OnAttackHit(0.1f);
                rb.velocity = new Vector2(0, 0);
            }
            else
            {

                rb.velocity = new Vector2(0, 0);
                if (this.GetComponent<Common>().dawn >= 19)
                {
                    rb.isKinematic = true;
                    //seq.SetDelay(0.1f);
                    //if (this.GetComponent<Common>().dawn == 19) seq.AppendCallback(() => transform.Translate(0, -0.4f, 0));
                    //dflag += 1;
                    if(this.GetComponent<Common>().dawn == 20)
                    {
                        //transform.Translate(0, -0.4f, 0);
                        this.GetComponent<Common>().dawn = 21;
                        var seq = DOTween.Sequence();
                        seq.SetDelay(0.8f);
                        seq.AppendCallback(() => this.GetComponent<Common>().dawn = 0);
                    }
                }

                if (this.GetComponent<Common>().flagh == 19 && this.GetComponent<Common>().guard == 0 || Input.GetKeyDown(KeyCode.R))
                {
                    anim.SetInteger("Guard", 0);
                    anim.SetInteger("waza", 0);
                    state = 0;
                    this.GetComponent<Common>().guard = 0;
                    this.GetComponent<Common>().waza = 0;
                }

                if (state == 1 && this.GetComponent<Common>().flagh == 0 && this.GetComponent<Common>().rstime >= 360)
                {
                    //float x = Input.GetAxisRaw("Horizontal");//方向キー横
                    //rb.velocity = new Vector2(x*0.5f, (float)(5 -(9.8 / 120) * gr));
                    transform.Translate(0, (float)(0.03 - (0.098 / 240) * gr), 0);
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

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Instantiate(transitionPrefab);

                    myGameManagerData.Reset();
                    var seq = DOTween.Sequence();
                    seq.SetDelay(0.2f);
                    seq.AppendCallback(() => SceneManager.LoadScene("Charaselsect1"));
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
                if (atf != 0)
                {
                    atf -= 1;
                    if (this.GetComponent<Common>().flagh == 19) atf = 0;
                    if (atf <= 0)
                    {
                        //if (this.GetComponent<Common>().waza == 1) transform.Translate(-0.05f* this.GetComponent<Common>().face, 0, 0);
                        this.GetComponent<Common>().waza = 0;
                        anim.SetInteger("waza", 0);
                        if (state == 0) anim.SetInteger("Tati", 0);
                        if (state == 1) anim.SetInteger("Jamp", 19);
                        if (state == 2) anim.SetInteger("Syagami", 19);
                    }
                }
                if (this.GetComponent<Common>().flagh != 19 && state != 1 && waza == 0)
                {
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        anim.SetInteger("Guard", 1);
                        state = 0;
                        this.GetComponent<Common>().guard = 1;
                    }
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        anim.SetInteger("Guard", 2);
                        state = 1;
                        gr = 0;
                        rb.isKinematic = true;
                        this.GetComponent<Common>().guard = 2;
                    }
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        anim.SetInteger("Guard", 3);
                        state = 2;
                        this.GetComponent<Common>().guard = 3;
                    }
                    if (Input.GetKeyDown(KeyCode.T) && atf <= 0)
                    {
                        anim.SetInteger("Guard", 0);
                        state = 0;
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
                            atf = 66;
                            anim.SetInteger("waza", 1);
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
                    if (Input.GetKeyDown(KeyCode.Y) && atf <= 0)
                    {
                        anim.SetInteger("Guard", 0);
                        state = 0;
                        this.GetComponent<Common>().waza = 2;
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
                            atf = 30;
                            anim.SetInteger("waza", 2);
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
                }
            }
        }


        public void Attack1()
        {
            audioSource.PlayOneShot(soundKopan);
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
                        hitEnemy.GetComponent<Common>().OnDamage((int)(40 * (10 - hitcount) / 10), 0.03f * this.GetComponent<Common>().face, 0, 4, 8, 19);
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

        public void Attack2()
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
                        hitEnemy.GetComponent<Common>().OnDamage((int)(30 * (10 - hitcount) / 10), 0.08f * this.GetComponent<Common>().face, 0, 4, 8, 16);
                        Instantiate(hiteffect1, attackPoint.position, transform.rotation);
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

    }
}
