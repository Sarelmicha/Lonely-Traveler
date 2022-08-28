using System;
using HappyFlow.LonelyTraveler.Player;
using HappyFlow.LonelyTraveler.World;
using Unity.VisualScripting;
using UnityEngine;

namespace HappyFlow.LonelyTraveler.World.Enemies
{
    public class ForceField : TriggerAbility
    {
        [SerializeField] private Vector2 m_Force;

        private void Update()
        {
            if (m_IsPlayerInsideCollider)
            {
                m_PlayerController.AddForce(m_Force);
            }
        }
    }   
}
