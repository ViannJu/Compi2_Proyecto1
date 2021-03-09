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
            RegexBasedTerminal entero = new RegexBasedTerminal("num", "[0-9]+");
            RegexBasedTerminal real = new RegexBasedTerminal("real", "[0-9]+.[0-9]+");
            //RegexBasedTerminal ID = new RegexBasedTerminal("id", "[A-ZÑa-zñ][_0-9A-ZÑa-zñ]*");
            IdentifierTerminal ID = new IdentifierTerminal("id");
            StringLiteral cadena = new StringLiteral("cadena", "'"); /* se indica que la cadena va a empezar con ' (comillas simples) y con esto acepta cualquier cosa que venga despues de las comillas dobles */

            #endregion

            //Definicion de terminales
            #region Terminales

            var program = ToTerm("program");

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
            var igualigual = ToTerm("==");
            var diferenteque = ToTerm("<>");
            var and = ToTerm("and");
            var or = ToTerm("or");
            var not = ToTerm("not");
            var parIzquierdo = ToTerm("(");
            var parDerecho = ToTerm(")");

            var tString = ToTerm("string");
            var tint = ToTerm("int");
            var tinteger = ToTerm("integer");
            var treal = ToTerm("real");
            var tboolean = ToTerm("boolean");
            var tvoid = ToTerm("void");
            var ttype = ToTerm("type");//?
            var tobject = ToTerm("object");//?
            var tarray = ToTerm("array");//?
            var ttrue = ToTerm("true");
            var tfalse = ToTerm("false");

            var ptcoma = ToTerm(";");
            var dospuntos = ToTerm(":");
            var coma = ToTerm(",");
            var punto = ToTerm(".");

            var tvar = ToTerm("var");            
            var twriteln = ToTerm("writeln");
            var twrite = ToTerm("write");

            var tif = ToTerm("if");
            var telse = ToTerm("else");
            var tbegin = ToTerm("begin");
            var tend = ToTerm("end");

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
            NonTerminal IMPRESION = new NonTerminal("IMPRESION");

            NonTerminal E = new NonTerminal("E");
            NonTerminal L_IDS = new NonTerminal("L_IDS");
            NonTerminal L_IDS_Accesos = new NonTerminal("L_IDS_Accesos");
            NonTerminal TIPO = new NonTerminal("TIPO");

            NonTerminal SENT_IF = new NonTerminal("SENT_IF");
            NonTerminal L_IF = new NonTerminal("L_IF");
            NonTerminal BLOQUE = new NonTerminal("BLOQUE");

            #endregion


            #region Gramatica

            INICIO.Rule = program + ID + ptcoma + L_INSTRUCCIONES;

            L_INSTRUCCIONES.Rule = L_INSTRUCCIONES + INSTRUCCION
                | INSTRUCCION              
                ;

            INSTRUCCION.Rule =
                 DECLARACION + ptcoma
                |IMPRESION + ptcoma
                |SENT_IF + ptcoma
                |BLOQUE +punto      //El unico bloque independiente es el *Main*
                ;

            SENT_IF.Rule = L_IF + telse + BLOQUE
                |L_IF
                ;

            L_IF.Rule = L_IF + telse + tif + parIzquierdo + E + parDerecho + BLOQUE
                |tif + parIzquierdo + E + parDerecho + BLOQUE;

            BLOQUE.Rule = tbegin + tend
                |tbegin + L_INSTRUCCIONES + tend
                ;

            IMPRESION.Rule = 
                twrite + parIzquierdo + E + parDerecho
                |twriteln + parIzquierdo + E + parDerecho;

            DECLARACION.Rule = tvar + L_IDS + dospuntos + TIPO
                | tvar + ID + dospuntos + TIPO + igualdad + E
                ;

            TIPO.Rule = tString
                |tint
                |tinteger
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
                |E + modulo + E
                |E + mayorque + E
                |E + menorque + E
                |E + mayorigual + E
                |E + menorigual + E
                |E + diferenteque + E
                |E + igualigual + E
                |E + or + E
                |E + and + E
                |not + E
                |parIzquierdo + E + parDerecho
                |menos + E  //para manejar numeros negativos
                |entero
                |real
                |ttrue
                |tfalse
                |cadena
                |L_IDS_Accesos
                ;

            L_IDS_Accesos.Rule = L_IDS_Accesos + punto + ID
                |ID;

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
