# Création d'Interactions

Les interactions orchestrent les conditions, triggers et effets pour créer des interactions complètes.

## Structure de Base

```csharp
public class MonInteraction : AInteractable
{
    [SerializeField] private TypeDonnee parametre;
    
    protected override void Init()
    {
        // S'abonner à l'événement d'interaction
        onInteractAction += MaMethodeInteraction;
    }
    
    private void MaMethodeInteraction()
    {
        // Logique d'interaction
        
        // Terminer l'interaction
        EndInteraction();
    }
}
```

## Configuration dans l'Inspector

| Propriété | Description |
|-----------|-------------|
| **ActivationType** | `OnStart` = auto au démarrage, `OnEnable` = à chaque réactivation du GameObject, `Manual` = via code (`Enable()`) |
| **Delay** | Délai avant activation (en secondes) |
| **Once** | `true` = une seule fois (définitif, même après un cycle disable/enable — voir `ResetInteractionState()`), `false` = répétable |
| **Interaction Trigger** | Référence au trigger (zone, clic, collision) |
| **Effects** | Tableau des effets à exécuter |
| **Conditions** | Tableau des conditions requises |

## Méthodes Disponibles

```csharp
Enable()                  // Activer l'interaction (ignoré si une interaction "once" a déjà été consommée)
Disable()                 // Désactiver l'interaction (déclenche les effets Exit si en cours)
OnEnter()                 // Joueur entre dans l'interaction
OnExit()                  // Joueur sort de l'interaction
OnInteract()              // Joueur interagit
EndInteraction()          // Terminer l'interaction proprement
ResetInteractionState()   // Réarmer une interaction "once" consommée (pooling/respawn)
```

> **Note** : le cycle de vie est protégé — un appel effectué depuis un état invalide est
> simplement ignoré (ex : `OnInteract()` pendant une interaction déjà en cours, `OnExit()`
> sans entrée préalable, `OnEnter()` pendant une interaction). Pas besoin de vérifier
> l'état avant d'appeler ces méthodes.

## Événements Disponibles

```csharp
protected event Action onEnableAction;      // Interaction activée
protected event Action onDisableAction;     // Interaction désactivée
protected event Action onEnterAction;       // Joueur entre
protected event Action onExitAction;        // Joueur sort
protected event Action onInteractAction;    // Joueur interagit
```

## Exemple : Porte Simple

```csharp
public class InteractionPorte : AInteractable
{
    [SerializeField] private Animator animatorPorte;
    [SerializeField] private float dureeOuverture = 2f;
    
    protected override void Init()
    {
        onInteractAction += OuvrirPorte;
    }
    
    private void OuvrirPorte()
    {
        animatorPorte.SetTrigger("Ouvrir");
        StartCoroutine(AttendreOuverture());
    }
    
    private IEnumerator AttendreOuverture()
    {
        yield return new WaitForSeconds(dureeOuverture);
        EndInteraction();
    }
}
```

## Bonnes Pratiques

✅ Appelez **EndInteraction()** pour terminer proprement  
✅ Laissez la **logique métier** dans l'interaction  
✅ Créez des interactions réutilisables et paramétrées
