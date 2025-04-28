using UnityEngine;

public class CharacterStats : MonoBehaviour
{


    [Header("기본적인 스탯")]
    public Stat strength; //힘
    public Stat agility;  //회피
    public Stat intelligence; //마법데미지
    public Stat vitality; //체력

    [Header("공격관련 스탯")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;


    [Header("방어관련 스탯")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;


    [Header("마법 스탯")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;


    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;

    private float ignitedTimer; // 화염 상태 지속 시간
    private float chilledTimer; // 빙결 상태 지속 시간
    private float shockedTimer; // 감전 상태 지속 시간

    private float igniteDamageCooldown = 0.3f; // 화염 상태 데미지 쿨타임
    private float igniteDamageTimer; // 화염 상태 데미지 타이머

    private int igniteDamage; // 화염 상태 데미지



    
    public int currentHealth;

    public System.Action onHealthChanged;



    protected virtual void Start()
    {

        critPower.SetDefaultValue(150); // 치명타 피해량 150%
        currentHealth = GetMaxHealth(); //현재 체력 초기화
        onHealthChanged?.Invoke();

    }

    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;


        igniteDamageTimer -= Time.deltaTime; // 화염 틱뎀 

        if (ignitedTimer < 0)
        {
            isIgnited = false; //화염상태 해제
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
            Debug.Log("화염 데미지");
            DecreaseHealth(igniteDamage);
            if (currentHealth < 0)
            {
                Die();
            }

            igniteDamageTimer = igniteDamageCooldown;// 쿨타임 적용
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

        _targetStats.TakeDamage(totalDamage); // 일반공격

        DoMagicalDamage(_targetStats); // 마법공격

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
            return; // 모든 속성데미지 0이하일 경우 상태이상 적용 x 
        }

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        

        // 할당된 상태 이상 없을경우 랜덤하게 속성부여 (화염 5, 아이스 5 인경우 값이같으니 위에서 할당안됨.)
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

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock); // 상태이상 적용
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
            isIgnited = _ignite;  // 점화 상태 적용
            ignitedTimer = 2; // 점화 상태 지속 시간 설정
        }

        if (_chill)
        {
            isChilled = _chill; // 빙결 상태 적용
            chilledTimer = 2; // 빙결 상태 지속 시간 설정
        }
        if (_shock)
        {
            isShocked = _shock; // 감전 상태 적용
            shockedTimer = 2; // 감전 상태 지속 시간 설정
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
            totalEvasion += 20; // 감전 상태인 경우 회피율 20%증가
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
