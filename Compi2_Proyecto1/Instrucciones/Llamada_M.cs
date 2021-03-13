using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Principales;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Objetos;
using Compi2_Proyecto1.Expresiones;
using System.Linq;

namespace Compi2_Proyecto1.Instrucciones
{
    /*
     * Clase para gestionar las llamadas a los procedimientos
     */
    public class Llamada_M : Instruccion
    {
        String nombreFuncion;
        LinkedList<Expresion> enviados; //expresiones enviadas como parametros
        Lista_Accesos id_;

        //Constructor para cuando tenga parametros enviados
        public Llamada_M(String nombreFuncion, LinkedList<Expresion> enviados, int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
            this.nombreFuncion = nombreFuncion;
            this.enviados = enviados;
        }

        //Constructor para cuando no reciba parametros enviados
        public Llamada_M(String nombreFuncion, int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
            this.nombreFuncion = nombreFuncion;
            this.enviados = null;
        }

        //Constructor para cuando tenga parametros enviados y sea Acceso
        public Llamada_M(Lista_Accesos id_, LinkedList<Expresion> enviados, int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
            this.id_ = id_;
            this.enviados = enviados;
        }

        //Constructor para cuando no reciba parametros enviados y sea acceso
        public Llamada_M(Lista_Accesos id_, int linea, int columna)
        {
            this.linea = linea;
            this.columna = columna;
            this.id_ = id_;
            this.enviados = null;
        }


        public override object ejecutar(Entorno ent)
        {   //ent en este caso es el main
            LinkedList<Expresion> resueltos = new LinkedList<Expresion>();

            MasterClass.PilaMF.AddFirst(MasterClass.TipoMF.Metodo_Funcion);

            //Creamos un nuevo entorno para ejecutar el metodo
            Entorno nuevo = new Entorno(ent); //El global es el del objeto

            //crea un clon del acceso y manda null porque no hay lista de accesos
            Lista_Accesos acceso = new Lista_Accesos(null);

            foreach (Id myId in this.id_.accesos) {
                acceso.accesos.AddLast(myId);
            }

            //creamos un literal para poder regresar
            Object retorno = null;

            //Modificamos el nombre
            String aux = "";
            String a2 = acceso.accesos.Last.Value.id;
            String a3 = a2.Substring(0,1);
            if (!a3.Equals("#")) {
                aux = "#";
            }

            String nombreFuncion2 = acceso.accesos.Last.Value.id + aux;
            //ahora ejecutamos las expresiones -> parametros enviados si esque tiene
            if (this.enviados != null) {
                //recorremos los parametros enviados
                //Aqui se le puede enviar un id -> debemos ir a buscarlo
                foreach (Expresion parametro in this.enviados) {

                    var parametroType = parametro.GetType();
                    var listaAccesosType = new Lista_Accesos().GetType();

                    if (parametroType == listaAccesosType)
                    {
                        Lista_Accesos a = (Lista_Accesos)enviados.First.Value;

                        foreach (Id id in a.accesos)
                        {

                            Variable sim = id.getSimbolo(nuevo);
                            Primitivo l = new Primitivo(sim.tipo, sim.valor);
                            nombreFuncion2 += l.tipo.tipo.ToString();
                            resueltos.AddLast(l);
                        }

                    }
                    else {

                        nombreFuncion2 += parametro.getValor(nuevo).tipo.tipo.ToString();
                        resueltos.AddLast(parametro.getValor(nuevo));
                    }

                }
            }

            //adjuntar el último valor modificado para el método
            if (!aux.Equals("", StringComparison.InvariantCultureIgnoreCase)) {

                Id id = acceso.accesos.Last.Value;
                acceso.accesos.RemoveLast();
                acceso.accesos.AddLast(new Id(nombreFuncion2, id.linea, id.columna));

            }

            //obtenemos el entorno global del objeto cuando realizamos la llamada
            if (id_.accesos.Count > 1)
            {

                Entorno nuevo2 = ent.getEntornoAcceso(id_);
                if (nuevo2 != null)
                {
                    nuevo.global = nuevo2;
                }
                else
                {
                    nuevo.global = ent.global;
                }
            }
            else {
                nuevo.global = ent.global;
            }

            Variable f;
            //luego buscamos la funcion
            f = nuevo.buscar(acceso, linea, columna, "El procedimiento");
            //si lo encontro
            if (f != null)
            {
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

                    //Ejecutamos la lista de declaraciones para crear las variables
                    //y le asignamos el valor del enviado correspondiente para que sea asignado de una vez
                    foreach (Instruccion declaracion in ((Tipo_MF)f.valor).getparametros())
                    {

                        ((Declaracion)declaracion).valor = resueltos.ElementAt(iterador);
                        declaracion.ejecutar(nuevo);
                        iterador++;

                    }

                }

                //ejecutamos el bloque de instrucciones
                retorno = ((Tipo_MF)f.valor).getbloque().ejecutar(nuevo);

                //verificamos si enviaron un return
                if (retorno != null)
                {

                    Primitivo sim = (Primitivo)retorno;
                    //validamos el retorno dentro del metodo o funcion
                    if (f.tipo.tipo == Tipo.enumTipo.Void)
                    {
                        //error, porque si retorna algo no debe ser null
                        MasterClass.Instance.addError(new C_Error("Semantico", "No se esperaba retorno en metodo: " + nombreFuncion, linea, columna));
                        retorno = null;
                    }
                    else
                    {

                        //como no es void tiene tipo y verificamos que sea el mismo que la expresion recibida
                        if (f.tipo.tipo != sim.tipo.tipo)
                        {
                            //si no es el mismo entonces es un error
                            MasterClass.Instance.addError(new C_Error("Semantico", "El tipo de retorno y funcion no coinciden: " + sim.tipo.tipo + " = " + f.tipo.tipo, linea, columna));
                            retorno = null;
                        }
                    }
                }

            }
            else {

                MasterClass.Instance.addError(new C_Error("Semantico", "La funcion " + nombreFuncion + " no existe en el contexto", linea, columna));
            }

            MasterClass.PilaMF.RemoveLast();
            return retorno;


        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}
