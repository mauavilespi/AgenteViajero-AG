using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Individuo2{ 
    public Vector3[] Nodos; //Lista de vectores para representar los nodos 
    public float Puntaje; 
}


public class AG : MonoBehaviour{
    public Transform[] Nodos;

    public List<Individuo2> poblacion = new List<Individuo2>(); //Lista de individuos 

    public int Iteraciones = 50;//número de iteraciones
    IEnumerator Start() 
    {
        lr = GetComponent<LineRenderer>();
        Iniciar();
        for (int i = 0; i < Iteraciones; i++)
        {
            Probar(); //Prueba las distancias recorridas
            Organizar(); //Organiza el recorrido 
            yield return new WaitForSeconds(0.2f); //Espera 0,2 segundos para poder mostrar el resultado
            Mostrar(); //Muestra el camino más optimo
            Combinar();
        }
        print("FIN");
        print("Distancia Minima: " + 1f/poblacion[0].Puntaje);
    }
    LineRenderer lr;
    
    
    //FUNCIÓN MOSTRAR
    void Mostrar() 
    {
        lr.positionCount = Nodos.Length+1;
        lr.SetPositions(poblacion[0].Nodos); //posiciones de los nodos 
        lr.SetPosition(Nodos.Length, poblacion[0].Nodos[0]); //pocicion final o posicion que cierra el camino   

    }

    public int CantidadDeIndividuos = 10;
    
    
    //FUNCIÓN INICIAR
    void Iniciar()
    {
        
        for (int i = 0; i < CantidadDeIndividuos; i++)
        {
            Individuo2 ind = new Individuo2(); //Por cada individuo se crea uno nuevo 

            List<Transform> usables = new List<Transform>();// Se crea una nueva lista usando los nodos
            usables.AddRange(Nodos);
            usables = DesordenarLista<Transform>(usables); //Desordena la lista

            List<Vector3> NodosIndividuo = new List<Vector3>(); //Se crea una lista vector

            for (int j = 0; j < usables.Count; j++) //transformamos la lista a un vector 
            {
                NodosIndividuo.Add(usables[j].position);
            }

            ind.Nodos = NodosIndividuo.ToArray();
            poblacion.Add(ind);
        }
    }
    List<T> DesordenarLista<T>(List<T> input)//Funcion para desordernar una lista 
    {
        List<T> arr = input;
        List<T> arrDes = new List<T>();

        while (arr.Count > 0) //Ciclo para desordenar la lista de manera aleatorea
        {
            int val = Random.Range(0, arr.Count - 1);
            arrDes.Add(arr[val]);
            arr.RemoveAt(val);
        }

        return arrDes;
    }
    
    
//FUNCIÓN PROBAR
    void Probar() 
    {
        for (int i = 0; i < poblacion.Count; i++)  //recorre cada individuo de la población 
        {
            float distancia = 0; //variable diastancia 
            for (int k = 0; k < Nodos.Length - 1; k++) //Se cuenta desde cada nodo hasta su proximo destino
            {
                distancia += Vector3.Distance(poblacion[i].Nodos[k], poblacion[i].Nodos[k + 1]); // Cuenta las distacias recorridas 
                print("Individuo: " + i + " Distancia: " + distancia);
            }
            Individuo2 ind = poblacion[i]; // Se crea un nuevo individuo
            ind.Puntaje = 1f / distancia; //Se saca el puntaje de cada distancia recorrida (NOTA: el puntajes es minimo cuando la distancia es mayor)
            poblacion[i] = ind;// se le asigna el puntaje
            print("**Individuo: " + i + "** -> Distancia final= " + poblacion[i].Puntaje);
        }
    }
    
    //FUNCIÓN ORGANIZAR
    void Organizar() 
    {
        poblacion.Sort((s1, s2) => s1.Puntaje.CompareTo(s2.Puntaje));
        poblacion.Reverse();
        for(int i = 0; i<=poblacion.Count-1; i++) {
            print("Posición " + i + " = " + 1f/poblacion[i].Puntaje);
        }
    }

    [Range(0,100)]
    public int RatioDeMantenimiento = 40;
    [Range(0,100)]
    public int ProbabilidadDeMutacion = 20;
    void Combinar()
    {
        List<Individuo2> supervivientes = BorrarInservibles();
        int count = supervivientes.Count;
        int faltantes = poblacion.Count - count;

        for (int i = 0; i < faltantes; i++)
        {   
            Individuo2 padre1 = supervivientes[Random.Range(0, count)]; //Se eligen dos padres de manera aleatorea
            Individuo2 padre2 = supervivientes[Random.Range(0, count)];

            Individuo2 hijo = new Individuo2(); //Se crea el hijo 
            hijo.Nodos = new Vector3[Nodos.Length];

            // 4231
            // 2413
            // 4231


            //Se elige una subruta del padre 1
            int Inicio = Random.Range(0, Nodos.Length - 2);
            int final = Random.Range(Inicio, Nodos.Length);
            for (int j = Inicio; j < final; j++)
            {
                hijo.Nodos[j] = padre1.Nodos[j]; //Se pone la subruta del padre dentro del hijo 
            }
            
            //Preparar padre 2
            List<Vector3> v = new List<Vector3>(); //se crea una lista
            v.AddRange(padre2.Nodos); //Se usan los nodos del padre 2 para la lista 
            for (int j = Inicio; j < final; j++)
            {
                v.Remove(padre1.Nodos[j]); //Elimina los nodos del padre 1 
            }

            //Llenar espacios
            int c = 0;
            for (int j = 0; j < Nodos.Length; j++) //Recorre las rutas 
            {
                if (hijo.Nodos[j] == Vector3.zero) //Confirmar si las rutas son igual a cero 
                {
                    hijo.Nodos[j] = v[c]; //Si son cero se cagaran en la lista del padre   
                    c++;
                }
            }          

            //Mutacion
            if (Random.Range(0, 100) <= ProbabilidadDeMutacion)
            {
                int g1 = Random.Range(0, Nodos.Length); //Se eligen 2 pares 
                int g2 = Random.Range(0, Nodos.Length);
                 
                //Se intercambian si hay mutación 
                Vector2 aux = hijo.Nodos[g1];
                hijo.Nodos[g1] = hijo.Nodos[g2]; 
                hijo.Nodos[g2] = aux;
            }

            supervivientes.Add(hijo);
        }

        poblacion = supervivientes;
    }
    List<Individuo2> BorrarInservibles()
    {
        List<Individuo2> Supervivientes = new List<Individuo2>();
        for (int i = 0; i < CantidadDeIndividuos * RatioDeMantenimiento / 100f; i++)
        {
            Supervivientes.Add(poblacion[i]);
        }
        return Supervivientes;
    }


}
/*
//Organizar mediante el método burbuja  
        bool sw = false;
        while (!sw)
        {
            sw = true;
            for (int i = 1; i < poblacion.Count; i++)
            {
                if (poblacion[i].Puntaje > poblacion[i - 1].Puntaje)
                {
                    Individuo2 ind = poblacion[i];
                    poblacion[i] = poblacion[i - 1];
                    poblacion[i - 1] = ind;
                    sw = false;
                }
            }
        }
*/