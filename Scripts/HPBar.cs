using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HPBar : MonoBehaviour
{
    [SerializeField] public Image _hpBarcurrent;
    [SerializeField] public Image _hpBarpast;
    [SerializeField] public Image _powerBarcurrent;
    [SerializeField] public Image _win1;
    [SerializeField] public Image _win2;
    [SerializeField] public Text _name;
    [SerializeField] public Text _hits;
    [SerializeField] public Text _hitcount;
    [SerializeField] public Text _roundstate;
    [SerializeField] public int _maxHealth = 1000;
    [SerializeField] public int _maxPower = 2000;
    public float j;
    public int currentHealth = 0, winK, currentPower = 0;
    void Awake()
    {
        currentHealth = 0;
    }
    public void SetName(string name)
    {
        _name.text = string.Format(name);
    }
    public void SetWin(int win)
    {
        winK = win;
        if (win >= 1 && win < 2) _win1.color = new Color(1.0f, 1.0f, 0.0f, 1.0f); 
        if (win >= 2) _win2.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
    }
    public void SetHits(int hitcount)
    {
        if (hitcount > 0)
        {
            if (hitcount >= 100) _hitcount.text = string.Format("{0}", hitcount);
            if (hitcount < 100 && hitcount >= 10) _hitcount.text = string.Format(" {0}", hitcount);
            if (hitcount < 10) _hitcount.text = string.Format("  {0}", hitcount);
            _hits.text = string.Format("HITS");
        }
        if (hitcount <= 0)
        {
            _hitcount.text = string.Format("");
            _hits.text = string.Format("");
        }
    }
    public void UpdateHP(int damageval)
    {
        currentHealth = Mathf.Clamp(currentHealth + damageval, 0, _maxHealth);
        _hpBarcurrent.fillAmount = currentHealth / (float)_maxHealth;
    }
    public void UpdatePower(int power)
    {
        currentPower = power;
        if (currentPower >= _maxPower) currentPower = _maxPower;
        _powerBarcurrent.fillAmount = currentPower / (float)_maxPower;
        if (currentPower >= _maxPower)
        {
            _powerBarcurrent.color = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        }else if (currentPower < (int)_maxPower/2)
        {
            _powerBarcurrent.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
        else
        {
            _powerBarcurrent.color = new Color(0.5f, 1.0f, 0.0f, 1.0f);
        }
    }
    public void ChangeColor()
    {
        _hpBarcurrent.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    }
    public void ReColor()
    {
        var seq = DOTween.Sequence();
        //_hpBarpast.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        while (true)
        {
            j += 0.01f;
            seq.SetDelay(0.2f);
            seq.AppendCallback(() => _hpBarpast.fillAmount = j);
            if (j >= currentHealth / (float)_maxHealth)
            {
                j = currentHealth / (float)_maxHealth;
                _hpBarpast.fillAmount = j;
                break;
            }
        }
    }
}
