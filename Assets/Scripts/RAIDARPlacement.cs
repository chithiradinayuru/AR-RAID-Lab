
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RAIDARPlacement : MonoBehaviour
{
    public GameObject raidPrefab;
    public ARRaycastManager raycastManager;

    private GameObject placedRAID;

    private static readonly List<ARRaycastHit> hits = new();

    void Update()
    {
        if (placedRAID != null)
            return;

        Vector2 screenPosition;

        // Mouse input for Unity Editor XR Simulation.
#if UNITY_EDITOR
        if (Mouse.current == null ||
            !Mouse.current.leftButton.wasPressedThisFrame)
            return;

        screenPosition = Mouse.current.position.ReadValue();
#else
        // Touch input for Android devices.
        if (Touchscreen.current == null)
            return;

        var touch = Touchscreen.current.primaryTouch;

        if (!touch.press.wasPressedThisFrame)
            return;

        screenPosition = touch.position.ReadValue();
#endif

        // Do not place the RAID model when clicking UI.
        if (EventSystem.current != null)
        {
            var pointerData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };

            var uiHits = new List<RaycastResult>();

            EventSystem.current.RaycastAll(pointerData, uiHits);

            if (uiHits.Count > 0)
                return;
        }

        if (raycastManager.Raycast(
            screenPosition,
            hits,
            TrackableType.PlaneWithinPolygon))
        {
            placedRAID = Instantiate(
                raidPrefab,
                hits[0].pose.position,
                hits[0].pose.rotation
            );

            Debug.Log("RAID model placed successfully.");
        }
        else
        {
            Debug.Log("No AR plane detected at clicked position.");
        }
    }
}
