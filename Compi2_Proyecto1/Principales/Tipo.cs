using System;
using System.Collections.Generic;
using System.Text;

namespace Compi2_Proyecto1.Principales
{
    /* Clase tipo normal para el manejo de los tipos de los valores primitivos
     *  
     */
    public class Tipo
    {

        public enumTipo tipo;   //para declarar tipos
        public string referencia;   //tipo referencia para cuando tenga un objeto

        //constructor cuando solo es primitivo
        public Tipo(enumTipo tipo)
        {
            this.tipo = tipo;
            this.referencia = "";
        }

        //constructor cuando es un objeto
        public Tipo(enumTipo tipo, String referencia)
        {
            this.tipo = tipo;
            this.referencia = referencia;
        }

        
        //setear un tipo
        public Tipo setTipo(enumTipo tipo)
        {
            this.tipo = tipo;
            return this;
        }

        public enumTipo getTipo() {
            return this.tipo;
        }

        public enum enumTipo { 
            //los tipos de primitivos
            entero, real, cadena, booleano, Void, Null, error, MF, Objecto 
        }

    }
}
