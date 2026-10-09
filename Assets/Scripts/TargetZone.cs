using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;

public class TargetZone : MonoBehaviour
{
    public GameObject exercisePanel;
    public GameObject completionPanel;

    [Header("Exercise Settings")]
    public int targetRepetitions = 10;

    public float requiredMovementDistance = 0.5f;

    public float placementDistance = 0.35f;

    public TMP_Text repetitionText;

    [Header("Colors")]
    public Color normalColor = Color.white;
    public Color correctColor = Color.green;

    private int repetitions = 0;

    private bool exerciseCompleted = false;

    private Renderer targetRenderer;

    private XRGrabInteractable grabInteractable;

    private Vector3 grabStartPosition;

    private float maximumDistanceMoved;

    private bool exerciseStarted = false;


    private void Start()
    {
        UpdateRepetitionUI();

        targetRenderer = GetComponent<Renderer>();

        if (targetRenderer != null)
            targetRenderer.material.color = normalColor;

        GameObject rehabObject =
            GameObject.FindGameObjectWithTag("RehabObject");

        if (rehabObject != null)
        {
            grabInteractable =
                rehabObject.GetComponent<XRGrabInteractable>();

            if (grabInteractable != null)
            {
                grabInteractable.selectEntered.AddListener(OnGrab);
                grabInteractable.selectExited.AddListener(OnRelease);
            }
        }

        if (exercisePanel != null)
            exercisePanel.SetActive(true);

        if (completionPanel != null)
            completionPanel.SetActive(false);
    }


    private void Update()
    {
        if (!exerciseStarted || grabInteractable == null)
            return;

        float distance =
            Vector3.Distance(
                grabStartPosition,
                grabInteractable.transform.position
            );

        if (distance > maximumDistanceMoved)
        {
            maximumDistanceMoved = distance;
        }
    }


    private void OnGrab(SelectEnterEventArgs args)
    {
        if (exerciseCompleted)
            return;

        exerciseStarted = true;

        grabStartPosition =
            grabInteractable.transform.position;

        maximumDistanceMoved = 0f;

        if (targetRenderer != null)
            targetRenderer.material.color = normalColor;

        Debug.Log("Object Placement exercise started. Move the object.");
    }


    private void OnRelease(SelectExitEventArgs args)
    {
        if (exerciseCompleted)
            return;

        exerciseStarted = false;

        float distanceToTarget =
            Vector3.Distance(
                grabInteractable.transform.position,
                transform.position
            );

        Debug.Log(
            "Movement: " +
            maximumDistanceMoved.ToString("F2") +
            " m"
        );

        Debug.Log(
            "Distance to target: " +
            distanceToTarget.ToString("F2") +
            " m"
        );


        // Check BOTH conditions
        if (maximumDistanceMoved >= requiredMovementDistance &&
            distanceToTarget <= placementDistance)
        {
            repetitions++;

            // Prevent going above 10
            if (repetitions > targetRepetitions)
                repetitions = targetRepetitions;

            UpdateRepetitionUI();

            // =====================================
            // SAVE OBJECT PLACEMENT PROGRESS
            // =====================================

            PlayerPrefs.SetInt(
                "ObjectPlacementRepetitions",
                repetitions
            );

            if (repetitions >= targetRepetitions)
            {
                PlayerPrefs.SetInt(
                    "ObjectPlacementCompleted",
                    1
                );
            }

            PlayerPrefs.Save();

            // =====================================

            if (targetRenderer != null)
                targetRenderer.material.color = correctColor;

            Debug.Log(
                "OBJECT PLACEMENT CORRECT! Repetition " +
                repetitions +
                " / " +
                targetRepetitions
            );


            if (repetitions >= targetRepetitions)
            {
                exerciseCompleted = true;

                repetitions = targetRepetitions;

                UpdateRepetitionUI();

                Debug.Log(
                    "OBJECT PLACEMENT EXERCISE COMPLETE!"
                );

                if (exercisePanel != null)
                    exercisePanel.SetActive(false);

                if (completionPanel != null)
                    completionPanel.SetActive(true);

                return;
            }
        }
        else
        {
            if (targetRenderer != null)
                targetRenderer.material.color = normalColor;

            Debug.Log("Object placement not counted.");

            if (maximumDistanceMoved < requiredMovementDistance)
            {
                Debug.Log(
                    "Move the object farther before releasing."
                );
            }

            if (distanceToTarget > placementDistance)
            {
                Debug.Log(
                    "Object is not close enough to the target."
                );
            }
        }
    }


    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }


    private void UpdateRepetitionUI()
    {
        if (repetitionText != null)
        {
            repetitionText.text =
                repetitions +
                " / " +
                targetRepetitions;
        }
    }
}