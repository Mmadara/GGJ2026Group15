using System.Collections;
using UnityEngine;

// ENUM DE EVENTOS
public enum EventoRandom
{
    //CambioMascaraPatos,
    //CambiarPatronMovimiento,
    LucesParpadeantes,
    LucesApagadas,
    //CubrirRostroMascara,
    ApagarLuzDerecha,
    ApagarLuzIzquierda,
    VisionBorrosa
}

public class ramdomenum : MonoBehaviour
{
    [Header("CONFIGURACIÓN DE TIEMPOS")]
    public float tiempoMinEvento = 5f;
    public float tiempoMaxEvento = 10f;

    private bool partidaActiva = false;
    [SerializeField] private VisualObstacleController _visualObstacleController;

    // 🔹 LLAMAR DESDE EL BOTÓN PLAY
    public void IniciarEventos()
    {
        //if (partidaActiva) return;

        partidaActiva = true;
        StartCoroutine(EventosAleatorios());
    }

    // 🔹 LLAMAR AL TERMINAR LA PARTIDA
    public void DetenerEventos()
    {
        partidaActiva = false;
        StopAllCoroutines();
    }

    IEnumerator EventosAleatorios()
    {
        while (partidaActiva)
        {
            float espera = Random.Range(tiempoMinEvento, tiempoMaxEvento);
            yield return new WaitForSeconds(espera);

            EjecutarEventoRandom();
        }
    }

    void EjecutarEventoRandom()
    {
        EventoRandom evento = (EventoRandom)Random.Range(
            0,
            System.Enum.GetValues(typeof(EventoRandom)).Length
        );

        Debug.Log("🎲 Evento activado: " + evento);

        switch (evento)
        {
           /* case EventoRandom.CambioMascaraPatos:
                CambioMascaraPatos();
                break;

            case EventoRandom.CambiarPatronMovimiento:
                CambiarPatronMovimiento();
                break;*/

            case EventoRandom.LucesParpadeantes:
                LucesParpadeantes();
                break;

            case EventoRandom.LucesApagadas:
                LucesApagadas();
                break;

            /*case EventoRandom.CubrirRostroMascara:
                CubrirRostroMascara();
                break;*/

            case EventoRandom.ApagarLuzDerecha:
                ApagarLuzDerecha();
                break;

            case EventoRandom.ApagarLuzIzquierda:
                ApagarLuzIzquierda();
                break;

            case EventoRandom.VisionBorrosa:
                VisionBorrosa();
                break;
        }
    }

    // =======================
    // MÉTODOS DE EVENTOS
    // =======================

    void CambioMascaraPatos()
    {
        Debug.Log("🦆🎭 Cambio de máscara en los patos");
        // Aquí cambias el sprite / modelo / material
    }

    void CambiarPatronMovimiento()
    {
        Debug.Log("🔀 Cambio de patrón de movimiento");
        // Cambiar IA / velocidad / rutas
    }

    void LucesParpadeantes()
    {
        Debug.Log("💡⚡ Luces parpadeantes");
        _visualObstacleController.StartBlickingLight();
    }

    void LucesApagadas()
    {
        Debug.Log("🌑 Luces apagadas");
        _visualObstacleController.StartLightOff();
    }

    void CubrirRostroMascara()
    {
        Debug.Log("🙈🎭 El pato se cubre el rostro");
        // Animación o overlay
    }

    void ApagarLuzDerecha()
    {
        Debug.Log("➡️💡 Luz derecha apagada");
        _visualObstacleController.CovertHalfTheScreen(Direction.Right);
    }

    void ApagarLuzIzquierda()
    {
        Debug.Log("⬅️💡 Luz izquierda apagada");
        _visualObstacleController.CovertHalfTheScreen(Direction.Left);
    }

    void VisionBorrosa()
    {
        Debug.Log("👁️‍🗨️ Visión borrosa");
        _visualObstacleController.StartBlurry();
    }
}
