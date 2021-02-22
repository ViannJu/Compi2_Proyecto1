using System;
using System.Collections.Generic;
using System.Text;

namespace Compi2_Proyecto1.Principales
{
    /*  Clase simple solamente para retornar el valor primitivo y enviarlo a
     *  evaluador   (solo par encapsular)
     * 
     */

    public class ValorConstante : Expresion
    {
        Expresion valor;

        public ValorConstante(Expresion valor, int linea, int columna)
        {
            this.valor = valor;
        }

        public override Expresion getValor(Entorno ent)
        {
            return this.valor;   
        }

        public override string getDot()
        {
            throw new NotImplementedException();
        }

        
    }
}
