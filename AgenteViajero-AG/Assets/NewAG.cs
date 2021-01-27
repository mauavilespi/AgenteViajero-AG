using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Individuo{
    public Vector3[] Nodos_Recorridos; //Lista de XYZ para representar posición de los nodos 
    public float Puntaje; //Distancia obtenida
    public float Fitness;
}

public class NewAG : MonoBehaviour{
    //Objetos
    public Transform[] Nodos;

    public List<Individuo> Poblacion = new List<Individuo>(); //Lista de individuos 

    //Generaciones ***Pedir mediante inputField***
    public int Generaciones = 0;

    //Individuos **Pedir mediante inputField***
    public int Num_Individuos = 0;

    IEnumerator Start(){
        line_ren = GetComponent<LineRenderer>();
        CrearPoblacion();
        for (int j=0; j<Generaciones; j++) {
            Aptitud();
            Ordenar();
            yield return new WaitForSeconds(0.2f); //Tiempo de espera antes de mostrar el resultado
            MostrarCaminos();
            Cruza();
        }
        print("Distancia Mínima: " + Poblacion[0].Puntaje);
        
    }
    LineRenderer line_ren;

    //Función Creación de población
    void CrearPoblacion() {
        for (int i = 0; i < Num_Individuos; i++) {
            Individuo ind = new Individuo(); //Creación de la estructura Individuo

            List<Transform> usables = new List<Transform>(); // Se crea una nueva lista de objetos
            usables.AddRange(Nodos); // Se agregan los Nodos (puntos)
            usables = OrdenAleatorio<Transform>(usables); // Pone los nodos de forma aleatoria

            List<Vector3> Nodos_Individuo = new List<Vector3>(); //Nueva lista de tipo Vector3 (X,Y,Z)

            for (int j = 0; j < usables.Count; j++) { //Pasamos los datos de los nodos aleatorios a una lista con sus datos de posición 
                Nodos_Individuo.Add(usables[j].position); //Agregamos las posiciones de los nodos
            }

            Nodos_Individuo.Add(usables[0].position); //Agregamos al final el nodo de inicio para hacer un ciclo

            ind.Nodos_Recorridos = Nodos_Individuo.ToArray(); //Agregamos en la estructura esta lista de posiciones de nodos aleatorios
            Poblacion.Add(ind); //Agregamos el individuo creado a la lista de la Población

        }
    }

    //Función Orden Aleatorio: Regresa una lista con los nodos recibidos en forma aleatoria
    List<T> OrdenAleatorio<T>(List<T> input){
        List<T> arr = input; //Parámetro de la función
        List<T> arrTMP = new List<T>(); //Lista temporal

        while (arr.Count > 0) { //Ciclo para desordenar la lista de manera aleatoria
            if (arr.Count == 1) { //Si es uno se agrega el elemento 0 de la lista
                arrTMP.Add(arr[0]);
                arr.RemoveAt(0);
                break;
            }
            int val = Random.Range(0, arr.Count - 1); 
            arrTMP.Add(arr[val]);
            arr.RemoveAt(val);
        }

        return arrTMP;
    }

    // Función aptitud para medir las distancias de los puntos
    void Aptitud() {
        for (int i=0; i<Poblacion.Count; i++) { //Recorremos cada individuo de la población
            float distancia = 0; //Variable donde se llevará el conteo de distancia entre los puntos
            for (int j=0; j<Nodos.Length; j++) { //Recorremos la cantidad de nodos que tiene nuestro programa
                distancia += Vector2.Distance(Poblacion[i].Nodos_Recorridos[j], Poblacion[i].Nodos_Recorridos[j + 1]);
                print("Individuo: " + i + " Distancia: " + distancia); //Imprimir cómo va sumándose la distancia
            }
            Individuo tmp = Poblacion[i];
            tmp.Puntaje = distancia;
            tmp.Fitness = 100 - distancia;
            Poblacion[i] = tmp;
            print("**Individuo: " + i + "** || Distancia final= " + Poblacion[i].Puntaje + "|| Resultado Aptitud = "+Poblacion[i].Fitness);
        }
    }

    //Función para ordenar los mejores resultados
    void Ordenar() {
        Poblacion.Sort((s1,s2) => s1.Puntaje.CompareTo(s2.Puntaje));
        for (int i = 0; i <= Poblacion.Count - 1; i++) {
            print("Posición " + i + " = " + Poblacion[i].Puntaje);
        }
    }


    //Función para mostrar caminos
    void MostrarCaminos() {
        line_ren.positionCount = Nodos.Length+1;
        line_ren.SetPositions(Poblacion[0].Nodos_Recorridos);
    }


    //Operación Selección
    void Cruza() {

    }

    // Update is called once per frame
    void Update(){
        
    }
}
