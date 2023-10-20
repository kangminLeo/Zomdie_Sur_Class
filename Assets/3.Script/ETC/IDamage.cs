using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void OnDamage(float Damage, Vector3 hitPosition, Vector3 hitNormal);
}
