using System;
using System.Collections.Generic;
using System.Text;
using Irony.Ast;
using Irony.Parsing;

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
                MasterClass.Instance.addMessage("Entrada incorrecta");
            }
            else
            {
                MasterClass.Instance.addMessage("Entrada correcta");
                //evaluateInstructions(raiz.ChildNodes[0]);
            }

        }

        //Aqui empezamos a construir nuestro propio arbol ast

    }
}
