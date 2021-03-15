using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Instrucciones
{
    public class Exit : Instruccion
    {
        public Expresion regreso;

        public Exit(int linea, int columna, Expresion regreso)
        {
            this.linea = linea;
            this.columna = columna;
            this.regreso = regreso;
        }

        public Exit(int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
            this.regreso = null;
        }

        public override object ejecutar(Entorno ent)
        {
            //verificamos que si esta dentro de un Metodo o funcion
            if (MasterClass.Instance.EstoyDentroDeMF(MasterClass.TipoMF.Metodo_Funcion))
            {
                return this.regreso.getValor(ent);
            }
            else
            {
                MasterClass.Instance.addError(new C_Error("Semántico", "La sentencia return esta fuera de un metodo o funcion", linea, columna));
                return null;
            }
        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}
