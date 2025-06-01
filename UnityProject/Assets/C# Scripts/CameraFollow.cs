using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;        // Player 오브젝트
    public float offsetY = 2f;      // 카메라 수직 오프셋
    public float followThreshold = 3f; // Player가 이 높이 이상으로 올라가면 따라감
    public float returnSpeed = 5f;  // 카메라가 원래 위치로 돌아오는 속도

    private Vector3 initialPosition; // 카메라의 초기 위치
    private bool isFollowing = false; // 카메라가 따라가는 중인지 여부

    private void Start()
    {
        // 카메라의 초기 위치 저장
        initialPosition = transform.position;
    }

    private void LateUpdate()
    {
        if (player.position.y > followThreshold)
        {
            // 플레이어가 일정 높이 이상으로 올라가면 카메라가 따라감
            isFollowing = true;
        }
        else if (player.position.y <= followThreshold)
        {
            // 플레이어가 다시 일정 높이 이하로 내려오면 카메라가 초기 위치로 돌아감
            isFollowing = false;
        }

        if (isFollowing)
        {
            // 카메라가 플레이어를 따라감
            Vector3 targetPosition = new Vector3(transform.position.x, player.position.y + offsetY, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * returnSpeed);
        }
        else
        {
            // 카메라가 초기 위치로 부드럽게 돌아옴
            transform.position = Vector3.Lerp(transform.position, initialPosition, Time.deltaTime * returnSpeed);
        }
    }
}