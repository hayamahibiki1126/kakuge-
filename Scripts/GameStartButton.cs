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
            //　MyGameManagerDataに保存されている次のシーンに移動する
            sceneTransition.GameStart();
        }
    }
}

