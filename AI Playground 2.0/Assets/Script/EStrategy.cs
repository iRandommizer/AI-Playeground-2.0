public enum EStrategy 
{
    BurstAttack, // When target is immobilized && entity's attack token > 1
    Bait, // When there are nearby hazardous environments
    Kite, // When target's speed < entity's speed && entity is half health or below
    Protect, // When protector's health is > half && protectee is low health && no dodge
    Dodge, // When target is within range && attacks || there is nearby projectile
    CrowdControl, // When target attacks && is within range || right after target get's CrowedControlled || >50% of allies can burst attack 
    Trap, // When target's DPS is lower than average DPS of herd
    FocusFire, // When target is top priority(to be elaborated on)
    Heal, // When ally's health is < than 100% (need to elaborate on different priorities)
    HoldGround, // When target does not move too much && is trapped
    Scatter, // [Ranged, Assassins and other glass cannons] 
    Flank, // When target is running away
    FindCover // When health first enters low health
}
