using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;

    [SerializeField] private CanvasGroup fade;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;

            // Estado inicial
            fade.alpha = 0f;
            fade.blocksRaycasts = false;
            fade.interactable = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TrocarCena(string nomeCena)
    {
        StartCoroutine(CarregarCena(nomeCena));
    }

    IEnumerator CarregarCena(string nomeCena)
    {
        // Bloqueia os cliques durante a transição
        fade.blocksRaycasts = true;
        fade.interactable = true;

        // Fade para preto
        yield return fade.DOFade(1f, 0.8f).WaitForCompletion();

        // Espera um pouco com a tela preta
        yield return new WaitForSeconds(2f);

        // Carrega a cena
        AsyncOperation op = SceneManager.LoadSceneAsync(nomeCena);

        while (!op.isDone)
            yield return null;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeEntrada());
    }

    IEnumerator FadeEntrada()
    {
        // Começa totalmente preto
        fade.alpha = 1f;

        // Continua bloqueando os cliques
        fade.blocksRaycasts = true;
        fade.interactable = true;

        // Espera um pouco para a cena carregar completamente
        yield return new WaitForSeconds(0.3f);

        // Faz a tela desaparecer lentamente
        yield return fade.DOFade(0f, 1f).WaitForCompletion();

        // Libera os cliques
        fade.blocksRaycasts = false;
        fade.interactable = false;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}