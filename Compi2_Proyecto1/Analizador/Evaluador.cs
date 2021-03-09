using System;
using System.Text;
using Irony.Ast;
using Irony.Parsing;
using System.Windows.Forms;
using System.Collections.Generic;
using Compi2_Proyecto1.Expresiones;
using Compi2_Proyecto1.Principales;
using Compi2_Proyecto1.Instrucciones;
using Compi2_Proyecto1.Aritmeticas;
using Compi2_Proyecto1.Objetos;
using Compi2_Proyecto1.Relacionales;
using Compi2_Proyecto1.Logicas;
using Compi2_Proyecto1.Control;

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
                MasterClass.Instance.addMessage("/****    Entrada correcta    ****/", true);
                evaluarL_Instrucciones(raiz.ChildNodes[3], false, null);
            }

        }

        /********** AQUI EMPEZAMOS A CONSTRUIR EL ARBOL DE ANALISIS SINTACTICO **********/

        private void evaluarL_Instrucciones(ParseTreeNode nodo, bool bloque, LinkedList<Instruccion> guardadosBloque) {
            if (nodo.ChildNodes.Count == 2) {
                //L_Instrucciones = L_Instrucciones + Instruccion + ptcoma
                evaluarL_Instrucciones(nodo.ChildNodes[0], bloque, guardadosBloque);
                evaluarInstruccion(nodo.ChildNodes[1], bloque, guardadosBloque);
            } else if (nodo.ChildNodes.Count == 1) {
                //L_Instrucciones = Instruccion + ptcoma
                evaluarInstruccion(nodo.ChildNodes[0], bloque, guardadosBloque);

            }
        }

        private void evaluarInstruccion(ParseTreeNode nodo, bool bloque, LinkedList<Instruccion> guardadosInstruccion) {

            //Bifurcacion de metodos dependiendo de la instruccion

            //Como se llama la instruccion que viene
            switch (nodo.ChildNodes[0].Term.Name) {

                case "SENT_IF":

                    break;



                case "BLOQUE":
                    //entonces el bloque tiene instrucciones
                    if (nodo.ChildNodes[0].ChildNodes.Count == 3)
                    {

                        //mandamos a traer la lista de instrucciones
                        LinkedList<Instruccion> listaInsBloque = new LinkedList<Instruccion>();
                        evaluarL_Instrucciones(nodo.ChildNodes[0].ChildNodes[1], true, listaInsBloque);

                        Instruccion temp;
                        temp = new Bloque(listaInsBloque);

                        //Si es un bloque se guarda en la lista de instrucciones de bloque
                        if (bloque)
                        {
                            guardadosInstruccion.AddLast(temp);
                        }
                        //Si no es un bloque es la lista general de instrucciones de MasterClass
                        else
                        {
                            //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                            MasterClass.Instance.addInstruction(temp);
                            //MessageBox.Show("guarde el bloque en la masterclass");
                        }

                    }
                    //entonces el bloque no tiene instrucciones
                    else if (nodo.ChildNodes[0].ChildNodes.Count == 2)
                    {

                        Instruccion temp;
                        temp = new Bloque();

                        //Si es un bloque se guarda en la lista de instrucciones de bloque
                        if (bloque)
                        {
                            guardadosInstruccion.AddLast(temp);
                        }
                        //Si no es un bloque es la lista general de instrucciones de MasterClass
                        else
                        {
                            //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                            MasterClass.Instance.addInstruction(temp);
                        }

                    }
                    break;


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

                        Instruccion temp;
                        temp = new Declaracion(tipe, listaIDS, 0, 0);


                        //Si es un bloque se guarda en la lista de instrucciones de bloque
                        if (bloque)
                        {
                            guardadosInstruccion.AddLast(temp);
                        }
                        //Si no es un bloque es la lista general de instrucciones de MasterClass
                        else {
                            //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                            MasterClass.Instance.addInstruction(temp);
                        }
                        
                        
                        
                    }
                    else {

                        //Declaracion/Asignacion
                        //tvar + ID + dospuntos + TIPO + igualdad + E

                        string identificador = nodo.ChildNodes[0].ChildNodes[1].Token.ValueString;

                        //Tengo que mandar el tipo
                        Tipo tipe = evaluarTipo(nodo.ChildNodes[0].ChildNodes[3]);

                        //Tengo que mandar la Expresion
                        Expresion expression = evaluarExpresion(nodo.ChildNodes[0].ChildNodes[5]);

                        //Mandamos la declaracion
                        Instruccion temp;
                        temp = new Declaracion(tipe, identificador, expression, 0, 0);

                        //Si es un bloque se guarda en la lista de instrucciones de bloque
                        if (bloque)
                        {
                            guardadosInstruccion.AddLast(temp);
                        }
                        //Si no es un bloque es la lista general de instrucciones de MasterClass
                        else
                        {
                            //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                            MasterClass.Instance.addInstruction(temp);
                        }


                    }

                    break;

                case "IMPRESION":

                    //no tiene salto de linea
                    if (nodo.ChildNodes[0].ChildNodes[0].Term.Name == "write") {

                        //primero mando a traer la expresion;
                        Expresion expression = evaluarExpresion(nodo.ChildNodes[0].ChildNodes[2]);

                        Instruccion temp;
                        temp = new Impresion(expression, false);

                        //Si es un bloque se guarda en la lista de instrucciones de bloque
                        if (bloque)
                        {
                            guardadosInstruccion.AddLast(temp);
                        }
                        //Si no es un bloque es la lista general de instrucciones de MasterClass
                        else
                        {
                            //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                            MasterClass.Instance.addInstruction(temp);
                        }



                    }
                    
                    //tiene salto de linea
                    else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name == "writeln") {

                        //primero mando a traer la expresion;
                        Expresion expression = evaluarExpresion(nodo.ChildNodes[0].ChildNodes[2]);

                        Instruccion temp;
                        temp = new Impresion(expression, true);



                        //Si es un bloque se guarda en la lista de instrucciones de bloque
                        if (bloque)
                        {
                            guardadosInstruccion.AddLast(temp);
                        }
                        //Si no es un bloque es la lista general de instrucciones de MasterClass
                        else
                        {
                            //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                            MasterClass.Instance.addInstruction(temp);
                        }

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
                IDobjeto = evaluarID(nodo.ChildNodes[2]);
                lista.AddLast(IDobjeto.ToString());
            }
            else {
                Object IDobjeto;
                //L_IDS = ID //El siguiente nodo es un valor primitivo entonces guardamos en la lista
                IDobjeto = evaluarID(nodo.ChildNodes[0]);
                lista.AddLast(IDobjeto.ToString());
            }

            return lista;

        }

        private Object evaluarID(ParseTreeNode nodo) {

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
                case "string":
                    return new Tipo(Tipo.enumTipo.cadena);
                case "boolean":
                    return new Tipo(Tipo.enumTipo.booleano);

            }

            return new Tipo(Tipo.enumTipo.Null);
        }

        
        private Expresion evaluarExpresion(ParseTreeNode nodo) {
            //verificar de la gramatica cuantos hijos tiene

            //Operaciones Aritmeticas, logicas, relacionales
            if (nodo.ChildNodes.Count == 3)
            {
                switch (nodo.ChildNodes[1].Term.Name) {

                    case "+":
                        MessageBox.Show("Entre a suma");
                        return new Suma(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), 0, 0);
                    case "-":
                        return new Resta(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), 0, 0);
                    case "*":
                        return new Multiplicacion(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), 0, 0);
                    case "/":
                        return new Division(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), 0, 0);
                    case "%":
                        return new Modulo(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), 0, 0);
                    case ">":
                        return new Relacional(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), ">", 0, 0);
                    case "<":
                        return new Relacional(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), "<", 0, 0);
                    case ">=":
                        return new Relacional(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), ">=", 0, 0);
                    case "<=":
                        return new Relacional(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), "<=", 0, 0);
                    case "==":
                        return new Relacional(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), "==", 0, 0);
                    case "<>":
                        return new Relacional(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), "!=", 0, 0);
                    case "or":
                        return new Logica(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), "or", 0, 0);
                    case "and":
                        return new Logica(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), "and", 0, 0);
                    default:

                        //verificamos si es ( + E + )
                        if (nodo.ChildNodes[0].Term.Name == "(" && nodo.ChildNodes[2].Term.Name == ")") {

                            MessageBox.Show("Entre a un parentesis");

                        }

                        break;

                }

            } else if (nodo.ChildNodes.Count == 2) {

                switch (nodo.ChildNodes[0].Term.Name) {

                    case "-":
                        return new Negativo(evaluarExpresion(nodo.ChildNodes[1]), 0, 0);
                    case "not":
                        return new Logica(evaluarExpresion(nodo.ChildNodes[1]), "not", 0, 0);

                }

            }
            //entonces es un primitivo
            else {
                return evaluarPrimitivo(nodo.ChildNodes[0]);

            }

            return null;
        }

        private Expresion evaluarPrimitivo(ParseTreeNode nodo) {

            //MessageBox.Show(nodo.Term.Name);    //entero, real, cadena, true, false
            //MessageBox.Show(nodo.Token.ValueString);    //el valor como tal

            switch (nodo.Term.Name) {

                case "num":
                    return new Primitivo(new Tipo(Tipo.enumTipo.entero), int.Parse(nodo.Token.ValueString));
                case "real":
                    return new Primitivo(new Tipo(Tipo.enumTipo.real), float.Parse(nodo.Token.ValueString));
                case "cadena":
                    return new Primitivo(new Tipo(Tipo.enumTipo.cadena), nodo.Token.ValueString);
                case "true":
                    return new Primitivo(new Tipo(Tipo.enumTipo.booleano), bool.Parse(nodo.Token.ValueString));
                case "false":
                    return new Primitivo(new Tipo(Tipo.enumTipo.booleano), bool.Parse(nodo.Token.ValueString));

            }

            //si llega aqui no tenia tipo y por lo tanto es una lista de accesos

            //Mandamos a traer la lista de los ids
            LinkedList<Id> ListaIDS = new LinkedList<Id>();

            ListaIDS = evaluarL_IDS_Accesos(nodo, ListaIDS);

            Lista_Accesos milistaAccesos = new Lista_Accesos(ListaIDS);

            return milistaAccesos;


        }

        public LinkedList<Id> evaluarL_IDS_Accesos(ParseTreeNode nodo, LinkedList<Id> lista) {

            if (nodo.ChildNodes.Count == 3)
            {
                Id IDobjeto;
                //L_IDS = L_IDS + punto + ID
                evaluarL_IDS_Accesos(nodo.ChildNodes[0], lista);
                //el siguiente nodo es un valor primitivo entonces guardamos en la lista
                IDobjeto = evaluarID_Acceso(nodo.ChildNodes[2]);
                lista.AddLast(IDobjeto);
            }
            else
            {
                Id IDobjeto;
                //L_IDS = ID //El siguiente nodo es un valor primitivo entonces guardamos en la lista
                IDobjeto = evaluarID_Acceso(nodo.ChildNodes[0]);
                lista.AddLast(IDobjeto);
            }

            return lista;

        }

        private Id evaluarID_Acceso(ParseTreeNode nodo)
        {
            return new Id(nodo.Token.ValueString, 0, 0);
        }





    }
}
