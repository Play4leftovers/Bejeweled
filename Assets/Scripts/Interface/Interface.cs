using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour
{
    private Vector3 _savedMouseWorldPosition;
    public Camera worldCamera;
    public GameObject board;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 currentMouseClick = GetMouseWorldPosition();
            if (_savedMouseWorldPosition == Vector3.zero)
            {
                _savedMouseWorldPosition = currentMouseClick;
                return;
            }
            board.GetComponent<Board>().MouseBoardBridge(_savedMouseWorldPosition, currentMouseClick);
            _savedMouseWorldPosition = Vector3.zero;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 vec = worldCamera.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0f;
        return vec;
    }
}
