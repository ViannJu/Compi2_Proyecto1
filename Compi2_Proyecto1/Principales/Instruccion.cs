using System;
using System.Collections.Generic;
using System.Text;

namespace Compi2_Proyecto1.Principales
{
    public abstract class Instruccion : Nodo
    {
        //funcion abstracta para ejecutar todas las instrucciones
        public abstract Object ejecutar(Entorno ent);

        //funcion abstracta para traer el stringDot de cada instruccion
        public abstract string stringDot();


    }
}
