using System.Xml.Linq;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player)")) ;
        {
            Debug.Log("Key picked up");

            Gate.SetActive(false);
        }
    }
}
