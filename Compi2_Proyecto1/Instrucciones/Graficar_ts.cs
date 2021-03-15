using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Compi2_Proyecto1.Analizador;
using Compi2_Proyecto1.Principales;

namespace Compi2_Proyecto1.Instrucciones
{
    class Graficar_ts : Instruccion
    {

        public Graficar_ts() { }

        public override object ejecutar(Entorno ent)
        {

            this.ReporteEntornos(ent);
            return null;
        }


        private void ReporteEntornos(Entorno ent)
        {
            string nombredoc = "C:\\compiladores2\\ReporteEntornos.html";
            FileStream fs = new FileStream(nombredoc, FileMode.Create);
            StreamWriter Archivo = new StreamWriter(fs, System.Text.Encoding.Default);
            Archivo.Write("<!DOCTYPE html> \n");
            Archivo.Write("<html> \n");
            Archivo.Write("<head> \n");
            Archivo.Write("<title>Tabla de Simbolos</title> \n");
            Archivo.Write("<link rel=stylesheet href=\"style.css\"> \n");
            Archivo.Write("</head> \n");
            Archivo.Write("<body> \n");
            Archivo.Write("<center> \n");
            Archivo.Write("<h2>Tablas de Simbolos<br></h2> \n");
            Archivo.Write("</center> \n");
            Archivo.Write("<table align=center> \n");
            Archivo.Write("<tr> \n");
            Archivo.Write("<td><h3>Nombre</h3></td> \n");
            Archivo.Write("<td><h3>Tipo</h3></td> \n");
            Archivo.Write("<td><h3>Ambito</h3></td> \n");
            Archivo.Write("<td><h3>Fila</h3></td> \n");
            Archivo.Write("<td><h3>Columna</h3></td> \n");
            Archivo.Write("</tr> \n");
            Archivo.Write("<center> \n");

            for (Entorno e = ent; e != null; e = e.anterior)
            {

                foreach (KeyValuePair<string, Variable> simbolo in e.tabla)
                {
                    Archivo.Write("<tr> \n");
                    Archivo.Write("<td><p>" + simbolo.Key.ToString() + "</p></td>\n");
                    Archivo.Write("<td><p>" + simbolo.Value.tipo.tipo.ToString() + "<p></td>\n");
                    Archivo.Write("<td><p>" + e.nombreEntorno + "<p></td>\n");
                    Archivo.Write("<td><p>" + simbolo.Value.linea + "<p></td>\n");
                    Archivo.Write("<td><p>" + simbolo.Value.columna + "<p></td>\n");
                    Archivo.Write("</tr> \n");

                }
            }


            Archivo.Write("</center> \n");
            Archivo.Write("</table> \n");


            Archivo.Write("</body> \n");
            Archivo.Write("</html> \n");
            Archivo.Close();
            //System.Diagnostics.Process.Start(nombredoc);

            string strCmdText;
            strCmdText = "/C start /b C:\\compiladores2\\ReporteEntornos.html";
            System.Diagnostics.Process.Start("CMD.exe", strCmdText);


        }


        private void GuardandoCodigo(Entorno ent) {


            //MessageBox.Show("Entre al ejecutar de Graficar_ts");
            //creamos el dot de una tabla
            String stringDot = "";

            stringDot +=
                "digraph { \n" +
                "tbl[shape = plaintext label =< \n " +
                    "<table border = '0' cellborder = '1' color = 'blue' cellspacing = '0'> \n" +
                      "<tr>\n" +
                      "<td bgcolor =\"#f0e3ff\"> Simbolo </td> \n" +
                      "<td bgcolor =\"#f0e3ff\"> Tipo </td> \n" +
                      "<td bgcolor =\"#f0e3ff\"> Ámbito </td> \n" +
                      "<td bgcolor =\"#f0e3ff\"> Fila </td> \n" +
                      "<td bgcolor =\"#f0e3ff\"> Columna </td></tr> \n";

            for (Entorno e = ent; e != null; e = e.anterior)
            {

                foreach (KeyValuePair<string, Variable> simbolo in e.tabla)
                {
                    //Console.WriteLine(string.Format("Key-{0}:Value-{1}", result.Key, result.Value));
                    stringDot +=
                        "<tr> \n" +
                        "<td>" + simbolo.Key.ToString() + "</td> \n" +
                        "<td>" + simbolo.Value.tipo.tipo.ToString() + "</td> \n" +
                        "<td>" + e.nombreEntorno + "</td> \n" +
                        "<td>" + simbolo.Value.linea + "</td> \n" +
                        "<td>" + "0" + "</td> \n" +
                        "</tr> \n";

                }
            }

            stringDot += "</table> \n" +
                  ">]; \n" +
            "} \n";

            String comando = "cd " + "C:\\compiladores2\\images\\" + " & " + "dot -Tsvg " + "TS.dot" + " -o " + "TS.svg";
            ExecuteCommand(comando, stringDot);
        }

