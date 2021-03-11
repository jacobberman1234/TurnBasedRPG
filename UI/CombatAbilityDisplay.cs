using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatAbilityDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CombatManager _combatManager;
    [SerializeField] SpawnManager _spawnManager;
    [SerializeField] CharacterAbilities _playerAbilities;
    [SerializeField] PlayerUseAbility _useAbility;
    [SerializeField] Button[] _buttons;

    public TMP_Text[] abilityTexts;

    public bool AbilitiesChanged;
     
    void Start()
    {
        _combatManager = CombatManager.instance;
        _spawnManager = SpawnManager.instance;
        _playerAbilities = GameObject.FindGameObjectWithTag(Helper.PLAYER_TAG).GetComponent<CharacterAbilities>();
        _useAbility = GameObject.FindGameObjectWithTag(Helper.PLAYER_TAG).GetComponent<PlayerUseAbility>();
        AbilitiesChanged = true;
    }

    void Update() => DisplayAbilities();

    void DisplayAbilities()
    {
        if(_playerAbilities == null)
            _playerAbilities = _spawnManager.PlayersSpawned[0].GetComponent<CharacterAbilities>();
        if(_useAbility == null)
            _useAbility = _spawnManager.PlayersSpawned[0].GetComponent<PlayerUseAbility>();

        if (!_combatManager.InCombat && gameObject.activeSelf)
            this.gameObject.SetActive(false);

        if (!AbilitiesChanged)
        {
            return;
        }
        for (int i = 0; i < abilityTexts.Length; i++)
        {
            if (_playerAbilities.Abilities == null)
            {
                print($"Abilities are null");
                return;
            }
            var ability = _playerAbilities.Abilities[i];
            if(ability != null)
            {
                abilityTexts[i].text = ability.name;
                _buttons[i].onClick.AddListener(delegate { _useAbility.SelectAbility(ability); });
            }
            else
            {
                _buttons[i].gameObject.SetActive(false);
            }
        }
        AbilitiesChanged = false;
    }
}
