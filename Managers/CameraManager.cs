using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CinemachineVirtualCamera _cinemachine;

    void Update()
    {
        FindPlayer();
    }

    void FindPlayer()
    {
        if(_cinemachine == null)
        {
            var cam = FindObjectOfType<ActiveCamera>();
            if(cam == null) { return; }
            _cinemachine = cam.GetComponent<CinemachineVirtualCamera>();
            if (_cinemachine == null) { return; }
            var players = FindObjectsOfType<Player>();
            var player = players[0];
            _cinemachine.Follow = player.transform;
            _cinemachine.LookAt = player.transform;
        }
    }
}
