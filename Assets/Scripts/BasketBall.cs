using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBall : MonoBehaviour
{

    private static Vector3 gravity = new Vector3(0f, -10f, 0f);

    public Vector3 velocity = Vector3.zero;
    [SerializeField] public bool launched = false;

    private CharacterController c;
    void Start()
    {
        c = GetComponent<CharacterController>();
        // velocity = new Vector3(0, 12, 3);
        // launched = true;
    }

    void Update()
    {
        if (!launched) return;
        velocity += gravity * Time.deltaTime;
        c.Move(velocity * Time.deltaTime);

        if (transform.position.y < -1)
        {
            Destroy(gameObject);
            PlayerManager.Instance.spawnBall();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Basket");
        PlayerManager.Instance.score += 1;
        PlayerManager.Instance.UpdateScore();
    }
}
