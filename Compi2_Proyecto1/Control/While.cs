using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Principales;
using Compi2_Proyecto1.Instrucciones;

namespace Compi2_Proyecto1.Control
{
    public class While : Instruccion
    {

        Expresion condicion;
        Bloque sentencias;

        public While(Expresion condicion, Bloque sentencias, int linea, int columna)
        {
            this.condicion = condicion;
            this.sentencias = sentencias;
            this.linea = linea;
            this.columna = columna;

        }

        public override object ejecutar(Entorno ent)
        {
            MasterClass.Display.AddLast(MasterClass.TipoCiclo.Ciclo);

            //obtenemos la expresion de condicion siempre y cuando sea booleana y sea true
            Expresion valor = condicion.getValor(ent);
            if (valor.tipo.tipo == Tipo.enumTipo.booleano)
            {

                //Si es booleana!
                Boolean condition = bool.Parse(valor.valor.ToString());

                //hacemos el ciclo while puramente
                while (condition)
                {

                    //Creamos su nuevo entorno (sirve para while anidados)
                    Entorno nuevo = new Entorno(ent);
                    Object retornar = sentencias.ejecutar(nuevo);   //se ejecutan todas las instrucciones de su bloque

                    if (retornar != null)
                    {
                        if (typeof(Break).IsInstanceOfType(retornar)) {

                            //si viene un break se detiene el flujo del ciclo
                            break;

                        } else if (typeof(Continue).IsInstanceOfType(retornar)) {

                            //aqui solo se debe continuar el ciclo
                            //continue;
                        } else if (typeof(Primitivo).IsInstanceOfType(retornar)) {

                            //Aqui devolvemos el valor del retorno
                            return retornar;
                        }
                    }

                    Expresion valor1 = condicion.getValor(nuevo);
                    condition = bool.Parse(valor1.valor.ToString());

                }

            }
            else { 

                MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + valor.tipo.tipo, linea, columna));
            }

            MasterClass.Display.RemoveLast();
            return null;

        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}
