using UnityEngine;

public class JohnMovement : MonoBehaviour
{
    //prefab de la bala
    public GameObject BulletPrefab;
    public float JumpForce;
    public float Speed;
    //Rigi de john
    private Rigidbody2D Rigidbody2D;
    private float Horizontal;
    //variable para la tierra
    private bool Grounded;
    private Animator Animator;
    //variable para almacenar el tiempo en que se hizo el ultimo disparo
    private float LastShoot;
    //variable para la vida de john
    public int Health = 5;

    // Start is called before the first frame update
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //si oprime horizontal
        Horizontal = Input.GetAxisRaw("Horizontal");

        //if de validar pa donde mira john
        if (Horizontal < 0.0f) transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (Horizontal > 0.0f) transform.localScale = new Vector3(1.0f,1.0f,1.0f);

        //verifica si se mueve y si si vale true y se pone a correr
        Animator.SetBool("running",Horizontal != 0.0f);

        //pinta una linea roja desde john hasta el piso
        Debug.DrawRay(transform.position, Vector3.down* 0.1f, Color.red);
        //valida la posicion si salta desde el suelo o desde el aire
        if (Physics2D.Raycast(transform.position, Vector3.down, 0.1f))
            Grounded = true;
        else Grounded = false;

        //valida si se oprime la tecla espacio, que el tiempo actual sea mayor que el ultimo disparo
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > LastShoot + 0.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }
        //valida que se oprima la tecla arriba o w y que el salto sea desde el piso y no del cielo   
        if ((Input.GetKeyDown(KeyCode.W)|| Input.GetKeyDown(KeyCode.UpArrow))&& Grounded)
        {
            Jump();
        }
    }

    private void Jump()
    {
        //Funcion de salto
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    private void Shoot()
    {
        Vector3 direction;
        //posicion de la bala, donde este mirando john
        if (transform.localScale.x == 1) direction = Vector2.right;
        else direction = Vector2.left;
        //Disparo
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = new Vector2(Horizontal * Speed, Rigidbody2D.velocity.y);
    }

    public void Hit()
    {
        //funcion de vida
        Health -= 1;
        if (Health == 0) Destroy(gameObject);
    }

}
