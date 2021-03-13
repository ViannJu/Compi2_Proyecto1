using System;
using System.Collections.Generic;
using System.Text;

namespace Compi2_Proyecto1.Principales
{
    public class C_Error
    {
        public String tipo;     //lexico, sintactico o semantico
        public String descripcion;    //descripcion del error
        public int linea;       //linea del error
        public int columna;     //columna del error

        public C_Error(String tipo, String descripcion, int linea, int columna)
        {
            this.tipo = tipo;
            this.descripcion = descripcion;
            this.linea = linea;
            this.columna = columna;
        }

        public String getTipo()
        {
            return tipo;
        }

        public String getDescripcionError()
        {
            return descripcion;
        }

        public string getlinea()
        {
            return linea.ToString();
        }

        public string getColumna()
        {
            return columna.ToString();
        }

    }
}
