using Compi2_Proyecto1.Principales;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compi2_Proyecto1.Aritmeticas
{
    public class Suma : Expresion
    {
        Expresion hijoIzq;
        Expresion hijoDer;

        public Suma(Expresion hijoIzq, Expresion hijoDer, int linea, int columna) {
            this.hijoIzq = hijoIzq;
            this.hijoDer = hijoDer;
            this.linea = linea;
            this.columna = columna;
        }

        //resolvemos la operacion y devolvemos que valor nos dio
        public override Expresion getValor(Entorno ent)
        {
            //declaramos un valor que por defecto sera error
            Primitivo resultadoSuma = new Primitivo(new Tipo(Tipo.enumTipo.error), "@error@");
            Expresion resultadoIzq = this.hijoIzq.getValor(ent);
            Expresion resultadoDer = this.hijoDer.getValor(ent);

            //Hijo izquierdo y dentro de cada uno se puede validar el hijo derecho (para saber con cual se esta sumando)
            switch (resultadoIzq.tipo.tipo) {
                case Tipo.enumTipo.entero:

                    switch (resultadoDer.tipo.tipo) {
                        case Tipo.enumTipo.entero:
                            resultadoSuma = new Primitivo(new Tipo(Tipo.enumTipo.entero), int.Parse(resultadoIzq.valor.ToString()) + int.Parse(resultadoDer.valor.ToString()));
                            return resultadoSuma;

                        default:
                            //entonces es un error semantico
                            break;
                    }

                    break;

                case Tipo.enumTipo.real:

                    switch (resultadoDer.tipo.tipo)
                    {
                        case Tipo.enumTipo.real:
                            resultadoSuma = new Primitivo(new Tipo(Tipo.enumTipo.real), float.Parse(resultadoIzq.valor.ToString()) + float.Parse(resultadoDer.valor.ToString()));
                            return resultadoSuma;

                        default:
                            //entonces es un error semantico
                            break;
                    }

                    break;

                default:
                    //Error semantico
                    break;
            }

            return resultadoSuma;
        }


        public override string getDot()
        {
            throw new NotImplementedException();
        }

        
    }
}
