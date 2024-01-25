using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            Player player = new Player();
            Console.WriteLine("Добро пожаловать в игру");
            Console.WriteLine("статистика игрока: " + player.GetStats());

            while (true)
            {
                Console.WriteLine("\nчто вы собираетесь делать?");
                Console.WriteLine("1.выбрать с кем сразиться");
                Console.WriteLine("2.выйти");
                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("статистика игрока: " + player.GetStats());
                        Enemy enemyToFight = SelectEnemy();
                        Console.WriteLine("\nвы против " + enemyToFight.GetName());
                        BattleResult result = Battle(player, enemyToFight);
                        Console.WriteLine(result.GetResult());
                        if (result.IsPlayerWin)
                        {
                            player.LevelUp();
                        }
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("выход из игры...");
                        return;

                    default:
                        Console.Clear();
                        Console.WriteLine("такого варианта нет(.");
                        break;
                }
            }
        }

        static List<Enemy> GenerateEnemies()
        {
            List<Enemy> enemies = new List<Enemy>();
            Random random = new Random();
            int numOfEnemies = random.Next(3, 6);

            for (int i = 0; i < numOfEnemies; i++)
            {
                int enemyLevel = random.Next(1, 10);
                Enemy enemy = new Enemy("враг " + (i + 1), enemyLevel);
                enemies.Add(enemy);
            }

            return enemies;
        }

        static Enemy SelectEnemy()
        {
            List<Enemy> enemies = GenerateEnemies();
            Console.WriteLine("\nкого вызвать на дуэль?");

            for (int i = 0; i < enemies.Count; i++)
            {
                Console.WriteLine((i + 1) + ". " + enemies[i].GetStats());
            }

            int choice = int.Parse(Console.ReadLine());
            return enemies[choice - 1];
        }

        static BattleResult Battle(Player player, Enemy enemy)
        {
            while (true)
            {
                // Атака игрока
                int playerAttackDamage = player.Attack();
                enemy.TakeDamage(playerAttackDamage);
                DisplayBattleInfo(player, enemy, playerAttackDamage);

                if (enemy.IsDead())
                {
                    Console.Clear();
                    Console.WriteLine("статистика игрока: " + player.GetStats());
                    return new BattleResult(true, "вы победили " + enemy.GetName() + "!");
                }

                // Атака врага
                int enemyAttackDamage = enemy.Attack();
                player.TakeDamage(enemyAttackDamage);
                DisplayBattleInfo(player, enemy, enemyAttackDamage);

                if (player.IsDead())
                {
                    Console.Clear();
                    return new BattleResult(false, "вас убил " + enemy.GetName() + "(");
                }

               
            }
        }

        static void DisplayBattleInfo(Player player, Enemy enemy, int damage)
        {
            Console.WriteLine($"Текущее здоровье игрока: {player.GetStats()}");
            Console.WriteLine($"Текущее здоровье врага {enemy.GetName()}: {enemy.GetStats()}");
            Console.WriteLine($"Нанесенный урон: {damage}\n");
            Thread.Sleep(3000);
            Console.Clear();
        }
    }

    class Player
    {
        private int level;
        private int health;
        private int attackDamage;

        public Player()
        {
            level = 1;
            health = 100;
            attackDamage = 10;
        }

        public void LevelUp()
        {
            level++;
            health += 10;
            attackDamage += 5;
        }

        public int Attack()
        {
            return attackDamage;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
        }

        public bool IsDead()
        {
            return health <= 0;
        }

        public string GetStats()
        {
            return "уровень: " + level + " | здоровье: " + health + " | урон: " + attackDamage;
        }
    }

    class Enemy
    {
        private string name;
        private int level;
        private int health;
        private int attackDamage;

        public Enemy(string name, int level)
        {
            this.name = name;
            this.level = level;
            health = level * 10;
            attackDamage = level * 2;
        }

        public string GetName()
        {
            return name;
        }

        public int Attack()
        {
            return attackDamage;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
        }

        public bool IsDead()
        {
            return health <= 0;
        }

        public string GetStats()
        {
            return "враг: " + name + " | уровень: " + level + " | здоровье: " + health + " | урон: " + attackDamage;
        }
    }

    class BattleResult
    {
        public bool IsPlayerWin { get; }
        public string ResultMessage { get; }

        public BattleResult(bool isPlayerWin, string resultMessage)
        {
            IsPlayerWin = isPlayerWin;
            ResultMessage = resultMessage;
        }

        public string GetResult()
        {
            return ResultMessage;
        }
    }
}