# Gutter Grinder - Part 1 (Unity 2D)

Idle-Slayer-like but original: junkyard hopper, no death, jump to hunt.

## Where this goes
1. Finish Unity Hub install (6000 LTS or 2022.3 LTS + Android module).
2. Hub > New Project > 2D > Name `GutterGrinder`, Path OUTSIDE your RuneLite folder e.g. `C:\Users\Ced\Documents\GutterGrinder`
3. Copy `Assets/Scripts/*.cs` from this folder into your new Unity project's `Assets/Scripts/`
4. Setup scene (5 min, no art needed):
   - Create Ground: 2D Object > Sprite (square stretched) at y=-3, BoxCollider2D, Layer=Ground, Tag=Ground
   - Create Player: 2D Object > Sprite (capsule/square) at (-4, -2), Rigidbody2D (Freeze Rotation Z, Collision Detection Continuous), CircleCollider2D, Tag=Player, add `PlayerController.cs`
   - Add `ZoneSystem` empty + `ZoneSystem.cs`, `PrestigeSystem` empty + `PrestigeSystem.cs`, `QuestSystem` empty + `QuestSystem.cs`
   - Create Canvas > HUD text: Coins, CPS, Distance, Kills + Shop panel with Dagger Button + `ShopUI.cs` + Zone buttons + `ZoneUI.cs` + Reboot panel + `PrestigeUI.cs`
   - Add 3 Buttons for zones (Yard/Roof/Sewer) and 1 Reboot button.
   - Set Ground layer in Edit > Project Settings > Physics2D if needed
5. Press Play in Editor first (no phone needed). You should auto-run in place, tap Space/click to jump, Grubs scroll in, touch = auto-hits, coins pop.

## Tuning (Part 2 numbers - Idle Slayer skeleton)
- Zones: Scrapyard x1, Rooftops x3 (50 kills + Dagger 15), Sewers x8 (200 kills + Dagger 30). Camera bg changes per zone.
- Monsters: Grub 1HP/5c, Shroom 2HP/12c, Batic 3HP/25c high-air (zone 1+). +bonus HP per zone.
- Dagger infinite + CPS x zone x prestige. All coin gains x multiplier.
- Reboot prestige: lifetime/20000 sqrt = chips, each +10% CPS permanent. Resets coins/dagger/kills/zone.

## Next parts (not yet)
Part 2: Batic flyer + quests UI + unlock popups
Part 3: Axe/Sword tier 2 + combo + IAP shop wiring + Android build
