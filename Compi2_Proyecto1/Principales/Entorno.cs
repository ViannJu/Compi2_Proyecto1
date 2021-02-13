using System;
using System.Collections.Generic;
using System.Text;

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
        public Dictionary<string, Variable> tabla;

        public Entorno(Entorno anterior) {
            this.anterior = anterior;
            this.tabla = new Dictionary<string, Variable>();
        }

        public Entorno(Entorno anterior, Entorno global)
        {
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
                //Interfaz1.listaErrores.add(new C_Error("Semántico", cadenaerror + " '" + nombre + "' ya existe", linea, columna));
                //interpretecompi1.Interfaz1.jTextArea1.setText(interpretecompi1.InterpreteCompi1.interfaz.jTextArea1.getText() + "Error Semántico:" + cadenaerror + " '" + nombre + "' ya existe. Línea: " + linea + ", Columna: " + columna + "\n");
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

            //Interfaz1.listaErrores.add(new C_Error("Semantico", cadenaerror + " '" + nombre + "' NO existe", linea, columna));
            //interpretecompi1.Interfaz1.jTextArea1.setText(interpretecompi1.InterpreteCompi1.interfaz.jTextArea1.getText()+"Error Semántico: " + cadenaerror + " '" + nombre + "' NO existe. Línea: " + linea + " Columna: " + columna+"\n");
            return null;
        }

    }
}
