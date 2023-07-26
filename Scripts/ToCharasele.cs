using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class ToCharasele : MonoBehaviour
{
    [SerializeField]
    GameObject transitionPrefab;

    readonly float waitTime = 0.2f;
    public int flag = 0;

    public AudioClip soundSelect;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchScene();
        }
    }
            
    public void SwitchScene()
    {
        if (flag == 0) {
            audioSource.PlayOneShot(soundSelect); 
        }
        flag = 19;

        var seq = DOTween.Sequence();
        seq.SetDelay(0.5f);
        seq.AppendCallback(() => ToNext());
    }

    public void ToNext()
    {
        Instantiate(transitionPrefab);
        var seq = DOTween.Sequence();
        seq.SetDelay(waitTime);
        seq.AppendCallback(() => SceneManager.LoadScene("Charaselsect1"));
    }
}
