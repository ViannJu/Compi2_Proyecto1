using System;
using System.Collections.Generic;
using System.Text;

namespace Compi2_Proyecto1.Principales
{
    /* Clase expresion para trabajar todas las clases que devuelven un valor
     * (aritmeticas, logicas, primitivos, etc)
     * 
     */
    public abstract class Expresion : Nodo
    {
        public Tipo tipo;
        public Object valor;

        //funcion abstracta para obtener el valor de cualquier expresion
        public abstract Expresion getValor(Entorno ent);

        //funcion abstracta para gestionar el stringDot de cada expresion
        public abstract string getDot();

    }
}
