using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

namespace Characelsect1
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField]
        GameObject transitionPrefab;
        // Start is called before the first frame update
        public float moveSpeed = 7;
        public double hitpause, hitback;
        public int hp = 1000, backf, atf, waza, state, gr, dash;
        public Transform attackPoint;
        public Transform attackPointJJ;
        public float attackRadius;
        public LayerMask enemyLayer;
        public HPBar hpBar;
        public GameObject hiteffect1;
        public GameObject hiteffect2;
        public GameObject enemyObj;
        Rigidbody2D rb;
        Animator anim;

        public AudioClip soundKopan;
        public AudioClip soundKopanHit;
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
            }
            else
            {
                var seq = DOTween.Sequence();
                seq.SetDelay(0.1f);
                seq.AppendCallback(() => enemyLayer = 1 << LayerMask.NameToLayer("Player"));
                enemyObj = GameObject.FindGameObjectWithTag("Player");
            }
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
                if (this.GetComponent<Common>().ycommon >= -1.04f && this.GetComponent<Common>().dawn == 19)
                {
                    rb.isKinematic = true;
                    var seq = DOTween.Sequence();
                    seq.SetDelay(0.1f);
                    if (this.GetComponent<Common>().dawn == 19) seq.AppendCallback(() => transform.Translate(0, -0.4f, 0));
                }

                if (this.GetComponent<Common>().flagh == 19 && this.GetComponent<Common>().guard == 0 || Input.GetKeyDown(KeyCode.R))
                {
                    anim.SetInteger("Guard", 0);
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
                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        anim.SetInteger("Guard", 0);
                        state = 0;
                        this.GetComponent<Common>().waza = 1;
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
