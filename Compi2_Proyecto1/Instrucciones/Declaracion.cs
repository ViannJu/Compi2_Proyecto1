using Compi2_Proyecto1.Principales;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Compi2_Proyecto1.Instrucciones
{
    public class Declaracion : Instruccion
    {
        public Tipo tipo;
        string id;
        public LinkedList<string> IDS;
        public Expresion valor;

        public Declaracion() { }    //Solamente para capturar el tipo
        public Declaracion(Tipo tipo, String id, Expresion valor, int linea, int columna)
        {
            this.tipo = tipo;
            this.id = id;
            this.valor = valor;
            this.linea = linea;
            this.columna = columna;
        }

        public Declaracion(Tipo tipo, LinkedList<string> IDS, int linea, int columna)
        {
            this.tipo = tipo;
            this.IDS = IDS;
            this.valor = null;
            this.linea = linea;
            this.columna = columna;
        }



        public override Object ejecutar(Entorno ent)
        {
            //MessageBox.Show("Entre al ejecutar de la clase Declaracion");
            //si se le asigno un valor a la variable    ASIGNACION/DECLARACION
            if (this.valor != null)
            {
                Expresion resultado = this.valor.getValor(ent);

                Variable variable;

                switch (tipo.tipo) {
                    //en cada caso creamos una variable del tipo que nos pidieron y la guardamos en su entorno.

                    case Tipo.enumTipo.entero:

                        variable = new Variable(this.tipo, resultado.valor);
                        ent.insertar(id, variable, linea, columna, "La variable");
                        //MessageBox.Show("La variable "+id+" insertada en el entorno de tipo entero");
                        return null;

                    case Tipo.enumTipo.real:

                        variable = new Variable(this.tipo, resultado.valor);
                        ent.insertar(id, variable, linea, columna, "La variable");
                        //MessageBox.Show("La variable " + id + " insertada en el entorno de tipo real");
                        return null;

                    case Tipo.enumTipo.cadena:

                        variable = new Variable(this.tipo, resultado.valor);
                        ent.insertar(id, variable, linea, columna, "La variable");
                        //MessageBox.Show("La variable " + id + " insertada en el entorno de tipo cadena");
                        return null;

                    case Tipo.enumTipo.booleano:

                        variable = new Variable(this.tipo, resultado.valor);
                        ent.insertar(id, variable, linea, columna, "La variable");
                        //MessageBox.Show("La variable " + id + " insertada en el entorno de tipo booleano");
                        return null;
                }

                //si llega hasta aquí entonces hubo un error de tipos

            }
            //Declaracion sin valor (puede ser una lista o un solo ID) se le asigna un valor por defecto
            else
            {
                
                //todas las variables seran de un solo tipo, lo recojemos

                switch (tipo.tipo) {

                    case Tipo.enumTipo.entero:

                        //Para cada variable de la lista
                        foreach (string idVariable in this.IDS)
                        {
                            //La guardamos en el entorno con el valor por defecto
                            ent.insertar(idVariable, new Variable(this.tipo, 0), linea, columna, "La variable");
                        }

                        break;

                    case Tipo.enumTipo.cadena:

                        //Para cada variable de la lista
                        foreach (string idVariable in this.IDS)
                        {
                            //La guardamos en el entorno con el valor por defecto
                            ent.insertar(idVariable, new Variable(this.tipo, ' '), linea, columna, "La variable");
                        }

                        break;

                    case Tipo.enumTipo.booleano:

                        //Para cada variable de la lista
                        foreach (string idVariable in this.IDS)
                        {
                            //La guardamos en el entorno con el valor por defecto
                            ent.insertar(idVariable, new Variable(this.tipo, false), linea, columna, "La variable");
                        }

                        break;

                    case Tipo.enumTipo.real:

                        //Para cada variable de la lista
                        foreach (string idVariable in this.IDS)
                        {
                            //La guardamos en el entorno con el valor por defecto
                            ent.insertar(idVariable, new Variable(this.tipo, 0.0), linea, columna, "La variable");
                        }

                        break;

                    case Tipo.enumTipo.Objecto:

                        //Para cada variable de la lista
                        foreach (string idVariable in this.IDS)
                        {
                            //La guardamos en el entorno con el valor por defecto
                            ent.insertar(idVariable, new Variable(this.tipo, null), linea, columna, "La variable");
                        }

                        break;
                }

                



            }

            return null;
        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}
