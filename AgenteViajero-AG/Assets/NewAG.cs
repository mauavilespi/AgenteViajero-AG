using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Individuo{
    public Vector2[] Nodo; //XY para representar posición de los nodos 
    public float Puntaje;
}

public class NewAG : MonoBehaviour{
    public Transform[] Nodo;

    public List<Individuo> Poblacion = new List<Individuo>(); //Lista de individuos 

    //Generaciones ***Pedir mediante inputField***
    public int Generaciones = 0;

    //Individuos **Pedir mediante inputField***
    public int Num_Individuos = 0;

    IEnumerator Start(){
        lr = GetComponent<LineRenderer>();
        CrearPoblacion();
    }

    //Función Creación de población
    void CrearPoblacion() {
        for (int i = 0; i<Num_Individuos; i++) {
            Individuo ind = new Individuo();

        }
    }


    // Update is called once per frame
    void Update(){
        
    }
}
