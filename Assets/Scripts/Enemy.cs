using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Enemy {
    void damage( int value, int force, Vector2 direction );
}
