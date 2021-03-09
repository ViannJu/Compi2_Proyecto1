using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Instrucciones
{
    public class Asignacion : Instruccion
    {
        String id;
        //ListaAccesos id_;
        Expresion valor;

        public Asignacion(String id,Expresion valor, int linea, int columna)
        {
            this.id = id;
            this.linea = linea;
            this.columna = columna;
            this.valor = valor;
        }

        /*
        public Asignacion(ListaAccesos id, int linea, int columna, Expresion valor)
        {
            this.id_ = id;
            this.linea = linea;
            this.columna = columna;
            this.valor = valor;
        }
        */

        public override object ejecutar(Entorno ent)
        {
            Variable sim = ent.buscar(id, linea, columna, "La variable"); //Busco la variable en la tabla de símbolos

            if (sim != null)
            { //Si la variable existe

                Expresion resultado = valor.getValor(ent);

                switch (sim.tipo.tipo)
                { //Tipo de la variable
                    case Tipo.enumTipo.entero:
                        switch (resultado.tipo.tipo)
                        {
                            case Tipo.enumTipo.entero:
                                sim.valor = resultado.valor;
                                return null;

                            case Tipo.enumTipo.real:
                                sim.valor = int.Parse(resultado.valor.ToString());
                                return null;
                                /*
                                case Tipo.enumTipo.caracter:
                                    int ascii = (int)resultado.valor.toString().charAt(0);
                                    sim.valor = ascii;
                                    return null;
                                */
                        }
                        break;
                    case Tipo.enumTipo.real:
                        switch (resultado.tipo.tipo)
                        {
                            /*
                            case caracter:
                                int ascii = (int)resultado.valor.toString().charAt(0);
                                sim.valor = ascii;
                                return null;
                            */
                            case Tipo.enumTipo.entero:
                                //si es entero se convierte a float
                                sim.valor = float.Parse(resultado.valor.ToString());
                                return null;
                            case Tipo.enumTipo.real:
                                sim.valor = resultado.valor;
                                return null;
                        }
                        break;
                    /*
                    case caracter:
                        switch (resultado.tipo.tipo)
                        {
                            case caracter:
                                sim.valor = resultado.valor.toString().charAt(0);
                                return null;
                        }
                        break;
                    */
                    case Tipo.enumTipo.booleano:
                        switch (resultado.tipo.tipo)
                        {
                            case Tipo.enumTipo.booleano:
                                sim.valor = resultado.valor;
                                return null;
                        }
                        break;

                    case Tipo.enumTipo.cadena:
                        switch (resultado.tipo.tipo)
                        {
                            case Tipo.enumTipo.cadena:
                                sim.valor = resultado.valor;
                                return null;
                        }
                        break;

                    case Tipo.enumTipo.Objecto:
                        switch (resultado.tipo.tipo)
                        {
                            case Tipo.enumTipo.Objecto:
                                //aqui debemos hacer la validacion de que si son de la misma clase se asignen
                                //revisar si llega la clase como tal y no un entorno, si llega un objeto
                                sim.valor = resultado.valor;
                                return null;
                            case Tipo.enumTipo.Null:
                                //Simbolo k = new Simbolo(new Tipo(Tipo.EnumTipo.Object,sim.tipo.tr),new Literal(new Tipo(Tipo.EnumTipo.Null),null));
                                Variable k = new Variable(new Tipo(sim.tipo.tipo, sim.tipo.referencia), null); // <-- No se si funciona
                                sim.valor = k.valor;
                                return null;
                        }
                        break;
                }

                //Si llega aquí el tipo de dato que se le quiere asignar a la variable es incorrecto
                MasterClass.Instance.addError(new C_Error("Semantico", "Incorrecto tipo de dato a la variable: " + id, linea, columna));

            } //Si la variable NO existe ya se marcó el error
            return null;
        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }

        


    }
}
