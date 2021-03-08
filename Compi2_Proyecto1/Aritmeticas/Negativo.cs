using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Aritmeticas
{
    public class Negativo : Expresion
    {

        Expresion dato;

        public Negativo(Expresion dato, int linea, int columna)
        {
            this.dato = dato;
            this.linea = linea;
            this.columna = columna;
        }
                
        public override Expresion getValor(Entorno ent)
        {
            Primitivo resultadoN = new Primitivo(new Tipo(Tipo.enumTipo.error), "@error@");

            Expresion resultado = dato.getValor(ent);

            switch (resultado.tipo.tipo)
            {
                case Tipo.enumTipo.entero:
                    resultadoN = new Primitivo(new Tipo(Tipo.enumTipo.entero), -int.Parse(resultado.valor.ToString()));
                    return resultadoN;
                case Tipo.enumTipo.real:
                    resultadoN = new Primitivo(new Tipo(Tipo.enumTipo.real), -float.Parse(resultado.valor.ToString()));
                    return resultadoN;
                /*
                case caracter:
                    int contrario = -resultado.valor.toString().charAt(0);
                    resultadoN = new Literal(new Tipo(Tipo.EnumTipo.caracter), contrario);
                    return resultadoN;
                */
            }

            //si llega aqui es porque ningun tipo fue compatible para volverlo negativo
            //entonces es error
            MasterClass.Instance.addError(new C_Error("Semantico", "No se le puede asignar signo negativo a: " + resultado.tipo.tipo, linea, columna));

            return resultadoN;
        }

        public override string getDot()
        {
            throw new NotImplementedException();
        }

    }
}
