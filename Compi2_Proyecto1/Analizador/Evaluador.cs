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

                foreach (var error in arbol.ParserMessages) {

                    MasterClass.Instance.addError(new C_Error("Lexico/Sintactico", error.Message, error.Location.Line, error.Location.Column));
                }

            }
            else
            {
                MasterClass.Instance.addMessage("/****    Entrada correcta    ****/", true);
                evaluarL_Instrucciones_Principales(raiz.ChildNodes[3]);
            }

        }

        /********** AQUI EMPEZAMOS A CONSTRUIR EL ARBOL DE ANALISIS SINTACTICO **********/

        private void evaluarL_Instrucciones_Principales(ParseTreeNode nodo) {
            if (nodo.ChildNodes.Count == 2) {
                //L_Instrucciones_Principales = L_Instrucciones_Principales + Instruccion_Principal
                evaluarL_Instrucciones_Principales(nodo.ChildNodes[0]);
                evaluarInstruccionPrincipal(nodo.ChildNodes[1]);

            } else if (nodo.ChildNodes.Count == 1) {
                //L_Instrucciones_Principales = InstruccionPrincipal
                evaluarInstruccionPrincipal(nodo.ChildNodes[0]);

            }
        }

        private void evaluarInstruccionPrincipal(ParseTreeNode nodo) {

            switch (nodo.ChildNodes[0].Term.Name) {

                case "BLOQUE":
                    //entonces el bloque tiene instrucciones
                    if (nodo.ChildNodes[0].ChildNodes.Count == 3)
                    {

                        //mandamos a traer la lista de instrucciones
                        LinkedList<Instruccion> listaInsBloque = new LinkedList<Instruccion>();
                        evaluarL_Instrucciones(nodo.ChildNodes[0].ChildNodes[1], true, listaInsBloque);

                        Instruccion temp;
                        temp = new Bloque(listaInsBloque);

                        //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                        MasterClass.Instance.addInstruction(temp);
                        //MessageBox.Show("guarde el bloque en la masterclass");

                    }
                    //entonces el bloque no tiene instrucciones
                    else if (nodo.ChildNodes[0].ChildNodes.Count == 2)
                    {

                        Instruccion temp;
                        temp = new Bloque();

                        //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                        MasterClass.Instance.addInstruction(temp);

                    }
                    break;

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

                        //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                        MasterClass.Instance.addInstruction(temp);

                    }
                    else
                    {
                        //Declaracion/Asignacion
                        //tvar + L_IDS + dospuntos + TIPO + igualdad + E

                        //string identificador = nodo.ChildNodes[0].ChildNodes[1].Token.ValueString;

                        //Mando a traer al identificador en L_IDS.Count = 1
                        LinkedList<String> listaIDS = new LinkedList<string>();
                        evaluarL_IDS(nodo.ChildNodes[0].ChildNodes[1], listaIDS);
                        String identificador = "";

                        if (listaIDS.Count == 1)
                        {

                            identificador = listaIDS.First.Value;
                        }

                        //Tengo que mandar el tipo
                        Tipo tipe = evaluarTipo(nodo.ChildNodes[0].ChildNodes[3]);

                        //Tengo que mandar la Expresion
                        Expresion expression = evaluarExpresion(nodo.ChildNodes[0].ChildNodes[5]);

                        //Mandamos la declaracion
                        Instruccion temp;
                        temp = new Declaracion(tipe, identificador, expression, 0, 0);

                        //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                        MasterClass.Instance.addInstruction(temp);


                    }

                    break;

                case "DECLARACION_MF":

                    //tfunction + ID + CUERPO_PARAMETROS + dospuntos + TIPO + CUERPO_INTERNO
                    if (nodo.ChildNodes[0].ChildNodes.Count == 6) {

                        //Mando a traer el ID
                        String nombre = nodo.ChildNodes[0].ChildNodes[1].Token.ValueString;

                        //Mando a traer el cuerpo de los parametros
                        LinkedList<Instruccion> listaDeclaraciones = new LinkedList<Instruccion>();
                        evaluarCuerpoParametros(nodo.ChildNodes[0].ChildNodes[2], listaDeclaraciones);

                        //Mando a traer el tipo
                        Tipo tipe = evaluarTipo(nodo.ChildNodes[0].ChildNodes[4]);

                        //Mando a traer el cuerpo interno de la declaracionMF
                        LinkedList<Instruccion> listaInstruccionesMF = new LinkedList<Instruccion>();
                        evaluarL_InstruccionesMF(nodo.ChildNodes[0].ChildNodes[5], listaInstruccionesMF);

                        Instruccion temp;
                        if (listaDeclaraciones.Count == 0)
                        {
                            //no tiene parametros
                            temp = new Declaracion_MF(nombre, tipe, listaInstruccionesMF, 0, 0);
                        }
                        else {
                            temp = new Declaracion_MF(nombre, listaDeclaraciones, tipe, listaInstruccionesMF, 0, 0);
                        }

                        MasterClass.Instance.addInstruction(temp);

                    }
                    //tprocedure + ID + CUERPO_PARAMETROS + CUERPO_INTERNO
                    else if (nodo.ChildNodes[0].ChildNodes.Count == 4) {

                        //Mando a traer el ID
                        String nombre = nodo.ChildNodes[0].ChildNodes[1].Token.ValueString;

                        //Mando a traer el cuerpo de los parametros
                        LinkedList<Instruccion> listaDeclaraciones = new LinkedList<Instruccion>();
                        evaluarCuerpoParametros(nodo.ChildNodes[0].ChildNodes[2], listaDeclaraciones);

                        //Mando a traer el tipo
                        Tipo tipe = new Tipo(Tipo.enumTipo.Void);

                        //Mando a traer el cuerpo interno de la declaracionMF
                        LinkedList<Instruccion> listaInstruccionesMF = new LinkedList<Instruccion>();
                        evaluarL_InstruccionesMF(nodo.ChildNodes[0].ChildNodes[3], listaInstruccionesMF);

                        Instruccion temp;
                        if (listaDeclaraciones.Count == 0)
                        {
                            //no tiene parametros
                            temp = new Declaracion_MF(nombre, tipe, listaInstruccionesMF, 0, 0);
                        }
                        else
                        {
                            temp = new Declaracion_MF(nombre, listaDeclaraciones, tipe, listaInstruccionesMF, 0, 0);
                        }

                        MasterClass.Instance.addInstruction(temp);
                    }
                    break;

            }

        }

        private void evaluarL_InstruccionesMF(ParseTreeNode nodo, LinkedList<Instruccion> listaInstruccionesMF) {

            //ptcoma + L_DECLARACIONES + BLOQUE
            if (nodo.ChildNodes.Count == 3) {

                //Mando a traer la lista de declaraciones
                LinkedList<Instruccion> listaDeclaraciones = new LinkedList<Instruccion>();
                evaluarL_Declaraciones(nodo.ChildNodes[1], listaInstruccionesMF);

                //Mando a traer el bloque
                Bloque bloque = evaluarBloque(nodo.ChildNodes[2]);

                //guardamos todo en la lista de instrucciones
                foreach (Instruccion declaracion in listaDeclaraciones) {
                    listaInstruccionesMF.AddLast(declaracion);
                }

                listaInstruccionesMF.AddLast(bloque);

            }
            //ptcoma + BLOQUE
            else if (nodo.ChildNodes.Count == 2) {

                //Mando a traer el bloque
                Bloque bloque = evaluarBloque(nodo.ChildNodes[1]);

                //guardamos el bloque en la lista
                listaInstruccionesMF.AddLast(bloque);

            }

            
        }

        private void evaluarL_Declaraciones(ParseTreeNode nodo, LinkedList<Instruccion> listaDeclaraciones) {

            //L_DECLARACIONES + DECLARACION + ptcoma
            if (nodo.ChildNodes.Count == 3) {

                evaluarL_Declaraciones(nodo.ChildNodes[0], listaDeclaraciones);
                evaluarDeclaracion(nodo.ChildNodes[1], listaDeclaraciones);

            }
            //DECLARACION + ptcoma
            else if (nodo.ChildNodes.Count == 2) {

                evaluarDeclaracion(nodo.ChildNodes[0], listaDeclaraciones);
            }

            
        }

        private void evaluarDeclaracion(ParseTreeNode nodo, LinkedList<Instruccion> listaDeclaraciones) {

            //tvar + L_IDS + dospuntos + TIPO   
            if (nodo.ChildNodes.Count == 4)
            {
                //tvar + L_IDS + dospuntos + TIPO
                //Lista de declaraciones
                LinkedList<string> listaIDS = new LinkedList<string>();

                //tengo que mandar a traer la lista de ID's
                evaluarL_IDS(nodo.ChildNodes[1], listaIDS);

                //MasterClass.Instance.addMessage("La lista de ids tiene: " + listaIDS.Count, true);

                //Tengo que mandar el tipo 
                Tipo tipe = evaluarTipo(nodo.ChildNodes[3]);

                Instruccion temp;
                temp = new Declaracion(tipe, listaIDS, 0, 0);

                //mandarle a declaracion -> se guarda en una lista de instrucciones
                listaDeclaraciones.AddLast(temp);

            }
            //tvar + L_IDS + dospuntos + TIPO + igualdad + E
            else if(nodo.ChildNodes.Count == 6)
            {
                //Mando a traer al identificador en L_IDS.Count = 1
                LinkedList<String> listaIDS = new LinkedList<string>();
                evaluarL_IDS(nodo.ChildNodes[1], listaIDS);
                String identificador = "";

                if (listaIDS.Count == 1)
                {

                    identificador = listaIDS.First.Value;
                }

                //Tengo que mandar el tipo
                Tipo tipe = evaluarTipo(nodo.ChildNodes[3]);

                //Tengo que mandar la Expresion
                Expresion expression = evaluarExpresion(nodo.ChildNodes[5]);

                //Mandamos la declaracion
                Instruccion temp;
                temp = new Declaracion(tipe, identificador, expression, 0, 0);

                //mandarle a declaracion -> se guarda en una lista de instrucciones 
                listaDeclaraciones.AddLast(temp);


            }
        }

        private void evaluarCuerpoParametros(ParseTreeNode nodo, LinkedList<Instruccion> listaDeclaracionesEspeciales) {

            //parIzquierdo + L_DECLARACIONES_ESPECIALES + parDerecho
            if (nodo.ChildNodes.Count == 3) {

                //Mando a traer la lista de declaraciones especiales
                evaluarL_DECLARACIONES_ESPECIALES(nodo.ChildNodes[1], listaDeclaracionesEspeciales);
            }
            //parIzquierdo + parDerecho
            else if (nodo.ChildNodes.Count == 2) {

                //retorno la lista de declaraciones especiales asi como viene
            }   
        }

        private void evaluarL_DECLARACIONES_ESPECIALES(ParseTreeNode nodo, LinkedList<Instruccion> listaDeclaracionesEspeciales) {

            //L_DECLARACIONES_ESPECIALES + ptcoma + DECLARACION_ESPECIAL
            if (nodo.ChildNodes.Count == 3) {

                evaluarL_DECLARACIONES_ESPECIALES(nodo.ChildNodes[0], listaDeclaracionesEspeciales);
                evaluarDeclaracionEspecial(nodo.ChildNodes[2], listaDeclaracionesEspeciales);
            }
            //DECLARACION_ESPECIAL
            else if (nodo.ChildNodes.Count == 1) {

                evaluarDeclaracionEspecial(nodo.ChildNodes[0], listaDeclaracionesEspeciales);
            }
            
                
        }

        private void evaluarDeclaracionEspecial(ParseTreeNode nodo, LinkedList<Instruccion> listaDeclaraciones) {

            //L_IDS + dospuntos + TIPO
            if (nodo.ChildNodes.Count == 3) {

                //Mando a traer la lista de IDS
                LinkedList<string> listaIDS = new LinkedList<string>();
                evaluarL_IDS(nodo.ChildNodes[0], listaIDS);

                //Mando a traer el tipo
                Tipo tipo = evaluarTipo(nodo.ChildNodes[2]);

                //Guardamos en la lista de declaraciones
                Instruccion temp;
                temp = new Declaracion(tipo, listaIDS, 0, 0);
                listaDeclaraciones.AddLast(temp);

            }
            //tvar + L_IDS + dospuntos + TIPO
            else if (nodo.ChildNodes.Count == 4) {

                //Mando a traer la lista de IDS
                LinkedList<String> listaIDS = new LinkedList<string>();
                evaluarL_IDS(nodo.ChildNodes[1], listaIDS);

                //Mando a traer el tipo
                Tipo tipo = evaluarTipo(nodo.ChildNodes[3]);

                bool VariablePorReferencia = true;

                //Guardamos en la lista de declaraciones
                Instruccion temp;
                temp = new Declaracion(tipo, listaIDS, VariablePorReferencia, 0, 0);
                listaDeclaraciones.AddLast(temp);

            }
            
        }

        private void evaluarL_Instrucciones(ParseTreeNode nodo, bool bloque, LinkedList<Instruccion> guardadosBloque)
        {
            if (nodo.ChildNodes.Count == 2)
            {
                //L_Instrucciones_Principales = L_Instrucciones_Principales + Instruccion_Principal
                evaluarL_Instrucciones(nodo.ChildNodes[0], bloque, guardadosBloque);
                evaluarInstruccion(nodo.ChildNodes[1], bloque, guardadosBloque);

            }
            else if (nodo.ChildNodes.Count == 1)
            {
                //L_Instrucciones_Principales = InstruccionPrincipal
                evaluarInstruccion(nodo.ChildNodes[0], bloque, guardadosBloque);

            }
        }

        private void evaluarInstruccion(ParseTreeNode nodo, bool bloque, LinkedList<Instruccion> guardadosInstruccion) {

            //Bifurcacion de metodos dependiendo de la instruccion

            //Como se llama la instruccion que viene
            switch (nodo.ChildNodes[0].Term.Name) {

                case "LLAMADA_M":
                    //L_IDS_Accesos + parIzquierdo + parDerecho
                    if (nodo.ChildNodes[0].ChildNodes.Count == 3)
                    {
                        //Mando a traer la lista de accesos
                        LinkedList<Id> listaAccesos = new LinkedList<Id>();
                        evaluarL_IDS_Accesos(nodo.ChildNodes[0].ChildNodes[0], listaAccesos);

                        //creamos la instruccion y la insertamos
                        Lista_Accesos milistaAccesos = new Lista_Accesos(listaAccesos);
                        Instruccion temp;
                        temp = new Llamada_M(milistaAccesos, 0,0);

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
                    //L_IDS_Accesos + parIzquierdo + L_Expresiones + parDerecho
                    else if(nodo.ChildNodes[0].ChildNodes.Count == 4)
                    {
                        //Mando a traer la lista de accesos
                        LinkedList<Id> listaAccesos = new LinkedList<Id>();
                        evaluarL_IDS_Accesos(nodo.ChildNodes[0].ChildNodes[0], listaAccesos);
                        Lista_Accesos milistaAccesos = new Lista_Accesos(listaAccesos);

                        //Mando a traer la lista de expresiones
                        LinkedList<Expresion> listaExpresiones = new LinkedList<Expresion>();
                        evaluarL_Expresiones(nodo.ChildNodes[0].ChildNodes[2], listaExpresiones);

                        //creamos el objeto instruccion
                        Instruccion temp;
                        temp = new Llamada_M(milistaAccesos, listaExpresiones, 0, 0);

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
                    break;



                //tfor + ASIGNACION + tto + E + tdo + BLOQUE;
                case "SENT_FOR":

                    //verificamos el valor del hijo2

                    //Mando a traer el objeto Asignacion como instruccion
                    Instruccion myAsignacion = evaluarAsignacion(nodo.ChildNodes[0].ChildNodes[1]);

                    //Mando a traer el id del objeto asignacion
                    String identificadorAsignacion = nodo.ChildNodes[0].ChildNodes[1].ChildNodes[0].Token.ValueString;

                    //Mando a traer la expresion del valor final
                    Expresion exp4 = evaluarExpresion(nodo.ChildNodes[0].ChildNodes[3]);

                    //Mando a traer el bloque de instrucciones
                    Bloque bloque4 = evaluarBloque(nodo.ChildNodes[0].ChildNodes[5]);

                    bool incremental = true;
                    //Mandamos si es Up_to
                    if (nodo.ChildNodes[0].ChildNodes[2].Term.Name == "to")
                    {
                        incremental = true;
                    }
                    else if (nodo.ChildNodes[0].ChildNodes[2].Term.Name == "downto") {

                        incremental = false;
                    }



                    //debo enviar Instruccion InsInicial, String nombreVarAsignacion, Expresion valorFinal, Bloque instrucciones, bool up_to
                    Instruccion temp4 = new For(myAsignacion, identificadorAsignacion, exp4, bloque4, incremental);

                    //Si es un bloque se guarda en la lista de instrucciones de bloque
                    if (bloque)
                    {
                        guardadosInstruccion.AddLast(temp4);
                    }
                    //Si no es un bloque es la lista general de instrucciones de MasterClass
                    else
                    {
                        //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                        MasterClass.Instance.addInstruction(temp4);
                        //MessageBox.Show("guarde el bloque en la masterclass");
                    }
                    break;

                case "SENT_REPEAT_UNTIL":
                    //trepeat + BLOQUE + tuntil + E;

                    //Mando a traer el bloque
                    Bloque block1 = evaluarBloque(nodo.ChildNodes[0].ChildNodes[1]);

                    //Mando a traer la expresion
                    Expresion exp3 = evaluarExpresion(nodo.ChildNodes[0].ChildNodes[3]);

                    Instruccion temp3;
                    temp3 = new Repeat_Until(block1, exp3);

                    //Si es un bloque se guarda en la lista de instrucciones de bloque
                    if (bloque)
                    {
                        guardadosInstruccion.AddLast(temp3);
                    }
                    //Si no es un bloque es la lista general de instrucciones de MasterClass
                    else
                    {
                        //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                        MasterClass.Instance.addInstruction(temp3);
                        //MessageBox.Show("guarde el bloque en la masterclass");
                    }
                    break;


                case "SENT_SWITCH":
                    //tcase + parIzquierdo + E + parDerecho + tof + L_BLOQUES_CASE + tend

                    //Mando a traer la expresion 
                    Expresion exp2 = evaluarExpresion(nodo.ChildNodes[0].ChildNodes[2]);

                    //Mando a traer la lista de bloques
                    LinkedList<Bloque> listaBloques = new LinkedList<Bloque>();
                    evaluarBloques_Case(nodo.ChildNodes[0].ChildNodes[5], listaBloques);

                    Instruccion temp2;
                    temp2 = new Switch(exp2, listaBloques);


                    //Si es un bloque se guarda en la lista de instrucciones de bloque
                    if (bloque)
                    {
                        guardadosInstruccion.AddLast(temp2);
                    }
                    //Si no es un bloque es la lista general de instrucciones de MasterClass
                    else
                    {
                        //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                        MasterClass.Instance.addInstruction(temp2);
                        //MessageBox.Show("guarde el bloque en la masterclass");
                    }
                    break;


                case "SENT_WHILE":
                    //twhile + E + tdo + BLOQUE;

                    //Mando a traer la expresion de condicion
                    Expresion exp1 = evaluarExpresion(nodo.ChildNodes[0].ChildNodes[1]);

                    //Mando a traer el bloque de sentencias
                    Bloque lego = evaluarBloque(nodo.ChildNodes[0].ChildNodes[3]);

                    Instruccion temp1;
                    temp1 = new While(exp1, lego, 0, 0);

                    //Si es un bloque se guarda en la lista de instrucciones de bloque
                    if (bloque)
                    {
                        guardadosInstruccion.AddLast(temp1);
                    }
                    //Si no es un bloque es la lista general de instrucciones de MasterClass
                    else
                    {
                        //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                        MasterClass.Instance.addInstruction(temp1);
                        //MessageBox.Show("guarde el bloque en la masterclass");
                    }
                    break;


                case "SENT_IF":
                    //L_IF + telse + BLOQUE  -> tiene else
                    if (nodo.ChildNodes[0].ChildNodes.Count == 3) {

                        //Mando a traer la lista de condiciones if
                        LinkedList<Condicion_If> listacondiciones = new LinkedList<Condicion_If>();
                        evaluarListaCondiciones_IF(nodo.ChildNodes[0], listacondiciones);

                        //Mando a traer la lista de instruccion del bloque
                        Bloque block = evaluarBloque(nodo.ChildNodes[0].ChildNodes[2]);

                        Instruccion temp;
                        temp = new Clase_IF(listacondiciones, block);

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
                    //L_IF
                    else if (nodo.ChildNodes[0].ChildNodes.Count == 1) {

                        //Mando a traer la lista de condiciones if
                        LinkedList<Condicion_If> listacondiciones = new LinkedList<Condicion_If>();
                        evaluarListaCondiciones_IF(nodo.ChildNodes[0], listacondiciones);

                        Instruccion temp;
                        temp = new Clase_IF(listacondiciones);

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
                         
                    break;


                case "ASIGNACION":
                    //ID + dospuntos + igualdad + E;

                    //por el momento solo le envio el id de la variable y la expresion
                    String nombreVar = nodo.ChildNodes[0].ChildNodes[0].Token.ValueString;
                    //MessageBox.Show("El nombre de la variable es: "+nombreVar);

                    //Mando a traer la expresion
                    Expresion exp = evaluarExpresion(nodo.ChildNodes[0].ChildNodes[3]);

                    //Creo el objeto instruccion
                    Instruccion temporal;
                    temporal = new Asignacion(nombreVar, exp, 0,0);

                    //Si es un bloque se guarda en la lista de instrucciones de bloque
                    if (bloque)
                    {
                        guardadosInstruccion.AddLast(temporal);
                    }
                    //Si no es un bloque es la lista general de instrucciones de MasterClass
                    else
                    {
                        //mandarle a declaracion -> se guarda en una lista de instrucciones para su ejecucion en la master class
                        MasterClass.Instance.addInstruction(temporal);
                        //MessageBox.Show("guarde el bloque en la masterclass");
                    }


                    break;


                case "IMPRESION":

                    //no tiene salto de linea
                    //twrite + parIzquierdo + L_Expresiones + parDerecho
                    if (nodo.ChildNodes[0].ChildNodes[0].Term.Name == "write") {

                        //primero mando a traer la lista de expresiones;
                        LinkedList<Expresion> listaExpresiones = new LinkedList<Expresion>();
                        listaExpresiones = evaluarL_Expresiones(nodo.ChildNodes[0].ChildNodes[2], listaExpresiones);

                        Instruccion temp;
                        temp = new Impresion(listaExpresiones, false);

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
                    //twriteln + parIzquierdo + L_Expresiones + parDerecho;
                    else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name == "writeln") {

                        //primero mando a traer la lista de Expresiones
                        LinkedList<Expresion> listaExpresiones = new LinkedList<Expresion>();
                        listaExpresiones = evaluarL_Expresiones(nodo.ChildNodes[0].ChildNodes[2], listaExpresiones);

                        Instruccion temp;
                        temp = new Impresion(listaExpresiones, true);

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

        private Instruccion evaluarAsignacion(ParseTreeNode nodo) {

            //ID + dospuntos + igualdad + E;

            //por el momento solo le envio el id de la variable y la expresion
            String nombreVar = nodo.ChildNodes[0].Token.ValueString;
            //MessageBox.Show("El nombre de la variable es: "+nombreVar);

            //Mando a traer la expresion
            Expresion exp = evaluarExpresion(nodo.ChildNodes[3]);

            //Creo el objeto instruccion
            Instruccion temporal = new Asignacion(nombreVar, exp, 0, 0);

            return temporal;

        }

        private void evaluarBloques_Case(ParseTreeNode nodo, LinkedList<Bloque> listaBloques) {
            //L_BLOQUES_CASE + BLOQUE_DEFAULT
            if (nodo.ChildNodes.Count == 2) {

                evaluarBloques_Case(nodo.ChildNodes[0], listaBloques);
                evaluarBloques_Default(nodo.ChildNodes[1], listaBloques);

            } 
            //BLOQUES_DEFAULT
            else if (nodo.ChildNodes.Count == 1) {

                evaluarBloques_Default(nodo.ChildNodes[0], listaBloques);

            }

        }

        private void evaluarBloques_Default(ParseTreeNode nodo, LinkedList<Bloque> listaBloques) {

            //E + dospuntos + BLOQUE
            if (nodo.ChildNodes.Count == 3) {

                //Mando a traer la expresion
                Expresion exp = evaluarExpresion(nodo.ChildNodes[0]);

                //Mando a traer el Bloque beginEnd
                Bloque bloque = evaluarBloque(nodo.ChildNodes[2]);

                //Mando a traer la lista de instruccioes

                //creamos el objeto bloque 
                Bloques_Case littleCase = new Bloques_Case(exp,bloque);

                //guardamos en la lista
                listaBloques.AddLast(littleCase);

            }
            //telse + BLOQUE;
            else if (nodo.ChildNodes.Count == 2) {

                //Mandamos a traer el bloque
                Bloque bloque = evaluarBloque(nodo.ChildNodes[1]);

                //creamos el objeto bloque
                Bloques_Default littleDefault = new Bloques_Default(bloque);

                //guardamos en la lista
                listaBloques.AddLast(littleDefault);
            }

        }

        private LinkedList<Condicion_If> evaluarListaCondiciones_IF(ParseTreeNode nodo, LinkedList<Condicion_If> listaCondicion) {

            //L_IF + telse + tif + parIzquierdo + E + parDerecho + then + BLOQUE
            if (nodo.ChildNodes[0].ChildNodes.Count == 8) {

                listaCondicion = evaluarListaCondiciones_IF(nodo.ChildNodes[0], listaCondicion);
                Expresion exp = evaluarExpresion(nodo.ChildNodes[0].ChildNodes[4]);

                //Mando a traer las instrucciones del bloque
                Bloque block = evaluarBloque(nodo.ChildNodes[0].ChildNodes[7]);

                Condicion_If condition = new Condicion_If(exp, block);
                listaCondicion.AddLast(condition);

                return listaCondicion;

            }
            //tif + parIzquierdo + E + parDerecho + tthen + BLOQUE
            else if (nodo.ChildNodes[0].ChildNodes.Count == 6) {

                //mando a traer la expresion
                Expresion exp = evaluarExpresion(nodo.ChildNodes[0].ChildNodes[2]);

                //Mando a traer las instrucciones del bloque
                Bloque block = evaluarBloque(nodo.ChildNodes[0].ChildNodes[5]);

                //devuelvo una lista de objetos condicion
                Condicion_If condition = new Condicion_If(exp, block);
                listaCondicion.AddLast(condition);

                return listaCondicion;

            }

            return listaCondicion;
            

        }

        private Bloque evaluarBloque(ParseTreeNode nodo) {

            Bloque lego;
            LinkedList<Instruccion> mylistaInstrucciones = new LinkedList<Instruccion>();
            //si tiene instrucciones
            if (nodo.ChildNodes.Count == 3) {

                evaluarL_Instrucciones(nodo.ChildNodes[1], true, mylistaInstrucciones);
                lego = new Bloque(mylistaInstrucciones);
                return lego;
            }

             lego = new Bloque();
             return lego;            

        }

        private LinkedList<Instruccion> evaluarL_Instrucciones_Bloque(ParseTreeNode nodo) {

            LinkedList<Instruccion> mylistaInstrucciones = new LinkedList<Instruccion>();

            evaluarL_Instrucciones(nodo.ChildNodes[1], true, mylistaInstrucciones);

            return mylistaInstrucciones;
        }

        private LinkedList<Expresion> evaluarL_Expresiones(ParseTreeNode nodo, LinkedList<Expresion> listaExpresiones) {
            //L_Expresiones + coma + E
            if (nodo.ChildNodes.Count == 3) {

                evaluarL_Expresiones(nodo.ChildNodes[0], listaExpresiones);
                //El siguiente es una expresion entonces lo guardamos en la lista
                Expresion expri = evaluarExpresion(nodo.ChildNodes[2]);

                listaExpresiones.AddLast(expri);

            }
            //E
            else if (nodo.ChildNodes.Count == 1) {

                //Mando a traer la expresion
                Expresion expri = evaluarExpresion(nodo.ChildNodes[0]);

                //lo guardamos en la lista
                listaExpresiones.AddLast(expri);
            }

            return listaExpresiones;

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
                    case "=":
                        return new Relacional(evaluarExpresion(nodo.ChildNodes[0]), evaluarExpresion(nodo.ChildNodes[2]), "=", 0, 0);
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

                case "entero":

                    if (nodo.Token.Value.GetType() == Type.GetType("System.Double")) {

                        return new Primitivo(new Tipo(Tipo.enumTipo.real), float.Parse(nodo.Token.ValueString));
                    }
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
                //L_IDS = L_IDS + punto + OTRO_ID
                evaluarL_IDS_Accesos(nodo.ChildNodes[0], lista);

                lista.AddLast(evaluarOTRO_ID(nodo.ChildNodes[2]));

                //el siguiente nodo es un valor primitivo entonces guardamos en la lista
                //IDobjeto = evaluarID_Acceso(nodo.ChildNodes[2]);
                //lista.AddLast(IDobjeto);
            }
            //OTRO_ID
            else
            {
                lista.AddLast(evaluarOTRO_ID(nodo.ChildNodes[0]));
                //Id IDobjeto;
                //L_IDS = ID //El siguiente nodo es un valor primitivo entonces guardamos en la lista
                //IDobjeto = evaluarID_Acceso(nodo.ChildNodes[0]);
                //lista.AddLast(IDobjeto);
            }

            return lista;

        }

        private Id evaluarOTRO_ID(ParseTreeNode nodo) {

            //ID + parIzquierdo + parDerecho
            if (nodo.ChildNodes.Count == 3)
            {
                //capturo el nombre porque es una funcion
                String nombreFuncion = nodo.ChildNodes[0].Token.ValueString;
                return new Llamada_M_E(nombreFuncion, 0, 0);
            }
            //ID + parIzquierdo + L_Expresiones + parDerecho
            else if (nodo.ChildNodes.Count == 4)
            {
                //capturo el nombre
                String nombreFuncion = nodo.ChildNodes[0].Token.ValueString;

                //evaluar la lista de expresiones
                LinkedList<Expresion> listaExpresiones = new LinkedList<Expresion>();
                evaluarL_Expresiones(nodo.ChildNodes[2], listaExpresiones);

                return new Llamada_M_E(nombreFuncion, listaExpresiones, 0, 0);
            }
            //ID
            else /*if (nodo.ChildNodes.Count == 1)*/
            {
                return new Id(nodo.ChildNodes[0].Token.ValueString,0,0);
            }
            
            
        }

        private Id evaluarID_Acceso(ParseTreeNode nodo)
        {
            return new Id(nodo.Token.ValueString, 0, 0);
        }





    }
}
