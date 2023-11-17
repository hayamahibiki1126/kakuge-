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
            //@MyGameManagerData‚É•Û‘¶‚³‚ê‚Ä‚¢‚éŸ‚ÌƒV[ƒ“‚ÉˆÚ“®‚·‚é
            sceneTransition.GameStart();
        }
    }
}

