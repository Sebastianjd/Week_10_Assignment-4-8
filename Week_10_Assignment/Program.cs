//Sebastian Diaz
//GAME-1343 
//Apr 7
//Description: A turn-based RPG battle system in which a player character will fight against an enemy until one dies.

using System;
using System.Collections.Generic;

namespace Week_10_Assignment
{
    class Program
    {
        public static Random random;
        static void Main(string[] args)
        {
            //Creating list and combatant objects and random
            List<Fighter> fighters = new List<Fighter>();
            Player player = new Player(10);
            Enemy enemy = new Enemy(12);
            random = new Random();

            //Adding combatants into the list
            fighters.Add(player);
            fighters.Add(enemy);
            int turn = 0;

            Console.Write("Press \"Enter\" to start");
            Console.ReadLine();
            do
            {
                //Player's turn
                if (fighters[turn] == player)
                {
                    turn += 1;
                    Console.WriteLine("\n=== Player's turn ===");
                    Console.WriteLine(" Player's Health: " + player.CurrentHealth);
                    Console.WriteLine(" Enemy's Health: " + enemy.CurrentHealth);
                    player.TakeAction(player, enemy, random);
                }
                //Enemy's turn
                else if (fighters[turn] == enemy)
                {
                    turn -= 1;
                    Console.WriteLine("\n=== Enemy's turn ===");
                    Console.WriteLine(" Player's Health: " + player.CurrentHealth);
                    Console.WriteLine(" Enemy's Health: " + enemy.CurrentHealth);
                    enemy.TakeAction(player, enemy, random);
                }

                //An input confirmation to let the user read the result of the turn
                if ((player.IsDead || enemy.IsDead) != true)
                {
                    Console.Write("Press \"Enter\" to continue");
                    Console.ReadLine();
                }
                //Until one of the combatants die, the loop will continue
            } while ((player.IsDead || enemy.IsDead) != true);

            if (player.IsDead == true)
            {
                Console.WriteLine("\nThe player has died - you lose!");
            }
            else if (enemy.IsDead == true)
            {
                Console.WriteLine("\nThe enemy has died - you win!");
            }

        }
    }

    //Base class for the other classes
    abstract class Fighter
    {
        int maxHealth;
        int currentHealth;
        bool isDead;

        //Constructor
        public Fighter(int h)
        {
            maxHealth = h;
            currentHealth = h;
        }

        //Setters and Getters
        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public bool IsDead
        {
            get { return isDead; }
            set { isDead = value; }
        }

        //Method for taking damage and calling the "Death" method when health reaches 0
        public void TakeDamage(int d)
        {
            currentHealth -= d;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                Death();
            }
        }

        //Method that sets the "isDead" Variable to true
        public void Death()
        {
            isDead = true;
        }

        //Method intended to be overridden
        public virtual void TakeAction(Player opponent1, Enemy opponent2, Random random) { }
    }

    //The "Enemy" class inherits from Fighter.
    class Enemy : Fighter
    {
        public Enemy(int h) : base (h)
        {
            MaxHealth = h;
            CurrentHealth = h;
        }

        //Method overridden to deal damage to the player
        public override void TakeAction(Player player, Enemy enemy, Random random)
        {
            
            int damage = random.Next(2, 6);
            player.TakeDamage(damage);
            Console.WriteLine("\n> Attacking player for " + damage + " points of damage!");
        }
    }

    //The "Player" class inherits from Fighter.
    class Player : Fighter
    {
        int potions;

        //Constructor
        public Player(int h) : base(h)
        {
            MaxHealth = h;
            CurrentHealth = h;
            Potions = 3;
        }

        //Setters and Getters
        public int Potions
        {
            get { return potions; }
            set { potions = value; }
        }

        public override void TakeAction(Player player, Enemy enemy, Random random)
        {
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine(" 1 - Attack Enemy");
            Console.WriteLine(" 2 - Drink Potion (" + Potions + " remaining)");
            string playerInput;

            do
            {
                Console.Write("Enter \"1\" or \"2\": ");
                playerInput = Console.ReadLine();
                //Input verification
                while (!((playerInput == "1") || (playerInput == "2")))
                {
                Console.Write("Enter \"1\" or \"2\": ");
                playerInput = Console.ReadLine();
                }

                //Verification for choosing potions while having no remaining Potions
                if ((playerInput == "2") && (player.Potions == 0))
                {
                    Console.WriteLine("\n> You have no remaining Potions\n");
                }
            } while ((playerInput == "2") && (player.Potions == 0));
            
            //Call AttackEnemy method
            if (playerInput == "1")
            {
                AttackEnemy(enemy, random);
            }
            //Call DrinkPotion method
            else if ((playerInput == "2") && (player.Potions != 0))
            {
                DrinkPotion(player, random);
            }
        }

        //Method for dealing damage to an enemy
        public void AttackEnemy(Enemy enemy, Random random)
        {
            int damage = random.Next(2, 6);
            enemy.TakeDamage(damage);
            Console.WriteLine("\n> Attacked the enemy for " + damage + " points of damage!");
        }
        //Method for restoring health
        public void DrinkPotion(Player player, Random random)
        {
            int heal = random.Next(4, 7);
            player.Potions -= 1;
            player.CurrentHealth += heal;

            if (player.CurrentHealth > player.MaxHealth)
            {
                player.CurrentHealth = player.MaxHealth;
            }
            Console.WriteLine("\n> Restored " + heal + " points of health");
        }
    }
}
