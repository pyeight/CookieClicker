var cookies = 0.0;
var clickAmount = 1;
var multiplier = 1.0;

var cookiesPerClick = clickAmount * multiplier;

var multiplierLevel = 0;
var multiplierCost = 10;

var clickAmountLevel = 0;
var clickAmountCost = 5;

var cookieDisplay = "o";
var currentIteration = 0;

var menuState = MenuState.Playing;
var selectedUpgrade = SelectedUpgrade.Multiplier;

DrawWelcome();

while (true)
{
    HandleInput();
    Draw();
    Thread.Sleep(125);
}

void HandleInput()
{
    while (Console.KeyAvailable)
    {
        var key = Console.ReadKey();

        switch (key.Key)
        {
            case ConsoleKey.Q:
                HandleClick();
                break;
        
            case ConsoleKey.C:
                ToggleShop();
                break;
        
            case ConsoleKey.E:
                HandleUpgrade();
                break;
        
            case ConsoleKey.LeftArrow:
                SwitchUpgrade("left");
                break;
        
            case ConsoleKey.RightArrow:
                SwitchUpgrade("right");
                break;
        }
    }
}

void HandleClick()
{
    if (menuState != MenuState.Playing) return;
    cookies += cookiesPerClick;
}

void HandleUpgrade()
{
    if (menuState != MenuState.Shop) return;
    
    switch (selectedUpgrade)
    {
        case SelectedUpgrade.Multiplier:
            if (cookies < multiplierCost) return;
    
            cookies -= multiplierCost;
            multiplierLevel++;
    
            multiplier *= 2;
            cookiesPerClick = clickAmount * multiplier;
    
            multiplierCost *= 2;
            break;
        
        case SelectedUpgrade.ClickAmount:
            if (cookies < clickAmountCost) return;
            
            cookies -= clickAmountCost;
            clickAmountLevel++;
            
            clickAmount += 1;
            cookiesPerClick = clickAmount * multiplier;
            
            clickAmountCost *= 2;
            break;
    }
}

void ToggleShop()
{
    switch (menuState)
    {
        case MenuState.Playing:
            menuState = MenuState.Shop;
            break;
        
        case MenuState.Shop:
            menuState = MenuState.Playing;
            break;
    }
}

void SwitchUpgrade(string input)
{
    if (menuState != MenuState.Shop) return;
    
    switch (input)
    {
        case "left":
            switch (selectedUpgrade)
            {
                case SelectedUpgrade.Multiplier:
                    break;
                
                case SelectedUpgrade.ClickAmount:
                    selectedUpgrade = SelectedUpgrade.Multiplier;
                    break;
            }
            break;
            
        case "right":
            switch (selectedUpgrade)
            {
                case SelectedUpgrade.Multiplier:
                    selectedUpgrade = SelectedUpgrade.ClickAmount;
                    break;
                    
                case SelectedUpgrade.ClickAmount:
                    break;
            }
            break;
    }
}

void Draw()
{
    Console.Clear();
    Console.WriteLine("");
    // Console.WriteLine("Current iteration: " + currentIteration);
    switch (menuState)
    {
        case MenuState.Playing:
            DrawPlaying();
            break;
        
        case MenuState.Shop:
            DrawShop();
            break;
    }
}

void DrawWelcome()
{
    Console.WriteLine("Welcome to Cookie Clicker!");
    Console.WriteLine("Press Q to click ze cookie");
    
    DrawCookie("welcome");
}

void DrawPlaying()
{
    Console.WriteLine("Q - Click Cookie");
    Console.WriteLine("C - Enter Shop");
    Console.WriteLine("Current Cookies: " +  cookies);
    Console.WriteLine("Current Cookies Per Click: " +  cookiesPerClick);
    
    DrawCookie("gameplay");
}

void DrawShop()
{
    Console.WriteLine("C - Leave Shop");
    Console.WriteLine("E - Buy Upgrade");
    Console.WriteLine("<- Switch Upgrade ->");
    Console.WriteLine("You have " + cookies + " cookies to spend!");
    Console.WriteLine("");

    switch (selectedUpgrade)
    {
        case SelectedUpgrade.Multiplier:
            DrawMultiplierUpgrade();
            break;
        
        case SelectedUpgrade.ClickAmount:
            DrawClickAmountUpgrade();
            break;
    }
}

void DrawMultiplierUpgrade()
{
    Console.WriteLine("Selected Upgrade: Multiplier");
    Console.WriteLine("Upgrade Level: " + multiplierLevel);
    Console.WriteLine("Current Multiplier: " + multiplier + "x");
    Console.WriteLine("Cost for upgrade: " +  multiplierCost);
    Console.WriteLine("Do you wanna upgrade?");
}

void DrawClickAmountUpgrade()
{
    Console.WriteLine("Selected Upgrade: Click Amount");
    Console.WriteLine("Upgrade Level: " + clickAmountLevel);
    Console.WriteLine("Current Click Amount: " + clickAmount);
    Console.WriteLine("Cost for upgrade: " +  clickAmountCost);
    Console.WriteLine("Do you wanna upgrade?");
}

void DrawCookie(string type)
{
    var upgradesPerSide = multiplierLevel / 2;

    var leftSide = "";
    var rightSide = "";
    
    if (type == "welcome")
    {
        leftSide = "click me -> ";
        rightSide = " <- click me";
    }
    else if (type == "gameplay")
    {
        leftSide = "";
        rightSide = "";
    }

    AnimateCookie(type);
    
    for (int i = 1; i <= upgradesPerSide; i++)
    {
        leftSide += "- ";
        rightSide += " -";
    }
    
    Console.WriteLine("");
    Console.WriteLine(leftSide + cookieDisplay + rightSide);
    Console.WriteLine("");
}

void AnimateCookie(string type)
{
    if (currentIteration >= 30)
    {
        cookieDisplay = "o";
        currentIteration = 0;
    }

    if (currentIteration < 10)
    {
        cookieDisplay = "o";
    }
    else if (currentIteration < 20)
    {
        cookieDisplay = "O";
    }
    else
    {
        cookieDisplay = "0";
    }

    if (type != "welcome")
    {
        currentIteration++;
    }
}

enum MenuState : byte
{
    Playing = 1,
    Shop = 2
}

enum SelectedUpgrade : byte
{
    Multiplier = 1,
    ClickAmount = 2
}