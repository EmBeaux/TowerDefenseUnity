using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    [SerializeField]
    private LayerMask placementLayerMask;

    private Vector3 lastPosition;

    public Vector3 GetSelectedMapPosition(Vector3? other = null) {
        Vector3 mousePos;
        if (other == null)
        {
            mousePos = Input.mousePosition;
        } else {
            mousePos = (Vector3)other;
        }
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayerMask)) {
            lastPosition = hit.point;
        }
        
        return lastPosition;
    }
}
