
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

    private Coroutine simulation;

    public void StartRAID0()
    {
        if (dataBlockPrefab == null || disk1 == null || disk2 == null)
        {
            Debug.LogError("RAID 0 references are missing!");
            return;
        }

        ResetRAID0();
        simulation = StartCoroutine(DistributeBlocks());
    }

    private IEnumerator DistributeBlocks()
    {
        for (int i = 0; i < 4; i++)
        {
            Transform targetDisk = (i % 2 == 0) ? disk1 : disk2;
            int row = i / 2;

            GameObject block = Instantiate(dataBlockPrefab, transform);

            // Position relative to the RAIDSystem root.
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

            Debug.Log(block.name + " created on " + targetDisk.name);

            yield return new WaitForSeconds(delayBetweenBlocks);
        }

        simulation = null;
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
    }
}
