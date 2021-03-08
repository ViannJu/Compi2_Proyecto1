using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Logicas
{
    public class Logica : Expresion
    {

        String operacion;   //para saber que operacion vamos a realizar
        Expresion hijo1;    //rama izquierda de la operacion
        Expresion hijo2;    //rama derecha de la operacion

        public Logica(Expresion hijo1, Expresion hijo2, String operacion, int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
            this.hijo1 = hijo1;
            this.hijo2 = hijo2;
            this.operacion = operacion;
        }

        public Logica(Expresion hijo1, String operacion, int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
            this.hijo1 = hijo1;
            this.hijo2 = null;
            this.operacion = operacion;
        }

        public override Expresion getValor(Entorno ent)
        {
            Primitivo resultadoBool = new Primitivo(new Tipo(Tipo.enumTipo.error), "@error@");

            Expresion resultado1 = hijo1.getValor(ent);
            Expresion resultado2;

            if (hijo2 == null)
            {
                resultado2 = null;
            }

            switch (this.operacion)
            {
                case "or":
                    if (resultado1.tipo.tipo == Tipo.enumTipo.booleano)
                    {
                        bool comparador1 = bool.Parse(resultado1.valor.ToString());
                        if (comparador1 == true)
                        {
                            //acepta la condicion y devuelve true
                            resultadoBool = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                        }
                        else
                        {
                            //si no funciono el primero resolvemos el segundo
                            resultado2 = hijo2.getValor(ent);
                            if (resultado2.tipo.tipo == Tipo.enumTipo.booleano)
                            {

                                bool comparador2 = bool.Parse(resultado2.valor.ToString());
                                if (comparador2 == true)
                                {
                                    resultadoBool = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                }
                                else
                                {
                                    resultadoBool = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);

                                }
                            }

                        }
                    }
                    else
                    {
                        MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + hijo2.getValor(ent).tipo.tipo, linea, columna));

                    }
                    return resultadoBool;

                case "and":
                    if (resultado1.tipo.tipo == Tipo.enumTipo.booleano)
                    {
                        bool comparador1 = bool.Parse(resultado1.valor.ToString());
                        if (comparador1 == false)
                        {
                            //si es falso el primero lo devuelve y termina
                            resultadoBool = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                        }
                        else
                        {
                            //si el primero es verdadero,  debemos verificar el otro
                            resultado2 = hijo2.getValor(ent);
                            if (resultado2.tipo.tipo == Tipo.enumTipo.booleano)
                            {

                                bool comparador2 = bool.Parse(resultado2.valor.ToString());
                                if (comparador2 == true)
                                {
                                    resultadoBool = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                                }
                                else
                                {
                                    resultadoBool = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);

                                }
                            }

                        }
                    }
                    else
                    {
                        MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + hijo2.getValor(ent).tipo.tipo, linea, columna));
                    }
                    return resultadoBool;

                case "xor":
                    resultado2 = hijo2.getValor(ent);
                    if (resultado1.tipo.tipo == Tipo.enumTipo.booleano && resultado2.tipo.tipo == Tipo.enumTipo.booleano)
                    {
                        bool comparador1 = bool.Parse(resultado1.valor.ToString());
                        bool comparador2 = bool.Parse(resultado2.valor.ToString());
                        if (comparador1 == true && comparador2 == true)
                        {
                            //iguales devuelve false
                            resultadoBool = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                        }
                        else if (comparador1 == false && comparador2 == false)
                        {
                            //iguales devuelve false
                            resultadoBool = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                        }
                        else if (comparador1 == true && comparador2 == false)
                        {
                            //diferentes devuelve true
                            resultadoBool = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                        }
                        else if (comparador1 == false && comparador2 == true)
                        {
                            //diferentes devuelve true
                            resultadoBool = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                        }
                    }
                    else
                    {
                        MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato: " + resultado1.tipo.tipo + " con: " + resultado2.tipo.tipo, linea, columna));
                    }

                    return resultadoBool;

                case "not":
                    resultado2 = null;
                    if (resultado1.tipo.tipo == Tipo.enumTipo.booleano && resultado2 == null)
                    {
                        bool comparador1 = bool.Parse(resultado1.valor.ToString());
                        if (comparador1 == true)
                        {
                            //negamos y devuelve false
                            resultadoBool = new Primitivo(new Tipo(Tipo.enumTipo.booleano), false);
                        }
                        else if (comparador1 == false)
                        {
                            //negamos y devuelve true
                            resultadoBool = new Primitivo(new Tipo(Tipo.enumTipo.booleano), true);
                        }
                    }
                    else
                    {
                        MasterClass.Instance.addError(new C_Error("Semantico", "No se puede comparar tipo de dato para negarse: " + resultado1.tipo.tipo, linea, columna));
                    }
                    return resultadoBool;
            }

            return resultadoBool;
        }

        public override string getDot()
        {
            throw new NotImplementedException();
        }

    }
}
