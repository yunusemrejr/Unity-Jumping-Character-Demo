using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   [SerializeField] private float speed;
   [SerializeField] private LayerMask groundLayer;
   [SerializeField] private LayerMask wallLayer;

   private Rigidbody2D body;
   private Animator anim;
   private BoxCollider2D boxCollider;
   private float wallJumpCooldown;

   private void Awake()
   {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
   }

   private void Update()
   {
       float horizontalInput = Input.GetAxis("Horizontal");
       body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
       
       if(horizontalInput > 0.01f)
       transform.localScale = Vector3.one;

        else if(horizontalInput < -0.01f)
       transform.localScale = new Vector3(-1, 1, 1);

       if(Input.GetKey(KeyCode.Space))
       body.velocity = new Vector2(body.velocity.x, speed);

       if(wallJumpCooldown > 0.2f)
       {
          if(Input.GetKey(KeyCode.Space)&& isGrounded())
          {
              Jump();

              body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

              if(onWall() && !isGrounded())
              { 
                 body.gravityScale = 0; 
                 body.velocity = Vector2.zero;
              }
          }
       }
   }

   private void Jump()
   {
       body.velocity = new Vector2(body.velocity.x,speed);
       anim.SetTrigger("jump");
   }
   private bool isGrounded()
   {
       RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
       return raycastHit.collider != null;
   }


      private bool onWall()
   {
       RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
       return raycastHit.collider != null;
   }
}
