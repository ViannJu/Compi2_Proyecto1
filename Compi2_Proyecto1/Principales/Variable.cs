using System;
using System.Collections.Generic;
using System.Text;

namespace Compi2_Proyecto1.Principales
{
    /* Clase normal Simbolo para el manejo de los simbolos que se guardaran en la
     * tabla de simbolos de cada ambito
     * 
     */
    public class Variable
    {

        public Tipo tipo;
        public Object valor;

        /**
         * @param tipo Tipo del símbolo, el enum Tipo está en la clase Tipo
         * @param valor Valor del símbolo
         */
        public Variable(Tipo tipo, Object valor)
        {
            this.tipo = tipo;
            this.valor = valor;
        }

        /*public LinkedList<Instruccion> getParametros(Variable sim)
        {
            if (sim instanceof Tipo_Metodo_Funcion){
                return ((Tipo_Metodo_Funcion)sim).getparametros();
            }else
            {
                System.out.println("no es");
                return null;
            }

        }
        */

    }
}
