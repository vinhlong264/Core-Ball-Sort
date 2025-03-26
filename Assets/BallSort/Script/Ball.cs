using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private ColorType type;
    public ColorType Type { get => type; }
    private float duration; // thời gian di chuyển của ball
    private Rigidbody2D rb;
    private int index;

    private void Start()
    {
        duration = 1f;
        rb = GetComponent<Rigidbody2D>();
    }

    public void MoveBallTop(Vector2 pos) // hàm dùng để di chuyển ball lên đỉnh của tube được chọn
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        StartCoroutine(MoveBallTopCrountine(pos));
    }

    IEnumerator MoveBallTopCrountine(Vector2 pos)
    {
        float time = 0;
        Vector3 startPos = transform.position;
        Vector3 EndPos = pos;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPos, EndPos, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void MoveBallDestination(Vector2 startPos, Vector2 endPos, float height) // Di chuyển ball được chọn đến đích
    {
        StopCoroutine(MoveBallTopCrountine(startPos));
        StartCoroutine(MoveBallDestinationCroutine(startPos, endPos, height));
    }

    IEnumerator MoveBallDestinationCroutine(Vector2 startPos, Vector2 destination, float height)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.position = GetPos(startPos, destination, elapsed / duration, height);
            yield return null;
        }
        transform.position = destination;
    }

    private Vector3 GetPos(Vector2 startPos, Vector2 destination, float t, float height) // Tính toán điểm di chuyển từ startPos -> endPos
    {
        // tính chiều ngang mà bóng tới
        float x = Mathf.Lerp(startPos.x, destination.x, t);

        // tính độ cao mà ball sẽ tới
        float y = Mathf.Lerp(startPos.y, destination.y, t) + height * 4 * t * (1 - t);

        return new Vector3(x, y);
    }

    public void changeBodyRb()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
}

public enum ColorType
{
    RED, BLUE, GREEN, YELLOW, NONE
}
