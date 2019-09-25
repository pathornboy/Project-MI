using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float FollowSpeed = 0.4f;
    public float maxScreenPoint = 0.7f;
    public Transform Target;

    private void Update()
    {
        //Vector3 newPosition = Target.position;
        //newPosition.z = -10;
        //transform.position = Vector3.Slerp(transform.position, newPosition, FollowSpeed * Time.deltaTime);



        Vector3 mousePos = Input.mousePosition * maxScreenPoint + new Vector3(Screen.width, Screen.height, 0f) * ((1f - maxScreenPoint) * 0.5f);
        //Vector3 position = (target.position + GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition)) / 2f;
        Vector3 position = (Target.position + GetComponent<Camera>().ScreenToWorldPoint(mousePos)) / 2f;
        Vector3 destination = new Vector3(position.x, position.y, -10);
        //transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        transform.position = Vector3.Slerp(transform.position, destination, FollowSpeed * Time.deltaTime);

    }
}