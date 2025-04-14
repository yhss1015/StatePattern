using UnityEngine;

public class Sword_Skill : Skill
{
    [Header("��ų ����")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float swordGravity;

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position,transform.rotation);
        Sword_Skill_Controller newSwordScript = newSword.GetComponent<Sword_Skill_Controller>();

        newSwordScript.SetupSword(launchDir, swordGravity);

    }
    
}
