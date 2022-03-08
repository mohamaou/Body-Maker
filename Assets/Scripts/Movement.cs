using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public float speed = 1f;
    [SerializeField] private Transform cameraPoint;
    private Vector3  direction;
    private static Vector3 firstMousePoint;
    private float localSpeed;
    
    
    private void FixedUpdate()
    {
        if(GameManager.State != GameState.Play) return;
        localSpeed = Run() ? Mathf.Lerp(localSpeed,speed,6 * Time.deltaTime) : Mathf.Lerp(localSpeed,speed / 3,6 * Time.deltaTime);
        
        rb.MovePosition(transform.position + Direction() * localSpeed * Time.deltaTime); 
        if (Direction() != Vector3.zero)
        { 
            transform.rotation = 
                Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Direction()), speed * 2 * Time.deltaTime);
        }
        cameraPoint.position = transform.position;
    }

    public static  bool Run()
    {
        var dist = Vector3.Distance(firstMousePoint, Input.mousePosition) / Screen.height;
        return dist > 0.05f;
    }
    private Vector3 Direction()
    {
        if (!Input.GetMouseButton(0)) return Vector3.zero;
        if (Input.GetMouseButtonDown(0)) firstMousePoint = Input.mousePosition;
        var dist = Vector3.Distance(Input.mousePosition, firstMousePoint);
        if (dist > Screen.height / 8f)
        {
            var x = (firstMousePoint.x - Input.mousePosition.x) / 4;
            var y = (firstMousePoint.y - Input.mousePosition.y) / 4;
            firstMousePoint = Input.mousePosition + new Vector3(x, y);
        }
        var dir = (firstMousePoint - Input.mousePosition).normalized;
        return new Vector3(-dir.x, 0, -dir.y);
    }
}