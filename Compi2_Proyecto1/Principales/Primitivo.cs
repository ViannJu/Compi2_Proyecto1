using System;
using System.Collections.Generic;
using System.Text;

namespace Compi2_Proyecto1.Principales
{
    public class Primitivo : Expresion
    {
        public Primitivo(Tipo tipo, Object valor) {
            this.tipo = tipo;
            this.valor = valor;
        }

        public override Expresion getValor(Entorno ent)
        {
            throw new NotImplementedException();
        }

        public override string getDot()
        {
            throw new NotImplementedException();
        }

        
    }
}
