using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Principales;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Objetos;
using System.Windows.Forms;

namespace Compi2_Proyecto1.Instrucciones
{
    public class Declaracion_MF : Instruccion
    {

        /*
         * DECLARAR_MF.Rule = tfunction + ID + parIzquierdo + L_DECLARACIONES_MF + parDerecho + dospuntos + TIPO + ptcoma + L_INSTRUCCIONES_MF
                |tprocedure + ID + parIzquierdo + L_DECLARACIONES_MF + parDerecho + dospuntos + TIPO + ptcoma + L_INSTRUCCIONES_MF;
         */

        public String nombre;
        public LinkedList<Instruccion> listaDeclaraciones;
        public Tipo tipo;
        public LinkedList<Instruccion> listaInstruccionesMF;
        public Lista_Accesos id_;

        public Declaracion_MF(String nombre, LinkedList<Instruccion> listaDeclaraciones, Tipo tipo, LinkedList<Instruccion> listaInstruccionesMF, int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
            this.tipo = tipo;
            this.nombre = nombre;
            this.listaDeclaraciones = listaDeclaraciones;
            this.listaInstruccionesMF = listaInstruccionesMF;
        }

        public Declaracion_MF(String nombre, Tipo tipo, LinkedList<Instruccion> listaInstruccionesMF, int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
            this.tipo = tipo;
            this.nombre = nombre;
            this.listaDeclaraciones = null;
            this.listaInstruccionesMF = listaInstruccionesMF;
        }

        public Declaracion_MF(Lista_Accesos id_, LinkedList<Instruccion> listaDeclaraciones, Tipo tipo, LinkedList<Instruccion> listaInstruccionesMF, int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
            this.tipo = tipo;
            this.id_ = id_;
            this.listaDeclaraciones = listaDeclaraciones;
            this.listaInstruccionesMF = listaInstruccionesMF;
        }

        public Declaracion_MF(Lista_Accesos id_, Tipo tipo, LinkedList<Instruccion> listaInstruccionesMF, int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
            this.tipo = tipo;
            this.id_ = id_;
            this.listaDeclaraciones = null;
            this.listaInstruccionesMF = listaInstruccionesMF;
        }

        
        public override object ejecutar(Entorno ent)
        {
            String nombreEntorno = ent.nombreEntorno;

            //creamos una nueva variable
            Tipo_MF f = new Tipo_MF(tipo, this.nombre);

            //debemos guardarlo con caracter especial para diferenciarlo de variables
            String nuevoNombre = nombre + "#";

            //Si trae una listaDeclaraciones
            if (this.listaDeclaraciones != null)
            {

                f.setTipo(this.listaDeclaraciones, this.listaInstruccionesMF);

                //recorremos las declaraciones (parametros) 
                foreach (Instruccion parametro in this.listaDeclaraciones)
                {
                    var declaracionType = new Declaracion().GetType();
                    var parametroType = parametro.GetType();

                    //verificamos si es una declaracion para guardarlo en el nombre
                    if (parametroType == declaracionType)
                    {
                        foreach (String nombreParametro in ((Declaracion)parametro).IDS) {

                            nuevoNombre += ((Declaracion)parametro).tipo.tipo;  //si vienen dos IDS en lista, seguiran siendo del mismo tipo
                        }                        
                    }
                    else
                    {
                        //Si no era declaracion entonces es un error
                        MasterClass.Instance.addError(new C_Error("Semantico", "Se esperaba una declaracion de parametro para el metodo o funcion: " + nombre, linea, columna));

                    }
                }

            }
            else
            {

                //si no tiene parametros entonces solo seteamos la lista de instrucciones
                f.setTipo(this.listaInstruccionesMF);
            }

            Variable p = new Variable(tipo, f);

            switch (tipo.tipo)
            {                
                case Tipo.enumTipo.Void:
                    ent.insertar(nuevoNombre, p, linea, columna, "El procedimiento");
                    //MessageBox.Show("Insertando procedimiento");
                    break;
                default:
                    ent.insertar(nuevoNombre, p, linea, columna, "La funcion");
                    //MessageBox.Show("Insertando funcion");
                    break;
            }

            return null;

        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}

