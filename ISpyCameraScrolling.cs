using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script handles the camera scrolling in I Spy. The camera scrolling is enabled only if the screen width matches a tablet size.

[RequireComponent(typeof(Camera))]
public class ISpyCameraScrolling : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Vector3 touchStart;
    public bool ableSwipe = false;

    private void Start()
    {
        if (Screen.width / Screen.height <= 1.33f) ableSwipe = true;
        else ableSwipe = false;
    }

    private void Update()
    {
        if(ableSwipe)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = GetWorldPosition();
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 direction = new Vector3(touchStart.x - GetWorldPosition().x, 0, 0);

                if (Camera.main.transform.position.x + direction.x <= 8f && Camera.main.transform.position.x + direction.x >= -8f)
                    Camera.main.transform.position += direction;

            }
        }  
    }

    private Vector3 GetWorldPosition()
    {
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.forward, new Vector3(0, 0, 0));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }
}
