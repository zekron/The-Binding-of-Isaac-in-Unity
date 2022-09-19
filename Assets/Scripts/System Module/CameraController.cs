using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private TwoVector3EventChannelSO onEnterRoomEvent;
    private bool needMove = false;
    private Vector3 finalPosition;
    private Vector3 velocity = Vector3.zero;
    private const float SmoothTime = 0.1f;

    private void OnEnable()
    {
        onEnterRoomEvent.OnEventRaised += MoveCamera;
    }
    private void OnDisable()
    {
        onEnterRoomEvent.OnEventRaised -= MoveCamera;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    void LateUpdate()
    {
        if (needMove)
        {
            transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref velocity, SmoothTime);

            //if (Mathf.Abs(transform.position.x - finalPosition.x) <= 1e-2) needMove = false;
        }
    }

    private void MoveCamera(Vector3 cameraPosition, Vector3 playerPosition)
    {
        cameraPosition.z = -10;
        finalPosition = cameraPosition;
        needMove = true;
    }
}
