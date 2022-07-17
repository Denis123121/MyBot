using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBot
{
    internal class GameLogic
    {
        private static Player player = new Player();
        private static Enemy bot = new Enemy();
        private Random random;
        private bool isWeaponChoose = false;
        private bool isBattleBegun = false;
        private bool isShop = false;
        private int botWeapon;
        private int maxDistance = 10;
        private int botAction;

        public GameLogic()
        {
            random = new Random();
        }

        public string ProcessMessage(string messageText)
        {

            if (!isBattleBegun)
            {
                if (messageText == "/shop")
                {
                    BotChooseWeapon();
                    isShop = true;
                    return $"Добро пожаловать в магазин, выбирайте оружие\nмеч: /sword\nлук: /bowl\nкопье: /spear";
                }
                if (isShop)
                {
                    if (messageText == "/sword")
                    {
                        player.UseSword();
                        return PlayerChooseWeapon("меч");
                    }
                    else if (messageText == "/bowl")
                    {
                        player.UseBowl();
                        return PlayerChooseWeapon("лук");
                    }
                    else if (messageText == "/spear")
                    {
                        player.UseSpear();
                        return PlayerChooseWeapon("копье");
                    }
                }
                else
                {
                    WeaponError();
                }

                if (isWeaponChoose)
                {
                    if (messageText == "/battle")
                    {
                        isBattleBegun = true;
                        return "битва начинается\n" +
                            "главная задача - убить противника\n" +
                            ShowChoice();
                    }
                }
                else
                {
                    return WeaponError();
                }
            }
            else
            {
                if (messageText == "/check")
                {
                    return $"{CheckBot()}\n{CheckDeathFighters()}";
                }
                else if (messageText == "/go_ahead")
                {
                    return $"{PlayerGoAhead()}\n{CheckDeathFighters()}"; ;
                }
                else if (messageText == "/attack")
                {
                    return $"{PlayerTryAttack()}\n{CheckDeathFighters()}"; ;
                }
                else if (messageText == "/shield")
                {
                    return $"{PlayerBlock()}\n{CheckDeathFighters()}"; ;
                }
                else if (messageText == "/go_back")
                {
                    return $"{PlayerGoBack()}\n{CheckDeathFighters()}";
                }
            }

            return "";
        }

        public string CheckBot()
        {
            return $"Просто бот: здоровье: {bot.hp}, атака: {bot.damage}, дистанция: {bot.weaponDistance}.\n" +
                $"Он старается, чтобы тебе было интересно\n";
        }

        public string PlayerGoAhead()
        {
            if (player.distance == 0)
            {
                return "Вы стоите впритык ко мне, вам не куда идти!";
            }

            player.Move(bot, -1);
            return $"Вы идете вперед, расстояние между нами {player.distance}";
        }

        public string PlayerTryAttack()
        {
            if (player.weaponDistance >= player.distance)
            {
                bot.TakeDamage(player);
                return "Вы ударили меня! Вам повезло, что я не чувствую боли, как и вы кстати";
            }
            else
            {
                return "Вы не можете достать до меня, подойдите ближе, если хотите ударить";
            }
        }

        public string PlayerBlock()
        {
            player.Block();
            return "Вы пытаетесь спрятаться за щитом";
        }

        public string PlayerGoBack()
        {
            if (player.distance >= maxDistance)
            {
                return "Вы дошли до края стадиона, теперь только вперед";
            }

            player.Move(bot, 1);
            return $"Вы отходите от меня, расстояние между нами {player.distance}";
        }

        public void BotChooseWeapon()
        {
            botWeapon = random.Next(1, 3 + 1);

            if (botWeapon == 1)
            {
                bot.UseSword();
            }
            else if (botWeapon == 2)
            {
                bot.UseBowl();
            }
            else if (botWeapon == 3)
            {
                bot.UseSpear();
            }
        }

        public string BotChooseAction(int botMinAction, int botMaxAction)
        {
            if (player.IsAlive == true && bot.IsAlive == true)
            {
                botAction = random.Next(botMinAction, botMaxAction + 1);

                if (botAction == 1)
                {
                    player.TakeDamage(bot);
                    return $"Я бью тебя, твое здоровье: {player.hp}";
                }

                if (botAction == 2)
                {
                    bot.Move(player, -1);
                    return $"Я иду вперед, расстояние между нами: {bot.distance}";
                }

                if (botAction == 3)
                {
                    bot.Block();
                    return $"Я вспомнил о своем щите";
                }

                if (botAction == 4)
                {
                    bot.Move(player, 1);
                    return $"Я иду назад, расстояние между нами: {bot.distance}";
                }
            }
            else
            {
                return CheckWinner();
            }

            return "";
        }

        public string CheckWinner()
        {
            if (player.IsAlive == false)
            {
                EndButtle();
                return "Похоже я победил, можете начать новую битву (/battle) или посетить магазин (/shop)";
            }
            else if (bot.IsAlive == false)
            {
                EndButtle();
                return "Поздравляю, вы выйграли, можете начать новую битву (/battle) или посетить магазин (/shop)";
            }

            return "";
        }

        public string EndPlayerTurn()
        {
            if (player.weaponDistance >= 3 && player.distance > 2 && bot.weaponDistance <= 3)
            {
                return $"{BotChooseAction(1, 3)}\n{ShowChoice()}";
            }
            else if (bot.distance == 0)
            {
                return $"{BotChooseAction(2, 4)}\n{ShowChoice()}";
            }
            else if (bot.distance == maxDistance)
            {
                return $"{BotChooseAction(1, 3)}\n{ShowChoice()}";
            }
            else if (player.isBlock == true)
            {
                return $"{BotChooseAction(2, 4)}\n{ShowChoice()}";
            }

            return $"{BotChooseAction(1, 4)}\n{ShowChoice()}";
        }

        public string CheckDeathFighters()
        {
            if (player.IsAlive == true && bot.IsAlive == true)
            {
                return EndPlayerTurn();
            }
            else
            {
                return CheckWinner();
            }
        }

        public string ShowChoice()
        {
            return "Cейчас вы можете:\nосмотреть противника (/check),\nидти вперед (/go_ahead),\nатаковать (/attack),\n" +
                            "выставить щит (/shield),\nидти назад (/go_back)";
        }

        public string PlayerChooseWeapon(string weapon)
        {
            isWeaponChoose = true;
            return $"Вы выбрали {weapon} (урон:{player.damage}, дистанция:{player.weaponDistance}), " +
                $"чтобы пригласить меня на дуэль напишите /battle или можете просмотреть другое оружие";
        }

        public string WeaponError()
        {
            return "Сначала зайдите в магазин и возьмите оружие (напишите /shop)";
        }

        public void EndButtle()
        {
            isBattleBegun = false;
            player.RecoverHealth();
            bot.RecoverHealth();
        }
    }
}
