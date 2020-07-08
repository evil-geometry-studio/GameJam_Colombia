using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommandInput;
using UnityEngine.SceneManagement;

/*  Este main manager controlaro los eventos para cambiar los inputs y aumento de la 
    dificultad, dado que tiene un singleton que va a existir durante la ejecución del juego
    sera posible acceder a sus componentes, solo hay que cuidar de como hacerlo
*/
public class MainManager : MonoBehaviour
{   
    #region SingletondeMainManager
    private static MainManager instance;
    public static MainManager Instace{get => instance;}
    #endregion

    private void Awake() 
    {
        #region InicializaSingleton
        if(instance == null) //Si no hay ninguna instancia, eso quiere decir que acaba de iniciar el juego, no hay valor inicializados
        {
            //ASignamos la instacia de clase actual a la variable
            instance = this;
            //Le decimos que no queremos que se destruya cuando se inicie una nueva escena
            DontDestroyOnLoad(this);
        }
        else
        {
            //Si se inicia una escena en donde llegase a existir esta misma instancia de clase
            //decirle que la destruya, pues solo puede existir una instancia de clase
            Destroy(this.gameObject);
        }
        #endregion
    }

    private void Start() 
    {
        ChangeGameElements();
    }

    private void ChangeGameElements()
    {
        //Podría ser que, cada que se cargue la escena, este metodo se mande llamar,
        //o solo mandarlo llamar cuando pierde
        ChangeInputs();
        //Mandar llamar el metodo para aumentar dificultad de enemigos
        //Mandar llamar el metodo para cambiar el nivel
    }

    //Mandar llamar este metodo cuando se reinicie el nivel, para cambiar los inputs
    public void ChangeInputs()
    {
        //Si se busca tener más opciones de inputs, ir al script CommandInputs

        //Tomamos el valor actual que tenga como input el jugador
        CommandInputs.StateInput temp = PlayerMovement.Instance.curStateInputs;
        //Que siga escogiendo de forma aleatoria un valor, hasta que sea diferente al del jugador
        while (temp == PlayerMovement.Instance.curStateInputs)
        {
            //Cambiar el valor máximo en base a las opciones del enum StateInput en el 
            //script de CommandInputs
            int rand = Random.Range(0, 3);
            temp = (CommandInputs.StateInput)rand; //Haz un casteo de la posicion del enum para obtener su valor
        }
        
        PlayerMovement.Instance.curStateInputs = temp; //Asigna a los inputs del jugar el estado random seleccionado

        //Si quieres mostrar en interfaz, ya sea para el tutorial o almenos indicarle de cierta forma al jugador
        //el cambio de input, puedes hacerl in game mandando llamar el metodo que esta concatenado, ese metodo regresa un string
        //el cual se puede añadir en un text o text mesh pro en la UI
        Debug.Log("El input para ir a la izquierda es con: "+CommandInputs.ShowButtons(CommandInputs.TheKeysButtons.GoLeft, temp));
        Debug.Log("El input para ir a la derecha es con: "+CommandInputs.ShowButtons(CommandInputs.TheKeysButtons.GoRight, temp));
        Debug.Log("El input para crear brechas es con: "+CommandInputs.ShowButtons(CommandInputs.TheKeysButtons.CreateGap, temp));
        Debug.Log("El input para interactuar con objetos es con: "+CommandInputs.ShowButtons(CommandInputs.TheKeysButtons.InteractionObjs, temp));
    }

    //Mandar llamar esta corutina, cuando muera el jugador o se reinicie el nivel, solo tomar en cuenta que a los botones no se les puede asignar directamente
    //los metodos de esta clase, pues como se van a destruir cuando cargue la escena y esta clase seguira existiendo, los botones perderán las referencias a los metodos
    IEnumerator LoadLevelAsync(string scene)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);

        while (!async.isDone)
        {
            //Quizá aqui llenar el progreso de una barra para cargar el nivel, activando
            //un fade
            //Eje. progressBar.fillAmount = async.progress; //Donde progress bar es un image y se ha cambiado la forma en que 
            yield return new WaitForEndOfFrame();
        }
    }
}
