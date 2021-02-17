using System;
using System.Collections.Generic;
using System.Text;
using Irony.Ast;
using Irony.Parsing;
using Compi2_Proyecto1.Expresiones;
using Compi2_Proyecto1.Principales;
using Compi2_Proyecto1.Instrucciones;

namespace Compi2_Proyecto1.Analizador
{
    public class Evaluador
    {

        public Evaluador() { }

        public void analizar(string cadena)
        {
            GramaticaInterprete gramatica = new GramaticaInterprete(); //instancia de la gramatica
            LanguageData lenguaje = new LanguageData(gramatica); //genera un lenguaje para nuestra gramática. 
            Parser parser = new Parser(lenguaje); //con esto generamos el parser

            ParseTree arbol = parser.Parse(cadena); //generamos el arbol de analisis sintactico
            ParseTreeNode raiz = arbol.Root;        //obtenemos la raiz del arbol, en este caso será 'S'.


            //Manejo de errores

            if (raiz == null)
            {
                //significa que la cadena de entrada contiene errores, por ello no se generó el arbol de analisis sintactico. 
                MasterClass.Instance.addMessage("Entrada incorrecta", true);
            }
            else
            {
                MasterClass.Instance.addMessage("Entrada correcta", true);
                evaluarL_Instrucciones(raiz.ChildNodes[0]);
            }

        }

        /********** AQUI EMPEZAMOS A CONSTRUIR EL ARBOL DE ANALISIS SINTACTICO **********/

        private void evaluarL_Instrucciones(ParseTreeNode nodo) {
            if (nodo.ChildNodes.Count == 3) {
                //L_Instrucciones = L_Instrucciones + Instruccion + ptcoma
                evaluarL_Instrucciones(nodo.ChildNodes[0]);
                evaluarInstruccion(nodo.ChildNodes[1]);
            } else if (nodo.ChildNodes.Count == 2) {
                //L_Instrucciones = Instruccion + ptcoma
                MasterClass.Instance.addMessage("Entramos aqui", true);
                evaluarInstruccion(nodo.ChildNodes[0]);

            }
        }

        private void evaluarInstruccion(ParseTreeNode nodo) {

            //Bifurcacion de metodos dependiendo de la instruccion

            //Como se llama la instruccion que viene
            switch (nodo.ChildNodes[0].Term.Name) {

                //Declaracion
                case "DECLARACION":

                    if (nodo.ChildNodes[0].ChildNodes.Count == 4)
                    {
                        //tvar + L_IDS + dospuntos + TIPO
                        //Lista de declaraciones
                        LinkedList<string> listaIDS = new LinkedList<string>();

                        //tengo que mandar a traer la lista de ID's
                        evaluarL_IDS(nodo.ChildNodes[0].ChildNodes[1], listaIDS);

                        //MasterClass.Instance.addMessage("La lista de ids tiene: " + listaIDS.Count, true);


                        //Tengo que mandar el tipo 
                        Tipo tipe = evaluarTipo(nodo.ChildNodes[0].ChildNodes[3]);

                        //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                        Instruccion temp;
                        temp = new Declaracion(tipe, listaIDS, 0, 0);
                        MasterClass.Instance.addInstruction(temp);
                    }
                    else {

                        //Declaracion/Asignacion
                        //tvar + ID + dospuntos + TIPO + igualdad + E

                        string identificador = nodo.ChildNodes[0].ChildNodes[1].Token.ValueString;

                        //Tengo que mandar el tipo
                        Tipo tipe = evaluarTipo(nodo.ChildNodes[0].ChildNodes[3]);

                        //Tengo que mandar la Expresion
                        //Expresion expression = evaluarExpresion(nodo.ChildNodes[0].ChildNodes[5]);

                        //Mandamos la declaracion
                        //Instruccion temp;
                        //temp = new Declaracion(tipe, identificador, expression, 0, 0);
                        //MasterClass.Instance.addInstruction(temp);
                    }

                    break;
            }

        }

        private LinkedList<string> evaluarL_IDS(ParseTreeNode nodo, LinkedList<string> lista) {

            if (nodo.ChildNodes.Count == 3)
            {
                Object IDobjeto;
                //L_IDS = L_IDS + coma + ID
                evaluarL_IDS(nodo.ChildNodes[0], lista);
                //el siguiente nodo es un valor primitivo entonces guardamos en la lista
                IDobjeto = evaluarPrimitivo(nodo.ChildNodes[2]);
                lista.AddLast(IDobjeto.ToString());
            }
            else {
                Object IDobjeto;
                //L_IDS = ID //El siguiente nodo es un valor primitivo entonces guardamos en la lista
                IDobjeto = evaluarPrimitivo(nodo.ChildNodes[0]);
                lista.AddLast(IDobjeto.ToString());
            }

            return lista;

        }

        private Object evaluarPrimitivo(ParseTreeNode nodo) {
            new Id(nodo.Token.ValueString, 0, 0);
            return nodo.Token.ValueString;
        }

        private Tipo evaluarTipo(ParseTreeNode nodo) {
            //verificamos que tipo viene
            switch (nodo.ChildNodes[0].Term.Name) {
                case "integer":
                    return new Tipo(Tipo.enumTipo.entero);
                case "int":
                    return new Tipo(Tipo.enumTipo.entero);
                case "real":
                    return new Tipo(Tipo.enumTipo.real);
                case "cadena":
                    return new Tipo(Tipo.enumTipo.cadena);
                case "booleano":
                    return new Tipo(Tipo.enumTipo.booleano);

            }

            return new Tipo(Tipo.enumTipo.Null);
        }

        /*
        private Expresion evaluarExpresion(ParseTreeNode nodo) {

            //verificar de la gramatica cuantos hijos tiene

        }
        */





    }
}
