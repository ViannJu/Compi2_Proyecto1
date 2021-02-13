using System;
using System.Collections.Generic;
using System.Text;
using Irony.Ast;
using Irony.Parsing;

namespace Compi2_Proyecto1.Analizador
{
    /*
     * Clase para la definicion de la gramatica
     * 
     */
    class GramaticaInterprete : Grammar
    {

        public GramaticaInterprete() : base(caseSensitive: false) {

            //Expresiones regulares
            #region ER

            CommentTerminal commentUnilinea = new CommentTerminal("comentarioLinea", "//", "\n", "\r\n"); //si viene una nueva linea se termina de reconocer el comentario.
            CommentTerminal commentMultilinea1 = new CommentTerminal("comentarioBloque", "(*", "*)");
            CommentTerminal commentMultilinea2 = new CommentTerminal("comentarioBloque", "{", "*)");
            RegexBasedTerminal num = new RegexBasedTerminal("num", "[0-9]+");
            RegexBasedTerminal real = new RegexBasedTerminal("real", "[0-9]+[.[0-9]]?");
            RegexBasedTerminal ID = new RegexBasedTerminal("id", "[A-ZÑa-zñ][_0-9A-ZÑa-zñ]*");
            StringLiteral cadena = new StringLiteral("cadena", "'"); /* se indica que la cadena va a empezar con ' (comillas simples) y con esto acepta cualquier cosa que venga despues de las comillas dobles */

            #endregion

            //Definicion de terminales
            #region Terminales

            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var dividido = ToTerm("/");
            var por = ToTerm("*");
            var modulo = ToTerm("%");
            var mayorque = ToTerm(">");
            var menorque = ToTerm("<");
            var mayorigual = ToTerm(">=");
            var menorigual = ToTerm("<=");
            var igualdad = ToTerm("=");
            var diferenteque = ToTerm("<>");
            var and = ToTerm("and");
            var or = ToTerm("or");
            var not = ToTerm("not");

            var tString = ToTerm("string");
            var tint = ToTerm("int");
            var treal = ToTerm("real");
            var tboolean = ToTerm("boolean");
            var tvoid = ToTerm("void");
            var ttype = ToTerm("type");//?
            var tobject = ToTerm("object");//?
            var tarray = ToTerm("array");//?

            var ptcoma = ToTerm(";");
            var dospuntos = ToTerm(":");
            var coma = ToTerm(",");
            #endregion

            #region PRIORIDAD
            //definiendo prioridad, de menos a mas prioridad
            RegisterOperators(1, Associativity.Left, and, or, not);
            RegisterOperators(2, Associativity.Left, mayorque, menorque, mayorigual, menorigual, igualdad, diferenteque);
            RegisterOperators(3, Associativity.Left, mas, menos);
            RegisterOperators(4, Associativity.Left, por, dividido);
            //Aqui falta el umenos

            NonGrammarTerminals.Add(commentUnilinea);
            NonGrammarTerminals.Add(commentMultilinea1);
            NonGrammarTerminals.Add(commentMultilinea2);

            #endregion

            //Definicion de No terminales
            #region NoTerminales
            NonTerminal INICIO = new NonTerminal("INICIO");
            NonTerminal L_INSTRUCCIONES = new NonTerminal("L_INSTRUCCIONES");
            NonTerminal INSTRUCCION = new NonTerminal("INSTRUCCION");

            NonTerminal DECLARACION = new NonTerminal("DECLARACION");
            NonTerminal ASIGNACION = new NonTerminal("ASIGNACION");
            NonTerminal E = new NonTerminal("E");
            NonTerminal L_IDS = new NonTerminal("L_IDS");
            NonTerminal TIPO = new NonTerminal("TIPO");

            #endregion


            #region Gramatica

            INICIO.Rule = L_INSTRUCCIONES;

            L_INSTRUCCIONES.Rule = L_INSTRUCCIONES + INSTRUCCION + ptcoma
                | INSTRUCCION + ptcoma
                //| DECLARACION
                //| ASIGNACION
                ;

            INSTRUCCION.Rule = E
                //|ASIGNACION
                ;

            //DECLARACION.Rule = L_IDS + dospuntos + TIPO;

            TIPO.Rule = tString
                |tint
                |treal
                |tboolean
                |tvoid
                |ttype
                |tobject
                |tarray
                ;

            E.Rule = 
                E + mas + E
                |E + menos + E
                |E + por + E
                |E + dividido + E
                |num
                ;

            L_IDS.Rule = L_IDS + coma + ID
                |ID
                ;


            #endregion

            #region Preferencias
            this.Root = INICIO;
            #endregion



        }

    }
}
