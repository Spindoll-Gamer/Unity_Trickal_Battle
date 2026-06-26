using UnityEngine;

public class BattleEnums
{
 
    public enum PositionRow { Front, Mid, Back}
    public enum ElementType 
    {
        Pure,           //순수
        Madness,        //광기
        Cool,           //냉정
        Lively,         //활발
        Gloomy,         //우울
        Resonance       //공명
    }
    public enum UnitRole { Tanker, Dealer, Supporter }

    public enum AttackType { Physical, Magical }
}
