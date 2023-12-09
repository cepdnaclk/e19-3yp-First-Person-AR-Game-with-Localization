using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void MovetoScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}
