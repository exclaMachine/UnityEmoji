using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class EmojiInteraction : MonoBehaviour
{
    public System.Action onPlayerEnter;
    public System.Action onPlayerExit;

    //private TMP_InputField inputField;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the collider is the player
        {
            var gameController = FindObjectOfType<EmojiGameController>();
            gameController.m_answerChecker = this.GetComponent<EmojiAnswerChecker>();
            onPlayerEnter?.Invoke(); // Invoke the enter action
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the collider is the player
        {
            //var gameController = FindObjectOfType<EmojiGameController>();
            //gameController.m_answerChecker = this.GetComponent<EmojiAnswerChecker>();
            onPlayerExit?.Invoke(); // Invoke the exit action
        }
    }
}
