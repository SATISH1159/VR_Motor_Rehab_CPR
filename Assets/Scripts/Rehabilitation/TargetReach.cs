using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TargetReach : MonoBehaviour
{
    [Header("Controller")]
    public Transform controllerTransform;

    [Header("Exercise Settings")]
    public int targetRepetitions = 10;
    public float reachDistance = 0.25f;


    [Header("UI")]
    public TMP_Text repetitionText;
    public Slider progressBar;
    public TMP_Text statusText;

    [Header("Completion UI")]
    public GameObject exercisePanel;
    public GameObject completionPanel;

    [Header("Target Positions")]
    public Transform[] targetPositions;

    [Header("Target Animation")]
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.08f;
    public float moveSpeed = 3f;

    private int repetitions = 0;
    private int currentTarget = 0;
    private bool exerciseCompleted = false;
    private bool movingToNextTarget = false;

    private Vector3 originalScale;
    private Vector3 targetPosition;

    private void Start()
    {
        if (exercisePanel != null)
            exercisePanel.SetActive(true);

        if (completionPanel != null)
            completionPanel.SetActive(false);

        originalScale = transform.localScale;
        targetPosition = transform.position;

        UpdateUI();

        if (statusText != null)
        {
            statusText.text = "Reach the highlighted target";
        }

        if (progressBar != null)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = targetRepetitions;
            progressBar.value = 0;
        }
    }

    private void Update()
    {
        if (exerciseCompleted)
            return;

        AnimateTarget();

        if (movingToNextTarget)
        {
            MoveTarget();
            return;
        }

        if (controllerTransform == null)
            return;

        float distance = Vector3.Distance(
            controllerTransform.position,
            transform.position
        );

        if (distance <= reachDistance)
        {
            CompleteReach();
        }
    }

    private void AnimateTarget()
    {
        float pulse =
            1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

        transform.localScale =
            originalScale * pulse;
    }

    private void CompleteReach()
    {
        if (exerciseCompleted)
            return;

        repetitions++;

        PlayerPrefs.SetInt("TargetReachRepetitions", repetitions);
        PlayerPrefs.Save();

        if (statusText != null)
        {
            statusText.text = "Target reached !";
        }

        UpdateUI();

        Debug.Log(
            "TARGET REACHED: " +
            repetitions +
            " / " +
            targetRepetitions
        );

        if (repetitions >= targetRepetitions)
        {
            exerciseCompleted = true;
            repetitions = targetRepetitions;

            PlayerPrefs.SetInt("TargetReachRepetitions", targetRepetitions);
            PlayerPrefs.SetInt("TargetReachCompleted", 1);
            PlayerPrefs.Save();

            if (statusText != null)
                statusText.text = "Exercise Complete!";

            if (exercisePanel != null)
                exercisePanel.SetActive(false);

            if (completionPanel != null)
                completionPanel.SetActive(true);

            Debug.Log("TARGET REACH EXERCISE COMPLETE!");

            return;
        }

        if (statusText != null)
        {
            statusText.text = "Next target...";
        }

        MoveToNextTarget();
    }

    private void MoveToNextTarget()
    {
        if (targetPositions == null ||
            targetPositions.Length == 0)
            return;

        currentTarget++;

        if (currentTarget >= targetPositions.Length)
            currentTarget = 0;

        targetPosition =
            targetPositions[currentTarget].position;

        movingToNextTarget = true;
    }

    private void MoveTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(
                transform.position,
                targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            movingToNextTarget = false;
        }
    }

    private void UpdateUI()
    {
        if (repetitionText != null)
        {
            repetitionText.text =
                "REPETITIONS\n" +
                repetitions +
                " / " +
                targetRepetitions;
        }

        if (progressBar != null)
        {
            progressBar.value = repetitions;
        }
    }
}