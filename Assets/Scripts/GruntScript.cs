using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntScript : MonoBehaviour
{
    public float seguir;
    public Transform John;
    public GameObject BulletPrefab;
    //variable para almacenar el tiempo en que se hizo el ultimo disparo
    private float LastShoot;
    private Animator Animator;
    //variable para la tierra
    private bool Grounded;

    private Rigidbody2D Rigidbody2D;
    public float JumpForce;

    private int Health = 3;

    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (John == null) return;

        if (Time.time > LastShoot + 2.25f)
        {
            Shoot();
            LastShoot = Time.time;
        }
        //Direccion de john
        Vector3 direction = John.position - transform.position;
        Vector3 rayoTierra;
        if (direction.x >= 0.0f)
        {
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            rayoTierra = new Vector3(transform.position.x + 0.02f, transform.position.y, transform.position.z);
        }
        else
        {
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
            rayoTierra = new Vector3(transform.position.x - 0.02f, transform.position.y, transform.position.z);
        }

        //pinta una linea roja desde john hasta el piso
        Debug.DrawRay(rayoTierra, Vector3.down * 0.1f, Color.red);

        if (Physics2D.Raycast(rayoTierra, Vector3.down, 0.1f))
            Run(direction);
        else
        {
            Animator.SetBool("running", false);
            transform.localScale = new Vector3(transform.localScale.x, 1.0f, 1.0f);
        }

    }
    private void Jump()
    {
        Rigidbody2D.AddForce(Vector2.up * JumpForce);
    }

    private void Run(Vector3 direction)
    {
        //verifica si se mueve y si si vale true y se pone a correr
        Animator.SetBool("running", true);

        if (direction.x <= 0.0f)
            transform.position = new Vector3(transform.position.x - seguir, transform.position.y, 1.0f);
        else 
            transform.position = new Vector3(transform.position.x + seguir, transform.position.y, 1.0f);
    }

    private void Shoot()
    {
        Debug.Log(Animator.GetBool("shot"));
        Animator.SetBool("shot", true);
        Vector3 direction = new Vector3(transform.localScale.x, 0.0f, 0.0f);
        GameObject bullet = Instantiate(BulletPrefab, transform.position + direction * 0.1f, Quaternion.identity);
        bullet.GetComponent<BulletScript>().SetDirection(direction);
    }

    public void Hit()
    {
        Health -= 1;
        if (Health == 0) Destroy(gameObject);
    }
}
