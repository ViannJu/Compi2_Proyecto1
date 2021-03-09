using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Principales;
using Compi2_Proyecto1.Analizador;

namespace Compi2_Proyecto1.Control
{
    public class Condicion_If : Instruccion
    {

        Expresion Condicion;
        public bool ejecutado;
        Bloque instrucciones;

        public Condicion_If(Expresion Condicion, Bloque instrucciones)
        {
            this.Condicion = Condicion;
            this.instrucciones = instrucciones;
        }

        public override object ejecutar(Entorno ent)
        {
            ejecutado = false;
            Expresion valor = Condicion.getValor(ent);

            if (valor.tipo.tipo == Tipo.enumTipo.booleano)
            {
                bool condition = bool.Parse(valor.valor.ToString());
                if (condition)
                {

                    Entorno nuevo = new Entorno(ent, ent.global);    //
                    Object retornar = instrucciones.ejecutar(nuevo);
                    ejecutado = true;
                    if (retornar != null)
                    {
                        return retornar;
                    }
                }

            }
            else
            {
                MasterClass.Instance.addError(new C_Error("Semantico", "No se puede operar tipo de dato: " + valor.tipo.tipo + " como condicion ", linea, columna));
            }

            return null;
        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}
