using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private ColorType type;
    public ColorType Type { get => type; }
    private float duration;
    private Rigidbody2D rb;
    private int index;

    private void Start()
    {
        duration = 2f;
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveBallStart(Vector3 position)
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        StartCoroutine(MoveCroutine(position));
    }

    IEnumerator MoveCroutine(Vector3 position, System.Action callBack = null)
    {
        Vector3 startPoint = transform.position;
        Vector3 endPoint = position;

        float time = 0;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPoint, endPoint, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.position = endPoint;
        Debug.Log("End crountine");
    }

    public void MoveDestination(Vector3[] destination)
    {
        StartCoroutine(MoveDestinationCroutine(destination));
    }

    IEnumerator MoveDestinationCroutine(Vector3[] destination)
    {
        if (Vector2.Distance(transform.position, destination[index]) < 0.1f)
        {
            index++;
        }


        while (index < destination.Length)
        {
            Vector3 startPoint = transform.position;
            Vector3 endPoint = destination[index];

            float time = 0;
            while (time < duration)
            {
                transform.position = Vector3.Lerp(startPoint, endPoint, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            transform.position = endPoint;
            index++;
            yield return null;
        }
        index = 0;
        changeBody();
        StopAllCoroutines();
    }


    public void changeBody()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}

public enum ColorType
{
    RED, BLUE, GREEN, YELLOW
}
