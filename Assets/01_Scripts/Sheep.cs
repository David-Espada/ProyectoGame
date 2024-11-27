
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Sheep : MonoBehaviour
{
    [Header("Stats")]
    public float maxLife;
    public float life;
    public float speed;
    public bool isDay;
    public float maxEatCapacity;
    public float eatSpeed;
    public float whoolOncarry;
    [Header("Rest")]
    public float restDelay;
    public float restSpeed;
    public float restTimer;
    public SheepState state;
    public SheepType type;
    [Header("References")]
    public Slider lifebar;
    public List<GameObject> WheatPlaces;
    public GameObject farm;
    [Header("Internal")]
    public Rigidbody body;
    public Animator animator;
    public WheatPlace targetWheatPlace;
    private void Start()
    {
        body = GetComponent<Rigidbody>();
        targetWheatPlace = null;
        life = maxLife;
        farm = GameObject.FindGameObjectWithTag("Farm");
    }
    private void Update()
    {
        if (isDay && targetWheatPlace == null )
        {
            gameObject.SetActive(true);
            ChangeState(SheepState.Eating);
        }
        else if (!isDay && targetWheatPlace != null)
        {
            ChangeState(SheepState.Sleeping);
        }
        
        switch (type)
        {
            case SheepType.Worker:
                switch (state)
                {
                    case SheepState.Resting:
                        Goto(farm.transform);
                        break;
                    case SheepState.Eating:
                        Goto(targetWheatPlace.transform);
                        if (whoolOncarry > maxEatCapacity)
                        {
                            whoolOncarry = maxEatCapacity;
                            ChangeState(SheepState.Resting);
                        }
                        break;
                    case SheepState.Sleeping:
                        Goto(farm.transform);
                        break;
                    default:
                        break;
                }
                break;
            case SheepType.Warrior:
                break;
            default:
                break;
        }
    }
    public void Goto(Transform target)
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
        if (distance <= 10)
        {
            body.velocity = Vector3.zero;
        }
    }
    public WheatPlace ChooseRandonWheatPlace()
    {
        List<WheatPlace> enabledPlaces = new List<WheatPlace>();
        foreach (GameObject w in WheatPlaces)
        {
            if (w.GetComponent<WheatPlace>().IsEnable && !w.GetComponent<WheatPlace>().IsOcuped)
            {
                enabledPlaces.Add(w.GetComponent<WheatPlace>());
            }
        }
        if (enabledPlaces.Count == 0)
        {
            Debug.Log("nai");
            return null;
        }

        var randomIndex = Random.Range(0, enabledPlaces.Count); // Genera un �ndice aleatorio.
        enabledPlaces[randomIndex].GetComponent<WheatPlace>().IsOcuped = true;
        enabledPlaces[randomIndex].GetComponent<WheatPlace>().sheep = this;
        return enabledPlaces[randomIndex]; // Devuelve el elemento aleatorio.
    }
    public void WalkFordWard()
    {
        Vector3 forwardMovement = transform.forward * speed; // Movimiento calculado en X y Z
        forwardMovement.y = body.velocity.y; // Mantener la velocidad actual en Y
        body.velocity = forwardMovement;
    }
    public void ChangeState(SheepState newState)
    {
        state = newState;
        switch (state)
        {
            case SheepState.Resting:
                targetWheatPlace = null;
                break;
            case SheepState.Eating:
                targetWheatPlace = ChooseRandonWheatPlace();
                break;
            case SheepState.Sleeping:
                targetWheatPlace.IsOcuped = false;
                targetWheatPlace.sheep = null;
                targetWheatPlace = null;
                break;
            default:
                break;
        }
    }
    public void Eat(float amount)
    {
        whoolOncarry += amount * eatSpeed;
    }
    public float LeaveWhool()
    {
        float collectedWhool = whoolOncarry;
        whoolOncarry = 0;
        return collectedWhool;
       
    }
}

public enum SheepState
{
    Resting,
    Eating,
    Sleeping
}
public enum SheepType
{
    Worker,
    Warrior
}
