using Compi2_Proyecto1.Principales;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Compi2_Proyecto1.Expresiones
{
    public class Id : Expresion
    {
        public string id;

        public Id(string id, int linea, int columna) {
            this.id = id;
            this.linea = linea;
            this.columna = columna;
        }

        public override string getDot()
        {
            throw new NotImplementedException();
        }

        public override Expresion getValor(Entorno ent)
        {
            MessageBox.Show("Estoy en el get valor de la variable "+id);
            Variable sim = ent.buscar(id, linea, columna, "La variable");
            if (sim != null)
            { //Si se encontró la variable
                MessageBox.Show("Si se encontro la variable " + id);
                Primitivo retorno = new Primitivo(sim.tipo, sim.valor);
                //retorna tal cual el valor
                return retorno;
            }
            //Quiere decir que la variable NO existe, devuelvo un error
            return new Primitivo(new Tipo(Tipo.enumTipo.error), "@Error@");
        }

        //Metodo para retornar no un valor como tal sino un simbolo -> Que contiene el valor como tal + un tipo
        public Variable getSimbolo(Entorno ent1)
        {
            Variable sim = ent1.buscar(id, linea, columna, "La variable");
            if (sim != null)
            {
                return sim;
            }
            else
            {
                sim = ent1.global.buscar(id, linea, columna, "La variable");
                if (sim != null)
                {
                    return sim;
                }
            }
            return new Variable(new Tipo(Tipo.enumTipo.error), new Primitivo(new Tipo(Tipo.enumTipo.error), "@error@"));
        }
    }
}
