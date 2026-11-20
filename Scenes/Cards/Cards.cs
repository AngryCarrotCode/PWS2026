using System;
using System.Collections.Generic;
using Godot;

public partial class Cards : Node2D
{
public static Dictionary<string, Dictionary<string,string>>cards = new Dictionary<string, Dictionary<string, string>>
       {
            // Name of the card --> card type, card bonus, bonus type, card archetype, archetype override, description
           ["Ice Bullets"] = new Dictionary<string, string>
           {
               ["card type"] = "BonusDamagePercent",
               ["card bonus"] = "20",
               ["side effect"] = "none",
               ["side bonus"] = "none",
               ["bonus type"] = "ice",
               ["card archetype"] = "ice",
               ["archetype override"] = "True",
               ["description"] = "Your turret's bullets become frozen. Changes turrets damage type to ice, and deal 20% more damage with each shot",
               ["desc short"] = "+20% damage",
               ["image"] = "card_clubs_08.png"
           },
           ["Fire Bullets"] = new Dictionary<string, string>
           {
               ["card type"] = "BonusDamage",
               ["card bonus"] = "5",
               ["side effect"] = "none",
               ["side bonus"] = "none",
               ["bonus type"] = "fire",
               ["card archetype"] = "fire",
               ["archetype override"] = "True",
               ["description"] = "Your turret's bullets become engulfed in fire. Changes turrets damage type to fire, and deal 5 more damage with each shot",
               ["desc short"] = "+5 damage",
               ["image"] = "card_diamonds_02.png"
           },
           ["Spyglass"] = new Dictionary<string, string>
           {
               ["card type"] = "BonusRange",
               ["card bonus"] = "5",
               ["side effect"] = "none",
               ["side bonus"] = "none",
               ["bonus type"] = "range",
               ["card archetype"] = "mechanical",
               ["archetype override"] = "False",
               ["description"] = "Give your turret a spyglass! Increases the range by 5",
               ["desc short"] = "+5 range",
               ["image"] = "card_hearts_K.png"
                          },
           ["Smaller Bullets"] = new Dictionary<string, string>
           {
               ["card type"] = "BonusFireRate",
               ["card bonus"] = "0.2",
               ["side effect"] = "DecreaseDamagePercent",
               ["side bonus"] = "10",
               ["bonus type"] = "fire rate",
               ["card archetype"] = "bullets",
               ["archetype override"] = "False",
               ["description"] = "Your turrets bullets shrink in size! Decrease the firerate by 0.2, but decrease the damage by 10%",
               ["desc short"] = "-0.2 firerate",
               ["image"] = "card_spades_A.png"
           }
       };

}