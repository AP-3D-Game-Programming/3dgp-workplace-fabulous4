using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    public RecipeDatabaseSO recipeDB;
    public int capacity = 4;          // max aantal items in de oven
    public string applianceTag = "Oven";       // filtert geschikte recepten
    public Transform outputPoint;              // waar prefab/heldItem kan spawnen (optioneel visueel)

    private Dictionary<string, int> _contents = new(); // itemId -> count
    private bool _isCooking = false;
    private float _cookTimer = 0f;
    private RecipeSO _activeRecipe = null;
    private int _pendingOutputCount = 0;
    private string _pendingOutputItemId = null;

    public enum State { Idle, Loading, Cooking, Ready }
    public State CurrentState { get; private set; } = State.Idle;

    // Wordt aangeroepen via PlayerInteract (E)
    public void Interact()
    {
        // 1) Als klaar -> geef output aan speler
        if (CurrentState == State.Ready && _pendingOutputCount > 0 && !string.IsNullOrEmpty(_pendingOutputItemId))
        {
            GiveOutputToPlayer();
            return;
        }

        // 2) Als aan het koken -> toon status
        if (CurrentState == State.Cooking)
        {
            Debug.Log($"Oven is cooking... {Mathf.CeilToInt(_activeRecipe.cookTimeSeconds - _cookTimer)}s remaining");
            return;
        }

        // 3) Probeer item te deponeren
        TryDepositFromPlayer();

        // 4) Na deposit: check recept
        if (CurrentState != State.Cooking && CurrentState != State.Ready)
        {
            _activeRecipe = recipeDB != null ? recipeDB.FindMatch(_contents, applianceTag) : null;
            if (_activeRecipe != null)
            {
                StartCoroutine(CookRoutine(_activeRecipe));
            }
            else
            {
                // Nog niet genoeg/juist: blijft in Loading
                CurrentState = _contents.Count > 0 ? State.Loading : State.Idle;
            }
        }
    }

    private void TryDepositFromPlayer()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;

        var inv = player.GetComponent<PlayerInventory>();          // inventory met string-IDs
        if (inv == null) return;                                   // :contentReference[oaicite:3]{index=3}
        var hand = player.GetComponent<PickUpScript>();            // visuals + current in-hand
        // we voegen hieronder een public getter toe; zie sectie 3. :contentReference[oaicite:4]{index=4}

        // kies item om te droppen: bij voorkeur het item in de hand
        string candidate = null;
        if (hand != null && !string.IsNullOrEmpty(hand.CurrentItemId))
        {
            candidate = hand.CurrentItemId;
        }
        else if (inv.items.Count > 0)
        {
            candidate = inv.items[inv.items.Count - 1]; // bv. laatste item
        }

        if (string.IsNullOrEmpty(candidate)) return;
        if (TotalCount(_contents) >= capacity)
        {
            Debug.Log("Oven is vol.");
            return;
        }

        if (!inv.HasItem(candidate)) return;

        // verplaats item van speler -> oven
        inv.RemoveItem(candidate);               // haalt ook het held object weg via PickUpScript.RemoveItemFromHand(...) :contentReference[oaicite:5]{index=5}
        AddToContents(candidate, 1);

        CurrentState = State.Loading;
        Debug.Log($"Item '{candidate}' toegevoegd aan oven. Inhoud: {DebugContents()}");
    }

    private void AddToContents(string itemId, int amount)
    {
        if (!_contents.ContainsKey(itemId)) _contents[itemId] = 0;
        _contents[itemId] += amount;
    }

    private int TotalCount(Dictionary<string, int> map)
    {
        int t = 0; foreach (var kv in map) t += kv.Value; return t;
    }

    private IEnumerator CookRoutine(RecipeSO recipe)
    {
        _isCooking = true;
        _cookTimer = 0f;
        CurrentState = State.Cooking;

        Debug.Log($"Start cooking '{recipe.outputItemId}'... ({recipe.cookTimeSeconds}s)");

        while (_cookTimer < recipe.cookTimeSeconds)
        {
            _cookTimer += Time.deltaTime;
            yield return null;
        }

        // klaar: maak de oven leeg en zet output klaar
        _contents.Clear();
        _pendingOutputItemId = recipe.outputItemId;
        _pendingOutputCount = Mathf.Max(1, recipe.outputAmount);
        _activeRecipe = null;
        _isCooking = false;
        CurrentState = State.Ready;

        Debug.Log($"Oven klaar! Output: '{_pendingOutputItemId}' x{_pendingOutputCount}");
    }

    private void GiveOutputToPlayer()
    {
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;

        var inv = player.GetComponent<PlayerInventory>();
        var hand = player.GetComponent<PickUpScript>();

        if (inv == null) return;

        for (int i = 0; i < _pendingOutputCount; i++)
            inv.AddItem(_pendingOutputItemId);

        if (hand != null)
            hand.SpawnItemInHand(_pendingOutputItemId); // laat resultaat meteen �in de hand� zien (visueel). :contentReference[oaicite:6]{index=6}

        Debug.Log($"Speler kreeg '{_pendingOutputItemId}' x{_pendingOutputCount}");

        _pendingOutputItemId = null;
        _pendingOutputCount = 0;
        CurrentState = State.Idle;
    }

    public string DebugContents()
    {
        if (_contents.Count == 0) return "(leeg)";
        var parts = new List<string>();
        foreach (var kv in _contents) parts.Add($"{kv.Key} x{kv.Value}");
        return string.Join(", ", parts);
    }
}
