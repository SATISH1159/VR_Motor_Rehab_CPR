using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    public void OpenMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenRehabilitationMenu()
    {
        SceneManager.LoadScene("RehabilitationMenu");
    }

    public void OpenTargetReach()
    {
        SceneManager.LoadScene("Rehabilitation_TargetReach");
    }

    public void OpenObjectPlacement()
    {
        SceneManager.LoadScene("Rehabilitation_ObjectPlacement");
    }

    public void OpenCPRTraining()
    {
        SceneManager.LoadScene("CPRTraining");
    }

    public void OpenProgress()
    {
        SceneManager.LoadScene("Progress");
    }

    public void OpenSettings()
    {
        SceneManager.LoadScene("Settings");
    }
}