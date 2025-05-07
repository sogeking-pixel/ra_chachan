using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    public GameObject ARObject0;
    public GameObject ARObject1;
    public GameObject ARObject2;
    public GameObject ARObject = null;

    [SerializeField] private Camera arCamera;

    [Header("Configuración")]
    private float rotateSpeed = 4.5f; // Reducida velocidad de rotación
    private float zoomSpeed = 0.05f; // Ajustada velocidad de zoom
    //[SerializeField] private float zoomSmoothing = 1f; // Nuevo parámetro de suavizado

    private float touchStartDistance;
    private int previousTouchCount;
    private Vector3 targetScale;


    public void usarTigre()
    {
        this.ARObject = ARObject0;

    }

    public void usarRoedor()
    {
        this.ARObject = ARObject1;

    }

    public void sinEnfoque()
    {
        this.ARObject = null;
    }


    void Start()
    {
        if (ARObject != null) targetScale = ARObject.transform.localScale;
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

        //Suavizado del zoom
        //if (ARObject != null && ARObject.transform.localScale != targetScale)
        //{
        //    ARObject.transform.localScale = Vector3.Lerp(
        //        ARObject.transform.localScale,
        //        targetScale,
        //        zoomSmoothing * Time.deltaTime * 50
        //    );
        //}
    }

    private void HandleMultiTouch()
    {
        if (ARObject == null) return;

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

        if (ARObject == null)
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
        ARObject.transform.Rotate(Vector3.up, rotationAmount, Space.World);
    }

    private void ApplyZoom(float delta)
    {
        float zoomFactor = delta * zoomSpeed * Time.deltaTime;
        Vector3 newScale = ARObject.transform.localScale + Vector3.one * zoomFactor;
        newScale = Vector3.Max(newScale, Vector3.one * 0.01f);
        newScale = Vector3.Min(newScale, Vector3.one * 3f);
        ARObject.transform.localScale = newScale;
    }

    private void CheckObjectSelection(Vector2 screenPos)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("ARObject"))
        {
            ARObject = hit.transform.gameObject;
            targetScale = ARObject.transform.localScale;
        }
    }
}