        public void prueba(String codigoDot) {
            /*public readonly string rutaDots = "C:\\compiladores2\\dots\\";*/

            System.IO.Directory.CreateDirectory("C:\\compiladores2\\images\\");
            using (FileStream fs = File.Create("C:\\compiladores2\\images\\AST.dot"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(codigoDot);
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


        static void ExecuteCommand(string _Command, String codigoDot)
        {

            System.IO.Directory.CreateDirectory("C:\\compiladores2\\images\\");
            using (FileStream fs = File.Create("C:\\compiladores2\\images\\TS.dot"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(codigoDot);
                fs.Write(info, 0, info.Length);
            }


            //Indicamos que deseamos inicializar el proceso cmd.exe junto a un comando de arranque. 
            //(/C, le indicamos al proceso cmd que deseamos que cuando termine la tarea asignada se cierre el proceso).
            //Para mas informacion consulte la ayuda de la consola con cmd.exe /? 
            System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + _Command);
            // Indicamos que la salida del proceso se redireccione en un Stream
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.UseShellExecute = false;
            //Indica que el proceso no despliegue una pantalla negra (El proceso se ejecuta en background)
            procStartInfo.CreateNoWindow = false;
            //Inicializa el proceso
            System.Diagnostics.Process proc = new System.Diagnostics.Process();
            proc.StartInfo = procStartInfo;
            proc.Start();
            //Consigue la salida de la Consola(Stream) y devuelve una cadena de texto
            string result = proc.StandardOutput.ReadToEnd();
            //Muestra en pantalla la salida del Comando
            MessageBox.Show(result);
        }


        public void generarPNG(String codigoDot) 
        {
            System.IO.Directory.CreateDirectory("C:\\compiladores2\\images\\");
            using (FileStream fs = File.Create("C:\\compiladores2\\images\\graficoTS.dot"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(codigoDot);
                fs.Write(info, 0, info.Length);
            }

            ProcessStartInfo p = new ProcessStartInfo("dot.exe");
            p.WorkingDirectory = @"C:/compiladores2/images/";    //aqui el directorio de trabajo
            p.WindowStyle = ProcessWindowStyle.Normal;
            p.RedirectStandardOutput = true;
            p.UseShellExecute = false;
            p.CreateNoWindow = true;
            p.WindowStyle = ProcessWindowStyle.Hidden;
            p.Arguments = "-Tpng graficoTS.dot -o graficoTS.png";
            Process.Start(p);
        }

        public void generarArchivoDot(String codigoDot) {

            System.IO.Directory.CreateDirectory("C:\\compiladores2\\images");
            using (FileStream fs = File.Create("C:\\compiladores2\\images\\graficoTS.dot"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(codigoDot);
                fs.Write(info, 0, info.Length);
            }
        }


        public readonly string rutaBase = "C:\\compiladores2\\";
        public readonly string rutaDots = "C:\\compiladores2\\dots\\";
        public void GuardarDot(string contenido, string nombre)
        {
            System.IO.Directory.CreateDirectory(rutaDots);
            using (FileStream fs = File.Create($"{rutaDots}{nombre}.dot"))
            {
                byte[] info = new UTF8Encoding(true).GetBytes(contenido);
                fs.Write(info, 0, info.Length);
            }
        }

        public void CompileDot(string nombre)
        {
            ProcessStartInfo p = new ProcessStartInfo("dot.exe");
            p.WorkingDirectory = @rutaDots;//aqui el directorio de trabajo
            p.WindowStyle = ProcessWindowStyle.Normal;
            p.RedirectStandardOutput = true;
            p.UseShellExecute = false;
            p.CreateNoWindow = true;
            p.WindowStyle = ProcessWindowStyle.Hidden;
            p.Arguments = $"-Tpng {nombre}.dot -o {nombre}.png";
            Process.Start(p);
        }



        public override string stringDot()
        {
            throw new NotImplementedException();
        }
    }
}
