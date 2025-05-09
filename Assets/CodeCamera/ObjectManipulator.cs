using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    private GameObject CurrentARObject = null;

    [SerializeField] private Camera arCamera;

    // prueba de rama

    private float rotateSpeed = 4.5f; // Reducida velocidad de rotación
    private float zoomSpeed = 0.05f; // Ajustada velocidad de zoom

    private float touchStartDistance;
    private int previousTouchCount;


    public void usedObject(GameObject ArOject)
    {
        CurrentARObject = ArOject;

    }

    public void sinEnfoque(GameObject ArOject)
    {   
        if(CurrentARObject == ArOject) CurrentARObject = null;

    }


    void Update()
    {
        int touchCount = Input.touchCount;

        if (touchCount == 2)
        {
            HandleMultiTouch();
        }
        else if (touchCount == 1)
        {
            if (previousTouchCount != 2) HandleSingleTouch();
        }

        previousTouchCount = touchCount;

    }

    private void HandleMultiTouch()
    {
        if (CurrentARObject == null) return;

        Touch t1 = Input.GetTouch(0);
        Touch t2 = Input.GetTouch(1);

        if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
        {
            touchStartDistance = Vector2.Distance(t1.position, t2.position);
        }

        if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
        {
            float currentDistance = Vector2.Distance(t1.position, t2.position);
            ApplyZoom(currentDistance - touchStartDistance);
            touchStartDistance = currentDistance;
        }
    }

    private void HandleSingleTouch()
    {
        Touch touch = Input.GetTouch(0);

        if (CurrentARObject == null)
        {
            if (touch.phase == TouchPhase.Ended) CheckObjectSelection(touch.position);
            return;
        }

        if (touch.phase == TouchPhase.Moved)
        {
            ApplyRotation(touch.deltaPosition.x);
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            CheckObjectSelection(touch.position);
        }
    }

    private void ApplyRotation(float deltaX)
    {
        float rotationAmount = - deltaX * rotateSpeed * Time.deltaTime; // Más suave y con deltaTime
        CurrentARObject.transform.Rotate(Vector3.up, rotationAmount, Space.World);
    }

    private void ApplyZoom(float delta)
    {
        float zoomFactor = delta * zoomSpeed * Time.deltaTime;
        Vector3 newScale = CurrentARObject.transform.localScale + Vector3.one * zoomFactor;
        newScale = Vector3.Max(newScale, Vector3.one * 0.1f);
        newScale = Vector3.Min(newScale, Vector3.one * 3f);
        CurrentARObject.transform.localScale = newScale;
    }

    private void CheckObjectSelection(Vector2 screenPos)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("ARObject"))
        {
            CurrentARObject = hit.transform.gameObject;
        }
    }
}