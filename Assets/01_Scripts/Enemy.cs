using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("stats")]
    public float life;
    public float speed;
    public float damage;
    public float attackDelay;
    public float attackSpeed;
    public EnemyType type;
    [Header("References")]
    public Farm farm;
    public Transform farmPosition;
    public Sheep trackedSheep;
    [Header("Internal")]
    public Rigidbody body;
    public float attackTimer;
    void Start()
    {
        body = GetComponent<Rigidbody>();
        farm = GameObject.FindGameObjectWithTag("FarmAttack").GetComponent<Farm>();
        farmPosition = GameObject.FindGameObjectWithTag("Farm").GetComponent<Transform>();
    }
    public void GotoAttack(Transform target)
    {
        // Calcula la direcci�n desde este objeto hacia el objetivo
        Vector3 direction = target.position - transform.position;

        // Aseg�rate de que la direcci�n sea horizontal (ignora el eje Y si no es necesario)
        direction.y = 0;

        // Solo rota si hay una direcci�n v�lida
        if (direction.sqrMagnitude > 0.01f)
        {
            // Calcula la rotaci�n hacia el objetivo
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Aplica la rotaci�n al GameObject
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        float distance = Vector3.Distance(transform.position, target.position);
        WalkFordWard();
        if (distance <= 15)
        {
            body.velocity = Vector3.zero;
            AttackFarm();
        }
    }
    public void WalkFordWard()
    {
        Vector3 forwardMovement = transform.forward * speed; // Movimiento calculado en X y Z
        forwardMovement.y = body.velocity.y; // Mantener la velocidad actual en Y
        body.velocity = forwardMovement;
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case EnemyType.SheepAttacker:
                break;
            case EnemyType.FarmAttacker:
                GotoAttack(farmPosition);
                break;
            default:
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

    }
    public void AttackFarm()
    {
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            farm.TakeDamage(damage);
        }
        else
        {
            attackTimer += Time.deltaTime *attackSpeed;
        }
        
    }
}
public enum EnemyType { SheepAttacker , FarmAttacker }

