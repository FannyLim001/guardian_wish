using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 100f;
    public GameObject mc_boy;
    public GameObject mc_girl;
    Rigidbody2D rb;
    Vector2 movement;
    Animator animator;
    void Start()
    {
        string player_gender = PlayerPrefs.GetString("PlayerGender");

        if (player_gender == "mc boy")
        {
            mc_boy.SetActive(true);
            mc_girl.SetActive(false);
            rb = mc_boy.GetComponent<Rigidbody2D>();
            animator = mc_boy.GetComponent<Animator>();
        }
        else if (player_gender == "mc girl")
        {
            mc_boy.SetActive(false);
            mc_girl.SetActive(true);
            rb = mc_girl.GetComponent<Rigidbody2D>();
            animator = mc_girl.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
