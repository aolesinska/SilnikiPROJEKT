using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    private int fruits = 0;

    [SerializeField] private Text fruitsText;
    [SerializeField] private AudioSource collectSoundEffect;
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Fruits"))
        {
            collectSoundEffect.Play();
            Destroy(collision.gameObject);
            fruits++;
            fruitsText.text = "Cherries: " + fruits;
        }
    }
    
}
