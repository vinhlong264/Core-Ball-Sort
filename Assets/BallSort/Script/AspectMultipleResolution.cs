using UnityEngine;

public class AspectMultipleResolution : MonoBehaviour
{
    private Camera _camera;
    [SerializeField, Range(-1f, 1f)] private float adapterRange = 0f;
    private ScreenOrientation lastOrientation;
    [SerializeField] private bool maintainWidth = true;
    private float targetAspect;

    private Vector3 camPos;
    private float defaultSize;

    void Start()
    {
        _camera = Camera.main;
        camPos = _camera.transform.position;
        lastOrientation = Screen.orientation;

        targetAspect = 16f / 9f;
        defaultSize = _camera.orthographicSize;

        AdjustCamera();
    }
    public void AdjustCamera()
    {
        float currentAspect = (float)Screen.width / Screen.height;

        float scaleFactor;
        if (maintainWidth)
        {
            scaleFactor = currentAspect / targetAspect;
            _camera.orthographicSize = defaultSize/scaleFactor;
        }
        else
        {
            scaleFactor = currentAspect / targetAspect;
            _camera.orthographicSize = defaultSize*scaleFactor;
        }

        float sizeDiff = defaultSize - _camera.orthographicSize; // điều chỉnh vị trí của cameraY
        transform.position = new Vector3(camPos.x, camPos.y + adapterRange * sizeDiff, camPos.z);
    }

    void Update()
    {
        if (Screen.orientation != lastOrientation)
        {
            AdjustCamera();
            lastOrientation = Screen.orientation;
        }
    }
}
