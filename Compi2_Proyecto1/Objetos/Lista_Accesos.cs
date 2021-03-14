using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Principales;
using Compi2_Proyecto1.Expresiones;

namespace Compi2_Proyecto1.Objetos
{
    public class Lista_Accesos : Expresion
    {
        public LinkedList<Id> accesos = new LinkedList<Id>();

        public Lista_Accesos() { }//para capturar tipo

        public Lista_Accesos(LinkedList<Id> accesos)
        {
            this.accesos = accesos;
        }


        public override Expresion getValor(Entorno ent)
        {
            //creamos el objeto que vamos a devolver
            Expresion l = new Primitivo(new Tipo(Tipo.enumTipo.error), "@error@");

            //Creamos una variable de entorno para ir buscando en ellos
            //Iniciamos en el entorno actual
            Entorno entBuscar = ent;

            //recorremos la lista
            foreach (Id id in accesos)
            {
                //buscamos el objeto en el entorno que viene
                Expresion sim = id.getValor(entBuscar);

                //vamos preguntando si es el ultimo
                if (id == accesos.Last.Value)
                {
                    //si es el ultimo no debe ser un objeto
                    //retornamos el simbolo
                    l = new Primitivo(sim.tipo, sim.valor);
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
                    }
                    //Si es de tipo objeto nos metemos a su entorno y seguimos buscando
                    //entBuscar = ((Objecto)sim.valor).global;
                }
            }

            return l;
        }


        public override string getDot()
        {
            throw new NotImplementedException();
        }

    }
}
