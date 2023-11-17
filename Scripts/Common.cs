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
            KOdasu = GameObject.Find("Canvas"); //��ʕ`����Q��
            if (this.gameObject.tag == "Player") //�L�����̗����ʒu�ŏ�������
            {
                face = 1;
                hpBar = GameObject.Find("Canvas/HPBarP").GetComponent<HPBar>(); //HPbar�̃I�u�W�F�N�g���Q��
                side = 1;
            }
            else
            {
                face = -1;
                hpBar = GameObject.Find("Canvas/HPBar").GetComponent<HPBar>(); 
                side = 2;
            }
            power = myGameManagerData.GetPower(side); //�p���[�Q�[�W�Q��
            win = myGameManagerData.GetWin(this.GetComponent<Common>().side); //win���\�����Q��
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
                if (power > powermax) power = powermax; //powermax���z���Ȃ��悤��
                if (power != powerprev || power == 0) hpBar.UpdatePower(power); //�p���[�Q�[�W�ɔ��f
                powerprev = power;
                Vector3 tmp = transform.position;
                if (tmp.x >= 2.765f) transform.Translate(2.765f - tmp.x, 0, 0); //�ʒu�C��
                if (tmp.x <= -2.547f) transform.Translate(-2.547f - tmp.x, 0, 0);
                if (tmp.y <= -1.04f && dawn < 19 && (syagami != 19 || flagh == 19)) transform.Translate(0, -1.04f - tmp.y, 0);
                tmp = transform.position; //���̈ʒu���L�^
                xcommon = tmp.x;
                ycommon = tmp.y;
                if (ycommon >= -1.04f && flagh != 19) //�W�����v��
                {
                    airf = 0;
                    onaji = 0;
                    rb.isKinematic = false; //�������Z��؂�
                }
                else
                {
                    rb.isKinematic = true;
                }
                if (flagh == 19) //��炢������
                {
                    if (pause > 0) //��e�d��
                    {
                        pause -= 1;
                    }
                    if (pause <= 0 && hits >= 1) //�d�����ĂȂ��ړ�����
                    {
                        transform.Translate(xx, yy - (0.098f / 180) * gr, 0); //xy�����ɓ�����
                        hits -= 1;
                        gr += 1;
                    }
                    //tmp = transform.position;
                    if (pause <= 0 && hits <= 0 && ycommon >= -1) //�󒆔�e
                    {
                        xx = 0;
                        yy -= (0.098f / 120);
                        transform.Translate(0, yy, 0);//y��������������
                        gr += 1;
                    }
                    if (pause <= 0 && hits <= 0 && hitt > 0) //�ړ����ԉ߂�����̋�炢����
                    {
                        hitt -= 1;
                    }
                    tmp = transform.position;
                    if (pause <= 0 && hits <= 0 && ycommon <= -1 && (hitt <= 0 || yy != 0f)) /�_�E������
                    {
                        gr = 0;
                        rb.velocity = new Vector2(0, 0); //�d�̓��Z�b�g
                        xx = 0;
                        hpBar.ReColor();
                        var seq = DOTween.Sequence(); //�ҋ@���ԗp
                        if (yy != 0 || airf == 19)
                        {
                            airf = 0;
                            rb.isKinematic = true; //�������Z�I��
                            yy = 0; 
                            transform.Translate(0, -0.45f, 0); //�ʒu���炷
                            dawn = 19;
                            muteki = 1;
                            if (hp <= 0) Die(); //hp0�Ȃ玀�S������
                            anim.SetInteger("Dawn", 19); //�_�E���A�j����
                            if (hp > 0)
                            {
                                myGameManagerData.SetPower(side, power); //�p���[�Q�[�W�C��
                                seq.SetDelay(0.6f); //0.6f�ҋ@���Ԕ���
                                seq.AppendCallback(() => hitcount = 0); //�ҋ@�㕜�A�����ŕϐ����Z�b�g���_�E���A�j������
                                seq.AppendCallback(() => anim.SetInteger("Dawn", 0));
                                seq.AppendCallback(() => muteki = 0);
                                seq.AppendCallback(() => dawn = 20);
                            }
                        }
                        flagh = 0;
                        seq.SetDelay(0.2f); //0.2f�ҋ@
                        if (yy <= 0) seq.AppendCallback(() => hitcount = 0); //�q�b�g�J�E���g���Z�b�g
                        anim.SetInteger("nondamaged", 1);
                        anim.SetInteger("damage", 0);
                    }
                }
            }
        }

        IEnumerator Wait(double f)
        {
            yield return new WaitForSeconds((float)f); //�w��f�ҋ@
        }

        public void OnDamage(int damage, float xh, float yh, int pausetime, int hitslide, int hittime)
        {
            hitcount += 1; //�q�b�g�����Z
            anim.SetInteger("Tati", 0); //�U���A�j������
            anim.SetInteger("Jamp", 0);
            anim.SetInteger("waza", 0);
            power += damage; //�_���[�W���p���[����
            anim.SetInteger("nondamaged", 0); //��e�A�j���ڍs
            dawn = 0; //�s���t���O����
            waza = 0;
            guard = 0;
            anim.SetInteger("Guard", 0); //�K�[�h����
            hp -= damage; //�_���[�W����
            if (hp <= 0 && noko != 0) hp = 1; //noko�t���O��������Ƃ���1�őς���
            hpBar.ChangeColor(); //hp�̐F��ύX
            hpBar.UpdateHP(damage); //hp�o�[�Ƀ_���[�W���f
            if (hp <= 0)
            {
                KOdasu.GetComponent<KOdasu>().KOhyouzi(); //hp0�Ȃ�KO�\��
                //Die();
            }
            hits = hitslide;
            //if (onaji != enemyObj.GetComponent<Common>().waza)
            //{
            //hitt = hittime + 20; 
            //}
            //else
            //{
            hitt = hittime; //��炢����
            //}
            //naji = enemyObj.GetComponent<Common>().waza;
            pause = pausetime; //�d������
            xx += xh; //��炢���ړ����x(x)
            yy += yh; //(y)
            //if (ycommon > -1.04f && yy <= 0) yy = 0.01f;
            if (hp <= 0 && yy <= 0.05f) yy = 0.05f; //���ʂƂ��������
            if (ycommon >= -1 && yy <= 0.01f) yy = 0.01f; //���ł�Ȃ������ɔ�΂�
            if (yy != 0 || airf == 19) //�󒆔�e���ǂ���
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
            if (yy > 0) anim.SetInteger("Syagami", 0); //��ɔ�΂���鎞���Ⴊ�݉���
            if (yy > 0.01f) syagami = 0; //���Ⴊ�݉���
            flagh = 19; //��e�t���O
            gr = 0;
        }

        public void OnGuard(int damage, int hittime) //�K�[�h����
        {
            hp -= damage;
            hpBar.ChangeColor();
            hpBar.UpdateHP(damage);
            if (hp <= 0)
            {
                OnDamage(0, 0, 0, 0, 0, 0); //���񂾂Ƃ��̂ݔ�e������
            }
            hitt = hittime;
            flagh = 19;
            gr = 0;
        }

        void Die() //���S����
        {
            hp = 0;
            anim.SetTrigger("Die"); //���S�A�j��
            var seq = DOTween.Sequence(); 
            seq.SetDelay(1.0f); //1.0f�ҋ@
            seq.AppendCallback(() => ToNext()); //���̎����ֈڍs
        }

        void ToNext() //���̎�����
        {
            hitcount = 0;
            Instantiate(transitionPrefab); //�����ڍs���o
            myGameManagerData.SetPower(this.GetComponent<Common>().side, this.GetComponent<Common>().power); //���p�������Z�b�g
            var seq = DOTween.Sequence();
            seq.SetDelay(0.2f); //0.2f�ҋ@
            if (lose <= 0) seq.AppendCallback(() => SceneManager.LoadScene("MainScene")); //�܂������������Ȃ玟�̎�����ʂ�
            if (lose >= 1) seq.AppendCallback(() => SceneManager.LoadScene("Charaselsect1")); //�����������Ȃ�L�����Z����ʂ�
        }
    }
}
