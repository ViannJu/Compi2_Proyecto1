using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Control
{
    public class Clase_IF : Instruccion
    {

        LinkedList<Condicion_If> listaCondiciones;
        Bloque Else;

        public Clase_IF(LinkedList<Condicion_If> listaCondiciones, Bloque Else)
        {
            this.listaCondiciones = listaCondiciones;
            this.Else = Else;
        }

        public Clase_IF(LinkedList<Condicion_If> listaCondiciones)
        {
            this.listaCondiciones = listaCondiciones;
            this.Else = null;
        }

        public override object ejecutar(Entorno ent)
        {
            foreach (Condicion_If condicion in listaCondiciones)
            {
                Object retornar = condicion.ejecutar(ent);
                if (condicion.ejecutado)
                {
                    if (retornar != null)
                    {
                        return retornar;
                    }
                    return retornar;
                }

            }
            //si llego aqui es porque no retorno en ninguna true
            if (Else != null)
            {
                Entorno ent1 = new Entorno(ent, ent.global);
                Object retornar = Else.ejecutar(ent1);  //aqui puede que sea new Entorno(ent.anterior, ent.global); para no tomar el del if
                if (retornar != null)
                {
                    return retornar;
                }
                return retornar;
            }
            return null;
        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}
