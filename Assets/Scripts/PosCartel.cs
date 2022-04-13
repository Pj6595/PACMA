using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosCartel : MonoBehaviour
{
    /// <summary>
    /// Aquí va el empty colocado en la posición donde irá el poste
    /// </summary>
    [SerializeField] GameObject posicionCartel;

    public Vector3 GetPos() { return posicionCartel.transform.position; }
}
