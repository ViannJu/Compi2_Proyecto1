using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Relacionales
{
    public class Relacional : Expresion
    {

        String operacion;   //para guardar el simbolo de la operacion
        Expresion hijo1;    //rama izquierda de la operacion
        Expresion hijo2;    //rama derecha de la operacion

        public Relacional(Expresion hijo1, Expresion hijo2, String operacion, int linea, int columna)
        {

            this.operacion = operacion;
            this.linea = linea;
            this.columna = columna;
            this.hijo1 = hijo1;
            this.hijo2 = hijo2;

        }
                
        public override Expresion getValor(Entorno ent)
        {
            Primitivo resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.error), "@error@");
            Expresion resultado1 = hijo1.getValor(ent);
            Expresion resultado2 = hijo2.getValor(ent);
            int comparador1, comparador2;
            double comparador3, comparador4;

            switch (this.operacion)
            {
                case ">":
                    switch (resultado1.tipo.tipo)
                    {
                        case Tipo.enumTipo.entero:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.entero:
                                    comparador1 = int.Parse(resultado1.valor.ToString());
                                    comparador2 = int.Parse(resultado2.valor.ToString());
                                    if (comparador1 > comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.real:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 > comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                /*
                                case caracter:
                                    comparador1 = int.Parse(resultado1.valor.ToString());
                                    comparador2 = (int)(resultado2.valor.ToString().charAt(0));
                                    if (comparador1 > comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                */
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;


                        case Tipo.enumTipo.real:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.entero:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 > comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.real:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 > comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                /*
                                    case caracter:
                                        comparador3 = float.Parse(resultado1.valor.ToString());
                                        comparador4 = (double)(resultado2.valor.ToString().charAt(0));
                                        if (comparador3 > comparador4)
                                        {
                                            resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                        }
                                        else
                                        {
                                            resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                        }
                                        return resultadoMayor;
                                */
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;

                        /*
                        case caracter:
                            switch (resultado2.tipo.tipo)
                            {
                                case entero:
                                    comparador1 = (int)(resultado1.valor.ToString().charAt(0));
                                    comparador2 = int.Parse(resultado2.valor.ToString());
                                    if (comparador1 > comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case doble:
                                    comparador3 = (double)(resultado1.valor.ToString().charAt(0));
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 > comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case caracter:
                                    comparador1 = (int)(resultado1.getValor(ent).valor.ToString().charAt(0));
                                    comparador2 = (int)(resultado2.getValor(ent).valor.ToString().charAt(0));
                                    if (comparador1 > comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        */
                    }
                    break;


                case ">=":
                    switch (resultado1.tipo.tipo)
                    {
                        case Tipo.enumTipo.entero:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.entero:
                                    comparador1 = int.Parse(resultado1.valor.ToString());
                                    comparador2 = int.Parse(resultado2.valor.ToString());
                                    if (comparador1 >= comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.real:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 >= comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                /*
                                case caracter:
                                    comparador1 = int.Parse(resultado1.valor.ToString());
                                    comparador2 = (int)(resultado2.valor.ToString().charAt(0));
                                    if (comparador1 >= comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                */
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        case Tipo.enumTipo.real:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.entero:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 >= comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.real:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 >= comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                /*
                                case caracter:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = (double)(resultado2.valor.ToString().charAt(0));
                                    if (comparador3 >= comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                */
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        /*
                        case caracter:
                            switch (resultado2.tipo.tipo)
                            {
                                case entero:
                                    comparador1 = (int)(resultado1.valor.ToString().charAt(0));
                                    comparador2 = int.Parse(resultado2.valor.ToString());
                                    if (comparador1 >= comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case doble:
                                    comparador3 = (double)(resultado1.valor.ToString().charAt(0));
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 >= comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case caracter:
                                    comparador1 = (int)(resultado1.getValor(ent).valor.ToString().charAt(0));
                                    comparador2 = (int)(resultado2.getValor(ent).valor.ToString().charAt(0));
                                    if (comparador1 >= comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        */
                    }
                    break;


                case "<":
                    switch (resultado1.tipo.tipo)
                    {
                        case Tipo.enumTipo.entero:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.entero:
                                    comparador1 = int.Parse(resultado1.valor.ToString());
                                    comparador2 = int.Parse(resultado2.valor.ToString());
                                    if (comparador1 < comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.real:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 < comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                /*
                                case caracter:
                                    comparador1 = int.Parse(resultado1.valor.ToString());
                                    comparador2 = (int)(resultado2.valor.ToString().charAt(0));
                                    if (comparador1 < comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                */
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        case Tipo.enumTipo.real:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.entero:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 < comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.real:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 < comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                /*
                                case caracter:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = (double)(resultado2.valor.ToString().charAt(0));
                                    if (comparador3 < comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                */
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        /*
                        case caracter:
                            switch (resultado2.tipo.tipo)
                            {
                                case entero:
                                    comparador1 = (int)(resultado1.valor.ToString().charAt(0));
                                    comparador2 = int.Parse(resultado2.valor.ToString());
                                    if (comparador1 < comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case doble:
                                    comparador3 = (double)(resultado1.valor.ToString().charAt(0));
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 < comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case caracter:
                                    comparador1 = (int)(resultado1.getValor(ent).valor.ToString().charAt(0));
                                    comparador2 = (int)(resultado2.getValor(ent).valor.ToString().charAt(0));
                                    if (comparador1 < comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        */
                    }
                    break;


                case "<=":
                    switch (resultado1.tipo.tipo)
                    {
                        case Tipo.enumTipo.entero:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.entero:
                                    comparador1 = int.Parse(resultado1.valor.ToString());
                                    comparador2 = int.Parse(resultado2.valor.ToString());
                                    if (comparador1 <= comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.real:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 <= comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                /*
                                case caracter:
                                    comparador1 = int.Parse(resultado1.valor.ToString());
                                    comparador2 = (int)(resultado2.valor.ToString().charAt(0));
                                    if (comparador1 <= comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                */
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        case Tipo.enumTipo.real:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.entero:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 <= comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.real:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 <= comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                /*
                                case caracter:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = (double)(resultado2.valor.ToString().charAt(0));
                                    if (comparador3 <= comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                */
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        /*
                        case caracter:
                            switch (resultado2.tipo.tipo)
                            {
                                case entero:
                                    comparador1 = (int)(resultado1.valor.ToString().charAt(0));
                                    comparador2 = int.Parse(resultado2.valor.ToString());
                                    if (comparador1 <= comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case doble:
                                    comparador3 = (double)(resultado1.valor.ToString().charAt(0));
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 <= comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case caracter:
                                    comparador1 = (int)(resultado1.getValor(ent).valor.ToString().charAt(0));
                                    comparador2 = (int)(resultado2.getValor(ent).valor.ToString().charAt(0));
                                    if (comparador1 <= comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        */
                    }
                    break;

                case "=":
                    switch (resultado1.tipo.tipo)
                    {
                        case Tipo.enumTipo.entero:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.entero:
                                    comparador1 = int.Parse(resultado1.valor.ToString());
                                    comparador2 = int.Parse(resultado2.valor.ToString());
                                    if (comparador1 == comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.real:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 == comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                /*
                                case caracter:
                                    comparador1 = int.Parse(resultado1.valor.ToString());
                                    comparador2 = (int)(resultado2.valor.ToString().charAt(0));
                                    if (comparador1 == comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                */
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;


                        case Tipo.enumTipo.real:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.entero:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 == comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.real:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 == comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                /*
                                case caracter:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = (double)(resultado2.valor.ToString().charAt(0));
                                    if (comparador3 == comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                */
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;

                        /*
                        case caracter:
                            switch (resultado2.tipo.tipo)
                            {
                                case entero:
                                    comparador1 = (int)(resultado1.valor.ToString().charAt(0));
                                    comparador2 = int.Parse(resultado2.valor.ToString());
                                    if (comparador1 == comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case doble:
                                    comparador3 = (double)(resultado1.valor.ToString().charAt(0));
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 == comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case caracter:
                                    comparador1 = (int)(resultado1.getValor(ent).valor.ToString().charAt(0));
                                    comparador2 = (int)(resultado2.getValor(ent).valor.ToString().charAt(0));
                                    if (comparador1 == comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        */


                        case Tipo.enumTipo.cadena:
                            if (resultado2.tipo.tipo == Tipo.enumTipo.cadena)
                            {
                                if (resultado1.valor.ToString().Equals(resultado2.valor.ToString()))
                                {
                                    resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                }
                                else
                                {
                                    resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                }
                                return resultadoMayor;
                            }
                            break;


                        case Tipo.enumTipo.booleano:
                            if (resultado2.tipo.tipo == Tipo.enumTipo.booleano)
                            {
                                if (resultado1.valor.ToString().Equals(resultado2.valor.ToString()))
                                {
                                    resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                }
                                else
                                {
                                    resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                }
                                return resultadoMayor;
                            }
                            break;

                        case Tipo.enumTipo.Objecto:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.Objecto:
                                    //Si el primero es Object y el segundo Object --> tiene que valer exactamente lo mismo
                                    if (resultado1.valor == resultado2.valor)
                                    {
                                        //entonces devolvemos verdadero
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        //de lo contrario devolvemos false
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    break;
                                case Tipo.enumTipo.Null:
                                    //Si el primero es Object y el segundo Null
                                    if (resultado1.valor == null)
                                    {
                                        //entonces devolvemos verdadero
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        //de lo contrario devolvemos false
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                            }
                            break;
                        case Tipo.enumTipo.Null:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.Objecto:
                                    //Si el primero es Null y el segundo Object
                                    if (resultado2.valor == null)
                                    {
                                        //entonces devolvemos verdadero
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        //de lo contrario devolvemos false
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    break;
                                case Tipo.enumTipo.Null:
                                    //Si el primero es Null y el segundo Null  -> tienen que valer exactamente lo mismo
                                    if (resultado1.tipo.tipo == resultado2.tipo.tipo)
                                    {
                                        //entonces devolvemos verdadero
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        //de lo contrario devolvemos false
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    break;
                            }
                            break;

                        default:
                            MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                            break;


                    }
                    break;



                case "!=":
                    switch (resultado1.tipo.tipo)
                    {
                        case Tipo.enumTipo.entero:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.entero:
                                    comparador1 = int.Parse(resultado1.valor.ToString());
                                    comparador2 = int.Parse(resultado2.valor.ToString());
                                    if (comparador1 != comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.real:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 != comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                /*
                                case caracter:
                                    comparador1 = int.Parse(resultado1.valor.ToString());
                                    comparador2 = (int)(resultado2.valor.ToString().charAt(0));
                                    if (comparador1 != comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                */
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        case Tipo.enumTipo.real:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.entero:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 != comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.real:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 != comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                /*
                                case caracter:
                                    comparador3 = float.Parse(resultado1.valor.ToString());
                                    comparador4 = (double)(resultado2.valor.ToString().charAt(0));
                                    if (comparador3 != comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                */
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        /*
                        case caracter:
                            switch (resultado2.tipo.tipo)
                            {
                                case entero:
                                    comparador1 = (int)(resultado1.valor.ToString().charAt(0));
                                    comparador2 = int.Parse(resultado2.valor.ToString());
                                    if (comparador1 != comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case doble:
                                    comparador3 = (double)(resultado1.valor.ToString().charAt(0));
                                    comparador4 = float.Parse(resultado2.valor.ToString());
                                    if (comparador3 != comparador4)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case caracter:
                                    comparador1 = (int)(resultado1.getValor(ent).valor.ToString().charAt(0));
                                    comparador2 = (int)(resultado2.getValor(ent).valor.ToString().charAt(0));
                                    if (comparador1 != comparador2)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                default:
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                    break;
                            }
                            break;
                        */
                        case Tipo.enumTipo.cadena:
                            if (resultado2.tipo.tipo == Tipo.enumTipo.cadena)
                            {
                                if (!resultado1.valor.ToString().Equals(resultado2.valor.ToString()))
                                {
                                    resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                }
                                else
                                {
                                    resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                }
                                return resultadoMayor;
                            }
                            break;

                        case Tipo.enumTipo.booleano:
                            if (resultado2.tipo.tipo == Tipo.enumTipo.booleano)
                            {
                                if (!resultado1.valor.ToString().Equals(resultado2.valor.ToString()))
                                {
                                    resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                }
                                else
                                {
                                    resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                }
                                return resultadoMayor;
                            }
                            break;

                        case Tipo.enumTipo.Objecto:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.Objecto:
                                    if (resultado1.valor != resultado2.valor)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.Null:
                                    if (resultado1.valor != null)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                            }
                            break;
                        case Tipo.enumTipo.Null:
                            switch (resultado2.tipo.tipo)
                            {
                                case Tipo.enumTipo.Objecto:
                                    if (resultado2.valor != null)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                                case Tipo.enumTipo.Null:
                                    if (resultado1.valor != resultado2.valor)
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                    }
                                    else
                                    {
                                        resultadoMayor = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                                    }
                                    return resultadoMayor;
                            }
                            break;

                        default:
                            MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                            break;
                    }
                    break;
            }

            return resultadoMayor;
        }

        public override string getDot()
        {
            throw new NotImplementedException();
        }

    }
}
