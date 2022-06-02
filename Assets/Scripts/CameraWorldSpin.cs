using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWorldSpin : MonoBehaviour
{
    [SerializeField] private Transform centerObject;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float distanceFromCenter;
    [SerializeField] private float spinSpeed;
    [SerializeField] private float spinDir;
    private Vector3 posCenter;

    private void Awake()
    {
        posCenter = centerObject.GetComponent<Collider>().bounds.center;
    }

    private void Update()
    {
        Camera camera = Camera.main;
        //Vector3 pos = posCenter + offset * -distanceFromCenter;
        //camera.transform.position = pos;
        //camera.transform.LookAt(posCenter, Vector3.up);
        camera.transform.RotateAround(posCenter, Vector3.up, spinDir);
    }

    private IEnumerator CoroutineSpin()
    {
        yield return new WaitForEndOfFrame();
    }
}
