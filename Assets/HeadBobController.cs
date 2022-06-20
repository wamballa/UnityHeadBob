using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour
{
    [SerializeField] private bool isEnabled;
    [SerializeField] [Range(0, 1f)] private float amplitude = 0.015f;
    [SerializeField] [Range(0, 30f)] private float frequency = 10f;
    [SerializeField] private Transform camera = null;
    [SerializeField] private Transform cameraHolder = null;
    [SerializeField] private float stepMultiplier = 2;
    [SerializeField] private float focusDistance = 15;
    private float toggleSpeed = 3f;
    private Vector3 startPos;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        startPos = camera.localPosition;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnabled) return;

        CheckMotion();
        ResetPosition();
        camera.LookAt(FocusTarget());
    }
    private void PlayMotion(Vector3 motion)
    {
        camera.localPosition += motion;
    }
    private void CheckMotion()
    {
        float speed = new Vector3(playerRigidbody.velocity.x, 0, playerRigidbody.velocity.z).magnitude;

        if (speed < toggleSpeed) return;
        PlayMotion(FootStepMotion());
    }
    private void ResetPosition()
    {
        if (camera.localPosition == startPos) return;
        camera.localPosition = Vector3.Lerp(camera.localPosition, startPos, 1 * Time.deltaTime);
    }
    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y = Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x = Mathf.Cos(Time.time * frequency / stepMultiplier) * amplitude * stepMultiplier;
        return pos;
    }
    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * focusDistance;
        return pos;
    }
}
