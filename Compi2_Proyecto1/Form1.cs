using Compi2_Proyecto1.Analizador;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compi2_Proyecto1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        //boton de EJECUTAR
        private void button1_Click(object sender, EventArgs e)
        {
            //limpiamos el textbox de salida
            this.richTextBox3.Text = "";

            //limpiamos el contenido de la singleton
            MasterClass.Instance.clear();
            Evaluador evaluator = new Evaluador();
            evaluator.analizar(this.richTextBox1.Text);
            //MessageBox.Show(MasterClass.Instance.getMessages()+"\nCantidad de instrucciones: "+MasterClass.Instance.getCantidad());
            //MasterClass.Instance.ejecutar();
            this.richTextBox3.Text = MasterClass.Instance.getMessages();
        }
    }
}
