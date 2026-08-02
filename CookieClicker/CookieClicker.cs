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

var menuState = MenuState.Welcome;
var selectedUpgrade = SelectedUpgrade.Multiplier;

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
        
            case ConsoleKey.LeftArrow:
                SwitchUpgrade("left");
                break;
        
            case ConsoleKey.RightArrow:
                SwitchUpgrade("right");
                break;
            
            case ConsoleKey.Escape:
                if (menuState != MenuState.Shop) return;
                menuState = MenuState.Playing;
                break;
            
            case ConsoleKey.Enter:
                switch (menuState)
                {
                    case MenuState.Welcome:
                        menuState = MenuState.Playing;
                        break;
                    
                    case MenuState.Shop:
                        HandleUpgrade();
                        break;
                }
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
        case MenuState.Welcome:
            DrawWelcome();
            break;
        
        case MenuState.Playing:
            DrawPlaying();
            break;
        
        case MenuState.Shop:
            DrawShop();
            break;
    }

    DrawKeymap();
}

void DrawWelcome()
{
    Console.WriteLine("Welcome to Cookie Clicker!");
    
    DrawCookie("welcome");
}

void DrawKeymap()
{
    Console.WriteLine("");
    switch (menuState)
    {
        case  MenuState.Welcome:
            Console.WriteLine("Press Enter to start the game");
            break;
        
        case MenuState.Playing:
            Console.WriteLine("Q - Click Cookie,  C - Enter Shop");
            break;
        
        case MenuState.Shop:
            Console.WriteLine("E - Buy Upgrade,  Arrows - Select Upgrade,  C/ESC - Leave Shoop");
            break;
    }
}

void DrawPlaying()
{
    Console.WriteLine("Current Cookies: " +  cookies);
    Console.WriteLine("Current Cookies Per Click: " +  cookiesPerClick);
    
    DrawCookie("gameplay");
}

void DrawShop()
{
    Console.WriteLine("The Upgrade Shop");
    Console.WriteLine("");
    Console.WriteLine("You have " + cookies + " cookies to spend!");
    Console.WriteLine("");
    Console.WriteLine("<- Switch Upgrade ->");
    switch (selectedUpgrade)
    {
        case SelectedUpgrade.Multiplier:
            DrawMultiplierUpgrade();
            break;
        
        case SelectedUpgrade.ClickAmount:
            DrawClickAmountUpgrade();
            break;
    }
    Console.WriteLine("");
    Console.WriteLine("Do you want to upgrade?");
}

void DrawMultiplierUpgrade()
{
    Console.WriteLine("Multiplier Upgrade");
    Console.WriteLine("Current Level: " + multiplierLevel);
    Console.WriteLine("Current Multiplier: " + multiplier + "x");
    Console.WriteLine("Cost for upgrade: " +  multiplierCost);
}

void DrawClickAmountUpgrade()
{
    Console.WriteLine("Click Amount Upgrade");
    Console.WriteLine("Current Level: " + clickAmountLevel);
    Console.WriteLine("Current Click Amount: " + clickAmount);
    Console.WriteLine("Cost for upgrade: " +  clickAmountCost);
}

void DrawCookie(string type)
{
    var upgradesPerSide = multiplierLevel / 2;

    var spacer = "      ";
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
    Console.WriteLine(spacer + leftSide + cookieDisplay + rightSide);
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
    Welcome = 1,
    Playing = 2,
    Shop = 3
}

enum SelectedUpgrade : byte
{
    Multiplier = 1,
    ClickAmount = 2
}