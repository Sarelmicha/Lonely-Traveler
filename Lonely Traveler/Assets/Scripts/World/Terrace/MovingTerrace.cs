using System;
using System.Collections;
using System.Collections.Generic;
using HappyFlow.LonelyTraveler.Player;
using HappyFlow.LonelyTraveler.Utils;
using UnityEngine;

namespace HappyFlow.LonelyTraveler.World.Terrace
{
    public class MovingTerrace : TriggerAbility
    {
        [SerializeField] private Transform m_Target;
        [SerializeField] private float m_Speed;
        [SerializeField] private ShakeBehavior m_ShakeBehavior;
        private IMovementTweener m_MovementTweener;
        
        private void Awake()
        {
            m_MovementTweener = new DoTweenTweener();
        }
        
        private void Move()
        {
            m_MovementTweener.MoveTo(transform, m_Target.position, m_Speed, null, OnTargetReached);
        }

        private void OnTargetReached()
        {
            m_ShakeBehavior.Shake();
        }

        private bool IsReachedTarget()
        {
            return Vector2.Distance(transform.position, m_Target.position) < m_Speed * Time.deltaTime;
        }

        protected override void OnPlayerTriggerEnter2D(PlayerController playerController)
        {
            if (IsReachedTarget())
            {
                return;
            }

            Move();
        }
    }
  
}
