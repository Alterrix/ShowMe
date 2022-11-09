using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GentleCat.ScriptableObjects.Properties;
using GentleCat.ScriptableObjects.Sets;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float maxHp;
    public float hp;
    public float regenRate;
    public float hitDelay;
    public bool isInShrine;
    public GameObject deathScreen;
    public Image healthBar;
    public ShrineSet shrines;
    [SerializeField] private TransformVariable playerTransform;
    public CharacterController controller;
    public GameObject chaseSound;

    public float speed = 6f;

    private float gravity;

    private void Awake()
    {
        playerTransform.CurrentValue = transform;
        hp = maxHp;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (hitDelay >= 0)
            hitDelay -= Time.deltaTime;
        
        healthBar.fillAmount = hp / maxHp;
        healthBar.transform.parent.gameObject.SetActive(hp < maxHp);

        if (hp <= 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            enabled = false;
            deathScreen.SetActive(true);
            chaseSound.SetActive(false);
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude >= 0.1f)
        {
            Vector3 moveDir = new Vector3(horizontal, 0f, vertical);
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        foreach (Shrine shrine in shrines.Items.Where(shrine => shrine.IsInside(transform.position)))
        {
            if (hp < maxHp)
            {
                hp += regenRate * Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("test");
            if (hitDelay < 0)
            {
                hitDelay = 0.05f;
                hp--;
            }
        }
    }

    private void FixedUpdate()
    {
        gravity -= 9.81f * Time.fixedDeltaTime;
        controller.Move(new Vector3(0, gravity, 0));
        if (controller.isGrounded) 
        { 
                gravity = 0; 
        }
    }
}
