using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto1
{
    public class Enemy : MonoBehaviour, IHittable
    {
        void Start()
        {
            
        }
    
        void Update()
        {
            
        }

        public void Hit()
        {
            Destroy(gameObject);
        }
    }
}
