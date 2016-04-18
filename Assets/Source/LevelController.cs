using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Assets.Source
{
    public class LevelController :MonoBehaviour
    {
        public MelodyData MelodyData;

        public void ToMainMenu()
        {
            GlobalState.Instance.LastLevelName = SceneManager.GetActiveScene().name;
            GlobalState.Instance.LastLevelSolved = true;
            GlobalState.Instance.CompletedMelodies.Add( MelodyData);




            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

         
    }
}