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
            CommentTerminal commentMultilinea2 = new CommentTerminal("comentarioBloque", "{", "}");
            NumberLiteral entero = new NumberLiteral("entero");
            //RegexBasedTerminal real = new RegexBasedTerminal("real", "[0-9]+.[0-9]+");
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
            var tthen = ToTerm("then");
            var tbegin = ToTerm("begin");
            var tend = ToTerm("end");
            var twhile = ToTerm("while");
            var tdo = ToTerm("do");
            var tcase = ToTerm("case");
            var tof = ToTerm("of");
            var trepeat = ToTerm("repeat");
            var tuntil = ToTerm("until");
            var tto = ToTerm("to");
            var tfor = ToTerm("for");
            var tdownto = ToTerm("downto");

            var tfunction = ToTerm("function");
            var tprocedure = ToTerm("procedure");

            var tbreak = ToTerm("break");
            var tcontinue = ToTerm("continue");
            var texit = ToTerm("exit");

            var tgraficar_ts = ToTerm("graficar_ts");
            var tconst = ToTerm("const");

            #endregion

            #region PRIORIDAD
            //definiendo prioridad, de menos a mas prioridad
            RegisterOperators(1, Associativity.Left, and, or, not);
            RegisterOperators(2, Associativity.Left, mayorque, menorque, mayorigual, menorigual, igualdad, diferenteque);
            RegisterOperators(3, Associativity.Left, mas, menos);
            RegisterOperators(4, Associativity.Left, por, dividido, modulo);
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
            NonTerminal L_Expresiones = new NonTerminal("L_Expresiones");
            NonTerminal TIPO = new NonTerminal("TIPO");

            NonTerminal SENT_IF = new NonTerminal("SENT_IF");
            NonTerminal L_IF = new NonTerminal("L_IF");
            NonTerminal BLOQUE = new NonTerminal("BLOQUE");
            NonTerminal SENT_WHILE = new NonTerminal("SENT_WHILE");
            NonTerminal SENT_SWITCH = new NonTerminal("SENT_SWITCH");
            NonTerminal L_BLOQUES_CASE = new NonTerminal("L_BLOQUES_CASE");
            NonTerminal BLOQUE_DEFAULT = new NonTerminal("BLOQUE_DEFAULT");
            NonTerminal SENT_REPEAT_UNTIL = new NonTerminal("SENT_REPEAT_UNTIL");
            NonTerminal SENT_FOR = new NonTerminal("SENT_FOR");

            NonTerminal BREAK = new NonTerminal("BREAK");
            NonTerminal CONTINUE = new NonTerminal("CONTINUE");
            NonTerminal EXIT = new NonTerminal("EXIT");

            NonTerminal DECLARACION_MF = new NonTerminal("DECLARACION_MF");
            NonTerminal L_DECLARACIONES = new NonTerminal("L_DECLARACIONES");
            NonTerminal DECLARACION_ESPECIAL = new NonTerminal("DECLARACION_ESPECIAL");
            NonTerminal L_DECLARACIONES_ESPECIALES = new NonTerminal("L_DECLARACIONES_ESPECIALES");
            NonTerminal CUERPO_PARAMETROS = new NonTerminal("CUERPO_PARAMETROS");
            NonTerminal CUERPO_INTERNO = new NonTerminal("CUERPO_INTERNO");
            NonTerminal LLAMADA_M = new NonTerminal("LLAMADA_M");
            NonTerminal OTRO_ID = new NonTerminal("OTRO_ID");

            NonTerminal L_INSTRUCCIONES_PRINCIPALES = new NonTerminal("L_INSTRUCCIONES_PRINCIPALES");
            NonTerminal INSTRUCCION_PRINCIPAL = new NonTerminal("INSTRUCCION_PRINCIPAL");

            NonTerminal GRAFICAR_ENTS = new NonTerminal("GRAFICAR_ENTS");
            NonTerminal CONSTANTES = new NonTerminal("CONSTANTES");

            #endregion


            #region Gramatica

            INICIO.Rule = program + ID + ptcoma + L_INSTRUCCIONES_PRINCIPALES;

            L_INSTRUCCIONES_PRINCIPALES.Rule = L_INSTRUCCIONES_PRINCIPALES + INSTRUCCION_PRINCIPAL
                | INSTRUCCION_PRINCIPAL
                ;

            INSTRUCCION_PRINCIPAL.Rule =
                 DECLARACION + ptcoma
                | CONSTANTES + ptcoma
                | DECLARACION_MF + ptcoma
                | BLOQUE + punto    //El unico bloque independiente es el *Main*
                | GRAFICAR_ENTS + ptcoma
                ;

            CONSTANTES.Rule = tconst + ID +  dospuntos + TIPO +  igualdad + E;

            DECLARACION_MF.Rule =
                 tfunction + ID + CUERPO_PARAMETROS + dospuntos + TIPO + CUERPO_INTERNO
                | tprocedure + ID + CUERPO_PARAMETROS + CUERPO_INTERNO
                ;

            CUERPO_INTERNO.Rule = ptcoma + L_DECLARACIONES + BLOQUE
                | ptcoma + BLOQUE
                ;

            CUERPO_PARAMETROS.Rule = parIzquierdo + L_DECLARACIONES_ESPECIALES + parDerecho
                | parIzquierdo + parDerecho
                ;

            L_DECLARACIONES_ESPECIALES.Rule = L_DECLARACIONES_ESPECIALES + ptcoma + DECLARACION_ESPECIAL
                | DECLARACION_ESPECIAL
                ;

            L_DECLARACIONES.Rule = L_DECLARACIONES + DECLARACION + ptcoma
                | DECLARACION + ptcoma
                ;

            DECLARACION_ESPECIAL.Rule = L_IDS + dospuntos + TIPO
                | tvar + L_IDS + dospuntos + TIPO
                ;

            DECLARACION.Rule = tvar + L_IDS + dospuntos + TIPO
                | tvar + L_IDS + dospuntos + TIPO + igualdad + E
                ;

            BLOQUE.Rule = tbegin + tend
                | tbegin + L_INSTRUCCIONES + tend
                ;

            L_INSTRUCCIONES.Rule = L_INSTRUCCIONES + INSTRUCCION
                | INSTRUCCION              
                ;

            INSTRUCCION.Rule =
                 ASIGNACION + ptcoma
                |IMPRESION + ptcoma
                |SENT_IF + ptcoma
                |SENT_WHILE + ptcoma
                |SENT_SWITCH + ptcoma
                |SENT_REPEAT_UNTIL + ptcoma
                |SENT_FOR + ptcoma
                |LLAMADA_M + ptcoma
                |BREAK + ptcoma
                |CONTINUE + ptcoma
                |EXIT + ptcoma
                |GRAFICAR_ENTS + ptcoma
                ;

            GRAFICAR_ENTS.Rule = tgraficar_ts + parIzquierdo + parDerecho;

            BREAK.Rule = tbreak;

            CONTINUE.Rule = tcontinue;

            EXIT.Rule = texit + parIzquierdo + parDerecho
                | texit + parIzquierdo + E + parDerecho
                ;

            LLAMADA_M.Rule = L_IDS_Accesos + parIzquierdo + parDerecho
                |L_IDS_Accesos + parIzquierdo + L_Expresiones + parDerecho
                ;

            INSTRUCCION.ErrorRule = SyntaxError + ptcoma;

            SENT_FOR.Rule = tfor + ASIGNACION + tto + E + tdo + BLOQUE
                |tfor + ASIGNACION + tdownto + E + tdo + BLOQUE
                ;

            SENT_REPEAT_UNTIL.Rule = trepeat + BLOQUE + tuntil + E;

            SENT_SWITCH.Rule = tcase + parIzquierdo + E + parDerecho + tof + L_BLOQUES_CASE + tend;

            L_BLOQUES_CASE.Rule = L_BLOQUES_CASE + BLOQUE_DEFAULT
                |BLOQUE_DEFAULT;

            BLOQUE_DEFAULT.Rule = E + dospuntos + BLOQUE
                |telse + BLOQUE;

            SENT_WHILE.Rule = twhile + E + tdo + BLOQUE;

            SENT_IF.Rule = L_IF + telse + BLOQUE
                |L_IF
                ;

            L_IF.Rule = L_IF + telse + tif + parIzquierdo + E + parDerecho + tthen + BLOQUE
                |tif + parIzquierdo + E + parDerecho + tthen + BLOQUE
                ;            

            IMPRESION.Rule = 
                twrite + parIzquierdo + L_Expresiones + parDerecho
                |twriteln + parIzquierdo + L_Expresiones + parDerecho;

            ASIGNACION.Rule = ID + dospuntos + igualdad + E;            

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
                |E + igualdad + E
                |E + or + E
                |E + and + E
                |not + E
                |parIzquierdo + E + parDerecho
                |menos + E  //para manejar numeros negativos
                |entero
                //|real
                |ttrue
                |tfalse
                |cadena
                |L_IDS_Accesos
                ;

            L_Expresiones.Rule = L_Expresiones + coma + E
                |E
                ;

            L_IDS_Accesos.Rule = L_IDS_Accesos + punto + OTRO_ID
                |OTRO_ID
                ;

            OTRO_ID.Rule = ID + parIzquierdo + parDerecho
                | ID + parIzquierdo + L_Expresiones + parDerecho
                | ID
                //|ID + corAbierto + corCerrado
                ;

            L_IDS.Rule = L_IDS + coma + ID
                |ID
                ;


            //L_IF.Rule = MakePlusRule(L_IF, telse + tif + parIzquierdo + E + parDerecho + tthen + BLOQUE)
            //   | tif + parIzquierdo + E + parDerecho + tthen + BLOQUE;

            #endregion

            #region Preferencias
            this.Root = INICIO;
            #endregion



        }

    }
}
