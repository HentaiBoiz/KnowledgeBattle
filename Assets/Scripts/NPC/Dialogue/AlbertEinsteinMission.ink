VAR mission = ""

->main
=== main ===
Hello Joe! 
Im have some quest for you. Do you want to do it?
    * [yes]
    -- I have 3 quests here, choose one.
        **[go to stadium]
        ~ mission = "go to stadium"
        ->do_the_mission
        **[go to vending machine]
        ~ mission = "go to vending machine"
        ->do_the_mission
        **[go to shop]
        ~ mission = "go to shop"
        ->do_the_mission
    * [no]
        Goodbye.
        ->END   
    
=== do_the_mission ===
Nice. The quest is {mission}.
Hurry up, I'll give you the reward after you complete the quest.
->END
