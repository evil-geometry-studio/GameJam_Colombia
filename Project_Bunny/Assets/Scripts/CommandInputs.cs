using System.Collections;
using UnityEngine;

/*
    Patron de diseño comando, esto permite intercambiar los inputs, siempre mandando llamar
    a la misma acción

*/

namespace CommandInput
{
    public static class CommandInputs
    {
        //Las acciones que se pueden ejecutar
        public enum TheKeysButtons { GoRight, GoLeft, CreateGap, InteractionObjs }
        //En que estado se encuentran los inputs, puede ser que ya se hayan invertido
        //Si se quieren más estados, añadirlo al enum y crear la sentencia if correspondiente en el metodo GetKeyCode
        public enum StateInput { Default, Inverted_1, Inverted_2}

        //El estado cuando un boton es presionado a fondo
        public static bool GetKeyButtonDown(TheKeysButtons btn, StateInput state)
        {
            KeyCode code = GetKeyCode(btn, state);
            return Input.GetKeyDown(code);
        }

        //Se libera el boton
        public static bool GetKeyButtonUp(TheKeysButtons btn, StateInput state)
        {
            KeyCode code = GetKeyCode(btn, state);
            return Input.GetKeyUp(code);
        }

        //Si se esta presionando una tecla
        public static bool GetKeyButton(TheKeysButtons btn, StateInput state)
        {
            KeyCode code = GetKeyCode(btn, state);
            return Input.GetKey(code);
        }

        //Metodo para saber la asignación de los botones
        public static string ShowButtons(TheKeysButtons btn, StateInput state)
        {
            KeyCode code = GetKeyCode(btn, state);
            return code.ToString();
        }

        static KeyCode GetKeyCode(TheKeysButtons btn, StateInput state)
        {
            //Se pregunta por el estado actual del boton
            if (state == StateInput.Default)
            {
                //Estado normal, devuelve el la asignación de la acción al estado correspondiente
                switch (btn)
                {
                    case TheKeysButtons.GoRight: return KeyCode.D;
                    case TheKeysButtons.GoLeft: return KeyCode.A;
                    case TheKeysButtons.CreateGap: return KeyCode.Mouse0;
                    case TheKeysButtons.InteractionObjs: return KeyCode.Mouse1;
                }
            }
            else if(state == StateInput.Inverted_1)
            {
                //Estado invertido 1, devuelve el la asignación de la acción al estado correspondiente
                switch (btn)
                {
                    case TheKeysButtons.GoRight: return KeyCode.Mouse1;
                    case TheKeysButtons.GoLeft: return KeyCode.Mouse0;
                    case TheKeysButtons.CreateGap: return KeyCode.A;
                    case TheKeysButtons.InteractionObjs: return KeyCode.D;
                }
            }
            else if(state == StateInput.Inverted_2)
            {
                //Estado invertido 2, devuelve el la asignación de la acción al estado correspondiente
                switch (btn)
                {
                    case TheKeysButtons.GoRight: return KeyCode.D;
                    case TheKeysButtons.GoLeft: return KeyCode.Mouse0;
                    case TheKeysButtons.CreateGap: return KeyCode.A;
                    case TheKeysButtons.InteractionObjs: return KeyCode.Mouse1;
                }
            }
            

            return KeyCode.None;
        }
    }
}


