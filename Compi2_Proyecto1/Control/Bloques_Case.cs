using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Control
{
    public class Bloques_Case : Bloque  
    {
        /*
         * Extendemos de Bloque para poder guardar Bloques_Case y Bloques_Default 
         */

        public Expresion Comparacion;
        public Bloque bloque;

        public Bloques_Case(Expresion Comparacion, LinkedList<Instruccion> listaInstrucciones)
        {
            this.Comparacion = Comparacion;
            this.listaInstrucciones = listaInstrucciones;
        }

        public Bloques_Case(Expresion Comparacion, Bloque bloque)
        {
            this.Comparacion = Comparacion;
            this.bloque = bloque;
            this.listaInstrucciones = this.bloque.listaInstrucciones;
        }

        public Bloques_Case(Expresion Comparacion)
        {
            this.Comparacion = Comparacion;
        }

        public Bloques_Case()
        {

        }

    }
}
