using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Characelsect1
{
    public class Tyouhieffe : MonoBehaviour
    {
        public LayerMask enemyLayer;
        public Transform attackPoint;
        public float attackRadius;
        public GameObject hiteffect;
        public float lefttime;
        public AudioClip soundHit;
        AudioSource audioSource;
        public GameObject enemyObj;
        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            transform.Translate(0, 0.4f, 0);
            Destroy(gameObject, lefttime);
            if (this.gameObject.tag == "Play") {
                enemyLayer = 1 << LayerMask.NameToLayer("Enemy");
                enemyObj = GameObject.FindGameObjectWithTag("Enemy");
            }
            else
            {
                enemyLayer = 1 << LayerMask.NameToLayer("Player");
                enemyObj = GameObject.FindGameObjectWithTag("Player");
            }
            Vector3 tmp = this.transform.position;
            transform.Translate(enemyObj.GetComponent<Common>().xcommon - tmp.x, enemyObj.GetComponent<Common>().ycommon - tmp.y, 0);
        }

        public void Uke(GameObject enemy, LayerMask enemyL)
        {
            enemyObj = enemy;
            enemyLayer = enemyL;
            Vector3 tmp = this.transform.position;
            transform.Translate(enemyObj.GetComponent<Common>().xcommon - tmp.x, enemyObj.GetComponent<Common>().ycommon - tmp.y, 0);
        }

        public void Attack1()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

            foreach (Collider2D hitEnemy in hitEnemys)
            {
                hitEnemy.GetComponent<Common>().OnDamage(20, 0, 0, 99, 8, 99);
                Instantiate(hiteffect, attackPoint.position, transform.rotation);
                audioSource.PlayOneShot(soundHit);
            }
        }

        public void Attack2()
        {
            Collider2D[] hitEnemys = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, enemyLayer);

            foreach (Collider2D hitEnemy in hitEnemys)
            {
                hitEnemy.GetComponent<Common>().OnDamage(60, 0, 0.06f, 12, 8, 30);
                Instantiate(hiteffect, attackPoint.position, transform.rotation);
                audioSource.PlayOneShot(soundHit);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
