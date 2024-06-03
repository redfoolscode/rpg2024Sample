using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowScript : MonoBehaviour
{
    public float speed = 3.5f;

    void Start()
    {

    }

    void Update()
    {
        if (transform.rotation == Quaternion.Euler(new Vector3(0, 0, 90)))
        {
            transform.Translate((transform.up * -speed * Time.deltaTime));
        }
        if (transform.rotation == Quaternion.Euler(new Vector3(0, 0, -90)))
        {
            transform.Translate((transform.up * speed * Time.deltaTime));
        }
        if (transform.rotation == Quaternion.Euler(new Vector3(0, 0, 0)))
        {
            transform.Translate((transform.right * speed * Time.deltaTime));
        }
        if (transform.rotation == Quaternion.Euler(new Vector3(0, 0, -180)))
        {
            transform.Translate((transform.right * -speed * Time.deltaTime));
        }

        Destroy(gameObject, 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
