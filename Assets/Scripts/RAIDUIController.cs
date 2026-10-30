
using UnityEngine;

public class RAIDUIController : MonoBehaviour
{
    private RAID0Controller raid0Controller;

    private RAID0Controller GetRAIDController()
    {
        if (raid0Controller == null)
        {
            raid0Controller =
                FindFirstObjectByType<RAID0Controller>();
        }

        return raid0Controller;
    }

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
}
