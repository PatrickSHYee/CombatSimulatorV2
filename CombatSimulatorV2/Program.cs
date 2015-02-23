using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CombatSimulatorV2
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.PlayGame();
        }
    }

    class Game
    {
        Player user = null;
        Enemy NPC = null;

        /// <summary>
        /// Initializes the game components and prompts the user for a name
        /// </summary>
        public Game()
        {
            DisplayTitleScreen();   // displays the title and the story.  ** title screen ** and all that goes with the title screen
            Console.Write("Please enter your name? ");
            string userName = Console.ReadLine();
            // If the user doesn't puts in a it defaults to Princess
            if (userName == "")
            {
                userName = "Princess";
            }
            user = new Player(userName, 100);
            NPC = new Enemy("5k3l3t0r", 200);
        }

        /// <summary>
        /// Main game logic
        /// </summary>
        public void PlayGame()
        {
            ConsoleKeyInfo keyboardRead;
            
            // Only runs until the condition is false.
            while (this.user.IsAlive && this.NPC.IsAlive)
            {
                Console.WriteLine("[Enter] to continue....");
                keyboardRead = Console.ReadKey(true);
                Console.Clear();
                if (keyboardRead.Key == ConsoleKey.Enter)
                {
                    DisplayCombatInfo();
                    this.user.DoAttact(this.NPC);
                    this.NPC.DoAttact(this.user);
                }
            }
            if (this.user.IsAlive)
            {
                Console.WriteLine("You won!!!");
            }
            else { Console.WriteLine("You have lost"); }

            Console.WriteLine("You have a nice day, {0}.\n\nAny to quit....", user.Name);
            Console.ReadKey();
        }

        /// <summary>
        /// Displays the current HP of the player and the enemy
        /// </summary>
        public void DisplayCombatInfo()
        {
            // display Information here
            Console.SetCursorPosition(10, 0);
            Console.Write(user.Name);
            Console.SetCursorPosition(Console.WindowWidth - NPC.Name.Length - 10, 0);
            Console.WriteLine(NPC.Name);

            Console.SetCursorPosition(5, 1);
            Console.BackgroundColor = ConsoleColor.DarkRed;
            for (int hp = 0; hp < user.HP; hp += 5)
            {
                Console.Write(" ");
            }
            Console.SetCursorPosition(Console.WindowWidth - NPC.HP / 5 - 5, 1);
            for (int hp = 0; hp < NPC.HP; hp += 5)
            {
                Console.Write(" ");
            }
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Displays the title and the story of the game
        /// </summar>
        public void DisplayTitleScreen()
        {
            string title = "#_____________________________________________________________#\n|#####_______#_______#_______#_______#_______##______#########|\n|#####|##|##|#|#####|#|##########|####|#######|#####|#########|\n|#####|##|##|#|_____|#|______####|####|______#|_____/#########|\n|#####|##|##|#|#####|#######|####|####|#######|###\\###########|\n|#_###|#_|__|#|#_###|#______|####|####|______#|#___\\___#______|\n|#|#####|#|\\####|#_____#_##of#the_____#_______##|#######|#####|\n|#|#####|#|#\\###|###|####\\####/#|______#|#####|#|______#|_____|\n|#|#####|#|##\\##|###|#####\\##/##|______#|_____/#######|#|#####|\n|#|_____|#|###\\_|#__|__####\\/###|______#|####\\_#______|#|_____|\n|_____________________________________________________________|\n";
            string text = "You finally found Skeletor and trade off insults back and forth.\n\nSkeletor is a bad ass villain that loves to think of ways to conquer the\n universe.  On his spare time he likes to play Counter-Strike, but you keep his from this.\n\nYou can use your sword, you raise your sword and yell out “I have the power!!!”, or you can eat a bag of chips.\n\nYour sword does most of the damage, but for 70% of the time. When you raise your sword and yell out “I have the power!!!” always hits Skeletor because He-man is a stupid jock that doesn't know anything else to yell out and deals a little\n damage. Or you can eat a bag of chips to heal you.\n\n<Skeletor>\tHe-man, you are a foolish child. Why don't just leave alone?";
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            for (int i = 0; i < title.Length; i++)
            {
                if (title[i] == '#')
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                if (title[i] == '_' || title[i] == '|' || title[i] == '/' || title[i] == '\\')
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                if (Char.IsLetter(title[i]))
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                Console.Write(title[i]);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(text);
        }
    }

    /// <summary>
    /// Base class where most of the methods are called and used
    /// </summary>
    public abstract class Actor
    {
        public string Name { get; set; }
        public int HP { get; set; }
        public bool IsAlive
        {
            get
            {
                if (HP <= 0) return false;
                else return true;
            }
            set
            {
                IsAlive = value;
            }
        }
        public Random RNG = null;

        public Actor(string ActorName, int health)
        {
            this.Name = ActorName;
            this.HP = health;
            RNG = new Random();
        }

        public virtual void DoAttact(Actor actor)
        {
            Console.WriteLine("{0} is attacking {1}.", this.Name, actor.Name);
        }

        /// <summary>
        /// Heals the actor
        /// </summary>
        /// <param name="addHealth"></param>
        /// <returns>the Healing Points</returns>
        public int Healing(int addHealth)
        {
            if (this.HP + addHealth >= 100)
            {
                addHealth = 100 - this.HP - addHealth;
            }
            this.HP += addHealth;
            return addHealth;
        }
    }

    /// <summary>
    /// This is where the user does there thing.  The logic is computed for the user.
    /// </summary>
    public class Player : Actor
    {
        enum AttackType
        {
            Sword, Magic, Heal
        }


        // nothing is doing, but pasting information for the base class
        public Player(string PlayerName, int health)
            : base(PlayerName, health)
        {

        }

        /// <summary>
        /// the logic is done for the user to the actor (enemy)
        /// </summary>
        /// <param name="actor">Enemy actor</param>
        public override void DoAttact(Actor actor)
        {
            int chance = 0;
            int attackPower = 0;

            base.DoAttact(actor);
            switch (ChooseAttack()){
                case AttackType.Sword:{
                    chance = RNG.Next(1, 100);
                    attackPower = RNG.Next(25, 45);
                    // 70% chance to hit 5k3l3t0r
                    if (chance < 70)
                    {
                        if (attackPower >= 35)
                        {
                            Console.WriteLine("You deal {0} damage a great feat with your {1}.\nWay to go, He-man...", attackPower, AttackType.Sword);
                        }
                        else
                        {
                            Console.WriteLine("You use your {0} and hit Skeletor with {1} damage.", AttackType.Sword, attackPower);
                        }
                    }
                    else
                    {
                        Console.WriteLine("<Skeletor>\nYou weakling, go back to your football.\nYou miss with your {0}.", "Uppercut sword slash");
                        attackPower = 0;
                    }
                    break;
                }
                case AttackType.Magic:{  // always hits
                    attackPower = RNG.Next(1, 15);
                    Console.WriteLine("{0}\nYou deal {1} damage to {2}.", "You raise your sword and yell out: I HAVE THE POWER!!!!", attackPower, actor.Name);
                    break;
                }
                case AttackType.Heal:{  // healing food
                    attackPower = Healing(RNG.Next(10, 20));
                    Console.WriteLine("{0}\n They restore your health {1} points.", "You reach for a bag of chips and eat them.", attackPower);
                    if (attackPower == 0){
                        Console.WriteLine("You were full.");
                    }
                    attackPower = 0; // doesn't attack
                    break;
                }
            }
            actor.HP -= attackPower;
        }

        /// <summary>
        /// User's chooses an attack
        /// </summary>
        /// <returns></returns>
        private AttackType ChooseAttack()
        {
            int temp = 0;       // Default attack is going to be heal.

            Console.Write("Choose attack:\n1) Sword\n2) Insult\n3) Heal\n>> ");
            AttackType userAttack = (AttackType) temp;

            // this makes sure that the user enters in an attack
            while (!Validator(Console.ReadLine(), out temp))
            {
                userAttack = (AttackType)temp - 1;  
            }
            return userAttack;
        }

        /// <summary>
        /// Validator the specified userInput.
        /// </summary>
        /// <param name="userInput">User input.</param>
        /// <returns>Invalid input</returns>
        public bool Validator(string userInput, out int output)
        {
            if (int.TryParse(userInput, out output))
            {
                Console.WriteLine("<Skeletor>\nIs that the only thing you can do right?");
            }
            if (output <= 3 && output != 0)
            {
                return true;
            }
            Console.WriteLine("<Skeletor>\nYou, mencing child... he he");
            return false;
        }
    }

    /// <summary>
    /// The non-player character and it's logic
    /// </summary>
    public class Enemy : Actor
    {
        // nothing done, but passing information to the base constructor
        public Enemy(string name, int health)
            : base(name, health)
        {

        }

        /// <summary>
        /// the enemy logic
        /// </summary>
        /// <param name="actor">The user's actor</param>
        public override void DoAttact(Actor actor)
        {
            base.DoAttact(actor);

            int CompInput = RNG.Next(1, 3);
            int chances = 0;
            int attackPower = 0;

            if (this.HP < 15)
            {
                CompInput = 3;
            }

            //PrintAttack (CompInput);
            Console.WriteLine();

            if (CompInput == 1)
            {
                // randomly generates attack power
                attackPower = RNG.Next(25, 50);

                // the chances that Skeletor attacks
                chances = RNG.Next(1, 100);
                // 70% of the time Skeletor will attack
                if (chances < 70)
                {
                    if (attackPower >= 35)
                    {
                        Console.WriteLine("Skeletor deals {0} damage with his {1}.", attackPower, "Thief of Dreams");
                    }
                    else
                    {
                        Console.WriteLine("Skeletor shoots {0} bolts to deal {0} damage with his Havoc Staff.", attackPower);
                    }

                }
                else
                {
                    // you miss and skeletor insults you
                    Console.WriteLine("<He-Man>\nSkeletor, you need something to eat. You are just bones.\nSkeletor miss with his {0}.", "Staff missed fires");
                    attackPower = 0;
                }
            }

            // Always hits
            if (CompInput == 2)
            {
                attackPower = RNG.Next(1, 10);
                Console.WriteLine("<Skeletor>\n{0}", "Your furry, flea bitten fool I'll cover my throne with your hide.");
                Console.WriteLine("Skeletor dealt {0} damage to you.", attackPower);
            }

            // healing powers
            if (CompInput == 3)
            {
                attackPower = Healing(RNG.Next(10, 20));
                Console.WriteLine("{0} That restore his health {1} points.", "Skeletor raises his staff over his head, yells out \"Comin' you Royal boob\"", attackPower);
                if (attackPower < 0)
                {
                    Console.WriteLine("Skeletor was full.");
                }
                attackPower = 0;  // Don't damage player
            }

            actor.HP -= attackPower;
        }
    }
}
