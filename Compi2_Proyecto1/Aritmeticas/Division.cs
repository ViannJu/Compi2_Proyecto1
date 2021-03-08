using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Aritmeticas
{
    public class Division : Expresion
    {

        String operacion;   //para guardar el simbolo de la operacion
        Expresion hijo1;    //rama izquierda de la operacion    
        Expresion hijo2;    //rama derecha de la operacion

        public Division(Expresion hijo1, Expresion hijo2, int linea, int columna)
        {

            this.operacion = "/";
            this.linea = linea;
            this.columna = columna;
            this.hijo1 = hijo1;
            this.hijo2 = hijo2;
        }

        public override Expresion getValor(Entorno ent)
        {
            Primitivo resultadoDivi = new Primitivo(new Tipo(Tipo.enumTipo.error), "@error@");
            Expresion resultado1 = hijo1.getValor(ent);
            Expresion resultado2 = hijo2.getValor(ent);


            switch (resultado1.tipo.tipo)
            {
                //si el primer hijo es entero
                case Tipo.enumTipo.entero:

                    switch (resultado2.tipo.tipo)
                    {

                        case Tipo.enumTipo.entero:
                            //si hijo1 es entero e hijo2 es entero = entero
                            if (int.Parse(resultado2.valor.ToString()) == 0)
                            {
                                MasterClass.Instance.addError(new C_Error("Semantico", "No se puede dividir entre 0 una cantidad ", linea, columna));
                                return resultadoDivi;
                            }
                            resultadoDivi = new Primitivo(new Tipo(Tipo.enumTipo.entero), int.Parse(resultado1.valor.ToString()) / int.Parse(resultado2.valor.ToString()));
                            return resultadoDivi;
                        case Tipo.enumTipo.real:
                            //si hijo1 es entero e hijo2 es doble = doble
                            if (float.Parse(resultado2.valor.ToString()) == 0.0)
                            {
                                MasterClass.Instance.addError(new C_Error("Semantico", "No se puede dividir entre 0 una cantidad ", linea, columna));
                                return resultadoDivi;
                            }
                            resultadoDivi = new Primitivo(new Tipo(Tipo.enumTipo.real), float.Parse(resultado1.valor.ToString()) / float.Parse(resultado2.valor.ToString()));
                            return resultadoDivi;
                        /*
                        case Tipo.enumTipo.caracter:
                            //si hijo1 es entero e hijo2 es un caracter = entero
                            if ((int)(resultado2.valor.ToString().charAt(0)) == 0)
                            {
                                MasterClass.Instance.addError(new C_Error("Semantico", "No se puede dividir entre 0 una cantidad ", linea, columna));
                                return resultadoDivi;
                            }
                            resultadoDivi = new Primitivo(new Tipo(Tipo.enumTipo.entero), int.Parse(resultado1.valor.ToString()) / (int)(resultado2.valor.ToString().charAt(0)));
                            return resultadoDivi;
                        */
                        default:
                            MasterClass.Instance.addError(new C_Error("Semantico", "No se puede dividir: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                            break;
                    }
                    break;

                //si el primer hijo es double
                case Tipo.enumTipo.real:

                    switch (resultado2.tipo.tipo)
                    {

                        case Tipo.enumTipo.entero:
                            //si el hijo1 es doble y el hijo2 es  entero = doble
                            if (float.Parse(resultado2.valor.ToString()) == 0.0)
                            {
                                MasterClass.Instance.addError(new C_Error("Semantico", "No se puede dividir entre 0 una cantidad ", linea, columna));
                                return resultadoDivi;
                            }
                            resultadoDivi = new Primitivo(new Tipo(Tipo.enumTipo.real), float.Parse(resultado1.valor.ToString()) / float.Parse(resultado2.valor.ToString()));
                            return resultadoDivi;
                        case Tipo.enumTipo.real:
                            if (float.Parse(resultado2.valor.ToString()) == 0.0)
                            {
                                MasterClass.Instance.addError(new C_Error("Semantico", "No se puede dividir entre 0 una cantidad ", linea, columna));
                                return resultadoDivi;
                            }
                            //si el hijo1 es doble y el hijo2 es doble = doble
                            resultadoDivi = new Primitivo(new Tipo(Tipo.enumTipo.real), float.Parse(resultado1.valor.ToString()) / float.Parse(resultado2.valor.ToString()));
                            return resultadoDivi;
                        /*
                        case Tipo.enumTipo.caracter:
                            if ((double)(resultado2.valor.ToString().charAt(0)) == 0.0)
                            {
                                MasterClass.Instance.addError(new C_Error("Semantico", "No se puede dividir entre 0 una cantidad ", linea, columna));
                                return resultadoDivi;
                            }
                            //si el hijo1 es doble y el hijo2 es caracter = doble
                            resultadoDivi = new Primitivo(new Tipo(Tipo.enumTipo.entero), float.Parse(resultado1.valor.ToString()) / (double)resultado2.valor.ToString().charAt(0));
                            return resultadoDivi;
                        */
                        default:
                            MasterClass.Instance.addError(new C_Error("Semantico", "No se puede dividir: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                            break;
                    }


                    break;
                    //si el primer hijo es caracter
                    /*
                    case caracter:

                        switch (resultado2.tipo.tipo)
                        {

                            case entero:
                                //si el hijo1 es caracter y el hijo2 es  entero = entero
                                if (int.Parse(resultado2.valor.ToString()) == 0)
                                {
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede dividir entre 0 una cantidad ", linea, columna));
                                    return resultadoDivi;
                                }
                                resultadoDivi = new Primitivo(new Tipo(Tipo.enumTipo.entero), (int)resultado1.valor.ToString().charAt(0) / int.Parse(resultado2.valor.ToString()));
                                return resultadoDivi;
                            case doble:
                                //si el hijo1 es caracter y el hijo2 es doble = doble
                                if (float.Parse(resultado2.valor.ToString()) == 0.0)
                                {
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede dividir entre 0 una cantidad ", linea, columna));
                                    return resultadoDivi;
                                }
                                resultadoDivi = new Primitivo(new Tipo(Tipo.enumTipo.doble), (double)resultado1.valor.ToString().charAt(0) / float.Parse(resultado2.valor.ToString()));
                                return resultadoDivi;
                            case caracter:
                                //si el hijo1 es caracter y el hijo2 es caracter = entero
                                if ((int)(resultado2.valor.ToString().charAt(0)) == 0)
                                {
                                    MasterClass.Instance.addError(new C_Error("Semantico", "No se puede dividir entre 0 una cantidad ", linea, columna));
                                    return resultadoDivi;
                                }
                                resultadoDivi = new Primitivo(new Tipo(Tipo.enumTipo.entero), (int)resultado1.valor.ToString().charAt(0) / (int)resultado2.valor.ToString().charAt(0));
                                return resultadoDivi;
                            default:
                                MasterClass.Instance.addError(new C_Error("Semantico", "No se puede dividir: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                                break;
                        }
                        break;
                    */
            }
            MasterClass.Instance.addError(new C_Error("Semantico", "No se puede dividir: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
            return resultadoDivi;

        }

        public override string getDot()
        {
            throw new NotImplementedException();
        }

    }
    
}
