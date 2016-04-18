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
            GlobalState.Instance.MarkLevelCompleted(SceneManager.GetActiveScene().name, MelodyData);


            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }

     
    }
}