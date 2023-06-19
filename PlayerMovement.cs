using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
        [Header("Movement Parameters")]
        [SerializeField] private float speed;
        [SerializeField] private float jumpPower;

        [Header("Coyote Time")]
        [SerializeField] private float coyoteTime; //How much time the player can hang in the air before jumping
        private float coyoteCounter; //How much time passed since the player ran off the edge
    public DialogueTrigger dialogueTrigger;
    private int jumpCounter;


        [Header("Layers")]
        [SerializeField] private LayerMask groundLayer;

        [Header("Sounds")]
        [SerializeField] private AudioClip jumpSound;

        private Rigidbody2D body;
        private Animator anim;
        private BoxCollider2D boxCollider;
        private float horizontalInput;

        private void Awake()
        {
            //Grab references for rigidbody and animator from object
            body = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider2D>();
    }

        private void Update()
        {
        if (CanMoveOrInteract() == false)
            return;

        horizontalInput = Input.GetAxis("Horizontal");
        //Flip player when moving left-right
        if (horizontalInput > 0.01f)
                transform.localScale = new Vector3((float)2.5, 4, 1);
        else if (horizontalInput < -0.01f)
                transform.localScale = new Vector3((float)-2.5, 4, 1);

            //Set animator parameters
            anim.SetBool("Run", horizontalInput != 0);
            anim.SetBool("grounded", isGrounded());

        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
                Jump();

            //Adjustable jump height
            if (Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0)
                body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);

            else
            {
                body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

                if (isGrounded())
                {
                    coyoteCounter = coyoteTime; //Reset coyote counter when on the ground
                }
                else
                    coyoteCounter -= Time.deltaTime; //Start decreasing coyote counter when not on the ground
            }
        }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NPC"))
        {
            // Знайти компонент DialogueTrigger серед дочірніх об'єктів NPC
            DialogueTrigger dialogueTrigger = collision.GetComponentInChildren<DialogueTrigger>();

            // Перевірити, чи знайдено компонент DialogueTrigger
            if (dialogueTrigger != null)
            {
                // Викликати метод OnTriggerEnter2D у скрипті DialogueTrigger
                dialogueTrigger.OnTriggerEnter2D(collision);
            }
        }
    }



    bool CanMoveOrInteract()
    {
        bool can = true;

        if (FindObjectOfType<InteractionSystem>().isExamining)
            can = false;
        if (FindObjectOfType<InventorySystem>().isOpen)
            can = false;

        return can;
    }

    private void Jump()
        {
            if (coyoteCounter <= 0 ) return;
            //If coyote counter is 0 or less  don't do anything
            else
            {
                if (isGrounded())
                    body.velocity = new Vector2(body.velocity.x, jumpPower);
                    
                else
                {
                    //If not on the ground and coyote counter bigger than 0 do a normal jump
                    if (coyoteCounter > 0)
                        body.velocity = new Vector2(body.velocity.x, jumpPower);
               
                }

                //Reset coyote counter to 0 to avoid double jumps
                coyoteCounter = 0;
            }
        }



        private bool isGrounded()
        {
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
            return raycastHit.collider != null;
        }
        public bool canAttack()
        {
            return isGrounded();
        }

}
