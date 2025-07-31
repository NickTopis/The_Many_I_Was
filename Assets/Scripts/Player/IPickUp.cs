using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickup
{
    void ShowPrompt(bool show);
    void PickUp();
}
