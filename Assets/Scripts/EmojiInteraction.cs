using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// public class EmojiInteraction : MonoBehaviour
// {
//     public UnityEvent onPlayerEnter = new UnityEvent();
//     public UnityEvent onPlayerExit = new UnityEvent();

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))  // Check if the collider is the player
//         {
//             onPlayerEnter.Invoke();
//         }
//     }

//     void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.CompareTag("Player"))
//         {
//             onPlayerExit.Invoke();
//         }
//     }
// }

//using UnityEngine;

public class EmojiInteraction : MonoBehaviour
{
    public System.Action onPlayerEnter;
    public System.Action onPlayerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the collider is the player
        {
            onPlayerEnter?.Invoke(); // Invoke the enter action
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the collider is the player
        {
            onPlayerExit?.Invoke(); // Invoke the exit action
        }
    }
}
