using UnityEngine;
using TMPro;

public class ProgressManager : MonoBehaviour
{
    public TMP_Text rehabProgressText;
    public TMP_Text cprProgressText;

    private void Start()
    {
        // ==============================
        // TARGET REACH
        // ==============================
        int targetReachRepetitions =
            PlayerPrefs.GetInt("TargetReachRepetitions", 0);

        string targetReachStatus =
            targetReachRepetitions >= 10
            ? "Completed"
            : "In Progress";

        // ==============================
        // OBJECT PLACEMENT
        // ==============================
        int objectPlacementRepetitions =
            PlayerPrefs.GetInt("ObjectPlacementRepetitions", 0);

        string objectPlacementStatus =
            objectPlacementRepetitions >= 10
            ? "Completed"
            : "In Progress";

        // ==============================
        // DISPLAY MOTOR REHABILITATION
        // ==============================
        rehabProgressText.text = "Motor Rehabilitation\n\n" +

            "TARGET REACH\n" +
            "Repetitions: " +
            targetReachRepetitions +
            " / 10\n" +
            "Status: " +
            targetReachStatus +
            "\n\n" +

            "OBJECT PLACEMENT\n" +
            "Repetitions: " +
            objectPlacementRepetitions +
            " / 10\n" +
            "Status: " +
            objectPlacementStatus;

        // ==============================
        // CPR
        // ==============================
        cprProgressText.text =
            "CPR Training\n\n" +
            "Status: Not Completed";
    }
}