using Compi2_Proyecto1.Principales;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

        public static string contenidoDot = "";

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

        private bool hayErrores() {
            if (this.listaErrores.Count > 0)
            {
                return true;
            }
            else {
                return false;
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


        private void ReporteTokensErroneos()
        {
            string nombredoc = "C:\\compiladores2\\Reporte_Errores.html";
            FileStream fs = new FileStream(nombredoc, FileMode.Create);
            StreamWriter Archivo = new StreamWriter(fs, System.Text.Encoding.Default);
            Archivo.Write("<!DOCTYPE html> \n");
            Archivo.Write("<html> \n");
            Archivo.Write("<head> \n");
            Archivo.Write("<title>listado errores</title> \n");
            Archivo.Write("<link rel=stylesheet href=\"style.css\"> \n");
            Archivo.Write("</head> \n");
            Archivo.Write("<body> \n");
            Archivo.Write("<center> \n");
            Archivo.Write("<h2>Listado de Errores<br></h2> \n");
            Archivo.Write("</center> \n");
            Archivo.Write("<table align=center> \n");
            Archivo.Write("<tr> \n");
            Archivo.Write("<td><h3>FILA</h3></td> \n");
            Archivo.Write("<td><h3>COLUMNA</h3></td> \n");
            Archivo.Write("<td><h3>TIPO</h3></td> \n");
            Archivo.Write("<td><h3>DESCRIPCION</h3></td> \n");
            Archivo.Write("</tr> \n");
            Archivo.Write("<center> \n");
            foreach (C_Error error in this.listaErrores)
            {
                Archivo.Write("<tr> \n");
                Archivo.Write("<td><p>" + error.linea.ToString() + "</p></td>\n");
                Archivo.Write("<td><p>" + error.columna.ToString() + "<p></td>\n");
                Archivo.Write("<td><p>" + error.tipo.ToString() + "<p></td>\n");
                Archivo.Write("<td><p>" + error.descripcion.ToString() + "<p></td>\n");
                Archivo.Write("</tr> \n");
            }



            Archivo.Write("</center> \n");
            Archivo.Write("</table> \n");


            Archivo.Write("</body> \n");
            Archivo.Write("</html> \n");
            Archivo.Close();
            //System.Diagnostics.Process.Start(nombredoc);

            string strCmdText;
            strCmdText = "/C start /b C:\\compiladores2\\Reporte_Errores.html";
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);

        }


        private void crearDot() {

            /*public readonly string rutaDots = "C:\\compiladores2\\dots\\";*/

            System.IO.Directory.CreateDirectory("C:\\compiladores2\\images\\");
            using (FileStream fs = File.Create("C:\\compiladores2\\images\\AST.dot"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(MasterClass.contenidoDot);
                fs.Write(info, 0, info.Length);
            }

            String rutadiagrama = "C:\\compiladores2\\images\\";
            String nombrearchivo = "AST.dot";
            String comand = ("cd " + rutadiagrama + " & " + "dot -Tsvg " + nombrearchivo + " -o " + "AST" + ".svg");

            Console.WriteLine(comand);
            //MessageBox.Show(comand);

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c" + comand,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = false
                }
            };
            proc.Start();
        }


        public void ejecutar()
        {
            
            general = new Entorno(null, general, "General");
            general.global = general;

            foreach (Instruccion nodo in instrucciones)
            {
                nodo.ejecutar(general);
            }

            //cuando termine de ejecutar
            if (this.hayErrores())
            {

                //Si hay errores entonces mandamos a crear el html de errores
                this.ReporteTokensErroneos();
            }
            else 
            {
                crearDot();
            }
        }


    }



}
