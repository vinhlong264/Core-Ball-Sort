using UnityEngine;

public class AspectMultipleResolution : MonoBehaviour
{
    private Camera _camera;
    [Range(-1f, 1f)]
    [SerializeField] private float adapterRange;
    private Vector3 camPos;
    private float defaultWith;
    private float defaultHeight;
    private float sizeCam;

    private void Start()
    {
        _camera = Camera.main;
        camPos = _camera.transform.position;

        defaultHeight = _camera.orthographicSize; // lấy ra chiều cao mặc định của màn hình hiện tại
        defaultWith = _camera.orthographicSize * _camera.aspect; // Lấy ra chiều rộng mặc định của màn hình hiện tại
        AdjustCamera();
        
    }

    private void Update()
    {
        AdjustCamera();
    }

    private void AdjustCamera()
    {
        sizeCam = defaultWith / _camera.aspect;
        _camera.orthographicSize = sizeCam;
        transform.position = new Vector3(camPos.x, adapterRange * (defaultHeight - _camera.orthographicSize), camPos.z);
    }
}
