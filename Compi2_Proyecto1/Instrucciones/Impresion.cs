﻿using System;
using System.Collections.Generic;
using System.Text;
using Compi2_Proyecto1.Principales;
using Compi2_Proyecto1.Analizador;

namespace Compi2_Proyecto1.Instrucciones
{
    /*
     * Clase instruccion Impresion para el manejo de los write y los writeln en el lenguaje
     */

    public class Impresion : Instruccion
    {

        Expresion valor;
        Boolean salto;

        public Impresion(Expresion valor, Boolean salto) {

            this.valor = valor;
            this.salto = salto;

        }

        public override object ejecutar(Entorno ent)
        {
            //Expresion anterior = this.valor.getValor(ent);    //por la lista de accesos
            Expresion resultado = valor.getValor(ent);            

            if (resultado.tipo.tipo != Tipo.enumTipo.error) {
                //si no es un error entonces imprimimos
                if (salto)
                {
                    MasterClass.Instance.addMessage(resultado.valor.ToString(), true);
                }
                else {
                    MasterClass.Instance.addMessage(resultado.valor.ToString(), false);
                }
                
            }

            return null;
        }

        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}
