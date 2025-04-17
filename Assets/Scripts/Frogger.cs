using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Processors;
public class Frogger : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;

    public Sprite idleSprite;
    public Sprite leapSprite;

    public Sprite frogDeath;

    private Vector3 spawnPoint;

    private float farthestRow;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawnPoint = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateCharacter(Quaternion.Euler(0f, 0f, 0f));
            Move(Vector3.up);

        }

        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            RotateCharacter(Quaternion.Euler(0f, 0f, 180f));
            Move(Vector3.down);
        }

        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            RotateCharacter(Quaternion.Euler(0f, 0f, 90f));
            Move(Vector3.left);
        }

        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            RotateCharacter(Quaternion.Euler(0f, 0f, -90f));
            Move(Vector3.right);
        }
    }

    private void Move(Vector3 direction)
    {
       

        Vector3 destination = transform.position + direction;

        Collider2D barrier = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Barrier"));

        Collider2D platform = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Platform"));

        Collider2D obstacle = Physics2D.OverlapBox(destination, Vector2.zero, 0f, LayerMask.GetMask("Obstacle"));

        if (barrier != null) {
            return;
        }

        if (platform != null) { 
            transform.SetParent(platform.transform);
        }
        else
        {
            transform.SetParent(null);
        }

        if (obstacle != null && platform == null) {

            transform.position = destination;
            Death();

        }
        else
        {
            // Coroutine

            if(destination.y > farthestRow)
            {
                farthestRow = destination.y;
                FindAnyObjectByType<GameManager>().AdvancedRow();
            }

            StartCoroutine(Leap(destination));
        }

         
    }

    private void RotateCharacter(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    private IEnumerator Leap(Vector3 destination) 
    {
        Vector3 startPosition = transform.position;

        float elapsedTime = 0f;
        float duration = 0.125f;

        spriteRenderer.sprite = leapSprite;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, destination, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = destination;

        spriteRenderer.sprite = idleSprite;
       
    }


    public void Death()
    {

        StopAllCoroutines();

        transform.rotation = Quaternion.identity;
        spriteRenderer.sprite = frogDeath;
        enabled = false;

        FindAnyObjectByType<GameManager>().Died();
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(enabled && 
           collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") &&
           transform.parent == null)
        {
            Death();

        }
    }


    public void Respawn()
    {
        StopAllCoroutines();

        transform.rotation = Quaternion.identity;
        transform.position = spawnPoint;
        farthestRow = spawnPoint.y;
        spriteRenderer.sprite = idleSprite;
        gameObject.SetActive(true);
        enabled = true;

    }
}
