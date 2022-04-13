using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using UnityEngine.SceneManagement;

public class PlayerCollisionHandler : MonoBehaviour
{
    public CityGenerator cityManager;

    private Vector3 logicForward;
    private Animator carAnimator;
    private BoxCollider playerCollider;

    private CarSound carSound;

    public GameObject rotationPivot;
    [Range(0.0f, 4.0f)]
    public float anguloRotDer;
    [Range(0.0f, 4.0f)]
    public float anguloRotIzq;
    // Start is called before the first frame update
    void Start()
    {
        logicForward = transform.forward;
        carAnimator = GetComponentInChildren<Animator>();
        playerCollider = GetComponent<BoxCollider>();
        carSound = GetComponentInChildren<CarSound>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Intersection inter = other.gameObject.GetComponent<Intersection>();
        hospitalTile hospital = other.gameObject.GetComponent<hospitalTile>();
        if (inter)
        {
            cityManager.EnteringIntersection(inter);
        }
        else if (hospital)
        {
            //jaja ganaste
            SceneManager.LoadSceneAsync("Victoria");
        }
    }

    public Vector3 getLogicF() {
        return logicForward;
    }

    public void setLogicF(Vector3 t) {
        logicForward = t;
    }

    public void logicFRotate(float angle ) {
        logicForward = Quaternion.AngleAxis(angle, transform.up) * logicForward;
    }

    public void rotatationTiltAnimation(Sentido s)
    {
        if (s == Sentido.Derecha)
        {
            carAnimator.SetTrigger("Derecha");
        }
        else carAnimator.SetTrigger("Izquierda");
        carSound.giroSound();
    }

    public void prepareRotation(Sentido s)
    {
        if (s == Sentido.Izquierda)
        {
            rotationPivot.transform.localPosition = transform.localPosition -anguloRotIzq * transform.right;
            transform.SetParent(rotationPivot.transform);
            playerCollider.enabled = false;
        }
        else if (s == Sentido.Derecha)
        {
            rotationPivot.transform.localPosition = transform.localPosition + anguloRotDer * transform.right;
            transform.SetParent(rotationPivot.transform);
            playerCollider.enabled = false;
        }
    }

    public void endRotation()
    {
        transform.SetParent(null);
        playerCollider.enabled = true;
    }
}
