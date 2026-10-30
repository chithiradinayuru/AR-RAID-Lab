
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class RAIDARPlacement : MonoBehaviour
{
    [SerializeField] private GameObject raidPrefab;
    [SerializeField] private ARRaycastManager raycastManager;

    private GameObject placedRAID;
    private static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        if (placedRAID != null || Touchscreen.current == null)
            return;

        var touch = Touchscreen.current.primaryTouch;

        if (!touch.press.wasPressedThisFrame)
            return;

        Vector2 position = touch.position.ReadValue();

        if (EventSystem.current != null &&
            EventSystem.current.IsPointerOverGameObject(touch.touchId.ReadValue()))
            return;

        if (raycastManager.Raycast(position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;
            placedRAID = Instantiate(raidPrefab, pose.position, pose.rotation);
        }
    }
}
