using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Characelsect1
{
    public class GameStartButton : MonoBehaviour
    {
        private SceneTrans sceneTransition;

        private void Start()
        {
            sceneTransition = FindObjectOfType<SceneTrans>();
        }

        public void OnGameStart()
        {
            //�@MyGameManagerData�ɕۑ�����Ă��鎟�̃V�[���Ɉړ�����
            sceneTransition.GameStart();
        }
    }
}

