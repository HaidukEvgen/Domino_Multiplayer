using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DominoServer
{
    public class RP
    {
        public const string GAME_STARTED = "01";
        public const string YOUR_DOMINOES = "02";
        public const string NO_SUCH_DOMINO = "03";
        public const string UNSUTABLE_DOMINO = "04";
        public const string PLAYER_GOES = "05";
        public const string WAITING_PLAYER = "06";
        public const string GAME_ENDED = "07";
        public const string GAME_NUMS = "08";
        public const string WHICH_DIRECTION = "09";
        public const string BAZAR = "10";
        public static Dictionary<string, string> frases = new Dictionary<string, string>
        {
            { GAME_STARTED, "Игра началась"},
            { YOUR_DOMINOES, "Ваши домино"},
            { NO_SUCH_DOMINO, "У вас нет такого домино"},
            { UNSUTABLE_DOMINO, "Домино не подходит" },
            { PLAYER_GOES, "Ходит игрок "},
            { WAITING_PLAYER, "Ожидаем игрока " }
        };
    }
}
