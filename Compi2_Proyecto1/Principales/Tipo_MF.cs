using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Principales;
using Compi2_Proyecto1.Control;

namespace Compi2_Proyecto1.Principales
{
    public class Tipo_MF : Variable
    {

        LinkedList<Instruccion> parametros;
        LinkedList<Instruccion> listaInstrucciones;
        Bloque bloque;

        public Tipo_MF(Tipo tipo, Object valor) : base(tipo, valor)
        {

        }

        public void setTipo(LinkedList<Instruccion> parametros, Bloque bloque)
        {

            this.parametros = parametros;
            this.bloque = bloque;
        }

        public void setTipo(LinkedList<Instruccion> parametros, LinkedList<Instruccion> listaInstrucciones)
        {

            this.parametros = parametros;
            this.listaInstrucciones = listaInstrucciones;
        }

        public void setTipo(LinkedList<Instruccion> listaInstrucciones)
        {
            this.listaInstrucciones = listaInstrucciones;
        }

        public void setTipo(Bloque bloque)
        {
            this.bloque = bloque;
        }

        public Bloque getbloque()
        {
            return bloque;
        }

        public LinkedList<Instruccion> getparametros()
        {
            return parametros;
        }

    }
}
