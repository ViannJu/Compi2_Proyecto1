using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Principales;
using Compi2_Proyecto1.Instrucciones;

namespace Compi2_Proyecto1.Control
{
    public class For : Instruccion
    {
        Instruccion InsInicial; //esto es la asignacion
        Expresion valorFinal;    //este es el valor final
        String nombreVarAsignacion = "";
        bool up_to; // Up to = true; down to = false
        Bloque instrucciones;

        public For(Instruccion InsInicial, String nombreVarAsignacion, Expresion valorFinal, Bloque instrucciones, bool up_to)
        {
            this.InsInicial = InsInicial;
            this.nombreVarAsignacion = nombreVarAsignacion;
            this.valorFinal = valorFinal;
            this.instrucciones = instrucciones;
            this.up_to = up_to;
        }


        public override object ejecutar(Entorno ent)
        {
            //insertamos a la pila de ciclos
            MasterClass.Display.AddFirst(MasterClass.TipoCiclo.Ciclo);

            //creamos un nuevo entorno y ejecutamos la instruccion inicial
            Entorno entIntermedio = new Entorno(ent);
            this.InsInicial.ejecutar(entIntermedio);

            //ahora obtenemos el valor del valorFinal
            Expresion valorFinal = this.valorFinal.getValor(entIntermedio);

            //mandamos a buscar la variable que acabamos de asignar
            Variable variable = entIntermedio.buscar(this.nombreVarAsignacion, this.linea, this.columna, "La variable");

            //pasamos las variables a un tipo integer
            int valorInicial = int.Parse(variable.valor.ToString());
            int valorCondicional = int.Parse(valorFinal.valor.ToString());

            //Bifurcacion Up_to  vs  Down_to
            if (this.up_to)
            {

                //validamos que el valor sea menor o igual que el valor final
                while (valorInicial <= valorCondicional)
                {
                    //MessageBox.Show("To "+valorInicial +" , "+valorCondicional);

                    //creamos un nuevo entorno para ejecutar las instrucciones del bloque
                    Entorno ent3 = new Entorno(entIntermedio);
                    Object retorno = this.instrucciones.ejecutar(ent3);

                    if (retorno != null)
                    {


                    }

                    Tipo tempTipo = new Tipo(Tipo.enumTipo.entero);
                    Primitivo temp = new Primitivo(tempTipo, valorInicial+1);

                    //aumentamos el valor de la variable - Creamos una asignacion con un +1
                    Instruccion cheat = new Asignacion(this.nombreVarAsignacion, temp, this.linea, this.columna);
                    cheat.ejecutar(ent3);

                    //evaluamos de nuevo el valor inicial
                    Variable variable2 = ent3.buscar(this.nombreVarAsignacion, this.linea, this.columna, "La variable");
                    valorInicial = int.Parse(variable2.valor.ToString());
                    //MessageBox.Show("Saliendo" + valorInicial + " , " + valorCondicional);
                }

            }
            else {

                //validamos que el valor sea menor o igual que el valor final
                while (valorInicial >= valorCondicional)
                {
                    //MessageBox.Show("Downto "+valorInicial + " , " + valorCondicional);
                    //creamos un nuevo entorno para ejecutar las instrucciones del bloque
                    Entorno ent3 = new Entorno(entIntermedio);
                    Object retorno = this.instrucciones.ejecutar(ent3);

                    if (retorno != null)
                    {


                    }
                    Tipo tempTipo = new Tipo(Tipo.enumTipo.entero);
                    Primitivo temp = new Primitivo(tempTipo, valorInicial - 1);

                    //aumentamos el valor de la variable - Creamos una asignacion con un +1
                    Instruccion cheat = new Asignacion(this.nombreVarAsignacion, temp, this.linea, this.columna);
                    cheat.ejecutar(ent3);

                    //evaluamos de nuevo el valor inicial
                    Variable variable2 = ent3.buscar(this.nombreVarAsignacion, this.linea, this.columna, "La variable");
                    valorInicial = int.Parse(variable2.valor.ToString());
                    //MessageBox.Show("Saliendo" + valorInicial + " , " + valorCondicional);
                }



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
