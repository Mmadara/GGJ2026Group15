using System.Collections;
using UnityEngine;

public class ramdomenum : MonoBehaviour
{
    // ENUM DE EVENTOS
    public enum EventoRandom
    {
        CambioMascaraPatos,
        CambiarPatronMovimiento,
        LucesParpadeantes,
        LucesApagadas,
        CubrirRostroMascara,
        ApagarLuzDerecha,
        ApagarLuzIzquierda,
        VisionBorrosa
    }

    [Header("CONFIGURACIÓN DE TIEMPOS")]
    public float tiempoMinEvento = 10f;
    public float tiempoMaxEvento = 25f;

    private bool partidaActiva = false;

    // 🔹 LLAMAR DESDE EL BOTÓN PLAY
    public void IniciarEventos()
    {
        if (partidaActiva) return;

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
            case EventoRandom.CambioMascaraPatos:
                CambioMascaraPatos();
                break;

            case EventoRandom.CambiarPatronMovimiento:
                CambiarPatronMovimiento();
                break;

            case EventoRandom.LucesParpadeantes:
                LucesParpadeantes();
                break;

            case EventoRandom.LucesApagadas:
                LucesApagadas();
                break;

            case EventoRandom.CubrirRostroMascara:
                CubrirRostroMascara();
                break;

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
        // Coroutine de parpadeo
    }

    void LucesApagadas()
    {
        Debug.Log("🌑 Luces apagadas");
        // Apagar iluminación general
    }

    void CubrirRostroMascara()
    {
        Debug.Log("🙈🎭 El pato se cubre el rostro");
        // Animación o overlay
    }

    void ApagarLuzDerecha()
    {
        Debug.Log("➡️💡 Luz derecha apagada");
        // Light derecha OFF
    }

    void ApagarLuzIzquierda()
    {
        Debug.Log("⬅️💡 Luz izquierda apagada");
        // Light izquierda OFF
    }

    void VisionBorrosa()
    {
        Debug.Log("👁️‍🗨️ Visión borrosa");
        // Post Processing / Blur
    }
}
