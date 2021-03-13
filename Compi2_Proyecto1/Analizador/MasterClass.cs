using Compi2_Proyecto1.Principales;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Compi2_Proyecto1.Analizador
{
    public sealed class MasterClass
    {
        private string output = "";
        private string mensajes = "";
        public static LinkedList<TipoCiclo> Display = new LinkedList<TipoCiclo>();
        public static LinkedList<TipoMF> PilaMF = new LinkedList<TipoMF>();
        private LinkedList<Instruccion> instrucciones = new LinkedList<Instruccion>();
        private LinkedList<C_Error> listaErrores = new LinkedList<C_Error>();
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

        /*
         * @param mensaje: el mensaje que se quiere concatenar para la salida
         * @param salto: indica si existe salto de linea 
         */

        public enum TipoCiclo
        {
            //los tipos de primitivos
            Ciclo, Switch
        }

        public enum TipoMF
        {
            Metodo, Funcion, Metodo_Funcion
        }


        public void addMessage(string mensaje, Boolean salto)
        {
            if (salto)
            {
                //añadimos salto
                this.mensajes += mensaje + "\n";
            }
            else {
                this.mensajes += mensaje;
            }
            
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

        public void addInstruction(Instruccion instruccion)
        {
            this.instrucciones.AddLast(instruccion);
        }

        public void addError(C_Error mistake) {
            this.listaErrores.AddLast(mistake);
        }

        public void showAllErrors() {
            foreach (C_Error mistake in this.listaErrores) {
                //Console.WriteLine(mistake.getDescripcionError());
                this.addMessage(mistake.getDescripcionError() +", Linea: "+ mistake.getlinea()+", Columna: "+mistake.getColumna(), true);
                //MessageBox.Show(mistake.getDescripcionError());
            }
        }

        public void ifMistakes() {
            if (this.listaErrores.Count != 0) {
                MessageBox.Show("Se detectaron errores en la entrada");
            }
        }

        public void clearMistakes() {
            this.listaErrores.Clear();
        }

        public int getCantidad()
        {
            return this.instrucciones.Count;
        }

        /*Para llevar el control de los break*/
        public bool EstoyDentroDeCiclo(TipoCiclo tipoCiclo)
        {
            foreach (TipoCiclo recorrer in Display)
            {
                if (recorrer == tipoCiclo)
                {

                    return true;
                }
            }
            return false;
        }

        public bool EstoyDentroDeMF(TipoMF tipoMF)
        {
            foreach (TipoMF recorrer in PilaMF)
            {
                if (recorrer == tipoMF)
                {
                    return true;
                    //es recorrer
                }
            }
            return false;
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
