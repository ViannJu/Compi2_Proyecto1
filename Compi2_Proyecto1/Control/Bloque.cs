using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Compi2_Proyecto1.Principales;


namespace Compi2_Proyecto1.Control
{
    public class Bloque : Instruccion
    {

        LinkedList<Instruccion> listaInstrucciones = new LinkedList<Instruccion>();
        public Bloque()
        {
        }

        public Bloque(LinkedList<Instruccion> listaInstrucciones)
        {
            this.listaInstrucciones = listaInstrucciones;
        }

        public override object ejecutar(Entorno ent)
        {
            //MessageBox.Show("Entre al ejecutar de la clase Bloque");
            foreach (Instruccion instruccion in this.listaInstrucciones)
            {
                Object retornar = instruccion.ejecutar(ent);
                //es decir hubo una instruccion que devolvio algo... por ejemplo la clase break y continue
                if (retornar != null)
                {
                    return retornar;
                }
            }

            return null;
        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}
