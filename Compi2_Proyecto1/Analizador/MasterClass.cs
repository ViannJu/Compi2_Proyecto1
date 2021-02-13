using Compi2_Proyecto1.Principales;
using System;
using System.Collections.Generic;
using System.Text;

namespace Compi2_Proyecto1.Analizador
{
    public sealed class MasterClass
    {
        private string output = "";
        private string mensajes = "";
        private LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
        private static readonly MasterClass instance = new MasterClass();
        public Entorno general;

        static MasterClass() { }
        private MasterClass() { }
        public static MasterClass Instance
        {
            get
            {
                return instance;
            }
        }

        public void addMessage(string mensaje)
        {
            this.mensajes += "\n" + mensaje;
        }

        public string getMessages()
        {
            return this.mensajes;
        }

        public void addOutput(string mensaje)
        {
            this.output += "\n" + mensaje;
        }
        public string getOutput()
        {
            return this.output;
        }

        public void clear()
        {
            this.output = "";
            this.mensajes = "";
            this.instrucciones = new LinkedList<Instruccion>();
        }

        public void addInstruction(Instruccion nodo)
        {
            this.instrucciones.AddLast(nodo);
        }

        public int getCantidad()
        {
            return this.instrucciones.Count;
        }

        public void ejecutar()
        {
            
            general = new Entorno(null, general);
            general.global = general;

            foreach (Instruccion nodo in instrucciones)
            {
                nodo.ejecutar(general);
            }
        }


    }
}
