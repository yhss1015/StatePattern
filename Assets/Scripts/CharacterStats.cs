using UnityEngine;

public class CharacterStats : MonoBehaviour
{


    [Header("�⺻���� ����")]
    public Stat strength; //��
    public Stat agility;  //ȸ��
    public Stat intelligence; //����������
    public Stat vitality; //ü��

    [Header("���ݰ��� ����")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;


    [Header("������ ����")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;


    [Header("���� ����")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;


    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    private float ignitedTimer; // ȭ�� ���� ���� �ð�
    private float chilledTimer; // ���� ���� ���� �ð�
    private float shockedTimer; // ���� ���� ���� �ð�

    private float igniteDamageCooldown = 0.3f; // ȭ�� ���� ������ ��Ÿ��
    private float igniteDamageTimer; // ȭ�� ���� ������ Ÿ�̸�

    private int igniteDamage; // ȭ�� ���� ������



    
    public int currentHealth;

    public System.Action onHealthChanged;



    protected virtual void Start()
    {

        critPower.SetDefaultValue(150); // ġ��Ÿ ���ط� 150%
        currentHealth = GetMaxHealth(); //���� ü�� �ʱ�ȭ
        onHealthChanged?.Invoke();

    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;


        igniteDamageTimer -= Time.deltaTime; // ȭ�� ƽ�� 

        if (ignitedTimer < 0)
        {
            isIgnited = false; //ȭ������ ����
        }
        if (chilledTimer < 0)
        {
            isChilled = false;
        }
        if (shockedTimer < 0)
        {
            isShocked = false;
        }

        if (igniteDamageTimer < 0 && isIgnited)
        {
            Debug.Log("ȭ�� ������");
            DecreaseHealth(igniteDamage);
            if (currentHealth < 0)
            {
                Die();
            }

            igniteDamageTimer = igniteDamageCooldown;// ��Ÿ�� ����
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();


        if (CanCrit())
        {
            totalDamage = CalculateCriticalDamage(totalDamage);

        }


        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage); // �Ϲݰ���

        DoMagicalDamage(_targetStats); // ��������

    }

    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistance(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
        {
            return; // ��� �Ӽ������� 0������ ��� �����̻� ���� x 
        }

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        

        // �Ҵ�� ���� �̻� ������� �����ϰ� �Ӽ��ο� (ȭ�� 5, ���̽� 5 �ΰ�� ���̰����� ������ �Ҵ�ȵ�.)
        if (!canApplyIgnite && !canApplyChill && canApplyShock)
        {
            float rand = Random.value;
            if (rand < 0.5f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            else if (rand < 0.75f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }
            else if (rand <= 1f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }


        }

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
        }

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // �����̻� ����
    }

    private int CheckTargetResistance(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
        {
            return;
        }

        if (_ignite)
        {
            isIgnited = _ignite;  // ��ȭ ���� ����
            ignitedTimer = 2; // ��ȭ ���� ���� �ð� ����
        }

        if (_chill)
        {
            isChilled = _chill; // ���� ���� ����
            chilledTimer = 2; // ���� ���� ���� �ð� ����
        }
        if (_shock)
        {
            isShocked = _shock; // ���� ���� ����
            shockedTimer = 2; // ���� ���� ���� �ð� ����
        }

        
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage; 



    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }
        
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {

        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
        {
            totalEvasion += 20; // ���� ������ ��� ȸ���� 20%����
        }


        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;

        }
        return false;
    }







    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealth(_damage);
        Debug.Log(_damage);

        if (currentHealth < 0)
            Die();
    }
    
    protected virtual void DecreaseHealth(int _damage)
    {
        currentHealth -= _damage;

        onHealthChanged?.Invoke();
    }


    protected virtual void Die()
    {

    }



    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }

        return false;
    }

    private int CalculateCriticalDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float critDamage = _damage * totalCritPower;


        return Mathf.RoundToInt(critDamage);
    }

    public int GetMaxHealth()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

}
