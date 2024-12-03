using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EventClickInterface
{
    void Interact(RaycastHit2D hit);
}

public interface EventInterface
{
    void InteractButton();
}
