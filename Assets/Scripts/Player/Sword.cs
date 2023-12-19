using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform slashAnimSpawnPoint;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float swordAttackCD = .5f;


    private PlayerControls playerControls;
    private Animator myAnimator;
    private PlayerController playerController;
    private ActiveWeapon activeWeapon;
    private bool attackButtonDown, isAttacking = false;

    private GameObject slashAnim;
    //inicializa junto a inicializacao da cena para pegar os componentes abaixo
    private void Awake() {
        playerController = GetComponentInParent<PlayerController>();
        activeWeapon = GetComponentInParent<ActiveWeapon>();
        myAnimator = GetComponent<Animator>();
        playerControls = new PlayerControls();
    }
    //habilita o uso de playerControls
    private void OnEnable() {
        playerControls.Enable();
    }

    void Start()
    {   
        playerControls.Combat.Attack.started += _ =>  StartAttacking();
        playerControls.Combat.Attack.canceled += _ =>  StopAttacking();

    }
    //Update fica chamando a toda hora, logo a todo momento ele verifica MouseFollowWithOffset
    private void Update() {
        MouseFollowWithOffset();
        Attack();
    }

    private void StartAttacking(){
        attackButtonDown = true;
    }

    private void StopAttacking(){
        attackButtonDown = false;
    }

    //Ao ativar o trigger Attack, que foi dado uma tecla de ativacao, ativa a animacao de ataque e da Instantiate na animacao do slash
    private void Attack() {
        if(attackButtonDown && !isAttacking){
            isAttacking = true;
        myAnimator.SetTrigger("Attack");
    //Ativa o WeaponCollider para ativar ao atacar somente
        weaponCollider.gameObject.SetActive(true);
        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;
        StartCoroutine(AttackCDRoutine());
        }
    }

    private IEnumerator AttackCDRoutine(){
        yield return new WaitForSeconds(swordAttackCD);
        isAttacking = false;
    }
    //seta o weaponCollider para false assim que o ataque finaliza para dar dano so ao bater no inimigo, e nao ao encostar o collider
    public void DoneAttackingAnimEvent(){
        weaponCollider.gameObject.SetActive(false);
    }
    //animacao de slash para cima, assim como a alternancia de lado para dar match com a espada, continuando em PlayerController -> AdjustPlayerFacingDirection
    public void SwingUpFlipAnim(){
    //inverte a animacao doo slash para ficar "de baixo para cima"
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
    //Vira a animacao de slash caso o player esteja virado para a esquerda
        if (playerController.FacingLeft){
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    //animacao de slash para baixo, assim como a alternancia de lado para dar match com a espada, continuando em PlayerController -> AdjustPlayerFacingDirection
    public void SwingDownFlipAnim(){
    //corrige a animacao do slash para sua direcao normal "de cima para baixo"
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    //Vira a animacao de slash caso o player esteja virado para a esquerda
        if (playerController.FacingLeft){
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    //verifica a posicao do mouse e a posicao do player, para direcionar corretamente baseado na posicao do mouse
    private void MouseFollowWithOffset() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

    //\/bugou a animacao movendo a espada pra frente do lugar devido\/
    //float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

    //muda a direcao apontada da espada e do collider para ataque
        if(mousePos.x < playerScreenPoint.x) {
            activeWeapon.transform.rotation = Quaternion.Euler(0, -180, 0);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
    //#faz voltar para a posicao inicial quando necessario
        else {
            activeWeapon.transform.rotation = Quaternion.Euler(0, 0, 0);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
