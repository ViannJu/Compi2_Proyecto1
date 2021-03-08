using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Aritmeticas
{
    public class Resta : Expresion
    {

        Expresion hijoIzq;
        Expresion hijoDer;

        public Resta(Expresion hijoIzq, Expresion hijoDer, int linea, int columna)
        {
            this.hijoIzq = hijoIzq;
            this.hijoDer = hijoDer;
            this.linea = linea;
            this.columna = columna;
        }

        
        public override Expresion getValor(Entorno ent)
        {
            //declaramos un valor que por defecto sera error
            Primitivo resultadoGeneral = new Primitivo(new Tipo(Tipo.enumTipo.error), "@error@");
            Expresion resultadoIzq = this.hijoIzq.getValor(ent);
            Expresion resultadoDer = this.hijoDer.getValor(ent);

            //Hijo izquierdo y dentro de cada uno se puede validar el hijo derecho (para saber con cual se esta sumando)
            switch (resultadoIzq.tipo.tipo)
            {
                case Tipo.enumTipo.entero:

                    switch (resultadoDer.tipo.tipo)
                    {
                        case Tipo.enumTipo.entero:
                            resultadoGeneral = new Primitivo(new Tipo(Tipo.enumTipo.entero), int.Parse(resultadoIzq.valor.ToString()) - int.Parse(resultadoDer.valor.ToString()));
                            return resultadoGeneral;

                        case Tipo.enumTipo.real:
                            resultadoGeneral = new Primitivo(new Tipo(Tipo.enumTipo.real), float.Parse(resultadoIzq.valor.ToString()) - float.Parse(resultadoDer.valor.ToString()));
                            return resultadoGeneral;

                        default:
                            MasterClass.Instance.addError(new C_Error("Semantico", "No se puede restar: " + resultadoDer.tipo.tipo + " con: " + resultadoIzq.tipo.tipo, linea, columna));
                            break;
                    }

                    break;

                case Tipo.enumTipo.real:

                    switch (resultadoDer.tipo.tipo)
                    {
                        case Tipo.enumTipo.entero:
                            resultadoGeneral = new Primitivo(new Tipo(Tipo.enumTipo.real), float.Parse(resultadoIzq.valor.ToString()) - float.Parse(resultadoDer.valor.ToString()));
                            return resultadoGeneral;

                        case Tipo.enumTipo.real:
                            resultadoGeneral = new Primitivo(new Tipo(Tipo.enumTipo.real), float.Parse(resultadoIzq.valor.ToString()) - float.Parse(resultadoDer.valor.ToString()));
                            return resultadoGeneral;

                        default:
                            MasterClass.Instance.addError(new C_Error("Semantico", "No se puede restar: " + resultadoDer.tipo.tipo + " con: " + resultadoIzq.tipo.tipo, linea, columna));
                            break;
                    }

                    break;

                default:
                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede restar: " + resultadoDer.tipo.tipo + " con: " + resultadoIzq.tipo.tipo, linea, columna));
                    break;
            }

            return resultadoGeneral;
        }

        public override string getDot()
        {
            throw new NotImplementedException();
        }


    }
}
