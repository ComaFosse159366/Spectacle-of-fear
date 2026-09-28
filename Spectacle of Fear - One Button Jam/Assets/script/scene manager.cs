using UnityEngine;

public class scenemanager : MonoBehaviour
{
   public void changescene(string sceneName)
   {
       UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
   }
}
