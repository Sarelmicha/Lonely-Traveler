using HappyFlow.LonelyTraveler.Player;
using HappyFlow.LonelyTraveler.Utils;
using UnityEngine;

namespace HappyFlow.LonelyTraveler.World
{
    /// <summary>
    /// This class responsible for follow the camera after the user
    /// </summary>
    public class FollowCamera : TriggerAbility
    {
        [SerializeField] private Transform m_UpperLimit;
        [SerializeField] private float m_FollowDuration;
        
        private IMovementTweener m_MovementTweener;
        private Vector3 m_InitialPosition;

        private void Awake()
        {
            m_MovementTweener = new DoTweenTweener();
            m_InitialPosition = transform.position;
        }

        protected override void OnPlayerTriggerExit2D(PlayerController playerController)
        {
            var targetPosition = new Vector3(transform.position.x, playerController.transform.position.y, transform.position.z);

            if (targetPosition.y <= m_InitialPosition.y)
            {
                targetPosition = m_InitialPosition;
            }

            if (targetPosition.y >= m_UpperLimit.position.y)
            {
                targetPosition = m_UpperLimit.position;
            }

            m_MovementTweener.MoveTo(transform, targetPosition, m_FollowDuration);
        }
    }
}