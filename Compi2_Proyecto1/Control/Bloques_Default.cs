using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Control
{
    public class Bloques_Default : Bloque
    {
        Bloque bloque;

        public Bloques_Default(LinkedList<Instruccion> listaInstrucciones)
        {
            this.listaInstrucciones = listaInstrucciones;
        }

        public Bloques_Default(Bloque bloque)
        {
            this.bloque = bloque;
            this.listaInstrucciones = this.bloque.listaInstrucciones;
        }

        public Bloques_Default()
        {

        }

    }
}
