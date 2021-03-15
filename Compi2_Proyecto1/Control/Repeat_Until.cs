using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Instrucciones;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Control
{
    public class Repeat_Until : Instruccion
    {
        Expresion condicion;
        LinkedList<Instruccion> listaInstrucciones;
        Bloque bloque;

        public Repeat_Until(LinkedList<Instruccion> listaInstrucciones, Expresion condicion) {

            this.listaInstrucciones = listaInstrucciones;
            this.condicion = condicion;
        }

        public Repeat_Until(Bloque bloque, Expresion condicion) {

            this.bloque = bloque;
            this.condicion = condicion;
            this.listaInstrucciones = this.bloque.listaInstrucciones;
        }

        public Repeat_Until() {

            this.listaInstrucciones = new LinkedList<Instruccion>();
        }

        public override object ejecutar(Entorno ent)
        {
            //añadimos este ciclo a la pila de ciclos para controlar los break y los continue
            MasterClass.Display.AddFirst(MasterClass.TipoCiclo.Ciclo);

            //Creamos un entorno para la primera ejecucion
            Entorno nuevo = new Entorno(ent);

            //Ejecutamos las instrucciones por primera vez
            if (this.listaInstrucciones != null) {

                foreach (Instruccion ins in this.listaInstrucciones) {

                    Object retornar = ins.ejecutar(nuevo);
                    /*Aqui van las validaciones para cuando el return no sea nulo*/
                    if (retornar != null)
                    {
                        if (typeof(Break).IsInstanceOfType(retornar))
                        {

                            //si viene un break se detiene el flujo del ciclo
                            break;

                        }
                        else if (typeof(Continue).IsInstanceOfType(retornar))
                        {

                            //aqui solo se debe continuar el ciclo
                            //continue;
                        }
                        else if (typeof(Primitivo).IsInstanceOfType(retornar))
                        {

                            //Aqui devolvemos el valor del retorno
                            return retornar;
                        }
                    }

                }

            }

            //ahora validamos la condicion
            Expresion valor = this.condicion.getValor(nuevo);
            if (valor.tipo.tipo == Tipo.enumTipo.booleano)
            {

                bool condition = bool.Parse(valor.valor.ToString());

                //entonces ahora se crear el bucle
                while (!condition)
                {
                    //MessageBox.Show("Entre al ciclo");

                    Entorno interno = new Entorno(nuevo); //su padre es el nuevo
                    if (this.listaInstrucciones != null)
                    {

                        foreach (Instruccion ins in this.listaInstrucciones)
                        {

                            Object retorno1 = ins.ejecutar(interno);
                            /*Aqui van las validaciones para cuando el return no sea nulo*/


                        }

                    }

                    //validamos de nuevo la condicion
                    Expresion valor1 = this.condicion.getValor(interno);
                    condition = bool.Parse(valor1.valor.ToString());

                }

            }
            else {

                //si la condicion nunca fue booleana
                MasterClass.Instance.addError(new C_Error("Semantico", "No se puede operar tipo de dato: " + valor.tipo.tipo + " como condicion ", linea, columna));
            }

            MasterClass.Display.RemoveFirst();
            return null;

        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}
