using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Principales;
using Compi2_Proyecto1.Analizador;

namespace Compi2_Proyecto1.Instrucciones
{
    public class Break : Instruccion
    {

        public Break(int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
        }

        public override object ejecutar(Entorno ent)
        {
            //Validamos que cuando se ejecute un break, si estemos dentro de un ciclo
            if (MasterClass.Instance.EstoyDentroDeCiclo(MasterClass.TipoCiclo.Ciclo) || MasterClass.Instance.EstoyDentroDeCiclo(MasterClass.TipoCiclo.Switch))
            {
                return this;
            }
            else
            {
                MasterClass.Instance.addError(new C_Error("Semántico", "la sentencia break esta fuera de un ciclo o switch", linea, columna));
                return null;
            }
        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}
