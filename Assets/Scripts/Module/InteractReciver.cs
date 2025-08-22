using System.Collections;
using System.Collections.Generic;
using UltEvents;
using UnityEngine;

public class InteractReciver : MonoBehaviour
{
    public UltEvent<Interacter> onInteractEvent_Start;
    // Start is called before the first frame update
   
    public void Interact(Interacter interacter)
    {
      //  Debug.Log("Interact");
        onInteractEvent_Start.Invoke(interacter);
    }
}
