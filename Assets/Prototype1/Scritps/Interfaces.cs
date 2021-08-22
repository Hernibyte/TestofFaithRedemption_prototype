using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proto1
{
    interface IHittable
    {
        void Hit(int amountDamage, float knockBackForce, Vector2 posAttacker);
    }
}
