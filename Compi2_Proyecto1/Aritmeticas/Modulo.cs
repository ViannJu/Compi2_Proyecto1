using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Aritmeticas
{
    public class Modulo : Expresion
    {

        String operador;
        Expresion hijo1;
        Expresion hijo2;

        public Modulo(int linea, int columna, Expresion hijo1, Expresion hijo2)
        {
            this.linea = linea;
            this.columna = columna;
            this.hijo1 = hijo1;
            this.hijo2 = hijo2;
        }

        
        public override Expresion getValor(Entorno ent)
        {
            //declaramos un valor que por defecto sera error
            Primitivo resultadoGeneral = new Primitivo(new Tipo(Tipo.enumTipo.error), "@error@");
            Expresion resultadoIzq = this.hijo1.getValor(ent);
            Expresion resultadoDer = this.hijo2.getValor(ent);

            //Hijo izquierdo y dentro de cada uno se puede validar el hijo derecho (para saber con cual se esta sumando)
            switch (resultadoIzq.tipo.tipo)
            {
                case Tipo.enumTipo.entero:

                    switch (resultadoDer.tipo.tipo)
                    {
                        case Tipo.enumTipo.entero:
                            resultadoGeneral = new Primitivo(new Tipo(Tipo.enumTipo.entero), int.Parse(resultadoIzq.valor.ToString()) % int.Parse(resultadoDer.valor.ToString()));
                            return resultadoGeneral;

                        case Tipo.enumTipo.real:
                            resultadoGeneral = new Primitivo(new Tipo(Tipo.enumTipo.real), float.Parse(resultadoIzq.valor.ToString()) % float.Parse(resultadoDer.valor.ToString()));
                            return resultadoGeneral;

                        default:
                            MasterClass.Instance.addError(new C_Error("Semantico", "No se puede operar modulo: " + resultadoDer.tipo.tipo + " con: " + resultadoIzq.tipo.tipo, linea, columna));
                            break;
                    }

                    break;

                case Tipo.enumTipo.real:

                    switch (resultadoDer.tipo.tipo)
                    {
                        case Tipo.enumTipo.entero:
                            resultadoGeneral = new Primitivo(new Tipo(Tipo.enumTipo.real), float.Parse(resultadoIzq.valor.ToString()) % float.Parse(resultadoDer.valor.ToString()));
                            return resultadoGeneral;

                        case Tipo.enumTipo.real:
                            resultadoGeneral = new Primitivo(new Tipo(Tipo.enumTipo.real), float.Parse(resultadoIzq.valor.ToString()) % float.Parse(resultadoDer.valor.ToString()));
                            return resultadoGeneral;

                        default:
                            MasterClass.Instance.addError(new C_Error("Semantico", "No se puede operar modulo: " + resultadoDer.tipo.tipo + " con: " + resultadoIzq.tipo.tipo, linea, columna));
                            break;
                    }

                    break;

                default:
                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede operar modulo: " + resultadoDer.tipo.tipo + " con: " + resultadoIzq.tipo.tipo, linea, columna));
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
