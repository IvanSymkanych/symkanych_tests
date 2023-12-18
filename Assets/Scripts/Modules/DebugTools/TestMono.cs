using System;
using System.Collections;
using Core.Game;
using Core.Service;
using Core.StateMachine;
using Helpers;
using Modules.Player;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using Zenject;
using Random = UnityEngine.Random;

namespace Modules.DebugTools
{ 
    public class TestMono : MonoBehaviour
    {
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
            Gizmos.DrawWireSphere (transform.position , 2);
        }
    }
}