using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Instrucciones;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Control
{
    public class Switch : Instruccion
    {
        Expresion Condicion;
        LinkedList<Bloque> listaBloquesCase_Default;
        bool execUno;    //indica si por lo menos algun bloque se ejecuto
        bool barridoCase;    //por si es necesario hacer un barrido en los case
        bool despuesDefault; //para saber si los bloques que siguen vienen despues de un default en caso de un barrido
        bool Defaultprimero; //para saber si ya tenemos un unico bloque default
        Bloques_Default Default;
        LinkedList<Bloque> faltantes = new LinkedList<Bloque>();

        public Switch(Expresion Condicion, LinkedList<Bloque> listaBloquesCase_Default)
        {
            this.Condicion = Condicion;
            this.listaBloquesCase_Default = listaBloquesCase_Default;
        }

        public override object ejecutar(Entorno ent)
        {
            //variables de control
            var bloqueCaseType = new Bloques_Case();
            var bloqueDefaultType = new Bloques_Default();

            //guardamos en la pila de ciclos para que sean validos los breaks
            MasterClass.Display.AddFirst(MasterClass.TipoCiclo.Switch);

            //preguntamos de que tipo es cada bloque de la lista
            foreach (Bloque block in listaBloquesCase_Default) {

                var blockType = block.GetType();

                //Si el bloque es de tipo Bloques_Case
                if (blockType.IsInstanceOfType(bloqueCaseType)) {

                    //obtenemos la condicion del bloque case
                    Expresion condicionCase = ((Bloques_Case)block).Comparacion.getValor(ent);

                    //obtenemos la condicion del switch
                    Expresion condicionSwitch = this.Condicion.getValor(ent);

                    //verificamos que sean del mismo tipo
                    if (condicionCase.tipo.tipo == condicionSwitch.tipo.tipo) {

                        //ahora verificamos que sean iguales para que logre entrar
                        String condicioncaseString = condicionCase.valor.ToString();
                        String condicionswitchString = condicionSwitch.valor.ToString();
                        if (condicioncaseString.Equals(condicionswitchString))
                        {
                            this.execUno = true;
                            //si entramos, ejecutamos ese bloque
                            Entorno nuevo = new Entorno(ent);
                            Object retornar = block.ejecutar(nuevo);
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
                }
                //si es una instancia del default (para mientras solo lo guardamos)
                else if (blockType.IsInstanceOfType(bloqueDefaultType)) {

                    this.Default = (Bloques_Default)block;

                }

            }

            //si se sale del for es porque ninguno hizo match y tenemos que ejecutar el bloque else
            //Preguntamos si tenemos un bloque default
            if (this.Default != null) {

                //si todavia no se ha ejecutado ninguno
                if (!this.execUno) {

                    Entorno nuevo = new Entorno(ent, ent.global);
                    Object retorno = this.Default.ejecutar(nuevo);
                    return retorno;

                }

            }

            MasterClass.Display.RemoveFirst();
            this.execUno = false;
            return null;
        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}
