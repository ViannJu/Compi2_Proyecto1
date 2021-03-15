using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Instrucciones
{
    public class Continue : Instruccion
    {
        public Continue(int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
        }

        public override object ejecutar(Entorno ent)
        {
            //verificamos que la instruccion continue este dentro de un ciclo
            if (MasterClass.Instance.EstoyDentroDeCiclo(MasterClass.TipoCiclo.Ciclo))
            {
                return this;
            }
            else
            {
                MasterClass.Instance.addError(new C_Error("Semántico", "Error, la sentencia Continue, viene afera de un ciclo o de un switch", linea, columna));
                return null;
            }
        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}
