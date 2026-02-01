using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class tiempodepartida : MonoBehaviour
{
    [Header("TIEMPO")] public float tiempoTotal = 120f; // 2 minutos
    private float tiempoActual;
    private bool partidaActiva;

    [Header("UI")] public TextMeshProUGUI textoTiempo;

    [Header("SONIDO")] public AudioSource audioSource;
    public AudioClip sonidoFinal;
    private bool sonidoReproducido = false;

    [Header("BOTÓN")] public int puntosPerdidosBoton = 10;
    public int puntosMaximos = 100;
    public GameObject botonReducir;

    void Start()
    {
        if (botonReducir != null) botonReducir?.SetActive(false);
        ActualizarUI();
    }

    void Update()
    {
        if (!partidaActiva) return;

        tiempoActual -= Time.deltaTime;

        if (tiempoActual <= 10 && !sonidoReproducido)
        {
            audioSource.PlayOneShot(sonidoFinal);
            sonidoReproducido = true;
        }

        if (tiempoActual <= 0)
        {
            tiempoActual = 0;
            FinalizarPartida();
        }

        ActualizarUI();
    }

    // INICIAR PARTIDA
    public void IniciarPartida()
    {
        tiempoActual = tiempoTotal;
        partidaActiva = true;
        sonidoReproducido = false;

        if (botonReducir != null) botonReducir?.SetActive(true);
        ActualizarUI();
    }

    // MOSTRAR TIEMPO EN MM:SS
    void ActualizarUI()
    {
        int minutos = Mathf.FloorToInt(tiempoActual / 60);
        int segundos = Mathf.FloorToInt(tiempoActual % 60);
        textoTiempo.text = minutos.ToString("00") + ":" + segundos.ToString("00");
    }

    // REDUCCIÓN PROPORCIONAL A PUNTOS PERDIDOS
    // puntosPerdidos = cuantos perdió
    // puntosMaximos = total posible
    // MÉTODO REAL (lógica)
    public void ReducirTiempoPorPuntos(int puntosPerdidos, int puntosMaximos)
    {
        float porcentaje = (float)puntosPerdidos / puntosMaximos;
        float reduccion = tiempoTotal * porcentaje;

        tiempoActual -= reduccion;

        if (tiempoActual < 0)
            tiempoActual = 0;

        ActualizarUI();
    }

// MÉTODO PARA EL BOTÓN (SIN PARÁMETROS)
    public void BotonReducirTiempo()
    {
        ReducirTiempoPorPuntos(puntosPerdidosBoton, puntosMaximos);
    }

    void FinalizarPartida()
    {
        partidaActiva = false;
        Debug.Log("⛔ Tiempo agotado");
    }

    public float ObtenerTiempo()
    {
        return tiempoActual;
    }
}