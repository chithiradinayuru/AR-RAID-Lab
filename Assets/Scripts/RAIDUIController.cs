
using UnityEngine;
using TMPro;

public class RAIDUIController : MonoBehaviour
{
    [Header("Educational UI")]
    public TMP_Text raidDescription;

    private RAID0Controller raid0Controller;

    // Find the RAID model placed in AR.
    private RAID0Controller GetRAIDController()
    {
        if (raid0Controller == null)
        {
            raid0Controller =
                FindAnyObjectByType<RAID0Controller>();

            if (raid0Controller != null)
            {
                raid0Controller.OnExplanationChanged +=
                    UpdateExplanation;
            }
        }

        return raid0Controller;
    }

    // Update the educational information panel.
    private void UpdateExplanation(string explanation)
    {
        if (raidDescription != null)
        {
            raidDescription.text = explanation;
        }
    }

    // Start automatic RAID 0 simulation.
    public void StartRAID0Simulation()
    {
        RAID0Controller controller = GetRAIDController();

        if (controller != null)
        {
            controller.StartRAID0();
            Debug.Log("RAID 0 simulation started.");
        }
        else
        {
            Debug.LogWarning(
                "Place the RAID system on a surface first."
            );
        }
    }

    // Perform one manual RAID 0 step.
    public void NextRAID0Step()
    {
        RAID0Controller controller = GetRAIDController();

        if (controller != null)
        {
            controller.NextStep();
            Debug.Log("RAID 0 next step executed.");
        }
        else
        {
            Debug.LogWarning(
                "Place the RAID system on a surface first."
            );
        }
    }

    // Reset RAID 0 simulation.
    public void ResetRAID0Simulation()
    {
        RAID0Controller controller = GetRAIDController();

        if (controller != null)
        {
            controller.ResetRAID0();
            Debug.Log("RAID 0 simulation reset.");
        }
        else
        {
            Debug.LogWarning(
                "Place the RAID system on a surface first."
            );
        }
    }

    // Unsubscribe from events when destroyed.
    private void OnDestroy()
    {
        if (raid0Controller != null)
        {
            raid0Controller.OnExplanationChanged -=
                UpdateExplanation;
        }
    }
}
