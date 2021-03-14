using Compi2_Proyecto1.Principales;
using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Objetos;
using Compi2_Proyecto1.Instrucciones;
using System.Linq;

namespace Compi2_Proyecto1.Expresiones
{
    public class Llamada_M_E : Id
    {
        LinkedList<Expresion> enviados; //expresiones enviadas como parametros

        public Llamada_M_E(string id, LinkedList<Expresion> enviados, int linea, int columna):base(id, linea, columna)
        {
            this.enviados = enviados;
        }

        public Llamada_M_E(string id, int linea, int columna) : base(id, linea, columna)
        {
            this.enviados = new LinkedList<Expresion>();
        }

        public override string getDot()
        {
            return base.getDot();
        }


        public override Expresion getValor(Entorno ent)
        {
            //ent en este caso es el main
            LinkedList<Expresion> resueltos = new LinkedList<Expresion>();

            MasterClass.PilaMF.AddFirst(MasterClass.TipoMF.Metodo_Funcion);

            //Creamos un nuevo entorno para ejecutar el metodo
            Entorno nuevo = new Entorno(ent); //El global es el del objeto


            //creamos un literal para poder regresar
            Expresion retorno = null;

            //Modificamos el nombre
            String aux = "";
            String a2 = this.id;
            String a3 = a2.Substring(0, 1);
            if (!a3.Equals("#"))
            {
                aux = "#";
            }



            String nombreFuncion2 = this.id + aux;
            //ahora ejecutamos las expresiones -> parametros enviados si esque tiene
            if (this.enviados != null)
            {
                //recorremos los parametros enviados
                //Aqui se le puede enviar un id -> debemos ir a buscarlo
                foreach (Expresion parametro in this.enviados)
                {

                    var parametroType = parametro.GetType();
                    var listaAccesosType = new Lista_Accesos().GetType();

                    if (typeof(Lista_Accesos).IsInstanceOfType(parametro))
                    {
                        Lista_Accesos a = (Lista_Accesos)parametro;
                        Expresion retAcceso = a.getValor(nuevo);

                        nombreFuncion2 += retAcceso.tipo.tipo.ToString();
                        resueltos.AddLast(retAcceso);

                        /*
                        foreach (Id id in a.accesos)
                        {
                            Variable sim = id.getSimbolo(nuevo);
                            Primitivo l = new Primitivo(sim.tipo, sim.valor);
                            nombreFuncion2 += l.tipo.tipo.ToString();
                            resueltos.AddLast(l);
                        }
                        */

                    }
                    else
                    {

                        nombreFuncion2 += parametro.getValor(nuevo).tipo.tipo.ToString();
                        resueltos.AddLast(parametro.getValor(nuevo));
                    }

                }
            }

            //adjuntar el último valor modificado para el método
            /*
            if (!aux.Equals("", StringComparison.InvariantCultureIgnoreCase))
            {

                Id id = acceso.accesos.Last.Value;
                acceso.accesos.RemoveLast();
                acceso.accesos.AddLast(new Id(nombreFuncion2, id.linea, id.columna));

            }
            */

            
            nuevo.global = ent.global;


            Variable f;
            //luego buscamos la funcion
            f = nuevo.buscar(nombreFuncion2, linea, columna, "La Funcion");
            //si lo encontro
            if (f != null)
            {
                //aqui pedimos el tipo
                //creamos una variable (exit) 
                LinkedList<String> lista = new LinkedList<String>();
                lista.AddLast(id);
                Declaracion nombreFuncionDec = new Declaracion(f.tipo, lista, 0, 0);
                nombreFuncionDec.ejecutar(nuevo);

                //creamos un iterador
                int iterador = 0;
                //si tiene parametros y tambien tiene parametros enviados
                //transformamos el simbolo encontrado a un simbolo nuevo de tipo metodo_Funcion para obtener la lista de parametros

                if (enviados != null && ((Tipo_MF)f.valor).getparametros() != null)
                {

                    //resolvemos los parametros enviados para tener el valor
                    foreach (Expresion enviado in enviados)
                    {
                        resueltos.AddLast(enviado.getValor(ent));
                    }

                    int cont = 0;

                    //Ejecutamos la lista de declaraciones para crear las variables
                    //y le asignamos el valor del enviado correspondiente para que sea asignado de una vez
                    foreach (Instruccion declaracion in ((Tipo_MF)f.valor).getparametros())
                    {
                        //((Declaracion)declaracion).valor = resueltos.ElementAt(iterador);

                        Tipo tipe = new Tipo(((Declaracion)declaracion).tipo.tipo);

                        foreach (String identify in ((Declaracion)declaracion).IDS)
                        {//Identificador a,b:integer m:integer

                            if (((Declaracion)declaracion).PorReferencia)
                            {

                                if (typeof(Lista_Accesos).IsInstanceOfType(this.enviados.ElementAt(cont)))
                                {
                                    Lista_Accesos accesopos = (Lista_Accesos)this.enviados.ElementAt(cont);

                                    Entorno temporal = nuevo;
                                    Variable sim = null;

                                    for (var temp = accesopos.accesos.First; temp != null; temp = temp.Next)
                                    {
                                        sim = temporal.buscar(temp.Value.id, linea, columna, "La variable");

                                        if (sim != null)
                                        {
                                            if (temp.Next != null)
                                            {

                                                //esperamos un objeto
                                            }
                                        }
                                        else
                                        {
                                            //error no se encontro la variable
                                            return null;
                                        }


                                    }

                                    if (tipe.tipo == sim.tipo.tipo && sim.tipo.referencia == tipe.referencia)
                                    {


                                        nuevo.insertar(identify, sim, linea, columna, "La variable");
                                    }

                                }
                                else
                                {
                                    //Se esperaba un valor por referencia
                                }

                            }
                            else
                            {

                                Declaracion temp = new Declaracion(tipe, identify, resueltos.ElementAt(iterador), 0, 0);

                                ((Declaracion)temp).valor = resueltos.ElementAt(iterador);//Expresion 1,2
                                temp.ejecutar(nuevo);
                                iterador++;
                            }

                            cont += 1;
                        }


                    }

                }

                /*
                 * Ejecucion como tal de la lista de declaraciones y de la lista de instrucciones (bloque)
                 */

                if (((Tipo_MF)f.valor).listaDeclaraciones != null)
                {

                    foreach (Instruccion declaration in ((Tipo_MF)f.valor).listaDeclaraciones)
                    {

                        declaration.ejecutar(nuevo);
                    }

                }

                if (((Tipo_MF)f.valor).getbloque() == null)
                {

                    foreach (Instruccion ins in ((Tipo_MF)f.valor).listaInstrucciones)
                    {
                        retorno = (Expresion)ins.ejecutar(nuevo);
                    }
                }
                else
                {

                    //ejecutamos el bloque de instrucciones
                    retorno = (Expresion)((Tipo_MF)f.valor).getbloque().ejecutar(nuevo);
                }



                //verificamos si enviaron un return
                if (retorno != null)
                {

                    Primitivo sim = (Primitivo)retorno;
                    //validamos el retorno dentro del metodo o funcion
                    if (f.tipo.tipo == Tipo.enumTipo.Void && ((Expresion)retorno).tipo.tipo != Tipo.enumTipo.Void)  //viene exit vacio
                    {
                        //error, porque si retorna algo no debe ser null
                        MasterClass.Instance.addError(new C_Error("Semantico", "No se esperaba retorno en metodo: " + nombreFuncion2, linea, columna));
                        retorno = new Primitivo(new Tipo(Tipo.enumTipo.error), "@error@");
                    }
                    else
                    {

                        //como no es void tiene tipo y verificamos que sea el mismo que la expresion recibida
                        if (f.tipo.tipo != sim.tipo.tipo)
                        {
                            //si no es el mismo entonces es un error
                            MasterClass.Instance.addError(new C_Error("Semantico", "El tipo de retorno y funcion no coinciden: " + sim.tipo.tipo + " = " + f.tipo.tipo, linea, columna));
                            retorno = new Primitivo(new Tipo(Tipo.enumTipo.error), "@error@");
                        }
                    }
                }
                else {

                    //buscamos la variable con el mismo de la funcion
                    Variable nombreFuncionVar = nuevo.buscar(id, linea, columna, "La variable");
                    retorno = new Primitivo(f.tipo, nombreFuncionVar.valor);

                }

            }
            else
            {

                MasterClass.Instance.addError(new C_Error("Semantico", "El metodo " + nombreFuncion2 + " no existe en el contexto", linea, columna));
            }

            MasterClass.PilaMF.RemoveLast();
            return retorno;

        }
    }
}
