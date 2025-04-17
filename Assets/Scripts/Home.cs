using UnityEngine;

public class Home : MonoBehaviour
{

    public GameObject frog;

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        frog.SetActive(true);
    }

    private void OnDisable()
    {
        frog.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") {
            
            enabled = true;

            //Frogger frogger = collision.GetComponent<Frogger>();
            //frogger.gameObject.SetActive(false);
            //frogger.Invoke(nameof(frogger.Respawn), 2f);

            FindObjectOfType<GameManager>().HomeOccupied();
        }
    }

  
}
