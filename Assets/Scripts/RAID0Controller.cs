
using System;
using System.Collections;
using UnityEngine;

public class RAID0Controller : MonoBehaviour
{
    [Header("References")]
    public GameObject dataBlockPrefab;
    public Transform disk1;
    public Transform disk2;

    [Header("Simulation Settings")]
    public float delayBetweenBlocks = 0.8f;

    public event Action<string> OnExplanationChanged;

    private Coroutine simulation;
    private int currentStep = 0;

    private readonly string[] explanations =
    {
        "Block A1 is written to Disk 1. This begins data striping.",
        "Block A2 is written to Disk 2. Data is distributed across both disks.",
        "Block A3 is written to Disk 1. The alternating pattern continues.",
        "Block A4 is written to Disk 2. The striping example is complete."
    };

    public void StartRAID0()
    {
        if (!ReferencesValid())
            return;

        ResetRAID0();
        simulation = StartCoroutine(AutoSimulation());
    }

    private IEnumerator AutoSimulation()
    {
        while (currentStep < 4)
        {
            CreateNextBlock();
            yield return new WaitForSeconds(delayBetweenBlocks);
        }

        simulation = null;
    }

    public void NextStep()
    {
        if (!ReferencesValid())
            return;

        // Switch from automatic to manual mode.
        if (simulation != null)
        {
            StopCoroutine(simulation);
            simulation = null;
        }

        if (currentStep < 4)
        {
            CreateNextBlock();
        }
        else
        {
            OnExplanationChanged?.Invoke(
                "All four blocks have been distributed. " +
                "RAID 0 provides no redundancy."
            );
        }
    }

    private void CreateNextBlock()
    {
        int i = currentStep;

        Transform targetDisk = (i % 2 == 0) ? disk1 : disk2;
        int row = i / 2;

        GameObject block = Instantiate(dataBlockPrefab, transform);

        block.transform.localPosition = new Vector3(
            targetDisk.localPosition.x,
            0.19f,
            targetDisk.localPosition.z - 0.06f + row * 0.12f
        );

        block.transform.localRotation = Quaternion.identity;
        block.transform.localScale = new Vector3(
            0.08f, 0.025f, 0.06f
        );

        block.name = "DataBlock_A" + (i + 1);

        OnExplanationChanged?.Invoke(explanations[i]);

        Debug.Log(block.name + " created on " + targetDisk.name);

        currentStep++;
    }

    public void ResetRAID0()
    {
        if (simulation != null)
        {
            StopCoroutine(simulation);
            simulation = null;
        }

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);

            if (child.name.StartsWith("DataBlock_A"))
            {
                Destroy(child.gameObject);
            }
        }

        currentStep = 0;

        OnExplanationChanged?.Invoke(
            "RAID 0 divides data into blocks and distributes them " +
            "alternately across disks. Press Start or Next Step to begin."
        );
    }

    private bool ReferencesValid()
    {
        if (dataBlockPrefab == null || disk1 == null || disk2 == null)
        {
            Debug.LogError("RAID 0 references are missing!");
            return false;
        }

        return true;
    }
}
