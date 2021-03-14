using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Objetos;
using Compi2_Proyecto1.Expresiones;

namespace Compi2_Proyecto1.Principales
{
    /* Clase entorno normal para el manejo de los ámbitos 
     * 
     * Pendiente el manejo de errores
     */
    public class Entorno
    {
        public Entorno global;
        public Entorno anterior;
        public String nombreEntorno;
        public Dictionary<string, Variable> tabla;

        public Entorno(Entorno anterior)
        {
            this.nombreEntorno = "";
            this.anterior = anterior;
            this.tabla = new Dictionary<string, Variable>();
        }

        public Entorno(Entorno anterior, Entorno global)
        {
            this.nombreEntorno = "";
            this.anterior = anterior;
            this.global = global;
            this.tabla = new Dictionary<string, Variable>();
        }

        public Entorno(Entorno anterior, String nombreEntorno) {
            this.nombreEntorno = nombreEntorno;
            this.anterior = anterior;
            this.tabla = new Dictionary<string, Variable>();
        }

        public Entorno(Entorno anterior, Entorno global, String nombreEntorno)
        {
            this.nombreEntorno = nombreEntorno;
            this.anterior = anterior;
            this.global = global;
            this.tabla = new Dictionary<string, Variable>();
        }


        /**
         * @param nombre Nombre de la variable a insertar
         * @param Variable Símbolo que va a ser el valor de la variable
         * @param linea Línea donde se declara la variable para marcar un posible
         * error
         * @param columna Columna donde se declara la variable para marcar un
         * posible error
         * @param cadenaerror Cadena que me dice qué estoy insertando (La variable,
         * La funcion, El arreglo)
         */
        public void insertar(String nombre, Variable sim, int linea, int columna, String cadenaerror)
        {
            if (tabla.ContainsKey(nombre))
            {
                //Añadimos a la lista de errores
                MasterClass.Instance.addError(new C_Error("Semántico", cadenaerror + " '" + nombre + "' ya existe", linea, columna));
                return;
            }
            tabla.Add(nombre, sim);
        }


        public Variable buscar(String nombre, int linea, int columna, String cadenaerror)
        {

            for (Entorno e = this; e != null; e = e.anterior)
            {
                if (e.tabla.ContainsKey(nombre))
                {
                    Variable sim = e.tabla[nombre];
                    return sim;
                }
            }

            MasterClass.Instance.addError(new C_Error("Semantico", cadenaerror + " '" + nombre + "' NO existe", linea, columna));
            return null;
        }


        public Variable buscar(Lista_Accesos nombre, int linea, int columna, String cadenaerror)
        {


            //creamos el objeto que vamos a devolver
            Variable l = null;

            LinkedList<Id> aux = nombre.accesos;
            //Creamos una variable de entorno para ir buscando en ellos
            //Iniciamos en el entorno actual
            Entorno entBuscar = this;
            //recorremos la lista
            foreach (Id id in aux)
            {
                //buscamos el objeto en el entorno indicado
                Variable sim = id.getSimbolo(entBuscar);

                //vamos preguntando si es el ultimo
                if (id == aux.Last.Value)
                {
                    //si es el ultimo no debe ser un objeto
                    //retornamos el simbolo
                    l = sim;
                    break;
                }
                else
                {
                    //Como no es ultimo de la lista de ids
                    //Tiene que ser de tipo objeto
                    if (sim.tipo.tipo != Tipo.enumTipo.Objecto)
                    {
                        //Si es diferente de objeto no puede ser acceso
                        /*Error*/
                        MasterClass.Instance.addError(new C_Error("Semántico", "Fallo en variables de acceso al obtener el valor de: " + id, linea, columna));
                        

                        break;
                    }
                    //Si es de tipo objeto nos metemos a su entorno y seguimos buscando
                    //------------------------------entBuscar = ((Objeto)sim.valor).global;
                }
            }

            return l;
        }


        public Entorno getEntornoAcceso(Lista_Accesos nombre)
        {
            Entorno retorno = null;

            LinkedList<Id> aux = nombre.accesos;

            if (aux.Count > 1)
            {
                aux.RemoveLast(); // elimina el último elemento
            }

            Entorno busqueda = this;

            // recorrer todos los id
            foreach (Id id in aux)
            {
                Variable l = id.getSimbolo(busqueda); // obtiene el objeto en el entorno indicado
                                                     //aux.pollFirst();

                //verificar si es el último elemento
                if (id == aux.Last.Value)
                {
                    // estamos en el ùltimo id
                    // no debe ser un objeto
                    //-----------------------retorno = ((Tipo.enumTipo.Objecto)l.valor).global; // retorno del ojeto final
                    break;
                }
                else
                {
                    // no es el último id
                    // verifica sea de tipo objeto
                    if (l.tipo.tipo != Tipo.enumTipo.Objecto)
                    {
                        // error
                        MasterClass.Instance.addError(new C_Error("Semántico", "Fallo en variables de acceso al obtener el valor de: " + id, 0, 0));
                        break;
                    }

                    // sí es de tipo objeto, ahora vamos a buscar en el entorno del objeto
                    //------------------------busqueda = ((Objeto)l.valor).global;
                }

            }

            return retorno;
        }

    }
}
